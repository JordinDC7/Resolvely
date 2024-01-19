using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.SurveyQuestions
{
    public class BaseSurveyQuestionAnswerOptions
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public string AdditionalInfo { get; set; }
        public bool ShowTextBox { get; set; }
    }
}
