﻿using Sabio.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Tasks
{
    public class Task
    {
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string ImageUrl { get; set; }
        public LookUp Status { get; set; }
        public int SortOrder { get; set; }
        public BaseUser CreatedBy { get; set; }
        public BaseUser ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
