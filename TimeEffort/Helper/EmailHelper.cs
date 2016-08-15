using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace TimeEffort.Helper
{
    public class EmailHelper
    {
        public void SendEmail(string text, string to, string cc)
        {
            var Smtp = new SmtpClient("192.168.1.77", 25);
            //create new account and enter crenditails here
            //Smtp.Credentials = new NetworkCredential("Administrator", "P@ssw0rd", "LGCNS");
            //Smtp.EnableSsl = true;
            //Smtp.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;

            //Smtp.UseDefaultCredentials=true;
            //Message formating
            MailMessage Message = new MailMessage();
            Message.From = new MailAddress("info@tapps.uz");


            MailAddress recipient = new MailAddress(to);
            MailAddress copy = new MailAddress(cc);
            Message.To.Add(recipient);
            Message.CC.Add(copy);

            string subject = "TAPPS Notification";
            Message.Subject = subject;
            Message.Body = text;

            //sending
            Smtp.Send(Message);
        }
    }
}