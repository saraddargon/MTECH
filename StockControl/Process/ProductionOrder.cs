﻿using System;
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
            HistoryView hw = new HistoryView(this.Name, txtJobNo.Text);
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
                        txtRev.Text = t.Rev;
                        txtRefDocNo.Text = t.RefDocNo;
                        txtRefDocNo_TEMP.Text = t.RefDocNo_TEMP;
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
                        cbCloseJob.Checked = t.CloseJob;
                        if (cbHoldJob.Checked)
                        {
                            btnHoldJob.Text = "Unhold Job";
                            btnRecal.Enabled = false;
                        }
                        else
                        {
                            btnHoldJob.Text = "Hold Job";
                            btnRecal.Enabled = true;
                        }
                        txtidJob.Text = t.id.ToSt();
                        txtSeqStatus.Text = t.SeqStatus.ToSt();

                        //dt
                        var dts = db.mh_ProductionOrderRMs.Where(x => x.JobNo == t.JobNo && x.Active).ToList();
                        foreach (var dt in dts)
                        {
                            var outQ = dt.OutQty;
                            var accdQ = baseClass.GetQtyAccidenSlip(dt.id);
                            outQ += accdQ;

                            var rm2 = db.mh_ProductionOrderRM_2s.Where(x => x.Active && x.idProductionOrderRM == dt.id).ToList();
                            if (rm2.Count > 0)
                                outQ -= rm2.Sum(x => x.Qty);

                            addRow(dt.id, dt.ItemNo, dt.ItemName, dt.Qty, dt.UOM, dt.PCSUnit
                                , outQ, dt.GroupType, dt.Type, dt.InvGroup);
                        }

                        if (t.CloseJob)
                            txtStatus.Text = "Completed";
                        else if (txtFGQty.Value.ToDecimal() != txtOutQty.Value.ToDecimal()
                            || dgvData.Rows.Where(x => x.Cells["Shipped"].Value.ToDecimal() > 0).Count() > 0)
                            txtStatus.Text = "Process";
                        else if (txtSeqStatus.Text.ToInt() == 2)
                            txtStatus.Text = "Approved";
                        else if (txtSeqStatus.Text.ToInt() == 1)
                            txtStatus.Text = "Waiting Approve";
                        else
                            txtStatus.Text = "Waiting";

                        if (t.CloseJob || txtSeqStatus.Text.ToInt() > 0 || lblStatus.Text == "Process")
                        {
                            btnRecal.Enabled = false;
                            btnHoldJob.Enabled = false;
                        }
                        else
                        {
                            btnRecal.Enabled = true;
                            btnHoldJob.Enabled = true;
                        }

                        //****Load Pr in Job
                        LoadPRwithJob(t.RefDocId);
                        //****Load Shipping History
                        var ship1 = db.tb_Shippings.Where(x => x.idCSTMPODt == t.RefDocId && x.Status != "Cancel" && x.ShipType == "ForJob").ToList();
                        foreach (var s in ship1)
                        {
                            var shipH = db.tb_ShippingHs.Where(x => x.ShippingNo == s.ShippingNo && x.Status != "Cancel").FirstOrDefault();
                            if (shipH == null) continue;
                            var tool = db.mh_Items.Where(x => x.InternalNo == s.CodeNo).FirstOrDefault();
                            if (tool == null) continue;
                            addRow2(s.id, s.CodeNo, tool.InternalName, s.QTY.Value, s.UnitShip, Math.Round(s.PCSUnit.ToDecimal(), 2)
                                , tool.GroupType, tool.Type, tool.InventoryGroup, s.ShippingNo
                                , shipH.ShipDate.Value.Date, s.Status);
                        }
                        //Load Accident Slip --- None
                        var acc = db.mh_Accident_SlipHs.Where(x => x.JobCard == t_JobNo && x.Status != "Cancel")
                            .Join(db.mh_Accident_Slips.Where(x => x.Status == "Completed")
                            , hd => hd.DocNo
                            , dt => dt.DocNo
                            , (hd, dt) => new { hd, dt }).ToList();
                        foreach (var s in acc)
                        {
                            var tool = db.mh_Items.Where(x => x.InternalNo == s.dt.CodeNo).FirstOrDefault();
                            if (tool == null) continue;
                            var qX = (s.hd.Type == "None") ? -1.00m : 1.00m;
                            addRow2(s.dt.id, s.dt.CodeNo, s.dt.ItemNo, qX * s.dt.QTY, s.dt.UnitShip, s.dt.PCSUnit,
                                tool.GroupType, tool.Type, tool.InventoryGroup, s.dt.DocNo
                                , s.hd.DocDate.Value.Date, s.dt.Status);
                        }
                        //Load Return RM
                        var st = db.tb_StockAdjustHs.Where(x => x.Status == "Completed" && x.ADNo.Substring(0, 2) == "RT")
                            .Join(db.tb_StockAdjusts.Where(x => x.Status == "Completed" && x.RefJobCard == t_JobNo)
                            , hd => hd.ADNo
                            , dt => dt.AdjustNo
                            , (hd, dt) => new { hd, dt }).ToList();
                        foreach (var s in st)
                        {
                            var tool = db.mh_Items.Where(x => x.InternalNo == s.dt.CodeNo).FirstOrDefault();
                            if (tool == null) continue;
                            addRow2(s.dt.id, s.dt.CodeNo, s.dt.ItemNo, -1 * s.dt.Qty.ToDecimal(), s.dt.Unit, s.dt.PCSUnit.ToDecimal(),
                                tool.GroupType, tool.Type, tool.InventoryGroup, s.dt.AdjustNo
                                , s.hd.ADDate.Value.Date, s.dt.Status);
                        }
                        dgvShipHistory.Columns["ShipDate"].SortOrder = RadSortOrder.Ascending;

                        //****Load Receive FG
                        //Receive From Packing
                        var pkList = db.mh_PackingDts.Where(x => x.Active && x.idJob == t.id)
                            .Join(db.mh_Packings
                            , dt => dt.PackingNo
                            , hd => hd.PackingNo
                            , (dt, hd) => new { hd, dt }).ToList();
                        foreach (var pk in pkList)
                        {
                            string StatusS = (pk.hd.Active) ? "Completed" : "Cancel";
                            addRow3(pk.dt.id.ToSt(), "รับจากใบผลิต", pk.hd.PackingNo, pk.dt.Qty
                                , pk.dt.UOM, pk.dt.PCSUnit, pk.hd.PackingDate, pk.hd.CreateBy
                                , StatusS);
                        }
                        //Cancel FG Q'ty from Sale document
                        var pdCancel = db.mh_ProductionOrder_CancelQties.Where(x => x.Active && x.JobNo == t_JobNo).ToList();
                        foreach (var pd in pdCancel)
                        {
                            string SS = (pd.SeqStatus == 0) ? "Waiting" : (pd.SeqStatus == 1) ? "Waiting Approve" : "Completed";
                            addRow3(pd.DocNo, "เอกสารการเปลี่ยนแปลง", pd.DocNo, pd.Qty, pd.UOM, pd.PCSUnit, pd.DocDate, pd.CreateBy, SS);
                        }
                        ////Close Job Special
                        //var pdClose = db.mh_ProductionOrder_CloseSpecials.Where(x => x.Active && x.JobNo == t_JobNo).ToList();
                        //foreach (var pd in pdClose)
                        //{
                        //    addRow3(pd.DocNo, "Close Job (Special)", pd.DocNo, pd.Qty, pd.UOM, pd.PCSUnit, pd.CreateDate, pd.CreateBy, "Completed");
                        //}
                        dgvReceiveFG.Columns["ReceiveDate"].SortOrder = RadSortOrder.Ascending;


                        SetRowNo1(dgvData);
                        SetRowNo1(dgvPurchase);
                        SetRowNo1(dgvShipHistory);
                        SetRowNo1(dgvReceiveFG);
                        btnView_Click(null, null);
                        txtJobNo.ReadOnly = true;
                    }
                    else if (warningMssg)
                        baseClass.Warning("ไม่พบเอกสาร Job Order Sheet.!!");
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
        private void addRow2(int id, string ItemNo, string ItemName, decimal Qty, string UOM
            , decimal PCSUnit, string GroupType, string Type, string InvGroup
            , string RefNo, DateTime ShipDate, string Status)
        {
            var rowe = dgvShipHistory.Rows.AddNew();
            rowe.Cells["RNo"].Value = rowe.Index + 1;
            rowe.Cells["ItemNo"].Value = ItemNo;
            rowe.Cells["ItemName"].Value = ItemName;
            rowe.Cells["Qty"].Value = Qty;
            rowe.Cells["UOM"].Value = UOM;
            rowe.Cells["PCSUnit"].Value = PCSUnit;
            rowe.Cells["id"].Value = id;
            rowe.Cells["GroupType"].Value = GroupType;
            rowe.Cells["Type"].Value = Type;
            rowe.Cells["InvGroup"].Value = InvGroup;
            rowe.Cells["RefNo"].Value = RefNo;
            rowe.Cells["ShipDate"].Value = ShipDate;
            rowe.Cells["Status"].Value = Status;
        }
        private void addRow3(string id, string ReceiveType, string RefNo, decimal Qty, string UOM, decimal PCSUnit
            , DateTime ReceiveDate, string ReceiveBy, string Status)
        {
            var rowe = dgvReceiveFG.Rows.AddNew();
            rowe.Cells["id"].Value = id;
            rowe.Cells["ReceiveType"].Value = ReceiveType;
            rowe.Cells["RefNo"].Value = RefNo;
            rowe.Cells["Qty"].Value = Qty;
            rowe.Cells["UOM"].Value = UOM;
            rowe.Cells["PCSUnit"].Value = PCSUnit;
            rowe.Cells["ReceiveDate"].Value = ReceiveDate;
            rowe.Cells["ReceiveBy"].Value = ReceiveBy;
            rowe.Cells["Status"].Value = Status;
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
            txtRev.Text = "";
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
            cbCloseJob.Checked = false;
            txtStatus.Text = "Waiting";
            txtidJob.Text = "";
            txtSeqStatus.Text = "";
            txtJobNo.ReadOnly = false;

            //using (var db = new DataClasses1DataContext())
            //{
            //    var d = DateTime.Now.Date;
            //    var m = db.mh_LotFGs.Where(x => x.LotDate == d).FirstOrDefault();
            //    if (m != null)
            //        txtLotNo.Text = m.LotNo;
            //}
            //

            dgvData.DataSource = null;
            dgvData.Rows.Clear();
            dgvPurchase.DataSource = null;
            dgvPurchase.Rows.Clear();
            dgvShipHistory.DataSource = null;
            dgvShipHistory.Rows.Clear();
            dgvReceiveFG.DataSource = null;
            dgvReceiveFG.Rows.Clear();

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

            txtRev.ReadOnly = true;
            txtFGQty.ReadOnly = true;
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

            txtRev.ReadOnly = false;
            txtFGQty.ReadOnly = false;
            //dgvData.ReadOnly = false;

            Enable_Status(true, "Edit");
            lblStatus.Text = "Edit";
            Ac = "Edit";

            txtRev.Focus();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtFGNo.Text == "") return;
            if (cbCloseJob.Checked)
            {
                baseClass.Warning("- ไม่สามารถลบได้เนื่องจากสถานะเป็น 'Completed'.\n");
                return;
            }
            else if (Math.Round(txtFGQty.Value.ToDecimal(), 2) != txtOutQty.Value.ToDecimal()
                            || dgvData.Rows.Where(x => x.Cells["Shipped"].Value.ToDecimal() > 0).Count() > 0)
            {
                baseClass.Warning("- ไม่สามารถลบได้เนื่องจากสถานะเป็น 'Process'.\n");
                return;
            }
            else if (txtSeqStatus.Text.ToInt() > 0)
            {
                string StatusNow = (txtSeqStatus.Text.ToInt() == 1) ? "Waiting Approve" : "Approved";
                baseClass.Warning($"- ไม่สามารถลบได้เนื่องจากสถานะเป็น '{StatusNow}'.\n");
                return;
            }

            if (dgvData.Rows.Where(x => x.Cells["Qty"].Value.ToDecimal() != x.Cells["OutShip"].Value.ToDecimal()).Count() > 0)
            {
                baseClass.Warning("- RM หรือ SEMI มีการเบิกใช้สำหรับใน Job ดังกล่าวแล้ว, ไม่สามารถลบได้.\n");
                return;
            }

            if (baseClass.IsDel("ต้องการ 'ลบ' ?"))
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

                        //var po = db.mh_CustomerPODTs.Where(x => x.id == m.RefDocId).FirstOrDefault();
                        //if (po != null)
                        //{
                        //    //po.OutPlan += (m.Qty * m.PCSUnit) - m.OutQty;
                        //    po.OutPlan = Math.Round(m.Qty, 2); //Full Return Qty
                        //    po.Status = baseClass.setCustomerPOStatus(po);
                        //    db.SubmitChanges();

                        //}

                        var so = db.mh_SaleOrderDTs.Where(x => x.id == m.RefDocId).FirstOrDefault();
                        if (so != null)
                        {
                            //so.OutPlan += (m.Qty + m.PCSUnit) - m.OutQty;
                            so.OutPlan = Math.Round(m.Qty, 2);//Full Return Qty;
                            db.SubmitChanges();
                            //remove CustomerP/O OutPlan
                            if (so.RefId > 0)
                            {
                                var po = db.mh_CustomerPODTs.Where(x => x.id == so.RefId).FirstOrDefault();
                                if (po != null) //Sale Order เปิดจาก Customer P/O
                                {
                                    po.OutPlan = Math.Round(po.Qty * po.PCSUnit, 2);
                                    var soAll = db.mh_SaleOrderDTs.Where(x => x.Active && x.RefId == po.id)
                                        .Join(db.mh_SaleOrders.Where(x => x.Active)
                                        , dt => dt.SONo
                                        , hd => hd.SONo
                                        , (dt, hd) => new { hd, dt }).ToList();
                                    foreach (var s in soAll)
                                    {
                                        var q = Math.Round(s.dt.Qty * s.dt.PCSUnit, 2) - s.dt.OutPlan;
                                        po.OutPlan -= q;
                                        po.Status = baseClass.setCustomerPOStatus(po);
                                    }
                                    if (soAll.Count > 0)
                                        db.SubmitChanges();
                                }//Customer P/O not null
                            }

                            //remove capa
                            int idJob = txtidJob.Text.ToInt();
                            var capa = db.mh_CapacityLoads.Where(x => x.DocId == idJob).ToList();
                            db.mh_CapacityLoads.DeleteAllOnSubmit(capa);
                            //remove calendar
                            var cal = db.mh_CalendarLoads.Where(x => x.idJob == idJob).ToList();
                            db.mh_CalendarLoads.DeleteAllOnSubmit(cal);

                            dbClss.Delete_ApproveList(jobNo);

                            db.SubmitChanges();
                            AddHistory($"Remove Job Order Sheet {m.JobNo}", m.JobNo);

                            btnSave.Enabled = false;
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
                ////if (txtCodeNo.Text.Equals(""))
                ////    err += " “รหัสพาร์ท:” เป็นค่าว่าง \n";

                //if (txtFGQty.Value.ToDecimal() != txtOutQty.Value.ToDecimal())
                //    err += "- ไม่สามารถบันทึกได้เนื่องจากสถานะเป็น 'Process'.\n";
                ////else if (txtStatus.Text == "Approved")
                ////    err += "- Cannot Save because Status is 'Approved'.\n";
                //else if (txtSeqStatus.Text.ToInt() > 0)
                //{
                //    string StatusNow = (txtSeqStatus.Text.ToInt() == 1) ? "Waiting Approve" : "Approved";
                //    baseClass.Warning($"- ไม่สามารถบันทึกได้เนื่องจากสถานะเป็น '{StatusNow}'.\n");
                //}

                var allPk = dgvReceiveFG.Rows.Where(x => x.Cells["ReceiveType"].Value.ToSt() == "รับจากใบผลิต").ToList();
                if (allPk.Count > 0)
                {
                    var rcQ = dgvReceiveFG.Rows.Where(x => x.Cells["ReceiveType"].Value.ToSt() == "รับจากใบผลิต").Sum(x => x.Cells["Qty"].Value.ToDecimal());
                    if (txtFGQty.Value.ToDecimal() < rcQ)
                    {
                        err += "- ไม่สามารถแก้ไขได้ เนื่องจาก 'จำนวน FG Q'ty' น้อยกว่า 'จำนวนที่รับจากใบผลิต'.\n";
                    }
                }

                if (!err.Equals(""))
                    baseClass.Warning(err);
                else
                    re = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("JobOrderSheet", ex.Message, this.Name);
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
                        SaveRev();
                    //SaveE();
                }
                else
                    MessageBox.Show("ไม่สามารถบันทึกได้เนื่องจากสถานะไม่ใช่ 'New' หรือ 'Edit'");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        void SaveRev()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    string jobNo = txtJobNo.Text.Trim();
                    var m = db.mh_ProductionOrders.Where(x => x.JobNo == jobNo).FirstOrDefault();
                    if (m != null)
                    {
                        if (m.Rev != txtRev.Text.Trim()) AddHistory($"Revision from {m.Rev} to {txtRev.Text.Trim()}", m.JobNo);
                        m.Rev = txtRev.Text.Trim(); //change reversion
                        if (m.Qty != txtFGQty.Value.ToDecimal())
                        {
                            //change FG Qty
                            AddHistory($"FG Q'ty from {m.Qty} to {txtFGQty.Value.ToDecimal()}", m.JobNo);
                            var oldQ = m.Qty;
                            var newQ = txtFGQty.Value.ToDecimal();
                            var outQ = m.OutQty;
                            var recQ = 0.00m;// oldQ - outQ;
                            if (dgvReceiveFG.Rows.Where(x => x.Cells["ReceiveType"].Value.ToSt() == "รับจากใบผลิต").Count() > 0)
                                recQ = dgvReceiveFG.Rows.Where(x => x.Cells["ReceiveType"].Value.ToSt() == "รับจากใบผลิต" && x.Cells["Status"].Value.ToSt() != "Cancel")
                                    .Sum(x => x.Cells["Qty"].Value.ToDecimal());
                            m.Qty = newQ;
                            m.OutQty = newQ - recQ;
                            var so = db.mh_SaleOrderDTs.Where(x => x.id == m.RefDocId).FirstOrDefault();
                            if (so != null)
                                so.OutQty = m.OutQty;
                            if (m.OutQty <= 0)
                            {
                                if (m.CloseJob != true) AddHistory($"Close Job Order Sheet", m.JobNo);
                                m.CloseJob = true;
                            }
                            else
                            {
                                if (m.CloseJob != false) AddHistory($"Cancel Close Job Order Sheet", m.JobNo);
                                m.CloseJob = false;
                            }
                        }
                        m.UpdateDate = DateTime.Now;
                        m.UpdateBy = ClassLib.Classlib.User;
                        db.SubmitChanges();

                        var docno = jobNo;
                        baseClass.Info("Save complete(s).");
                        ClearData();
                        t_JobNo = docno;
                        DataLoad();
                    }
                    else
                    {
                        baseClass.Warning("Job Order Sheet not found.\n");
                        btnView_Click(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                baseClass.Error(ex.Message);
            }
            finally { this.Cursor = Cursors.Default; }
        }
        void SaveE()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    string jobNo = (txtidJob.Text.ToInt() > 0) ? txtJobNo.Text : "";
                    bool newJob = false;
                    //Hd
                    bool newDoc = false;
                    var m = db.mh_ProductionOrders.Where(x => x.JobNo == jobNo).FirstOrDefault();
                    if (m == null)
                    {
                        newDoc = true;
                        m = new mh_ProductionOrder();
                        m.CloseJob = false;
                        m.CreateBy = ClassLib.Classlib.User;
                        m.CreateDate = DateTime.Now;
                        m.JobDate = DateTime.Now.Date;
                        m.JobNo = dbClss.GetNo(29, 2);
                        m.Rev = "0";
                        m.SeqStatus = 0;
                        db.mh_ProductionOrders.InsertOnSubmit(m);
                        newJob = true;

                        AddHistory($"New Job Order Sheet {m.JobNo}", m.JobNo);
                    }
                    //history
                    if (!newDoc)
                    {
                        string fgNo = txtFGNo.Text.Trim();
                        if (m.FGNo != fgNo) AddHistory($"FG from {m.FGNo} to {fgNo}", m.JobNo);
                        if (m.FGName != txtFGName.Text) AddHistory($" {m.FGName} to {txtFGName.Text}", m.JobNo);
                        if (m.LotNo != txtLotNo.Text) AddHistory($" {m.LotNo} to {txtLotNo.Text}", m.JobNo);
                        if (m.Qty != txtFGQty.Value.ToDecimal()) AddHistory($" {m.Qty} to {txtFGQty.Value.ToDecimal()}", m.JobNo);
                        if (m.PCSUnit != txtPCSUnit.Value.ToDecimal()) AddHistory($" {m.PCSUnit} to {txtPCSUnit.Value.ToDecimal()}", m.JobNo);
                        if (m.UOM != txtUOM.Text) AddHistory($" {m.UOM} to {txtUOM.Text}", m.JobNo);
                        if (m.ReqDate != txtReqDate.Text.ToDateTime().Value) AddHistory($" {m.ReqDate} to {txtReqDate.Text.ToDateTime().Value}", m.JobNo);
                        if (m.EndingDate != txtEndingDate.Text.ToDateTime().Value) AddHistory($"Ending Date from {m.EndingDate} to {txtEndingDate.Text.ToDateTime().Value}", m.JobNo);
                        if (m.Rev != txtRev.Text.Trim()) AddHistory($"Revision from {m.Rev} to {txtRev.Text.Trim()}", m.JobNo);
                    }
                    m.Rev = txtRev.Text;
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

                    db.sp_062_mh_ApproveList_Add(m.JobNo, "Job Req", ClassLib.Classlib.User);

                    //update Customer P/O [Only New Job] - Out Plan
                    if (newJob)
                    {
                        dbClss.AddHistory(this.Name, "Job Order Sheet", $"New Job Order Sheet : {m.JobNo}", m.JobNo);
                        //var po = db.mh_CustomerPODTs.Where(x => x.id == m.RefDocId).FirstOrDefault();
                        //if (po != null)
                        //{
                        //    var pohd = db.mh_CustomerPOs.Where(x => x.id == po.idCustomerPO).FirstOrDefault();
                        //    string pono = (pohd != null) ? pohd.CustomerPONo : "";
                        //    dbClss.AddHistory("CustomerPO", "Customer P/O", $"New job Order Sheet : {m.JobNo} for P/O {pohd.CustomerPONo}", pohd.CustomerPONo);
                        //    //po.OutPlan -= m.OutQty;
                        //    po.OutPlan = 0;//Full Ref Customer P/O
                        //    po.Status = baseClass.setCustomerPOStatus(po);
                        //    db.SubmitChanges();
                        //}
                        var so = db.mh_SaleOrderDTs.Where(x => x.id == m.RefDocId).FirstOrDefault();
                        if (so != null)
                        {
                            var sohd = db.mh_SaleOrders.Where(x => x.SONo == so.SONo).FirstOrDefault();
                            string sono = so.SONo;
                            //so.OutPlan -= m.OutQty;
                            so.OutPlan = 0;
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
            dgvPurchase.EnableFiltering = true;
            dgvShipHistory.EnableFiltering = true;
        }

        private void btnUnfilter1_Click(object sender, EventArgs e)
        {
            dgvData.EnableFiltering = false;
            dgvPurchase.EnableFiltering = false;
            dgvShipHistory.EnableFiltering = false;
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
        public void SetRowNo1(RadGridView Grid)//เลขลำดับ
        {
            int i = 1;

            if (Grid.Name == dgvReceiveFG.Name)
            {
                Grid.Rows.OrderBy(x => x.Cells["ReceiveDate"].Value.ToDateTime().Value).Where(o => o.IsVisible).ToList().ForEach(o =>
                  {
                      o.Cells["RNo"].Value = i;
                      i++;
                  });
            }
            else if (Grid.Name == dgvShipHistory.Name)
            {
                Grid.Rows.OrderBy(x => x.Cells["ShipDate"].Value.ToDateTime().Value).Where(o => o.IsVisible).ToList().ForEach(o =>
                {
                    o.Cells["RNo"].Value = i;
                    i++;
                });
            }
            else
            {
                Grid.Rows.Where(o => o.IsVisible).ToList().ForEach(o =>
                {
                    o.Cells["RNo"].Value = i;
                    i++;
                });
            }
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
                var pd = new ProductionOrder_List(2);
                this.Cursor = Cursors.Default;
                pd.ShowDialog();
                if (pd.t_JobNo != "")
                {
                    this.t_JobNo = pd.t_JobNo;
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
            var tj = t_JobNo;
            ClearData();
            Enable_Status(false, "View");
            t_JobNo = tj;
            DataLoad();
            Ac = "View";
        }


        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                string JobNo = txtJobNo.Text.Trim();
                //if (JobNo != "")
                //{
                    Report.Reportx1.Value = new string[1];
                    Report.Reportx1.Value[0] = JobNo;
                    Report.Reportx1.WReport = "JOBNoList";
                    Report.Reportx1 op = new Report.Reportx1("JobOrderSheet3.rpt");
                    op.Show();
                //}
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
                mssg += "- กรุณา บันทึก ข้อมูลก่อนการ Recalculate Job.\n";
            if (txtSeqStatus.Text.ToInt() > 0)
                mssg += "- สถานะไม่สามารถ Recalculate Job ได้.\n";
            if (txtFGQty.Value.ToDecimal() != txtOutQty.Value.ToDecimal())
                mssg += "- ไม่สามารถ Recalculate Job ได้เนื่องจาก FG ถูกเบิกใช้งานแล้ว.\n";
            if (cbCloseJob.Checked)
                mssg += "- ไม่สามารถ Recalculate Job ได้ เนื่องจากสถานะ Completed แล้ว.\n";

            if (mssg != "")
            {
                baseClass.Warning(mssg);
                ret = false;
            }
            return ret;
        }
        private void btnRecal_Click(object sender, EventArgs e)
        {
            if (chkRecalE() && baseClass.Question("ต้องการ 'Recalculate Job (Capacity)' ?"))
                Recal();
        }
        void Recal()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //baseClass.Info("Comming soon...");
                //MoveJobToLast();
                MoveJobToList_New(true);
                AddHistory($"Recalculate Job Order Sheet {txtJobNo.Text}", txtJobNo.Text);
                baseClass.Info("Recalculate completed.");
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
                mssg += "- กรุณา บันทึก ก่อน Hold Job.\n";
            if (txtSeqStatus.Text.ToInt() > 0)
                mssg += "- สถานะไม่สามารถ Hold Job ได้.\n";
            if (txtFGQty.Value.ToDecimal() != txtOutQty.Value.ToDecimal())
                mssg += "- ไม่สามารถ Hold Job ได้เนื่องจาก FG ถูกเบิกใช้ไปแล้ว.\n";
            if (cbCloseJob.Checked)
                mssg += "- ไม่สามารถ Hold Job ได้เนื่องจากสถานะเป็น Completed.\n";

            if (mssg != "")
            {
                ret = false;
                baseClass.Warning(mssg);
            }
            return ret;
        }
        private void btnHoldJob_Click(object sender, EventArgs e)
        {
            if (ChkHoldJob() && baseClass.Question("ต้องการ 'Hold Job' ?"))
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
                            AddHistory($"Hold Job Order Sheet {txtJobNo.Text}", txtJobNo.Text);
                            //Remove Capacity
                            var capaList = db.mh_CapacityLoads.Where(x => x.DocId == m.id).ToList();
                            db.mh_CapacityLoads.DeleteAllOnSubmit(capaList);
                            //Remove Calandar
                            var calList = db.mh_CalendarLoads.Where(x => x.idJob == m.id).ToList();
                            db.mh_CalendarLoads.DeleteAllOnSubmit(calList);
                            db.SubmitChanges();
                        }
                        else
                        {
                            m.HoldJob = false;
                            db.SubmitChanges();
                            AddHistory($"Unhold Job Order Sheet {txtJobNo.Text}", txtJobNo.Text);
                            //หาว่าวันที่ Unhold Job มากกว่าหรือเท่ากับ StartingDate ไหม ถ้าใช่ให้ move Capacity ไปไว้ท้ายสุดเลย
                            //if (DateTime.Now.Date >= txtStartingDate.Text.ToDateTime().Value.Date)
                            //{
                            //    MoveJobToLast();
                            //}
                            //MoveJobToLast();
                            MoveJobToList_New();
                        }

                        cbHoldJob.Checked = m.HoldJob;
                        cbCloseJob.Checked = m.CloseJob;

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

        void MoveJobToLast(bool deleteOld)
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
                using (var db = new DataClasses1DataContext())
                {
                    var tdata = new ItemData(txtFGNo.Text);

                    var maxDateJob = db.mh_ProductionOrders.Where(x => x.Active && x.JobNo != txtJobNo.Text).Max(x => x.EndingDate);
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
                    DateTime? finalStartingDate = null; //วันเริ่มงานจริงๆ
                    DateTime? finalEndingDate = null; //วันสิ้นสุด

                    DateTime? tempStarting = maxDateJob;
                    var rt = db.mh_RoutingDTs.Where(x => x.RoutingId == tdata.Routeid && x.Active)
                        .Join(db.mh_WorkCenters.Where(x => x.Active)
                        , hd => hd.idWorkCenter
                        , workcenter => workcenter.id
                        , (hd, workcenter)
                        => new { hd, hd.idWorkCenter, hd.id, hd.SetupTime, hd.RunTime, hd.WaitTime, workcenter })
                        .ToList();
                    //foreach (var r in rt)
                    //{
                    //    //cal capa
                    //    if (workLoads.Where(x => x.idWorkCenter == r.idWorkCenter).Count() < 1)
                    //    {
                    //        var wls = baseClass.getWorkLoad(dFrom.Date, dTo.Date, r.idWorkCenter);
                    //    }
                    //}
                    //
                    decimal FGQty = txtFGQty.Text.ToDecimal();
                    decimal FGPCSUnit = txtPCSUnit.Text.ToDecimal();
                    decimal UseQty = Math.Round(FGQty * FGPCSUnit, 2);
                    foreach (var r in rt)
                    {
                        int idWorkCenter = r.idWorkCenter;
                        //decimal CapacityOfWorkCenter = r.workcenter.Capacity;
                        var totalCapa_All = 0.00m;
                        var SetupTime = r.SetupTime * manuTime; //แปลงเป็นนาทีเสมอ
                        var RunTime = r.RunTime * manuTime;
                        var RunTimeCapa = Math.Round(((RunTime * UseQty) / r.workcenter.Capacity), 2);
                        var WaitingTime = r.WaitTime * manuTime;
                        totalCapa_All = SetupTime + RunTimeCapa + r.WaitTime;
                        var CapaUseX = 0.00m;
                        var CapaUse = 0.00m; //Capacity ไม่รวม WaitTime
                        CapaUseX = totalCapa_All; //เวลาการทำงานที่ถูกใช้ทั้งหมดใน Workcenter นี้
                        CapaUse = SetupTime + Math.Round(RunTime * UseQty, 2);

                        //หาว่า Capacity จากต้องใช้ Starting - Ending ใด โดยวนไปจนกว่า CapaUseX จะเป็น 0
                        do
                        {
                            //หาวันที่เริ่มต้นที่สามารถจะใช้ได้จาก mh_CapacityAvailable join mh_CapacityLoad
                            WorkLoad wl = null;
                            if (workLoads.Count == 0)
                            {
                                var w = baseClass.getWorkLoad_From(tempStarting.Value.Date, idWorkCenter);
                                if (w == null) //ไม่มีจริงๆ แสดงว่า Capacity หมด ต้องกลับไปคำนวนใหม่
                                {
                                    string mssg = "Capacity is not available, Please check Capacity Work load on Capacity Calculation (Work Centers).!!!\n";
                                    baseClass.Warning(mssg);
                                    throw new Exception(mssg);
                                }
                                else // เจอแล้ว Add ใส่ list ไว้เช็คในรอบถัดไป
                                {
                                    workLoads.Add(w);
                                    wl = w;
                                }
                            }
                            else
                            {
                                while (wl == null)
                                {
                                    if (workLoads.Where(x => x.Date == tempStarting.Value.Date && x.CapacityAfterX <= 0 && x.idWorkCenter == idWorkCenter).Count() > 0)
                                        tempStarting = tempStarting.Value.Date.AddDays(1);
                                    else
                                    {
                                        wl = workLoads.Where(x => x.Date >= tempStarting.Value.Date && x.CapacityAfterX > 0
                                            && x.idWorkCenter == idWorkCenter).OrderBy(x => x.Date).FirstOrDefault();
                                        if (wl == null) //ถ้า WorkLoad เป็น Null ให้ไปหาใหม่จาก Db ตรงๆ
                                        {
                                            //var w = baseClass.getWorkLoad(tempStarting.Value.Date, null, idWorkCenter).Where(x => x.CapacityAfterX > 0).FirstOrDefault();
                                            var w = baseClass.getWorkLoad_From(tempStarting.Value.Date, idWorkCenter);
                                            if (w == null) //ไม่มีจริงๆ แสดงว่า Capacity หมด ต้องกลับไปคำนวนใหม่
                                            {
                                                string mssg = "Capacity is not available, Please check Capacity Work load on Capacity Calculation (Work Centers).!!!\n";
                                                baseClass.Warning(mssg);
                                                throw new Exception(mssg);
                                            }
                                            else // เจอแล้ว Add ใส่ list ไว้เช็คในรอบถัดไป
                                            {
                                                if (workLoads.Where(x => x.Date == w.Date.Date && x.CapacityAfterX <= 0 && x.idWorkCenter == idWorkCenter).Count() > 0)
                                                {
                                                    tempStarting = tempStarting.Value.AddDays(1);
                                                    continue;
                                                }
                                                workLoads.Add(w);
                                                wl = w;
                                            }
                                        }
                                    }
                                }
                            }
                            //กรณีที่ วันที่หามาได้จาก WorkLoad มากกว่าวันปัจจุบันให้เปลี่ยนเป็นเริ่ม Start
                            if (wl.Date.Date > tempStarting.Value.Date)
                                tempStarting = wl.Date.Date;

                            int dow = baseClass.getDayOfWeek(tempStarting.Value.DayOfWeek);
                            //หาช่วงเวลาทำงานใน mh_WorkingDay
                            var wd = db.mh_WorkCenters.Where(x => x.id == wl.idWorkCenter)
                                .Join(db.mh_WorkingDays.Where(x => x.Day == dow && x.Active)
                                , hd => hd.Calendar
                                , dt => dt.idCalendar
                                , (hd, dt) => new
                                {
                                    hd,
                                    dt,
                                    StartingTime = baseClass.setTimeSpan(dt.StartingTime)
                                ,
                                    EndingTime = baseClass.setTimeSpan(dt.EndingTime)
                                }).ToList();
                            if (wd.Count > 0)
                            {
                                //เตรียมเวลาเริ่ม(sTime) - สิ้นสุด(eTime)
                                var sTime = wd.Min(x => x.StartingTime);
                                var eTime = wd.Max(x => x.EndingTime);

                                //ถ้าต่อจาก work ก่อนหน้าให้ใส่เวลา work ก่อนหน้าใน sTime
                                if (tempStarting != null && sTime < tempStarting.Value.TimeOfDay)
                                    sTime = tempStarting.Value.TimeOfDay;

                                int idCalendar = wd.First().hd.Calendar;
                                //หาวันลา(Abs), วันหยุดงาน(Holiday), ช่วงที่ Workcenter ทำงานไปแล้ว (Workcenter)
                                var calendars = calLoad.Where(x => x.Date == tempStarting.Value.Date
                                    && ((x.idCal == idCalendar && x.idWorkcenter == 0)
                                         || (x.idWorkcenter == idWorkCenter && x.idCal == idCalendar)
                                         || (x.idAbs > 0 && x.idWorkcenter == idWorkCenter)
                                        )
                                    ).OrderBy(x => x.StartingTime).ThenBy(x => x.EndingTime).ToList();
                                if (calendars.Count == 0) //ถ้าไม่มีให้ค้นจาก Db
                                {
                                    calendars = db.mh_CalendarLoads.Where(x => x.Date == tempStarting.Value.Date
                                    && ((x.idCal == idCalendar && x.idWorkcenter == 0)
                                         || (x.idWorkcenter == idWorkCenter && x.idCal == idCalendar)
                                         || (x.idAbs > 0 && x.idWorkcenter == idWorkCenter)
                                        )
                                    ).OrderBy(x => x.StartingTime).ThenBy(x => x.EndingTime).ToList();
                                    if (calendars.Count > 0) //ถ้าค้นเจอใน Db
                                        calLoad.AddRange(calendars);
                                }
                                //
                                //หาเวลาเริ่มต้น Starting Time
                                bool foundTime = false; //เจอเวลา Starting ที่ใช้ได้หรือป่าว
                                                        //หาว่าไปทับกับช่วงเวลาไหนไหม
                                if (calendars.Count > 0)
                                {
                                    var idCal = new List<int>();
                                    while (sTime < eTime) //วนจนกว่าจะหมดวัน
                                    {
                                        //หาว่าเวลาเริ่มต้นอยู่ในช่วงเวลาทำงานปกติไหม
                                        var ww = calendars.Where(x => !idCal.Any(q => q == x.id)
                                            && sTime >= x.StartingTime && sTime <= x.EndingTime).FirstOrDefault();
                                        if (ww != null)
                                        {
                                            sTime = ww.EndingTime;
                                            idCal.Add(ww.id);
                                        }
                                        else
                                        {
                                            if (sTime < eTime)
                                                foundTime = true;
                                            break;
                                        }
                                    }
                                }
                                else //ไม่มีวันลา วันหยุด หรือการทำงานที่เวลาดังกล่าวแสดงว่าใช้เวลานี้ได้
                                    foundTime = true;

                                //เช็คว่าเวลาที่หามาได้นั้นอยู่ในช่วงเวลาทำงานจริงๆ และไม่ใช่เวลา break
                                if (foundTime && wd.Where(x => sTime >= x.StartingTime && sTime <= x.EndingTime).Count() > 0) //อยู่ในช่วงเวลาทำงานแน่ๆ แต่ต้องเช็คว่าเป็นเวลาสุดท้ายของวันทำงานหรือป่าว
                                {
                                    if (sTime >= eTime) //เวลาที่หามาได้มีค่ามากกว่าหรือเท่ากับ เวลาสิ้นสุดการทำงานของวันนั้น ให้ข้ามไปวันถัดไป
                                    {
                                        tempStarting = tempStarting.Value.Date.AddDays(1);
                                        continue;
                                    }
                                    else if (wd.Where(x => sTime >= x.StartingTime && sTime == x.EndingTime).Count() > 0) //เวลาที่หามาได้เท่ากับเวลาสิ้นสุดของช่วงเวลานั้นไม่ได้ต้องปรับเป็นเวลาแรกที่ใกล้ที่สุดที่มากกว่า sTime
                                    {
                                        sTime = wd.Where(x => sTime < x.StartingTime).FirstOrDefault().StartingTime;
                                        tempStarting = tempStarting.Value.Date.SetTimeToDate(sTime);
                                    }
                                    else //เป็นช่วงเวลาที่ใช้ได้จริงๆ <<<<****>>>>
                                        tempStarting = tempStarting.Value.Date.SetTimeToDate(sTime);
                                    //tempStarting = tempStarting.Value.Date.AddHours(sTime.Hours).AddMinutes(sTime.Minutes).AddMilliseconds(sTime.Milliseconds);
                                }
                                else //ไม่อยู่ในช่วงเวลาทำงาน
                                {
                                    tempStarting = tempStarting.Value.Date.AddDays(1);
                                    continue;
                                }

                                //**ใส่ Starting Date
                                if (finalStartingDate == null)
                                    finalStartingDate = tempStarting.Value;


                                //*******************************
                                //หาเวลาสิ้นสุด EndingTime
                                foundTime = false;
                                var timeStart = tempStarting.Value.TimeOfDay; //เริ่มนับจากเวลาที่หาได้ใน Starting
                                var timeEnd = eTime;
                                int autoid = 0;

                                do
                                {
                                    var rd = new Random();
                                    autoid = rd.Next(-999999, -100);
                                } while (calLoad.Where(x => x.id == autoid).Count() > 0);

                                //ดึงปฏิทินเฉพาะเวลาที่มากกว่า timeStart
                                var calendars2 = calendars.Where(x => x.StartingTime >= timeStart).OrderBy(x => x.StartingTime).ToList();
                                if (calendars2.Count > 0) //หาว่าไปตรงกับปฏิทินวันลา, วันหยุด, วันที่ถูกทำงานไปแล้วหรือไม่
                                {
                                    var idCal = new List<int>();
                                    var t_timeEnd = timeEnd;
                                    while (wl.CapacityAfterX > 0) //วนไปจนกว่า Capacity Time ของวันนั้นจะหมด หรือ CapaUseX = 0
                                    {
                                        do
                                        {
                                            var rd = new Random();
                                            autoid = rd.Next(-999999, -100);
                                        } while (calLoad.Where(x => x.id == autoid).Count() > 0);

                                        //วนเวลาที่ถูกใช้ทีละอัน
                                        var aTime = calendars2.Where(x => !idCal.Any(q => q == x.id)).FirstOrDefault();
                                        if (aTime == null)
                                        {
                                            timeEnd = eTime;
                                            t_timeEnd = eTime;
                                        }
                                        else
                                        {
                                            timeEnd = aTime.StartingTime;
                                            t_timeEnd = aTime.EndingTime;
                                            idCal.Add(aTime.id);
                                        }

                                        //หาว่าเป็นเวลาที่อยู่ในช่วงเวลาทำงานไหม
                                        //ถ้าไม่ใช่ช่วงเวลาทำงาน
                                        if (wd.Where(x => timeStart >= x.StartingTime && timeStart <= x.EndingTime
                                                     && timeEnd >= x.StartingTime && timeEnd <= x.EndingTime).Count() < 1)
                                            timeEnd = wd.Where(x => timeStart >= x.StartingTime && timeStart <= x.EndingTime).First().EndingTime;

                                        //คำนวน Capacity Time
                                        decimal diffTime = (timeEnd - timeStart).TotalMinutes.ToDecimal();
                                        if (diffTime >= CapaUseX) //ถ้าเวลาที่หามาได้มากกว่าเวลาที่ ต้องใช้จริงๆ จะต้องปรับให้ เวลาสิ้นสุดเป็นเวลาจริง = CapaUseX
                                        {
                                            timeEnd = timeStart.Add(TimeSpan.FromMinutes(CapaUseX.ToDouble()));

                                            var capaLoad = baseClass.newCapaLoad(CapaUseX, CapaUse, tempStarting.Value.Date, idJob, 0, idWorkCenter);
                                            capacityLoad.Add(capaLoad);

                                            wl.CapacityAlocateX += CapaUseX;
                                            wl.CapacityAlocate += CapaUse;
                                            CapaUseX = 0;
                                            CapaUse = 0;

                                            var cl = baseClass.newCalendar(autoid, r.id, idWorkCenter, idCalendar, tempStarting.Value.Date, timeStart, timeEnd, idJob, -1);
                                            calLoad.Add(cl);
                                            break; //CapaUseX หมดแล้ว ออกได้เลย
                                        }
                                        else
                                        {
                                            wl.CapacityAlocateX += diffTime;
                                            wl.CapacityAlocate += diffTime;
                                            CapaUseX -= diffTime;
                                            CapaUse -= diffTime;
                                            var tCapa = diffTime;
                                            if (CapaUse < 0)
                                            {
                                                tCapa = tCapa + CapaUse; // a + (-b)
                                                CapaUse = 0;
                                            }
                                            var capaLoad = baseClass.newCapaLoad(diffTime, tCapa, tempStarting.Value.Date, idJob, 0, idWorkCenter);
                                            capacityLoad.Add(capaLoad);

                                            var cl = baseClass.newCalendar(autoid, r.id, idWorkCenter, idCalendar, tempStarting.Value.Date, timeStart, timeEnd, idJob, -1);
                                            calLoad.Add(cl);
                                        }

                                        timeStart = t_timeEnd;
                                    }
                                }
                                else //ไม่ตรงกับเวลาที่ถูกใช้งานใดๆ ให้ลด Capacity
                                {
                                    if (wl.CapacityAfterX >= CapaUseX) //กรณีที่เหลือเวลา Capacity ในวันนั้นเพียงพอ
                                    {
                                        timeEnd = timeStart.Add(TimeSpan.FromMinutes(CapaUseX.ToDouble()));
                                    }
                                    else //กรณีที่ในวันนั้น Capaciy ไม่เพียงพอ
                                    {
                                        timeEnd = eTime; //ตั้งเป็นเวลาสิ้นสุดทำงานได้เลย
                                    }

                                    //เช็คว่าเวลาที่หามาได้นั้นอยู่ในช่วงเวลาทำงานจริงๆ และไม่ใช่เวลา break
                                    if (wd.Where(x => timeStart >= x.StartingTime && timeEnd <= x.EndingTime).Count() < 1) //ไม่อยู่ในช่วงเวลาทำงานแน่ๆ
                                    {
                                        //ต้องหาว่าอยู่เกินช่วงไหนโดยเอาเวลา Start ไปหา
                                        var f = wd.Where(x => timeStart >= x.StartingTime && timeStart <= x.EndingTime).FirstOrDefault();
                                        timeEnd = f.EndingTime;

                                        var diffTime = (timeEnd - timeStart).TotalMinutes.ToDecimal();

                                        wl.CapacityAlocateX += diffTime;
                                        wl.CapacityAlocate += diffTime;
                                        CapaUseX -= diffTime;
                                        CapaUse -= diffTime;
                                        var tCapa = diffTime;
                                        if (CapaUse < 0)
                                        {
                                            tCapa = tCapa + CapaUse; // a + (-b)
                                            CapaUse = 0;
                                        }
                                        foundTime = true;

                                        var capaload = baseClass.newCapaLoad(diffTime, tCapa, tempStarting.Value.Date, idJob, 0, idWorkCenter);
                                        capacityLoad.Add(capaload);
                                    }
                                    else
                                    {
                                        var diffTime = (timeEnd - timeStart).TotalMinutes.ToDecimal();

                                        //wl.CapacityAlocateX += CapaUseX;
                                        //wl.CapacityAlocate += CapaUse;
                                        //CapaUseX = 0;
                                        //CapaUse = 0;
                                        wl.CapacityAlocateX += diffTime;
                                        wl.CapacityAlocate += diffTime;
                                        CapaUseX -= diffTime;
                                        CapaUse -= diffTime;
                                        var tCapa = diffTime;
                                        if (CapaUse < 0)
                                        {
                                            tCapa = tCapa + CapaUse; // a + (-b)
                                            CapaUse = 0;
                                        }
                                        foundTime = true;

                                        var capaLoad = baseClass.newCapaLoad(diffTime, tCapa, tempStarting.Value.Date, idJob, 0, idWorkCenter);
                                        capacityLoad.Add(capaLoad);
                                    }

                                    if (foundTime)
                                    {
                                        var cl = baseClass.newCalendar(autoid, r.id, idWorkCenter, idCalendar, tempStarting.Value.Date, timeStart, timeEnd, idJob, -1);
                                        calLoad.Add(cl);
                                    }
                                }

                                tempStarting = tempStarting.Value.Date.SetTimeToDate(timeEnd);
                                //tempStarting = tempStarting.Value.Date.AddHours(timeEnd.Hours).AddMinutes(timeEnd.Minutes).AddMilliseconds(timeEnd.Milliseconds);
                                finalEndingDate = tempStarting.Value;
                            }
                            else //วันดังกล่าวไม่ใช่ Working Day
                            {
                                string mssg = "Work center ไม่มีวันธรรมงาน.!!!\n";
                                baseClass.Warning(mssg);
                                throw new Exception(mssg);
                            }

                        } while (Math.Round(CapaUseX, 2) > 0);

                        StartingDate = finalStartingDate;
                        EndingDate = finalEndingDate;
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
                        m.CapacityUseX = 0;
                        m.CostOverhead = 0;

                        if (deleteOld)
                        {
                            //ลบ Capacity เก่า
                            var capa = db.mh_CapacityLoads.Where(x => x.DocId == idJob).ToList();
                            db.mh_CapacityLoads.DeleteAllOnSubmit(capa);
                            //ลบ Calendar เก่า
                            var cal = db.mh_CalendarLoads.Where(x => x.idJob == idJob).ToList();
                            db.mh_CalendarLoads.DeleteAllOnSubmit(cal);
                        }

                        var calOvers = new List<CalOverhead>();
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
                            m.CapacityUseX += c.CapacityX;

                            var co = calOvers.Where(x => x.idDoc == c.DocId && x.idWorkcenter == c.WorkCenterID).FirstOrDefault();
                            if (co == null)
                                calOvers.Add(new CalOverhead
                                {
                                    CapacityX = c.CapacityX,
                                    idDoc = c.DocId,
                                    idWorkcenter = c.WorkCenterID
                                });
                            else
                                co.CapacityX += c.CapacityX;
                        }
                        db.SubmitChanges();
                        //ใส่ Calendar ใหม่
                        foreach (var c in calLoad)
                        {
                            if (c.idAbs >= 0) continue;

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

                            var co = calOvers.Where(x => x.idWorkcenter == c.idWorkcenter && x.idDoc == c.idJob && x.idRoute == 0).FirstOrDefault();
                            if (co != null)
                                co.idRoute = c.idRoute;
                        }
                        db.SubmitChanges();

                        //save Cost Overhead
                        var manuTime = 1;
                        var manu = db.mh_ManufacturingSetups.FirstOrDefault();
                        if (manu != null)
                        {
                            if (manu.ShowCapacityInUOM == 2) //Hour
                                manuTime = 60;
                            else if (manu.ShowCapacityInUOM == 3) //Day
                                manuTime = (24 * 60);
                        }
                        foreach (var co in calOvers)
                        {
                            var rt = db.mh_RoutingDTs.Where(x => x.id == co.idRoute && x.idWorkCenter == co.idWorkcenter && x.Active).FirstOrDefault();
                            if (rt != null)
                            {
                                var costAll = Math.Round(rt.UnitCost / manuTime, 2);
                                m.CostOverhead += Math.Round(costAll * co.CapacityX, 2);
                            }
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

        //2018-10-17
        void MoveJobToList_New(bool deleteOld = false)

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
                using (var db = new DataClasses1DataContext())
                {
                    var tdata = new ItemData(txtFGNo.Text);

                    var maxDateJob = db.mh_ProductionOrders.Where(x => x.Active && x.JobNo != txtJobNo.Text).Max(x => x.EndingDate);
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
                    DateTime? finalStartingDate = null; //วันเริ่มงานจริงๆ
                    DateTime? finalEndingDate = null; //วันสิ้นสุด

                    DateTime? tempStarting = maxDateJob;
                    var rt = db.mh_RoutingDTs.Where(x => x.RoutingId == tdata.Routeid && x.Active)
                        .Join(db.mh_WorkCenters.Where(x => x.Active)
                        , hd => hd.idWorkCenter
                        , workcenter => workcenter.id
                        , (hd, workcenter)
                        => new { hd, hd.idWorkCenter, hd.id, hd.SetupTime, hd.RunTime, hd.WaitTime, workcenter })
                        .ToList();
                    decimal FGQty = txtFGQty.Text.ToDecimal();
                    decimal FGPCSUnit = txtPCSUnit.Text.ToDecimal();
                    decimal UseQty = Math.Round(FGQty * FGPCSUnit, 2);

                    //หา Capa Hr ที่น้อยที่สุด
                    var minCapa = 0.00m; //Capa น้อยสุดต่อนาที
                    var setupAll = 0.00m;
                    if (rt.Count > 0)
                    {
                        //var minCapaHr = rt.Min(x => x.workcenter.CapacityHour);
                        var minCapaHr = rt.Min(x => x.hd.CapacityHour.ToDecimal());
                        minCapa = Math.Round(minCapaHr / 60, 9);
                        setupAll = rt.Sum(x => x.SetupTime);

                        //หาว่าต้องใช้กี่นาที ในการผลิต ReqQty ตัว
                        var totalCapaAll = Math.Round(UseQty / minCapa, 9);
                        var CapaUseX = totalCapaAll + setupAll;
                        var CapaUse = totalCapaAll;

                        if (tempStarting == null) //เริ่มจาก period
                            tempStarting = dFrom;

                        var idRoute = rt.FirstOrDefault().hd.RoutingId;
                        //หาวันที่ working day จาก work ที่มี min Capa
                        var ttCapa = rt.FirstOrDefault().workcenter.CapacityHour;
                        var idWorkCenter = 0;
                        foreach (var item in rt)
                        {
                            if (ttCapa >= item.workcenter.CapacityHour)
                            //if(ttCapa >= item.hd.CapacityHour.ToDecimal())
                            {
                                ttCapa = item.workcenter.CapacityHour;
                                //ttCapa = item.hd.CapacityHour.ToDecimal();
                                idWorkCenter = item.idWorkCenter;
                            }
                        }
                        if (idWorkCenter == 0) throw new Exception("Work center not found.\n");
                        var firstD = true;
                        //หาว่า Capacity จากต้องใช้ Starting - Ending ใด โดยวนไปจนกว่า CapaUseX จะเป็น 0
                        do
                        {
                            //หาวันที่เริ่มต้นที่สามารถจะใช้ได้จาก mh_CapacityAvailable join mh_CapacityLoad
                            WorkLoad wl = null;
                            if (workLoads.Count == 0)
                            {
                                var w = baseClass.getWorkLoad_From(tempStarting.Value.Date, idWorkCenter);
                                if (w == null) //ไม่มีจริงๆ แสดงว่า Capacity หมด ต้องกลับไปคำนวนใหม่
                                {
                                    string mssg = "Capacity is not available, Please check Capacity Work load on Capacity Calculation (Work Centers).!!!\n";
                                    baseClass.Warning(mssg);
                                    throw new Exception(mssg);
                                }
                                else // เจอแล้ว Add ใส่ list ไว้เช็คในรอบถัดไป
                                {
                                    workLoads.Add(w);
                                    wl = w;
                                }
                            }
                            else
                            {
                                while (wl == null)
                                {
                                    if (workLoads.Where(x => x.Date == tempStarting.Value.Date && x.CapacityAfterX <= 0 && x.idWorkCenter == idWorkCenter).Count() > 0)
                                        tempStarting = tempStarting.Value.Date.AddDays(1);
                                    else
                                    {
                                        wl = workLoads.Where(x => x.Date >= tempStarting.Value.Date && x.CapacityAfterX > 0
                                            && x.idWorkCenter == idWorkCenter).OrderBy(x => x.Date).FirstOrDefault();
                                        if (wl == null) //ถ้า WorkLoad เป็น Null ให้ไปหาใหม่จาก Db ตรงๆ
                                        {
                                            //var w = baseClass.getWorkLoad(tempStarting.Value.Date, null, idWorkCenter).Where(x => x.CapacityAfterX > 0).FirstOrDefault();
                                            var w = baseClass.getWorkLoad_From(tempStarting.Value.Date, idWorkCenter);
                                            if (w == null) //ไม่มีจริงๆ แสดงว่า Capacity หมด ต้องกลับไปคำนวนใหม่
                                            {
                                                string mssg = "Capacity is not available, Please check Capacity Work load on Capacity Calculation (Work Centers).!!!\n";
                                                baseClass.Warning(mssg);
                                                throw new Exception(mssg);
                                            }
                                            else // เจอแล้ว Add ใส่ list ไว้เช็คในรอบถัดไป
                                            {
                                                if (workLoads.Where(x => x.Date == w.Date.Date && x.CapacityAfterX <= 0 && x.idWorkCenter == idWorkCenter).Count() > 0)
                                                {
                                                    tempStarting = tempStarting.Value.AddDays(1);
                                                    continue;
                                                }
                                                workLoads.Add(w);
                                                wl = w;
                                            }
                                        }
                                    }
                                }
                            }

                            //กรณีที่ วันที่หามาได้จาก WorkLoad มากกว่าวันปัจจุบันให้เปลี่ยนเป็นเริ่ม Start
                            if (wl.Date.Date > tempStarting.Value.Date)
                                tempStarting = wl.Date.Date;
                            //หาเวลาเริ่มทำงานในวันนั้น
                            int dow = baseClass.getDayOfWeek(tempStarting.Value.DayOfWeek);
                            //หาช่วงเวลาทำงานใน mh_WorkingDay
                            var wd = db.mh_WorkCenters.Where(x => x.id == wl.idWorkCenter)
                                .Join(db.mh_WorkingDays.Where(x => x.Day == dow && x.Active)
                                , hd => hd.Calendar
                                , dt => dt.idCalendar
                                , (hd, dt) => new
                                {
                                    hd,
                                    dt,
                                    StartingTime = baseClass.setTimeSpan(dt.StartingTime)
                                ,
                                    EndingTime = baseClass.setTimeSpan(dt.EndingTime)
                                }).ToList();
                            if (wd.Count > 0)
                            {
                                //เตรียมเวลาเริ่ม(sTime) - สิ้นสุด(eTime)
                                var sTime = wd.Min(x => x.StartingTime);
                                var eTime = wd.Max(x => x.EndingTime);

                                //ถ้าต่อจาก work ก่อนหน้าให้ใส่เวลา work ก่อนหน้าใน sTime
                                if (tempStarting != null && sTime < tempStarting.Value.TimeOfDay)
                                    sTime = tempStarting.Value.TimeOfDay;

                                int idCalendar = wd.First().hd.Calendar;
                                //หาวันลา(Abs), วันหยุดงาน(Holiday), ช่วงที่ Workcenter ทำงานไปแล้ว (Workcenter)
                                var calendars = calLoad.Where(x => x.Date == tempStarting.Value.Date
                                    && ((x.idCal == idCalendar && x.idWorkcenter == 0)
                                         || (x.idWorkcenter == idWorkCenter && x.idCal == idCalendar)
                                         || (x.idAbs > 0 && x.idWorkcenter == idWorkCenter)
                                        )
                                    ).OrderBy(x => x.StartingTime).ThenBy(x => x.EndingTime).ToList();
                                if (calendars.Count == 0) //ถ้าไม่มีให้ค้นจาก Db
                                {
                                    calendars = db.mh_CalendarLoads.Where(x => x.Date == tempStarting.Value.Date
                                    && ((x.idCal == idCalendar && x.idWorkcenter == 0)
                                         || (x.idWorkcenter == idWorkCenter && x.idCal == idCalendar)
                                         || (x.idAbs > 0 && x.idWorkcenter == idWorkCenter)
                                        )
                                    ).OrderBy(x => x.StartingTime).ThenBy(x => x.EndingTime).ToList();
                                    if (calendars.Count > 0) //ถ้าค้นเจอใน Db
                                        calLoad.AddRange(calendars);
                                }

                                //หาเวลาเริ่มต้น Starting Time
                                bool foundTime = false; //เจอเวลา Starting ที่ใช้ได้หรือป่าว
                                                        //หาว่าไปทับกับช่วงเวลาไหนไหม
                                if (calendars.Count > 0)
                                {
                                    var idCal = new List<int>();
                                    while (sTime < eTime) //วนจนกว่าจะหมดวัน
                                    {
                                        //หาว่าเวลาเริ่มต้นอยู่ในช่วงเวลาทำงานปกติไหม โดยจะต้องไม่เท่ากับเวลาสิ้นสุด
                                        var ww = calendars.Where(x => !idCal.Any(q => q == x.id)
                                            && sTime >= x.StartingTime && sTime < x.EndingTime).FirstOrDefault();
                                        if (ww != null)
                                        {
                                            sTime = ww.EndingTime;
                                            idCal.Add(ww.id);
                                        }
                                        else
                                        {
                                            if (sTime < eTime)
                                                foundTime = true;
                                            break;
                                        }
                                    }
                                }
                                else //ไม่มีวันลา วันหยุด หรือการทำงานที่เวลาดังกล่าวแสดงว่าใช้เวลานี้ได้
                                    foundTime = true;

                                //เช็คว่าเวลาที่หามาได้นั้นอยู่ในช่วงเวลาทำงานจริงๆ และไม่ใช่เวลา break
                                if (foundTime && wd.Where(x => sTime >= x.StartingTime && sTime <= x.EndingTime).Count() > 0) //อยู่ในช่วงเวลาทำงานแน่ๆ แต่ต้องเช็คว่าเป็นเวลาสุดท้ายของวันทำงานหรือป่าว
                                {
                                    if (sTime >= eTime) //เวลาที่หามาได้มีค่ามากกว่าหรือเท่ากับ เวลาสิ้นสุดการทำงานของวันนั้น ให้ข้ามไปวันถัดไป
                                    {
                                        tempStarting = tempStarting.Value.Date.AddDays(1);
                                        continue;
                                    }
                                    else if (wd.Where(x => sTime >= x.StartingTime && sTime == x.EndingTime).Count() > 0) //เวลาที่หามาได้เท่ากับเวลาสิ้นสุดของช่วงเวลานั้นไม่ได้ต้องปรับเป็นเวลาแรกที่ใกล้ที่สุดที่มากกว่า sTime
                                    {
                                        sTime = wd.Where(x => sTime < x.StartingTime).FirstOrDefault().StartingTime;
                                        tempStarting = tempStarting.Value.Date.SetTimeToDate(sTime);
                                    }
                                    else //เป็นช่วงเวลาที่ใช้ได้จริงๆ <<<<****>>>>
                                        tempStarting = tempStarting.Value.Date.SetTimeToDate(sTime);
                                    //tempStarting = tempStarting.Value.Date.AddHours(sTime.Hours).AddMinutes(sTime.Minutes).AddMilliseconds(sTime.Milliseconds);
                                }
                                else //ไม่อยู่ในช่วงเวลาทำงาน
                                {
                                    tempStarting = tempStarting.Value.Date.AddDays(1);
                                    continue;
                                }

                                //**ใส่ Starting Date
                                if (finalStartingDate == null)
                                    finalStartingDate = tempStarting.Value;

                                //*******************************
                                //หาเวลาสิ้นสุด EndingTime
                                foundTime = false;
                                var timeStart = tempStarting.Value.TimeOfDay; //เริ่มนับจากเวลาที่หาได้ใน Starting
                                var timeEnd = eTime;

                                //ดึงปฏิทินเฉพาะเวลาที่มากกว่า timeStart
                                var calendars2 = calendars.Where(x => x.StartingTime >= timeStart).OrderBy(x => x.StartingTime).ToList();
                                if (calendars2.Count > 0) //หาว่าไปตรงกับปฏิทินวันลา, วันหยุด, วันที่ถูกทำงานไปแล้วหรือไม่
                                {
                                    var idCal = new List<int>();
                                    var t_timeEnd = timeEnd;
                                    while (wl.CapacityAfterX > 0) //วนไปจนกว่า Capacity Time ของวันนั้นจะหมด หรือ CapaUseX = 0
                                    {
                                        //วนเวลาที่ถูกใช้ทีละอัน
                                        var aTime = calendars2.Where(x => !idCal.Any(q => q == x.id)).FirstOrDefault();
                                        if (aTime == null)
                                        {
                                            timeEnd = eTime;
                                            //t_timeEnd = eTime;
                                        }
                                        else
                                        {
                                            timeEnd = aTime.StartingTime;
                                            t_timeEnd = aTime.EndingTime;
                                        }

                                        //หาว่าเป็นเวลาที่อยู่ในช่วงเวลาทำงานไหม
                                        //ถ้าไม่ใช่ช่วงเวลาทำงาน
                                        if (wd.Where(x => timeStart >= x.StartingTime && timeStart <= x.EndingTime
                                                     && timeEnd >= x.StartingTime && timeEnd <= x.EndingTime).Count() < 1)
                                        {
                                            timeEnd = wd.Where(x => timeStart >= x.StartingTime && timeStart <= x.EndingTime).First().EndingTime;
                                            t_timeEnd = timeEnd;
                                        }
                                        else if (aTime != null)
                                            idCal.Add(aTime.id);
                                        //คำนวน Capacity Time
                                        decimal diffTime = (timeEnd - timeStart).TotalMinutes.ToDecimal();
                                        if (diffTime > 0)
                                        {
                                            if (diffTime >= CapaUseX) //ถ้าเวลาที่หามาได้มากกว่าเวลาที่ ต้องใช้จริงๆ จะต้องปรับให้ เวลาสิ้นสุดเป็นเวลาจริง = CapaUseX
                                            {
                                                timeEnd = timeStart.Add(TimeSpan.FromMinutes(Math.Round(CapaUseX.ToDouble(), 2)));

                                                //ใส่ทุก work ด้วยเวลาเท่ากัน
                                                foreach (var r in rt)
                                                {
                                                    var capaLoad = baseClass.newCapaLoad(CapaUseX, CapaUse, tempStarting.Value.Date, idJob, 0, r.idWorkCenter);
                                                    capacityLoad.Add(capaLoad);
                                                }

                                                wl.CapacityAlocateX += CapaUseX;
                                                wl.CapacityAlocate += CapaUse;
                                                CapaUseX = 0;
                                                CapaUse = 0;

                                                //ใส่ทุก work ด้วยเวลาเท่ากัน
                                                foreach (var r in rt)
                                                {
                                                    var cl = baseClass.newCalendar(autoMe(calLoad), r.id, r.idWorkCenter, idCalendar, tempStarting.Value.Date, timeStart, timeEnd, idJob, -1);
                                                    calLoad.Add(cl);
                                                    if (firstD)
                                                    {
                                                        finalStartingDate = tempStarting.Value.Date.SetTimeToDate(timeStart);
                                                        firstD = false;
                                                    }
                                                }
                                                break; //CapaUseX หมดแล้ว ออกได้เลย
                                            }
                                            else
                                            {
                                                wl.CapacityAlocateX += diffTime;
                                                wl.CapacityAlocate += diffTime;
                                                CapaUseX -= diffTime;
                                                CapaUse -= diffTime;
                                                var tCapa = diffTime;
                                                if (CapaUse < 0)
                                                {
                                                    tCapa = tCapa + CapaUse; // a + (-b)
                                                    CapaUse = 0;
                                                }

                                                //ใส่ทุก work ด้วยเวลาเท่ากัน
                                                foreach (var r in rt)
                                                {
                                                    var capaLoad = baseClass.newCapaLoad(diffTime, tCapa, tempStarting.Value.Date, idJob, 0, r.idWorkCenter);
                                                    capacityLoad.Add(capaLoad);

                                                    var cl = baseClass.newCalendar(autoMe(calLoad), r.id, r.idWorkCenter, idCalendar, tempStarting.Value.Date, timeStart, timeEnd, idJob, -1);
                                                    calLoad.Add(cl);
                                                    if (firstD)
                                                    {
                                                        finalStartingDate = tempStarting.Value.Date.SetTimeToDate(timeStart);
                                                        firstD = false;
                                                    }
                                                }
                                            }
                                        }

                                        timeStart = t_timeEnd;
                                        if (wd.Where(x => timeStart == x.EndingTime).Count() > 0)
                                        {
                                            var ww = wd.Where(x => x.StartingTime > timeStart).FirstOrDefault();
                                            if (ww != null)
                                                timeStart = ww.StartingTime;
                                        }
                                    }
                                }
                                else //ไม่ตรงกับเวลาที่ถูกใช้งานใดๆ ให้ลด Capacity
                                {
                                    if (wl.CapacityAfterX >= CapaUseX) //กรณีที่เหลือเวลา Capacity ในวันนั้นเพียงพอ
                                    {
                                        timeEnd = timeStart.Add(TimeSpan.FromMinutes(Math.Round(CapaUseX.ToDouble(), 2)));
                                    }
                                    else //กรณีที่ในวันนั้น Capaciy ไม่เพียงพอ
                                    {
                                        timeEnd = eTime; //ตั้งเป็นเวลาสิ้นสุดทำงานได้เลย
                                    }

                                    //เช็คว่าเวลาที่หามาได้นั้นอยู่ในช่วงเวลาทำงานจริงๆ และไม่ใช่เวลา break
                                    if (wd.Where(x => timeStart >= x.StartingTime && timeEnd <= x.EndingTime).Count() < 1) //ไม่อยู่ในช่วงเวลาทำงานแน่ๆ
                                    {
                                        //ต้องหาว่าอยู่เกินช่วงไหนโดยเอาเวลา Start ไปหา
                                        var f = wd.Where(x => timeStart >= x.StartingTime && timeStart <= x.EndingTime).FirstOrDefault();
                                        timeEnd = f.EndingTime;

                                        var diffTime = (timeEnd - timeStart).TotalMinutes.ToDecimal();

                                        wl.CapacityAlocateX += diffTime;
                                        wl.CapacityAlocate += diffTime;
                                        CapaUseX -= diffTime;
                                        CapaUse -= diffTime;
                                        var tCapa = diffTime;
                                        if (CapaUse < 0)
                                        {
                                            tCapa = tCapa + CapaUse; // a + (-b)
                                            CapaUse = 0;
                                        }
                                        foundTime = true;

                                        //ใส่เท่าๆกันทุก work
                                        foreach (var r in rt)
                                        {
                                            var capaload = baseClass.newCapaLoad(diffTime, tCapa, tempStarting.Value.Date, idJob, 0, r.idWorkCenter);
                                            capacityLoad.Add(capaload);
                                        }
                                    }
                                    else
                                    {
                                        var diffTime = (timeEnd - timeStart).TotalMinutes.ToDecimal();

                                        //wl.CapacityAlocateX += CapaUseX;
                                        //wl.CapacityAlocate += CapaUse;
                                        //CapaUseX = 0;
                                        //CapaUse = 0;
                                        wl.CapacityAlocateX += diffTime;
                                        wl.CapacityAlocate += diffTime;
                                        CapaUseX -= diffTime;
                                        CapaUse -= diffTime;

                                        var tCapa = diffTime;
                                        if (CapaUse < 0)
                                        {
                                            tCapa = tCapa + CapaUse; // a + (-b)
                                            CapaUse = 0;
                                        }
                                        foundTime = true;

                                        //ใส่เท่าๆกันทุก work
                                        foreach (var r in rt)
                                        {
                                            var capaLoad = baseClass.newCapaLoad(diffTime, tCapa, tempStarting.Value.Date, idJob, 0, r.idWorkCenter);
                                            capacityLoad.Add(capaLoad);
                                        }
                                    }

                                    if (foundTime)
                                    {
                                        foreach (var r in rt)
                                        {
                                            var cl = baseClass.newCalendar(autoMe(calLoad), r.id, r.idWorkCenter, idCalendar, tempStarting.Value.Date, timeStart, timeEnd, idJob, -1);
                                            calLoad.Add(cl);
                                            if (firstD)
                                            {
                                                finalStartingDate = tempStarting.Value.Date.SetTimeToDate(timeStart);
                                                firstD = false;
                                            }
                                        }
                                    }
                                }

                                tempStarting = tempStarting.Value.Date.SetTimeToDate(timeEnd);

                                finalEndingDate = tempStarting.Value;
                            }
                            else //วันดังกล่าวไม่ใช่ Working Day
                            {
                                string mssg = "Work center not having Working days.!!!\n";
                                baseClass.Warning(mssg);
                                throw new Exception(mssg);
                            }


                        } while (Math.Round(CapaUseX, 2) > 0);
                        StartingDate = finalStartingDate;
                        EndingDate = finalEndingDate;
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
                        m.CapacityUseX = 0;
                        m.CostOverhead = 0;
                        if (deleteOld)
                        {
                            //ลบ Capacity เก่า
                            var capa = db.mh_CapacityLoads.Where(x => x.DocId == idJob).ToList();
                            db.mh_CapacityLoads.DeleteAllOnSubmit(capa);
                            //ลบ Calendar เก่า
                            var cal = db.mh_CalendarLoads.Where(x => x.idJob == idJob).ToList();
                            db.mh_CalendarLoads.DeleteAllOnSubmit(cal);
                        }

                        var calOvers = new List<CalOverhead>();
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
                            m.CapacityUseX += c.CapacityX;

                            var co = calOvers.Where(x => x.idDoc == c.DocId && x.idWorkcenter == c.WorkCenterID).FirstOrDefault();
                            if (co == null)
                                calOvers.Add(new CalOverhead
                                {
                                    CapacityX = c.CapacityX,
                                    idDoc = c.DocId,
                                    idWorkcenter = c.WorkCenterID
                                });
                            else
                                co.CapacityX += c.CapacityX;
                        }
                        db.SubmitChanges();
                        //ใส่ Calendar ใหม่
                        foreach (var c in calLoad)
                        {
                            if (c.idAbs >= 0) continue;

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

                            var co = calOvers.Where(x => x.idWorkcenter == c.idWorkcenter && x.idDoc == c.idJob && x.idRoute == 0).FirstOrDefault();
                            if (co != null)
                                co.idRoute = c.idRoute;
                        }
                        db.SubmitChanges();

                        //save Cost Overhead
                        var manuTime = 1;
                        var manu = db.mh_ManufacturingSetups.FirstOrDefault();
                        if (manu != null)
                        {
                            if (manu.ShowCapacityInUOM == 2) //Hour
                                manuTime = 60;
                            else if (manu.ShowCapacityInUOM == 3) //Day
                                manuTime = (24 * 60);
                        }
                        foreach (var co in calOvers)
                        {
                            var rt = db.mh_RoutingDTs.Where(x => x.id == co.idRoute && x.idWorkCenter == co.idWorkcenter && x.Active).FirstOrDefault();
                            if (rt != null)
                            {
                                var costAll = Math.Round(rt.UnitCost / manuTime, 2);
                                m.CostOverhead += Math.Round(costAll * co.CapacityX, 2);
                            }
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
        int autoMe(List<mh_CalendarLoad> calLoad)
        {
            int autoid = 0;
            do
            {
                var rd = new Random();
                autoid = rd.Next(-999999, -100);
            } while (calLoad.Where(x => x.id == autoid).Count() > 0);
            return autoid;
        }

        private void txtJobNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                t_JobNo = txtJobNo.Text.Trim();
                DataLoad(true);
            }
        }


        void AddHistory(string Detail, string DocNo)
        {
            dbClss.AddHistory(this.Name, "Job Order Sheet", Detail, DocNo);
        }

        private void btnExportFile_Click(object sender, EventArgs e)
        {

        }

        private void btnShipping_Click(object sender, EventArgs e)
        {
            //if (txtJobNo.Text.Trim() != "")
            //{
                Report.Reportx1.Value = new string[1];
                Report.Reportx1.Value[0] = txtJobNo.Text.Trim();
                Report.Reportx1.WReport = "PickSlip_RM";
                //Report.Reportx1 op = new Report.Reportx1("ReportShipping2.rpt");
                Report.Reportx1 op = new Report.Reportx1("PickSlip_RM.rpt");

                op.Show();
            //}
        }

        private void btnSendApprove_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    if (db.mh_ProductionOrders.Where(x => x.JobNo == txtJobNo.Text.Trim() && x.Active).Count() > 0 && txtSeqStatus.Text.ToInt() < 1)
                    {
                        if (baseClass.IsSendApprove())
                        {
                            db.sp_062_mh_ApproveList_Add(txtJobNo.Text.Trim(), "Job Req", Classlib.User);
                            MessageBox.Show("Send complete.");
                            btnRefresh_Click(null, null);
                        }
                    }
                    else
                        baseClass.Warning("สถานะไม่สามารถส่ง Approve ได้.\n");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void MasterTemplate_Click(object sender, EventArgs e)
        {

        }
    }
}
