using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests
{
    public class ModuleAddRequest
    {
        [Required]
        [StringLength(250, MinimumLength = 2)]
        public string Title { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 2)]
        public string Description { get; set; }
        [Required]
        public int StatusTypeId { get; set; }
        [Required]
        public int SortOrder { get; set; }
        [Required]
        public bool HasTasks { get; set; }
        public string ImageUrl { get; set; }        
    }
}
