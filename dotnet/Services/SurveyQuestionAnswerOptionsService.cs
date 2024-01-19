using Sabio.Data.Providers;
using Sabio.Models.Requests;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Domain.SurveyQuestions;
using Sabio.Data;
using Sabio.Models.Domain.Users;
using Sabio.Models.Domain;


namespace Sabio.Services.Services
{
    public class SurveyQuestionAnswerOptionsService : ISurveyQuestionAnswerOptionsService
    {

        IDataProvider _data = null;
        ILookUpService _lookUpService = null;

        public SurveyQuestionAnswerOptionsService(IDataProvider data, ILookUpService lookup)
        {

            _data = data;
            _lookUpService = lookup;

        }

        public List<SurveyQuestion> GetQuestionById(int id)
        {
            string procName = "[dbo].[SurveyQuestionAnswerOptions_SelectByQuestionId]";

            List<SurveyQuestion> list = null;

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@Id", id);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int index = 0;
                    SurveyQuestion survey = MapQuestionsAnswerOptions(reader, ref index);

                    if (list == null)
                    {
                        list = new List<SurveyQuestion>();
                    }
                    list.Add(survey);
                });

            return list;
        }

        private SurveyQuestion MapQuestionsAnswerOptions(IDataReader reader, ref int index)
        {
            SurveyQuestion surveyAnswerOptions = new SurveyQuestion();

            surveyAnswerOptions.Id = reader.GetSafeInt32(index++);
            surveyAnswerOptions.Question = reader.GetSafeString(index++);
            surveyAnswerOptions.AnswerOptions = reader.DeserializeObject<List<BaseSurveyQuestionAnswerOptions>>(index++);
            surveyAnswerOptions.HelpText = reader.GetSafeString(index++);
            surveyAnswerOptions.IsRequired = reader.GetSafeBool(index++);
            surveyAnswerOptions.IsMultipleAllowed = reader.GetSafeBool(index++);
            surveyAnswerOptions.QuestionType = _lookUpService.MapSingleLookUp(reader, ref index);
            surveyAnswerOptions.SortOrder = reader.GetSafeInt32(index++);
            surveyAnswerOptions.CreatedBy = reader.DeserializeObject<BaseUser>(index++);
            surveyAnswerOptions.DateModified = reader.GetSafeDateTime(index++);
            surveyAnswerOptions.DateCreated = reader.GetSafeDateTime(index++);

            return surveyAnswerOptions;
        }
    }
}
