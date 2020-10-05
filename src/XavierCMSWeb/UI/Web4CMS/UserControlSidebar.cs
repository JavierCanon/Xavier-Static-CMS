using System;
using System.Collections.Generic;

namespace XavierCMSWeb.UI
{

	/// <summary>
	/// Summary description for UserControlSidebar
	/// </summary>
	public class UserControlSidebar : UserControlBase
	{
		protected string GetIsActiveLink(string path)
		{
			if (Request.Url.ToString().Contains(path))
			{
				return "class=\"active\"";
			}
			else
			{
				return string.Empty;
			}
		}
	}
}
