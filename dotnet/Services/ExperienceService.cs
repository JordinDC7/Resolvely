using Sabio.Data.Providers;
using Sabio.Models.Domain.Locations;
using Sabio.Models;
using Sabio.Models.Requests.Experience;
using Sabio.Models.Requests.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Domain.Experience;
using Sabio.Services.Interfaces;
using System.Data.SqlClient;
using System.Data;
using Sabio.Data;
using Sabio.Models.Domain.Users;

namespace Sabio.Services.Services
{
    public class ExperienceService : IExperienceService
    {
        IDataProvider _data = null;
        ILookUpService _lookUpService = null;

        public ExperienceService(IDataProvider data, ILookUpService lookUpService)
        {
            _data = data;
            _lookUpService = lookUpService;
        }

        public void Add(List<ExperienceAddRequest> requestModel, int userId)
        {

            string procName = "[dbo].[Experience_InsertBatch]";
            DataTable experienceTable = MapExperiencesToTable(requestModel);

            _data.ExecuteNonQuery
                (procName,
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@BatchExperience", experienceTable);
                paramCollection.AddWithValue("@UserId", userId);
            }
            );

        }

        public void Update(ExperienceUpdateRequest requestModel, int userId)
        {
            string procName = "[dbo].[Experience_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", requestModel.Id);
                paramCollection.AddWithValue("@UserId", userId);
                paramCollection.AddWithValue("@ExperienceTypeId", requestModel.ExperienceTypeId);
                paramCollection.AddWithValue("@IsCurrent", requestModel.IsCurrent);
                paramCollection.AddWithValue("@StartDate", requestModel.StartDate);
                if (requestModel.EndDate == null) { paramCollection.AddWithValue("@EndDate", DBNull.Value); }
                else { paramCollection.AddWithValue("@EndDate", requestModel.EndDate); }
                paramCollection.AddWithValue("@JobTitle", requestModel.JobTitle);
                paramCollection.AddWithValue("@CompanyName", requestModel.CompanyName);
                paramCollection.AddWithValue("@City", requestModel.City);
                paramCollection.AddWithValue("@State", requestModel.State);
                paramCollection.AddWithValue("@Country", requestModel.Country);
                paramCollection.AddWithValue("@Description", requestModel.Description);

            },
            returnParameters: null);
        }


        public void Delete(int id)
        {
            string procName = "[dbo].[Experience_Delete_ById]";
            _data.ExecuteNonQuery(procName,
            inputParamMapper: delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@Id", id);
            },
            returnParameters: null
            );
        }

        public Experience GetById(int id)
        {
            string procName = "[dbo].[Experience_Select_ById]";
            Experience experience = null;

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@Id", id);
            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                experience = SingleExperienceMapper(reader, ref startingIndex);
            }
            );
            return experience;
        }

        public Paged<Experience> GetAll(int pageIndex, int pageSize)
        {
            Paged<Experience> pagedList = null;
            List<Experience> list = null;
            int totalCount = 0;
            string procName = "[dbo].[Experience_Select_All_Paginated]";

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@PageIndex", pageIndex);
                    paramCollection.AddWithValue("@PageSize", pageSize);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    Experience experience = SingleExperienceMapper(reader, ref startingIndex);
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }

                    if (list == null)
                    {
                        list = new List<Experience>();
                    }
                    list.Add(experience);
                });
            if (list != null)
            {
                pagedList = new Paged<Experience>(list, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }

        private DataTable MapExperiencesToTable(List<ExperienceAddRequest> requestModel)
        {
            DataTable experienceTable = new DataTable();

            experienceTable.Columns.Add("ExperienceTypeId", typeof(int));
            experienceTable.Columns.Add("IsCurrent", typeof(int));
            experienceTable.Columns.Add("StartDate", typeof(DateTime));
            experienceTable.Columns.Add("EndDate", typeof(DateTime)).AllowDBNull = true;
            experienceTable.Columns.Add("JobTitle", typeof(string));
            experienceTable.Columns.Add("CompanyName", typeof(string));
            experienceTable.Columns.Add("City", typeof(string));
            experienceTable.Columns.Add("State", typeof(string));
            experienceTable.Columns.Add("Country", typeof(string));
            experienceTable.Columns.Add("Description", typeof(string));

            foreach (ExperienceAddRequest oneExperience in requestModel)
            {
                DataRow row = experienceTable.NewRow();
                int startingIndex = 0;

                row[startingIndex++] = oneExperience.ExperienceTypeId;
                row[startingIndex++] = oneExperience.IsCurrent;
                row[startingIndex++] = oneExperience.StartDate;
                if (oneExperience.EndDate == null)
                {
                    row[startingIndex++] = DBNull.Value;
                }
                else
                { row[startingIndex++] = oneExperience.StartDate; }
                row[startingIndex++] = oneExperience.JobTitle;
                row[startingIndex++] = oneExperience.CompanyName;
                row[startingIndex++] = oneExperience.City;
                row[startingIndex++] = oneExperience.State;
                row[startingIndex++] = oneExperience.Country;
                row[startingIndex++] = oneExperience.Description;

                experienceTable.Rows.Add(row);

            }

            return experienceTable;
        }



        private Experience SingleExperienceMapper(IDataReader reader, ref int startingIndex)
        {
            Experience experience = new Experience();

            experience.Id = reader.GetSafeInt32(startingIndex++);
            experience.UserId = reader.GetSafeInt32(startingIndex++);
            experience.ExperienceType = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            experience.IsCurrent = reader.GetSafeInt32(startingIndex++);
            experience.StartDate = reader.GetSafeDateTime(startingIndex++);
            experience.EndDate = reader.GetSafeDateTime(startingIndex++);
            experience.JobTitle = reader.GetSafeString(startingIndex++);
            experience.CompanyName = reader.GetSafeString(startingIndex++);
            experience.City = reader.GetSafeString(startingIndex++);
            experience.State = reader.GetSafeString(startingIndex++);
            experience.Country = reader.GetSafeString(startingIndex++);
            experience.Description = reader.GetSafeString(startingIndex++);
            return experience;
        }

    }

}
