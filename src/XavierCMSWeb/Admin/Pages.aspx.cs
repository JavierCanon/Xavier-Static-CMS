﻿// GNU Version 3 License Copyright (c) 2020 Javier Cañon | https://javiercanon.com
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
using DevExpress.Export;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace XavierCMSWeb.Admin
{
    public partial class Pages : XavierCMSWeb.UI.PageBaseGrid
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack && !IsCallback)
            {

                DatasourcesCachesInit(DataSourceMaster.CacheKeyDependency);
                //DatasourcesCachesInit( DataSourceDetail.CacheKeyDependency );
                //ASPxGridView_Init_FixRowsCache_Datasources(); // fix initial binding for empty tables or queries


                //ASPxGridView1.LoadClientLayout(MasterGridCookieName);
            }
            else
            {
                //ASPxGridView_Load_FixRowsCache_Datasources();
            }

            if (IsCallback || IsPostBack)
            {
            }



        }


        #region ASPxGridView1

        protected void ASPxGridView1_Init(object sender, EventArgs e)
        {

            using (ASPxGridView gv = (sender as ASPxGridView))
            {

                gv.SettingsCookies.CookiesID = MasterGridCookieName;
                //gv.DataSourceID = "DataSourceMaster";
                //gv.KeyFieldName = "ID";
                //gv.SettingsText.Title = "";

                // Grid Columns
                // calculating decent grid size
                double size = 240;
                double minsize = 100;
                foreach (GridViewColumn col in gv.Columns)
                {
                    if (col.Visible == true)
                    {
                        if (col.Width.IsEmpty || col.Width.Value <= minsize)
                        {
                            col.Width = Unit.Pixel(Convert.ToInt32(minsize));
                            size += minsize;
                        }
                        else if (col.Width.Value > minsize)
                        {
                            size += col.Width.Value;
                        }
                        else
                        {
                            size += 100;
                        }
                    }
                }

                if (gv.Width.Value < size) gv.Width = Unit.Pixel(Convert.ToInt32(size));

                //gv.AutoGenerateColumns = true;

                /*>GRID_COLUMNS<*/

            };


        }

        protected void Grid_ToolbarItemClick(object source, ASPxGridToolbarItemClickEventArgs e)
        {

            //TODO: dont firing... 2018-12-16

            ASPxGridView grid = (ASPxGridView)source;
            switch (e.Item.Name)
            {
                case "CustomExportToXLS":
                    grid.ExportXlsToResponse(new DevExpress.XtraPrinting.XlsExportOptionsEx { ExportType = ExportType.WYSIWYG });
                    break;
                case "CustomExportToXLSX":
                    grid.ExportXlsxToResponse(new DevExpress.XtraPrinting.XlsxExportOptionsEx { ExportType = ExportType.WYSIWYG });
                    break;
                default:
                    break;
            }
        }


        protected void ASPxGridView1_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "databind")
            {
                DatasourcesCachesInvalidate(DataSourceMaster.CacheKeyDependency);
                //DatasourcesCachesInvalidate(UniqueIdPageName + ".DataSourceDetail");
                (sender as ASPxGridView).DataBind();
            }
        }

        protected void ASPxGridView1_DetailRowExpandedChanged(object sender, ASPxGridViewDetailRowEventArgs e)
        {
        }


        protected void ASPxGridView1_DataBinding(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Control the insertion on the DB.
        /// We have 2 options:
        /// 1. update a field value of the grid with: e.NewValues["prodDescripcionHtml"] = htmlEditor.Html; (Prefer method)
        /// 2. update the default or parameter value of the datasource: SqlDataSource1.SelectParameters[0].DefaultValue = novedadid;
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ASPxGridView1_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            /*
            var UserPass = ASPxGridView1.FindEditFormTemplateControl("UserPass") as ASPxTextBox;
            if (UserPass.Value != null)
            {
                var userId = Session["User.UserId"].ToString();
                e.NewValues["UserPass"] = UserLogin.GetNewUserPassword(userId, UserPass.Value.ToString(), UserLogin.EncryptionHashAlgorithm.SHA512, Web4Helpdesk.Global.DAL.GetConnectionStringDBWeb4Helpdesk());
            }
            else
            {
                e.NewValues["UserPass"] = string.Empty;
            }
             */

            /*
            ASPxGridView grid = sender as ASPxGridView;

            ASPxComboBox cb = grid.FindEditRowCellTemplateControl( grid.Columns["Region"] as GridViewDataColumn, "cbAddress1State" ) as ASPxComboBox;
            if (cb.Text != null)
            {
                e.NewValues["Region"] = cb.Text.ToString();

            }

            cb = grid.FindEditRowCellTemplateControl( grid.Columns["Ciudad"] as GridViewDataColumn, "cbAddress1City" ) as ASPxComboBox;
            if (cb.Text != null)
            {
                e.NewValues["Ciudad"] = cb.Text.ToString();

            }
            */


        }




        /// <summary>
        /// Check values before insert and after Parse_Value()
        /// http://www.devexpress.com/Support/Center/Question/Details/Q308089
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ASPxGridView1_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            /*
            if (e.NewValues["UserId"] == null) {
                e.NewValues["UserId"] = 0;
            }
            */
            /*
            var UserPass = ASPxGridView1.FindEditFormTemplateControl("UserPass") as ASPxTextBox;
            if (UserPass.Value != null)
            {
                var userId = Session["User.UserId"].ToString();
                e.NewValues["UserPass"] = UserLogin.GetNewUserPassword(userId, UserPass.Value.ToString(), UserLogin.EncryptionHashAlgorithm.SHA512, Web4Helpdesk.Global.DAL.GetConnectionStringDBWeb4Helpdesk());
            }
            else
            {
                e.NewValues["UserPass"] = System.Guid.NewGuid().ToString();
            }
             */

            /*
            ASPxGridView grid = sender as ASPxGridView;

            ASPxComboBox cb = grid.FindEditRowCellTemplateControl( grid.Columns["Region"] as GridViewDataColumn, "cbAddress1State" ) as ASPxComboBox;
            if (cb.Text != null)
            {
                e.NewValues["Region"] = cb.Text.ToString();

            }

            cb = grid.FindEditRowCellTemplateControl( grid.Columns["Ciudad"] as GridViewDataColumn, "cbAddress1City" ) as ASPxComboBox;
            if (cb.Text != null)
            {
                e.NewValues["Ciudad"] = cb.Text.ToString();

            }
            */

        }

        protected void ASPxGridView1_RowInserted(object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e)
        {
            // DatasourcesCachesInvalidate(DataSourceMaster.CacheKeyDependency);
            // DatasourcesCachesInvalidate(DataSourceDetail.CacheKeyDependency);

            var grid = (sender as ASPxGridView);
            var datasrc = FindControlRecursive(grid.DataSourceID) as SqlDataSource;
            DatasourcesCachesInvalidate(datasrc.CacheKeyDependency);

            // TODO: BUG not raising events... 
            // TODO: use name in database (ContentType)
            
            string contentType = string.Empty;
            string contentTypeID = e.NewValues["ContentTypeID"].ToString();
            string category = e.NewValues["Category"].ToString();
            string subCategory = e.NewValues["SubCategory"].ToString();
            string title = e.NewValues["Title"].ToString();


            if (contentTypeID == "8") contentType = "Productos";
            if (contentTypeID == "9") contentType = "Servicios";
            if (contentTypeID == "10") contentType = "Recursos";
            if (contentTypeID == "11") contentType = "Aplicaciones";
            if (contentTypeID == "12") contentType = "Industrias";

            if (!string.IsNullOrEmpty(contentType)
                && !string.IsNullOrEmpty(category)
                && !string.IsNullOrEmpty(subCategory)
                && !string.IsNullOrEmpty(title)
                ) {

                string imgDir = Server.MapPath("~/Content/Img/" + contentType + "/" + category + "/" + subCategory + "/" + title);
                string fileDir = Server.MapPath("~/Content/Files/" + contentType + "/" + category + "/" + subCategory + "/" + title);

                if (!Directory.Exists(imgDir)) { _ = Directory.CreateDirectory(imgDir); }
                if (!Directory.Exists(fileDir)) { _ = Directory.CreateDirectory(fileDir); }

            }


        }
        protected void ASPxGridView1_RowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
        {
            // DatasourcesCachesInvalidate(DataSourceMaster.CacheKeyDependency);
            // DatasourcesCachesInvalidate(DataSourceDetail.CacheKeyDependency);
            var grid = (sender as ASPxGridView);
            var datasrc = FindControlRecursive(grid.DataSourceID) as SqlDataSource;
            DatasourcesCachesInvalidate(datasrc.CacheKeyDependency);

            // TODO: BUG not raising events... 
            // TODO: use name in database (ContentType)

            string contentType = string.Empty;
            string contentTypeID = e.NewValues["ContentTypeID"].ToString();
            string category = e.NewValues["Category"].ToString();
            string subCategory = e.NewValues["SubCategory"].ToString();
            string title = e.NewValues["Title"].ToString();


            if (contentTypeID == "8") contentType = "Productos";
            if (contentTypeID == "9") contentType = "Servicios";
            if (contentTypeID == "10") contentType = "Recursos";
            if (contentTypeID == "11") contentType = "Aplicaciones";
            if (contentTypeID == "12") contentType = "Industrias";



            if (!string.IsNullOrEmpty(contentType)
                && !string.IsNullOrEmpty(category)
                && !string.IsNullOrEmpty(subCategory)
                && !string.IsNullOrEmpty(title)
                )
            {

                string imgDir = Server.MapPath("~/Content/Img/" + contentType + "/" + category + "/" + subCategory + "/" + title);
                string fileDir = Server.MapPath("~/Content/Files/" + contentType + "/" + category + "/" + subCategory + "/" + title);

                if (!Directory.Exists(imgDir)) { _ = Directory.CreateDirectory(imgDir); }
                if (!Directory.Exists(fileDir)) { _ = Directory.CreateDirectory(fileDir); }

            }

        }

        protected void ASPxGridView1_RowDeleted(object sender, DevExpress.Web.Data.ASPxDataDeletedEventArgs e)
        {
            // DatasourcesCachesInvalidate(DataSourceMaster.CacheKeyDependency);
            // DatasourcesCachesInvalidate(DataSourceDetail.CacheKeyDependency);
            var grid = (sender as ASPxGridView);
            var datasrc = FindControlRecursive(grid.DataSourceID) as SqlDataSource;
            DatasourcesCachesInvalidate(datasrc.CacheKeyDependency);
        }


        /// <summary>
        /// http://demos.devexpress.com/ASPxGridViewDemos/GridEditing/Validation.aspx
        /// if (e.NewValues["FirstName"] != null && e.NewValues["FirstName"].ToString().Length
        /// AddError(e.Errors, grid.Columns["Categoria"], grid.Columns["Categoria"].Caption + ": El valor ya existe con otro registro...");
        /// TIP: si el campo esta invisible, e.NewValues devuelve NULL
        /// </summary>
        protected void ASPxGridView1_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {

            /*
            if (ASPxGridView1.IsNewRowEditing && e.NewValues["UserPass"] == null)
            {
                AddError(e.Errors, ASPxGridView1.Columns["UserPass"], ASPxGridView1.Columns["UserPass"].Caption + ": Escriba una contraseña.");
            }
            */

            /*
            string tsql = "";

            // validar valores contra DB
            // TIP: si el campo esta invisible, e.NewValues devuelve NULL
            // http://devexpress.com/Support/Center/p/Q109251.aspx
            if (e.NewValues["UserLogin"] != null)
            {
                int Id = 0;
                int currentIndex = 0;

                if (e.Keys.Count > 0)
                    currentIndex = ASPxGridView1.FindVisibleIndexByKeyValue(e.Keys[0]);

                object IdvalGrid = ASPxGridView1.GetRowValues(currentIndex, "UserId");

                if (IdvalGrid != null)
                    Id = Convert.ToInt32(IdvalGrid);

                SqlParameter[] paramsToSP = {

                             new SqlParameter("@UserLogin", e.NewValues["UserLogin"].ToString())
                            ,new SqlParameter("@UserId", Id )
                             };


                if (e.IsNewRow) // insert
                    tsql = "SELECT UserId FROM [User] WHERE UserLogin = @UserLogin";
                else // update
                    tsql = "SELECT UserId FROM [User] WHERE UserLogin = @UserLogin AND UserId != @UserId";


                int? codigo = SqlApiSqlClient.GetIntRecordValue(tsql, paramsToSP, Global.GetConnectionStringDBWeb4CRM());
                if (codigo != null)
                {
                    AddError(e.Errors, ASPxGridView1.Columns["UserLogin"], ASPxGridView1.Columns["UserLogin"].Caption + ": El valor ya existe con otro registro...");
                }
            }

            */
            if (string.IsNullOrEmpty(e.RowError) && e.Errors.Count > 0)
            {
                e.RowError = "Por favor corrija los errores.";
            }

        }
        private void AddError(Dictionary<GridViewColumn, string> errors, GridViewColumn column, string errorText)
        {
            if (errors.ContainsKey(column))
            {
                return;
            }
            errors[column] = errorText;
        }
        /// <summary>
        /// Para mostrar una fila de algun color especifico.
        ///
        /// bool hasError = e.GetValue("FirstName").ToString().Length 
        /// </<></summary>
        protected void ASPxGridView1_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
        }

        protected void ASPxGridView1_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            var grid = (sender as ASPxGridView);

            if (!grid.IsNewRowEditing)
            {
                grid.DoRowValidation();
            }
        }
        /// <summary>
        /// Validar tipo datos de los valores introducidos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ASPxGridView1_ParseValue(object sender, DevExpress.Web.Data.ASPxParseValueEventArgs e)
        {
            /*
            if (ASPxGridView1.IsNewRowEditing)
            {
                if (e.FieldName == "UserId") e.Value = 0;

                if (e.FieldName == "UserPass")
                {
                    if (string.IsNullOrEmpty(e.Value.ToString())) e.Value = System.Guid.NewGuid().ToString();
                }

            }
             */

        }

        protected void ASPxGridView1_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            /*
            if (e.Column.GetType() == typeof( GridViewDataTextColumn ))
            {
                (e.Column as GridViewDataTextColumn).PropertiesTextEdit.ValidationSettings.ValidationGroup =
                    (e.Editor.NamingContainer as GridViewEditFormTemplateContainer).ValidationGroup;
            }

            if (e.Column.GetType() == typeof( GridViewDataComboBoxColumn ))
            {
                (e.Column as GridViewDataComboBoxColumn).PropertiesComboBox.ValidationSettings.ValidationGroup =
                    (e.Editor.NamingContainer as GridViewEditFormTemplateContainer).ValidationGroup;
            }
            */

            /*
            ASPxGridView grid = sender as ASPxGridView;

            // comboboxes
            if (grid.IsEditing || grid.IsNewRowEditing)
            {

                object val, val2;
                if (e.Column.FieldName == "Region")
                {

                    ASPxComboBox combo = e.Editor as ASPxComboBox;
                    combo.Callback += new CallbackEventHandlerBase( AddressState_Callback );

                    if (!grid.IsNewRowEditing)
                    {

                        val = grid.GetRowValuesByKeyValue( e.KeyValue, "Pais" );

                        if (val == DBNull.Value || val == null) return;
                        string country = (string)val;
                        FillRegionCombo( combo, country );
                        // set the current value:
                        if(!grid.IsNewRowEditing)
                            if (!string.IsNullOrEmpty( e.Value.ToString() ))
                            {
                                combo.SelectedIndex = combo.Items.IndexOfValue( e.Value.ToString() ) - 1;
                            }

                    }
                }

                if (e.Column.FieldName == "Ciudad")
                {
                    ASPxComboBox combo2 = e.Editor as ASPxComboBox;
                    combo2.Callback += new CallbackEventHandlerBase( AddressCity_Callback );

                    if (!grid.IsNewRowEditing)
                    {

                        val = grid.GetRowValuesByKeyValue( e.KeyValue, "Pais" );
                        if (val == DBNull.Value || val == null) return;
                        string country = (string)val;
                        val2 = grid.GetRowValuesByKeyValue( e.KeyValue, "Region" );
                        if (val2 == DBNull.Value) return;
                        string state = (string)val2;

                        FillCityCombo( combo2, country, state );
                        // set the current value:
                        if (!grid.IsNewRowEditing)
                            if (!string.IsNullOrEmpty( e.Value.ToString() ))
                        {
                            combo2.SelectedIndex = combo2.Items.IndexOfValue( e.Value.ToString() ) - 1;
                        }
                    }
                }
            }*/



        }
        #endregion ASPxGridView1


        #region grid detail

        protected void ASPxGridView1Detail_BeforePerformDataSelect(object sender, EventArgs e)
        {
            // var id = (sender as ASPxGridView).GetMasterRowKeyValue();
            // DataSourceDetail.SelectParameters[0].DefaultValue = (sender as ASPxGridView).GetMasterRowKeyValue().ToString();
            //var grid = (sender as ASPxGridView);
            // var datasrc = FindControlRecursive(grid.DataSourceID) as SqlDataSource;
            // datasrc.SelectParameters["ContactoID"].DefaultValue = grid.GetMasterRowKeyValue().ToString();
        }


        protected void ASPxGridView1Detail_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            /*
            if (e.NewValues["ContactoID"] == null)
            {
                e.NewValues["ContactoID"] = (sender as ASPxGridView).GetMasterRowKeyValue().ToString();
            }
            */
        }

        /*
        protected void ASPxGridView1Detail_RowInserted( object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e )
        {
            DatasourcesCachesInvalidate( DataSourceDetail.CacheKeyDependency );

        }
        */
        #endregion grid detail



        #region Import

        protected void ASPxButtonImport_Click(object sender, EventArgs e)
        {

            //string table = "inventario";

            //Response.Redirect( "~/Web/CRM/Import/ImportFiles.aspx?cat=CRM" + table + "&ReturnUrl=" + Server.UrlEncode( Request.RawUrl ) );

        }

        #endregion Import


        #region Controls Callbacks
        /*
        protected void FillRegionCombo( ASPxComboBox cb, string country )
        {
            if (string.IsNullOrEmpty( country ))
                return;

            SqlDataSource_State.SelectParameters[0].DefaultValue = country;

            cb.Value = null;
            cb.DataBindItems(); 			// http://www.devexpress.com/Support/Center/Question/Details/Q134409

            ListEditItem item = new ListEditItem( "", null );
            cb.Items.Insert( 0, item );

        }

        protected void AddressState_Callback( object sender, DevExpress.Web.CallbackEventArgsBase e )
        {

            string country = e.Parameter;

            if (string.IsNullOrEmpty( country ))
                return;

            FillRegionCombo( sender as ASPxComboBox, e.Parameter );


        }


        protected void FillCityCombo( ASPxComboBox cb, string country, string state )
        {
            if (string.IsNullOrEmpty( country ) || string.IsNullOrEmpty( state ))
                return;

            SqlDataSource_City.SelectParameters["Country_ID"].DefaultValue = country.Trim();
            SqlDataSource_City.SelectParameters["Region_ID"].DefaultValue = state.Trim();

            cb.Value = null;
            //cb.DataBind();
            cb.DataBindItems();

            ListEditItem item = new ListEditItem( "", null );
            cb.Items.Insert( 0, item );

        }


        protected void AddressCity_Callback( object sender, DevExpress.Web.CallbackEventArgsBase e )
        {

            string[] param = e.Parameter.Split( '|' );
            string country = param[0];
            string state = param[1];

            if (string.IsNullOrEmpty( country ) || string.IsNullOrEmpty( state ))
                return;

            FillCityCombo( sender as ASPxComboBox, country, state );


        }
        */
        #endregion  Controls Callbacks



        #region Filters

        protected void ASPxButtonSearch_Click(object sender, EventArgs e)
        {
            Page.Validate();

            //  if(IsPostBack && ASPxEdit.ValidateEditorsInContainer(this))
            if (Page.IsValid)
            {

                // dont need this because the parameter is now a control:
                // masterDataSource.SelectParameters["Search"].DefaultValue = ASPxTextBoxSearch.Value.ToString();
                //masterDataSource.DataBind();
                //ASPxGridView1.DataBind();




            }
        }

        #endregion Filters

    }
}