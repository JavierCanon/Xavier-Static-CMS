<%-- 
// GNU Version 3 License Copyright (c) 2020 Javier Cañon | https://javiercanon.com
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
--%>


<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/AdminMaster.master" AutoEventWireup="true" CodeBehind="Pages.aspx.cs" Inherits="XavierCMSWeb.Admin.Pages" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentBody" runat="server">

    <script type="text/javascript">
        function OnToolbarItemClick(s, e) {
            if (IsCustomExportToolbarCommand(e.item.name)) {
                e.processOnServer = true;
                e.usePostBack = true;
                return;
            } else if (e.item.name == "ResetGrid") { ResetGrid(grid); return; }
        }
        function IsCustomExportToolbarCommand(command) {
            if (command == "CustomExportToXLS" || command == "CustomExportToXLSX")
                return true;
            else
                return false;
        }
        function gridDeleteRow(targetGrid, id) {
            if (confirm('¿Esta seguro de ELIMINAR el registro actual? (' + id + ')')) {
                targetGrid.DeleteRowByKey(id);
            }
        }
        function ResetGrid(targetGrid) {
            if (confirm('¿Esta seguro de RESETEAR el diseño de la grilla actual?')) {
                ASPxClientUtils.DeleteCookie("<%= XavierCMSWeb.UI.PageBaseGrid.MasterGridCookieName  %>");<%-- *** WARNING SET THE SAME VALUE OF SettingsCookies-CookiesID OF GRID *** --%>
                targetGrid.Refresh();
                window.location.reload();
            }
        }
        function ResetDetGrid(targetGrid) {
            if (confirm('¿Esta seguro de RESETEAR el diseño de la grilla actual?')) {
                ASPxClientUtils.DeleteCookie("<%= XavierCMSWeb.UI.PageBaseGrid.DetailGridCookieName  %>");<%-- *** WARNING SET THE SAME VALUE OF SettingsCookies-CookiesID OF GRID *** --%>
                targetGrid.Refresh();
                window.location.reload();
            }
        }
        function Grid_Update_Click(grid) {
            var isValid = ASPxClientEdit.ValidateEditorsInContainer(grid.GetMainElement());
            if (isValid)
                grid.UpdateEdit();
        }
        var detgridcommand = "";
        function detailGridOnBeginCallback(s, e) {
            detgridcommand = e.command;
        }
        function detailGridOnEndCallback(s, e) {
            /*  if (detgridcommand == "ADDNEWROW" || detgridcommand == "UPDATEEDIT" || detgridcommand == "DELETEROW") {
                  s.Refresh();
              }*/
        }
        function sPopup(userId, table) {
            /*
            var spage = "UsersDataFilter.aspx?time=" + new Date().getTime() + "&userId=" + userId + "&table=" + table;
            showPopupMasterModal('Edicion Filtros', spage);*/
        }

        function OnCountryChanged(cmbCountry, cmbRegion) {
            if (cmbRegion.InCallback())
                return;
            else
                cmbRegion.PerformCallback(cmbCountry.GetValue().toString());
        }
        function OnEndCallbackRegion(s, e) {
        }
        function OnRegionChanged(cmbCountry, cmbRegion, cmbCity) {
            if (cmbCity.InCallback()) return;
            else
                cmbCity.PerformCallback(cmbCountry.GetValue().toString() + '|' + cmbRegion.GetValue().toString());
        }
        function OnEndCallbackCity(s, e) {
        }
    </script>


    <dx:ASPxGridView ID="ASPxGridView1" runat="server"
        KeyFieldName="PageID"
        DataSourceID="DataSourceMaster"
        ClientInstanceName="grid"
        OnInit="ASPxGridView1_Init"
        OnHtmlRowPrepared="ASPxGridView1_HtmlRowPrepared"
        OnRowValidating="ASPxGridView1_RowValidating"
        OnStartRowEditing="ASPxGridView1_StartRowEditing"
        OnRowUpdating="ASPxGridView1_RowUpdating"
        OnParseValue="ASPxGridView1_ParseValue"
        OnRowDeleted="ASPxGridView1_RowDeleted"
        OnRowInserted="ASPxGridView1_RowInserted"
        OnRowUpdated="ASPxGridView1_RowUpdated"
        OnDataBinding="ASPxGridView1_DataBinding"
        OnRowInserting="ASPxGridView1_RowInserting"
        OnDetailRowExpandedChanged="ASPxGridView1_DetailRowExpandedChanged"
        OnCustomCallback="ASPxGridView1_CustomCallback"
        OnCellEditorInitialize="ASPxGridView1_CellEditorInitialize"
        OnToolbarItemClick="Grid_ToolbarItemClick"
        AutoGenerateColumns="False">

        <SettingsText />

        <Settings ColumnMinWidth="60" HorizontalScrollBarMode="Hidden" VerticalScrollBarMode="Hidden"
            ShowFilterRow="True" ShowTitlePanel="True" ShowFilterBar="Hidden" />

        <SettingsCommandButton
            DeleteButton-ButtonType="Image"
            EditButton-ButtonType="Image"
            NewButton-ButtonType="Image"
            RenderMode="Button">
            <NewButton Image-IconID="actions_additem_16x16office2013" />
            <EditButton Image-IconID="edit_edit_16x16office2013" />
            <DeleteButton Image-IconID="spreadsheet_deletesheetrows_16x16" />
            <UpdateButton Image-IconID="actions_apply_16x16office2013" />
            <CancelButton Image-IconID="actions_cancel_16x16office2013" />
            <SearchPanelApplyButton>
            </SearchPanelApplyButton>
            <SearchPanelClearButton>
            </SearchPanelClearButton>
            <ApplyFilterButton></ApplyFilterButton>
            <ClearFilterButton></ClearFilterButton>
        </SettingsCommandButton>

        <SettingsDataSecurity />
        <SettingsResizing ColumnResizeMode="Control" Visualization="Live" />

        <SettingsBehavior ConfirmDelete="True" EnableRowHotTrack="True"
            AllowDragDrop="True" AllowFocusedRow="True" AllowGroup="True" AllowSort="True" AllowSelectByRowClick="True"
            EnableCustomizationWindow="True" AllowEllipsisInText="False" />

        <%-- cookie bug on paging  
        <SettingsPager SEOFriendly="Enabled">
        </SettingsPager>
        --%>

        <SettingsEditing Mode="PopupEditForm" EditFormColumnCount="1" />

        <SettingsPopup>
            <EditForm HorizontalAlign="WindowCenter" ShowCollapseButton="True" ShowFooter="True" ShowMaximizeButton="True">
                <SettingsAdaptivity Mode="OnWindowInnerWidth" />
            </EditForm>
        </SettingsPopup>

        <Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True"
            ShowFooter="True" ShowGroupPanel="True" ShowTitlePanel="True" ShowColumnHeaders="True"
            ShowGroupButtons="True" ShowHeaderFilterButton="False" />

        <SettingsCookies Enabled="True" StoreControlWidth="True" />

        <SettingsDetail ShowDetailRow="true" AllowOnlyOneMasterRowExpanded="False" ExportMode="All"></SettingsDetail>

        <SettingsContextMenu Enabled="True" EnableFooterMenu="True" EnableGroupPanelMenu="True"
            EnableRowMenu="True" EnableColumnMenu="True">
        </SettingsContextMenu>

        <SettingsSearchPanel Visible="False" CustomEditorID="tbToolbarSearch" ShowClearButton="False" ShowApplyButton="False" Delay="1500" />
        <SettingsExport EnableClientSideExportAPI="true" ExcelExportMode="DataAware" />

        <Columns>
            <dx:GridViewDataTextColumn FieldName="PageID" Caption="ID" ReadOnly="True" VisibleIndex="0" Width="60">
                <EditFormSettings Visible="False"></EditFormSettings>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataComboBoxColumn FieldName="ContentTypeID" Caption="Tipo" VisibleIndex="1" Width="120">
                <PropertiesComboBox DataSourceID="SqlDataSourceContentType" TextField="ContentType" ValueField="ContentTypeID" ValueType="System.Int32">
                    <ValidationSettings>
                        <RequiredField IsRequired="true" />
                    </ValidationSettings>
                </PropertiesComboBox>
            </dx:GridViewDataComboBoxColumn>

            <dx:GridViewDataComboBoxColumn FieldName="PageStatusID" Caption="Estado" VisibleIndex="2" Width="120">
                <PropertiesComboBox DataSourceID="SqlDataSourcePageStatus" TextField="PageStatus" ValueField="PageStatusID" ValueType="System.Int32">
                    <ValidationSettings>
                        <RequiredField IsRequired="true" />
                    </ValidationSettings>
                </PropertiesComboBox>
            </dx:GridViewDataComboBoxColumn>

            <dx:GridViewDataTextColumn FieldName="Category" Caption="Categoria" VisibleIndex="3" Width="120">
                <PropertiesTextEdit>
                    <ValidationSettings>
                        <RegularExpression ValidationExpression="^([0-9A-Za-z\u00C0-\u00D6\u00D8-\u00f6\u00f8-\u00ff\s.-]*)$" ErrorText="Algunos Caracteres Invalidos" />
                        <RequiredField IsRequired="true" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="SubCategory" Caption="Subcategoria" VisibleIndex="4" Width="120">
                <PropertiesTextEdit>
                    <ValidationSettings>
                        <RegularExpression ValidationExpression="^([0-9A-Za-z\u00C0-\u00D6\u00D8-\u00f6\u00f8-\u00ff\s.-]*)$" ErrorText="Algunos Caracteres Invalidos" />
                        <RequiredField IsRequired="true" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Title" Caption="Titulo" VisibleIndex="5" Width="240">
                <PropertiesTextEdit>
                    <ValidationSettings>
                        <RegularExpression ValidationExpression="^([0-9A-Za-z\u00C0-\u00D6\u00D8-\u00f6\u00f8-\u00ff\s.-]*)$" ErrorText="Algunos Caracteres Invalidos" />
                        <RequiredField IsRequired="true" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTokenBoxColumn FieldName="MetaKeywords" VisibleIndex="6" Width="240">
                <PropertiesTokenBox HelpText="Lista de palabras separadas por comas.">
                </PropertiesTokenBox>
            </dx:GridViewDataTokenBoxColumn>


        </Columns>

        <GroupSummary>
            <dx:ASPxSummaryItem FieldName="ContentTypeID" SummaryType="Count" DisplayFormat="n0" />
        </GroupSummary>
        <TotalSummary>
            <dx:ASPxSummaryItem FieldName="ContentTypeID" SummaryType="Count" DisplayFormat="n0" />
        </TotalSummary>


        <ClientSideEvents ToolbarItemClick="OnToolbarItemClick" />

        <ClientSideEvents RowDblClick="function(s, e) {
        s.StartEditRow(e.visibleIndex);
    }" />

        <Toolbars>

            <dx:GridViewToolbar ItemAlign="Left">
                <Items>
                    <dx:GridViewToolbarItem BeginGroup="true">
                        <Template>
                            <dx:ASPxButtonEdit ID="tbToolbarSearch" runat="server" NullText="Buscar..." Height="100%">
                                <Buttons>
                                    <dx:SpinButtonExtended Image-IconID="find_find_16x16gray" />
                                </Buttons>
                            </dx:ASPxButtonEdit>
                        </Template>
                    </dx:GridViewToolbarItem>

                    <dx:GridViewToolbarItem Text="Exportar" BeginGroup="true" Image-IconID="actions_download_16x16office2013">
                        <Items>
                            <dx:GridViewToolbarItem Command="ExportToPdf" />
                            <dx:GridViewToolbarItem Command="ExportToDocx" />
                            <dx:GridViewToolbarItem Command="ExportToRtf" />
                            <dx:GridViewToolbarItem Command="ExportToCsv" />
                            <dx:GridViewToolbarItem Command="ExportToXls" Text="XLS(DataAware)" />
                            <dx:GridViewToolbarItem Visible="false" Name="CustomExportToXLS" Text="XLS(WYSIWYG)" Image-IconID="export_exporttoxls_16x16office2013" />
                            <dx:GridViewToolbarItem Command="ExportToXlsx" Text="XLSX(DataAware)" />
                            <dx:GridViewToolbarItem Visible="false" Name="CustomExportToXLSX" Text="XLSX(WYSIWYG)" Image-IconID="export_exporttoxlsx_16x16office2013" />
                        </Items>
                    </dx:GridViewToolbarItem>

                </Items>
            </dx:GridViewToolbar>

            <dx:GridViewToolbar ItemAlign="Left">
                <Items>
                    <dx:GridViewToolbarItem Command="New" />
                    <dx:GridViewToolbarItem Command="Edit" />
                    <dx:GridViewToolbarItem Command="Delete" />
                    <dx:GridViewToolbarItem Command="Refresh" BeginGroup="true" DisplayMode="Image" />
                    <dx:GridViewToolbarItem Command="FullExpand" DisplayMode="Image" />
                    <dx:GridViewToolbarItem Command="FullCollapse" DisplayMode="Image" />
                    <dx:GridViewToolbarItem Name="ResetGrid" ToolTip="Resetear Diseño" DisplayMode="Image" Image-IconID="grid_autofittocontent_16x16" />

                </Items>
            </dx:GridViewToolbar>

        </Toolbars>

        <Styles>

            <AlternatingRow Enabled="false" CssClass="gridDataRowAlt"></AlternatingRow>
            <RowHotTrack CssClass="gridDataHover"></RowHotTrack>
            <SelectedRow CssClass="gridDataSelectedRow"></SelectedRow>

            <SearchPanel>
            </SearchPanel>

            <Cell Wrap="True"></Cell>
            <EditForm CssClass="gridEditForm-1Col"></EditForm>

        </Styles>

        <Templates>
            <EmptyDataRow>
                <div class="alert alert-warning" role="alert">No existen registros.</div>
            </EmptyDataRow>
            <%-- 
                <EditForm>
                <div class="gridEditForm">

                    <dx:ContentControl ID="ContentControl1" runat="server">
                        <dx:ASPxGridViewTemplateReplacement ID="Editors" ReplacementType="EditFormEditors"
                            runat="server"></dx:ASPxGridViewTemplateReplacement>
                    </dx:ContentControl>

                </div>
                <hr />
                <div class="gridEditForm">
                    <dx:ASPxValidationSummary ID="ASPxValidationSummary1" runat="server" RenderMode="OrderedList" ShowErrorsInEditors="True"
                        HeaderText="Errores en el Formulario a Corregir" CssClass="panel panel-danger"
                        HeaderStyle-CssClass="panel-heading panel-danger" ValidationGroup='<%# Container.ValidationGroup %>' />
                    <span>
                        <dx:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton" runat="server" />
                    </span>
                    &nbsp;|&nbsp; 
                            <span>
                                <dx:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton" runat="server" />
                            </span>
                </div>
            </EditForm>
            --%>

            <DetailRow>

                <dx:ASPxMemo runat="server" EnableViewState="false" Width="600" Text='<%# Eval("Description") %>' Rows="10" ReadOnly="true">
                </dx:ASPxMemo>

            </DetailRow>

        </Templates>

    </dx:ASPxGridView>

    <asp:SqlDataSource runat="server" ID="DataSourceMaster" OnInit="DBMainDataSources_Init" OnSelecting="DBMainDataSources_Selecting"
        EnableCaching="true" CacheDuration="60" CacheExpirationPolicy="Sliding" ConnectionString='<%$ ConnectionStrings:MsSqlServer.Main %>'
        SelectCommand="SELECT [PageID], [ContentTypeID], [PageStatusID], [PageType], [Category], [SubCategory], [Title], [Description], [MetaKeywords] FROM [CMSPage]"
        DeleteCommand="DELETE FROM [CMSPage] WHERE [PageID] = @PageID"
        InsertCommand="INSERT INTO [CMSPage] ([Category], [SubCategory], [Title], [MetaKeywords], [ContentTypeID], [PageStatusID], [PageType]) 
        SELECT @Category, @SubCategory, @Title, @MetaKeywords, @ContentTypeID, @PageStatusID, ContentType AS PageType FROM CMSContentType WHERE ContentTypeID = @ContentTypeID;"
        UpdateCommand="        
        UPDATE [CMSPage] SET [Category] = @Category, [SubCategory] = @SubCategory, [Title] = @Title, [MetaKeywords] = @MetaKeywords, [ContentTypeID] = @ContentTypeID, [PageStatusID] = @PageStatusID, [PageType] = [ContentType] 
        FROM CMSContentType   
        WHERE [PageID] = @PageID AND [CMSContentType].ContentTypeID = @ContentTypeID;">
        <DeleteParameters>
            <asp:Parameter Name="PageID" Type="Int32"></asp:Parameter>
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="ContentTypeID" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="PageStatusID" Type="Byte"></asp:Parameter>
            <asp:Parameter Name="Category" Type="String"></asp:Parameter>
            <asp:Parameter Name="SubCategory" Type="String"></asp:Parameter>
            <asp:Parameter Name="Title" Type="String"></asp:Parameter>
            <asp:Parameter Name="MetaKeywords" Type="String"></asp:Parameter>

        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="ContentTypeID" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="PageStatusID" Type="Byte"></asp:Parameter>
            <asp:Parameter Name="Category" Type="String"></asp:Parameter>
            <asp:Parameter Name="SubCategory" Type="String"></asp:Parameter>
            <asp:Parameter Name="Title" Type="String"></asp:Parameter>
            <asp:Parameter Name="MetaKeywords" Type="String"></asp:Parameter>
            <asp:Parameter Name="PageID" Type="Int32"></asp:Parameter>
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource runat="server" ID="SqlDataSourceContentType" OnInit="DBMainDataSources_Init"
        OnSelecting="DBMainDataSources_Selecting"
        EnableCaching="true" CacheDuration="60" CacheExpirationPolicy="Sliding"
        ConnectionString='<%$ ConnectionStrings:MsSqlServer.Main %>'
        SelectCommand="
SELECT [ContentTypeID]
      ,[ContentType]
  FROM [CMSContentType]
WHERE
[Active] = 1
ORDER BY [DisplayOrder], ContentType
        "></asp:SqlDataSource>


    <asp:SqlDataSource runat="server" ID="SqlDataSourcePageStatus" OnInit="DBMainDataSources_Init"
        OnSelecting="DBMainDataSources_Selecting"
        EnableCaching="true" CacheDuration="60" CacheExpirationPolicy="Sliding"
        ConnectionString='<%$ ConnectionStrings:MsSqlServer.Main %>'
        SelectCommand="
SELECT [PageStatusID]
      ,[PageStatus]
  FROM [CMSPageStatus]
WHERE
[Active] = 1
ORDER BY [DisplayOrder], PageStatus
        "></asp:SqlDataSource>


</asp:Content>
