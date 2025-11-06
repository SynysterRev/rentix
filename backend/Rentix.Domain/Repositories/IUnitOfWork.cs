using System.Threading.Tasks;

namespace Rentix.Domain.Repositories
{
    /// <summary>
    /// Defines a unit of work for coordinating changes and transactions across multiple repositories.
    /// </summary>
    public interface IUnitOfWork : System.IDisposable
    {
        /// <summary>
        /// Gets the property repository for managing Property entities.
        /// </summary>
        IPropertyRepository Properties { get; }

        /// <summary>
        /// Gets the address repository for managing Address entities.
        /// </summary>
        IAddressRepository Addresses { get; }

        /// <summary>
        /// Gets the tenant repository for managing Tenant entities.
        /// </summary>
        ITenantRepository Tenants { get; }

        /// <summary>
        /// Persists all changes made in the context to the database asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous save operation.</returns>
        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Persists all changes made in the context to the database synchronously.
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Begins a new database transaction asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous begin transaction operation.</returns>
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Commits the current database transaction asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous commit operation.</returns>
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Rolls back the current database transaction asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous rollback operation.</returns>
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}