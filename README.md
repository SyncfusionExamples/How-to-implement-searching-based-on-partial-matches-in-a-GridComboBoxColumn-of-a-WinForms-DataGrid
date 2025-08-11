# How to implement searching based on partial matches in a GridComboBoxColumn of a WinForms DataGrid?

In [WinForms DataGrid](https://www.syncfusion.com/winforms-ui-controls/datagrid) (SfDataGrid), partial search functionality in the [GridComboBoxColumn](https://help.syncfusion.com/cr/windowsforms/Syncfusion.WinForms.DataGrid.GridComboBoxColumn.html) can be implemented by overriding the [GridComboBoxCellRenderer](https://help.syncfusion.com/cr/windowsforms/Syncfusion.WinForms.DataGrid.Renderers.GridComboBoxCellRenderer.html).

The **OnInitializeEditElement** method is overridden in the custom renderer. After invoking the base method, the **TextChanged** event of the [WinForms ComboBox](https://www.syncfusion.com/winforms-ui-controls/combobox) (SfComboBox) is hooked, as **ComboBox** acts as the edit element in the GridComboBoxColumn. 

In the event handler, the ComboBox view is filtered based on the input text, using the **Contains** method to match partial entries.
 
 ```csharp
// Remove the old Renderer
sfDataGrid1.CellRenderers.Remove("ComboBox");

// Add the custom Renderer
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

Take a moment to peruse the [WinForms DataGrid - Column Renderer](https://help.syncfusion.com/windowsforms/datagrid/columntypes#customize-column-renderer) documentation, to learn more about column renderer with examples.