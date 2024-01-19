using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Requests;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Domain.Locations;
using Sabio.Models.Domain.Users;
using Sabio.Data;
using Sabio.Models.Domain.Surveys;

namespace Sabio.Services.Services
{
    public class SurveyInstancesService : ISurveyInstancesService

    {
        IDataProvider _data = null;

        public SurveyInstancesService(IDataProvider data)
        {
            _data = data;

        }

        public int Add(int surveyId, int userId)
        {

            int Id = 0;

            string procName = "[dbo].[SurveyInstances_Insert]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {

                col.AddWithValue("@UserId", userId);
                col.AddWithValue("@SurveyId", surveyId);

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



        public Paged<SurveyInstances> SelectAllPaginated(int pageIndex, int pageSize)
        {

            string procName = "[dbo].[SurveyInstances_SelectAll]";

            Paged<SurveyInstances> pagedList = null;
            List<SurveyInstances> aSurveyInstance = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection param)
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
            },

                delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    SurveyInstances surveyInstances = MapSingleSurveyInstance(reader, ref startingIndex);


                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }

                    if (aSurveyInstance == null)
                    {
                        aSurveyInstance = new List<SurveyInstances>();
                    }

                    aSurveyInstance.Add(surveyInstances);
                });

            if (aSurveyInstance != null)
            {
                pagedList = new Paged<SurveyInstances>(aSurveyInstance, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }




        public void Update(SurveyInstancesUpdateRequest model, int userId)
        {
            string procName = "[dbo].[SurveyInstances_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@SurveyId", model.SurveyId);
                col.AddWithValue("@UserId", userId);
                col.AddWithValue("@Id", model.Id);

            }, returnParameters: null);
        }


        public void DeleteById(int id)
        {
            string procName = "[dbo].[SurveyInstances_Delete_ById]";

            _data.ExecuteNonQuery(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            });
        }



        public SurveyInstances Get(int id)
        {

            string procName = "[dbo].[SurveyInstances_Select_ById]";

            SurveyInstances surveyInstances = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {

                paramCollection.AddWithValue("@Id", id);

            }, delegate (IDataReader reader, short set)


            {
                int startingIndex = 0;

                surveyInstances = MapSingleSurveyInstance(reader, ref startingIndex);
            });

            return surveyInstances;
        }


        public Paged<SurveyInstances> SelectByCreatedBy(int pageIndex, int pageSize, int createdBy)
        {

            string procName = "[dbo].[SurveyInstances_Select_ByCreatedBy]";

            Paged<SurveyInstances> pagedList = null;
            List<SurveyInstances> anInstance = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection param)
            {

                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
                param.AddWithValue("@UserId", createdBy);

            },

                delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    SurveyInstances surveyInstances = MapSingleSurveyInstance(reader, ref startingIndex);


                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }

                    if (anInstance == null)
                    {
                        anInstance = new List<SurveyInstances>();
                    }

                    anInstance.Add(surveyInstances);
                });

            if (anInstance != null)
            {
                pagedList = new Paged<SurveyInstances>(anInstance, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }


        public Paged<SurveyInstances> SelectBySurveyId(int pageIndex, int pageSize, int surveyId)
        {

            string procName = "[dbo].[SurveyInstances_Select_BySurveyId]";

            Paged<SurveyInstances> pagedList = null;
            List<SurveyInstances> anInstance = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection param)
            {

                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
                param.AddWithValue("@SurveyId", surveyId);

            },

                delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    SurveyInstances surveyInstances = MapSingleSurveyInstance(reader, ref startingIndex);


                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }

                    if (anInstance == null)
                    {
                        anInstance = new List<SurveyInstances>();
                    }

                    anInstance.Add(surveyInstances);
                });

            if (anInstance != null)
            {
                pagedList = new Paged<SurveyInstances>(anInstance, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        private SurveyInstances MapSingleSurveyInstance(IDataReader reader, ref int startingIndex)
        {
            SurveyInstances anInstance = new SurveyInstances();


            anInstance.Id = reader.GetSafeInt32(startingIndex++);
            anInstance.SurveyName = reader.GetSafeString(startingIndex++);
            anInstance.UserId = reader.DeserializeObject<BaseUser>(startingIndex++);
            anInstance.DateCreated = reader.GetSafeDateTime(startingIndex++);
            anInstance.DateModified = reader.GetSafeDateTime(startingIndex++);



            return anInstance;

        }






    }

}