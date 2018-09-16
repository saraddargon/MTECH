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

                    if (dFrom < DateTime.Now.Date)
                        dFrom = DateTime.Today;
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
                    var d = db.mh_CapacityAvailables.Where(x => x.Date >= dFrom && x.Date <= dTo).ToList();
                    foreach (var dd in d)
                    {
                        var m = capacityLoad.Where(x => x.Date == dd.Date
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
                        wl.CapacityAlocate += loadCapa;
                        wl.CapacityAlocateX += loadCapaX;
                    }

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
                            ReqQty = Math.Round(item.PODt.OutPlan * item.PODt.PCSUnit, 2),
                            mainNo = mainNo
                        });
                        if (gPlan == null)
                            continue;
                        gPlan.root = true;
                        mainNo++;
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
                    //3.Find Stock is enought ? ---> only stock q'ty not for JOB/Customer P/O
                    if (data.ReqQty <= tdata.QtyOnHand) //3.1 Stock is enought
                    {
                        tdata.QtyOnHand -= data.ReqQty;
                        return null;
                    }

                    //3.2 Stock not enought
                    var t_QtyOnHand = tdata.QtyOnHand;
                    data.ReqQty -= t_QtyOnHand;
                    tdata.QtyOnHand = 0;

                    //set data
                    var gPlan = new grid_Planning();
                    gPlan.ReqDate = data.ReqDate;
                    gPlan.idRef = data.DocId;
                    gPlan.ItemNo = data.ItemNo;
                    gPlan.ItemName = tdata.ItemName;
                    gPlan.PlanningType = tdata.RepType_enum == ReplenishmentType.Production ? "Production" : "Purchase";
                    gPlan.Qty = data.ReqQty;
                    gPlan.RefDocNo = data.DocNo;
                    gPlan.GroupType = tdata.GroupType;
                    gPlan.Type = tdata.Type;
                    gPlan.InvGroup = tdata.InvGroup;
                    gPlan.VendorNo = tdata.VendorNo;
                    gPlan.VendorName = tdata.VendorName;
                    gPlan.UOM = tdata.UOM;
                    gPlan.PCSUnit = tdata.PCSUnit;
                    gPlan.LocationItem = tdata.LocationItem;
                    gPlan.refNo = data.mainNo;
                    mainNo++;
                    gPlan.mainNo = mainNo;
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
                                ReqQty = Math.Round(b.Qty.ToDecimal() * b.PCSUnit.ToDecimal() * data.ReqQty, 2),
                                mainNo = gPlan.mainNo,
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

                        //find from Reorder Type
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
        public DateTime ReqDate { get; set; }
        public string DocNo { get; set; }
        public int DocId { get; set; }
        public ReplenishmentType repType { get; set; }

        public int mainNo { get; set; } = 0;
    }
}
