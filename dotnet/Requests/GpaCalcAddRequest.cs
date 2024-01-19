using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests
{
    public class GpaCalcAddRequest
    {
        [Required]
        [Range(1,4)]
        public int LevelTypeId { get; set; }
        [Required]
        public int CourseId { get; set; }
        [Required]
        public int GradeTypeId { get; set; }
        [Required]
        public int CourseWeightTypeId { get; set; }
        [Required]
        [Range(1,10)]
        public decimal Credits { get; set; }
        [Required]
        [Range(1,5)]
        public int Semester { get; set; }

    }
}
