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

namespace XavierCMSWeb
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {

            if (Request.QueryString["typ"] == null)
            {
                // frontpage
                this.MasterPageFile = "~/Templates/FrontPageMaster.Master";

            }
            else if (Request.QueryString["typ"] != null)
            {

                /*
                if (Request.QueryString["typ"].Equals("productos"))
                {

                    // now in web.config rewrite
                    //Server.Transfer("Products.aspx", true);

                }
                else if (Request.QueryString["typ"].Equals("producto"))
                {

                    Server.Transfer("Product.aspx", true);

                }
                else if (Request.QueryString["typ"].Equals("servicios"))
                {
                    // now in web.config rewrite
                    //Server.Transfer("Services.aspx", true);

                }
                else if (Request.QueryString["typ"].Equals("servicio"))
                {

                    Server.Transfer("Service.aspx", true);

                }
                else if (Request.QueryString["typ"].Equals("recursos"))
                {

                    // now in web.config rewrite
                    // Server.Transfer("Resources.aspx", true);
                }
                else if (Request.QueryString["typ"].Equals("recurso"))
                {

                    Server.Transfer("Resource.aspx", true);
                }
                else
                {

                    Response.Redirect("~/Error-404.html"); // not found...

                }

                */
            }
            else if (Request.QueryString["utm_source"] != null & Request.QueryString["utm_medium"] != null & Request.QueryString["utm_campaign"] != null & Request.QueryString["utm_term"] != null & Request.QueryString["utm_content"] != null)
            {
                // TODO: Landing pages
                // https://en.wikipedia.org/wiki/UTM_parameters

            }
            else
            {

                Response.Redirect("~/Error-404.html"); // not found...

            }

        }

        protected void Page_Init(object sender, EventArgs e)
        {



        }


    }
}