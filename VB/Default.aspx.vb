Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Linq
Imports DevExpress.Web.Data
Imports DevExpress.Web.ASPxGridView

Partial Public Class _Default
	Inherits System.Web.UI.Page
	Protected ReadOnly Property GridData() As List(Of GridDataItem)
		Get
			Dim key = "34FAA431-CF79-4869-9488-93F6AAE81263"
			If (Not IsPostBack) OrElse Session(key) Is Nothing Then
				Session(key) = Enumerable.Range(1, 3).Select(Function(i) New GridDataItem With {.ID = i, .Mon = i * 10 Mod 3, .Tue = i * 5 Mod 3, .Wen = i Mod 2}).ToList()
			End If
			Return CType(Session(key), List(Of GridDataItem))
		End Get
	End Property
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
		Grid.DataSource = GridData
		Grid.DataBind()
	End Sub
	Protected Sub Grid_RowInserting(ByVal sender As Object, ByVal e As ASPxDataInsertingEventArgs)
		InsertNewItem(e.NewValues)
		CancelEditing(e)
	End Sub
	Protected Sub Grid_RowUpdating(ByVal sender As Object, ByVal e As ASPxDataUpdatingEventArgs)
		UpdateItem(e.Keys, e.NewValues)
		CancelEditing(e)
	End Sub
	Protected Sub Grid_RowDeleting(ByVal sender As Object, ByVal e As ASPxDataDeletingEventArgs)
		DeleteItem(e.Keys, e.Values)
		CancelEditing(e)
	End Sub
	Protected Sub Grid_BatchUpdate(ByVal sender As Object, ByVal e As ASPxDataBatchUpdateEventArgs)
		For Each args In e.InsertValues
			InsertNewItem(args.NewValues)
		Next args
		For Each args In e.UpdateValues
			UpdateItem(args.Keys, args.NewValues)
		Next args
		For Each args In e.DeleteValues
			DeleteItem(args.Keys, args.Values)
		Next args

		e.Handled = True
	End Sub
	Protected Function InsertNewItem(ByVal newValues As OrderedDictionary) As GridDataItem
		Dim item = New GridDataItem() With {.ID = GridData.Count}
		LoadNewValues(item, newValues)
		GridData.Add(item)
		Return item
	End Function
	Protected Function UpdateItem(ByVal keys As OrderedDictionary, ByVal newValues As OrderedDictionary) As GridDataItem
		Dim id = Convert.ToInt32(keys("ID"))
		Dim item = GridData.First(Function(i) i.ID = id)
		LoadNewValues(item, newValues)
		Return item
	End Function
	Protected Function DeleteItem(ByVal keys As OrderedDictionary, ByVal values As OrderedDictionary) As GridDataItem
		Dim id = Convert.ToInt32(keys("ID"))
		Dim item = GridData.First(Function(i) i.ID = id)
		GridData.Remove(item)
		Return item
	End Function
	Protected Sub LoadNewValues(ByVal item As GridDataItem, ByVal values As OrderedDictionary)
		item.Mon = Convert.ToInt32(values("Mon"))
		item.Tue = Convert.ToInt32(values("Tue"))
		item.Wen = Convert.ToInt32(values("Wen"))
	End Sub
	Protected Sub CancelEditing(ByVal e As CancelEventArgs)
		e.Cancel = True
		Grid.CancelEdit()
	End Sub
	Public Class GridDataItem
		Private privateID As Integer
		Public Property ID() As Integer
			Get
				Return privateID
			End Get
			Set(ByVal value As Integer)
				privateID = value
			End Set
		End Property
		Private privateMon As Integer
		Public Property Mon() As Integer
			Get
				Return privateMon
			End Get
			Set(ByVal value As Integer)
				privateMon = value
			End Set
		End Property
		Private privateTue As Integer
		Public Property Tue() As Integer
			Get
				Return privateTue
			End Get
			Set(ByVal value As Integer)
				privateTue = value
			End Set
		End Property
		Private privateWen As Integer
		Public Property Wen() As Integer
			Get
				Return privateWen
			End Get
			Set(ByVal value As Integer)
				privateWen = value
			End Set
		End Property
	End Class
	Protected Sub Grid_CustomUnboundColumnData(ByVal sender As Object, ByVal e As ASPxGridViewColumnDataEventArgs)
		If e.Column.FieldName = "Total" Then
			Dim tue As Integer = Convert.ToInt32(e.GetListSourceFieldValue("Tue"))
			Dim mon As Integer = Convert.ToInt32(e.GetListSourceFieldValue("Mon"))
			Dim wen As Integer = Convert.ToInt32(e.GetListSourceFieldValue("Wen"))

			e.Value = mon + tue + wen
		End If

	End Sub
	Protected Sub Grid_HtmlDataCellPrepared(ByVal sender As Object, ByVal e As ASPxGridViewTableDataCellEventArgs)
		If e.DataColumn.FieldName = "Total" Then
			e.Cell.Attributes.Add("onclick", "event.cancelBubble = true")
		End If
	End Sub


	Protected Function GetSummaryValue(ByVal fieldName As String) As Object
		Dim summaryItem As ASPxSummaryItem = Grid.TotalSummary.First(Function(i) i.Tag = fieldName & "_Sum")
		Return Grid.GetTotalSummaryValue(summaryItem)
	End Function
End Class