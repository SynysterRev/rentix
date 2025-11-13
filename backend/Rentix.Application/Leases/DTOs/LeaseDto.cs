using Rentix.Application.RealEstate.DTOs.Documents;
using Rentix.Application.RealEstate.DTOs.Properties;
using Rentix.Domain.Entities;
using System.Net;

namespace Rentix.Application.Leases.DTOs
{
    public record LeaseDto
    {
        public int Id { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public decimal RentAmount { get; init; }
        public decimal ChargesAmount { get; init; }
        public decimal Deposit { get; init; }
        public int PropertyId { get; init; }
        public DocumentDto LeaseDocument { get; init; } = null!;

        public static LeaseDto FromEntity(Lease lease)
        {
            ArgumentNullException.ThrowIfNull(lease);

            return new LeaseDto
            {
                Id = lease.Id,
                PropertyId = lease.PropertyId,
                ChargesAmount = lease.ChargesAmount,
                Deposit = lease.Deposit,
                EndDate = lease.EndDate,
                StartDate = lease.StartDate,
                RentAmount = lease.RentAmount,
                LeaseDocument = DocumentDto.FromEntity(lease.LeaseDocument)
            };
        }
    }
}
