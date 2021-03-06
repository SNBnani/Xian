<%-- License

Copyright (c) 2011, ClearCanvas Inc.
All rights reserved.
http://www.clearcanvas.ca

This software is licensed under the Open Software License v3.0.
For the complete license, see http://www.clearcanvas.ca/OSLv3.0
--%>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AutoRouteWorkQueueDetailsView.ascx.cs" Inherits="ClearCanvas.ImageServer.Web.Application.Pages.Queues.WorkQueue.Edit.AutoRouteWorkQueueDetailsView" %>

<ccAsp:SectionPanel ID="AutoRouteInfoSectionPanel" runat="server" HeadingText="Auto-Route" HeadingCSS="CSSDefaultSectionHeading">
<SectionContentTemplate>
    <asp:DetailsView ID="AutoRouteDetailsView" runat="server" AutoGenerateRows="False" CellPadding="4"
    GridLines="Horizontal" CssClass="GlobalGridView" Width="100%">
    <Fields>
        <asp:TemplateField HeaderText="<%$Resources: DetailedViewFieldLabels, WorkQueue_Type %>">
            <HeaderStyle CssClass="StudyDetailsViewHeader" Wrap="False" />
            <ItemTemplate>
                <asp:Label ID="Type" runat="server"  Text='<%# Eval("Type.Description") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="<%$Resources: DetailedViewFieldLabels, WorkQueue_AutoRoute_Destination %>">
            <HeaderStyle CssClass="StudyDetailsViewHeader" Wrap="False" />
            <ItemTemplate>
                <asp:Label ID="Destination" runat="server" Text='<%# Eval("DestinationAE") %>' ></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>  
        <asp:TemplateField HeaderText="<%$Resources: DetailedViewFieldLabels, StudyInstanceUID %>">
            <HeaderStyle CssClass="StudyDetailsViewHeader" Wrap="False" />
            <ItemTemplate>
                <asp:Label ID="StudyInstanceUid" runat="server" Text='<%# Eval("StudyInstanceUid") %>' ></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>      
    </Fields>
    <RowStyle CssClass="GlobalGridViewRow"/>
    <AlternatingRowStyle CssClass="GlobalGridViewAlternatingRow" />
</asp:DetailsView>
</SectionContentTemplate>
</ccAsp:SectionPanel>

<ccAsp:SectionPanel ID="GeneralInfoSectionPanel" runat="server" HeadingText="General Information" HeadingCSS="CSSDefaultSectionHeading">
<SectionContentTemplate>
    <asp:DetailsView ID="GeneralInfoDetailsView" runat="server" AutoGenerateRows="False" CellPadding="4"
    GridLines="Horizontal" CssClass="GlobalGridView" Width="100%">
    <Fields>
    
        <asp:TemplateField HeaderText="<%$Resources: DetailedViewFieldLabels, WorkQueue_ScheduledDateTime %>">
            <HeaderStyle CssClass="StudyDetailsViewHeader" Wrap="False" />
            <ItemTemplate>
                <ccUI:DateTimeLabel ID="ScheduledDateTime" runat="server"  Value='<%# Eval("ScheduledDateTime") %>' ></ccUI:DateTimeLabel>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="<%$Resources: DetailedViewFieldLabels, WorkQueue_ExpirationDateTIme %>">
            <HeaderStyle CssClass="StudyDetailsViewHeader" Wrap="False" />
            <ItemTemplate>
                <ccUI:DateTimeLabel ID="ExpirationTime" runat="server"  Value='<%# Eval("ExpirationTime") %>' ></ccUI:DateTimeLabel>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="<%$Resources: DetailedViewFieldLabels, WorkQueue_InsertDateTime %>">
            <HeaderStyle CssClass="StudyDetailsViewHeader" Wrap="False" />
            <ItemTemplate>
                <ccUI:DateTimeLabel ID="InsertTime" runat="server"  Value='<%# Eval("InsertTime") %>' ></ccUI:DateTimeLabel>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="<%$Resources: DetailedViewFieldLabels, WorkQueue_Status %>">
            <HeaderStyle CssClass="StudyDetailsViewHeader" Wrap="False" />
            <ItemTemplate>
                <asp:Label ID="Status" runat="server" Text='<%# Eval("Status.Description") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="<%$Resources: DetailedViewFieldLabels, WorkQueue_Priority %>">
            <HeaderStyle CssClass="StudyDetailsViewHeader" Wrap="False" />
            <ItemTemplate>
                <asp:Label ID="Priority" runat="server" Text='<%# Eval("Priority.Description") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:BoundField HeaderText="<%$Resources: DetailedViewFieldLabels, WorkQueue_ProcessingServer %>" HeaderStyle-CssClass="StudyDetailsViewHeader" DataField="ServerDescription" />

        <asp:TemplateField HeaderText="<%$Resources: DetailedViewFieldLabels, AccessionNumber %>">
            <HeaderStyle CssClass="StudyDetailsViewHeader" Wrap="false" />
            <ItemTemplate>
                 <asp:Label runat="server" ID="AccessionNumber" Text='<%# Eval("Study.AccessionNumber") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="<%$Resources: DetailedViewFieldLabels, StudyDateTime %>">
            <HeaderStyle CssClass="StudyDetailsViewHeader" Wrap="false" />
            <ItemTemplate>
                <ccUI:DALabel ID="StudyDate" runat="server"  Value='<%# Eval("Study.StudyDate") %>' ></ccUI:DALabel>
                <ccUI:TMLabel ID="StudyTime" runat="server"  Value='<%# Eval("Study.StudyTime") %>' ></ccUI:TMLabel>
            </ItemTemplate>
        </asp:TemplateField> 
        
        <asp:TemplateField HeaderText="<%$Resources: DetailedViewFieldLabels, Modalities %>">
            <HeaderStyle CssClass="StudyDetailsViewHeader" Wrap="false" />
            <ItemTemplate>
                <asp:Label runat="server" ID="Modalities" Text='<%# Eval("Study.Modalities") %>' />
            </ItemTemplate>
        </asp:TemplateField>
                
        <asp:TemplateField HeaderText="<%$Resources: DetailedViewFieldLabels, PatientName %>">
            <HeaderStyle CssClass="StudyDetailsViewHeader" Wrap="False" />        
            <ItemTemplate>
                <ccUI:PersonNameLabel ID="ReferringPhysiciansName" runat="server" PersonName='<%# Eval("Study.PatientName") %>' PersonNameType="Dicom"></ccUI:PersonNameLabel>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<%$Resources: DetailedViewFieldLabels, PatientID %>">
            <HeaderStyle CssClass="StudyDetailsViewHeader" Wrap="False" />        
            <ItemTemplate>
                <asp:Label runat="server" ID="PatientID" Text='<%# Eval("Study.PatientId") %>' />
            </ItemTemplate>
        </asp:TemplateField>
       
        <asp:BoundField HeaderText="<%$Resources: DetailedViewFieldLabels, WorkQueue_FailureCount %>" HeaderStyle-CssClass="StudyDetailsViewHeader" DataField="FailureCount" />
        <asp:BoundField HeaderText="<%$Resources: DetailedViewFieldLabels, WorkQueue_FailureDescription %>" HeaderStyle-CssClass="StudyDetailsViewHeader" DataField="FailureDescription" />
        <asp:BoundField HeaderText="<%$Resources: DetailedViewFieldLabels, WorkQueue_InstancesPending %>" HeaderStyle-CssClass="StudyDetailsViewHeader" DataField="NumInstancesPending" />
        <asp:BoundField HeaderText="<%$Resources: DetailedViewFieldLabels, WorkQueue_SeriesPending %>" HeaderStyle-CssClass="StudyDetailsViewHeader" DataField="NumSeriesPending" />
        <asp:BoundField HeaderText="<%$Resources: DetailedViewFieldLabels, WorkQueue_StorageLocation %>" HeaderStyle-CssClass="StudyDetailsViewHeader" DataField="StorageLocationPath" />
        
    </Fields>
    <HeaderStyle CssClass="StudyDetailsViewHeader" Wrap="false" />
    <RowStyle CssClass="GlobalGridViewRow"/>
    <AlternatingRowStyle CssClass="GlobalGridViewAlternatingRow" />
</asp:DetailsView>
</SectionContentTemplate>
</ccAsp:SectionPanel>



