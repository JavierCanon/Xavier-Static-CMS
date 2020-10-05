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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace XavierCMSWeb.Admin
{
    public partial class PageNew : XavierCMSWeb.UI.PageBaseGrid
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void ASPxButtonSave_Click(object sender, EventArgs e)
        {

            Page.Validate();
            if (Page.IsValid) SaveHtml();
            else
            {
                divMessage.Visible = true;
                divMessage.InnerHtml = "Errores en el formulario, por favor corrija.";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "toastMsg", "$(\"#pageToastMsg\").modal('show');", true);
            }

        }


        void SaveHtml()
        {
            bool recordsaved = false;

            try
            {

                DataSourceMaster.InsertParameters["PageType"].DefaultValue = ContentTypeASPxComboBox.Text;
                DataSourceMaster.Insert();
                recordsaved = true;
            }
            catch (Exception e)
            {
                // throw;
                recordsaved = false;
                divMessage.Visible = true;
                divMessage.InnerHtml = "ERROR: " + Environment.NewLine + e.Message;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "toastMsg", "$(\"#pageToastMsg\").modal('show');", true);

            }
            finally
            {
                if (recordsaved)
                {

                    string contentType = ContentTypeASPxComboBox.Text.ToString().ToLower();
                    string category = (CategoryComboBox.Value.ToString());
                    string subCategory = (SubCategoryComboBox.Value.ToString());
                    string title = (TitleTextBox.Value.ToString());

                    if (!string.IsNullOrEmpty(contentType)
                        && !string.IsNullOrEmpty(category)
                        && !string.IsNullOrEmpty(subCategory)
                        && !string.IsNullOrEmpty(title)
                        )
                    {

                        string imgDir = Server.MapPath("~/Content/Img/" + contentType + "/" + category + "/" + subCategory + "/" + title);
                        string fileDir = Server.MapPath("~/Content/Files/" + contentType + "/" + category + "/" + subCategory + "/" + title);

                        if (!Directory.Exists(imgDir)) { _ = Directory.CreateDirectory(imgDir); }
                        if (!Directory.Exists(fileDir)) { _ = Directory.CreateDirectory(fileDir); }

                    }

                    divMessage.Visible = true;
                    divMessage.InnerHtml = "Página Guardada.";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "toastMsg", "$(\"#pageToastMsg\").modal('show');", true);

                }

            }




        }




    }
}