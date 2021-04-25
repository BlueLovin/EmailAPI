using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmailService.Services;
using EmailService.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

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
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            mailSettings.Value.Host = configuration.GetValue<string>("MailSettings:Host");
            mailSettings.Value.Port = configuration.GetValue<int>("MailSettings:Port");
            mailSettings.Value.Mail = configuration.GetValue<string>("MailSettings:Mail");
            mailSettings.Value.DisplayName = configuration.GetValue<string>("MailSettings:DisplayName");
            mailSettings.Value.Password = configuration.GetValue<string>("MailSettings:Password");

        }

        [TestMethod]
        public async Task TestSend()
        {
            bool passed = false;
            try
            {
                await mailService.SendEmailAsync("Adam has a headache",
                    "Please call a doctor",
                    "johnnyboy@gomat.com");
                passed = true;
            }
            catch { }
            Assert.IsTrue(passed);

        }
        [TestMethod]
        public void TestFail()
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
