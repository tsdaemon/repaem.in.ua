<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SMSProject._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SmsPage</title>
</head>
<body>
    <form id="idForm" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div>
        <div style="float:left; height:300px; width:40%">
            <asp:Menu ID="idMainMenu" runat="server" OnMenuItemClick="idMainMenu_MenuItemClick">
                <Items>
                    <asp:MenuItem Text="�����������" Value="Auth" />
                    <asp:MenuItem Text="������� �� �����" Value="CreditBalance"></asp:MenuItem>    
                    <asp:MenuItem Text="��������� SMS" Value="SendSMS"></asp:MenuItem>
                    <asp:MenuItem Text="��������� � ����������� ��������" Value="NewMessages"></asp:MenuItem>
                    <asp:MenuItem Text="������ ��������" Value="MessageStatus"></asp:MenuItem>
                </Items>
            </asp:Menu>
        </div>
        <div style="float:right; height:300px; width:60%">
            <asp:Panel runat="server" ID="pAuth" Width="100%" Visible="false">
                <div>��� ������������&nbsp<asp:TextBox runat="server" ID="idLogin"></asp:TextBox></div>
                <div>������&nbsp<asp:TextBox runat="server" ID="idPass" TextMode="Password"></asp:TextBox></div>
            </asp:Panel>
            <asp:Panel ID="pGetBalance" runat="server" Visible="false">
                ������� "���������"
            </asp:Panel>
            <asp:Panel ID="pSendSMS" runat="server" Visible="false">
                <table>
                    <tr>
                        <td>�����������</td>
                        <td><asp:TextBox runat="server" ID="idSender"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>����������</td>
                        <td><asp:TextBox runat="server" ID="idReceivers"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>����� ���</td>
                        <td><asp:TextBox runat="server" ID="idText"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>������ WAP PUSH</td>
                        <td><asp:TextBox runat="server" ID="idWap"></asp:TextBox></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pNewMessages" runat="server" Visible="false">
                ������� "���������"
            </asp:Panel>
            <asp:Panel ID="pMessageStatus" runat="server" Visible="false">
                ID ��������� &nbsp&nbsp<asp:TextBox runat="server" ID="idMessageID"></asp:TextBox>
            </asp:Panel>
        </div>
        <div style="float:left; width:300px;"><asp:Button runat="server" ID="bOk" Text="���������" Width="100px" OnClick="bOk_Click" /></div>
        <div style="float:left;"><asp:Label runat="server" ID="lResult" Font-Bold="true" Font-Size="Large"></asp:Label></div>
        <div style="width:100%"><hr /></div>
    </div>
    </form>
</body>
</html>

