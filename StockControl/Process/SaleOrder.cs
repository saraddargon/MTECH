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
    public partial class SaleOrder : Telerik.WinControls.UI.RadRibbonForm
    {
        string t_SONo = "";
        string t_CustomerNo = "";
        //Customer P/O to SaleORder
        List<int> idList = new List<int>();
        //List<po_to_so> potoso = new List<po_to_so>();

        public SaleOrder()
        {
            InitializeComponent();
        }
        public SaleOrder(string PONo)
        {
            InitializeComponent();
            this.t_SONo = PONo;
        }
        public SaleOrder(string PONo, string CustomerNo)
        {
            InitializeComponent();
            this.t_SONo = PONo;
            this.t_CustomerNo = CustomerNo;
        }
        public SaleOrder(List<int> idList)
        {
            InitializeComponent();
            this.idList = idList;
        }


        private void Unit_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                //dgvData.ReadOnly = true;
                dgvData.AutoGenerateColumns = false;
                GETDTRow();
                LoadDefault();

                var a = new List<int>();
                foreach (var item in idList)
                {
                    a.Add(item);
                }

               
                btnNew_Click(null, null);

                foreach (var item in a)
                {
                    idList.Add(item);
                }

                if (t_SONo != "" && t_CustomerNo != "")
                    DataLoad();
                else if (t_SONo!="")
                    DataLoad();
                else if (idList.Count > 0)
                    LoadFromId();
                

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void LoadDefault()
        {
            using (var db = new DataClasses1DataContext())
            {
                var cus = db.mh_Customers.Where(x => x.Active).Select(x => new { x.No, x.Name }).ToList();
                cbbCSTM.MultiColumnComboBoxElement.AutoSizeDropDownToBestFit = true;
                cbbCSTM.DisplayMember = "Name";
                cbbCSTM.ValueMember = "No";
                cbbCSTM.MultiColumnComboBoxElement.DataSource = cus;
                cbbCSTM.SelectedIndex = -1;

                var lo = db.mh_Locations.Where(x => x.Active).ToList();
                var com = dgvData.Columns["LocationItem"] as GridViewComboBoxColumn;
                com.DisplayMember = "Name";
                com.ValueMember = "Code";
                com.DataSource = lo;

                var vt = db.mh_VATTypes.Where(x => x.Active.Value).ToList();
                // VatType
                var com3 = dgvData.Columns["VatType"] as GridViewComboBoxColumn;
                com3.DisplayMember = "VatType";
                com3.ValueMember = "VatType";
                com3.DataSource = vt;

                var uom = db.mh_Units.Where(x => x.UnitActive.Value).ToList();
                //UnitCode UnitDetail
                var com4 = dgvData.Columns["Unit"] as GridViewComboBoxColumn;
                com4.DisplayMember = "UnitDetail";
                com4.ValueMember = "UnitCode";
                com4.DataSource = uom;
            }
        }

        //
        private void DataLoad(bool warningMssg = true)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int ck = 0;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var t = db.mh_SaleOrders.Where(x => x.Active && x.SONo == t_SONo).FirstOrDefault();
                    if (t != null)
                    {
                        string CustNo = t.CustomerNo;
                        txtSONo.Text = t_SONo;
                        cbbCSTM.SelectedValue = CustNo;
                        txtCSTMNo.Text = CustNo;
                        dtSODate.Value = t.SODate;
                        txtAddress.Text = t.CustomerAddress;
                        txtRemark.Text = t.Remark;
                        txtTotal.Value = t.TotalPrice;
                        txtCreateDate.Text = t.CreateDate.ToDtString();
                        txtCreateBy.Text = t.CreateBy;

                        dgvData.Rows.Clear();
                        var dt = db.mh_SaleOrderDTs.Where(x => x.Active && x.SONo == t_SONo).ToList();
                        if (dt.Count>0)
                        {
                            dgvData.DataSource = dt;
                        }


                            SetRowNo1(dgvData);
                        CallTotal();

                        btnView_Click(null, null);

                        lblStatus.Text = t.Status.ToSt();
                    }
                    else if (warningMssg)
                        baseClass.Warning("Sale Order not found.!!");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        private void LoadFromId()
        {
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    bool fRow = true;
                    foreach (var id in idList)
                    {
                        var c = db.mh_CustomerPODTs.Where(x => x.id == id).ToList();
                        if (fRow)
                        {
                            var dd = db.mh_CustomerPOs.Where(x => x.id == dbClss.TInt(c.FirstOrDefault().idCustomerPO)).ToList();
                            if (dd.Count > 0)
                            {
                                txtCSTMNo.Text = dbClss.TSt(dd.FirstOrDefault().CustomerNo);
                                cbbCSTM.SelectedValue = dbClss.TSt(dd.FirstOrDefault().CustomerNo);
                                dtSODate.Value = DateTime.Now;
                                cbbCSTM_SelectedIndexChanged(null, null);
                                txtRemark.Text = "";// c.RemarkHD;
                                fRow = false;

                                if (c.Count > 0)
                                {
                                    //detail
                                    var rowe = dgvData.Rows.AddNew();
                                    var t = db.mh_Items.Where(x => x.InternalNo == dbClss.TSt(c.FirstOrDefault().ItemNo)).ToList();
                                    var cstm = db.mh_Customers.Where(x => x.No == txtCSTMNo.Text).First();
                                    decimal Qty = 0;
                                    Qty = Math.Round(dbClss.TDe(c.FirstOrDefault().OutSO) / dbClss.TDe(c.FirstOrDefault().PCSUnit), 2);

                                    addRow(rowe.Index, Convert.ToDateTime(c.FirstOrDefault().ReqDate)
                                        , dbClss.TSt(c.FirstOrDefault().ItemNo)
                                        , dbClss.TSt(c.FirstOrDefault().ItemName)
                                        , dbClss.TSt(t.FirstOrDefault().InternalDescription)
                                        , dbClss.TSt(t.FirstOrDefault().Location)
                                        , Qty //Math.Round(c.OutSO / c.PCSUnit, 2)
                                        , dbClss.TSt(c.FirstOrDefault().UOM)
                                        , dbClss.TDe(c.FirstOrDefault().PCSUnit)
                                        , dbClss.TDe(c.FirstOrDefault().UnitPrice)
                                        , dbClss.TDe(c.FirstOrDefault().Amount)
                                        , false,
                                        dbClss.TDe(c.FirstOrDefault().OutSO)
                                        , dbClss.TDe(c.FirstOrDefault().OutPlan)
                                        , 0
                                        , "Waiting", "Waiting", cstm.VatGroup
                                        , dbClss.TSt(t.FirstOrDefault().VatType)
                                        , dbClss.TSt(dd.FirstOrDefault().CustomerPONo)
                                        , dbClss.TInt(c.FirstOrDefault().id)
                                        , dbClss.TSt(t.FirstOrDefault().ReplenishmentType)
                                        , "T");

                                    cbbCSTM.Enabled = false;
                                }
                            }
                        }
                        //potoso.Add(new po_to_so
                        //{
                        //    idPO = c.id,
                        //    poQty = c.Quantity * c.PCSUnit,
                        //    poAmnt = c.Amount
                        //});
                    }
                    SetRowNo1(dgvData);
                    CallTotal();
                }
            }
            catch (Exception ex)
            {
                baseClass.Error(ex.Message);
            }
        }
        //
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
        private void Enable_Status(bool ss, string Condition)
        {
            if (Condition.Equals("-") || Condition.Equals("New"))
            {
                cbbCSTM.Enabled = ss;
                txtAddress.Enabled = ss;
                dtSODate.Enabled = ss;
                dgvData.ReadOnly = false;
                txtRemark.Enabled = ss;
                cbVat.Enabled = ss;
                txtVatA.Enabled = ss;
            }
            else if (Condition.Equals("View"))
            {
                cbbCSTM.Enabled = ss;
                txtAddress.Enabled = ss;
                dtSODate.Enabled = ss;
                dgvData.ReadOnly = true;
                txtRemark.Enabled = ss;
                cbVat.Enabled = ss;
                txtVatA.Enabled = ss;
            }
            else if (Condition.Equals("Edit"))
            {
                cbbCSTM.Enabled = ss;
                txtAddress.Enabled = ss;
                dtSODate.Enabled = ss;
                dgvData.ReadOnly = false;
                txtRemark.Enabled = ss;
                cbVat.Enabled = ss;
                txtVatA.Enabled = ss;
            }
        }

        private void ClearData()
        {
            cbbCSTM.SelectedIndex = -1;
            txtCSTMNo.Text = "";
            txtSONo.Text = "";
            dtSODate.Value = Convert.ToDateTime(DateTime.Today,new CultureInfo("en-US"));
            dgvData.Rows.Clear();
            dgvData.DataSource = null;
            txtRemark.Text = "";
            txtAddress.Text = "";
            txtTotal.Text = (0).ToMoney();
            txtCreateBy.Text = ClassLib.Classlib.User;
            txtCreateDate.Text = Convert.ToDateTime(DateTime.Today, new CultureInfo("en-US")).ToString("dd/MMM/YYYY");
            txtTotal.Text = "0.00";
            txtGrandTotal.Text = "0.00";
            txtVatAmnt.Text = "0.00";
            txtVatA.Text = "7.00";
            cbVat.Checked = true;
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
            dtSODate.ReadOnly = false;
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
            //txtTempNo.Text = StockControl.dbClss.GetNo(28, 0);

            idList.Clear();
            //potoso.Clear();

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
            dtSODate.ReadOnly = true;
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
            dtSODate.ReadOnly = false;
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
                string poNo = txtSONo.Text.Trim();
                string cstmNo = txtCSTMNo.Text.Trim();
                if (poNo != "" && cstmNo != "")
                {
                   
                    if (baseClass.IsDel($"Do you want to Delete Sale Order: {poNo} ?"))
                    {
                        //Status
                        //Waiting
                        //Process
                        //Partial-- > Shipment Partial
                        //Complete --> Shipment Full

                        using (var db = new DataClasses1DataContext())
                        {
                            var p = db.mh_SaleOrders.Where(x => x.SONo == poNo && x.Active).ToList();
                            if (p.Where(x => x.Active ==true && (x.Status.ToSt()=="Waiting" || x.Status.ToSt() == "Process")).Count() >0)
                            {
                                foreach (var pp in p)
                                {
                                    pp.Active = false;
                                    pp.Status = "Cancel";
                                    pp.UpdateBy = Classlib.User;
                                    pp.UpdateDate = Convert.ToDateTime( DateTime.Now,new CultureInfo("en-US"));
                                }
                                
                                var d = db.mh_SaleOrderDTs.Where(x => x.SONo == poNo && x.Active).ToList();
                                if (d.Where(x => x.Active == true && x.OutShip==x.Qty ).Count() > 0)
                                {
                                    foreach (var pp in d)
                                        pp.Active = false;
                                }

                                db.SubmitChanges();
                                
                                updateOutSO();
                              
                                baseClass.Info("Delete Sale Order complete.");
                                ClearData();
                                btnNew_Click(null, null);
                            }
                            else
                                baseClass.Warning("Sale Order Status cannot Delete.");
                        }
                    }
                }

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
                if (dgvData.Rows.Where(x => x.IsVisible).Count() < 1)
                    err += " “Items:” is empty \n";
              

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
                dgvData.EndInit();
                if (Ac.Equals("New"))// || Ac.Equals("Edit"))
                {
                    if (Check_Save())
                        return;
                    else if (baseClass.IsSave())
                        SaveE();
                }
                else
                    MessageBox.Show("สถานะต้องเป็น New เท่านั่น");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        void SaveE()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                string sono = txtSONo.Text;
                string cstmNo = txtCSTMNo.Text;
                CallTotal();
                using (var db = new DataClasses1DataContext())
                {

                    if (Ac.Equals("New"))
                    {
                        //ถ้ามีการใส่เลขที่ PR เช็คดูว่ามีการใส่เลขนี้แล้วหรือไม่ ถ้ามีให้ใส่เลขอื่น
                        if (!txtSONo.Text.Equals(""))
                        {

                            var p = (from ix in db.mh_SaleOrders
                                     where ix.SONo.ToUpper().Trim() == txtSONo.Text.Trim()
                                     && ix.Active != false
                                     //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                                     select ix).ToList();
                            if (p.Count > 0)  //มีรายการในระบบ
                            {
                                MessageBox.Show("เลขที่เอกสารถูกใช้ไปแล้ว กรุณาใส่เลขใหม่");
                                return;
                            }
                        }
                        else

                            txtSONo.Text = StockControl.dbClss.GetNo(28, 2);
                        sono = txtSONo.Text;

                    }


                    if (txtSONo.Text != "")
                    {
                        SaveHerder(sono);
                        SaveDetail();
                        

                        t_SONo = sono;
                        t_CustomerNo = cstmNo;
                       

                        updateOutSO();
                    }
                }


                baseClass.Info("Save complete(s).");
                ClearData();
                txtCSTMNo.Text = t_CustomerNo;
                txtSONo.Text = t_SONo;
                DataLoad();
            }
            catch (Exception ex)
            {
                baseClass.Error(ex.Message);
            }
            finally { this.Cursor = Cursors.Default; }
        }
        private void SaveHerder(string SONo)
        {
            using (var db = new DataClasses1DataContext())
            {
                mh_SaleOrder gg = new mh_SaleOrder();
                //gg.LocationRunning = ddlFactory.Text;
                gg.UpdateBy = ClassLib.Classlib.User;
                gg.UpdateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                gg.CreateBy = ClassLib.Classlib.User;
                gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                gg.SONo = SONo;
                gg.CustomerNo = txtCSTMNo.Text;
                gg.CustomerName = cbbCSTM.Text;
                gg.CustomerAddress = txtAddress.Text;
                gg.SODate = Convert.ToDateTime(dtSODate.Value, new CultureInfo("en-US"));
                gg.Remark = txtRemark.Text;
                //gg.VatGroup
                gg.TotalPrice = dbClss.TDe(txtTotal.Text);
                gg.Vat = cbVat.Checked;
                gg.VatA = dbClss.TDe(txtVatA);
                gg.VatAmnt = dbClss.TDe(txtVatAmnt);
                gg.TotalPriceIncVat = dbClss.TDe(txtGrandTotal.Text);
                gg.Active = true;
                gg.Status = "Waiting";
                gg.SendApproveBy = "";
                gg.ApproveBy = "";


                db.mh_SaleOrders.InsertOnSubmit(gg);
                db.SubmitChanges();

                dbClss.AddHistory(this.Name, "เพิ่ม Sale order", "สร้าง Sale order [" + SONo  + "]", txtSONo.Text);
            }
        }
        private void SaveDetail()
        {
            dgvData.EndEdit();
            using (var db = new DataClasses1DataContext())
            {
                string ItemNo = "";
                foreach (var ix in dgvData.Rows)
                {
                    ItemNo = dbClss.TSt(ix.Cells["ItemNo"].Value);


                    mh_SaleOrderDT gg = new mh_SaleOrderDT();
                    gg.ItemNo = ItemNo;
                    gg.ItemName = dbClss.TSt(ix.Cells["ItemName"].Value);
                    gg.LocationItem = dbClss.TSt(ix.Cells["LocationItem"].Value);
                    gg.OutPlan = dbClss.TDe(ix.Cells["OutPlan"].Value);
                    gg.OutShip = dbClss.TDe(ix.Cells["OutShip"].Value);
                    gg.PCSUnit = dbClss.TDe(ix.Cells["PCSUnit"].Value);
                    gg.PriceIncVat = cbVat.Checked;
                    gg.Qty = dbClss.TDe(ix.Cells["Qty"].Value);
                    gg.RefDocNo = dbClss.TSt(ix.Cells["RefDocNo"].Value);
                    gg.RefId = dbClss.TInt(ix.Cells["RefId"].Value);
                    gg.ReplenishmentType = dbClss.TSt(ix.Cells["ReplenishmentType"].Value);
                    gg.RNo = dbClss.TInt(ix.Cells["RNo"].Value);
                    gg.SONo = txtSONo.Text;
                    gg.UnitPrice = dbClss.TDe(ix.Cells["UnitPrice"].Value);
                    gg.UOM = dbClss.TSt(ix.Cells["Unit"].Value);
                    gg.VatType = dbClss.TSt(ix.Cells["VatType"].Value);
                    gg.Amount = dbClss.TDe(ix.Cells["Amount"].Value);
                    gg.Description = dbClss.TSt(ix.Cells["Description"].Value);
                    gg.Active = true;


                    db.mh_SaleOrderDTs.InsertOnSubmit(gg);
                    db.SubmitChanges();

                    dbClss.AddHistory(this.Name, "เพิ่ม Sale order", "สร้าง Sale order [" + ItemNo + "]", txtSONo.Text);
                }
            }
        }
        private void updateOutSO()
        {

            using (var db = new DataClasses1DataContext())
            {
                //Update Customer P/O (Out Sale Order Q'ty)
                foreach (var idPO in dgvData.Rows.Select(x => x.Cells["RefId"].Value.ToInt()))
                {
                    if (idPO == 0) continue;
                    var c = db.mh_CustomerPODTs.Where(x => x.id == idPO).First();
                    
                    var m = db.mh_SaleOrderDTs.Where(x => x.Active && x.RefId == idPO).ToList();
                    decimal qq = 0.00m;
                    if (m.Count > 0)
                        qq = m.Sum(x => x.Qty * x.PCSUnit);
                    c.OutSO = (c.Qty * c.PCSUnit) - qq;
                    c.Status = baseClass.setCustomerPOStatus(c);

                    //if (c.OutSO == c.Qty)
                    //    c.Status = "Waiting";
                    //else if (c.OutSO <= 0)
                    //    c.Status = "Completed";
                    //else
                    //    c.Status = "Proeces";
                    db.SubmitChanges();

                    
                }
            }
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
                            dgvData.Rows[e.RowIndex].Cells["Amount"].Value = m;
                        }
                        else
                            dgvData.Rows[e.RowIndex].Cells["Amount"].Value = 0;

                        e.Row.Cells["OutShip"].Value = e.Row.Cells["Qty"].Value.ToDecimal() * e.Row.Cells["PCSUnit"].Value.ToDecimal();
                        e.Row.Cells["OutPlan"].Value = e.Row.Cells["Qty"].Value.ToDecimal() * e.Row.Cells["PCSUnit"].Value.ToDecimal();
                        CallTotal();
                    }
                    else if (e.Column.Name.Equals("ItemNo"))
                    {
                        if (txtCSTMNo.Text == "")
                        {
                            baseClass.Warning("Please select Customer first.\n");
                            return;
                        }
                        using (var db = new DataClasses1DataContext())
                        {
                            var t = db.mh_Items.Where(x => x.InternalNo.Equals(itemNo)).FirstOrDefault();
                            if (t == null)
                            {
                                baseClass.Warning($"Item no. ({itemNo}) not found.!!");
                                e.Row.Cells["ItemNo"].Value = beginItem;
                                return;
                            }
                            var tU = db.mh_ItemUOMs.Where(x => x.ItemNo == t.InternalNo && x.UOMCode == t.BaseUOM).FirstOrDefault();
                            var pcsunit = (tU != null) ? tU.QuantityPer : 1;

                            //set Tool
                            if (beginItem == "")
                            {
                                var cc = db.mh_Customers.Where(x => x.No == txtCSTMNo.Text).First();
                                addRow(e.RowIndex, DateTime.Now, t.InternalNo, t.InternalName, "", t.Location
                                    , 1, t.BaseUOM, pcsunit, 0, 0, false, 1 * pcsunit, 1 * pcsunit, 0
                                    , "Waiting", "Waiting", cc.VatGroup, t.VatType, "", 0, t.ReplenishmentType, "T");
                            }
                            else
                            {
                                e.Row.Cells["ItemName"].Value = t.InternalName;
                                e.Row.Cells["UOM"].Value = t.BaseUOM;
                                e.Row.Cells["PCSUnit"].Value = pcsunit;
                                e.Row.Cells["OutShip"].Value = e.Row.Cells["Qty"].Value.ToDecimal() * e.Row.Cells["PCSUnit"].Value.ToDecimal();
                                e.Row.Cells["OutPlan"].Value = e.Row.Cells["Qty"].Value.ToDecimal() * e.Row.Cells["PCSUnit"].Value.ToDecimal();
                            }

                            //
                            SetRowNo1(dgvData);
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
                            e.Row.Cells["OutShip"].Value = e.Row.Cells["Qty"].Value.ToDecimal() * e.Row.Cells["PCSUnit"].Value.ToDecimal();
                            e.Row.Cells["OutPlan"].Value = e.Row.Cells["Qty"].Value.ToDecimal() * e.Row.Cells["PCSUnit"].Value.ToDecimal();
                        }
                    }

                    e.Row.Cells["dgvC"].Value = "T";
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void MasterTemplate_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            if (e.RowIndex >= -1)
            {
                if (e.Row.Cells["Status"].Value.ToSt() != "Waiting")
                {
                    e.Cancel = true;
                    return;
                }

                string itemNo = e.Row.Cells["ItemNo"].Value.ToSt();
                if (e.Column.Name.Equals("UOM"))
                {
                    using (var db = new DataClasses1DataContext())
                    {
                        var unit = db.mh_ItemUOMs.Where(x => x.ItemNo == itemNo).ToList();
                        unit = unit.Where(x => x.Active.ToBool()).ToList();
                        var c1 = dgvData.Columns["UOM"] as GridViewComboBoxColumn;
                        c1.ValueMember = "UOMCode";
                        c1.DisplayMember = "UOMCode";
                        c1.DataSource = unit;
                    }
                }
                else if (e.Column.Name.Equals("ItemNo"))
                {
                    beginItem = itemNo;
                }
            }
        }
        void addRow(int rowIndex, DateTime ReqDate, string ItemNo, string ItemName, string Desc
            , string Location, decimal Qty, string UOM, decimal PCSUnit, decimal UnitPrice, decimal Amount
            , bool PriceIncVat, decimal OutShip, decimal OutPlan, int id
            , string Status, string PlanStatus, int VatGroup, string VatType
            , string RefDocNo, int RefId, string RepType, string dgvC)
        {
            var rowE = dgvData.Rows[rowIndex];
            try
            {
                rowE.Cells["id"].Value = id;
                rowE.Cells["ReqDate"].Value = ReqDate;
                rowE.Cells["ItemNo"].Value = ItemNo;
                rowE.Cells["ItemName"].Value = ItemName;
                rowE.Cells["Description"].Value = Desc;
                rowE.Cells["LocationItem"].Value = Location;
                rowE.Cells["Qty"].Value = Qty;
                rowE.Cells["Unit"].Value = UOM;
                rowE.Cells["PCSUnit"].Value = PCSUnit;
                rowE.Cells["UnitPrice"].Value = UnitPrice;
                rowE.Cells["Amount"].Value = Amount;
                rowE.Cells["PriceIncVat"].Value = PriceIncVat;
                rowE.Cells["VatType"].Value = VatType;
                rowE.Cells["OutShip"].Value = OutShip;
                rowE.Cells["Status"].Value = Status;
                rowE.Cells["RefDocNo"].Value = RefDocNo;
                rowE.Cells["RefId"].Value = RefId;
                rowE.Cells["ReplenishmentType"].Value = RepType;
                rowE.Cells["dgvC"].Value = dgvC; //if Edit row -> value = T
                rowE.Cells["OutPlan"].Value = OutPlan;

                SetRowNo1(dgvData);


                rowE.Cells["ReqDate"].ReadOnly = true;
                rowE.Cells["ItemNo"].ReadOnly = true;
                rowE.Cells["ItemName"].ReadOnly = true;
                rowE.Cells["Description"].ReadOnly = true;
                rowE.Cells["LocationItem"].ReadOnly = true;
                rowE.Cells["Qty"].ReadOnly = true;
                rowE.Cells["Unit"].ReadOnly = true;
                rowE.Cells["PCSUnit"].ReadOnly = true;
                rowE.Cells["UnitPrice"].ReadOnly = true;
                rowE.Cells["Amount"].ReadOnly = true;
                rowE.Cells["PriceIncVat"].ReadOnly = true;
                rowE.Cells["VatType"].ReadOnly = true;
                rowE.Cells["OutShip"].ReadOnly = true;
                rowE.Cells["Status"].ReadOnly = true;
                rowE.Cells["RefDocNo"].ReadOnly = true;
                rowE.Cells["RefId"].ReadOnly = true;
                rowE.Cells["ReplenishmentType"].ReadOnly = true;
                rowE.Cells["dgvC"].ReadOnly = true;
                rowE.Cells["OutPlan"].ReadOnly = true;

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


        private void ลบพารทToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvData.Rows.Count < 0)
                    return;

                if (Ac.Equals("New") || Ac.Equals("Edit"))
                {
                    this.Cursor = Cursors.WaitCursor;

                    //if (dgvData.CurrentCell.RowInfo.Cells["PlanStatus"].Value.ToSt() != "Waiting")
                    //{
                    //    baseClass.Warning("Cannot Delete because Already Planned.\n");
                    //    return;
                    //}

                    if (dgvData.CurrentCell.RowInfo.Cells["Status"].Value.ToSt() == "Waiting")
                    {

                        int id = 0;
                        int.TryParse(StockControl.dbClss.TSt(dgvData.CurrentCell.RowInfo.Cells["id"].Value), out id);
                        if (id <= 0)
                            dgvData.Rows.Remove(dgvData.CurrentCell.RowInfo);

                        //else
                        //{
                        //    row = dgvData.CurrentCell.RowInfo.Index;
                        //    //btnDelete_Click(null, null);
                        //    using (var db = new DataClasses1DataContext())
                        //    {
                        //        var m = db.mh_SaleOrders.Where(x => x.id == id && x.Status =="Waiting" && x.Status =="Waiting Approve" ).FirstOrDefault();
                        //        if (m != null)
                        //        {
                        //            m.Active = false;
                        //            m.Status = "Cancel";
                        //            m.UpdateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                        //            m.UpdateBy = ClassLib.Classlib.User;
                        //            db.SubmitChanges();

                        //            updateOutSO();
                        //            dgvData.Rows.Remove(dgvData.CurrentCell.RowInfo);
                        //        }
                        //    }
                        //}
                        //CallTotal();
                        ////getTotal();
                        //SetRowNo1(dgvData);

                    }
                    else
                        MessageBox.Show("Cannot Delete please check Status");
                }
                else
                {
                    MessageBox.Show("Cannot Delete");
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
                List<GridViewRowInfo> dgvRow_List = new List<GridViewRowInfo>();
                var selP = new SaleOrder_List2(dgvRow_List);
                selP.ShowDialog();
                if (dgvRow_List.Count > 0)
                {
                    string SONo = "";
                    this.Cursor = Cursors.WaitCursor;
                    
                    foreach (GridViewRowInfo ee in dgvRow_List)
                    {
                        SONo = dbClss.TSt(ee.Cells["SONo"].Value);
                        
                    }

                    txtSONo.Text = SONo;
                    t_SONo = SONo;
                }

                     
                ////var pol = new SaleOrder_List2(txtSONo);
                //this.Cursor = Cursors.Default;
                //pol.ShowDialog();
                //if (pol.PONo != "" && pol.CstmNo != "")
                //{
                //    t_SONo = pol.PONo;
                //    t_CustomerNo = pol.CstmNo;
                //    //LoadData
                    
                //}
                if(t_SONo!="")
                    DataLoad();


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

        private void CallTotal()
        {
            try
            {
                decimal amnt = 0.00m;
                foreach (var item in dgvData.Rows.Where(x => x.IsVisible))
                {
                    amnt += item.Cells["Amount"].Value.ToDecimal();
                }
                txtTotal.Value = amnt;
                var vat = 0.00m;
                var vatA = txtVatA.Value.ToDecimal();
                if (cbVat.Checked)
                    vat = amnt * Math.Round(vatA / 100, 2);
                txtVatAmnt.Value = vat;
                txtGrandTotal.Value = amnt + vatA;
            }
            catch (Exception ex) { MessageBox.Show("err2: " + ex.Message); }
        }

        private void cbbCSTM_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtCSTMNo.Text = cbbCSTM.SelectedValue.ToSt();
                using (var db = new DataClasses1DataContext())
                {
                    var c = db.mh_Customers.Where(x => x.No == txtCSTMNo.Text).First();
                    txtAddress.Text = c.Address;
                }
            }
            catch
            {
                txtCSTMNo.Text = "";
                txtAddress.Text = "";
            }
        }

        private void btnAddPart_Click(object sender, EventArgs e)
        {
            AddPartE();
            CallTotal();
        }
        void AddPartE()
        {
            try
            {
                if (txtCSTMNo.Text == "")
                {
                    baseClass.Warning("Please select Customers first.\n");
                    return;
                }

                //if(dgvData.Rows.Count>0)
                //{
                //    return;
                //}

                List<GridViewRowInfo> dgvRow_List = new List<GridViewRowInfo>();
                var selP = new SaleOrder_ADD(dgvRow_List,txtCSTMNo.Text,cbbCSTM.Text);
                selP.ShowDialog();
                if (dgvRow_List.Count > 0)
                {
                    //string CodeNo = "";
                    this.Cursor = Cursors.WaitCursor;
                    int id = 0;
                    foreach (GridViewRowInfo ee in dgvRow_List)
                    {
                        id = dbClss.TInt(ee.Cells["id"].Value);

                        using (var db = new DataClasses1DataContext())
                        {
                            var c = db.mh_CustomerPODTs.Where(x => x.id == id).ToList();
                            if (c.Count > 0)
                            {
                                var dd = db.mh_CustomerPOs.Where(x => x.id == dbClss.TInt(c.FirstOrDefault().idCustomerPO)).ToList();
                                if (dd.Count > 0)
                                {
                                    txtCSTMNo.Text = dbClss.TSt(dd.FirstOrDefault().CustomerNo);
                                    cbbCSTM.SelectedValue = dbClss.TSt(dd.FirstOrDefault().CustomerNo);
                                    dtSODate.Value = DateTime.Now;
                                    cbbCSTM_SelectedIndexChanged(null, null);
                                    txtRemark.Text = "";// c.RemarkHD;
                                    
                                    if (c.Count > 0)
                                    {
                                        //detail
                                        var rowe = dgvData.Rows.AddNew();
                                        var t = db.mh_Items.Where(x => x.InternalNo == dbClss.TSt(c.FirstOrDefault().ItemNo)).ToList();
                                        var cstm = db.mh_Customers.Where(x => x.No == txtCSTMNo.Text).First();
                                        decimal Qty = 0;
                                        Qty = Math.Round(dbClss.TDe(c.FirstOrDefault().OutSO) / dbClss.TDe(c.FirstOrDefault().PCSUnit), 2);

                                        addRow(rowe.Index, Convert.ToDateTime(c.FirstOrDefault().ReqDate)
                                            , dbClss.TSt(c.FirstOrDefault().ItemNo)
                                            , dbClss.TSt(c.FirstOrDefault().ItemName)
                                            , dbClss.TSt(t.FirstOrDefault().InternalDescription)
                                            , dbClss.TSt(t.FirstOrDefault().Location)
                                            , Qty //Math.Round(c.OutSO / c.PCSUnit, 2)
                                            , dbClss.TSt(c.FirstOrDefault().UOM)
                                            , dbClss.TDe(c.FirstOrDefault().PCSUnit)
                                            , dbClss.TDe(c.FirstOrDefault().UnitPrice)
                                            , dbClss.TDe(c.FirstOrDefault().Amount)
                                            , false
                                            , Qty//dbClss.TDe(c.FirstOrDefault().OutSO)
                                            , dbClss.TDe(c.FirstOrDefault().OutPlan)
                                            , 0
                                            , "Waiting", "Waiting", cstm.VatGroup
                                            , dbClss.TSt(t.FirstOrDefault().VatType)
                                            , dbClss.TSt(dd.FirstOrDefault().CustomerPONo)
                                            , dbClss.TInt(c.FirstOrDefault().id)
                                            , dbClss.TSt(t.FirstOrDefault().ReplenishmentType)
                                            , "T");

                                    }
                                }
                            }
                        }
                    }

                }

            }catch(Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
            //if (m.Count() > 0)
            //{
            //    using (var db = new DataClasses1DataContext())
            //    {
            //        foreach (var item in m)
            //        {
            //            var itemNo = item.Cells["CodeNo"].Value.ToSt();
            //            var t = db.mh_Items.Where(x => x.InternalNo == itemNo).FirstOrDefault();
            //            if (t == null)
            //            {
            //                baseClass.Warning($"Item ({itemNo} not found.!!!");
            //                return;
            //            }

            //            var tU = db.mh_ItemUOMs.Where(x => x.ItemNo == itemNo && x.UOMCode == t.BaseUOM).FirstOrDefault();
            //            decimal u = (tU != null) ? tU.QuantityPer : 1;

            //            var rowE = dgvData.Rows.AddNew();
            //            var cc = db.mh_Customers.Where(x => x.No == txtCSTMNo.Text).First();
            //            addRow(rowE.Index, DateTime.Now, itemNo, t.InternalName, "", t.Location
            //                , 1, t.BaseUOM, u, 0, 0, false, 1 * u, 1 * u, 0, "Waiting", "Waiting"
            //                , cc.VatGroup, t.VatType, "", 0, t.ReplenishmentType, "T");
            //        }
            //        SetRowNo1(dgvData);

            //    }
            //}
        }

        private void txtPONo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtSONo.Text.Trim() == "")
                {
                    baseClass.Warning("Please enter Sale Order no.");
                    return;
                }
                if (txtCSTMNo.Text.Trim() == "")
                {
                    baseClass.Warning("Please enter Customer no.");
                    return;
                }

                t_SONo = txtSONo.Text;
                t_CustomerNo = txtCSTMNo.Text;
                ClearData();
                DataLoad();
            }
        }

        private void cbVat_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            CallTotal();
        }

        private void txtVatA_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                CallTotal();
        }

        private void btnSendApprove_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    if (lblStatus.Text == "Waiting")
                    {
                        if (baseClass.IsApprove())
                        {
                            var p = db.mh_SaleOrders.Where(x => x.SONo == txtSONo.Text && x.Active).ToList();
                            if (p.Where(x => x.Active == true && (x.Status.ToSt() == "Waiting")).Count() > 0)
                            {
                                foreach (var pp in p)
                                {
                                    pp.Status = "Process";
                                    pp.UpdateBy = Classlib.User;
                                    pp.UpdateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                }

                                db.SubmitChanges();

                                baseClass.Info("Approve complete.");

                            }
                        }
                    }
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void radButtonElement3_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    if (lblStatus.Text == "Waiting")
                    {
                        if (baseClass.IsSendApprove())
                        {
                            db.sp_062_mh_ApproveList_Add(txtSONo.Text.Trim(), "Sale Order", Classlib.User);
                            MessageBox.Show("Send complete.");
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }


    public class po_to_so
    {
        public int idPO { get; set; }
        public decimal poQty { get; set; }
        public decimal poAmnt { get; set; }
        public decimal pricePer
        {
            get
            {
                return Math.Round(poAmnt / poQty, 2);
            }
        }
    }
}
