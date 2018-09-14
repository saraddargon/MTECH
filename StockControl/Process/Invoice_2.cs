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
            txtSONo.Text = SHNo;
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

                ClearData();
                btnNew_Click(null, null);

                foreach (var item in a)
                {
                    idList.Add(item);
                }

                if (t_SONo != "" && t_CustomerNo != "")
                    DataLoad();
                else if (idList.Count > 0)
                    LoadFromId();

                txtSONo.Text = t_SONo;
                DataLoad();

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
                com4.DisplayMember = "UnitDetail";
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
                    mh_InvoiceHD mh = db.mh_InvoiceHDs.Where(s => s.IVNo == txtSONo.Text).FirstOrDefault();
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

                        var deletTemp = db.mh_InvoiceDTTemps.Where(t =>  t.IVNo == txtSONo.Text).ToList();
                        if(deletTemp.Count>0)
                        {
                            db.mh_InvoiceDTTemps.DeleteAllOnSubmit(deletTemp);
                        }

                        int rows1 = 0;

                        var list1 = db.mh_InvoiceDTs.Where(w => w.IVNo == txtSONo.Text && !w.Status.Equals("Cancel")).ToList();
                        if (list1.Count > 0)
                        {
                            foreach (var rd in list1)
                            {
                                mh_Item im = db.mh_Items.Where(m => m.InternalNo == rd.ItemNo).FirstOrDefault();
                                if (im != null)
                                {
                                    rows1 += 1;
                                    mh_InvoiceDTTemp st = new mh_InvoiceDTTemp();
                                    st.RNo = rows1;
                                    st.IVNo = txtSONo.Text;
                                    //st.UserID = ClassLib.Classlib.User;
                                    st.ItemNo = rd.ItemNo;
                                    st.ItemName = rd.ItemName;
                                    st.Description = rd.Description;
                                    st.Qty = rd.Qty;
                                    st.PCSUnit = rd.PCSUnit;
                                    st.LocationItem = rd.LocationItem;
                                    st.UnitPrice = rd.UnitPrice;
                                    st.Amount = rd.Amount;
                                    st.Active = true;
                                    st.UOM = rd.UOM;
                                    db.mh_InvoiceDTTemps.InsertOnSubmit(st);
                                    db.SubmitChanges();
                                }
                            }
                            LoadShipment();
                        }
                            
                        
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
                    //bool fRow = true;
                    //foreach (var id in idList)
                    //{
                    //    var c = db.mh_CustomerPOs.Where(x => x.id == id).First();
                    //    if (fRow)
                    //    {
                    //        txtCSTMNo.Text = c.CustomerNo;
                    //        dtSODate.Value = DateTime.Now;
                    //        cbbCSTM_SelectedIndexChanged(null, null);
                    //        txtRemark.Text = c.RemarkHD;
                    //        fRow = false;
                    //    }
                    //    //detail
                    //    var rowe = dgvData.Rows.AddNew();
                    //    var t = db.mh_Items.Where(x => x.InternalNo == c.ItemNo).First();
                    //    var cstm = db.mh_Customers.Where(x => x.No == c.CustomerNo).First();
                    //    addRow(rowe.Index, c.ReqDate, c.ItemNo, c.ItemName, "", t.Location
                    //        , Math.Round(c.OutSO / c.PCSUnit, 2), c.UOM, c.PCSUnit, c.PricePerUnit, c.Amount, false, c.OutSO, c.OutPlan
                    //        , 0, "Waiting", "Waiting", cstm.VatGroup, t.VatType, c.CustomerPONo, c.id, t.ReplenishmentType
                    //        , "T");

                    //    //potoso.Add(new po_to_so
                    //    //{
                    //    //    idPO = c.id,
                    //    //    poQty = c.Quantity * c.PCSUnit,
                    //    //    poAmnt = c.Amount
                    //    //});
                    //}
                    //SetRowNo1(dgvData);
                    //CallTotal();
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
            txtSONo.Text = "";
            txtCSTMNo.Text = "";
            dtSODate.Value = DateTime.Today;
            dgvData.Rows.Clear();
            dgvData.DataSource = null;
            txtRemark.Text = "";
            txtTotal.Text = (0).ToMoney();
            txtSONo.Text = dbClss.GetNo(31, 0);
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
            txtSONo.Text = dbClss.GetNo(31, 0);

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
                //string poNo = txtSONo.Text.Trim();
                //string cstmNo = txtCSTMNo.Text.Trim();
                //if (poNo != "" && cstmNo != "")
                //{
                //    if (dgvData.Rows.Where(x => x.Cells["PlanStatus"].Value.ToSt() != "Waiting").Count() > 0)
                //    {
                //        baseClass.Warning("Cannot Delete because Already Planned.\n");
                //        return;
                //    }

                //    if (baseClass.IsDel($"Do you want to Delete Sale Order: {poNo} ?"))
                //    {
                //        using (var db = new DataClasses1DataContext())
                //        {
                //            var p = db.mh_SaleOrders.Where(x => x.SONo == poNo && x.Active).ToList();
                //            if (p.Where(x => x.Status != "Waiting").Count() < 1)
                //            {
                //                foreach (var pp in p)
                //                {
                //                    pp.Active = false;
                //                    pp.UpdateBy = Classlib.User;
                //                    pp.UpdateDate = DateTime.Now;
                //                }

                //                db.SubmitChanges();

                //                updateOutSO();

                //                baseClass.Info("Delete Sale Order complete.");
                //                ClearData();
                //                btnNew_Click(null, null);
                //            }
                //            else
                //                baseClass.Warning("Sale Order Status cannot Delete.");
                //        }
                //    }
                //}
                MessageBox.Show("Comming Soon");

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
                if (dgvData.Rows.Where(x => x.IsVisible).Count() < 1)
                    err += " “Items:” is empty \n";
                if (err == "")
                {
                    //foreach (var item in dgvData.Rows.Where(x => x.IsVisible))
                    //{
                    //    string itemNo = item.Cells["ItemNo"].Value.ToSt();
                    //    if (itemNo == "") continue;
                    //    if (item.Cells["ReqDate"].Value == null)
                    //        err += " “Request Date.:” is empty \n";
                    //    if (item.Cells["Qty"].Value.ToDecimal() <= 0)
                    //        err += " “Qty:” is less than 0 \n";
                    //    int idPO = item.Cells["RefId"].Value.ToInt();
                    //    if (idPO > 0)
                    //    {
                    //        var Qty = item.Cells["Qty"].Value.ToDecimal();
                    //        using (var db = new DataClasses1DataContext())
                    //        {
                    //            var qtyPO = db.mh_CustomerPOs.Where(x => x.id == idPO).First().Quantity;
                    //            int idSO = item.Cells["id"].Value.ToInt();
                    //            var qtySO = db.mh_SaleOrders.Where(x => x.Active && x.RefId == idPO && x.id != idSO).ToList();
                    //            if (qtySO.Sum(x => x.Qty * x.PCSUnit) + Qty > qtyPO)
                    //            {
                    //                err += " “Qty:” is more than Customer P/O Qty.\n";
                    //            }
                    //        }
                    //    }

                    //    if (err != "")
                    //        break;
                    //}

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
                txtSONo.Text = dbClss.GetNo(31, 2);
                string sono = txtSONo.Text;
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
                        sh1.IVNo = txtSONo.Text;
                        sh1.Remark = txtRemark.Text;
                        sh1.SSDate = dtSODate.Value;
                        sh1.Tel = txtTel.Text;
                        sh1.Email = txtEmail.Text;
                        sh1.Fax = txtFax.Text;
                        sh1.UpdateDate = DateTime.Now;
                        sh1.UpdateBy = ConnectDB.user;
                        sh1.CreateDate = DateTime.Now;
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
                        db.mh_InvoiceHDs.InsertOnSubmit(sh1);
                        db.SubmitChanges();

                        ////Addd DT///
                        dgvData.EndEdit();
                        int rno = 0;
                        foreach(GridViewRowInfo rd in dgvData.Rows)
                        {
                            rno += 1;
                            mh_InvoiceDTTemp mtem = db.mh_InvoiceDTTemps.Where(t => t.id == Convert.ToInt32(rd.Cells["id"].Value.ToSt())).FirstOrDefault();
                            if (mtem != null)
                            {
                                mh_InvoiceDT nd = new mh_InvoiceDT();
                               
                                nd.ItemNo = rd.Cells["ItemNo"].Value.ToSt();
                                nd.RefDocNo = Convert.ToString(rd.Cells["RefDocNo"].Value);
                                //nd.RefId = 0;
                                nd.Qty = Convert.ToDecimal(rd.Cells["Qty"].Value.ToSt());
                                nd.Amount = Convert.ToDecimal(rd.Cells["Amount"].Value.ToSt());
                                nd.ItemName = mtem.ItemName;
                                nd.LocationItem = mtem.LocationItem;
                                nd.UOM = Convert.ToString(rd.Cells["Unit"].Value);
                                nd.Description = mtem.Description;
                                nd.UnitPrice= Convert.ToDecimal(rd.Cells["UnitPrice"].Value.ToSt());
                                nd.PCSUnit = mtem.PCSUnit;
                                nd.IVNo = txtSONo.Text;
                                nd.Status = "Process";
                                nd.RefId = mtem.RefId;
                                //nd.RefDocNo = mtem.RefDocNo;
                                nd.Active = Convert.ToBoolean(true);                               

                                db.mh_InvoiceDTs.InsertOnSubmit(nd);   
                                db.mh_InvoiceDTTemps.DeleteOnSubmit(mtem);
                                db.SubmitChanges();




                                var v = (from ix in db.mh_ShipmentDTs
                                         where //ix.RefPOid == StockControl.dbClss.TInt(g.Cells["dgvid"].Value)
                                               // ix.TempNo == txtTempNo.Text 
                                                   ix.id == mtem.RefId
                                         select ix).ToList();
                                if (v.Count > 0)
                                {
                                    var p = (from ix in db.mh_ShipmentDTs
                                             where //ix.RefPOid == StockControl.dbClss.TInt(g.Cells["dgvid"].Value)
                                                   // ix.TempNo == txtTempNo.Text 
                                                ix.id == mtem.RefId
                                             select ix).First();
                                   
                                    p.OutShip = p.OutShip - Convert.ToDecimal(rd.Cells["Qty"].Value.ToSt());

                                    dbClss.AddHistory(this.Name, "ปรับสถานะ mh_ShipmentDT ", "ปรับ OutShip : " + (rd.Cells["Qty"].Value.ToSt())
                                    + " ShipmentNo :" + mtem.RefDocNo
                                    + " ปรับโดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", mtem.RefDocNo);

                                    db.SubmitChanges();
                                    
                                }

                            }

                        }


                    }
                }


                    baseClass.Info("Save complete(s).");
                ClearData();
                //txtContactName.Text = t_CustomerNo;
                txtSONo.Text = sono;
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
                //foreach (var idPO in dgvData.Rows.Select(x => x.Cells["RefId"].Value.ToInt()))
                //{
                //    if (idPO == 0) continue;
                //    var c = db.mh_CustomerPOs.Where(x => x.id == idPO).First();
                //    var m = db.mh_SaleOrders.Where(x => x.Active && x.RefId == idPO).ToList();
                //    decimal qq = 0.00m;
                //    if (m.Count > 0)
                //        qq = m.Sum(x => x.Qty * x.PCSUnit);
                //    c.OutSO = (c.Quantity * c.PCSUnit) - qq;
                //    if (c.OutSO == c.Quantity)
                //        c.Status = "Waiting";
                //    else if (c.OutSO <= 0)
                //        c.Status = "Completed";
                //    else
                //        c.Status = "Proeces";
                //    db.SubmitChanges();
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
                    if (e.Column.Name.Equals("UnitPrice") || e.Column.Name.Equals("Qty"))
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
                        CallTotal();
                    }
                    //else if (e.Column.Name.Equals("ItemNo"))
                    //{
                    //    if (txtContactName.Text == "")
                    //    {
                    //        baseClass.Warning("Please select Customer first.\n");
                    //        return;
                    //    }
                    //    using (var db = new DataClasses1DataContext())
                    //    {
                    //        var t = db.mh_Items.Where(x => x.InternalNo.Equals(itemNo)).FirstOrDefault();
                    //        if (t == null)
                    //        {
                    //            baseClass.Warning($"Item no. ({itemNo}) not found.!!");
                    //            e.Row.Cells["ItemNo"].Value = beginItem;
                    //            return;
                    //        }
                    //        var tU = db.mh_ItemUOMs.Where(x => x.ItemNo == t.InternalNo && x.UOMCode == t.BaseUOM).FirstOrDefault();
                    //        var pcsunit = (tU != null) ? tU.QuantityPer : 1;

                    //        //set Tool
                    //        if (beginItem == "")
                    //        {
                    //            var cc = db.mh_Customers.Where(x => x.No == txtContactName.Text).First();
                    //            addRow(e.RowIndex, DateTime.Now, t.InternalNo, t.InternalName, "", t.Location
                    //                , 1, t.BaseUOM, pcsunit, 0, 0, false, 1 * pcsunit, 1 * pcsunit, 0
                    //                , "Waiting", "Waiting", cc.VatGroup, t.VatType, "", 0, t.ReplenishmentType, "T");
                    //        }
                    //        else
                    //        {
                    //            e.Row.Cells["ItemName"].Value = t.InternalName;
                    //           // e.Row.Cells["Unit"].Value = t.BaseUOM;
                    //            e.Row.Cells["PCSUnit"].Value = pcsunit;
                    //            e.Row.Cells["OutShip"].Value = e.Row.Cells["Qty"].Value.ToDecimal() * e.Row.Cells["PCSUnit"].Value.ToDecimal();
                    //            e.Row.Cells["OutPlan"].Value = e.Row.Cells["Qty"].Value.ToDecimal() * e.Row.Cells["PCSUnit"].Value.ToDecimal();
                    //        }

                    //        //
                    //        SetRowNo1(dgvData);
                    //    }
                    //}
                    //else if (e.Column.Name.Equals("Unit"))
                    //{
                    //    var unit = e.Row.Cells["Unit"].Value.ToSt();
                    //    using (var db = new DataClasses1DataContext())
                    //    {
                    //        var u = db.mh_ItemUOMs.Where(x => x.ItemNo == itemNo && x.UOMCode == unit).FirstOrDefault();
                    //        var pcsunit = (u != null) ? u.QuantityPer : 1;

                    //        e.Row.Cells["PCSUnit"].Value = pcsunit;
                    //        e.Row.Cells["OutShip"].Value = e.Row.Cells["Qty"].Value.ToDecimal() * e.Row.Cells["PCSUnit"].Value.ToDecimal();
                    //        e.Row.Cells["OutPlan"].Value = e.Row.Cells["Qty"].Value.ToDecimal() * e.Row.Cells["PCSUnit"].Value.ToDecimal();
                    //    }
                    //}

                    e.Row.Cells["dgvC"].Value = "T";
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void MasterTemplate_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            if (e.RowIndex >= -1)
            {
                string itemNo = e.Row.Cells["ItemNo"].Value.ToSt();
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
                else if (e.Column.Name.Equals("ItemNo"))
                {
                    beginItem = itemNo;
                }
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
               if(lblStatus.Text.Equals("New"))
                {
                    if (row >= 0)
                    {
                        int RNo = 0;
                        int.TryParse(dgvData.Rows[row].Cells["id"].Value.ToString(), out RNo);
                        using (DataClasses1DataContext db = new DataClasses1DataContext())
                        {
                            if(RNo>0)
                            {
                                mh_InvoiceDTTemp md = db.mh_InvoiceDTTemps.Where(s => s.id == RNo).FirstOrDefault();
                                if(md!=null)
                                {
                                    db.mh_InvoiceDTTemps.DeleteOnSubmit(md);
                                    db.SubmitChanges();
                                }
                                LoadShipment();
                            }
                        }
                        
                    }

                }else
                {
                    //DataLoad();
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
                        

                    }

                    txtSONo.Text = SONo;
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
            return;

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
                if (txtSONo.Text.Trim() == "")
                {
                    baseClass.Warning("Please enter Sale Order no.");
                    return;
                }
                if (txtContactName.Text.Trim() == "")
                {
                    baseClass.Warning("Please enter Customer no.");
                    return;
                }

                t_SONo = txtSONo.Text;
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
                //InvoiceListPart sh = new InvoiceListPart(txtSONo.Text);
                Invoice_Shipment_List sh = new Invoice_Shipment_List(txtSONo.Text);
                sh.ShowDialog();
                LoadShipment();
            }
            catch { }
        }
        private void LoadShipment()
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var ShipList = db.mh_InvoiceDTTemps.Where(s => s.IVNo == txtSONo.Text).ToList();
                    if (ShipList.Count > 0)
                    {
                        dgvData.DataSource = ShipList;

                        int No1 = 0;
                        foreach(GridViewRowInfo rd in dgvData.Rows)
                        {
                            No1 += 1;
                            rd.Cells["RNo"].Value = No1;                            
                        }
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
            AddPartE();
        }

        private void radButton3_Click(object sender, EventArgs e)
        {

        }
    }


}
