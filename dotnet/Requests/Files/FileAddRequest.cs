using Sabio.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Files
{
    public class FileAddRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Url { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public int FileTypeId { get; set; }

    }
}
