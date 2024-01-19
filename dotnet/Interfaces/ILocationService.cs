using Sabio.Models;
using Sabio.Models.Domain.Locations;
using Sabio.Models.Requests.Locations;
using System.Data;

namespace Sabio.Services.Interfaces
{
    public interface ILocationService
    {
        int Add(LocationAddRequest requestModel, int createdById);
        void Delete(int id);
        Paged<Location> GetAll(int pageIndex, int pageSize);
        Paged<Location> GetByCreatedBy(int createdById, int pageIndex, int pageSize);
        Location GetById(int id);
        void Update(LocationUpdateRequest requestModel, int modifiedById);

        public Location SingleLocationMapper(IDataReader reader, ref int startingIndex);


    }
}