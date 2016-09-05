using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceProject1.Jobs
{
    public class EmailHelper
    {
        public void SendEmail(string text, string to, string cc)
        {
            MailMessage Message = new MailMessage();
            Message.From = new MailAddress("info@tapps.uz");

            MailAddress recipient = new MailAddress(to);
            MailAddress copy = new MailAddress(cc.ToString());

            Message.To.Add(recipient);
            Message.CC.Add(copy);

            string subject = "TAPPS Notification";
            Message.Subject = subject;
            Message.Body = text;


            Message.IsBodyHtml = true;
            //sending
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "localhost";
            smtpClient.Send(Message);
        }
        public void SendEmail(string text, string to)
        {
            MailMessage Message = new MailMessage();
            Message.From = new MailAddress("info@tapps.uz");

            MailAddress recipient = new MailAddress(to);
            Message.To.Add(recipient);

            string subject = "TAPPS Notification";
            Message.Subject = subject;
            Message.Body = text;
            Message.IsBodyHtml = true;

            //sending
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "localhost";
            smtpClient.Send(Message);
        }

        public void SendDebugMail(string to)
        {
            MailMessage Message = new MailMessage();
            Message.From = new MailAddress("info@tapps.uz");

            MailAddress recipient = new MailAddress(to);

            Message.To.Add(recipient);

            string subject = "TAPPS Notification";
            Message.Subject = subject;
            Message.Body = "Email is working";
            Message.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "localhost";
            smtpClient.Send(Message);

            //SmtpClient client = new SmtpClient();
            //client.Port = 25;
            //client.Host = "LGCNS";
            //client.EnableSsl = true;
            //client.Timeout = 10000;
            //client.EnableSsl = false;

            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            ////client.UseDefaultCredentials = false;
            ////client.Credentials = new System.Net.NetworkCredential("user@gmail.com", "password");

            //MailMessage mm = new MailMessage("info@tapps.uz", to, "TAPPS Notification", "Email is working");
            //mm.BodyEncoding = UTF8Encoding.UTF8;
            //mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            //client.Send(mm);
        }
    }
}
