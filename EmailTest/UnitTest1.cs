using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmailService.Services;
using EmailService.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace EmailTest
{
    [TestClass]
    public class UnitTest1
    {
        static IOptions<MailSettings> mailSettings = Options.Create<MailSettings>(new MailSettings());
        MailService mailService = new MailService(mailSettings);

        [TestInitialize]
        public void TestInitialize()
        {
            mailSettings.Value.Host = "smtp.ethereal.email";
            mailSettings.Value.Port = 587;
            mailSettings.Value.Mail = "tyler84@ethereal.email";
            mailSettings.Value.DisplayName = "Tyler Rutherford";
            mailSettings.Value.Password = "vnTK8QjYzzjwHUNmCF";
        }

        [TestMethod]
        public void TestSend()
        {
            bool passed = false;
            try
            {
                var result = mailService.SendEmailAsync("Adam has a headache",
                    "Please call a doctor",
                    "johnnyboy@gomat.com");
                result.GetAwaiter().GetResult();
                passed = true;
            }
            catch { }
            Assert.IsTrue(passed);

        }
        [TestMethod]
        public void Phony()
        {
            mailSettings.Value.Host = "smtp.fake.net";
            MailService mailService1 = new MailService(mailSettings);
            bool failed = true;
            try
            {
                var result = mailService.SendEmailAsync("Adam has a headache",
                    "Please call a doctor",
                    "johnnyboy@gomat.com");
                result.GetAwaiter().GetResult();
                Console.WriteLine(result.IsCompletedSuccessfully);
                failed = !result.IsCompletedSuccessfully;
            }
            catch { }
            Assert.IsTrue(failed);
        }
    }
}
