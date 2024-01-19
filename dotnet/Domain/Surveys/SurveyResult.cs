using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Surveys
{
    public class SurveyResult
    {
        public int SurveyInstanceId { get; set; }
        public int SurveyId { get; set; }
        public string SurveyName { get; set; }
        public int QuestionId { get; set; }
        public string Question { get; set; }

        public List<Answer> QuestionAnswers { get; set; }
    }
}
