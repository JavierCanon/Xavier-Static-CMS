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
using System.Web.UI;
using System.IO;
using System.Text;
using System.Globalization;
using System.Web.UI.HtmlControls;

namespace XavierCMSWeb.UI
{
	public class StaticWebPage : System.Web.UI.Page
	{
		protected static bool IsFrontPage; // = false;
		protected static string PageLanguage; // = "EN";
		protected static bool IsGeneratingStaticPage; // = false;

		protected static string PageTitle;



		public StaticWebPage()
		{

		}


		protected void Page_PreInit(object sender, EventArgs e)
		{

			Response.Headers.Add("Content-Language", PageLanguage);

			if (Request.QueryString["genstatic"] != null)
			{
				if (Request.QueryString["genstatic"] == "1")
					IsGeneratingStaticPage = true;
				else
					IsGeneratingStaticPage = false;

			}

			HtmlElement html = (HtmlElement)Master.FindControl("html");
			html.Attributes.Add("lang", PageLanguage.ToLowerInvariant());

			// TODO: security checks
			if (!IsGeneratingStaticPage)
			{

				/*
				HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("body");
				LiteralControl btn = new LiteralControl("<div class=\"alert alert-info alert-dismissable\">Click for <a href=\"?genstatic=1\" class=\"alert-link\">GENERATE STATIC PAGE</a></div>");
				body.Controls.AddAt(Controls.Count - 1, btn);
				*/
				LiteralControl btn1 = new LiteralControl("<div class=\"alert alert-info alert-dismissable\">Click for <a href=\"?genstatic=1\" class=\"alert-link\">GENERATE STATIC PAGE</a></div>");
				Master.FindControl("EditTools").Controls.Add(btn1);
			}

		}

		protected override void Render(HtmlTextWriter writer)
		{

			if (!string.IsNullOrEmpty(PageTitle)) Title = PageTitle;


			if (IsGeneratingStaticPage)
			{
				using (StringWriter stringWriter = new StringWriter(new StringBuilder(), CultureInfo.InvariantCulture))
				{
					HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);
					base.Render(htmlTextWriter);

					string path, writefilename = GetPageFileName();

					if (IsFrontPage)
					{
						path = Server.MapPath(".") + "\\" + writefilename;
						File.WriteAllText(path, stringWriter.ToString(), Encoding.UTF8);
						Response.Redirect(writefilename);
					}
					else
					{
						//path = Server.MapPath(PageLanguage);
						// language dir exist?
						//if (!Directory.Exists(path)) Directory.CreateDirectory(path);

						path = Server.MapPath(".") + "\\" + writefilename;
						File.WriteAllText(path, stringWriter.ToString(), Encoding.UTF8);
						Response.Redirect(writefilename);

					}

				}
			}
			else
			{
				base.Render(writer);

			}
		}

		string GetPageFileName()
		{
			//TODO sanitize and replace invalid chars for valid url and for filename.

			if (IsFrontPage)
				return "Index.html";
			else
			{

				string _pageLanguage = PageLanguage != "EN" ? "." + PageLanguage : "";


				if (string.IsNullOrEmpty(PageTitle))
				{
					//get the current page name
					string pageName = Path.GetFileNameWithoutExtension(Request.Path);
					return pageName.Replace(".CMS", "") + _pageLanguage + ".html";
				}
				else
					return CheckValidFilename(PageTitle) + _pageLanguage + ".html";

			}
		}


		static string CheckValidFilename(string title)
		{

			return HtmlHelper.TitleRemoveIllegalCharacters(title);

		}


	}

}