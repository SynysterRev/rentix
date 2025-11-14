using Microsoft.EntityFrameworkCore;
using Rentix.Application.Common.Interfaces.Queries;
using Rentix.Application.Leases.DTOs;

namespace Rentix.Infrastructure.Persistence.Queries
{
    public class LeaseQueries : ILeaseQueries
    {
        private readonly ApplicationDbContext _dbContext;

        public LeaseQueries(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<LeaseSummaryDto>> GetLeaseSummariesAsync()
        {
            return await _dbContext.Leases
            .Select(l => new LeaseSummaryDto
            {
                Id = l.Id,
                StartDate = l.StartDate,
                EndDate = l.EndDate,
                RentAmount = l.RentAmount,
                IsActive = l.IsActive,
                PropertyName = l.Property.Name
            })
            .ToListAsync();
        }

        public async Task<List<ActiveLeaseWithTenantCountDto>> GetActiveLeasesWithTenantCountAsync()
        {
            return await _dbContext.Leases
            .Where(l => l.IsActive)
            .Select(l => new ActiveLeaseWithTenantCountDto
            {
                LeaseId = l.Id,
                StartDate = l.StartDate,
                EndDate = l.EndDate,
                TenantCount = l.Tenants.Count
            })
            .ToListAsync();
        }
    }
}
