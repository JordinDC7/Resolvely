using Sabio.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Podcasts
{
    public class Podcasts
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public LookUp PodcastType { get; set; }
        public string CoverImageUrl { get; set; }
        public DateTime DateCreated {  get; set; }
        public DateTime DateModified { get; set; }
        public BaseUser CreatedBy { get; set; }
        public BaseUser ModifiedBy { get; set; }

    }
}
