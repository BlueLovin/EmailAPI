using EmailService.Models;
using EmailService.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmailService.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings mailConfig;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            mailConfig = mailSettings.Value;
        }

        public async Task SendEmailAsync(string Subject, string Body, string Recipient, List<IFormFile> Attachments = null, int maxRetries = 3)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(mailConfig.Mail);      // set the sender
            email.To.Add(MailboxAddress.Parse(Recipient));             // set "to" or recipient
            email.Subject = Subject;                                   // set email subject
            var builder = new BodyBuilder();

            if (Attachments != null)                                   // If there is an attachment...
            {
                byte[] bytes;                                          // allocate memory
                foreach (var file in Attachments)
                {
                    if (file.Length > 0)                               // if valid attachment
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            bytes = ms.ToArray();                      // convert file to byte array
                        }
                        // add attachments
                        builder.Attachments.Add(file.FileName, bytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = Body;
            email.Body = builder.ToMessageBody(); 
            

            bool success = false; 
            int attempts = 0;
            while (!success && attempts <= maxRetries) // retry loop!
            {
                if (attempts > 0)
                {
                    Thread.Sleep(5000); // sleep 5 seconds if not first attempt
                }
                try
                {
                    using var smtpClient = new SmtpClient();
                    smtpClient.Connect(mailConfig.Host, mailConfig.Port, SecureSocketOptions.StartTls);
                    smtpClient.Authenticate(mailConfig.Mail, mailConfig.Password);
                    await smtpClient.SendAsync(email);

                    smtpClient.Disconnect(true);
                    success = true;
                }
                catch
                {
                    if (attempts >= maxRetries)
                    {
                        throw;
                    }
                }
                attempts++;
                Console.WriteLine(DateTime.Now.ToString() + " - attempting SendEmailAsync... attempt #" + attempts); // log iteration
            }
        }
    }
}
