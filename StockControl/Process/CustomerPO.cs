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
using System.Globalization;
using Microsoft.VisualBasic;
using ClassLib;

namespace StockControl
{
    public partial class CustomerPO : Telerik.WinControls.UI.RadRibbonForm
    {
        string t_PONo = "";
        string t_CustomerNo = "";

        public CustomerPO()
        {
            InitializeComponent();
        }
        public CustomerPO(string PONo, string CustomerNo)
        {
            InitializeComponent();
            this.t_PONo = PONo;
            this.t_CustomerNo = CustomerNo;
        }

        List<GridViewRowInfo> RetDT;
        string TempNo_temp = "";
        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            HistoryView hw = new HistoryView(this.Name, "txtPONo.Text");
            this.Cursor = Cursors.Default;
            hw.ShowDialog();
        }

        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }
        private void GETDTRow()
        {

        }

        string Ac = "";
        private void Unit_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                //dgvData.ReadOnly = true;
                dgvData.AutoGenerateColumns = false;
                GETDTRow();

                using (var db = new DataClasses1DataContext())
                {

                    var cus = db.mh_Customers.Where(x => x.Active).Select(x => new { x.No, x.Name }).ToList();
                    cbbCSTM.MultiColumnComboBoxElement.AutoSizeDropDownToBestFit = true;
                    cbbCSTM.DisplayMember = "Name";
                    cbbCSTM.ValueMember = "No";
                    cbbCSTM.MultiColumnComboBoxElement.DataSource = cus;
                    cbbCSTM.SelectedIndex = -1;
                }

                ClearData();
                btnNew_Click(null, null);

                if (t_PONo != "" && t_CustomerNo != "")
                    DataLoad();


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        private void DataLoad(bool warningMssg = true)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int ck = 0;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var t = db.mh_CustomerPOs.Where(x => x.Active && x.DemandType == 0 && x.CustomerPONo == t_PONo && x.CustomerNo == t_CustomerNo).ToList();
                    if (t.Count > 0)
                    {
                        string CustNo = t.First().CustomerNo;
                        cbbCSTM.SelectedValue = CustNo;
                        txtCSTMNo.Text = CustNo;
                        txtPONo.Text = t_PONo;
                        dtOrderDate.Value = t.First().OrderDate.Date;
                        txtRemark.Text = t.First().RemarkHD;
                        txtTotal.Text = t.Sum(x => x.Amount).ToString("#,0.00");
                        foreach (var tt in t)
                        {
                            var rowe = dgvData.Rows.AddNew();
                            var ss = baseClass.ToEnum<CustomerPO_SS>(tt.Status);
                            addRow(rowe.Index, tt.ReqDate.Date, tt.ItemNo, tt.ItemName
                                , tt.Quantity, tt.UOM, tt.PCSUnit, tt.PricePerUnit, tt.Amount
                                , tt.OutSO, tt.RemarkDT, tt.id, ss, tt.ForcastType);
                        }

                        SetRowNo1(dgvData);
                        CallTotal();

                        btnView_Click(null, null);
                    }
                    else if(warningMssg)
                        baseClass.Warning("P/O not found.!!");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        private void Enable_Status(bool ss, string Condition)
        {
            if (Condition.Equals("-") || Condition.Equals("New"))
            {
            }
            else if (Condition.Equals("View"))
            {
            }
            else if (Condition.Equals("Edit"))
            {
            }
        }

        private void ClearData()
        {
            cbbCSTM.SelectedIndex = -1;
            txtCSTMNo.Text = "";
            txtPONo.Text = "";
            dtOrderDate.Value = DateTime.Today;
            dgvData.Rows.Clear();
            dgvData.DataSource = null;
            txtRemark.Text = "";
            txtTotal.Text = (0).ToMoney();
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            btnView.Enabled = true;
            btnEdit.Enabled = true;
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            btnDelete.Enabled = true;

            btnAdd_Row.Enabled = true;
            btnDel_Item.Enabled = true;
            btnAddPart.Enabled = true;
         
            cbbCSTM.Enabled = true;
            txtPONo.ReadOnly = false;
            dtOrderDate.ReadOnly = false;
            txtRemark.ReadOnly = false;

            dgvData.ReadOnly = false;

            ClearData();
            Enable_Status(true, "New");
            lblStatus.Text = "New";
            Ac = "New";
            row = dgvData.Rows.Count - 1;
            if (row < 0)
                row = 0;
            //getมาไว้ก่อน แต่ยังไมได้ save 
            //txtTempNo.Text = StockControl.dbClss.GetNo(10, 0);

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            btnView.Enabled = false;
            btnEdit.Enabled = true;
            btnNew.Enabled = true;
            btnSave.Enabled = true;

            btnAdd_Row.Enabled = false;
            btnDel_Item.Enabled = false;
            btnAddPart.Enabled = false;

            dgvData.ReadOnly = true;
            
            cbbCSTM.Enabled = false;
            txtPONo.ReadOnly = true;
            dtOrderDate.ReadOnly = true;
            txtRemark.ReadOnly = true;

            Enable_Status(false, "View");
            lblStatus.Text = "View";
            Ac = "View";
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            btnView.Enabled = true;
            btnEdit.Enabled = false;
            btnNew.Enabled = true;
            btnSave.Enabled = true;
            btnDelete.Enabled = true;

            btnAdd_Row.Enabled = true;
            btnDel_Item.Enabled = true;
            btnAddPart.Enabled = true;
            
            cbbCSTM.Enabled = true;
            txtPONo.ReadOnly = false;
            dtOrderDate.ReadOnly = false;
            txtRemark.ReadOnly = false;

            dgvData.ReadOnly = false;


            Enable_Status(true, "Edit");
            lblStatus.Text = "Edit";
            Ac = "Edit";

        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string poNo = txtPONo.Text.Trim();
                string cstmNo = txtCSTMNo.Text.Trim();
                if (poNo != "" && cstmNo != "")
                {
                    if (baseClass.IsDel($"Do you want to Delete Customer P/O {poNo} ?"))
                    {
                        using (var db = new DataClasses1DataContext())
                        {
                            var p = db.mh_CustomerPOs.Where(x => x.CustomerNo == cstmNo && x.DemandType == 0 && x.CustomerPONo == poNo && x.Active).ToList();
                            if (p.Where(x => x.Status > 0).Count() < 1)
                            {
                                foreach (var pp in p)
                                {
                                    pp.Active = false;
                                    pp.UpdateBy = Classlib.User;
                                    pp.UpdateDate = DateTime.Now;
                                }

                                db.SubmitChanges();

                                baseClass.Info("Delete P/O complete.");
                                ClearData();
                                btnNew_Click(null, null);
                            }
                            else
                                baseClass.Warning("P/O Status cannot Delete.");
                        }
                    }
                }
                //if(row >= 0)
                //{
                //    string itemNo = dgvData.Rows[row].Cells["Item"].Value.ToSt();
                //    if (baseClass.IsDel($"Do you want to Delete item ({itemNo}) ?"))
                //    {
                //        var rowe = dgvData.Rows[row];
                //        if (rowe.Cells["id"].Value.ToInt() <= 0)
                //            dgvData.Rows.Remove(rowe);
                //        else
                //        {
                //            using (var db = new DataClasses1DataContext())
                //            {
                //                int id = rowe.Cells["id"].Value.ToInt();
                //                var m = db.mh_CustomerPOs.Where(x => x.id == id).FirstOrDefault();
                //                if (m != null)
                //                {
                //                    m.Active = false;
                //                    m.UpdateDate = DateTime.Now;
                //                    m.UpdateBy = Classlib.User;
                //                    db.SubmitChanges();
                //                    dgvData.Rows.Remove(rowe);
                //                }
                //            }
                //        }

                //        SetRowNo1(dgvData);
                //        CallTotal();
                //        if (dgvData.CurrentRow != null)
                //            row = dgvData.CurrentRow.Index;
                //        else
                //            row = -1;
                //    }
                //}
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }

        }
        private bool Check_Save()
        {
            bool re = true;
            string err = "";
            try
            {
                if (cbbCSTM.SelectedValue.ToSt() == "" || txtCSTMNo.Text == "")
                    err += " “Customer:” is empty \n";
                if (txtPONo.Text.Trim() == "")
                    err += " “P/O No.:” is empty \n";
                if (dgvData.Rows.Where(x => x.IsVisible).Count() < 1)
                    err += " “Items:” is empty \n";
                if (err == "")
                {
                    foreach (var item in dgvData.Rows.Where(x => x.IsVisible))
                    {
                        string itemNo = item.Cells["Item"].Value.ToSt();
                        if (itemNo == "") continue;
                        if (item.Cells["ReqDate"].Value == null)
                            err += " “Request Date.:” is empty \n";
                        if (item.Cells["Qty"].Value.ToDecimal() <= 0)
                            err += " “Qty:” is less than 0 \n";

                        if (err != "")
                            break;
                    }
                }

                if (!err.Equals(""))
                    MessageBox.Show(err);
                else
                    re = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("CreatePO", ex.Message, this.Name);
            }

            return re;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Ac.Equals("New") || Ac.Equals("Edit"))
                {
                    if (Check_Save())
                        return;
                    else if (baseClass.IsSave())
                        SaveE();
                }
                else
                    MessageBox.Show("สถานะต้องเป็น New หรือ Edit เท่านั่น");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        void SaveE()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                string pono = txtPONo.Text;
                string cstmNo = txtCSTMNo.Text;
                using (var db = new DataClasses1DataContext())
                {
                    foreach (var item in dgvData.Rows)
                    {
                        int id = item.Cells["id"].Value.ToInt();
                        var t = db.mh_CustomerPOs.Where(x => x.id == id).FirstOrDefault();
                        if (t != null)
                        {
                            //edit
                            t.Active = item.IsVisible;
                        }
                        else if (item.IsVisible)
                        {
                            //add
                            t = new mh_CustomerPO();
                        t.Active = true;
                            db.mh_CustomerPOs.InsertOnSubmit(t);
                        }
                        else
                            continue;
                        t.DemandType = 0;//Customer P/O
                        t.Amount = item.Cells["Amount"].Value.ToDecimal();
                        t.id = 0;
                        t.CreateBy = Classlib.User;
                        t.CreateDate = DateTime.Now;
                        t.UpdateBy = Classlib.User;
                        t.UpdateDate = DateTime.Now;
                        t.CustomerNo = cstmNo;
                        t.CustomerPONo = pono;
                        t.id = item.Cells["id"].Value.ToInt();
                        t.ItemName = item.Cells["ItemName"].Value.ToSt();
                        t.ItemNo = item.Cells["Item"].Value.ToSt();
                        t.OrderDate = dtOrderDate.Value.Date;
                        t.OutSO = item.Cells["OutSO"].Value.ToDecimal();
                        t.PCSUnit = item.Cells["PCSUnit"].Value.ToDecimal();
                        t.PricePerUnit = item.Cells["PricePerUnit"].Value.ToDecimal();
                        t.Quantity = item.Cells["Qty"].Value.ToDecimal();
                        t.RemarkDT = item.Cells["Remark"].Value.ToSt();
                        t.RemarkHD = txtRemark.Text;
                        t.ReqDate = item.Cells["ReqDate"].Value.ToDateTime().Value.Date;
                        t.Status = item.Cells["Status"].Value.ToInt();
                        t.UOM = item.Cells["Unit"].Value.ToSt();
                        t.ForcastType = item.Cells["ForcastType"].Value.ToSt();
                        //t.ReqReceiveDate = item.Cells["ReqReceiveDate"].Value.ToDateTime();
                    }

                    t_PONo = pono;
                    t_CustomerNo = cstmNo;
                    db.SubmitChanges();
                }

                baseClass.Info("Save complete(s).");
                ClearData();
                txtCSTMNo.Text = t_CustomerNo;
                txtPONo.Text = t_PONo;
                DataLoad();
            }
            catch (Exception ex)
            {
                baseClass.Error(ex.Message);
            }
            finally { this.Cursor = Cursors.Default; }
        }

        string beginItem = "";
        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                dgvData.EndEdit();
                if (e.RowIndex >= -1)
                {
                    var itemNo = e.Row.Cells["Item"].Value.ToSt();
                    if (e.Column.Name.Equals("Amount") || e.Column.Name.Equals("Qty"))
                    {
                        if (e.Row.Cells["Qty"].Value.ToDecimal() > 0)
                        {
                            var m = Math.Round(e.Row.Cells["Amount"].Value.ToDecimal() / e.Row.Cells["Qty"].Value.ToDecimal(), 2);
                            dgvData.Rows[e.RowIndex].Cells["PricePerUnit"].Value = m;
                        }
                        else
                            dgvData.Rows[e.RowIndex].Cells["PricePerUnit"].Value = 0;

                        CallTotal();
                    }
                    else if (e.Column.Name.Equals("Item"))
                    {
                        using (var db = new DataClasses1DataContext())
                        {
                            var t = db.mh_Items.Where(x => x.InternalNo.Equals(itemNo)).FirstOrDefault();
                            if (t == null)
                            {
                                baseClass.Warning($"Item no. ({itemNo}) not found.!!");
                                e.Row.Cells["Item"].Value = beginItem;
                                return;
                            }
                            var tU = db.mh_ItemUOMs.Where(x => x.ItemNo == t.InternalNo && x.UOMCode == t.BaseUOM).FirstOrDefault();
                            var pcsunit = (tU != null) ? tU.QuantityPer : 1;

                            //set Tool
                            if (beginItem == "")
                            {
                                addRow(e.RowIndex
                                    , DateTime.Now, t.InternalNo, t.InternalName
                                    , 1, t.BaseUOM, pcsunit, 0, 0
                                    , 1, "", 0, CustomerPO_SS.Waiting, t.ReplenishmentType);
                            }
                            else
                            {
                                e.Row.Cells["ItemName"].Value = t.InternalName;
                                e.Row.Cells["Unit"].Value = t.BaseUOM;
                                e.Row.Cells["PCSUnit"].Value = pcsunit;
                            }

                            //
                            SetRowNo1(dgvData);
                        }
                    }
                    else if (e.Column.Name.Equals("Unit"))
                    {
                        var unit = e.Row.Cells["Unit"].Value.ToSt();
                        using (var db = new DataClasses1DataContext())
                        {
                            var u = db.mh_ItemUOMs.Where(x => x.ItemNo == itemNo && x.UOMCode == unit).FirstOrDefault();
                            var pcsunit = (u != null) ? u.QuantityPer : 1;

                            e.Row.Cells["PCSUnit"].Value = pcsunit;
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void MasterTemplate_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            if (e.RowIndex >= -1)
            {
                string itemNo = e.Row.Cells["Item"].Value.ToSt();
                if (e.Column.Name.Equals("Unit"))
                {
                    using (var db = new DataClasses1DataContext())
                    {
                        var unit = db.mh_ItemUOMs.Where(x => x.ItemNo == itemNo).ToList();
                        unit = unit.Where(x => x.Active.ToBool()).ToList();
                        var c1 = dgvData.Columns["Unit"] as GridViewComboBoxColumn;
                        c1.ValueMember = "UOMCode";
                        c1.DisplayMember = "UOMCode";
                        c1.DataSource = unit;
                    }
                }
                else if (e.Column.Name.Equals("Item"))
                {
                    beginItem = itemNo;
                }
            }
        }
        void addRow(int rowIndex, DateTime ReqDate, string ItemNo, string ItemName, decimal Qty
            , string UOM, decimal PCSUnit, decimal PricePerUnit, decimal Amount
            , decimal OutSO, string RemarkDT, int id, CustomerPO_SS Status, string forcastType)
        {
            var rowE = dgvData.Rows[rowIndex];
            try
            {
                rowE.Cells["ReqDate"].Value = ReqDate;
                rowE.Cells["Item"].Value = ItemNo;
                rowE.Cells["ItemName"].Value = ItemName;
                rowE.Cells["Qty"].Value = Qty;
                rowE.Cells["Unit"].Value = UOM;
                rowE.Cells["PCSUnit"].Value = PCSUnit;
                rowE.Cells["PricePerUnit"].Value = PricePerUnit;
                rowE.Cells["Amount"].Value = Amount;
                rowE.Cells["OutSO"].Value = OutSO;
                rowE.Cells["Remark"].Value = RemarkDT;
                rowE.Cells["id"].Value = id;
                rowE.Cells["Status"].Value = Status.ToInt();
                rowE.Cells["ForcastType"].Value = forcastType;

                SetRowNo1(dgvData);
            }
            catch (Exception ex)
            {
                baseClass.Error(ex.Message);
            }
        }
        
        private void radGridView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // MessageBox.Show(e.KeyCode.ToString());

            if (e.KeyData == (Keys.Control | Keys.S))
            {
                btnSave_Click(null, null);
            }
        }



        int row = -1;
        private void radGridView1_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            row = e.RowIndex;
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

        private void radPanel1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void เพมพารทToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        private bool check_Duppicate(string CodeNo)
        {
            bool re = false;
            foreach (var rd1 in dgvData.Rows)
            {
                if (rd1.IsVisible.Equals(true))
                {
                    if (StockControl.dbClss.TSt(rd1.Cells["dgvCodeNo"].Value).Equals(CodeNo))
                        re = true;
                }
            }

            return re;

        }
        private void ลบพารทToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                if (dgvData.Rows.Count < 0)
                    return;


                if (Ac.Equals("New") || Ac.Equals("Edit"))
                {
                    this.Cursor = Cursors.WaitCursor;


                    if (dgvData.CurrentRow.Cells["Status"].Value.ToInt() == CustomerPO_SS.Waiting.ToInt())
                    {

                        int id = 0;
                        int.TryParse(StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["id"].Value), out id);
                        if (id <= 0)
                            dgvData.Rows.Remove(dgvData.CurrentRow);

                        else
                        {
                            row = dgvData.CurrentRow.Index;
                            btnDelete_Click(null, null);
                        }
                        CallTotal();
                        //getTotal();
                        SetRowNo1(dgvData);
                    }
                    else
                        MessageBox.Show("ไม่สามารถทำการลบรายการได้ สถานะไม่ถูกต้อง");
                }
                else
                {
                    MessageBox.Show("ไม่สามารถทำการลบรายการได้");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        public static void SetRowNo1(RadGridView Grid)//เลขลำดับ
        {
            int i = 1;
            Grid.Rows.Where(o => o.IsVisible).ToList().ForEach(o =>
            {
                o.Cells["RNo"].Value = i;
                i++;
            });
        }


        private void MasterTemplate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                ลบพารทToolStripMenuItem_Click(null, null);
        }

        private void btnListItem_Click(object sender, EventArgs e)
        {
            ////DataLoad();
            try
            {
                btnEdit.Enabled = true;
                btnView.Enabled = false;
                btnNew.Enabled = true;
                ClearData();
                Ac = "View";
                Enable_Status(false, "View");

                this.Cursor = Cursors.WaitCursor;
                var pol = new CustomerPO_List(2);
                this.Cursor = Cursors.Default;
                pol.ShowDialog();
                if (pol.PONo != "" && pol.CstmNo != "")
                {
                    t_PONo = pol.PONo;
                    t_CustomerNo = pol.CstmNo;
                    //LoadData
                    DataLoad();
                }


                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError("CreatePart", ex.Message + " : radButtonElement1_Click", this.Name); }
            finally { this.Cursor = Cursors.Default; }



        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            btnEdit.Enabled = true;
            btnView.Enabled = false;
            btnNew.Enabled = true;
            ClearData();
            Enable_Status(false, "View");
            DataLoad();
            Ac = "View";
        }


        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void radButtonElement3_Click(object sender, EventArgs e)
        {
            //string RefPO = "";
            //string TempNo = txtTempNo.Text;
            //if (!txtTempNo.Text.Equals(""))
            //{
            //    string GetMarkup = Interaction.InputBox("ใส่เลขที่ P/O ใหม่!", "P/O New : ", "", 400, 250);
            //    if (!GetMarkup.Trim().Equals(""))
            //    {
            //        RefPO = GetMarkup;
            //        using (DataClasses1DataContext db = new DataClasses1DataContext())
            //        {
            //            db.sp_UpdatePO(TempNo, RefPO);
            //        }
            //        MessageBox.Show("Update Completed.");
            //        btnRefresh_Click(sender, e);
            //    }
            //}
        }

        private void btnAdd_Row_Click(object sender, EventArgs e)
        {
            try
            {
                dgvData.Rows.AddNew();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void btnDiscon_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (lblStatus.Text != "Completed")
            //    {
            //        lblStatus.Text = "Discon";
            //        Ac = "Discon";
            //        int cc = 0;
            //        if (MessageBox.Show("ต้องการยกเลิกรายการ ( " + txtPONo.Text + " ) หรือไม่ ?", "ยกเลิกรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //        {
            //            this.Cursor = Cursors.WaitCursor;
            //            using (DataClasses1DataContext db = new DataClasses1DataContext())
            //            {

            //                foreach (var g in dgvData.Rows)
            //                {
            //                    if (g.IsVisible.Equals(true) && StockControl.dbClss.TBo(g.Cells["dgvDiscon_B"].Value))
            //                    {

            //                        if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) > 0
            //                            && StockControl.dbClss.TSt(g.Cells["dgvStatus"].Value) != "Full"
            //                            && StockControl.dbClss.TSt(g.Cells["dgvStatus"].Value) == "Partial"
            //                            )
            //                        {
            //                            foreach (DataRow row in dt_PODT.Rows)
            //                            {
            //                                if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) == StockControl.dbClss.TInt(row["id"]))
            //                                {
            //                                    var u = (from ix in db.tb_PurchaseOrderDetails
            //                                             where ix.TempPNo == txtTempNo.Text.Trim()
            //                                             // && ix.TempNo == txtTempNo.Text                                             
            //                                             && ix.id == StockControl.dbClss.TInt(g.Cells["dgvid"].Value)
            //                                             && ix.BackOrder > 0
            //                                             select ix).First();

            //                                    u.Discon = u.BackOrder;
            //                                    u.BackOrder = 0;
            //                                    cc += 1;

            //                                    db.SubmitChanges();
            //                                    dbClss.AddHistory(this.Name, "แก้ไข Item PO", "แก้ไขยกเลิกการรับส่วนที่เหลือ [" + u.CodeNo.ToString() + "]", txtPONo.Text);

            //                                    db.sp_010_Update_StockItem(StockControl.dbClss.TSt(g.Cells["dgvCodeNo"].Value), "BackOrder");
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //                //Calculate Status
            //                db.sp_022_POHD_Cal_Status(txtTempNo.Text, txtPONo.Text);

            //            }

            //            if (cc > 0)
            //                MessageBox.Show("บันทึกรายการ สำเร็จ!");
            //            btnRefresh_Click(null, null);

            //        }
            //    }

            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
            //finally { this.Cursor = Cursors.Default; }
        }


        private void CallDiscontLast(bool am)
        {
            //try
            //{
            //    decimal TaxBase = 0;
            //    decimal Amount = 0;
            //    decimal DisP = 0;
            //    decimal DisA = 0;

            //    decimal.TryParse(lbOrderSubtotal.Text, out TaxBase);
            //    decimal.TryParse(txtLessPoDiscountAmount.Text, out DisA);
            //    decimal.TryParse(txtLessPoDiscountAmountPersen.Text, out DisP);
            //    //decimal SumDis = 0;
            //    dgvData.EndEdit();
            //    foreach (var r2 in dgvData.Rows)
            //    {
            //        Amount = 0;
            //        decimal.TryParse(Convert.ToString(r2.Cells["dgvAmount"].Value), out Amount);
            //        if (!am) // Persent
            //        {
            //            //r2.Cells["dgvdiscount"].Value = (Amount*DisP) / 100;
            //            r2.Cells["dgvDF"].Value = 4;
            //            r2.Cells["dgvDiscountAmount"].Value = ((Amount * DisP) / 100);
            //            r2.Cells["dgvDiscountExt"].Value = ((Amount * DisP) / 100);
            //            r2.Cells["dgvDiscount"].Value = (((Amount * DisP) / 100) / Amount) * 100;
            //            // SumDis += ((Amount * DisP) / 100);
            //        }
            //        else // Amount
            //        {
            //            // MessageBox.Show("xx" + TaxBase+","+Amount);

            //            r2.Cells["dgvDF"].Value = 5;
            //            r2.Cells["dgvDiscountAmount"].Value = ((Amount * DisA) / TaxBase);
            //            r2.Cells["dgvDiscountExt"].Value = ((Amount * DisA) / TaxBase);
            //            r2.Cells["dgvDiscount"].Value = (((Amount * DisA) / TaxBase) / Amount) * 100;
            //        }
            //    }

            //}
            //catch { }
        }

        private void CallSumDiscountLast(bool am)
        {
            //try
            //{
            //    decimal UnitCost = 0;
            //    decimal ExtendedCost = 0;
            //    decimal Qty = 0;
            //    decimal PA = 0;
            //    decimal PR = 0;
            //    decimal SumP = 0;
            //    decimal SumA = 0;
            //    dgvData.EndEdit();
            //    foreach (var r2 in dgvData.Rows)
            //    {
            //        UnitCost = 0;
            //        Qty = 0;
            //        PA = 0;
            //        PR = 0;

            //        decimal.TryParse(Convert.ToString(r2.Cells["dgvOrderQty"].Value), out Qty);
            //        decimal.TryParse(Convert.ToString(r2.Cells["dgvCost"].Value), out UnitCost);
            //        decimal.TryParse(Convert.ToString(r2.Cells["dgvDiscountAmount"].Value), out PA);
            //        decimal.TryParse(Convert.ToString(r2.Cells["dgvDiscount"].Value), out PR);
            //        decimal.TryParse(Convert.ToString(r2.Cells["dgvAmount"].Value), out ExtendedCost);

            //        SumP += ExtendedCost;
            //        SumA += PA;
            //    }
            //    if (am)
            //    {
            //        txtLessPoDiscountAmountPersen.Text = ((SumA / SumP) * 100).ToString("##0.00");
            //    }
            //    else
            //    {

            //        txtLessPoDiscountAmount.Text = (SumA).ToString("###,###,##0.00");
            //    }
            //}
            //catch { }
        }
        private void CallTotal()
        {
            try
            {
                decimal amnt = 0.00m;
                foreach (var item in dgvData.Rows.Where(x => x.IsVisible))
                {
                    amnt += item.Cells["Amount"].Value.ToDecimal();
                }
                txtTotal.Text = amnt.ToString("#,0.00");
            }
            catch (Exception ex) { MessageBox.Show("err2: " + ex.Message); }
        }

        private void cbbCSTM_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtCSTMNo.Text = cbbCSTM.SelectedValue.ToSt();
            }
            catch
            {

                txtCSTMNo.Text = "";
            }
        }

        private void btnAddPart_Click(object sender, EventArgs e)
        {
            AddPartE();
        }
        void AddPartE()
        {
            var m = new List<GridViewRowInfo>();
            var selP = new ListPart_CreatePR(m);
            selP.ShowDialog();
            if (m.Count() > 0)
            {
                using (var db = new DataClasses1DataContext())
                {
                    foreach (var item in m)
                    {
                        var itemNo = item.Cells["CodeNo"].Value.ToSt();
                        var t = db.mh_Items.Where(x => x.InternalNo == itemNo).FirstOrDefault();
                        if (t == null)
                        {
                            baseClass.Warning($"Item ({itemNo} not found.!!!");
                            return;
                        }

                        var tU = db.mh_ItemUOMs.Where(x => x.ItemNo == itemNo && x.UOMCode == t.BaseUOM).FirstOrDefault();
                        decimal u = (tU != null) ? tU.QuantityPer : 1;

                        var rowE = dgvData.Rows.AddNew();
                        addRow(rowE.Index, DateTime.Now, itemNo, t.InternalName
                            , 1, t.BaseUOM, u, 0, 0, 0
                            , "", 0, CustomerPO_SS.Waiting, t.ReplenishmentType);
                    }
                    SetRowNo1(dgvData);

                }
            }
        }

        private void txtPONo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtPONo.Text.Trim() == "")
                {
                    baseClass.Warning("Please enter P/O no.");
                    return;
                }
                if (txtCSTMNo.Text.Trim() == "")
                {
                    baseClass.Warning("Please enter Customer no.");
                    return;
                }

                t_PONo = txtPONo.Text;
                t_CustomerNo = txtCSTMNo.Text;
                ClearData();
                DataLoad();
            }
        }
    }
}
