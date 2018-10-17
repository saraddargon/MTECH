using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockControl
{
    public static class getGrid
    {
        public static List<grid_CustomerPO> GetGrid_CustomerPO(string CustomerPO = ""
            , string CSTMNo = "", string Item = "", DateTime? dFrom = null, DateTime? dTo = null
            , string Status = "")
        {
            var l = new List<grid_CustomerPO>();
            using (var db = new DataClasses1DataContext())
            {
                try
                {
                    var m = db.mh_CustomerPOs.Where(x => x.Active && x.DemandType == 0
                        && (CustomerPO == "" || x.CustomerPONo == CustomerPO)
                        && (CSTMNo == "" || x.CustomerNo == CSTMNo)
                        && (dFrom == null || (x.OrderDate >= dFrom && x.OrderDate <= dTo))).ToList();
                    foreach (var hd in m)
                    {
                        var t = db.mh_CustomerPODTs.Where(x => x.Active && x.idCustomerPO == hd.id
                            && (Status == "" || x.Status == Status)).ToList();
                        foreach (var dt in t)
                        {
                            if (Item != "" && dt.ItemNo != Item) continue;
                            var tool = db.mh_Items.Where(x => x.InternalNo == dt.ItemNo).FirstOrDefault();
                            var shipQ = 0.00m;
                            var s = db.mh_SaleOrderDTs.Where(x => x.Active && x.RefId == dt.id)
                                .Join(db.mh_SaleOrders.Where(x => x.Active),
                                    dtso => dtso.SONo,
                                    hdShip => hdShip.SONo,
                                    (dtso, hdShip) => new { dtso, hdShip }
                                ).ToList();
                            foreach (var mm in s)
                            {
                                //find Shipment
                                var ss = db.mh_ShipmentDTs.Where(x => x.Active && x.RefId == mm.dtso.id)
                                    .Join(db.mh_Shipments.Where(x => x.Active != null && x.Active.Value),
                                    dtSS => dtSS.SSNo,
                                    hdSS => hdSS.SSNo,
                                    (dtSS, hdSS) => new { dtSS, hdSS }).ToList();
                                if (ss.Count > 0)
                                    shipQ += ss.Sum(x => x.dtSS.Qty).ToDecimal();
                            }

                            //find Job
                            string JobNo = "";
                            //if (dt.OutPlan == 0)
                            //{
                            //    var q = db.mh_ProductionOrders.Where(x => x.Active && x.RefDocId == dt.id).FirstOrDefault();
                            //    if (q != null) JobNo = q.JobNo;
                            //}

                            l.Add(new grid_CustomerPO
                            {
                                Status = dt.Status,
                                CustomerPONo = hd.CustomerPONo,
                                CustomerNo = hd.CustomerNo,
                                ReqDate = dt.ReqDate,
                                ItemNo = dt.ItemNo,
                                ItemName = dt.ItemName,
                                Qty = dt.Qty,
                                UOM = dt.UOM,
                                PCSUnit = dt.PCSUnit,
                                UnitPrice = dt.UnitPrice,
                                Amount = dt.Amount,
                                OutSO = dt.OutSO,
                                OutPlan = dt.OutPlan,
                                id = dt.id,
                                idCustomerPO = dt.idCustomerPO,
                                GroupType = tool.GroupType,
                                InvGroup = tool.InventoryGroup,
                                Type = tool.Type,
                                VendorNo = tool.VendorNo,
                                VendorName = tool.VendorName,
                                Shipped = shipQ,//
                                JobNo = JobNo,
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    baseClass.Warning(ex.Message);
                }
            }
            return l;
        }
    }

    public class CustomerPOCal
    {
        public mh_CustomerPO POHd { get; set; }
        public mh_CustomerPODT PODt { get; set; }
    }
    public class listforPlanning
    {
        public int DocId { get; set; }
        public string DocNo { get; set; }
        public DateTime DocDate { get; set; }
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public ReplenishmentType RepType { get; set; }
        public DateTime ReqDate { get; set; }
        public decimal ReqQty { get; set; }
        public decimal PCSUnit { get; set; }
        public string UOM { get; set; }
        public bool alreadyJob { get; set; } = false;
    }

    public class grid_CustomerPO
    {
        public string Status
        {
            get; set;
        }
        public string CustomerPONo { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName
        {
            get {
                using (var db = new DataClasses1DataContext())
                {
                    var c = db.mh_Customers.Where(x => x.Active && x.No == CustomerNo).FirstOrDefault();
                    if (c != null)
                        return c.Name;
                }
                return "";
            }
        }
        public DateTime ReqDate { get; set; }
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public decimal Qty { get; set; } = 0;
        public decimal Remain
        {
            get {
                return (Qty) - Shipped;
            }
        }
        private decimal _ship = -1.00m;
        public decimal Shipped
        {
            get; set;
        }
        public bool Plan
        {
            get {
                return (Qty * PCSUnit != OutPlan);
                //find from JobNo
            }
        }
        public bool SaleOrder
        {
            get {
                return (Qty * PCSUnit != OutSO);
                //find in SaleOrder
            }
        }
        public string UOM { get; set; }
        public decimal PCSUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public decimal OutSO { get; set; }
        public decimal OutPlan { get; set; }
        public int id { get; set; }
        public int idCustomerPO { get; set; }

        public string GroupType { get; set; }
        public string Type { get; set; }
        public string InvGroup { get; set; }
        public string VendorNo { get; set; }
        public string VendorName { get; set; }

        public string JobNo { get; set; }
    }
    public class grid_Planning
    {
        public string Status
        {
            get {

                if (RM_BackOrder)
                    return "Waiting Receive Order";
                else if (DueDate.Date > ReqDate.Date)
                    return "Over Due";
                else
                    return "OK";
            }
        }
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public DateTime ReqDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        public decimal Qty { get; set; }
        public decimal UseQty { get; set; }
        public string PlanningType { get; set; }
        public int idRef { get; set; }
        public string RefDocNo { get; set; }
        public string RefDocNo_TEMP
        {
            get {
                string a = RefDocNo;
                if (a.Contains("FGTEMPJOB"))
                    a = "Safety stock(Production)";
                return a;
            }
        }

        public string GroupType { get; set; }
        public string Type { get; set; }
        public string InvGroup { get; set; }
        public ReorderType ReorderTypeEnum { get; set; }
        public string ReorderTypeText
        {
            get {
                return baseClass.getReorderTypeText(ReorderTypeEnum);
            }
        }
        public string VendorNo { get; set; }
        public string VendorName { get; set; }
        public string UOM { get; set; }
        public decimal PCSUnit { get; set; }

        public decimal TotalCost { get; set; } = 0.00m;

        public string LocationItem { get; set; }

        public bool root { get; set; } = false;
        public int mainNo { get; set; } = 0;
        public int refNo { get; set; } = 0;

        public bool RM_BackOrder { get; set; } = false;

        public ItemData itemData { get; set; }
    }


    public class CustomerCombo
    {
        public string No { get; set; }
        public string Name { get; set; }
    }
    public class ItemCombo
    {
        public string Item { get; set; }
        public string ItemName { get; set; }
    }


    public class ItemData
    {
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public ReorderType ReorderType { get; set; }
        public decimal QtyOnHand { get; set; } //Stock Free
        public decimal QtyOnHand_Backup { get; private set; } //Stock Free
        public decimal SafetyStock { get; set; }
        public decimal ReorderPoint { get; set; }
        public decimal ReorderQty { get; set; }
        public decimal MinQty { get; set; }
        public decimal MaxQty { get; set; }
        public decimal MaximumInventory { get; set; }
        public int TimeBuket { get; set; }
        public int LeadTime { get; set; }
        public ReplenishmentType RepType_enum { get; set; }
        public InventoryGroup InvGroup_enum { get; set; }

        public string UOM { get; set; }
        public decimal PCSUnit { get; set; }

        public string LocationItem { get; set; }

        public string GroupType { get; set; }
        public string Type { get; set; }
        public string InvGroup { get; set; } //Inventory Group
        public string VendorNo { get; set; }
        public string VendorName { get; set; }

        public int Routeid { get; set; }

        public string BaseUOM { get; set; }
        public decimal PCSUnit_BaseUOM { get; set; }
        public string PurchaseUOM { get; set; }
        public decimal PCSUnit_PurchaseUOM { get; set; }
        public decimal StandardCost { get; set; }

        public string BomNo { get; set; }

        public ItemData(string ItemNo, string ItemName = "")
        {
            //**************
            //***************
            //***************
            //***************
            //*** ระบบเปลี่ยนจาก idCustomerPODt เป็น id ของ SaleOrder Dt แทน
            //***************
            //***************
            //***************
            this.ItemNo = ItemNo;
            this.ItemName = ItemName;
            using (var db = new DataClasses1DataContext())
            {
                var t = db.mh_Items.Where(x => x.InternalNo == this.ItemNo).FirstOrDefault();
                if (t == null) return;
                if (ItemNo == "MET-T-001")
                { }

                this.ItemName = t.InternalName;
                this.ReorderType = baseClass.getReorderType(t.ReorderType);
                this.SafetyStock = t.SafetyStock;
                this.ReorderPoint = t.ReorderPoint.ToDecimal();
                this.ReorderQty = t.ReorderQty.ToDecimal();
                this.MinQty = t.MinimumQty;
                this.MaxQty = t.MaximumQty;
                this.MaximumInventory = t.MaximumInventory.ToDecimal();
                this.TimeBuket = t.Timebucket.ToInt();
                this.LeadTime = t.InternalLeadTime.ToInt();
                this.RepType_enum = baseClass.getRepType(t.ReplenishmentType);
                this.InvGroup_enum = baseClass.getInventoryGroup(t.InventoryGroup);
                //this.BomNo = t.BillOfMaterials;
                var bom = db.tb_BomHDs.Where(x => x.id == t.BillOfMaterials)
                    .Join(db.tb_BomDTs,
                    hd => hd.BomNo,
                    dt => dt.BomNo,
                    (hd, dt) => new { hd, dt }).ToList();
                if (bom.Count > 0)
                    this.BomNo = bom.First().hd.BomNo;
                //this.QtyOnHand = baseClass.StockQty(this.ItemNo, "Warehouse");
                decimal? a = null;
                if (InvGroup_enum == InventoryGroup.FG || InvGroup_enum == InventoryGroup.SEMI) //FG,SEMI กรณีไม่ได้เปิดจากเพื่อ Customer PO ใดๆ
                    a = db.Cal_QTY_Remain_Location(this.ItemNo, "FGPlan", 0, "Warehouse", 0);
                else //RM ที่สั่งซื้อเพื่อ Customer P/O แต่ Job สำหรับ Customer P/O Item นั้นๆถูกปิดไปแล้ว ;;; หรือ Customer P/O นั้นๆถูกยกเลิกไปแล้ว
                    a = db.Cal_QTY_Remain_Location(this.ItemNo, "RMPlan", 0, "Warehouse", 0);
                if (a != null)
                {
                    this.QtyOnHand = a.Value.ToDecimal();
                    this.QtyOnHand_Backup = a.Value.ToDecimal();
                }

                this.UOM = t.BaseUOM;
                var u = db.mh_ItemUOMs.Where(x => x.ItemNo == this.ItemNo && x.UOMCode == t.BaseUOM).FirstOrDefault();
                if (u != null) this.PCSUnit = u.QuantityPer.ToDecimal();
                else this.PCSUnit = 1;

                this.LocationItem = t.Location;

                this.GroupType = t.GroupType;
                this.Type = t.Type;
                this.InvGroup = t.InventoryGroup;
                this.VendorNo = t.VendorNo;
                this.VendorName = t.VendorName;

                this.Routeid = t.Routing;

                this.BaseUOM = t.BaseUOM;
                this.PCSUnit_BaseUOM = this.PCSUnit;
                this.PurchaseUOM = t.PurchaseUOM;
                var u2 = db.mh_ItemUOMs.Where(x => x.ItemNo == this.ItemNo && x.UOMCode == t.PurchaseUOM).FirstOrDefault();
                if (u2 != null) this.PCSUnit_PurchaseUOM = u2.QuantityPer.ToDecimal();
                else this.PCSUnit_PurchaseUOM = 1;
                this.StandardCost = t.StandardCost;

                if (ItemNo == "PHT-T-001")
                { }

                //**Stock Customer P/O --> BackOrder Q'ty for Customer P/O, Receive stock Q'ty for Customer P/O
                //find Received stock Q'ty for Customer P/O idDt ,,, reserve or not reserve(if job closed)
                var st = db.tb_Stocks.Where(x => x.CodeNo == this.ItemNo
                    && x.idCSTMPODt != null && x.TLQty > 0).OrderBy(x => x.id).ToList();
                foreach (var s in st)
                {
                    //var j = db.mh_ProductionOrders.Where(x => x.Active && x.RefDocId == s.idCSTMPODt).FirstOrDefault();
                    ////cstmPO job ยังไม่ปิด หรือ เปิดแล้วแต่ยังรับเข้าไม่ครบแสดงว่ายังไม่ปิด ซึ่งแสดงว่าเป็น Stock ปกติ
                    //if (j == null || (j != null && j.OutQty > 0))
                    if (!s.Free.ToBool())
                    {//CustomerPO ยังไม่เปิด Job หรือ เปิดแล้วแต่ยังไม่ปิด Job
                        var s1 = stockCustomerPO.Where(x => x.idCstmPODt == s.idCSTMPODt).FirstOrDefault();
                        if (s1 == null)
                        {
                            s1 = new stockForCustomerDt();
                            s1.ItemNo = s.CodeNo;
                            stockCustomerPO.Add(s1);
                        }
                        s1.idCstmPODt = s.idCSTMPODt.ToInt();
                        s1.StockQty += s.TLQty.ToDecimal();
                        s1.StockAll += s.TLQty.ToDecimal();
                    }
                    else
                    {//ปิดแล้วแสดงว่าเป็น Free Stock
                        //ดูว่าเป็น Free stock ที๋โดนจองไปหรือยัง(โดนจองไปแล้วและไอ้ตัวที่มาจองมันจะต้องไม่ปิดจ๊อบด้วย) ?
                        var freeStock = s.TLQty.ToDecimal();
                        var rs = db.mh_StockReserves.Where(x => x.idCstmPODt_Free == s.idCSTMPODt
                            //&& !x.closeJob 
                            && x.ItemNo == s.CodeNo).ToList();
                        if (rs.Count > 0)
                        {//โดนจองไปแล้ว(และยังไม่ปิด Job) ต้องดูว่าโดนจองกี่ตัว
                            foreach (var r in rs)
                            {
                                freeStock -= r.ReserveQty;
                                var ss = stockCustomerPO.Where(x => x.idCstmPODt == r.idCstmPODt).FirstOrDefault();
                                if (ss == null)
                                {
                                    ss = new stockForCustomerDt();
                                    ss.ItemNo = s.CodeNo;
                                    stockCustomerPO.Add(ss);
                                }
                                ss.idCstmPODt = r.idCstmPODt;
                                ss.StockQty += r.ReserveQty;
                                ss.StockAll += r.ReserveQty;
                            }
                        }
                        //จำนวนคงเหลือของ freeStock > 0 แสดงว่ายังมีบางส่วนยังไม่โดนจอง
                        if (freeStock > 0)
                        {
                            //free stock รอการจองจาก Customer PO อื่นๆ
                            var sf = stockFree_List.Where(x => x.idCstmPODt_Free == s.idCSTMPODt.ToInt()
                                && x.id_tb_Stock == s.id).FirstOrDefault();
                            if (sf == null)
                            {
                                sf = new stockFree();
                                stockFree_List.Add(sf);
                            }
                            //new stockFree();
                            sf.id_tb_Stock = s.id;
                            sf.ItemNo = s.CodeNo;
                            sf.idCstmPODt_Free = s.idCSTMPODt.ToInt();
                            sf.QtyFree = freeStock;
                            sf.QtyFree_Backup = freeStock;
                        }
                    }
                }
                //find Back order Q'ty from P/R(Qty) ถ้าเปิด P/O แล้วให้เอา BackOrder P/O
                var pr = db.mh_PurchaseRequestLines.Where(x => x.SS == 1 && x.CodeNo == this.ItemNo && x.idCstmPODt != null)
                    .Join(db.mh_PurchaseRequests.Where(x => x.Status != "Cancel")
                    , dt => dt.PRNo
                    , hd => hd.PRNo
                    , (dt, hd) => new { hd, dt }).ToList();
                foreach (var p in pr)
                {
                    var s1 = stockCustomerPO.Where(x => x.idCstmPODt == p.dt.idCstmPODt.ToInt()).FirstOrDefault();
                    if (s1 == null)
                    {
                        s1 = new stockForCustomerDt();
                        s1.ItemNo = p.dt.CodeNo;
                        s1.idCstmPODt = p.dt.idCstmPODt.ToInt();
                        stockCustomerPO.Add(s1);
                    }

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
                            var aa = po.Sum(x => Math.Round(x.dt.BackOrder.ToDecimal() * x.dt.PCSUnit.ToDecimal(), 2));
                            s1.BackOrder += aa;
                            s1.StockAll += aa;
                        }
                        else
                        {//not found P/O
                            var aa = Math.Round(p.dt.OrderQty.ToDecimal() * p.dt.PCSUOM.ToDecimal(), 2);
                            s1.BackOrder += aa;
                            s1.StockAll += aa;
                        }
                    }
                    //not Create P/O
                    else
                    {
                        var aa = Math.Round(p.dt.OrderQty.ToDecimal() * p.dt.PCSUOM.ToDecimal(), 2);
                        s1.BackOrder += aa;
                        s1.StockAll += aa;
                    }
                }
                //**Item เป็น FG หรือ SEMI ที่เป็นลูกเพื่อไปใช้ใน FG จริงๆของ idCstmPODt
                if (this.InvGroup_enum != InventoryGroup.RM)
                {
                    var prod = db.mh_ProductionOrders.Where(x => x.Active && x.FGNo == this.ItemNo).ToList();
                    foreach (var s in prod)
                    {
                        var s1 = stockCustomerPO.Where(x => x.idCstmPODt == s.RefDocId).FirstOrDefault();
                        if (s1 == null)
                        {
                            s1 = new stockForCustomerDt();
                            s1.ItemNo = s.FGNo;
                            stockCustomerPO.Add(s1);
                        }
                        s1.idCstmPODt = s.RefDocId.ToInt();
                        s1.StockQty += s.Qty.ToDecimal();
                        s1.StockAll += s.Qty.ToDecimal();
                    }
                }

                //var m = db.mh_CustomerPOs.Where(x => x.Active && x.DemandType == 1)
                //    .Join(db.mh_CustomerPODTs.Where(x => x.Active && x.forSafetyStock && x.genPR
                //        && x.ItemNo == this.ItemNo && x.OutQty > 0)
                //    , hd => hd.id
                //    , dt => dt.idCustomerPO
                //    , (hd, dt) => new { hd, dt }).ToList();
                var m = db.mh_SaleOrders.Where(x => x.Active && x.DemandType == 1)
                    .Join(db.mh_SaleOrderDTs.Where(x => x.Active && x.forSafetyStock && x.genPR
                        && x.ItemNo == this.ItemNo && x.OutQty > 0)
                        , hd => hd.SONo
                        , dt => dt.SONo
                        , (hd, dt) => new { hd, dt }).ToList();
                foreach (var mm in m)
                {
                    SafetyStockFG += mm.dt.OutQty;
                    SafetyStockFG_Backup += mm.dt.OutQty;
                }
            }

        }

        public List<stockForCustomerDt> stockCustomerPO { get; set; } = new List<stockForCustomerDt>();
        public List<stockFree> stockFree_List { get; set; } = new List<stockFree>();
        public decimal SafetyStockFG { get; set; } = 0.00m;
        public decimal SafetyStockFG_Backup { get; set; } = 0.00m;
        public decimal SafetyStockPRPO { get; set; } = 0.00m;
        public decimal SafetyStockPRPO_Backup { get; set; } = 0.00m;

        public decimal findStock_CustomerPO(int idCstmPODt)
        {
            var ret = 0.00m;
            var a = stockCustomerPO.Where(x => x.idCstmPODt == idCstmPODt && x.StockAll > 0).ToList();
            if (a.Count > 0)
                ret = a.Sum(x => x.StockAll);
            return ret;
        }
        public decimal findBackOrder_CustomerPO(int idCstmPODt)
        {
            var ret = 0.00m;
            var a = stockCustomerPO.Where(x => x.idCstmPODt == idCstmPODt && x.StockAll > 0).ToList();
            if (a.Count > 0)
                ret = a.Sum(x => x.BackOrder);
            return ret;
        }
        public decimal findStock_Free()
        {
            var ret = 0.00m;
            var a = stockFree_List.Where(x => x.QtyFree > 0).ToList();
            if (a.Count > 0)
                ret = a.Sum(x => x.QtyFree);
            return ret;
        }

        public void cutStock_CstmPO(int idCstmPODt, ref decimal useQty)
        {
            var a = stockCustomerPO.Where(x => x.idCstmPODt == idCstmPODt && x.StockAll > 0).FirstOrDefault();
            if (a != null)
            {
                if (useQty >= a.StockAll)
                {
                    useQty -= a.StockAll;
                    a.StockAll = 0;
                }
                else
                {
                    a.StockAll -= useQty;
                    useQty = 0;
                }
            }
        }
        public void cutStock_Free(int idCstmPODt, ref decimal useQty, ref List<stockReserve> sReserve)
        {
            var a = stockFree_List.Where(x => x.QtyFree > 0).ToList();
            foreach (var aa in a)
            {
                var sr = sReserve.Where(x => x.ItemNo == this.ItemNo && x.idCstmPODt == idCstmPODt
                    && x.idCstmPODt_Free == aa.idCstmPODt_Free && x.id_tb_Stock == aa.id_tb_Stock).FirstOrDefault();
                if (sr == null)
                {
                    sr = new stockReserve();
                    sr.ItemNo = this.ItemNo;
                    sr.idCstmPODt = idCstmPODt;
                    sr.idCstmPODt_Free = aa.idCstmPODt_Free;
                    sr.id_tb_Stock = aa.id_tb_Stock;
                    if (useQty >= aa.QtyFree)
                    {
                        sr.ReserveQty = aa.QtyFree;
                        useQty -= aa.QtyFree;
                        aa.QtyFree = 0;
                    }
                    else
                    {
                        sr.ReserveQty = useQty;
                        aa.QtyFree -= useQty;
                        useQty = 0;
                    }
                    sReserve.Add(sr);
                }
                else
                {
                    if (useQty >= aa.QtyFree)
                    {
                        sr.ReserveQty += aa.QtyFree;
                        useQty -= aa.QtyFree;
                        aa.QtyFree = 0;
                    }
                    else
                    {
                        sr.ReserveQty += useQty;
                        aa.QtyFree -= useQty;
                        useQty = 0;
                    }
                }


                if (useQty <= 0)
                    break;
            }
        }

    }

    public class stockForCustomerDt
    {//Stock ที่ถูกรับเข้ามาเพื่อ CustomerPO
        public int idCstmPODt { get; set; }
        public string ItemNo { get; set; }
        public decimal StockQty { get; set; } = 0.00m;
        public decimal BackOrder { get; set; } = 0.00m;
        public decimal StockAll { get; set; } = 0.00m; //StockQty + BackOrder
    }
    public class stockFree
    {//Stock ที่ CUstomerPO(Job) ถูกปิดไปแล้ว และยังไม่ถูกจองจะกลายเป็น Freestock
        public int id_tb_Stock { get; set; }
        public string ItemNo { get; set; }
        public int idCstmPODt_Free { get; set; }
        public decimal QtyFree { get; set; } = 0.00m;
        public decimal QtyFree_Backup { get; set; } = 0.00m;
    }
    public class stockReserve
    {
        public int id { get; set; } = 0;
        public int id_tb_Stock { get; set; }
        public string ItemNo { get; set; }
        public int idCstmPODt { get; set; }
        public int idCstmPODt_Free { get; set; }
        public decimal ReserveQty { get; set; }
    }

    public class WorkLoad
    {
        public DateTime Date { get; set; }
        public int idWorkCenter { get; set; }
        public decimal CapacityAvailable { get; set; } = 0.00m;
        public decimal CapacityAvailableX { get; set; } = 0.00m;
        public decimal CapacityAlocate { get; set; } = 0.00m;
        public decimal CapacityAlocateX { get; set; } = 0.00m;
        public decimal CapacityAfter
        {
            get {
                return CapacityAvailable - CapacityAlocate;
            }
        }
        public decimal CapacityAfterX
        {
            get {
                return CapacityAvailableX - CapacityAlocateX;
            }
        }
    }
    public class CalendarLoad
    {
        public int id { get; set; }
        public int idJob { get; set; } = 0;
        public int idRoute { get; set; } = 0;
        public int idCal { get; set; } = 0;
        public int idWorkcenter { get; set; } = 0;
        public int idHol { get; set; } = 0;
        public int idAbs { get; set; } = 0;
        public DateTime Date { get; set; }
        public TimeSpan StartingTime { get; set; }
        public TimeSpan EndingTime { get; set; }
    }
    public class CalOverhead
    {
        public int idDoc { get; set; }
        public int idWorkcenter { get; set; }
        public int idRoute { get; set; } = 0;
        public decimal CapacityX { get; set; } = 0.00m;
    }


    public class calPartData
    {
        public string ItemNo { get; set; }
        public decimal ReqQty { get; set; }
        //public decimal UseQty { get; set; }
        public string UOM { get; set; }
        public decimal PCSUnit { get; set; }
        public DateTime ReqDate { get; set; }
        public string DocNo { get; set; }
        public int DocId { get; set; }
        public ReplenishmentType repType { get; set; }

        public int mainNo { get; set; } = 0;
        public bool alreadyJob { get; set; } = false;
    }

    public enum ReplenishmentType
    {
        Purchase, //MRP
        Production //MPS
    }
    public enum ReorderType
    {
        Fixed,
        MinMax,
        ByOrder
    }
    public enum InventoryGroup
    {
        RM,
        SEMI,
        FG
    }
}
