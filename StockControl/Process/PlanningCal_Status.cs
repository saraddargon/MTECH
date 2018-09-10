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
        private DateTime dtFrom = DateTime.Now;
        private DateTime dtTo = DateTime.Now;
        private string ItemNo = "";
        private string Location = "";

        public PlanningCal_Status()
        {
            InitializeComponent();
        }
        public PlanningCal_Status(DateTime dFrom, DateTime dTo, string ItemNo, string Location)
        {
            InitializeComponent();
            this.dtFrom = dFrom;
            this.dtTo = dTo;
            this.ItemNo = ItemNo;
            this.Location = Location;
        }

        bool startCal = false;
        private void PlanningCal_Status_Load(object sender, EventArgs e)
        {
            if (!startCal)
            {
                Thread td = new Thread(new ThreadStart(calE));
                td.Start();
                startCal = false;
            }
        }


        void calE()
        {
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    int i = 1;
                    do
                    {
                        changeLabel($"Calculating...CSTMPO1809-{(i).ToString("000")}");
                        System.Threading.Thread.Sleep(1000);
                    } while (i++ < 5);
                    changeLabel("Calculating complete...");
                    System.Threading.Thread.Sleep(1000);
                    startCal = false;
                    this.Invoke(new MethodInvoker(() =>
                    {
                        this.Close();
                    }));


                    ////1.Get Customer P/O (OutPlan) and SaleOrder (OutPlan) [Only not customer P/O]
                    //var cstmPOs = db.mh_CustomerPOs.Where(x => x.Active
                    //            && x.OutPlan > 0 && x.PlanStatus != "Completed"
                    //            && x.ReqDate >= dtFrom && x.ReqDate <= dtTo
                    //            && x.ItemNo.Contains(ItemNo) /*Location*/
                    //        ).ToList();
                    //var saleOrders = db.mh_SaleOrders.Where(x => x.Active
                    //            && x.RefId == 0 && x.OutPlan > 0
                    //            && x.PlanStatus != "Completed" //not Refer with Customer P/O
                    //            && x.ReqDeliveryDate >= dtFrom && x.ReqDeliveryDate <= dtTo
                    //            && x.ItemNo.Contains(ItemNo)
                    //        ).ToList();
                    //// หา PD กับ PO
                    //foreach (var item in cstmPOs)
                    //{
                    //    var t = db.mh_Items.Where(x => x.InternalNo == item.ItemNo).FirstOrDefault();
                    //    calPart(item.ItemNo, item.OutPlan, item.ReqDate, item.CustomerPONo, item.id, item.ForcastType);
                    //}
                    //foreach (var item in saleOrders)
                    //{
                    //    var t = db.mh_Items.Where(x => x.InternalNo == item.ItemNo).FirstOrDefault();
                    //    calPart(item.ItemNo, item.OutPlan, item.ReqDeliveryDate, item.SONo, item.id, item.RepType);
                    //}
                }
            }
            catch (Exception ex)
            {
                baseClass.Warning("CalE : " + ex.Message);
            }
            finally
            {
                startCal = false;
            }
        }
        void calPart(string itemNo, decimal reqQty, DateTime reqDate, string DocNo, int DocId, string RepType)
        {
            try
            {
                //RepType == Production, Purchase
                using (var db = new DataClasses1DataContext())
                {

                }
            }
            catch (Exception ex)
            {
                baseClass.Warning("Cal Part : " + ex.Message);
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
            if (startCal)
                e.Cancel = true;
        }
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
        public int rNo { get; set; } = 0;
        public int ref_rNo { get; set; } = 0;
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public ReorderType ReorderType { get; set; }
        public decimal QtyOnHand { get; set; }
        public decimal SafetyStock { get; set; }
        public decimal ReorderPoint { get; set; }
        public decimal ReorderQty { get; set; }
        public decimal MinQty { get; set; }
        public decimal MaxQty { get; set; }
        public int TimeBuket { get; set; }
        public int LeadTime { get; set; }
        public ReplenishmentType repType { get; set; }
        public InventoryGroup invGroup { get; set; }

        public decimal StockQty
        {
            get {
                return baseClass.StockQty(ItemNo, "Warehouse");
            }
        }
        public DateTime ReqDate { get; set; }

        public ItemData(string ItemNo, string ItemName)
        {
            this.ItemNo = ItemNo;
            this.ItemName = ItemName;

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
            get {
                if (repType == ReplenishmentType.Production)
                    return "MPS";
                else
                    return "MRP";
            }
        }
        public string RefOrderType
        {
            get {
                return repType.ToSt();
            }
        }

        public PlanData(string ItemNo, string ItemName)
        {
            this.ItemNo = ItemNo;
            this.ItemName = ItemName;
        }
    }

    //1 Job --> 1 Customer P/O or 1 Sale Order, 1 FG
    //RM รวมกันได้



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
        Semi,
        FG
    }
}
