using Hotelbooking_System.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace Hotelbooking_System.Utils
{
    public class EmailSender
    {
        // Please use your API KEY here.
        private const String API_KEY = "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";

        public void Send(SendEmailViewModel model, String fullPath)
        {
            string fileName = Path.GetFileName(model.AttachedFile.FileName);

            var bytes = System.IO.File.ReadAllBytes(fullPath);
            var file = Convert.ToBase64String(bytes);
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("980935713@qq.com", "FIT5032 Example Email User");
            var to_emails = new List<EmailAddress>();
            String s = model.ToEmail;
            String[] email_list = s.Split(' ');
            for (int i = 0; i < email_list.Length; i++)
            {
                to_emails.Add(new EmailAddress(email_list[i], ""));
            }
            var msg = new SendGridMessage();
            msg.SetFrom("980935713@qq.com", "Zhenyuan Tao");
            msg.PlainTextContent = model.Contents;
            msg.Subject = model.Subject;
            msg.AddAttachment(model.AttachedFile.FileName, file);
            msg.AddTos(to_emails);
            var response = client.SendEmailAsync(msg);
        }

    }
}