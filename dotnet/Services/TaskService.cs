using Sabio.Data.Providers;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using Sabio.Models.Domain.Tasks;
using Sabio.Data;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests;

namespace Sabio.Services.Services
{
    public class TaskService : ITaskService
    {
        IDataProvider _data = null;
        ILookUpService _lookUp = null;

        public TaskService(IDataProvider data, ILookUpService lookUp)
        {
            _data = data;
            _lookUp = lookUp;
        }

        public Task Get(int id)
        {
            string procName = "[dbo].[Tasks_Select_ById]";
            Task task = null;

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@Id", id);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    task = MapSingleTask(reader, ref startingIndex);

                }
                );
            return task;
        }

        public List<Task> GetAll(int moduleId)
        {
            string procName = "[dbo].[Tasks_Select_All_ByModuleId]";
            List<Task> list = null;

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@ModuleId", moduleId);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    Task task = MapSingleTask(reader, ref startingIndex);
                    if (list == null)
                    {
                        list = new List<Task>();
                    }
                    list.Add(task);
                }
                );
            return list;
        }

        public int Add(TaskAddRequest model, int userId)
        {
            int id = 0;
            string procName = "[dbo].[Tasks_Insert]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection coll)
                {
                    AddCommonParams(model, coll);

                    coll.AddWithValue("@CreatedBy", userId);

                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;

                    coll.Add(idOut);

                },
                returnParameters: delegate (SqlParameterCollection returnCollection)
                {
                    object OId = returnCollection["@Id"].Value;

                    int.TryParse(OId.ToString(), out id);
                }
                );
            return id;
        }

        public void Update(TaskUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Tasks_Update]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection coll)
                {
                    AddCommonParams(model, coll);
                    coll.AddWithValue("@Id", model.Id);
                    coll.AddWithValue("@ModifiedBy", userId);
                },
                returnParameters: null);
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[Tasks_Delete_ById]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection coll)
                {
                    coll.AddWithValue("@Id", id);
                },
                returnParameters: null
                );
        }



        private static void AddCommonParams(TaskAddRequest model, SqlParameterCollection coll)
        {
            coll.AddWithValue("@ModuleId", model.ModuleId);
            coll.AddWithValue("@Title", model.Title);
            coll.AddWithValue("@Description", model.Description);
            coll.AddWithValue("@Duration", model.Duration);
            coll.AddWithValue("@ImageUrl", model.ImageUrl);
            coll.AddWithValue("@StatusTypeId", model.StatusTypeId);
            coll.AddWithValue("@SortOrder", model.SortOrder);
        }

        private Task MapSingleTask(IDataReader reader, ref int startingIndex)
        {
            Task task = new Task();
            task.Id = reader.GetSafeInt32(startingIndex++);
            task.ModuleId = reader.GetSafeInt32(startingIndex++);
            task.Title = reader.GetSafeString(startingIndex++);
            task.Description = reader.GetSafeString(startingIndex++);
            task.Duration = reader.GetSafeString(startingIndex++);
            task.ImageUrl = reader.GetSafeString(startingIndex++);
            task.Status = _lookUp.MapSingleLookUp(reader, ref startingIndex);
            task.SortOrder = reader.GetSafeInt32(startingIndex++);
            task.CreatedBy = reader.DeserializeObject<BaseUser>(startingIndex++);
            task.ModifiedBy = reader.DeserializeObject<BaseUser>(startingIndex++);
            task.DateCreated = reader.GetSafeDateTime(startingIndex++);
            task.DateModified = reader.GetSafeDateTime(startingIndex++);
            return task;
        }
    }
}
