﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using Telerik.WinControls.UI;
using Telerik.WinControls;

namespace StockControl
{
    public partial class PlanningCal : Telerik.WinControls.UI.RadRibbonForm
    {
        bool openFilter = true;
        public PlanningCal(bool openFilter = true)
        {
            InitializeComponent();
            this.openFilter = openFilter;
        }
        public PlanningCal()
        {
            InitializeComponent();
            openFilter = false;
        }

        private void radMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            HistoryView hw = new HistoryView(this.Name);
            this.Cursor = Cursors.Default;
            hw.ShowDialog();
        }

        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }
        private void Unit_Load(object sender, EventArgs e)
        {
            dgvData.ReadOnly = true;
            dgvData.AutoGenerateColumns = false;


            if (openFilter)
            {
                btnFil_Click(null, null);
            }
        }


        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                dgvData.Rows[e.RowIndex].Cells["dgvC"].Value = "T";
                //string check1 = Convert.ToString(radGridView1.Rows[e.RowIndex].Cells["VendorName"].Value);
                //string TM= Convert.ToString(radGridView1.Rows[e.RowIndex].Cells["dgvCodeTemp2"].Value);
                //if (!check1.Trim().Equals("") && TM.Equals(""))
                //{

                //    if (!CheckDuplicate(check1.Trim()))
                //    {
                //        MessageBox.Show("ชื้อผู้ขายซ้ำ ซ้ำ");
                //        radGridView1.Rows[e.RowIndex].Cells["GroupCode"].Value = "";
                //        radGridView1.Rows[e.RowIndex].Cells["GroupCode"].IsSelected = true;

                //    }
                //}


            }
            catch (Exception ex) { }
        }

        private void radGridView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //// MessageBox.Show(e.KeyCode.ToString());

            //if (e.KeyData == (Keys.Control | Keys.S))
            //{
            //    btnSave_Click(null, null);
            //}
            //else if (e.KeyData == (Keys.Control | Keys.N))
            //{
            //    if (MessageBox.Show("ต้องการสร้างใหม่ ?", "สร้างใหม่", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //    {
            //        NewClick();
            //    }

            //}
        }


        int row = -1;
        private void radGridView1_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            row = e.RowIndex;

        }


        private void MasterTemplate_CellValueChanged(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {

        }

        private void MasterTemplate_RowFormatting(object sender, RowFormattingEventArgs e)
        {

        }

        private void MasterTemplate_CellFormatting(object sender, CellFormattingEventArgs e)
        {

        }

        private void btnRecal_Click(object sender, EventArgs e)
        {
            var ft = new PlanningCal_Filter();
            ft.ShowDialog();
            if (ft.okFilter)
            {
                radLabelElement1.Text = $"Filter date : {ft.dateFrom.ToDtString()}-{ft.dateTo.ToDtString()}";
                LoadData(true, ft.dateFrom, ft.dateTo, ft.MRP, ft.MPS, ft.ItemNo, ft.locationItem);
            }
        }

        private void btnFil_Click(object sender, EventArgs e)
        {
            var ft = new PlanningCal_Filter();
            ft.ShowDialog();
            if (ft.okFilter)
            {
                radLabelElement1.Text = $"Filter date : {ft.dateFrom.ToDtString()}-{ft.dateTo.ToDtString()}";
                LoadData(false, ft.dateFrom, ft.dateTo, ft.MRP, ft.MPS, ft.ItemNo, ft.locationItem);
            }
        }
        void LoadData(bool reCal, DateTime dtFrom, DateTime dtTo, bool MRP, bool MPS, string ItemNo, string LocationItem)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                t_dtFrom = dtFrom;
                t_dtTo = dtTo;
                t_MRP = MRP;
                t_MPS = MPS;
                t_ItemNo = ItemNo;
                t_LocationItem = LocationItem;
                using (var db = new DataClasses1DataContext())
                {
                    dgvData.Rows.Clear();
                    if (reCal) Recalculate();
                }

            }
            catch (Exception ex)
            {
                baseClass.Error(ex.Message);
            }
            finally { this.Cursor = Cursors.Default; }
        }

        DateTime t_dtFrom = DateTime.Now;
        DateTime t_dtTo = DateTime.Now;
        bool t_MPS = false;
        bool t_MRP = false;
        string t_ItemNo = "";
        string t_LocationItem = "";
        void Recalculate()
        {
            try
            {
                itemDatas = new List<ItemData>();
                planDatas = new List<PlanData>();
                using (var db = new DataClasses1DataContext())
                {
                    int comp = CustomerPO_SS.Completed.ToInt();
                    var cpo = db.mh_CustomerPOs.Where(x => x.Active && x.Quantity > 0 && x.Status != comp
                                    && x.ReqDate >= t_dtFrom && x.ReqDate <= t_dtTo
                                    && x.ItemNo.Contains(t_ItemNo)).ToList();
                    var tlist = cpo.GroupBy(x => x.ItemNo).Select(x => new
                    {
                        x.First().ItemNo,
                        ReqQty = x.Sum(q => q.Quantity * q.PCSUnit)
                    }).ToList();

                    foreach (var t in tlist)
                        DrillItem(t.ItemNo, t.ReqQty);

                    foreach (var item in itemDatas)
                        CalItem(item);
                }
            }
            catch (Exception e)
            {
                baseClass.Error(e.Message);
            }
        }

        List<ItemData> itemDatas = new List<ItemData>();
        List<PlanData> planDatas = new List<PlanData>();
        public void DrillItem(string ItemNo, decimal ReqQty)
        {
            using (var db = new DataClasses1DataContext())
            {
                var item = itemDatas.Where(x => x.ItemNo == ItemNo).FirstOrDefault();
                if (item == null)
                {
                    var t = db.mh_Items.Where(x => x.InternalNo == ItemNo).FirstOrDefault();
                    var tt = new ItemData(t.InternalNo, t.InternalName);
                    tt.ReqQty = ReqQty;
                    tt.ReorderType = getReorderType(t.ReorderType);
                    tt.ReorderPoint = t.ReorderPoint.ToDecimal();
                    tt.ReorderQty = t.ReorderQty;
                    tt.MinQty = t.MinimumQty;
                    tt.MaxQty = t.MaximumQty;
                    tt.SafetyStock = t.SafetyStock;
                    tt.TimeBuket = t.Timebucket.ToInt();
                    tt.LeadTime = t.InternalLeadTime;
                    tt.repType = getRepType(t.ReplenishmentType);
                    tt.invGroup = getInvGroup(t.InventoryGroup);
                    itemDatas.Add(tt);
                    item = tt;
                }
                else
                {
                    item.ReqQty += ReqQty;
                }

                //BOMs
                var boms = (from x in db.tb_BomHDs join y in db.tb_BomDTs on x.PartNo equals y.PartNo where x.PartNo == ItemNo select y).ToList();
                foreach (var b in boms)
                {
                    var rQty = Math.Round(ReqQty * b.Qty * b.PCSUnit.ToDecimal(), 2);
                    DrillItem(b.Component, rQty);
                }
            }
        }
        public void CalItem(ItemData item)
        {
            using (var db = new DataClasses1DataContext())
            {
                //find data
                if (t_MRP && item.repType == ReplenishmentType.Purchase) //Purchase
                {
                    //1. Reorder from Reorder Type
                    //2. Order from Calculate Qty
                    //3. After Shipment check Stock Qty for Order from Reorder Type

                }
                else if (t_MPS && item.repType == ReplenishmentType.Production) //Production
                {

                }
            }
        }

        private InventoryGroup getInvGroup(string inventoryGroup)
        {
            switch (inventoryGroup)
            {
                case "RM": return InventoryGroup.RM;
                case "SEMI": return InventoryGroup.Semi;
                case "FG": return InventoryGroup.FG;
                default: return InventoryGroup.RM;
            }
        }
        public ReorderType getReorderType(string reName)
        {
            switch (reName)
            {
                case "Fixed Reorder Qty": return ReorderType.Fixed;
                case "Minimum & Maximum Qty": return ReorderType.MinMax;
                case "By Order": return ReorderType.ByOrder;
                default: return ReorderType.Fixed;
            }
        }
        public ReplenishmentType getRepType(string reName)
        {
            switch (reName)
            {
                case "Purchase": return ReplenishmentType.Purchase;
                case "Production": return ReplenishmentType.Production;
                default: return ReplenishmentType.Purchase;
            }
        }



    }

    public class ItemData
    {
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
