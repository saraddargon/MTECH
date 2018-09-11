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

        //private int RowView = 50;
        //private int ColView = 10;
        DataTable dt = new DataTable();
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
        private void GETDTRow()
        {

            dt.Columns.Add(new DataColumn("CodeNo", typeof(string)));
            dt.Columns.Add(new DataColumn("ItemDescription", typeof(string)));
            dt.Columns.Add(new DataColumn("Order", typeof(decimal)));
            dt.Columns.Add(new DataColumn("BackOrder", typeof(decimal)));
            dt.Columns.Add(new DataColumn("StockQty", typeof(decimal)));
            dt.Columns.Add(new DataColumn("UnitBuy", typeof(string)));
            dt.Columns.Add(new DataColumn("PCSUnit", typeof(decimal)));
            dt.Columns.Add(new DataColumn("LeadTime", typeof(decimal)));
            dt.Columns.Add(new DataColumn("MaxStock", typeof(decimal)));
            dt.Columns.Add(new DataColumn("MinStock", typeof(decimal)));
            dt.Columns.Add(new DataColumn("VendorNo", typeof(string)));
            dt.Columns.Add(new DataColumn("VendorName", typeof(string)));



        }
        int crow = 99;
        private void Unit_Load(object sender, EventArgs e)
        {
            // radGridView1.ReadOnly = true;
            // radGridView1.AutoGenerateColumns = false;
            //GETDTRow();
            //DefaultItem();
            //radGridView1.ReadOnly = false;
            // DataLoad();
            GETDataTable();
            InitializeGanttView();
            radButton2_Click(null, null);
            crow = 0;
        }
        private void DefaultItem()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                //cboVendor.AutoCompleteMode = AutoCompleteMode.Append;
                //cboVendor.DisplayMember = "VendorName";
                //cboVendor.ValueMember = "VendorNo";
                //cboVendor.DataSource =(from ix in db.tb_Vendors.Where(s => s.Active == true) select new { ix.VendorNo,ix.VendorName}).ToList();
                //cboVendor.SelectedIndex = -1;

                try
                {

               

                    //GridViewMultiComboBoxColumn col = (GridViewMultiComboBoxColumn)radGridView1.Columns["CodeNo"];
                    //col.DataSource = (from ix in db.tb_Items.Where(s => s.Status.Equals("Active")) select new { ix.CodeNo, ix.ItemDescription }).ToList();
                    //col.DisplayMember = "CodeNo";
                    //col.ValueMember = "CodeNo";

                    //col.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDown;
                    //col.FilteringMode = GridViewFilteringMode.DisplayMember;

                    // col.AutoSizeMode = BestFitColumnMode.DisplayedDataCells;
                }
                catch { }

                //col.TextAlignment = ContentAlignment.MiddleCenter;
                //col.Name = "CodeNo";
                //this.radGridView1.Columns.Add(col);

                //this.radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

                //this.radGridView1.CellEditorInitialized += radGridView1_CellEditorInitialized;
            }
        }
       
        
    
        private void btnCancel_Click(object sender, EventArgs e)
        {
            radButton2_Click(null, null);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            return;
            //radGridView1.ReadOnly = false;
            //radGridView1.AllowAddNewRow = false;
            //radGridView1.Rows.AddNew();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            //radGridView1.ReadOnly = true;
            //btnView.Enabled = false;
            ////btnEdit.Enabled = true;
            //radGridView1.AllowAddNewRow = false;
            //DataLoad();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
           // radGridView1.ReadOnly = false;
           //// btnEdit.Enabled = false;
           // btnView.Enabled = true;
           // radGridView1.AllowAddNewRow = false;
            //DataLoad();
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

        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
               // radGridView1.Rows[e.RowIndex].Cells["dgvC"].Value = "T";
                //string TM1 = Convert.ToString(radGridView1.Rows[e.RowIndex].Cells["ModelName"].Value);
                ////string TM2 = Convert.ToString(radGridView1.Rows[e.RowIndex].Cells["MMM"].Value);
                //string Chk = Convert.ToString(radGridView1.Rows[e.RowIndex].Cells["dgvCodeTemp"].Value);
                //if (Chk.Equals("") && !TM1.Equals(""))
                //{

                //    if (!CheckDuplicate(TM1, Chk))
                //    {
                //        MessageBox.Show("ข้อมูล รายการซ้า");
                //        radGridView1.Rows[e.RowIndex].Cells["ModelName"].Value = "";
                //        //  radGridView1.Rows[e.RowIndex].Cells["MMM"].Value = "";
                //        //  radGridView1.Rows[e.RowIndex].Cells["UnitCode"].IsSelected = true;

                //    }
                //}


            }
            catch (Exception ex) { }
        }

        private void Unit_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // MessageBox.Show(e.KeyCode.ToString());
        }

        private void radGridView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // MessageBox.Show(e.KeyCode.ToString());

            //if (e.KeyData == (Keys.Control | Keys.S))
            //{
            //    if (MessageBox.Show("ต้องการบันทึก ?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //    {
            //        return;
            //        AddUnit();
            //        DataLoad();
            //    }
            //}
        }


        int row = -1;
        private void radGridView1_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            row = e.RowIndex;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            //dbClss.ExportGridCSV(radGridView1);
           // dbClss.ExportGridXlSX(radGridView1);
        }

    
        private void btnFilter1_Click(object sender, EventArgs e)
        {
          //  radGridView1.EnableFiltering = true;
        }

        private void btnUnfilter1_Click(object sender, EventArgs e)
        {
           // radGridView1.EnableFiltering = false;
        }

        private void radMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
          
        }

        private void radGridView1_Click(object sender, EventArgs e)
        {

        }

        private void chkActive_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {

        }

        private void radButton1_Click_1(object sender, EventArgs e)
        {
            //try
            //{


            //    this.Cursor = Cursors.WaitCursor;
            //    ReturnReceiveList sc = new ReturnReceiveList(txtInvoiceNo,"ChangeInvoice");
            //    this.Cursor = Cursors.Default;
            //    sc.ShowDialog();
            //    GC.Collect();
            //    GC.WaitForPendingFinalizers();

            //    ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            //    ClassLib.Memory.Heap();

            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError("ReturnReceive", ex.Message + " : radButton1_Click_1", this.Name); }
            //finally { this.Cursor = Cursors.Default; }
        }

        private void radGridView1_CellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            //if (e.CellElement.ColumnInfo.Name == "ModelName")
            //{
            //    if (e.CellElement.RowInfo.Cells["ModelName"].Value != null)
            //    {
            //        if (!e.CellElement.RowInfo.Cells["ModelName"].Value.Equals(""))
            //        {
            //            e.CellElement.DrawFill = true;
            //            // e.CellElement.ForeColor = Color.Blue;
            //            e.CellElement.NumberOfColors = 1;
            //            e.CellElement.BackColor = Color.WhiteSmoke;
            //        }

            //    }
            //}
        }

        private void txtModelName_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void radPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cboModelName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (crow == 0)
            //    DataLoad();
        }

        private void GETDataTable()
        {
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("ParentId", typeof(int));
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("FGName", typeof(string));
            dt.Columns.Add("Qty", typeof(decimal));
            dt.Columns.Add("DueDate", typeof(DateTime));
            dt.Columns.Add("Start", typeof(DateTime));
            dt.Columns.Add("End", typeof(DateTime));
            dt.Columns.Add("Deadline", typeof(DateTime));
        }
        private void radButton2_Click(object sender, EventArgs e)
        {
            try
            {
                this.radGanttView1.DataSource = null;
                this.radGanttView1.DataSource = GetData();
                this.radGanttView1.GanttViewElement.GraphicalViewElement.TimelineStart = DateTime.Now.AddDays(-1);
                this.radGanttView1.GanttViewElement.GraphicalViewElement.TimelineEnd = DateTime.Now.AddDays(180);
                this.radGanttView1.GanttViewElement.GraphicalViewElement.TimelineRange = TimeRange.Month;
                this.radGanttView1.EnableCustomPainting = true;
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
        public void InitializeGanttView()
        {
            //    this.radGanttView1.GanttViewElement.Columns.Add(new GanttViewTextViewColumn("Id"));
            //    this.radGanttView1.GanttViewElement.Columns.Add(new GanttViewTextViewColumn("ParentId"));
            //    this.radGanttView1.GanttViewElement.Columns.Add(new GanttViewTextViewColumn("No"));
            //    this.radGanttView1.GanttViewElement.Columns.Add(new GanttViewTextViewColumn("Title"));
            //    this.radGanttView1.GanttViewElement.Columns.Add(new GanttViewTextViewColumn("Start"));
            //    this.radGanttView1.GanttViewElement.Columns.Add(new GanttViewTextViewColumn("End"));

            //    this.radGanttView1.GanttViewElement.Columns[0].Visible = false;
            //    this.radGanttView1.GanttViewElement.Columns[1].Visible = false;
            //    this.radGanttView1.GanttViewElement.Columns[2].Width = 45;
            //    this.radGanttView1.GanttViewElement.Columns[3].Width = 180;
            //    this.radGanttView1.GanttViewElement.Columns[4].Width = 110;
            //    this.radGanttView1.GanttViewElement.Columns[5].Width = 110;

            this.radGanttView1.GanttViewElement.TaskDataMember = "Tasks";
            this.radGanttView1.GanttViewElement.ChildMember = "Id";
            this.radGanttView1.GanttViewElement.ParentMember = "ParentId";            
            this.radGanttView1.GanttViewElement.TitleMember = "Title";
            this.radGanttView1.GanttViewElement.StartMember = "Start";
            this.radGanttView1.GanttViewElement.EndMember = "End";

            this.radGanttView1.GanttViewElement.ProgressMember = "Progress";
            this.radGanttView1.GanttViewElement.LinkDataMember = "Links";
            this.radGanttView1.GanttViewElement.LinkStartMember = "StartId";
            this.radGanttView1.GanttViewElement.LinkEndMember = "EndId";
            this.radGanttView1.GanttViewElement.LinkTypeMember = "LinkType";

            GetData();

        }
        private DataSet GetData()
        {

            DataSet set = new DataSet();
            DataTable tasks = new DataTable("Tasks");
            tasks.Columns.Add("Id", typeof(int));
            tasks.Columns.Add("ParentId", typeof(int));
            tasks.Columns.Add("Title", typeof(string));
            tasks.Columns.Add("FGName", typeof(string));
            tasks.Columns.Add("Qty", typeof(decimal));
            tasks.Columns.Add("DueDate", typeof(DateTime));
            tasks.Columns.Add("Start", typeof(DateTime));
            tasks.Columns.Add("End", typeof(DateTime));
            tasks.Columns.Add("Deadline", typeof(DateTime));
            tasks.Columns.Add("Days", typeof(decimal));


            int countid = 0;
            int Running = 0;
            int Loop = 0;
            int row = 0;
            int rcount = 0;
            DateTime start = DateTime.Now;
            DateTime endDate = DateTime.Now;
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                var g = (from ix in db.sp_053_Ganttview(txtProductsOrder.Text) select ix).ToList();

                if (g.Count > 0)
                {
                    foreach (var r in g)
                    {

                        Loop = 0;
                        row += 1;
                        Running += 1;
                        countid += 1;
                        tasks.Rows.Add(Running, 0, r.DocumentNo.Trim()
                             ,r.FGName
                             ,r.Qty
                             ,r.DueDate
                            , r.StartingDate, r.EndingDate, DBNull.Value, r.Days);
                        //var x = (from io in db.sp_053_Ganttview(txtProductsOrder.Text) select io).ToList();
                        //start = DateTime.Now;
                        //endDate = DateTime.Now;

                        //if (x.Count > 0)
                        //{
                        //    foreach (var rd in x)
                        //    {
                        //        rcount += 1;
                        //        if (Loop == 0)
                        //        {
                        //            start = Convert.ToDateTime(r.StartingDate, new CultureInfo("en-US"));
                        //            endDate = start.AddDays(Convert.ToDouble(5));
                        //            Loop += 1;
                        //        }
                        //        else
                        //        {
                        //            start = endDate;
                        //            endDate = start.AddDays(Convert.ToDouble(5));
                        //        }

                        //        //Console.WriteLine("Detail ->" + rd.LineName.Trim() + ":" + endDate.ToString());
                        //        Running += 1;
                        //        tasks.Rows.Add(Running, countid, rd.DocumentNo.Trim(), start, endDate, endDate, rd.Days);

                        //    }
                        //}
                    }

                    // Console.WriteLine("END...");
                    DataTable links = new DataTable("Links");
                    links.Columns.Add("StartId", typeof(int));
                    links.Columns.Add("EndId", typeof(int));
                    links.Columns.Add("LinkType", typeof(int));
                    if (rcount >= 2)
                        links.Rows.Add(2, 3, 1);
                    if (rcount >= 3)
                        links.Rows.Add(3, 4, 1);
                    if (rcount >= 4)
                        links.Rows.Add(4, 5, 1);
                    if (rcount >= 5)
                        links.Rows.Add(5, 6, 1);
                    if (rcount >= 6)
                        links.Rows.Add(6, 7, 1);
                    if (rcount == 7)
                        links.Rows.Add(7, 8, 1);
                    if (rcount >= 8)
                        links.Rows.Add(8, 9, 1);
                    if (rcount >= 9)
                        links.Rows.Add(9, 10, 1);
                    if (rcount >= 10)
                        links.Rows.Add(10, 11, 1);
                    //links.Rows.Add(13, 14, 1);
                    //links.Rows.Add(14, 15, 1);
                    //links.Rows.Add(15, 16, 1);

                    set.Tables.Add(tasks);
                    set.Tables.Add(links);

                }
            }

            return set;

        }

        private void radGanttView1_ItemPaint(object sender, GanttViewItemPaintEventArgs e)
        {
            GanttViewDataItem item = e.Element.Data;
            DataRowView row = item.DataBoundItem as DataRowView;

            object deadlineValue = row["Deadline"];

            if (deadlineValue.Equals(DBNull.Value))
            {
                return;
            }

            DateTime deadline = (DateTime)deadlineValue;
            bool overdue = deadline < item.End;

            RectangleF deadlineRect = e.Element.GraphicalViewElement.GetDrawRectangle(item, deadline);
            RectangleF overdueFlagRect = e.Element.GraphicalViewElement.GetDrawRectangle(item, item.End.AddHours(2));

            SmoothingMode mode = e.Graphics.SmoothingMode;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            this.DrawOverdueFlag(e.Graphics, overdueFlagRect, overdue);
            this.DrawDeadline(e.Graphics, deadlineRect);
            e.Graphics.SmoothingMode = mode;
        }
        private void DrawDeadline(Graphics graphics, RectangleF rect)
        {
            RectangleF topBallRect = new RectangleF(rect.X - 2, rect.Y + 1, 4f, 4f);
            RectangleF bottomBallRect = new RectangleF(rect.X - 2, rect.Bottom - 2, 4f, 4f);

            graphics.DrawLine(Pens.Red, new PointF(rect.X, rect.Y + 2), new PointF(rect.X, rect.Bottom));
            graphics.FillEllipse(Brushes.Red, topBallRect);
            graphics.FillEllipse(Brushes.Red, bottomBallRect);
        }

        private void DrawOverdueFlag(Graphics graphics, RectangleF rect, bool overdue)
        {
            PointF[] points = new PointF[7];
            points[0] = new PointF(rect.X, rect.Y + 2);
            points[1] = new PointF(rect.X + 20, rect.Y + 2);
            points[2] = new PointF(rect.X + 15, rect.Y + 2 + rect.Height / 4f);
            points[3] = new PointF(rect.X + 20, rect.Y + 2 + rect.Height / 2f);
            points[4] = new PointF(rect.X + 2, rect.Y + 2 + rect.Height / 2f);
            points[5] = new PointF(rect.X + 2, rect.Bottom);
            points[6] = new PointF(rect.X, rect.Bottom);


            GraphicsPath flagPath = new GraphicsPath();
            flagPath.AddClosedCurve(points, 0f);

            if (overdue)
            {
                graphics.FillPath(Brushes.Red, flagPath);
            }
            else
            {
                graphics.FillPath(Brushes.Green, flagPath);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            //dbClss.ExportGridXlSX(radGanttView1);

            //XlsxFormatProvider formatProvider = new XlsxFormatProvider();

            //using (Stream output = new FileStream(fileName, FileMode.Create))
            //{
            //    formatProvider.Export(this.radSpreadsheet.Workbook, output);
            //}


            //var printDialog = new PrintDialog();
            //if (printDialog.ShowDialog() == true)
            //{
            //    var exportImages = Enumerable.Empty<BitmapSource>();
            //    var printingSettings = new ImageExportSettings(new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight), true, GanttArea.AllAreas);
            //    using (var export = ganttView.ExportingService.BeginExporting(printingSettings))
            //    {
            //        exportImages = export.ImageInfos.ToList().Select(info => info.Export());
            //    }

            //    var paginator = new GanttPaginator(exportImages);
            //    printDialog.PrintDocument(paginator, "Print demo");
            //}
        }
    }
}
