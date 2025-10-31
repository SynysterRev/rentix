using MediatR;
using Rentix.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentix.Application.RealEstate.Commands.Create
{
    public record CreatePropertyCommand() : IRequest<int>
    {
        public string Name { get; init; } = string.Empty;
        public decimal MaxRent { get; init; }
        public PropertyStatus PropertyStatus { get; init; }
        public decimal Surface { get; init; }
        public int NumberRooms { get; init; }
        public int AddressId { get; init; }
        public Guid LandLordId { get; init; }
    }
}
