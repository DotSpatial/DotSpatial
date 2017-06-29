<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="DemoWEB.WebForm1" %>

<%@ Register Assembly="DotSpatial.WebControls" Namespace="DotSpatial.WebControls"
    TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="LegDiv" style="position:absolute; left:3px; top:3px; bottom:3px; width:230px; border:1px solid black; overflow: auto;">
            <cc1:WebLegend ID="WebLegend1" runat="server"  Width="100%" Font-Size="10pt" >
                <SelectedNodeStyle BackColor="#33CCFF" BorderColor="#000099" />
            </cc1:WebLegend>
            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Ref.gif" 
                  style="position:absolute; right:3px; top:3px; bottom:3px; left: 203px;" />
        </div>


            <div id="Div1" style="padding:3px; position:absolute; left:236px;top:3px; right:3px; height:33; border: 1px solid black;">
            <cc1:WebToolBar ID="WebToolBar1" runat="server" Width="80%" />
        </div>    

        <div id="Web" style="position:absolute; left:236px;top:36px; right:3px; bottom:3px; border: 1px solid black">
           <cc1:WebMap ID="WebMap1" runat="server" Height="100%" Width="100%" />
        </div>    
    </div>
    </form>
</body>
</html>
