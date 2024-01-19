using Sabio.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests
{
    public class SurveyAddRequest 
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }
        [Required]
        [StringLength(2000, MinimumLength = 5)]
        public string Description { get; set; }
        public int StatusId { get; set; }
        public int SurveyTypeId { get; set; }
        public int? TaskId { get; set; }       
    }
}
