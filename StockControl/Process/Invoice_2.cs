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
using Telerik.WinControls;
using Telerik.WinControls.Data;

namespace StockControl
{
    public partial class Invoice_2 : Telerik.WinControls.UI.RadRibbonForm
    {
        string t_SONo = "";
        string t_CustomerNo = "";
        //Customer P/O to SaleORder
        List<int> idList = new List<int>();
        //List<po_to_so> potoso = new List<po_to_so>();

        public Invoice_2()
        {
            InitializeComponent();
        }
        public Invoice_2(string SHNo)
        {
            InitializeComponent();
            txtIVNo.Text = SHNo;
            t_SONo = SHNo;
        }
        public Invoice_2(string PONo, string CustomerNo)
        {
            InitializeComponent();
            this.t_SONo = PONo;
            this.t_CustomerNo = CustomerNo;
        }
        public Invoice_2(List<int> idList)
        {
            InitializeComponent();
            this.idList = idList;
        }

        bool demo = false;
        public Invoice_2(bool demo)
        {
            InitializeComponent();
            this.demo = demo;
        }

        private void Unit_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                //dgvData.ReadOnly = true;
                dgvData.AutoGenerateColumns = false;
                //GETDTRow();
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
                txtIVNo.Text = t_SONo;
                
                if (t_SONo != "" && t_CustomerNo != "")
                    DataLoad();
                else if (t_SONo != "")
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

                var uom = db.mh_Units.Where(x => x.UnitActive.Value).ToList();
                //UnitCode UnitDetail
                var com4 = dgvData.Columns["Unit"] as GridViewComboBoxColumn;
                com4.DisplayMember = "UnitCode";
                com4.ValueMember = "UnitCode";
                com4.DataSource = uom;

                try
                {
                    cboCustomer.Items.Clear();
                    cboCustomer.Items.Add("");
                    var mh = db.mh_Customers.Where(c => c.Active == true).ToList();
                    if(mh.Count>0)
                    {
                       foreach(var rd in mh)
                        {
                            cboCustomer.Items.Add(rd.Name);
                        }
                    }
                }
                catch { }
                try
                {

                    GridViewMultiComboBoxColumn col = (GridViewMultiComboBoxColumn)dgvData.Columns["LocationItem"];
                    col.DataSource = (from ix in db.mh_Locations.Where(s => Convert.ToBoolean(s.Active.Equals(true)) && s.Active == true)
                                      select new { ix.Code, ix.Name }).ToList();

                    col.DisplayMember = "Code";
                    col.ValueMember = "Code";
                    col.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDown;
                    col.FilteringMode = GridViewFilteringMode.DisplayMember;

                    col.AutoSizeMode = BestFitColumnMode.DisplayedDataCells;
                    col.TextAlignment = ContentAlignment.MiddleCenter;
                    col.DropDownStyle = RadDropDownStyle.DropDownList;

                }
                catch { }
            }
        }

        void DemoLoad()
        {
            //demoAdd(1, new DateTime(2018, 09, 19), "I0001", "Item A", 100, "PCS", 100, "SO1809-001");
            //demoAdd(2, new DateTime(2018, 09, 20), "I0001", "Item B", 100, "PCS", 100, "SO1809-002");
            //demoAdd(2, new DateTime(2018, 09, 22), "I0001", "Item C", 50, "PCS", 100, "SO1809-003");
            //txtCSTMNo.Text = "C0001";
            //txtCustomerNo.Text = "TT FUJI TOOL SUPPORT CO.,LTD";
            //txtSONo.Text = "SP1809-001";
            //CallTotal();
        }
        void demoAdd(int rno, DateTime reqDate, string ItemNO, string ItemName, decimal Qty, string Unit, decimal UnitPrice, string RefDocNo)
        {
            var rowe = dgvData.Rows.AddNew();
            rowe.Cells["RNo"].Value = rno;
            rowe.Cells["ReqDate"].Value = reqDate;
            rowe.Cells["ItemNo"].Value = ItemNO;
            rowe.Cells["ItemName"].Value = ItemName;
            rowe.Cells["Qty"].Value = Qty;
            rowe.Cells["Unit"].Value = Unit;
            rowe.Cells["UnitPrice"].Value = UnitPrice;
            rowe.Cells["Amount"].Value = Qty * UnitPrice;
            rowe.Cells["RefDocNo"].Value = RefDocNo;
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
                    mh_InvoiceHD mh = db.mh_InvoiceHDs.Where(s => s.IVNo == txtIVNo.Text).FirstOrDefault();
                    if(mh!=null)
                    {
                        txtTel.Text = mh.Tel;
                        txtTotal.Text = mh.TotalPrice.ToString();
                        txtGrandTotal.Text = mh.TotalPriceIncVat.ToString();
                        txtFax.Text = mh.Fax;
                        txtEmail.Text = mh.Email;
                        txtContactName.Text = mh.ContactName;
                        txtAddress.Text = mh.CustomerAddress;
                        cboCustomer.Text = mh.CustomerName;
                        txtRemark.Text = mh.Remark;
                        txtCSTMNo.Text = mh.CustomerNo;
                        dtSODate.Value = mh.SSDate;
                        txtVatA.Text = mh.VatA.ToSt();
                        cbVat.Checked = mh.Vat;
                        if (mh.Type.ToSt() == "1")
                            Type_Button = 1;
                        else if (mh.Type.ToSt() == "2")
                            Type_Button = 2;
                        else if (mh.Type.ToSt() == "3")
                            Type_Button = 3;
                        
                        var list1 = db.mh_InvoiceDTs.Where(w => w.IVNo == txtIVNo.Text && !w.Status.Equals("Cancel")).ToList();
                        if (list1.Count > 0)
                        {
                            dgvData.DataSource = list1;
                        }
                        btnSave.Enabled = false;
                        SetRowNo1(dgvData);
                    }
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
                        var c = db.mh_ShipmentDTs.Where(x => x.id == id && x.OutInv > 0 && x.OutShip==0).ToList();
                        if (c.Count > 0)
                        {
                            
                            if (c.Count > 0)
                            {
                                var dd = db.mh_Shipments.Where(x => x.SSNo == c.FirstOrDefault().SSNo.ToSt()).ToList();
                                if (dd.Count > 0)
                                {
                                    txtCSTMNo.Text = dbClss.TSt(dd.FirstOrDefault().CustomerNo);
                                    cboCustomer.SelectedValue = dbClss.TSt(dd.FirstOrDefault().CustomerNo);
                                    //dtSODate.Value = DateTime.Now;
                                    cbbCSTM_SelectedIndexChanged(null, null);
                                    //txtRemark.Text = "";// c.RemarkHD;


                                    var rowE = dgvData.Rows.AddNew();
                                    addRow(rowE.Index, DateTime.Now,
                                        c.FirstOrDefault().ItemNo
                                        , c.FirstOrDefault().ItemName, c.FirstOrDefault().Description, c.FirstOrDefault().LocationItem
                                        , 0, c.FirstOrDefault().UOM, dbClss.TDe(c.FirstOrDefault().PCSUnit)
                                        , dbClss.TDe(c.FirstOrDefault().UnitPrice)
                                        , dbClss.TDe(c.FirstOrDefault().UnitPrice) * 0
                                        , false, 0, 0, 0, "Waiting", "Waiting"
                                        , c.FirstOrDefault().SSNo, dbClss.TInt(c.FirstOrDefault().id)
                                        , dbClss.TDe(c.FirstOrDefault().OutInv), "T", "SH");
                                    
                                }
                            }
                        }
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
                cboCustomer.Enabled = ss;
                txtAddress.Enabled = ss;
                dtSODate.Enabled = ss;
                dgvData.ReadOnly = false;
                txtRemark.Enabled = ss;
                cbVat.Enabled = ss;
                txtVatA.Enabled = ss;
                txtContactName.Enabled = ss;
                txtEmail.Enabled = ss;
                txtFax.Enabled = ss;
                txtTel.Enabled = ss;
            }
            else if (Condition.Equals("View"))
            {
                cboCustomer.Enabled = ss;
                txtAddress.Enabled = ss;
                dtSODate.Enabled = ss;
                dgvData.ReadOnly = true;
                txtRemark.Enabled = ss;
                cbVat.Enabled = ss;
                txtVatA.Enabled = ss;
                txtEmail.Enabled = ss;
                txtFax.Enabled = ss;
                txtTel.Enabled = ss;
            }
            else if (Condition.Equals("Edit"))
            {
                cboCustomer.Enabled = ss;
                txtAddress.Enabled = ss;
                dtSODate.Enabled = ss;
                dgvData.ReadOnly = false;
                txtRemark.Enabled = ss;
                cbVat.Enabled = ss;
                txtVatA.Enabled = ss;
                txtEmail.Enabled = ss;
                txtFax.Enabled = ss;
                txtTel.Enabled = ss;

            }
        }

        private void ClearData()
        {
            // txtCSTMNo.Text = "";
            dtSODate.Value = DateTime.Now;
            txtIVNo.Text = "";
            txtCSTMNo.Text = "";
            dtSODate.Value = DateTime.Today;
            dgvData.Rows.Clear();
            dgvData.DataSource = null;
            txtRemark.Text = "";
            txtTotal.Text = (0).ToMoney();
            txtIVNo.Text = dbClss.GetNo(31, 0);

            Type_Button = 0;
            radButton1.Enabled = true;
            radButton2.Enabled = true;
            radButton3.Enabled = true;
            dgvData.Columns["LocationItem"].IsVisible = true;

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
            //txtTempNo.Text = StockControl.dbClss.GetNo(10, 0);
            txtIVNo.Text = dbClss.GetNo(31, 0);

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
                string poNo = txtIVNo.Text.Trim();
                string cstmNo = txtCSTMNo.Text.Trim();
                if (poNo != "" && cstmNo != "")
                {
                    if (baseClass.IsDel($"Do you want to Delete invoice: {poNo} ?"))
                    {
                        using (var db = new DataClasses1DataContext())
                        {
                            var g = (from ix in db.mh_InvoiceHDs
                                     where ix.IVNo == txtIVNo.Text && ix.Active==true
                                     select ix).ToList();
                            if (g.Count > 0)  //มีรายการในระบบ
                            {
                                var gg = (from ix in db.mh_InvoiceHDs
                                          where ix.IVNo == txtIVNo.Text && ix.Active == true
                                          select ix).First();

                                try
                                {
                                    var s = (from ix in db.mh_InvoiceDTs
                                             where ix.IVNo.Trim() == txtIVNo.Text && ix.Active == true
                                             select ix).ToList();
                                    if (s.Count > 0)
                                    {
                                        foreach (var ss in s)
                                        {
                                            ss.Status = "Cancel";
                                            ss.Active = false;
                                            db.SubmitChanges();
                                        }

                                    }
                                }
                                catch (Exception ex) { MessageBox.Show(ex.Message); }
                                //----------------------//

                                gg.StatusHD = "Cancel";
                                gg.Active = false;
                                gg.UpdateBy = Classlib.User;
                                gg.UpdateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));

                                dbClss.AddHistory(this.Name, "ลบ Invoice", "Delete Invoice [" + txtIVNo.Text.Trim() + "]", txtIVNo.Text);
                                updateOutSO();

                                baseClass.Info("Delete invoice complete.");
                           
                                btnNew_Click(null, null);
                            }
                            else
                                baseClass.Warning("Invoice Status cannot Delete.");
                            
                        }
                    }
                }
                else
                    btnNew_Click(null, null);


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
                if (cboCustomer.Text=="")
                    err += " “Customer Name:” is empty \n";
                if (dtSODate.Text == "")
                    err += " “Delivery Date:” is empty \n";

                if (dgvData.Rows.Where(x => x.IsVisible).Count() < 1)
                    err += " “Items:” is empty \n";
                if (err == "")
                {
                    foreach (var item in dgvData.Rows.Where(x => x.IsVisible))
                    {
                        if (dbClss.TSt(item.Cells["ItemNo"].Value) == "")
                            err += " “Item No:” is null \n";
                        //if (item.Cells["ReqDate"].Value == null)
                        //    err += " “Request Date.:” is empty \n";
                        if (item.Cells["Qty"].Value.ToDecimal() <= 0)
                            err += " “Qty:” is less than 0 \n";
                        if (item.Cells["Qty"].Value.ToDecimal() > item.Cells["OutInv"].Value.ToDecimal())
                            err += " “Qty:” is more than Remain Invoice \n";
                        if (dbClss.TSt(item.Cells["Unit"].Value) =="" )
                            err += " “Unit:” is null \n";
                        if (item.Cells["PCSUnit"].Value.ToDecimal() <= 0)
                            err += " “PCSUnit:” is less than 0 \n";
                        if(Type_Button==3)
                        {
                            if (dbClss.TSt(item.Cells["LocationItem"].Value) == "")
                                err += " “Location:” is null \n";
                        }
                       

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
                dbClss.AddError(this.Name, ex.Message, this.Name);
            }

            return re;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            ///
            /////
            ////////////////ADDDDD Item//////////////
            try
            {
                dgvData.EndInit();
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
                txtIVNo.Text = dbClss.GetNo(31, 2);
                string sono = txtIVNo.Text;
                string cstmNo = txtContactName.Text;
                CallTotal();

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    mh_InvoiceHD sh = db.mh_InvoiceHDs.Where(s => s.IVNo == sono).FirstOrDefault();
                    if (sh != null)
                    {

                    }
                    else
                    {
                        decimal vatA = 0;
                        decimal totalPrice = 0;
                        decimal vatAmount = 0;
                        decimal grantotal = 0;
                        decimal.TryParse(txtGrandTotal.Text, out grantotal);
                        decimal.TryParse(txtVatAmnt.Text, out vatAmount);
                        decimal.TryParse(txtTotal.Text, out totalPrice);
                        decimal.TryParse(txtVatA.Text, out vatA);

                        mh_InvoiceHD sh1 = new mh_InvoiceHD();
                        sh1.IVNo = txtIVNo.Text;
                        sh1.Remark = txtRemark.Text;
                        sh1.SSDate = dtSODate.Value;
                        sh1.Tel = txtTel.Text;
                        sh1.Email = txtEmail.Text;
                        sh1.Fax = txtFax.Text;
                        sh1.UpdateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                        sh1.UpdateBy = ConnectDB.user;
                        sh1.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                        sh1.CreateBy = ConnectDB.user;// ClassLib.Classlib.User;
                        
                        sh1.CustomerNo = txtCSTMNo.Text;
                        sh1.CustomerName = cboCustomer.Text;
                        sh1.CustomerAddress = txtAddress.Text;
                        sh1.Vat = cbVat.Checked;
                        sh1.VatA = vatA;
                        sh1.VatAmnt = vatAmount;
                        sh1.TotalPrice = totalPrice;
                        sh1.TotalPriceIncVat = grantotal;
                        sh1.StatusHD = "Process";
                        sh1.ContactName = txtContactName.Text;
                        sh1.Active = true;
                        sh1.Type = Type_Button.ToSt();
                        db.mh_InvoiceHDs.InsertOnSubmit(sh1);
                        db.SubmitChanges();

                        ////Addd DT///
                        dgvData.EndEdit();
                        int rno = 0;
                        foreach (GridViewRowInfo rd in dgvData.Rows)
                        {
                            rno += 1;

                           int ab = Convert.ToInt16(rd.Cells["RefId"].Value);

                            mh_InvoiceDT nd = new mh_InvoiceDT();

                            nd.ItemNo = rd.Cells["ItemNo"].Value.ToSt();
                            nd.RefDocNo = Convert.ToString(rd.Cells["RefDocNo"].Value);
                            nd.Qty = Convert.ToDecimal(rd.Cells["Qty"].Value.ToSt());
                            nd.Amount = Convert.ToDecimal(rd.Cells["Amount"].Value.ToSt());
                            nd.ItemName = Convert.ToString(rd.Cells["ItemName"].Value);
                            nd.LocationItem = Convert.ToString(rd.Cells["LocationItem"].Value);
                            nd.UOM = Convert.ToString(rd.Cells["Unit"].Value);
                            nd.Description = Convert.ToString(rd.Cells["Description"].Value);
                            nd.UnitPrice = Convert.ToDecimal(rd.Cells["UnitPrice"].Value.ToSt());
                            nd.PCSUnit = Convert.ToDecimal(rd.Cells["PCSUnit"].Value.ToSt());
                            nd.IVNo = txtIVNo.Text;
                            nd.Status = "Process";
                            nd.RefId = Convert.ToInt16(rd.Cells["RefId"].Value);
                            nd.Active = Convert.ToBoolean(true);
                          
                            db.mh_InvoiceDTs.InsertOnSubmit(nd);
                            db.SubmitChanges();


                            ////Cut Stock
                            //if (Type_Button == 1)
                            //{
                                var v = (from ix in db.mh_ShipmentDTs
                                         where
                                                   ix.id == Convert.ToInt16(rd.Cells["RefId"].Value.ToSt())
                                         select ix).ToList();
                            if (v.Count > 0)
                            {
                                var p = (from ix in db.mh_ShipmentDTs
                                         where
                                            ix.id == Convert.ToInt16(rd.Cells["RefId"].Value.ToSt())
                                         select ix).First();

                                p.OutInv = p.OutInv - Convert.ToDecimal(rd.Cells["Qty"].Value.ToSt());
                                if (p.OutInv <= 0)
                                    p.OutInv = 0;

                                if (dbClss.TDe(p.OutShip) == dbClss.TDe(p.Qty))
                                    p.Status = "Waiting";
                                else if (dbClss.TDe(p.OutInv) == 0)
                                    p.Status = "Completed";
                                else
                                    p.Status = "Process";

                                dbClss.AddHistory(this.Name, "ปรับสถานะ mh_ShipmentDT ", "ลบ OutInv : " + (rd.Cells["Qty"].Value.ToSt())
                                    + " ShipmentNo :" + Convert.ToString(rd.Cells["RefDocNo"].Value)
                                    + " ปรับโดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", Convert.ToString(rd.Cells["RefDocNo"].Value));

                                db.SubmitChanges();

                                db.sp_058_Cal_ShipmentHD_Status(p.SSNo);
                            }
                            //}
                            //else if (Type_Button == 2)
                            //{
                            //    int id = Convert.ToInt16(rd.Cells["RefId"].Value.ToSt());
                            //    if (id != 0)
                            //    {
                            //        mh_SaleOrderDT sd = db.mh_SaleOrderDTs.Where(ss => ss.id == id).FirstOrDefault();
                            //        if (sd != null)
                            //        {
                            //            sd.OutShip = sd.OutShip - Math.Round(Convert.ToDecimal(rd.Cells["Qty"].Value.ToSt()) * Convert.ToDecimal(rd.Cells["PCSUnit"].Value), 2);
                            //            if (sd.OutShip <= 0)
                            //                sd.OutShip = 0;

                            //            dbClss.AddHistory(this.Name, "ปรับสถานะ mh_SaleOrderDTs ", "ปรับ OutShip : " + (rd.Cells["Qty"].Value.ToSt())
                            //        + " SaleOrder :" + Convert.ToString(rd.Cells["RefDocNo"].Value)
                            //        + " ปรับโดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", Convert.ToString(rd.Cells["RefDocNo"].Value));

                            //            // db.SubmitChanges();
                            //        }
                            //    }
                            //    db.SubmitChanges();
                            //}
                            //else if (Type_Button == 3)
                            //{
                            //    decimal Qty = Convert.ToDecimal(rd.Cells["Qty"].Value.ToSt());
                            //    decimal PCSUnit = dbClss.TDe(rd.Cells["PCSUnit"].Value);
                            //    string BaseUOM = "";//dbClss.TSt(e.Row.Cells["BaseUOM"].Value);
                            //    decimal BasePCSUOM = 0;


                            //    var g = (from ix in db.mh_Items select ix).Where(a => a.InternalNo == rd.Cells["ItemNo"].Value.ToSt()).ToList();
                            //    if (g.Count() > 0)
                            //    {
                            //        BaseUOM = dbClss.TSt(g.FirstOrDefault().BaseUOM);
                            //        BasePCSUOM = dbClss.Con_UOM(rd.Cells["ItemNo"].Value.ToSt(), BaseUOM);
                            //    }


                            //    decimal RemainQty = (Convert.ToDecimal(db.Cal_QTY_Remain_Location(Convert.ToString(rd.Cells["ItemNo"].Value), "Invoice", 0, Convert.ToString(rd.Cells["LocationItem"].Value))));//decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["RemainQty"].Value), out RemainQty);
                            //    if (BasePCSUOM <= 0)
                            //        BasePCSUOM = 1;

                            //    decimal Temp = 0;
                            //    Temp = Math.Round((BasePCSUOM * PCSUnit * Qty), 2);


                            //    db.sp_057_Cut_Stock(txtSONo.Text, rd.Cells["ItemNo"].Value.ToSt()
                            //    , Temp, ClassLib.Classlib.User
                            //    , Convert.ToString(rd.Cells["RefDocNo"].Value), "", 0
                            //    , Convert.ToString(rd.Cells["LocationItem"].Value), "Shipping", "Shipping", 3);

                            //    dbClss.AddHistory(this.Name, "ปรับ Stock Item ", "Shipping Item : " + (rd.Cells["Qty"].Value.ToSt())
                            //        + " Item :" + Convert.ToString(rd.Cells["ItemNo"].Value)
                            //        + " ปรับโดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", Convert.ToString(rd.Cells["ItemNo"].Value));

                            //}
                        }
                    }
                }


                    baseClass.Info("Save complete(s).");
                ClearData();
                //txtContactName.Text = t_CustomerNo;
                txtIVNo.Text = sono;
                DataLoad();
            }
            catch (Exception ex)
            {
                baseClass.Error(ex.Message);
            }
            finally { this.Cursor = Cursors.Default; }
        }

        private void updateOutSO()
        {
            using (var db = new DataClasses1DataContext())
            {//Update Customer P/O (Out Sale Order Q'ty)

                //if (Type_Button == 1)
                //{
                foreach (GridViewRowInfo rd in dgvData.Rows)
                {
                    var v = (from ix in db.mh_ShipmentDTs
                             where
                                       ix.id == Convert.ToInt16(rd.Cells["RefId"].Value.ToSt())
                             select ix).ToList();
                    if (v.Count > 0)
                    {
                        var p = (from ix in db.mh_ShipmentDTs
                                 where
                                    ix.id == Convert.ToInt16(rd.Cells["RefId"].Value.ToSt())
                                 select ix).First();

                        p.OutInv = p.OutInv + Convert.ToDecimal(rd.Cells["Qty"].Value.ToSt());
                        if (p.Qty < p.OutInv)
                            p.OutInv = p.Qty;

                        if (dbClss.TDe(p.OutShip) == dbClss.TDe(p.Qty))
                            p.Status = "Waiting";
                        else if (dbClss.TDe(p.OutInv) == 0)
                            p.Status = "Completed";
                        else
                            p.Status = "Process";
                        
                        dbClss.AddHistory(this.Name, "ปรับสถานะ mh_ShipmentDT ", "บวก OutShip เพราะลบ Invoice : " + (rd.Cells["Qty"].Value.ToSt())
                        + " ShipmentNo :" + Convert.ToString(rd.Cells["RefDocNo"].Value)
                        + " ปรับโดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", Convert.ToString(rd.Cells["RefDocNo"].Value));

                        db.SubmitChanges();
                        db.sp_058_Cal_ShipmentHD_Status(p.SSNo);
                    }
                }
                //}
                //else if (Type_Button == 2)
                //{
                //    foreach (GridViewRowInfo rd in dgvData.Rows)
                //    {

                //        int id = Convert.ToInt16(rd.Cells["RefId"].Value.ToSt());
                //        if (id != 0)
                //        {
                //            mh_SaleOrderDT sd = db.mh_SaleOrderDTs.Where(ss => ss.id == id).FirstOrDefault();
                //            if (sd != null)
                //            {
                //                sd.OutShip = sd.OutShip + Math.Round(Convert.ToDecimal(rd.Cells["Qty"].Value.ToSt()) * Convert.ToDecimal(rd.Cells["PCSUnit"].Value), 2);
                //                if (sd.Qty < sd.OutShip)
                //                    sd.OutShip = sd.Qty;

                //                dbClss.AddHistory(this.Name, "ปรับสถานะ mh_SaleOrderDTs ", "ปรับ OutShip เพราะลบ Invoice : " + (rd.Cells["Qty"].Value.ToSt())
                //            + " SaleOrder :" + Convert.ToString(rd.Cells["RefDocNo"].Value)
                //            + " ปรับโดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", Convert.ToString(rd.Cells["RefDocNo"].Value));

                //            }
                //        }
                //        db.SubmitChanges();
                //    }
                //}
                //else if (Type_Button == 3)
                //{
                //    DateTime? CalDate = null;
                //    DateTime? AppDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                //    int Seq = 0;
                //    string Type = "รับด้วยใบ Invoice";
                //    string Category = "Invoice";
                //    int Flag_ClearTemp = 0;
                //    string Type_in_out = "In";
                //    decimal RemainQty = 0;
                //    decimal Amount = 0;
                //    decimal RemainAmount = 0;
                //    decimal RemainUnitCost = 0;
                //    //decimal Avg = 0;
                //    decimal UnitCost = 0;
                //    decimal sum_Remain = 0;
                //    decimal sum_Qty = 0;
                //    decimal BasePCSUnit = 0;
                   

                //    foreach (GridViewRowInfo rd in dgvData.Rows)
                //    {
                //        //db.sp_057_Cut_Stock(txtSONo.Text, rd.Cells["ItemNo"].Value.ToSt()
                //        //, Convert.ToDecimal(rd.Cells["Qty"].Value.ToSt()), ClassLib.Classlib.User
                //        //, Convert.ToString(rd.Cells["RefDocNo"].Value), "", 0
                //        //, Convert.ToString(rd.Cells["LocationItem"].Value), "Shipping", "Shipping", 3);

                //        string Unit = dbClss.TSt(rd.Cells["Unit"].Value);
                //        string CodeNo = dbClss.TSt(rd.Cells["ItemNo"].Value);


                //        decimal PCSUnit = dbClss.TDe(rd.Cells["PCSUnit"].Value);
                //        string BaseUOM = "";//dbClss.TSt(e.Row.Cells["BaseUOM"].Value);
                //        decimal CostPerUnit = dbClss.TDe(rd.Cells["UnitPrice"].Value);
                //        decimal QTY = dbClss.TDe(rd.Cells["Qty"].Value);
                //         Amount = dbClss.TDe(rd.Cells["Amount"].Value);
                //        string Location = dbClss.TSt(rd.Cells["LocationItem"].Value);
                //        string ShelfNo = "";
                //        var g = (from ix in db.mh_Items select ix).Where(a => a.InternalNo == CodeNo).ToList();
                //        if (g.Count() > 0)
                //        {
                //            BaseUOM = dbClss.TSt(g.FirstOrDefault().BaseUOM);
                //            BasePCSUnit = dbClss.Con_UOM(CodeNo, BaseUOM);
                //            ShelfNo = dbClss.TSt(g.FirstOrDefault().ShelfNo);
                //        }

                        
                //            //Amount = Math.Round((QTY * CostPerUnit), 2);
                //        UnitCost = CostPerUnit;//Math.Round((Amount / (QTY * PCSUnit * BasePCSUnit)), 2);
                //        //แบบที่ 1 จะไป sum ใหม่
                //        RemainQty = (Convert.ToDecimal(db.Cal_QTY_Remain_Location(CodeNo, "", 0, Location)));
                //        sum_Remain = Convert.ToDecimal(dbClss.Get_Stock(CodeNo, "", "", "RemainAmount", Location))
                //            + Amount;

                //        sum_Qty = RemainQty + Math.Round(((QTY * PCSUnit) * BasePCSUnit), 2);
                     
                //        RemainAmount = sum_Remain;
                //        if (sum_Qty <= 0)
                //            RemainUnitCost = 0;
                //        else
                //            RemainUnitCost = Math.Round((Math.Abs(RemainAmount) / Math.Abs(sum_Qty)), 2);

                //        tb_Stock gg = new tb_Stock();
                //        gg.AppDate = AppDate;
                //        gg.Seq = Seq;
                //        gg.App = "Receive";
                //        gg.Appid = Seq;
                //        gg.CreateBy = ClassLib.Classlib.User;
                //        gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                //        gg.DocNo =txtSONo.Text;
                //        gg.RefNo = txtSONo.Text;
                //        gg.CodeNo = CodeNo;
                //        gg.Type = Type;
                //        gg.QTY = Math.Round(((QTY * PCSUnit) * BasePCSUnit), 2);
                //        gg.Inbound = Math.Round(((QTY * PCSUnit) * BasePCSUnit), 2);
                //        gg.Outbound = 0;
                //        gg.Type_i = 1;  //Receive = 1,Cancel Receive 2,Shipping = 3,Cancel Shipping = 4,Adjust stock = 5,ClearTemp = 6
                //        gg.Category = Category;
                //        gg.Refid = dbClss.TInt(rd.Cells["RefId"].Value);
                //        gg.Type_in_out = Type_in_out;
                //        gg.AmountCost = Amount;
                //        gg.UnitCost = UnitCost;
                //        gg.RemainQty = sum_Qty;
                //        gg.RemainUnitCost = RemainUnitCost;
                //        gg.RemainAmount = RemainAmount;
                //        gg.Avg = 0;// Avg;
                //        gg.CalDate = CalDate;
                //        gg.Status = "Active";
                //        gg.Flag_ClearTemp = Flag_ClearTemp;   //0 คือ invoice,1 คือ Temp , 2 คือ clear temp แล้ว
                //        gg.TLCost = Amount;
                //        gg.TLQty = Math.Round(((QTY * PCSUnit) * BasePCSUnit), 2);
                //        gg.ShipQty = 0;
                //        gg.Location = Location;
                //        gg.ShelfNo = ShelfNo;
                //        gg.RefJobCode = dbClss.TSt(rd.Cells["RefDocNo"].Value);

                //        //ต้องไม่ใช่ Item ที่มีในระบบ
                //        var c = (from ix in db.mh_Items
                //                 where ix.InternalNo.Trim().ToUpper() == CodeNo.Trim().ToUpper() && ix.Active == true
                //                 select ix).ToList();
                //        if (c.Count <= 0)
                //        {
                //            gg.TLQty = 0;
                //            gg.ShipQty = Math.Round(((QTY * PCSUnit) * BasePCSUnit), 2);
                //        }

                //        db.tb_Stocks.InsertOnSubmit(gg);
                //        db.SubmitChanges();

                //        //update Stock เข้า item
                //        db.sp_010_Update_StockItem(CodeNo, "");

                //        dbClss.AddHistory(this.Name, "ปรับ Stock Item ", "Cancel invoice : " + (rd.Cells["Qty"].Value.ToSt())
                //            + " Item :" + Convert.ToString(rd.Cells["ItemNo"].Value)
                //            + " ปรับโดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", Convert.ToString(rd.Cells["ItemNo"].Value));
                //    }
                //}


               
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
                    if (e.Column.Name.Equals("Qty") || (e.Column.Name.Equals("PCSUnit")))
                    {
                        if (Type_Button == 1) //SH
                        {
                            using (DataClasses1DataContext db = new DataClasses1DataContext())
                            {
                                decimal QTY = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["Qty"].Value), out QTY);
                                decimal RemainQty = dbClss.TDe(e.Row.Cells["OutInv"].Value);

                                int id = dbClss.TInt(e.Row.Cells["Refid"].Value);
                                var v = (from ix in db.mh_ShipmentDTs
                                         where
                                                   ix.id == id
                                         select ix).ToList();
                                if (v.Count > 0)
                                {
                                    var p = (from ix in db.mh_ShipmentDTs
                                             where
                                                ix.id == id
                                             select ix).First();

                                    RemainQty = Convert.ToDecimal(p.OutInv);
                                    e.Row.Cells["OutInv"].Value = RemainQty;
                                }

                                if (QTY > RemainQty)
                                {
                                    MessageBox.Show("ไม่สามารถระบุจำนวนแจ้งหนี้ เกินจำนวนคงเหลือได้");
                                    e.Row.Cells["Qty"].Value = 0;
                                    dgvData.Rows[e.RowIndex].Cells["Amount"].Value = 0;
                                    QTY = 0;
                                }
                                else if (QTY <= 0)
                                {
                                    QTY = 0;
                                    dgvData.Rows[e.RowIndex].Cells["Amount"].Value = 0;
                                }
                                else
                                {
                                    var m = Math.Round(e.Row.Cells["UnitPrice"].Value.ToDecimal() * e.Row.Cells["Qty"].Value.ToDecimal(), 2);
                                    dgvData.Rows[e.RowIndex].Cells["Amount"].Value = m;
                                }
                            }
                        }
                        // else   if (Type_Button == 2) //SO
                        //{
                        //    decimal QTY = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["Qty"].Value), out QTY);
                        //    decimal RemainQty = 0;
                        //    //decimal PCSUnit = dbClss.TDe(e.Row.Cells["PCSUnit"].Value);
                        //    int id = dbClss.TInt(e.Row.Cells["Refid"].Value);
                        //    using (DataClasses1DataContext db = new DataClasses1DataContext())
                        //    {
                        //        var im = db.mh_SaleOrderDTs.Where(ix => ix.id == id && dbClss.TBo(ix.Active) == true && Convert.ToDecimal(ix.OutShip) > 0).FirstOrDefault();
                        //        if (im != null)
                        //        {
                        //            RemainQty = Convert.ToDecimal(im.OutShip);
                        //        }
                        //    }
                        //        if (QTY > RemainQty)
                        //    {
                        //        MessageBox.Show("ไม่สามารถเบิกเกินจำนวนคงเหลือได้");
                        //        e.Row.Cells["Qty"].Value = 0;
                        //        dgvData.Rows[e.RowIndex].Cells["Amount"].Value = 0;
                        //        QTY = 0;
                        //    }
                        //    else if (QTY <= 0)
                        //    {
                        //        QTY = 0;
                        //        dgvData.Rows[e.RowIndex].Cells["Amount"].Value = 0;
                        //    }
                        //    else
                        //    {
                        //        var m = Math.Round(e.Row.Cells["UnitPrice"].Value.ToDecimal() * e.Row.Cells["Qty"].Value.ToDecimal(), 2);
                        //        dgvData.Rows[e.RowIndex].Cells["Amount"].Value = m;
                        //    }

                        //}
                        //else if (Type_Button == 3) //Item
                        //{
                        //    string dgvUOM = dbClss.TSt(e.Row.Cells["Unit"].Value);
                        //    string CodeNo = dbClss.TSt(e.Row.Cells["ItemNo"].Value);


                        //    decimal PCSUnit = dbClss.TDe(e.Row.Cells["PCSUnit"].Value);
                        //    string BaseUOM = "";//dbClss.TSt(e.Row.Cells["BaseUOM"].Value);
                        //    decimal BasePCSUOM = 0;
                        //    using (DataClasses1DataContext db = new DataClasses1DataContext())
                        //    {

                        //        var g = (from ix in db.mh_Items select ix).Where(a => a.InternalNo == CodeNo).ToList();
                        //        if (g.Count() > 0)
                        //        {
                        //            BaseUOM = dbClss.TSt(g.FirstOrDefault().BaseUOM);
                        //            BasePCSUOM = dbClss.Con_UOM(CodeNo, BaseUOM);
                        //        }

                        //        decimal QTY = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["Qty"].Value), out QTY);
                        //        decimal RemainQty = (Convert.ToDecimal(db.Cal_QTY_Remain_Location(Convert.ToString(e.Row.Cells["ItemNo"].Value), "Invoice", 0, Convert.ToString(e.Row.Cells["LocationItem"].Value))));//decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["RemainQty"].Value), out RemainQty);
                        //        if (BasePCSUOM <= 0)
                        //            BasePCSUOM = 1;

                        //        decimal Temp = 0;
                        //        Temp = BasePCSUOM * PCSUnit * QTY;

                        //        if (Temp > RemainQty)
                        //        {
                        //            MessageBox.Show("ไม่สามารถเบิกเกินจำนวนคงเหลือได้");
                        //            e.Row.Cells["Qty"].Value = 0;
                        //            dgvData.Rows[e.RowIndex].Cells["Amount"].Value = 0;
                        //            QTY = 0;
                        //        }
                        //        else if (Temp <= 0)
                        //        {
                        //            QTY = 0;
                        //            dgvData.Rows[e.RowIndex].Cells["Amount"].Value = 0;
                        //        }
                        //        else
                        //        {
                        //            var m = Math.Round(e.Row.Cells["UnitPrice"].Value.ToDecimal() * e.Row.Cells["Qty"].Value.ToDecimal(), 2);
                        //            dgvData.Rows[e.RowIndex].Cells["Amount"].Value = m;
                        //        }
                        //    }
                        //}

                    }

                    if (e.Column.Name.Equals("UnitPrice"))
                    {
                        if (e.Row.Cells["Qty"].Value.ToDecimal() > 0)
                        {
                            var m = Math.Round(e.Row.Cells["UnitPrice"].Value.ToDecimal() * e.Row.Cells["Qty"].Value.ToDecimal(), 2);
                            dgvData.Rows[e.RowIndex].Cells["Amount"].Value = m;
                        }
                        else
                            dgvData.Rows[e.RowIndex].Cells["Amount"].Value = 0;

                        //e.Row.Cells["OutShip"].Value = e.Row.Cells["Qty"].Value.ToDecimal() * e.Row.Cells["PCSUnit"].Value.ToDecimal();
                        //e.Row.Cells["OutPlan"].Value = e.Row.Cells["Qty"].Value.ToDecimal() * e.Row.Cells["PCSUnit"].Value.ToDecimal();
                        //CallTotal();
                    }

                    else if (e.Column.Name.Equals("Unit"))
                    {
                        string dgvUOM = dbClss.TSt(e.Row.Cells["Unit"].Value);
                        string CodeNo = dbClss.TSt(e.Row.Cells["ItemNo"].Value);
                        e.Row.Cells["PCSUnit"].Value = dbClss.Con_UOM(CodeNo, dgvUOM);

                        //if (Type_Button == 3)
                        //{
                        //    decimal PCSUnit = dbClss.TDe(e.Row.Cells["PCSUnit"].Value);
                        //    string BaseUOM = "";//dbClss.TSt(e.Row.Cells["BaseUOM"].Value);
                        //    decimal BasePCSUOM = 0;
                        //    using (DataClasses1DataContext db = new DataClasses1DataContext())
                        //    {

                        //        var g = (from ix in db.mh_Items select ix).Where(a => a.InternalNo == CodeNo).ToList();
                        //        if (g.Count() > 0)
                        //        {
                        //            BaseUOM = dbClss.TSt(g.FirstOrDefault().BaseUOM);
                        //            BasePCSUOM = dbClss.Con_UOM(CodeNo, BaseUOM);
                        //        }

                        //        decimal QTY = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["Qty"].Value), out QTY);
                        //        decimal RemainQty = (Convert.ToDecimal(db.Cal_QTY_Remain_Location(Convert.ToString(e.Row.Cells["ItemNo"].Value), "Invoice", 0, Convert.ToString(e.Row.Cells["LocationItem"].Value))));//decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["RemainQty"].Value), out RemainQty);
                        //        if (BasePCSUOM <= 0)
                        //            BasePCSUOM = 1;

                        //        decimal Temp = 0;
                        //        Temp = BasePCSUOM * PCSUnit * QTY;

                        //        if (Temp > RemainQty)
                        //        {
                        //            MessageBox.Show("ไม่สามารถเบิกเกินจำนวนคงเหลือได้");
                        //            e.Row.Cells["Qty"].Value = 0;
                        //            dgvData.Rows[e.RowIndex].Cells["Amount"].Value = 0;
                        //            QTY = 0;
                        //        }
                        //        else if (Temp <= 0)
                        //        {
                        //            QTY = 0;
                        //            dgvData.Rows[e.RowIndex].Cells["Amount"].Value = 0;
                        //        }
                        //        else
                        //        {
                        //            var m = Math.Round(e.Row.Cells["UnitPrice"].Value.ToDecimal() * e.Row.Cells["Qty"].Value.ToDecimal(), 2);
                        //            dgvData.Rows[e.RowIndex].Cells["Amount"].Value = m;
                        //        }
                        //    }
                        //}
                    }
                    //else if (e.Column.Name.Equals("LocationItem"))
                    //{
                    //    if (Type_Button == 3)
                    //    {
                    //        string dgvUOM = dbClss.TSt(e.Row.Cells["Unit"].Value);
                    //        string CodeNo = dbClss.TSt(e.Row.Cells["ItemNo"].Value);

                    //        decimal PCSUnit = dbClss.TDe(e.Row.Cells["PCSUnit"].Value);
                    //        string BaseUOM = "";//dbClss.TSt(e.Row.Cells["BaseUOM"].Value);
                    //        decimal BasePCSUOM = 0;
                    //        using (DataClasses1DataContext db = new DataClasses1DataContext())
                    //        {

                    //            var g = (from ix in db.mh_Items select ix).Where(a => a.InternalNo == CodeNo).ToList();
                    //            if (g.Count() > 0)
                    //            {
                    //                BaseUOM = dbClss.TSt(g.FirstOrDefault().BaseUOM);
                    //                BasePCSUOM = dbClss.Con_UOM(CodeNo, BaseUOM);
                    //            }

                    //            decimal QTY = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["Qty"].Value), out QTY);
                    //            decimal RemainQty = (Convert.ToDecimal(db.Cal_QTY_Remain_Location(Convert.ToString(e.Row.Cells["ItemNo"].Value), "Invoice", 0, Convert.ToString(e.Row.Cells["LocationItem"].Value))));//decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["RemainQty"].Value), out RemainQty);
                    //            if (BasePCSUOM <= 0)
                    //                BasePCSUOM = 1;

                    //            decimal Temp = 0;
                    //            Temp = BasePCSUOM * PCSUnit * QTY;

                    //            if (Temp > RemainQty)
                    //            {
                    //                MessageBox.Show("ไม่สามารถเบิกเกินจำนวนคงเหลือได้");
                    //                e.Row.Cells["Qty"].Value = 0;
                    //                dgvData.Rows[e.RowIndex].Cells["Amount"].Value = 0;
                    //                QTY = 0;
                    //            }
                    //            else if (Temp <= 0)
                    //            {
                    //                QTY = 0;
                    //                dgvData.Rows[e.RowIndex].Cells["Amount"].Value = 0;
                    //            }
                    //            else
                    //            {
                    //                var m = Math.Round(e.Row.Cells["UnitPrice"].Value.ToDecimal() * e.Row.Cells["Qty"].Value.ToDecimal(), 2);
                    //                dgvData.Rows[e.RowIndex].Cells["Amount"].Value = m;
                    //            }
                    //        }
                    //    }
                    //}
                        e.Row.Cells["dgvC"].Value = "T";
                    CallTotal();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void MasterTemplate_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            if (e.RowIndex >= -1)
            {
                string itemNo = e.Row.Cells["ItemNo"].Value.ToSt();
                //if (e.Column.Name.Equals("Unit"))
                //{
                //    using (var db = new DataClasses1DataContext())
                //    {
                //        var unit = db.mh_ItemUOMs.Where(x => x.ItemNo == itemNo).ToList();
                //        unit = unit.Where(x => x.Active.ToBool()).ToList();
                //        var c1 = dgvData.Columns["Unit"] as GridViewComboBoxColumn;
                //        c1.ValueMember = "UOMCode";
                //        c1.DisplayMember = "UOMCode";
                //        c1.DataSource = unit;
                //    }
                //}
                 if (e.Column.Name.Equals("ItemNo"))
                {
                    beginItem = itemNo;
                }
                else if (e.Column.Name.Equals("LocationItem"))
                {
                    /////////////มีการ เคลียร์ การ Add ก่อน แล้วค่อย Add ใหม่////////////////
                    //Row = e.RowIndex;
                    RadMultiColumnComboBoxElement Comcol = (RadMultiColumnComboBoxElement)e.ActiveEditor;
                    Comcol.Columns.Clear();

                    //RadMultiColumnComboBoxElement Comcol = (RadMultiColumnComboBoxElement)e.ActiveEditor;
                    Comcol.DropDownSizingMode = SizingMode.UpDownAndRightBottom;
                    Comcol.DropDownWidth = 300;
                    Comcol.DropDownHeight = 150;
                    //Comcol.EditorControl.BestFitColumns(BestFitColumnMode.AllCells);
                    Comcol.EditorControl.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                    //ปรับอัตโนมัติ
                    //Comcol.EditorControl.AutoGenerateColumns = false;
                    //Comcol.BestFitColumns(true, true);
                    Comcol.AutoFilter = true;

                    //Comcol.EditorControl.AllowAddNewRow = true;
                    Comcol.EditorControl.EnableFiltering = true;
                    Comcol.EditorControl.ReadOnly = false;
                    Comcol.ClearFilter();


                    //Comcol.DisplayMember = "ItemNo";
                    //Comcol.ValueMember = "ItemNo";

                    // //----------------------------- ปรับโดยกำหนดเอง
                    Comcol.EditorControl.Columns.Add(new GridViewTextBoxColumn
                    {
                        HeaderText = "รหัส",
                        Name = "Code",
                        FieldName = "Code",
                        Width = 80,
                        AllowFiltering = true,
                        ReadOnly = false
                    }
                   );
                    Comcol.EditorControl.Columns.Add(new GridViewTextBoxColumn
                    {
                        HeaderText = "ชื่อ",
                        Name = "Name",
                        FieldName = "Name",
                        Width = 150,
                        AllowFiltering = true,
                        ReadOnly = false
                    }
                   );
                    // Comcol.EditorControl.Columns.Add(new GridViewTextBoxColumn
                    // {
                    //     HeaderText = "Description",
                    //     Name = "Description",
                    //     FieldName = "Description",
                    //     Width = 300,
                    //     AllowFiltering = true,
                    //     ReadOnly = false

                    // }
                    //);


                    //dgvDataDetail.CellEditorInitialized += MasterTemplate_CellEditorInitialized;

                }
                else if (e.Column.Name.Equals("Unit"))
                {
                    /////////////มีการ เคลียร์ การ Add ก่อน แล้วค่อย Add ใหม่////////////////
                    //Row = e.RowIndex;
                    RadMultiColumnComboBoxElement Comcol = (RadMultiColumnComboBoxElement)e.ActiveEditor;
                    Comcol.Columns.Clear();

                    //RadMultiColumnComboBoxElement Comcol = (RadMultiColumnComboBoxElement)e.ActiveEditor;
                    Comcol.DropDownSizingMode = SizingMode.UpDownAndRightBottom;
                    Comcol.DropDownWidth = 150;
                    Comcol.DropDownHeight = 150;
                    //Comcol.EditorControl.BestFitColumns(BestFitColumnMode.AllCells);
                    Comcol.EditorControl.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                    //ปรับอัตโนมัติ
                    //Comcol.EditorControl.AutoGenerateColumns = false;
                    //Comcol.BestFitColumns(true, true);
                    Comcol.AutoFilter = true;

                    //Comcol.EditorControl.AllowAddNewRow = true;
                    Comcol.EditorControl.EnableFiltering = true;
                    Comcol.EditorControl.ReadOnly = false;
                    Comcol.ClearFilter();


                    //Comcol.DisplayMember = "ItemNo";
                    //Comcol.ValueMember = "ItemNo";

                    // //----------------------------- ปรับโดยกำหนดเอง
                    Comcol.EditorControl.Columns.Add(new GridViewTextBoxColumn
                    {
                        HeaderText = "Unit",
                        Name = "UnitCode",
                        FieldName = "UnitCode",
                        Width = 100,
                        AllowFiltering = true,
                        ReadOnly = false
                    }
                   );
                }
            }
        }
        void addRow(int rowIndex, DateTime ReqDate, string ItemNo, string ItemName, string Desc
            , string Location, decimal Qty, string UOM, decimal PCSUnit, decimal UnitPrice, decimal Amount
            , bool PriceIncVat, decimal OutShip, decimal OutPlan, int id, string Status, string PlanStatus            
            , string RefDocNo, int RefId,decimal OutInv, string dgvC,string TypeADD)
        {
            var rowE = dgvData.Rows[rowIndex];
            try
            {
                rowE.Cells["id"].Value = id;
                rowE.Cells["ReqDate"].Value = ReqDate;
                rowE.Cells["ItemNo"].Value = ItemNo;
                rowE.Cells["ItemName"].Value = ItemName;
                rowE.Cells["Description"].Value = Desc;
                //rowE.Cells["Location"].Value = Location;
                rowE.Cells["Qty"].Value = Qty;
                rowE.Cells["Unit"].Value = UOM;
                rowE.Cells["PCSUnit"].Value = PCSUnit;
                rowE.Cells["UnitPrice"].Value = UnitPrice;
                rowE.Cells["Amount"].Value = Amount;
                rowE.Cells["LocationItem"].Value = Location;
                //rowE.Cells["PriceIncVat"].Value = PriceIncVat;
                //rowE.Cells["VatGroup"].Value = VatGroup;
                //rowE.Cells["VatType"].Value = VatType;
                //rowE.Cells["OutShip"].Value = OutShip;
                //rowE.Cells["Status"].Value = Status;
                rowE.Cells["RefDocNo"].Value = RefDocNo;
                rowE.Cells["OutInv"].Value = OutInv;
                rowE.Cells["RefId"].Value = RefId;
                //rowE.Cells["RepType"].Value = RepType;
                rowE.Cells["dgvC"].Value = dgvC; //if Edit row -> value = T
                                                 //rowE.Cells["PlanStatus"].Value = PlanStatus;
                                                 //rowE.Cells["OutPlan"].Value = OutPlan;

                rowE.Cells["ItemNo"].ReadOnly = true;
                rowE.Cells["ItemName"].ReadOnly = true;
                rowE.Cells["Description"].ReadOnly = true;
                ////rowE.Cells["Unit"].ReadOnly = true;
                ////rowE.Cells["PCSUnit"].ReadOnly = true;
                ////rowE.Cells["UnitPrice"].ReadOnly = true;
                //if (TypeADD == "SO")
                //{
                    rowE.Cells["LocationItem"].ReadOnly = true;
                    rowE.Cells["Unit"].ReadOnly = true;
                    rowE.Cells["PCSUnit"].ReadOnly = true;
                    //rowE.Cells["UnitPrice"].ReadOnly = true;
                //}
                //else
                    rowE.Cells["LocationItem"].ReadOnly = false;

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


        private void ลบพารทToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
               if(lblStatus.Text.Equals("New"))
                {
                    if (row >= 0)
                    {
                       
                        try
                        {

                            if (dgvData.Rows.Count < 0)
                                return;


                            if (Ac.Equals("New"))// || Ac.Equals("Edit"))
                            {
                                this.Cursor = Cursors.WaitCursor;

                                int id = 0;
                                int.TryParse(StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["id"].Value), out id);
                                if (id <= 0)
                                    dgvData.Rows.Remove(dgvData.CurrentRow);

                                //else
                                //{
                                //    string CodeNo = "";
                                //    CodeNo = StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["ItemNo"].Value);
                                //    if (MessageBox.Show("ต้องการลบรายการ ( " + CodeNo + " ) ออกจากรายการ หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                //    {
                                //        dgvData.CurrentRow.IsVisible = false;
                                //    }
                                //}
                                SetRowNo1(dgvData);
                            }
                            else
                            {
                                MessageBox.Show("ไม่สามารถทำการลบรายการได้");
                            }
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }
                        finally { this.Cursor = Cursors.Default; }

                        //using (DataClasses1DataContext db = new DataClasses1DataContext())
                        //{
                        //    if(RNo>0)
                        //    {
                        //        mh_InvoiceDTTemp md = db.mh_InvoiceDTTemps.Where(s => s.id == RNo).FirstOrDefault();
                        //        if(md!=null)
                        //        {
                        //            db.mh_InvoiceDTTemps.DeleteOnSubmit(md);
                        //            db.SubmitChanges();
                        //        }
                        //        LoadShipment();
                        //    }
                        //}

                    }

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

                List<GridViewRowInfo> dgvRow_List = new List<GridViewRowInfo>();
                this.Cursor = Cursors.WaitCursor;
                var sm = new Invoice_List2(dgvRow_List);
                this.Cursor = Cursors.Default;
                sm.ShowDialog();
                if (dgvRow_List.Count > 0)
                {
                    string SONo = "";
                    this.Cursor = Cursors.WaitCursor;

                    foreach (GridViewRowInfo ee in dgvRow_List)
                    {
                        SONo = dbClss.TSt(ee.Cells["IVNo"].Value);
                        break;
                    }

                    txtIVNo.Text = SONo;
                    DataLoad();
                }


                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : btnListItem_Click", this.Name); }
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

            radButton1.Enabled = false;
            radButton2.Enabled = false;
            radButton3.Enabled = false;
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

                txtGrandTotal.Value = amnt + dbClss.TDe(txtVatAmnt.Value);
            }
            catch (Exception ex) { MessageBox.Show("err2: " + ex.Message); }
        }

        private void cbbCSTM_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnAddPart_Click(object sender, EventArgs e)
        {
            radButton1_Click(null, null);
            //AddPartE();
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
                        
                        decimal PCSUnit = dbClss.Con_UOM(itemNo, StockControl.dbClss.TSt(t.PurchaseUOM));

                        //var tU = db.mh_ItemUOMs.Where(x => x.ItemNo == itemNo && x.UOMCode == t.BaseUOM).ToList();
                        //if(tU.Count>0)
                        //{

                        //}
                        decimal RemainQty = 0;
                        RemainQty = (Convert.ToDecimal(db.Cal_QTY_Remain_Location(itemNo, "Invoice", 0, t.Location,0)));
                        if (RemainQty > 0)
                        {
                            var rowE = dgvData.Rows.AddNew();
                            //var cc = db.mh_Customers.Where(x => x.No == txtContactName.Text).ToList();

                            addRow(rowE.Index, DateTime.Now, itemNo, t.InternalName, t.InternalDescription, t.Location
                                , 1, t.BaseUOM, PCSUnit, dbClss.TDe(t.StandardPrice), dbClss.TDe(t.StandardPrice) * 1
                                , false, 1 * PCSUnit, 1 * PCSUnit, 0, "Waiting", "Waiting",
                                 "", 0,0, "T","Item");
                        }
                    }
                    SetRowNo1(dgvData);

                }
            }
        }

        private void txtPONo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtIVNo.Text.Trim() == "")
                {
                    baseClass.Warning("Please enter Sale Order no.");
                    return;
                }
                if (txtContactName.Text.Trim() == "")
                {
                    baseClass.Warning("Please enter Customer no.");
                    return;
                }

                t_SONo = txtIVNo.Text;
                t_CustomerNo = txtContactName.Text;
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
        int Type_Button = 0;
        private void radButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCSTMNo.Text == "")
                {
                    baseClass.Warning("Please select Customers first.\n");
                    return;
                }
                Type_Button = 1;
                //if(dgvData.Rows.Count>0)
                //{
                //    return;
                //}

                List<GridViewRowInfo> dgvRow_List = new List<GridViewRowInfo>();
                var selP = new Invoice_Shipment_List(dgvRow_List, txtCSTMNo.Text, cboCustomer.Text);
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
                            var c = db.mh_ShipmentDTs.Where(x => x.id == id && x.OutInv>0 && x.OutShip==0 ).ToList();
                            if (c.Count > 0)
                            {
                                //mh_ShipmentDT im = db.mh_ShipmentDTs
                                //    .Where(m => m.SSNo == rd.Cells["ShipmentNo"].Value.ToSt()).FirstOrDefault();
                                //if (im != null)
                                //{
                                //    rows1 += 1;
                                //    mh_InvoiceDTTemp st = new mh_InvoiceDTTemp();
                                //    st.RNo = rows1;
                                //    st.IVNo = CodeNo;
                                //    //st.UserID = ClassLib.Classlib.User;
                                //    st.ItemNo = rd.Cells["ShipmentNo"].Value.ToSt();
                                //    st.ItemName = im.ItemName;
                                //    st.Description = im.Description;
                                //    st.Qty = 1;
                                //    st.PCSUnit = 1;
                                //    st.LocationItem = im.LocationItem;
                                //    st.UnitPrice = im.UnitPrice;
                                //    st.Amount = im.UnitPrice * 1;
                                //    st.Active = true;
                                //    st.UOM = im.UOM;
                                //    st.RefId = im.id;
                                //    st.RefDocNo = im.SSNo;
                                //    db.mh_InvoiceDTTemps.InsertOnSubmit(st);
                                //    db.SubmitChanges();
                                //}
                                if (c.Count > 0)
                                {
                                    var dd = db.mh_Shipments.Where(x => x.SSNo == c.FirstOrDefault().SSNo.ToSt()).ToList();
                                if (dd.Count > 0)
                                {
                                    txtCSTMNo.Text = dbClss.TSt(dd.FirstOrDefault().CustomerNo);
                                    cboCustomer.SelectedValue = dbClss.TSt(dd.FirstOrDefault().CustomerNo);
                                    //dtSODate.Value = DateTime.Now;
                                    cbbCSTM_SelectedIndexChanged(null, null);
                                    //txtRemark.Text = "";// c.RemarkHD;

                                    
                                            var rowE = dgvData.Rows.AddNew();
                                        addRow(rowE.Index, DateTime.Now,
                                            c.FirstOrDefault().ItemNo
                                            , c.FirstOrDefault().ItemName, c.FirstOrDefault().Description
                                            , c.FirstOrDefault().LocationItem
                                            , 0, c.FirstOrDefault().UOM, dbClss.TDe(c.FirstOrDefault().PCSUnit)
                                            , dbClss.TDe(c.FirstOrDefault().UnitPrice)
                                            , dbClss.TDe(c.FirstOrDefault().UnitPrice) * 0
                                            , false, 0, 0, 0, "Waiting", "Waiting"
                                            , c.FirstOrDefault().SSNo
                                            , dbClss.TInt(c.FirstOrDefault().id)
                                            ,dbClss.TDe(c.FirstOrDefault().OutInv), "T", "SH");
                                                
                                        
                                    }
                                }
                            }
                        }
                    }

                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }

         
        }
        private void LoadShipment()
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var ShipList = db.mh_InvoiceDTTemps.Where(s => s.IVNo == txtIVNo.Text).ToList();
                    if (ShipList.Count > 0)
                    {
                        dgvData.DataSource = ShipList;

                        int No1 = 0;
                        dgvData.Columns["LocationItem"].IsVisible = false;
                        foreach (GridViewRowInfo rd in dgvData.Rows)
                        {
                            No1 += 1;
                            rd.Cells["RNo"].Value = No1;

                            rd.Cells["ItemNo"].ReadOnly = true;
                            rd.Cells["ItemName"].ReadOnly = true;
                            rd.Cells["Description"].ReadOnly = true;
                            //rd.Cells["Unit"].ReadOnly = true;
                            //rd.Cells["PCSUnit"].ReadOnly = true;
                            //rd.Cells["UnitPrice"].ReadOnly = true;
                            rd.Cells["LocationItem"].ReadOnly = true;
                        }
                    }

                    var inv = db.mh_InvoiceDTTemps.Where(s => s.IVNo == txtIVNo.Text).ToList();
                    if (inv.Count>0)
                    {
                        foreach(var gg in inv)
                            db.mh_InvoiceDTTemps.DeleteOnSubmit(gg);

                        db.SubmitChanges();
                    }
                }
            }
            catch { }
        }

        private void cboCustomer_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            try
            {

                
                if (!cboCustomer.Text.Equals(""))
                {
                    txtAddress.Text = "";
                    txtEmail.Text = "";
                    txtFax.Text = "";
                    txtTel.Text = "";
                    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    {
                        var cm = db.mh_CustomerContacts.ToList();
                        if (cm.Count > 0)
                        {
                            foreach (var rd in cm)
                            {
                                // txtAddress.Text =
                                txtContactName.Text = rd.ContactName;
                                txtEmail.Text = rd.Email;
                                txtFax.Text = rd.Fax;
                                txtTel.Text = rd.Tel;
                               
                            }
                        }

                        mh_Customer mc = db.mh_Customers.Where(c => c.Name == cboCustomer.Text).FirstOrDefault();
                        if(mc!=null)
                        {
                            txtAddress.Text = mc.ShippingAddress;
                            txtCSTMNo.Text = mc.No;
                        }

                    }
                }
            }
            catch { }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            if (txtContactName.Text == "")
            {
                baseClass.Warning("Please select Customers first.\n");
                return;
            }
            AddPartE();
            radButton3.Enabled = false;
            radButton1.Enabled = false;
            Type_Button = 3;
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            if (txtContactName.Text == "")
            {
                baseClass.Warning("Please select Customers first.\n");
                return;
            }
            Add_SO();
            radButton1.Enabled = false;
            radButton2.Enabled = false;
            Type_Button = 2;
        }
        private void Add_SO()
        {
            var m = new List<GridViewRowInfo>();
            var selP = new SaleOrder_List2(m, txtCSTMNo.Text);
            selP.ShowDialog();
            if (m.Count() > 0)
            {
                using (var db = new DataClasses1DataContext())
                {
                    foreach (var item in m)
                    {
                        int id = dbClss.TInt(item.Cells["id"].Value);
                        if (id > 0)
                        {
                            var gg = (from ix in db.mh_SaleOrderDTs
                                     where ix.id == id && ix.Active == true && Convert.ToDecimal(ix.OutShip) > 0
                                     select ix).ToList();
                            if (gg.Count > 0)
                            {
                                    var im = db.mh_SaleOrderDTs.Where(ix => ix.id == id && ix.Active == true && Convert.ToDecimal(ix.OutShip) > 0).FirstOrDefault();
                                if (im != null)
                                {
                                    string itemNo = dbClss.TSt(im.ItemNo);
                                    string InternalName = dbClss.TSt(im.ItemName);
                                    string InternalDescription = dbClss.TSt(im.Description);
                                    decimal RemainQty = 0;
                                    string UOM = dbClss.TSt(im.UOM);
                                    string Location = dbClss.TSt(im.LocationItem);
                                    decimal PCSUnit = Convert.ToDecimal(im.PCSUnit);//dbClss.Con_UOM(itemNo, UOM);
                                    decimal StandardPrice = Convert.ToDecimal(im.UnitPrice);

                                    RemainQty = (Convert.ToDecimal(db.Cal_QTY_Remain_Location(itemNo, "Invoice", 0, Location,0)));
                                    if (RemainQty > 0)
                                    {
                                        var rowE = dgvData.Rows.AddNew();
                                        addRow(rowE.Index, DateTime.Now, itemNo, InternalName, InternalDescription, Location
                                            , 0, UOM, PCSUnit, StandardPrice, StandardPrice * 0
                                            , false, 0 * PCSUnit, 0 * PCSUnit, 0, "Waiting", "Waiting",
                                             "", 0,0, "T", "SO");
                                    }
                                }
                            }
                        }
                    }
                    SetRowNo1(dgvData);

                }
            }
        }

        private void MasterTemplate_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            RadMultiColumnComboBoxElement mccbEl = e.ActiveEditor as RadMultiColumnComboBoxElement;
            if (mccbEl != null)
            {
                mccbEl.DropDownSizingMode = SizingMode.UpDownAndRightBottom;
                mccbEl.DropDownMinSize = new Size(150, 200);
                mccbEl.DropDownMaxSize = new Size(150, 200);

                mccbEl.AutoSizeDropDownToBestFit = false;
                mccbEl.DropDownAnimationEnabled = false;
                mccbEl.AutoFilter = true;
                FilterDescriptor filterDescriptor = new FilterDescriptor(mccbEl.DisplayMember, FilterOperator.Contains, string.Empty);
                mccbEl.EditorControl.MasterTemplate.FilterDescriptors.Add(filterDescriptor);
            }
        }
    }


}
