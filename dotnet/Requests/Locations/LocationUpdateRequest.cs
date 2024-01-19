using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Locations
{
    public class LocationUpdateRequest : LocationAddRequest, IModelIdentifier
    {      
        public int Id { get; set; }
    }
}
