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


<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/PagesMaster.Master" AutoEventWireup="true" CodeBehind="Resource.aspx.cs" Inherits="XavierCMSWeb.Resource" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentHeadTop" runat="server">

    <meta name="description" content="<%# PageMetaDescription %>" />
    <meta name="keywords" content="<%# PageMetaKeywords %>" />
    <meta name="author" content="Precisur" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHeadCSS" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentHeadJS" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentBodyTop" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ContentHeader" runat="server">

        <div class="row">
        <div class="col-md-12">

            <nav aria-label="breadcrumb">
                <ol class="breadcrumb" runat="server" id="Breadcrum" enableviewstate="false">
                    <li class="breadcrumb-item"><a href="/">INICIO</a></li>
                </ol>
            </nav>

        </div>
    </div>

</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="ContentMain" runat="server">

    <section>
        <div class="container-fluid">
            <div class="row">
                <h1 class="mx-auto"><%: PageTitle %></h1>

                <div class="col-md-12 pt-5 pagebody">

                    <%= PageBody %>
                </div>

                <div class="col-md-12 pt-5">

                    <a href="/FormularioContacto.aspx" class="btn btn-success btn-lg"><i class="mdi mdi-36px mdi-account-multiple"></i>Consultar </a>

                </div>

                <div class="col-md-12 mt-5">
                    <h2>Descargas <i class="icon icon-md-smaller icon-gray-light mdi mdi-download"></i></h2>
                    <div class="form-text text-muted">Seleccione el item y luego haga click en el botón descargar.</div>

                    <div class="alert alert-warning" role="alert" runat="server" id="warninglogin" visible="false">
                        <h4 class="alert-heading"><i class="icon mdi mdi-alert-circle"></i>Registro</h4>
                        <p>Debe de ser un <b>usuario registrado</b> para poder realizar las descargas.</p>
                        <hr />
                        <a class="btn btn-lg btn-secondary text-light" href="/FormularioRegistro.aspx">Registrarse</a> | <a class="btn btn-lg btn-primary text-light" href="/UserLogin.aspx">Iniciar Sesión</a>
                    </div>

                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false"
                        CssClass="table table-striped" ShowHeader="false"
                        EmptyDataText="No existen archivos...">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HyperLink runat="server" CssClass="mdi mdi-18px mdi-file-pdf-box" Text='<%# Eval("Text") %>' NavigateUrl='<%# Eval("Value") %>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                </div>


            </div>
        </div>
    </section>

    <asp:SqlDataSource runat="server" ID="SqlDataSourceMaster" EnableCaching="true" CacheDuration="60" CacheExpirationPolicy="Sliding"
        ConnectionString='<%$ ConnectionStrings:MsSqlServer.Main %>' CancelSelectOnNullParameter="false"
        SelectCommand="SELECT [PageID], [Category], [SubCategory], [Title], [MetaKeywords], [MetaDescription], [Description], [Body]
        FROM [CMSPage] 
        WHERE 
        PageStatusID = 2
        AND ContentTypeID = 10
        AND Category = @Category 
        AND SubCategory = @SubCategory
        AND Title = @Title
        ;">

        <SelectParameters>
            <asp:QueryStringParameter Name="Category" QueryStringField="cat" />
            <asp:QueryStringParameter Name="SubCategory" QueryStringField="sub" />
            <asp:QueryStringParameter Name="Title" QueryStringField="pag" />

        </SelectParameters>
    </asp:SqlDataSource>

</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="ContentFooter" runat="server">
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderID="ContentBodyBottom" runat="server">
</asp:Content>
<asp:Content ID="Content11" ContentPlaceHolderID="ContentBodyJS" runat="server">
</asp:Content>
