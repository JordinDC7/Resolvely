using Sabio.Models;
using Sabio.Models.Domain.Surveys;
using Sabio.Models.Requests;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface ISurveysService
    {
        int Add(SurveyAddRequest model, int userId);
        void DeleteById(int id);
        Paged<Survey> GetAllByCreatedByPaginated(int pageIndex, int pageSize, int userId);
        Paged<Survey> GetAllPaginated(int pageIndex, int pageSize, int statusId, bool excluded);
        Survey GetById(int id);
        void Update(SurveyUpdateRequest model, int id);     
    }
}