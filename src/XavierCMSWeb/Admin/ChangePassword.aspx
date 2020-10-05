<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeBehind="ChangePassword.aspx.cs" Inherits="XavierCMSWeb.ChangePassword" %>

<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
    <div class="accountHeader">
 
    <h2>Change Password</h2>
    <p>Use the form below to change your password.</p>
    <p>New passwords are required to be a minimum of <%= Membership.MinRequiredPasswordLength %> characters in length.</p>
</div>

<dx:BootstrapTextBox ID="tbCurrentPassword" runat="server" Caption="Old Password" Password="true" Width="200px">
  <CaptionSettings Position="Before" />
  <ValidationSettings ValidationGroup="ChangeUserPasswordValidationGroup">
    <RequiredField ErrorText="Old Password is required." IsRequired="true" />
  </ValidationSettings>
</dx:BootstrapTextBox>
<dx:BootstrapTextBox ID="tbPassword" ClientInstanceName="Password" Caption="Password" Password="true" runat="server"
      Width="200px">
  <CaptionSettings Position="Before" />
  <ValidationSettings ValidationGroup="ChangeUserPasswordValidationGroup">
    <RequiredField ErrorText="Password is required." IsRequired="true" />
  </ValidationSettings>
</dx:BootstrapTextBox>
<dx:BootstrapTextBox ID="tbConfirmPassword" Password="true" Caption="Confirm password" runat="server" Width="200px">
  <CaptionSettings Position="Before" />
  <ValidationSettings ValidationGroup="ChangeUserPasswordValidationGroup">
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
<dx:BootstrapButton ID="btnChangePassword" runat="server" Text="Change Password" ValidationGroup="ChangeUserPasswordValidationGroup"
    OnClick="btnChangePassword_Click">
</dx:BootstrapButton>
</asp:Content>