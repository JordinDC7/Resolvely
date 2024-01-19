using Sabio.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Notes
{
    public class Notes
    {
        public int Id { get; set; }

        public string Note { get; set; }

        public int TaskId { get; set; }


    }
}
