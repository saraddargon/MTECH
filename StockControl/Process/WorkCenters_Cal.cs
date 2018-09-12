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
                        var cal = db.mh_Calendars.Where(x => x.id == t.Calendar).First(); //Calendar
                        var dow = db.mh_WorkingDays.Where(x => x.Active && x.idCalendar == cal.id).ToList(); //0:Mon - 6:Sun
                        var hol = db.mh_Holidays.Where(x => x.Active && x.idCalendar == cal.id).ToList(); //Holiday
                        var abs = db.mh_CapacityAbsences.Where(x => x.Active && x.idWorkCenters == t.id).ToList(); //Absence
                        do
                        {
                            dTemp = dTemp.AddDays(1);
                            if (dTemp > dtTo.Value.Date)
                                break;

                            set_lbStatus($"Calculating...[{t.WorkCenterNo}] : {t.WorkCenterName} - {dTemp.ToDtString()}");
                            //find total minute day of work
                            decimal minWork = 0.00m;
                            //not day of work
                            if (!dow.Select(x => GetDayOfWeek(x.Day)).Contains(dTemp.DayOfWeek))
                            {
                                minWork = -1;
                            }
                            else
                            {
                                //find minute of work
                                foreach (var d in dow.Where(x => GetDayOfWeek(x.Day) == dTemp.DayOfWeek))
                                {
                                    var sTime = new TimeSpan(d.StartingTime.Substring(0, 2).ToInt(), d.StartingTime.Substring(3).ToInt(), 0);
                                    var eTime = new TimeSpan(d.EndingTime.Substring(0, 2).ToInt(), d.EndingTime.Substring(3).ToInt(), 0);
                                    minWork += (eTime - sTime).TotalMinutes.ToDecimal() * t.Capacity;
                                }

                                //chk hol
                                if (minWork > 0 && hol.Where(x => dTemp >= x.StartingDate && dTemp <= x.EndingDate).Count() > 0)
                                {
                                    foreach (var h in hol.Where(x => dTemp >= x.StartingDate && dTemp <= x.EndingDate))
                                    {
                                        var sTime = new TimeSpan(h.StartTime.Substring(0, 2).ToInt(), h.StartTime.Substring(3).ToInt(), 0);
                                        var eTime = new TimeSpan(h.EndingTime.Substring(0, 2).ToInt(), h.EndingTime.Substring(3).ToInt(), 0);
                                        if (eTime > new TimeSpan(0, 0, 0))
                                            minWork -= ((eTime - sTime).TotalMinutes).ToDecimal() * t.Capacity;
                                        else
                                        {
                                            eTime = new TimeSpan(23, 59, 0);
                                            minWork -= ((eTime - sTime).TotalMinutes + 1).ToDecimal() * t.Capacity;
                                        }
                                        if (minWork < 0)
                                            break;
                                    }
                                }

                                //chk absence
                                if (minWork > 0 && abs.Where(x => x.Date.Date == dTemp.Date).Count() > 0)
                                {
                                    foreach (var a in abs.Where(x => x.Date.Date == dTemp.Date))
                                    {
                                        var sTime = new TimeSpan(a.StartingTime.Substring(0, 2).ToInt(), a.StartingTime.Substring(3).ToInt(), 0);
                                        var eTime = new TimeSpan(a.EndingTime.Substring(0, 2).ToInt(), a.EndingTime.Substring(3).ToInt(), 0);
                                        minWork -= (eTime - sTime).TotalMinutes.ToDecimal() * a.Capacity;

                                        if (minWork < 0)
                                            break;
                                    }
                                }
                            }

                            //find capa
                            //Minwork * capa
                            decimal? totalCapa = null;
                            if (minWork > 0)
                                totalCapa = minWork.ToDecimal();
                            var mh = db.mh_CapacityAvailables.Where(x => x.WorkCenterID == t.id && x.Date == dTemp).FirstOrDefault();
                            if (mh == null)
                            {
                                mh = new mh_CapacityAvailable();
                                db.mh_CapacityAvailables.InsertOnSubmit(mh);
                            }
                            mh.Date = dTemp;
                            mh.Capacity = totalCapa;
                            mh.WorkCenterID = t.id;


                            //next loop
                        }
                        while (dTemp <= dtTo.Value.Date);

                    }

                    //save
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


        DayOfWeek GetDayOfWeek(int id)
        {
            if (id == 0) return DayOfWeek.Monday;
            else if (id == 1) return DayOfWeek.Tuesday;
            else if (id == 2) return DayOfWeek.Wednesday;
            else if (id == 3) return DayOfWeek.Thursday;
            else if (id == 4) return DayOfWeek.Friday;
            else if (id == 5) return DayOfWeek.Saturday;
            else return DayOfWeek.Sunday;


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
