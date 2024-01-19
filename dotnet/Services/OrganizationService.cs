using Sabio.Data.Providers;
using Sabio.Models.Domain;
using Sabio.Models.Requests;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Data;
using Sabio.Models;
using Sabio.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using Sabio.Models.Domain.Locations;
using System.Reflection;
using Sabio.Models.Domain.Users;

namespace Sabio.Services.Services
{
    public class OrganizationService : IOrganizationService
    {
        IDataProvider _data = null;
        ILookUpService _lookUpService = null;

        public OrganizationService(IDataProvider data, ILookUpService lookUpService)
        {
            _data = data;
            _lookUpService = lookUpService;
        }

        public int Add(OrganizationAddRequest model, int userId)
        {

            int Id = 0;

            string procName = "[dbo].[Organizations_Insert]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col);
                    col.AddWithValue("@UserId", userId);

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


        public Paged<Organization> SelectAllPaginated(int pageIndex, int pageSize)
        {

            string procName = "[dbo].[Organizations_SelectAll]";

            Paged<Organization> pagedList = null;
            List<Organization> anOrg = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection param)
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
            },

                delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    Organization organization = MapSingleOrganization(reader, ref startingIndex);


                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }

                    if (anOrg == null)
                    {
                        anOrg = new List<Organization>();
                    }

                    anOrg.Add(organization);
                });

            if (anOrg != null)
            {
                pagedList = new Paged<Organization>(anOrg, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }


        public void Update(OrganizationUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Organizations_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);

                col.AddWithValue("@Id", model.Id);
                col.AddWithValue("@CreatedBy", userId);

            }, returnParameters: null);
        }



        public void DeleteById(int id)
        {
            string procName = "[dbo].[Organizations_Delete_ById]";

            _data.ExecuteNonQuery(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            });
        }


        public Organization Get(int id)
        {

            string procName = "[dbo].[Organizations_Select_ById]";

            Organization organization = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {

                paramCollection.AddWithValue("@Id", id);

            }, delegate (IDataReader reader, short set)


            {
                int startingIndex = 0;

                organization = MapSingleOrganization(reader, ref startingIndex);
            });

            return organization;
        }



        public Paged<Organization> SelectByCreatedBy(int pageIndex, int pageSize, int createdBy)
        {

            string procName = "[dbo].[Organizations_Select_ByCreatedBy]";

            Paged<Organization> pagedList = null;
            List<Organization> anOrg = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection param)
            {

                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
                param.AddWithValue("@CreatedBy", createdBy);

            },

                delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    Organization organization = MapSingleOrganization(reader, ref startingIndex);


                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }

                    if (anOrg == null)
                    {
                        anOrg = new List<Organization>();
                    }

                    anOrg.Add(organization);
                });

            if (anOrg != null)
            {
                pagedList = new Paged<Organization>(anOrg, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }


        private static void AddCommonParams(OrganizationAddRequest model, SqlParameterCollection col)
        {


            col.AddWithValue("@OrganizationTypeId", model.OrganizationTypeId);
            col.AddWithValue("@Name", model.Name);
            col.AddWithValue("@Headline", model.Headline);
            col.AddWithValue("@Description", model.Description);
            col.AddWithValue("@Logo", model.Logo);
            col.AddWithValue("@LocationId", model.LocationId);
            col.AddWithValue("@Phone", model.Phone);
            col.AddWithValue("@SiteUrl", model.SiteUrl);

        }


        private Organization MapSingleOrganization(IDataReader reader, ref int startingIndex)
        {
            Organization anOrg = new Organization();

            anOrg.Id = reader.GetSafeInt32(startingIndex++);
            anOrg.OrganizationType = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            anOrg.Name = reader.GetString(startingIndex++);
            anOrg.Headline = reader.GetString(startingIndex++);
            anOrg.Description = reader.GetString(startingIndex++);
            anOrg.Logo = reader.GetString(startingIndex++);
            anOrg.Location = new Location();
            anOrg.Location.Id = reader.GetSafeInt32(startingIndex++);
            anOrg.Location.LocationType = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            anOrg.Location.LineOne = reader.GetString(startingIndex++);
            anOrg.Location.LineTwo = reader.GetSafeString(startingIndex++);
            anOrg.Location.City = reader.GetString(startingIndex++);
            anOrg.Location.Zip = reader.GetSafeString(startingIndex++);
            anOrg.Location.State = _lookUpService.MapLookUp3Col(reader, ref startingIndex);
            anOrg.Location.Latitude = reader.GetSafeDouble(startingIndex++);
            anOrg.Location.Longitude = reader.GetSafeDouble(startingIndex++);
            anOrg.Phone = reader.GetString(startingIndex++);
            anOrg.SiteUrl = reader.GetString(startingIndex++);
            anOrg.DateCreated = reader.GetSafeDateTime(startingIndex++);
            anOrg.DateModified = reader.GetSafeDateTime(startingIndex++);
            anOrg.CreatedBy = reader.DeserializeObject<BaseUser>(startingIndex++);
            anOrg.ModifiedBy = reader.DeserializeObject<BaseUser>(startingIndex++);

            return anOrg;

        }



    }

}