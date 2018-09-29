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
using System.Globalization;
using System.Drawing.Drawing2D;
using System.IO;

namespace StockControl
{
    public partial class GanttCharts_Job : Telerik.WinControls.UI.RadRibbonForm
    {
        public GanttCharts_Job()
        {
            InitializeComponent();
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
            //GETDataTable();
            cbbView.SelectedIndex = 2;
            InitializeGanttView();
            radButton2_Click(null, null);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            radButton2_Click(null, null);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    string err = "";
            //    if (txtInvoiceNo.Text.Trim().Equals(""))
            //    {
            //        err = "กรุณาเลือก Invoice No , DL No เดิม \n";
            //    }
            //    if (txtInvoiceNo2.Text.Trim().Equals(""))
            //    {
            //        err += "เลขที่ Invoice No , DL No ใหม่ ";
            //    }

            //    if(!err.Equals(""))
            //    {
            //        MessageBox.Show(err);
            //        return;
            //    }

            //    this.Cursor = Cursors.WaitCursor;
            //    if (MessageBox.Show("ต้องการบันทึก ?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //    {
            //        using (DataClasses1DataContext db = new DataClasses1DataContext())
            //        {
            //            var g = (from ix in db.tb_ReceiveHs
            //                     where ix.InvoiceNo.Trim() == txtInvoiceNo.Text.Trim() && ix.Status != "Cancel"
            //                     //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
            //                     select ix).ToList();
            //            if (g.Count > 0)  //มีรายการในระบบ
            //            {
            //                //Herder
            //                string RCNo = StockControl.dbClss.TSt(g.FirstOrDefault().RCNo);
            //                var gg = (from ix in db.tb_ReceiveHs
            //                          where ix.InvoiceNo.Trim() == txtInvoiceNo.Text.Trim() && ix.Status != "Cancel"
            //                          //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
            //                          select ix).First();

            //                gg.UpdateBy = ClassLib.Classlib.User;
            //                gg.UpdateDate = DateTime.Now;
            //                gg.InvoiceNo = txtInvoiceNo2.Text;


            //                //detail
            //                var vv = (from ix in db.tb_Receives
            //                          where ix.InvoiceNo.Trim() == txtInvoiceNo.Text.Trim() && ix.Status != "Cancel"
            //                          //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
            //                          select ix).First();

            //                vv.InvoiceNo = txtInvoiceNo2.Text;


            //                db.SubmitChanges();
            //                //dbClss.AddHistory(this.Name + txtInvoiceNo.Text.Trim(), "เปลี่ยนเลขที่ Invoice/DL No การรับ Receive", " โดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", "");
            //                //dbClss.AddHistory(this.Name + RCNo, "เปลี่ยนเลขที่ Invoice/DL No การรับ Receive", " โดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", "");
            //                //dbClss.AddHistory(this.Name, "เปลี่ยนเลขที่ Invoice/DL No การรับ Receive"," เลขเดิม : " +txtInvoiceNo.Text + " เลขใหม่ : "+ txtInvoiceNo2.Text +" โดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", "");
            //                dbClss.AddHistory(this.Name , "เปลี่ยนเลขที่ Invoice/DL No การรับ Receive", " โดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", txtInvoiceNo.Text.Trim());
            //                dbClss.AddHistory(this.Name, "เปลี่ยนเลขที่ Invoice/DL No การรับ Receive", " โดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", RCNo);
            //                dbClss.AddHistory(this.Name, "เปลี่ยนเลขที่ Invoice/DL No การรับ Receive", " เลขเดิม : " + txtInvoiceNo.Text + " เลขใหม่ : " + txtInvoiceNo2.Text + " โดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", txtInvoiceNo2.Text);
            //                dbClss.AddHistory(this.Name, "เปลี่ยนเลขที่ Invoice/DL No การรับ Receive", " เลขเดิม : " + txtInvoiceNo.Text + " เลขใหม่ : " + txtInvoiceNo2.Text + " โดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", RCNo);

            //            }
            //        }
            //        MessageBox.Show("บันทึกสำเร็จ!");
            //        txtInvoiceNo.Text = "";
            //        txtInvoiceNo2.Text = "";
            //    }
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
            //finally { this.Cursor = Cursors.Default; }
        }

        private void Unit_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // MessageBox.Show(e.KeyCode.ToString());
        }


        int row = -1;
        private void radMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                this.radGanttView1.DataSource = null;
                try
                {
                    this.radGanttView1.DataSource = GetData();
                }
                catch { }
                DateTime dFrom = dtFrom.Value.Date;
                DateTime dTo = dtTo.Value.Date.AddDays(1).AddMinutes(-1);
                //DateTime dTo = dFrom.AddDays(5);
                //if(tasks.Rows.Count > 0)
                //{
                //    dFrom = tasks.Rows.Cast<DataRow>().Min(x => x["Start"].ToDateTime().Value);
                //    dTo = tasks.Rows.Cast<DataRow>().Max(x => x["End"].ToDateTime().Value);
                //}
                this.radGanttView1.GanttViewElement.GraphicalViewElement.TimelineStart = dFrom;
                this.radGanttView1.GanttViewElement.GraphicalViewElement.TimelineEnd = dTo;
                //this.radGanttView1.EnableCustomPainting = true;
                //radGanttView1.GanttViewElement.GraphicalViewElement.OnePixelTime = new TimeSpan(0, 1, 0);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        public void InitializeGanttView()
        {
            this.radGanttView1.GanttViewElement.TaskDataMember = "Tasks";
            this.radGanttView1.GanttViewElement.ChildMember = "id";
            this.radGanttView1.GanttViewElement.ParentMember = "idD";
            this.radGanttView1.GanttViewElement.TitleMember = "Title";
            this.radGanttView1.GanttViewElement.StartMember = "Start";
            this.radGanttView1.GanttViewElement.EndMember = "End";

            this.radGanttView1.GanttViewElement.ProgressMember = "Progress";
            this.radGanttView1.GanttViewElement.LinkDataMember = "Links";
            this.radGanttView1.GanttViewElement.LinkStartMember = "StartId";
            this.radGanttView1.GanttViewElement.LinkEndMember = "EndId";
            this.radGanttView1.GanttViewElement.LinkTypeMember = "LinkType";

            //GetData();

        }

        DataTable tasks;
        private DataSet GetData()
        {

            DataSet set = new DataSet();
            tasks = new DataTable("Tasks");
            tasks.Columns.Add("id", typeof(int));
            tasks.Columns.Add("idD", typeof(int));
            tasks.Columns.Add("Title", typeof(string));
            tasks.Columns.Add("FGNo", typeof(string));
            tasks.Columns.Add("FGName", typeof(string));
            tasks.Columns.Add("Qty", typeof(decimal));
            tasks.Columns.Add("UOM", typeof(string));
            tasks.Columns.Add("DueDate", typeof(DateTime));
            tasks.Columns.Add("Start", typeof(DateTime));
            tasks.Columns.Add("End", typeof(DateTime));
            tasks.Columns.Add("Status", typeof(string));


            int countid = 0;
            int Running = 0;
            int Loop = 0;
            int row = 0;
            int rcount = 0;
            DateTime start = DateTime.Now;
            DateTime endDate = DateTime.Now;
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                //var g = (from ix in db.sp_053_Ganttview(txtProductsOrder.Text) select ix).ToList();
                var g = db.sp_079_JobChart(txtProductsOrder.Text.Trim(), dtFrom.Value.Date
                    , dtTo.Value.Date.AddDays(1).AddMinutes(-1)).ToList();
                if (g.Count > 0)
                {
                    //tasks = dbClss.LINQToDataTable(g);
                    foreach (var r in g)
                    {
                        Loop = 0;
                        row += 1;
                        Running += 1;
                        countid += 1;
                        //tasks.Rows.Add(Running, 0, r.DocumentNo.Trim()
                        //    , ""
                        //    , r.FGName
                        //    , r.Qty, "UOM"
                        //    , r.DueDate
                        //    , r.StartingDate, r.EndingDate, "");
                        tasks.Rows.Add(r.id, 0, r.Title
                            , r.FGNo
                            , r.FGName
                            , r.Qty, r.UOM
                            , r.ReqDate
                            , r.Start, r.End, r.Status);
                    }

                    DataTable links = new DataTable("Links");
                    links.Columns.Add("StartId", typeof(int));
                    links.Columns.Add("EndId", typeof(int));
                    links.Columns.Add("LinkType", typeof(int));

                    set.Tables.Add(tasks);
                    set.Tables.Add(links);
                }
            }

            return set;

        }

        private void radGanttView1_ItemPaint(object sender, GanttViewItemPaintEventArgs e)
        {
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
        }

        private void cbbView_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            try
            {
                TimeRange tr = TimeRange.Day;
                if (cbbView.Text == "Week")
                    tr = TimeRange.Week;
                else if (cbbView.Text == "Month")
                    tr = TimeRange.Month;
                else if (cbbView.Text == "Year")
                    tr = TimeRange.Year;
                this.radGanttView1.GanttViewElement.GraphicalViewElement.TimelineRange = tr;
            }
            catch 
            {

            }
        }
    }
}
