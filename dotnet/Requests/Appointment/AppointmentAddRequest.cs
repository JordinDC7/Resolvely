using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Appointment
{
    public class AppointmentAddRequest
    {
        [Required]
        public int AppointmentTypeId { get; set; }        
        public int? ClientId { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 2)]
        public string Notes { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Location { get; set; }
        [Required]
        public bool IsConfirmed { get; set; }
        [Required]
        public DateTime AppointmentStart { get; set; }
        [Required]
        public DateTime AppointmentEnd { get; set; }
        [Required]
        public int StatusId { get; set; }          
    }
}
