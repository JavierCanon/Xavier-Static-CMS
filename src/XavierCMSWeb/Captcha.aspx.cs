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
using XavierCMSWeb.Safety.Recaptcha;

namespace XavierCMSWeb
{
    public partial class Captcha : System.Web.UI.Page
    {


        protected void Page_Init(object sender, EventArgs e)
        {
            recaptcha.Text = "<div class='g-recaptcha' data-sitekey='" + Global.Configuration.Security.Google.Recaptcha.v2.GetGoogleRecaptchaWebsiteKey() + "' data-size='compact' ></div>";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BootstrapButtonLogin_Click(object sender, EventArgs e)
        {
            divMessage.Visible = true;

            // validate captcha:
            if (Request["g-recaptcha-response"] == null)
            {
                divMessage.InnerHtml = "Error, no Captcha field form.  Intente mas tarde o por favor contacte a soporte técnico si el error persiste.";
                return;

            }

            var Recaptchav2 = new RecaptchaVerificationHelper();


            if (string.IsNullOrEmpty(Request["g-recaptcha-response"].ToString()))
            {
                divMessage.InnerHtml = "El Captcha no puede estar vacio.";
                return;
            }
            else
            {

                string secretkey = Global.Configuration.Security.Google.Recaptcha.v2.GetGoogleRecaptchaSecretKey();

                RecaptchaVerificationResult result = Recaptchav2.VerifyRecaptchaResponse(secretkey, Request["g-recaptcha-response"].ToString());

                if (result == RecaptchaVerificationResult.Success)
                {
                    //Response.Redirect( "Welcome.aspx" );
                    //divMessage.InnerHtml = "Captcha OK :D";

                    Response.Redirect("~/Admin/Login.aspx", true);
                    //Server.Transfer("~/Default.aspx", true);

                }
                else if (result == RecaptchaVerificationResult.IncorrectCaptchaSolution)
                {
                    divMessage.InnerHtml = "Valor de Captcha NO Valido.";
                    return;
                }
                else
                {
                    divMessage.InnerHtml = "Existe un problema para validar el captcha, intente mas tarde o por favor contacte a soporte técnico.";
                    return;
                }
            }





        }








    }
}