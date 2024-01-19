using Sabio.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Appointment
{
    public class Appointment
    {
        public int Id { get; set; }
        public LookUp AppointmentType { get; set; }
        public BaseUser Client { get; set; }
        public string Notes {  get; set; }
        public string Location { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime AppointmentStart { get; set; }
        public DateTime AppointmentEnd { get; set;}
        public LookUp Status { get; set; }       
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public BaseUser CreatedBy { get; set; }
        public BaseUser ModifiedBy { get; set; }

    }
}
