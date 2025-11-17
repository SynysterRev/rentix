using Rentix.Application.RealEstate.DTOs.Properties;
using Rentix.Domain.Entities;

namespace Rentix.Application.RealEstate.Mappers
{
    public interface IPropertyMapper
    {
        public PropertyDetailDto Map(Property property);
    }
}
