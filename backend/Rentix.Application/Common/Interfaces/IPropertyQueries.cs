using Rentix.Application.RealEstate.DTOs.Properties;

namespace Rentix.Application.Common.Interfaces
{
    public interface IPropertyQueries
    {
        public Task<List<PropertyListDto>> GetPropertyListAsync();
    }
}
