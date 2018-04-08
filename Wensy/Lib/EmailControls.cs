using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ServicePoint.Lib
{
    public class EmailControls
    {
        public struct EMailSettings
        {
            public int iPort;
            public string strHost;
            public string strMessage;
            public string strTo;
            public string strCC;
            public string strSender;
            public string strPassword;
            public string strHeader;
            public string strFile;
        }

        public bool bHasEmailException = false;

        public EMailSettings g_EMailSettings = new EMailSettings();

        string[] astrTo = null;
        string[] astrCC = null;

        //EventRecorder WSPEvent = new EventRecorder();

        void ResolveMailAddress()
        {
            astrTo = null;
            astrCC = null;

            astrTo = g_EMailSettings.strTo.Split(';');
            astrCC = g_EMailSettings.strCC.Split(';');

        }

        public string SendGmail()
        {
            string result;

            try
            {
                if (!g_EMailSettings.strSender.Contains("gmail.com"))
                {
                    bHasEmailException = true;
                    //WSPEvent.WriteEvent("Please check your email settings to use GMail." + g_EMailSettings.strSender, "E", 1112);
                    //Console.WriteLine("Please check your email settings to use GMail." + g_EMailSettings.strSender);
                    result = "Please check your email settings to use GMail." + g_EMailSettings.strSender;
                    //return;
                }

                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(g_EMailSettings.strSender, g_EMailSettings.strPassword);

                MailMessage mm = new MailMessage();

                if (g_EMailSettings.strTo.Contains(";"))
                {
                    ResolveMailAddress();
                    foreach (string strTo in astrTo)
                        mm.To.Add(strTo);
                }
                else
                {
                    mm.To.Add(g_EMailSettings.strTo);
                }

                if (!string.IsNullOrEmpty(g_EMailSettings.strCC))
                {
                    if (g_EMailSettings.strCC.Contains(";"))
                    {
                        ResolveMailAddress();
                        foreach (string strCC in astrCC)
                            mm.To.Add(strCC);
                    }
                    else if (g_EMailSettings.strCC.Contains("@"))
                    {
                        mm.CC.Add(g_EMailSettings.strCC);
                    }
                }

                mm.From = new MailAddress(g_EMailSettings.strSender);
                mm.IsBodyHtml = true;
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                mm.Subject = g_EMailSettings.strHeader;
                mm.Body = g_EMailSettings.strMessage;

                if (!string.IsNullOrEmpty(g_EMailSettings.strFile))
                {
                    mm.Attachments.Add(new Attachment(g_EMailSettings.strFile));
                }

                client.Send(mm);

                //Console.WriteLine("메일이 성공적으로 보내졌습니다!!");
                result = "success";
            }
            catch (Exception ex)
            {
                bHasEmailException = true;
                //WSPEvent.WriteEvent("Sending email via GMail has been failed. - " + ex.Message, "E", 1109);
                //Console.WriteLine("Sending email via GMail has been failed. - " + ex.Message);
                result = "Sending email via GMail has been failed. - " + ex.Message;
            }

            return result;
        }

        public void SendHOTMail()
        {
            try
            {
                if (!g_EMailSettings.strSender.ToLower().Contains("hotmail.com") && !g_EMailSettings.strSender.ToLower().Contains("live") && !g_EMailSettings.strSender.ToLower().Contains("msn"))
                {
                    bHasEmailException = true;
                    //WSPEvent.WriteEvent("Please check your email settings to use Windows Live. - " + g_EMailSettings.strSender, "E", 1112);
                    Console.WriteLine("Please check your email settings to use Windows Live. - " + g_EMailSettings.strSender);
                    return;
                }

                SmtpClient SmtpServer = new SmtpClient("smtp.live.com");
                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential(g_EMailSettings.strSender, g_EMailSettings.strPassword);
                SmtpServer.EnableSsl = true;

                var mail = new MailMessage();

                ///////////////////////////////////////////////////////////////
                if (g_EMailSettings.strTo.Contains(";"))
                {
                    ResolveMailAddress();
                    foreach (string strTo in astrTo)
                        mail.To.Add(strTo);
                }
                else
                {
                    mail.To.Add(g_EMailSettings.strTo);
                }

                if (!string.IsNullOrEmpty(g_EMailSettings.strCC))
                {
                    if (g_EMailSettings.strCC.Contains(";"))
                    {
                        ResolveMailAddress();
                        foreach (string strCC in astrCC)
                            mail.To.Add(strCC);
                    }
                    else if (g_EMailSettings.strCC.Contains("@"))
                    {
                        mail.CC.Add(g_EMailSettings.strCC);
                    }
                }

                ///////////////////////////////////////////////////////////////

                mail.From = new MailAddress(g_EMailSettings.strSender);

                mail.Subject = g_EMailSettings.strHeader;
                mail.Body = g_EMailSettings.strMessage;
                mail.BodyEncoding = UTF8Encoding.UTF8;
                mail.IsBodyHtml = true;

                SmtpServer.Send(mail);

                Console.WriteLine("메일이 성공적으로 보내졌습니다!! ㅎㅎ");
            }
            catch (Exception ex)
            {
                bHasEmailException = true;
                //WSPEvent.WriteEvent("Sending email via Windows Live, has been failed. - " + ex.Message, "E", 1110);
                Console.WriteLine("Sending email via Windows Live, has been failed. - " + ex.Message);
            }

        }

        public void SendEmailviaSMTP()
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("jchoi02");

                SmtpServer.Port = 25;
                SmtpServer.EnableSsl = false;

                if (g_EMailSettings.strTo.Contains(";"))
                {
                    ResolveMailAddress();
                    foreach (string strTo in astrTo)
                        mail.To.Add(strTo);
                }
                else
                {
                    mail.To.Add(g_EMailSettings.strTo);
                }

                if (!string.IsNullOrEmpty(g_EMailSettings.strCC))
                {
                    if (g_EMailSettings.strCC.Contains(";"))
                    {
                        ResolveMailAddress();
                        foreach (string strCC in astrCC)
                            mail.To.Add(strCC);
                    }
                    else if (g_EMailSettings.strCC.Contains("@"))
                    {
                        mail.CC.Add(g_EMailSettings.strCC);
                    }
                }

                mail.From = new MailAddress(g_EMailSettings.strSender);

                //Attachment attachment = new Attachment(filename);
                //mail.Attachments.Add(attachment);
                mail.Subject = g_EMailSettings.strHeader + " Service Alert Information";
                mail.Body = g_EMailSettings.strMessage;
                mail.BodyEncoding = UTF8Encoding.UTF8;
                mail.IsBodyHtml = true;

                SmtpServer.Send(mail);

                Console.WriteLine("메일이 성공적으로 보내졌습니다!! ㅎㅎ");
            }
            catch (Exception ex)
            {
                bHasEmailException = true;
                //WSPEvent.WriteEvent("Sending email via your SMTP, has been failed. - " + ex.Message, "E", 1111);
                Console.WriteLine("Sending email via your SMTP, has been failed. - " + ex.Message);
            }
        }


    }
}
