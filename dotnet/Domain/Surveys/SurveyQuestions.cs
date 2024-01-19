using Sabio.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Surveys
{
    public class SurveyQuestionAnswerOption
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public BaseUser CreatedBy { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
