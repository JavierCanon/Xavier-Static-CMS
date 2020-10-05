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


<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/AdminMaster.master" AutoEventWireup="true" CodeBehind="ToolGenerateXMLSitemap.aspx.cs" Inherits="XavierCMSWeb.Admin.ToolGenerateXMLSitemap" %>

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
                Esta utilidad SEO, genera y sobre-escribe el archivo sitemap.xml sobre las páginas de este sitio web.
            </div>

            <dx:BootstrapButton runat="server" ID="btnGenerate" OnClick="btnGenerate_Click"
                Text="Generar Sitemap" SettingsBootstrap-RenderOption="Primary" SettingsBootstrap-Sizing="Large" ></dx:BootstrapButton>

        </div>
    </div>

</asp:Content>
