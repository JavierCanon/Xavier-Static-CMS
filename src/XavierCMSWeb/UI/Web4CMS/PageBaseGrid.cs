using System;
using System.Web.UI.WebControls;

namespace XavierCMSWeb.UI
{
    public abstract class PageBaseGrid : PageBase
    {

        public static string UniqueIdPageName, MasterTableName, MasterKeyFieldName, MasterGridCookieName, DetailGridCookieName;


        protected new void Page_PreInit(object sender, EventArgs e)
        {

            //base.Page_PreInit( sender, e );
            
            UniqueIdPageName = Page.ToString().Replace(".", "_");
            MasterGridCookieName = UniqueIdPageName + "_MasterGrid";
            DetailGridCookieName = UniqueIdPageName + "_DetailGrid";

            //MasterTableName = "ticket";
            //MasterKeyFieldName = "ID";

        }


        protected void DBMainDataSources_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.CommandTimeout = 60 * 5;
        }


        protected void DBMainDataSources_Init(object sender, EventArgs e)
        {
            var ds = (sender as SqlDataSource);
            ds.CacheKeyDependency = UniqueIdPageName + "_" + ds.UniqueID;
            ds.ConnectionString = Global.Configuration.DB.GetConnectionStringDBMain();

        }


    }
}
