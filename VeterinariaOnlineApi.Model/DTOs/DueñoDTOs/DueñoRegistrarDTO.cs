﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinariaOnlineApi.Core.DTOs.DueñoDTOs
{
    public class DueñoRegistrarDTO
    {
        [EmailAddress] 
        public string Email {  get; set; }
        
        [PasswordPropertyText]
        public string Password { get; set; }

        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
