using Sabio.Models;
using Sabio.Models.Requests;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Sabio.Models.Domain.Surveys;

namespace Sabio.Services.Services
{
    public interface ISurveyAnswersService
    {
        int Add(SurveyAnswersAddRequest model);

        Paged<SurveyAnswers> SelectAllPaginated(int pageIndex, int pageSize);

        void Update(SurveyAnswersUpdateRequest model);

        void DeleteById(int id);

        SurveyAnswers Get(int id);
        List<SurveyResult> GetSurveyAnswersInstanceId(int instanceId);

        List<Survey> GetAllSurveys();



    }
}