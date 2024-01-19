﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Email
{
    public class ContactUsRequest : BaseEmailRequest
    {

        [Required]
        public string From { get; set; }



    }
}
