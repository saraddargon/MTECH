﻿using System;
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
    public partial class Invoice_List : Telerik.WinControls.UI.RadRibbonForm
    {
        public string PONo { get; private set; } = "";
        public string CstmNo { get; private set; } = "";

        //sType = 1 : btnNew to Create Customer P/O,,, 2: btnNew to Select Customer P/O
        int sType = 1;

        public Invoice_List(int sType = 1)
        {
            InitializeComponent();
            this.sType = sType;
        }
        public Invoice_List()
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
            //DataLoad();

            demo();
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

                }


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;

        }

        void demo()
        {
            dgvData.DataSource = null;
            dgvData.Rows.Clear();

            demorow("Completed", "IV0001", "SP1809-001", "SO1809-001", "I0001", "Item A", 100, 50, "PCS", false
                , "C0001", "TT FUJI TOOL SUPPORT CO.,LTD", "CSTMPO1809-001", "JOB1809-001", 80);
            demorow("Completed", "IV0001", "SP1809-001", "SO1809-002", "I0002", "Item B", 100, 100, "PCS", false
                , "C0001", "TT FUJI TOOL SUPPORT CO.,LTD", "CSTMPO1809-002", "JOB1809-002", 90);
            demorow("Completed", "IV0001", "SP1809-001", "SO1809-003", "I0003", "Item C", 100, 100, "PCS", false
                , "C0001", "TT FUJI TOOL SUPPORT CO.,LTD", "CSTMPO1809-003", "JOB1809-003", 100);
            demorow("Completed", "IV0003", "SP1809-001", "SO1809-004", "I0004", "Item D", 100, 100, "PCS", false
                , "C0001", "TT FUJI TOOL SUPPORT CO.,LTD", "CSTMPO1809-004", "JOB1809-004", 110);
            demorow("Cancel", "IV0004", "SP1809-001", "SO1809-005", "I0005", "Item E", 100, 100, "PCS", true
                , "C0001", "TT FUJI TOOL SUPPORT CO.,LTD", "CSTMPO1809-005", "JOB1809-005", 120);
            demorow("Cancel", "IV0004", "SP1809-001", "SO1809-005", "I0005", "Item E", 100, 100, "PCS", true
                , "C0001", "TT FUJI TOOL SUPPORT CO.,LTD", "CSTMPO1809-005", "JOB1809-005", 120);
            demorow("Completed", "IV0001", "SP1809-001", "SO1809-001", "I0001", "Item A", 100, 50, "PCS", false
                , "C0001", "TT FUJI TOOL SUPPORT CO.,LTD", "CSTMPO1809-001", "JOB1809-001", 80);
            demorow("Completed", "IV0001", "SP1809-001", "SO1809-002", "I0002", "Item B", 100, 100, "PCS", false
                , "C0001", "TT FUJI TOOL SUPPORT CO.,LTD", "CSTMPO1809-002", "JOB1809-002", 90);
            demorow("Completed", "IV0001", "SP1809-001", "SO1809-003", "I0003", "Item C", 100, 100, "PCS", false
                , "C0001", "TT FUJI TOOL SUPPORT CO.,LTD", "CSTMPO1809-003", "JOB1809-003", 100);
            demorow("Completed", "IV0003", "SP1809-001", "SO1809-004", "I0004", "Item D", 100, 100, "PCS", false
                , "C0001", "TT FUJI TOOL SUPPORT CO.,LTD", "CSTMPO1809-004", "JOB1809-004", 110);
            demorow("Completed", "IV0001", "SP1809-001", "SO1809-001", "I0001", "Item A", 100, 50, "PCS", false
                , "C0001", "TT FUJI TOOL SUPPORT CO.,LTD", "CSTMPO1809-001", "JOB1809-001", 80);
            demorow("Completed", "IV0001", "SP1809-001", "SO1809-002", "I0002", "Item B", 100, 100, "PCS", false
                , "C0001", "TT FUJI TOOL SUPPORT CO.,LTD", "CSTMPO1809-002", "JOB1809-002", 90);
            demorow("Completed", "IV0001", "SP1809-001", "SO1809-003", "I0003", "Item C", 100, 100, "PCS", false
                , "C0001", "TT FUJI TOOL SUPPORT CO.,LTD", "CSTMPO1809-003", "JOB1809-003", 100);
            demorow("Completed", "IV0003", "SP1809-001", "SO1809-004", "I0004", "Item D", 100, 100, "PCS", false
                , "C0001", "TT FUJI TOOL SUPPORT CO.,LTD", "CSTMPO1809-004", "JOB1809-004", 110);
            demorow("Completed", "IV0001", "SP1809-001", "SO1809-001", "I0001", "Item A", 100, 50, "PCS", false
                , "C0001", "TT FUJI TOOL SUPPORT CO.,LTD", "CSTMPO1809-001", "JOB1809-001", 80);
            demorow("Completed", "IV0001", "SP1809-001", "SO1809-002", "I0002", "Item B", 100, 100, "PCS", false
                , "C0001", "TT FUJI TOOL SUPPORT CO.,LTD", "CSTMPO1809-002", "JOB1809-002", 90);
            demorow("Completed", "IV0001", "SP1809-001", "SO1809-003", "I0003", "Item C", 100, 100, "PCS", false
                , "C0001", "TT FUJI TOOL SUPPORT CO.,LTD", "CSTMPO1809-003", "JOB1809-003", 100);
            demorow("Completed", "IV0003", "SP1809-001", "SO1809-004", "I0004", "Item D", 100, 100, "PCS", false
                , "C0001", "TT FUJI TOOL SUPPORT CO.,LTD", "CSTMPO1809-004", "JOB1809-004", 110);

        }
        void demorow(string SS, string InvNo, string ShipmentNo, string SaleOrderNo, string ItemNo, string ItemName
            , decimal ShipQty, decimal InvQty, string Unit, bool Cancel, string CustomerNo, string CustomerName
            , string CustomerPO, string ProductionNo, decimal UnitPrice)
        {
            var rowe = dgvData.Rows.AddNew();
            rowe.Cells["SS"].Value = SS;
            rowe.Cells["InvNo"].Value = "IV1809-" + (rowe.Index + 1).ToString("000");
            rowe.Cells["ShipmentNo"].Value = ShipmentNo;
            rowe.Cells["SaleOrderNo"].Value = SaleOrderNo;
            rowe.Cells["ItemNo"].Value = ItemNo;
            rowe.Cells["ItemName"].Value = ItemName;
            rowe.Cells["ShipQty"].Value = ShipQty;
            rowe.Cells["InvQty"].Value = InvQty;
            rowe.Cells["Unit"].Value = Unit;
            rowe.Cells["Cancel"].Value = Cancel;
            rowe.Cells["CustomerNo"].Value = CustomerNo;
            rowe.Cells["CustomerName"].Value = CustomerName;
            rowe.Cells["CustomerPO"].Value = CustomerPO;
            rowe.Cells["ProductionNo"].Value = ProductionNo;
            rowe.Cells["UnitPrice"].Value = UnitPrice;
            rowe.Cells["Amount"].Value = InvQty * UnitPrice;
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


        void CreateShipment()
        {
            //var sm = new Shipment(true);
            //sm.ShowDialog();
            dgvData.EndEdit();
            try
            {
                if (dgvData.Rows.Where(x => x.Cells["S"].Value.ToBool()).Count() > 0)
                {
                    //var so = new SaleOrder(0);
                    //so.ShowDialog();
                }
                else
                {
                    baseClass.Warning("Please select data.");
                    return;
                }
            }
            catch (Exception ex)
            {
                baseClass.Warning(ex.Message);
            }
        }
        private void btnCreateShipment_Click(object sender, EventArgs e)
        {
            dgvData.EndEdit();
            CreateShipment();
        }

        private void btn_PrintPR_Click_1(object sender, EventArgs e)
        {
            //INV1809-0020
            //Invoice
           
        }
    }


}
