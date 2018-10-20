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
        //-2 = Recieve Safety P/O ลอย
        //-1 = Return RM
        //0 = Safety Stock from P/R(Plan)
        //>0 = Free Stock


        public DateTime dFrom { get; set; } = DateTime.Now;
        public DateTime dTo { get; set; } = DateTime.Now;
        public string ItemNo { get; set; } = "";
        public string LocationItem { get; set; } = "";
        public bool MRP { get; set; }
        public bool MPS { get; set; }

        //public PlanningCal_Status()
        //{
        //    InitializeComponent();
        //}
        public PlanningCal_Status(DateTime dFrom, DateTime dTo, string ItemNo, string Location, bool MRP, bool MPS)
        {
            InitializeComponent();
            this.dFrom = dFrom;
            this.dTo = dTo;
            this.ItemNo = ItemNo;
            this.LocationItem = Location;
            this.MRP = MRP;
            this.MPS = MPS;
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
        public List<stockReserve> sReserve = new List<stockReserve>(); //Stock ที่มีการจอง Stock Qty Free
        int mainNo = 0;
        void calE()
        {
            try
            {
                gridPlans.Clear();
                itemDatas.Clear();
                workLoads.Clear();
                capacityLoad.Clear();
                calLoad.Clear();
                sReserve.Clear();
                mainNo = 0;


                changeLabel("ค้นหา Sale Order สำหรับการคำนวณ...");
                //var cstmPO_List = new List<CustomerPOCal>();
                var listForPlan = new List<listforPlanning>();
                using (var db = new DataClasses1DataContext())
                {
                    if (this.MRP)
                    {
                        var sodt_Temp = db.mh_SaleOrderDTs.Where(x => x.Active && x.forSafetyStock && !x.genPR).ToList();
                        db.mh_SaleOrderDTs.DeleteAllOnSubmit(sodt_Temp);
                        db.SubmitChanges();
                    }

                    var temp_dTo = dTo;

                    //เตรียมข้อมูลจาก Sale ORder
                    var soDt = db.mh_SaleOrderDTs.Where(x => x.Active
                        && x.ReqDate >= dFrom && x.ReqDate <= temp_dTo
                        && x.OutPlan > 0)
                        .OrderBy(x => x.ReqDate).ThenBy(x => x.id).ToList();
                    foreach (var dt in soDt)
                    {
                        var sohd = db.mh_SaleOrders.Where(x => x.SONo == dt.SONo && x.Active).FirstOrDefault();
                        if (sohd == null) continue;
                        if (sohd.SeqStatus != 2) continue; //Only Approved

                        if (ItemNo != "" && dt.ItemNo != ItemNo) continue;
                        var t = db.mh_Items.Where(x => x.InternalNo == dt.ItemNo).FirstOrDefault();
                        if (t == null) continue;
                        if (LocationItem != "" && t.Location != LocationItem) continue;

                        //var pohd = db.mh_CustomerPOs.Where(x => x.id == dt.idCustomerPO
                        //        && x.Active).FirstOrDefault();
                        //if (pohd == null) continue;
                        if (sohd == null) continue;
                        //string docNo = pohd.CustomerPONo;
                        string docNo = sohd.SONo;

                        listForPlan.Add(new listforPlanning
                        {
                            DocId = dt.id,
                            DocNo = docNo,
                            //DocDate = pohd.OrderDate,
                            DocDate = sohd.SODate,
                            ItemNo = dt.ItemNo,
                            RepType = baseClass.getRepType(dt.ReplenishmentType),
                            ReqDate = dt.ReqDate,
                            ReqQty = dt.Qty,
                            PCSUnit = dt.PCSUnit,
                            UOM = dt.UOM,
                        });
                    }

                    //1.1 Reorder Production สำหรับเก็บ Stock ตาม Safety Stock
                    if (this.MRP)
                    {
                        changeLabel($"ค้นหา Tool(สั่งผลิตเพื่อ Safety Stock) สำหรับการคำนวน...");
                        var tools1 = db.mh_Items.Where(x => x.Active).ToList();
                        foreach (var item in tools1)
                        {
                            var tdata = new ItemData(item.InternalNo);
                            if (tdata == null) continue;
                            if (tdata.RepType_enum == ReplenishmentType.Purchase) continue;
                            if (tdata.InvGroup_enum == InventoryGroup.RM) continue;

                            var reorderQty = 0.00m;

                            //FGTEMPJOByymm-000000
                            var stockCustomerPO = tdata.QtySafety; //tdata.findStock_CustomerPO(0);
                            var stockforSafety = tdata.SafetyStockFG;
                            var stockAll = stockCustomerPO + stockforSafety;
                            if (tdata.ReorderType == ReorderType.Fixed)
                            {
                                bool comP = false;
                                while (!comP)
                                {
                                    if (stockAll + reorderQty < tdata.SafetyStock)
                                        reorderQty += tdata.ReorderQty;
                                    else
                                        comP = true;
                                }
                            }
                            else if (tdata.ReorderType == ReorderType.MinMax)
                            {
                                bool comP = false;
                                while (!comP)
                                {
                                    if (stockAll + reorderQty <= tdata.MinQty)
                                        reorderQty += tdata.MaxQty - stockAll;
                                    else
                                        comP = true;
                                }
                            }
                            else
                                reorderQty = tdata.SafetyStock - stockAll;

                            if (reorderQty == 0) continue;

                            //Create TEMP JOB P/O
                            string tempNo = $"SO-SF{DateTime.Now.ToString("yyyyMM")}-0001";
                            var so = db.mh_SaleOrders.Where(x => x.DemandType == 1 && x.Active
                                && x.SONo == tempNo).FirstOrDefault();
                            if (so == null)
                            {
                                so = new mh_SaleOrder
                                {
                                    UpdateBy = ClassLib.Classlib.User,
                                    UpdateDate = DateTime.Now,
                                    CreateDate = DateTime.Now,
                                    CreateBy = ClassLib.Classlib.User,
                                    SONo = tempNo,
                                    CustomerNo = "ForSafety",
                                    CustomerName = "ForSafety",
                                    CustomerAddress = "ForSafety",
                                    SODate = DateTime.Now,
                                    Remark = "For Safety Stock",
                                    TotalPrice = 0,
                                    Vat = false,
                                    VatA = 0,
                                    VatAmnt = 0,
                                    TotalPriceIncVat = 0,
                                    Active = true,
                                    Status = "Waiting",
                                    SendApproveBy = "",
                                    ApproveBy = "",
                                    DemandType = 1,
                                    SeqStatus = 2,
                                    ApproveDate = DateTime.Now,
                                    SandApproveDate = DateTime.Now,
                                    VatGroup = 0,
                                };
                                db.mh_SaleOrders.InsertOnSubmit(so);
                            }
                            else
                            {
                                so.UpdateBy = ClassLib.Classlib.User;
                                so.UpdateDate = DateTime.Now;
                            }
                            db.SubmitChanges();
                            var sodt = new mh_SaleOrderDT
                            {
                                ItemNo = tdata.ItemNo,
                                ItemName = tdata.ItemName,
                                LocationItem = "Warehouse",
                                OutPlan = reorderQty,
                                OutShip = reorderQty,
                                PCSUnit = tdata.PCSUnit_BaseUOM,
                                PriceIncVat = false,
                                Qty = reorderQty,
                                RefDocNo = "",
                                RefId = 0,
                                ReplenishmentType = "Production",
                                RNo = 1,
                                SONo = so.SONo,
                                UnitPrice = 0,
                                UOM = tdata.BaseUOM,
                                VatType = "",
                                Amount = 0,
                                Description = "",
                                Active = true,
                                genPR = false,
                                forSafetyStock = true,
                                OutQty = reorderQty,
                                ReqDate = dTo,
                                CustomerPartName = tdata.CustomerPartName,
                                CustomerPartNo = tdata.CustomerPartNo,
                            };
                            db.mh_SaleOrderDTs.InsertOnSubmit(sodt);
                            db.SubmitChanges();

                            listForPlan.Add(new listforPlanning
                            {
                                DocId = sodt.id,
                                DocNo = so.SONo,
                                DocDate = so.SODate,
                                ItemNo = sodt.ItemNo,
                                RepType = tdata.RepType_enum,
                                ReqDate = sodt.ReqDate,
                                ReqQty = reorderQty,
                                PCSUnit = tdata.PCSUnit_BaseUOM,
                                UOM = tdata.BaseUOM,
                            });

                            if (false)
                                BackupCoding();
                        }
                    }

                    changeLabel("เตรียมข้อมูลวันทำงาน สำหรับการคำนวน.\n");
                    ////1.1 Get work load for prepare Calculation
                    capacityLoad = db.mh_CapacityLoads.Where(x => x.Active && x.Date >= dFrom && x.Date <= dTo).ToList();

                    changeLabel("เตรียมข้อมูลวันทำงาน(2) สำหรับการคำนวน.\n");
                    ////1.2 Get Calandar Load for prepare Calculation
                    calLoad = db.mh_CalendarLoads.Where(x => x.Date >= dFrom && x.Date <= dTo).ToList();

                    //2.Loop order by Due date (Dt) then Order date (Hd) then Order id (Hd)
                    listForPlan = listForPlan.OrderBy(x => x.ReqDate).ThenBy(x => x.DocDate)
                        .ThenBy(x => x.DocId).ToList();
                    int currItem = 1;
                    int allItem = listForPlan.Count;

                    foreach (var item in listForPlan)
                    {
                        changeLabel($"คำนวน (&{currItem++}&/&{allItem})... เลขที่.{item.DocNo} : [{item.ItemNo}] {item.ItemName}");
                        var t = db.mh_Items.Where(x => x.InternalNo == item.ItemNo).FirstOrDefault();
                        var u = db.mh_ItemUOMs.Where(x => x.UOMCode == t.BaseUOM && x.ItemNo == item.ItemNo && x.Active == true).FirstOrDefault();
                        var pBase = (u == null) ? 1.00m : u.QuantityPer.ToDecimal();
                        //Purchase or Production on BaseUOM
                        var gPlan = calPart_19(new calPartData
                        {
                            DocId = item.DocId,
                            DocNo = item.DocNo,
                            ItemNo = item.ItemNo,
                            repType = item.RepType,
                            ReqDate = item.ReqDate,
                            ReqQty = Math.Round(item.ReqQty * item.PCSUnit, 2),
                            mainNo = mainNo,
                            PCSUnit = pBase,
                            UOM = t.BaseUOM,
                            alreadyJob = item.alreadyJob,
                        });
                        if (gPlan == null)
                            continue;
                        gPlan.root = true;
                        //if(gPlan.RefDocNo.Contains("FGTEMPJOB"))
                        //{
                        //    gPlan.ReqDate = gPlan.DueDate.Date.AddDays(1);
                        //    var d = db.mh_CustomerPODTs.Where(x => x.id == item.DocId).FirstOrDefault();
                        //    if(d != null)
                        //    {
                        //        d.ReqDate = gPlan.ReqDate;
                        //        db.SubmitChanges();
                        //    }
                        //}

                        mainNo++;
                    }

                    var temp_gp = new List<grid_Planning>();
                    if (this.MRP)
                    {
                        changeLabel($"Calculating... Reorder Stock");
                        //3 Find Purchase for Reorder Type
                        List<string> itemNoList = new List<string>();
                        if (false)
                            BackupCoding2();
                        //3.1 Find Purchase all items
                        var tools = db.mh_Items.Where(x => x.Active).ToList();
                        foreach (var item in tools)
                        {
                            changeLabel($"คำนวน... สั่งซื้อเพื่อเก็บ Safety Stock - Item : {item.InternalNo}");
                            string itemNo = item.InternalNo;
                            if (item.ReplenishmentType == "Production") continue; //not production on MRP
                            if (itemNoList.Any(x => x == itemNo)) continue; //not already Calculation
                            itemNoList.Add(itemNo);

                            var tdata = new ItemData(itemNo);
                            if (tdata == null) continue;

                            if (tdata.ReorderType == ReorderType.MinMax && tdata.MaxQty <= 0) continue;
                            else if (tdata.SafetyStock <= 0) continue;

                            var reorderQty = 0.00m;
                            //var stockCustomerPO = tdata.findStock_CustomerPO(0); //idCstmPO = 0 = Safety Stock
                            //var stockFree = 0;//tdata.findStock_Free();
                            //var stockAll = stockCustomerPO + stockFree;
                            var stockAll = tdata.SafetyStock + tdata.QtyBackOrderSafety;

                            if (tdata.ReorderType == ReorderType.Fixed)
                            {
                                bool comP = false;
                                while (!comP)
                                {
                                    if ((stockAll + reorderQty) < tdata.SafetyStock
                                       || (stockAll + reorderQty) < tdata.ReorderPoint)
                                        reorderQty += tdata.ReorderQty;
                                    else
                                        comP = true;
                                }
                            }
                            else if (tdata.ReorderType == ReorderType.MinMax)
                            {
                                bool comP = false;
                                while (!comP)
                                {
                                    if ((stockAll + reorderQty) <= tdata.MinQty)
                                        reorderQty += tdata.MaxQty - stockAll;
                                    else
                                        comP = true;
                                }
                            }
                            else//by Order
                            {
                                if (stockAll < tdata.SafetyStock)
                                    reorderQty += tdata.SafetyStock - stockAll;
                            }

                            if (reorderQty > 0)
                            {
                                var gp1 = newGridPlan_PurchaseAfter(this.dFrom, tdata);
                                gp1.UseQty = reorderQty;
                                gp1.UOM = tdata.PurchaseUOM;
                                gp1.PCSUnit = Math.Round(tdata.PCSUnit_PurchaseUOM, 2);
                                var q = Math.Round(reorderQty / gp1.PCSUnit, 2);
                                if (q <= 0)
                                    q = 1;
                                else
                                    q = Math.Ceiling(q);
                                if (q < tdata.STDPacking)
                                    q = tdata.STDPacking;
                                gp1.Qty = q;

                                gp1.EndingDate = gp1.StartingDate.Value.Date.AddDays(tdata.LeadTime);
                                var vndr = db.mh_Vendors.Where(x => x.No == gp1.VendorNo).FirstOrDefault();
                                if (vndr != null)
                                    gp1.EndingDate = gp1.EndingDate.Value.AddDays(vndr.ShippingTime);
                                gp1.EndingDate = baseClass.setStandardTime(gp1.EndingDate.Value.Date, false);
                                gp1.DueDate = gp1.EndingDate.Value.Date.AddDays(1);
                                gp1.ReqDate = gp1.DueDate.AddDays(1);
                                gridPlans.Add(gp1);
                            }
                        }


                        //remove MPS - Production
                        temp_gp.AddRange(gridPlans.Where(x => x.PlanningType == "Production").ToList());
                        temp_gp.ForEach(x =>
                        {
                            gridPlans.Remove(x);
                        });
                    }
                    else
                    {
                        //remove MPR - Purchase
                        temp_gp.AddRange(gridPlans.Where(x => x.PlanningType == "Purchase").ToList());
                        temp_gp.ForEach(x =>
                        {
                            gridPlans.Remove(x);
                        });
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

        //update 2018-09-19
        grid_Planning calPart_19(calPartData data)
        {
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    var tdata = itemDatas.Where(x => x.ItemNo == data.ItemNo).FirstOrDefault();
                    if (tdata == null)
                    {
                        tdata = new ItemData(data.ItemNo);
                        itemDatas.Add(tdata);
                    }

                    //3. Find stock is enought ?
                    var useQ = data.ReqQty; //Math.Round(data.ReqQty * data.PCSUnit, 2);
                    //var sumStockCstmPO = 0.00m; //Stock Qty + BackOrder = Stock All for this CUstomerPO dt
                    //var sumStockFree = 0.00m; //Stock Free not on CUstomer PO ใดๆ
                    //sumStockCstmPO = tdata.findStock_CustomerPO(data.DocId);
                    //sumStockFree = tdata.findStock_Free();

                    //if (useQ <= sumStockCstmPO + sumStockFree)
                    //{ //3.1 stock is enough not order/production but cut stock customer p/o or cut stock free
                    //    if (sumStockCstmPO > 0)
                    //        tdata.cutStock_CstmPO(data.DocId, ref useQ);
                    //    if (useQ > 0 && sumStockFree > 0)
                    //        tdata.cutStock_Free(data.DocId, ref useQ, ref sReserve);
                    //    return null; //stock มีให้ ship ไม่ต้องผลิตหรือซื้อ
                    //}

                    //3.1 Stock is enough ? 
                    if (useQ <= tdata.QtyOnHand + tdata.QtyBackOrder)
                    {
                        tdata.cutQtyOnHand(ref useQ);
                        tdata.cutBackOrder(ref useQ);
                        return null; //Stock เพียงพอ "ไม่ต้อง" ผลิต หรือสั่งซื้อเพิ่ม
                    }

                    ////3.2 stock not enought
                    //if (sumStockCstmPO > 0)
                    //    tdata.cutStock_CstmPO(data.DocId, ref useQ);
                    //if (sumStockFree > 0)
                    //    tdata.cutStock_Free(data.DocId, ref useQ, ref sReserve);
                    //data.ReqQty = useQ;

                    //3.2 Stock not enought => goto => Order or Production
                    tdata.cutQtyOnHand(ref useQ);
                    tdata.cutBackOrder(ref useQ);
                    data.ReqQty = useQ; //หลังจากหัก Stock ปัจจุบันแล้วจะได้จำนวนที่ต้อง ซื้อ หรือ ผลิต จริงๆ

                    //set data
                    var gPlan = newGridPlan(data, tdata);
                    var thisMain = mainNo;
                    if (tdata.RepType_enum == ReplenishmentType.Production)
                    {
                        //gPlan = ProductionE(gPlan, data, tdata, thisMain);
                        gPlan = ProductionE_New(gPlan, data, tdata, thisMain);
                        if (gPlan == null)
                            return null;
                        gPlan.DueDate = gPlan.EndingDate.Value.Date.AddDays(1);
                    }
                    //Purchase
                    else if (this.MRP)
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

                        decimal StockQty = gPlan.itemData.QtyOnHand + gPlan.itemData.QtyBackOrder; //gPlan.itemData.findStock_Free() + gPlan.itemData.findStock_CustomerPO(data.DocId);
                        decimal ReorderPoint = gPlan.itemData.ReorderPoint;
                        //Find Reorder Qty
                        decimal ReorderQty = 0.00m; //ซื้อด้วยหน่วย Base UOM
                        decimal ReqQty = gPlan.Qty; //คำนวนมาจาก ReqQty * PCSUnit แล้ว

                        bool OrderComp = false;
                        //find from Reorder Type
                        if (gPlan.ReorderTypeEnum == ReorderType.Fixed)
                        {
                            while (!OrderComp)
                            {
                                if ((StockQty + ReorderQty) < ReqQty)
                                    ReorderQty += gPlan.itemData.ReorderQty;
                                else
                                    OrderComp = true;
                            }
                        }
                        //ไม่ต้องคำนวนเคส Min-Max
                        else
                        {//By Order
                            ReorderQty += ReqQty - StockQty;
                        }

                        //แปลงเป็นหน่วยซื้อ
                        gPlan.UOM = PurchaseUOM;
                        gPlan.PCSUnit = Math.Round(gPlan.itemData.PCSUnit_PurchaseUOM, 2);
                        var q = Math.Round(ReorderQty / gPlan.PCSUnit, 2);
                        if (q <= 0)
                            q = 1;
                        else
                            q = Math.Ceiling(q);
                        if (q < gPlan.itemData.STDPacking) //จำนวนสั่งซื้อขั้นต่ำ
                            q = gPlan.itemData.STDPacking;
                        gPlan.Qty = q;
                        gPlan.DueDate = gPlan.EndingDate.Value.Date.AddDays(1);
                    }

                    gridPlans.Add(gPlan);
                    return gPlan;
                }
            }
            catch (Exception ex)
            {
                baseClass.Error(ex.Message);
            }
            return null;
        }
        grid_Planning ProductionE(grid_Planning gPlanN, calPartData data, ItemData tdata, int thisMain)
        {
            var gPlan = gPlanN;
            using (var db = new DataClasses1DataContext())
            {
                bool RMready = true;
                //manu Unit Time
                decimal manuTime = 1;
                var manuUnit = db.mh_ManufacturingSetups.Select(x => x.ShowCapacityInUOM).FirstOrDefault();
                if (manuUnit == 2)
                    manuTime = 60;
                else if (manuUnit == 3)
                    manuTime = (24 * 60);
                //find BOM

                var boms = db.tb_BomDTs.Where(x => x.BomNo == tdata.BomNo).ToList();
                if (boms.Count == 0)
                    RMready = false; //ถ้าไม่มี Bom จะไม่แพลน
                var bomHd = db.tb_BomHDs.Where(x => x.BomNo == tdata.BomNo).FirstOrDefault();
                if (bomHd == null)
                {
                    RMready = false;
                    return null;
                }
                //var yield = bomHd.YieldOperation.ToDecimal();
                var exYield = 100 - bomHd.YieldOperation.ToDecimal();

                //เช็คว่าทุก RM มีของพอจริงไหม ถ้าไม่พอจะไม่แพลน ถ้าพอจะแพลน
                foreach (var b in boms)
                {
                    //จะผลิตได้ก็ต่อเมื่อมี component พร้อมเท่านั้น
                    var t = itemDatas.Where(x => x.ItemNo == b.Component).FirstOrDefault();
                    if (t == null)
                    {
                        t = new ItemData(b.Component);
                        itemDatas.Add(t);
                    }

                    decimal useQ = Math.Round(b.Qty * gPlan.UseQty, 2);
                    decimal yieldItem = 0.00m;
                    if (b.chk_YieldOperation.ToBool())
                        yieldItem = Math.Ceiling((exYield / 100) * useQ);
                    useQ += yieldItem;
                    decimal useQAll = Math.Round(useQ * b.PCSUnit.ToDecimal(), 2);
                    var sumStockCstmPO = 0.00m; //Stock Qty + BackOrder = Stock All for this CUstomerPO dt
                    var sumStockFree = 0.00m; //Stock Free not on CUstomer PO ใดๆ
                    sumStockCstmPO = t.findStock_CustomerPO(data.DocId);
                    sumStockFree = t.findStock_Free();
                    if (useQAll <= sumStockCstmPO + sumStockFree) //RM พอ
                    { //3.1 stock is enough can production
                        if (sumStockCstmPO > 0)
                            t.cutStock_CstmPO(data.DocId, ref useQAll);
                        if (useQAll > 0 && sumStockFree > 0)
                            t.cutStock_Free(data.DocId, ref useQAll, ref sReserve);
                    }
                    else
                    { //RM ไม่พอ ไม่ผลิต แต่ต้องคำนวนสั่งซื้อ หรือผลิต
                        var tool = db.mh_Items.Where(x => x.InternalNo == b.Component).FirstOrDefault();
                        var cd = new calPartData
                        {
                            DocId = data.DocId,
                            DocNo = data.DocNo,
                            ItemNo = b.Component,
                            repType = baseClass.getRepType(tool.ReplenishmentType),
                            ReqDate = data.ReqDate,
                            ReqQty = useQAll,
                            mainNo = gPlan.mainNo,
                            UOM = t.BaseUOM,
                            PCSUnit = t.PCSUnit_BaseUOM,
                        };
                        calPart_19(cd);
                        RMready = false;
                    }
                }

                if (data.alreadyJob) return null; //สร้าง job ไปแล้วไม่ทำ plan
                if (!RMready) return null; //RM ไม่พอไม่ทำ plan
                if (this.MRP) return null; //ไม่คำนวน production เพราะทำแค่ MRP

                DateTime? finalStartingDate = null; //วันเริ่มงานจริงๆ
                DateTime? finalEndingDate = null; //วันสิ้นสุด

                DateTime? tempStarting = null;

                //find Duedate from P/O, P/R
                var pr = db.mh_PurchaseRequestLines.Where(x => x.SS == 1 && x.idCstmPODt != null && x.idCstmPODt == data.DocId)
                    .Join(db.mh_PurchaseRequests.Where(x => x.Status != "Cancel")
                    , dt => dt.PRNo
                    , hd => hd.PRNo
                    , (dt, hd) => new { hd, dt }).ToList();
                foreach (var p in pr)
                {
                    //already Create P/O
                    if (p.dt.RefPOid > 0)
                    {
                        var po = db.mh_PurchaseOrderDetails.Where(x => x.id == p.dt.RefPOid && x.SS == 1)
                            .Join(db.mh_PurchaseOrders.Where(x => x.Status != "Cancel")
                            , dt => dt.PONo
                            , hd => hd.PONo
                            , (dt, hd) => new { hd, dt }).ToList();
                        if (po.Count > 0)
                        {
                            var tdate = tempStarting = po.Max(x => x.dt.DeliveryDate.Value.Date);
                            if (tempStarting == null || tempStarting < tdate)
                                tempStarting = tdate;
                        }
                        else
                        {//not found P/O
                            var tdate = tempStarting = p.dt.DeliveryDate.Value.Date;
                            if (tempStarting == null || tempStarting < tdate)
                                tempStarting = tdate;
                        }
                    }
                    //not Create P/O
                    else
                    {
                        var tdate = tempStarting = p.dt.DeliveryDate.Value.Date;
                        if (tempStarting == null || tempStarting < tdate)
                            tempStarting = tdate;
                    }
                }
                //find Duedate from SEMI
                var prod = db.mh_ProductionOrders.Where(x => x.Active && x.RefDocId == data.DocId).ToList();
                if (prod.Count > 0)
                {
                    var sDate = prod.Max(x => x.EndingDate).Date.AddDays(1);
                    if (tempStarting < sDate)
                        tempStarting = sDate;
                }

                //วน Routing ของ Item นั้น
                var rt = db.mh_RoutingDTs.Where(x => x.RoutingId == tdata.Routeid && x.Active)
                    .Join(db.mh_WorkCenters.Where(x => x.Active)
                    , hd => hd.idWorkCenter
                    , workcenter => workcenter.id
                    , (hd, workcenter)
                    => new { hd, hd.idWorkCenter, hd.id, hd.SetupTime, hd.RunTime, hd.WaitTime, workcenter })
                    .ToList();

                foreach (var r in rt)
                {
                    int idWorkCenter = r.idWorkCenter;
                    //decimal CapacityOfWorkCenter = r.workcenter.Capacity;
                    var totalCapa_All = 0.00m;
                    var SetupTime = r.SetupTime * manuTime; //แปลงเป็นนาทีเสมอ
                    var RunTime = r.RunTime * manuTime;
                    var RunTimeCapa = Math.Round(((RunTime * gPlan.UseQty) / r.workcenter.Capacity), 2);
                    var WaitingTime = r.WaitTime * manuTime;
                    totalCapa_All = SetupTime + RunTimeCapa + r.WaitTime;
                    var CapaUseX = 0.00m;
                    var CapaUse = 0.00m; //Capacity ไม่รวม WaitTime
                    CapaUseX = totalCapa_All; //เวลาการทำงานที่ถูกใช้ทั้งหมดใน Workcenter นี้
                    CapaUse = SetupTime + Math.Round(RunTime * gPlan.UseQty, 2);

                    if (tempStarting == null) //เริ่มจากวันที่ลูกค้าเลือก period หรือยังหาวันเริ่มไม่ได้
                        tempStarting = dFrom;

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
                                        timeEnd = timeStart.Add(TimeSpan.FromMinutes(Math.Round(CapaUseX.ToDouble(), 2)));

                                        var capaLoad = baseClass.newCapaLoad(CapaUseX, CapaUse, tempStarting.Value.Date, thisMain, 0, idWorkCenter);
                                        capacityLoad.Add(capaLoad);

                                        wl.CapacityAlocateX += CapaUseX;
                                        wl.CapacityAlocate += CapaUse;
                                        CapaUseX = 0;
                                        CapaUse = 0;

                                        var cl = baseClass.newCalendar(autoid, r.id, idWorkCenter, idCalendar, tempStarting.Value.Date, timeStart, timeEnd, thisMain, -1);
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
                                        var capaLoad = baseClass.newCapaLoad(diffTime, tCapa, tempStarting.Value.Date, thisMain, 0, idWorkCenter);
                                        capacityLoad.Add(capaLoad);

                                        var cl = baseClass.newCalendar(autoid, r.id, idWorkCenter, idCalendar, tempStarting.Value.Date, timeStart, timeEnd, thisMain, -1);
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

                                    var capaload = baseClass.newCapaLoad(diffTime, tCapa, tempStarting.Value.Date, thisMain, 0, idWorkCenter);
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

                                    var capaLoad = baseClass.newCapaLoad(diffTime, tCapa, tempStarting.Value.Date, thisMain, 0, idWorkCenter);
                                    capacityLoad.Add(capaLoad);
                                }

                                if (foundTime)
                                {
                                    var cl = baseClass.newCalendar(autoid, r.id, idWorkCenter, idCalendar, tempStarting.Value.Date, timeStart, timeEnd, thisMain, -1);
                                    calLoad.Add(cl);
                                }
                            }

                            tempStarting = tempStarting.Value.Date.SetTimeToDate(timeEnd);
                            //tempStarting = tempStarting.Value.Date.AddHours(timeEnd.Hours).AddMinutes(timeEnd.Minutes).AddMilliseconds(timeEnd.Milliseconds);
                            finalEndingDate = tempStarting.Value;
                        }
                        else //วันดังกล่าวไม่ใช่ Working Day
                        {
                            string mssg = "Work center not having Working days.!!!\n";
                            baseClass.Warning(mssg);
                            throw new Exception(mssg);
                        }


                    } while (CapaUseX > 0);

                    gPlan.StartingDate = finalStartingDate;
                    gPlan.EndingDate = finalEndingDate;
                }
            }

            return gPlan;
        }


        //update 2018-10-17
        grid_Planning ProductionE_New(grid_Planning gPlanN, calPartData data, ItemData tdata, int thisMain)
        {

            var gPlan = gPlanN;
            using (var db = new DataClasses1DataContext())
            {
                bool RMready = true;
                ////manu Unit Time
                //decimal manuTime = 1;
                //var manuUnit = db.mh_ManufacturingSetups.Select(x => x.ShowCapacityInUOM).FirstOrDefault();
                //if (manuUnit == 2)
                //    manuTime = 60;
                //else if (manuUnit == 3)
                //    manuTime = (24 * 60);
                //find BOM

                var boms = db.tb_BomDTs.Where(x => x.BomNo == tdata.BomNo).ToList();
                if (boms.Count == 0)
                    RMready = false; //ถ้าไม่มี Bom จะไม่แพลน
                var bomHd = db.tb_BomHDs.Where(x => x.BomNo == tdata.BomNo).FirstOrDefault();
                if (bomHd == null)
                {
                    RMready = false;
                    return null;
                }
                //var yield = bomHd.YieldOperation.ToDecimal();
                var exYield = 100 - bomHd.YieldOperation.ToDecimal();

                //เช็คว่าทุก RM มีของพอจริงไหม ถ้าไม่พอจะไม่แพลน ถ้าพอจะแพลน
                foreach (var b in boms)
                {
                    //จะผลิตได้ก็ต่อเมื่อมี component พร้อมเท่านั้น
                    var t = itemDatas.Where(x => x.ItemNo == b.Component).FirstOrDefault();
                    if (t == null)
                    {
                        t = new ItemData(b.Component);
                        itemDatas.Add(t);
                    }

                    decimal useQ = Math.Round(b.Qty * gPlan.UseQty, 2); //จำนวน Component ที่ใช้
                    decimal yieldItem = 0.00m;
                    if (b.chk_YieldOperation.ToBool())
                        yieldItem = Math.Ceiling((exYield / 100) * useQ);
                    useQ += yieldItem;
                    decimal useQAll = (useQ * b.PCSUnit.ToDecimal()).Round2(); //จำนวนที่ต้องใช้ + โอกาสที่จะสูญเสีย RM

                    if (false)
                        BackupCoding3();

                    var QtyOnHand = t.QtyOnHand; //มองว่าเป็น Stock ทั่วไป
                    var QtyBackOrder = t.QtyBackOrder; //มองว่าเป็น BackOrder
                    if (useQAll <= QtyOnHand + QtyBackOrder)
                    {
                        gPlan.RM_BackOrder = (useQ > QtyOnHand);
                        //RM พอ ผลิตได้
                        t.cutQtyOnHand(ref useQAll);
                        t.cutBackOrder(ref useQAll);
                    }
                    else
                    { //RM ไม่พอ ผลิตไม่ได้ ต้องไปสั่งซื้อหรือผลิตเพิ่ม
                        var tool = db.mh_Items.Where(x => x.InternalNo == b.Component).FirstOrDefault();
                        var cd = new calPartData
                        {
                            DocId = data.DocId,
                            DocNo = data.DocNo,
                            ItemNo = b.Component,
                            repType = baseClass.getRepType(tool.ReplenishmentType),
                            ReqDate = data.ReqDate,
                            ReqQty = useQAll,
                            mainNo = gPlan.mainNo,
                            UOM = t.BaseUOM,
                            PCSUnit = t.PCSUnit_BaseUOM,
                        };
                        calPart_19(cd);
                        RMready = false;
                    }
                }

                if (data.alreadyJob) return null; //สร้าง job ไปแล้วไม่ทำ plan
                if (!RMready) return null; //RM ไม่พอไม่ทำ plan
                if (this.MRP) return null; //ไม่คำนวน production เพราะทำแค่ MRP

                DateTime? finalStartingDate = null; //วันเริ่มงานจริงๆ
                DateTime? finalEndingDate = null; //วันสิ้นสุด

                DateTime? tempStarting = null;
                //วน Routing ของ Item นั้น
                var rt = db.mh_RoutingDTs.Where(x => x.RoutingId == tdata.Routeid && x.Active)
                    .Join(db.mh_WorkCenters.Where(x => x.Active)
                    , hd => hd.idWorkCenter
                    , workcenter => workcenter.id
                    , (hd, workcenter)
                    => new { hd, hd.idWorkCenter, hd.id, hd.SetupTime, hd.RunTime, hd.WaitTime, workcenter })
                    .ToList();
                //หา Capa Hr ที่น้อยที่สุด
                var minCapa = 0.00m; //Capa น้อยสุดต่อนาที
                var setupAll = 0.00m;
                if (rt.Count > 0)
                {
                    var minCapaHr = rt.Min(x => x.workcenter.CapacityHour);
                    minCapa = Math.Round(minCapaHr / 60, 9);
                    setupAll = rt.Sum(x => x.SetupTime);

                    //หาว่าต้องใช้กี่นาที ในการผลิต ReqQty ตัว
                    var totalCapaAll = Math.Round(gPlan.UseQty / minCapa, 9);
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
                        {
                            ttCapa = item.workcenter.CapacityHour;
                            idWorkCenter = item.idWorkCenter;
                        }
                    }
                    if (idWorkCenter == 0) return null;
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
                                                var capaLoad = baseClass.newCapaLoad(CapaUseX, CapaUse, tempStarting.Value.Date, thisMain, 0, r.idWorkCenter);
                                                capacityLoad.Add(capaLoad);
                                            }

                                            wl.CapacityAlocateX += CapaUseX;
                                            wl.CapacityAlocate += CapaUse;
                                            CapaUseX = 0;
                                            CapaUse = 0;

                                            //ใส่ทุก work ด้วยเวลาเท่ากัน
                                            foreach (var r in rt)
                                            {
                                                var cl = baseClass.newCalendar(autoMe(), r.id, r.idWorkCenter, idCalendar, tempStarting.Value.Date, timeStart, timeEnd, thisMain, -1);
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
                                                var capaLoad = baseClass.newCapaLoad(diffTime, tCapa, tempStarting.Value.Date, thisMain, 0, r.idWorkCenter);
                                                capacityLoad.Add(capaLoad);

                                                var cl = baseClass.newCalendar(autoMe(), r.id, r.idWorkCenter, idCalendar, tempStarting.Value.Date, timeStart, timeEnd, thisMain, -1);
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
                                        var capaload = baseClass.newCapaLoad(diffTime, tCapa, tempStarting.Value.Date, thisMain, 0, r.idWorkCenter);
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
                                        var capaLoad = baseClass.newCapaLoad(diffTime, tCapa, tempStarting.Value.Date, thisMain, 0, r.idWorkCenter);
                                        capacityLoad.Add(capaLoad);
                                    }
                                }

                                if (foundTime)
                                {
                                    foreach (var r in rt)
                                    {
                                        var cl = baseClass.newCalendar(autoMe(), r.id, r.idWorkCenter, idCalendar, tempStarting.Value.Date, timeStart, timeEnd, thisMain, -1);
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
                            //tempStarting = tempStarting.Value.Date.AddHours(timeEnd.Hours).AddMinutes(timeEnd.Minutes).AddMilliseconds(timeEnd.Milliseconds);
                            //if (firstD)
                            //{
                            //    finalStartingDate = tempStarting.Value.Date.SetTimeToDate(timeStart);
                            //    firstD = false;
                            //}
                            finalEndingDate = tempStarting.Value;
                        }
                        else //วันดังกล่าวไม่ใช่ Working Day
                        {
                            string mssg = "Work center not having Working days.!!!\n";
                            baseClass.Warning(mssg);
                            throw new Exception(mssg);
                        }


                    } while (Math.Round(CapaUseX, 2) > 0);
                    gPlan.StartingDate = finalStartingDate;
                    gPlan.EndingDate = finalEndingDate;
                }
            }


            return gPlan;
        }

        private grid_Planning newGridPlan(calPartData data, ItemData tdata)
        {
            grid_Planning gPlan = new grid_Planning();
            gPlan.ReqDate = data.ReqDate;
            gPlan.idRef = data.DocId;
            gPlan.ItemNo = data.ItemNo;
            gPlan.ItemName = tdata.ItemName;
            gPlan.PlanningType = tdata.RepType_enum == ReplenishmentType.Production ? "Production" : "Purchase";
            gPlan.Qty = data.ReqQty; //Qty จริงๆที่ต้องใช้เพราะใช้ตาม BaseUOM (PCSUnit:1)
            gPlan.UseQty = data.ReqQty;
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
            return gPlan;
        }
        private grid_Planning newGridPlan_PurchaseAfter(DateTime ReqDate, ItemData itemData)

        {
            grid_Planning gp1 = new grid_Planning();
            gp1.ReqDate = ReqDate.AddDays(1);
            gp1.idRef = 0;
            gp1.ItemNo = itemData.ItemNo;
            gp1.ItemName = itemData.ItemName;
            gp1.PlanningType = "Purchase";
            gp1.RefDocNo = "Safety stock(Purchase)";
            gp1.GroupType = itemData.GroupType;
            gp1.Type = itemData.Type;
            gp1.InvGroup = itemData.InvGroup;
            gp1.ReorderTypeEnum = itemData.ReorderType;
            gp1.VendorNo = itemData.VendorNo;
            gp1.VendorName = itemData.VendorName;
            gp1.UOM = itemData.UOM;
            gp1.PCSUnit = itemData.PCSUnit;
            gp1.LocationItem = itemData.LocationItem;
            gp1.refNo = 0;
            gp1.mainNo = 0;
            gp1.itemData = itemData;
            //gp1.StartingDate = baseClass.setStandardTime(gp1.ReqDate, true);
            //gp1.EndingDate = baseClass.setStandardTime(gp1.ReqDate, false);
            gp1.StartingDate = baseClass.setStandardTime(ReqDate, true);
            gp1.EndingDate = baseClass.setStandardTime(ReqDate, false);
            return gp1;
        }
        private grid_Planning newGridPlan_PurchaseAfter(grid_Planning gp, DateTime ReqDate, ItemData itemData)
        {
            grid_Planning gp1 = new grid_Planning();
            gp1.ReqDate = ReqDate.AddDays(1);
            gp1.idRef = 0;
            gp1.ItemNo = gp.ItemNo;
            gp1.ItemName = gp.ItemName;
            gp1.PlanningType = "Purchase";
            gp1.RefDocNo = "Safety stock(Purchase)";
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
            return gp1;
        }

        //idRef :: 0 :: Safety stock


        void changeLabel(string lb)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                lbStatus.Text = lb;
                this.Update();
            }));
        }

        decimal findQtyBackorder(ItemData data, int idCustomerPODt)
        {
            decimal sumBackOrder = 0.00m;
            if (idCustomerPODt == 4)
            { }

            using (var db = new DataClasses1DataContext())
            {
                var prAll = db.mh_PurchaseRequestLines.Where(x => x.SS == 1 && x.CodeNo == data.ItemNo && x.idCstmPODt == idCustomerPODt)
                        .Join(db.mh_PurchaseRequests.Where(x => x.Status != "Cancel")
                        , dt => dt.PRNo
                        , hd => hd.PRNo
                        , (dt, hd) => new { hd, dt }).ToList();
                foreach (var pr in prAll)
                {
                    var poAll = db.mh_PurchaseOrderDetails.Where(x => x.SS == 1 && x.PRItem == pr.dt.id)
                        .Join(db.mh_PurchaseOrders.Where(x => x.Status != "Cancel")
                        , dt => dt.PONo
                        , hd => hd.PONo
                        , (dt, hd) => new { hd, dt })
                        .ToList();
                    if (poAll.Count > 0)
                        sumBackOrder += poAll.Sum(x => Math.Round(x.dt.BackOrder.ToDecimal() * x.dt.PCSUnit.ToDecimal(), 2));
                    else
                        sumBackOrder += Math.Round(pr.dt.OrderQty * pr.dt.PCSUOM, 2);
                }
            }

            return sumBackOrder;
        }


        int autoMe()
        {
            int autoid = 0;
            do
            {
                var rd = new Random();
                autoid = rd.Next(-999999, -100);
            } while (calLoad.Where(x => x.id == autoid).Count() > 0);
            return autoid;
        }


        private void PlanningCal_Status_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = startCal;
            if (startCal)
                baseClass.Warning("Waiting for calculation.\n");
        }




        void BackupCoding()
        {
            //var po = db.mh_CustomerPOs.Where(x => x.DemandType == 1 
            //&& x.Active && x.CustomerPONo == tempNo).FirstOrDefault();
            //if (po == null)
            //{
            //po = new mh_CustomerPO();
            //po.CustomerPONo = tempNo;
            //po.CustomerNo = "ForSafety";
            //po.Active = true;
            //po.CreateBy = ClassLib.Classlib.User;
            //po.CreateDate = DateTime.Now;
            //po.DemandType = 1;
            //po.OrderDate = DateTime.Now;
            //po.Remark = "Reorder for FG Safety stock";
            //po.UpdateBy = ClassLib.Classlib.User;
            //po.UpdateDate = DateTime.Now;
            //db.mh_CustomerPOs.InsertOnSubmit(po);
            //db.SubmitChanges();
            //}
            //
            //var podt = new mh_CustomerPODT
            //{
            //    Active = true,
            //    Amount = Math.Round(reorderQty * tdata.StandardCost, 2),
            //    forSafetyStock = true,
            //    genPR = false,
            //    idCustomerPO = po.id,
            //    ItemName = tdata.ItemName,
            //    ItemNo = tdata.ItemNo,
            //    OutPlan = reorderQty,
            //    OutQty = reorderQty,
            //    OutSO = reorderQty,
            //    PCSUnit = tdata.PCSUnit_BaseUOM,
            //    Qty = reorderQty,
            //    Remark = "Reorder for Safety Stock",
            //    ReplenishmentType = "Production",
            //    ReqDate = dTo,
            //    ReqReceiveDate = dTo,
            //    Status = "Waiting",
            //    UnitPrice = tdata.StandardCost,
            //    UOM = tdata.BaseUOM,
            //};
            //db.mh_CustomerPODTs.InsertOnSubmit(podt);
            //db.SubmitChanges();
            //
            //listForPlan.Add(new listforPlanning
            //{
            //    DocId = podt.id,
            //    DocNo = po.CustomerPONo,
            //    DocDate = po.OrderDate,
            //    ItemNo = tdata.ItemNo,
            //    RepType = tdata.RepType_enum,
            //    ReqDate = podt.ReqDate,
            //    ReqQty = reorderQty,
            //    PCSUnit = tdata.PCSUnit_BaseUOM,
            //    UOM = tdata.BaseUOM,
            //});
        }
        void BackupCoding2()
        {
            //var tempGridplan = gridPlans.Where(x => x.PlanningType == "Purchase").ToList();
            //foreach (var gp in tempGridplan)
            //{
            //    changeLabel($"Calculating... Reorder Stock Item : {gp.ItemNo}");
            //    if (itemNoList.Any(x => x == gp.ItemNo)) continue;
            //    itemNoList.Add(gp.ItemNo);

            //    var qItem = gridPlans.Where(x => x.ItemNo == gp.ItemNo).ToList();
            //    var itemData = qItem.First().itemData;

            //    if (itemData.ReorderType == ReorderType.MinMax && itemData.MaxQty <= 0)
            //        continue;
            //    else if (itemData.SafetyStock <= 0)
            //        continue;

            //    decimal sum_UseQty = qItem.Sum(x => x.UseQty);// Qty ที่ถูกใช้ไปจริงๆ
            //    decimal sum_Reorder = qItem.Sum(x => x.Qty);//Qty ที่ถูกสั่งซื้อจากการแพลน
            //    decimal QtyOnHand_Backup = qItem.Select(x => x.itemData.findStock_CustomerPO(-99) + x.itemData.findStock_Free()).First(); //qItem.Select(x => x.itemData.QtyOnHand_Backup).First();
            //    decimal ReorderQty = 0.00m;
            //    var mDate = gridPlans.Max(x => x.EndingDate.Value.Date); //max date ของการ Plan ครั้งนี้
            //    var gp1 = newGridPlan_PurchaseAfter(gp, mDate.AddDays(1), itemData);

            //    if (itemData.ReorderType == ReorderType.Fixed)
            //    {
            //        bool comP = false;
            //        while (!comP)
            //        {
            //            if ((QtyOnHand_Backup + sum_Reorder + ReorderQty) - sum_UseQty < itemData.SafetyStock
            //                || (QtyOnHand_Backup + sum_Reorder + ReorderQty) - sum_UseQty < itemData.ReorderPoint)
            //            {
            //                ReorderQty += itemData.ReorderQty;
            //            }
            //            else
            //                comP = true;
            //        }
            //    }
            //    else if (itemData.ReorderType == ReorderType.MinMax)
            //    {
            //        bool comP = false;
            //        while (!comP)
            //        {
            //            if ((QtyOnHand_Backup + sum_Reorder + ReorderQty) - sum_UseQty <= itemData.MinQty)
            //                ReorderQty += itemData.MaxQty - (QtyOnHand_Backup + sum_Reorder + ReorderQty - sum_UseQty);
            //            else if ((QtyOnHand_Backup + sum_Reorder + ReorderQty) - sum_UseQty < itemData.SafetyStock)
            //                ReorderQty += itemData.SafetyStock - (QtyOnHand_Backup + sum_Reorder + ReorderQty - sum_UseQty);
            //            else
            //                comP = true;
            //        }
            //    }
            //    else
            //    {//By Order
            //        if ((QtyOnHand_Backup + sum_Reorder) - sum_UseQty < itemData.SafetyStock)
            //        {
            //            ReorderQty = itemData.SafetyStock - (QtyOnHand_Backup + sum_Reorder - sum_UseQty);
            //        }
            //    }
            //    //แปลงเป็นหน่วยซื้อ
            //    gp1.UseQty = ReorderQty;
            //    gp1.UOM = itemData.PurchaseUOM;
            //    gp1.PCSUnit = Math.Round(itemData.PCSUnit_PurchaseUOM, 2);
            //    var q = Math.Round(ReorderQty / gp1.PCSUnit, 2);
            //    if (q <= 0)
            //        q = 1;
            //    else
            //        q = Math.Ceiling(q);
            //    gp1.Qty = q;

            //    gp1.DueDate = gp1.EndingDate.Value.Date.AddDays(1);
            //    gp1.ReqDate = gp1.DueDate.AddDays(1);
            //    gridPlans.Add(gp1);
            //}
        }
        void BackupCoding3()
        {

            //var sumStockCstmPO = 0.00m; //Stock Qty + BackOrder = Stock All for this CUstomerPO dt
            //var sumStockFree = 0.00m; //Stock Free not on CUstomer PO ใดๆ
            //sumStockCstmPO = t.findStock_CustomerPO(data.DocId);
            //sumStockFree = t.findStock_Free();
            //if (useQAll <= sumStockCstmPO + sumStockFree) //RM พอ
            //{ //3.1 stock is enough can production
            //    var tStock = sumStockCstmPO + sumStockFree - t.findBackOrder_CustomerPO(data.DocId);
            //    gPlan.RM_BackOrder = (useQAll > tStock);

            //    if (sumStockCstmPO > 0)
            //        t.cutStock_CstmPO(data.DocId, ref useQAll);
            //    if (useQAll > 0 && sumStockFree > 0)
            //        t.cutStock_Free(data.DocId, ref useQAll, ref sReserve);
            //}
            //else
            //{ //RM ไม่พอ ไม่ผลิต แต่ต้องคำนวนสั่งซื้อ หรือผลิต
            //    var tool = db.mh_Items.Where(x => x.InternalNo == b.Component).FirstOrDefault();
            //    var cd = new calPartData
            //    {
            //        DocId = data.DocId,
            //        DocNo = data.DocNo,
            //        ItemNo = b.Component,
            //        repType = baseClass.getRepType(tool.ReplenishmentType),
            //        ReqDate = data.ReqDate,
            //        ReqQty = useQAll,
            //        mainNo = gPlan.mainNo,
            //        UOM = t.BaseUOM,
            //        PCSUnit = t.PCSUnit_BaseUOM,
            //    };
            //    calPart_19(cd);
            //    RMready = false;
            //}

        }
    }


    ////1 Job --> 1 Customer P/O or 1 Sale Order, 1 FG
    ////RM รวมกันได้

    ////cal Part แบบ Gen P/R และ Production plan เฉพาะที่มี RM พอ
    //grid_Planning calPart(calPartData data)

    //{
    //    try
    //    {
    //        //RepType == Production, Purchase
    //        using (var db = new DataClasses1DataContext())
    //        {
    //            var tdata = itemDatas.Where(x => x.ItemNo == data.ItemNo).FirstOrDefault();
    //            if (tdata == null)
    //            {
    //                tdata = new ItemData(data.ItemNo);
    //                itemDatas.Add(tdata);
    //            }

    //            //Find Backorder ref id Customer in mh_PuchaseOrderDT (ถ้าเปิด PO แล้ว) หรือ จาก mh_PurchaseReqeustLine(ถ้ายังไม่เปิด P/O)
    //            decimal sumBackOrder = findQtyBackorder(tdata, data.DocId);

    //            //3.Find Stock is enought ? ---> only stock q'ty not for JOB/Customer P/O
    //            if (data.ReqQty <= tdata.QtyOnHand + sumBackOrder) //3.1 Stock is enought
    //            {
    //                //tdata.QtyOnHand -= data.ReqQty;
    //                if (tdata.QtyOnHand >= data.ReqQty)
    //                    tdata.QtyOnHand -= data.ReqQty;
    //                else
    //                    tdata.QtyOnHand = 0;
    //                return null;
    //            }

    //            //3.2 Stock + Back order not enought
    //            data.ReqQty -= tdata.QtyOnHand;
    //            data.ReqQty -= sumBackOrder;
    //            tdata.QtyOnHand = 0;
    //            sumBackOrder = 0;

    //            //set data
    //            var gPlan = newGridPlan(data, tdata);
    //            var thisMain = mainNo;

    //            bool RMready = true;
    //            //set Production or Purchase
    //            if (tdata.RepType_enum == ReplenishmentType.Production)
    //            {
    //                //manu Unit Time
    //                decimal manuTime = 1;
    //                var manuUnit = db.mh_ManufacturingSetups.Select(x => x.ShowCapacityInUOM).FirstOrDefault();
    //                if (manuUnit == 2)
    //                    manuTime = 60;
    //                else if (manuUnit == 3)
    //                    manuTime = (24 * 60);
    //                //Prodction
    //                //find BOM
    //                var boms = db.tb_BomDTs.Where(x => x.PartNo == gPlan.ItemNo).ToList();
    //                if (boms.Count == 0)
    //                    RMready = false;
    //                //เช็คว่าทุก RM มีของพอจริงไหม
    //                foreach (var b in boms)
    //                {
    //                    //จะผลิตได้ก็ต่อเมื่อมี component พร้อมเท่านั้น
    //                    var t = itemDatas.Where(x => x.ItemNo == b.Component).FirstOrDefault();
    //                    if (t == null)
    //                        t = new ItemData(b.Component);

    //                    decimal useCompoQty = b.Qty * gPlan.UseQty;
    //                    if (t.QtyOnHand >= useCompoQty) // RM พอ
    //                        t.QtyOnHand -= useCompoQty;
    //                    else
    //                    { //RM ไม่พอ ไม่ผลิต แต่ต้องคำนวนสั่งซื้อ
    //                        var tool = db.mh_Items.Where(x => x.InternalNo == b.Component).FirstOrDefault();
    //                        var cd = new calPartData
    //                        {
    //                            DocId = data.DocId,
    //                            DocNo = data.DocNo,
    //                            ItemNo = b.Component,
    //                            repType = baseClass.getRepType(tool.ReplenishmentType),
    //                            ReqDate = data.ReqDate,
    //                            ReqQty = useCompoQty,
    //                            mainNo = gPlan.mainNo,
    //                            UOM = b.Unit,
    //                            PCSUnit = b.PCSUnit.ToDecimal(),
    //                        };
    //                        calPart(cd);
    //                        RMready = false;
    //                    }
    //                }

    //                if (data.alreadyJob) return null;
    //                if (!RMready) return null;

    //                ////BEgin Production
    //                //var rmList = gridPlans.Where(x => x.idRef == gPlan.idRef).ToList();
    //                DateTime tempStarting = new DateTime();
    //                //if (rmList.Count > 0)
    //                //{
    //                //    tempStarting = rmList.OrderByDescending(x => x.DueDate).First().DueDate.Date;
    //                //}
    //                //else
    //                //    tempStarting = dFrom;
    //                tempStarting = dFrom;

    //                //find time in Routing...
    //                var rt = db.mh_RoutingDTs.Where(x => x.RoutingId == tdata.Routeid)
    //                    .Join(db.mh_WorkCenters.Where(x => x.Active)
    //                    , hd => hd.idWorkCenter
    //                    , workcenter => workcenter.id
    //                    , (hd, workcenter)
    //                    => new { hd, hd.idWorkCenter, hd.id, hd.SetupTime, hd.RunTime, hd.WaitTime, workcenter })
    //                    .ToList();
    //                var t_StartingDate = (DateTime?)null;
    //                var t_EndingDate = (DateTime?)null;
    //                bool firstStart = false;
    //                foreach (var r in rt)
    //                {
    //                    if (t_EndingDate != null)
    //                        tempStarting = t_EndingDate.Value;
    //                    int idWorkCenter = r.idWorkCenter;
    //                    var totalCapa_All = 0.00m;
    //                    var SetupTime = r.SetupTime * manuTime;
    //                    var RunTime = r.RunTime * manuTime;
    //                    var RunTimeCapa = Math.Round(((RunTime * gPlan.Qty) / r.workcenter.Capacity), 2);
    //                    var WaitingTime = r.WaitTime * manuTime;
    //                    totalCapa_All = SetupTime + RunTimeCapa + r.WaitTime;
    //                    var CapaUseX = 0.00m;
    //                    CapaUseX = totalCapa_All;

    //                    //find capacity Available (Workcenter) on date
    //                    do
    //                    {
    //                        //1. หาว่าเวลาเริ่มสามารถใช้ได้ไหม
    //                        var wl = workLoads.Where(x => x.Date >= tempStarting.Date && x.CapacityAfterX > 0
    //                            && x.idWorkCenter == idWorkCenter).OrderBy(x => x.Date).FirstOrDefault();
    //                        if (wl == null)
    //                        {
    //                            var w = baseClass.getWorkLoad(tempStarting.Date, null, idWorkCenter).Where(x => x.CapacityAfterX > 0).FirstOrDefault();
    //                            if (w == null)
    //                            {
    //                                string mssg = "Capacity is not available, Please check Capacity Work load on Capacity Calculation (Work Centers).!!!\n";
    //                                baseClass.Warning(mssg);
    //                                throw new Exception(mssg);
    //                            }
    //                            else
    //                            {
    //                                workLoads.Add(w);
    //                                wl = w;
    //                            }
    //                        }
    //                        if (tempStarting == null || wl.Date.Date > tempStarting.Date)
    //                            tempStarting = wl.Date.Date;

    //                        //set Starting
    //                        int dow = baseClass.getDayOfWeek(tempStarting.DayOfWeek);
    //                        //
    //                        var wd = db.mh_WorkCenters.Where(x => x.id == wl.idWorkCenter)
    //                            .Join(db.mh_WorkingDays.Where(x => x.Day == dow && x.Active)
    //                            , hd => hd.Calendar
    //                            , dt => dt.idCalendar
    //                            , (hd, dt) => new
    //                            {
    //                                hd,
    //                                dt,
    //                                StartingTime = baseClass.setTimeSpan(dt.StartingTime)
    //                            ,
    //                                EndingTime = baseClass.setTimeSpan(dt.EndingTime)
    //                            }).ToList();
    //                        if (wd.Count > 0)
    //                        {
    //                            var sTime = wd.Min(x => x.StartingTime); //Starting Time of Working Day
    //                            if (sTime < tempStarting.TimeOfDay)
    //                                sTime = tempStarting.TimeOfDay;
    //                            var eTime = wd.Max(x => x.EndingTime); //Ending Time of Working Day
    //                            var meTime = sTime; //for starting Time
    //                            int idCalendar = wd.First().hd.Calendar;
    //                            //หาว่าเวาลาเริ่มของ Work center นี้ใช้ไปหรือยัง หรือเป็นวันหยุดหรือวันลาหรือไม่
    //                            var calLoads = calLoad.Where(x => x.Date == tempStarting.Date
    //                                    && ((x.idCal == idCalendar && x.idWorkcenter == 0)
    //                                            ||
    //                                         x.idWorkcenter == wl.idWorkCenter && x.idCal == idCalendar)
    //                                ).OrderBy(x => x.StartingTime).ThenBy(x => x.EndingTime).ToList();
    //                            if (calLoads.Count == 0)
    //                            {
    //                                var cl = db.mh_CalendarLoads.Where(x => x.Date == tempStarting.Date
    //                                    && (x.idCal == idCalendar || x.idWorkcenter == wl.idWorkCenter)
    //                                ).OrderBy(x => x.StartingTime).ThenBy(x => x.EndingTime).ToList();
    //                                if (cl.Count > 0)
    //                                {
    //                                    calLoads = cl;
    //                                    calLoad.AddRange(calLoads);
    //                                }
    //                            }
    //                            if (calLoads.Count > 0)
    //                            {
    //                                bool foundTime = false;
    //                                List<int> idCal = new List<int>();
    //                                while (meTime < eTime)
    //                                {
    //                                    var ww = calLoads.Where(x => meTime >= x.StartingTime
    //                                        && meTime <= x.EndingTime && !idCal.Any(q => q == x.id)).FirstOrDefault();
    //                                    if (ww != null)
    //                                    {
    //                                        idCal.Add(ww.id);
    //                                        meTime = ww.EndingTime;
    //                                        //ถ้าเป็นช่วงเวลาที่ไม่ใช่เวลาทำงาน
    //                                        if (wd.Where(x => meTime >= x.StartingTime
    //                                             && meTime <= x.EndingTime).ToList().Count < 1)
    //                                        {
    //                                            //หาเวลาที่น้อยที่สุดที่มากกว่า meTime
    //                                            var a = wd.Where(x => meTime < x.StartingTime).FirstOrDefault();
    //                                            if (a != null)
    //                                                meTime = a.StartingTime;
    //                                        }
    //                                        //ถ้าเป็นช่วงเวลาทำงานปกติ ต้องเช็คต่อว่ายังมีเวลา CalendarLoad เหลือให้เช็คอีกไหม และ
    //                                        //ถ้าไม่เหลือแล้ว และน้อยกว่า Ending Time WorkingDay
    //                                        else if (idCal.Count == calLoads.Count()
    //                                            && meTime < eTime)
    //                                        {
    //                                            foundTime = true;
    //                                            break;
    //                                        }
    //                                    }
    //                                    else
    //                                    {
    //                                        foundTime = true;
    //                                        break;
    //                                    }

    //                                    if (foundTime)
    //                                        break;
    //                                }
    //                                if (foundTime)
    //                                    t_StartingDate = tempStarting.Date.AddHours(meTime.Hours).AddMinutes(meTime.Minutes);
    //                            }
    //                            else
    //                                t_StartingDate = tempStarting.Date.AddHours(meTime.Hours).AddMinutes(meTime.Minutes);

    //                            var meTime2 = meTime; //for ending time
    //                                                  //Find Ending Date-Time
    //                            if (t_StartingDate != null)
    //                            {
    //                                int autoid = 0;
    //                                do
    //                                {
    //                                    var rd = new Random();
    //                                    autoid = rd.Next(1, 99999999);
    //                                } while (calLoad.Where(x => x.id == autoid).Count() > 0);
    //                                //
    //                                var cal = calLoad.Where(x => x.Date == tempStarting
    //                                        && ((x.idCal == idCalendar && x.idWorkcenter == 0)
    //                                                ||
    //                                             x.idWorkcenter == wl.idWorkCenter && x.idCal == idCalendar)
    //                                         && x.StartingTime >= meTime
    //                                     ).OrderBy(x => x.StartingTime).ThenBy(x => x.EndingTime).ToList();
    //                                if (cal.Count > 0)
    //                                {
    //                                    var tempcal = new List<mh_CalendarLoad>();
    //                                    cal.ForEach(x =>
    //                                    {
    //                                        tempcal.Add(x);
    //                                    });
    //                                    //var t_meTime = meTime;
    //                                    var t_meTime2 = meTime2;
    //                                    var isNull = false;
    //                                    while (wl.CapacityAfterX > 0)
    //                                    {
    //                                        var aTime = tempcal.FirstOrDefault();
    //                                        bool AddC = false;
    //                                        if (aTime == null)
    //                                        {
    //                                            if (eTime > meTime)
    //                                            {
    //                                                if ((eTime - t_meTime2).TotalMinutes.ToDecimal() <= CapaUseX)
    //                                                {
    //                                                    wl.CapacityAlocateX += (eTime - t_meTime2).TotalMinutes.ToDecimal();
    //                                                    var capaLoad = new mh_CapacityLoad
    //                                                    {
    //                                                        Active = true,
    //                                                        CapacityX = (eTime - t_meTime2).TotalMinutes.ToDecimal(),
    //                                                        Capacity = 0,
    //                                                        Date = tempStarting.Date,
    //                                                        DocId = thisMain, //idTemp
    //                                                        id = 0,
    //                                                        WorkCenterID = r.idWorkCenter,
    //                                                    };
    //                                                    capacityLoad.Add(capaLoad);
    //                                                    CapaUseX -= (eTime - t_meTime2).TotalMinutes.ToDecimal();
    //                                                    AddC = true;
    //                                                }
    //                                                else
    //                                                {
    //                                                    wl.CapacityAlocateX += CapaUseX;
    //                                                    var capaLoad = new mh_CapacityLoad
    //                                                    {
    //                                                        Active = true,
    //                                                        CapacityX = CapaUseX,
    //                                                        Capacity = 0,
    //                                                        Date = tempStarting.Date,
    //                                                        DocId = thisMain, //idTemp
    //                                                        id = 0,
    //                                                        WorkCenterID = r.idWorkCenter,
    //                                                    };
    //                                                    capacityLoad.Add(capaLoad);
    //                                                    eTime = meTime.Add(TimeSpan.FromMinutes(CapaUseX.ToDouble()));
    //                                                    CapaUseX = 0;
    //                                                    AddC = true;
    //                                                }
    //                                            }
    //                                            t_meTime2 = eTime;
    //                                            meTime2 = t_meTime2;
    //                                            isNull = true;
    //                                        }
    //                                        else
    //                                        {
    //                                            var minStartingTime = aTime.StartingTime;
    //                                            if (minStartingTime > meTime)
    //                                            {
    //                                                if ((minStartingTime - meTime).TotalMinutes.ToDecimal() <= CapaUseX)
    //                                                {
    //                                                    wl.CapacityAlocateX += (minStartingTime - meTime2).TotalMinutes.ToDecimal();
    //                                                    var capaLoad = new mh_CapacityLoad
    //                                                    {
    //                                                        Active = true,
    //                                                        CapacityX = (minStartingTime - meTime2).TotalMinutes.ToDecimal(),
    //                                                        Capacity = 0,
    //                                                        Date = tempStarting.Date,
    //                                                        DocId = thisMain, //idTemp
    //                                                        id = 0,
    //                                                        WorkCenterID = r.idWorkCenter,
    //                                                    };
    //                                                    capacityLoad.Add(capaLoad);
    //                                                    CapaUseX -= (minStartingTime - meTime2).TotalMinutes.ToDecimal();
    //                                                    AddC = true;
    //                                                }
    //                                                else
    //                                                {
    //                                                    wl.CapacityAlocateX += CapaUseX;
    //                                                    var capaLoad = new mh_CapacityLoad
    //                                                    {
    //                                                        Active = true,
    //                                                        CapacityX = CapaUseX,
    //                                                        Capacity = 0,
    //                                                        Date = tempStarting.Date,
    //                                                        DocId = thisMain, //idTemp
    //                                                        id = 0,
    //                                                        WorkCenterID = r.idWorkCenter,
    //                                                    };
    //                                                    capacityLoad.Add(capaLoad);
    //                                                    minStartingTime = meTime.Add(TimeSpan.FromMinutes(CapaUseX.ToDouble()));
    //                                                    CapaUseX = 0;
    //                                                    AddC = true;
    //                                                }
    //                                            }
    //                                            meTime2 = minStartingTime;
    //                                            t_meTime2 = aTime.EndingTime;
    //                                            tempcal.Remove(tempcal.FirstOrDefault());
    //                                        }

    //                                        if (AddC)
    //                                        {
    //                                            var cl = new mh_CalendarLoad
    //                                            {
    //                                                id = autoid,
    //                                                idRoute = r.id,
    //                                                idWorkcenter = r.idWorkCenter,
    //                                                idCal = idCalendar,
    //                                                Date = tempStarting.Date,
    //                                                StartingTime = meTime,
    //                                                EndingTime = meTime2,
    //                                                idJob = thisMain, //id Temp
    //                                                idAbs = -1,
    //                                            };
    //                                            calLoad.Add(cl);
    //                                        }
    //                                        meTime = t_meTime2;
    //                                        if (CapaUseX == 0)
    //                                            break;
    //                                        if (isNull)
    //                                            break;
    //                                    }
    //                                }
    //                                else
    //                                {//ไม่มี Calendar Load เลย
    //                                    if (wl.CapacityAfterX >= CapaUseX)
    //                                    {
    //                                        meTime2 = meTime.Add(TimeSpan.FromMinutes(CapaUseX.ToDouble()));
    //                                        wl.CapacityAlocateX += CapaUseX;
    //                                        var capaLoad = new mh_CapacityLoad
    //                                        {
    //                                            Active = true,
    //                                            CapacityX = CapaUseX,
    //                                            Capacity = 0,
    //                                            Date = tempStarting.Date,
    //                                            DocId = thisMain, //idTemp
    //                                            id = 0,
    //                                            WorkCenterID = r.idWorkCenter,
    //                                        };
    //                                        capacityLoad.Add(capaLoad);
    //                                        CapaUseX = 0;
    //                                        //CapaUse = 0;
    //                                        //wl.CapacityAlocate += CapaUse;
    //                                    }
    //                                    else //CapacityAfterX < CapaUseX
    //                                    {
    //                                        meTime2 = meTime.Add(TimeSpan.FromMinutes(wl.CapacityAfterX.ToDouble()));
    //                                        var capaLoad = new mh_CapacityLoad
    //                                        {
    //                                            Active = true,
    //                                            CapacityX = wl.CapacityAfterX,
    //                                            Capacity = 0,
    //                                            Date = tempStarting.Date,
    //                                            DocId = thisMain, //idTemp
    //                                            id = 0,
    //                                            WorkCenterID = r.idWorkCenter,
    //                                        };
    //                                        capacityLoad.Add(capaLoad);
    //                                        CapaUseX -= wl.CapacityAlocateX;
    //                                        wl.CapacityAlocateX = wl.CapacityAvailableX;
    //                                        //wl.CapacityAlocate += (wl.CapacityAlocate - CapaUse);
    //                                    }

    //                                    var cl = new mh_CalendarLoad
    //                                    {
    //                                        id = autoid,
    //                                        idRoute = r.id,
    //                                        idWorkcenter = r.idWorkCenter,
    //                                        idCal = idCalendar,
    //                                        Date = tempStarting.Date,
    //                                        StartingTime = meTime,
    //                                        EndingTime = meTime2,
    //                                        idJob = thisMain,
    //                                        idAbs = -1,
    //                                    };
    //                                    calLoad.Add(cl);
    //                                }


    //                                t_EndingDate = tempStarting.Date.AddHours(meTime2.Hours).AddMinutes(meTime2.Minutes);
    //                            }
    //                        }
    //                        else
    //                        {
    //                            string mssg = "Work center not having Working days.!!!\n";
    //                            baseClass.Warning(mssg);
    //                            throw new Exception(mssg);
    //                        }

    //                        if (CapaUseX > 0)
    //                            tempStarting = tempStarting.AddDays(1).Date;
    //                    } while (CapaUseX > 0);

    //                    if (!firstStart)
    //                        gPlan.StartingDate = t_StartingDate;
    //                    gPlan.EndingDate = t_EndingDate;
    //                    firstStart = true;
    //                }

    //            }
    //            else
    //            {
    //                //Purchase
    //                gPlan.StartingDate = baseClass.setStandardTime(dFrom, true);
    //                gPlan.EndingDate = gPlan.StartingDate.Value.Date.AddDays(tdata.LeadTime);
    //                var vndr = db.mh_Vendors.Where(x => x.No == gPlan.VendorNo).FirstOrDefault();
    //                if (vndr != null)
    //                    gPlan.EndingDate = gPlan.EndingDate.Value.AddDays(vndr.ShippingTime);
    //                gPlan.EndingDate = baseClass.setStandardTime(gPlan.EndingDate.Value, false);

    //                //Find Standard Unit Purchase
    //                decimal StandardCost = gPlan.itemData.StandardCost;
    //                string PurchaseUOM = gPlan.itemData.PurchaseUOM;

    //                decimal StockQty = gPlan.itemData.QtyOnHand;
    //                decimal SafetyStock = gPlan.itemData.SafetyStock;
    //                decimal ReorderPoint = gPlan.itemData.ReorderPoint;
    //                //Find Reorder Qty
    //                decimal ReorderQty = 0.00m; //ซื้อด้วยหน่วย Base UOM
    //                decimal ReqQty = Math.Round(gPlan.Qty * gPlan.PCSUnit, 2);

    //                bool OrderComp = false;
    //                //find from Reorder Type
    //                if (gPlan.ReorderTypeEnum == ReorderType.Fixed)
    //                {
    //                    while (!OrderComp)
    //                    {
    //                        if ((StockQty + ReorderQty) - ReqQty < SafetyStock
    //                            || (StockQty + ReorderQty) - ReqQty < ReorderPoint)
    //                        {
    //                            ReorderQty += gPlan.itemData.ReorderQty;
    //                        }
    //                        else
    //                            OrderComp = true;
    //                    }
    //                }
    //                else if (gPlan.ReorderTypeEnum == ReorderType.MinMax)
    //                {
    //                    while (!OrderComp)
    //                    {
    //                        if ((StockQty + ReorderQty) - ReqQty < SafetyStock
    //                            || (StockQty + ReorderQty) - ReqQty < gPlan.itemData.MinQty)
    //                        {
    //                            ReorderQty += gPlan.itemData.MaxQty - (StockQty + ReorderQty) - ReqQty;
    //                        }
    //                        else
    //                            OrderComp = true;
    //                    }
    //                }
    //                else
    //                {//By Order
    //                    ReorderQty += ReqQty - StockQty;
    //                }
    //                //แปลงเป็นหน่วยซื้อ
    //                gPlan.UOM = PurchaseUOM;
    //                gPlan.PCSUnit = Math.Round(gPlan.itemData.PCSUnit_PurchaseUOM, 2);
    //                gPlan.Qty = Math.Round(ReorderQty / gPlan.PCSUnit, 2);
    //            }
    //            gPlan.DueDate = gPlan.EndingDate.Value.AddDays(1).Date;

    //            gridPlans.Add(gPlan);
    //            return gPlan;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        baseClass.Warning("Cal Part : " + ex.Message);
    //        return null;
    //    }
    //}

    ////cal part แบบ Gen ทั้ง P/R และ Production Plan
    //grid_Planning calPart_new(calPartData data)

    //{
    //    try
    //    {
    //        //RepType == Production, Purchase
    //        using (var db = new DataClasses1DataContext())
    //        {
    //            var tdata = itemDatas.Where(x => x.ItemNo == data.ItemNo).FirstOrDefault();
    //            if (tdata == null)
    //            {
    //                tdata = new ItemData(data.ItemNo);
    //                itemDatas.Add(tdata);
    //            }

    //            //Find Backorder ref id Customer in mh_PuchaseOrderDT (ถ้าเปิด PO แล้ว) หรือ จาก mh_PurchaseReqeustLine(ถ้ายังไม่เปิด P/O)
    //            decimal sumBackOrder = 0.00m;
    //            var prAll = db.mh_PurchaseRequestLines.Where(x => x.SS == 1 && x.CodeNo == data.ItemNo && x.idCstmPODt == data.DocId)
    //                .Join(db.mh_PurchaseRequests.Where(x => x.Status != "Cancel")
    //                , dt => dt.PRNo
    //                , hd => hd.PRNo
    //                , (dt, hd) => new { hd, dt }).ToList();
    //            foreach (var pr in prAll)
    //            {
    //                var poAll = db.mh_PurchaseOrderDetails.Where(x => x.SS == 1 && x.PRItem == pr.dt.id)
    //                    .Join(db.mh_PurchaseOrders.Where(x => x.Status != "Cancel")
    //                    , dt => dt.PONo
    //                    , hd => hd.PONo
    //                    , (dt, hd) => new { hd, dt })
    //                    .ToList();
    //                if (poAll.Count > 0)
    //                    sumBackOrder += poAll.Sum(x => Math.Round(x.dt.BackOrder.ToDecimal() * x.dt.PCSUnit.ToDecimal(), 2));
    //                else
    //                    sumBackOrder += Math.Round(pr.dt.OrderQty * pr.dt.PCSUOM, 2);
    //            }

    //            //3.Find Stock is enought ? ---> only stock q'ty not for JOB/Customer P/O
    //            if (data.ReqQty <= tdata.QtyOnHand + sumBackOrder) //3.1 Stock is enought
    //            {
    //                //tdata.QtyOnHand -= data.ReqQty;
    //                if (tdata.QtyOnHand >= data.ReqQty)
    //                    tdata.QtyOnHand -= data.ReqQty;
    //                else
    //                    tdata.QtyOnHand = 0;
    //                return null;
    //            }

    //            //3.2 Stock + Back order not enought
    //            var t_QtyOnHand = tdata.QtyOnHand;
    //            data.ReqQty -= t_QtyOnHand;
    //            data.ReqQty -= sumBackOrder;
    //            tdata.QtyOnHand = 0;
    //            sumBackOrder = 0;

    //            //set data
    //            var gPlan = newGridPlan(data, tdata);
    //            var thisMain = mainNo;

    //            //set Production or Purchase
    //            if (tdata.RepType_enum == ReplenishmentType.Production)
    //            {
    //                //Prodction
    //                //find BOM
    //                var boms = db.tb_BomDTs.Where(x => x.PartNo == gPlan.ItemNo).ToList();
    //                foreach (var b in boms)
    //                {
    //                    var tool = db.mh_Items.Where(x => x.InternalNo == b.Component).FirstOrDefault();
    //                    var cd = new calPartData
    //                    {
    //                        DocId = data.DocId,
    //                        DocNo = data.DocNo,
    //                        ItemNo = b.Component,
    //                        repType = baseClass.getRepType(tool.ReplenishmentType),
    //                        ReqDate = data.ReqDate,
    //                        ReqQty = Math.Round(b.Qty.ToDecimal() * data.ReqQty, 2),
    //                        mainNo = gPlan.mainNo,
    //                        UOM = b.Unit,
    //                        PCSUnit = b.PCSUnit.ToDecimal(),
    //                    };
    //                    calPart_new(cd);
    //                }

    //                if (data.alreadyJob) return null;

    //                //BEgin Production
    //                var rmList = gridPlans.Where(x => x.idRef == gPlan.idRef).ToList();
    //                DateTime tempStarting = new DateTime();
    //                if (rmList.Count > 0)
    //                {
    //                    tempStarting = rmList.OrderByDescending(x => x.DueDate).First().DueDate.Date;
    //                }
    //                else
    //                    tempStarting = dFrom;
    //                //find time in Routing...
    //                var rt = db.mh_RoutingDTs.Where(x => x.RoutingId == tdata.Routeid)
    //                    .Join(db.mh_WorkCenters.Where(x => x.Active)
    //                    , hd => hd.idWorkCenter
    //                    , workcenter => workcenter.id
    //                    , (hd, workcenter)
    //                    => new { hd, hd.idWorkCenter, hd.id, hd.SetupTime, hd.RunTime, hd.WaitTime, workcenter })
    //                    .ToList();
    //                var t_StartingDate = (DateTime?)null;
    //                var t_EndingDate = (DateTime?)null;
    //                bool firstStart = false;
    //                foreach (var r in rt)
    //                {
    //                    if (t_EndingDate != null)
    //                        tempStarting = t_EndingDate.Value;
    //                    int idWorkCenter = r.idWorkCenter;
    //                    var totalCapa_All = 0.00m;
    //                    var SetupTime = r.SetupTime;
    //                    var RunTime = r.RunTime;
    //                    var RunTimeCapa = Math.Round(((RunTime * gPlan.Qty) / r.workcenter.Capacity), 2);
    //                    var WaitingTime = r.WaitTime;
    //                    totalCapa_All = SetupTime + RunTimeCapa + r.WaitTime;
    //                    var CapaUseX = 0.00m;
    //                    CapaUseX = totalCapa_All;

    //                    //find capacity Available (Workcenter) on date
    //                    do
    //                    {
    //                        //1. หาว่าเวลาเริ่มสามารถใช้ได้ไหม
    //                        var wl = workLoads.Where(x => x.Date >= tempStarting.Date && x.CapacityAfterX > 0
    //                            && x.idWorkCenter == idWorkCenter).OrderBy(x => x.Date).FirstOrDefault();
    //                        if (wl == null)
    //                        {
    //                            string mssg = "Capacity is not available, Please check Capacity Work load on Capacity Calculation (Work Centers).!!!\n";
    //                            baseClass.Warning(mssg);
    //                            throw new Exception(mssg);
    //                        }
    //                        if (tempStarting == null || wl.Date.Date > tempStarting.Date)
    //                            tempStarting = wl.Date.Date;

    //                        //set Starting
    //                        int dow = baseClass.getDayOfWeek(tempStarting.DayOfWeek);
    //                        //
    //                        var wd = db.mh_WorkCenters.Where(x => x.id == wl.idWorkCenter)
    //                            .Join(db.mh_WorkingDays.Where(x => x.Day == dow && x.Active)
    //                            , hd => hd.Calendar
    //                            , dt => dt.idCalendar
    //                            , (hd, dt) => new
    //                            {
    //                                hd,
    //                                dt,
    //                                StartingTime = baseClass.setTimeSpan(dt.StartingTime)
    //                            ,
    //                                EndingTime = baseClass.setTimeSpan(dt.EndingTime)
    //                            }).ToList();
    //                        if (wd.Count > 0)
    //                        {
    //                            var sTime = wd.Min(x => x.StartingTime); //Starting Time of Working Day
    //                            if (sTime < tempStarting.TimeOfDay)
    //                                sTime = tempStarting.TimeOfDay;
    //                            var eTime = wd.Max(x => x.EndingTime); //Ending Time of Working Day
    //                            var meTime = sTime; //for starting Time
    //                            int idCalendar = wd.First().hd.Calendar;
    //                            //หาว่าเวาลาเริ่มของ Work center นี้ใช้ไปหรือยัง หรือเป็นวันหยุดหรือวันลาหรือไม่
    //                            var calLoads = calLoad.Where(x => x.Date == tempStarting.Date
    //                                    && (x.idCal == idCalendar || x.idWorkcenter == wl.idWorkCenter)
    //                                ).OrderBy(x => x.StartingTime).ThenBy(x => x.EndingTime).ToList();
    //                            if (calLoads.Count > 0)
    //                            {
    //                                bool foundTime = false;
    //                                List<int> idCal = new List<int>();
    //                                while (meTime < eTime)
    //                                {
    //                                    var ww = calLoads.Where(x => meTime >= x.StartingTime
    //                                        && meTime <= x.EndingTime && !idCal.Any(q => q == x.id)).FirstOrDefault();
    //                                    if (ww != null)
    //                                    {
    //                                        idCal.Add(ww.id);
    //                                        meTime = ww.EndingTime;
    //                                        //ถ้าเป็นช่วงเวลาที่ไม่ใช่เวลาทำงาน
    //                                        if (wd.Where(x => meTime >= x.StartingTime
    //                                             && meTime <= x.EndingTime).ToList().Count < 1)
    //                                        {
    //                                            //หาเวลาที่น้อยที่สุดที่มากกว่า meTime
    //                                            var a = wd.Where(x => meTime < x.StartingTime).FirstOrDefault();
    //                                            if (a != null)
    //                                                meTime = a.StartingTime;
    //                                        }
    //                                        //ถ้าเป็นช่วงเวลาทำงานปกติ ต้องเช็คต่อว่ายังมีเวลา CalendarLoad เหลือให้เช็คอีกไหม และ
    //                                        //ถ้าไม่เหลือแล้ว และน้อยกว่า Ending Time WorkingDay
    //                                        else if (idCal.Count == calLoads.Count()
    //                                            && meTime < eTime)
    //                                        {
    //                                            foundTime = true;
    //                                            break;
    //                                        }
    //                                    }
    //                                    else
    //                                    {
    //                                        foundTime = true;
    //                                        break;
    //                                    }

    //                                    if (foundTime)
    //                                        break;
    //                                }
    //                                if (foundTime)
    //                                    t_StartingDate = tempStarting.Date.AddHours(meTime.Hours).AddMinutes(meTime.Minutes);
    //                            }
    //                            else
    //                                t_StartingDate = tempStarting.Date.AddHours(meTime.Hours).AddMinutes(meTime.Minutes);

    //                            var meTime2 = meTime; //for ending time
    //                                                  //Find Ending Date-Time
    //                            if (t_StartingDate != null)
    //                            {
    //                                int autoid = 0;
    //                                do
    //                                {
    //                                    var rd = new Random();
    //                                    autoid = rd.Next(1, 99999999);
    //                                } while (calLoad.Where(x => x.id == autoid).Count() > 0);
    //                                //
    //                                var cal = calLoad.Where(x => x.Date == tempStarting
    //                                         && (x.idCal == idCalendar || x.idWorkcenter == wl.idWorkCenter)
    //                                         && x.StartingTime >= meTime
    //                                     ).OrderBy(x => x.StartingTime).ThenBy(x => x.EndingTime).ToList();
    //                                if (cal.Count > 0)
    //                                {
    //                                    var tempcal = new List<mh_CalendarLoad>();
    //                                    cal.ForEach(x =>
    //                                    {
    //                                        tempcal.Add(x);
    //                                    });
    //                                    //var t_meTime = meTime;
    //                                    var t_meTime2 = meTime2;
    //                                    var isNull = false;
    //                                    while (wl.CapacityAfterX > 0)
    //                                    {
    //                                        var aTime = tempcal.FirstOrDefault();
    //                                        bool AddC = false;
    //                                        if (aTime == null)
    //                                        {
    //                                            if (eTime > meTime)
    //                                            {
    //                                                wl.CapacityAlocateX += (eTime - t_meTime2).TotalMinutes.ToDecimal();
    //                                                var capaLoad = new mh_CapacityLoad
    //                                                {
    //                                                    Active = true,
    //                                                    CapacityX = (eTime - t_meTime2).TotalMinutes.ToDecimal(),
    //                                                    Capacity = 0,
    //                                                    Date = tempStarting.Date,
    //                                                    DocId = thisMain, //idTemp
    //                                                    id = 0,
    //                                                    WorkCenterID = r.idWorkCenter,
    //                                                };
    //                                                capacityLoad.Add(capaLoad);
    //                                                CapaUseX -= (eTime - t_meTime2).TotalMinutes.ToDecimal();
    //                                                AddC = true;
    //                                            }
    //                                            t_meTime2 = eTime;
    //                                            meTime2 = t_meTime2;
    //                                            isNull = true;
    //                                        }
    //                                        else
    //                                        {
    //                                            var minStartingTime = aTime.StartingTime;
    //                                            if (minStartingTime > meTime)
    //                                            {
    //                                                wl.CapacityAlocateX += (minStartingTime - meTime2).TotalMinutes.ToDecimal();
    //                                                var capaLoad = new mh_CapacityLoad
    //                                                {
    //                                                    Active = true,
    //                                                    CapacityX = (minStartingTime - meTime2).TotalMinutes.ToDecimal(),
    //                                                    Capacity = 0,
    //                                                    Date = tempStarting.Date,
    //                                                    DocId = thisMain, //idTemp
    //                                                    id = 0,
    //                                                    WorkCenterID = r.idWorkCenter,
    //                                                };
    //                                                capacityLoad.Add(capaLoad);
    //                                                CapaUseX -= (minStartingTime - meTime2).TotalMinutes.ToDecimal();
    //                                                AddC = true;
    //                                            }
    //                                            meTime2 = minStartingTime;
    //                                            t_meTime2 = aTime.EndingTime;
    //                                            tempcal.Remove(tempcal.FirstOrDefault());
    //                                        }

    //                                        if (AddC)
    //                                        {
    //                                            var cl = new mh_CalendarLoad
    //                                            {
    //                                                id = autoid,
    //                                                idRoute = r.id,
    //                                                idWorkcenter = r.idWorkCenter,
    //                                                Date = tempStarting.Date,
    //                                                StartingTime = meTime,
    //                                                EndingTime = meTime2,
    //                                                idJob = thisMain, //id Temp
    //                                                idAbs = -1,
    //                                            };
    //                                            calLoad.Add(cl);
    //                                        }
    //                                        meTime = t_meTime2;
    //                                        if (isNull)
    //                                            break;
    //                                    }
    //                                }
    //                                else
    //                                {//ไม่มี Calendar Load เลย
    //                                    if (wl.CapacityAfterX >= CapaUseX)
    //                                    {
    //                                        meTime2 = meTime.Add(TimeSpan.FromMinutes(CapaUseX.ToDouble()));
    //                                        wl.CapacityAlocateX += CapaUseX;
    //                                        var capaLoad = new mh_CapacityLoad
    //                                        {
    //                                            Active = true,
    //                                            CapacityX = CapaUseX,
    //                                            Capacity = 0,
    //                                            Date = tempStarting.Date,
    //                                            DocId = thisMain, //idTemp
    //                                            id = 0,
    //                                            WorkCenterID = r.idWorkCenter,
    //                                        };
    //                                        capacityLoad.Add(capaLoad);
    //                                        CapaUseX = 0;
    //                                        //CapaUse = 0;
    //                                        //wl.CapacityAlocate += CapaUse;
    //                                    }
    //                                    else //CapacityAfterX < CapaUseX
    //                                    {
    //                                        meTime2 = meTime.Add(TimeSpan.FromMinutes(wl.CapacityAfterX.ToDouble()));
    //                                        var capaLoad = new mh_CapacityLoad
    //                                        {
    //                                            Active = true,
    //                                            CapacityX = wl.CapacityAfterX,
    //                                            Capacity = 0,
    //                                            Date = tempStarting.Date,
    //                                            DocId = thisMain, //idTemp
    //                                            id = 0,
    //                                            WorkCenterID = r.idWorkCenter,
    //                                        };
    //                                        capacityLoad.Add(capaLoad);
    //                                        CapaUseX -= wl.CapacityAlocateX;
    //                                        wl.CapacityAlocateX = wl.CapacityAvailableX;
    //                                        //wl.CapacityAlocate += (wl.CapacityAlocate - CapaUse);
    //                                    }

    //                                    var cl = new mh_CalendarLoad
    //                                    {
    //                                        id = autoid,
    //                                        idRoute = r.id,
    //                                        idWorkcenter = r.idWorkCenter,
    //                                        Date = tempStarting.Date,
    //                                        StartingTime = meTime,
    //                                        EndingTime = meTime2,
    //                                        idJob = thisMain,
    //                                        idAbs = -1,
    //                                    };
    //                                    calLoad.Add(cl);

    //                                    t_EndingDate = tempStarting.Date.AddHours(meTime2.Hours).AddMinutes(meTime2.Minutes);
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            string mssg = "Work center not having Working days.!!!\n";
    //                            baseClass.Warning(mssg);
    //                            throw new Exception(mssg);
    //                        }

    //                        if (CapaUseX > 0)
    //                            tempStarting = tempStarting.AddDays(1).Date;
    //                    } while (CapaUseX > 0);

    //                    if (!firstStart)
    //                        gPlan.StartingDate = t_StartingDate;
    //                    gPlan.EndingDate = t_EndingDate;
    //                    firstStart = true;
    //                }

    //            }
    //            else
    //            {
    //                //Purchase
    //                gPlan.StartingDate = baseClass.setStandardTime(dFrom, true);
    //                gPlan.EndingDate = gPlan.StartingDate.Value.Date.AddDays(tdata.LeadTime);
    //                var vndr = db.mh_Vendors.Where(x => x.No == gPlan.VendorNo).FirstOrDefault();
    //                if (vndr != null)
    //                    gPlan.EndingDate = gPlan.EndingDate.Value.AddDays(vndr.ShippingTime);
    //                gPlan.EndingDate = baseClass.setStandardTime(gPlan.EndingDate.Value, false);

    //                //Find Standard Unit Purchase
    //                decimal StandardCost = gPlan.itemData.StandardCost;
    //                string PurchaseUOM = gPlan.itemData.PurchaseUOM;

    //                decimal StockQty = gPlan.itemData.QtyOnHand;
    //                decimal SafetyStock = gPlan.itemData.SafetyStock;
    //                decimal ReorderPoint = gPlan.itemData.ReorderPoint;
    //                //Find Reorder Qty
    //                decimal ReorderQty = 0.00m; //ซื้อด้วยหน่วย Base UOM
    //                decimal ReqQty = Math.Round(gPlan.Qty * gPlan.PCSUnit, 2);

    //                bool OrderComp = false;
    //                //find from Reorder Type
    //                if (gPlan.ReorderTypeEnum == ReorderType.Fixed)
    //                {
    //                    while (!OrderComp)
    //                    {
    //                        if ((StockQty + ReorderQty) - ReqQty < SafetyStock
    //                            || (StockQty + ReorderQty) - ReqQty < ReorderPoint)
    //                        {
    //                            ReorderQty += gPlan.itemData.ReorderQty;
    //                        }
    //                        else
    //                            OrderComp = true;
    //                    }
    //                }
    //                else if (gPlan.ReorderTypeEnum == ReorderType.MinMax)
    //                {
    //                    while (!OrderComp)
    //                    {
    //                        if ((StockQty + ReorderQty) - ReqQty < SafetyStock
    //                            || (StockQty + ReorderQty) - ReqQty < gPlan.itemData.MinQty)
    //                        {
    //                            ReorderQty += gPlan.itemData.MaxQty - (StockQty + ReorderQty) - ReqQty;
    //                        }
    //                        else
    //                            OrderComp = true;
    //                    }
    //                }
    //                else
    //                {//By Order
    //                    ReorderQty += ReqQty - StockQty;
    //                }
    //                //แปลงเป็นหน่วยซื้อ
    //                gPlan.UOM = PurchaseUOM;
    //                gPlan.PCSUnit = Math.Round(gPlan.itemData.PCSUnit_PurchaseUOM, 2);
    //                gPlan.Qty = Math.Round(ReorderQty / gPlan.PCSUnit, 2);
    //            }
    //            gPlan.DueDate = gPlan.EndingDate.Value.AddDays(1).Date;

    //            gridPlans.Add(gPlan);
    //            return gPlan;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        baseClass.Warning("Cal Part : " + ex.Message);
    //        return null;
    //    }
    //}

}
