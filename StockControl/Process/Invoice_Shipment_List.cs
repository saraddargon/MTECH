using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using Telerik.WinControls.UI;

namespace StockControl
{
    public partial class Invoice_Shipment_List : Telerik.WinControls.UI.RadRibbonForm
    {
        public string PONo { get; private set; } = "";
        public string CstmNo { get; private set; } = "";

        //sType = 1 : btnNew to Create Customer P/O,,, 2: btnNew to Select Customer P/O
        int sType = 1;
        List<GridViewRowInfo> RetDT;
        string CSTMNo = "";
        string CSTMName = "";
        public Invoice_Shipment_List(string CodeNox)
        {
            InitializeComponent();
            CodeNo = CodeNox;
            //this.Text = "ประวัติ "+ Screen;
        }
        public Invoice_Shipment_List(List<GridViewRowInfo> RetDT, string CSTMNo, string CSTMName)
        {
            InitializeComponent();
            this.RetDT = RetDT;
            this.CSTMNo = CSTMNo;
            this.CSTMName = CSTMName;
            sType = 3;
        }
        string CodeNo = "";
        public Invoice_Shipment_List(int sType = 1)
        {
            InitializeComponent();
            this.sType = sType;
        }
        public Invoice_Shipment_List()
        {
            InitializeComponent();
        }


        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }

        private void Unit_Load(object sender, EventArgs e)
        {
            //radGridView1.ReadOnly = true;

            LoadDefault();
            if (sType == 3)
            {
                cbbCSTM.Text = CSTMName;
                txtCSTMNo.Text = CSTMNo;
                cbbCSTM.Enabled = false;
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


           // demo();
        }
        private void LoadDefault()
        {
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
                    //dgvData.DataSource = db.mh_Shipments.Where(ab=>ab.SSNo.Contains(txtPONo.Text)).ToList();
                    string dt1 = "";
                    string dt2 = "";
                    if (cbChkDate.Checked)
                    {
                        dt1 = Convert.ToDateTime(dtFrom.Value).ToString("yyyyMMdd");
                        dt2 = Convert.ToDateTime(dtTo.Value).ToString("yyyyMMdd");
                    }
                    dgvData.DataSource = db.sp_069_Shipment_List_2(txtPONo.Text,cbbCSTM.Text,cbbItem.Text, dt1, dt2, "", txtCSTMNo.Text);

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;

        }

        void demo()
        {
            //dgvData.DataSource = null;
            //dgvData.Rows.Clear();

            //demorow("Waiting", "SP1809-001", "I0001", "Item A", 100, 0, 100, "PCS", "C00001", "TT FUJI TOOL SUPPORT CO., LTD", "SO1809-001", "JOB1809-001");
            //demorow("Waiting", "SP1809-001", "I0002", "Item B", 100, 0, 100, "PCS", "C00001", "TT FUJI TOOL SUPPORT CO., LTD", "SO1809-001", "JOB1809-002");
            //demorow("Waiting", "SP1809-002", "I0003", "Item C", 100, 0, 100, "PCS", "C00001", "TT FUJI TOOL SUPPORT CO., LTD", "SO1809-001", "JOB1809-003");
            //demorow("Waiting", "SP1809-002", "I0004", "Item D", 100, 0, 100, "PCS", "C00001", "TT FUJI TOOL SUPPORT CO., LTD", "SO1809-001", "JOB1809-004");
            //demorow("Completed", "SP1809-002", "I0005", "Item E", 100, 100, 0, "PCS", "C00001", "TT FUJI TOOL SUPPORT CO., LTD", "SO1809-001", "JOB1809-005");
            //demorow("Waiting", "SP1809-001", "I0001", "Item A", 100, 0, 100, "PCS", "C00001", "TT FUJI TOOL SUPPORT CO., LTD", "SO1809-001", "JOB1809-001");
            //demorow("Waiting", "SP1809-001", "I0002", "Item B", 100, 0, 100, "PCS", "C00001", "TT FUJI TOOL SUPPORT CO., LTD", "SO1809-001", "JOB1809-002");
            //demorow("Waiting", "SP1809-002", "I0003", "Item C", 100, 0, 100, "PCS", "C00001", "TT FUJI TOOL SUPPORT CO., LTD", "SO1809-001", "JOB1809-003");
            //demorow("Waiting", "SP1809-002", "I0004", "Item D", 100, 0, 100, "PCS", "C00001", "TT FUJI TOOL SUPPORT CO., LTD", "SO1809-001", "JOB1809-004");
            //demorow("Waiting", "SP1809-001", "I0001", "Item A", 100, 0, 100, "PCS", "C00001", "TT FUJI TOOL SUPPORT CO., LTD", "SO1809-001", "JOB1809-001");
            //demorow("Waiting", "SP1809-001", "I0002", "Item B", 100, 0, 100, "PCS", "C00001", "TT FUJI TOOL SUPPORT CO., LTD", "SO1809-001", "JOB1809-002");
            //demorow("Waiting", "SP1809-002", "I0003", "Item C", 100, 0, 100, "PCS", "C00001", "TT FUJI TOOL SUPPORT CO., LTD", "SO1809-001", "JOB1809-003");
            //demorow("Waiting", "SP1809-002", "I0004", "Item D", 100, 0, 100, "PCS", "C00001", "TT FUJI TOOL SUPPORT CO., LTD", "SO1809-001", "JOB1809-004");
            //demorow("Completed", "SP1809-002", "I0005", "Item E", 100, 100, 0, "PCS", "C00001", "TT FUJI TOOL SUPPORT CO., LTD", "SO1809-001", "JOB1809-005");
            //demorow("Completed", "SP1809-002", "I0005", "Item E", 100, 100, 0, "PCS", "C00001", "TT FUJI TOOL SUPPORT CO., LTD", "SO1809-001", "JOB1809-005");
            //demorow("Completed", "SP1809-002", "I0005", "Item E", 100, 100, 0, "PCS", "C00001", "TT FUJI TOOL SUPPORT CO., LTD", "SO1809-001", "JOB1809-005");

        }
        void demorow(string SS, string ShipmentNo, string ItemNo, string ItemName, decimal ShipQty, decimal InvQty, decimal RemainQty
            , string Unit, string CustomerNo, string CustomerName, string RefDocNo, string ProductionNo)
        {
            //var rowe = dgvData.Rows.AddNew();
            //rowe.Cells["SS"].Value = SS;
            //rowe.Cells["ShipmentNo"].Value = "SP1809-" + (rowe.Index + 1).ToString("000");
            //rowe.Cells["ItemNo"].Value = ItemNo;
            //rowe.Cells["ItemName"].Value = ItemName;
            //rowe.Cells["ShipQty"].Value = ShipQty;
            //rowe.Cells["InvQty"].Value = InvQty;
            //rowe.Cells["RemainQty"].Value = ShipQty;
            //rowe.Cells["Unit"].Value = Unit;
            //rowe.Cells["CustomerNo"].Value = CustomerNo;
            //rowe.Cells["CustomerName"].Value = CustomerName;
            //rowe.Cells["RefDocNo"].Value = RefDocNo;
            //rowe.Cells["ProductionNo"].Value = ProductionNo;
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
            ////select Item
            //if (sType == 1)
            //{
            //    var t = new SaleOrder();
            //    t.ShowDialog();
            //}
            //else
            //    selRow();
            if(dgvData.Rows.Count>0)
            {
                try
                {
                    dgvData.EndEdit();
                    dgvData.EndUpdate();

                    if (sType == 3)
                    {
                        dgvData.EndEdit();
                        foreach (GridViewRowInfo rowinfo in dgvData.Rows.Where(o => Convert.ToBoolean(o.Cells["S"].Value)))
                        {
                            RetDT.Add(rowinfo);
                        }

                        this.Close();
                    }

                    //using (DataClasses1DataContext db = new DataClasses1DataContext())
                    //{


                    //    int rows1 = 0;
                    //    foreach (GridViewRowInfo rd in dgvData.Rows)
                    //    {

                    //        if (Convert.ToBoolean(rd.Cells["S"].Value))
                    //        {
                    //            mh_ShipmentDT im = db.mh_ShipmentDTs
                    //                .Where(m => m.SSNo == rd.Cells["ShipmentNo"].Value.ToSt()).FirstOrDefault();
                    //            if (im != null)
                    //            {
                    //                rows1 += 1;
                    //                mh_InvoiceDTTemp st = new mh_InvoiceDTTemp();
                    //                st.RNo = rows1;
                    //                st.IVNo = CodeNo;
                    //                //st.UserID = ClassLib.Classlib.User;
                    //                st.ItemNo = rd.Cells["ShipmentNo"].Value.ToSt();
                    //                st.ItemName = im.ItemName;
                    //                st.Description = im.Description;
                    //                st.Qty = 1;
                    //                st.PCSUnit = 1;
                    //                st.LocationItem = im.LocationItem;
                    //                st.UnitPrice = im.UnitPrice;
                    //                st.Amount = im.UnitPrice * 1;
                    //                st.Active = true;
                    //                st.UOM = im.UOM;
                    //                st.RefId = im.id;
                    //                st.RefDocNo = im.SSNo;
                    //                db.mh_InvoiceDTTemps.InsertOnSubmit(st);
                    //                db.SubmitChanges();
                    //            }
                    //        }
                    //    }
                    //}

                }
                catch { }
                this.Close();
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataLoad();
          //  demo();
        }

        private void radGridView1_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            //////select Item from Double click
            ////selRow();
            //// demo();
            //try
            //{
            //    if (e.RowIndex >= 0)
            //    {
            //        Shipment sh = new Shipment(dgvData.Rows[e.RowIndex].Cells["ShipmentNo"].Value.ToSt());
            //        sh.ShowDialog();
            //        DataLoad();
            //    }

            //}catch { }

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


        void CreateInvoice()
        {
            var inv = new Invoice(true);
            inv.ShowDialog();
        }
        private void btnCreateInvoice_Click(object sender, EventArgs e)
        {
            dgvData.EndInit();
            CreateInvoice();
        }
    }


}
