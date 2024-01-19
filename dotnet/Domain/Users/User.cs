using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Users
{
    public class User : BaseUser
    {
        public string Email { get; set; }
        public bool IsConfirmed { get; set; }
        public LookUp Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public LookUp Role { get; set; }
    }
}
