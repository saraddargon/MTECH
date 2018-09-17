using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace StockControl
{
    public partial class PlanningCal_Status : Form
    {
        public DateTime dFrom { get; set; } = DateTime.Now;
        public DateTime dTo { get; set; } = DateTime.Now;
        public string ItemNo { get; set; } = "";
        public string LocationItem { get; set; } = "";

        public PlanningCal_Status()
        {
            InitializeComponent();
        }
        public PlanningCal_Status(DateTime dFrom, DateTime dTo, string ItemNo, string Location)
        {
            InitializeComponent();
            this.dFrom = dFrom;
            this.dTo = dTo;
            this.ItemNo = ItemNo;
            this.LocationItem = Location;
        }

        bool startCal = false;
        private void PlanningCal_Status_Load(object sender, EventArgs e)
        {
            if (!startCal)
            {
                startCal = false;
                Thread td = new Thread(new ThreadStart(calE));
                td.Start();
            }
        }

        public bool calComplete = false;
        public List<grid_Planning> gridPlans = new List<grid_Planning>(); //mh_Planning_TEMP
        private List<ItemData> itemDatas = new List<ItemData>();
        public List<WorkLoad> workLoads = new List<WorkLoad>();
        public List<mh_CapacityLoad> capacityLoad = new List<mh_CapacityLoad>(); //เชื่อมกับ mh_CapacityLoad_TEMP
        public List<mh_CalendarLoad> calLoad = new List<mh_CalendarLoad>(); //เชื่อมกับ mh_CalendarLoad_TEMP
        private DateTime? PDDate = null;
        int mainNo = 0;
        void calE()
        {
            try
            {
                gridPlans.Clear();
                itemDatas.Clear();
                workLoads.Clear();
                capacityLoad.Clear();
                PDDate = null;
                mainNo = 0;

                changeLabel("Finding Docno for plan...");
                var cstmPO_List = new List<CustomerPOCal>();
                using (var db = new DataClasses1DataContext())
                {
                    //1.Get Customer P/O (OutPlan) and SaleOrder (OutPlan) [Only not customer P/O]
                    var poDt = db.mh_CustomerPODTs.Where(x => x.Active
                        && x.ReqDate >= dFrom && x.ReqDate <= dTo
                        && x.OutPlan > 0
                    ).OrderBy(x => x.ReqDate).ToList();

                    //if (dFrom < DateTime.Now.Date)
                    //    dFrom = DateTime.Today;
                    foreach (var dt in poDt)
                    {
                        if (ItemNo != "" && dt.ItemNo != ItemNo) continue;
                        var t = db.mh_Items.Where(x => x.InternalNo == dt.ItemNo).FirstOrDefault();
                        if (t == null) continue;
                        if (LocationItem != "" && t.Location != LocationItem) continue;

                        var pohd = db.mh_CustomerPOs.Where(x => x.id == dt.idCustomerPO
                                && x.Active).FirstOrDefault();
                        if (pohd == null) continue;

                        cstmPO_List.Add(new CustomerPOCal
                        {
                            POHd = pohd,
                            PODt = dt
                        });
                    }

                    changeLabel("Prepare Working Day(Capacity Loaded).\n");
                    ////1.1 Get work load for prepare Calculation
                    //workLoads = baseClass.getWorkLoad(dFrom, dTo);
                    capacityLoad = db.mh_CapacityLoads.Where(x => x.Active && x.Date >= dFrom && x.Date <= dTo).ToList();
                    workLoads = baseClass.getWorkLoad(dFrom, dTo);

                    changeLabel("Prepare Calendar Load.\n");
                    ////1.2 Get Calandar Load for prepare Calculation
                    //calLoad = baseClass.getCalendarLoad(dFrom);
                    calLoad = db.mh_CalendarLoads.Where(x => x.Date >= dFrom && x.Date <= dTo).ToList();

                    //2.Loop order by Due date (Dt) then Order date (Hd) then Order id (Hd)
                    cstmPO_List = cstmPO_List.OrderBy(x => x.PODt.ReqDate)
                        .ThenBy(x => x.POHd.OrderDate).ThenBy(x => x.POHd.id).ToList();
                    foreach (var item in cstmPO_List)
                    {
                        var m = PDDate;
                        changeLabel($"Calculating... Doc no.{item.POHd.CustomerPONo} : [{item.PODt.ItemNo}] {item.PODt.ItemName}");
                        var gPlan = calPart_new(new calPartData
                        {
                            DocId = item.PODt.id,
                            DocNo = item.POHd.CustomerPONo,
                            ItemNo = item.PODt.ItemNo,
                            repType = baseClass.getRepType(item.PODt.ReplenishmentType),
                            ReqDate = item.PODt.ReqDate,
                            ReqQty = item.PODt.OutPlan,
                            mainNo = mainNo,
                            PCSUnit = item.PODt.PCSUnit,
                            UOM = item.PODt.UOM,
                        });
                        if (gPlan == null)
                            continue;
                        gPlan.root = true;
                        mainNo++;
                    }

                    changeLabel($"Calculating... Reorder Stock");
                    //3 Find Purchase for Reorder Type
                    List<string> itemNoList = new List<string>();
                    foreach (var gp in gridPlans.Where(x => x.PlanningType == "Purchase"))
                    {
                        if (itemNoList.Any(x => x == gp.ItemNo)) continue;

                        itemNoList.Add(gp.ItemNo);

                        var qItem = gridPlans.Where(x => x.ItemNo == gp.ItemNo).ToList();
                        var itemData = qItem.First().itemData;

                        if (itemData.ReorderType == ReorderType.MinMax && itemData.MaxQty <= 0)
                            continue;
                        else if (itemData.SafetyStock <= 0)
                            continue;

                        decimal sum_UseQty = qItem.Sum(x => x.UseQty);// Qty ที่ถูกใช้ไปจริงๆ
                        decimal sum_Reorder = qItem.Sum(x => x.Qty);//Qty ที่ถูกสั่งซื้อจากการแพลน
                        decimal QtyOnHand_Backup = qItem.Select(x => x.itemData.QtyOnHand_Backup).First();
                        decimal ReorderQty = 0.00m;
                        var mDate = gridPlans.Max(x => x.EndingDate.Value.Date); //max date ของการ Plan ครั้งนี้
                        var gp1 = new grid_Planning();
                        gp1.ReqDate = mDate.AddDays(1);
                        gp1.idRef = 0;
                        gp1.ItemNo = gp.ItemNo;
                        gp1.ItemName = gp.ItemName;
                        gp1.PlanningType = "Purchase";
                        gp1.RefDocNo = "";
                        gp1.GroupType = gp.GroupType;
                        gp1.Type = gp.Type;
                        gp1.InvGroup = gp.InvGroup;
                        gp1.ReorderTypeEnum = gp.ReorderTypeEnum;
                        gp1.VendorNo = gp.VendorNo;
                        gp1.VendorName = gp.VendorName;
                        gp1.UOM = itemData.UOM;
                        gp1.PCSUnit = itemData.PCSUnit;
                        gp1.LocationItem = gp.LocationItem;
                        gp1.refNo = 0;
                        gp1.mainNo = 0;
                        gp1.itemData = itemData;
                        gp1.StartingDate = baseClass.setStandardTime(gp1.ReqDate, true);
                        gp1.EndingDate = baseClass.setStandardTime(gp1.ReqDate, false);

                        if (itemData.ReorderType == ReorderType.Fixed)
                        {
                            bool comP = false;
                            while (!comP)
                            {
                                if ((QtyOnHand_Backup + sum_Reorder + ReorderQty) - sum_UseQty < itemData.SafetyStock
                                    || (QtyOnHand_Backup + sum_Reorder + ReorderQty) - sum_UseQty < itemData.ReorderPoint)
                                {
                                    ReorderQty += itemData.ReorderQty;
                                }
                                else
                                    comP = true;
                            }
                        }
                        else if (itemData.ReorderType == ReorderType.MinMax)
                        {
                            bool comP = false;
                            while (!comP)
                            {
                                if ((QtyOnHand_Backup + sum_Reorder + ReorderQty) - sum_UseQty < itemData.SafetyStock
                                    || (QtyOnHand_Backup + sum_Reorder + ReorderQty) - sum_UseQty < itemData.MaxQty)
                                {
                                    ReorderQty += itemData.MaxQty - (QtyOnHand_Backup + sum_Reorder + ReorderQty) - sum_UseQty;
                                }
                                else
                                    comP = true;
                            }
                        }
                        else
                        {//By Order
                            if ((QtyOnHand_Backup + sum_Reorder) - sum_UseQty < itemData.SafetyStock)
                            {
                                ReorderQty = itemData.SafetyStock - (QtyOnHand_Backup + sum_Reorder) - sum_UseQty;
                            }
                        }

                        gp1.UseQty = ReorderQty;
                        gp1.UOM = itemData.PurchaseUOM;
                        gp1.PCSUnit = Math.Round(itemData.PCSUnit_PurchaseUOM, 2);
                        gp1.Qty = Math.Round(ReorderQty / gp1.PCSUnit, 2);

                        gp1.DueDate = gp1.EndingDate.Value.Date.AddDays(1);
                        gridPlans.Add(gp1);
                    }

                    changeLabel($"Calculate complete...\n");
                    calComplete = true;
                }
            }
            catch (Exception ex)
            {
                baseClass.Warning("CalE : " + ex.Message);
            }
            finally
            {
                startCal = false;
                this.Invoke(new MethodInvoker(() =>
                {
                    this.Close();
                }));
            }
        }

        grid_Planning calPart_new(calPartData data)

        {
            try
            {
                //RepType == Production, Purchase
                using (var db = new DataClasses1DataContext())
                {
                    var tdata = itemDatas.Where(x => x.ItemNo == data.ItemNo).FirstOrDefault();
                    if (tdata == null)
                    {
                        tdata = new ItemData(data.ItemNo);
                        itemDatas.Add(tdata);
                    }

                    //Find Backorder ref id Customer in mh_PuchaseOrderDT
                    decimal sumBackOrder = 0.00m;
                    var prAll = db.mh_PurchaseRequestLines.Where(x => x.SS == 1 && x.idCstmPODt == data.DocId)
                        .Join(db.mh_PurchaseRequests.Where(x => x.Status != "Cancel")
                        , dt => dt.PRNo
                        , hd => hd.PRNo
                        , (dt, hd) => new { hd, dt }).ToList();
                    foreach(var pr in prAll)
                    {
                        var poAll = db.mh_PurchaseOrderDetails.Where(x => x.SS == 1 && x.PRItem == pr.dt.id)
                            .Join(db.mh_PurchaseOrders.Where(x=>x.Status != "Cancel")
                            , dt => dt.PONo
                            , hd => hd.PONo
                            , (dt, hd) => new { hd, dt })
                            .ToList();
                        sumBackOrder += poAll.Sum(x => x.dt.BackOrder.ToDecimal());
                    }

                    //3.Find Stock is enought ? ---> only stock q'ty not for JOB/Customer P/O
                    if (data.ReqQty <= tdata.QtyOnHand + sumBackOrder) //3.1 Stock is enought
                    {
                        //tdata.QtyOnHand -= data.ReqQty;
                        if (tdata.QtyOnHand >= data.ReqQty)
                            tdata.QtyOnHand -= data.ReqQty;
                        else
                            tdata.QtyOnHand = 0;
                        return null;
                    }

                    //3.2 Stock + Back order not enought
                    var t_QtyOnHand = tdata.QtyOnHand;
                    data.ReqQty -= t_QtyOnHand;
                    data.ReqQty -= sumBackOrder;
                    tdata.QtyOnHand = 0;
                    sumBackOrder = 0;

                    //set data
                    var gPlan = new grid_Planning();
                    gPlan.ReqDate = data.ReqDate;
                    gPlan.idRef = data.DocId;
                    gPlan.ItemNo = data.ItemNo;
                    gPlan.ItemName = tdata.ItemName;
                    gPlan.PlanningType = tdata.RepType_enum == ReplenishmentType.Production ? "Production" : "Purchase";
                    gPlan.Qty = data.ReqQty;
                    gPlan.UseQty = Math.Round(data.ReqQty * data.PCSUnit, 2);
                    gPlan.RefDocNo = data.DocNo;
                    gPlan.GroupType = tdata.GroupType;
                    gPlan.Type = tdata.Type;
                    gPlan.InvGroup = tdata.InvGroup;
                    gPlan.ReorderTypeEnum = tdata.ReorderType;
                    gPlan.VendorNo = tdata.VendorNo;
                    gPlan.VendorName = tdata.VendorName;
                    gPlan.UOM = data.UOM;
                    gPlan.PCSUnit = data.PCSUnit;
                    gPlan.LocationItem = tdata.LocationItem;
                    gPlan.refNo = data.mainNo;
                    mainNo++;
                    gPlan.mainNo = mainNo;
                    gPlan.itemData = tdata;
                    var thisMain = mainNo;

                    //set Production or Purchase
                    if (tdata.RepType_enum == ReplenishmentType.Production)
                    {
                        //Prodction
                        //find BOM
                        var boms = db.tb_BomDTs.Where(x => x.PartNo == gPlan.ItemNo).ToList();
                        foreach (var b in boms)
                        {
                            var tool = db.mh_Items.Where(x => x.InternalNo == b.Component).FirstOrDefault();
                            var cd = new calPartData
                            {
                                DocId = data.DocId,
                                DocNo = data.DocNo,
                                ItemNo = b.Component,
                                repType = baseClass.getRepType(tool.ReplenishmentType),
                                ReqDate = data.ReqDate,
                                ReqQty = Math.Round(b.Qty.ToDecimal() * data.ReqQty, 2),
                                mainNo = gPlan.mainNo,
                                UOM = b.Unit,
                                PCSUnit = b.PCSUnit.ToDecimal(),
                            };
                            calPart_new(cd);
                        }

                        //BEgin Production
                        var rmList = gridPlans.Where(x => x.idRef == gPlan.idRef).ToList();
                        DateTime tempStarting = new DateTime();
                        if (rmList.Count > 0)
                        {
                            tempStarting = rmList.OrderByDescending(x => x.DueDate).First().DueDate.Date;
                        }
                        else
                            tempStarting = dFrom;
                        //find time in Routing...
                        var rt = db.mh_RoutingDTs.Where(x => x.RoutingId == tdata.Routeid)
                            .Join(db.mh_WorkCenters.Where(x => x.Active)
                            , hd => hd.idWorkCenter
                            , workcenter => workcenter.id
                            , (hd, workcenter)
                            => new { hd, hd.idWorkCenter, hd.id, hd.SetupTime, hd.RunTime, hd.WaitTime, workcenter })
                            .ToList();
                        var t_StartingDate = (DateTime?)null;
                        var t_EndingDate = (DateTime?)null;
                        bool firstStart = false;
                        foreach (var r in rt)
                        {
                            if (t_EndingDate != null)
                                tempStarting = t_EndingDate.Value;
                            int idWorkCenter = r.idWorkCenter;
                            var totalCapa_All = 0.00m;
                            var SetupTime = r.SetupTime;
                            var RunTime = r.RunTime;
                            var RunTimeCapa = Math.Round(((RunTime * gPlan.Qty) / r.workcenter.Capacity), 2);
                            var WaitingTime = r.WaitTime;
                            totalCapa_All = SetupTime + RunTimeCapa + r.WaitTime;
                            var CapaUseX = 0.00m;
                            CapaUseX = totalCapa_All;

                            //find capacity Available (Workcenter) on date
                            do
                            {
                                //1. หาว่าเวลาเริ่มสามารถใช้ได้ไหม
                                var wl = workLoads.Where(x => x.Date >= tempStarting.Date && x.CapacityAfterX > 0
                                    && x.idWorkCenter == idWorkCenter).OrderBy(x => x.Date).FirstOrDefault();
                                if (wl == null)
                                {
                                    string mssg = "Capacity is not available, Please check Capacity Work load on Capacity Calculation (Work Centers).!!!\n";
                                    baseClass.Warning(mssg);
                                    throw new Exception(mssg);
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
                                        StartingTime = baseClass.setTimeSpan(dt.StartingTime)
                                    ,
                                        EndingTime = baseClass.setTimeSpan(dt.EndingTime)
                                    }).ToList();
                                if (wd.Count > 0)
                                {
                                    var sTime = wd.Min(x => x.StartingTime); //Starting Time of Working Day
                                    if (sTime < tempStarting.TimeOfDay)
                                        sTime = tempStarting.TimeOfDay;
                                    var eTime = wd.Max(x => x.EndingTime); //Ending Time of Working Day
                                    var meTime = sTime; //for starting Time
                                    int idCalendar = wd.First().hd.Calendar;
                                    //หาว่าเวาลาเริ่มของ Work center นี้ใช้ไปหรือยัง หรือเป็นวันหยุดหรือวันลาหรือไม่
                                    var calLoads = calLoad.Where(x => x.Date == tempStarting.Date
                                            && (x.idCal == idCalendar || x.idWorkcenter == wl.idWorkCenter)
                                        ).OrderBy(x => x.StartingTime).ThenBy(x => x.EndingTime).ToList();
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
                                                 && (x.idCal == idCalendar || x.idWorkcenter == wl.idWorkCenter)
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
                                                            DocId = thisMain, //idTemp
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
                                                            DocId = thisMain, //idTemp
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
                                                        Date = tempStarting.Date,
                                                        StartingTime = meTime,
                                                        EndingTime = meTime2,
                                                        idJob = thisMain, //id Temp
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
                                                    DocId = thisMain, //idTemp
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
                                                    DocId = thisMain, //idTemp
                                                    id = 0,
                                                    WorkCenterID = r.idWorkCenter,
                                                };
                                                capacityLoad.Add(capaLoad);
                                                CapaUseX -= wl.CapacityAlocateX;
                                                wl.CapacityAlocateX = wl.CapacityAvailable;
                                                //wl.CapacityAlocate += (wl.CapacityAlocate - CapaUse);
                                            }

                                            var cl = new mh_CalendarLoad
                                            {
                                                id = autoid,
                                                idRoute = r.id,
                                                idWorkcenter = r.idWorkCenter,
                                                Date = tempStarting.Date,
                                                StartingTime = meTime,
                                                EndingTime = meTime2,
                                                idJob = thisMain,
                                                idAbs = -1,
                                            };
                                            calLoad.Add(cl);

                                            t_EndingDate = tempStarting.Date.AddHours(meTime2.Hours).AddMinutes(meTime2.Minutes);
                                        }
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
                                gPlan.StartingDate = t_StartingDate;
                            gPlan.EndingDate = t_EndingDate;
                            firstStart = true;
                        }

                    }
                    else
                    {
                        //Purchase
                        gPlan.StartingDate = baseClass.setStandardTime(dFrom, true);
                        gPlan.EndingDate = gPlan.StartingDate.Value.Date.AddDays(tdata.LeadTime);
                        var vndr = db.mh_Vendors.Where(x => x.No == gPlan.VendorNo).FirstOrDefault();
                        if (vndr != null)
                            gPlan.EndingDate = gPlan.EndingDate.Value.AddDays(vndr.ShippingTime);
                        gPlan.EndingDate = baseClass.setStandardTime(gPlan.EndingDate.Value, false);

                        //Find Standard Unit Purchase
                        decimal StandardCost = gPlan.itemData.StandardCost;
                        string PurchaseUOM = gPlan.itemData.PurchaseUOM;

                        decimal StockQty = gPlan.itemData.QtyOnHand;
                        decimal SafetyStock = gPlan.itemData.SafetyStock;
                        decimal ReorderPoint = gPlan.itemData.ReorderPoint;
                        //Find Reorder Qty
                        decimal ReorderQty = 0.00m; //ซื้อด้วยหน่วย Base UOM
                        decimal ReqQty = Math.Round(gPlan.Qty * gPlan.PCSUnit, 2);

                        bool OrderComp = false;
                        //find from Reorder Type
                        if (gPlan.ReorderTypeEnum == ReorderType.Fixed)
                        {
                            while (!OrderComp)
                            {
                                if ((StockQty + ReorderQty) - ReqQty < SafetyStock
                                    || (StockQty + ReorderQty) - ReqQty < ReorderPoint)
                                {
                                    ReorderQty += gPlan.itemData.ReorderQty;
                                }
                                else
                                    OrderComp = true;
                            }
                        }
                        else if (gPlan.ReorderTypeEnum == ReorderType.MinMax)
                        {
                            while (!OrderComp)
                            {
                                if ((StockQty + ReorderQty) - ReqQty < SafetyStock
                                    || (StockQty + ReorderQty) - ReqQty < gPlan.itemData.MinQty)
                                {
                                    ReorderQty += gPlan.itemData.MaxQty - (StockQty + ReorderQty) - ReqQty;
                                }
                                else
                                    OrderComp = true;
                            }
                        }
                        else
                        {//By Order
                            ReorderQty += ReqQty - StockQty;
                        }
                        //แปลงเป็นหน่วยซื้อ
                        gPlan.UOM = PurchaseUOM;
                        gPlan.PCSUnit = Math.Round(gPlan.itemData.PCSUnit_PurchaseUOM, 2);
                        gPlan.Qty = Math.Round(ReorderQty / gPlan.PCSUnit, 2);
                    }
                    gPlan.DueDate = gPlan.EndingDate.Value.AddDays(1).Date;

                    gridPlans.Add(gPlan);
                    return gPlan;
                }
            }
            catch (Exception ex)
            {
                baseClass.Warning("Cal Part : " + ex.Message);
                return null;
            }
        }
        void changeLabel(string lb)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                lbStatus.Text = lb;
                this.Update();
            }));
        }


        private void PlanningCal_Status_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = startCal;
            if (startCal)
                baseClass.Warning("Waiting for calculation.\n");
        }
    }


    //1 Job --> 1 Customer P/O or 1 Sale Order, 1 FG
    //RM รวมกันได้

    public class calPartData
    {
        public string ItemNo { get; set; }
        public decimal ReqQty { get; set; }
        public string UOM { get; set; }
        public decimal PCSUnit { get; set; }
        public DateTime ReqDate { get; set; }
        public string DocNo { get; set; }
        public int DocId { get; set; }
        public ReplenishmentType repType { get; set; }

        public int mainNo { get; set; } = 0;
    }
}
