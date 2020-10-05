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
using Dalayer.DAL;
using System.Data;
using System.Data.SqlClient;
using Rolex.Authentication.Passwords;

namespace XavierCMSWeb
{


    public partial class FormularioRegistro : System.Web.UI.Page
    {

        protected string GOOGLE_SITE_KEY = Global.Configuration.Security.Google.Recaptcha.v3.GetGoogleRecaptchaWebsiteKey();

        //Below is used to generate a password policy that you may use to check that passwords adhere to this policy
        const int numberUpper = 1;
        const int numberNonAlphaNumeric = 1;
        const int numberNumeric = 1;
        const int minPwdLength = 8;
        const int maxPwdLength = Int32.MaxValue;
        const int iterations = 10002;  //Number of hash iterations
        const int saltSize = PWDTK.CDefaultSaltLength + 2; //Salt length


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


                if (CreateUser())
                {

                    Msg.Visible = true;
                    Msg.InnerHtml = "<b>Gracias, registro completado correctamente.</b> Ahora puede navegar y descargar los archivos de los recursos. <a href=\"recursos/\" class=\"btn btn-lg btn-primary\">Acceder a Recursos</a>";

                    HttpCookie userid = new HttpCookie("User.Email", Email.Value.ToString())
                    {
                        Expires = DateTime.Now.AddYears(1)
                    };
                    Response.Cookies.Add(userid);


                }
                else
                {
                    Msg.Visible = true;
                    Msg.InnerHtml = "No pudo crearse el usuario. Por favor intenta mas tarde o contacta a servicio al cliente.";
                    return;
                }

                //
                // format msg...
                //
                // IMPORANT:  Your smtp login email MUST be same as your FROM address. 
                string[] to = { Global.Configuration.Mail.GetEmailContacto()};
                string from = Global.Configuration.Mail.GetMailServerLogin();
                //string[] CC;
                //string[] BCC;
                string domainName = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                string emailSubject = "Formulario de Contacto Sitio Web " + domainName;
                bool isBodyHtml = false;
                string emailMessage = @"
----------------------------------------
- FORMULARIO NUEVO REGISTRO SITIO WEB
----------------------------------------
Nombre: " + Names.Text + Environment.NewLine + @"
Apellido: "+ LastName.Text + Environment.NewLine + @"
Movil: "+ Mobile.Text + Environment.NewLine + @"
Email: "+ Email.Text + Environment.NewLine + @"
Cargo: "+ Position.Text + Environment.NewLine + @"
Empresa: "+ Business.Text + Environment.NewLine + @"
Ciudad: "+ City.Text + Environment.NewLine + @"
Telefono: "+ Telephone.Text + Environment.NewLine + @"
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
                    //msg = "Gracias, mensaje enviado...";
                    //ClientScript.RegisterStartupScript(this.GetType(), "UserMsg" ,"alert('"+ msg + "');", true);
                    //Msg.Visible = true;
                    //Msg.InnerHtml = msg;

                }
                catch (Exception ex)
                {
                    Global.LogError(this.Context, Global.EnumLogCategories.EMAIL, ex.Message);
                    //msg = "Lo sentimos, su mensaje no pudo ser enviado, intente mas tarde...";
                    //ClientScript.RegisterStartupScript(this.GetType(), "UserMsg", "alert('" + msg + "');", true);
                    //Msg.Visible = true;
                    //Msg.InnerHtml = msg;
                }

            }
            else
            {
                Msg.Visible = true;
                Msg.InnerHtml = "Existe un problema para validar la seguridad, intente mas tarde o por favor contacte a soporte técnico.";
                return;
            }

        }

        /*
        bool IsUserAlreadyExist()
        {
            
            SqlParameter[] parameters = {
                new SqlParameter { ParameterName="UserLogin", DbType= DbType.AnsiString, Size=128, Value= Email.Value.ToString()}

            };

            string email = SqlApiSqlClient.GetStringRecordValue("SELECT [UserLogin] FROM Users WHERE [UserLogin] = @UserLogin;", parameters, Global.Configuration.DB.GetConnectionStringDBMain());

            if (!string.IsNullOrEmpty(email)) return true;
            else return false;
            
        }
        */

        //TODO: send confirmation email
        bool CreateUser()
        {

            string salt, encrypass;

            Byte[] _salt;
            Byte[] _hash;

            //This is the password policy that all passwords must adhere to, if the password doesn't meet the policy we save CPU processing time by not even bothering to calculate hash of a clearly incorrect password
            PWDTK.PasswordPolicy PwdPolicy = new PWDTK.PasswordPolicy(numberUpper, numberNonAlphaNumeric, numberNumeric, minPwdLength, maxPwdLength);

            //or we can just use the default password policy provided by the API like below
            //PWDTK.PasswordPolicy PwdPolicy = PWDTK.cDefaultPasswordPolicy;

            //Get a random salt
            _salt = PWDTK.GetRandomSalt(saltSize);
            //Generate the hash value
            _hash = PWDTK.PasswordToHash(_salt, PasswordReg.Value.ToString(), iterations);

            encrypass = PWDTK.HashBytesToHexString(_hash);
            salt = PWDTK.HashBytesToHexString(_salt); // reverse operation PWDTK.HashHexStringToBytes();
             

            SqlParameter[] parameters = {
                new SqlParameter { ParameterName="Names", DbType= DbType.AnsiString, Size=50, Value= Names.Value.ToString()}
               ,new SqlParameter { ParameterName="LastName", DbType= DbType.AnsiString, Size=50, Value= LastName.Value.ToString()}
               ,new SqlParameter { ParameterName="Mobile", DbType= DbType.AnsiString, Size=50, Value= Mobile.Value.ToString()}
               ,new SqlParameter { ParameterName="Email", DbType= DbType.AnsiString, Size=50, Value= Email.Value.ToString()}
               ,new SqlParameter { ParameterName="Business", DbType= DbType.AnsiString, Size=50, Value= Business.Value.ToString()}
               ,new SqlParameter { ParameterName="Position", DbType= DbType.AnsiString, Size=50, Value= Position.Value.ToString()}
               ,new SqlParameter { ParameterName="Country", DbType= DbType.AnsiString, Size=50, Value= Country.Value.ToString()}
               ,new SqlParameter { ParameterName="City", DbType= DbType.AnsiString, Size=50, Value= City.Value.ToString()}
               ,new SqlParameter { ParameterName="Telephone", DbType= DbType.AnsiString, Size=50, Value= Telephone.Value.ToString()}
               ,new SqlParameter { ParameterName="Password", DbType= DbType.AnsiString, Size=1000, Value= encrypass}
               ,new SqlParameter { ParameterName="PasswordSalt", DbType= DbType.AnsiString, Size=1000, Value= salt}

            };

            string tsql = @"
SET NOCOUNT OFF;
INSERT INTO [CMSUserRegister] ([Names], [LastName], [Mobile], [Email], [Business], [Position], [Country], [City], [Telephone], [RegisterDate], [Password], [PasswordSalt], [LastLogin]) VALUES (@Names, @LastName, @Mobile, @Email, @Business, @Position, @Country, @City, @Telephone, GETDATE(), @Password, @PasswordSalt, GETDATE());
; ";
            var sqlserver = new SqlApiSqlClient();
            int r = sqlserver.CommandExecuteSqlString(tsql, parameters, Global.Configuration.DB.GetConnectionStringDBMain());

            if (r == 1) return true;
            else return false;

            


        }




    }
}