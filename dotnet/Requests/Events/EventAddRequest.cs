using Sabio.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Events
{
    public class EventAddRequest
    {
        [Required]        
        public int EventTypeId { get; set; }
        [Required]
        [MinLength(0), MaxLength(255)]
        public string Name { get; set; }
        [Required]
        [MinLength(0), MaxLength(255)]
        public string Summary { get; set; }
        [Required]
        [MinLength(0), MaxLength(4000)]
        public string ShortDescription { get; set; }
        [Required]
        public int VenueId { get; set; }
        [Required]
        public int EventStatusId { get; set; }       
        [MinLength(0), MaxLength(400)]
        public string ImageUrl { get; set; }
        [MinLength(0), MaxLength(400)]
        public string ExternalSiteUrl { get; set; }
        [Required]
        public bool IsFree { get; set; }
        [Required]
        public DateTime DateStart { get; set; }
        [Required]
        public DateTime DateEnd { get; set; }       
    }
}
