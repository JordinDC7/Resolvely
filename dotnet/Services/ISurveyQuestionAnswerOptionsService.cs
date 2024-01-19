using Sabio.Models.Domain.SurveyQuestions;
using Sabio.Models.Requests;
using System.Collections.Generic;

namespace Sabio.Services.Services
{
    public interface ISurveyQuestionAnswerOptionsService
    {
        List<SurveyQuestion> GetQuestionById(int id);
    }
}