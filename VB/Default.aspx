'INSTANT VB NOTE: This code snippet uses implicit typing. You will need to set 'Option Infer On' in the VB file or set 'Option Infer' at the project level:

<%@ Page Language="vb" AutoEventWireup="true" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
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
			var originalValue = s.batchEditApi.GetCellValue(e.visibleIndex, currentColumnName);
			var newValue = e.rowValues[(s.GetColumnByField(currentColumnName).index)].value;

			var dif = newValue - originalValue;
			var label = ASPxClientControl.GetControlCollection().GetByName('label' + currentColumnName);

			label.SetValue((parseFloat(label.GetValue()) + dif));


			window.setTimeout(function () {
				var tue = s.batchEditApi.GetCellValue(e.visibleIndex, "Tue");
				var mon = s.batchEditApi.GetCellValue(e.visibleIndex, "Mon");
				var wen = s.batchEditApi.GetCellValue(e.visibleIndex, "Wen");
				s.batchEditApi.SetCellValue(e.visibleIndex, "Total", tue + mon + wen);

				var total = parseInt(labelMon.GetValue()) + parseInt(labelTue.GetValue())+ parseInt(labelWen.GetValue())
				labelTotal.SetText(total);
			}, 10);

		}
	</script>
</head>
<body>
	<form id="frmMain" runat="server">
		<dx:ASPxGridView ID="Grid" runat="server" KeyFieldName="ID" Width="600" OnBatchUpdate="Grid_BatchUpdate"
			OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating" OnRowDeleting="Grid_RowDeleting" OnCustomUnboundColumnData="Grid_CustomUnboundColumnData" OnHtmlDataCellPrepared="Grid_HtmlDataCellPrepared">
			<ClientSideEvents BatchEditStartEditing="OnBatchEditStartEditing" BatchEditEndEditing="OnBatchEditEndEditing" />
			<Columns>
				 <dx:GridViewDataSpinEditColumn FieldName="Mon">
					 <PropertiesSpinEdit MinValue="0" MaxValue="9999"></PropertiesSpinEdit>
					<FooterTemplate>
						<dx:ASPxLabel ID="ASPxLabel1" runat="server" ClientInstanceName="labelMon" Text='<%#GetSummaryValue((TryCast(Container.Column, GridViewDataColumn)).FieldName)%>'></dx:ASPxLabel>
					</FooterTemplate>
				</dx:GridViewDataSpinEditColumn>
				<dx:GridViewDataSpinEditColumn FieldName="Tue">
					 <PropertiesSpinEdit MinValue="0" MaxValue="9999"></PropertiesSpinEdit>
					<FooterTemplate>
						<dx:ASPxLabel ID="ASPxLabel1" runat="server" ClientInstanceName="labelTue" Text='<%#GetSummaryValue((TryCast(Container.Column, GridViewDataColumn)).FieldName)%>'></dx:ASPxLabel>
					</FooterTemplate>
				</dx:GridViewDataSpinEditColumn>
				<dx:GridViewDataSpinEditColumn FieldName="Wen">
					 <PropertiesSpinEdit MinValue="0" MaxValue="9999"></PropertiesSpinEdit>
					<FooterTemplate>
						<dx:ASPxLabel ID="ASPxLabel1" runat="server" ClientInstanceName="labelWen" Text='<%#GetSummaryValue((TryCast(Container.Column, GridViewDataColumn)).FieldName)%>'></dx:ASPxLabel>
					</FooterTemplate>
				</dx:GridViewDataSpinEditColumn>
				<dx:GridViewDataTextColumn FieldName="Total" UnboundType="Decimal" ReadOnly="true">
					<FooterTemplate>
						<dx:ASPxLabel ID="ASPxLabel1" runat="server" ClientInstanceName="labelTotal" Text='<%#GetSummaryValue((TryCast(Container.Column, GridViewDataColumn)).FieldName)%>'></dx:ASPxLabel>
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