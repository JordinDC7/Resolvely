using System.ComponentModel.DataAnnotations;

namespace Sabio.Models.Domain.FAQs
{
    public class FAQ 
    {
        public int Id { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public LookUp Category { get; set; }

        public int SortOrder { get; set; }
    }
}
