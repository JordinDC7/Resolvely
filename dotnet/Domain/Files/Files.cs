using Sabio.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Files
{
    public class Files : BaseFile
    {
        public string Name { get; set; }
        public LookUp FileType { get; set; }
        public BaseUser CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
