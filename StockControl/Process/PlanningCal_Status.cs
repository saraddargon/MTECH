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
        public List<grid_Planning> gridPlans = new List<grid_Planning>();
        private List<ItemData> itemDatas = new List<ItemData>();
        private List<WorkLoad> workLoads = new List<WorkLoad>();
        private DateTime? PDDate = null;
        int mainNo = 0;
        void calE()
        {
            try
            {
                gridPlans.Clear();
                itemDatas.Clear();
                workLoads.Clear();
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
                    workLoads = baseClass.getWorkLoad(dFrom);

                    //2.Loop order by Due date (Dt) then Order date (Hd) then Order id (Hd)
                    cstmPO_List = cstmPO_List.OrderBy(x => x.PODt.ReqDate)
                        .ThenBy(x => x.POHd.OrderDate).ThenBy(x => x.POHd.id).ToList();
                    foreach (var item in cstmPO_List)
                    {
                        var m = PDDate;
                        changeLabel($"Calculating... Doc no.{item.POHd.CustomerPONo} : [{item.PODt.ItemNo}] {item.PODt.ItemName}");
                        var gPlan = calPart(new calPartData
                        {
                            DocId = item.PODt.id,
                            DocNo = item.POHd.CustomerNo,
                            ItemNo = item.PODt.ItemNo,
                            repType = baseClass.getRepType(item.PODt.ReplenishmentType),
                            ReqDate = item.PODt.ReqDate,
                            ReqQty = Math.Round(item.PODt.OutPlan * item.PODt.PCSUnit, 2),
                            mainNo = mainNo
                        });
                        if (gPlan == null)
                            continue;
                        //var gPlan = calPartDemo(new calPartData
                        //{
                        //    DocId = item.PODt.id,
                        //    DocNo = item.POHd.CustomerPONo,
                        //    ItemNo = item.PODt.ItemNo,
                        //    repType = baseClass.getRepType(item.PODt.ReplenishmentType),
                        //    ReqDate = item.PODt.ReqDate,
                        //    ReqQty = item.PODt.OutPlan,
                        //    mainNo = mainNo
                        //});
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
        grid_Planning calPart(calPartData data)
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
                    t_QtyOnHand = 0;

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
                                ReqQty = b.Qty.ToDecimal() * b.PCSUnit.ToDecimal(),
                                mainNo = gPlan.mainNo,
                            };
                            calPart(cd);
                        }

                        var rmList = gridPlans.Where(x => x.idRef == gPlan.idRef).ToList();
                        DateTime tempStarting = new DateTime();
                        if (rmList.Count > 0)
                        {
                            tempStarting = rmList.OrderByDescending(x => x.DueDate).First().DueDate;
                        }
                        else
                            tempStarting = dFrom;
                        //find time in Routing...
                        var CapaUse = 0.00m;
                        var rt = db.mh_RoutingDTs.Where(x => x.RoutingId == tdata.Routeid).ToList();
                        foreach (var r in rt)
                        {
                            int idWorkCenter = r.idWorkCenter;
                            var totalCapa_All = 0.00m;
                            var SetupTime = r.SetupTime;
                            var RunTime = r.RunTime;
                            var WaitingTime = r.WaitTime;
                            totalCapa_All = SetupTime + (RunTime * gPlan.Qty) + r.WaitTime;
                            CapaUse += totalCapa_All;

                            var t_StartingDate = (DateTime?)null;
                            //find capacity Available (Workcenter) on date
                            do
                            {
                                var wl = workLoads.Where(x => x.Date >= tempStarting && x.CapacityAfter > 0
                                    && x.idWorkCenter == idWorkCenter).OrderBy(x => x.Date).FirstOrDefault();
                                if (wl == null)
                                {
                                    string mssg = "Capacity is not available, Please check Capacity Work load on Capacity Calculation (Work Centers).!!!\n";
                                    baseClass.Warning(mssg);
                                    throw new Exception(mssg);
                                }
                                tempStarting = wl.Date.Date;
                                //set Starting
                                if (t_StartingDate == null)
                                {
                                    t_StartingDate = tempStarting;
                                    if (wl.CapacityAlocate > 0)
                                    { }
                                }



                                //
                                if (CapaUse > 0)
                                    tempStarting = tempStarting.AddDays(1).Date;
                            } while (CapaUse > 0);

                        }

                        ////find Capacity Available on date
                        //do
                        //{
                        //    var wl = workLoads.Where(x => x.Date >= tempStarting && x.CapacityAfter > 0)
                        //        .OrderBy(x => x.Date).FirstOrDefault();
                        //    if(wl == null)
                        //    {
                        //        string mssg = "Capacity is not available, Please check Capacity Work load on Capacity Calculation (Work Centers).!!!\n";
                        //        baseClass.Warning(mssg);
                        //        throw new Exception(mssg);
                        //    }

                        //    if (t_StartingDate == null)
                        //        t_StartingDate = null;


                        //    //next Date
                        //    if (CapaUse > 0)
                        //        tempStarting = tempStarting.AddDays(1);
                        //}
                        //while (CapaUse > 0);
                    }
                    else
                    {
                        //Purchase
                        gPlan.StartingDate = baseClass.setStandardTime(dFrom, true);
                        gPlan.EndingDate = gPlan.StartingDate.Value.Date.AddDays(tdata.LeadTime);
                        var vndr = db.mh_Vendors.Where(x => x.No == gPlan.VendorNo).FirstOrDefault();
                        if (vndr != null)
                            gPlan.EndingDate = gPlan.EndingDate.Value.AddDays(vndr.ShippingTime);
                        gPlan.EndingDate = baseClass.setStandardTime(dTo, false);
                    }
                    gPlan.DueDate = gPlan.EndingDate.Value.AddDays(1);

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

        grid_Planning calPartDemo(calPartData data)
        {
            //demo
            //Purchase Leadtime + 7
            //Production 4Hr
            using (var db = new DataClasses1DataContext())
            {
                var tdata = itemDatas.Where(x => x.ItemNo == data.ItemNo).FirstOrDefault();
                if (tdata == null)
                {
                    tdata = new ItemData(data.ItemNo);
                    itemDatas.Add(tdata);
                }

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
                            ReqQty = Math.Round(b.Qty.ToDecimal() * b.PCSUnit.ToDecimal() * data.ReqQty.ToDecimal(), 2),
                            mainNo = gPlan.mainNo,
                        };
                        calPartDemo(cd);
                    }

                    var rmList = gridPlans.Where(x => x.idRef == gPlan.idRef).ToList();
                    DateTime tempStarting = new DateTime();
                    if (rmList.Count > 0)
                    {
                        tempStarting = rmList.OrderByDescending(x => x.DueDate).First().DueDate;
                        tempStarting = tempStarting.Date.AddDays(1);
                    }
                    else
                        tempStarting = dFrom;

                    if (PDDate != null && tempStarting > PDDate)
                        PDDate = tempStarting;

                    //find time in Routing...
                    if (PDDate == null)
                    {
                        gPlan.StartingDate = baseClass.setStandardTime(tempStarting.Date, true);
                    }
                    else
                    {
                        if (PDDate.Value.Hour == 12)
                            gPlan.StartingDate = PDDate.Value.AddHours(1);
                        else //17:00
                            gPlan.StartingDate = baseClass.setStandardTime(PDDate.Value.AddDays(1).Date, true);
                    }
                    gPlan.EndingDate = gPlan.StartingDate.Value.AddHours(4);
                    PDDate = gPlan.EndingDate;
                }
                else
                {
                    //Purchase
                    gPlan.StartingDate = baseClass.setStandardTime(dFrom, true);
                    gPlan.EndingDate = gPlan.StartingDate.Value.Date.AddDays(7);
                    gPlan.EndingDate = baseClass.setStandardTime(gPlan.EndingDate.Value.Date, false);
                }
                //gPlan.DueDate = gPlan.EndingDate.Value.AddDays(1);
                gPlan.DueDate = gPlan.EndingDate.Value;

                gridPlans.Add(gPlan);
                return gPlan;
            }
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
