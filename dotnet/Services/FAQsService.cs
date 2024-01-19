using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models.Domain.FAQs;
using Sabio.Models.Requests.FAQs;
using Sabio.Services.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sabio.Services.Services
{
    public class FAQsService : IFAQsService
    {
        IDataProvider _dataProvider = null;
        ILookUpService _lookupService = null;
        public FAQsService(IDataProvider data, ILookUpService lookUpService)
        {
            _dataProvider = data;
            _lookupService = lookUpService;
        }

        private static void AddCommonParams(FAQsAddRequest requestModel, SqlParameterCollection col)
        {
            col.AddWithValue("@Question", requestModel.Question);
            col.AddWithValue("@Answer", requestModel.Answer);
            col.AddWithValue("@CategoryId", requestModel.CategoryId);
            col.AddWithValue("@SortOrder", requestModel.SortOrder);
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[FAQs_Delete]";

            _dataProvider.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);

            }, returnParameters: null);
        }


        public int Add(FAQsAddRequest modelFAQ, int createdById)
        {
            int id = 0;
            string procName = $"[dbo].[FAQs_Insert]";

            _dataProvider.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(modelFAQ, col);
                col.AddWithValue("@CreatedBy", createdById);
                col.AddWithValue("@ModifiedBy", createdById);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;
                col.Add(idOut);
            },
            returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object oId = returnCollection["@Id"].Value;
                int.TryParse(oId.ToString(), out id);
            });
            return id;
        }

        public List<FAQ> GetAllFAQs()
        {
            List<FAQ> list = null;

            string procName = $"[dbo].[FAQs_SelectAll]";

            _dataProvider.ExecuteCmd(procName, inputParamMapper: null
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                FAQ aFAQ = MapSingleFAQ(reader);

                if (list == null)
                {
                    list = new List<FAQ>();
                }
                list.Add(aFAQ);
            });
            return list;
        }

        public List<FAQ> GetByCategoryId(int categoryId)
        {
            List<FAQ> list = null;

            string procName = $"[dbo].[FAQs_Select_ByCategoryId]";

            _dataProvider.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection parameters)
            {
                parameters.AddWithValue("@CategoryId", categoryId);
            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                FAQ aFAQ = MapSingleFAQ(reader);

                if (list == null)
                {
                    list = new List<FAQ>();
                }
                list.Add(aFAQ);
            });
            return list;
        }


        public void UpdateFAQ(FAQUpdateRequest requestModel, int modifiedById)
        {
            string procName = "[dbo].[FAQs_Update]";
            _dataProvider.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(requestModel, col);
                col.AddWithValue("@Id", requestModel.Id);
                col.AddWithValue("@ModifiedBy", modifiedById);
            },
            returnParameters: null
            );
        }


        private FAQ MapSingleFAQ(IDataReader reader)
        {
            FAQ aFAQ = new FAQ();

            int index = 0;
            aFAQ.Id = reader.GetSafeInt32(index++);
            aFAQ.Question = reader.GetSafeString(index++);
            aFAQ.Answer = reader.GetSafeString(index++);
            aFAQ.Category = _lookupService.MapSingleLookUp(reader, ref index);
            aFAQ.SortOrder = reader.GetSafeInt32(index++);
            return aFAQ;
        }
    }
}