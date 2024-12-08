using Infoline.Framework.Database;
using CarTender.BusinessData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace CarTender.BusinessAccess
{
    public class Sender
    {
        private SmtpClient _smtpClient = new SmtpClient();
        public MailMessage _mailMessage = new MailMessage();
        private string _username { get; set; }
        private string _password { get; set; }
        private bool _isBodyHtml { get; set; }
        private string _content { get; set; }
        private AlternateView _alternativeView { get; set; }
        private static object obj = new object();


        public Sender(string userName, string password, string host, int port, bool ssl)
        {
            _defaults(userName, password, host, port, ssl);
        }

        public Sender(string userName, string password, string host, int port, bool ssl, string mesaj, bool isbodyHtml)
        {
            _defaults(userName, password, host, port, ssl);
            _content = mesaj;
            _isBodyHtml = isbodyHtml;
        }

        public Sender(string userName, string password, string host, int port, bool ssl, AlternateView view)
        {
            _defaults(userName, password, host, port, ssl);
            _alternativeView = view;
            _isBodyHtml = true;
        }

        public void Send(string email, string baslik, bool asenkron = true, string[] cc = null, string[] files = null, string[] bcc = null)
        {

            if (string.IsNullOrEmpty(email))
            {
                return;
            }


            var configEmail = "";
#if DEBUG
            configEmail = System.Configuration.ConfigurationManager.AppSettings["EmailControl"];
#endif

            var ts = Task.Run(() => _send(string.IsNullOrEmpty(configEmail) ? email : configEmail, baslik, cc, files, bcc));
            if (!asenkron)
            {
                ts.Wait();
            }
        }

        private void _send(string email, string title, string[] cc = null, string[] files = null, string[] bcc = null)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            var result = new ResultStatus();
            _mailMessage.Body = _content;
            _mailMessage.IsBodyHtml = _isBodyHtml;
            _mailMessage.From = new System.Net.Mail.MailAddress(_username, _username);
            _mailMessage.Subject = title;

            if (_mailMessage.IsBodyHtml && _alternativeView != null)
            {
                _mailMessage.AlternateViews.Add(_alternativeView);
            }



            foreach (var item in email.Split(';').Where(a => _isValidEmail(a.Trim())).Select(a => a.Trim()))
            {
                _mailMessage.To.Add(item);
            }

            foreach (var item in cc ?? new string[] { })
            {
                _mailMessage.CC.Add(item);
            }

            foreach (var item in bcc ?? new string[] { })
            {
                _mailMessage.Bcc.Add(item);
            }


            foreach (var file in files ?? new string[] { })
            {
                var fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                var mimeType = MimeMapping.GetMimeMapping(file);
                var ct = new ContentType(mimeType);
                var data = new Attachment(fs, ct);
                data.Name = Path.GetFileName(file);
                _mailMessage.Attachments.Add(data);
            }


            try
            {
                lock (obj)
                {
                    _smtpClient.Send(_mailMessage);
                }
                result.result = true;
                result.message = "Gönderim işlemi başarılı.";
            }
            catch (Exception ex)
            {
                result.result = false;
                result.message = ex.Message;
            }



            var db = new CarTenderDatabase();
            db.InsertSYS_Email(new SYS_Email
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                SendingIsBodyHtml = _isBodyHtml,
                SendingMail = email,
                SendingTitle = title,
                SendingMessage = _content,
                Result = result.result.ToString(),
                SendingTime = DateTime.Now,
            });

        }

        private bool _isValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void _defaults(string userName, string password, string host, int port, bool ssl)
        {
            _username = userName;
            _password = password;
            _smtpClient.Host = host;
            _smtpClient.Port = port;
            _smtpClient.EnableSsl = ssl;
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            _smtpClient.Credentials = new System.Net.NetworkCredential(_username, _password);
        }

        public void SendCalendar(string email, string baslik, string mesaj, DateTime startDate, DateTime endDate, string file = "", bool isBodyHtml = true)
        {
            var configEmail = System.Configuration.ConfigurationManager.AppSettings["EmailControl"];
            email = configEmail == "" || configEmail == null ? email : configEmail;
            _mailMessage.To.Clear();
            _mailMessage.Body = mesaj;
            if (email.Split(';').Count() > 0)
            {
                foreach (var mailAdr in email.Split(';'))
                {
                    _mailMessage.To.Add(mailAdr);
                }
            }
            else
            {
                _mailMessage.To.Add(email);
            }

            _mailMessage.IsBodyHtml = isBodyHtml;
            _mailMessage.From = new System.Net.Mail.MailAddress(_username, _username);
            _mailMessage.Subject = baslik;

            if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file))
            {
                System.Net.Mail.Attachment item = new System.Net.Mail.Attachment(file);
                _mailMessage.Attachments.Add(item);
            }

            StringBuilder str = new StringBuilder();
            str.AppendLine("BEGIN:VCALENDAR");

            //PRODID: identifier for the product that created the Calendar object
            str.AppendLine("PRODID:-//ABC Company//Outlook MIMEDIR//EN");
            str.AppendLine("VERSION:2.0");
            str.AppendLine("METHOD:REQUEST");

            str.AppendLine("BEGIN:VEVENT");

            str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", TimeZoneInfo.ConvertTimeToUtc(startDate).ToString("yyyyMMddTHHmmssZ")));//TimeZoneInfo.ConvertTimeToUtc("BeginTime").ToString("yyyyMMddTHHmmssZ")));
            str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", TimeZoneInfo.ConvertTimeToUtc(DateTime.UtcNow).ToString("yyyyMMddTHHmmssZ")));
            str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", TimeZoneInfo.ConvertTimeToUtc(endDate).ToString("yyyyMMddTHHmmssZ")));//TimeZoneInfo.ConvertTimeToUtc("EndTime").ToString("yyyyMMddTHHmmssZ")));

            //str.AppendLine(string.Format("LOCATION: {0}", "Location"));

            // UID should be unique.
            str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));
            str.AppendLine(string.Format("DESCRIPTION:{0}", _mailMessage.Body));
            str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", _mailMessage.Body));
            str.AppendLine(string.Format("SUMMARY:{0}", _mailMessage.Subject));

            str.AppendLine("STATUS:CONFIRMED");
            str.AppendLine("BEGIN:VALARM");
            str.AppendLine("TRIGGER:-PT15M");
            str.AppendLine("ACTION:Accept");
            str.AppendLine("DESCRIPTION:Reminder");
            str.AppendLine("X-MICROSOFT-CDO-BUSYSTATUS:BUSY");
            str.AppendLine("END:VALARM");
            str.AppendLine("END:VEVENT");

            str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", _mailMessage.From.Address));
            str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", _mailMessage.To[0].DisplayName, _mailMessage.To[0].Address));

            str.AppendLine("END:VCALENDAR");
            System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType("text/calendar");
            if (ct.Parameters != null)
            {
                ct.Parameters.Add("method", "REQUEST");
                ct.Parameters.Add("name", "meeting.ics");
            }
            var avCal = AlternateView.CreateAlternateViewFromString(str.ToString(), ct);
            _mailMessage.AlternateViews.Add(avCal);

            try
            {
                _smtpClient.Send(_mailMessage);
            }
            catch (Exception ex)
            {
                // hatamesaji = ex.Message;
            }
        }
    }

    public class Email
    {
        private string _username { get; set; } = "noreply@workoftime.com";
        private string _password { get; set; } = "zaq1-2Wsx!9";
        private string _host { get; set; } = "smtp.office365.com";
        private int _port { get; set; } = 587;
        private bool _enableSSL { get; set; } = true;

        public Email()
        {


        }

        public Email(string userName, string password, string host, int port, bool ssl)
        {
            _username = userName;
            _password = password;
            _host = host;
            _port = port;
            _enableSSL = ssl;
        }

        public Sender Template(string templateName, string image, string title, string content)
        {
            var appDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            var templateFileDirectory = string.Format(@"{0}\App_Data\MailTemplate\{1}\index.html", appDirectory, templateName);
            templateFileDirectory = templateFileDirectory.Remove(0, 6);
            string html = System.IO.File.ReadAllText(templateFileDirectory);

            var siteUrl = System.Configuration.ConfigurationManager.AppSettings["SiteUrl"];
            var resourceCollection = new List<LinkedResource>();

            html = html.Replace("{{title}}", title);
            html = html.Replace("{{content}}", content);
            html = html.Replace("{{siteUrl}}", siteUrl);

            if (!string.IsNullOrEmpty(image) && html.IndexOf("{{imagePath}}") > 0)
            {
                var baseImagePath = string.Format(@"{0}\App_Data\ContentImages\{1}", appDirectory, image);
                baseImagePath = baseImagePath.Remove(0, 6);
                html = html.Replace("{{imagePath}}", "cid:0");
                resourceCollection.Add(new LinkedResource(FileToImageStream(baseImagePath), new ContentType(MimeMapping.GetMimeMapping(baseImagePath)))
                {
                    ContentId = "0"
                });
            }

            var imgCount = 1;
            foreach (Match m in Regex.Matches(html, "<img(?<value>.*?)>"))
            {
                var imgContent = m.Groups["value"].Value.ToLower();
                try
                {
                    var imagePath = string.Join("", Regex.Match(imgContent, "((src)=['\"])(.*?)['\"]").Value.Replace("src=", "").Split(new char[] { '\'', '"' }));
                    var imageDirectoryPath = HttpUtility.UrlDecode(string.Join(@"\", imagePath.Split('/')));
                    var serveImagePath = string.Format(@"{0}\App_Data\MailTemplate\{1}\{2}", appDirectory, templateName, imageDirectoryPath);
                    serveImagePath = serveImagePath.Remove(0, 6);
                    if (!File.Exists(serveImagePath))
                    {
                        continue;
                    }

                    html = html.Replace(imagePath, "cid:" + imgCount);
                    var tempResource = new LinkedResource(FileToImageStream(serveImagePath), new ContentType(MimeMapping.GetMimeMapping(serveImagePath)))
                    {
                        ContentId = imgCount.ToString()
                    };
                    resourceCollection.Add(tempResource);

                    imgCount++;

                }
                catch (Exception)
                {
                    continue;
                }

            }

            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html);
            foreach (var item in resourceCollection)
            {
                alternateView.LinkedResources.Add(item);
            }

            return new Sender(_username, _password, _host, _port, _enableSSL, alternateView);
        }

        public void Send(string email, string baslik, string mesaj, bool isBodyHtml = false, bool asenkron = false, string[] cc = null, string[] files = null)
        {
            new Sender(_username, _password, _host, _port, _enableSSL, mesaj, isBodyHtml).Send(email, baslik, asenkron, cc);

        }

        public void SendCalendar(string email, string baslik, string mesaj, DateTime startDate, DateTime endDate, string file = "", bool isBodyHtml = true)
        {
            new Sender(_username, _password, _host, _port, _enableSSL).SendCalendar(email, baslik, mesaj, startDate, endDate, file, isBodyHtml);
        }

        private static MemoryStream FileToImageStream(string filePath)
        {
            using (Image image = Image.FromFile(filePath))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                    return ms;
                }
            }
        }



    }

}
