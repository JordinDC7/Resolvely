using Newtonsoft.Json;
using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Surveys;
using Sabio.Models.Domain.SurveyQuestions;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services.Services
{
    public class SurveyAnswersService : ISurveyAnswersService
    {

        IDataProvider _data = null;

        public SurveyAnswersService(IDataProvider data)
        {
            _data = data;

        }

        public int Add(SurveyAnswersAddRequest model)
        {

            int Id = 0;

            string procName = "[dbo].[SurveyAnswers_Insert]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);


                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);


            }, returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object oId = returnCollection["@Id"].Value;

                int.TryParse(oId.ToString(), out Id);
            });

            return Id;
        }

        public Paged<SurveyAnswers> SelectAllPaginated(int pageIndex, int pageSize)
        {

            string procName = "[dbo].[SurveyAnswers_SelectAll]";

            Paged<SurveyAnswers> pagedList = null;
            List<SurveyAnswers> theSurveyAnswers = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection param)
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
            },

                delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    SurveyAnswers surveyAnswers = MapSingleSurveyAnswer(reader, ref startingIndex);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }

                    if (theSurveyAnswers == null)
                    {
                        theSurveyAnswers = new List<SurveyAnswers>();
                    }

                    theSurveyAnswers.Add(surveyAnswers);
                });

            if (theSurveyAnswers != null)
            {
                pagedList = new Paged<SurveyAnswers>(theSurveyAnswers, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }


        public void Update(SurveyAnswersUpdateRequest model)
        {
            string procName = "[dbo].[SurveyAnswers_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);

                col.AddWithValue("@Id", model.Id);

            }, returnParameters: null);
        }

        public void DeleteById(int id)
        {
            string procName = "[dbo].[SurveyAnswers_Delete_ById]";

            _data.ExecuteNonQuery(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            });
        }




        public SurveyAnswers Get(int id)
        {

            string procName = "[dbo].[SurveyAnswers_Select_ById]";

            SurveyAnswers surveyAnswers = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {

                paramCollection.AddWithValue("@Id", id);

            }, delegate (IDataReader reader, short set)


            {
                int startingIndex = 0;

                surveyAnswers = MapSingleSurveyAnswer(reader, ref startingIndex);
            });

            return surveyAnswers;
        }


        public List<SurveyResult> GetSurveyAnswersInstanceId(int instanceId)
        {

            string procName = "[dbo].[SurveyAnswers_Select_BySurveyInstanceId]";

            List<SurveyResult> list = null;
            SurveyResult surveyResult = null;

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {

                paramCollection.AddWithValue("@InstanceId", instanceId);

            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;

                surveyResult = MapSingleSurveyResult(reader, ref startingIndex);
                if (list == null)
                {
                    list = new List<SurveyResult>();
                }
                list.Add(surveyResult);
            });

            return list;
        }

        public List<Survey> GetAllSurveys()
        {

            string procName = "[dbo].[Surveys_SelectAll]";

            List<Survey> list = null;
            Survey survey = null;

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {

            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;

                survey = MapSingleSurvey(reader, ref startingIndex);
                if (list == null)
                {
                    list = new List<Survey>();
                }
                list.Add(survey);
            });

            return list;
        }


        private static void AddCommonParams(SurveyAnswersAddRequest model, SqlParameterCollection col)
        {

            col.AddWithValue("@InstanceId", model.InstanceId);
            col.AddWithValue("@QuestionId", model.QuestionId);
            col.AddWithValue("@AnswerOptionId", model.AnswerOptionId);
            col.AddWithValue("@Answer", model.Answer);
            col.AddWithValue("@AnswerNumber", model.AnswerNumber);
        }

        private SurveyAnswers MapSingleSurveyAnswer(IDataReader reader, ref int startingIndex)
        {
            SurveyAnswers answer = new SurveyAnswers();


            answer.Id = reader.GetSafeInt32(startingIndex++);
            answer.InstanceId = reader.GetSafeInt32(startingIndex++);
            answer.Question = new SurveyQuestionAnswerOption();
            answer.Question.QuestionId = reader.GetSafeInt32(startingIndex++);
            answer.Question.Question = reader.GetSafeString(startingIndex++);
            answer.AnswerOption = new BaseSurveyQuestionAnswerOptions();
            answer.AnswerOption.Id = reader.GetSafeInt32(startingIndex++);
            answer.AnswerOption.Text = reader.GetSafeString(startingIndex++);
            answer.AnswerOption.Value = reader.GetSafeString(startingIndex++);
            answer.Answer = reader.GetSafeString(startingIndex++);
            answer.AnswerNumber = reader.GetSafeInt32(startingIndex++);
            answer.DateCreated = reader.GetSafeDateTime(startingIndex++);
            answer.DateModified = reader.GetSafeDateTime(startingIndex++);

            return answer;

        }

        private Survey MapSingleSurvey(IDataReader reader, ref int startingIndex)
        {

            Survey survey = new Survey();

            survey.Id = reader.GetSafeInt32(startingIndex++);
            survey.Name = reader.GetSafeString(startingIndex++);


            return survey;
        }

        private SurveyResult MapSingleSurveyResult(IDataReader reader, ref int startingIndex)
        {

            SurveyResult survey = new SurveyResult();

            survey.SurveyInstanceId = reader.GetSafeInt32(startingIndex++);
            survey.SurveyId = reader.GetSafeInt32(startingIndex++);
            survey.SurveyName = reader.GetSafeString(startingIndex++);
            survey.QuestionId = reader.GetSafeInt32(startingIndex++);
            survey.Question = reader.GetSafeString(startingIndex++);
            survey.QuestionAnswers = JsonConvert.DeserializeObject<List<Answer>>(reader.GetSafeString(startingIndex++));


            return survey;
        }

    }
}

