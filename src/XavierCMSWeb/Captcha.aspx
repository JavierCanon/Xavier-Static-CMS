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


<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/PagesMaster.Master" AutoEventWireup="true" CodeBehind="Captcha.aspx.cs" Inherits="XavierCMSWeb.Captcha" %>
<%@ OutputCache CacheProfile="Server" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">

    <meta http-equiv="refresh" content="120" />
    <script src="https://www.google.com/recaptcha/api.js" async="async" defer="defer"></script>

    <style>
        .g-recaptcha {
            width: 100% !important;
        }

        iframe {
            /* recaptcha fix */
            width: 480px !important;
            height: 700px !important;
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentMain" runat="server">

    <div class="container">

        <div class="row">

            <div class="col-12">


                <div id="formlogin1">


                    <div class="text-center">
                        <div class="card">
                            <h3 class="card-header">Inicio de Sesión</h3>
                            <div class="card-body text-left">
                                <p>No parece actividad humana, si no de un bot o robot, por favor resuelva el siguiente Chaptcha para continuar.</p>
                                <p>Si sigue teniendo problemas, por favor contacte a servicio al cliente.</p>
                            </div>
                        </div>
                    </div>

                    <asp:Literal runat="server" ID="recaptcha" EnableViewState="false"></asp:Literal>

                    <hr />
                    <div runat="server" id="divMessage" class="alert alert-danger" visible="false"></div>

                    <dx:BootstrapButton ID="BootstrapButtonLogin" runat="server" ClientInstanceName="btnLogin" Text="Iniciar Sesión" CausesValidation="true" ClientVisible="true"
                        OnClick="BootstrapButtonLogin_Click" SettingsBootstrap-Sizing="Large" SettingsBootstrap-RenderOption="Primary" CssClasses-Control="btn-block">
                        <ClientSideEvents Click="function(s, e) { 
                                                                    LoadingPanel.SetText('Enviando datos al servidor...');
                                                                    LoadingPanel.Show();
                                                                    e.processOnServer = true;                    
                    }" />

                    </dx:BootstrapButton>


                </div>

            </div>

        </div>

    </div>

    <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="true" ContainerElementID="formlogin1" />

</asp:Content>

