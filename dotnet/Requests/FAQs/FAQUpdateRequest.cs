using System.ComponentModel.DataAnnotations;

namespace Sabio.Models.Requests.FAQs
{
    public class FAQUpdateRequest : FAQsAddRequest , IModelIdentifier
    {
        public int Id { get; set; }
    }
}
