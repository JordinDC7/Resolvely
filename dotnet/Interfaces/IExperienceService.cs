using Sabio.Models;
using Sabio.Models.Domain.Experience;
using Sabio.Models.Requests.Experience;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IExperienceService
    {
        void Add(List<ExperienceAddRequest> requestModel, int userId);
        void Delete(int id);
        Paged<Experience> GetAll(int pageIndex, int pageSize);
        Experience GetById(int id);
        void Update(ExperienceUpdateRequest requestModel, int userId);
    }
}