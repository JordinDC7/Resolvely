using System;
using System.ComponentModel.DataAnnotations;

namespace Sabio.Models.Requests.FAQs
{
    public class FAQsAddRequest
    {
        [Required]
        [StringLength(225, ErrorMessage = "Question must be between 1 and 255 characters.", MinimumLength = 1)]
        public string Question { get; set; }

        [Required]
        [StringLength(2000, ErrorMessage = "Answer must be between 1 and 2000 characters.", MinimumLength = 1)]
        public string Answer { get; set; }

        [Required(ErrorMessage = "CategoryId is required.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "SortOrder is required.")]
        public int SortOrder { get; set; }
    }
}
