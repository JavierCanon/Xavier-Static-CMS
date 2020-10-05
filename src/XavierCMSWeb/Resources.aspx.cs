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
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace XavierCMSWeb
{
    public partial class Resources : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack && !IsCallback)
            {
                if (Request.QueryString["query"] != null) BootstrapCardView1.SearchPanelFilter = Request.QueryString["query"];
                this.Title = "Recursos Pesaje Industrial";
            }

        }

        protected string GetImage(string category, string subcategory, string title)
        {

            string dir = "~/Content/Img/Recursos/" + category.Trim() + "/" + subcategory.Trim() + "/" + title.Trim() + "/";
            string file = dir + title.Trim() + "-card.jpg";

            if (File.Exists(Server.MapPath(file)))
                return (file);
            else
            {

                // get first image
                string physicalpath = Server.MapPath(dir);

                if (!Directory.Exists(physicalpath)) Directory.CreateDirectory(physicalpath);
                var dinfo = new DirectoryInfo(physicalpath);

                /*
                var firstFileName = dinfo.EnumerateFiles()
                                      .Select(f => f.FullName)
                                      .FirstOrDefault();
                */

                string firstFileName = string.Empty;

                FileInfo[] fi = dinfo.GetFiles("*.*");
                if (fi.Length > 0)
                {

                    firstFileName = fi[0].Name;
                }

                if (!string.IsNullOrEmpty(firstFileName))
                    return ResolveClientUrl(dir + firstFileName);
                else
                    return ResolveClientUrl("~/Content/Img/Template/recursos-tab.jpg");

            }
        }

    }
}