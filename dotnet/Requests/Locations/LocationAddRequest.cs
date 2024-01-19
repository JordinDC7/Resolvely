using Sabio.Models.Domain;
using Sabio.Models.Domain.Locations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Locations
{
    public class LocationAddRequest
    {
        [Required]
        public int LocationTypeId { get; set; }

        [Required]
        [MaxLength(255)]
        public string LineOne { get; set; }

        //Line Two is nullable (not required)
        [MaxLength(255)]
        public string LineTwo { get; set; }

        [Required]
        [MaxLength(255)]
        public string City { get; set; }

        //Zip is nullable (not required)
        [MaxLength(50)]
        public string Zip { get; set; }

        [Required]
        [Range(1, 999)]
        public int StateId { get; set; }

        [Required]
        [Range(-90, 90)]
        public double Latitude { get; set; }

        [Required]
        [Range(-180, 180)]
        public double Longitude { get; set; }





    }
}
