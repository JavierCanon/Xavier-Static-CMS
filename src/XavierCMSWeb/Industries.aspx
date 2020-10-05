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


<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/PagesMaster.Master" AutoEventWireup="true" CodeBehind="Industries.aspx.cs" Inherits="XavierCMSWeb.Industries" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentHeadTop" runat="server">
    <meta name="description" content="Soluciones de pesaje industrial" />
    <meta name="keywords" content="balanzas,pesaje,mettler toledo" />
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
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="ContentMain" runat="server">

    <section>
        <div class="container-fluid">
            <div class="row">

                <h1 class="pl-4">Industrias</h1>

                <div class="col-md-12">

                    <dx:BootstrapCardView runat="server" ID="BootstrapCardView1" DataSourceID="SqlDataSourceMaster"
                        AutoGenerateColumns="False" KeyFieldName="PageID">
                        <Columns>
                            <dx:BootstrapCardViewTextColumn FieldName="PageID" ReadOnly="True" Visible="False">
                                <Settings AllowSort="False" AllowHeaderFilter="False" />
                            </dx:BootstrapCardViewTextColumn>
                            <dx:BootstrapCardViewTextColumn FieldName="Category" Caption="Categoria" VisibleIndex="0">
                                <SettingsHeaderFilter Mode="CheckedList" />
                            </dx:BootstrapCardViewTextColumn>
                            <dx:BootstrapCardViewTextColumn FieldName="SubCategory" Caption="Subcategoria" VisibleIndex="1">
                                <SettingsHeaderFilter Mode="CheckedList" />
                            </dx:BootstrapCardViewTextColumn>
                            <dx:BootstrapCardViewTextColumn FieldName="Title" Caption="Producto" VisibleIndex="2">
                                <Settings AllowSort="False" AllowHeaderFilter="False" />
                            </dx:BootstrapCardViewTextColumn>
                            <dx:BootstrapCardViewTextColumn FieldName="Description" VisibleIndex="3">
                                <Settings AllowSort="False" AllowHeaderFilter="False" />
                            </dx:BootstrapCardViewTextColumn>
                            <dx:BootstrapCardViewTextColumn FieldName="MetaKeywords" Caption="Tags" VisibleIndex="4">
                                <Settings AllowSort="False" AllowHeaderFilter="True" />
                                <SettingsHeaderFilter Mode="CheckedList"></SettingsHeaderFilter>
                            </dx:BootstrapCardViewTextColumn>
                        </Columns>

                        <Templates>

                            <CardHeader>

                                <a href="/industria/<%# Server.UrlEncode(Eval("Category").ToString()) + "/" + Server.UrlEncode(Eval("SubCategory").ToString()) +"/"+ Server.UrlEncode(Eval("Title").ToString()) %>">
                                    <img src="<%# GetImage(Eval("Category").ToString(),Eval("SubCategory").ToString(), Eval("Title").ToString() ) %>" class="card-img-top shadow-sm mb-3 bg-white rounded" alt="<%# Eval("Title").ToString().ToUpper() %>">
                                    <div><i class="icon mdi mdi-link"></i><%# Eval("Title").ToString().ToUpper() %></div>
                                </a>

                            </CardHeader>

                            <CardFooter>
                                <a href="/industria/<%# Server.UrlEncode(Eval("Category").ToString()) + "/" + Server.UrlEncode(Eval("SubCategory").ToString()) +"/"+ Server.UrlEncode(Eval("Title").ToString()) %>"><i class="icon mdi mdi-link"></i><%# Eval("Title").ToString().ToUpper() %></a>
                            </CardFooter>

                            <Card>
                                <%# Eval("Description") %>
                            </Card>

                        </Templates>

                        <CssClasses Card="cardview-card-body" CardHeader="cardview-card-header" />

                        <CardLayoutProperties EncodeHtml="False">
                        </CardLayoutProperties>

                        <SettingsSearchPanel Visible="true" ShowApplyButton="true" ShowClearButton="false" />
                        <Settings ShowHeaderFilterButton="false" ShowHeaderPanel="false" ShowStatusBar="Visible" ShowCardHeader="True" ShowCardFooter="True" />
                        <SettingsPager SEOFriendly="Enabled" Mode="ShowPager" EnableAdaptivity="True"></SettingsPager>
                        <SettingsLayout />
                        <SettingsBehavior EncodeErrorHtml="False" />
                        <SettingsBootstrap />
                        <SettingsText />

                    </dx:BootstrapCardView>

                </div>
            </div>
        </div>
    </section>

    <asp:SqlDataSource runat="server" ID="SqlDataSourceMaster" EnableCaching="true" CacheDuration="60" CacheExpirationPolicy="Sliding"
        ConnectionString='<%$ ConnectionStrings:MsSqlServer.Main %>'
        SelectCommand="SELECT [PageID], [MetaKeywords], [Category], [SubCategory], [Title], [Description] FROM [CMSPage] WHERE PageStatusID = 2 AND ContentTypeID = 12;"></asp:SqlDataSource>


</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="ContentFooter" runat="server">
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderID="ContentBodyBottom" runat="server">
</asp:Content>
<asp:Content ID="Content11" ContentPlaceHolderID="ContentBodyJS" runat="server">
</asp:Content>
