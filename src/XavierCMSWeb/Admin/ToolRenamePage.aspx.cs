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
using System.Data;

namespace XavierCMSWeb.Admin
{
    public partial class ToolRenamePage : XavierCMSWeb.UI.PageBaseGrid
    {


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
           Page.Validate();
           if (!Page.IsValid) return;

            SqlDataSourceMaster.SelectParameters["PageID"].DefaultValue = ASPxComboBoxSearch.Value.ToString();
            string newTitle = TitleTextBox.Text.ToLower().Trim();

            divMessage.Visible = true;

            DataView dv = (DataView)SqlDataSourceMaster.Select(DataSourceSelectArguments.Empty); ;
            if (dv.Table.Rows.Count > 0)
            {


                string oldURL = dv.Table.Rows[0]["URL"].ToString();

                if (string.IsNullOrEmpty(oldURL))
                {
                    divMessage.InnerHtml = "<b>Error, URL página antigua no se encuentra en la base de datos.</b>";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "toastMsg", "$(\"#pageToastMsg\").modal('show');", true);
                    return;
                }

                string oldImgDir = Server.MapPath("~/Content/Img" + oldURL);
                if (!Directory.Exists(oldImgDir))
                {
                    _ = Directory.CreateDirectory(oldImgDir);
                    divMessage.InnerHtml += "<p>Atención, directorio de imágenes de la página antigua no se encuentra en el servidor.</p>";
                }

                string oldFileDir = Server.MapPath("~/Content/Files" + oldURL);
                if (!Directory.Exists(oldFileDir))
                {
                    _ = Directory.CreateDirectory(oldFileDir);
                    divMessage.InnerHtml += "<p>Atención, directorio de archivos de la página antigua no se encuentra en el servidor.</p>";

                }

                string[] rute = oldURL.Split('/');


                string imgDir = Server.MapPath("~/Content/Img/" + rute[1] + "/" + rute[2] + "/" + rute[3] + "/" + newTitle + "/");
                if (!Directory.Exists(imgDir))
                {
                    try
                    {
                        Directory.Move(oldImgDir, imgDir);

                        if (CheckDeleteDirectories.Checked) DeleteDirectory(oldImgDir);

                    }
                    catch (Exception ex)
                    {
                        Global.LogError(this.Context, Global.EnumLogCategories.FILESYSTEM, ex.Message);
                        divMessage.InnerHtml += "<p>Error, " + Server.HtmlEncode(ex.Message) + "</p>";
                    }
                }
                else
                {

                    divMessage.InnerHtml += "<p>Atención, el directorio de imágenes para la página ya existe en el servidor. No se cambio nada.</p>";
                }


                string fileDir = Server.MapPath("~/Content/Files/" + rute[1] + "/" + rute[2] + "/" + rute[3] + "/" + newTitle + "/");

                if (!Directory.Exists(fileDir))
                {
                    try
                    {
                        Directory.Move(oldFileDir, fileDir);

                        if (CheckDeleteDirectories.Checked) DeleteDirectory(oldFileDir);
                    }
                    catch (Exception ex)
                    {
                        Global.LogError(this.Context, Global.EnumLogCategories.FILESYSTEM, ex.Message);
                        divMessage.InnerHtml += "<p>Error, " + Server.HtmlEncode(ex.Message) + "</p>";
                    }


                }
                else
                {

                    divMessage.InnerHtml += "<p>Atención, el directorio de archivos para la página ya existe en el servidor. No se cambio nada.</p>";
                }


                SqlDataSourceMaster.UpdateParameters["PageID"].DefaultValue = ASPxComboBoxSearch.Value.ToString();
                SqlDataSourceMaster.UpdateParameters["Title"].DefaultValue = newTitle;
                SqlDataSourceMaster.Update();


                DatasourcesCachesInvalidate(SqlDataSourceMaster.CacheKeyDependency);
                DatasourcesCachesInvalidate(SqlDataSourcePages.CacheKeyDependency);
                ASPxComboBoxSearch.DataBind();

                divMessage.InnerHtml = "<b>Página renombrada...</b>";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "toastMsg", "$(\"#pageToastMsg\").modal('show');", true);


            }
            else {

                divMessage.InnerHtml = "<b>Error, no se encontró la página en la base de datos. No se actualizó.</b>";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "toastMsg", "$(\"#pageToastMsg\").modal('show');", true);

            }


        }

        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }

    }
}