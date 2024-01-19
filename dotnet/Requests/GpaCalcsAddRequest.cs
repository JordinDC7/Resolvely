using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests
{
    public class GpaCalcsAddRequest
    {
        [Required]
        public List <GpaCalcAddRequest> GpaCalc {  get; set; }
    }
}
