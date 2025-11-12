using System;

namespace Rentix.Application.Lease.DTOs
{
    public class LeaseSummaryDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal RentAmount { get; set; }
        public bool IsActive { get; set; }
        public string PropertyName { get; set; } = string.Empty;
    }
}
