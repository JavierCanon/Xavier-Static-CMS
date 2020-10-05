using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        WYSIWYG.Editor.AddTo(TextArea1); //WYSIWYG .net editor is easy to implement in your .NET web application: You need to add only this line of code!
        if (!IsPostBack)
        {
            System.IO.StreamReader Reader = new System.IO.StreamReader(MapPath("app_data\\htmlcode.txt"));
            TextArea1.Value = Reader.ReadToEnd();
            //Load content
            Reader.Close();
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        System.IO.StreamWriter Writer = new System.IO.StreamWriter(MapPath("app_data\\htmlcode.txt"));
        Writer.Write(TextArea1.Value);
        //Save content
        Writer.Close();
    }
}