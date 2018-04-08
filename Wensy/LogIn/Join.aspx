<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Join.aspx.cs" Inherits="ServicePoint.LogIn.Join" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">



    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
        <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->
    <title>Servic Point</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="description" content="" />
    <meta name="keywords" content="" />
    <meta name="author" content="" />
    <link href="/Common/Bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="/Common/Bootstrap/css/bootstrap-responsive.min.css" rel="stylesheet" type="text/css" />
    <link href="/Common/Bootstrap/css/bootstrap-theme.min.css" rel="stylesheet" type="text/css" />
    <link href="/Common/Bootstrap/css/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <%--<link href="/Common/Bootstrap/css/common.css" rel="stylesheet" />--%>
    <%-- <link href="/Common/Bootstrap/css/bootstrap-carousel-vertical.css" rel="stylesheet" type="text/css" />--%>
    <%-- <link href="/Common/Bootstrap/css/gms.css" rel="stylesheet" type="text/css" />--%>
    <script type="text/javascript" src="/Common/js/jquery.min.js"></script>
    <script type="text/javascript" src="/Common/Bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="/Common/js/gms.js"></script>
    <script type="text/javascript" src="/Common/js/date.js"></script>
    <script type="text/javascript" src="/Common/js/Flotr2/flashcanvas.js"></script>
    <script type="text/javascript" src="/Common/js/Flotr2/flotr2.min.js"></script>
    <script type="text/javascript" src="/Common/js/Flotr2/Chart.js"></script>

</head>
<body style="background-color: #f3f1f1;">
      <script>
          function allowOnlyNumber(evt) {
              var charCode = (evt.which) ? evt.which : event.keyCode
              if (charCode > 31 && (charCode < 48 || charCode > 57))
                  return false;
              return true;
          }
    </script>
    <form id="frm" runat="server" class="form-horizontal">
        <asp:HiddenField ID="hdn_Email" runat="server" />
        <asp:Literal ID="litScript" runat="server"></asp:Literal>
        <div class="container-fluid">
            <div class="row-fluid">
                <div class="col-lg-3"></div>
                <div class="col-lg-6">
                    <div class="row-fluid">
                        <div class="col-lg-12">
                            <input type="image" src="/images/joinus_header.png" class="center-block" />
                        </div>
                        <div class="col-lg-12">
                            <div class="form-group">
                                <asp:Label ID="lbl_Email" runat="server" for="txt_Email" CssClass="col-lg-2 control-label">EMAIL ADDRESS</asp:Label>
                                <div class="col-lg-10">
                                    <asp:TextBox ID="txt_Email" runat="server" CssClass="form-control" TextMode="Email" placeholder="Email"></asp:TextBox>
                                    <asp:Button ID="btn_Check" runat="server" CssClass="btn btn-primary" Text="Email Check" OnClick="btn_Check_Click" />
                                </div>

                            </div>
                            <div class="form-group">
                                <asp:Label ID="lbl_Pass" runat="server" CssClass="col-lg-2 control-label">PASSWORD</asp:Label>
                                <div class="col-lg-10">
                                    <asp:TextBox ID="txt_Pass" runat="server" TextMode="Password" CssClass="form-control" placeholder="Max length 25"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lbl_Pass_Confirm" runat="server" CssClass="col-lg-2 control-label">CONFIRM PASSWORD</asp:Label>
                                <div class="col-lg-10">
                                    <asp:TextBox ID="txt_Pass_Confirm" runat="server" TextMode="Password" CssClass="form-control" placeholder="Max length 25"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lbl_Name" runat="server" CssClass="col-lg-2 control-label">NAME</asp:Label>
                                <div class="col-lg-10">
                                    <asp:TextBox ID="txt_Name" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lbl_Company" runat="server" CssClass="col-lg-2 control-label">COMPANY</asp:Label>
                                <div class="col-lg-10">
                                    <asp:TextBox ID="txt_Company" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-lg-4"></div>
                                <div class="col-lg-8">
                                    <asp:Button ID="btn_Join" runat="server" CssClass="btn btn-primary" OnClick="btn_Join_Click" Text="JOIN" />
                                    <asp:Button ID="btn_Main" runat="server" CssClass="btn btn-warning" OnClick="btn_Main_Click" Text="GO TO MAIN" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="col-lg-3"></div>
            </div>
        </div>
    </form>
</body>
</html>
