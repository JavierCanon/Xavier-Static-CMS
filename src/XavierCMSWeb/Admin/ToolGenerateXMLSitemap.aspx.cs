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
using System.Data.SqlClient;
using System.Xml;
using System.Text;
using System.IO;

namespace XavierCMSWeb.Admin
{
    public partial class ToolGenerateXMLSitemap : System.Web.UI.Page
    {
        enum changefreq
        {
            always, // Use for pages that change every time they are accessed.
            hourly,
            daily,
            weekly,
            monthly,
            yearly,
            never, //Use this value for archived URLs.

        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {

            // http://www.ashishblog.com/create-a-sitemap-with-asp-net/

            string file = Server.MapPath("~/sitemap.xml");
            if (File.Exists(file))
            {
                File.Delete(file);
                File.Delete(Server.MapPath("~/sitemap_index.xml"));

            }

            // Response.Clear();
            // Response.ContentType = "text/xml";
            // using (XmlTextWriter writer = new XmlTextWriter(Response.OutputStream, Encoding.UTF8))
            using (XmlTextWriter writer = new XmlTextWriter(file, Encoding.UTF8))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("urlset");
                writer.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");

                string connect = Global.Configuration.DB.GetConnectionStringDBMain();
                using (SqlConnection con = new SqlConnection(connect))
                {
                    con.Open();
                    string query = @"
SELECT 
    CASE [PageType]
    WHEN 'PRODUCTOS' THEN
    'https://precisur.com.co/producto/' + LOWER([Category])+ '/' + LOWER([SubCategory])+ '/' + LOWER([Title]) + '/' 
    WHEN 'SERVICIOS' THEN
    'https://precisur.com.co/servicio/' + LOWER([Category])+ '/' + LOWER([SubCategory])+ '/' + LOWER([Title]) + '/' 
    WHEN 'RECURSO' THEN
    'https://precisur.com.co/recurso/' + LOWER([Category])+ '/' + LOWER([SubCategory])+ '/' + LOWER([Title]) + '/' 
    WHEN 'APLICACIONES' THEN
    'https://precisur.com.co/aplicacion/' + LOWER([Category])+ '/' + LOWER([SubCategory])+ '/' + LOWER([Title]) + '/' 
    WHEN 'INDUSTRIAS' THEN
    'https://precisur.com.co/industria/' + LOWER([Category])+ '/' + LOWER([SubCategory])+ '/' + LOWER([Title]) + '/' 
    END AS URL
    ,CONVERT(DATE, [DateUpdated]) [DateUpdated]
FROM [dbo].[CMSPage]
WHERE
[PageStatusID] = 2
;";

                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {

                        if (!string.IsNullOrEmpty(dr[0].ToString()))
                        {
                            writer.WriteStartElement("url");
                            writer.WriteElementString("loc", (dr[0].ToString()));
                            writer.WriteElementString("lastmod", String.Format("{0:yyyy-MM-dd}", dr[1]));
                            writer.WriteElementString("changefreq", Enum.GetName(typeof(changefreq), changefreq.monthly));
                            writer.WriteElementString("priority", "1.0");
                            writer.WriteEndElement();
                        }
                    }
                    con.Close();
                    con.Dispose();
                }


                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
            }

            File.Copy(file, Server.MapPath("~/sitemap_index.xml"), true);

            divMessage.Visible = true;
            divMessage.InnerHtml = "<b>Archivo sitemap.xml generado.</b>";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "toastMsg", "$(\"#pageToastMsg\").modal('show');", true);

        }
    }
}