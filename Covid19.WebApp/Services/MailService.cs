using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Covid19.WebApp.Models;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Covid19.WebApp.Services
{
    public class MailService : IMailService
    {

        private readonly string _userName;
        private readonly string _fromEmail;
        private readonly string _host;
        private readonly string _port;
        private readonly string _password;

        public MailService(IConfiguration config)
        {
            _host = config["MailSettings:Host"];
            _userName = config["MailSettings:Username"];
            _fromEmail = config["MailSettings:Mail"];
            _port = config["MailSettings:Port"];
            _password = config["MailSettings:Password"];

        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            
            var smtpClient = new SmtpClient(_host)
            {
                Port = int.Parse(_port),
                Credentials = new NetworkCredential(_userName, _password),
                EnableSsl = false,
            };



            MailMessage mailMessage = new MailMessage()
            {
                From = new MailAddress(_fromEmail),
                BodyEncoding = Encoding.UTF8,
                Subject = subject,
                To = {new MailAddress(email)},
                Body = message,
                IsBodyHtml = true
            };

            await smtpClient.SendMailAsync(mailMessage);

        }
    }
}
