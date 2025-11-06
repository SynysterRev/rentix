using Microsoft.EntityFrameworkCore.Storage;
using Rentix.Domain.Repositories;
using System.Threading.Tasks;

namespace Rentix.Infrastructure.Persistence
{
    /// <summary>
    /// Implements the unit of work pattern for coordinating changes and transactions across multiple repositories.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private IDbContextTransaction? _currentTransaction;

        public IPropertyRepository Properties { get; }
        public IAddressRepository Addresses { get; }
        public ITenantRepository Tenants { get; }

        public UnitOfWork(
            ApplicationDbContext dbContext,
            IPropertyRepository propertyRepository,
            IAddressRepository addressRepository,
            ITenantRepository tenantRepository)
        {
            _dbContext = dbContext;
            Properties = propertyRepository;
            Addresses = addressRepository;
            Tenants = tenantRepository;
        }

        /// <summary>
        /// Persists all changes made in the context to the database asynchronously.
        /// </summary>
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Persists all changes made in the context to the database synchronously.
        /// </summary>
        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Begins a new database transaction asynchronously.
        /// </summary>
        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
                return; // Already in a transaction

            _currentTransaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        }

        /// <summary>
        /// Commits the current database transaction asynchronously.
        /// </summary>
        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
                return;

            await _currentTransaction.CommitAsync(cancellationToken);
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        /// <summary>
        /// Rolls back the current database transaction asynchronously.
        /// </summary>
        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
                return;

            await _currentTransaction.RollbackAsync(cancellationToken);
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        /// <summary>
        /// Disposes the database context and any active transaction.
        /// </summary>
        public void Dispose()
        {
            _currentTransaction?.Dispose();
            _dbContext.Dispose();
        }
    }
}