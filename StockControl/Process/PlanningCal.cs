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
using Telerik.WinControls;
using System.Threading;

namespace StockControl
{
    public partial class PlanningCal : Telerik.WinControls.UI.RadRibbonForm
    {
        bool openFilter = true;
        public PlanningCal()
        {
            InitializeComponent();
            openFilter = false;
        }

        private void radMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            HistoryView hw = new HistoryView(this.Name);
            this.Cursor = Cursors.Default;
            hw.ShowDialog();
        }

        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }
        private void Unit_Load(object sender, EventArgs e)
        {
            //dgvData.ReadOnly = true;
            dgvData.AutoGenerateColumns = false;

            DataLoad();

            dgvData.Columns.ToList().ForEach(x =>
            {
                x.ReadOnly = x.Name != "S";
            });
        }

        private void DataLoad()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    var m = db.mh_Planning_TEMPs.ToList();
                    dgvData.DataSource = null;
                    dgvData.AutoGenerateColumns = false;
                    dgvData.DataSource = m;

                    //var rowList = new List<GridViewRowInfo>();
                    //foreach (var item in dgvData.Rows)
                    //{
                    //    int idRef = item.Cells["idRef"].Value.ToInt();
                    //    if (item.Cells["PlanningType"].Value.ToSt() == "Production")
                    //    {
                    //        var d = db.mh_ProductionOrders.Where(x => x.RefDocId == idRef && x.Active).ToList();
                    //        if (d.Count > 0) //already on job
                    //            rowList.Add(item);
                    //    }
                    //    else //Purchase
                    //    {
                    //        var p = db.mh_PurchaseRequestLines.Where(x => x.idCstmPODt == idRef && x.SS == 1).ToList();
                    //        if (p.Count > 0) //already on P/R
                    //            rowList.Add(item);
                    //    }
                    //}

                    ////remove row
                    //rowList.ForEach(x =>
                    //{
                    //    dgvData.Rows.Remove(x);
                    //});
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

        //

        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {

            }
            catch (Exception ex) { }
        }

        private void radGridView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }


        int row = -1;
        private void radGridView1_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            row = e.RowIndex;

        }


        private void MasterTemplate_CellValueChanged(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.Column.Name.Equals("S") && e.Row.Cells["S"].Value.ToBool())
                {
                    if (e.Row.Cells["Status"].Value.ToSt() == "Over Due")
                        e.Row.Cells["S"].Value = false;
                }
            }
        }

        private void MasterTemplate_RowFormatting(object sender, RowFormattingEventArgs e)
        {

        }

        private void MasterTemplate_CellFormatting(object sender, CellFormattingEventArgs e)
        {

        }

        private void btnRecal_Click(object sender, EventArgs e)
        {
            var ft = new PlanningCal_Filter(true, false);
            ft.ShowDialog();
            if (ft.okFilter)
            {
                ClearPlan();

                radLabelElement1.Text = $"Filter date : {ft.dateFrom.ToDtString()}-{ft.dateTo.ToDtString()}";
                //LoadData(true, ft.dateFrom, ft.dateTo, ft.MRP, ft.MPS, ft.ItemNo, ft.locationItem);
                var ps = new PlanningCal_Status(ft.dateFrom, ft.dateTo, ft.ItemNo, ft.locationItem, ft.MRP, ft.MPS);
                //ps.dFrom = ft.dateFrom;
                //ps.dTo = ft.dateTo;
                //ps.ItemNo = ft.ItemNo;
                //ps.LocationItem = ft.locationItem;
                ps.ShowDialog();
                if (ps.calComplete)
                {
                    dgvData.DataSource = null;
                    dgvData.AutoGenerateColumns = false;
                    //dgvData.DataSource = ps.gridPlans.OrderBy(x => x.ReqDate);

                    SavePlan(ps.gridPlans, ft.dateFrom, ft.dateTo);
                    //SaveCapacity_TEMP(ps.capacityLoad);
                    //SaveCalendar_TEMP(ps.calLoad);
                    SaveReserve(ps.sReserve);

                    DataLoad();
                    dFrom = ps.dFrom;

                    //FilterE(ft.dateFrom, ft.dateTo, ft.MRP, ft.MPS, ft.ItemNo, ft.locationItem);
                }
            }
        }
        private void btnFil_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count == 0)
            {
                baseClass.Warning("Not have data for Filter.\n");
                return;
            }
            //
            this.Cursor = Cursors.WaitCursor;
            try
            {
                var ft = new PlanningCal_Filter(false, false);
                ft.ShowDialog();
                if (ft.okFilter)
                {
                    FilterE(ft.dateFrom, ft.dateTo, ft.MRP, ft.MPS, ft.ItemNo, ft.locationItem);
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
        void FilterE(DateTime dateFrom, DateTime dateTo, bool MRP, bool MPS, string _itemNo, string _locationItem)
        {
            DateTime dFrom = dateFrom.Date;
            DateTime dTo = dateTo.Date.AddDays(1).AddMinutes(-1);
            string ItemNo = _itemNo;
            string LocationItem = _locationItem;
            List<int> idA = new List<int>();
            foreach (var item in dgvData.Rows)
            {
                //Date
                var ReqDate = item.Cells["ReqDate"].Value.ToDateTime().Value;
                if (ReqDate < dFrom || ReqDate > dTo)
                {
                    item.IsVisible = false;
                    continue;
                }
                else
                    item.IsVisible = true;
                //Location
                var Lo = item.Cells["LocationItem"].Value.ToSt();
                if (LocationItem == "") { }
                else
                {
                    if (LocationItem != Lo)
                    {
                        item.IsVisible = false;
                        continue;
                    }
                    else
                        item.IsVisible = true;
                }
                //MPS MRP
                if (MPS && MRP) { }
                else
                {
                    string pType = item.Cells["PlanningType"].Value.ToSt();
                    if (MPS && pType == "Production")
                        item.IsVisible = true;
                    else if (MPS && pType == "Purchase")
                    {
                        item.IsVisible = false;
                        continue;
                    }
                    else if (!MPS && pType == "Production")
                    {
                        item.IsVisible = false;
                        continue;
                    }
                    else if (MRP && pType == "Purchase")
                        item.IsVisible = true;
                    else if (MRP && pType == "Production")
                    {
                        item.IsVisible = false;
                        continue;
                    }
                    else if (!MRP && pType == "Purchase")
                    {
                        item.IsVisible = false;
                        continue;
                    }

                }
                //Item
                var FGNo = item.Cells["ItemNo"].Value.ToSt();
                int idRef = item.Cells["idRef"].Value.ToInt();
                if (ItemNo == "") { }
                else
                {
                    if (FGNo != ItemNo)
                    {
                        item.IsVisible = false;
                        continue;
                    }
                    else if (!idA.Any(x => x == idRef))
                    {
                        idA.Add(idRef);
                    }
                }
            }

            //find idRef
            foreach (var item in dgvData.Rows.Where(x => !x.IsVisible))
            {
                int idRef = item.Cells["idRef"].Value.ToInt();
                item.IsVisible = idA.Any(x => x == idRef);
            }
        }

        DateTime dFrom = DateTime.Now;
        private void MasterTemplate_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.Column.Name.Equals("S"))
                {
                    e.Cancel = e.Row.Cells["Status"].Value.Equals("Over Due");
                }
            }
        }

        private void btnClearPlan_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count > 0 && baseClass.Question("Do you want to 'Clear Plan data' ?"))
                ClearPlan(true);
        }
        void ClearPlan(bool warningMssg = false)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    db.mh_Planning_TEMPs.DeleteAllOnSubmit(db.mh_Planning_TEMPs);
                    //db.mh_CapacityLoad_TEMPs.DeleteAllOnSubmit(db.mh_CapacityLoad_TEMPs);
                    //db.mh_CalendarLoad_TEMPs.DeleteAllOnSubmit(db.mh_CalendarLoad_TEMPs);
                    db.mh_StockReserves.DeleteAllOnSubmit(db.mh_StockReserves);
                    db.SubmitChanges();

                    dgvData.DataSource = null;
                    dgvData.Rows.Clear();

                    if (warningMssg)
                        baseClass.Warning("Clear Plan complete.\n");

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
        void SavePlan(List<grid_Planning> gPlans, DateTime dFrom, DateTime dTo)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    foreach (var g in gPlans)
                    {
                        var p = new mh_Planning_TEMP
                        {
                            DueDate = g.DueDate,
                            EndingDate = g.EndingDate.Value,
                            GroupType = g.GroupType,
                            idRef = g.idRef,
                            InvGroup = g.InvGroup,
                            ItemName = g.ItemName,
                            ItemNo = g.ItemNo,
                            PCSUnit = g.PCSUnit,
                            PlanDateFrom = dFrom,
                            PlanDateTo = dTo,
                            PlanningType = g.PlanningType,
                            Qty = g.Qty,
                            RefDocNo = g.RefDocNo,
                            RefDocNo_TEMP = g.RefDocNo_TEMP,
                            ReqDate = g.ReqDate,
                            StartingDate = g.StartingDate.Value,
                            Status = g.Status,
                            TotalCost = g.TotalCost,
                            Type = g.Type,
                            UOM = g.UOM,
                            VendorName = g.VendorName,
                            VendorNo = g.VendorNo,
                            LocationItem = g.LocationItem,
                            root = g.root,
                            mainNo = g.mainNo,
                            refNo = g.refNo,
                        };
                        db.mh_Planning_TEMPs.InsertOnSubmit(p);
                    }
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                baseClass.Warning(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        void SaveCapacity_TEMP(List<mh_CapacityLoad> capas)
        {
            using (var db = new DataClasses1DataContext())
            {
                foreach (var item in capas)
                {
                    if (item.id == 0)
                    {
                        var a = new mh_CapacityLoad_TEMP();
                        db.mh_CapacityLoad_TEMPs.InsertOnSubmit(a);

                        a.Active = true;
                        a.Capacity = item.Capacity;
                        a.CapacityX = item.CapacityX;
                        a.DocId = item.DocId;
                        a.Date = item.Date;
                        a.DocNo = item.DocNo;
                        a.WorkCenterID = item.WorkCenterID;
                    }
                }
                db.SubmitChanges();
            }
        }
        void SaveCalendar_TEMP(List<mh_CalendarLoad> calens)
        {
            using (var db = new DataClasses1DataContext())
            {
                foreach (var item in calens)
                {
                    if (item.idAbs < 0)
                    {
                        var a = new mh_CalendarLoad_TEMP();
                        db.mh_CalendarLoad_TEMPs.InsertOnSubmit(a);
                        a.Date = item.Date;
                        a.EndingTime = item.EndingTime;
                        a.idAbs = item.idAbs;
                        a.idCal = item.idCal;
                        a.idHol = item.idHol;
                        a.idJob = item.idJob;
                        a.idRoute = item.idRoute;
                        a.idWorkcenter = item.idWorkcenter;
                        a.StartingTime = item.StartingTime;
                    }
                }
                db.SubmitChanges();
            }
        }
        void SaveReserve(List<stockReserve> sReserve)
        {
            using (var db = new DataClasses1DataContext())
            {
                foreach (var sr in sReserve)
                {
                    var s = new mh_StockReserve();
                    s.idCstmPODt = sr.idCstmPODt;
                    s.idCstmPODt_Free = sr.idCstmPODt_Free;
                    s.id_tb_Stock = sr.id_tb_Stock;
                    s.ItemNo = sr.ItemNo;
                    s.ReserveQty = sr.ReserveQty;
                    s.mainNo = 0;
                    db.mh_StockReserves.InsertOnSubmit(s);
                    db.SubmitChanges();
                }
            }
        }

        private void radButtonElement1_Click(object sender, EventArgs e)
        {
            dgvData.EnableFiltering = true;
        }

        private void radButtonElement2_Click(object sender, EventArgs e)
        {
            dgvData.EnableFiltering = false;
        }

        private void btnGenJob_Click(object sender, EventArgs e)
        {
            dgvData.EndEdit();
            GenJob();
        }
        void GenJob()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                var rowS = dgvData.Rows.Where(x => x.Cells["S"].Value.ToBool()).ToList();
                if (rowS.Count < 1)
                {
                    baseClass.Warning("Please select data.!");
                    return;
                }
                if (rowS.Where(x => x.Cells["PlanningType"].Value.ToSt() == "Production").Count() < 1)
                {
                    baseClass.Warning("Please select PlanningType: Production.\n");
                    return;
                }

                if (!baseClass.Question("Do you want to 'Generate Job Order Sheet' ?"))
                    return;

                using (var db = new DataClasses1DataContext())
                {
                    foreach (var item in rowS.Where(x => x.Cells["PlanningType"].Value.ToSt() == "Production"))
                    {
                        //Hd
                        var m = new mh_ProductionOrder();
                        m.CreateBy = ClassLib.Classlib.User;
                        m.CreateDate = DateTime.Now;
                        m.JobDate = DateTime.Now.Date;
                        m.JobNo = dbClss.GetNo(29, 2);
                        //
                        m.Active = true;
                        m.EndingDate = item.Cells["EndingDate"].Value.ToDateTime().Value;
                        m.FGName = item.Cells["ItemName"].Value.ToSt();
                        m.FGNo = item.Cells["ItemNo"].Value.ToSt();
                        m.ReqDate = item.Cells["ReqDate"].Value.ToDateTime().Value;
                        var reqDate = m.ReqDate.Date;
                        var lot = db.mh_LotFGs.Where(x => x.LotDate == reqDate).FirstOrDefault();
                        if (lot != null)
                            m.LotNo = lot.LotNo;
                        else
                            m.LotNo = "";
                        m.Qty = item.Cells["Qty"].Value.ToDecimal();
                        m.PCSUnit = item.Cells["PCSUnit"].Value.ToDecimal();
                        m.OutQty = m.Qty;
                        m.RefDocId = item.Cells["idRef"].Value.ToInt();
                        m.RefDocNo = item.Cells["RefDocNo"].Value.ToSt();
                        m.StartingDate = item.Cells["StartingDate"].Value.ToDateTime().Value;
                        m.UOM = item.Cells["UOM"].Value.ToSt();
                        m.UpdateBy = ClassLib.Classlib.User;
                        m.UpdateDate = DateTime.Now;
                        m.HoldJob = false;
                        db.mh_ProductionOrders.InsertOnSubmit(m);
                        //Update Customer P/O
                        if (item.Cells["root"].Value.ToBool())
                        {
                            var po = db.mh_CustomerPODTs.Where(x => x.id == m.RefDocId).FirstOrDefault();
                            if (po != null)
                            {
                                //po.OutPlan -= m.OutQty;
                                po.OutPlan = 0;//Full Ref Customer P/O
                                po.Status = baseClass.setCustomerPOStatus(po);
                                db.SubmitChanges();
                            }
                            db.SubmitChanges();
                        }

                        var calOvers = new List<CalOverhead>();
                        //Dt
                        //**Component**
                        int mainNo = item.Cells["mainNo"].Value.ToInt(); //find all component of Item
                        var rowDt = db.tb_BomDTs.Where(x => x.PartNo == m.FGNo).ToList();
                        foreach (var r in rowDt)
                        {
                            var itemA = db.mh_Items.Where(x => x.InternalNo == r.Component).FirstOrDefault();
                            if (itemA == null) continue;
                            var dt = new mh_ProductionOrderRM
                            {
                                Active = true,
                                GroupType = itemA.GroupType,
                                InvGroup = itemA.InventoryGroup,
                                ItemName = itemA.InternalName,
                                ItemNo = itemA.InternalNo,
                                JobNo = m.JobNo,
                                PCSUnit = r.PCSUnit.ToDecimal(),
                                Qty = m.Qty * r.Qty,
                                OutQty = m.Qty * r.Qty,
                                Type = itemA.Type,
                                UOM = r.Unit,
                                CostOverall = 0.00m,
                            };
                            db.mh_ProductionOrderRMs.InsertOnSubmit(dt);
                            db.SubmitChanges();
                        }
                        //save Capacity Load --mh_CapacityLoad_TEMP <---> mh_CapacityLoad
                        var capaList = db.mh_CapacityLoad_TEMPs.Where(x => x.DocId == mainNo && x.DocNo == null).ToList();
                        foreach (var c in capaList)
                        {
                            if (c.DocNo.ToSt() != "") continue;

                            var cc = new mh_CapacityLoad
                            {
                                Active = true,
                                Capacity = c.Capacity,
                                CapacityX = c.CapacityX,
                                Date = c.Date,
                                DocId = m.id,//idJob
                                DocNo = m.JobNo,
                                WorkCenterID = c.WorkCenterID,
                            };
                            db.mh_CapacityLoads.InsertOnSubmit(cc);
                            c.DocId = m.id;
                            c.DocNo = m.JobNo;
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

                        //save Calendar Load --mh_CalendarLoad_TEMP <---> mh_CalendarLoad
                        var calList = db.mh_CalendarLoad_TEMPs.Where(x => x.idJob == mainNo && x.idAbs == -1).ToList();
                        foreach (var c in calList)
                        {
                            var cc = new mh_CalendarLoad
                            {
                                Date = c.Date,
                                EndingTime = c.EndingTime,
                                idAbs = (c.idAbs >= 0) ? c.idAbs : 0,
                                idCal = c.idCal,
                                idHol = c.idHol,
                                idJob = m.id, //idJob
                                idRoute = c.idRoute,
                                idWorkcenter = c.idWorkcenter,
                                StartingTime = c.StartingTime,
                            };
                            db.mh_CalendarLoads.InsertOnSubmit(cc);
                            c.idJob = m.id;
                            c.idAbs = 0;

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

                        //delete gridPlan
                        int id = item.Cells["id"].Value.ToInt();
                        var d = db.mh_Planning_TEMPs.Where(x => x.id == id).ToList();
                        if (d.Count > 0)
                        {
                            db.mh_Planning_TEMPs.DeleteAllOnSubmit(d);
                            db.SubmitChanges();
                        }


                        //Send to Approve
                        db.sp_062_mh_ApproveList_Add(m.JobNo, "Job Req", ClassLib.Classlib.User);
                    }
                    DataLoad();

                    baseClass.Info("Generate Job Order Sheet complete.\n");
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

        private void btnGenPR_Click(object sender, EventArgs e)
        {
            dgvData.EndEdit();
            GenPR();
        }
        void GenPR()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {

                var rowS = dgvData.Rows.Where(x => x.Cells["S"].Value.ToBool()).ToList();
                if (rowS.Count < 1)
                {
                    baseClass.Warning("Please select data.!");
                    return;
                }
                if (rowS.Where(x => x.Cells["PlanningType"].Value.ToSt() == "Purchase").Count() < 1)
                {
                    baseClass.Warning("Please select PlanningType: Purchase.\n");
                    return;
                }


                if (!baseClass.Question("Do you want to 'Generate Purchase Request (P/R)' ?"))
                    return;

                DataTable _dt = new DataTable();
                _dt.Columns.Add("idCstmPO", typeof(int));
                _dt.Columns.Add("PRNo", typeof(string));
                using (var db = new DataClasses1DataContext())
                {
                    foreach (var item in rowS.Where(x => x.Cells["PlanningType"].Value.ToSt() == "Purchase"))
                    {//Purchase
                        //Hd
                        byte[] b = null;
                        int idRef = item.Cells["idRef"].Value.ToInt();
                        var poDt = db.mh_CustomerPODTs.Where(x => x.id == idRef).FirstOrDefault();
                        int idPoHd = 0;
                        if (poDt != null)
                            idPoHd = poDt.idCustomerPO;
                        string CstmPoNo = item.Cells["RefDocNo"].Value.ToSt();

                        var hd = new mh_PurchaseRequest();
                        if (_dt.Rows.Cast<DataRow>().Where(x => x["idCstmPO"].ToInt() == idPoHd).Count() > 0)
                        {
                            var row = _dt.Rows.Cast<DataRow>().Where(x => x["idCstmPO"].ToInt() == idPoHd).First();
                            string prNo = row["PRNo"].ToSt();
                            hd = db.mh_PurchaseRequests.Where(x => x.PRNo == prNo).FirstOrDefault();
                        }
                        else
                        {
                            //New PR
                            hd = new mh_PurchaseRequest
                            {
                                Barcode = b,
                                ClearBill = false,
                                CreateBy = ClassLib.Classlib.User,
                                CreateDate = DateTime.Now,
                                Department = "Planing",
                                HDRemark = "",
                                idCstmPO = idPoHd,
                                LocationRunning = null,
                                PRNo = dbClss.GetNo(12, 2),
                                RefDocument = CstmPoNo, //Customer PoNo
                                RequestBy = ClassLib.Classlib.User,
                                RequestDate = DateTime.Now,
                                Status = "Waiting",
                                TEMPNo = dbClss.GetNo(3, 2),
                                Total = 0,//update
                                UpdateBy = ClassLib.Classlib.User,
                                UpdateDate = DateTime.Now,
                            };
                            db.mh_PurchaseRequests.InsertOnSubmit(hd);
                            _dt.Rows.Add(idPoHd, hd.PRNo);
                        }
                        if (poDt != null) poDt.genPR = true;
                        db.SubmitChanges();

                        //Dt
                        string itemNo = item.Cells["ItemNo"].Value.ToSt();
                        var tool = db.mh_Items.Where(x => x.InternalNo == itemNo).FirstOrDefault();
                        var amnt = Math.Round(item.Cells["Qty"].Value.ToDecimal() * tool.StandardCost, 2);
                        var vn = db.mh_Vendors.Where(x => x.No == tool.VendorNo).FirstOrDefault();

                        var dt = db.mh_PurchaseRequestLines.Where(x => x.PRNo == hd.PRNo && x.idCstmPODt == idRef && x.CodeNo == itemNo && x.SS == 1
                                                            && x.PCSUOM == item.Cells["PCSUnit"].Value.ToDecimal() && x.UOM == item.Cells["UOM"].Value.ToSt()).FirstOrDefault();
                        if (dt != null)
                        {
                            dt.OrderQty += item.Cells["Qty"].Value.ToDecimal();
                        }
                        else
                        { //new Item
                            dt = new mh_PurchaseRequestLine
                            {
                                Amount = amnt,
                                CodeNo = itemNo,
                                Cost = tool.StandardCost,
                                GroupCode = tool.GroupType,
                                idCstmPODt = idRef, //idPODt
                                ItemDesc = item.Cells["ItemName"].Value.ToSt(),
                                ItemName = item.Cells["ItemName"].Value.ToSt(),
                                OrderQty = item.Cells["Qty"].Value.ToDecimal(),
                                PCSUOM = item.Cells["PCSUnit"].Value.ToDecimal(),
                                PRNo = hd.PRNo,
                                SS = 1,
                                Status = "Waiting",
                                TempNo = hd.TEMPNo,
                                UOM = item.Cells["UOM"].Value.ToSt(),
                                VATType = tool.VatType,
                                VendorName = tool.VendorName,
                                VendorNo = tool.VendorNo,
                                DeliveryDate = item.Cells["DueDate"].Value.ToDateTime().Value.Date
                            };
                            db.mh_PurchaseRequestLines.InsertOnSubmit(dt);
                        }
                        hd.Total += amnt;
                        db.SubmitChanges();

                        //delete gridPlan
                        int id = item.Cells["id"].Value.ToInt();
                        var d = db.mh_Planning_TEMPs.Where(x => x.id == id).ToList();
                        if (d.Count > 0)
                        {
                            db.mh_Planning_TEMPs.DeleteAllOnSubmit(d);
                            db.SubmitChanges();
                        }
                    }

                    DataLoad();
                    baseClass.Info("Generate Purchase Request complete.\n");
                }
            }
            catch (Exception ex)
            {
                baseClass.Error(ex.Message);
            }
            finally { this.Cursor = Cursors.Default; }
        }

        private void btnLinkToJob_Click(object sender, EventArgs e)
        {
            var j = new ProductionOrder_List();
            j.ShowDialog();
        }

        private void btnLinkToPR_Click(object sender, EventArgs e)
        {
            var j = new CreatePR_List();
            j.ShowDialog();
        }

        private void radButtonElement3_Click(object sender, EventArgs e)
        {
            dbClss.ExportGridXlSX(dgvData);
        }
    }

}
