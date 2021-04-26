using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using EmailService.Services;
using EmailService.Models;
using EmailService.Settings;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmailAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService mailService;

        private readonly MailSettings mailSettings;
        public MailController(IMailService mailService, IOptions<MailSettings> _mailSettings)
        {
            this.mailService = mailService;                       // contains function to send E-Mail
            mailSettings = _mailSettings.Value;                   // need the mail settings to get Sender E-Mail
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm] MailRequest request)
        {
            try
            {
                await mailService.SendEmailAsync(request.Subject, // Re-usable function from MailService DLL   
                    request.Body,
                    request.Recipient,
                    request.Attachments);          
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
