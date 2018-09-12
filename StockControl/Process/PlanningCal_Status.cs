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
                Thread td = new Thread(new ThreadStart(calE));
                td.Start();
                startCal = false;
            }
        }


        List<ItemData> itemDatas = new List<ItemData>();
        void calE()
        {
            try
            {
                itemDatas.Clear();
                
                var cstmPO_List = new List<CustomerPOCal>();
                using (var db = new DataClasses1DataContext())
                {
                    //1.Get Customer P/O (OutPlan) and SaleOrder (OutPlan) [Only not customer P/O]
                    var poDt = db.mh_CustomerPODTs.Where(x => x.Active
                        && x.ReqDate >= dFrom && x.ReqDate <= dTo
                    ).OrderBy(x => x.ReqDate).ToList();
                    foreach (var dt in poDt)
                    {
                        if (ItemNo != "" && dt.ItemNo != ItemNo) continue;
                        var t = db.mh_Items.Where(x => x.InternalNo == dt.ItemNo).FirstOrDefault();
                        if (t == null) continue;
                        if (LocationItem != "" && t.Location != LocationItem) continue;

                        var pohd = db.mh_CustomerPOs.Where(x => x.id == dt.idCustomerPO && x.Active).FirstOrDefault();
                        if (pohd == null) continue;

                        cstmPO_List.Add(new CustomerPOCal
                        {
                            POHd = pohd,
                            PODt = dt
                        });
                    }

                    //2.Loop order by Due date (Dt) then Order date (Hd) then Order id (Hd)
                    cstmPO_List = cstmPO_List.OrderBy(x => x.PODt.ReqDate)
                        .ThenBy(x => x.POHd.OrderDate).ThenBy(x => x.POHd.id).ToList();
                    foreach (var item in cstmPO_List)
                    {
                        calPart(new calPartData
                        {
                            DocId = item.PODt.id,
                            DocNo = item.POHd.CustomerNo,
                            ItemNo = item.PODt.ItemNo,
                            repType = baseClass.getRepType(item.PODt.ReplenishmentType),
                            ReqDate = item.PODt.ReqDate,
                            ReqQty = item.PODt.OutPlan,
                        });
                    }
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
        void calPart(calPartData data)
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
                    }
                    //3.Find Stock is enought ?
                    if (data.ReqQty <= tdata.QtyOnHand) //3.1 Stock is enought
                    {
                        tdata.QtyOnHand -= data.ReqQty;
                        return;
                    }
                    //3.2 Stock not enought
                    var t_QtyOnHand = tdata.QtyOnHand;
                    data.ReqQty -= t_QtyOnHand;
                    t_QtyOnHand = 0;
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
    }
}
