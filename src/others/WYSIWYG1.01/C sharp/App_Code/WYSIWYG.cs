using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace WYSIWYG
{
    public static class Editor
    {
        public static void AddTo(TextBox TextBox, bool AddCenterButton = false)
        {
            AddTo((Control)TextBox, AddCenterButton);
        }
        public static void AddTo(HtmlTextArea TextArea, bool AddCenterButton = false)
        {
            AddTo((Control)TextArea, AddCenterButton);
        }
        private static void AddTo(Control TextArea, bool AddCenterButton = false)
        {
            Page Page = TextArea.Page;
            Control Tools = new Control();
            AddAt(TextArea, Tools);
            AddAt(TextArea, new WebControl(HtmlTextWriterTag.Br));
            WebControl Editor = new WebControl(HtmlTextWriterTag.Div);
            Editor.ID = "editor";
            Editor.Attributes.Add("contenteditable", "true");
            Editor.Style.Add("border-style", "dashed");
            Editor.Style.Add("width", "inherit");
            Editor.Style.Add("overflow-y", "scroll");
            Editor.Style.Add("padding", "10px");
            AddAt(TextArea, Editor);
            Page.Form.Controls.Add(new LiteralControl("<script>document.getElementById('" + Editor.ClientID + "').style.height=parseInt(document.documentElement.clientHeight*0.7)+'px';var Editor=document.getElementById('" + Editor.ClientID + "');var TextArea=document.getElementById('" + TextArea.ClientID + "');TextArea.style.display='none';Editor.innerHTML=TextArea.value;function Save(){TextArea.value=Editor.innerHTML}</script>"));
            Page.Form.Attributes.Add("onsubmit", "Save()");
            int Anchor = (int)IconName.Anchor;
            int Bold = (int)IconName.Bold;
            int Italic = (int)IconName.Italic;
            int Neutral = 0x2115;
            int BulletedList = (int)IconName.BulletedList;
            int EnumeratedList = 0x2419;
            dynamic Req = HttpContext.Current.Request;
            if (Req.Browser.Browser == "Chrome")
            {
                //Chrome dont support this unichode chars, then change this!
                Anchor = 0x21ac;
                Bold = 0x24b7;
                Italic = 0x24be;
            }
            else if (Req.UserAgent.Contains("Android"))
            {
                Anchor = 0x21ac;
                Bold = 0x24b7;
                Italic = 0x24be;
                Neutral = 0x24dd;
                BulletedList = 0x21f6;
                EnumeratedList = 0x2116;
            }
            string ReqLink = null;
            bool ReqLinkInsideAlgoritm = false;
            if (Req.Browser.Browser == "IE")
            {
                ReqLinkInsideAlgoritm = true;
                ReqLink = "null";
            }
            else
            {
                ReqLink = "prompt('URL:', 'http://')";
            }
            Ico(Tools, (int)IconName.Undo, "Undo", "undo");
            Ico(Tools, (int)IconName.Redo, "Redo", "redo");
            Ico(Tools, Anchor, "Link", "createlink'," + ReqLinkInsideAlgoritm.ToString().ToLower() + "," + ReqLink, ")");
            Ico(Tools, Bold, "Bold", "bold");
            Ico(Tools, Italic, "Italic", "italic");
            Ico(Tools, (int)IconName.Underline, "Underline", "underline");
            Ico(Tools, 0x20a6, "Strikethrough", "strikethrough");
            Ico(Tools, 0xaa, "Superscript", "'superscript'");
            Ico(Tools, (int)IconName.One, "H1", "formatBlock',false,'<h1>");
            Ico(Tools, (int)IconName.Two, "H2", "formatBlock',false,'<h2>");
            Ico(Tools, (int)IconName.Three, "H3", "formatBlock',false,'<h3>");
            Ico(Tools, 0x24d2, "Block code", "formatBlock',false,'<pre>");
            Ico(Tools, Neutral, "Neutral", "formatBlock',false,'<div>");
            Ico(Tools, 0x21e4, "Justify left", "justifyleft");
            if (AddCenterButton)
                Ico(Tools, 0x2194, "Justify center", "justifycenter");
            Ico(Tools, 0x21e5, "Justify right", "justifyright");
            Ico(Tools, BulletedList, "List", "insertunorderedlist");
            Ico(Tools, EnumeratedList, "Enumerated list", "insertorderedlist");
            HyperLink Link = new HyperLink();
            Link.NavigateUrl = "http://wysiwygnet.com/";
            TextArea.Parent.Controls.AddAt(TextArea.Parent.Controls.IndexOf(TextArea) + 1, Link);
            Link.Style.Add("font-size", "x-small");
            Link.Text = "wysiwyg";
            //The remotion of this line is a violation of Creative Common licenze
        }

        private static void AddAt(Control TextArea, Control Ctrl)
        {
            TextArea.Parent.Controls.AddAt(TextArea.Parent.Controls.IndexOf(TextArea), Ctrl);
        }

        private static void Ico(Control Tools, int Name, string ToolTip, string Cmd, string E = "')")
        {
            System.Web.UI.WebControls.HyperLink Icon = new System.Web.UI.WebControls.HyperLink();
            Icon.CssClass = "icon";
            Icon.Style.Add("font-size", "xx-large");
            Icon.Style.Add("text-decoration", "none!important");
            Icon.ToolTip = ToolTip;
            Icon.NavigateUrl = "javascript:var status=document.execCommand('" + Cmd + E;
            Icon.Text = char.ConvertFromUtf32(Name);
            Tools.Controls.Add(Icon);
        }

        private enum IconName
        {
            Anchor = 9875,
            Bold = 119809,
            BulletedList = 8788,
            Italic = 119868,
            Underline = 95,
            Undo = 0x21b6,
            One = 9312,
            Redo = 0x21b7,
            Three = 9314,
            Two = 9313
        }
    }

}