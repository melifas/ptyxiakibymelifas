using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Providers.Entities;
using Microsoft.AspNetCore.Identity;

namespace Covid19.WebApp.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        public ForgotPasswordInputViewModel Input { get; set; }

        public UserManager<User> UserManager { get; set; }
    }
}
