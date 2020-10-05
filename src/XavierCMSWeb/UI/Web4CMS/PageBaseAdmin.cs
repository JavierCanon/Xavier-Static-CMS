using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace XavierCMSWeb.UI
{
    public class PageBaseAdmin : PageBase
    {

        protected override void CheckSession()
        {
            if (Session["User.UserID"] == null)
            {
                Session.Clear();
                Session.Abandon();
                Response.Redirect( "~/Web/Login.aspx?ReturnUrl=" + Request.RawUrl );
            }
            else
            {

                // if no admin show error permission

                if (   Session["User.UserRolID"].ToString() != "0" //( (int)UserRolEnum.System ) 
                    && Session["User.UserRolID"].ToString() != "1" //( (int)UserRolEnum.SuperAdministrator )
                    && Session["User.UserRolID"].ToString() != "2"  //( (int)UserRolEnum.Administrator )
                    )
                {
                    Response.Redirect( "~/ErrorPermission.html" );

                }


            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override bool CheckIsValidSession()
        {
            if (Session["User.UserID"] != null)
            {

                // if no admin show error permission

                if (   Session["User.UserRolID"].ToString() != "0" //( (int)UserRolEnum.System ) 
                    && Session["User.UserRolID"].ToString() != "1" //( (int)UserRolEnum.SuperAdministrator )
                    && Session["User.UserRolID"].ToString() != "2"  //( (int)UserRolEnum.Administrator )
                    )
                {
                    return false;
                }
                  
                else
                    return true;
            }
            else
            {
                return false;
            }
        }


    }
}