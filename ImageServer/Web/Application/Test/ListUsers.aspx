<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListUsers.aspx.cs" Inherits="ClearCanvas.ImageServer.Web.Application.Test.ListUsers" %>
<%@ Import namespace="ClearCanvas.Enterprise.Common.Admin.UserAdmin"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label runat="server" ID="Login" />
        <asp:GridView runat="server" ID="UserGridView">
        </asp:GridView>
    </div>
    </form>
</body>
</html>
