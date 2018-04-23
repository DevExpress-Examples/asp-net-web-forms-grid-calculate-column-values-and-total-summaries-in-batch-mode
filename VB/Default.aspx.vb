Imports System
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Linq
Imports DevExpress.Web.Data
Imports DevExpress.Web

Partial Public Class _Default
    Inherits System.Web.UI.Page

    Protected ReadOnly Property GridData() As List(Of GridDataItem)
        Get
            Dim key = "34FAA431-CF79-4869-9488-93F6AAE81263"
            If (Not IsPostBack) OrElse Session(key) Is Nothing Then
                Session(key) = Enumerable.Range(1, 3).Select(Function(i) New GridDataItem With {.ID = i, .Mon = i * 10 Mod 3, .Tue = i * 5 Mod 3, .Wen = i Mod 2}).ToList()
            End If
            Return DirectCast(Session(key), List(Of GridDataItem))
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

        Dim id_Renamed = Convert.ToInt32(keys("ID"))
        Dim item = GridData.First(Function(i) i.ID = id_Renamed)
        LoadNewValues(item, newValues)
        Return item
    End Function
    Protected Function DeleteItem(ByVal keys As OrderedDictionary, ByVal values As OrderedDictionary) As GridDataItem

        Dim id_Renamed = Convert.ToInt32(keys("ID"))
        Dim item = GridData.First(Function(i) i.ID = id_Renamed)
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
        Public Property ID() As Integer
        Public Property Mon() As Integer
        Public Property Tue() As Integer
        Public Property Wen() As Integer
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