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
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace XavierCMSWeb
{
    public partial class Product : System.Web.UI.Page
    {
        protected string PageTitle, PageBody, PageMetaKeywords, PageMetaDescription;
        protected List<ListItem> files = new List<ListItem>();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsCallback && !IsPostBack)
            {

                DataView dv = (DataView)SqlDataSourceMaster.Select(DataSourceSelectArguments.Empty); ;
                if (dv.Table.Rows.Count > 0)
                {
                    this.Title = dv.Table.Rows[0]["Title"].ToString();
                    PageBody = dv.Table.Rows[0]["Body"].ToString();
                    PageMetaKeywords = dv.Table.Rows[0]["MetaKeywords"].ToString();
                    // TODO: Create Meta's SEO fields/page Editing in editor 
                    PageMetaDescription = StripHTML(dv.Table.Rows[0]["Description"].ToString().Substring(0, Math.Min(dv.Table.Rows[0]["Description"].ToString().Length, 160)));


                    string resfolder = "~/Content/Files/Productos/" + dv.Table.Rows[0]["Category"].ToString().Trim() + "/" + dv.Table.Rows[0]["SubCategory"].ToString().Trim() + "/" + dv.Table.Rows[0]["Title"].ToString().Trim();

                    if (Directory.Exists(Server.MapPath(resfolder)))
                    {
                        string[] filePaths = Directory.GetFiles(Server.MapPath(resfolder));

                        string filename, downloadurl;
                        foreach (string filePath in filePaths)
                        {
                            filename = Path.GetFileName(filePath);
                            downloadurl = ResolveUrl(resfolder + "/" + filename);
                            files.Add(new ListItem(filename.Substring(0, filename.Length -3).Replace('-',' ').Replace('_',' '), downloadurl));
                        }
                        GridView1.DataSource = files;
                        GridView1.DataBind();
                    }

                    string resfolderImg = "~/Content/Img/Productos/" + dv.Table.Rows[0]["Category"].ToString().Trim() + "/" + dv.Table.Rows[0]["SubCategory"].ToString().Trim() + "/" + dv.Table.Rows[0]["Title"].ToString().Trim();

                    if (Directory.Exists(Server.MapPath(resfolderImg)))
                    {
                        ASPxImageSlider1.ImageSourceFolder = resfolderImg;
                    }
                    else
                    {
                        ASPxImageSlider1.Enabled = false; // for security
                        ASPxImageSlider1.Visible = false;
                    }

                    // BREADCRUM                    
                    if (Request.QueryString["typ"] != null) Breadcrum.InnerHtml += string.Format("<li class=\"breadcrumb-item\"><a href = \"{0}\"> {1} </a></li>", "/products.aspx", Request.QueryString["typ"].ToUpper());
                    if (Request.QueryString["cat"] != null) Breadcrum.InnerHtml += string.Format("<li class=\"breadcrumb-item\"><a href = \"{0}\"> {1} </a></li>", "/products.aspx?query=" + Request.QueryString["cat"], Request.QueryString["cat"].ToUpper());
                    if (Request.QueryString["sub"] != null) Breadcrum.InnerHtml += string.Format("<li class=\"breadcrumb-item\"><a href = \"{0}\"> {1} </a></li>", "/products.aspx?query=" + Request.QueryString["sub"], Request.QueryString["sub"].ToUpper());
                    if (Request.QueryString["pag"] != null) Breadcrum.InnerHtml += string.Format("<li class=\"breadcrumb-item active\">{0}</li>", Request.QueryString["pag"].ToUpper());

                    Header.DataBind();

                }

            }

        }

        string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

    }
}