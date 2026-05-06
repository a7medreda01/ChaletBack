using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using AppBL.IService;
using Microsoft.Extensions.Configuration;

namespace AppBL.Service
{
    public class EmailService : IEmailService
    {
            private readonly IConfiguration _config;

            public EmailService(IConfiguration config)
            {
                _config = config;
            }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtpServer = _config["EmailSettings:SmtpServer"];
            var port = int.Parse(_config["EmailSettings:Port"]);
            var senderEmail = _config["EmailSettings:SenderEmail"];
            var password = _config["EmailSettings:Password"];

            var client = new SmtpClient(smtpServer, port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,   // 👈 مهم جدًا
                Credentials = new NetworkCredential(senderEmail, password)
            };

            var mail = new MailMessage
            {
                From = new MailAddress(senderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mail.To.Add(to);

            await client.SendMailAsync(mail);
        }
    }
    }
