using Sabio.Models.Domain.Locations;
using Sabio.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain
{
    public class Organization
    {
        public int Id { get; set; }

        public LookUp OrganizationType { get; set; }
        
        public string Name { get; set; }

        public string Headline { get; set; }

        public string Description { get; set; }

        public string Logo { get; set; }


        public Location Location { get; set; }

        public string Phone { get; set; }

        public string SiteUrl { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public BaseUser CreatedBy { get; set; }
        public BaseUser ModifiedBy { get; set; }



    }
}
