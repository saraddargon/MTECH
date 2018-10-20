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

                LoadDefault();

                var a = new List<int>();
                foreach (var item in idList)
                    a.Add(item);


                btnNew_Click(null, null);

                foreach (var item in a)
                    idList.Add(item);

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
                var cus = db.mh_Customers.Where(x => x.Active).Select(x => new { x.No, x.Name }).ToList();
                cbbCSTM.MultiColumnComboBoxElement.AutoSizeDropDownToBestFit = true;
                cbbCSTM.DisplayMember = "Name";
                cbbCSTM.ValueMember = "No";
                cbbCSTM.MultiColumnComboBoxElement.DataSource = cus;
                cbbCSTM.SelectedIndex = -1;

                //var lo = db.mh_Locations.Where(x => x.Active).ToList();
                //var com = dgvData.Columns["LocationItem"] as GridViewComboBoxColumn;
                //com.DisplayMember = "Name";
                //com.ValueMember = "Code";
                //com.DataSource = lo;

                //var vt = db.mh_VATTypes.Where(x => x.Active.Value).ToList();
                //// VatType
                //var com3 = dgvData.Columns["VatType"] as GridViewComboBoxColumn;
                //com3.DisplayMember = "VatType";
                //com3.ValueMember = "VatType";
                //com3.DataSource = vt;

                //var uom = db.mh_Units.Where(x => x.UnitActive.Value).ToList();
                ////UnitCode UnitDetail
                //var com4 = dgvData.Columns["UOM"] as GridViewComboBoxColumn;
                //com4.DisplayMember = "UnitDetail";
                //com4.ValueMember = "UnitCode";
                //com4.DataSource = uom;
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
                    var t = db.mh_SaleOrders.Where(x => x.Active && x.SONo == t_SONo && x.DemandType == 0).FirstOrDefault();
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
                        int ss = 0;
                        var comp = 0;
                        var pp = 0;
                        var dt = db.mh_SaleOrderDTs.Where(x => x.Active && x.SONo == t_SONo).ToList();
                        if (dt.Count > 0)
                        {
                            dgvData.DataSource = dt;
                            if (dt.Where(x => x.RefId > 0).Count() > 0)
                            {
                                foreach (var item in dgvData.Rows)
                                    item.Cells["CSTMNo"].Value = t.CustomerNo;
                            }
                            dgvData.Rows.ToList().ForEach(x =>
                            {
                                x.Cells["QtyB"].Value = x.Cells["Qty"].Value;
                                x.Cells["OutShipB"].Value = x.Cells["OutShip"].Value;
                                x.Cells["OutQtyB"].Value = x.Cells["OutQty"].Value;
                                var id = x.Cells["id"].Value.ToInt();
                                var d = dt.Where(q => q.id == id).FirstOrDefault();
                                if (d != null)
                                {
                                    x.Cells["Status"].Value = baseClass.setSaleOrderStatus(d);
                                }
                            });

                            lblStatus.Text = (t.SeqStatus == 0) ? "Waiting" : (t.SeqStatus == 1) ? "Waiting Approved" : "Approved";

                            foreach (var x in dgvData.Rows)
                            {
                                string St = x.Cells["Status"].Value.ToSt();
                                if (St == "Waiting")
                                    ss = 0;
                                else if (St == "Waiting Approved")
                                    ss = 1;
                                else if (St == "Completed")
                                    comp += 1;
                                else
                                    pp += 1;
                            }
                        }
                        SetRowNo1(dgvData);
                        CallTotal();

                        btnView_Click(null, null);
                        if (comp >= dgvData.Rows.Count)
                        {
                            lblStatus.Text = "Completed";
                            btnDelete.Enabled = false;
                            radButtonElement3.Enabled = false;
                            btnAddPart.Enabled = false;
                            btnAdd_Row.Enabled = false;
                            btnDel_Item.Enabled = false;
                            btnGetPrice.Enabled = false;
                            radButton2.Enabled = false;
                        }
                        else if (pp > 0)
                        {
                            lblStatus.Text = "Process";
                            btnDelete.Enabled = false;
                            radButtonElement3.Enabled = false;
                            btnAddPart.Enabled = false;
                            btnAdd_Row.Enabled = false;
                            btnDel_Item.Enabled = false;
                            btnGetPrice.Enabled = false;
                            radButton2.Enabled = false;
                        }
                        else if (ss == 0)
                        {
                            lblStatus.Text = "Waiting";
                            btnDelete.Enabled = false;
                            radButtonElement3.Enabled = true;
                            btnAddPart.Enabled = true;
                            btnAdd_Row.Enabled = true;
                            btnDel_Item.Enabled = true;
                            btnGetPrice.Enabled = true;
                            radButton2.Enabled = true;
                        }
                        else if (ss == 0)
                        {
                            lblStatus.Text = "Waiting Approved";
                            radButtonElement3.Enabled = false;
                            btnAddPart.Enabled = true;
                            btnAdd_Row.Enabled = true;
                            btnDel_Item.Enabled = true;
                            btnGetPrice.Enabled = true;
                            radButton2.Enabled = true;
                        }
                        txtSOStatus.Text = lblStatus.Text;

                        if (t.SeqStatus <= 0 || t.SeqStatus == null)
                            radButtonElement3.Enabled = true;
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
                        var c = db.mh_CustomerPODTs.Where(x => x.id == id && x.OutSO > 0).ToList();
                        if (c.Count > 0)
                        {
                            var dd = db.mh_CustomerPOs.Where(x => x.id == dbClss.TInt(c.FirstOrDefault().idCustomerPO)).ToList();
                            if (dd.Count > 0)
                            {
                                if (!fRow)
                                {
                                    if (txtCSTMNo.Text != dd.FirstOrDefault().CustomerNo) continue;
                                }

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
                                    decimal Qty = c.FirstOrDefault().OutSO.ToDecimal();
                                    //Qty = Math.Round(dbClss.TDe(c.FirstOrDefault().OutSO) / dbClss.TDe(c.FirstOrDefault().PCSUnit), 2);

                                    addRow(rowe.Index, Convert.ToDateTime(c.FirstOrDefault().ReqDate)
                                        , dbClss.TSt(c.FirstOrDefault().ItemNo)
                                        , dbClss.TSt(c.FirstOrDefault().ItemName)
                                        , dbClss.TSt(t.FirstOrDefault().InternalDescription)
                                        , "Warehouse"
                                        , Qty //Math.Round(c.OutSO / c.PCSUnit, 2)
                                        , dbClss.TSt(c.FirstOrDefault().UOM)
                                        , dbClss.TDe(c.FirstOrDefault().PCSUnit)
                                        , dbClss.TDe(c.FirstOrDefault().UnitPrice)
                                        , dbClss.TDe(c.FirstOrDefault().Amount)
                                        , false
                                        , Qty //OutSO
                                        , dbClss.TDe(c.FirstOrDefault().OutPlan)
                                        , c.FirstOrDefault().OutQty.ToDecimal()
                                        , 0
                                        , "Waiting", "Waiting", cstm.VatGroup
                                        , dbClss.TSt(t.FirstOrDefault().VatType)
                                        , dbClss.TSt(dd.FirstOrDefault().CustomerPONo)
                                        , dbClss.TInt(c.FirstOrDefault().id)
                                        , dbClss.TSt(t.FirstOrDefault().ReplenishmentType)
                                        , "T"
                                        , c.FirstOrDefault().genPR
                                        , c.FirstOrDefault().forSafetyStock
                                        , dd.FirstOrDefault().CustomerNo
                                        , t.FirstOrDefault().CustomerPartNo
                                        , t.FirstOrDefault().CustomerPartName);

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
            HistoryView hw = new HistoryView(this.Name, txtSONo.Text);
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
            txtSONo.Text = dbClss.GetNo(28, 0);
            dtSODate.Value = Convert.ToDateTime(DateTime.Today, new CultureInfo("en-US"));
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
            cbbCSTM.Enabled = true;
            txtSOStatus.Text = "Waiting";
            cQty = 0.00m;
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
            txtSONo.Text = StockControl.dbClss.GetNo(28, 0);

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
            btnGetPrice.Enabled = false;

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
            btnGetPrice.Enabled = true;

            cbbCSTM.Enabled = true;

            dtSODate.ReadOnly = false;
            txtRemark.ReadOnly = false;

            //dgvData.ReadOnly = false;


            Enable_Status(true, "Edit");
            lblStatus.Text = "Edit";
            Ac = "Edit";
            if (dgvData.Rows.Count > 0 && dgvData.Rows.Where(x => x.Cells["CSTMNo"].Value.ToSt() != "").ToList().Count > 0)
                cbbCSTM.Enabled = false;

            if (txtSOStatus.Text != "Waiting"
                && txtSOStatus.Text != "Waiting Approved")
            {
                btnDelete.Enabled = false;
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string poNo = txtSONo.Text.Trim();
                string cstmNo = txtCSTMNo.Text.Trim();
                if (poNo != "" && cstmNo != "")
                {
                    using (var db = new DataClasses1DataContext())
                    {
                        int Temp = 0;
                        var ck = db.mh_SaleOrderDTs.Where(x => x.SONo == poNo && x.Active).ToList();
                        if (ck.Where(x => x.Active == true && x.OutShip != x.Qty).Count() > 0)
                        {
                            foreach (var pp in ck)
                            {
                                Temp = 1;
                                break;
                            }
                        }

                        if (Temp == 1)
                        {
                            baseClass.Warning("ไม่สามารถลบเอกสารได้เนื่องจากสถานะไม่ถูกต้อง.");
                            return;
                        }


                        if (baseClass.IsDel($"Do you want to Delete Sale Order: {poNo} ?"))
                        {
                            //Status
                            //Waiting
                            //Process  --Approved
                            //Partial-- > Shipment Partial
                            //Complete --> Shipment Full


                            var p = db.mh_SaleOrders.Where(x => x.SONo == poNo && x.Active).ToList();
                            if (p.Where(x => x.Active == true && (x.Status.ToSt() == "Waiting"
                            || x.Status.ToSt() == "Process")).Count() > 0)
                            {
                                foreach (var pp in p)
                                {
                                    pp.Active = false;
                                    pp.Status = "Cancel";
                                    pp.UpdateBy = Classlib.User;
                                    pp.UpdateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                }

                                var d = db.mh_SaleOrderDTs.Where(x => x.SONo == poNo && x.Active).ToList();
                                if (d.Where(x => x.Active == true && x.OutShip == x.Qty).Count() > 0)
                                {
                                    foreach (var pp in d)
                                        pp.Active = false;
                                }
                                //Cancel ListApprove
                                dbClss.Delete_ApproveList(poNo);

                                db.SubmitChanges();

                                updateOutSO_Customer();

                                baseClass.Info("Delete Sale Order complete.");
                                //ClearData();
                                btnNew_Click(null, null);
                            }
                            else
                                baseClass.Warning("Sale Order Status cannot Delete.");
                        }
                    }
                }
                else
                {
                    btnNew_Click(null, null);
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
                //if (txtSOStatus.Text != "Waiting")
                //    err += " “Status:” ไม่ใช่ Waiting \n";
                if (cbbCSTM.Text.ToSt() == "" || txtCSTMNo.Text == "")
                    err += " “Customer:” เป็นค่าว่าง \n";
                if (dgvData.Rows.Where(x => x.IsVisible).Count() < 1)
                    err += " “Items:” เป็นค่าว่าง \n";
                else
                {
                    if (dgvData.Rows.Where(x => x.Cells["RefId"].Value.ToInt() > 0 && x.IsVisible).Count() > 0)
                    {
                        var idList = new List<int>();
                        using (var db = new DataClasses1DataContext())
                        {
                            foreach (var item in dgvData.Rows.Where(x => x.IsVisible))
                            {
                                if (item.Cells["Qty"].Value.ToDecimal() <= 0)
                                {
                                    err += " “Q'ty:” เป็น 0. \n";
                                    break;
                                }
                                int RefId = item.Cells["RefId"].Value.ToInt();
                                if (idList.Contains(RefId)) continue;
                                var podt = db.mh_CustomerPODTs.Where(x => x.id == RefId).FirstOrDefault();
                                if (podt == null) continue;
                                var dtQty = podt.OutSO;
                                var rowList = dgvData.Rows.Where(x => x.Cells["RefId"].Value.ToInt() == RefId && x.IsVisible).ToList();
                                var OrderQty = 0.00m;
                                if (rowList.Count > 0)
                                {
                                    //คืนก่อนกรณีที่ id > 0
                                    foreach (var r in rowList.Where(x => x.Cells["id"].Value.ToInt() > 0))
                                    {
                                        var sodt = db.mh_SaleOrderDTs.Where(x => x.id == r.Cells["id"].Value.ToInt()).FirstOrDefault();
                                        if (sodt != null) dtQty += sodt.Qty;
                                    }

                                    //จำนวนรวมในใบ
                                    OrderQty = Math.Round(rowList.Sum(x => x.Cells["Qty"].Value.ToDecimal()), 2);
                                }
                                if (OrderQty > dtQty)
                                {
                                    err += $"Item No. : {podt.ItemNo} มีจำนวนมากกว่าจำนวนคงเหลือใน Customer P/O";
                                    break;
                                }

                                //
                                idList.Add(RefId);
                            }
                        }
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
                dgvData.EndInit();
                if (Check_Save())
                    return;
                else if (baseClass.IsSave())
                    SaveE();
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
                        ////ถ้ามีการใส่เลขที่ PR เช็คดูว่ามีการใส่เลขนี้แล้วหรือไม่ ถ้ามีให้ใส่เลขอื่น
                        //if (!txtSONo.Text.Equals(""))
                        //{

                        //    var p = (from ix in db.mh_SaleOrders
                        //             where ix.SONo.ToUpper().Trim() == txtSONo.Text.Trim()
                        //             && ix.Active != false
                        //             //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                        //             select ix).ToList();
                        //    if (p.Count > 0)  //มีรายการในระบบ
                        //    {
                        //        MessageBox.Show("เลขที่เอกสารถูกใช้ไปแล้ว กรุณาใส่เลขใหม่");
                        //        return;
                        //    }
                        //}
                        //else
                        txtSONo.Text = StockControl.dbClss.GetNo(28, 2);
                        sono = txtSONo.Text;

                    }


                    if (txtSONo.Text != "")
                    {
                        SaveHerder(sono);
                        SaveDetail();


                        t_SONo = sono;
                        t_CustomerNo = cstmNo;


                        updateOutSO_Customer();
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

                var gg = db.mh_SaleOrders.Where(x => x.SONo == SONo).FirstOrDefault();
                if (gg == null)
                {
                    gg = new mh_SaleOrder();
                    gg.CreateBy = ClassLib.Classlib.User;
                    gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    gg.Status = "Waiting";
                    gg.SendApproveBy = "";
                    gg.ApproveBy = "";
                    gg.SeqStatus = 0;
                    db.mh_SaleOrders.InsertOnSubmit(gg);
                    dbClss.AddHistory(this.Name, "เพิ่ม Sale order", "สร้าง Sale order [" + SONo + "]", txtSONo.Text);
                }
                else
                {

                    dbClss.AddHistory(this.Name, "แก้ไข Sale order", "แก้ไข Sale order [" + SONo + "]", txtSONo.Text);
                }
                //gg.LocationRunning = ddlFactory.Text;
                gg.UpdateBy = ClassLib.Classlib.User;
                gg.UpdateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                gg.SONo = SONo;
                gg.CustomerNo = txtCSTMNo.Text;
                gg.CustomerName = cbbCSTM.Text;
                gg.CustomerAddress = txtAddress.Text;
                gg.SODate = Convert.ToDateTime(dtSODate.Value, new CultureInfo("en-US"));
                gg.Remark = txtRemark.Text;
                //gg.VatGroup
                gg.TotalPrice = dbClss.TDe(txtTotal.Text);
                gg.Vat = cbVat.Checked;
                gg.VatA = dbClss.TDe(txtVatA.Text);
                gg.VatAmnt = dbClss.TDe(txtVatAmnt.Text);
                gg.TotalPriceIncVat = dbClss.TDe(txtGrandTotal.Text);
                gg.Active = true;
                gg.DemandType = 0;


                db.SubmitChanges();
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
                    int id = ix.Cells["id"].Value.ToInt();
                    var gg = db.mh_SaleOrderDTs.Where(x => x.id == id).FirstOrDefault();
                    if (gg == null)
                    {
                        gg = new mh_SaleOrderDT();
                        db.mh_SaleOrderDTs.InsertOnSubmit(gg);
                        dbClss.AddHistory(this.Name, "เพิ่ม Sale order", "สร้าง Sale order [" + ItemNo + "]", txtSONo.Text);
                    }
                    else
                    {//Edit
                        dbClss.AddHistory(this.Name, "แก้ไข Sale order", "แก้ไข Sale order [" + ItemNo + "]", txtSONo.Text);
                        if (gg.Qty != ix.Cells["Qty"].Value.ToDecimal())
                            dbClss.AddHistory(this.Name, "แก้ไข Sale Order", $"แก้ไข Q'ty [{gg.Qty}] เป็น [{ix.Cells["Qty"].Value.ToDecimal()}]", txtSONo.Text);
                    }

                    if (ix.IsVisible)
                    {
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
                        gg.UOM = dbClss.TSt(ix.Cells["UOM"].Value);
                        gg.VatType = dbClss.TSt(ix.Cells["VatType"].Value);
                        gg.Amount = dbClss.TDe(ix.Cells["Amount"].Value);
                        gg.Description = dbClss.TSt(ix.Cells["Description"].Value);
                        gg.Active = true;
                        gg.genPR = false;
                        gg.forSafetyStock = false;
                        gg.OutQty = ix.Cells["OutQty"].Value.ToDecimal();
                        gg.ReqDate = ix.Cells["ReqDate"].Value.ToDateTime().Value.Date;
                        gg.CustomerPartNo = ix.Cells["CustomerPartNo"].Value.ToSt();
                        gg.CustomerPartName = ix.Cells["CustomerPartName"].Value.ToSt();
                    }
                    else if(!ix.IsVisible && id > 0)
                    {
                        gg.Active = false;
                        dbClss.AddHistory(this.Name, "แก้ไข Sale Order", $"ลบรายการ [{gg.ItemNo}]", txtSONo.Text);
                    }

                    db.SubmitChanges();

                }
            }
        }
        private void updateOutSO_Customer()
        {

            using (var db = new DataClasses1DataContext())
            {
                //Update Customer P/O (Out Sale Order Q'ty)
                List<int> calId = new List<int>();
                foreach (var idPO in dgvData.Rows.Select(x => x.Cells["RefId"].Value.ToInt()))
                {
                    if (calId.Contains(idPO)) continue;
                    calId.Add(idPO);

                    if (idPO == 0) continue;
                    var c = db.mh_CustomerPODTs.Where(x => x.id == idPO).First();
                    var m = db.mh_SaleOrderDTs.Where(x => x.Active && x.RefId == idPO).ToList();
                    decimal qq = 0.00m;
                    if (m.Count > 0)
                        qq = Math.Round(m.Sum(x => x.Qty), 2);
                    c.OutSO = Math.Round(c.Qty, 2) - qq;
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
        decimal cQty = 0.00m;
        private void radGridView1_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            if (e.RowIndex >= -1)
            {
                //if (txtSOStatus.Text != "Waiting")
                //{
                //    e.Cancel = true;
                //    return;
                //}

                string itemNo = e.Row.Cells["ItemNo"].Value.ToSt();
                if (e.Column.Name.Equals("Qty"))
                {
                    cQty = e.Row.Cells["Qty"].Value.ToDecimal();
                }
                else
                {
                    if (txtSOStatus.Text != "Waiting")
                    {
                        e.Cancel = true;
                        return;
                    }

                    if (e.Column.Name.Equals("UOM"))
                    {
                        using (var db = new DataClasses1DataContext())
                        {
                            if (e.Row.Cells["RefDocNo"].Value.ToSt() == "")
                            {
                                var unit = db.mh_ItemUOMs.Where(x => x.ItemNo == itemNo).ToList();
                                unit = unit.Where(x => x.Active.ToBool()).ToList();
                                var c1 = dgvData.Columns["UOM"] as GridViewComboBoxColumn;
                                //c1.ValueMember = "UOMCode";
                                //c1.DisplayMember = "UOMCode";
                                //c1.DataSource = unit;
                                c1.DataSource = unit.Select(x => x.UOMCode).ToList();
                            }
                            else //มาจาก Customer P/O ไม่สามารถเปลี่ยนหน่วยได้
                                e.Cancel = true;
                        }
                    }
                    else if (e.Column.Name.Equals("ItemNo"))
                    {
                        beginItem = itemNo;
                    }
                }


            }
        }
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
                        if (e.Column.Name.Equals("Qty"))
                        {
                            var qtyB = e.Row.Cells["QtyB"].Value.ToDecimal();
                            var outShipB = e.Row.Cells["OutShipB"].Value.ToDecimal();
                            var newQ = e.Row.Cells["Qty"].Value.ToDecimal();
                            var pcsunit = e.Row.Cells["PCSUnit"].Value.ToDecimal();
                            if (outShipB == qtyB) //ยังไม่ได้ทำ shipment
                            {
                                e.Row.Cells["OutShip"].Value = newQ;
                                if (e.Row.Cells["OutPlan"].Value.ToDecimal() > 0)
                                {
                                    e.Row.Cells["OutPlan"].Value = newQ * pcsunit;
                                    e.Row.Cells["OutQty"].Value = newQ * pcsunit;
                                }
                            }
                            else
                            {
                                var smQB = qtyB - outShipB;
                                if (newQ < smQB)
                                {
                                    baseClass.Warning("ไม่สามารถแก้ไขจำนวน Q'ty น้อยกว่าจำนวนที่เปิด Shipment ไปแล้ว.!");
                                    e.Row.Cells["Qty"].Value = cQty;
                                }
                                else if (newQ > qtyB)
                                {
                                    baseClass.Warning("ไม่สามารถแก้ไขจำนวน Q'ty เพิ่มได้ เนื่องจากมีการทำ Shipment แล้ว.!");
                                    e.Row.Cells["Qty"].Value = cQty;
                                }
                                else
                                {
                                    e.Row.Cells["OutShip"].Value = newQ - smQB;
                                    if (e.Row.Cells["OutPlan"].Value.ToDecimal() > 0) //แสดงว่ายังไม่ทำใบ Job
                                    {
                                        e.Row.Cells["OutPlan"].Value = newQ * pcsunit;
                                        e.Row.Cells["OutQty"].Value = newQ * pcsunit;
                                    }
                                }
                            }

                        }


                        if (e.Row.Cells["Qty"].Value.ToDecimal() > 0)
                        {
                            var m = Math.Round(e.Row.Cells["UnitPrice"].Value.ToDecimal() * e.Row.Cells["Qty"].Value.ToDecimal(), 2);
                            dgvData.Rows[e.RowIndex].Cells["Amount"].Value = m;
                        }
                        else
                            dgvData.Rows[e.RowIndex].Cells["Amount"].Value = 0;

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
                                addRow(e.RowIndex, DateTime.Now, t.InternalNo, t.InternalName, "", "Warehouse"
                                    , 1, t.BaseUOM, pcsunit, 0, 0, false, 1, 1 * pcsunit, 1 * pcsunit, 0
                                    , "Waiting", "Waiting", cc.VatGroup, t.VatType, "", 0, t.ReplenishmentType, "T", false, false, ""
                                    , t.CustomerPartNo, t.CustomerPartName);
                            }
                            else
                            {
                                e.Row.Cells["ItemName"].Value = t.InternalName;
                                e.Row.Cells["UOM"].Value = t.BaseUOM;
                                e.Row.Cells["PCSUnit"].Value = pcsunit;
                                e.Row.Cells["OutShip"].Value = e.Row.Cells["Qty"].Value.ToDecimal();// * e.Row.Cells["PCSUnit"].Value.ToDecimal();
                                e.Row.Cells["OutPlan"].Value = e.Row.Cells["Qty"].Value.ToDecimal() * e.Row.Cells["PCSUnit"].Value.ToDecimal();
                                e.Row.Cells["OutQty"].Value = e.Row.Cells["Qty"].Value.ToDecimal() * e.Row.Cells["PCSUnit"].Value.ToDecimal();
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
                            e.Row.Cells["OutShip"].Value = e.Row.Cells["Qty"].Value.ToDecimal();// * e.Row.Cells["PCSUnit"].Value.ToDecimal();
                            e.Row.Cells["OutPlan"].Value = e.Row.Cells["Qty"].Value.ToDecimal() * e.Row.Cells["PCSUnit"].Value.ToDecimal();
                            e.Row.Cells["OutQty"].Value = e.Row.Cells["Qty"].Value.ToDecimal() * e.Row.Cells["PCSUnit"].Value.ToDecimal();
                        }
                    }
                    else if (e.Column.Name.Equals("ReqDate"))
                    {
                        //get from price List
                        DateTime? ReqDate = e.Row.Cells["ReqDate"].Value.ToDateTime();
                        if (ReqDate == null) return;
                        if (e.Row.Cells["RefDocNo"].Value.ToSt() == "")
                        {
                            var m = baseClass.GetPriceList(itemNo, ReqDate.Value.Date);
                            e.Row.Cells["UnitPrice"].Value = m;
                            CallTotal();
                        }
                    }

                    e.Row.Cells["dgvC"].Value = "T";
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        void addRow(int rowIndex, DateTime ReqDate, string ItemNo, string ItemName, string Desc
            , string Location, decimal Qty, string UOM, decimal PCSUnit, decimal UnitPrice, decimal Amount
            , bool PriceIncVat, decimal OutShip, decimal OutPlan, decimal OutQty, int id
            , string Status, string PlanStatus, int VatGroup, string VatType
            , string RefDocNo, int RefId, string RepType, string dgvC, bool genPR, bool forSafetyStock
            , string CSTMNo, string CustomerPartNo, string CustomerPartName)
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
                rowE.Cells["UOM"].Value = UOM;
                rowE.Cells["PCSUnit"].Value = PCSUnit;
                rowE.Cells["UnitPrice"].Value = UnitPrice;
                rowE.Cells["Amount"].Value = Amount;
                rowE.Cells["PriceIncVat"].Value = PriceIncVat;
                rowE.Cells["VatType"].Value = VatType;
                rowE.Cells["OutShip"].Value = OutShip;
                rowE.Cells["OutQty"].Value = OutQty;
                rowE.Cells["Status"].Value = Status;
                rowE.Cells["RefDocNo"].Value = RefDocNo;
                rowE.Cells["RefId"].Value = RefId;
                rowE.Cells["ReplenishmentType"].Value = RepType;
                rowE.Cells["dgvC"].Value = dgvC; //if Edit row -> value = T
                rowE.Cells["OutPlan"].Value = OutPlan;
                rowE.Cells["genPR"].Value = genPR;
                rowE.Cells["forSafetyStock"].Value = forSafetyStock;
                rowE.Cells["CSTMNo"].Value = CSTMNo;
                rowE.Cells["CustomerPartNo"].Value = CustomerPartNo;
                rowE.Cells["CustomerPartName"].Value = CustomerPartName;

                SetRowNo1(dgvData);

                //rowE.Cells["ReqDate"].ReadOnly = true;
                //rowE.Cells["ItemNo"].ReadOnly = true;
                //rowE.Cells["ItemName"].ReadOnly = true;
                //rowE.Cells["Description"].ReadOnly = true;
                //rowE.Cells["LocationItem"].ReadOnly = true;
                //rowE.Cells["Qty"].ReadOnly = true;
                //rowE.Cells["UOM"].ReadOnly = true;
                //rowE.Cells["PCSUnit"].ReadOnly = true;
                //rowE.Cells["UnitPrice"].ReadOnly = true;
                //rowE.Cells["Amount"].ReadOnly = true;
                //rowE.Cells["PriceIncVat"].ReadOnly = true;
                //rowE.Cells["VatType"].ReadOnly = true;
                //rowE.Cells["OutShip"].ReadOnly = true;
                //rowE.Cells["Status"].ReadOnly = true;
                //rowE.Cells["RefDocNo"].ReadOnly = true;
                //rowE.Cells["RefId"].ReadOnly = true;
                //rowE.Cells["ReplenishmentType"].ReadOnly = true;
                //rowE.Cells["dgvC"].ReadOnly = true;
                //rowE.Cells["OutPlan"].ReadOnly = true;
                //rowE.Cells["OutQty"].ReadOnly = true;

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
                //if (txtSOStatus.Text != "Waiting")
                //{
                //    baseClass.Warning("สถานะไม่สามารถลบรายการได้.\n");
                //    return;
                //}
                if(dgvData.CurrentCell == null)
                {
                    baseClass.Warning("กรุณาเลือกรายการ.\n");
                    return;
                }
                var rowe = dgvData.CurrentCell.RowInfo;
                var qtyB = rowe.Cells["QtyB"].Value.ToDecimal();
                var shipB = rowe.Cells["OutShipB"].Value.ToDecimal();
                var planB = rowe.Cells["OutPlan"].Value.ToDecimal();
                if(planB == 0)
                {
                    baseClass.Warning("ไม่สามารถลบรายการได้เนื่องจาก รายการดังกล่าวทำ Planning Job Order Sheet แล้ว.\n");
                    return;
                }
                else if(shipB != qtyB)
                {
                    baseClass.Warning("ไม่สามารถลบรายการได้เนื่องจาก รายการดังกล่าวทำ Shipment แล้ว.\n");
                }


                if (Ac.Equals("New") || Ac.Equals("Edit"))
                {
                    this.Cursor = Cursors.WaitCursor;

                    //using (var db = new DataClasses1DataContext())
                    //{
                    //    var so = db.mh_SaleOrders.Where(x => x.SONo == txtSONo.Text.Trim()).FirstOrDefault();
                    //    if (so != null && so.SeqStatus > 0)
                    //    {
                    //        MessageBox.Show("ไม่สามารถลบรายการที่สถานะไม่ใช่ Waiting ได้");
                    //        return;
                    //    }
                    //}

                    //if (dgvData.CurrentCell.RowInfo.Cells["Status"].Value.ToSt() == "Waiting")
                    //{

                    int id = 0;
                    int.TryParse(StockControl.dbClss.TSt(dgvData.CurrentCell.RowInfo.Cells["id"].Value), out id);
                    if (id <= 0)
                        dgvData.Rows.Remove(dgvData.CurrentCell.RowInfo);
                    else
                        rowe.IsVisible = false;

                    if (dgvData.Rows.Count == 0)
                        cbbCSTM.Enabled = true;
                    //}
                    //else
                    //    MessageBox.Show("ไม่สามารถลบรายการที่สถานะไม่ใช่ Waiting ได้");
                }
                else
                {
                    baseClass.Warning("สถานะไม่ใช่ 'New' หรือ 'Edit' ไม่สามารถลบได้.\n");
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
                var selP = new SaleOrder_List2(2);
                selP.ShowDialog();
                if (selP.PONo != "")
                {
                    this.Cursor = Cursors.WaitCursor;
                    txtSONo.Text = selP.PONo.ToSt();
                    t_SONo = selP.PONo.ToSt();
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
                string AdNo1 = "";
                string AdNo2 = "";


                AdNo1 = txtSONo.Text;

                AdNo2 = txtSONo.Text;

                Report.Reportx1.Value = new string[2];
                Report.Reportx1.Value[0] = AdNo1;
                Report.Reportx1.Value[1] = AdNo1;
                Report.Reportx1.WReport = "SaleOrder";
                Report.Reportx1 op = new Report.Reportx1("CustomerPO.rpt");
                op.Show();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnAdd_Row_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbbCSTM.SelectedValue.ToSt() == "")
                {
                    baseClass.Warning("กรุณาเลือก Customer No.");
                    return;
                }
                if (dgvData.Rows.Where(x => x.Cells["CSTMNo"].Value.ToSt() != "").Count() > 0)
                {
                    baseClass.Warning("ไม่สามารถเลือก Item ได้เนื่องจากเป็นเอกสารที่ถูกสร้างมาจาก Customer P/O.");
                    return;
                }
                if (txtSOStatus.Text != "Waiting")
                {
                    baseClass.Warning("สถานะไม่สามารถเพิ่มรายการได้.\n");
                    return;
                }
                //dgvData.Rows.AddNew();
                AddItems();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        void AddItems()
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
                        var p = baseClass.GetPriceList(itemNo, DateTime.Now);

                        var tU = db.mh_ItemUOMs.Where(x => x.ItemNo == itemNo && x.UOMCode == t.SalesUOM).FirstOrDefault();
                        decimal u = (tU != null) ? tU.QuantityPer : 1;
                        var cstm = db.mh_Customers.Where(x => x.No == txtCSTMNo.Text).First();

                        var rowE = dgvData.Rows.AddNew();
                        //addRow(rowE.Index, DateTime.Now, itemNo, t.InternalName
                        //    , 1, t.BaseUOM, u, 0, 0, 1 * u, 1 * u
                        //    , "", 0, "Waiting", "Waiting", t.ReplenishmentType);
                        var outso = 1;
                        var outplan = 1 * u;
                        var outqty = 1 * u;
                        addRow(rowE.Index, DateTime.Now, itemNo, t.InternalName, "", "Warehouse"
                            , 1, t.SalesUOM, u, p, p, false, outso, outplan, outqty
                            , 0, "Waiting", "Waiting", cstm.VatGroup, t.VatType, "", 0, t.ReplenishmentType
                            , "T", false, false, "", t.CustomerPartNo, t.CustomerPartName);
                    }
                    SetRowNo1(dgvData);
                    CallTotal();

                }
            }
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
                txtTotal.Value = amnt;
                var vat = 0.00m;
                var vatA = txtVatA.Value.ToDecimal();
                if (cbVat.Checked)
                    vat = amnt * Math.Round(vatA / 100, 2);
                txtVatAmnt.Value = vat;
                txtGrandTotal.Value = amnt + vat;
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
            if (dgvData.Rows.Where(x => x.Cells["RefDocNo"].Value.ToSt() == "").Count() > 0)
            {
                baseClass.Warning("ไม่สามารถเลือกรายการจาก Customer P/O ได้เนื่องจาก มี Item ในรายการที่ไม่ได้ถูกเปิดมาจาก Customer P/O");
                return;
            }

            if (txtCSTMNo.Text == "")
            {
                baseClass.Warning("กรุณาเลือก Customer No.\n");
                return;
            }

            if (txtSOStatus.Text != "Waiting")
            {
                baseClass.Warning("สถานะไม่สามารถเพิ่มรายการได้.\n");
                return;
            }

            AddPartE();
            CallTotal();
        }
        void AddPartE()
        {
            try
            {
                List<GridViewRowInfo> dgvRow_List = new List<GridViewRowInfo>();
                var selP = new SaleOrder_ADD(dgvRow_List, txtCSTMNo.Text, cbbCSTM.Text);
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
                            var c = db.mh_CustomerPODTs.Where(x => x.id == id
                            && x.OutSO > 0 && !x.forSafetyStock).ToList();
                            if (c.Count > 0)
                            {
                                var dd = db.mh_CustomerPOs.Where(x => x.id == dbClss.TInt(c.FirstOrDefault().idCustomerPO)).ToList();
                                if (dd.Count > 0)
                                {
                                    if (cbbCSTM.SelectedValue.ToSt() != dd.FirstOrDefault().CustomerNo) continue;
                                    if (dgvData.Rows.Where(x => x.Cells["CSTMNo"].Value.ToSt() != dd[0].CustomerNo).Count() > 0) continue;
                                    //if (dgvData.Rows.Where(x => x.Cells["RefId"].Value.ToInt() == c.FirstOrDefault().id).Count() > 0) continue;

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
                                        decimal Qty = c.FirstOrDefault().OutSO.ToDecimal();
                                        //Qty = Math.Round(dbClss.TDe(c.FirstOrDefault().OutSO) / dbClss.TDe(c.FirstOrDefault().PCSUnit), 2);

                                        addRow(rowe.Index, Convert.ToDateTime(c.FirstOrDefault().ReqDate)
                                            , dbClss.TSt(c.FirstOrDefault().ItemNo)
                                            , dbClss.TSt(c.FirstOrDefault().ItemName)
                                            , dbClss.TSt(t.FirstOrDefault().InternalDescription)
                                            , "Warehouse"
                                            , Qty //Math.Round(c.OutSO / c.PCSUnit, 2)
                                            , dbClss.TSt(c.FirstOrDefault().UOM)
                                            , dbClss.TDe(c.FirstOrDefault().PCSUnit)
                                            , dbClss.TDe(c.FirstOrDefault().UnitPrice)
                                            , dbClss.TDe(c.FirstOrDefault().Amount)
                                            , false
                                            , Qty//dbClss.TDe(c.FirstOrDefault().OutSO)
                                            , dbClss.TDe(c.FirstOrDefault().OutPlan)
                                            , c.FirstOrDefault().OutQty.ToDecimal()
                                            , 0
                                            , "Waiting", "Waiting", cstm.VatGroup
                                            , dbClss.TSt(t.FirstOrDefault().VatType)
                                            , dbClss.TSt(dd.FirstOrDefault().CustomerPONo)
                                            , dbClss.TInt(c.FirstOrDefault().id)
                                            , dbClss.TSt(t.FirstOrDefault().ReplenishmentType)
                                            , "T"
                                            , false, false, dd.FirstOrDefault().CustomerNo
                                            , t.FirstOrDefault().CustomerPartNo
                                            , t.FirstOrDefault().CustomerPartName);

                                    }
                                }
                            }
                        }
                    }

                    if (dgvData.Rows.Count > 0)
                        cbbCSTM.Enabled = false;
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }


            if (false)
            {
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
                    if (lblStatus.Text == "Waiting Approve")
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
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void radButtonElement3_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    var so = db.mh_SaleOrders.Where(x => x.Active && x.SONo.Equals(txtSONo.Text.Trim())).ToList();
                    if (so.Count > 0 && (lblStatus.Text == "Waiting" || lblStatus.Text == "Waiting Approve"))
                    {
                        if (baseClass.IsSendApprove())
                        {
                            db.sp_062_mh_ApproveList_Add(txtSONo.Text.Trim(), "Sale Order", Classlib.User);
                            MessageBox.Show("Send complete.");
                            btnRefresh_Click(null, null);
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            btnAddPart_Click(null, null);
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


    public class po_to_so
    {
        public int idPO { get; set; }
        public decimal poQty { get; set; }
        public decimal poAmnt { get; set; }
        public decimal pricePer
        {
            get {
                return Math.Round(poAmnt / poQty, 2);
            }
        }
    }
}
