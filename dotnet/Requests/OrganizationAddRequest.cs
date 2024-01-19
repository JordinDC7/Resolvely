using Sabio.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests
{
    public class OrganizationAddRequest
    {
        [Required]
        public int OrganizationTypeId { get; set; }

        [StringLength(200, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(200, MinimumLength = 3)]
        public string Headline { get; set; }

        [StringLength(10000, MinimumLength = 3)]
        public string Description { get; set; }

        [StringLength(255, MinimumLength = 3)]
        public string Logo { get; set; }

        [Required]
        public int LocationId { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Phone { get; set; }

        [StringLength(255, MinimumLength = 3)]
        [DataType(DataType.Url)]
        public string SiteUrl { get; set; }


    }
}
