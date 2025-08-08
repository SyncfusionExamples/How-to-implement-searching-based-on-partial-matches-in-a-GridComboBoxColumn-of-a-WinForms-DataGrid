# How to implement searching based on partial matches in a GridComboBoxColumn of a WinForms DataGrid (SfDataGrid)

In [WinForms DataGrid](https://www.syncfusion.com/winforms-ui-controls/datagrid) (SfDataGrid), partial search functionality in the [GridComboBoxColumn](https://help.syncfusion.com/cr/windowsforms/Syncfusion.WinForms.DataGrid.GridComboBoxColumn.html) can be implemented by overriding the [GridComboBoxCellRenderer](https://help.syncfusion.com/cr/windowsforms/Syncfusion.WinForms.DataGrid.Renderers.GridComboBoxCellRenderer.html).

Within this custom renderer, the OnInitializeEditElement method should be overridden. After invoking the base method, the [TextChanged](https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.control.textchanged?view=windowsdesktop-9.0&redirectedfrom=MSDN) event of the [WinForms ComboBox](https://www.syncfusion.com/winforms-ui-controls/combobox) (SfComboBox) must be hooked, as **ComboBox** serves as the edit element in the GridComboBoxColumn. In this event handler, a filter can be applied to the view of the ComboBox based on the text entered.

Filtering is performed using the **Contains** method. The filtering logic can be customized according to specific requirements.
 
 ```csharp
//Renderer subscription
sfDataGrid1.CellRenderers.Remove("ComboBox");
sfDataGrid1.CellRenderers.Add("ComboBox", new GridCellComboBoxRendererExt());

//Renderer customization
public class GridCellComboBoxRendererExt : GridComboBoxCellRenderer
{
    public GridCellComboBoxRendererExt() { }

    protected override void OnInitializeEditElement(DataColumnBase column, RowColumnIndex rowColumnIndex, SfComboBox uiElement)
    {
       
        base.OnInitializeEditElement(column, rowColumnIndex, uiElement);
        uiElement.TextChanged += UiElement_TextChanged;
    }
    string filteredText;
    private void UiElement_TextChanged(object sender, EventArgs e)
    {
        var comboBox = sender as SfComboBox;
        if (comboBox != null && comboBox.DropDownListView != null && comboBox.DropDownListView.View != null && comboBox.DropDownListView.View.Filter != null)
        {
            filteredText = comboBox.Text;
            comboBox.DropDownListView.View.Filter = FilterItem;
            comboBox.DropDownListView.View.RefreshFilter();
        } 
    }
    private bool FilterItem(object data)
    {
        if (data != null)
        {
            var orderInfo = data as OrderInfo;
            if (orderInfo != null && filteredText != null && orderInfo.CustomerName.ToLower().Contains(filteredText.ToLower()))
                return true;
        }
        return false;
    } 
}
 ```

 ![Partial search for GridComboBoxColumn](GridComboBoxColumnPartialSearch.GIF)

Take a moment to peruse the [WinForms DataGrid - Column Renderer](https://help.syncfusion.com/windowsforms/datagrid/columntypes#customize-column-renderer) documentation, to learn more about DataGrid column renderer with examples.