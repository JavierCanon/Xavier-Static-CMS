using System.Web.UI.WebControls;

namespace Web4CRM.BLL.Modules.Web4CMS.UI
{

	/// <summary>
	/// Summary description for ContentPlaceHolderCMS
	/// </summary>
	public class ContentPlaceHolderBase : ContentPlaceHolder
	{

		/// <summary>
		/// Gets the name. It must be exactly the same as the folder that contains the widget.
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// css name
		/// </summary>
		public string CssClass { get; set; }
		/// <summary>
		/// can edit 
		/// </summary>
		public bool IsEditable { get; set; }
		/// <summary>
		/// If render in the head tag
		/// </summary>
		public bool IsInHead { get; set; }

		/// <summary>
		/// Initialize control
		/// </summary>
		public ContentPlaceHolderBase()
		{
			IsInHead = false;
			IsEditable = true;
			Name = "ContentPlaceHolder";
			CssClass = "ContentPlaceHolder";
		}
	}
}
