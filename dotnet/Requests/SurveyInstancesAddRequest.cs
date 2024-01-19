﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests
{
    public class SurveyInstancesAddRequest
    {

        [Required]
        public int SurveyId { get; set; }

        [Required]
        public int UserId { get; set; }

    }
}