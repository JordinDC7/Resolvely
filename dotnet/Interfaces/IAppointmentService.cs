using Sabio.Models;
using Sabio.Models.Domain.Appointment;
using Sabio.Models.Requests.Appointment;
using System.Collections.Generic;

namespace Sabio.Services
{    
    public interface IAppointmentService
    {        
        Appointment Get(int id);
        Paged<Appointment> GetPaginatedByClientId(int pageIndex, int pageSize, int clientId);
        int Add(AppointmentAddRequest model, int userId);
        void Update(AppointmentUpdateRequest model, int userId);
        void Delete(int id);
        List<Appointment> GetByCreatedBy(int userId);
    }
}