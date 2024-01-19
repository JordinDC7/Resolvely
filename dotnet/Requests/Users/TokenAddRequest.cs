﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Users
{
    public class TokenAddRequest
    {
        [Required]
        public string TokenId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TokenType { get; set; }
    }
}
