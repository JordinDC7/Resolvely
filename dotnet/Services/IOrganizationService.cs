using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests;
using System.Collections.Generic;

namespace Sabio.Services.Services
{
    public interface IOrganizationService
    {
        int Add(OrganizationAddRequest model, int userId);

        Paged<Organization> SelectAllPaginated(int pageIndex, int pageSize);

        void Update(OrganizationUpdateRequest model, int userId);

        void DeleteById(int id);

        Organization Get(int id);

        Paged<Organization> SelectByCreatedBy(int pageIndex, int pageSize, int createdBy);
    }
}