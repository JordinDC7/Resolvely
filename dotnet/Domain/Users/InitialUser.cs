using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Users
{
    public class InitialUser : BaseUser
    {
        public string Email { get; set; }
        public string Role { get; set; }

    }
}
