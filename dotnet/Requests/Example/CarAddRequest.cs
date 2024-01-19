using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sabio.Models.Requests.Example
{
    public class CarAddRequest
    {
        [Required]
        [MinLength(2), MaxLength(100)]
        public string Make { get; set; }

        [Required]
        [MinLength(2), MaxLength(100)]

        public string Model { get; set; }

        [Required]
        [Range(2020, short.MaxValue)]

        public short Year { get; set; }

    }
}
