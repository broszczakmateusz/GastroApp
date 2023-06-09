﻿using Microsoft.AspNetCore.Identity;

namespace GastroApp.Models
{
    public class User : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
