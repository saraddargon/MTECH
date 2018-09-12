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
                    foreach(var hd in m)
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

}
