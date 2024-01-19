using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Appointment;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.Appointment;
using Sabio.Services.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class AppointmentService : IAppointmentService
    {
        IDataProvider _data = null;
        ILookUpService _lookUp = null;

        public AppointmentService(IDataProvider data, ILookUpService lookUp)
        {
            _data = data;
            _lookUp = lookUp;
        }

        public Appointment Get(int id)
        {
            string procName = "[dbo].[Appointments_Select_ById]";
            Appointment appointment = null;

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@Id", id);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    appointment = MapSingleAppointment(reader, ref startingIndex);
                }
                );
            return appointment;
        }

        public Paged<Appointment> GetPaginatedByClientId(int pageIndex, int pageSize, int clientId)
        {
            Paged<Appointment> paginated = null;
            List<Appointment> list = null;
            int totalCount = 0;
            string procName = "[dbo].[Appointments_Select_ByClientId_Paginated]";

            _data.ExecuteCmd(procName,
                (param) =>
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                    param.AddWithValue("@ClientId", clientId);
                },
                (reader, recordSetIndex) =>
                {
                    int startingIndex = 0;
                    Appointment appointment = MapSingleAppointment(reader, ref startingIndex);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }                    

                    if (list == null)
                    {
                        list = new List<Appointment>();
                    }

                    list.Add(appointment);
                }
                );
            if (list != null)
            {
                paginated = new Paged<Appointment>(list, pageIndex, pageSize, totalCount);
            }
            return paginated;
        }

        public List<Appointment> GetByCreatedBy(int userId)
        {
            string procName = "[dbo].[Appointments_Select_ByCreatedBy]";
            List<Appointment> list = null;

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@CreatedBy", userId);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    Appointment appointment = MapSingleAppointment(reader, ref startingIndex);
                    if (list == null)
                    {
                        list = new List<Appointment>();
                    }
                    list.Add(appointment);
                }
                );
            return list;
        }

        public int Add(AppointmentAddRequest model, int userId)
        {
            int id = 0;
            string procName = "[dbo].[Appointments_Insert]";

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

        public void Update(AppointmentUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Appointments_Update]";

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
            string procName = "[dbo].[Appointments_Delete]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection coll)
                {
                    coll.AddWithValue("@Id", id);
                },
                returnParameters: null
                );
        }

        private static void AddCommonParams(AppointmentAddRequest model, SqlParameterCollection coll)
        {
            coll.AddWithValue("@AppointmentTypeId", model.AppointmentTypeId);
            coll.AddWithValue("@ClientId", model.ClientId);
            coll.AddWithValue("@Notes", model.Notes);
            coll.AddWithValue("@Location", model.Location);
            coll.AddWithValue("@IsConfirmed", model.IsConfirmed);
            coll.AddWithValue("@AppointmentStart", model.AppointmentStart);
            coll.AddWithValue("@AppointmentEnd", model.AppointmentEnd);
            coll.AddWithValue("@StatusId", model.StatusId);            
        }

        private  Appointment MapSingleAppointment(IDataReader reader, ref int startingIndex)
        {
            Appointment appointment = new Appointment();          
                        
            appointment.Id = reader.GetSafeInt32(startingIndex++);
            appointment.AppointmentType = _lookUp.MapSingleLookUp(reader, ref startingIndex);            
            appointment.Client = reader.DeserializeObject<BaseUser>(startingIndex++);
            appointment.Notes = reader.GetSafeString(startingIndex++);
            appointment.Location = reader.GetSafeString(startingIndex++);
            appointment.IsConfirmed = reader.GetSafeBool(startingIndex++);
            appointment.AppointmentStart = reader.GetSafeDateTime(startingIndex++);
            appointment.AppointmentEnd = reader.GetSafeDateTime(startingIndex++);
            appointment.Status = _lookUp.MapSingleLookUp(reader, ref startingIndex);             
            appointment.DateCreated = reader.GetSafeDateTime(startingIndex++);
            appointment.DateModified = reader.GetSafeDateTime(startingIndex++);
            appointment.CreatedBy = reader.DeserializeObject<BaseUser>(startingIndex++);
            appointment.ModifiedBy = reader.DeserializeObject<BaseUser>(startingIndex++);
            return appointment;
        }
    }
}
