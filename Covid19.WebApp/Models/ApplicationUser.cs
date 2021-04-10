using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Covid19.WebApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string wantVaccine { get; set; }
    }
}
