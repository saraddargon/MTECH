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
    public partial class ProductionOrder : Telerik.WinControls.UI.RadRibbonForm
    {
        string t_JobNo = "";

        bool demo = false;
        public ProductionOrder()
        {
            InitializeComponent();
        }
        public ProductionOrder(string JobNo)
        {
            InitializeComponent();
            this.t_JobNo = JobNo;
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

                LoadDefault();
                var m = t_JobNo;
                ClearData();
                btnNew_Click(null, null);
                t_JobNo = m;

                if (t_JobNo != "")
                    DataLoad();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void LoadDefault()
        {
            using (var db = new DataClasses1DataContext())
            {

            }
        }

        private void DataLoad(bool warningMssg = true)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int ck = 0;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var t = db.mh_ProductionOrders.Where(x => x.Active && x.JobNo == t_JobNo).FirstOrDefault();
                    if (t != null)
                    {
                        txtJobNo.Text = t.JobNo;
                        txtRefDocNo.Text = t.RefDocNo;
                        txtRefDocId.Text = t.RefDocId.ToSt();
                        txtFGNo.Text = t.FGNo;
                        txtFGName.Text = t.FGName;
                        txtStartingDate.Text = t.StartingDate.ToDtTimeString();
                        txtEndingDate.Text = t.EndingDate.ToDtTimeString();
                        txtReqDate.Text = t.ReqDate.ToDtString();
                        txtUOM.Text = t.UOM;
                        txtPCSUnit.Value = t.PCSUnit;
                        txtFGQty.Value = t.Qty;
                        txtOutQty.Value = t.OutQty;
                        txtCreateDate.Text = t.CreateDate.ToDtString();
                        txtCreateBy.Text = t.CreateBy;
                        txtLotNo.Text = t.LotNo;
                        cbHoldJob.Checked = t.HoldJob;
                        if (cbHoldJob.Checked)
                            btnHoldJob.Text = "Unhold Job";
                        else
                            btnHoldJob.Text = "Hold Job";
                        txtidJob.Text = t.id.ToSt();
                        txtSeqStatus.Text = t.SeqStatus.ToSt();

                        if (txtSeqStatus.Text.ToInt() == 2)
                            txtStatus.Text = "Approved";
                        else
                            txtStatus.Text = "Waiting";

                        //dt
                        var dts = db.mh_ProductionOrderRMs.Where(x => x.JobNo == t.JobNo && x.Active).ToList();
                        foreach (var dt in dts)
                        {
                            addRow(dt.id, dt.ItemNo, dt.ItemName, dt.Qty, dt.UOM, dt.PCSUnit
                                , dt.OutQty, dt.GroupType, dt.Type, dt.InvGroup);
                        }

                        //Load Pr in Job
                        LoadPRwithJob(t.RefDocId);


                        SetRowNo1(dgvData);
                        SetRowNo1(dgvPurchase);
                        btnView_Click(null, null);
                    }
                    else if (warningMssg)
                        baseClass.Warning("Job Orders not found.!!");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void addRow(int id, string itemNo, string itemName, decimal qty, string uOM, decimal pCSUnit
            , decimal outQty, string groupType, string type, string invGroup)
        {
            var rowe = dgvData.Rows.AddNew();
            rowe.Cells["RNo"].Value = rowe.Index + 1;
            rowe.Cells["id"].Value = id;
            rowe.Cells["ItemNo"].Value = itemNo;
            rowe.Cells["ItemName"].Value = itemName;
            rowe.Cells["Qty"].Value = qty;
            rowe.Cells["UOM"].Value = uOM;
            rowe.Cells["PCSUnit"].Value = pCSUnit;
            rowe.Cells["GroupType"].Value = groupType;
            rowe.Cells["Type"].Value = type;
            rowe.Cells["InvGroup"].Value = invGroup;
            rowe.Cells["dgvC"].Value = "T";
            rowe.Cells["OutShip"].Value = outQty;
            rowe.Cells["Shipped"].Value = qty - outQty;

            ////find Shipped
            //if (txtJobNo.Text != "")
            //{
            //    using (var db = new DataClasses1DataContext())
            //    {
            //        var m = db.Get_ShipQty(itemNo, txtJobNo.Text);
            //        rowe.Cells["Shipped"].Value = m.ToDecimal();
            //        rowe.Cells["OutShip"].Value = qty - m.ToDecimal();
            //    }
            //}
            //else
            //{
            //    rowe.Cells["Shipped"].Value = 0.00m;
            //    rowe.Cells["OutShip"].Value = qty;
            //}
        }

        void LoadPRwithJob(int idCustomerPoDt)
        {
            try
            {
                dgvPurchase.DataSource = null;
                dgvPurchase.Rows.Clear();
                if (idCustomerPoDt == 0) return;
                using (var db = new DataClasses1DataContext())
                {
                    //load Purchase, P/O, Receive
                    var pr = db.mh_PurchaseRequestLines.Where(x => x.SS == 1 && x.idCstmPODt != null
                            && x.idCstmPODt == idCustomerPoDt)
                        .Join(db.mh_PurchaseRequests.Where(q => q.Status != "Cancel")
                        , dt => dt.PRNo
                        , hd => hd.PRNo
                        , (dt, hd) => new { dt, hd }
                    ).ToList();
                    foreach (var itemPr in pr)
                    {
                        var rowe = addRowPr(itemPr.dt.id, itemPr.hd.PRNo, itemPr.dt.PoNo
                             , itemPr.dt.CodeNo, itemPr.dt.ItemName, itemPr.dt.OrderQty
                             , 0, itemPr.dt.UOM, itemPr.dt.PCSUOM);
                        //find PO
                        if (itemPr.dt.RefPOid.ToInt() > 0)
                        {
                            var po = db.mh_PurchaseOrderDetails.Where(x => x.SS == 1 && x.id == itemPr.dt.RefPOid)
                                .Join(db.mh_PurchaseOrders.Where(x => x.Status != "Cancel")
                                , dt => dt.PONo
                                , hd => hd.PONo
                                , (dt, hd) => new { dt, hd });
                            foreach (var p in po)
                            {
                                //set OutReceive ---- BackOrder
                                var OutReceive = rowe.Cells["OutReceive"].Value.ToDecimal();
                                var OrderQty = rowe.Cells["Qty"].Value.ToDecimal();
                                rowe.Cells["OutReceive"].Value = Math.Round(OutReceive + p.dt.BackOrder.ToDecimal(), 2);
                                OutReceive = rowe.Cells["OutReceive"].Value.ToDecimal();
                                rowe.Cells["ReceiveQty"].Value = Math.Round(OrderQty - OutReceive, 2);
                            }
                        }
                        else
                        {
                            var OrderQty = rowe.Cells["Qty"].Value.ToDecimal();
                            rowe.Cells["OutReceive"].Value = OrderQty;
                            var OutReceive = rowe.Cells["OutReceive"].Value.ToDecimal();
                            rowe.Cells["ReceiveQty"].Value = Math.Round(OrderQty - OutReceive, 2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                baseClass.Warning(ex.Message);
            }
        }

        private GridViewRowInfo addRowPr(int idPR, string pRNo, string poNo, string ItemNo
            , string itemName, decimal orderQty, decimal outReceive, string uOM, decimal pCSUnit)
        {
            var rowe = dgvPurchase.Rows.AddNew();
            rowe.Cells["rNo"].Value = rowe.Index + 1;
            rowe.Cells["idPR"].Value = idPR;
            rowe.Cells["PRNo"].Value = pRNo;
            rowe.Cells["PONo"].Value = poNo;
            rowe.Cells["ItemNo"].Value = ItemNo;
            rowe.Cells["ItemName"].Value = itemName;
            rowe.Cells["Qty"].Value = Math.Round(orderQty, 2);
            rowe.Cells["ReceiveQty"].Value = Math.Round(orderQty - outReceive, 2);
            rowe.Cells["OutReceive"].Value = outReceive;
            rowe.Cells["UOM"].Value = uOM;
            rowe.Cells["PCSUnit"].Value = pCSUnit;
            return rowe;
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
            txtJobNo.Text = "";
            txtRefDocId.Text = "0";
            txtRefDocNo.Text = "";
            txtFGNo.Text = "";
            txtFGName.Text = "";
            txtReqDate.Text = "";
            txtFGQty.Value = 0;
            txtUOM.Text = "";
            txtPCSUnit.Value = 0;
            txtStartingDate.Text = "";
            txtEndingDate.Text = "";
            txtOutQty.Value = 0;
            txtLotNo.Text = "";
            txtCreateBy.Text = Classlib.User;
            txtCreateDate.Text = DateTime.Now.ToDtString();
            cbHoldJob.Checked = false;
            txtStatus.Text = "Waiting";
            txtidJob.Text = "";
            txtSeqStatus.Text = "";

            using (var db = new DataClasses1DataContext())
            {
                var d = DateTime.Now.Date;
                var m = db.mh_LotFGs.Where(x => x.LotDate == d).FirstOrDefault();
                if (m != null)
                    txtLotNo.Text = m.LotNo;
            }
            //

            dgvData.DataSource = null;
            dgvData.Rows.Clear();

            t_JobNo = "";
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

            //dgvData.ReadOnly = false;

            ClearData();
            Enable_Status(true, "New");
            lblStatus.Text = "New";
            Ac = "New";
            row = dgvData.Rows.Count - 1;
            if (row < 0)
                row = 0;

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

            //dgvData.ReadOnly = true;

            Enable_Status(false, "View");
            lblStatus.Text = "View";
            lblStatus.Text = txtStatus.Text;
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

            //dgvData.ReadOnly = false;

            Enable_Status(true, "Edit");
            lblStatus.Text = "Edit";
            Ac = "Edit";

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtFGNo.Text == "") return;
            if (Math.Round(txtFGQty.Value.ToDecimal(), 2) != txtOutQty.Value.ToDecimal())
            {
                baseClass.Warning("- Cannot Delete because Status is 'Process'.\n");
                return;
            }
            else if (txtSeqStatus.Text.ToInt() > 0)
            {
                baseClass.Warning("- Cannot Delete because Status is 'Approved'.\n");
                return;
            }

            if (dgvData.Rows.Where(x => x.Cells["Qty"].Value.ToDecimal() != x.Cells["OutShip"].Value.ToDecimal()).Count() > 0)
            {
                baseClass.Warning("- RM or SEMI alreday shipped into this 'Job', Please cancel shipping RM.\n");
                return;
            }

            if (baseClass.IsDel("Do you want to 'Delete' ?"))
                DeleteE();
        }
        void DeleteE()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                string jobNo = txtJobNo.Text;
                using (var db = new DataClasses1DataContext())
                {
                    var m = db.mh_ProductionOrders.Where(x => x.JobNo == jobNo && x.Active).FirstOrDefault();
                    if (m != null)
                    {
                        m.Active = false;
                        m.UpdateDate = DateTime.Now;
                        m.UpdateBy = ClassLib.Classlib.User;

                        var po = db.mh_CustomerPODTs.Where(x => x.id == m.RefDocId).FirstOrDefault();
                        if (po != null)
                        {
                            //po.OutPlan += (m.Qty * m.PCSUnit) - m.OutQty;
                            po.OutPlan = Math.Round(m.Qty, 2); //Full Return Qty
                            po.Status = baseClass.setCustomerPOStatus(po);
                            db.SubmitChanges();

                            //remove capa
                            int idJob = txtidJob.Text.ToInt();
                            var capa = db.mh_CapacityLoads.Where(x => x.DocId == idJob).ToList();
                            db.mh_CapacityLoads.DeleteAllOnSubmit(capa);
                            //remove calendar
                            var cal = db.mh_CalendarLoads.Where(x => x.idJob == idJob).ToList();
                            db.mh_CalendarLoads.DeleteAllOnSubmit(cal);

                            db.SubmitChanges();

                            baseClass.Info("Delete complete.\n");
                            this.Close();
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
                //if (txtCodeNo.Text.Equals(""))
                //    err += " “รหัสพาร์ท:” เป็นค่าว่าง \n";

                if (txtFGQty.Value.ToDecimal() != txtOutQty.Value.ToDecimal())
                    err += "- Cannot Save because Status is 'Process'.\n";
                //else if (txtStatus.Text == "Approved")
                //    err += "- Cannot Save because Status is 'Approved'.\n";
                else if (txtStatus.Text.ToInt() > 0)
                    err += "- Cannot Save because Status is 'Approved'.\n";


                if (!err.Equals(""))
                    baseClass.Error(err);
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
                    MessageBox.Show("Can save only status 'New' or 'Edit'");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        void SaveE()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    string jobNo = txtJobNo.Text;
                    bool newJob = false;
                    //Hd
                    var m = db.mh_ProductionOrders.Where(x => x.JobNo == jobNo).FirstOrDefault();
                    if (m == null)
                    {
                        m = new mh_ProductionOrder();
                        m.CreateBy = ClassLib.Classlib.User;
                        m.CreateDate = DateTime.Now;
                        m.JobDate = DateTime.Now.Date;
                        m.JobNo = dbClss.GetNo(29, 2);
                        m.SeqStatus = 0;
                        db.mh_ProductionOrders.InsertOnSubmit(m);
                        newJob = true;
                        dbClss.AddHistory(this.Name, "Job Order Sheet", $"Edit Job Order Sheet : {m.JobNo}", m.JobNo);
                    }
                    m.Active = true;
                    m.EndingDate = txtEndingDate.Text.ToDateTime().Value;
                    m.FGName = txtFGName.Text;
                    m.FGNo = txtFGNo.Text;
                    m.LotNo = txtLotNo.Text;
                    m.Qty = txtFGQty.Value.ToDecimal();
                    m.PCSUnit = txtPCSUnit.Value.ToDecimal();
                    m.OutQty = m.Qty;
                    m.RefDocId = txtRefDocId.Text.ToInt();
                    m.RefDocNo = txtRefDocNo.Text;
                    m.ReqDate = txtReqDate.Text.ToDateTime().Value;
                    m.StartingDate = txtStartingDate.Text.ToDateTime().Value;
                    m.UOM = txtUOM.Text;
                    m.UpdateBy = ClassLib.Classlib.User;
                    m.UpdateDate = DateTime.Now;

                    db.sp_062_mh_ApproveList_Add(m.JobNo, "Job_Req", ClassLib.Classlib.User);

                    //update Customer P/O [Only New Job] - Out Plan
                    if (newJob)
                    {
                        dbClss.AddHistory(this.Name, "Job Order Sheet", $"New Job Order Sheet : {m.JobNo}", m.JobNo);
                        var po = db.mh_CustomerPODTs.Where(x => x.id == m.RefDocId).FirstOrDefault();
                        if (po != null)
                        {
                            var pohd = db.mh_CustomerPOs.Where(x => x.id == po.idCustomerPO).FirstOrDefault();
                            string pono = (pohd != null) ? pohd.CustomerPONo : "";
                            dbClss.AddHistory("CustomerPO", "CustomerPO", $"New job Order Sheet : {m.JobNo} for P/O {pohd.CustomerPONo}", pohd.CustomerPONo);
                            //po.OutPlan -= m.OutQty;
                            po.OutPlan = 0;//Full Ref Customer P/O
                            po.Status = baseClass.setCustomerPOStatus(po);
                            db.SubmitChanges();
                        }
                    }

                    t_JobNo = m.JobNo;
                    //dt
                    foreach (var item in dgvData.Rows)
                    {
                        int id = item.Cells["id"].Value.ToInt();
                        var d = db.mh_ProductionOrderRMs.Where(x => x.id == id).FirstOrDefault();
                        if (d == null)
                        {
                            d = new mh_ProductionOrderRM();
                            db.mh_ProductionOrderRMs.InsertOnSubmit(d);
                        }
                        d.Active = true;
                        d.ItemNo = item.Cells["ItemNo"].Value.ToSt();
                        d.ItemName = item.Cells["ItemName"].Value.ToSt();
                        d.Qty = item.Cells["Qty"].Value.ToDecimal();
                        d.UOM = item.Cells["UOM"].Value.ToSt();
                        d.PCSUnit = item.Cells["PCSUnit"].Value.ToDecimal();
                        d.OutQty = item.Cells["OutShip"].Value.ToDecimal();
                        d.GroupType = item.Cells["GroupType"].Value.ToSt();
                        d.Type = item.Cells["Type"].Value.ToSt();
                        d.InvGroup = item.Cells["InvGroup"].Value.ToSt();
                        db.SubmitChanges();
                    }
                }

                var docno = t_JobNo;
                baseClass.Info("Save complete(s).");
                ClearData();
                t_JobNo = docno;
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
                    //var itemNo = e.Row.Cells["ItemNo"].Value.ToSt();
                    //if (e.Column.Name.Equals("Amount") || e.Column.Name.Equals("Qty"))
                    //{
                    //    if (e.Row.Cells["Qty"].Value.ToDecimal() > 0)
                    //    {
                    //        var m = Math.Round(e.Row.Cells["Amount"].Value.ToDecimal() / e.Row.Cells["Qty"].Value.ToDecimal(), 2);
                    //        dgvData.Rows[e.RowIndex].Cells["PricePerUnit"].Value = m;
                    //    }
                    //    else
                    //        dgvData.Rows[e.RowIndex].Cells["PricePerUnit"].Value = 0;

                    //}
                    //else if (e.Column.Name.Equals("ItemNo"))
                    //{
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
                    //            //addRow(e.RowIndex
                    //            //    , DateTime.Now, t.InternalNo, t.InternalName
                    //            //    , 1, t.BaseUOM, pcsunit, 0, 0
                    //            //    , 1, 1, "", 0, "Waiting");
                    //        }
                    //        else
                    //        {
                    //            e.Row.Cells["ItemName"].Value = t.InternalName;
                    //            e.Row.Cells["Unit"].Value = t.BaseUOM;
                    //            e.Row.Cells["PCSUnit"].Value = pcsunit;
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
                    //    }
                    //}
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
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


        private void ลบพารทToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                //if (dgvData.Rows.Count < 0)
                //    return;


                //if (Ac.Equals("New") || Ac.Equals("Edit"))
                //{
                //    this.Cursor = Cursors.WaitCursor;


                //    if (dgvData.CurrentRow.Cells["Status"].Value.ToSt() == "Waiting")
                //    {

                //        int id = 0;
                //        int.TryParse(StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["id"].Value), out id);
                //        if (id <= 0)
                //            dgvData.Rows.Remove(dgvData.CurrentRow);

                //        else
                //        {
                //            row = dgvData.CurrentRow.Index;
                //            btnDelete_Click(null, null);
                //        }
                //        CallTotal();
                //        //getTotal();
                //        SetRowNo1(dgvData);
                //    }
                //    else
                //        MessageBox.Show("ไม่สามารถทำการลบรายการได้ สถานะไม่ถูกต้อง");
                //}
                //else
                //{
                //    MessageBox.Show("ไม่สามารถทำการลบรายการได้");
                //}
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
                var pd = new ProductionOrder_List();
                this.Cursor = Cursors.Default;
                pd.ShowDialog();
                //if (pol.PONo != "" && pol.CstmNo != "")
                //{
                //    t_PONo = pol.PONo;
                //    t_CustomerNo = pol.CstmNo;
                //    //LoadData
                //    DataLoad();
                //}


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
                PrintPR a = new PrintPR(txtJobNo.Text, txtJobNo.Text, "ReportProductionOrder");
                a.ShowDialog();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
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

            }
            catch (Exception ex) { MessageBox.Show("err2: " + ex.Message); }
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
                        //addRow(rowE.Index, DateTime.Now, itemNo, t.InternalName
                        //    , 1, t.BaseUOM, u, 0, 0, 0, 0
                        //    , "", 0, "Waiting");
                    }
                    SetRowNo1(dgvData);

                }
            }
        }

        bool chkRecalE()
        {
            bool ret = true;
            string mssg = "";

            if (txtJobNo.Text.Trim() == "")
                mssg += "- Please Save job before Hold Job.\n";
            if (txtFGQty.Value.ToDecimal() != txtOutQty.Value.ToDecimal())
                mssg += "- Cannot Hold Job because FG Already Received.\n";

            if (mssg != "")
            {
                baseClass.Warning(mssg);
                ret = false;
            }
            return ret;
        }
        private void btnRecal_Click(object sender, EventArgs e)
        {
            if (chkRecalE() && baseClass.Question("Do you want to 'Recal' ?"))
                Recal();
        }
        void Recal()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //baseClass.Info("Comming soon...");
                MoveJobToLast();

                baseClass.Info("Recal completed.");
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

        bool ChkHoldJob()
        {
            bool ret = true;
            string mssg = "";


            if (txtJobNo.Text.Trim() == "")
                mssg += "- Please Save job before Hold Job.\n";
            if (txtFGQty.Value.ToDecimal() != txtOutQty.Value.ToDecimal())
                mssg += "- Cannot Hold Job because FG Already Received.\n";

            if (mssg != "")
            {
                ret = false;
                baseClass.Warning(mssg);
            }
            return ret;
        }
        private void btnHoldJob_Click(object sender, EventArgs e)
        {
            if (ChkHoldJob() && baseClass.Question("Do you want to 'Hold Job' ?"))
                HoldJob();
        }
        void HoldJob()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                string jobno = txtJobNo.Text.Trim();
                using (var db = new DataClasses1DataContext())
                {
                    var m = db.mh_ProductionOrders.Where(x => x.JobNo == jobno).FirstOrDefault();
                    if (m != null)
                    {
                        m.UpdateBy = Classlib.User;
                        m.UpdateDate = DateTime.Now;
                        if (!m.HoldJob)
                        {
                            m.HoldJob = true;
                            db.SubmitChanges();
                        }
                        else
                        {
                            m.HoldJob = false;
                            db.SubmitChanges();
                            //หาว่าวันที่ Unhold Job มากกว่าหรือเท่ากับ StartingDate ไหม ถ้าใช่ให้ move Capacity ไปไว้ท้ายสุดเลย
                            if (DateTime.Now.Date >= txtStartingDate.Text.ToDateTime().Value.Date)
                            {
                                MoveJobToLast();
                            }
                        }

                        cbHoldJob.Checked = m.HoldJob;

                        dbClss.AddHistory(this.Name, "Job Order Sheet", $"Change Status Hold job to {((m.HoldJob) ? "Hold" : "UnHold")}", jobno);

                        if (m.HoldJob)
                        {
                            baseClass.Info("Hold Job completed.");
                            btnHoldJob.Text = "Unhold Job";
                        }
                        else
                        {
                            baseClass.Info("Unhold Job completed.");
                            btnHoldJob.Text = "Hold Job";
                        }
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

        void MoveJobToLast()
        {
            try
            {
                //**หา Ending date ที่มากที่สุดที่ถูกสร้งเป็น Job แล้ว
                int idJob = txtidJob.Text.ToInt();
                string JobNo = txtJobNo.Text.Trim();
                DateTime? StartingDate = null;
                DateTime? EndingDate = null;
                var capacityLoad = new List<mh_CapacityLoad>();
                var workLoads = new List<WorkLoad>();
                var calLoad = new List<mh_CalendarLoad>();
                //***ก๊อปมาจาก PlanningCal_Status
                var costOverHead = 0.00m;
                var capaUseX_All = 0.00m;
                using (var db = new DataClasses1DataContext())
                {
                    var tdata = new ItemData(txtFGNo.Text);

                    var maxDateJob = db.mh_ProductionOrders.Where(x => x.Active).Max(x => x.EndingDate);
                    var dFrom = maxDateJob.Date;
                    var dTo = maxDateJob.Date;

                    capacityLoad = db.mh_CapacityLoads.Where(x => x.Active && x.Date >= dFrom).ToList();
                    workLoads = baseClass.getWorkLoad(dFrom, dTo);
                    calLoad = db.mh_CalendarLoads.Where(x => x.Date >= dFrom && x.Date <= dTo).ToList();

                    //manu Unit Time
                    decimal manuTime = 1;
                    var manuUnit = db.mh_ManufacturingSetups.Select(x => x.ShowCapacityInUOM).FirstOrDefault();
                    if (manuUnit == 2)
                        manuTime = 60;
                    else if (manuUnit == 3)
                        manuTime = (24 * 60);
                    //**Routeing
                    var t_StartingDate = (DateTime?)null;
                    var t_EndingDate = (DateTime?)null;
                    bool firstStart = false;
                    DateTime tempStarting = maxDateJob;
                    var rt = db.mh_RoutingDTs.Where(x => x.RoutingId == tdata.Routeid)
                        .Join(db.mh_WorkCenters.Where(x => x.Active)
                        , hd => hd.idWorkCenter
                        , workcenter => workcenter.id
                        , (hd, workcenter)
                        => new { hd, hd.idWorkCenter, hd.id, hd.SetupTime, hd.RunTime, hd.WaitTime, workcenter })
                        .ToList();
                    foreach (var r in rt)
                    {
                        if (t_EndingDate != null)
                            tempStarting = t_EndingDate.Value;
                        int idWorkCenter = r.idWorkCenter;
                        var totalCapa_All = 0.00m;
                        var SetupTime = r.SetupTime * manuTime;
                        var RunTime = r.RunTime * manuTime;
                        var RunTimeCapa = Math.Round(((RunTime * txtFGQty.Text.ToDecimal()) / r.workcenter.Capacity), 2);
                        var WaitingTime = r.WaitTime * manuTime;
                        totalCapa_All = SetupTime + RunTimeCapa + r.WaitTime;
                        var CapaUseX = 0.00m;
                        CapaUseX = totalCapa_All;
                        var costPerU = r.hd.UnitCost;
                        costPerU = Math.Round(costPerU / manuTime, 2);

                        capaUseX_All += CapaUseX;
                        costOverHead += Math.Round(costPerU * CapaUseX, 2);

                        //find capacity Available (Workcenter) on date
                        do
                        {
                            //1. หาว่าเวลาเริ่มสามารถใช้ได้ไหม
                            var wl = workLoads.Where(x => x.Date >= tempStarting.Date && x.CapacityAfterX > 0
                                && x.idWorkCenter == idWorkCenter).OrderBy(x => x.Date).FirstOrDefault();
                            if (wl == null)
                            {
                                var w = baseClass.getWorkLoad(tempStarting.Date, null).Where(x => x.CapacityAfterX > 0 && x.idWorkCenter == idWorkCenter).FirstOrDefault();
                                if (w == null)
                                {
                                    string mssg = "Capacity is not available, Please check Capacity Work load on Capacity Calculation (Work Centers).!!!\n";
                                    baseClass.Warning(mssg);
                                    throw new Exception(mssg);
                                }
                                else
                                {
                                    workLoads.Add(w);
                                    wl = w;
                                }
                            }
                            if (tempStarting == null || wl.Date.Date > tempStarting.Date)
                                tempStarting = wl.Date.Date;

                            //set Starting
                            int dow = baseClass.getDayOfWeek(tempStarting.DayOfWeek);
                            //
                            var wd = db.mh_WorkCenters.Where(x => x.id == wl.idWorkCenter)
                                .Join(db.mh_WorkingDays.Where(x => x.Day == dow && x.Active)
                                , hd => hd.Calendar
                                , dt => dt.idCalendar
                                , (hd, dt) => new
                                {
                                    hd,
                                    dt,
                                    StartingTime = baseClass.setTimeSpan(dt.StartingTime),
                                    EndingTime = baseClass.setTimeSpan(dt.EndingTime)
                                }).ToList();
                            if (wd.Count > 0)
                            {
                                //
                                var sTime = wd.Min(x => x.StartingTime); //Starting Time of Working Day
                                if (sTime < tempStarting.TimeOfDay)
                                    sTime = tempStarting.TimeOfDay;
                                var eTime = wd.Max(x => x.EndingTime); //Ending Time of Working Day
                                var meTime = sTime; //for starting Time
                                int idCalendar = wd.First().hd.Calendar;
                                //หาว่าเวาลาเริ่มของ Work center นี้ใช้ไปหรือยัง หรือเป็นวันหยุดหรือวันลาหรือไม่
                                var calLoads = calLoad.Where(x => x.Date == tempStarting.Date
                                        && ((x.idCal == idCalendar && x.idWorkcenter == 0)
                                                ||
                                             x.idWorkcenter == wl.idWorkCenter && x.idCal == idCalendar)
                                    ).OrderBy(x => x.StartingTime).ThenBy(x => x.EndingTime).ToList();
                                if (calLoads.Count == 0)
                                {
                                    var cl = db.mh_CalendarLoads.Where(x => x.Date == tempStarting.Date
                                        && (x.idCal == idCalendar || x.idWorkcenter == wl.idWorkCenter)
                                    ).OrderBy(x => x.StartingTime).ThenBy(x => x.EndingTime).ToList();
                                    if (cl.Count > 0)
                                    {
                                        calLoads = cl;
                                        calLoad.AddRange(calLoads);
                                    }
                                }
                                //
                                if (calLoads.Count > 0)
                                {
                                    bool foundTime = false;
                                    List<int> idCal = new List<int>();
                                    while (meTime < eTime)
                                    {
                                        var ww = calLoads.Where(x => meTime >= x.StartingTime
                                            && meTime <= x.EndingTime && !idCal.Any(q => q == x.id)).FirstOrDefault();
                                        if (ww != null)
                                        {
                                            idCal.Add(ww.id);
                                            meTime = ww.EndingTime;
                                            //ถ้าเป็นช่วงเวลาที่ไม่ใช่เวลาทำงาน
                                            if (wd.Where(x => meTime >= x.StartingTime
                                                 && meTime <= x.EndingTime).ToList().Count < 1)
                                            {
                                                //หาเวลาที่น้อยที่สุดที่มากกว่า meTime
                                                var a = wd.Where(x => meTime < x.StartingTime).FirstOrDefault();
                                                if (a != null)
                                                    meTime = a.StartingTime;
                                            }
                                            //ถ้าเป็นช่วงเวลาทำงานปกติ ต้องเช็คต่อว่ายังมีเวลา CalendarLoad เหลือให้เช็คอีกไหม และ
                                            //ถ้าไม่เหลือแล้ว และน้อยกว่า Ending Time WorkingDay
                                            else if (idCal.Count == calLoads.Count()
                                                && meTime < eTime)
                                            {
                                                foundTime = true;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            foundTime = true;
                                            break;
                                        }

                                        if (foundTime)
                                            break;
                                    }
                                    if (foundTime)
                                        t_StartingDate = tempStarting.Date.AddHours(meTime.Hours).AddMinutes(meTime.Minutes);
                                }
                                else
                                    t_StartingDate = tempStarting.Date.AddHours(meTime.Hours).AddMinutes(meTime.Minutes);

                                var meTime2 = meTime; //for ending time
                                                      //Find Ending Date-Time
                                if (t_StartingDate != null)
                                {
                                    int autoid = 0;
                                    do
                                    {
                                        var rd = new Random();
                                        autoid = rd.Next(1, 99999999);
                                    } while (calLoad.Where(x => x.id == autoid).Count() > 0);
                                    //
                                    var cal = calLoad.Where(x => x.Date == tempStarting
                                            && ((x.idCal == idCalendar && x.idWorkcenter == 0)
                                                    ||
                                             x.idWorkcenter == wl.idWorkCenter && x.idCal == idCalendar)
                                             && x.StartingTime >= meTime
                                         ).OrderBy(x => x.StartingTime).ThenBy(x => x.EndingTime).ToList();
                                    if (cal.Count > 0)
                                    {
                                        var tempcal = new List<mh_CalendarLoad>();
                                        cal.ForEach(x =>
                                        {
                                            tempcal.Add(x);
                                        });
                                        //var t_meTime = meTime;
                                        var t_meTime2 = meTime2;
                                        var isNull = false;
                                        while (wl.CapacityAfterX > 0)
                                        {
                                            var aTime = tempcal.FirstOrDefault();
                                            bool AddC = false;
                                            if (aTime == null)
                                            {
                                                if (eTime > meTime)
                                                {
                                                    wl.CapacityAlocateX += (eTime - t_meTime2).TotalMinutes.ToDecimal();
                                                    var capaLoad = new mh_CapacityLoad
                                                    {
                                                        Active = true,
                                                        CapacityX = (eTime - t_meTime2).TotalMinutes.ToDecimal(),
                                                        Capacity = 0,
                                                        Date = tempStarting.Date,
                                                        DocId = idJob,
                                                        id = 0,
                                                        WorkCenterID = r.idWorkCenter,
                                                    };
                                                    capacityLoad.Add(capaLoad);
                                                    CapaUseX -= (eTime - t_meTime2).TotalMinutes.ToDecimal();
                                                    AddC = true;
                                                }
                                                t_meTime2 = eTime;
                                                meTime2 = t_meTime2;
                                                isNull = true;
                                            }
                                            else
                                            {
                                                var minStartingTime = aTime.StartingTime;
                                                if (minStartingTime > meTime)
                                                {
                                                    wl.CapacityAlocateX += (minStartingTime - meTime2).TotalMinutes.ToDecimal();
                                                    var capaLoad = new mh_CapacityLoad
                                                    {
                                                        Active = true,
                                                        CapacityX = (minStartingTime - meTime2).TotalMinutes.ToDecimal(),
                                                        Capacity = 0,
                                                        Date = tempStarting.Date,
                                                        DocId = idJob,
                                                        id = 0,
                                                        WorkCenterID = r.idWorkCenter,
                                                    };
                                                    capacityLoad.Add(capaLoad);
                                                    CapaUseX -= (minStartingTime - meTime2).TotalMinutes.ToDecimal();
                                                    AddC = true;
                                                }
                                                meTime2 = minStartingTime;
                                                t_meTime2 = aTime.EndingTime;
                                                tempcal.Remove(tempcal.FirstOrDefault());
                                            }

                                            if (AddC)
                                            {
                                                var cl = new mh_CalendarLoad
                                                {
                                                    id = autoid,
                                                    idRoute = r.id,
                                                    idWorkcenter = r.idWorkCenter,
                                                    idCal = idCalendar,
                                                    Date = tempStarting.Date,
                                                    StartingTime = meTime,
                                                    EndingTime = meTime2,
                                                    idJob = idJob,
                                                    idAbs = -1,
                                                };
                                                calLoad.Add(cl);
                                            }
                                            meTime = t_meTime2;
                                            if (isNull)
                                                break;
                                        }
                                    }
                                    else
                                    {//ไม่มี Calendar Load เลย
                                        if (wl.CapacityAfterX >= CapaUseX)
                                        {
                                            meTime2 = meTime.Add(TimeSpan.FromMinutes(CapaUseX.ToDouble()));
                                            wl.CapacityAlocateX += CapaUseX;
                                            var capaLoad = new mh_CapacityLoad
                                            {
                                                Active = true,
                                                CapacityX = CapaUseX,
                                                Capacity = 0,
                                                Date = tempStarting.Date,
                                                DocId = idJob,
                                                id = 0,
                                                WorkCenterID = r.idWorkCenter,
                                            };
                                            capacityLoad.Add(capaLoad);
                                            CapaUseX = 0;
                                            //CapaUse = 0;
                                            //wl.CapacityAlocate += CapaUse;
                                        }
                                        else //CapacityAfterX < CapaUseX
                                        {
                                            meTime2 = meTime.Add(TimeSpan.FromMinutes(wl.CapacityAfterX.ToDouble()));
                                            var capaLoad = new mh_CapacityLoad
                                            {
                                                Active = true,
                                                CapacityX = wl.CapacityAfterX,
                                                Capacity = 0,
                                                Date = tempStarting.Date,
                                                DocId = idJob,
                                                id = 0,
                                                WorkCenterID = r.idWorkCenter,
                                            };
                                            capacityLoad.Add(capaLoad);
                                            CapaUseX -= wl.CapacityAlocateX;
                                            wl.CapacityAlocateX = wl.CapacityAvailableX;
                                            //wl.CapacityAlocate += (wl.CapacityAlocate - CapaUse);
                                        }

                                        var cl = new mh_CalendarLoad
                                        {
                                            id = autoid,
                                            idRoute = r.id,
                                            idWorkcenter = r.idWorkCenter,
                                            idCal = idCalendar,
                                            Date = tempStarting.Date,
                                            StartingTime = meTime,
                                            EndingTime = meTime2,
                                            idJob = idJob,
                                            idAbs = -1,
                                        };
                                        calLoad.Add(cl);

                                    }
                                    t_EndingDate = tempStarting.Date.AddHours(meTime2.Hours).AddMinutes(meTime2.Minutes);
                                }
                            }
                            else
                            {
                                string mssg = "Work center not having Working days.!!!\n";
                                baseClass.Warning(mssg);
                                throw new Exception(mssg);
                            }

                            if (CapaUseX > 0)
                                tempStarting = tempStarting.AddDays(1).Date;
                        } while (CapaUseX > 0);

                        if (!firstStart)
                            StartingDate = t_StartingDate;
                        EndingDate = t_EndingDate;
                        firstStart = true;
                    }

                }


                //**Starting Date (Time) - Ending Date (Time)
                if (StartingDate != null && EndingDate != null)
                {
                    using (var db = new DataClasses1DataContext())
                    {
                        //ปรับ Starting - Ending ใน Job
                        var m = db.mh_ProductionOrders.Where(x => x.id == idJob).FirstOrDefault();
                        m.UpdateBy = Classlib.User;
                        m.UpdateDate = DateTime.Now;
                        m.StartingDate = StartingDate.Value;
                        m.EndingDate = EndingDate.Value;
                        m.CostOverhead = costOverHead;
                        m.CapacityUseX = capaUseX_All;
                        //ลบ Capacity เก่า
                        var capa = db.mh_CapacityLoads.Where(x => x.DocId == idJob).ToList();
                        db.mh_CapacityLoads.DeleteAllOnSubmit(capa);
                        //ลบ Calendar เก่า
                        var cal = db.mh_CalendarLoads.Where(x => x.idJob == idJob).ToList();
                        db.mh_CalendarLoads.DeleteAllOnSubmit(cal);
                        //ใส่ Capacity ใหม่
                        foreach (var c in capacityLoad)
                        {
                            if (c.id > 0) continue;

                            var cc = new mh_CapacityLoad
                            {
                                Active = true,
                                Capacity = c.Capacity,
                                CapacityX = c.CapacityX,
                                Date = c.Date,
                                DocId = idJob,//idJob
                                DocNo = JobNo,
                                WorkCenterID = c.WorkCenterID,
                            };
                            db.mh_CapacityLoads.InsertOnSubmit(cc);
                        }
                        //ใส่ Calendar ใหม่
                        foreach (var c in calLoad)
                        {
                            if (c.idAbs >= 0) continue;
                            if (c.idAbs == -99) continue; // ช่วงเวลา Break ระหว่างวัน ไม่ต้องบันทึก

                            var cc = new mh_CalendarLoad
                            {
                                Date = c.Date,
                                EndingTime = c.EndingTime,
                                idAbs = (c.idAbs >= 0) ? c.idAbs : 0,
                                idCal = c.idCal,
                                idHol = c.idHol,
                                idJob = idJob,
                                idRoute = c.idRoute,
                                idWorkcenter = c.idWorkcenter,
                                StartingTime = c.StartingTime,
                            };
                            db.mh_CalendarLoads.InsertOnSubmit(cc);
                        }

                        db.SubmitChanges();

                        dbClss.AddHistory(this.Name, "Job Order Sheet", $"Recalculate Job {txtStartingDate.Text}-{txtEndingDate.Text} to {StartingDate.Value.ToDtTimeString()}-{EndingDate.Value.ToDtTimeString()}", JobNo);
                        txtStartingDate.Text = StartingDate.Value.ToDtTimeString();
                        txtEndingDate.Text = EndingDate.Value.ToDtTimeString();
                    }
                }
            }
            catch (Exception ex)
            {
                baseClass.Error(ex.Message);
            }
        }

    }
}
