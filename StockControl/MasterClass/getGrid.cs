using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockControl
{
    public static class getGrid
    {
        public static List<grid_CustomerPO> GetGrid_CustomerPO(string CustomerPO = ""
            , string CSTMNo = "", string Item = "", DateTime? dFrom = null, DateTime? dTo = null)
        {
            var l = new List<grid_CustomerPO>();
            using (var db = new DataClasses1DataContext())
            {
                try
                {
                    var m = db.mh_CustomerPOs.Where(x => x.Active
                        && (CustomerPO == "" || x.CustomerPONo == CustomerPO)
                        && (CSTMNo == "" || x.CustomerNo == CSTMNo)
                        && (dFrom == null || (x.OrderDate >= dFrom && x.OrderDate <= dTo))).ToList();
                    foreach (var hd in m)
                    {
                        var t = db.mh_CustomerPODTs.Where(x => x.Active && x.idCustomerPO == hd.id).ToList();
                        foreach (var dt in t)
                        {
                            if (Item != "" && dt.ItemNo != Item) continue;
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
                                idCustomerPO = dt.idCustomerPO
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
            get
            {
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
            get
            {
                return 0;
                //not imprement
                //----Remain cut by Shipment
            }
        }
        public decimal Shipped
        {
            get
            {
                return Qty - Remain;
            }
        }
        public bool Plan
        {
            get
            {
                return (Qty * PCSUnit != OutPlan);
                //find from JobNo
            }
        }
        public bool SaleOrder
        {
            get
            {
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


    }
    public class grid_Planning
    {
        public string Status { get; set; }
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime EndingDate { get; set; }
        public decimal Qty { get; set; }
        public string PlanningType { get; set; }
        public int idRef { get; set; }
        public string RefDocNo { get; set; }
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


    public class PDPlan
    {
        public ItemData Item { get; set; }
        public string CstmNo { get; set; }
        public string RefDocNo { get; set; }
        public int RefId { get; set; }
        public DateTime ReqDate { get; set; }
        public decimal ReqQty { get; set; }

    }
    public class POPlan
    {
        public ItemData Item { get; set; }
        public List<DocPlan> DocRef { get; set; }
        public decimal ReqQty { get; set; }

    }
    public class DocPlan
    {
        public string DocNo { get; set; }
        public int DocId { get; set; }
        public decimal ReqQty { get; set; }
    }


    public class ItemData
    {
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public ReorderType ReorderType { get; set; }
        public decimal QtyOnHand { get; set; }
        public decimal SafetyStock { get; set; }
        public decimal ReorderPoint { get; set; }
        public decimal ReorderQty { get; set; }
        public decimal MinQty { get; set; }
        public decimal MaxQty { get; set; }
        public decimal MaximumInventory { get; set; }
        public int TimeBuket { get; set; }
        public int LeadTime { get; set; }
        public ReplenishmentType repType { get; set; }
        public InventoryGroup invGroup { get; set; }

        public decimal StockQty
        {
            get
            {
                return baseClass.StockQty(ItemNo, "Warehouse");
            }
        }
        public DateTime ReqDate { get; set; }

        public ItemData(string ItemNo, string ItemName = "")
        {
            this.ItemNo = ItemNo;
            this.ItemName = ItemName;
            using (var db = new DataClasses1DataContext())
            {
                var t = db.mh_Items.Where(x => x.InternalNo == this.ItemNo).FirstOrDefault();
                if (t == null) return;

                this.ItemName = t.InternalName;
                this.ReorderType = baseClass.getReorderType(t.ReorderType);
                this.QtyOnHand = baseClass.StockQty(this.ItemNo, "Warehouse");
                this.SafetyStock = t.SafetyStock;
                this.ReorderPoint = t.ReorderPoint.ToDecimal();
                this.ReorderQty = t.ReorderQty.ToDecimal();
                this.MinQty = t.MinimumQty;
                this.MaxQty = t.MaximumQty;
                this.MaximumInventory = t.MaximumInventory.ToDecimal();
                this.TimeBuket = t.Timebucket.ToInt();
                this.LeadTime = t.InternalLeadTime.ToInt();
                this.repType = baseClass.getRepType(t.ReplenishmentType);
                this.invGroup = baseClass.getInventoryGroup(t.InventoryGroup);
            }

        }

    }
    public class PlanData
    {
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime EndingDate { get; set; }
        public decimal Qty { get; set; }
        public ReplenishmentType repType { get; set; }
        public string PlanningType
        {
            get
            {
                if (repType == ReplenishmentType.Production)
                    return "MPS";
                else
                    return "MRP";
            }
        }
        public string RefOrderType
        {
            get
            {
                return repType.ToSt();
            }
        }

        public PlanData(string ItemNo, string ItemName)
        {
            this.ItemNo = ItemNo;
            this.ItemName = ItemName;
        }
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
