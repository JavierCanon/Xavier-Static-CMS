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
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.Security;



namespace XavierCMSWeb.UI
{
    public abstract class PageBaseCommon : System.Web.UI.Page
    {


        /*

        virtual protected void Page_Init(object sender, EventArgs e)
        {



        }

          
          
                virtual protected void Page_PreInit(object sender, EventArgs e)
        {

            if (GetIsDeveloperMode()) {

                    FormsAuthentication.SetAuthCookie(Global.Sessions.User.UserID, true);
            }


        } 
        
        virtual protected new void OnPreInit(EventArgs e)
        {


        }


        virtual protected void Page_Init(object sender, EventArgs e)
        {


        }

        virtual protected void Page_OnInit(EventArgs e)
        {


        }

        virtual protected void Page_OnInitComplete(EventArgs e)
        {

        }

        virtual protected void Page_Load(object sender, EventArgs e)
        {

        }

        virtual protected void Page_LoadComplete(object sender, EventArgs e)
        {

            //Global.LogDebug(this.Context, Global.EnumLogCategories.DEBUG, this.ToString() + " - Page_Load");
           // if (!IsPostBack && !IsCallback) Global.LogUser(Context, "INFO", "PAGE LOADED", this.ToString(), string.Empty);

        }
        */





        protected override void OnError(EventArgs e)
        {
            var lastError = Server.GetLastError();

            if (lastError.GetBaseException() is System.Web.HttpRequestValidationException)
            {
                System.Diagnostics.Debug.Assert(false);
                Response.Write("<h2>Advertencia</h2><p>Se detecto una entrada de texto con código peligroso para el sistema (<a href=\"http://es.wikipedia.org/wiki/Cross-site_scripting\" target=\"_blank\">Cross-site scripting</a>).<br /> Por favor revise que el texto que introdujo no contenga codigo HTML (<a href=\"http://es.wikipedia.org/wiki/HTML\" target=\"_blank\">Etiquetas HTML</a>) </p><p>Detalle Técnico:<br /></p><div style=\"width:100%;height:100%;overflow:auto;\">" + lastError.Message.ToString() + "</div>");
                Response.StatusCode = 200;
                Response.End();
            }
        }


        /// <summary>
        /// muestra un mensaje emergente en una ventana popup y la cierra.
        /// </summary>
        /// <param name="salert"></param>
        public void ResponseAlertAndCloseBrowserWindow(string salert)
        {
            string sresponse;
            sresponse = "<html><body><script language=\"javascript\" type=\"text/javascript\">";
            sresponse += @" alert('" + salert + "'); window.close();";
            sresponse += "</script></body></html>";
            Response.Write(sresponse);
            Response.Flush();
            Response.End();
        }

        /// <summary>
        /// muestra un mensaje emergente en una ventana popup y la cierra, actualizando el opener.
        /// </summary>
        /// <param name="salert"></param>
        public void ResponseAlertAndCloseBrowserWindowRefreshing(string salert)
        {
            string sresponse;
            sresponse = "<html><body><script language=\"javascript\" type=\"text/javascript\">";
            sresponse += @" alert('" + salert +
                         "'); opener.location.reload(); window.close(); getFocusedWindow().close();";
            sresponse += "</script></body></html>";
            Response.Write(sresponse);
            Response.Flush();
            Response.End();
        }
        /// <summary>
        /// escribe la pagina html y ejecuta un javascript
        /// </summary>
        /// <param name="sJScript"></param>
        public void ResponseWriteHtmlDocExecuteJs(string sJScript)
        {
            string sresponse;
            sresponse = "<html><body><script language=\"javascript\" type=\"text/javascript\">";
            sresponse += @sJScript;
            sresponse += @"</script></body></html>";
            Response.Write(@sresponse);
            Response.Flush();
            Response.End();
        }

        /// <summary>
        /// escribe la pagina html con codigo html
        /// </summary>
        /// <param name="sHtml"></param>
        public void ResponseWriteHtmlDoc(string sHtml)
        {
            string sresponse;
            sresponse = "<html><body>";
            sresponse += sHtml;
            sresponse += "</body></html>";
            Response.Write(sresponse);
            Response.Flush();
            Response.End();
        }

        protected void JSDisplayAlert(string message)
        {
            ClientScript.RegisterStartupScript(
                            this.GetType(),
                            Guid.NewGuid().ToString(),
                            string.Format("alert('{0}');", message.Replace("'", @"\'")),
                            true
                        );
        }

        /// <summary>
        /// Find a control from root.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected Control FindControlRecursive(string id)
        {
            return FindControlRecursive(id, this);
        }

        /// <summary>
        /// Find a control from root.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        protected Control FindControlRecursive(string id, Control parent)
        {
            // If parent is the control we're looking for, return it
            if (string.Compare(parent.ID, id, true) == 0)
                return parent;
            // Search through children
            foreach (Control child in parent.Controls)
            {
                Control match = FindControlRecursive(id, child);
                if (match != null)
                    return match;
            }
            // If we reach here then no control with id was found
            return null;
        }

        #region Cookies

        /// <summary>
        /// Get Cookie Value
        /// </summary>
        public string GetCookie(string sname)
        {
            return Request.Cookies[sname] == null ? null : Server.UrlDecode(Request.Cookies[sname].Value);
        }
        /// <summary>
        /// Update or create a new cookie.
        /// </summary>
        public void SetCookie(string sname, string svalue, double dExpiringDays)
        {
            if (Request.Cookies[sname] == null)
            {
                HttpCookie myCookie = new HttpCookie(sname, svalue);
                myCookie.Expires = DateTime.Now.AddDays(dExpiringDays);
                Response.Cookies.Add(myCookie);
            }
            else
            {
                Response.Cookies[sname].Value = svalue;
            }
        }

        #endregion Cookies

    }
}
