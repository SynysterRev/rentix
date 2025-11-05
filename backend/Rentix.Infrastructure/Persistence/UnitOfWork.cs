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

        public UnitOfWork(
            ApplicationDbContext dbContext,
            IPropertyRepository propertyRepository,
            IAddressRepository addressRepository)
        {
            _dbContext = dbContext;
            Properties = propertyRepository;
            Addresses = addressRepository;
        }

        /// <summary>
        /// Persists all changes made in the context to the database asynchronously.
        /// </summary>
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
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
        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
                return; // Already in a transaction

            _currentTransaction = await _dbContext.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// Commits the current database transaction asynchronously.
        /// </summary>
        public async Task CommitTransactionAsync()
        {
            if (_currentTransaction == null)
                return;

            await _currentTransaction.CommitAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        /// <summary>
        /// Rolls back the current database transaction asynchronously.
        /// </summary>
        public async Task RollbackTransactionAsync()
        {
            if (_currentTransaction == null)
                return;

            await _currentTransaction.RollbackAsync();
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