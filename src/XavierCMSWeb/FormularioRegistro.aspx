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


<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/PagesMaster.Master" AutoEventWireup="true" CodeBehind="FormularioContacto.aspx.cs" Inherits="XavierCMSWeb.FormularioRegistro" %>

<%@ OutputCache CacheProfile="Server" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentHeadTop" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHeadCSS" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentHeadJS" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentHead" runat="server">

    <meta http-equiv="refresh" content="120" />

    <script src="https://www.google.com/recaptcha/api.js?render=<%= GOOGLE_SITE_KEY %>" async="async" defer="defer"></script>
    <script>

        GOOGLE_SITE_KEY = "<%= GOOGLE_SITE_KEY %>";

        window.onload = (function () {

            grecaptcha.execute(GOOGLE_SITE_KEY, { action: 'login' })
                .then(function (token) {
                    $("#ContentBody_ContentMain_recaptcha").val(token);
                });

            btnSend.SetVisible(true);
        });

    </script>

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentBodyTop" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ContentHeader" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="ContentMain" runat="server">

    <h1>Formulario de Registro</h1>
    <h2>Para Colombia</h2>

    <div runat="server" id="Msg" class="alert alert-warning" role="alert" visible="false"></div>

    <p><a href="<%= ResolveClientUrl("~/Content/Files/PCS-PR107-v3controlada.doc-Procedimiento-Uso-de-Datos-Personales.pdf") %>" target="_blank"><i class="mdi mdi-18px mdi-file-pdf-box"></i>  Política de Tratamiento de Datos.</a></p>

    <div class="alert alert-info">
        El siguiente formulario es para crear una cuenta en el sistema y poder realizar las descargar de los archivos de todos los recursos.
    </div>

    <dx:BootstrapFormLayout runat="server" ID="BootstrapFormLayout1">
        <Items>
            <dx:BootstrapLayoutGroup Caption="Información Personal">
                <Items>

                    <dx:BootstrapLayoutItem Caption="Nombre" ColSpanMd="4">
                        <ContentCollection>
                            <dx:ContentControl>
                                <dx:BootstrapTextBox runat="server" ID="Names">

                                    <ValidationSettings CausesValidation="true">
                                        <RequiredField IsRequired="true" />
                                    </ValidationSettings>

                                </dx:BootstrapTextBox>
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:BootstrapLayoutItem>

                    <dx:BootstrapLayoutItem Caption="Apellido" ColSpanMd="4">
                        <ContentCollection>
                            <dx:ContentControl>
                                <dx:BootstrapTextBox runat="server" ID="LastName">
                                    <ValidationSettings CausesValidation="true">
                                        <RequiredField IsRequired="true" />
                                    </ValidationSettings>

                                </dx:BootstrapTextBox>
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:BootstrapLayoutItem>

                    <dx:BootstrapLayoutItem Caption="Movil" ColSpanMd="4">
                        <ContentCollection>
                            <dx:ContentControl>
                                <dx:BootstrapTextBox runat="server" ID="Mobile">
                                    <ValidationSettings CausesValidation="true">
                                        <RequiredField IsRequired="true" />
                                    </ValidationSettings>

                                </dx:BootstrapTextBox>
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:BootstrapLayoutItem>

                    <dx:BootstrapLayoutItem Caption="Email" HelpText="Será su usuario en el sistema." ColSpanMd="4">
                        <ContentCollection>
                            <dx:ContentControl>
                                <dx:BootstrapTextBox runat="server" ID="Email">
                                    <ValidationSettings CausesValidation="true">
                                        <RequiredField IsRequired="true" />
                                        <RegularExpression ErrorText="Email no válido." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                    </ValidationSettings>

                                </dx:BootstrapTextBox>
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:BootstrapLayoutItem>

                    <dx:BootstrapLayoutItem Caption="Password" HelpText="Para iniciar sesión en el sistema." ColSpanMd="4">
                        <ContentCollection>
                            <dx:ContentControl>

                                <dx:BootstrapTextBox runat="server" ID="PasswordReg" Password="true">
                                    <ValidationSettings ValidationGroup="register" ValidateOnLeave="true" CausesValidation="True">
                                        <RequiredField IsRequired="true" ErrorText="Password, Valor requerido" />
                                        <RegularExpression ErrorText="El password no cumple los requisitos minimos de seguridad (8 caracteres, 1 mayuscula, 1 minuscula)."
                                            ValidationExpression="^(?=.{8,})(?=.*[a-z])(?=.*[A-Z])(?!.*\s).*$" />
                                    </ValidationSettings>
                                </dx:BootstrapTextBox>

                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:BootstrapLayoutItem>


                </Items>
            </dx:BootstrapLayoutGroup>

            <dx:BootstrapLayoutGroup Caption="Información Laboral en COLOMBIA">
                <Items>
                    <dx:BootstrapLayoutItem Caption="Cargo" ColSpanMd="6">
                        <ContentCollection>
                            <dx:ContentControl>
                                <dx:BootstrapTextBox runat="server" ID="Position">
                                    <ValidationSettings CausesValidation="true">
                                        <RequiredField IsRequired="true" />
                                    </ValidationSettings>

                                </dx:BootstrapTextBox>
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:BootstrapLayoutItem>

                    <dx:BootstrapLayoutItem Caption="Empresa" ColSpanMd="6">
                        <ContentCollection>
                            <dx:ContentControl>
                                <dx:BootstrapTextBox runat="server" ID="Business">
                                    <ValidationSettings CausesValidation="true">
                                        <RequiredField IsRequired="true" />
                                    </ValidationSettings>

                                </dx:BootstrapTextBox>
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:BootstrapLayoutItem>

                    <dx:BootstrapLayoutItem Caption="País" ColSpanMd="6">
                        <ContentCollection>
                            <dx:ContentControl>
                                <dx:BootstrapTextBox runat="server" ID="Country" ReadOnly="true" Text="Colombia" />
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:BootstrapLayoutItem>

                    <dx:BootstrapLayoutItem Caption="Ciudad" ColSpanMd="6">
                        <ContentCollection>
                            <dx:ContentControl>
                                <dx:BootstrapTextBox runat="server" ID="City" />
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:BootstrapLayoutItem>

                    <dx:BootstrapLayoutItem Caption="Telefono" ColSpanMd="6">
                        <ContentCollection>
                            <dx:ContentControl>
                                <dx:BootstrapTextBox runat="server" ID="Telephone" />
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:BootstrapLayoutItem>

                </Items>
            </dx:BootstrapLayoutGroup>

            <dx:BootstrapLayoutItem HorizontalAlign="Right" ShowCaption="False" ColSpanMd="12">
                <ContentCollection>
                    <dx:ContentControl>

                        <dx:BootstrapButton runat="server" ID="BootstrapButtonSend" ClientInstanceName="btnSend" OnClick="BootstrapButtonSend_Click" Text="Enviar" 
                            SettingsBootstrap-RenderOption="Primary" AutoPostBack="false" UseSubmitBehavior="true">
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

                    </dx:ContentControl>
                </ContentCollection>
            </dx:BootstrapLayoutItem>
        </Items>
    </dx:BootstrapFormLayout>

    <asp:HiddenField runat="server" ID="recaptcha" EnableViewState="false" />
    <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="true" ContainerElementID="BootstrapFormLayout1" />

</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="ContentFooter" runat="server">
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderID="ContentBodyBottom" runat="server">
</asp:Content>
<asp:Content ID="Content11" ContentPlaceHolderID="ContentBodyJS" runat="server">
</asp:Content>
