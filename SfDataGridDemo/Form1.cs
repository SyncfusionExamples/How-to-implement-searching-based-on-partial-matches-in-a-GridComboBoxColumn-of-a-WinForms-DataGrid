using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Renderers;
using Syncfusion.WinForms.GridCommon.ScrollAxis;
using Syncfusion.WinForms.ListView;
using Syncfusion.WinForms.ListView.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;

namespace SfDataGridDemo
{
    public partial class Form1 : Form
    {
        public OrderInfoCollection collections;

        public Form1()
        {
            InitializeComponent();
            collections = new OrderInfoCollection();
            sfDataGrid1.AutoGenerateColumns = false;
            sfDataGrid1.AllowEditing = true;
            sfDataGrid1.DataSource = collections.Orders;
            sfDataGrid1.Columns.Add(new GridNumericColumn() { MappingName = "OrderID", HeaderText = "Order ID" });
            sfDataGrid1.Columns.Add(new GridTextColumn() { MappingName = "CustomerID", HeaderText = "Customer ID" });
            sfDataGrid1.Columns.Add(new GridTextColumn() { MappingName = "Country", HeaderText = "Country"});
            sfDataGrid1.Columns.Add(new GridComboBoxColumn()
            {
                MappingName = "CustomerName",
                HeaderText = "Customer Name",
                DisplayMember = "CustomerName",
                ValueMember = "CustomerName",
                Width = 200,
                DataSource = collections.Orders,
                DropDownStyle = DropDownStyle.DropDown
            });

            // Remove the old Renderer
            sfDataGrid1.CellRenderers.Remove("ComboBox");

            // Add the custom Renderer
            sfDataGrid1.CellRenderers.Add("ComboBox", new GridCellComboBoxRendererExt());
        }
    }

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
}
