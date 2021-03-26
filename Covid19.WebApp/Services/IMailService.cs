using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covid19.WebApp.Models;

namespace Covid19.WebApp.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
