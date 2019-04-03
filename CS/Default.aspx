<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        var currentColumnName;
        function OnBatchEditStartEditing(s, e) {
            currentColumnName = e.focusedColumn.fieldName;
        }
        function OnBatchEditEndEditing(s, e) {
            var label = ASPxClientControl.GetControlCollection().GetByName('label' + currentColumnName);
            CalculateSummary(s, label, e.rowValues, e.visibleIndex, currentColumnName, false);
            window.setTimeout(function () {
                var rowTotal = 0;
                for (var key in e.rowValues) {
                    if (s.GetColumn(key).fieldName == "Total")
                        continue;
                    rowTotal += e.rowValues[key].value;
                }
                s.batchEditApi.SetCellValue(e.visibleIndex, "Total", rowTotal, null, true);
                var total = parseInt(labelMon.GetValue()) + parseInt(labelTue.GetValue()) + parseInt(labelWen.GetValue())
                labelTotal.SetText(total);
            }, 0);
        }
        function CalculateSummary(grid, labelSum, rowValues, visibleIndex, columnName, isDeleting) {
            var originalValue = grid.batchEditApi.GetCellValue(visibleIndex, columnName);
            var newValue = rowValues[(grid.GetColumnByField(columnName).index)].value;
            var dif = isDeleting ? -newValue : newValue - originalValue;
            labelSum.SetValue((parseInt(labelSum.GetValue()) + dif));
        }
        function OnBatchEditRowDeleting(s, e) {
            var total = 0;
            for (var key in e.rowValues) {
                var columnFieldName = s.GetColumn(key).fieldName;
                if (columnFieldName == "Total")
                    continue;
                var label = ASPxClientControl.GetControlCollection().GetByName('label' + columnFieldName);
                CalculateSummary(s, label, e.rowValues, e.visibleIndex, columnFieldName, true);
                total += parseInt(label.GetValue());
            }
            labelTotal.SetText(total);
        }
        function OnChangesCanceling(s, e) {
            if (s.batchEditApi.HasChanges())
                setTimeout(function () {
                    s.Refresh();
                }, 0);
        }
    </script>
</head>
<body>
    <form id="frmMain" runat="server">
        <dx:ASPxGridView ID="Grid" runat="server" KeyFieldName="ID" Width="600" OnBatchUpdate="Grid_BatchUpdate"
            OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating" OnRowDeleting="Grid_RowDeleting"
            OnCustomUnboundColumnData="Grid_CustomUnboundColumnData" OnHtmlDataCellPrepared="Grid_HtmlDataCellPrepared"
            ClientInstanceName="gridView">
            <ClientSideEvents BatchEditChangesCanceling="OnChangesCanceling" BatchEditRowDeleting="OnBatchEditRowDeleting" BatchEditStartEditing="OnBatchEditStartEditing" BatchEditEndEditing="OnBatchEditEndEditing" />
            <Columns>
                <dx:GridViewCommandColumn ShowNewButtonInHeader="true" ShowDeleteButton="true"></dx:GridViewCommandColumn>
                <dx:GridViewDataSpinEditColumn FieldName="Mon">
                    <PropertiesSpinEdit MinValue="0" MaxValue="9999"></PropertiesSpinEdit>
                    <FooterTemplate>
                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" ClientInstanceName="labelMon" Text='<%# GetSummaryValue((Container.Column as GridViewDataColumn).FieldName) %>'></dx:ASPxLabel>
                    </FooterTemplate>
                </dx:GridViewDataSpinEditColumn>
                <dx:GridViewDataSpinEditColumn FieldName="Tue">
                    <PropertiesSpinEdit MinValue="0" MaxValue="9999"></PropertiesSpinEdit>
                    <FooterTemplate>
                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" ClientInstanceName="labelTue" Text='<%# GetSummaryValue((Container.Column as GridViewDataColumn).FieldName) %>'></dx:ASPxLabel>
                    </FooterTemplate>
                </dx:GridViewDataSpinEditColumn>
                <dx:GridViewDataSpinEditColumn FieldName="Wen">
                    <PropertiesSpinEdit MinValue="0" MaxValue="9999"></PropertiesSpinEdit>
                    <FooterTemplate>
                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" ClientInstanceName="labelWen" Text='<%# GetSummaryValue((Container.Column as GridViewDataColumn).FieldName) %>'></dx:ASPxLabel>
                    </FooterTemplate>
                </dx:GridViewDataSpinEditColumn>
                <dx:GridViewDataTextColumn FieldName="Total" UnboundType="Decimal" ReadOnly="true">
                    <Settings ShowEditorInBatchEditMode="false" />
                    <FooterTemplate>
                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" ClientInstanceName="labelTotal" Text='<%# GetSummaryValue((Container.Column as GridViewDataColumn).FieldName) %>'></dx:ASPxLabel>
                    </FooterTemplate>
                </dx:GridViewDataTextColumn>
            </Columns>

            <SettingsEditing Mode="Batch" />
            <Settings ShowFooter="true" />
            <TotalSummary>
                <dx:ASPxSummaryItem SummaryType="Sum" FieldName="Mon" Tag="Mon_Sum" />
                <dx:ASPxSummaryItem SummaryType="Sum" FieldName="Tue" Tag="Tue_Sum" />
                <dx:ASPxSummaryItem SummaryType="Sum" FieldName="Wen" Tag="Wen_Sum" />
                <dx:ASPxSummaryItem SummaryType="Sum" FieldName="Total" Tag="Total_Sum" />
            </TotalSummary>
        </dx:ASPxGridView>
    </form>
</body>
</html>
