using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Experience
{
    public class Experience


    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public LookUp ExperienceType { get; set; }       

        public int IsCurrent { get; set; }

        public DateTime StartDate {  get; set; }

        public DateTime EndDate { get; set; }

        public string JobTitle { get; set; }

        public string CompanyName { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string Description { get; set; }


    }
}
