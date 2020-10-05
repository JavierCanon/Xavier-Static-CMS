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
using Dalayer.DAL;
using Rolex.Authentication.Passwords;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using XavierCMSWeb.Safety.Recaptcha;

namespace XavierCMSWeb
{
    public partial class UserLogin : System.Web.UI.Page
    {


        protected string GOOGLE_SITE_KEY = Global.Configuration.Security.Google.Recaptcha.v3.GetGoogleRecaptchaWebsiteKey();

        //Below is used to generate a password policy that you may use to check that passwords adhere to this policy
        const int numberUpper = 1;
        const int numberNonAlphaNumeric = 1;
        const int numberNumeric = 1;
        const int minPwdLength = 8;
        const int maxPwdLength = Int32.MaxValue;
        const int iterations = 10002;  //Number of hash iterations

        protected void Page_Init(object sender, EventArgs e)
        {


            if (Global.Configuration.Development.GetIsEnabledDeveloperMode())
            {
                //Global.Development.SetUserTestSessionVariables();
                FormsAuthentication.RedirectFromLoginPage(Global.Sessions.User.UserGUID, false);
            }

        }



        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /*
        protected void btnLogin_Click(object sender, EventArgs e) {
            if (Membership.ValidateUser(tbUserName.Text, tbPassword.Text)) {
                if(string.IsNullOrEmpty(Request.QueryString["ReturnUrl"])) {
                    FormsAuthentication.SetAuthCookie(tbUserName.Text, false);
                    Response.Redirect("~/");
                }
                else
                    FormsAuthentication.RedirectFromLoginPage(tbUserName.Text, false);
            }
            else {
                tbUserName.ErrorText = "Invalid user";
                tbUserName.IsValid = false;
            }
        }
        */


        protected void ASPxButtonLogin_Click(object sender, EventArgs e)
        {

            Page.Validate();

            if (!Page.IsValid) return;


            if (string.IsNullOrEmpty(recaptchaUserValue.Value))
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
                , recaptchaUserValue.Value
                  );

            if (recaptchaResult == RecaptchaVerificationResult.Success)
            {
                //divMessage.InnerHtml = "Score: " + Recaptchav3.Score;
                decimal? minScore = new decimal(0.6);
                if (Recaptchav3.Score < minScore) Response.Redirect("~/Captcha.aspx", true);


                //create session
                // Global.Sessions.UserCreateSession();

                // Go main menu.
                if (ValidateLogin())
                {

                    HttpCookie userid = new HttpCookie("User.Email", Email.Value.ToString())
                    {
                        Expires = DateTime.Now.AddYears(1)
                    };
                    Response.Cookies.Add(userid);

                    Response.Redirect("~/recursos/");

                }
                else
                    Msg.Visible = true;
                Msg.InnerHtml = "Login fallido. Por favor revise sus datos e intente de nuevo.";

            }
            else
            {
                Msg.Visible = true;
                Msg.InnerHtml = "Existe un problema para validar la seguridad, intente mas tarde o por favor contacte a soporte técnico.";
            }




            bool ValidateLogin()
            {

                bool loginOK = false;
                string salt = string.Empty, encrypass = string.Empty, dbpassword = string.Empty;

                SqlParameter[] parameters = {
                 new SqlParameter { ParameterName="Email", DbType= DbType.AnsiString, Size=50, Value= Email.Value.ToString() }
                
            };

                string tsql = @"
SELECT TOP 1 
       [UserRegisterID]
      ,[Names]
      ,[LastName]
      ,[Email]
      ,[Password]
      ,[PasswordSalt]
  FROM [CMSUserRegister]
WHERE
Email = @Email 
ORDER BY [UserRegisterID] DESC
;";
                var sqlserver = new SqlApiSqlClient();


                using (sqlserver.Connection = new SqlConnection(Global.Configuration.DB.GetConnectionStringDBMain()))
                {

                    using (var dr = sqlserver.DataReaderSqlString(tsql, parameters))
                    {

                        if (dr.Read())
                        {

                            salt = dr["PasswordSalt"].ToString(); ;
                            dbpassword = dr["Password"].ToString(); ;


                            Byte[] _salt;
                            Byte[] _hash;

                            //This is the password policy that all passwords must adhere to, if the password doesn't meet the policy we save CPU processing time by not even bothering to calculate hash of a clearly incorrect password
                            PWDTK.PasswordPolicy PwdPolicy = new PWDTK.PasswordPolicy(numberUpper, numberNonAlphaNumeric, numberNumeric, minPwdLength, maxPwdLength);

                            //or we can just use the default password policy provided by the API like below
                            //PWDTK.PasswordPolicy PwdPolicy = PWDTK.cDefaultPasswordPolicy;

                            _salt = PWDTK.HashHexStringToBytes(salt); // reverse operation ;

                            //Generate the hash value
                            _hash = PWDTK.PasswordToHash(_salt, Password.Value.ToString(), iterations);

                            encrypass = PWDTK.HashBytesToHexString(_hash);


                            if (encrypass == dbpassword)
                            {

                                loginOK = true;

                                // Session["User.UserEmail"] = dr["UserEmail"].ToString();
                            }
                            else {
                                loginOK = false;
                            }


                        }
                        else
                        {
                            loginOK = false;
                        }

                        dr.Close();
                    }

                    sqlserver.Connection.Close();
                };


                if (loginOK) return true; else return false;



            }

        }

    }
}