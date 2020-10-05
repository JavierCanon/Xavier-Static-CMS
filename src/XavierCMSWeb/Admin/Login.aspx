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


<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Templates/PagesMaster.master" CodeBehind="Login.aspx.cs" Inherits="XavierCMSWeb.Admin.Login" %>
<%@ OutputCache CacheProfile="Server" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">

    <meta http-equiv="refresh" content="120" />

    <script src="https://www.google.com/recaptcha/api.js?render=<%= GOOGLE_SITE_KEY %>" async="async" defer="defer"></script>
    <script>

        var GOOGLE_SITE_KEY = "<%= GOOGLE_SITE_KEY %>";

        window.onload = (function () {

            grecaptcha.execute(GOOGLE_SITE_KEY, { action: 'login' })
                .then(function (token) {
                    $("#recaptchaUserValue").val(token);
                });

            btnLogin.SetVisible(true);
        });

    </script>

</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="ContentMain" runat="server">

    <div class="container">

        <div class="row">
            <div class="col-md-12">

                <h3>Admin Login</h3>

                <dx:BootstrapFormLayout runat="server" ID="BootstrapFormLayout1" LayoutType="Vertical">
                    <Items>

                        <dx:BootstrapLayoutItem Caption="Username" ColSpanLg="4" ColSpanMd="12">
                            <ContentCollection>
                                <dx:ContentControl>
                                    <dx:BootstrapTextBox ID="UsernameTextbox" runat="server" NullText="Username">

                                        <ValidationSettings ErrorDisplayMode="ImageWithText">
                                            <RequiredField IsRequired="true" />
                                        </ValidationSettings>

                                    </dx:BootstrapTextBox>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:BootstrapLayoutItem>

                        <dx:BootstrapLayoutItem Caption="Password" ColSpanLg="4" ColSpanMd="12">
                            <ContentCollection>
                                <dx:ContentControl>
                                    <dx:BootstrapTextBox ID="PasswordTextbox" runat="server" NullText="Password" Password="true">

                                        <ValidationSettings ValidateOnLeave="true" CausesValidation="True">
                                            <RequiredField IsRequired="true" ErrorText="Password, Valor requerido" />
                                            <RegularExpression ErrorText="El password no cumple los requisitos minimos de seguridad."
                                                ValidationExpression="^(?=.{8,})(?=.*[a-z])(?=.*[A-Z])(?!.*\s).*$" />
                                        </ValidationSettings>

                                    </dx:BootstrapTextBox>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:BootstrapLayoutItem>

                        <dx:BootstrapLayoutItem ShowCaption="False">
                            <ContentCollection>
                                <dx:ContentControl>
                                    <asp:Label ID="Msg" ForeColor="maroon" runat="server" CssClass="alert" /><br />

                                    <dx:BootstrapButton ID="ASPxButtonLogin" runat="server" ClientInstanceName="btnLogin" Text="Iniciar Sesión" CausesValidation="true"
                                        OnClick="ASPxButtonLogin_Click" SettingsBootstrap-Sizing="Large" SettingsBootstrap-RenderOption="Primary" CssClasses-Control="btn-block">
                                        <ClientSideEvents Click="function(s, e) { 
                                                                ASPxClientEdit.ValidateGroup(null);

                                                                if(ASPxClientEdit.AreEditorsValid()) {                                                                                                                                
                                                                    LoadingPanel.SetText('Enviando datos al servidor...');
                                                                    LoadingPanel.Show();
                                                                }else{
                                                                    e.processOnServer = false;
                                                                }
                    
                    }" />

                                    </dx:BootstrapButton>

                                    
                                    <%-- 
                                    <asp:CheckBox ID="NotPublicCheckBox" runat="server" />
                                    Save login, check here if this is <span style="text-decoration: underline">NOT</span> a public computer.
                                    --%>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:BootstrapLayoutItem>
                    </Items>
                </dx:BootstrapFormLayout>


            </div>
        </div>
    </div>

    <asp:HiddenField runat="server" ID="recaptchaUserValue" EnableViewState="false" ClientIDMode="Static" />
    <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="true" ContainerElementID="BootstrapFormLayout1" />

</asp:Content>
