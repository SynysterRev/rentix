using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Rentix.Application.Common.Interfaces;
using Rentix.Application.Common.Interfaces.Queries;
using Rentix.Application.Leases.Commands.Create;
using Rentix.Application.RealEstate.Commands.Create.Property;
using Rentix.Application.Tenants.DTOs.Tenants;
using Rentix.Domain.Entities;
using Rentix.Domain.Repositories;
using Rentix.Domain.ValueObjects;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Rentix.Application.Exceptions;

namespace Rentix.Tests.Unit.Leases.Commands.Create
{
    public class CreateLeaseCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<ILeaseRepository> _leaseRepositoryMock = new();
        private readonly Mock<IPropertyQueries> _propertyQueriesMock = new();
        private readonly Mock<ITenantRepository> _tenantRepositoryMock = new();
        private readonly Mock<IDocumentRepository> _documentRepositoryMock = new();
        private readonly Mock<IFileStorageService> _fileStorageServiceMock = new();
        private readonly Mock<ILogger<CreateLeaseCommandHandler>> _loggerMock = new();
        private readonly CreateLeaseCommandHandler _handler;

        public CreateLeaseCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new CreateLeaseCommandHandler(_leaseRepositoryMock.Object,
                _documentRepositoryMock.Object,
                _propertyQueriesMock.Object,
                _tenantRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _fileStorageServiceMock.Object,
                _loggerMock.Object);
        }

        private Tenant CreateTestTenant(int id = 1, string firstName = "Test", string lastName = "Test", string email = "test@test.com", string phone = "0123456789")
        {
            var tenant = Tenant.Create(
            firstName,
            lastName,
            Email.Create(email),
            Phone.Create(phone)
            );
            tenant.Id = id;
            return tenant;
        }

        private CreateLeaseCommand CreateValidCommand(
            int propertyId = 1,
            DateTime? startDate = null,
            DateTime? endDate = null,
            decimal rentAmount = 1000m,
            decimal chargesAmount = 100m,
            decimal deposit = 1000m,
            bool isActive = true,
            string? notes = "Test lease",
            byte[]? fileContent = null,
            string fileName = "lease.pdf",
            string contentType = "application/pdf"
        )
        {
            var start = startDate ?? DateTime.UtcNow.Date;
            var end = endDate ?? start.AddYears(1);

            var content = fileContent ?? Encoding.UTF8.GetBytes("dummy file content");

            return new CreateLeaseCommand
            {
                PropertyId = propertyId,
                StartDate = start,
                EndDate = end,
                RentAmount = rentAmount,
                ChargesAmount = chargesAmount,
                Deposit = deposit,
                IsActive = isActive,
                Notes = notes,
                FileStream = new MemoryStream(content),
                FileName = fileName,
                ContentType = contentType,
                FileSizeInBytes = content.LongLength,
                Tenants = new List<TenantCreateDto>
                {
                    new TenantCreateDto { FirstName = "Test", LastName = "Test", Email = "test@test.com", PhoneNumber = "0123456789" }
                }
            };
        }

        [Fact]
        public async Task Should_CreateLease_WhenFieldsValid()
        {
            int leaseId = 1;
            var document = Document.Create(
                LeaseDocumentType.LeaseAgreement,
                "lease.pdf",
                "Documents/lease.pdf",
                "application/pdf",
                10,
                1,
                DocumentEntityType.Lease,
                leaseId,
                null);
            var lease = Lease.Create(
                DateTime.UtcNow.Date,
                DateTime.UtcNow.Date.AddYears(1),
                1000m,
                100m,
                1000m,
                true,
                1,
                document,
                null);
            lease.Id = leaseId;
            var tenant = CreateTestTenant();
            var command = CreateValidCommand();

            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            _documentRepositoryMock.Setup(d => d.AddAsync(It.IsAny<Document>())).ReturnsAsync(document);
            _propertyQueriesMock.Setup(d => d.ExistsAsync(1)).ReturnsAsync(true);
            _leaseRepositoryMock.Setup(l => l.AddAsync(It.IsAny<Lease>())).ReturnsAsync(lease);
            _fileStorageServiceMock
                .Setup(f => f.SaveFileAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync(document.FilePath);
            _tenantRepositoryMock.Setup(t => t.AddAsync(It.IsAny<Tenant>())).ReturnsAsync(tenant);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Id.Should().Be(lease.Id);
            result.StartDate.Date.Should().Be(lease.StartDate.Date);
            result.EndDate.Date.Should().Be(lease.EndDate.Date);
            result.RentAmount.Should().Be(lease.RentAmount);
            result.ChargesAmount.Should().Be(lease.ChargesAmount);
            result.Deposit.Should().Be(lease.Deposit);
            result.PropertyId.Should().Be(lease.PropertyId);

            // Assert - document mapping
            result.LeaseDocument.Should().NotBeNull();
            result.LeaseDocument.FileName.Should().Be(document.FileName);
            result.LeaseDocument.FilePath.Should().Be(document.FilePath);
            result.LeaseDocument.FileType.Should().Be(document.DocumentType);
            result.LeaseDocument.Description.Should().Be(document.Description);

            // Verify interactions
            _fileStorageServiceMock.Verify(f => f.SaveFileAsync(It.IsAny<Stream>(), command.FileName), Times.Once);
            _documentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Document>()), Times.Once);
            _leaseRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Lease>()), Times.Once);

            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
            _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Should_Rollback_When_FileStorageThrows()
        {
            int leaseId = 1;
            var document = Document.Create(
                LeaseDocumentType.LeaseAgreement,
                "lease.pdf",
                "Documents/lease.pdf",
                "application/pdf",
                10,
                1,
                DocumentEntityType.Lease,
                leaseId,
                null);
            var lease = Lease.Create(
                DateTime.UtcNow.Date,
                DateTime.UtcNow.Date.AddYears(1),
                1000m,
                100m,
                1000m,
                true,
                1,
                document,
                null);
            lease.Id = leaseId;
            var command = CreateValidCommand();
            var tenant = CreateTestTenant();

            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _propertyQueriesMock.Setup(d => d.ExistsAsync(1)).ReturnsAsync(true);
            _leaseRepositoryMock.Setup(l => l.AddAsync(It.IsAny<Lease>())).ReturnsAsync(lease);
            _tenantRepositoryMock.Setup(t => t.AddAsync(It.IsAny<Tenant>())).ReturnsAsync(tenant);

            _fileStorageServiceMock
                .Setup(f => f.SaveFileAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Storage error"));

            var ex = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));

            ex.Message.Should().Be("Storage error");
            _unitOfWorkMock.Verify(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Should_Rollback_When_DocumentRepositoryThrows()
        {
            var command = CreateValidCommand();

            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _propertyQueriesMock.Setup(d => d.ExistsAsync(1)).ReturnsAsync(true);

            _fileStorageServiceMock
                .Setup(f => f.SaveFileAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync("path/to/file.pdf");

            _documentRepositoryMock.Setup(d => d.AddAsync(It.IsAny<Document>())).ThrowsAsync(new Exception("DB error on document"));

            var ex = await Assert.ThrowsAsync<System.Exception>(() => _handler.Handle(command, CancellationToken.None));

            ex.Message.Should().Be("DB error on document");
            _unitOfWorkMock.Verify(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Should_Rollback_When_LeaseRepositoryThrows()
        {
            var command = CreateValidCommand();

            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            _fileStorageServiceMock
                .Setup(f => f.SaveFileAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync("path/to/file.pdf");

            var tempDocument = Document.Create(LeaseDocumentType.LeaseAgreement, "tmp.pdf", "path/to/file.pdf", "application/pdf", 123, 1, DocumentEntityType.Lease, null, null);
            _documentRepositoryMock.Setup(d => d.AddAsync(It.IsAny<Document>())).ReturnsAsync(tempDocument);
            _leaseRepositoryMock.Setup(l => l.AddAsync(It.IsAny<Lease>())).ThrowsAsync(new Exception("DB error on lease"));
            _propertyQueriesMock.Setup(d => d.ExistsAsync(1)).ReturnsAsync(true);

            var ex = await Assert.ThrowsAsync<System.Exception>(() => _handler.Handle(command, CancellationToken.None));

            ex.Message.Should().Be("DB error on lease");
            _unitOfWorkMock.Verify(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Should_Rollback_When_TenantCreationFails()
        {
            int leaseId = 1;
            var document = Document.Create(
                LeaseDocumentType.LeaseAgreement,
                "lease.pdf",
                "Documents/lease.pdf",
                "application/pdf",
                10,
                1,
                DocumentEntityType.Lease,
                null,
                null);

            var lease = Lease.Create(
                DateTime.UtcNow.Date,
                DateTime.UtcNow.Date.AddYears(1),
                1000m,
                100m,
                1000m,
                true,
                1,
                document,
                null);
            lease.Id = leaseId;

            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            _fileStorageServiceMock
                .Setup(f => f.SaveFileAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync(document.FilePath);

            _documentRepositoryMock.Setup(d => d.AddAsync(It.IsAny<Document>())).ReturnsAsync(document);
            _leaseRepositoryMock.Setup(l => l.AddAsync(It.IsAny<Lease>())).ReturnsAsync(lease);
            _propertyQueriesMock.Setup(d => d.ExistsAsync(1)).ReturnsAsync(true);

            // tenant repository fails
            _tenantRepositoryMock.Setup(t => t.AddAsync(It.IsAny<Tenant>())).ThrowsAsync(new Exception("DB error on tenant"));

            var command = CreateValidCommand();;

            var ex = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));

            ex.Message.Should().Be("DB error on tenant");
            _unitOfWorkMock.Verify(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Should_ThrowNotFound_When_PropertyDoesNotExist()
        {
            // Arrange
            var command = CreateValidCommand();
            _propertyQueriesMock.Setup(p => p.ExistsAsync(command.PropertyId)).ReturnsAsync(false);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
            ex.Message.Should().Contain("does not exist");

            // Ensure no transaction or file operations were started
            _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _fileStorageServiceMock.Verify(f => f.SaveFileAsync(It.IsAny<Stream>(), It.IsAny<string>()), Times.Never);
            _documentRepositoryMock.Verify(d => d.AddAsync(It.IsAny<Document>()), Times.Never);
            _leaseRepositoryMock.Verify(l => l.AddAsync(It.IsAny<Lease>()), Times.Never);
            _tenantRepositoryMock.Verify(t => t.AddAsync(It.IsAny<Tenant>()), Times.Never);
        }
    }
}
