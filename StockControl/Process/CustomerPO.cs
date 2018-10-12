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
using System.IO;
using OfficeOpenXml;

namespace StockControl
{
    public partial class CustomerPO : Telerik.WinControls.UI.RadRibbonForm
    {
        int t_idCSTMPO = 0;

        public CustomerPO()
        {
            InitializeComponent();
        }
        public CustomerPO(int idCSTMPo)
        {
            InitializeComponent();
            this.t_idCSTMPO = idCSTMPo;
        }

        List<GridViewRowInfo> RetDT;
        string TempNo_temp = "";
        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            HistoryView hw = new HistoryView(this.Name, txtPONo.Text);
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

                LoadDef();
                ClearData();
                btnNew_Click(null, null);

                if (t_idCSTMPO > 0)
                    DataLoad();

                _dtLoad();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void LoadDef()
        {
            using (var db = new DataClasses1DataContext())
            {

                var cus = db.mh_Customers.Where(x => x.Active).Select(x => new { x.No, x.Name }).ToList();
                cbbCSTM.MultiColumnComboBoxElement.AutoSizeDropDownToBestFit = true;
                cbbCSTM.DisplayMember = "Name";
                cbbCSTM.ValueMember = "No";
                cbbCSTM.MultiColumnComboBoxElement.DataSource = cus;
                cbbCSTM.SelectedIndex = -1;

                txtCreateBy.Text = ClassLib.Classlib.User;
                txtCreateDate.Text = DateTime.Now.ToDtString();
            }
        }

        private void DataLoad(bool warningMssg = true)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var t = db.mh_CustomerPOs.Where(x => x.Active && x.id == t_idCSTMPO).FirstOrDefault();
                    if (t != null)
                    {
                        cbbCSTM.SelectedValue = t.CustomerNo;
                        txtCSTMNo.Text = t.CustomerNo;
                        txtPONo.Text = t.CustomerPONo;
                        dtOrderDate.Value = t.OrderDate;
                        txtid.Text = t.id.ToSt();
                        txtCreateDate.Text = t.CreateDate.ToDtString();
                        txtCreateBy.Text = t.CreateBy;

                        var m = db.mh_CustomerPODTs.Where(x => x.Active && x.idCustomerPO == t.id).ToList();
                        dgvData.DataSource = null;
                        dgvData.AutoGenerateColumns = false;
                        dgvData.DataSource = m;

                        bool foundSO = false;
                        dgvData.Rows.ToList().ForEach(x =>
                        {
                            int idDt = x.Cells["id"].Value.ToInt();
                            //var j = db.mh_ProductionOrders.Where(q => q.Active && q.RefDocId == idDt).FirstOrDefault();
                            //if (j != null) x.Cells["JobNo"].Value = j.JobNo;

                            if (!foundSO)
                            {
                                var so = db.mh_SaleOrderDTs.Where(q => q.RefId == idDt && q.Active)
                                    .Join(db.mh_SaleOrders.Where(q => q.Active)
                                    , dt => dt.SONo
                                    , hd => hd.SONo
                                    , (dt, hd) => new { hd, dt }).ToList();
                                if (so.Count > 0)
                                {
                                    btnHistoryOrder.Enabled = true;
                                    foundSO = true;
                                }
                            }
                        });

                        SetRowNo1(dgvData);
                        CallTotal();

                        btnView_Click(null, null);
                    }
                    else if (warningMssg)
                        baseClass.Warning("P/O not found.!!");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        //

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
            txtid.Text = "0";
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
            //btnNew.Enabled = false;
            btnSave.Enabled = true;
            btnDelete.Enabled = true;

            btnAdd_Row.Enabled = true;
            btnDel_Item.Enabled = true;
            btnAddPart.Enabled = true;
            btnGetPrice.Enabled = true;

            cbbCSTM.Enabled = true;
            txtPONo.ReadOnly = false;
            dtOrderDate.ReadOnly = false;
            txtRemark.ReadOnly = false;

            btnHistoryOrder.Enabled = false;

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
            btnGetPrice.Enabled = false;

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
            btnGetPrice.Enabled = true;

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
                int id = txtid.Text.ToInt();
                if (id == 0) { btnNew_Click(null, null); }
                else
                {
                    using (var db = new DataClasses1DataContext())
                    {
                        var m = db.mh_CustomerPOs.Where(x => x.id == id).FirstOrDefault();
                        if (m != null)
                        {
                            var dt = db.mh_CustomerPODTs.Where(x => x.idCustomerPO == m.id && x.Active).ToList();
                            if (dt.Where(x => x.Status != "Waiting").Count() > 0)
                            {
                                baseClass.Warning("Status cannot Delete.\n");
                                return;
                            }

                            if (!baseClass.Question("Do you want to 'Delete' ?"))
                            {
                                return;
                            }

                            m.Active = false;
                            m.UpdateDate = DateTime.Now;
                            m.UpdateBy = Classlib.User;
                            db.SubmitChanges();

                            dbClss.AddHistory(this.Name, "Customer P/O", $"Cancel Customer P/O {m.CustomerPONo}", m.CustomerPONo);
                            btnNew_Click(null, null);
                        }
                        else
                        {
                            baseClass.Warning("Customer P/O not found.\n");
                            return;
                        }
                    }
                }


                baseClass.Info("Delete Customer P/O complete.\n");
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
                else if (txtPONo.Text.Trim() == "")
                    err += " “P/O No.:” is empty \n";
                else
                {
                    int idPO = txtid.Text.ToInt();
                    string po = txtPONo.Text.Trim();
                    string cstmno = txtCSTMNo.Text.Trim();

                    using (var db = new DataClasses1DataContext())
                    {
                        var m = db.mh_CustomerPOs.Where(x => x.Active && x.id != idPO
                         && x.CustomerPONo == po && x.CustomerNo == cstmno).ToList();
                        if (m.Count > 0)
                        {
                            err += " “P/O No.:” is dupplicate. \n";
                        }
                    }
                }
                if (dgvData.Rows.Where(x => x.IsVisible).Count() < 1)
                    err += " “Items:” is empty \n";
                if (err == "")
                {
                    foreach (var item in dgvData.Rows.Where(x => x.IsVisible))
                    {
                        string itemNo = item.Cells["ItemNo"].Value.ToSt();
                        if (itemNo == "") continue;
                        if (item.Cells["ReqDate"].Value == null)
                            err += " “Request Date.:” is empty \n";
                        if (item.Cells["Qty"].Value.ToDecimal() <= 0)
                            err += " “Qty:” is less than 0 \n";

                        if (err != "")
                            break;
                    }
                }
                if (dgvData.Rows.Where(x => x.IsVisible && x.Cells["Status"].Value.ToSt() != "" && x.Cells["Status"].Value.ToSt() != "Waiting").Count() > 0)
                    err += " “Status:” cannot Save \n";

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
                    if (!Check_Save() && baseClass.Question("Do you want to 'Save' ?"))
                        SaveE();
                }
                else
                    baseClass.Warning("Status must only 'New' or 'Edit'");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        void SaveE()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                int id = txtid.Text.ToInt();
                string pono = txtPONo.Text.Trim();
                string cstmNo = txtCSTMNo.Text.Trim();
                DateTime OrderDate = dtOrderDate.Value.Date;
                string remark = txtRemark.Text.Trim();

                using (var db = new DataClasses1DataContext())
                {
                    //hd
                    bool newDoc = false;
                    var hd = db.mh_CustomerPOs.Where(x => x.Active && x.id == id).FirstOrDefault();
                    if (hd == null)
                    {
                        newDoc = true;
                        hd = new mh_CustomerPO();
                        hd.CreateDate = DateTime.Now;
                        hd.CreateBy = Classlib.User;
                        db.mh_CustomerPOs.InsertOnSubmit(hd);
                        dbClss.AddHistory(this.Name, "Customer P/O", $"Add Customer P/O {pono}", pono);
                    }
                    hd.DemandType = 0; //Customer P/O
                    if (!newDoc)
                    {//Edit Customer P/O
                        if (hd.CustomerPONo != pono) dbClss.AddHistory(this.Name, "Customer P/O", $"P/O No. from {hd.CustomerPONo} to {pono}", pono);
                        if (hd.CustomerNo != cstmNo) dbClss.AddHistory(this.Name, "Customer P/O", $"Customer No. from {hd.CustomerNo} to {cstmNo}", pono);
                        if (hd.OrderDate != OrderDate) dbClss.AddHistory(this.Name, "Customer P/O", $"Order date from {hd.OrderDate.Date} to {OrderDate.Date}", pono);
                        if (hd.Remark != remark) dbClss.AddHistory(this.Name, "Customer P/O", $"Remark from {hd.Remark} to {remark}", pono);
                    }

                    hd.CustomerPONo = pono;
                    hd.CustomerNo = cstmNo;
                    hd.OrderDate = OrderDate;
                    hd.Remark = remark;
                    hd.UpdateBy = Classlib.User;
                    hd.UpdateDate = DateTime.Now;
                    hd.Active = true;
                    db.SubmitChanges();

                    //DT
                    foreach (var item in dgvData.Rows)
                    {
                        int idDT = item.Cells["id"].Value.ToInt();
                        if (item.Cells["Status"].Value.ToSt() != "Waiting") continue;
                        string itemNo = item.Cells["ItemNo"].Value.ToSt();
                        if (itemNo == "") continue;
                        var t = db.mh_CustomerPODTs.Where(x => x.id == idDT).FirstOrDefault();
                        if (t == null)
                        {
                            //add
                            t = new mh_CustomerPODT();
                            t.Active = true;
                            db.mh_CustomerPODTs.InsertOnSubmit(t);
                        }
                        DateTime reqDate = item.Cells["ReqDate"].Value.ToDateTime().Value.Date;
                        string ItemName = item.Cells["ItemName"].Value.ToSt();
                        decimal Qty = item.Cells["Qty"].Value.ToDecimal();
                        string uom = item.Cells["UOM"].Value.ToSt();
                        decimal PCSUnit = item.Cells["PCSUnit"].Value.ToDecimal();
                        decimal UnitPrice = item.Cells["UnitPrice"].Value.ToDecimal();
                        decimal Amount = item.Cells["Amount"].Value.ToDecimal();
                        string Remark = item.Cells["Remark"].Value.ToSt();
                        string ReplenishmentType = item.Cells["ReplenishmentType"].Value.ToSt();

                        if (t.id > 0)
                        {
                            if (t.ReqDate != reqDate) dbClss.AddHistory(this.Name, "Customer P/O", $"Request date from {t.ReqDate.Date} to {reqDate.Date}", pono);
                            if (t.ItemNo != itemNo) dbClss.AddHistory(this.Name, "Customer P/O", $"Item No from {t.ItemNo} to {itemNo}", pono);
                            if (t.ItemName != ItemName) dbClss.AddHistory(this.Name, "Customer P/O", $"Item Name from {t.ItemName} to {ItemName}", pono);
                            if (t.Qty != Qty) dbClss.AddHistory(this.Name, "Customer P/O", $"Order Q'ty from {t.Qty} to {Qty}", pono);
                            if (t.UOM != uom) dbClss.AddHistory(this.Name, "Customer P/O", $"UOM from {t.UOM} to {uom}", pono);
                            if (t.PCSUnit != PCSUnit) dbClss.AddHistory(this.Name, "Customer P/O", $"PCS:Unit from {t.PCSUnit} to {PCSUnit}", pono);
                            if (t.UnitPrice != UnitPrice) dbClss.AddHistory(this.Name, "Customer P/O", $"Unit:Price from {t.UnitPrice} to {UnitPrice}", pono);
                            if (t.Amount != Amount) dbClss.AddHistory(this.Name, "Customer P/O", $"Amount from {t.Amount} to {Amount}", pono);
                            if (t.Remark != Remark) dbClss.AddHistory(this.Name, "Customer P/O", $"Remark(Detail) from {t.Remark} to {Remark}", pono);
                            if (t.ReplenishmentType != ReplenishmentType) dbClss.AddHistory(this.Name, "Customer P/O", $"Replenishment Type from {t.ReplenishmentType} to {ReplenishmentType}", pono);
                        }

                        t.idCustomerPO = hd.id;
                        t.ReqDate = reqDate;
                        t.ItemNo = itemNo;
                        t.ItemName = ItemName;
                        t.Qty = Qty;
                        t.UOM = uom;
                        t.PCSUnit = PCSUnit;
                        t.UnitPrice = UnitPrice;
                        t.Amount = Amount;
                        t.Remark = Remark;
                        t.Active = true;
                        t.ReplenishmentType = ReplenishmentType;
                        t.OutSO = item.Cells["OutSO"].Value.ToDecimal();
                        t.OutPlan = item.Cells["OutPlan"].Value.ToDecimal();
                        t.Status = item.Cells["Status"].Value.ToSt();
                        t.OutQty = item.Cells["OutQty"].Value.ToDecimal();
                    }

                    t_idCSTMPO = hd.id;
                    db.SubmitChanges();

                }

                baseClass.Info("Save complete(s).");
                ClearData();
                txtid.Text = t_idCSTMPO.ToSt();
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
                    var itemNo = e.Row.Cells["ItemNo"].Value.ToSt();
                    if (e.Column.Name.Equals("UnitPrice") || e.Column.Name.Equals("Qty"))
                    {
                        if (e.Row.Cells["Qty"].Value.ToDecimal() > 0)
                        {
                            var m = Math.Round(e.Row.Cells["UnitPrice"].Value.ToDecimal() * e.Row.Cells["Qty"].Value.ToDecimal(), 2);
                            e.Row.Cells["Amount"].Value = m;
                        }
                        else
                            dgvData.Rows[e.RowIndex].Cells["Amount"].Value = 0;

                        decimal outso = Math.Round(e.Row.Cells["Qty"].Value.ToDecimal(), 2);
                        var outplan = Math.Round(e.Row.Cells["Qty"].Value.ToDecimal() * e.Row.Cells["PCSUnit"].Value.ToDecimal(), 2);
                        var outqty = outplan;
                        e.Row.Cells["OutSO"].Value = outso;
                        e.Row.Cells["OutPlan"].Value = outplan;
                        e.Row.Cells["OutQty"].Value = outqty;
                        CallTotal();
                    }
                    else if (e.Column.Name.Equals("ItemNo"))
                    {
                        using (var db = new DataClasses1DataContext())
                        {
                            var t = db.mh_Items.Where(x => x.InternalNo.Equals(itemNo)).FirstOrDefault();
                            if (t == null)
                            {
                                baseClass.Warning($"Item no. ({itemNo}) not found.!!");
                                e.Row.Cells["ItemNo"].Value = beginItem;
                                return;
                            }
                            var tU = db.mh_ItemUOMs.Where(x => x.ItemNo == t.InternalNo && x.UOMCode == t.SalesUOM).FirstOrDefault();
                            var pcsunit = (tU != null) ? tU.QuantityPer : 1;

                            //set Tool
                            if (beginItem == "")
                            {
                                decimal outso = Math.Round(e.Row.Cells["Qty"].Value.ToDecimal(), 2);
                                var outplan = Math.Round(e.Row.Cells["Qty"].Value.ToDecimal() * e.Row.Cells["PCSUnit"].Value.ToDecimal(), 2);
                                var outqty = outplan;
                                addRow(e.RowIndex, 0, DateTime.Now, t.InternalNo, t.InternalName
                                    , 1, t.SalesUOM, pcsunit, t.StandardPrice, "", t.ReplenishmentType, outso, outplan, outqty
                                    , "Waiting");
                            }
                            else
                            {
                                e.Row.Cells["ItemName"].Value = t.InternalName;
                                e.Row.Cells["UOM"].Value = t.BaseUOM;
                                e.Row.Cells["PCSUnit"].Value = pcsunit;

                                decimal outso = Math.Round(e.Row.Cells["Qty"].Value.ToDecimal(), 2);
                                var outplan = Math.Round(e.Row.Cells["Qty"].Value.ToDecimal() * e.Row.Cells["PCSUnit"].Value.ToDecimal(), 2);
                                var outqty = outplan;
                                e.Row.Cells["OutSO"].Value = outso;
                                e.Row.Cells["OutPlan"].Value = outplan;
                                e.Row.Cells["OutQty"].Value = outqty;
                            }

                            //
                            SetRowNo1(dgvData);
                            CallTotal();
                        }
                    }
                    else if (e.Column.Name.Equals("UOM"))
                    {
                        var unit = e.Row.Cells["UOM"].Value.ToSt();
                        using (var db = new DataClasses1DataContext())
                        {
                            var u = db.mh_ItemUOMs.Where(x => x.ItemNo == itemNo && x.UOMCode == unit).FirstOrDefault();
                            var pcsunit = (u != null) ? u.QuantityPer : 1;

                            e.Row.Cells["PCSUnit"].Value = pcsunit;
                            decimal outso = Math.Round(e.Row.Cells["Qty"].Value.ToDecimal(), 2);
                            var outplan = Math.Round(e.Row.Cells["Qty"].Value.ToDecimal() * e.Row.Cells["PCSUnit"].Value.ToDecimal(), 2);
                            var outqty = outplan;
                            e.Row.Cells["OutSO"].Value = outso;
                            e.Row.Cells["OutPlan"].Value = outplan;
                            e.Row.Cells["OutQty"].Value = outqty;
                            CallTotal();
                        }
                    }
                    else if (e.Column.Name.Equals("ReqDate"))
                    {
                        //get from price List
                        DateTime? ReqDate = e.Row.Cells["ReqDate"].Value.ToDateTime();
                        if (ReqDate == null) return;
                        var m = baseClass.GetPriceList(itemNo, ReqDate.Value.Date);
                        e.Row.Cells["UnitPrice"].Value = m;
                        CallTotal();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void MasterTemplate_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            if (e.RowIndex >= -1)
            {
                if (e.Row.Cells["Status"].Value.ToSt() != "Waiting"
                    && e.Row.Cells["Status"].Value.ToSt() != "")
                {
                    e.Cancel = true;
                    return;
                }
                //
                string itemNo = e.Row.Cells["ItemNo"].Value.ToSt();
                if (e.Column.Name.Equals("UOM"))
                {
                    using (var db = new DataClasses1DataContext())
                    {
                        var unit = db.mh_ItemUOMs.Where(x => x.ItemNo == itemNo).ToList();
                        unit = unit.Where(x => x.Active.ToBool()).ToList();
                        var c1 = dgvData.Columns["UOM"] as GridViewComboBoxColumn;
                        //c1.ValueMember = "UOMCode";
                        //c1.DisplayMember = "UOMCode";
                        c1.DataSource = unit.Select(x => x.UOMCode).ToList();
                    }
                }
                else if (e.Column.Name.Equals("ItemNo"))
                {
                    beginItem = itemNo;
                }
            }
        }

        void addRow(int rowIndex, int id, DateTime ReqDate, string ItemNo, string ItemName
            , decimal Qty, string UOM, decimal PCSUnit, decimal UnitPrice
            , string Remark, string ReplenishmentType, decimal OutSO, decimal OutPlan, decimal OutQty, string Status)
        {
            var rowE = dgvData.Rows[rowIndex];
            try
            {
                rowE.Cells["id"].Value = id;
                rowE.Cells["ReqDate"].Value = ReqDate;
                rowE.Cells["ItemNo"].Value = ItemNo;
                rowE.Cells["ItemName"].Value = ItemName;
                rowE.Cells["Qty"].Value = Qty;
                rowE.Cells["UOM"].Value = UOM;
                rowE.Cells["PCSUnit"].Value = PCSUnit;
                rowE.Cells["UnitPrice"].Value = UnitPrice;
                rowE.Cells["Amount"].Value = Qty * UnitPrice;
                rowE.Cells["Remark"].Value = Remark;
                rowE.Cells["ReplenishmentType"].Value = ReplenishmentType;
                rowE.Cells["OutSO"].Value = OutSO;
                rowE.Cells["OutPlan"].Value = OutPlan;
                rowE.Cells["OutQty"].Value = OutQty;
                rowE.Cells["Status"].Value = Status;

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

            //if (e.KeyData == (Keys.Control | Keys.S))
            //{
            //    btnSave_Click(null, null);
            //}
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
                if (dgvData.Rows.Count <= 0) return;
                if (dgvData.CurrentCell == null) return;

                var row = dgvData.CurrentCell.RowInfo;
                if (row.Cells["Status"].Value.ToSt() != "Waiting"
                    && row.Cells["Status"].Value.ToSt() != "")
                {
                    baseClass.Warning("Status cannot 'Delete'.\n");
                    return;
                }


                int id = row.Cells["id"].Value.ToInt();
                using (var db = new DataClasses1DataContext())
                {
                    var m = db.mh_CustomerPODTs.Where(x => x.id == id).FirstOrDefault();
                    if (m != null)
                    {
                        m.Active = false;
                        db.SubmitChanges();

                        var d = db.mh_CustomerPOs.Where(x => x.id == m.idCustomerPO).FirstOrDefault();
                        dbClss.AddHistory(this.Name, "Customer P/O", $"Remove Item {m.ItemNo} in Customer PO No. {d.CustomerPONo}", d.CustomerPONo);
                    }

                }
                SetRowNo1(dgvData);
                CallTotal();
                dgvData.Rows.Remove(row);
                //baseClass.Info("Delete complete.\n");
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
                //btnEdit.Enabled = true;
                //btnView.Enabled = false;
                //btnNew.Enabled = true;
                //ClearData();
                //Ac = "View";
                //Enable_Status(false, "View");

                this.Cursor = Cursors.WaitCursor;
                var pol = new CustomerPO_List(2);
                this.Cursor = Cursors.Default;
                pol.ShowDialog();
                if (pol.idCustomerPO > 0)
                {
                    //t_PONo = pol.PONo;
                    //t_CustomerNo = pol.CstmNo;
                    //LoadData
                    t_idCSTMPO = pol.idCustomerPO;
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
                    item.Cells["Amount"].Value = Math.Round(item.Cells["Qty"].Value.ToDecimal() * item.Cells["UnitPrice"].Value.ToDecimal(), 2);
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
                        var priceL = baseClass.GetPriceList(itemNo, DateTime.Now.Date);

                        var tU = db.mh_ItemUOMs.Where(x => x.ItemNo == itemNo && x.UOMCode == t.SalesUOM).FirstOrDefault();
                        decimal u = (tU != null) ? tU.QuantityPer : 1;

                        var rowE = dgvData.Rows.AddNew();
                        //addRow(rowE.Index, DateTime.Now, itemNo, t.InternalName
                        //    , 1, t.BaseUOM, u, 0, 0, 1 * u, 1 * u
                        //    , "", 0, "Waiting", "Waiting", t.ReplenishmentType);
                        var outso = 1 * u;
                        var outplan = 1 * u;
                        var outqty = 1 * u;
                        addRow(rowE.Index, 0, DateTime.Now, itemNo, t.InternalName
                            , 1, t.SalesUOM, u, priceL, "", t.ReplenishmentType, outso, outplan, outqty
                            , "Waiting");
                    }
                    SetRowNo1(dgvData);
                    CallTotal();

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

                string customerPO = txtPONo.Text.Trim();
                string customerNo = cbbCSTM.SelectedValue.ToSt();
                using (var db = new DataClasses1DataContext())
                {
                    var m = db.mh_CustomerPOs
                        .Where(x => x.Active && x.CustomerPONo == txtPONo.Text
                        && x.CustomerNo == customerNo).FirstOrDefault();
                    if (m == null)
                    {
                        baseClass.Warning("Customer P/O not found.\n");
                        t_idCSTMPO = 0;
                        return;
                    }

                    t_idCSTMPO = m.id;
                }


                ClearData();
                DataLoad();
            }
        }

        private void lbJobNo_DoubleClick(object sender, EventArgs e)
        {
            OpenJobOrder();
        }
        private void btnLinkJob_Click(object sender, EventArgs e)
        {
            OpenJobOrder();
        }
        void OpenJobOrder()
        {

        }

        private void MasterTemplate_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.Column.Name.Equals("JobNo"))
                {
                    //string JobNo = e.Row.Cells["JobNo"].Value.ToSt();
                    //linkToJob(JobNo);
                }
            }
        }
        void linkToJob(string JobNo)
        {
            if (JobNo == "") return;
            var j = new ProductionOrder(JobNo);
            j.ShowDialog();
        }

        private void btnHistoryOrder_Click(object sender, EventArgs e)
        {
            OpenSaleorderList();
        }
        void OpenSaleorderList()
        {
            string cstmPO = txtPONo.Text.Trim();
            string cstmNo = cbbCSTM.SelectedValue.ToSt();
            var so = new SaleOrder_List2(cstmPO, cstmNo);
            so.ShowDialog();
        }

        DataTable _dtTemp = new DataTable();
        void _dtLoad()
        {
            _dtTemp = new DataTable();
            _dtTemp.Columns.Add("", typeof(string));
        }
        private void btnExportFile_Click(object sender, EventArgs e)
        {
            ExportE();
        }
        void ExportE()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                _dtLoad();

                string fName = "CustomerPO_Template.xlsx";
                string mFile = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, fName);
                string tFile = Path.Combine(Path.GetTempPath(), fName);
                File.Copy(mFile, tFile, true);

                System.Diagnostics.Process.Start(tFile);
            }
            catch (Exception ex)
            {
                baseClass.Error(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnImportCSV_Click(object sender, EventArgs e)
        {
            var op = new OpenFileDialog();
            op.Filter = "(*.csv)|*.csv";
            if (op.ShowDialog() == DialogResult.OK && op.FileName.ToSt() != "")
                OpenImport(op.FileName);
        }
        void OpenImport(string filePath)
        {
            //MessageBox.Show(filePath);
            this.Cursor = Cursors.WaitCursor;
            try
            {
                using (TextFieldParser parser = new TextFieldParser(filePath, Encoding.GetEncoding("windows-874")))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    int a = 0;
                    int c = 0;

                    int impCom = 0;
                    using (var db = new DataClasses1DataContext())
                    {
                        while (!parser.EndOfData)
                        {
                            a += 1;
                            string[] fields = parser.ReadFields();
                            c = 0;

                            string CustomerNo = "";
                            string CustomerPONo = "";
                            DateTime? OrderDate = null;
                            string Remark = "";
                            DateTime? CustomerReqDate = null;
                            string ItemNo = "";
                            string ItemName = "";
                            decimal OrderQty = 0.00m;
                            string UOM = "";
                            decimal PCSUnit = 0.00m;
                            decimal UnitPrice = 0.00m;
                            decimal Amnt = 0.00m;
                            foreach (string field in fields)
                            {
                                c += 1;
                                if (a < 8) continue;
                                string f = field.ToSt().Trim();
                                switch (c)
                                {
                                    case 2: CustomerNo = f; break;
                                    case 3: CustomerPONo = f; break;
                                    case 4: OrderDate = f.ToDateTime(); break;
                                    case 5: Remark = f; break;
                                    case 6: CustomerReqDate = f.ToDateTime(); break;
                                    case 7: ItemNo = f; break;
                                    case 8: ItemName = f; break;
                                    case 9: OrderQty = f.ToDecimal(); break;
                                    case 10: UOM = f; break;
                                    case 11: PCSUnit = f.ToDecimal(); break;
                                    case 12: UnitPrice = f.ToDecimal(); break;
                                    case 13: Amnt = f.ToDecimal(); break;
                                    default: break;
                                }
                            }
                            //check customerNo
                            var cstm = db.mh_Customers.Where(x => x.No == CustomerNo).FirstOrDefault();
                            if (cstm == null) continue;
                            //check OrderDate
                            if (OrderDate == null) continue;
                            //check CustomerReqDate
                            if (CustomerReqDate == null) continue;
                            //Check ItemNo
                            var tool = db.mh_Items.Where(x => x.InternalNo == ItemNo).FirstOrDefault();
                            if (tool == null) continue;
                            //Check Order Qty
                            if (OrderQty <= 0) continue;
                            //Check UOM
                            var unit = db.mh_ItemUOMs.Where(x => x.ItemNo == ItemNo && x.UOMCode == UOM).FirstOrDefault();
                            if (unit == null) continue;
                            //check PCSUnit
                            if (PCSUnit <= 0) PCSUnit = unit.QuantityPer;

                            Amnt = Math.Round(OrderQty * UnitPrice, 2);

                            //add to CustomerPO
                            var cstmPo = db.mh_CustomerPOs.Where(x => x.CustomerNo == CustomerNo && x.CustomerPONo == CustomerPONo).FirstOrDefault();
                            if (cstmPo == null)
                            {
                                cstmPo = new mh_CustomerPO();
                                db.mh_CustomerPOs.InsertOnSubmit(cstmPo);
                            }
                            cstmPo.Active = true;
                            cstmPo.CreateBy = Classlib.User;
                            cstmPo.CreateDate = DateTime.Now;
                            cstmPo.CustomerNo = CustomerNo;
                            cstmPo.CustomerPONo = CustomerPONo;
                            cstmPo.DemandType = 0;  //Customer P/O
                            cstmPo.OrderDate = OrderDate.Value.Date;
                            cstmPo.Remark = Remark;
                            cstmPo.UpdateBy = Classlib.User;
                            cstmPo.UpdateDate = DateTime.Now;
                            db.SubmitChanges();

                            //add to Customer PO Dt
                            decimal allQ = Math.Round(OrderQty * PCSUnit, 2);
                            var cstmpoDt = new mh_CustomerPODT
                            {
                                Active = true,
                                Amount = Amnt,
                                forSafetyStock = false,
                                genPR = false,
                                idCustomerPO = cstmPo.id,
                                ItemName = ItemName,
                                ItemNo = ItemNo,
                                OutPlan = OrderQty,
                                OutQty = allQ,
                                OutSO = OrderQty,
                                PCSUnit = PCSUnit,
                                Qty = OrderQty,
                                Remark = "",
                                ReplenishmentType = tool.ReplenishmentType,
                                ReqDate = CustomerReqDate.Value.Date,
                                ReqReceiveDate = CustomerReqDate.Value.Date,
                                Status = "Waiting",
                                UnitPrice = UnitPrice,
                                UOM = UOM,
                            };
                            db.mh_CustomerPODTs.InsertOnSubmit(cstmpoDt);
                            db.SubmitChanges();

                            impCom++;
                        }
                    }

                    if (impCom > 0)
                    {
                        baseClass.Info($"Improt Data({impCom}) completes.\n");
                    }
                }
            }
            catch (Exception ex)
            {
                baseClass.Error(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnGetPrice_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (dgvData.Rows.Count > 0 && dgvData.CurrentCell != null)
                {
                    var itemNo = dgvData.CurrentCell.RowInfo.Cells["ItemNo"].Value.ToSt();
                    var reqDate = dgvData.CurrentCell.RowInfo.Cells["ReqDate"].Value.ToDateTime();
                    if (itemNo != "" && reqDate != null)
                        dgvData.CurrentCell.RowInfo.Cells["UnitPrice"].Value = baseClass.GetPriceList(itemNo, reqDate.Value.Date);
                    CallTotal();
                }
            }
            catch (Exception ex)
            {
                baseClass.Error(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }
}
