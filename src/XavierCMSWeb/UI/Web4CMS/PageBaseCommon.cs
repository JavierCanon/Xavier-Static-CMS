using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;



namespace XavierCMSWeb.UI
{
    public abstract class PageBaseCommon : System.Web.UI.Page
    {

        public static bool GetIsDeveloperMode()
        {
            return Global.Configuration.Development.GetIsEnabledDeveloperMode();
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {



        }


        protected new void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

        }


        protected void Page_Init(object sender, EventArgs e)
        {

            Page.Header.Controls.AddAt(0, new HtmlMeta { HttpEquiv = "X-UA-Compatible", Content = "IE=11;chrome=1" });
        }

        protected void Page_OnInit(EventArgs e)
        {


        }

        protected void Page_OnInitComplete(EventArgs e)
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {

            //Global.LogDebug(this.Context, Global.EnumLogCategories.DEBUG, this.ToString() + " - Page_Load");

           // if (!IsPostBack && !IsCallback) Global.LogUser(Context, "INFO", "PAGE LOADED", this.ToString(), string.Empty);

        }



        protected override void InitializeCulture()
        {
            /*
            if (Request.Form["ctl00$ddlCultureUI"] != null)
            {
                Session["CultureUI"] = Request.Form["ctl00$ddlCultureUI"].ToString();
            }

            if (Session["CultureUI"] != null)
            {
                var selectedLanguage = Session["CultureUI"].ToString();
                UICulture = selectedLanguage;
                Culture = selectedLanguage;

                Thread.CurrentThread.CurrentUICulture = new
                            CultureInfo(selectedLanguage);
            }
            */
            // culture fixes

            // colombia
            /*
            CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            newCulture.DateTimeFormat.DateSeparator = "-";
            newCulture.NumberFormat.NumberDecimalSeparator = ".";
            newCulture.NumberFormat.NumberGroupSeparator = ",";
            newCulture.NumberFormat.CurrencyDecimalSeparator = ".";
            newCulture.NumberFormat.CurrencyGroupSeparator = ",";

            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;
             */

            base.InitializeCulture();
        }


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

        protected virtual void JSDisplayAlert(string message)
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
        protected virtual Control FindControlRecursive(string id)
        {
            return FindControlRecursive(id, this);
        }

        /// <summary>
        /// Find a control from root.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        protected virtual Control FindControlRecursive(string id, Control parent)
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
