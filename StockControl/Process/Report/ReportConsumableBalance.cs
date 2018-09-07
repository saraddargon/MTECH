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
using OfficeOpenXml;
using System.IO;

namespace StockControl
{
    public partial class ReportConsumableBalance : Telerik.WinControls.UI.RadRibbonForm
    {
        public ReportConsumableBalance()
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

            //dt.Columns.Add(new DataColumn("CodeNo", typeof(string)));
            //dt.Columns.Add(new DataColumn("ItemDescription", typeof(string)));
            //dt.Columns.Add(new DataColumn("Order", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("BackOrder", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("StockQty", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("UnitBuy", typeof(string)));
            //dt.Columns.Add(new DataColumn("PCSUnit", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("LeadTime", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("MaxStock", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("MinStock", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("VendorNo", typeof(string)));
            //dt.Columns.Add(new DataColumn("VendorName", typeof(string)));



        }
        int crow = 99;
        private void Unit_Load(object sender, EventArgs e)
        {
            //DateTime firstOfNextMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1);
            //DateTime lastOfThisMonth = firstOfNextMonth.AddDays(-1);
            //dtDate1.Value = firstOfNextMonth;
            //dtDate2.Value = lastOfThisMonth;
           // GETDTRow();
            DefaultItem();

            crow = 0;
        }
        private void DefaultItem()
        {

            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                //var gt = (from ix in db.tb_GroupTypes where ix.GroupActive == true select ix).ToList();
                ////GridViewComboBoxColumn comboBoxColumn = this.radGridView1.Columns["GroupCode"] as GridViewComboBoxColumn;
                //cboGroupType.DisplayMember = "GroupCode";
                //cboGroupType.ValueMember = "GroupCode";
                //cboGroupType.DataSource = gt;
                //cboGroupType.SelectedIndex = -1;

                ddlLocation.DisplayMember = "Code";
                ddlLocation.ValueMember = "Code";
                ddlLocation.DataSource = db.mh_Locations.Where(s => s.Active == true).ToList();


                ddlYear.DataSource = null;
                ddlYear.DisplayMember = "YYYY";
                ddlYear.ValueMember = "YYYY";

                var g = (from ix in db.sp_Select_Year() select ix).ToList();
                ddlYear.DataSource = g;
                ddlYear.SelectedIndex = 0;

                ddlMonth.Text = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("MM");
                ddlYear.Text = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("yyyy");


            }
        }
       

        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("ต้องการออกรายงาน หรือไม่ ?", "ออกรายงาน", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    string YYYY = ddlYear.Text;
                    string MM = ddlMonth.Text;

                    if (YYYY == "" || MM == "")
                    {
                        MessageBox.Show("เลือกปีหรือเดือน !");
                        return;
                    }
                    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    {
                        var g = (from ix in db.sp_R020_ConsumableBalance(txtCodeNo.Text
                                 , txtCodeNo.Text,ddlYear.Text,ddlMonth.Text,ddlLocation.Text
                                 , Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"))) select ix).ToList();
                        if (g.Count() > 0)
                        {
                            Report.Reportx1.Value = new string[5];
                            Report.Reportx1.Value[0] = txtCodeNo.Text;
                            Report.Reportx1.Value[1] = txtCodeNo.Text;
                            Report.Reportx1.Value[2] = ddlYear.Text;
                            Report.Reportx1.Value[3] = ddlMonth.Text;
                            Report.Reportx1.Value[4] = ddlLocation.Text;


                            Report.Reportx1.WReport = "ReportConsumableBalance";
                            Report.Reportx1 op = new Report.Reportx1("ReportConsumableBalance.rpt");
                            op.Show();
                        }
                        else
                            MessageBox.Show("not found.");
                    }

                }
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
        private bool GetData(string FileName)
        {
            bool ck = false;
            this.Cursor = Cursors.WaitCursor;
            try
            {

                ////System.IO.File.Copy(Report.CRRReport.dbPartReport + "Account_Sheet.xls", FileName, true);
                ////System.Diagnostics.Process.Start();

                //using (DataClasses1DataContext db = new DataClasses1DataContext())
                //{
                //    radGridView1.AutoGenerateColumns = true;

                //    string YYYY = ddlYear.Text;
                //    string MM = ddlMonth.Text;
                //    string Group = cboGroupType.Text;
                //    radGridView1.DataSource = db.sp_E009_Report_DAILY_PURCHASE_List(YYYY, MM, Group);

                //    decimal Amount = 0;
                //    foreach (var x in radGridView1.Rows)
                //    {
                //        Amount += dbClss.TDe(x.Cells["Amount"].Value);
                //        x.Cells["SumAmount"].Value = Amount;
                //    }
                //}



                dbClss.ExportGridXlSX2(radGridView1, FileName);

                dbClss.AddHistory(this.Name, "ออกรายงาน", "เลือกออกรายงาน ", "");
                ck = true;
                
            }
            catch { ck = false; }
            this.Cursor = Cursors.Default;
            return ck;
        }
        private void ExportData()
        {
            //try
            //{
            //    //if (radGridView1.Rows.Count > 0)
            //    //{
            //    //เขียนไฟล์ Excel
            //    string sourcefile = "";
            //    string descfile = "";
            //    SaveFileDialog dialog = new SaveFileDialog();
            //    dialog.Filter = "xlsx File (*.xlsx)|*.xlsx";
            //    if (dialog.ShowDialog() == DialogResult.OK)
            //    {
            //        this.Cursor = Cursors.WaitCursor;

            //        sourcefile = System.AppDomain.CurrentDomain.BaseDirectory + "\\Report\\Excel_Receive_DeliveryMonth.xlsx";
            //        descfile = dialog.FileName;
            //        System.IO.File.Copy(sourcefile, descfile, true);

            //        File.Copy(sourcefile, descfile, true);

            //        string YYYY = ddlYear.Text;
            //        string MM = ddlMonth.Text;
            //        string Group = cboGroupType.Text;
            //        using (DataClasses1DataContext db = new DataClasses1DataContext())
            //        {
            //            var Data = db.sp_E009_Report_DAILY_PURCHASE_List(YYYY, MM, Group).ToList();
            //            //var Data = db.sp_R016_Receive_Month(txtPRNo1.Text, txtPRNo2.Text, dt11, dt22, YYYY + MM, Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"))).ToList();

            //            using (var package = new ExcelPackage(new FileInfo(descfile)))
            //            {
            //                var ws_rm = package.Workbook.Worksheets[1];
            //                //ws_rm.Cells[1, 1].Value = "ใบบันทึกรับสินค้าประจำวัน ";
            //                if (cbYYYYMM.Checked)
            //                    ws_rm.Cells[1, 1].Value = "DAILY  PURCHASE  LIST  OF " + dbClss.Month_Eng(ddlMonth.Text) + " " + ddlYear.Text; //ประจำเดือน มกราคม 2018

            //                //" + ConnectDB.ConnectDB.Around + "/" + ConnectDB.ConnectDB.YYYY + " (" + String.Format("{0:dd MMM yy}", ConnectDB.ConnectDB.StartDate) + " - " + String.Format("{0:dd MMM yy}", ConnectDB.ConnectDB.EndDate) + ")";
            //                ws_rm.Cells[4, 1].LoadFromCollection(Data, false);

            //                package.Save();
            //            }
            //        }
            //        GC.GetTotalMemory(false);
            //        GC.Collect();
            //        GC.WaitForPendingFinalizers();
            //        GC.Collect();
            //        GC.GetTotalMemory(false);

            //        MessageBox.Show("Export Data to complete.");
            //        //dbClss.Info("Export Data to complete.");
            //        //
            //        //System.Diagnostics.Process.Start(descfile);
            //    }
            //}catch(Exception ex) { MessageBox.Show(ex.Message); }
            //finally
            //{            this.Cursor = Cursors.Default;
            //}
        }


        private void cboVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (!cboVendor.Text.Equals(""))
            //{
            //    txtVendorNo.Text = Convert.ToString(cboVendor.SelectedValue);
            //    //var I = (from ix in db.tb_Vendors select ix).Where(a => a.Active == true 
            //    //                && a.VendorName.Equals(cboVendor.Text)).ToList();
            //    //if (I.Count > 0)

            //}
            //else
            //    txtVendorNo.Text = "";
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

        }
    }
}
