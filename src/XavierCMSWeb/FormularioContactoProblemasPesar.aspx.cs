// GNU Version 3 License Copyright (c) 2020 Javier Cañon | https://javiercanon.com
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
using System;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using XavierCMSWeb.Safety.Recaptcha;

namespace XavierCMSWeb
{


    public partial class FormularioContactoProblemasPesar : System.Web.UI.Page
    {

        protected string GOOGLE_SITE_KEY = Global.Configuration.Security.Google.Recaptcha.v3.GetGoogleRecaptchaWebsiteKey();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BootstrapButtonSend_Click(object sender, EventArgs e)
        {


            Page.Validate();

            if (!Page.IsValid) return;


            if (string.IsNullOrEmpty(recaptcha.Value))
            {
                Msg.Visible = true;
                Msg.InnerHtml = "Error en los datos de seguridad, vuelva a recargar la página.";
                return;

            }


            var Recaptchav3 = new RecaptchaVerificationHelper();

            // If your site is behind CloudFlare, be sure you're suing the CF-Connecting-IP header value instead:
            // https://support.cloudflare.com/hc/en-us/articles/200170986-How-does-Cloudflare-handle-HTTP-Request-headers

            RecaptchaVerificationResult recaptchaResult = Recaptchav3.VerifyRecaptchav3Response(
                  Global.Configuration.Security.Google.Recaptcha.v3.GetGoogleRecaptchaSecretKey()
                , Global.Configuration.Security.Google.Recaptcha.v3.GetGoogleRecaptchaWebsiteKey()
                , Request.UserHostAddress
                , recaptcha.Value
                  );

            if (recaptchaResult == RecaptchaVerificationResult.Success)
            {
                //divMessage.InnerHtml = "Score: " + Recaptchav3.Score;
                decimal? minScore = new decimal(0.6);
                if (Recaptchav3.Score < minScore) Response.Redirect("~/Captcha.aspx", true);


                //
                // format msg...
                //
                // IMPORANT:  Your smtp login email MUST be same as your FROM address. 
                string[] to = { Global.Configuration.Mail.GetEmailContacto() };
                string from = Global.Configuration.Mail.GetMailServerLogin();
                //string[] CC;
                //string[] BCC;
                string domainName = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                string emailSubject = "Formulario de Contacto Sitio Web " + domainName;
                bool isBodyHtml = false;
                string emailMessage = @"
----------------------------------------
- FORMULARIO PROBLEMAS AL PESAR
----------------------------------------
Nombre: " + Names.Text + Environment.NewLine + @"
Apellido: "+ LastName.Text + Environment.NewLine + @"
Movil: "+ Mobile.Text + Environment.NewLine + @"
Email: "+ Email.Text + Environment.NewLine + @"
Cargo: "+ Position.Text + Environment.NewLine + @"
Empresa: "+ Business.Text + Environment.NewLine + @"
Ciudad: "+ City.Text + Environment.NewLine + @"
Telefono: "+ Telephone.Text + Environment.NewLine + @"
Inconveniente: " + Incident.SelectedItem.Value + Environment.NewLine + @"
Balanza: " + Balanza.Text + Environment.NewLine + @"
Capacidad: " + Capacidad.Text + Environment.NewLine + @"
Mensaje: " + Environment.NewLine + Notes.Text + Environment.NewLine + @"
--------------------------------------
";


                //var t = Task.Run( () => Global.Emails.SendEmailAsync(to, from, null, null, emailSubject, emailMessage, isBodyHtml) );
                //t.Wait();

                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage()
                {
                    From = new MailAddress(from, from, System.Text.Encoding.UTF8),
                    Subject = emailSubject,
                    SubjectEncoding = System.Text.Encoding.UTF8,
                    Body = emailMessage,
                    BodyEncoding = System.Text.Encoding.UTF8,
                    IsBodyHtml = isBodyHtml,
                    Priority = MailPriority.Normal

                };

                mail.To.Add(to[0]);
                string msg;

                SmtpClient client = new SmtpClient
                {
                    Credentials = new System.Net.NetworkCredential(Global.Configuration.Mail.GetMailServerLogin(), Global.Configuration.Mail.GetMailServerPassword()),
                    Port = Global.Configuration.Mail.GetMailServerPort(),
                    Host = Global.Configuration.Mail.GetMailServer(),
                    EnableSsl = Global.Configuration.Mail.GetMailServerIsEnableSSL()
                };
                try
                {
                    client.Send(mail);
                    msg = "Gracias, mensaje enviado...";
                    ClientScript.RegisterStartupScript(this.GetType(), "UserMsg" ,"alert('"+ msg + "');", true);
                    Msg.Visible = true;
                    Msg.InnerHtml = msg;

                }
                catch (Exception ex)
                {
                    Global.LogError(this.Context, Global.EnumLogCategories.EMAIL, ex.Message);
                    msg = "Lo sentimos, su mensaje no pudo ser enviado, intente mas tarde...";
                    ClientScript.RegisterStartupScript(this.GetType(), "UserMsg", "alert('" + msg + "');", true);
                    Msg.Visible = true;
                    Msg.InnerHtml = msg;
                }



            }
            else
            {
                Msg.Visible = true;
                Msg.InnerHtml = "Existe un problema para validar la seguridad, intente mas tarde o por favor contacte a soporte técnico.";
                return;
            }

        }
    }
}