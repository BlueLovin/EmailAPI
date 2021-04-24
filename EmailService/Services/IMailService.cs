using EmailService.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailService.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(string Subject, string Body, string Recipient, List<IFormFile> Attachments = null, int maxRetries = 3);
    }
}
