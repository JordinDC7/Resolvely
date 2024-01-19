using Sabio.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Events
{
    public class Event
    {
        
        public int Id { get; set; }
        public LookUp EventType {  get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string ShortDescription { get; set; }
        public int Venue { get; set; }
        public LookUp EventStatus { get; set; }
        public string ImageUrl { get; set; }
        public string ExternalSiteUrl { get; set; }
        public bool IsFree { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public BaseUser CreatedBy { get; set; }
       
    }
}
