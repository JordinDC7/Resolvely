using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain
{
    public class GpaCalc
    {
        public int Id { get; set; }
        public int LevelTypeId { get; set; }
        public int CourseId { get; set; }
        public int GradeTypeId { get; set; }
        public int CourseWeightTypeId { get; set; }
        public decimal Credits { get; set; }
        public int Semester {  get; set; }
        public int CreatedBy { get; set; }

    }
}
