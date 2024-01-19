using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain.Files;
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
    public class SurveyQuestionsService : ISurveyQuestionsService
    {
        IDataProvider _data = null;
        ILookUpService _lookUpService = null;

        public SurveyQuestionsService(IDataProvider data, ILookUpService lookup)
        {

            _data = data;
            _lookUpService = lookup;

        }

        public int AddQuestion(QuestionAddRequest model, int userId)
        {
            string procName = "[dbo].[SurveyQuestions_Insert]";
            int id = 0;

            DataTable optionsTable = null;


            if (model.Options != null)
            {
                optionsTable = MapOptionsToTable(model.Options);
            }

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col);
                    col.AddWithValue("@BatchQuestionAnswerOptions", optionsTable);
                    col.AddWithValue("@UserId", userId);
                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;

                    col.Add(idOut);

                }, returnParameters: delegate (SqlParameterCollection returnCollection)
                {
                    object oId = returnCollection["@Id"].Value;
                    int.TryParse(oId.ToString(), out id);
                });
            return id;
        }

        public void UpdateQuestion(QuestionUpdateRequest model, int userId)
        {
            string procName = "[dbo].[SurveyQuestions_Update]";

            DataTable optionsTable = null;


            if (model.Options != null)
            {
                optionsTable = MapOptionsToTable(model.Options);
            }

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col);
                    col.AddWithValue("@BatchQuestionAnswerOptions", optionsTable);
                    col.AddWithValue("UserId", userId);
                    col.AddWithValue("Id", model.Id);
                });
        }


        public void DeleteQuestion(int id)
        {
            string procName = "[dbo].[SurveyQuestions_Delete_ById]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {

                    col.AddWithValue("@Id", id);

                });
        }

        public List<SurveyQuestion> GetSurveyByIdWithQuestions(int id)
        {
            string procName = "[dbo].[SurveyQuestions_SelectBySurveyId]";

            List<SurveyQuestion> list = null;

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@Id", id);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int index = 0;
                    SurveyQuestion survey = MapSurveyQuestions(reader, ref index);

                    if (list == null)
                    {
                        list = new List<SurveyQuestion>();
                    }
                    list.Add(survey);
                });

            return list;
        }

        private SurveyQuestion MapSurveyQuestions(IDataReader reader, ref int index)
        {
            SurveyQuestion survey = new SurveyQuestion();

            survey.Id = reader.GetSafeInt32(index++);
            survey.Question = reader.GetSafeString(index++);
            survey.AnswerOptions = reader.DeserializeObject<List<BaseSurveyQuestionAnswerOptions>>(index++);
            survey.HelpText = reader.GetSafeString(index++);
            survey.IsRequired = reader.GetBoolean(index++);
            survey.IsMultipleAllowed = reader.GetBoolean(index++);
            survey.CreatedBy = reader.DeserializeObject<BaseUser>(index++);
            survey.QuestionType = _lookUpService.MapSingleLookUp(reader, ref index);
            survey.SortOrder = reader.GetSafeInt32(index++);
            survey.DateCreated = reader.GetDateTime(index++);
            survey.DateModified = reader.GetDateTime(index++);

            return survey;
        }

        private static void AddCommonParams(QuestionAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Question", model.Question);
            col.AddWithValue("@HelpText", model.HelpText);
            col.AddWithValue("@IsRequired", model.IsRequired);
            col.AddWithValue("@IsMultipleAllowed", model.IsMultipleAllowed);
            col.AddWithValue("@QuestionTypeId", model.QuestionTypeId);
            col.AddWithValue("@SurveyId", model.SurveyId);
            col.AddWithValue("@StatusId", model.StatusId);
            col.AddWithValue("@SortOrder", model.SortOrder);
        }
        private DataTable MapOptionsToTable(List<BaseSurveyQuestionAnswerOptions> options)
        {
            DataTable optionsTable = new DataTable();
            optionsTable.Columns.Add("Text", typeof(string));
            optionsTable.Columns.Add("Value", typeof(string));
            optionsTable.Columns.Add("AdditionalInfo", typeof(string));
            optionsTable.Columns.Add("ShowTextBox", typeof(bool));

            foreach (BaseSurveyQuestionAnswerOptions option in options)
            {
                DataRow optionsRow = optionsTable.NewRow();
                int index = 0;

                optionsRow[index++] = option.Text;
                optionsRow[index++] = option.Value;
                optionsRow[index++] = option.AdditionalInfo;
                optionsRow[index++] = option.ShowTextBox;

                optionsTable.Rows.Add(optionsRow);
            }
            return optionsTable;
        }
    }
}