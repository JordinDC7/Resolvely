using Sabio.Data.Providers;
using Sabio.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Domain.Podcasts;
using System.Reflection;
using Sabio.Models.Requests;
using Sabio.Data;
using Sabio.Services.Interfaces;
using System.Diagnostics;
using Sabio.Models.Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;
using Sabio.Models.Domain.Users;

namespace Sabio.Services.Services
{
    public class PodcastService : IPodcastService
    {
        IDataProvider _data = null;
        ILookUpService _lookUp = null;

        public PodcastService(IDataProvider data, ILookUpService lookup)
        {
            _data = data;
            _lookUp = lookup;
        }
        public int AddPodcast(PodcastAddRequest model, int userId)
        {
            string procName = "[dbo].[Podcasts_Insert]";
            int id = 0;

            _data.ExecuteNonQuery(procName,
             inputParamMapper: delegate (SqlParameterCollection col)
             {
                 AddCommonParams(model, col);

                 col.AddWithValue("@CreatedBy", userId);

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
        public Paged<Podcasts> SelectAllPagination(int pageIndex, int pageSize)
        {
            Paged<Podcasts> pageList = null;
            List<Podcasts> list = null;
            string procName = "[dbo].[Podcasts_SelectAll_Paginated]";
            int totalCount = 0;

            _data.ExecuteCmd(procName, (param) =>
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
            },
            (reader, recordSetIndex) =>
            {
                int index = 0;
                Podcasts podcasts = MapSinglePodcasts(reader, ref index);
                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(index++);
                }
                if (list == null)
                {
                    list = new List<Podcasts>();
                }
                list.Add(podcasts);
            });
            if (list != null)
            {
                pageList = new Paged<Podcasts>(list, pageIndex, pageSize, totalCount);
            }
            return pageList;
        }
        public Paged<Podcasts> SelectByCreatedBy(int pageIndex, int pageSize, int createdByIdQuery)
        {
            Paged<Podcasts> pageList = null;
            List<Podcasts> list = null;
            string procName = "[dbo].[Podcasts_Select_ByCreatedBy_Paginated]";
            int totalCount = 0;

            _data.ExecuteCmd(procName, (param) =>
            {
                param.AddWithValue("@CreatedBy", createdByIdQuery);
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
            },
            (reader, recordSetIndex) =>
            {
                int index = 0;
                Podcasts podcasts = MapSinglePodcasts(reader, ref index);
                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(index++);
                }
                if (list == null)
                {
                    list = new List<Podcasts>();
                }
                list.Add(podcasts);
            });
            if (list != null)
            {
                pageList = new Paged<Podcasts>(list, pageIndex, pageSize, totalCount);
            }
            return pageList;
        }
        public Paged<Podcasts> SearchPagination(int pageIndex, int pageSize, string query)
        {
            Paged<Podcasts> pageList = null;
            List<Podcasts> list = null;
            string procName = "[dbo].[Podcasts_Search_Paginated]";
            int totalCount = 0;

            _data.ExecuteCmd(procName, (param) =>
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
                param.AddWithValue("@Query", query);
            },
            (reader, recordSetIndex) =>
            {
                int index = 0;
                Podcasts podcasts = MapSinglePodcasts(reader, ref index);
                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(index++);
                }
                if (list == null)
                {
                    list = new List<Podcasts>();
                }
                list.Add(podcasts);
            });
            if (list != null)
            {
                pageList = new Paged<Podcasts>(list, pageIndex, pageSize, totalCount);
            }
            return pageList;
        }

        public void Update(PodcastUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Podcasts_Update]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col);
                    col.AddWithValue("@Id", model.Id);
                    col.AddWithValue("@ModifiedBy", userId);
                },
                        returnParameters: null);
        }

        public void PodcastsDelete(int id)
        {
            string procName = "[dbo].[Podcasts_Delete_ById]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", id);

                }, returnParameters: null);
        }
        private Podcasts MapSinglePodcasts(IDataReader reader, ref int index)
        {
            Podcasts podcasts = new Podcasts();

            podcasts.Id = reader.GetSafeInt32(index++);
            podcasts.Title = reader.GetSafeString(index++);
            podcasts.PodcastType = _lookUp.MapSingleLookUp(reader, ref index);
            podcasts.Description = reader.GetSafeString(index++);
            podcasts.Url = reader.GetSafeString(index++);
            podcasts.CoverImageUrl = reader.GetSafeString(index++);
            podcasts.DateCreated = reader.GetSafeDateTime(index++);
            podcasts.DateModified = reader.GetSafeDateTime(index++);
            podcasts.CreatedBy = reader.DeserializeObject<BaseUser>(index++);
            podcasts.ModifiedBy = reader.DeserializeObject<BaseUser>(index++);

            return podcasts;
        }
        private static void AddCommonParams(PodcastAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Title", model.Title);
            col.AddWithValue("@Description", model.Description);
            col.AddWithValue("@Url", model.Url);
            col.AddWithValue("@PodcastTypeId", model.PodcastTypeId);
            col.AddWithValue("@CoverImageUrl", model.CoverImageUrl);
        }
    }
}


