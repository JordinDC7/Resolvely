using Newtonsoft.Json;
using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Locations;
using Sabio.Models.Requests.Locations;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services.Services
{
    public class LocationService : ILocationService
    {
        IDataProvider _data = null;
        ILookUpService _lookUpService = null;


        public LocationService(IDataProvider data, ILookUpService lookUpService)
        {
            _data = data;
            _lookUpService = lookUpService;
        }

        public int Add(LocationAddRequest requestModel, int createdById)
        {
            int id = 0;
            string procName = "[dbo].[Locations_Insert]";

            _data.ExecuteNonQuery(
                procName
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    AddCommonParams(requestModel, paramCollection);
                    paramCollection.AddWithValue("@CreatedBy", createdById);

                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);

                    idOut.Direction = ParameterDirection.Output;
                    paramCollection.Add(idOut);
                },
                returnParameters: delegate (SqlParameterCollection returnCollection)
                {
                    object oId = returnCollection["@Id"].Value;
                    int.TryParse(oId.ToString(), out id);
                });
            return id;

        }

        public void Update(LocationUpdateRequest requestModel, int modifiedById)
        {
            string procName = "[dbo].[Locations_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                AddCommonParams(requestModel, paramCollection);
                paramCollection.AddWithValue("@Id", requestModel.Id);
                paramCollection.AddWithValue("@ModifiedBy", modifiedById);
            },
            returnParameters: null
            );
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[Locations_DeleteById]";
            _data.ExecuteNonQuery(procName,
            inputParamMapper: delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@Id", id);
            },
            returnParameters: null
            );
        }

        public Paged<Location> GetAll(int pageIndex, int pageSize)
        {
            Paged<Location> pagedList = null;
            List<Location> list = null;
            int totalCount = 0;
            string procName = "[dbo].[Locations_SelectAll_Paginated]";

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@PageIndex", pageIndex);
                paramCollection.AddWithValue("@PageSize", pageSize);
            },
                singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                Location location = SingleLocationMapper(reader, ref startingIndex);
                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(startingIndex++);
                }

                if (list == null)
                {
                    list = new List<Location>();
                }
                list.Add(location);
            });

            if (list != null)
            {
                pagedList = new Paged<Location>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Location GetById(int id)
        {
            string procName = "[dbo].[Locations_Select_ById]";
            Location location = null;

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                location = SingleLocationMapper(reader, ref startingIndex);
            }
            );
            return location;
        }

        public Paged<Location> GetByCreatedBy(int createdById, int pageIndex, int pageSize)
        {
            Paged<Location> pagedList = null;
            List<Location> list = null;
            int totalCount = 0;
            string procName = "[dbo].[Locations_Select_ByCreatedBy_Paginated]";


            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@CreatedBy", createdById);
                paramCollection.AddWithValue("@PageIndex", pageIndex);
                paramCollection.AddWithValue("@PageSize", pageSize);
            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                Location location = SingleLocationMapper(reader, ref startingIndex);
                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(startingIndex++);
                }

                if (list == null)
                {
                    list = new List<Location>();
                }
                list.Add(location);
            });

            if (list != null)
            {
                pagedList = new Paged<Location>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        private void AddCommonParams(LocationAddRequest requestModel, SqlParameterCollection paramCollection)
        {
            paramCollection.AddWithValue("@LocationTypeId", requestModel.LocationTypeId);
            paramCollection.AddWithValue("@LineOne", requestModel.LineOne);
            paramCollection.AddWithValue("@LineTwo", requestModel.LineTwo);
            paramCollection.AddWithValue("@City", requestModel.City);
            paramCollection.AddWithValue("@Zip", requestModel.Zip);
            paramCollection.AddWithValue("@StateId", requestModel.StateId);
            paramCollection.AddWithValue("@Latitude", requestModel.Latitude);
            paramCollection.AddWithValue("@Longitude", requestModel.Longitude);
        }

        public Location SingleLocationMapper(IDataReader reader, ref int startingIndex)
        {
            Location location = new Location();

            location.Id = reader.GetSafeInt32(startingIndex++);
            location.LocationType = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            location.LineOne = reader.GetSafeString(startingIndex++);
            location.LineTwo = reader.GetSafeString(startingIndex++);
            location.City = reader.GetSafeString(startingIndex++);
            location.Zip = reader.GetSafeString(startingIndex++);
            location.State = _lookUpService.MapLookUp3Col(reader, ref startingIndex);
            location.Latitude = reader.GetSafeDouble(startingIndex++);
            location.Longitude = reader.GetSafeDouble(startingIndex++);

            return location;
        }






    }
}
