using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Experience
{
    public class ExperienceUpdateRequest : ExperienceAddRequest, IModelIdentifier
    {
        public int Id { get; set; }
    }
}
