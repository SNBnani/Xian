<%@ Control Language="C#" AutoEventWireup="true" Codebehind="AddEditServerRuleDialog.ascx.cs"
    Inherits="ClearCanvas.ImageServer.Web.Application.Admin.Configuration.ServerRules.AddEditServerRuleDialog" %>
<%@ Register Assembly="Validators" Namespace="Sample.Web.UI.Compatibility" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:ScriptManagerProxy runat="server">
    <Services>
        <asp:ServiceReference Path="ServerRuleSamples.asmx" />
    </Services>
</asp:ScriptManagerProxy>

    <asp:UpdatePanel ID="AddEditUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
       
            <asp:Panel ID="DialogPanel" runat="server" CssClass="CSSPopupWindow" Style="display: none"
                Width="500px">
                <asp:Panel ID="TitleBarPanel" runat="server" CssClass="CSSPopupWindowTitleBar" Width="100%">
                    <table style="width: 100%">
                        <tr>
                            <td valign="middle">
                                <asp:Label ID="TitleLabel" runat="server" EnableViewState="False" Text="Add Server Rule"
                                    Width="100%"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <div class="CSSPopupWindowBody">
                    <ajaxToolkit:TabContainer ID="ServerPartitionTabContainer" runat="server" ActiveTabIndex="0"
                        CssClass="CSSDialogTabControl">
                        <ajaxToolkit:TabPanel ID="GeneralTabPanel" runat="server" HeaderText="GeneralTabPanel"
                            CssClass="CSSTabPanel">
                            <ContentTemplate>
                                <asp:Panel ID="Panel1" runat="server" CssClass="CSSDialogTabPanelContent">
                                    <asp:Panel runat="server">
                                        <table runat="server" width="100%">
                                            <tr align="left">
                                                <td colspan="2">
                                                    <asp:Label ID="Label1" runat="server" Text="Rule Name"></asp:Label><br />
                                                    <asp:TextBox ID="RuleNameTextBox" runat="server" Width="100%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="RuleTypeLabel" runat="server" Text="Rule Type"></asp:Label><br />
                                                    <asp:DropDownList ID="RuleTypeDropDownList" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="RuleApplyTimeLabel" runat="server" Text="Rule Apply Time"></asp:Label><br />
                                                    <asp:DropDownList ID="RuleApplyTimeDropDownList" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="EnabledCheckBox" runat="server" Text="Enabled" Checked="true" /></td>
                                                <td>
                                                    <asp:CheckBox ID="DefaultCheckBox" runat="server" Text="Default" Checked="false" /></td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </asp:Panel>
                            </ContentTemplate>
                            <HeaderTemplate>
                                General
                            </HeaderTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="RuleXmlTabPanel" runat="server" HeaderText="TabPanel2">
                            <ContentTemplate>
                                <asp:Panel ID="Panel2" runat="server" CssClass="CSSDialogTabPanelContent">
                                    <table width="100%" cellpadding="5" cellspacing="5">
                                        <tr>
                                            <td>
                                                <asp:Label ID="SelectSampleRuleLabel" runat="server" Text="Select Sample Rule"></asp:Label><br />
                                                <asp:DropDownList ID="SampleRuleDropDownList" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="RuleXmlTextBox" runat="server" EnableViewState="true" Width="100%"
                                                    Rows="12" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </ContentTemplate>
                            <HeaderTemplate>
                                Rule XML
                            </HeaderTemplate>
                        </ajaxToolkit:TabPanel>
                    </ajaxToolkit:TabContainer>
                    <br />
                    <center>
                        <table cellpadding="5" cellspacing="5" width="40%">
                            <tr>
                                <td align="center">
                                    <asp:Button ID="OKButton" runat="server" OnClick="OKButton_Click" Text="Add" Width="77px" />
                                </td>
                                <td align="center">
                                    <asp:Button ID="CancelButton" runat="server" Text="Cancel" />
                                </td>
                            </tr>
                        </table>
                        <br />
                    </center>
                    <asp:Panel ID="DummyPanel" runat="server" Height="1px" Style="z-index: 101; left: 522px;
                        position: absolute; top: 53px" Width="36px">
                    </asp:Panel>
                </div>
            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" BackgroundCssClass="CSSModalBackground"
                DropShadow="true" RepositionMode="RepositionOnWindowResize" Enabled="true" PopupControlID="DialogPanel" TargetControlID="DummyPanel" BehaviorID="MyStupidExtender">
            </ajaxToolkit:ModalPopupExtender>
            
        </ContentTemplate>
    </asp:UpdatePanel>

