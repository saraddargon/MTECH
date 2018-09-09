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
                //1.Get Customer P/O (OutPlan) and SaleOrder (OutPlan) [Only not customer P/O]
                using (var db = new DataClasses1DataContext())
                {
                    var cstmPOs = db.mh_CustomerPOs.Where(x => x.Active
                                && x.OutPlan > 0 && x.Status != "Completed"
                                && x.ReqDate >= dtFrom && x.ReqDate <= dtTo
                                && x.ItemNo.Contains(ItemNo) /*Location*/
                            ).ToList();
                    var saleOrders = db.mh_SaleOrders.Where(x => x.Active
                                && x.RefId == 0 && x.OutPlan > 0
                                && x.Status != "Completed" //not Refer with Customer P/O
                                && x.ReqDeliveryDate >= dtFrom && x.ReqDeliveryDate <= dtTo
                                && x.ItemNo.Contains(ItemNo)
                            ).ToList();
                }
            }
            catch (Exception ex)
            {
                baseClass.Error(ex.Message);
            }
            finally
            {
                startCal = false;
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

    }


    public class ItemData
    {
        public int rNo { get; set; } = 0;
        public int ref_rNo { get; set; } = 0;
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public decimal ReqQty { get; set; }
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
            get
            {
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
        Semi,
        FG
    }
}
