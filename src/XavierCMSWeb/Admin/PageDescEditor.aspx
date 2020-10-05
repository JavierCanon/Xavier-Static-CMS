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


<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/AdminMaster.master" AutoEventWireup="true" CodeBehind="PageDescEditor.aspx.cs" Inherits="XavierCMSWeb.Admin.PageDescEditor" %>
<%@ OutputCache CacheProfile="Server" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v20.1, Version=20.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxSpellChecker" Assembly="DevExpress.Web.ASPxSpellChecker.v20.1, Version=20.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentBody" runat="server">

        <div class="modal fade" id="pageToastMsg" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-header bg-warning">
                    <h5 class="modal-title">Atención</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div runat="server" id="divMessage" visible="false"></div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Aceptar</button>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <h1>Editor HTML</h1>

            <dx:ASPxComboBox ID="ASPxComboBox1" runat="server" DataSourceID="SqlDataSourcePages" Caption="Página"
                HelpText="Límite de tamaño de archivos es 20 megas."
                ValueField="PageID" ValueType="System.Int32" Width="100%">
                <Columns>
                    <dx:ListBoxColumn FieldName="PageID" Width="100"></dx:ListBoxColumn>
                    <dx:ListBoxColumn FieldName="Category" Width="200"></dx:ListBoxColumn>
                    <dx:ListBoxColumn FieldName="SubCategory" Width="200"></dx:ListBoxColumn>
                    <dx:ListBoxColumn FieldName="Title" Width="300"></dx:ListBoxColumn>
                </Columns>
                <ValidationSettings CausesValidation="True">
                    <RequiredField IsRequired="True" />
                </ValidationSettings>
            </dx:ASPxComboBox>

            <div runat="server" id="MsgDiv" class="alert alert-warning" visible="false"></div>

            <dx:ASPxButton runat="server" ID="ASPxButtonLoad" Text="Cargar" CssClass="btn btn-primary m-2" OnClick="ASPxButtonLoad_Click">
            </dx:ASPxButton>
            
            <dx:ASPxButton runat="server" ID="ASPxButtonSave" Text="Guardar" CssClass="btn btn-success m-2"  
                OnClick="ASPxButtonSave_Click"></dx:ASPxButton>

        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <dx:ASPxHtmlEditor runat="server" ID="HtmlEditor1" Height="800px" Width="100%" ToolbarMode="Ribbon">
                <RibbonTabs>
                    <dx:HEHomeRibbonTab>
                        <Groups>
                            <dx:HEUndoRibbonGroup>
                                <Items>
                                    <dx:HEUndoRibbonCommand Size="Large"></dx:HEUndoRibbonCommand>
                                    <dx:HERedoRibbonCommand Size="Large"></dx:HERedoRibbonCommand>
                                </Items>
                            </dx:HEUndoRibbonGroup>
                            <dx:HEClipboardRibbonGroup>
                                <Items>
                                    <dx:HEPasteFromWordRibbonCommand Size="Large"></dx:HEPasteFromWordRibbonCommand>
                                    <dx:HECutSelectionRibbonCommand></dx:HECutSelectionRibbonCommand>
                                    <dx:HECopySelectionRibbonCommand></dx:HECopySelectionRibbonCommand>
                                    <dx:HEPasteSelectionRibbonCommand></dx:HEPasteSelectionRibbonCommand>
                                </Items>
                            </dx:HEClipboardRibbonGroup>
                            <dx:HEFontRibbonGroup>
                                <Items>
                                    <dx:HEFontNameRibbonCommand>
                                        <Items>
                                            <dx:ListEditItem Text="Times New Roman" Value="Times New Roman"></dx:ListEditItem>
                                            <dx:ListEditItem Text="Tahoma" Value="Tahoma"></dx:ListEditItem>
                                            <dx:ListEditItem Text="Verdana" Value="Verdana"></dx:ListEditItem>
                                            <dx:ListEditItem Text="Arial" Value="Arial"></dx:ListEditItem>
                                            <dx:ListEditItem Text="MS Sans Serif" Value="MS Sans Serif"></dx:ListEditItem>
                                            <dx:ListEditItem Text="Courier" Value="Courier"></dx:ListEditItem>
                                            <dx:ListEditItem Text="Segoe UI" Value="Segoe UI"></dx:ListEditItem>
                                        </Items>

                                        <PropertiesComboBox NullText="(Font Name)" IncrementalFilteringMode="None" Width="160px"></PropertiesComboBox>
                                    </dx:HEFontNameRibbonCommand>
                                    <dx:HEFontSizeRibbonCommand>
                                        <Items>
                                            <dx:ListEditItem Text="1 (8pt)" Value="1"></dx:ListEditItem>
                                            <dx:ListEditItem Text="2 (10pt)" Value="2"></dx:ListEditItem>
                                            <dx:ListEditItem Text="3 (12pt)" Value="3"></dx:ListEditItem>
                                            <dx:ListEditItem Text="4 (14pt)" Value="4"></dx:ListEditItem>
                                            <dx:ListEditItem Text="5 (18pt)" Value="5"></dx:ListEditItem>
                                            <dx:ListEditItem Text="6 (24pt)" Value="6"></dx:ListEditItem>
                                            <dx:ListEditItem Text="7 (36pt)" Value="7"></dx:ListEditItem>
                                        </Items>

                                        <PropertiesComboBox NullText="(Font Size)" IncrementalFilteringMode="None" Width="100px"></PropertiesComboBox>
                                    </dx:HEFontSizeRibbonCommand>
                                    <dx:HEBackColorRibbonCommand></dx:HEBackColorRibbonCommand>
                                    <dx:HEFontColorRibbonCommand></dx:HEFontColorRibbonCommand>
                                    <dx:HERemoveFormatRibbonCommand></dx:HERemoveFormatRibbonCommand>
                                    <dx:HEParagraphFormattingRibbonCommand>
                                        <Items>
                                            <dx:ListEditItem Text="Normal" Value="p"></dx:ListEditItem>
                                            <dx:ListEditItem Text="Heading  1" Value="h1"></dx:ListEditItem>
                                            <dx:ListEditItem Text="Heading  2" Value="h2"></dx:ListEditItem>
                                            <dx:ListEditItem Text="Heading  3" Value="h3"></dx:ListEditItem>
                                            <dx:ListEditItem Text="Heading  4" Value="h4"></dx:ListEditItem>
                                            <dx:ListEditItem Text="Heading  5" Value="h5"></dx:ListEditItem>
                                            <dx:ListEditItem Text="Heading  6" Value="h6"></dx:ListEditItem>
                                            <dx:ListEditItem Text="Address" Value="address"></dx:ListEditItem>
                                            <dx:ListEditItem Text="Normal (DIV)" Value="div"></dx:ListEditItem>
                                        </Items>

                                        <PropertiesComboBox NullText="(Paragraph)" IncrementalFilteringMode="None" Width="120px"></PropertiesComboBox>
                                    </dx:HEParagraphFormattingRibbonCommand>
                                    <dx:HEBoldRibbonCommand></dx:HEBoldRibbonCommand>
                                    <dx:HEItalicRibbonCommand></dx:HEItalicRibbonCommand>
                                    <dx:HEUnderlineRibbonCommand></dx:HEUnderlineRibbonCommand>
                                    <dx:HEStrikeoutRibbonCommand></dx:HEStrikeoutRibbonCommand>
                                    <dx:HESuperscriptRibbonCommand></dx:HESuperscriptRibbonCommand>
                                    <dx:HESubscriptRibbonCommand></dx:HESubscriptRibbonCommand>
                                </Items>
                            </dx:HEFontRibbonGroup>
                            <dx:HEParagraphRibbonGroup>
                                <Items>
                                    <dx:HEInsertUnorderedListRibbonCommand></dx:HEInsertUnorderedListRibbonCommand>
                                    <dx:HEInsertOrderedListRibbonCommand></dx:HEInsertOrderedListRibbonCommand>
                                    <dx:HEOutdentRibbonCommand></dx:HEOutdentRibbonCommand>
                                    <dx:HEIndentRibbonCommand></dx:HEIndentRibbonCommand>
                                    <dx:HEAlignmentLeftRibbonCommand></dx:HEAlignmentLeftRibbonCommand>
                                    <dx:HEAlignmentCenterRibbonCommand></dx:HEAlignmentCenterRibbonCommand>
                                    <dx:HEAlignmentRightRibbonCommand></dx:HEAlignmentRightRibbonCommand>
                                </Items>
                            </dx:HEParagraphRibbonGroup>
                            <dx:HEEditingRibbonGroup>
                                <Items>
                                    <dx:HEFindAndReplaceDialogRibbonCommand Size="Large"></dx:HEFindAndReplaceDialogRibbonCommand>
                                </Items>
                            </dx:HEEditingRibbonGroup>
                        </Groups>
                    </dx:HEHomeRibbonTab>
                    <dx:HEInsertRibbonTab>
                        <Groups>
                            <dx:HEImagesRibbonGroup>
                                <Items>
                                    <dx:HEInsertImageRibbonCommand Size="Large"></dx:HEInsertImageRibbonCommand>
                                </Items>
                            </dx:HEImagesRibbonGroup>
                            <dx:HEMediaRibbonGroup>
                                <Items>
                                    <dx:HEInsertYouTubeVideoDialogRibbonCommand Size="Large"></dx:HEInsertYouTubeVideoDialogRibbonCommand>
                                    <dx:HEInsertFlashDialogRibbonCommand></dx:HEInsertFlashDialogRibbonCommand>
                                    <dx:HEInsertVideoDialogRibbonCommand></dx:HEInsertVideoDialogRibbonCommand>
                                    <dx:HEInsertAudioDialogRibbonCommand></dx:HEInsertAudioDialogRibbonCommand>
                                </Items>
                            </dx:HEMediaRibbonGroup>
                            <dx:HELinksRibbonGroup>
                                <Items>
                                    <dx:HEInsertLinkDialogRibbonCommand Size="Large"></dx:HEInsertLinkDialogRibbonCommand>
                                    <dx:HEUnlinkRibbonCommand Size="Large"></dx:HEUnlinkRibbonCommand>
                                </Items>
                            </dx:HELinksRibbonGroup>
                            <dx:RibbonGroup Text="Placeholders" Name="Placeholders">
                                <Items>
                                    <dx:HEInsertPlaceholderDialogRibbonCommand Size="Large"></dx:HEInsertPlaceholderDialogRibbonCommand>
                                </Items>
                            </dx:RibbonGroup>
                        </Groups>
                    </dx:HEInsertRibbonTab>
                    <dx:HEViewRibbonTab>
                        <Groups>
                            <dx:HEViewsRibbonGroup>
                                <Items>
                                    <dx:HEFullscreenRibbonCommand Size="Large"></dx:HEFullscreenRibbonCommand>
                                </Items>
                            </dx:HEViewsRibbonGroup>
                        </Groups>
                    </dx:HEViewRibbonTab>
                    <dx:RibbonTab Text="Export" Name="Export">
                        <Groups>
                            <dx:RibbonGroup Text="Common" Name="Common">
                                <Items>
                                    <dx:RibbonDropDownButtonItem Text="Export To" Name="Export To" ToolTip="Export To" Size="Large">
                                        <Items>
                                            <dx:HEExportToRtfDropDownRibbonCommand></dx:HEExportToRtfDropDownRibbonCommand>
                                            <dx:HEExportToPdfDropDownRibbonCommand></dx:HEExportToPdfDropDownRibbonCommand>
                                            <dx:HEExportToTxtDropDownRibbonCommand></dx:HEExportToTxtDropDownRibbonCommand>
                                            <dx:HEExportToDocxDropDownRibbonCommand></dx:HEExportToDocxDropDownRibbonCommand>
                                            <dx:HEExportToOdtDropDownRibbonCommand></dx:HEExportToOdtDropDownRibbonCommand>
                                            <dx:HEExportToMhtDropDownRibbonCommand></dx:HEExportToMhtDropDownRibbonCommand>
                                        </Items>

                                        <LargeImage IconID="export_exportfile_32x32"></LargeImage>

                                        <SmallImage IconID="export_exportfile_16x16"></SmallImage>
                                    </dx:RibbonDropDownButtonItem>
                                    <dx:HEPrintRibbonCommand Text="Save As" ToolTip="Save As" Size="Large"></dx:HEPrintRibbonCommand>
                                </Items>
                            </dx:RibbonGroup>
                        </Groups>
                    </dx:RibbonTab>
                </RibbonTabs>
                <RibbonContextTabCategories>
                    <dx:HETableToolsRibbonTabCategory Color="&#39;#d31313&#39;">
                        <Tabs>
                            <dx:HETableLayoutRibbonTab Text="Layout">
                                <Groups>
                                    <dx:HEDeleteTableRibbonGroup>
                                        <Items>
                                            <dx:HEDeleteTableRibbonCommand Size="Large"></dx:HEDeleteTableRibbonCommand>
                                            <dx:HEDeleteTableRowRibbonCommand Size="Large"></dx:HEDeleteTableRowRibbonCommand>
                                            <dx:HEDeleteTableColumnRibbonCommand Size="Large"></dx:HEDeleteTableColumnRibbonCommand>
                                        </Items>
                                    </dx:HEDeleteTableRibbonGroup>
                                    <dx:HEInsertTableRibbonGroup>
                                        <Items>
                                            <dx:HEInsertTableRowAboveRibbonCommand Size="Large"></dx:HEInsertTableRowAboveRibbonCommand>
                                            <dx:HEInsertTableRowBelowRibbonCommand Size="Large"></dx:HEInsertTableRowBelowRibbonCommand>
                                            <dx:HEInsertTableColumnToLeftRibbonCommand Size="Large"></dx:HEInsertTableColumnToLeftRibbonCommand>
                                            <dx:HEInsertTableColumnToRightRibbonCommand Size="Large"></dx:HEInsertTableColumnToRightRibbonCommand>
                                        </Items>
                                    </dx:HEInsertTableRibbonGroup>
                                    <dx:HEMergeTableRibbonGroup>
                                        <Items>
                                            <dx:HEMergeTableCellDownRibbonCommand Size="Large"></dx:HEMergeTableCellDownRibbonCommand>
                                            <dx:HEMergeTableCellRightRibbonCommand></dx:HEMergeTableCellRightRibbonCommand>
                                            <dx:HESplitTableCellHorizontallyRibbonCommand></dx:HESplitTableCellHorizontallyRibbonCommand>
                                            <dx:HESplitTableCellVerticallyRibbonCommand></dx:HESplitTableCellVerticallyRibbonCommand>
                                        </Items>
                                    </dx:HEMergeTableRibbonGroup>
                                    <dx:HETablePropertiesRibbonGroup>
                                        <Items>
                                            <dx:HETablePropertiesRibbonCommand Size="Large"></dx:HETablePropertiesRibbonCommand>
                                            <dx:HETableRowPropertiesRibbonCommand></dx:HETableRowPropertiesRibbonCommand>
                                            <dx:HETableColumnPropertiesRibbonCommand></dx:HETableColumnPropertiesRibbonCommand>
                                            <dx:HETableCellPropertiesRibbonCommand></dx:HETableCellPropertiesRibbonCommand>
                                        </Items>
                                    </dx:HETablePropertiesRibbonGroup>
                                </Groups>
                            </dx:HETableLayoutRibbonTab>
                        </Tabs>
                    </dx:HETableToolsRibbonTabCategory>
                </RibbonContextTabCategories>
                <ContextMenuItems>
                    <dx:HECutContextMenuItem BeginGroup="True" VisibleIndex="0"></dx:HECutContextMenuItem>
                    <dx:HECopyContextMenuItem VisibleIndex="1"></dx:HECopyContextMenuItem>
                    <dx:HEPasteContextMenuItem VisibleIndex="2"></dx:HEPasteContextMenuItem>
                    <dx:HEChangePlaceholderDialogContextMenuItem BeginGroup="True" VisibleIndex="3"></dx:HEChangePlaceholderDialogContextMenuItem>
                    <dx:HEChangeLinkDialogContextMenuItem BeginGroup="True" VisibleIndex="4"></dx:HEChangeLinkDialogContextMenuItem>
                    <dx:HEChangeImageDialogContextMenuItem BeginGroup="True" VisibleIndex="5"></dx:HEChangeImageDialogContextMenuItem>
                    <dx:HEChangeAudioDialogContextMenuItem BeginGroup="True" VisibleIndex="6"></dx:HEChangeAudioDialogContextMenuItem>
                    <dx:HEChangeVideoDialogContextMenuItem BeginGroup="True" VisibleIndex="7"></dx:HEChangeVideoDialogContextMenuItem>
                    <dx:HEChangeFlashDialogContextMenuItem BeginGroup="True" VisibleIndex="8"></dx:HEChangeFlashDialogContextMenuItem>
                    <dx:HEChangeYouTubeVideoDialogContextMenuItem BeginGroup="True" VisibleIndex="9"></dx:HEChangeYouTubeVideoDialogContextMenuItem>
                    <dx:HETablePropertiesDialogContextMenuItem BeginGroup="True" VisibleIndex="10"></dx:HETablePropertiesDialogContextMenuItem>
                    <dx:HETableRowPropertiesDialogContextMenuItem VisibleIndex="11"></dx:HETableRowPropertiesDialogContextMenuItem>
                    <dx:HETableColumnPropertiesDialogContextMenuItem VisibleIndex="12"></dx:HETableColumnPropertiesDialogContextMenuItem>
                    <dx:HETableCellPropertiesDialogContextMenuItem VisibleIndex="13"></dx:HETableCellPropertiesDialogContextMenuItem>
                    <dx:HEInsertTableRowAboveContextMenuItem BeginGroup="True" VisibleIndex="14"></dx:HEInsertTableRowAboveContextMenuItem>
                    <dx:HEInsertTableRowBelowContextMenuItem VisibleIndex="15"></dx:HEInsertTableRowBelowContextMenuItem>
                    <dx:HEInsertTableColumnToLeftContextMenuItem VisibleIndex="16"></dx:HEInsertTableColumnToLeftContextMenuItem>
                    <dx:HEInsertTableColumnToRightContextMenuItem VisibleIndex="17"></dx:HEInsertTableColumnToRightContextMenuItem>
                    <dx:HESplitTableCellHorizontalContextMenuItem BeginGroup="True" VisibleIndex="18"></dx:HESplitTableCellHorizontalContextMenuItem>
                    <dx:HESplitTableCellVerticalContextMenuItem VisibleIndex="19"></dx:HESplitTableCellVerticalContextMenuItem>
                    <dx:HEMergeTableCellRightContextMenuItem VisibleIndex="20"></dx:HEMergeTableCellRightContextMenuItem>
                    <dx:HEMergeTableCellDownContextMenuItem VisibleIndex="21"></dx:HEMergeTableCellDownContextMenuItem>
                    <dx:HEDeleteTableContextMenuItem BeginGroup="True" VisibleIndex="22"></dx:HEDeleteTableContextMenuItem>
                    <dx:HEDeleteTableRowContextMenuItem VisibleIndex="23"></dx:HEDeleteTableRowContextMenuItem>
                    <dx:HEDeleteTableColumnContextMenuItem VisibleIndex="24"></dx:HEDeleteTableColumnContextMenuItem>
                    <dx:HECommentContextMenuItem BeginGroup="True" VisibleIndex="25"></dx:HECommentContextMenuItem>
                    <dx:HEUncommentContextMenuItem VisibleIndex="26"></dx:HEUncommentContextMenuItem>
                    <dx:HECollapseTagContextMenuItem BeginGroup="True" VisibleIndex="27"></dx:HECollapseTagContextMenuItem>
                    <dx:HEExpandTagContextMenuItem VisibleIndex="28"></dx:HEExpandTagContextMenuItem>
                    <dx:HEFormatDocumentContextMenuItem BeginGroup="True" VisibleIndex="29"></dx:HEFormatDocumentContextMenuItem>
                    <dx:HESelectAllContextMenuItem BeginGroup="True" VisibleIndex="30"></dx:HESelectAllContextMenuItem>
                </ContextMenuItems>
                <Placeholders>
                    <dx:HtmlEditorPlaceholderItem Value="Placeholder1"></dx:HtmlEditorPlaceholderItem>
                    <dx:HtmlEditorPlaceholderItem Value="Placeholder2"></dx:HtmlEditorPlaceholderItem>
                    <dx:HtmlEditorPlaceholderItem Value="Placeholder3"></dx:HtmlEditorPlaceholderItem>
                </Placeholders>

                <Settings AllowCustomColorsInColorPickers="True" AllowScriptExecutionInPreview="True" ShowTagInspector="True" AllowSaveBinaryImageToServer="True">
                    <SettingsHtmlView ShowCollapseTagButtons="True" ShowLineNumbers="True" HighlightActiveLine="True" HighlightMatchingTags="True" EnableAutoCompletion="True"></SettingsHtmlView>
                    <SettingsHtmlView HighlightActiveLine="True" />
                    <SettingsRibbonToolbar />
                </Settings>

                <SettingsHtmlEditing EnterMode="BR" PasteMode="MergeFormatting" EnablePasteOptions="True" AllowScripts="True" AllowIFrames="True" AllowFormElements="True" AllowHTML5MediaElements="True" AllowObjectAndEmbedElements="True" AllowYouTubeVideoIFrames="True" AllowEditFullDocument="False">
                </SettingsHtmlEditing>

                <SettingsResize AllowResize="True"></SettingsResize>

                <SettingsValidation>
                    <RequiredField IsRequired="false" />
                </SettingsValidation>

                <SettingsDialogs>

                    <InsertImageDialog>

                        <SettingsImageSelector Enabled="true">
                            <CommonSettings RootFolder="~/Content/Img" InitialFolder="~/Content/Img" ThumbnailFolder="~/Thumbs/Img"></CommonSettings>
                            <EditingSettings AllowRename="True" AllowMove="True" AllowDelete="True" AllowDownload="True"></EditingSettings>
                            <ToolbarSettings ShowPath="True"></ToolbarSettings>
                            <PermissionSettings>
                                <AccessRules>
                                    <dx:FileManagerFolderAccessRule Path="~/Content/Img" Browse="Allow" Edit="Allow" Upload="Allow" />
                                    <dx:FileManagerFolderAccessRule Path="~/Content/Img" Browse="Allow" Edit="Allow" Upload="Allow" />
                                </AccessRules>
                            </PermissionSettings>
                        </SettingsImageSelector>

                        <SettingsImageUpload UseAdvancedUploadMode="False">
                            <FileSystemSettings UploadFolder="~/Content/Img" />
                            <ValidationSettings MaxFileSize="20971520">                                
                            </ValidationSettings>
                        </SettingsImageUpload>


                    </InsertImageDialog>

                    <InsertVideoDialog>

                        <SettingsVideoSelector Enabled="true">
                            <CommonSettings RootFolder="~/Content/Video" InitialFolder="~/Content/Video" ThumbnailFolder="~/Thumbs/Video"></CommonSettings>
                            <FileListSettings View="Details"></FileListSettings>
                            <PermissionSettings>
                                <AccessRules>
                                    <dx:FileManagerFolderAccessRule Path="~/Content/Video" Browse="Allow" Edit="Allow" Upload="Allow" />
                                    <dx:FileManagerFolderAccessRule Path="~/Content/Video" Browse="Allow" Edit="Allow" Upload="Allow" />
                                </AccessRules>
                            </PermissionSettings>
                        </SettingsVideoSelector>

                        <SettingsVideoUpload UseAdvancedUploadMode="False">
                            <FileSystemSettings UploadFolder="~/Content/Video" />
                            <ValidationSettings MaxFileSize="20971520">
                            </ValidationSettings>
                        </SettingsVideoUpload>

                    </InsertVideoDialog>

                    <InsertLinkDialog>

                        <SettingsDocumentSelector Enabled="true">
                            <CommonSettings RootFolder="~/Content/Files" InitialFolder="~/Content/Files" ThumbnailFolder="~/Thumbs/Files"></CommonSettings>
                            <EditingSettings AllowRename="True" AllowMove="True" AllowDelete="True" AllowDownload="True"></EditingSettings>
                            <BreadcrumbsSettings Visible="True"></BreadcrumbsSettings>
                            <PermissionSettings>
                                <AccessRules>
                                    <dx:FileManagerFolderAccessRule Path="~/Content/Files" Browse="Allow" Edit="Allow" Upload="Allow" />
                                    <dx:FileManagerFolderAccessRule Path="~/Content/Files" Browse="Allow" Edit="Allow" Upload="Allow" />
                                </AccessRules>
                            </PermissionSettings>
                        </SettingsDocumentSelector>

                    </InsertLinkDialog>

                </SettingsDialogs>
            </dx:ASPxHtmlEditor>

        </div>
    </div>

    <asp:SqlDataSource runat="server" ID="DataSourceMaster" OnInit="DBMainDataSources_Init"
        OnSelecting="DBMainDataSources_Selecting"
        EnableCaching="true" CacheDuration="60" CacheExpirationPolicy="Absolute" ConnectionString='<%$ ConnectionStrings:MsSqlServer.Main %>'
        SelectCommand="SELECT [PageID], [Description] FROM [CMSPage] WHERE [PageID] = @PageID"
        UpdateCommand="UPDATE [CMSPage] SET [Description] = @Description WHERE [PageID] = @PageID">

        <SelectParameters>
            <asp:Parameter Name="PageID" Type="Int32" />
        </SelectParameters>

        <UpdateParameters>
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="PageID" Type="Int32"></asp:Parameter>
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource runat="server" ID="SqlDataSourcePages" OnInit="DBMainDataSources_Init"
        OnSelecting="DBMainDataSources_Selecting"
        EnableCaching="true" CacheDuration="60" CacheExpirationPolicy="Sliding"
        ConnectionString='<%$ ConnectionStrings:MsSqlServer.Main %>'
        SelectCommand="
SELECT 
       [PageID]
      ,[Category]
      ,[SubCategory]
      ,[Title]
  FROM [CMSPage];
        "></asp:SqlDataSource>


</asp:Content>
