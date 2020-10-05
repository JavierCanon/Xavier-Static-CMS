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


<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/AdminMaster.master" AutoEventWireup="true" CodeBehind="ToolRenamePage.aspx.cs" Inherits="XavierCMSWeb.Admin.ToolRenamePage" %>

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

            <div class="alert alert-info">
                Con esta utilidad puede cambiar el nombre (título) de una página, moviendo todas las imágenes y archivos a las nuevas carpetas respectivas en el sistema de archivos.
            </div>

            <div class="form-group">

                <dx:ASPxComboBox ID="ASPxComboBoxSearch" runat="server" DataSourceID="SqlDataSourcePages" Caption="Página"
                    HelpText="Seleccione la página a cambiar el nombre"
                    ValueField="PageID" TextField="PageID" ValueType="System.Int32" Width="100%" 
                    SettingsAdaptivity-Mode="OnWindowInnerWidth" 
                    ForceDataBinding="True">
                    <Columns>
                        <dx:ListBoxColumn FieldName="PageID" Width="100"></dx:ListBoxColumn>
                        <dx:ListBoxColumn FieldName="Title" Width="300"></dx:ListBoxColumn>
                        <dx:ListBoxColumn FieldName="PageType" Width="200"></dx:ListBoxColumn>
                        <dx:ListBoxColumn FieldName="Category" Width="200"></dx:ListBoxColumn>
                        <dx:ListBoxColumn FieldName="SubCategory" Width="200"></dx:ListBoxColumn>
                    </Columns>
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxComboBox>

            </div>

            <div class="form-group">
                <label>Nuevo Título Página</label>
                <dx:ASPxTextBox ID="TitleTextBox" runat="server" Width="600">
                    <ValidationSettings ErrorDisplayMode="ImageWithText">
                        <RequiredField IsRequired="true" />
                        <RegularExpression ValidationExpression="^([0-9A-Za-z\u00C0-\u00D6\u00D8-\u00f6\u00f8-\u00ff\s.-]*)$" ErrorText="Algunos Caracteres Invalidos" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
                <small class="text-muted">Se crea un subdirectorio dentro de la subcategoria con este valor, evite tildes y ñ para compatibilidad con diferentes servidores y navegadores, recomendable solo letras y números, guiones (-) o espacios ( ).</small>
            </div>

            <div class="form-group">
                <label>Eliminar directorios antiguos</label>
                <dx:ASPxCheckBox runat="server" ID="CheckDeleteDirectories" ValueType="System.Boolean"></dx:ASPxCheckBox>

            </div>

            <div class="form-group">
                <dx:ASPxButton runat="server" ID="btnGenerate" Text="Guardar" CssClass="btn btn-success m-2"
                    UseSubmitBehavior="true" AutoPostBack="false" CausesValidation="true"
                    OnClick="btnGenerate_Click">
                    <ClientSideEvents Click="function(s, e) { 
                                                                ASPxClientEdit.ValidateGroup();

                                                                if(ASPxClientEdit.AreEditorsValid()) {                                                                                                                                
                                                                    e.processOnServer = true;
                                                                }else{
                                                                    alert('Por favor corrija los errores en el formulario.');
                                                                    e.processOnServer = false;
                                                                }
                    
                    }" />

                </dx:ASPxButton>
            </div>

        </div>
    </div>


    <asp:SqlDataSource runat="server" ID="SqlDataSourceMaster" OnInit="DBMainDataSources_Init"
        OnSelecting="DBMainDataSources_Selecting"
        EnableCaching="true" CacheDuration="60" ConnectionString='<%$ ConnectionStrings:MsSqlServer.Main %>'
        SelectCommand="SELECT 
        [PageID],
        [ContentTypeID],
        [PageStatusID],
        [MetaKeywords],
        [MetaDescription],
        [MetaAutor],
        [MetaCopyright],
        [PageType],
        [Category],
        [SubCategory],
        [Title],
        [Description],
        [Body],
        [Notes],
        URL
        FROM [CMSPage]
        WHERE 
        [PageID] = @PageID"
        UpdateCommand="UPDATE [CMSPage]  SET 
        [Title] = @Title
        WHERE 
        [PageID] = @PageID">

        <UpdateParameters>
            <asp:Parameter Name="Title" Type="String"></asp:Parameter>
            <asp:Parameter Name="PageID" Type="Int32"></asp:Parameter>
        </UpdateParameters>

        <SelectParameters>
            <asp:Parameter Name="PageID" Type="Int32"></asp:Parameter>
        </SelectParameters>

    </asp:SqlDataSource>

    <asp:SqlDataSource runat="server" ID="SqlDataSourcePages" OnInit="DBMainDataSources_Init"
        OnSelecting="DBMainDataSources_Selecting"
        EnableCaching="true" CacheDuration="60" CacheExpirationPolicy="Sliding"
        ConnectionString='<%$ ConnectionStrings:MsSqlServer.Main %>'
        SelectCommand="
SELECT 
       [PageID]
      ,PageType
      ,[Category]
      ,[SubCategory]
      ,[Title]
  FROM [CMSPage];
        "></asp:SqlDataSource>

</asp:Content>
