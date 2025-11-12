using System;

namespace Rentix.Application.Lease.DTOs
{
    public class ActiveLeaseWithTenantCountDto
    {
        public int LeaseId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TenantCount { get; set; }
    }
}
