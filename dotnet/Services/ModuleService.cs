using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models.Domain.Modules;
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
    public class ModuleService : IModuleService
    {
        IDataProvider _data = null;
        ILookUpService _lookUp = null;

        public ModuleService(IDataProvider data, ILookUpService lookUp)
        {
            _data = data;
            _lookUp = lookUp;
        }

        public Module Get(int id)
        {
            string procName = "[dbo].[Modules_Select_ById]";
            Module module = null;

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@Id", id);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    module = MapSingleModule(reader, ref startingIndex);
                }
                );
            return module;
        }



        public List<Module> GetAll()
        {
            string procName = "[dbo].[Modules_SelectAll]";
            List<Module> list = null;

            _data.ExecuteCmd(procName,
                inputParamMapper: null,
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    Module module = MapSingleModule(reader, ref startingIndex);
                    if (list == null)
                    {
                        list = new List<Module>();
                    }
                    list.Add(module);
                }
                );
            return list;
        }

        public int Add(ModuleAddRequest model, int userId)
        {
            int id = 0;
            string procName = "[dbo].[Modules_Insert]";

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
        public void Update(ModuleUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Modules_Update]";

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
            string procName = "[dbo].[Modules_Delete_ById]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection coll)
                {
                    coll.AddWithValue("@Id", id);
                },
                returnParameters: null
                );
        }

        private Module MapSingleModule(IDataReader reader, ref int startingIndex)
        {
            Module module = new Module();
            module.Id = reader.GetSafeInt32(startingIndex++);
            module.Title = reader.GetSafeString(startingIndex++);
            module.Description = reader.GetSafeString(startingIndex++);
            module.Status = _lookUp.MapSingleLookUp(reader, ref startingIndex);
            module.SortOrder = reader.GetSafeInt32(startingIndex++);
            module.HasTasks = reader.GetSafeBool(startingIndex++);
            module.ImageUrl = reader.GetSafeString(startingIndex++);
            module.CreatedBy = reader.DeserializeObject<BaseUser>(startingIndex++);
            module.ModifiedBy = reader.DeserializeObject<BaseUser>(startingIndex++);
            module.DateCreated = reader.GetSafeDateTime(startingIndex++);
            module.DateModified = reader.GetSafeDateTime(startingIndex++);
            return module;
        }

        private static void AddCommonParams(ModuleAddRequest model, SqlParameterCollection coll)
        {
            coll.AddWithValue("@Title", model.Title);
            coll.AddWithValue("@Description", model.Description);
            coll.AddWithValue("@StatusTypeId", model.StatusTypeId);
            coll.AddWithValue("@SortOrder", model.SortOrder);
            coll.AddWithValue("@HasTasks", model.HasTasks);
            coll.AddWithValue("@ImageUrl", model.ImageUrl);
        }

    }
}
