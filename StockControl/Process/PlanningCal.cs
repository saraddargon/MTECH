using System;
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
using System.Threading;

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
            //dgvData.ReadOnly = true;
            dgvData.AutoGenerateColumns = false;


            if (openFilter)
            {
                btnFil_Click(null, null);
            }

            dgvData.Columns.ToList().ForEach(x =>
            {
                x.ReadOnly = x.Name != "S";
            });
        }

        void demo()
        {
            dgvData.DataSource = null;
            dgvData.Rows.Clear();

            addrow("OK", "I0001", "Item FG 1", new DateTime(2018, 9, 30)
                , new DateTime(2018, 9, 10), new DateTime(2018, 9, 28), 100, "Production", "CSTMPO1809-001");
            addrow("OK", "I0002", "Item FG 2", new DateTime(2018, 9, 30)
                , new DateTime(2018, 9, 12), new DateTime(2018, 9, 20), 100, "Production", "CSTMPO1809-002");
            addrow("OK", "I0003", "Item FG 3", new DateTime(2018, 9, 30)
                , new DateTime(2018, 9, 12), new DateTime(2018, 9, 30), 100, "Production", "CSTMPO1809-003");
            addrow("Over Due", "I0004", "Item FG 4", new DateTime(2018, 9, 30)
                , new DateTime(2018, 9, 15), new DateTime(2018, 10, 5), 100, "Production", "CSTMPO1809-004");
        }
        void addrow(string SS, string Item, string ItemName, DateTime DueDate, DateTime StartDate, DateTime EndDate, decimal Qty, string PlanType, string RefDocNo)
        {
            var rowe = dgvData.Rows.AddNew();
            rowe.Cells["SS"].Value = SS;
            rowe.Cells["Item"].Value = Item;
            rowe.Cells["ItemName"].Value = ItemName;
            rowe.Cells["DueDate"].Value = DueDate;
            rowe.Cells["StartingDate"].Value = StartDate;
            rowe.Cells["EndingDate"].Value = EndDate;
            rowe.Cells["Quantity"].Value = Qty;
            rowe.Cells["PlanningType"].Value = PlanType;
            rowe.Cells["RefDocNo"].Value = RefDocNo;
        }
        //


        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {



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
                //radLabelElement1.Text = $"Filter date : {ft.dateFrom.ToDtString()}-{ft.dateTo.ToDtString()}";
                //LoadData(true, ft.dateFrom, ft.dateTo, ft.MRP, ft.MPS, ft.ItemNo, ft.locationItem);
                var ps = new PlanningCal_Status();
                ps.ShowDialog();
                demo();
            }
        }
        private void btnFil_Click(object sender, EventArgs e)
        {
            var ft = new PlanningCal_Filter();
            ft.ShowDialog();
            if (ft.okFilter)
            {
                //radLabelElement1.Text = $"Filter date : {ft.dateFrom.ToDtString()}-{ft.dateTo.ToDtString()}";
                //LoadData(false, ft.dateFrom, ft.dateTo, ft.MRP, ft.MPS, ft.ItemNo, ft.locationItem);
                var ps = new PlanningCal_Status();
                ps.ShowDialog();
                demo();
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
                    if (reCal)
                    {
                        Recalculate();
                    }
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
        DateTime t_ReqDate = DateTime.Now;
        DateTime minDate
        {
            get {
                if (DateTime.Now.Date > t_dtFrom.Date)
                    return DateTime.Now.Date;
                else
                    return t_dtFrom.Date;
            }
        }
        void Recalculate()
        {
            var pc = new PlanningCal_Status();
            pc.ShowDialog();
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
        public DateTime setStandardTime(DateTime dt, bool TimeStart)
        {
            using (var db = new DataClasses1DataContext())
            {
                var g = db.mh_ManufacturingSetups.FirstOrDefault();
                if (TimeStart)
                    dt = dt.AddHours(g.StandardStartingTime.Hours).AddMinutes(g.StandardStartingTime.Minutes);
                else
                    dt = dt.AddHours(g.StandardEndingTime.Hours).AddMinutes(g.StandardEndingTime.Minutes);
                return dt;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            dgvData.EndEdit();
            GenE();
        }
        void GenE()
        {
            if(dgvData.Rows.Where(x=>x.Cells["S"].Value.ToBool()).ToList().Count > 0)
            {
                if(dgvData.Rows.Where(x=>x.Cells["S"].Value.ToBool() && x.Cells["SS"].Value.ToSt() == "Over Due").Count() > 0)
                    return;
                this.Cursor = Cursors.WaitCursor;
                Thread.Sleep(3000);
                baseClass.Info("Generate to Production/Purchase complete.!!");
                this.Cursor = Cursors.Default;
            }
        }

    }

}
