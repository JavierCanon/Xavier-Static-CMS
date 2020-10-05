using System;
using System.Web.UI;
using System.Text;

namespace Web4CRM.BLL.Modules.Web4CMS.UI
{

	/// <summary>
	/// Summary description for ContentPlaceHolderCMS
	/// </summary>
	public class ContentPlaceHolderCMS : ContentPlaceHolderBase
	{
		public ContentPlaceHolderCMS()
		{
		}
		public override void RenderControl(HtmlTextWriter writer)
		{
			if (string.IsNullOrEmpty(Name))
			{
				throw new System.NullReferenceException("Name must be set on a ContentPlaceHolderCMS Masterpage Template");
			}
			if (!IsInHead)
			{
				var sb = new StringBuilder();
				sb.Append("<div class=\"" + CssClass + "\" id=\"" + this.ID + "\">");

				if (IsEditable)
				{
					sb.Append("<a class=\"delete\" href=\"javascript:void(0)\" onclick=\"removeContentPlaceHolder('" + this.ID + "');return false\" title=\"" + Resources.Labels.delete + " Bloque\">X</a>");
					sb.Append(" | <a class=\"edit\" href=\"javascript:void(0)\" onclick=\"editContentPlaceHolder('" + Name + "', '" + this.ID + "');return false\" title=\"" + Resources.Labels.edit + " Bloque\">" + Resources.Labels.edit + "</a>");
				}

				sb.Append("<div class=\"content\">");
				writer.Write(sb.ToString());
			}

			base.Render(writer);

			if (!IsInHead)
			{
				writer.Write("</div>" + Environment.NewLine + "</div>");
			}
		}
	}
}
