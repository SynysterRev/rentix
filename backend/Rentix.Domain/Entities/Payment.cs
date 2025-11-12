using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentix.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        public int LeaseId { get; set; }

        public decimal Amount { get; set; }

        public DateTime DatePaid { get; set; }

        public string? Method { get; set; } // ex: virement, chèque, espèces

        public string? Comment { get; set; }

        public Lease? Lease { get; set; }
    }
}
