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


<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/AdminMaster.master" AutoEventWireup="true" CodeBehind="ToolsImages.aspx.cs" Inherits="XavierCMSWeb.Admin.ToolsImages" Async="true" %>
<%@ OutputCache CacheProfile="Server" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentBody" runat="server">

    <div class="row">
        <div class="col-md-12">
            <h1>Herramientas de Imagenes</h1>
        
            <div class="card">
                <div class="card-header">
                    Imagenes por defecto
                </div>
                <div class="card-body">

                    <p class="card-text">Copia la imagen por defecto dentro de cada carpeta de imagen correspondiente a la pagina</p>
                    <asp:CheckBox runat="server" ID="CheckOverwriteImagesDefault_1" />
                    <a href="#" class="btn btn-primary">Crear Imagenes</a>

                    <dx:ASPxButton runat="server" ID="ASPxButtonCreateDefaultImages" OnClick="ASPxButtonCreateDefaultImages_Click"></dx:ASPxButton>

                </div>
            </div>

        
        
        
        </div>
    </div>

</asp:Content>
