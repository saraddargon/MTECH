﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;
using System.IO;
using OfficeOpenXml;

namespace StockControl
{
    public partial class PrintPR : Telerik.WinControls.UI.RadRibbonForm
    {
        public PrintPR(string PR1xx, string PR2xx, string Typexx)
        {
            InitializeComponent();
            PR1 = PR1xx;
            PR2 = PR2xx;
            txtPRNo1.Text = PR1xx;
            txtPRNo2.Text = PR2xx;
            Type = Typexx;
        }
        Telerik.WinControls.UI.RadTextBox CodeNo_tt = new Telerik.WinControls.UI.RadTextBox();
        int screen = 0;
        public PrintPR(Telerik.WinControls.UI.RadTextBox CodeNox)
        {
            InitializeComponent();
            CodeNo_tt = CodeNox;
            screen = 1;
        }
        public PrintPR()
        {
            InitializeComponent();
        }

        string PR1 = "";
        string PR2 = "";
        string Type = "";
        //private int RowView = 50;
        //private int ColView = 10;
        //DataTable dt = new DataTable();
        private void radMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }
        private void GETDTRow()
        {
            //dt.Columns.Add(new DataColumn("UnitCode", typeof(string)));
            //dt.Columns.Add(new DataColumn("UnitDetail", typeof(string)));
            //dt.Columns.Add(new DataColumn("UnitActive", typeof(bool)));
        }
        private void Unit_Load(object sender, EventArgs e)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                cbDate.Visible = false;
                dtDate1.Visible = false;
                dtDate2.Visible = false;
                lblToDate.Visible = false;

                if (Type.Equals("PR"))
                {
                    lblName.Text = "เลขที่ใบสั่งซื้อ";
                }
                else if (Type.Equals("Receive"))
                    lblName.Text = "เลขที่รับสินค้า";
                else if (Type.Equals("Shipping"))
                    lblName.Text = "เลขที่เบิกสินค้า";

                else if (Type.Equals("AdjustStock"))
                    lblName.Text = "เลขที่ปรับปรุงสินค้า";
                else if (Type.Equals("JobCard"))
                    lblName.Text = "เลขที่ใบงานการผลิต";
                else if (Type.Equals("PO"))
                    lblName.Text = "เลขที่ใบสั่งซื้อ";
                else if (Type.Equals("ReportStockMovement"))
                {
                    lblName.Text = "เลขที่รหัสทูล";

                    cbDate.Visible = true;
                    dtDate1.Visible = true;
                    dtDate2.Visible = true;
                    lblToDate.Visible = true;
                    lblLocation.Visible = true;
                    ddlLocation.Visible = true;

                    //ddlLocation.DisplayMember = "Location";
                    //ddlLocation.ValueMember = "Location";
                    //ddlLocation.DataSource = db.tb_Locations.Where(s => s.Active == true && s.Status == "Completed").ToList();
                    //ddlLocation.Text = "";
                    ddlLocation.DataSource = null;
                    ddlLocation.DisplayMember = "Code";
                    ddlLocation.ValueMember = "Code";
                    // ddlLocation.DataSource = db.tb_Locations.Where(s => s.Active == true && s.Status == "Completed").ToList();
                    var g = (from ix in db.mh_Locations select ix).Where(s => s.Active == true && s.Active == true).ToList();

                    List<string> a = new List<string>();
                    if (g.Count > 0)
                    {
                        foreach (var gg in g)
                            a.Add(gg.Code);
                    }
                    a.Add("");
                    ddlLocation.DataSource = a;
                    ddlLocation.Text = "";




                    dtDate1.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    dtDate2.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                }
                else if (Type.Equals("ReportStockCard"))
                {
                    lblName.Text = "เลขที่รหัสทูล";

                    txtPRNo1.ReadOnly = true;
                    txtPRNo2.ReadOnly = true;

                    cbDate.Visible = true;
                    dtDate1.Visible = true;
                    dtDate2.Visible = true;
                    lblToDate.Visible = true;
                    lblLocation.Visible = true;
                    ddlLocation.Visible = true;

                    ddlLocation.DataSource = null;
                    ddlLocation.DisplayMember = "Code";
                    ddlLocation.ValueMember = "Code";
                    var g = (from ix in db.mh_Locations select ix).Where(s => s.Active == true && s.Active == true).ToList();

                    List<string> a = new List<string>();
                    if (g.Count > 0)
                    {
                        foreach (var gg in g)
                            a.Add(gg.Code);
                    }
                    //a.Add("");
                    ddlLocation.DataSource = a;
                    ddlLocation.Text = "";

                    //ddlLocation.SelectedIndex = 1;

                    dtDate1.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    dtDate2.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                }
                else if (Type.Equals("ReceiveMonth"))
                {
                    lblName.Text = "เลขที่รับ";

                    cbDate.Visible = true;
                    dtDate1.Visible = true;
                    dtDate2.Visible = true;
                    lblToDate.Visible = true;
                    ddlMonth.Visible = true;
                    ddlYear.Visible = true;
                    cbYYYYMM.Visible = true;
                    radRibbonBarGroup2.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
                    btnExportExcel.Visibility = Telerik.WinControls.ElementVisibility.Visible;


                    ddlYear.DataSource = null;
                    ddlYear.DisplayMember = "YYYY";
                    ddlYear.ValueMember = "YYYY";

                    var g = (from ix in db.sp_Select_Year() select ix).ToList();
                    ddlYear.DataSource = g;
                    ddlYear.SelectedIndex = 0;

                    ddlMonth.Text = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("MM");
                    ddlYear.Text = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("yyyy");

                    dtDate1.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    dtDate2.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                }
                else if (Type.Equals("Claim"))
                    lblName.Text = "เลขที่เคลม";
                else if (Type.Equals("SellFG"))
                    lblName.Text = "เลขที่ใบขาย";
                else if (Type == "ShippingAVG")
                {
                    cbYYYYMM.Text = "ปีและเดือนที่เบิก";
                    lblName.Text = "รหัสทูล";

                    ddlYear.DataSource = null;
                    ddlYear.DisplayMember = "YYYY";
                    ddlYear.ValueMember = "YYYY";

                    var g = (from ix in db.sp_Select_Year() select ix).ToList();
                    ddlYear.DataSource = g;
                    ddlYear.SelectedIndex = 0;

                    ddlMonth.Text = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("MM");
                    ddlYear.Text = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("yyyy");
                    cbYYYYMM.Checked = true;
                    cbYYYYMM.ReadOnly = true;
                    cbYYYYMM.Visible = true;
                    ddlMonth.Visible = true;
                    ddlYear.Visible = true;

                }
                else if (Type.Equals("ShippingToDay"))
                {
                    lblName.Text = "เลขที่เบิกสินค้า";
                    cbDate.Visible = true;
                    dtDate1.Visible = true;
                    dtDate2.Visible = true;
                    lblToDate.Visible = true;

                    dtDate1.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    dtDate2.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));

                }
                else if (Type == "ReportProductionOrder")
                {
                    lblName.Text = "เลขที่เอกสาร";
                }
                else if (Type == "ReportAccidentSlip")
                {
                    lblName.Text = "เลขที่เอกสาร";
                }
                else if (Type == "PackingList")
                {
                    txtPRNo1.Visible = false;
                    txtPRNo2.Visible = false;
                    cbDate.Visible = false;
                    lblName.Visible = false;
                    cbDate.Visible = false;
                    radLabel37.Visible = false;
                    //lblName.Text = "เลขที่เบิกสินค้า";
                    dtDate1.Visible = true;
                    dtDate2.Visible = true;
                    lblToDate.Visible = true;
                    dtDate1.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    dtDate2.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    panel2.Location = new Point(panel2.Location.X, 30);
                }
                else if (Type == "ListPurchaseAndPR")
                {
                    lblName.Visible = false;
                    txtPRNo1.Visible = false;
                    txtPRNo2.Visible = false;
                    radLabel37.Visible = false;

                    cbDate.Checked = true;
                    cbDate.ReadOnly = true;
                    cbDate.Visible = true;
                    dtDate1.Visible = true;
                    dtDate2.Visible = true;
                    lblToDate.Visible = true;

                    dtDate1.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    dtDate2.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));

                    this.Size = new Size(400, 500);
                }
            }
        }

        private void DataLoad()
        {
            //dt.Rows.Clear();
            try
            {

                this.Cursor = Cursors.WaitCursor;
                //using (DataClasses1DataContext db = new DataClasses1DataContext())
                //{



                //}
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;


            //    radGridView1.DataSource = dt;
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {

        }

        private void btnView_Click(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {

        }

        private void Unit_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // MessageBox.Show(e.KeyCode.ToString());
        }

        private void radGridView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            DataLoad();

        }

        int row = -1;
        private void radGridView1_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            row = e.RowIndex;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (Type.Equals("ReceiveMonth"))
            {
                Export_ReceiveMonth();
            }
            ////  dbClss.ExportGridCSV(radGridView1);
            //dbClss.ExportGridXlSX(radGridView1);
        }
        private void Export_ReceiveMonth()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    string dt11 = "";
                    string dt22 = "";
                    string YYYY = "";
                    string MM = "";

                    if (cbDate.Checked)
                    {
                        dt11 = dtDate1.Value.ToString("yyyyMMdd");
                        dt22 = dtDate2.Value.ToString("yyyyMMdd");

                    }
                    if (cbYYYYMM.Checked)
                    {
                        if ((ddlYear.Text != "" && ddlMonth.Text == "")
                            || (ddlYear.Text == "" && ddlMonth.Text != ""))
                        {
                            //err += "- “เลขที่ PR:” เป็นค่าว่าง \n";
                            MessageBox.Show("- “กรณีที่ระบุ ปีหรือเดือน ต้องห้ามเป็นค่าว่าง”");
                            return;
                        }
                        YYYY = ddlYear.Text;
                        MM = ddlMonth.Text;
                    }
                    var Data = db.sp_R016_Receive_Month(txtPRNo1.Text, txtPRNo2.Text, dt11, dt22, YYYY + MM, Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"))).ToList();
                    if (Data != null)
                    {
                        if (Data.Count > 0)
                        {
                            //เขียนไฟล์ Excel
                            string sourcefile = "";
                            string descfile = "";
                            SaveFileDialog dialog = new SaveFileDialog();
                            dialog.Filter = "xlsx File (*.xlsx)|*.xlsx";
                            if (dialog.ShowDialog() == DialogResult.OK)
                            {
                                this.Cursor = Cursors.WaitCursor;

                                sourcefile = System.AppDomain.CurrentDomain.BaseDirectory + "\\Report\\Excel_ReceiveMonth.xlsx";
                                descfile = dialog.FileName;
                                System.IO.File.Copy(sourcefile, descfile, true);

                                File.Copy(sourcefile, descfile, true);

                                using (var package = new ExcelPackage(new FileInfo(descfile)))
                                {
                                    var ws_rm = package.Workbook.Worksheets[1];
                                    ws_rm.Cells[1, 1].Value = "ใบบันทึกรับสินค้าประจำวัน ";
                                    if (cbYYYYMM.Checked)
                                        ws_rm.Cells[2, 1].Value = "ประจำเดือน " + dbClss.Month_(ddlMonth.Text) + " " + ddlYear.Text; //ประจำเดือน มกราคม 2018

                                    //" + ConnectDB.ConnectDB.Around + "/" + ConnectDB.ConnectDB.YYYY + " (" + String.Format("{0:dd MMM yy}", ConnectDB.ConnectDB.StartDate) + " - " + String.Format("{0:dd MMM yy}", ConnectDB.ConnectDB.EndDate) + ")";
                                    ws_rm.Cells[5, 1].LoadFromCollection(Data, false);

                                    package.Save();
                                }

                                GC.GetTotalMemory(false);
                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                                GC.Collect();
                                GC.GetTotalMemory(false);

                                MessageBox.Show("Export Data to complete.");
                                //dbClss.Info("Export Data to complete.");
                                //
                                System.Diagnostics.Process.Start(descfile);
                            }
                        }
                    }
                }


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }


        private void btnFilter1_Click(object sender, EventArgs e)
        {
            //radGridView1.EnableFiltering = true;
        }

        private void btnUnfilter1_Click(object sender, EventArgs e)
        {
            //radGridView1.EnableFiltering = false;
        }

        private void radMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radButtonElement1_Click(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void radGridView1_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                if (screen.Equals(1))
                {
                    CodeNo_tt.Text = Convert.ToString(e.Row.Cells["TempNo"].Value);
                    this.Close();
                }
                else
                {
                    CreatePR a = new CreatePR(Convert.ToString(e.Row.Cells["TempNo"].Value));
                    a.ShowDialog();
                    this.Close();
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        DataTable dt_Kanban = new DataTable();

        private void Set_dt_Print()
        {


        }

        private void btn_Print_Barcode_Click(object sender, EventArgs e)
        {
            try
            {
                dt_Kanban.Rows.Clear();

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var g = (from ix in db.tb_Items select ix).Where(a => a.CodeNo == txtPRNo2.Text).ToList();
                    if (g.Count() > 0)
                    {
                        foreach (var gg in g)
                        {
                            dt_Kanban.Rows.Add(gg.CodeNo, gg.ItemNo, gg.ItemDescription, gg.ShelfNo, gg.Leadtime, gg.VendorItemName, gg.GroupCode, gg.Toollife, gg.MaximumStock, gg.MinimumStock, gg.ReOrderPoint, gg.BarCode);
                        }
                        //DataTable DT =  StockControl.dbClss.LINQToDataTable(g);
                        //Reportx1 po = new Reportx1("Report_PurchaseRequest_Content1.rpt", DT, "FromDT");
                        //po.Show();

                        Report.Reportx1 op = new Report.Reportx1("001_Kanban_Part.rpt", dt_Kanban, "FromDL");
                        op.Show();
                    }
                    else
                        MessageBox.Show("not found.");
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btn_PrintPR_Click(object sender, EventArgs e)
        {
            try
            {
                //dt_ShelfTag.Rows.Clear();
                string PRNo1 = txtPRNo1.Text;
                string PRNo2 = txtPRNo2.Text;


                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    if (Type.Equals("PR"))
                    {

                        var g = (from ix in db.sp_R005_ReportPR(PRNo1, PRNo2, Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"))) select ix).ToList();
                        if (g.Count() > 0)
                        {
                            Report.Reportx1.Value = new string[2];
                            Report.Reportx1.Value[0] = PRNo1;
                            Report.Reportx1.Value[1] = PRNo2;
                            Report.Reportx1.WReport = "PR";
                            Report.Reportx1 op = new Report.Reportx1("PR.rpt");
                            op.Show();
                        }
                        else
                            MessageBox.Show("not found.");
                    }
                    if (Type.Equals("PO"))
                    {

                        var g = (from ix in db.sp_R011_ReportPO(PRNo1, PRNo2, Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"))) select ix).ToList();
                        if (g.Count() > 0)
                        {
                            Report.Reportx1.Value = new string[2];
                            Report.Reportx1.Value[0] = PRNo1;
                            Report.Reportx1.Value[1] = PRNo2;
                            Report.Reportx1.WReport = "PO";
                            Report.Reportx1 op = new Report.Reportx1("PO.rpt");
                            op.Show();
                        }
                        else
                            MessageBox.Show("not found.");
                    }
                    else if (Type.Equals("Receive"))
                    {
                        //var g = (from ix in db.sp_R006_ReportReceive(PRNo1, PRNo2, Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"))) select ix).ToList();
                        //if (g.Count() > 0)
                        //{
                        Report.Reportx1.Value = new string[2];
                        Report.Reportx1.Value[0] = PRNo1;
                        Report.Reportx1.Value[1] = PRNo2;
                        Report.Reportx1.WReport = "ReportReceive2";
                        Report.Reportx1 op = new Report.Reportx1("ReportReceive2.rpt");
                        op.Show();
                        //}
                        //else
                        //    MessageBox.Show("not found.");
                    }
                    else if (Type.Equals("Shipping"))
                    {
                        //var g = (from ix in db.sp_R007_ReportShipping(PRNo1, PRNo2, Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"))) select ix).ToList();
                        //if (g.Count() > 0)
                        //{
                        Report.Reportx1.Value = new string[2];
                        Report.Reportx1.Value[0] = PRNo1;
                        Report.Reportx1.Value[1] = PRNo2;
                        Report.Reportx1.WReport = "ReportShipping2";
                        //Report.Reportx1 op = new Report.Reportx1("ReportShipping2.rpt");
                        Report.Reportx1 op = new Report.Reportx1("ReportShipping2.rpt");

                        op.Show();
                        //}
                        //else
                        //    MessageBox.Show("not found.");
                    }
                    else if (Type.Equals("ShippingToDay"))
                    {
                        string dt1 = "";
                        string dt2 = "";

                        if (cbDate.Checked)
                        {
                            // Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"))
                            dt1 = Convert.ToDateTime((dtDate1.Value), new CultureInfo("en-US")).ToString("yyyyMMdd");
                            dt2 = Convert.ToDateTime((dtDate2.Value), new CultureInfo("en-US")).ToString("yyyyMMdd");
                        }

                        var g = (from ix in db.sp_R021_ReportShipping_Today(PRNo1, PRNo2, dt1, dt2, Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"))) select ix).ToList();
                        if (g.Count() > 0)
                        {
                            Report.Reportx1.Value = new string[4];
                            Report.Reportx1.Value[0] = PRNo1;
                            Report.Reportx1.Value[1] = PRNo2;
                            Report.Reportx1.Value[2] = dt1;
                            Report.Reportx1.Value[3] = dt2;
                            Report.Reportx1.WReport = "ReportShippingToDay";
                            //Report.Reportx1 op = new Report.Reportx1("ReportShipping2.rpt");
                            Report.Reportx1 op = new Report.Reportx1("ReportShippingToDay.rpt");

                            op.Show();
                        }
                        else
                            MessageBox.Show("not found.");
                    }
                    else if (Type.Equals("AdjustStock"))
                    {
                        //var g = (from ix in db.sp_R008_ReportAdjustStock(PRNo1, PRNo2, Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"))) select ix).ToList();
                        //if (g.Count() > 0)
                        //{
                        Report.Reportx1.Value = new string[2];
                        Report.Reportx1.Value[0] = PRNo1;
                        Report.Reportx1.Value[1] = PRNo2;
                        Report.Reportx1.WReport = "ReportAdjustStock";
                        Report.Reportx1 op = new Report.Reportx1("ReportAdjustStock.rpt");
                        op.Show();
                        //}
                        //else
                        //    MessageBox.Show("not found.");
                    }
                    else if (Type.Equals("JobCard"))
                    {
                        //var g = (from ix in db.sp_R010_Report_JobCard(PRNo1, PRNo2, Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"))) select ix).ToList();
                        //if (g.Count() > 0)
                        //{
                        Report.Reportx1.Value = new string[2];
                        Report.Reportx1.Value[0] = PRNo1;
                        Report.Reportx1.Value[1] = PRNo2;
                        Report.Reportx1.WReport = "JobCard";
                        Report.Reportx1 op = new Report.Reportx1("JobCard.rpt");
                        op.Show();
                        //}
                        //else
                        //    MessageBox.Show("not found.");
                    }
                    else if (Type.Equals("ReportStockMovement"))
                    {
                        string dt1 = "";
                        string dt2 = "";

                        if (cbDate.Checked)
                        {
                            // Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"))
                            dt1 = Convert.ToDateTime((dtDate1.Value), new CultureInfo("en-US")).ToString("yyyyMMdd");
                            dt2 = Convert.ToDateTime((dtDate2.Value), new CultureInfo("en-US")).ToString("yyyyMMdd");
                        }
                        //var g = (from ix in db.sp_R009_Stock_Movement(PRNo1, PRNo2,dt1,dt2,ddlLocation.Text, Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"))) select ix).ToList();
                        //if (g.Count() > 0)
                        //{
                        Report.Reportx1.Value = new string[5];
                        Report.Reportx1.Value[0] = PRNo1;
                        Report.Reportx1.Value[1] = PRNo2;
                        Report.Reportx1.Value[2] = dt1;
                        Report.Reportx1.Value[3] = dt2;
                        Report.Reportx1.Value[4] = ddlLocation.Text;
                        Report.Reportx1.WReport = "ReportStockMovement";
                        Report.Reportx1 op = new Report.Reportx1("ReportStockMovement.rpt");
                        op.Show();
                        //}
                        //else
                        //    MessageBox.Show("not found.");
                    }
                    else if (Type.Equals("ReportStockCard"))
                    {
                        string dt1 = "";
                        string dt2 = "";

                        if (cbDate.Checked)
                        {
                            // Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"))
                            dt1 = Convert.ToDateTime((dtDate1.Value), new CultureInfo("en-US")).ToString("yyyyMMdd");
                            dt2 = Convert.ToDateTime((dtDate2.Value), new CultureInfo("en-US")).ToString("yyyyMMdd");
                        }

                        ddlLocation.Text = "Warehouse";

                        //clear user
                        var del = db.mh_StockCards.Where(x => x.USERID == ClassLib.Classlib.User).ToList();
                        db.mh_StockCards.DeleteAllOnSubmit(del);
                        db.SubmitChanges();

                        var stBefore = db.sp_getStockBefore(PR1, ddlLocation.Text, dtDate1.Value.Date);
                        var rem = stBefore.ToDecimal();

                        var tool = db.mh_Items.Where(x => x.InternalNo == PR1).FirstOrDefault();
                        if (tool == null)
                        {
                            baseClass.Error("ไม่พบทูล");
                            return;
                        }
                        var tooluom = db.mh_ItemUOMs.Where(x => x.UOMCode == tool.BaseUOM && x.ItemNo == tool.InternalNo).FirstOrDefault();
                        var pcsunit = 1.00m;
                        if (tooluom != null) pcsunit = tooluom.QuantityPer;
                        int rNo = 1;
                        var scard = new mh_StockCard
                        {
                            ItemNo = PR1,
                            ItemName = tool.InternalName,
                            dFrom = dtDate1.Value.Date,
                            dTo = dtDate2.Value.Date,
                            DocumentRef = "ยอดยกมา",
                            Location = ddlLocation.Text,
                            No = rNo,
                            UOM = tool.BaseUOM,
                            USERID = ClassLib.Classlib.User,
                            Shelf = tool.ShelfNo,
                            SubCust = "",
                            PCSUnit = pcsunit,
                            Date = dtDate1.Value.Date,
                            StartQty = rem,
                            InQty = 0,
                            OutQty = 0,
                            RemainQty = rem,
                        };
                        db.mh_StockCards.InsertOnSubmit(scard);
                        db.SubmitChanges();

                        DateTime? dFrom = (cbDate.Checked) ? (DateTime?)dtDate1.Value.Date : null;
                        DateTime? dTo = (cbDate.Checked) ? (DateTime?)dtDate2.Value.Date.AddDays(1).AddMinutes(-1) : null;

                        var st = db.tb_Stocks.Where(x => x.CodeNo == PRNo1
                            && (dFrom == null || (x.CreateDate >= dFrom && x.CreateDate <= dTo))
                            && x.Location == ddlLocation.Text
                            ).OrderBy(x => x.CreateDate).ToList();
                        foreach (var s in st)
                        {
                            string refNo = s.DocNo;
                            bool findCustFromJob = false;
                            string custNo = "";
                            string custName = "";
                            rNo++;
                            string sName = "";
                            if (s.DocNo.ToSt().Length > 2)
                            {
                                if (s.DocNo.ToSt().Substring(0, 2) == "RC")
                                {
                                    //receive
                                    var po = db.mh_PurchaseOrders.Where(x => x.PONo == s.RefNo).FirstOrDefault();
                                    if (po != null)
                                    {
                                        sName = po.VendorName.ToSt();
                                    }

                                    //Recieve from invoice
                                    var rc = db.tb_ReceiveHs.Where(x => x.RCNo == s.DocNo).FirstOrDefault();
                                    if (rc != null) refNo = rc.InvoiceNo;

                                }
                                else if (s.DocNo.ToSt().Substring(0, 2) == "PK") //recive job
                                {
                                    refNo = s.RefNo;
                                    findCustFromJob = true;
                                }
                                else if ((s.DocNo.ToSt().Substring(0, 2) == "SH")
                                    && s.DocNo.ToSt().Substring(2, 1).ToInt() == 0) //Ship not normal
                                {
                                    refNo = s.RefJobCode;
                                    findCustFromJob = true;
                                    if (s.DocNo.ToSt().Substring(0, 4) == "SHAS")
                                    {
                                        var acc = db.mh_Accident_SlipHs.Where(x => x.DocNo == s.RefJobCode).FirstOrDefault();
                                        if(acc != null)
                                            refNo = acc.JobCard;
                                    }
                                }
                                else if (s.DocNo.ToSt().Substring(0, 2) == "RT") //Return RM
                                {
                                    refNo = s.RefJobCode;
                                    findCustFromJob = true;
                                }
                                else if (s.DocNo.ToSt().Substring(0, 2) == "SE") //Shipment Shipping
                                {
                                    refNo = s.DocNo;
                                    var sm = db.mh_Shipments.Where(x => x.SSNo == s.RefJobCode).FirstOrDefault();
                                    if (sm != null)
                                    {
                                        refNo = sm.SSNo;
                                        sName = sm.CustomerName;
                                        //ถ้า Shipment ออก Invoice แล้ว ให้เอา
                                        var inv = db.mh_InvoiceDTs.Where(x => x.RefId == sm.id && x.Active)
                                            .Join(db.mh_InvoiceHDs.Where(x => x.Active)
                                            , dt => dt.IVNo
                                            , hd => hd.IVNo
                                            , (dt, hd) => new { hd, dt }).FirstOrDefault();
                                        if(inv != null)
                                        {
                                            refNo = inv.hd.IVNo;
                                            sName = inv.hd.CustomerName;
                                            
                                        }
                                    }
                                }
                            }

                            if (findCustFromJob)
                            {
                                var pd = db.mh_ProductionOrders.Where(x => x.JobNo == refNo).FirstOrDefault();
                                if(pd != null)
                                {
                                    var so = db.mh_SaleOrderDTs.Where(x => x.id == pd.RefDocId)
                                        .Join(db.mh_SaleOrders
                                        , dt => dt.SONo
                                        , hd => hd.SONo
                                        , (dt, hd) => new { hd, dt }).ToList();
                                    if(so.Count > 0)
                                    {
                                        sName = so.FirstOrDefault().hd.CustomerName;
                                    }
                                }
                            }

                            var startQ = rem;
                            var inQ = 0.00m;
                            var outQ = 0.00m;
                            if (s.Type_in_out == "In")
                                inQ = s.QTY.ToDecimal();
                            else
                                outQ = s.QTY.ToDecimal();
                            rem = startQ + inQ + outQ;

                            var sc = new mh_StockCard
                            {
                                ItemNo = PR1,
                                ItemName = tool.InternalName,
                                dFrom = dtDate1.Value.Date,
                                dTo = dtDate2.Value.Date,
                                DocumentRef = refNo,
                                Location = ddlLocation.Text,
                                No = rNo,
                                UOM = tool.BaseUOM,
                                USERID = ClassLib.Classlib.User,
                                Shelf = tool.ShelfNo,
                                SubCust = sName,
                                PCSUnit = pcsunit,
                                Date = s.CreateDate,
                                StartQty = startQ,
                                InQty = inQ,
                                OutQty = outQ,
                                RemainQty = rem,
                                CustNo = custNo,
                                CustName = custName,
                            };
                            db.mh_StockCards.InsertOnSubmit(sc);
                            db.SubmitChanges();
                        }


                        //var g = (from ix in db.sp_R009_Stock_Movement(PRNo1, PRNo2,dt1,dt2,ddlLocation.Text, Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"))) select ix).ToList();
                        //if (g.Count() > 0)
                        //{
                        Report.Reportx1.Value = new string[1];
                        Report.Reportx1.Value[0] = ClassLib.Classlib.User;
                        Report.Reportx1.WReport = "ReportStockCard";
                        Report.Reportx1 op = new Report.Reportx1("StockCard.rpt");
                        op.Show();
                        //}
                        //else
                        //    MessageBox.Show("not found.");
                    }
                    else if (Type.Equals("Claim"))
                    {
                        //var g = (from ix in db.sp_R017_ReportClaim(PRNo1, PRNo2, Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"))) select ix).ToList();
                        //if (g.Count() > 0)
                        //{
                        Report.Reportx1.Value = new string[2];
                        Report.Reportx1.Value[0] = PRNo1;
                        Report.Reportx1.Value[1] = PRNo2;
                        Report.Reportx1.WReport = "ReportClaim";
                        Report.Reportx1 op = new Report.Reportx1("ReportClaim.rpt");
                        op.Show();
                        //}
                        //else
                        //    MessageBox.Show("not found.");
                    }
                    else if (Type.Equals("SellFG"))
                    {
                        var g = (from ix in db.sp_R018_ReportSellFG(PRNo1, PRNo2, Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"))) select ix).ToList();
                        if (g.Count() > 0)
                        {
                            Report.Reportx1.Value = new string[2];
                            Report.Reportx1.Value[0] = PRNo1;
                            Report.Reportx1.Value[1] = PRNo2;
                            Report.Reportx1.WReport = "ReportSellFG";
                            Report.Reportx1 op = new Report.Reportx1("ReportSellFG.rpt");
                            op.Show();
                        }
                        else
                            MessageBox.Show("not found.");
                    }
                    else if (Type.Equals("ShippingAVG"))
                    {
                        string YYYY = "";
                        string MM = "";
                        if (cbYYYYMM.Checked)
                        {
                            if ((ddlYear.Text != "" && ddlMonth.Text == "")
                                || (ddlYear.Text == "" && ddlMonth.Text != ""))
                            {
                                //err += "- “เลขที่ PR:” เป็นค่าว่าง \n";
                                MessageBox.Show("- “กรณีที่ระบุ ปีหรือเดือน ต้องห้ามเป็นค่าว่าง”");
                                return;
                            }
                            YYYY = ddlYear.Text;
                            MM = ddlMonth.Text;
                        }

                        //var g = (from ix in db.sp_R019_ReportShippingAVG(YYYY,MM,PRNo1, PRNo2, Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"))) select ix).ToList();
                        //if (g.Count() > 0)
                        //{
                        Report.Reportx1.Value = new string[4];
                        Report.Reportx1.Value[0] = ddlYear.Text;
                        Report.Reportx1.Value[1] = ddlMonth.Text;
                        Report.Reportx1.Value[2] = PRNo1;
                        Report.Reportx1.Value[3] = PRNo2;
                        Report.Reportx1.WReport = "ReportShippingAVG";
                        Report.Reportx1 op = new Report.Reportx1("ReportShippingAVG.rpt");
                        op.Show();
                        //}
                        //else
                        //    MessageBox.Show("not found.");
                    }
                    else if (Type.Equals("ReportProductionOrder"))
                    {
                        string Dt1 = "";
                        string Dt2 = "";
                        //var g = (from ix in db.sp_R022_ReportProductsOrder(PRNo1, PRNo2, Dt1, Dt2, Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"))) select ix).ToList();
                        //if (g.Count() > 0)
                        //{

                        Report.Reportx1.Value = new string[4];
                        Report.Reportx1.Value[0] = PRNo1;
                        Report.Reportx1.Value[1] = PRNo2;
                        Report.Reportx1.Value[2] = Dt1;
                        Report.Reportx1.Value[3] = Dt2;
                        Report.Reportx1.WReport = "ReporProductionOrder";
                        Report.Reportx1 op = new Report.Reportx1("ReporProductionOrder.rpt");
                        op.Show();
                        //}
                        //else
                        //    MessageBox.Show("not found.");
                    }
                    else if (Type.Equals("ReportAccidentSlip"))
                    {
                        ////string Dt1 = "";
                        ////string Dt2 = "";
                        //var g = (from ix in db.sp_R007_ReportShipping(PRNo1, PRNo2, Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"))) select ix).ToList();
                        //if (g.Count() > 0)
                        //{

                        Report.Reportx1.Value = new string[2];
                        Report.Reportx1.Value[0] = PRNo1;
                        Report.Reportx1.Value[1] = PRNo2;
                        Report.Reportx1.WReport = "ReportAccidentSlip";
                        Report.Reportx1 op = new Report.Reportx1("ReportAccidentSlip.rpt");
                        op.Show();
                        //}
                        //else
                        //    MessageBox.Show("not found.");
                    }
                    else if (Type.Equals("PackingList"))
                    {
                        Report.Reportx1.Value = new string[2];
                        Report.Reportx1.Value[0] = dtDate1.Value.Date.ToString("dd/MMM/yyyy");
                        Report.Reportx1.Value[1] = dtDate2.Value.Date.ToString("dd/MMM/yyyy");
                        Report.Reportx1.WReport = "PackingList";
                        Report.Reportx1 op = new Report.Reportx1("ReceiveFG.rpt");
                        op.Show();
                    }
                    else if (Type.Equals("ListPurchaseAndPR"))
                    {
                        Report.Reportx1.Value = new string[2];

                        Report.Reportx1.Value[0] = dtDate1.Value.Date.ToString("dd/MMM/yyyy");
                        Report.Reportx1.Value[1] = dtDate2.Value.Date.ToString("dd/MMM/yyyy");
                        Report.Reportx1.WReport = "ListPurchaseAndPR";
                        Report.Reportx1 op = new Report.Reportx1("ListPurchaseAndPR.rpt");
                        op.Show();
                    }
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
