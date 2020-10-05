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


<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/AdminMaster.master" AutoEventWireup="true" CodeBehind="Files.aspx.cs" Inherits="XavierCMSWeb.Admin.Files" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentBody" runat="server">
    <%-- NO WORK WITH CACHE ENABLE WHEN CREATE FOLDERS AND UPDATE FILES --%>
    <div class="row">
        <div class="col-md-12">
            <h1>Editor HTML</h1>
            <div class="form-text text-muted">
                El límite de tamaño de subida de archivos es de 20 megas. Extensiones permitidas: .pdf,.xlsx,.docx,.png,.gif,.jpg,.webp,.mp4,.ogg.
            </div>

            <dx:ASPxFileManager ID="fileManager" runat="server" Width="100%" Height="600px">

                <Settings ThumbnailFolder="~/Thumbs/FileManager" RootFolder="~/Content" InitialFolder="~/Content" 
				AllowedFileExtensions=".pdf,.xlsx,.docx,.png,.gif,.jpg,.webp,.mp4,.ogg" EnableClientSideItemMetadata="True" />

                <SettingsFileList View="Details" ShowFolders="False" ShowParentFolder="False">
                    <DetailsViewSettings AllowColumnResize="true" AllowColumnDragDrop="true" AllowColumnSort="true" ShowHeaderFilterButton="True">
                        <Columns>
                            <dx:FileManagerDetailsColumn FileInfoType="Thumbnail" Caption=" " VisibleIndex="0"></dx:FileManagerDetailsColumn>
                            <dx:FileManagerDetailsColumn Caption="Name" VisibleIndex="1"></dx:FileManagerDetailsColumn>
                            <dx:FileManagerDetailsColumn FileInfoType="Size" Caption="Size" VisibleIndex="2"></dx:FileManagerDetailsColumn>
                            <dx:FileManagerDetailsColumn FileInfoType="LastWriteTime" Caption="Date modified" VisibleIndex="3"></dx:FileManagerDetailsColumn>
                            <dx:FileManagerDetailsColumn FileInfoType="Location" Caption="Location" VisibleIndex="4"></dx:FileManagerDetailsColumn>

                        </Columns>
                    </DetailsViewSettings>
                </SettingsFileList>

                <SettingsFolders EnableCallBacks="True" />

                <SettingsEditing AllowCreate="True" AllowRename="True" AllowMove="True" AllowDownload="True" AllowDelete="True"></SettingsEditing>

                <SettingsToolbar>
                    <Items>
                        <dx:FileManagerToolbarCreateButton ToolTip="Create (F7)"></dx:FileManagerToolbarCreateButton>
                        <dx:FileManagerToolbarRenameButton ToolTip="Rename (F2)"></dx:FileManagerToolbarRenameButton>
                        <dx:FileManagerToolbarMoveButton ToolTip="Move (F6)"></dx:FileManagerToolbarMoveButton>
                        <dx:FileManagerToolbarCopyButton ToolTip="Copy"></dx:FileManagerToolbarCopyButton>
                        <dx:FileManagerToolbarDeleteButton ToolTip="Delete (Del)"></dx:FileManagerToolbarDeleteButton>
                        <dx:FileManagerToolbarRefreshButton ToolTip="Refresh"></dx:FileManagerToolbarRefreshButton>
                        <dx:FileManagerToolbarDownloadButton ToolTip="Download"></dx:FileManagerToolbarDownloadButton>
                        <dx:FileManagerToolbarUploadButton ToolTip="Upload"></dx:FileManagerToolbarUploadButton>
                    </Items>
                </SettingsToolbar>

                <SettingsContextMenu>
                    <Items>
                        <dx:FileManagerToolbarCreateButton ToolTip="Create (F7)"></dx:FileManagerToolbarCreateButton>
                        <dx:FileManagerToolbarRenameButton ToolTip="Rename (F2)"></dx:FileManagerToolbarRenameButton>
                        <dx:FileManagerToolbarMoveButton ToolTip="Move (F6)"></dx:FileManagerToolbarMoveButton>
                        <dx:FileManagerToolbarCopyButton ToolTip="Copy"></dx:FileManagerToolbarCopyButton>
                        <dx:FileManagerToolbarDeleteButton ToolTip="Delete (Del)"></dx:FileManagerToolbarDeleteButton>
                        <dx:FileManagerToolbarRefreshButton ToolTip="Refresh"></dx:FileManagerToolbarRefreshButton>
                        <dx:FileManagerToolbarDownloadButton ToolTip="Download"></dx:FileManagerToolbarDownloadButton>
                        <dx:FileManagerToolbarUploadButton ToolTip="Upload"></dx:FileManagerToolbarUploadButton>
                    </Items>
                </SettingsContextMenu>

                <SettingsUpload Enabled="True" UseAdvancedUploadMode="True">
                    <AdvancedModeSettings EnableClientAccessRuleValidation="True" EnableMultiSelect="True"></AdvancedModeSettings>
                    <ValidationSettings MaxFileSize="20971520">
                    </ValidationSettings>
                </SettingsUpload>

                <SettingsAdaptivity Enabled="true" />

            </dx:ASPxFileManager>


        </div>

    </div>


</asp:Content>
