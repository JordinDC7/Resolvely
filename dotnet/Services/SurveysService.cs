using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain.Surveys;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services.Services
{
    public class SurveysService : ISurveysService
    {
        IDataProvider _data = null;
        ILookUpService _lookUpService = null;

        public SurveysService(IDataProvider data, ILookUpService lookUpService)
        {
            _data = data;
            _lookUpService = lookUpService;
        }

        #region Surveys Start

        public int Add(SurveyAddRequest model, int userId)
        {
            int id = 0;
            string procName = "[dbo].[Surveys_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection collection)
            {
                CommonSurveyParams(model, collection);
                collection.AddWithValue("@CreatedBy", userId);
                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;
                collection.Add(idOut);
            }, returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object outPutId = returnCollection["@Id"].Value;
                int.TryParse(outPutId.ToString(), result: out id);
            });

            return id;
        }

        public void Update(SurveyUpdateRequest model, int id)
        {
            string procName = "[dbo].[Surveys_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection collection)
            {
                CommonSurveyParams(model, collection);
                collection.AddWithValue("@Id", model.Id);
            }, returnParameters: null);
        }

        public Paged<Survey> GetAllByCreatedByPaginated(int pageIndex, int pageSize, int createdBy)
        {
            Paged<Survey> pagedList = null;
            List<Survey> list = null;
            int totalCount = 0;

            string procName = "[dbo].[Surveys_Select_ByCreatedBy_Paginated]";
            _data.ExecuteCmd(procName, (param) =>
            {
                param.AddWithValue("@createdBy", createdBy);
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
            },
            (reader, recordSetIndex) =>
            {
                int idx = 0;
                Survey survey = MapSingleSurvey(reader, ref idx);


                if (totalCount == 0)
                {

                    totalCount = reader.GetSafeInt32(idx++);

                }


                if (list == null)
                {
                    list = new List<Survey>();
                }
                list.Add(survey);
            });
            if (list != null)
            {
                pagedList = new Paged<Survey>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<Survey> GetAllPaginated(int pageIndex, int pageSize, int statusId, bool excluded)
        {
            Paged<Survey> pagedList = null;
            List<Survey> list = null;
            int totalCount = 0;

            string procName = "[dbo].[Surveys_SelectAll_Paginated]";
            _data.ExecuteCmd(procName, (param) =>
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
                param.AddWithValue("@StatusId", statusId);
                param.AddWithValue("@Excluded", excluded);

            },
            (reader, recordSetIndex) =>
            {
                int idx = 0;
                Survey survey = MapSingleSurvey(reader, ref idx);
                if (totalCount == 0)
                {

                    totalCount = reader.GetSafeInt32(idx++);

                }

                if (list == null)
                {
                    list = new List<Survey>();
                }
                list.Add(survey);
            });
            if (list != null)
            {
                pagedList = new Paged<Survey>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Survey GetById(int id)
        {
            Survey survey = null;

            string procName = "[dbo].[Surveys_Select_ById]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection collection)
            {
                collection.AddWithValue("@Id", id);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int idx = 0;
                survey = MapSingleSurvey(reader, ref idx);
            });
            return survey;
        }

        public void DeleteById(int id)
        {
            string procName = "[dbo].[Surveys_Delete_ById]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            });
        }

        #endregion

        private static void CommonSurveyParams(SurveyAddRequest model, SqlParameterCollection collection)
        {
            collection.AddWithValue("@Name", model.Name);
            collection.AddWithValue("@Description", model.Description);
            collection.AddWithValue("@StatusId", model.StatusId);
            collection.AddWithValue("@SurveyTypeId", model.SurveyTypeId);
            collection.AddWithValue("@TaskId", model.TaskId);

        }

        private Survey MapSingleSurvey(IDataReader reader, ref int idx)
        {

            Survey survey = new Survey();

            survey.Id = reader.GetSafeInt32(idx++);
            survey.Name = reader.GetSafeString(idx++);
            survey.Description = reader.GetSafeString(idx++);
            survey.TaskId = reader.GetSafeInt32(idx++);
            survey.CreatedBy = JsonConvert.DeserializeObject<BaseUser>(reader.GetSafeString(idx++));
            survey.DateCreated = reader.GetSafeDateTime(idx++);
            survey.DateModified = reader.GetSafeDateTime(idx++);
            survey.Status = _lookUpService.MapSingleLookUp(reader, ref idx);
            survey.SurveyType = _lookUpService.MapSingleLookUp(reader, ref idx);

            return survey;
        }
    }
}
