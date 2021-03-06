// GNU Version 3 License Copyright (c) 2020 Javier Ca�on | https://javiercanon.com
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
using System.Web.Security;

namespace XavierCMSWeb.Views.Masters
{
    public partial class AdminMaster : XavierCMSWeb.UI.Templates.MasterPages
    {

        protected new void Page_Init(object sender, EventArgs e)
        {
            base.Page_Init(sender, e);

            if (!Global.Sessions.GetIsValidSession()) Response.Redirect("~/Admin/Login.aspx" , true);

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            // fix rewrite controls post back problem. https://ruslany.net/2008/10/aspnet-postbacks-and-url-rewriting/
            form1.Action = Request.RawUrl;

        }

        protected void LoggedInMenuMenu_ItemClick(object source, DevExpress.Web.Bootstrap.BootstrapMenuItemEventArgs e)
        {
            if (e.Item.Name == "Logout")
            {
                FormsAuthentication.SignOut();
                Response.Redirect("~/Admin/Login.aspx");
            }
        }
    }
}