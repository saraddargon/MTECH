using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
namespace StockControl
{
    public partial class SaleOrder_List : Telerik.WinControls.UI.RadRibbonForm
    {
        public string PONo { get; private set; } = "";
        public string CstmNo { get; private set; } = "";

        //sType = 1 : btnNew to Create Customer P/O,,, 2: btnNew to Select Customer P/O
        int sType = 1;

        public SaleOrder_List(int sType = 1)
        {
            InitializeComponent();
            this.sType = sType;
        }
        public SaleOrder_List()
        {
            InitializeComponent();
        }


        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }

        private void Unit_Load(object sender, EventArgs e)
        {
            //radGridView1.ReadOnly = true;
            using (var db = new DataClasses1DataContext())
            {
                var cust = db.mh_Customers.Where(x => x.Active)
                    .Select(x => new CustomerCombo { No = x.No, Name = x.Name }).ToList();
                cust.Add(new CustomerCombo
                {
                    No = "",
                    Name = ""
                });
                cust = cust.OrderBy(x => x.No).ToList();
                cbbCSTM.AutoSizeDropDownToBestFit = true;
                cbbCSTM.MultiColumnComboBoxElement.DisplayMember = "Name";
                cbbCSTM.MultiColumnComboBoxElement.Value = "No";
                cbbCSTM.MultiColumnComboBoxElement.DataSource = cust;
                cbbCSTM.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

                var item = db.mh_Items.Where(x => x.Active)
                    .Select(x => new ItemCombo { Item = x.InternalNo, ItemName = x.InternalName }).ToList();
                item.Add(new ItemCombo
                {
                    Item = "",
                    ItemName = ""
                });
                item = item.OrderBy(x => x.Item).ToList();
                cbbItem.AutoSizeDropDownToBestFit = true;
                cbbItem.DisplayMember = "ItemName";
                cbbItem.ValueMember = "Item";
                cbbItem.DataSource = item;
                cbbItem.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            }

            dgvData.Columns.ToList().ForEach(x =>
            {
                if (x.Name == "S")
                    x.ReadOnly = false;
                else
                    x.ReadOnly = true;
            });

            dgvData.AutoGenerateColumns = false;
            DataLoad();
        }
        private void DataLoad()
        {

            //dt.Rows.Clear();
            try
            {
                dgvData.DataSource = null;
                this.Cursor = Cursors.WaitCursor;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    string pono = txtPONo.Text;
                    string cstmno = txtCSTMNo.Text;
                    string item = cbbItem.SelectedValue.ToSt();
                    DateTime dFrom = (cbChkDate.Checked) ? dtFrom.Value.Date : new DateTime(1999, 1, 1);
                    DateTime dTo = (cbChkDate.Checked) ? dtTo.Value.Date.AddDays(1).AddMinutes(1) : DateTime.MaxValue;

                    var t = db.mh_SaleOrders.Where(x =>
                                x.Active
                                && (x.SONo.Contains(pono))
                                && (x.CustomerNo == cstmno || cstmno == "")
                                && (x.ItemNo == item || item == "")
                                && (x.SODate >= dFrom && x.SODate <= dTo)).ToList();
                    dgvData.DataSource = null;
                    dgvData.AutoGenerateColumns = false;
                    dgvData.DataSource = t;

                    int rNo = 1;
                    dgvData.Rows.ToList().ForEach(x =>
                    {
                        x.Cells["RNo"].Value = rNo++;
                        string cNo = x.Cells["CustomerNo"].Value.ToSt();
                        var c = db.mh_Customers.Where(q => q.No == cNo).FirstOrDefault();
                        if (c != null)
                            x.Cells["CustomerName"].Value = c.Name;
                    });
                }


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;


            demo();
        }

        void demo()
        {
            dgvData.DataSource = null;
            dgvData.Rows.Clear();

            demorow("Ready", "SO1809-001", "I0001", "Item A", 120, 100, 0, "PCS", "C00001", "Customer A", "CSTM001", "");
            demorow("Ready", "SO1809-002", "I0002", "Item B", 100, 100, 0, "PCS", "C00001", "Customer A", "CSTM001", "");
            demorow("Waiting", "SO1809-003", "I0003", "Item C", 0, 100, 0, "PCS", "C00001", "Customer A", "CSTM001", "PD1809-001");
            demorow("Waiting", "SO1809-004", "I0001", "Item D", 50, 100, 0, "PCS", "C00001", "Customer A", "CSTM002", "PD1809-002");
            demorow("Completed", "SO1809-005", "I0001", "Item E", 100, 0, 100, "PCS", "C00001", "Customer A", "CSTM003", "PD1809-003");

        }
        void demorow(string SS, string SONo, string ItemNo, string ItemName, decimal Stock, decimal Qty, decimal ShipQty
            , string Unit, string CustomerNo, string CustomerName, string RefDocNo, string ProductionNo)
        {
            var rowe = dgvData.Rows.AddNew();
            rowe.Cells["SS"].Value = SS;
            rowe.Cells["SONo"].Value = SONo;
            rowe.Cells["ItemNo"].Value = ItemNo;
            rowe.Cells["ItemName"].Value = ItemName;
            rowe.Cells["Stock"].Value = Stock;
            rowe.Cells["Qty"].Value = Qty;
            rowe.Cells["ShipQty"].Value = ShipQty;
            rowe.Cells["Unit"].Value = Unit;
            rowe.Cells["CustomerNo"].Value = CustomerNo;
            rowe.Cells["CustomerName"].Value = CustomerName;
            rowe.Cells["RefDocNo"].Value = RefDocNo;
            rowe.Cells["ProductionNo"].Value = ProductionNo;
        }
        //


        private void btnCancel_Click(object sender, EventArgs e)
        {
            DataLoad();
        }


        int row = -1;
        private void btnExport_Click(object sender, EventArgs e)
        {
            //  dbClss.ExportGridCSV(radGridView1);
            dbClss.ExportGridXlSX(dgvData);
        }


        private void btnFilter1_Click(object sender, EventArgs e)
        {
            dgvData.EnableFiltering = true;
        }

        private void btnUnfilter1_Click(object sender, EventArgs e)
        {
            dgvData.EnableFiltering = false;
        }

        private void radMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radButtonElement1_Click(object sender, EventArgs e)
        {
            //select Item
            if (sType == 1)
            {
                var t = new SaleOrder();
                t.ShowDialog();
            }
            else
                selRow();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //DataLoad();
            demo();
        }

        private void radGridView1_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            ////select Item from Double click
            //selRow();
            demo();

        }
        void selRow()
        {
            if (dgvData.CurrentCell != null && dgvData.CurrentCell.RowIndex >= 0)
            {
                var rowe = dgvData.CurrentCell.RowInfo;
                PONo = rowe.Cells["SONo"].Value.ToSt();
                CstmNo = rowe.Cells["CustomerNo"].Value.ToSt();

                if (sType == 1)
                {
                    var p = new SaleOrder(PONo, CstmNo);
                    p.ShowDialog();
                    DataLoad();
                    PONo = "";
                    CstmNo = "";
                }
                else
                    this.Close();
            }
        }

        private void btn_PrintPR_Click(object sender, EventArgs e)
        {
            //select Item for Print
            //throw new NotImplementedException();
        }


        private void frezzRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvData.Rows.Count > 0)
                {

                    int Row = 0;
                    Row = dgvData.CurrentRow.Index;
                    dbClss.Set_Freeze_Row(dgvData, Row);

                    //foreach (var rd in radGridView1.Rows)
                    //{
                    //    if (rd.Index <= Row)
                    //    {
                    //        radGridView1.Rows[rd.Index].PinPosition = PinnedRowPosition.Top;
                    //    }
                    //    else
                    //        break;
                    //}
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void frezzColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvData.Columns.Count > 0)
                {
                    int Col = 0;
                    Col = dgvData.CurrentColumn.Index;
                    dbClss.Set_Freeze_Column(dgvData, Col);

                    //foreach (var rd in radGridView1.Columns)
                    //{
                    //    if (rd.Index <= Col)
                    //    {
                    //        radGridView1.Columns[rd.Index].PinPosition = PinnedColumnPosition.Left;
                    //    }
                    //    else
                    //        break;
                    //}
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void unFrezzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                dbClss.Set_Freeze_UnColumn(dgvData);
                dbClss.Set_Freeze_UnRows(dgvData);
                //foreach (var rd in radGridView1.Rows)
                //{
                //    radGridView1.Rows[rd.Index].IsPinned = false;
                //}
                //foreach (var rd in radGridView1.Columns)
                //{
                //    radGridView1.Columns[rd.Index].IsPinned = false;                   
                //}

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnCreateSaleOrder_Click(object sender, EventArgs e)
        {

        }
        void CreateSaleOrder()
        {

        }
    }


}
