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
    public partial class Shipment : Telerik.WinControls.UI.RadRibbonForm
    {
        string t_SONo = "";
        string t_CustomerNo = "";
        //Customer P/O to SaleORder
        List<int> idList = new List<int>();
        //List<po_to_so> potoso = new List<po_to_so>();
        DataTable dt_HD = new DataTable();
        DataTable dt_DT = new DataTable();
        public Shipment()
        {
            InitializeComponent();
        }
        public Shipment(string SHNo)
        {
            InitializeComponent();
            txtSHNo.Text = SHNo;
            t_SONo = SHNo;
        }
        public Shipment(string PONo, string CustomerNo)
        {
            InitializeComponent();
            this.t_SONo = PONo;
            this.t_CustomerNo = CustomerNo;
        }
        public Shipment(List<int> idList)
        {
            InitializeComponent();
            this.idList = idList;
        }

        bool demo = false;
        public Shipment(bool demo)
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
                {
                    txtSHNo.Text = t_SONo;
                    DataLoad();
                }
                else if (t_SONo != "")
                {
                    txtSHNo.Text = t_SONo;
                    DataLoad();
                }
                else if (idList.Count > 0)
                    LoadFromId();

                
               

                if (lblStatus.Text.Equals("New"))
                    txtSHNo.Text = dbClss.GetNo(30, 0);

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
                    if (mh.Count > 0)
                    {
                        foreach (var rd in mh)
                        {
                            cboCustomer.Items.Add(rd.Name);
                        }
                    }
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
                //int ck = 0;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var mh = db.mh_Shipments.Where(s => s.SSNo == txtSHNo.Text).ToList();
                    if (mh.Count>0)
                    {
                        txtTel.Text = dbClss.TSt(mh.FirstOrDefault().Tel);
                        txtTotal.Text = dbClss.TSt(mh.FirstOrDefault().TotalPrice); 
                        txtGrandTotal.Text = dbClss.TSt(mh.FirstOrDefault().TotalPriceIncVat).ToString();
                        txtFax.Text = dbClss.TSt(mh.FirstOrDefault().Fax);
                        txtEmail.Text = dbClss.TSt(mh.FirstOrDefault().Email);
                        txtContactName.Text = dbClss.TSt(mh.FirstOrDefault().ContactName);
                        txtAddress.Text = dbClss.TSt(mh.FirstOrDefault().CustomerAddress);
                        cboCustomer.Text = dbClss.TSt(mh.FirstOrDefault().CustomerName);
                        txtRemark.Text = dbClss.TSt(mh.FirstOrDefault().Remark);
                        txtCSTMNo.Text = dbClss.TSt(mh.FirstOrDefault().CustomerNo);
                        dtSODate.Value = Convert.ToDateTime(mh.FirstOrDefault().SSDate);
                        txtVatA.Text = dbClss.TSt(mh.FirstOrDefault().VatA);
                        cbVat.Checked = dbClss.TBo(mh.FirstOrDefault().Vat);
                        lblStatus.Text = dbClss.TSt(mh.FirstOrDefault().StatusHD);
                        dt_HD = StockControl.dbClss.LINQToDataTable(mh);
                        //var deletTemp = db.mh_ShipmentDTTemps.Where(t => t.UserID == ConnectDB.user && t.SSNo == txtSHNo.Text).ToList();
                        //if (deletTemp.Count > 0)
                        //{
                        //    db.mh_ShipmentDTTemps.DeleteAllOnSubmit(deletTemp);
                        //}

                        int rows1 = 0;
                        var list1 = db.mh_ShipmentDTs.Where(w => w.SSNo == txtSHNo.Text && !w.Status.Equals("Cancel")).ToList();
                        if (list1.Count > 0)
                        {
                            dt_DT = StockControl.dbClss.LINQToDataTable(list1);
                            dgvData.DataSource = list1;
                            int No1 = 0;
                            foreach (GridViewRowInfo rd in dgvData.Rows)
                            {
                                No1 += 1;
                                rd.Cells["RNo"].Value = No1;
                                rd.Cells["Qty"].ReadOnly = true;
                                rd.Cells["UnitPrice"].ReadOnly = true;
                                rd.Cells["RefDocNo"].ReadOnly = true;
                            }
                            //foreach (var rd in list1)
                            //{
                            //    mh_Item im = db.mh_Items.Where(m => m.InternalNo == rd.ItemNo).FirstOrDefault();
                            //    if (im != null)
                            //    {
                            //        rows1 += 1;
                            //        mh_ShipmentDTTemp st = new mh_ShipmentDTTemp();
                            //        st.RNo = rows1;
                            //        st.SSNo = txtSHNo.Text;
                            //        st.UserID = ClassLib.Classlib.User;
                            //        st.ItemNo = rd.ItemNo;
                            //        st.ItemName = rd.ItemName;
                            //        st.Description = rd.Description;
                            //        st.Qty = rd.Qty;
                            //        st.PCSUnit = rd.PCSUnit;
                            //        st.LocationItem = rd.LocationItem;
                            //        st.UnitPrice = rd.UnitPrice;
                            //        st.Amount = rd.Amount;
                            //        st.Active = true;
                            //        st.UOM = rd.UOM;
                            //        st.RefDocNo = rd.RefDocNo;
                            //        st.RefId = rd.RefId;
                            //        st.Status = rd.Status;
                            //        st.OutInv = rd.OutInv;

                            //        db.mh_ShipmentDTTemps.InsertOnSubmit(st);
                            //        db.SubmitChanges();
                            //    }
                            //}
                            //LoadShipment();
                        }

                        //no insert
                        txtSONo.Enabled = false;
                        radButton2.Enabled = false;
                        cboCustomer.Enabled = false;
                        btnEdit.Enabled = false;
                        btnView.Enabled = false;
                        btnSave.Enabled = false;
                        CallTotal();
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
                        var ShipList = db.mh_SaleOrderDTs.Where(s => s.id == id && s.OutShip > 0 && s.Active == true).ToList();
                        if (ShipList.Count > 0)
                            foreach (var gg in ShipList)
                            {
                                Add_Item("", dbClss.TSt(gg.ItemNo), dbClss.TSt(gg.ItemName)
                                    , dbClss.TSt(gg.Description), dbClss.TDe(gg.Qty), dbClss.TDe(gg.Qty)
                                    , dbClss.TSt(gg.UOM), dbClss.TDe(gg.PCSUnit)
                                    , dbClss.TDe(gg.UnitPrice), dbClss.TDe(gg.Amount), "",
                                    dbClss.TSt(gg.SONo), dbClss.TDe(gg.Qty), "", 0, dbClss.TInt(gg.id)
                                    ,dbClss.TSt(gg.LocationItem),dbClss.TSt(gg.VatType), dbClss.TSt(gg.ReplenishmentType), "Adding");
                            }
                    }
                    int No1 = 0;
                    foreach (GridViewRowInfo rd in dgvData.Rows)
                    {
                        No1 += 1;
                        rd.Cells["RNo"].Value = No1;
                    }
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
        //string TempNo_temp = "";
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

            dt_HD.Columns.Add(new DataColumn("id", typeof(int)));
            dt_HD.Columns.Add(new DataColumn("SSNo", typeof(string)));
            dt_HD.Columns.Add(new DataColumn("CustomerNo", typeof(string)));
            dt_HD.Columns.Add(new DataColumn("CustomerName", typeof(string)));
            dt_HD.Columns.Add(new DataColumn("CustomerAddress", typeof(string)));
            dt_HD.Columns.Add(new DataColumn("Tel", typeof(string)));
            dt_HD.Columns.Add(new DataColumn("Fax", typeof(string)));
            dt_HD.Columns.Add(new DataColumn("Email", typeof(string)));
            dt_HD.Columns.Add(new DataColumn("SSDate", typeof(DateTime)));
            dt_HD.Columns.Add(new DataColumn("VatGroup", typeof(int)));
            dt_HD.Columns.Add(new DataColumn("Remark", typeof(string)));
            dt_HD.Columns.Add(new DataColumn("TotalPrice", typeof(decimal)));
            dt_HD.Columns.Add(new DataColumn("Vat", typeof(bool)));
            dt_HD.Columns.Add(new DataColumn("VatA", typeof(decimal)));
            dt_HD.Columns.Add(new DataColumn("VatAmnt", typeof(decimal)));
            dt_HD.Columns.Add(new DataColumn("TotalPriceIncVat", typeof(decimal)));
            dt_HD.Columns.Add(new DataColumn("Active", typeof(bool)));
            dt_HD.Columns.Add(new DataColumn("CreateDate", typeof(DateTime)));
            dt_HD.Columns.Add(new DataColumn("CreateBy", typeof(string)));
            dt_HD.Columns.Add(new DataColumn("UpdateDate", typeof(DateTime)));
            dt_HD.Columns.Add(new DataColumn("UpdateBy", typeof(string)));
            dt_HD.Columns.Add(new DataColumn("StatusHD", typeof(string)));
            dt_HD.Columns.Add(new DataColumn("ContactName", typeof(string)));

            dt_DT.Columns.Add(new DataColumn("id", typeof(int)));
            dt_DT.Columns.Add(new DataColumn("SSNo", typeof(string)));
            dt_DT.Columns.Add(new DataColumn("RefDocNo", typeof(string)));
            dt_DT.Columns.Add(new DataColumn("RefId", typeof(int)));
            dt_DT.Columns.Add(new DataColumn("RNo", typeof(int)));
            dt_DT.Columns.Add(new DataColumn("ItemNo", typeof(string)));
            dt_DT.Columns.Add(new DataColumn("ItemName", typeof(string)));
            dt_DT.Columns.Add(new DataColumn("Description", typeof(string)));
            dt_DT.Columns.Add(new DataColumn("LocationItem", typeof(string)));
            dt_DT.Columns.Add(new DataColumn("Qty", typeof(decimal)));
            dt_DT.Columns.Add(new DataColumn("UOM", typeof(string)));
            dt_DT.Columns.Add(new DataColumn("PCSUnit", typeof(decimal)));
            dt_DT.Columns.Add(new DataColumn("UnitPrice", typeof(decimal)));
            dt_DT.Columns.Add(new DataColumn("Amount", typeof(decimal)));
            dt_DT.Columns.Add(new DataColumn("PriceIncVat", typeof(bool)));
            dt_DT.Columns.Add(new DataColumn("VatType", typeof(string)));
            dt_DT.Columns.Add(new DataColumn("ReplenishmentType", typeof(string)));
            dt_DT.Columns.Add(new DataColumn("Active", typeof(bool)));
            dt_DT.Columns.Add(new DataColumn("OutShip", typeof(decimal)));
            dt_DT.Columns.Add(new DataColumn("OutPlan", typeof(decimal)));
            dt_DT.Columns.Add(new DataColumn("Status", typeof(string)));
            dt_DT.Columns.Add(new DataColumn("OutInv", typeof(decimal)));
            dt_DT.Columns.Add(new DataColumn("DL", typeof(string)));
        }

        string Ac = "";
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
            // txtCSTMNo.Text = "";
            dtSODate.Value = DateTime.Now;
            txtSHNo.Text = "";
            txtCSTMNo.Text = "";
            dtSODate.Value = DateTime.Today;
            dgvData.Rows.Clear();
            dgvData.DataSource = null;
            txtRemark.Text = "";
            txtTotal.Text = (0).ToMoney();
            txtSHNo.Text = dbClss.GetNo(30, 0);
            txtSONo.Text = "";
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
            cbVat.Checked = true;
            Ac = "New";
            row = dgvData.Rows.Count - 1;
            if (row < 0)
                row = 0;
            ////getมาไว้ก่อน แต่ยังไมได้ save 
            ////txtTempNo.Text = StockControl.dbClss.GetNo(10, 0);
            //using (DataClasses1DataContext db = new DataClasses1DataContext())
            //{
            //    var md = db.mh_ShipmentDTTemps.Where(t => t.UserID == ConnectDB.user).ToList();
            //    if (md.Count > 0)
            //    {
            //        foreach (var rd in md)
            //        {
            //            db.mh_ShipmentDTTemps.DeleteOnSubmit(rd);
            //            db.SubmitChanges();
            //        }
            //    }

            //}


            idList.Clear();
            //potoso.Clear();
            //txtSHNo.Text = dbClss.GetNo(30, 0);

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
                string poNo = txtSHNo.Text.Trim();
                string cstmNo = txtCSTMNo.Text.Trim();
                if (poNo != "" && cstmNo != "")
                {
                    using (var db = new DataClasses1DataContext())
                    {
                        int Temp = 0;
                        var ck = db.mh_ShipmentDTs.Where(x => x.SSNo == poNo && x.Active).ToList();
                        if (ck.Where(x => x.Active == true && ( x.OutInv != x.Qty)).Count() > 0)
                        {
                            foreach (var pp in ck)
                            {
                                Temp = 1;
                                break;
                            }
                        }
                        if (Temp == 1)
                        {
                            baseClass.Warning("Shipment Status cannot Delete.");
                            return;
                        }


                        if (baseClass.IsDel($"Do you want to Delete Shipment: {poNo} ?"))
                        {

                            var p = db.mh_Shipments.Where(x => x.SSNo == poNo && x.StatusHD == "Waiting").ToList();
                            if (p.Where(x => x.StatusHD == "Waiting").Count() > 0)
                            {
                                foreach (var pp in p)
                                {
                                    pp.StatusHD = "Cancel";
                                    pp.Active = false;
                                    pp.UpdateBy = Classlib.User;
                                    pp.UpdateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                }

                                var d = db.mh_ShipmentDTs.Where(x => x.SSNo == poNo && x.Active).ToList();
                                if (d.Where(x => x.Active == true && x.OutShip == x.Qty).Count() > 0)
                                {
                                    foreach (var pp in d)
                                    {
                                        pp.Active = false;
                                        pp.Status = "Cancel";
                                    }
                                }

                                db.SubmitChanges();

                                updateOutSO();

                                baseClass.Info("Delete Shipment complete.");
                               // ClearData();
                                btnNew_Click(null, null);
                            }
                            else
                                baseClass.Warning("Shipment Status cannot Delete.");
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
                if (cboCustomer.SelectedValue.ToSt() == "" || txtCSTMNo.Text == "")
                    err += " “Customer:” is empty \n";
                if (dtSODate.Text == "")
                    err += " “Delivery Date:” is empty \n";
                if (dgvData.Rows.Where(x => x.IsVisible).Count() < 1)
                    err += " “Items:” is empty \n";
              
                if (err == "")
                {
                    foreach (var item in dgvData.Rows.Where(x => x.IsVisible))
                    {
                        if (item.Cells["Qty"].Value.ToDecimal() <= 0)
                            err += " “Qty:” is less than 0 \n";

                        //string itemNo = item.Cells["ItemNo"].Value.ToSt();
                        //if (itemNo == "") continue;
                        //if (item.Cells["ReqDate"].Value == null)
                        //    err += " “Request Date.:” is empty \n";

                        //int idPO = item.Cells["RefId"].Value.ToInt();
                        //if (idPO > 0)
                        //{
                        //    var Qty = item.Cells["Qty"].Value.ToDecimal();
                        //    using (var db = new DataClasses1DataContext())
                        //    {
                        //        var qtyPO = db.mh_CustomerPOs.Where(x => x.id == idPO).First().Quantity;
                        //        int idSO = item.Cells["id"].Value.ToInt();
                        //        var qtySO = db.mh_SaleOrders.Where(x => x.Active && x.RefId == idPO && x.id != idSO).ToList();
                        //        if (qtySO.Sum(x => x.Qty * x.PCSUnit) + Qty > qtyPO)
                        //        {
                        //            err += " “Qty:” is more than Customer P/O Qty.\n";
                        //        }
                        //    }
                        //}

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
                dbClss.AddError("Shipment", ex.Message, this.Name);
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
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    if (Ac.Equals("New"))
                    {
                        //ถ้ามีการใส่เลขที่ PR เช็คดูว่ามีการใส่เลขนี้แล้วหรือไม่ ถ้ามีให้ใส่เลขอื่น
                        if (!txtSHNo.Text.Equals(""))
                        {
                            var p = (from ix in db.mh_Shipments
                                     where ix.SSNo.ToUpper().Trim() == txtSHNo.Text.Trim()
                                     && ix.Active == true
                                     select ix).ToList();
                            if (p.Count > 0)  //มีรายการในระบบ
                            {
                                MessageBox.Show("เลขที่ใบเบิกถูกใช้ไปแล้ว กรุณาใส่เลขใหม่");
                                return;
                            }
                        }
                        else
                            txtSHNo.Text = dbClss.GetNo(30, 2);
                    }

                    string sono = txtSHNo.Text;
                    if (sono != "")
                    {
                        //string cstmNo = txtContactName.Text;
                        CallTotal();
                        
                        SaveHerder();
                        SaveDetail();

                        baseClass.Info("Save complete(s).");
                        ClearData();
                        txtSHNo.Text = sono;
                        DataLoad();
                    }                    
                }
            }
            catch (Exception ex)
            {
                baseClass.Error(ex.Message);
            }
            finally { this.Cursor = Cursors.Default; }
        }
        private void SaveHerder()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                string CSTMNo = "";
                CSTMNo = db.getCSTMNo(cboCustomer.Text);
                decimal vatA = 0;
                decimal totalPrice = 0;
                decimal vatAmount = 0;
                decimal grantotal = 0;
                decimal.TryParse(txtGrandTotal.Text, out grantotal);
                decimal.TryParse(txtVatAmnt.Text, out vatAmount);
                decimal.TryParse(txtTotal.Text, out totalPrice);
                decimal.TryParse(txtVatA.Text, out vatA);


                var p = (from ix in db.mh_Shipments
                         where ix.SSNo.ToUpper().Trim() == txtSHNo.Text.Trim()
                         && ix.Active == true
                         select ix).ToList();
                if (p.Count > 0)  //มีรายการในระบบ
                {
                    //foreach (DataRow row in dt_HD.Rows)
                    //{
                    //    var gg = (from ix in db.mh_Shipments
                    //              where ix.SSNo.Trim() == txtSHNo.Text.Trim() && ix.Active == true
                    //              //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                    //              select ix).First();
                    //    gg.ModifyBy = ClassLib.Classlib.User;
                    //    gg.ModifyDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    //    dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไข CreatePO โดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", txtPONo.Text);

                    //    if (!txtPONo.Text.Trim().Equals(row["PONo"].ToString()))
                    //    {
                    //        gg.PONo = txtPONo.Text;

                    //        dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขเลขที่ใบสั่งซื้อ [" + txtPONo.Text.Trim() + "]", txtPONo.Text);
                    //    }
                    //}
                }
                else
                {

                    mh_Shipment sh1 = new mh_Shipment();
                    sh1.SSNo = txtSHNo.Text;
                    sh1.Remark = txtRemark.Text;
                    sh1.SSDate = dtSODate.Value;
                    sh1.Tel = txtTel.Text;
                    sh1.Email = txtEmail.Text;
                    sh1.Fax = txtFax.Text;
                    sh1.UpdateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    sh1.UpdateBy = ConnectDB.user;
                    sh1.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    sh1.CreateBy = ConnectDB.user;// ClassLib.Classlib.User;

                    sh1.CustomerNo = CSTMNo;
                    sh1.CustomerName = cboCustomer.Text;
                    sh1.CustomerAddress = txtAddress.Text;
                    sh1.Vat = cbVat.Checked;
                    sh1.VatA = vatA;
                    sh1.VatAmnt = vatAmount;
                    sh1.TotalPrice = totalPrice;
                    sh1.TotalPriceIncVat = grantotal;
                    sh1.StatusHD = "Waiting";
                    sh1.ContactName = txtContactName.Text;
                    sh1.Active = true;
                    db.mh_Shipments.InsertOnSubmit(sh1);
                    db.SubmitChanges();

                    dbClss.AddHistory(this.Name, "เพิ่ม Shipment", "สร้าง Shipment [" + txtSHNo.Text + "]", txtSHNo.Text);

                }
            }
        }
        private void SaveDetail()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                ////Addd DT///
                dgvData.EndEdit();
                int Refid = 0;
                foreach (GridViewRowInfo rd in dgvData.Rows)
                {
                    if (rd.IsVisible && dbClss.TInt(rd.Cells["id"].Value)==0)
                    {
                        mh_ShipmentDT sh1 = new mh_ShipmentDT();
                        sh1.SSNo = txtSHNo.Text;
                        sh1.RefDocNo = dbClss.TSt(rd.Cells["RefDocNo"].Value);
                        sh1.RefId = dbClss.TInt(rd.Cells["RefId"].Value);
                        sh1.ItemName = dbClss.TSt(rd.Cells["ItemName"].Value);
                        sh1.Active = true;
                        sh1.Description = dbClss.TSt(rd.Cells["Description"].Value);
                        sh1.ItemNo = dbClss.TSt(rd.Cells["ItemNo"].Value);
                        sh1.Qty = dbClss.TDe(rd.Cells["Qty"].Value);
                        sh1.UOM = dbClss.TSt(rd.Cells["Unit"].Value);
                        sh1.PCSUnit = dbClss.TDe(rd.Cells["PCSUnit"].Value);
                        sh1.UnitPrice = dbClss.TDe(rd.Cells["UnitPrice"].Value);
                        sh1.Amount = dbClss.TDe(rd.Cells["Amount"].Value);
                        sh1.OutInv = dbClss.TDe(rd.Cells["Qty"].Value);
                        //sh1.OutPlan = 0;
                        sh1.OutShip = dbClss.TDe(rd.Cells["Qty"].Value);
                        sh1.PriceIncVat = cbVat.Checked;
                        sh1.ReplenishmentType = dbClss.TSt(rd.Cells["ReplenishmentType"].Value);
                        sh1.RNo = dbClss.TInt(rd.Cells["RNo"].Value);
                        sh1.VatType = dbClss.TSt(rd.Cells["VatType"].Value);
                        sh1.DL = false;
                        sh1.Active = true;
                        
                        db.mh_ShipmentDTs.InsertOnSubmit(sh1);
                        db.SubmitChanges();
                        dbClss.AddHistory(this.Name, "เพิ่ม Shipment", "สร้าง Shipment [ ItemNo : " + dbClss.TSt(rd.Cells["ItemNo"].Value) +" Qty : "+ dbClss.TSt(rd.Cells["Qty"].Value)+" Unit : "+ dbClss.TSt(rd.Cells["Unit"].Value) + "]", txtSHNo.Text);

                        var v = (from ix in db.mh_SaleOrderDTs
                                 where
                                           ix.id == Convert.ToInt16(rd.Cells["RefId"].Value.ToSt())
                                           && ix.OutShip>0 && ix.Active==true
                                 select ix).ToList();
                        if (v.Count > 0)
                        {
                            var p = (from ix in db.mh_SaleOrderDTs
                                     where
                                        ix.id == Convert.ToInt16(rd.Cells["RefId"].Value.ToSt())
                                         && ix.OutShip > 0 && ix.Active == true
                                     select ix).First();

                            p.OutShip = p.OutShip - Convert.ToDecimal(rd.Cells["Qty"].Value.ToSt());
                           

                            dbClss.AddHistory(this.Name, "ปรับสถานะ mh_SaleOrderDTs ", "ปรับ OutShip เพราะมีการทำ Shipment : " + rd.Cells["ItemNo"].ToSt() + " จำนวน : " + (rd.Cells["Qty"].Value.ToSt())
                            + " SaleOrderDT :" + Convert.ToString(rd.Cells["RefDocNo"].Value)
                            + " ปรับโดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", Convert.ToString(rd.Cells["RefDocNo"].Value));

                            db.SubmitChanges();

                        }


                        //mh_SaleOrderDT sd = db.mh_SaleOrderDTs
                        //    .Where(ss => ss.id == Refid).FirstOrDefault();
                        //if (sd != null)
                        //{
                        //    sd.OutShip = sd.OutShip - Math.Round(Convert.ToDecimal(rd.Cells["Qty"].Value.ToSt()) * mtem.PCSUnit, 2);
                        //     //db.SubmitChanges();
                        //}
                        /////////////////////////
                        ////db.mh_ShipmentDTs.InsertOnSubmit(sd);
                        //db.SubmitChanges();
                    }

                }
            }
        }

        private void CustStock()
        {
            try
            {

            }
            catch { }
        }

        private void updateOutSO()
        {
            using (var db = new DataClasses1DataContext())
            {//Update SaleOrder Qty)
                foreach (GridViewRowInfo rd in dgvData.Rows)
                {
                    var v = (from ix in db.mh_SaleOrderDTs
                             where
                                       ix.id == Convert.ToInt16(rd.Cells["RefId"].Value.ToSt()) && ix.Active == true
                             select ix).ToList();
                    if (v.Count > 0)
                    {
                        var p = (from ix in db.mh_SaleOrderDTs
                                 where
                                    ix.id == Convert.ToInt16(rd.Cells["RefId"].Value.ToSt()) && ix.Active == true
                                 select ix).First();

                        p.OutShip = p.OutShip + Convert.ToDecimal(rd.Cells["Qty"].Value.ToSt());
                        if (p.Qty < p.OutShip)
                            p.OutShip = p.Qty;

                        dbClss.AddHistory(this.Name, "ปรับสถานะ mh_SaleOrderDTs ", "ปรับ OutShip เพราะลบ Shipment : "+ rd.Cells["ItemNo"].ToSt() +" จำนวน : " + (rd.Cells["Qty"].Value.ToSt())
                        + " SaleOrderDT :" + Convert.ToString(rd.Cells["RefDocNo"].Value)
                        + " ปรับโดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", Convert.ToString(rd.Cells["RefDocNo"].Value));

                        db.SubmitChanges();

                    }
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
                        if (txtContactName.Text == "")
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
                                var cc = db.mh_Customers.Where(x => x.No == txtContactName.Text).First();
                                addRow(e.RowIndex, DateTime.Now, t.InternalNo, t.InternalName, "", t.Location
                                    , 1, t.BaseUOM, pcsunit, 0, 0, false, 1 * pcsunit, 1 * pcsunit, 0
                                    , "Waiting", "Waiting", cc.VatGroup, t.VatType, "", 0, t.ReplenishmentType, "T");
                            }
                            else
                            {
                                e.Row.Cells["ItemName"].Value = t.InternalName;
                                // e.Row.Cells["Unit"].Value = t.BaseUOM;
                                e.Row.Cells["PCSUnit"].Value = pcsunit;
                                e.Row.Cells["OutShip"].Value = e.Row.Cells["Qty"].Value.ToDecimal() * e.Row.Cells["PCSUnit"].Value.ToDecimal();
                                e.Row.Cells["OutPlan"].Value = e.Row.Cells["Qty"].Value.ToDecimal() * e.Row.Cells["PCSUnit"].Value.ToDecimal();
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
                            e.Row.Cells["OutShip"].Value = e.Row.Cells["Qty"].Value.ToDecimal() * e.Row.Cells["PCSUnit"].Value.ToDecimal();
                            e.Row.Cells["OutPlan"].Value = e.Row.Cells["Qty"].Value.ToDecimal() * e.Row.Cells["PCSUnit"].Value.ToDecimal();
                        }
                    }

                    e.Row.Cells["dgvC"].Value = "T";
                    //CallTotal();
                }
            }
            catch { }
        }
        private void MasterTemplate_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            if (e.RowIndex >= -1)
            {
                //string itemNo = e.Row.Cells["ItemNo"].Value.ToSt();
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
                //else if (e.Column.Name.Equals("ItemNo"))
                //{
                //    beginItem = itemNo;
                //}
            }
        }
        void addRow(int rowIndex, DateTime ReqDate, string ItemNo, string ItemName, string Desc
            , string Location, decimal Qty, string UOM, decimal PCSUnit, decimal UnitPrice, decimal Amount
            , bool PriceIncVat, decimal OutShip, decimal OutPlan, int id, string Status, string PlanStatus, int VatGroup, string VatType
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
                rowE.Cells["Location"].Value = Location;
                rowE.Cells["Qty"].Value = Qty;
                rowE.Cells["Unit"].Value = UOM;
                rowE.Cells["PCSUnit"].Value = PCSUnit;
                rowE.Cells["UnitPrice"].Value = UnitPrice;
                rowE.Cells["Amount"].Value = Amount;
                rowE.Cells["PriceIncVat"].Value = PriceIncVat;
                rowE.Cells["VatGroup"].Value = VatGroup;
                rowE.Cells["VatType"].Value = VatType;
                rowE.Cells["OutShip"].Value = OutShip;
                rowE.Cells["Status"].Value = Status;
                rowE.Cells["RefDocNo"].Value = RefDocNo;
                rowE.Cells["RefId"].Value = RefId;
                rowE.Cells["RepType"].Value = RepType;
                rowE.Cells["dgvC"].Value = dgvC; //if Edit row -> value = T
                rowE.Cells["PlanStatus"].Value = PlanStatus;
                rowE.Cells["OutPlan"].Value = OutPlan;

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


                if (dgvData.Rows.Count <= 0)
                    return;


                if (Ac.Equals("New") || Ac.Equals("Edit"))
                {
                    this.Cursor = Cursors.WaitCursor;

                    if (StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["Status"].Value) == "ADD"
                        || StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["Status"].Value) == "Adding")
                    {

                        int id = 0;
                        int.TryParse(StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["id"].Value), out id);
                        if (id <= 0)
                            dgvData.Rows.Remove(dgvData.CurrentRow);

                        else
                        {

                            decimal OutInv = dbClss.TDe(dgvData.CurrentRow.Cells["OutInv"].Value);
                            decimal Qty = dbClss.TDe(dgvData.CurrentRow.Cells["Qty"].Value);
                            if (OutInv != Qty)
                            {
                                MessageBox.Show("ไม่สามารถทำการลบรายการได้ บางรายการทำ Invoice แล้ว");
                            }
                            else
                            { 
                                string CodeNo = "";
                                CodeNo = StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["ItemNo"].Value);
                                if (MessageBox.Show("ต้องการลบรายการ ( " + CodeNo + " ) ออกจากรายการ หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    dgvData.CurrentRow.IsVisible = false;
                                }
                            }
                            
                            //row = dgvData.CurrentCell.RowInfo.Index;
                            ////btnDelete_Click(null, null);
                            //using (var db = new DataClasses1DataContext())
                            //{
                            //    var mm = db.mh_ShipmentDTs.Where(x => x.id == id 
                            //    && x.OutInv<=0 ).ToList();

                            //    if (mm.Count>0)
                            //    {
                            //        var m = db.mh_ShipmentDTs.Where(x => x.id == id
                            //        && x.OutInv < -0).ToList();

                            //        m.Active = false;
                            //        m.Status = "Cancel";
                            //        m.UpdateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                            //        m.UpdateBy = ClassLib.Classlib.User;
                            //        db.SubmitChanges();

                            //        updateOutSO();
                            //        dgvData.Rows.Remove(dgvData.CurrentCell.RowInfo);
                            //    }
                            //}
                        }
                        CallTotal();
                        Set_Row();
                    }
                    else
                        MessageBox.Show("ไม่สามารถทำการลบรายการได้ สถานะไม่ถูกต้อง");
                }
                else
                {
                    MessageBox.Show("สถานะต้องเป็น New หรือ Edit เท่านั่น");
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
                var sm = new Shipment_List(dgvRow_List);
                this.Cursor = Cursors.Default;
                sm.ShowDialog();
                if (dgvRow_List.Count > 0)
                {
                    string SONo = "";
                    this.Cursor = Cursors.WaitCursor;

                    foreach (GridViewRowInfo ee in dgvRow_List)
                    {
                        SONo = dbClss.TSt(ee.Cells["ShipmentNo"].Value);
                        break;
                    }

                    txtSHNo.Text = SONo;
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
                if (t_SONo != "")
                    DataLoad();


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
            
        }


        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {

                Report.Reportx1.Value = new string[2];
                Report.Reportx1.Value[0] = txtSHNo.Text;
                Report.Reportx1.Value[1] = txtSHNo.Text;
                Report.Reportx1.WReport = "Shipment";
                Report.Reportx1 op = new Report.Reportx1("ReportDelivery.rpt");
                op.Show();

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
        }

        private void btnAddPart_Click(object sender, EventArgs e)
        {
            AddPartE();
        }
        void AddPartE()
        {
            if (txtContactName.Text == "")
            {
                baseClass.Warning("Please select Customers first.\n");
                return;
            }
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
                        var cc = db.mh_Customers.Where(x => x.No == txtContactName.Text).First();
                        addRow(rowE.Index, DateTime.Now, itemNo, t.InternalName, "", t.Location
                            , 1, t.BaseUOM, u, 0, 0, false, 1 * u, 1 * u, 0, "Waiting", "Waiting"
                            , cc.VatGroup, t.VatType, "", 0, t.ReplenishmentType, "T");
                    }
                    SetRowNo1(dgvData);

                }
            }
        }

        private void txtPONo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtSHNo.Text.Trim() == "")
                {
                    baseClass.Warning("Please enter Sale Order no.");
                    return;
                }
                if (txtContactName.Text.Trim() == "")
                {
                    baseClass.Warning("Please enter Customer no.");
                    return;
                }

                t_SONo = txtSHNo.Text;
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

        private void radButton1_Click(object sender, EventArgs e)
        {
            try
            {
                ShipmentListPart sh = new ShipmentListPart(txtSHNo.Text);
                sh.ShowDialog();
                LoadShipment();
                CallTotal();
            }
            catch { }
        }
        private void LoadShipment()
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var ShipList = db.mh_ShipmentDTTemps.Where(s => s.SSNo == txtSHNo.Text).ToList();
                    if (ShipList.Count > 0)
                    {
                        //dgvData.DataSource = ShipList;
                        foreach (var gg in ShipList)
                        {
                            Add_Item("", dbClss.TSt(gg.ItemNo), dbClss.TSt(gg.ItemName)
                                , dbClss.TSt(gg.Description),dbClss.TDe(gg.Qty), dbClss.TDe(gg.Qty)
                                , dbClss.TSt(gg.UOM), dbClss.TDe(gg.PCSUnit)
                                , dbClss.TDe(gg.UnitPrice), dbClss.TDe(gg.Amount), "",
                                dbClss.TSt(gg.RefDocNo), dbClss.TDe(gg.Qty), "", 0, dbClss.TInt(gg.RefId)
                                ,dbClss.TSt(gg.LocationItem),dbClss.TSt(gg.VatType),dbClss.TSt(gg.ReplenishmentType), "Adding");
                            }
                        Set_Row();

                        mh_ShipmentDTTemp md = db.mh_ShipmentDTTemps.Where(s => s.SSNo == txtSHNo.Text).FirstOrDefault();
                        if (md != null)
                        {
                            db.mh_ShipmentDTTemps.DeleteOnSubmit(md);
                            db.SubmitChanges();
                        }
                    }
                }
            }
            catch { }
        }

        private void Add_Item(string RNo, string ItemNo
       , string ItemName,string Description ,decimal Remain,decimal Qty, string Unit, decimal PCSUnit
      , decimal UnitPrice, decimal Amount, string dgvC, string RefDocNo, decimal OutInv
      , string dgvA, int id, int refid,string LocationItem,string VatType,string ReplenishmentType, string Status
       )
        {
            try
            {

                if(refid>0)
                {
                    if (check_Duppicate(refid))
                        return;
                }

                int rowindex = -1;
                GridViewRowInfo ee;
                if (rowindex == -1)
                {
                    ee = dgvData.Rows.AddNew();
                }
                else
                    ee = dgvData.Rows[rowindex];

                ee.Cells["RNo"].Value = RNo.ToString();
                ee.Cells["Description"].Value = Description;
                ee.Cells["ItemNo"].Value = ItemNo;
                ee.Cells["ItemName"].Value = ItemName;
                ee.Cells["Qty"].Value = Qty;               
                ee.Cells["Unit"].Value = Unit;
                ee.Cells["PCSUnit"].Value = PCSUnit;
                ee.Cells["UnitPrice"].Value = UnitPrice;
                ee.Cells["Amount"].Value = Amount;
                ee.Cells["dgvC"].Value = dgvC;
                ee.Cells["RefDocNo"].Value = RefDocNo;
                ee.Cells["OutInv"].Value = OutInv;
                ee.Cells["dgvA"].Value = dgvA;
                ee.Cells["Location"].Value = Location;
                ee.Cells["LocationItem"].Value = LocationItem;
                ee.Cells["id"].Value = id;
                ee.Cells["RefId"].Value = refid;
                ee.Cells["Status"].Value = Status;
                ee.Cells["Remain"].Value = Remain;
                ee.Cells["VatType"].Value = VatType;
                ee.Cells["ReplenishmentType"].Value = ReplenishmentType;
                //    ee.Cells["dgvItemDesc"].ReadOnly = true;                

                ////if (lblStatus.Text.Equals("Completed"))//|| lbStatus.Text.Equals("Reject"))
                ////    dgvData.AllowAddNewRow = false;
                ////else
                ////    dgvData.AllowAddNewRow = true;

                ////dbclass.SetRowNo1(dgvData);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : Add_Item", this.Name); }

        }
        private void Set_Row()
        {
            int No1 = 0;
            foreach (GridViewRowInfo rd in dgvData.Rows)
            {
                No1 += 1;
                rd.Cells["RNo"].Value = No1;
            }
        }
        private bool check_Duppicate(int Refid)
        {
            bool re = false;
            foreach (var rd1 in dgvData.Rows)
            {
                if (rd1.IsVisible.Equals(true))
                {
                    if (StockControl.dbClss.TInt(rd1.Cells["refId"].Value).Equals(Refid))
                        re = true;
                }
            }

            return re;

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
                        if (mc != null)
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
            try
            {
                ShipmentOrderList sh = new ShipmentOrderList(txtSHNo.Text);
                sh.ShowDialog();
                LoadShipment();
                CallTotal();
            }
            catch { }
        }

        private void txtSONo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                LoadData_insert();
                txtSONo.Text = "";
                CallTotal();
            }

        }
        private void LoadData_insert()
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var sh = db.mh_SaleOrders.Where(s => s.SONo == txtSHNo.Text  
                        && s.Active == true && Convert.ToInt16(s.SeqStatus)==2).ToList();
                    if (sh.Count > 0)
                    {
                        var ShipList = db.mh_SaleOrderDTs.Where(s => s.SONo == txtSHNo.Text 
                            && s.OutShip > 0 && s.Active == true).ToList();
                        if (ShipList.Count > 0)
                            foreach (var gg in ShipList)
                            {
                                Add_Item("", dbClss.TSt(gg.ItemNo), dbClss.TSt(gg.ItemName)
                                    , dbClss.TSt(gg.Description), dbClss.TDe(gg.Qty), dbClss.TDe(gg.Qty)
                                    , dbClss.TSt(gg.UOM), dbClss.TDe(gg.PCSUnit)
                                    , dbClss.TDe(gg.UnitPrice), dbClss.TDe(gg.Amount), "",
                                    dbClss.TSt(gg.SONo), dbClss.TDe(gg.Qty), "", 0, dbClss.TInt(gg.id)
                                    ,dbClss.TSt(gg.LocationItem),dbClss.TSt(gg.VatType), dbClss.TSt(gg.ReplenishmentType), "Adding");
                            }
                        int No1 = 0;
                        foreach (GridViewRowInfo rd in dgvData.Rows)
                        {
                            No1 += 1;
                            rd.Cells["RNo"].Value = No1;
                        }
                    }
                }
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnAddPart_Click_1(object sender, EventArgs e)
        {
            radButton2_Click(null, null);
        }
    }

}
