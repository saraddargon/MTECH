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
            if(e.RowIndex >= 0)
            {
                if(e.Column.Name.Equals("S") && e.Row.Cells["S"].Value.ToBool())
                {
                    if (e.Row.Cells["Status"].Value.ToSt() == "Over Due")
                        e.Row.Cells["S"].Value = false;
                }
            }
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
                //LoadData(true, ft.dateFrom, ft.dateTo, ft.MRP, ft.MPS, ft.ItemNo, ft.locationItem);
                var ps = new PlanningCal_Status();
                ps.dFrom = ft.dateFrom;
                ps.dTo = ft.dateTo;
                ps.ItemNo = ft.ItemNo;
                ps.LocationItem = ft.locationItem;
                ps.ShowDialog();
                if (ps.calComplete)
                {
                    dgvData.DataSource = null;
                    dgvData.AutoGenerateColumns = false;
                    dgvData.DataSource = ps.gridPlans.OrderBy(x => x.ReqDate).ToList();
                }
            }
        }
        private void btnFil_Click(object sender, EventArgs e)
        {
            var ft = new PlanningCal_Filter();
            ft.ShowDialog();
            if (ft.okFilter)
            {

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
            get
            {
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


        private void btnGenerate_Click(object sender, EventArgs e)
        {
            dgvData.EndEdit();
            GenE();
        }
        void GenE()
        {
            if (dgvData.Rows.Where(x => x.Cells["S"].Value.ToBool()).ToList().Count > 0)
            {
                if (dgvData.Rows.Where(x => x.Cells["S"].Value.ToBool() && x.Cells["SS"].Value.ToSt() == "Over Due").Count() > 0)
                    return;
                this.Cursor = Cursors.WaitCursor;
                Thread.Sleep(3000);
                baseClass.Info("Generate to Production/Purchase complete.!!");
                this.Cursor = Cursors.Default;
            }
        }

        private void MasterTemplate_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.Column.Name.Equals("S"))
                {
                    e.Cancel = e.Row.Cells["Status"].Value.Equals("Over Due");
                }
            }
        }
    }

}
