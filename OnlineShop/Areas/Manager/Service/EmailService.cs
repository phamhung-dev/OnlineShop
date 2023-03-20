using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Configuration;

namespace OnlineShop.Areas.Manager.Service
{
    public interface IEmailService
    {
        void Send(string from, string to, string subject, string html);
    }
    public class EmailService
    {
        /*        public static bool Send(string toEmail, string subject, string body)
                {
                    try
                    {
                        using (var smtpClient = new SmtpClient())
                        {
                            smtpClient.EnableSsl = true;
                            smtpClient.Host = ConfigurationManager.AppSettings["smtpHost"].ToString();
                            smtpClient.Port = int.Parse(ConfigurationManager.AppSettings["smtpPort"].ToString());
                            smtpClient.UseDefaultCredentials = false;
                            smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["smtpUserName"].ToString(), ConfigurationManager.AppSettings["smtpPassword"].ToString());
                            var msg = new MailMessage
                            {
                                IsBodyHtml = true,
                                BodyEncoding = Encoding.UTF8,
                                From = new MailAddress(ConfigurationManager.AppSettings["smtpUserName"].ToString()),
                                Subject = subject,
                                Body = body,
                                Priority = MailPriority.Normal,
                            };
                            msg.To.Add(toEmail);
                            smtpClient.Send(msg);
                            return true;
                        }
                    }
                    catch (SmtpException ex)
                    {
                        Console.WriteLine(ex.ToString());
                        return false;
                    }
                }*/
        public static bool Send(string to, string subject, string html)
        {
            try
            {
                // create message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(ConfigurationManager.AppSettings["smtpUserName"].ToString()));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = html };

                // send email
                using (var smtp = new SmtpClient())
                {
                    smtp.Connect(ConfigurationManager.AppSettings["smtpHost"].ToString(), int.Parse(ConfigurationManager.AppSettings["smtpPort"].ToString()), SecureSocketOptions.StartTls);
                    smtp.Authenticate(ConfigurationManager.AppSettings["smtpUserName"].ToString(), ConfigurationManager.AppSettings["smtpPassword"].ToString());
                    smtp.Send(email);
                    smtp.Disconnect(true);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}