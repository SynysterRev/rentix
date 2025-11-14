using System.Threading.Tasks;
using System.Collections.Generic;
using Rentix.Application.Leases.DTOs;

namespace Rentix.Application.Common.Interfaces.Queries
{
    /// <summary>
    /// Provides query methods for retrieving Lease data (projections, filters, etc.).
    /// </summary>
    public interface ILeaseQueries
    {
        /// <summary>
        /// Retrieves a list of lease summaries (main info only).
        /// </summary>
        /// <returns>List of LeaseSummaryDto.</returns>
        Task<List<LeaseSummaryDto>> GetLeaseSummariesAsync();

        /// <summary>
        /// Retrieves all active leases with tenant count.
        /// </summary>
        /// <returns>List of ActiveLeaseWithTenantCountDto.</returns>
        Task<List<ActiveLeaseWithTenantCountDto>> GetActiveLeasesWithTenantCountAsync();

        // Add other advanced read methods as needed
    }
}
