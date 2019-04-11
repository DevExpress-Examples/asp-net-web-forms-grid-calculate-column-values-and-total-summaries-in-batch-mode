using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using DevExpress.Web.Data;
using DevExpress.Web;

public partial class _Default: System.Web.UI.Page {
    protected List<GridDataItem> GridData {
        get {
            var key = "34FAA431-CF79-4869-9488-93F6AAE81263";
            if (!IsPostBack || Session[key] == null)
                Session[key] = Enumerable.Range(1, 3).Select(i => new GridDataItem
                {
                    ID = i,
                    Mon = i * 10 % 3,
                    Tue = i * 5 % 3,
                    Wen = i % 2
                }).ToList();
            return (List<GridDataItem>)Session[key];
        }
    }
    protected void Page_Load(object sender, EventArgs e) {
        Grid.DataSource = GridData;
        Grid.DataBind();
    }
    protected void Grid_RowInserting(object sender, ASPxDataInsertingEventArgs e) {
        InsertNewItem(e.NewValues);
        CancelEditing(e);
    }
    protected void Grid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e) {
        UpdateItem(e.Keys, e.NewValues);
        CancelEditing(e);
    }
    protected void Grid_RowDeleting(object sender, ASPxDataDeletingEventArgs e) {
        DeleteItem(e.Keys, e.Values);
        CancelEditing(e);
    }
    protected void Grid_BatchUpdate(object sender, ASPxDataBatchUpdateEventArgs e) {
        foreach (var args in e.InsertValues)
            InsertNewItem(args.NewValues);
        foreach (var args in e.UpdateValues)
            UpdateItem(args.Keys, args.NewValues);
        foreach (var args in e.DeleteValues)
            DeleteItem(args.Keys, args.Values);

        e.Handled = true;
    }
    protected GridDataItem InsertNewItem(OrderedDictionary newValues) {
        var item = new GridDataItem() { ID = GridData.Count };
        LoadNewValues(item, newValues);
        GridData.Add(item);
        return item;
    }
    protected GridDataItem UpdateItem(OrderedDictionary keys, OrderedDictionary newValues) {
        var id = Convert.ToInt32(keys["ID"]);
        var item = GridData.First(i => i.ID == id);
        LoadNewValues(item, newValues);
        return item;
    }
    protected GridDataItem DeleteItem(OrderedDictionary keys, OrderedDictionary values) {
        var id = Convert.ToInt32(keys["ID"]);
        var item = GridData.First(i => i.ID == id);
        GridData.Remove(item);
        return item;
    }
    protected void LoadNewValues(GridDataItem item, OrderedDictionary values) {
        item.Mon = Convert.ToInt32(values["Mon"]);
        item.Tue = Convert.ToInt32(values["Tue"]);
        item.Wen = Convert.ToInt32(values["Wen"]);
    }
    protected void CancelEditing(CancelEventArgs e) {
        e.Cancel = true;
        Grid.CancelEdit();
    }
    public class GridDataItem {
        public int ID { get; set; }
        public int Mon { get; set; }
        public int Tue { get; set; }
        public int Wen { get; set; }
    }
    protected void Grid_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e) {
        if (e.Column.FieldName == "Total") {
            int tue = Convert.ToInt32(e.GetListSourceFieldValue("Tue"));
            int mon = Convert.ToInt32(e.GetListSourceFieldValue("Mon"));
            int wen = Convert.ToInt32(e.GetListSourceFieldValue("Wen"));

            e.Value = mon + tue + wen;
        }

    }
    protected void Grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e) {
        if (e.DataColumn.FieldName == "Total")
            e.Cell.Attributes.Add("onclick", "event.cancelBubble = true");
    }


    protected object GetSummaryValue(string fieldName) {
        ASPxSummaryItem summaryItem = Grid.TotalSummary.First(i => i.Tag == fieldName + "_Sum");
        return Grid.GetTotalSummaryValue(summaryItem);
    }
}