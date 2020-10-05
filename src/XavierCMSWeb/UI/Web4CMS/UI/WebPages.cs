using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using Softcanon.DAL;
using System.Data;
using System.Data.SqlClient;
using Web4CRM.BLL.Modules.Web4CMS.Enums;
using Web4CRM.DAL.Configuration;


namespace Web4CRM.BLL.Modules.Web4CMS.UI
{
    /// <summary>
    /// CMS System.Web.UI.Page child
    /// </summary>
    public class WebPages : System.Web.UI.Page
    {
        private WebPage _page = new WebPage();

        public WebPages()
        {
        }

        /// <summary>
        /// Assignes the selected theme to the pages.
        /// </summary>
        protected override void OnPreInit(EventArgs e)
        {
            _page = LoadPage();

            MasterPageFile = GetMasterPageFile();

            base.OnPreInit(e);
        }


        protected void Page_Init(object sender, EventArgs e)
        {
            LoadPageControls();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            


        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private string GetMasterPageFile()
        {
            var folder = "~/Themes/";
            if (_page.IsPrivateTheme)
            {
                folder += "Private/";
            }
            else
            {
                folder += "Public/";
            }

            folder += _page.Theme + "/";

            var masterpage = string.Empty;

            if (_page.ContentTypeId == 1)
            {
                masterpage = "FrontPageMasterPage.master";
            }
            else
            {
                if (_page.ContentTypeId == 2)
                {
                    masterpage = "PagesMasterPage.master";
                }
                else
                {
                    if (_page.ContentTypeId == 3)
                    {
                        masterpage = "FormsMasterPage.master";
                    }
                    else
                    {
                        if (_page.ContentTypeId == 4)
                        {
                            masterpage = "NewsMasterPage.master";
                        }
                        else
                        {
                            if (_page.ContentTypeId == 5)
                            {
                                masterpage = "BlogMasterPage.master";
                            }
                            else
                            {
                                if (_page.ContentTypeId == 6)
                                {
                                    masterpage = "ForumMasterPage.master";
                                }
                                else
                                {
                                    if (_page.ContentTypeId == 7)
                                    {
                                        masterpage = "LandingMasterPage.master";
                                    }
                                    else
                                    {
                                        throw new Exception("No page content type found for setup the apropiate masterpage.");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(Server.MapPath(folder + masterpage)))
            {
                folder += "FrontPageMasterPage.master";
            }
            else
            {
                folder += masterpage;
            }
            if (string.IsNullOrEmpty(Server.MapPath(folder)))
            {
                throw new Exception("Masterpage not found for current page in theme folder.");
            }
            return folder;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private string GetTitle()
        {
            return string.Empty;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private WebPage LoadPage()
        {
            var key = WebUtils.TitleRemoveIllegalCharacters(Request.Url.ToString());
            var obj = GetObjectFromCache(key);
            if (obj != null)
            {
                return (WebPage)obj;
            }
            else
            {
                var stitle = GetTitle();

                var tSql = "CMS_GetPage_byTitle";

                var sqlapi = new SqlApiSqlClient();
                var paramsToSP = new SqlParameter[] { new SqlParameter("@WebsiteId", SqlDbType.Int) { Value = Convert.ToInt32(Application["CMS_SitioWebId"].ToString()) }
                , new SqlParameter("@Title", SqlDbType.NVarChar, 120) { Value = stitle } };

                using (sqlapi.Connection = new SqlConnection(Web4CRM.Global.GetConnectionStringDBWeb4CRM()))
                {
                    var reader = sqlapi.DataReaderSqlSP(tSql, paramsToSP, 60);

                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            _page.IsFrontpage = false;
                            _page.ContentTypeId = 1;
                            _page.MetaRobotsId = 1;
                            _page.PageStatusId = 1;
                            _page.Title = "xxxxxxxxxxxxxxxxx";
                            _page.IsPrivateTheme = true;
                            _page.Theme = "Softcanon";

                            Page.Title = reader["Title"].ToString();

                            var masterContent = Master.FindControl("ContentPlaceHolderHead") as ContentPlaceHolder;
                            if (masterContent != null)
                            {
                                masterContent.Controls.Add(new Literal { Text = "<meta charset=\"UTF-8\" />" });
                                masterContent.Controls.Add(new Literal { Text = "<meta name=\"description\" content=\"" + reader["MetaDescription"].ToString() + "\" />" });
                                masterContent.Controls.Add(new Literal { Text = "<meta name=\"keywords\" content=\"" + reader["MetaKeywords"].ToString() + "\" />" });
                                masterContent.Controls.Add(new Literal { Text = "<meta name=\"author\" content=\"" + reader["MetaAutor"].ToString() + "\" />" });
                                masterContent.Controls.Add(new Literal { Text = "<meta name=\"copyright\" content=\"" + reader["MetaCopyright"].ToString() + "\" />" });

                                if (!String.IsNullOrEmpty(reader["MetaRobotsId"].ToString()))
                                {
                                    masterContent.Controls.Add(new Literal { Text = "<meta name=\"robots\" content=\"" + GetEnumMetaRobotsValue((EnumMetaRobots)reader.GetInt32(reader.GetOrdinal("MetaRobotsId"))) + "\" />" });
                                }
                                masterContent.Controls.Add(new Literal { Text = reader["ContentPlaceHolderHead"].ToString() });
                                masterContent.Controls.Add(new Literal { Text = "<style type=\"text/css\">" + Environment.NewLine + reader["ContentPlaceHolderHeadStyles"].ToString() + Environment.NewLine + "</style>" });


                                if (!String.IsNullOrEmpty(reader["GoogleAnalyticsTrackingCode"].ToString()))
                                {
                                    var GoogleAnalyticsTrackingCode = @"
<script>
  (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
  (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
  m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
  })(window,document,'script','//www.google-analytics.com/analytics.js','ga');

  ga('create', '" + reader["GoogleAnalyticsTrackingCode"].ToString() + "', '" + Web4CRM.Global.GetRequestDomainName() + @"');
  ga('send', 'pageview');

</script>
";

                                    masterContent.Controls.Add(new Literal { Text = GoogleAnalyticsTrackingCode });
                                }
                            }

                            masterContent = Master.FindControl("ContentPlaceHolderTop") as ContentPlaceHolder;
                            if (masterContent != null)
                            {
                                masterContent.Controls.Add(new Literal { Text = reader["ContentPlaceHolderTop"].ToString() });
                            }

                            masterContent = Master.FindControl("ContentPlaceHolderLogo") as ContentPlaceHolder;
                            if (masterContent != null)
                            {
                                masterContent.Controls.Add(new Literal { Text = reader["ContentPlaceHolderLogo"].ToString() });
                            }

                            masterContent = Master.FindControl("ContentPlaceHolderHeaderTop") as ContentPlaceHolder;
                            if (masterContent != null)
                            {
                                masterContent.Controls.Add(new Literal { Text = reader["ContentPlaceHolderHeaderTop"].ToString() });
                            }

                            masterContent = Master.FindControl("ContentPlaceHolderHeader") as ContentPlaceHolder;
                            if (masterContent != null)
                            {
                                masterContent.Controls.Add(new Literal { Text = reader["ContentPlaceHolderHeader"].ToString() });
                            }

                            masterContent = Master.FindControl("ContentPlaceHolderHeaderBottom") as ContentPlaceHolder;
                            if (masterContent != null)
                            {
                                masterContent.Controls.Add(new Literal { Text = reader["ContentPlaceHolderHeaderBottom"].ToString() });
                            }

                            masterContent = Master.FindControl("ContentPlaceHolderMenuTop") as ContentPlaceHolder;
                            if (masterContent != null)
                            {
                                masterContent.Controls.Add(new Literal { Text = reader["ContentPlaceHolderMenuTop"].ToString() });
                            }

                            masterContent = Master.FindControl("ContentPlaceHolderMenu") as ContentPlaceHolder;
                            if (masterContent != null)
                            {
                                masterContent.Controls.Add(new Literal { Text = reader["ContentPlaceHolderMenu"].ToString() });
                            }

                            masterContent = Master.FindControl("ContentPlaceHolderMenuBottom") as ContentPlaceHolder;
                            if (masterContent != null)
                            {
                                masterContent.Controls.Add(new Literal { Text = reader["ContentPlaceHolderMenuBottom"].ToString() });
                            }

                            masterContent = Master.FindControl("ContentPlaceHolderContentTop") as ContentPlaceHolder;
                            if (masterContent != null)
                            {
                                masterContent.Controls.Add(new Literal { Text = reader["ContentPlaceHolderContentTop"].ToString() });
                            }

                            masterContent = Master.FindControl("ContentPlaceHolderContent") as ContentPlaceHolder;
                            if (masterContent != null)
                            {
                                masterContent.Controls.Add(new Literal { Text = reader["ContentPlaceHolderContent"].ToString() });
                            }

                            masterContent = Master.FindControl("ContentPlaceHolderContentBottom") as ContentPlaceHolder;
                            if (masterContent != null)
                            {
                                masterContent.Controls.Add(new Literal { Text = reader["ContentPlaceHolderContentBottom"].ToString() });
                            }

                            masterContent = Master.FindControl("ContentPlaceHolderSideBarRightTop") as ContentPlaceHolder;
                            if (masterContent != null)
                            {
                                masterContent.Controls.Add(new Literal { Text = reader["ContentPlaceHolderSideBarRightTop"].ToString() });
                            }

                            masterContent = Master.FindControl("ContentPlaceHolderSideBarRight") as ContentPlaceHolder;
                            if (masterContent != null)
                            {
                                masterContent.Controls.Add(new Literal { Text = reader["ContentPlaceHolderSideBarRight"].ToString() });
                            }

                            masterContent = Master.FindControl("ContentPlaceHolderSideBarRightBottom") as ContentPlaceHolder;
                            if (masterContent != null)
                            {
                                masterContent.Controls.Add(new Literal { Text = reader["ContentPlaceHolderSideBarRightBottom"].ToString() });
                            }

                            masterContent = Master.FindControl("ContentPlaceHolderSideBarLeftTop") as ContentPlaceHolder;
                            if (masterContent != null)
                            {
                                masterContent.Controls.Add(new Literal { Text = reader["ContentPlaceHolderSideBarLeftTop"].ToString() });
                            }

                            masterContent = Master.FindControl("ContentPlaceHolderSideBarLeft") as ContentPlaceHolder;
                            if (masterContent != null)
                            {
                                masterContent.Controls.Add(new Literal { Text = reader["ContentPlaceHolderSideBarLeft"].ToString() });
                            }

                            masterContent = Master.FindControl("ContentPlaceHolderFooterTop") as ContentPlaceHolder;
                            if (masterContent != null)
                            {
                                masterContent.Controls.Add(new Literal { Text = reader["ContentPlaceHolderFooterTop"].ToString() });
                            }

                            masterContent = Master.FindControl("ContentPlaceHolderFooter") as ContentPlaceHolder;
                            if (masterContent != null)
                            {
                                masterContent.Controls.Add(new Literal { Text = reader["ContentPlaceHolderFooter"].ToString() });
                            }

                            masterContent = Master.FindControl("ContentPlaceHolderFooterBottom") as ContentPlaceHolder;
                            if (masterContent != null)
                            {
                                masterContent.Controls.Add(new Literal { Text = reader["ContentPlaceHolderFooterBottom"].ToString() });
                            }

                            masterContent = Master.FindControl("ContentPlaceHolderBottom") as ContentPlaceHolder;
                            if (masterContent != null)
                            {
                                masterContent.Controls.Add(new Literal { Text = reader["ContentPlaceHolderBottom"].ToString() });
                            }
                        }
                    }
                    else
                    {
                        Web4CRM.Global.LogError(Context, Web4CRM.Global.EnumLogCategories.WEB4CMS, "Page not found");
                        Server.Transfer("~/ErrorPageHttp404.aspx");
                    }

                    HttpRuntime.Cache.Insert(key, _page, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(AppConfiguration.GetCacheWeb4CMSPagesCacheDuration()));


                    _page.IsFrontpage = false;
                    _page.ContentTypeId = 1;
                    _page.MetaRobotsId = 1;
                    _page.PageStatusId = 1;
                    _page.Title = "xxxxxxxxxxxxxxxxx";
                    _page.IsPrivateTheme = true;
                    _page.Theme = "Softcanon";

                    return _page;
                }
            }
        }


        private void LoadPageControls()
        {
            Literal html;

            var ctrl = Master.FindControl(Enum.GetName(typeof(EnumContentPlaces), EnumContentPlaces.ContentPlaceHolderTop).ToString());
            if (ctrl != null)
            {
                html = new Literal();
                html.Text = "<h1>HOLA REMOCO TOP</h1>";
                ctrl.Controls.Add(html);
            }

            ctrl = Master.FindControl("ContentPlaceHolderFooterBottom");
            if (ctrl != null)
            {
                html = new Literal();
                html.Text = "<h1>HOLA REMOCO BOTTOM</h1>";
                ctrl.Controls.Add(html);
            }
        }

        private static object GetObjectFromCache(string key)
        {
            if (HttpRuntime.Cache[key] == null)
            {
                return null;
            }
            else
            {
                return (object)(HttpRuntime.Cache[key]);
            }
        }


        protected string GetEnumMetaRobotsValue(EnumMetaRobots e)
        {
            if (e.Equals(EnumMetaRobots.Index_Follow))
            {
                return "index, follow";
            }
            if (e.Equals(EnumMetaRobots.Index_NoFollow))
            {
                return "index, nofollow";
            }
            if (e.Equals(EnumMetaRobots.NoIndex_Follow))
            {
                return "noindex, follow";
            }
            if (e.Equals(EnumMetaRobots.NoIndex_NoFollow))
            {
                return "noindex, nofollow";
            }
            return string.Empty;
        }




        /// <summary>
        /// CMS webpage class
        /// </summary>
        private class WebPage
        {
            public bool IsFrontpage = false;
            public int ContentTypeId = 1;
            public int MetaRobotsId;
            public int PageStatusId;


            public string Title;

            public bool IsPrivateTheme = false;
            public string Theme;

            public string MetatagDescription;
            public string MetatagKeywords;
            public string MetatagAutor;
            public string MetatagCopyright;
            public string MetatagRobots;

            public string ContentPlaceHead;
            public string ContentPlaceHeadStyles;
            public string ContentPlaceHolderTop;
            public string ContentPlaceHolderLogo;
            public string ContentPlaceHolderHeaderTop;
            public string ContentPlaceHolderHeader;
            public string ContentPlaceHolderHeaderBottom;
            public string ContentPlaceHolderMenuTop;
            public string ContentPlaceHolderMenu;
            public string ContentPlaceHolderMenuBottom;
            public string ContentPlaceHolderContentTop;
            public string ContentPlaceHolderContent;
            public string ContentPlaceHolderContentBottom;
            public string ContentPlaceHolderSideBarRightTop;
            public string ContentPlaceHolderSideBarRight;
            public string ContentPlaceHolderSideBarRightBottom;
            public string ContentPlaceHolderSideBarLeftTop;
            public string ContentPlaceHolderSideBarLeft;
            public string ContentPlaceHolderSideBarLeftBottom;
            public string ContentPlaceHolderFooterTop;
            public string ContentPlaceHolderFooter;
            public string ContentPlaceHolderFooterBottom;
            public string ContentPlaceHolderBottom;

            public string TrackingCode;
            public string GoogleAnalyticsTrackingCode;


            public WebPage()
            {
            }
        }
    }
}

