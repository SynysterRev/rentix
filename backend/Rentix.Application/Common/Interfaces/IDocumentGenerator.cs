using Rentix.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentix.Application.Common.Interfaces
{
    public interface IDocumentGenerator
    {
        /// <summary>
        /// Asynchronously generates a lease agreement document as a byte array for the provided lease entity.
        /// </summary>
        /// <param name="lease">The lease entity for which to generate the agreement document.</param>
        /// <returns>A task representing the asynchronous operation, containing the generated document as a byte array.</returns>
        public Task<byte[]> GenerateLeaseAgreementAsync(Lease lease);
    }
}
