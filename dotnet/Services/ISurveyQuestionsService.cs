using Sabio.Models.Domain.SurveyQuestions;
using Sabio.Models.Requests;
using System.Collections.Generic;

namespace Sabio.Services.Services
{
    public interface ISurveyQuestionsService
    {
        int AddQuestion(QuestionAddRequest model, int userId);
        void DeleteQuestion(int id);
        List<SurveyQuestion> GetSurveyByIdWithQuestions(int id);
        void UpdateQuestion(QuestionUpdateRequest model, int userId);
    }
}