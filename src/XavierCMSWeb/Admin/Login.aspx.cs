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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using XavierCMSWeb.Safety.Recaptcha;
using System.Configuration;

namespace XavierCMSWeb.Admin {
    public partial class Login : System.Web.UI.Page {



        protected string GOOGLE_SITE_KEY = Global.Configuration.Security.Google.Recaptcha.v3.GetGoogleRecaptchaWebsiteKey();

        protected void Page_Init(object sender, EventArgs e)
        {


            if (Global.Configuration.Development.GetIsEnabledDeveloperMode())
            {

                //FormsAuthentication.RedirectFromLoginPage(Global.Sessions.User.UserGUID, false);
                Global.Debug.SetUserTestSessionVariables();
            }

        }



        protected void Page_Load(object sender, EventArgs e) {
            
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
         

        [Obsolete]
        protected void ASPxButtonLogin_Click(object sender, EventArgs e)
        {

            Page.Validate();

            if (!Page.IsValid) return;


            if (string.IsNullOrEmpty(recaptchaUserValue.Value))
            {
                Msg.Text = "Error en los datos de seguridad, vuelva a recargar la página.";
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

                if (UsernameTextbox.Text.Equals(ConfigurationManager.AppSettings["Authentication:Credentials.User.Login"].ToString(), StringComparison.InvariantCulture)
                    && PasswordTextbox.Text.Equals(ConfigurationManager.AppSettings["Authentication:Credentials.User.Password"].ToString(), StringComparison.InvariantCulture))
                {
                    Session["User.UserID"] = UsernameTextbox.Text;
                    Session.Timeout = 60;
                    Response.Redirect("~/Admin/Main.aspx");

                }
                else
                    Msg.Text = "Login failed. Please check your user name and password and try again.";

            }
            else
            {
                Msg.Text = "Existe un problema para validar la seguridad, intente mas tarde o por favor contacte a soporte técnico.";
                return;
            }






        }
    }
}