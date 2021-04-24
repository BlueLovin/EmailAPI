using EmailService.Models;
using EmailService.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(mailConfig.Mail);      // set the sender
            email.To.Add(MailboxAddress.Parse(mailRequest.Recipient)); // set "to" or recipient
            email.Subject = mailRequest.Subject;                       // set email subject
            var builder = new BodyBuilder();

            if (mailRequest.Attachments != null)                       // If there is an attachment...
            {
                byte[] bytes;                                          // allocate memory
                foreach (var file in mailRequest.Attachments)
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
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();  // set email body
            using var smtpClient = new SmtpClient();
            smtpClient.Connect(mailConfig.Host, mailConfig.Port, SecureSocketOptions.StartTls);
            smtpClient.Authenticate(mailConfig.Mail, mailConfig.Password);
            await smtpClient.SendAsync(email);
            smtpClient.Disconnect(true);
        }
    }
}
