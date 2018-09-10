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
                ListDefualt();

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

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void ListDefualt()
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
                var com = dgvData.Columns["Location"] as GridViewComboBoxColumn;
                com.DisplayMember = "Name";
                com.ValueMember = "Code";
                com.DataSource = lo;

                var vg = db.mh_VatGroups.Where(x => x.Active.Value).ToList();
                //id, Value
                var com2 = dgvData.Columns["VatGroup"] as GridViewComboBoxColumn;
                com2.DisplayMember = "Value";
                com2.ValueMember = "id";
                com2.DataSource = vg;

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
                    var t = db.mh_SaleOrders.Where(x => x.Active && x.SONo == t_SONo).ToList();
                    if (t.Count > 0)
                    {
                        string CustNo = t.First().CustomerNo;
                        txtSONo.Text = t_SONo;
                        cbbCSTM.SelectedValue = CustNo;
                        txtCSTMNo.Text = CustNo;
                        dtSODate.Value = t.First().SODate;
                        txtAddress.Text = t.First().CustomerAddress;
                        txtRemark.Text = t.First().RemarkHD;
                        txtTotal.Value = t.First().TotalPrice;

                        foreach (var tt in t)
                        {
                            var rowe = dgvData.Rows.AddNew();
                            addRow(rowe.Index, tt.ReqDeliveryDate.Date, tt.ItemNo, tt.ItemName, tt.Description, tt.Location
                                , tt.Qty, tt.UOM, tt.PCSUnit, tt.UnitPrice, tt.Amount, tt.PriceIncVat
                                , tt.OutShip, tt.OutPlan
                                , tt.id, tt.Status, tt.PlanStatus
                                , tt.VatGroup, tt.VatType, tt.RefDocNo, tt.RefId.ToInt(), tt.RepType, "");
                        }

                        SetRowNo1(dgvData);
                        CallTotal();

                        btnView_Click(null, null);
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
                        var c = db.mh_CustomerPOs.Where(x => x.id == id).First();
                        if (fRow)
                        {
                            txtCSTMNo.Text = c.CustomerNo;
                            cbbCSTM.SelectedValue = c.CustomerNo;
                            dtSODate.Value = DateTime.Now;
                            cbbCSTM_SelectedIndexChanged(null, null);
                            txtRemark.Text = c.RemarkHD;
                            fRow = false;
                        }
                        //detail
                        var rowe = dgvData.Rows.AddNew();
                        var t = db.mh_Items.Where(x => x.InternalNo == c.ItemNo).First();
                        var cstm = db.mh_Customers.Where(x => x.No == c.CustomerNo).First();
                        addRow(rowe.Index, c.ReqDate, c.ItemNo, c.ItemName, "", t.Location
                            , Math.Round(c.OutSO / c.PCSUnit, 2), c.UOM, c.PCSUnit, c.PricePerUnit, c.Amount, false, c.OutSO, c.OutPlan
                            , 0, "Waiting", "Waiting", cstm.VatGroup, t.VatType, c.CustomerPONo, c.id, t.ReplenishmentType
                            , "T");

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
            txtSONo.Text = "";
            dtSODate.Value = DateTime.Today;
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
                    if (dgvData.Rows.Where(x => x.Cells["PlanStatus"].Value.ToSt() != "Waiting").Count() > 0)
                    {
                        baseClass.Warning("Cannot Delete because Already Planned.\n");
                        return;
                    }

                    if (baseClass.IsDel($"Do you want to Delete Sale Order: {poNo} ?"))
                    {
                        using (var db = new DataClasses1DataContext())
                        {
                            var p = db.mh_SaleOrders.Where(x => x.SONo == poNo && x.Active).ToList();
                            if (p.Where(x => x.Status != "Waiting").Count() < 1)
                            {
                                foreach (var pp in p)
                                {
                                    pp.Active = false;
                                    pp.UpdateBy = Classlib.User;
                                    pp.UpdateDate = DateTime.Now;
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
                        int idPO = item.Cells["RefId"].Value.ToInt();
                        if (idPO > 0)
                        {
                            var Qty = item.Cells["Qty"].Value.ToDecimal();
                            using (var db = new DataClasses1DataContext())
                            {
                                var qtyPO = db.mh_CustomerPOs.Where(x => x.id == idPO).First().Quantity;
                                int idSO = item.Cells["id"].Value.ToInt();
                                var qtySO = db.mh_SaleOrders.Where(x => x.Active && x.RefId == idPO && x.id != idSO).ToList();
                                if (qtySO.Sum(x => x.Qty * x.PCSUnit) + Qty > qtyPO)
                                {
                                    err += " “Qty:” is more than Customer P/O Qty.\n";
                                }
                            }
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
                dbClss.AddError("CreatePO", ex.Message, this.Name);
            }

            return re;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
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
                string sono = txtSONo.Text;
                string cstmNo = txtCSTMNo.Text;
                CallTotal();
                using (var db = new DataClasses1DataContext())
                {
                    bool fItem = true;
                    foreach (var item in dgvData.Rows)
                    {
                        if (item.Cells["dgvC"].Value.ToSt() == "") continue;
                        int id = item.Cells["id"].Value.ToInt();
                        var t = db.mh_SaleOrders.Where(x => x.id == id).FirstOrDefault();
                        if (t != null)
                        {
                            //edit
                            t.Active = item.IsVisible;
                        }
                        else if (item.IsVisible)
                        {
                            //add
                            t = new mh_SaleOrder();
                            if (sono == "" && fItem)
                                sono = dbClss.GetNo(28, 2);
                            t.Active = true;
                            t.CreateDate = DateTime.Now;
                            t.CreateBy = ClassLib.Classlib.User;
                            db.mh_SaleOrders.InsertOnSubmit(t);
                        }
                        else
                            continue;
                        t.SONo = sono;
                        t.CustomerNo = cstmNo;
                        t.CustomerName = cbbCSTM.Text;
                        t.SODate = dtSODate.Value.Date;
                        t.CustomerAddress = txtAddress.Text;
                        t.TotalPrice = txtTotal.Value.ToDecimal();
                        t.Vat = cbVat.Checked;
                        t.VatA = txtVatA.Value.ToDecimal();
                        t.VatAmnt = txtVatAmnt.Value.ToDecimal();
                        t.TotalPriceIncVat = txtGrandTotal.Value.ToDecimal();
                        t.UpdateBy = Classlib.User;
                        t.UpdateDate = DateTime.Now;
                        t.RemarkHD = txtRemark.Text;
                        //rowe
                        t.ReqDeliveryDate = item.Cells["ReqDate"].Value.ToDateTime().Value;
                        t.ItemNo = item.Cells["ItemNo"].Value.ToSt();
                        t.ItemName = item.Cells["ItemName"].Value.ToSt();
                        t.Description = item.Cells["Description"].Value.ToSt();
                        t.Location = item.Cells["Location"].Value.ToSt();
                        t.Qty = item.Cells["Qty"].Value.ToDecimal();
                        t.UOM = item.Cells["Unit"].Value.ToSt();
                        t.PCSUnit = item.Cells["PCSUnit"].Value.ToDecimal();
                        t.UnitPrice = item.Cells["UnitPrice"].Value.ToDecimal();
                        t.Amount = item.Cells["Amount"].Value.ToDecimal();
                        t.PriceIncVat = item.Cells["PriceIncVat"].Value.ToBool();
                        t.VatGroup = item.Cells["VatGroup"].Value.ToInt();
                        t.VatType = item.Cells["VatType"].Value.ToSt();
                        if (item.Cells["Status"].Value.ToSt() == "Waiting")
                            t.OutShip = item.Cells["Qty"].Value.ToDecimal();
                        else
                            t.OutShip = item.Cells["OutShip"].Value.ToDecimal();
                        t.Status = item.Cells["Status"].Value.ToSt();
                        t.RefDocNo = item.Cells["RefDocNo"].Value.ToSt();
                        t.RefId = item.Cells["RefId"].Value.ToInt();
                        t.RepType = item.Cells["RepType"].Value.ToSt();
                        t.PlanStatus = item.Cells["PlanStatus"].Value.ToSt();
                        t.OutPlan = item.Cells["OutPlan"].Value.ToDecimal();
                    }

                    t_SONo = sono;
                    t_CustomerNo = cstmNo;
                    db.SubmitChanges();

                    updateOutSO();
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

        private void updateOutSO()
        {
            using (var db = new DataClasses1DataContext())
            {//Update Customer P/O (Out Sale Order Q'ty)
                foreach (var idPO in dgvData.Rows.Select(x => x.Cells["RefId"].Value.ToInt()))
                {
                    if (idPO == 0) continue;
                    var c = db.mh_CustomerPOs.Where(x => x.id == idPO).First();
                    var m = db.mh_SaleOrders.Where(x => x.Active && x.RefId == idPO).ToList();
                    decimal qq = 0.00m;
                    if (m.Count > 0)
                        qq = m.Sum(x => x.Qty * x.PCSUnit);
                    c.OutSO = (c.Quantity * c.PCSUnit) - qq;
                    if (c.OutSO == c.Quantity)
                        c.Status = "Waiting";
                    else if (c.OutSO <= 0)
                        c.Status = "Completed";
                    else
                        c.Status = "Proeces";
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
                                e.Row.Cells["Unit"].Value = t.BaseUOM;
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
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void MasterTemplate_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            if (e.RowIndex >= -1)
            {
                if (e.Row.Cells["PlanStatus"].Value.ToSt() != "Waiting")
                {
                    e.Cancel = true;
                    return;
                }
                if (e.Row.Cells["Status"].Value.ToSt() != "Waiting")
                {
                    e.Cancel = true;
                    return;
                }

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
                if (dgvData.Rows.Count < 0)
                    return;

                if (Ac.Equals("New") || Ac.Equals("Edit"))
                {
                    this.Cursor = Cursors.WaitCursor;

                    if (dgvData.CurrentCell.RowInfo.Cells["PlanStatus"].Value.ToSt() != "Waiting")
                    {
                        baseClass.Warning("Cannot Delete because Already Planned.\n");
                        return;
                    }

                    if (dgvData.CurrentCell.RowInfo.Cells["Status"].Value.ToSt() == "Waiting")
                    {

                        int id = 0;
                        int.TryParse(StockControl.dbClss.TSt(dgvData.CurrentCell.RowInfo.Cells["id"].Value), out id);
                        if (id <= 0)
                            dgvData.Rows.Remove(dgvData.CurrentCell.RowInfo);

                        else
                        {
                            row = dgvData.CurrentCell.RowInfo.Index;
                            //btnDelete_Click(null, null);
                            using (var db = new DataClasses1DataContext())
                            {
                                var m = db.mh_SaleOrders.Where(x => x.id == id).FirstOrDefault();
                                if (m != null)
                                {
                                    m.Active = false;
                                    m.UpdateDate = DateTime.Now;
                                    m.UpdateBy = ClassLib.Classlib.User;
                                    db.SubmitChanges();

                                    updateOutSO();
                                    dgvData.Rows.Remove(dgvData.CurrentCell.RowInfo);
                                }
                            }
                        }
                        CallTotal();
                        //getTotal();
                        SetRowNo1(dgvData);

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
                var pol = new SaleOrder_List(2);
                this.Cursor = Cursors.Default;
                pol.ShowDialog();
                if (pol.PONo != "" && pol.CstmNo != "")
                {
                    t_SONo = pol.PONo;
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
        }
        void AddPartE()
        {
            if (txtCSTMNo.Text == "")
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
                        var cc = db.mh_Customers.Where(x => x.No == txtCSTMNo.Text).First();
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
