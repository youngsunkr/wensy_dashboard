<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ServicePoint.LogIn.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge;chrome=1" />

    <link href="Style/StyleSheet.css" rel="stylesheet" type="text/css" />
    <script src="Script/common.js" type="text/javascript"></script>

    <style type="text/css">
        .auto-style1 {
            width: 400px;
            margin: auto;
            text-align: center;
        }

            .auto-style1 table input {
                width: 100%;
            }

        table {
            width: 100%;
        }

            table td {
                text-align: left;
                height: 45px;
            }

        .tablestyle1 {
            color: #ffffff;
            vertical-align: bottom;
            font-size: 18px;
            font-weight: bold;
        }

        .tablestyle2 {
            color: #666666;
            font-size: x-large;
            height: 45px;
        }

        .divBtn2 {
            margin: auto;
            padding: 10px;
            text-align: center;
            border-radius: 10px;
            background-color: #000960;
            margin-bottom: 20px;
        }
    </style>

    <title></title>
</head>

<body style="background-color: #23A0DA;">
    <form id="form1" runat="server">
        <div class="auto-style1">
            <img class="auto-style3" src="/images/Login_header.png" />

            <table>
                <tr>
                    <td class="tablestyle1"><%= ServicePoint.Lib.Locale.Instance.GetLocaleValue("MemberRegistLblMail")%></td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtEmail" type="Email" runat="server" CssClass="tablestyle2" Font-Names="Tahoma"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tablestyle1"><%= ServicePoint.Lib.Locale.Instance.GetLocaleValue("MemberRegistLblPassword")%></td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtPassword" type="password" runat="server" CssClass="tablestyle2" Font-Names="Tahoma"></asp:TextBox></td>
                </tr>
            </table>

            <div style="height: 55px;">
                <asp:Label ID="lblError" runat="server"></asp:Label>
            </div>

            <div class="divBtn2" style="width: 96%;">
                <asp:Button ID="btnOK" runat="server" Text="Login" Height="35" BackColor="Transparent" Font-Bold="true" ForeColor="White" Font-Size="X-Large" BorderStyle="None" OnClick="btnOK_Click" />
            </div>
            <div class="divBtn2" style="width: 96%;">
                <input type="button" value="Join" onclick="location.href('/login/join.aspx')" style="color: White; background-color: Transparent; border-style: None; font-size: X-Large; font-weight: bold; height: 35px;" />
            </div>
           
        </div>

    </form>
</body>
</html>
