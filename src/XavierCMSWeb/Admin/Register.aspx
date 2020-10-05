<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeBehind="Register.aspx.cs" Inherits="XavierCMSWeb.Register" %>

<asp:content id="ClientArea" contentplaceholderid="Content" runat="server">
    <div class="accountHeader">
    <h2>Create a New Account</h2>
    <p>Use the form below to create a new account.</p>
    <p>Passwords are required to be a minimum of <%= Membership.MinRequiredPasswordLength %> characters in length.</p>
</div>
<dx:BootstrapTextBox ID="tbUserName" runat="server" Width="200px" Caption="User Name">
  <CaptionSettings Position="Before" />
  <ValidationSettings ValidationGroup="RegisterUserValidationGroup">
    <RequiredField ErrorText="User Name is required." IsRequired="true" />
  </ValidationSettings>
</dx:BootstrapTextBox>
<dx:BootstrapTextBox ID="tbEmail" runat="server" Width="200px" Caption="E-mail">
  <CaptionSettings Position="Before" />
  <ValidationSettings ValidationGroup="RegisterUserValidationGroup">
    <RequiredField ErrorText="E-mail is required." IsRequired="true" />
    <RegularExpression ErrorText="Email validation failed" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
  </ValidationSettings>
</dx:BootstrapTextBox>
<dx:BootstrapTextBox ID="tbPassword" ClientInstanceName="Password" Password="true" runat="server" Width="200px" Caption="Password">
  <CaptionSettings Position="Before" />
  <ValidationSettings ValidationGroup="RegisterUserValidationGroup">
    <RequiredField ErrorText="Password is required." IsRequired="true" />
  </ValidationSettings>
</dx:BootstrapTextBox>
<dx:BootstrapTextBox ID="tbConfirmPassword" Password="true" runat="server" Width="200px" Caption="Confirm password">
  <CaptionSettings Position="Before" />
  <ValidationSettings ValidationGroup="RegisterUserValidationGroup">
    <RequiredField ErrorText="Confirm Password is required." IsRequired="true" />
  </ValidationSettings>
  <ClientSideEvents Validation="function(s, e) {
            var originalPasswd = Password.GetText();
            var currentPasswd = s.GetText();
            e.isValid = (originalPasswd  == currentPasswd );
            e.errorText = 'The Password and Confirmation Password must match.';
        }" />
</dx:BootstrapTextBox>
<br />
<dx:BootstrapButton ID="btnCreateUser" runat="server" Text="Create User" ValidationGroup="RegisterUserValidationGroup"
    OnClick="btnCreateUser_Click">
</dx:BootstrapButton>
</asp:content>