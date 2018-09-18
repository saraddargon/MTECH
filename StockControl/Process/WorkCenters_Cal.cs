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
    public partial class WorkCenters_Cal : Telerik.WinControls.UI.RadRibbonForm
    {
        public WorkCenters_Cal()
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
                var t = db.mh_WorkCenters.Where(x => x.Active).ToList();
                cbbWork.Items.Add(new RadListDataItem("", 0));
                foreach (var tt in t)
                    cbbWork.Items.Add(new RadListDataItem
                    {
                        Text = tt.WorkCenterName,
                        Value = tt.id
                    });
            }

        }

        System.Threading.Thread td;
        bool startC = false;
        private void btnRecal_Click(object sender, EventArgs e)
        {
            if (!startC)
            {
                startC = true;
                td = new System.Threading.Thread(new System.Threading.ThreadStart(Calculate));
                td.Start();
            }
        }
        void Calculate()
        {
            //this.Cursor = Cursors.WaitCursor;
            try
            {
                set_lbStatus("Calculating...");
                using (var db = new DataClasses1DataContext())
                {
                    int id = cbbWork.SelectedValue.ToInt();
                    var ts = db.mh_WorkCenters.Where(x => x.Active && (x.id == id || id == 0)).ToList();
                    foreach (var t in ts)
                    {
                        
                        set_lbStatus($"Calculating...[{t.WorkCenterNo}]:{t.WorkCenterName}");
                        //UOM : Minute, Hour, Day
                        //minWork*Capacity
                        DateTime dTemp = dtFrom.Value.Date.AddDays(-1);
                        do
                        {
                            dTemp = dTemp.AddDays(1);
                            if (dTemp > dtTo.Value.Date)
                                break;

                            set_lbStatus($"Calculating...[{t.WorkCenterNo}] : {t.WorkCenterName} - {dTemp.ToDtString()}");
                            //find total minute day of work
                            baseClass.CalCapacity_WorkCenter(t.id, dTemp);

                            //next loop
                        }
                        while (dTemp <= dtTo.Value.Date);

                    }

                    //save
                    set_lbStatus("Calculate completed -- Saving....");
                    db.SubmitChanges();
                }

                set_lbStatus("Calculate Work Center complete.");
            }
            catch (Exception ex)
            {
                baseClass.Error(ex.Message);
            }
            finally
            {
                //this.Cursor = Cursors.Default;
                startC = false;
            }
        }
        void set_lbStatus(string x)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                lbStatus.Text = x;
                lbStatus.Visible = true;
                this.Update();
                System.Threading.Thread.Sleep(10);
            }));
        }



        private void btnCapaView_Click(object sender, EventArgs e)
        {
            if (!startC)
            {
                var capa = new WorkCenters_Capa();
                capa.ShowDialog();
            }
        }

        private void WorkCenters_Cal_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = startC;
        }
    }
}
