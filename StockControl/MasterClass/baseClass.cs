using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace StockControl
{
    public static class baseClass
    {
        public static string ToSt(this object val)
        {
            try
            {
                if (val == null)
                    return "";
                else
                    return Convert.ToString(val);
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public static int ToInt(this object val)
        {
            try
            {
                if (val == null)
                    return 0;
                else
                    return Convert.ToInt32(val);
            }
            catch
            {
                return 0;
            }
        }
        public static double ToDouble(this object val)
        {
            try
            {
                if (val == null)
                    return 0;
                else
                    return Convert.ToDouble(val);
            }
            catch
            {
                return 0;
            }
        }
        public static decimal ToDecimal(this object val)
        {
            try
            {
                if (val == null)
                    return 0.00m;
                else
                    return Math.Round(Convert.ToDecimal(val), 2);
            }
            catch
            {
                return 0.00m;
            }
        }
        public static bool ToBool(this object val)
        {
            try
            {
                if (val == null)
                    return false;
                else
                    return Convert.ToBoolean(val);
            }
            catch
            {
                return false;
            }
        }
        public static DateTime? ToDateTime(this object val)
        {
            try
            {
                return (DateTime?)Convert.ToDateTime(val);
            }
            catch
            {
                return null;
            }
        }
        public static string ToDtString(this DateTime val)
        {
            return val.ToString("dd/MMM/yyyy");
        }
        public static string ToDtTimeString(this DateTime val)
        {
            return val.ToString("dd/MMM/yyyy HH:mm");
        }

        public static void Info(string Mssg)
        {
            RadMessageBox.Show(Mssg, "Infomation", MessageBoxButtons.OK, RadMessageIcon.Info);
        }
        public static void Error(string Mssg)
        {
            RadMessageBox.Show(Mssg, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
        }
        public static void Warning(string Mssg)
        {
            RadMessageBox.Show(Mssg, "Warning", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
        }
        public static bool Question(string Mssg, string Caption = "Question")
        {
            return RadMessageBox.Show(Mssg, Caption, MessageBoxButtons.YesNo, RadMessageIcon.Question) == DialogResult.Yes;
        }
        public static bool IsSave(string Mssg = "ต้องการ 'บันทึก' ?")
        {
            return Question(Mssg, "บันทึก");
        }
        public static bool IsApprove(string Mssg = "ต้องการ 'Approve' ?")
        {
            return Question(Mssg, "Approve");
        }
        public static bool IsReject(string Mssg = "ต้องการ 'Reject' ?")
        {
            return Question(Mssg, "Reject");
        }
        public static bool IsSendApprove(string Mssg = "ต้องการ 'Send Approve' ?")
        {
            return Question(Mssg, "Send Approve");
        }
        public static bool IsDel(string Mssg = "ต้องการ 'ลบ' ?")
        {
            return Question(Mssg, "ลบ");
        }

        public static string ToMoney(this object val)
        {
            try
            {
                return val.ToDecimal().ToString("#,0.00");
            }
            catch { return "0.00"; }
        }

        public static T ToEnum<T>(this object val)
        {
            return (T)Enum.Parse(typeof(T), val.ToSt(), true);
        }
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static decimal StockQty(string ItemNo, string LocationCode, string Category)
        {
            using (var db = new DataClasses1DataContext())
            {
                var g = db.Cal_QTY_Remain_Location(ItemNo, Category, 0, LocationCode, 0).Value.ToDecimal();
                return g;
            }
        }

        public static int getDayOfWeek(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Sunday: return 6;
                case DayOfWeek.Monday: return 0;
                case DayOfWeek.Tuesday: return 1;
                case DayOfWeek.Wednesday: return 2;
                case DayOfWeek.Thursday: return 3;
                case DayOfWeek.Friday: return 4;
                case DayOfWeek.Saturday: return 5;
                default: return -1; //not have day
            }
        }
        public static TimeSpan setTimeSpan(string TimeText)
        {
            TimeSpan t = new TimeSpan();
            //00:00
            TimeText = TimeText.Substring(0, 5);
            t = new TimeSpan(TimeText.Substring(0, 2).ToInt(), TimeText.Substring(3).ToInt(), 0);
            return t;
        }
        public static ReorderType getReorderType(string ReorderTypeSt)
        {
            if (ReorderTypeSt == "Fixed Reorder Qty")
                return ReorderType.Fixed;
            else if (ReorderTypeSt == "Minimum & Maximum Qty")
                return ReorderType.MinMax;
            else
                return ReorderType.ByOrder;
        }
        public static string getReorderTypeText(ReorderType rd)
        {
            if (rd == ReorderType.Fixed)
                return "Fixed Reorder Qty";
            else if (rd == ReorderType.MinMax)
                return "Minimum & Maximum Qty";
            else
                return "By Order";
        }
        public static ReplenishmentType getRepType(string RepTypeSt)
        {
            if (RepTypeSt == "Production")
                return ReplenishmentType.Production;
            else
                return ReplenishmentType.Purchase;
        }
        public static InventoryGroup getInventoryGroup(string InvGroupSt)
        {
            if (InvGroupSt == "FG")
                return InventoryGroup.FG;
            else if (InvGroupSt == "SEMI")
                return InventoryGroup.SEMI;
            else
                return InventoryGroup.RM;
        }
        public static DateTime setStandardTime(DateTime dt, bool TimeStart)
        {
            using (var db = new DataClasses1DataContext())
            {
                var g = db.mh_ManufacturingSetups.FirstOrDefault();
                if (TimeStart)
                    dt = dt.AddHours(g.StandardStartingTime.Hours).AddMinutes(g.StandardStartingTime.Minutes);
                else
                    dt = dt.AddHours(g.StandardEndingTime.Hours).AddMinutes(g.StandardEndingTime.Minutes);
                return dt;
            }
        }

        public static List<WorkLoad> getWorkLoad(DateTime beginDate, DateTime? dTo, int idWorkcenter = 0)
        {
            var workLoads = new List<WorkLoad>();
            using (var db = new DataClasses1DataContext())
            {
                if (idWorkcenter > 0)
                {
                    var bDate = beginDate.Date;
                    var tDate = (dTo != null) ? dTo.Value.Date : beginDate.Date.AddDays(7);
                    while (bDate <= tDate)
                    {
                        CalCapacity_WorkCenter(idWorkcenter, bDate);
                        bDate = bDate.AddDays(1);
                    }
                }
                //
                var d = db.mh_CapacityAvailables.Where(x => x.Date >= beginDate
                    && (dTo == null || x.Date <= dTo)).ToList();
                foreach (var dd in d)
                {
                    var m = db.mh_CapacityLoads.Where(x => x.Date == dd.Date
                            && x.WorkCenterID == dd.WorkCenterID && x.Active).ToList();
                    decimal loadCapa = 0.00m;
                    decimal loadCapaX = 0.00m;
                    //if (m.Count > 0)
                    loadCapa = m.Sum(x => x.Capacity);
                    loadCapaX = m.Sum(x => x.CapacityX);
                    var wl = workLoads.Where(x => x.Date == dd.Date
                        && x.idWorkCenter == dd.WorkCenterID).FirstOrDefault();
                    if (wl == null)
                    {
                        wl = new WorkLoad();
                        workLoads.Add(wl);
                    }
                    wl.idWorkCenter = dd.WorkCenterID;
                    wl.Date = dd.Date.Date;
                    wl.CapacityAvailable = dd.Capacity.ToDecimal();
                    wl.CapacityAvailableX = dd.CapacityX.ToDecimal();
                    wl.CapacityAlocate += loadCapa; //Capa ในวัน
                    wl.CapacityAlocateX += loadCapaX; //เวลาในวัน
                }
                return workLoads;
            }
        }
        public static WorkLoad getWorkLoad_From(DateTime beginDate, int idWorkcenter)
        {
            var wl = new WorkLoad();
            using (var db = new DataClasses1DataContext())
            {
                if (idWorkcenter > 0)
                {
                    var bDate = beginDate.Date;
                    bool findE = true;
                    while (findE)
                    {
                        var av = CalCapacity_WorkCenter(idWorkcenter, bDate);
                        var m = db.mh_CapacityLoads.Where(x => x.Date == av.Date
                                && x.WorkCenterID == av.WorkCenterID && x.Active).ToList();

                        decimal loadCapa = 0.00m;
                        decimal loadCapaX = 0.00m;
                        loadCapa = m.Sum(x => x.Capacity);
                        loadCapaX = m.Sum(x => x.CapacityX);

                        if (av.CapacityX - loadCapaX > 0)
                        {
                            wl.idWorkCenter = av.WorkCenterID;
                            wl.Date = av.Date.Date;
                            wl.CapacityAvailable = av.Capacity.ToDecimal();
                            wl.CapacityAvailableX = av.CapacityX.ToDecimal();
                            wl.CapacityAlocate += loadCapa; //Capa ในวัน
                            wl.CapacityAlocateX += loadCapaX; //เวลาในวัน
                            findE = false;
                        }
                        //
                        bDate = bDate.AddDays(1);
                    }
                }
                //
                return wl;
            }
        }
        public static List<CalendarLoad> getCalendarLoad(DateTime beginDate)
        {
            var calLoad = new List<CalendarLoad>();
            using (var db = new DataClasses1DataContext())
            {
                var d = db.mh_CalendarLoads.Where(x => x.Date >= beginDate).ToList();
                foreach (var dd in d)
                {
                    calLoad.Add(new CalendarLoad
                    {
                        Date = dd.Date,
                        StartingTime = dd.StartingTime,
                        EndingTime = dd.EndingTime,
                        idJob = dd.idJob,
                        idRoute = dd.idRoute,
                        idWorkcenter = dd.idWorkcenter,
                        id = dd.id,
                        idAbs = dd.idAbs,
                        idCal = dd.idCal,
                        idHol = dd.idHol,
                    });
                }
                return calLoad;
            }
        }
        public static void getStartingWork(ref DateTime d, int idWorkcenter)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DateTime setWorkTime(DateTime d, int idWorkCenter, decimal CapaLoad)
        {
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    var m = db.mh_WorkCenters.Where(x => x.id == idWorkCenter).First();
                    int dow = getDayOfWeek(d.DayOfWeek);
                    var c = db.mh_WorkingDays.Where(x => x.idCalendar == m.Calendar && x.Active
                                && x.Day == dow).ToList();
                    TimeSpan sTime = setTimeSpan(c.OrderBy(x => x.StartingTime.Replace(":", "").ToInt()).First().StartingTime);
                    //find Ending time
                    var endingTime = "00:00";
                    if (c.Where(x => x.EndingTime.Replace(":", "").ToInt() == 0).Count() < 1)
                    {
                        endingTime = c.OrderByDescending(x => x.EndingTime.Replace(":", "").ToInt()).First().EndingTime;
                    }
                    TimeSpan eTime = setTimeSpan(endingTime);

                    //find Time

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return d;
        }
        public static DateTime SetTimeToDate(this DateTime d, TimeSpan Ts)
        {
            //
            return d.AddHours(Ts.Hours).AddMinutes(Ts.Minutes).AddSeconds(Ts.Seconds).AddMilliseconds(Ts.Milliseconds);
            //return DateTime.Now;
        }

        //get Path Server
        public static string GetPathServer(PathCode pCode)
        {
            using (var db = new DataClasses1DataContext())
            {
                var m = db.tb_Paths.ToList();
                switch (pCode)
                {
                    case PathCode.Customer:
                        {
                            var a = m.Where(x => x.PathCode == "Customer").FirstOrDefault();
                            if (a != null)
                                return a.PathFile;
                            else
                                return "";
                        }
                    case PathCode.Vendor:
                        {
                            var a = m.Where(x => x.PathCode == "Vendor").FirstOrDefault();
                            if (a != null)
                                return a.PathFile;
                            else
                                return "";
                        }
                    case PathCode.PriceList:
                        {
                            var a = m.Where(x => x.PathCode == "PriceList").FirstOrDefault();
                            if (a != null)
                                return a.PathFile;
                            else
                                return "";
                        }
                    default: return "";
                }
            }
        }

        //Customer P/O Status
        public static string setCustomerPOStatus(mh_CustomerPODT dt)
        {
            var fullQty = dt.Qty * dt.PCSUnit;
            var Qty = dt.Qty;
            if (dt.OutPlan == fullQty && dt.OutSO == Qty)
                return "Waiting";
            else if (dt.OutPlan == 0 && dt.OutSO == 0)
                return "Completed";
            else if (dt.OutPlan != fullQty || dt.OutSO != Qty)
                return "Process";
            else
                return "Waiting";
        }

        //Sale Order DT Status
        public static string setSaleOrderStatus(mh_SaleOrderDT dt)
        {
            string ret = "Waiting";
            using (var db = new DataClasses1DataContext())
            {
                var m = db.mh_SaleOrders.Where(x => x.SONo == dt.SONo).FirstOrDefault();
                if (m != null)
                {
                    if (m.SeqStatus == 0)
                        ret = "Waiting";
                    else if (m.SeqStatus == 1)
                        ret = "Waiting Approved";
                    else
                    {//Approve
                        var ship = db.mh_ShipmentDTs.Where(x => x.Active && x.RefId == dt.id)
                            .Join(db.mh_Shipments.Where(x => x.Active == true)
                            , ss => ss.SSNo
                            , hd => hd.SSNo
                            , (ss, hd) => new { hd, ss }).ToList();
                        var shipQ = 0.00m;
                        if(ship.Count > 0)
                            shipQ = ship.Sum(x => x.ss.Qty - x.ss.OutShip);

                        if (dt.Qty - shipQ <= 0)
                            ret = "Completed";
                        else if (shipQ > 0)
                            ret = "Partial";
                        else if (shipQ == 0 || m.SeqStatus == 2)
                            ret = "Process";
                    }
                }
            }
            return ret;
        }

        //move reserve stock 
        public static void moveReserveStock(int idCstmPODt_Free, int idCstmPODt
            , int id_tb_Stock, decimal ReserveQty)
        {
            using (var db = new DataClasses1DataContext())
            {
                var tbst = db.tb_Stocks.Where(x => x.id == id_tb_Stock).FirstOrDefault();
                //**เขียน ShipQty ออกจาก Job
                //เขียน SHipping Header
                tb_ShippingH gg = new tb_ShippingH();
                int idProductionOrderRM = 0;
                string shipNo = dbClss.GetNo(5, 2);
                gg.ShippingNo = shipNo;
                gg.ShipDate = DateTime.Now;
                gg.UpdateBy = ClassLib.Classlib.User;
                gg.UpdateDate = DateTime.Now;
                gg.CreateBy = ClassLib.Classlib.User;
                gg.CreateDate = DateTime.Now;
                gg.ShipName = ClassLib.Classlib.User;
                var po = db.mh_CustomerPODTs.Where(x => x.id == idCstmPODt)
                    .Join(db.mh_CustomerPOs.Where(x => x.DemandType == 0)
                    , dt => dt.idCustomerPO
                    , hd => hd.id
                    , (dt, hd) => new { hd, dt }).FirstOrDefault();
                string cstmPOno = (po != null && po.hd != null) ? po.hd.CustomerPONo : idCstmPODt.ToSt();
                gg.Remark = $"Shipping for Move Stock to new CustomerPO ({cstmPOno})";
                gg.JobCard = "";
                gg.TempJobCard = "";
                byte[] barcode = null;
                gg.BarCode = barcode;
                gg.Status = "Completed";
                gg.ToLocation = "Warehouse";
                db.tb_ShippingHs.InsertOnSubmit(gg);
                db.SubmitChanges();
                //Shipping ADD
                var tool = db.mh_Items.Where(x => x.InternalNo == tbst.CodeNo).FirstOrDefault();
                var uom = db.mh_ItemUOMs.Where(x => x.UOMCode == tool.BaseUOM && x.ItemNo == tbst.CodeNo).FirstOrDefault();
                var pcsunit = 1.00m;
                if (uom != null) pcsunit = uom.QuantityPer;
                db.sp_024_tb_Shipping_ADD(shipNo, tbst.CodeNo
                                    , ReserveQty //QtyPlan
                                    , ReserveQty //QtyShip
                                    , "", "", "", ""
                                    , tbst.LotNo
                                    , "Completed", ClassLib.Classlib.User
                                    , "", ""//txtTempJobCard.Text.Trim()
                                    , 0, tbst.Location, "Warehouse", tool.BaseUOM, pcsunit
                                    , tool.BaseUOM, pcsunit, idCstmPODt_Free
                                    , idProductionOrderRM
                                    , -1
                                    );

                //**Stock Receive ใส่ Customer PODt id ใบใหม่
                var amntCost = Math.Round(tbst.UnitCost.ToDecimal() * ReserveQty.ToDecimal(), 2);
                var s = new tb_Stock();
                s.AppDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                s.Seq = 1;
                s.App = "Receive";
                s.Appid = 1;
                s.CreateBy = ClassLib.Classlib.User;
                s.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                s.DocNo = dbClss.GetNo(33, 2); //MOS -- Move stock free to new CustomerPO
                s.RefNo = id_tb_Stock.ToSt(); //Refer id tb_Stock ของ stock free Customer PO เก่า
                s.CodeNo = tbst.CodeNo;
                s.Type = "Move Stock Free to New Sale Order";
                s.QTY = ReserveQty;
                s.Inbound = s.QTY;
                s.Outbound = 0;
                s.Type_i = 1;
                s.Category = "Invoice";
                s.Refid = idCstmPODt_Free; //old idCstmPODt ---> Free Stock
                s.idCSTMPODt = idCstmPODt; //new idCstmPODt
                s.Type_in_out = "In";
                s.AmountCost = amntCost;
                if (s.AmountCost > 0)
                    s.UnitCost = Math.Round(s.AmountCost.ToDecimal() / s.QTY.ToDecimal(), 6);
                else
                    s.UnitCost = 0;

                decimal RemainQty = (Convert.ToDecimal(db.Cal_QTY_Remain_Location(s.CodeNo, "Free", 0, "Warehouse", -1)));
                decimal sum_Remain = Convert.ToDecimal(dbClss.Get_Stock(s.CodeNo, "", "", "RemainAmount", "Warehouse", 0/*idCstmPODt*/)) + s.AmountCost.ToDecimal();
                decimal sum_Qty = RemainQty.ToDecimal() + s.QTY.ToDecimal();
                var RemainAmount = sum_Remain;
                decimal RemainUnitCost = 0.00m;
                if (sum_Qty <= 0)
                    RemainUnitCost = 0;
                else
                    RemainUnitCost = Math.Round((Math.Abs(RemainAmount) / Math.Abs(sum_Qty)), 6);
                s.RemainQty = sum_Qty;
                s.RemainUnitCost = RemainUnitCost;
                s.RemainAmount = RemainAmount;
                s.Avg = 0;
                s.CalDate = null;
                s.Status = "Active";
                s.Flag_ClearTemp = 0;
                s.TLCost = s.AmountCost;
                s.TLQty = s.QTY;
                s.ShipQty = 0;
                s.Location = "Warehouse";
                s.LotNo = tbst.LotNo;
                s.ShelfNo = tbst.ShelfNo;
                //ต้องไม่ใช่ Item ที่มีในระบบ
                var c = (from ix in db.mh_Items
                         where ix.InternalNo.Trim().ToUpper() == s.CodeNo.Trim().ToUpper() && ix.Active
                         select ix).ToList();
                if (c.Count <= 0)
                {
                    s.TLQty = 0;
                    s.ShipQty = s.QTY;
                }

                db.tb_Stocks.InsertOnSubmit(s);
                db.SubmitChanges();

                //update Stock เข้า item
                db.sp_010_Update_StockItem(Convert.ToString(s.CodeNo), "");
            }
        }


        //Calculate Capacity Available
        public static mh_CapacityAvailable CalCapacity_WorkCenter(int idWorkCenter, DateTime dTemp)
        {
            using (var db = new DataClasses1DataContext())
            {
                var t = db.mh_WorkCenters.Where(x => x.id == idWorkCenter).FirstOrDefault();

                var cal = db.mh_Calendars.Where(x => x.id == t.Calendar).First(); //Calendar
                var dow = db.mh_WorkingDays.Where(x => x.Active && x.idCalendar == cal.id).ToList(); //0:Mon - 6:Sun
                var hol = db.mh_Holidays.Where(x => x.Active && x.idCalendar == cal.id).ToList(); //Holiday
                var abs = db.mh_CapacityAbsences.Where(x => x.Active && x.idWorkCenters == t.id).ToList(); //Absence

                decimal minWork = 0.00m;
                decimal minWorkCapa = 0.00m;
                //not day of work
                if (!dow.Select(x => GetDayOfWeek(x.Day)).Contains(dTemp.DayOfWeek))
                {
                    minWork = -1;
                }
                else
                {
                    //find minute of work
                    foreach (var d in dow.Where(x => GetDayOfWeek(x.Day) == dTemp.DayOfWeek))
                    {
                        var sTime = new TimeSpan(d.StartingTime.Substring(0, 2).ToInt(), d.StartingTime.Substring(3).ToInt(), 0);
                        var eTime = new TimeSpan(d.EndingTime.Substring(0, 2).ToInt(), d.EndingTime.Substring(3).ToInt(), 0);
                        minWork += (eTime - sTime).TotalMinutes.ToDecimal() * t.Capacity;
                        minWorkCapa += (eTime - sTime).TotalMinutes.ToDecimal();
                    }

                    //chk hol
                    if (minWork > 0 && hol.Where(x => dTemp >= x.StartingDate && dTemp <= x.EndingDate).Count() > 0)
                    {
                        foreach (var h in hol.Where(x => dTemp >= x.StartingDate && dTemp <= x.EndingDate))
                        {
                            var sTime = new TimeSpan(h.StartTime.Substring(0, 2).ToInt(), h.StartTime.Substring(3).ToInt(), 0);
                            var eTime = new TimeSpan(h.EndingTime.Substring(0, 2).ToInt(), h.EndingTime.Substring(3).ToInt(), 0);
                            if (eTime > new TimeSpan(0, 0, 0))
                            {
                                minWork -= (eTime - sTime).TotalMinutes.ToDecimal() * t.Capacity;
                                minWorkCapa -= (eTime - sTime).TotalMinutes.ToDecimal();
                            }
                            else
                            {
                                eTime = new TimeSpan(23, 59, 0);
                                minWork -= ((eTime - sTime).TotalMinutes + 1).ToDecimal() * t.Capacity;
                                minWorkCapa -= ((eTime - sTime).TotalMinutes + 1).ToDecimal();
                            }
                            if (minWork < 0)
                                break;
                        }
                    }

                    //chk absence
                    if (minWork > 0 && abs.Where(x => x.Date.Date == dTemp.Date).Count() > 0)
                    {
                        foreach (var a in abs.Where(x => x.Date.Date == dTemp.Date))
                        {
                            var sTime = new TimeSpan(a.StartingTime.Substring(0, 2).ToInt(), a.StartingTime.Substring(3).ToInt(), 0);
                            var eTime = new TimeSpan(a.EndingTime.Substring(0, 2).ToInt(), a.EndingTime.Substring(3).ToInt(), 0);
                            minWork -= (eTime - sTime).TotalMinutes.ToDecimal() * a.Capacity;
                            minWorkCapa -= (eTime - sTime).TotalMinutes.ToDecimal();

                            if (minWork < 0)
                                break;
                        }
                    }
                }

                //find capa
                //Minwork * capa
                decimal? totalCapa = null;
                decimal? totalMinWork = null;
                if (minWork > 0)
                {
                    totalCapa = minWork.ToDecimal();
                    totalMinWork = minWorkCapa.ToDecimal();
                }
                var mh = db.mh_CapacityAvailables.Where(x => x.WorkCenterID == t.id && x.Date == dTemp).FirstOrDefault();
                if (mh == null)
                {
                    mh = new mh_CapacityAvailable();
                    db.mh_CapacityAvailables.InsertOnSubmit(mh);
                }
                mh.Date = dTemp;
                mh.Capacity = totalCapa;
                mh.CapacityX = totalMinWork;
                mh.WorkCenterID = t.id;
                db.SubmitChanges();

                return mh;
            }
        }
        public static DayOfWeek GetDayOfWeek(int id)
        {
            if (id == 0) return DayOfWeek.Monday;
            else if (id == 1) return DayOfWeek.Tuesday;
            else if (id == 2) return DayOfWeek.Wednesday;
            else if (id == 3) return DayOfWeek.Thursday;
            else if (id == 4) return DayOfWeek.Friday;
            else if (id == 5) return DayOfWeek.Saturday;
            else return DayOfWeek.Sunday;


        }

        //For TempCal Plan
        public static mh_CapacityLoad newCapaLoad(decimal CapaUseX, decimal Capacity, DateTime Date, int DocId, int id, int idWorkCenter)
        {
            return new mh_CapacityLoad
            {
                Active = true,
                Capacity = Capacity,
                CapacityX = CapaUseX,
                Date = Date,
                DocId = DocId,
                id = id,
                WorkCenterID = idWorkCenter
            };
        }
        public static mh_CalendarLoad newCalendar(int id, int idRoute, int idWorkCenter, int idCalendar, DateTime Date, TimeSpan StartingTIme, TimeSpan EndingTime, int idJob, int idAbs)
        {
            return new mh_CalendarLoad
            {
                id = id,
                idRoute = idRoute,
                idWorkcenter = idWorkCenter,
                idCal = idCalendar,
                Date = Date,
                StartingTime = StartingTIme,
                EndingTime = EndingTime,
                idJob = idJob,
                idAbs = idAbs,
            };
        }

        //ger Price
        public static decimal GetPriceList(string ItemNo, DateTime dt)
        {
            decimal ret = 0.00m;
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    var m = db.mh_PriceLists.Where(x => x.InternalNo == ItemNo && dt >= x.StartDate && dt <= x.EndDate && x.SeqStatus == 2).FirstOrDefault();
                    if (m != null) ret = Math.Round(m.UnitPrice, 2);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ret;
        }

        //get Accident from RM id
        public static decimal GetQtyAccidenSlip(int idPdRM)
        {
            var ret = 0.00m;
            using (var db = new DataClasses1DataContext())
            {
                var accd = db.mh_Accident_Slips.Where(x => x.idProductionOrderRM == idPdRM && x.Status != "Cancel")
                                    .Join(db.mh_Accident_SlipHs.Where(x => x.Status != "Cancel" && x.Type == "None")
                                    , dd => dd.DocNo
                                    , hd => hd.DocNo
                                    , (dd, hd) => new { hd, dd }).ToList();
                if (accd.Count > 0)
                    ret = accd.Sum(x => Math.Round(x.dd.QTY * x.dd.PCSUnit, 2));
            }
            return ret;
        }

    }


    public enum TypeAction
    {
        Add,
        Edit,
        Delete,
        View
    }
    public enum PathCode
    {
        Vendor,
        Customer,
        PriceList,
    }



}
