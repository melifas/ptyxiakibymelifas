using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Providers.Entities;
using Microsoft.AspNetCore.Identity;

namespace Covid19.WebApp.ViewModels.Account
{
    public class ForgotPasswordInputViewModel
    {
        [Required(ErrorMessage = "Required field")]
        [EmailAddress]
        public string Email { get; set; }
    }

}
