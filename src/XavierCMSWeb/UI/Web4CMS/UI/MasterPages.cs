using System;
using System.Collections.Generic;
using Web4CRM.DAL.Configuration;

namespace Web4CRM.BLL.Modules.Web4CMS.UI
{

	/// <summary>
	/// Summary description for MasterPages
	/// </summary>
	public class MasterPages : System.Web.UI.MasterPage
	{
		protected void Page_Init(object sender, EventArgs e)
		{
			Page.Header.DataBind();
		}


		/// <summary>
		/// return if needed a minified version of file (css and js) with optimization settings in web.config.
		/// </summary>
		/// <param name="relativeUrl"></param>
		/// <returns></returns>
		protected string ResolveClientResource(string relativeUrl)
		{
			if (relativeUrl.EndsWith(".css") && !relativeUrl.EndsWith(".min.css") )
			{
				if (AppConfiguration.GetIsEnabledOptimizationCSSUseMinifiedFiles())
				{
					relativeUrl = relativeUrl.Replace(".css", ".min.css");
				}
			}

			if (relativeUrl.EndsWith(".js") && !relativeUrl.EndsWith(".min.js"))
			{
				if (AppConfiguration.GetIsEnabledOptimizationCSSUseMinifiedFiles())
				{
					relativeUrl = relativeUrl.Replace(".js", ".min.js");
				}
			}

			return ResolveClientUrl(relativeUrl);
		}
	}
}
