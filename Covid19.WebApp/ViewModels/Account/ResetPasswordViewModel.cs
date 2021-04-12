using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19.WebApp.ViewModels.Account
{
    public class ResetPasswordViewModel
    {
        public ResetPasswordInputViewModel Input { get; set; }

        public string Code { get; set; }
    }
}
