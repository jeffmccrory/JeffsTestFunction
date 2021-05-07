using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using SendGrid.Helpers.Mail;

namespace JeffsTestFunction
{
    public static class EmailLicenseFile
    {
        [FunctionName("EmailLicenseFile")]
        public static void Run([BlobTrigger("licenses/{orderId}.lic", Connection = "AzureWebJobsStorage")]string licenseFileContents,
            [SendGrid(ApiKey = "SendGridApiKey")] ICollector<SendGridMessage> sender, string orderId, 
            [Table("orders", "orders", "{orderId}")] Order order,
            ILogger log)
        {
            var email = order.Email;

            log.LogInformation($"C# Blob trigger function Processed blob\n Order Id :{orderId} \n Size: {licenseFileContents.Length} Bytes");

            var message = new SendGridMessage
            {
                From = new EmailAddress(Environment.GetEnvironmentVariable("EmailSender")),
                Subject = "Your license file",
                HtmlContent = "Thank you for your order"
            };
            message.AddTo(email);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(licenseFileContents);
            var base64 = Convert.ToBase64String(plainTextBytes);
            message.AddAttachment($"{orderId}.lic", base64, "text/plain");

            if(!email.EndsWith("@test.com"))
                sender.Add(message);

        }
    }
}
