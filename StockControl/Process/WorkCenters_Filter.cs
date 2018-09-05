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

namespace StockControl
{
    public partial class WorkCenters_Filter : Telerik.WinControls.UI.RadRibbonForm
    {
        public bool okFilter = false;
        public DateTime dateFrom { get; private set; }
        public DateTime dateTo { get; private set; }
        public int workId { get; private set; } = 0;

        public WorkCenters_Filter()
        {
            InitializeComponent();
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
            dtFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtTo.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            using (var db = new DataClasses1DataContext())
            {
                cbbWork.Items.Add(new RadListDataItem("", 0));
                var t = db.mh_WorkCenters.Where(x => x.Active).ToList();
                foreach (var tt in t)
                    cbbWork.Items.Add(new RadListDataItem(tt.WorkCenterName, tt.id));
            }

        }

        private void btnRecal_Click(object sender, EventArgs e)
        {
            Calculate();
        }
        void Calculate()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                okFilter = true;
                dateFrom = dtFrom.Value.Date;
                dateTo = dtTo.Value.Date;
                workId = cbbWork.SelectedValue.ToInt();

                this.Close();
            }
            catch (Exception ex)
            {
                baseClass.Error(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

    }
}
