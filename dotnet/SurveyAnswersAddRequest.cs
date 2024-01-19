using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests
{
    public class SurveyAnswersAddRequest
    {
        [Required]
        public int InstanceId { get; set; }
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public int AnswerOptionId { get; set; }
        [StringLength(500, MinimumLength = 1)]
        public string Answer { get; set; }
        [Required]
        public int AnswerNumber { get; set; }

    }
}
