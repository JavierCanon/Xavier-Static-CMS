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
using DevExpress.XtraRichEdit.Commands.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace XavierCMSWeb.Admin
{
    public partial class ToolsImages : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            

        }

        protected void ASPxButtonCreateDefaultImages_Click(object sender, EventArgs e)
        {

            // RegisterAsyncTask(new PageAsyncTask(CreateDefaultImages));
            
            Func<CancellationToken, Task> workItem = CreateDefaultImagesAsync;
            HostingEnvironment.QueueBackgroundWorkItem(workItem);


        }



        async Task CreateDefaultImagesAsync(CancellationToken cancellationToken)
        {

            // products
            DirectoryInfo root = new DirectoryInfo(Server.MapPath("~/Content/Productos"));
            string sourceFile = Server.MapPath("~/Content/Img/Template/productos-tab.jpg");

            foreach (DirectoryInfo level1 in root.GetDirectories()) {

                foreach (DirectoryInfo level2 in level1.GetDirectories())
                {

                    foreach (DirectoryInfo level3 in level2.GetDirectories())
                    {

                        File.Copy(sourceFile, Path.Combine(level3.FullName, "productos-tab.jpg"));

                    }


                }

            }


        }


    }
}