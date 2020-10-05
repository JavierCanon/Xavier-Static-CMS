using System;
using System.Collections.Generic;
using System.Web.UI;
using System.IO;
using System.Text;
using System.Globalization;
using System.Web.UI.HtmlControls;

namespace Web4CRM.BLL.Modules.Web4CMS.UI
{
	public class StaticWebPage : Page
	{
		protected bool IsFrontPage = false;
		protected string PageLanguage = "EN";
		protected bool IsGeneratingStaticPage = false;

		protected string PageTitle;



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

			return Web4CRM.BLL.Modules.Web4CMS.WebUtils.TitleRemoveIllegalCharacters(title);

		}


	}

}