using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Events;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.Events;
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
    public class EventService : IEventService
    {
        IDataProvider _data = null;
        ILookUpService _lookUpService = null;
        IUserService _userService = null;

        public EventService(IDataProvider data, ILookUpService lookUpService, IUserService userService)
        {
            _data = data;
            _lookUpService = lookUpService;
            _userService = userService;
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[Events_Delete_ById]";
            _data.ExecuteNonQuery(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            });
        }

        public Event GetById(int id)
        {
            string procName = "[dbo].[Events_Select_ById]";
            Event event1 = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            }, delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                event1 = SingleEventMapper(reader, ref startingIndex);
            });
            return event1;
        }

        public Paged<Event> GetByCreatedBy(int pageIndex, int pageSize, int userId)
        {
            Paged<Event> pagedList = null;
            List<Event> list = null;
            int totalCount = 0;
            string procName = "[dbo].[Events_Select_ByCreatedBy_Pagination]";

            _data.ExecuteCmd(procName,
                delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@PageIndex", pageIndex);
                    paramCollection.AddWithValue("@PageSize", pageSize);
                    paramCollection.AddWithValue("@CreatedBy", userId);
                },
                 delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    Event oneEvent = SingleEventMapper(reader, ref startingIndex);
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }
                    if (list == null)
                    {
                        list = new List<Event>();
                    }

                    list.Add(oneEvent);
                });
            if (list != null)
            {
                pagedList = new Paged<Event>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<Event> GetByQuery(int pageIndex, int pageSize, string query)
        {
            Paged<Event> pagedList = null;
            List<Event> list = null;
            int totalCount = 0;
            string procName = "[dbo].[Events_Search_Pagination]";

            _data.ExecuteCmd(procName,
                delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@PageIndex", pageIndex);
                    paramCollection.AddWithValue("@PageSize", pageSize);
                    paramCollection.AddWithValue("@Query", query);
                },
                 delegate (IDataReader reader, short set)
                 {
                     int startingIndex = 0;
                     Event oneEvent = SingleEventMapper(reader, ref startingIndex);
                     if (totalCount == 0)
                     {
                         totalCount = reader.GetSafeInt32(startingIndex++);
                     }
                     if (list == null)
                     {
                         list = new List<Event>();
                     }

                     list.Add(oneEvent);
                 });
            if (list != null)
            {
                pagedList = new Paged<Event>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<Event> GetAll(int pageIndex, int pageSize)
        {
            Paged<Event> pagedList = null;
            List<Event> list = null;
            int totalCount = 0;
            string procName = "[dbo].[Events_SelectAll_Pagination]";

            _data.ExecuteCmd(procName,
                 delegate (SqlParameterCollection paramCollection)
                 {
                     paramCollection.AddWithValue("@PageIndex", pageIndex);
                     paramCollection.AddWithValue("@PageSize", pageSize);
                 },
                 delegate (IDataReader reader, short set)
                 {
                     int startingIndex = 0;

                     Event anEvent = SingleEventMapper(reader, ref startingIndex);

                     if (totalCount == 0)
                     {
                         totalCount = reader.GetSafeInt32(startingIndex++);
                     }

                     if (list == null)
                     {
                         list = new List<Event>();
                     }

                     list.Add(anEvent);
                 });

            if (list != null)
            {
                pagedList = new Paged<Event>(list, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }

        public void Update(EventUpdateRequest model, int userId)
        {

            string procName = "[dbo].[Events_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                AddCommonParams(model, paramCollection);
                paramCollection.AddWithValue("@Id", model.Id);
                paramCollection.AddWithValue("@CreatedBy", userId);
            }, returnParameters: null);
        }

        public int Add(EventAddRequest request, int createdById)
        {
            int id = 0;
            string procName = "[dbo].[Events_Insert]";

            _data.ExecuteNonQuery(
                procName
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    AddCommonParams(request, paramCollection);
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

        private void AddCommonParams(EventAddRequest request, SqlParameterCollection paramCollection)
        {
            paramCollection.AddWithValue("@EventTypeId", request.EventTypeId);
            paramCollection.AddWithValue("@Name", request.Name);
            paramCollection.AddWithValue("@Summary", request.Summary);
            paramCollection.AddWithValue("@ShortDescription", request.ShortDescription);
            paramCollection.AddWithValue("@VenueId", request.VenueId);
            paramCollection.AddWithValue("@EventStatusId", request.EventStatusId);
            paramCollection.AddWithValue("@ImageUrl", request.ImageUrl);
            paramCollection.AddWithValue("@ExternalSiteUrl", request.ExternalSiteUrl);
            paramCollection.AddWithValue("@IsFree", request.IsFree);
            paramCollection.AddWithValue("@DateStart", request.DateStart);
            paramCollection.AddWithValue("@DateEnd", request.DateEnd);

        }
        private Event SingleEventMapper(IDataReader reader, ref int startingIndex)
        {
            Event aEvent = new Event();

            aEvent.Id = reader.GetSafeInt32(startingIndex++);
            aEvent.EventType = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            aEvent.Name = reader.GetSafeString(startingIndex++);
            aEvent.Summary = reader.GetSafeString(startingIndex++);
            aEvent.ShortDescription = reader.GetSafeString(startingIndex++);
            aEvent.Venue = reader.GetSafeInt32(startingIndex++);
            aEvent.EventStatus = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            aEvent.ImageUrl = reader.GetSafeString(startingIndex++);
            aEvent.ExternalSiteUrl = reader.GetSafeString(startingIndex++);
            aEvent.IsFree = reader.GetSafeBool(startingIndex++);
            aEvent.DateCreated = reader.GetSafeDateTime(startingIndex++);
            aEvent.DateModified = reader.GetSafeDateTime(startingIndex++);
            aEvent.DateStart = reader.GetSafeDateTime(startingIndex++);
            aEvent.DateEnd = reader.GetSafeDateTime(startingIndex++);
            aEvent.CreatedBy = reader.DeserializeObject<BaseUser>(startingIndex++);
            return aEvent;
        }

    }
}

