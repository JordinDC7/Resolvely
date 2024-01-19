using Sabio.Models.Domain.SurveyQuestions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Surveys
{
    public class SurveyAnswers
    {
        public int Id { get; set; }

        public int InstanceId { get; set; }

        public SurveyQuestionAnswerOption Question { get; set; }
        public BaseSurveyQuestionAnswerOptions AnswerOption { get; set; }

        public string Answer { get; set; }

        public int AnswerNumber { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

    }
}
