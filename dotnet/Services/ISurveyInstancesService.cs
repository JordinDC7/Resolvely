using Sabio.Models;
using Sabio.Models.Requests;
using Sabio.Models.Domain.Surveys;

namespace Sabio.Services.Services
{
    public interface ISurveyInstancesService
    {
        int Add(int surveyId, int userId);

        Paged<SurveyInstances> SelectAllPaginated(int pageIndex, int pageSize);

        void Update(SurveyInstancesUpdateRequest model, int userId);

        void DeleteById(int id);

        SurveyInstances Get(int id);

        Paged<SurveyInstances> SelectByCreatedBy(int pageIndex, int pageSize, int createdBy);

        Paged<SurveyInstances> SelectBySurveyId(int pageIndex, int pageSize, int surveyId);
    }
}