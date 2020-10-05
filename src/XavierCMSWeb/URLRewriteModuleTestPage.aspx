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


<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="URLRewriteModuleTestPage.aspx.cs" Inherits="XavierCMSWeb.URLRewriteModuleTestPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <h1>URL Rewrite Module Test Page</h1>

            TIPO: <%: Request.QueryString["typ"] %>
            <br />

            CAT: <%: Request.QueryString["cat"] %>
            <br />

            SUB:  <%: Request.QueryString["sub"] %>
            <br />
            PAG: <%: Request.QueryString["pag"] %>

            <table>
                <tr>
                    <th>Server Variable</th>
                    <th>Value</th>
                </tr>
                <tr>
                    <td>Original URL: </td>
                    <td><%= Request.ServerVariables["HTTP_X_ORIGINAL_URL"] %></td>
                </tr>
                <tr>
                    <td>Final URL: </td>
                    <td><%= Request.ServerVariables["SCRIPT_NAME"] %>?<%= Request.ServerVariables["QUERY_STRING"] %></td>
                </tr>
                <tr>
                    <td>Public Link: </td>
                    <td><a href="<%= Request.ServerVariables["SCRIPT_NAME"] %>?<%= Request.ServerVariables["QUERY_STRING"] %>">Link URL</a></td>
                </tr>
            </table>

        </div>
    </form>
</body>
</html>
