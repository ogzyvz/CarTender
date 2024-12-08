using System;
using Infoline.Helper;

namespace Infoline.Web.Utility
{

    public static class EMail
    {
        public static void Send(string email, string baslik, string mesaj, bool isBodyHtml = false, bool asenkron = true)
        {
            if (!asenkron)
                Mail().TaskSend(email, baslik, mesaj, null, isBodyHtml);
            else
                Mail().Send(email, baslik, mesaj, null, isBodyHtml);
        }

        private static E_MAIL Mail()
        {

            string host = "", username = "", password = "";
            bool ssl = true;
            int port = 0;

            if (System.Configuration.ConfigurationManager.AppSettings["mailHost"] != null)
            {
                host = System.Configuration.ConfigurationManager.AppSettings["mailHost"].ToString();
            }

            if (System.Configuration.ConfigurationManager.AppSettings["mailPort"] != null)
            {
                port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["mailPort"]);
            }

            if (System.Configuration.ConfigurationManager.AppSettings["mailUser"] != null)
            {
                username = System.Configuration.ConfigurationManager.AppSettings["mailUser"].ToString();
            }

            if (System.Configuration.ConfigurationManager.AppSettings["mailPass"] != null)
            {
                password = System.Configuration.ConfigurationManager.AppSettings["mailPass"].ToString();
            }

            if (System.Configuration.ConfigurationManager.AppSettings["mailSsl"] != null)
            {
                ssl = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["mailSsl"]);
            }


            var result = new E_MAIL(host, port, username, password, ssl, true);

            return result;
        }

        public static string MailTextTemp(string userFullName, string icerik)
        {
            var siteName = System.Configuration.ConfigurationManager.AppSettings["SiteUrl"] != null ? System.Configuration.ConfigurationManager.AppSettings["SiteUrl"].ToString() : "http://localhost:38663/";


            var mesaj = string.Format(@"<table cellpadding='0' cellspacing='0' border='0' id='backgroundTable' style='height:auto !important; margin:0; padding:0; width:100% !important; background-color:#FFFFFF; color:#222222;'>
    <tr>
        <td>

            <table id='contenttable' width='600' align='center' cellpadding='0' cellspacing='0' border='0' style='background-color:#FFFFFF; text-align:center !important; margin-top:0 !important; margin-right: auto !important; margin-bottom:0 !important; margin-left: auto !important; border:none; width: 100% !important; max-width:600px !important;'>
                <tr>
                    <td width='100%'>
                        <table bgcolor='#FFFFFF' border='0' cellspacing='0' cellpadding='0' width='100%'>
                            <tr>
                                <td width='100%' bgcolor='#ffffff' style='text-align:center;'>
                                    <a href='#'><img src='http://tarbil.com/MSVI/assets/themes/img/services/data_kontrol.jpg' alt='http://tarbil.com/MSVI/assets/themes/img/services/data_kontrol.jpg' style='display:inline-block; max-width:100% !important; width:100% !important; height:auto !important;border-bottom-right-radius:8px;border-bottom-left-radius:8px;' border='0'></a>
                                </td>
                            </tr>
                        </table>
                        <table bgcolor='#FFFFFF' border='0' cellspacing='0' cellpadding='25' width='100%'>
                            <tr>
                                <td width='100%' bgcolor='#ffffff' style='text-align:left;'>
                                    <p style='color:#222222; font-family:Arial, Helvetica, sans-serif; font-size:15px; line-height:19px; margin-top:0; margin-bottom:20px; padding:0; font-weight:normal;'>
                                        Merhaba {0},
                                    </p>
                                   
                                    <table border='0' cellspacing='0' cellpadding='0' width='100%' class='emailwrapto100pc'>
                                        <tr>
                                            <td class='emailcolsplit' align='left' valign='top' width='58%'>
                                                <p style='color:#222222; font-family:Arial, Helvetica, sans-serif; font-size:15px; line-height:19px; margin-top:0; margin-bottom:20px; padding:0; font-weight:normal;'>
                                                  {1}
                                                </p>
                                            </td>
                                         
                                        </tr>

                                    </table>

                                </td>
                            </tr>
                        </table>
                        <table bgcolor='#FFFFFF' border='0' cellspacing='0' cellpadding='0' width='100%'
                               >
                            <tr>
                                <td width='100%' bgcolor='#ffffff' style='text-align:center;'><a style='font-weight:bold; text-decoration:none;' href='#'></a></td>
                            </tr>
                        </table>
                        <table bgcolor='#FFFFFF' border='0' cellspacing='0' cellpadding='25' width='100%'>
                            <tr>
                                <td width='100%' bgcolor='#ffffff' style='text-align:left;'>
                                    
                                    
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

        </td>
    </tr>
</table>", userFullName == null ? string.Empty : userFullName.ToUpper(), icerik, siteName);
            return mesaj;
        }

    }

}