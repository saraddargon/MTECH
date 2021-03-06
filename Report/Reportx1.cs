﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
using System.Runtime.InteropServices;
using System.Globalization;

namespace Report
{
    public partial class Reportx1 : Form
    {
        public Reportx1()
        {
            InitializeComponent();
        }
        public Reportx1(string Rptx)
        {
            InitializeComponent();
            Rpt = Rptx;
        }
        public Reportx1(string Rptx, DataTable Dt, string Fromd)
        {
            InitializeComponent();
            Rpt = Rptx;
            _Dt = Dt;
            fromdt = Fromd;
        }
        private string Rpt = "";
        private DataTable _Dt = null;
        private string fromdt = "";
        public static string WReport { get; set; }
        public static ReportDocument rptNull { get; set; }
        public static string[] Value { get; set; }
        static string SERVERName = Report.CRRReport.ServerName;
        static string DBName = Report.CRRReport.DbName;
        static string Userdb = Report.CRRReport.dbUser;
        static string PassDb = Report.CRRReport.dbPass;
        static string DATA = "";

        private void Report_Load(object sender, EventArgs e)
        {
            try
            {
                SERVERName = Report.CRRReport.ServerName;
                DBName = Report.CRRReport.DbName;
                Userdb = Report.CRRReport.dbUser;
                PassDb = Report.CRRReport.dbPass;

                // MessageBox.Show(SERVERName + "," + DBName + "," + Userdb + "," + PassDb);
                // this.Cursor = Cursors.WaitCursor;
                // dbClass.rptSourceX.Refresh();
                // crystalReportViewer1.ReportSource = null;
                DATA = "";
                DATA = AppDomain.CurrentDomain.BaseDirectory;
                DATA = DATA + @"Report\" + Rpt;

                if (fromdt.Equals(""))
                {

                    CRRReport.rptSourceX.Load(DATA, OpenReportMethod.OpenReportByDefault);
                    SetDataSourceConnection(CRRReport.rptSourceX);
                    SetParameter(CRRReport.rptSourceX);
                    crystalReportViewer1.ReportSource = CRRReport.rptSourceX;
                    crystalReportViewer1.Zoom(100);
                }
                else
                {
                    //FromData Table
                    CRRReport.rptSourceX.Load(DATA, OpenReportMethod.OpenReportByDefault);
                    SetDataSourceConnection(CRRReport.rptSourceX);
                    //SetDataSourceConnection(JcControl.rptSourceX);
                    //JcControl.SetParameter(JcControl.rptSourceX);          
                    crystalReportViewer1.ReportSource = CryStal_pdf(_Dt, DATA);
                    crystalReportViewer1.Zoom(100);
                }
                //this.Cursor = Cursors.Default;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { }
            // this.reportViewer1.RefreshReport();

        }
        public static void SetDataSourceConnection(CrystalDecisions.CrystalReports.Engine.ReportDocument rpt)
        {
            try
            {

                for (int i = 0; i < rpt.Subreports.Count; i++)
                {

                    rpt.Subreports[i].DataSourceConnections[0].SetConnection(SERVERName, DBName, Userdb, PassDb);
                    rpt.Subreports[i].DataSourceConnections[0].IntegratedSecurity = false;
                    rpt.Subreports[i].DataSourceConnections[SERVERName, DBName].SetLogon(Userdb, PassDb);
                }
                rpt.DataSourceConnections[0].SetConnection(SERVERName, DBName, Userdb, PassDb);
                rpt.DataSourceConnections[0].IntegratedSecurity = false;
                rpt.DataSourceConnections[SERVERName, DBName].SetLogon(Userdb, PassDb);

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }
        public static CrystalDecisions.CrystalReports.Engine.ReportDocument CryStal_pdf(DataTable dt, string Rpt)
        {
            CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            try
            {
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new Exception("Search data not found");
                }
                else
                {
                    rpt.Load(Rpt);
                    //SetDataSourceConnection(rpt);
                    rpt.SetDataSource(dt);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            return rpt;
        }
        public static CrystalDecisions.CrystalReports.Engine.ReportDocument CryStal_pdf2(DataTable dt, string Rpt)
        {
            CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            try
            {
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new Exception("Search data not found");
                }
                else
                {

                    //  rpt.Load(Path.Combine(@"C:\Program Files\GBarcode", Rpt));
                    // SetDataSourceConnection(rpt);
                    rpt.SetDataSource(dt);
                }
            }
            catch (Exception ex) { MessageBox.Show("Error Report;01" + ex.Message); }
            return rpt;
        }
        public static void SetParameter(ReportDocument rptDc)
        {

            switch (WReport)
            {
                case "ALLReport":
                    {

                        rptDc.SetParameterValue("@DocumentNo", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));

                    }
                    break;
                case "ALLReport2":
                    {

                        // rptDc.SetParameterValue("@DocumentNo", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));

                    }
                    break;
                case "ItemList":
                    {

                        rptDc.SetParameterValue("@InventoryGroup", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));

                    }
                    break;
                case "JOBNoList":
                    {

                        rptDc.SetParameterValue("@JobNo", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));

                    }
                    break;
                case "CustomerList":
                    {

                        rptDc.SetParameterValue("@Status", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));

                    }
                    break;
                case "ProductionList":
                    {

                        rptDc.SetParameterValue("@Status", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));

                    }
                    break;
                case "SaleOrder":
                    {

                        rptDc.SetParameterValue("@SONo", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@SONo2", Convert.ToString(Value[1].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));

                    }
                    break;
                case "Invoice":
                    {

                        rptDc.SetParameterValue("@InvNo", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@InvNo2", Convert.ToString(Value[1].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));

                    }
                    break;
                case "Shipment":
                    {

                        rptDc.SetParameterValue("@ShippingNo1", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@ShippingNo2", Convert.ToString(Value[1].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));

                    }
                    break;
                case "001_Kanban_Part":
                    {

                        rptDc.SetParameterValue("@User", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));

                    }
                    break;
                case "002_BoxShelf_Part":
                    {

                        rptDc.SetParameterValue("@User", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));

                    }
                    break;
                case "ReportPR":
                    {

                        rptDc.SetParameterValue("@PRNo", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                        // rptDc.SetParameterValue("@Action", Convert.ToInt32(ClassReport.Value[1]));
                    }
                    break;
                case "PR":
                    {

                        rptDc.SetParameterValue("@PRNoFrom", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@PRNoTo", Convert.ToString(Value[1].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                        // rptDc.SetParameterValue("@Action", Convert.ToInt32(ClassReport.Value[1]));
                    }
                    break;
                case "PO":
                    {

                        rptDc.SetParameterValue("@PONoFrom", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@PONoTo", Convert.ToString(Value[1].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                        // rptDc.SetParameterValue("@Action", Convert.ToInt32(ClassReport.Value[1]));
                    }
                    break;
                case "ReportPO":
                    {

                        rptDc.SetParameterValue("@PONo", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@Datex", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                        // rptDc.SetParameterValue("@Action", Convert.ToInt32(ClassReport.Value[1]));
                    }
                    break;
                case "ReportReceive":
                    {

                        rptDc.SetParameterValue("@RCNo", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                        // rptDc.SetParameterValue("@Action", Convert.ToInt32(ClassReport.Value[1]));
                    }
                    break;
                case "ReportReceive2":
                    {
                        rptDc.SetParameterValue("@RCNo1", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@RCNo2", Convert.ToString(Value[1].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                        // rptDc.SetParameterValue("@Action", Convert.ToInt32(ClassReport.Value[1]));
                    }
                    break;
                case "ReportShipping":
                    {

                        rptDc.SetParameterValue("@ShippingNo", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                        // rptDc.SetParameterValue("@Action", Convert.ToInt32(ClassReport.Value[1]));
                    }
                    break;
                case "ReportShipping2":
                    {

                        rptDc.SetParameterValue("@ShippingNo1", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@ShippingNo2", Convert.ToString(Value[1].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                        // rptDc.SetParameterValue("@Action", Convert.ToInt32(ClassReport.Value[1]));
                    }
                    break;
                case "ReportShippingToDay":
                    {

                        rptDc.SetParameterValue("@ShippingNo1", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@ShippingNo2", Convert.ToString(Value[1].ToString()));
                        rptDc.SetParameterValue("@Dt1", Convert.ToString(Value[2].ToString()));
                        rptDc.SetParameterValue("@Dt2", Convert.ToString(Value[3].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                        // rptDc.SetParameterValue("@Action", Convert.ToInt32(ClassReport.Value[1]));
                    }
                    break;
                case "ReportAdjustStock":
                    {

                        rptDc.SetParameterValue("@AdjustNo1", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@AdjustNo2", Convert.ToString(Value[1].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                        // rptDc.SetParameterValue("@Action", Convert.ToInt32(ClassReport.Value[1]));
                    }
                    break;
                case "JobCard":
                    {

                        rptDc.SetParameterValue("@JobCardFrom", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@JobCardTo", Convert.ToString(Value[1].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                        // rptDc.SetParameterValue("@Action", Convert.ToInt32(ClassReport.Value[1]));
                    }
                    break;
                case "ReportStockMovement":
                    {
                        rptDc.SetParameterValue("@CodeNoFrom", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@CodeNoTo", Convert.ToString(Value[1].ToString()));
                        rptDc.SetParameterValue("@Location", Convert.ToString(Value[2].ToString()));
                        rptDc.SetParameterValue("@dt1", Convert.ToString(Value[3].ToString()));
                        rptDc.SetParameterValue("@dt2", Convert.ToString(Value[4].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                    }
                    break;
                case "ReportStockCard":
                    {
                        rptDc.SetParameterValue("@USERID", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                    }
                    break;
                case "JobCard_Cost":
                    {
                        rptDc.SetParameterValue("@JobCard", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@TempJobCard", Convert.ToString(Value[1].ToString()));
                        rptDc.SetParameterValue("@YYYY", Convert.ToString(Value[2].ToString()));
                        rptDc.SetParameterValue("@MM", Convert.ToString(Value[3].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                    }
                    break;
                case "Bom":
                    {
                        rptDc.SetParameterValue("@PartNo", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@BomNo", Convert.ToString(Value[1].ToString()));
                        rptDc.SetParameterValue("@dt1", Convert.ToString(Value[2].ToString()));
                        rptDc.SetParameterValue("@dt2", Convert.ToString(Value[3].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                    }
                    break;
                case "ReportClaim":
                    {

                        rptDc.SetParameterValue("@ClaimNo1", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@ClaimNo2", Convert.ToString(Value[1].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                        // rptDc.SetParameterValue("@Action", Convert.ToInt32(ClassReport.Value[1]));
                    }
                    break;
                case "ReportSellFG":
                    {

                        rptDc.SetParameterValue("@DocNo1", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@DocNo2", Convert.ToString(Value[1].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                        // rptDc.SetParameterValue("@Action", Convert.ToInt32(ClassReport.Value[1]));
                    }
                    break;
                case "ReportShippingAVG":
                    {

                        rptDc.SetParameterValue("@YYYY", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@MM", Convert.ToString(Value[1].ToString()));
                        rptDc.SetParameterValue("@CodeNo1", Convert.ToString(Value[2].ToString()));
                        rptDc.SetParameterValue("@CodeNo2", Convert.ToString(Value[3].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                        // rptDc.SetParameterValue("@Action", Convert.ToInt32(ClassReport.Value[1]));
                    }
                    break;
                case "ReportConsumableBalance":
                    {

                        rptDc.SetParameterValue("@CodeNoFrom", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@CodeNoTo", Convert.ToString(Value[1].ToString()));
                        rptDc.SetParameterValue("@YYYY", Convert.ToString(Value[2].ToString()));
                        rptDc.SetParameterValue("@MM", Convert.ToString(Value[3].ToString()));
                        rptDc.SetParameterValue("@Location", Convert.ToString(Value[4].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                        // rptDc.SetParameterValue("@Action", Convert.ToInt32(ClassReport.Value[1]));
                    }
                    break;
                case "ReporProductionOrder":
                    {
                        rptDc.SetParameterValue("@DocumentNo", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@DocumentNo2", Convert.ToString(Value[1].ToString()));
                        rptDc.SetParameterValue("@Dt1", Convert.ToString(Value[2].ToString()));
                        rptDc.SetParameterValue("@Dt2", Convert.ToString(Value[3].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                    }
                    break;
                case "WhereUsed":
                    {
                        rptDc.SetParameterValue("@Code", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));

                    }
                    break;
                case "WorkCenter":
                    {
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                    }
                    break;
                case "ProductionRM":
                    {
                        rptDc.SetParameterValue("@JobNo", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                    }
                    break;
                case "ReportAccidentSlip":
                    {
                        rptDc.SetParameterValue("@ShippingNo1", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@ShippingNo2", Convert.ToString(Value[1].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                    }
                    break;
                case "PickSlip":
                    {
                        rptDc.SetParameterValue("@ShipmentNo", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                    }
                    break;
                case "PickSlip_RM":
                    {
                        rptDc.SetParameterValue("@ShipmentNo", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                    }
                    break;
                case "CheckStockList":
                    {
                        rptDc.SetParameterValue("@CheckNo", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                    }
                    break;
                case "TagFG":
                    {
                        rptDc.SetParameterValue("@BomNo", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@USERID", Convert.ToString(Value[1].ToString()));
                        rptDc.SetParameterValue("@Datex", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                    }
                    break;
                case "PackingList":
                    {
                        rptDc.SetParameterValue("@Date1", Convert.ToDateTime(Value[0].ToString(), new CultureInfo("en-US")));
                        rptDc.SetParameterValue("@Date2", Convert.ToDateTime(Value[1].ToString(), new CultureInfo("en-US")));
                    }
                    break;
                case "CustomerList2":
                    {
                        rptDc.SetParameterValue("@CSTMNo", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                    }
                    break;
                case "VendorList2":
                    {
                        rptDc.SetParameterValue("@VendorNo", Convert.ToString(Value[0].ToString()));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                    }
                    break;
                case "ReceiveAndDelivery":
                    {
                        rptDc.SetParameterValue("@Date1", DateTime.ParseExact(Value[0], "dd/MMM/yyyy", new CultureInfo("en-US")));
                        rptDc.SetParameterValue("@Date2", DateTime.ParseExact(Value[1], "dd/MMM/yyyy", new CultureInfo("en-US")).AddDays(1).AddMinutes(-1));
                    }
                    break;
                case "DeliveryPlanning":
                    {
                        rptDc.SetParameterValue("@yy", Convert.ToInt32(Value[0]));
                        rptDc.SetParameterValue("@mm", Convert.ToInt32(Value[1]));
                        rptDc.SetParameterValue("@ItemNo", Convert.ToString(Value[2]));
                        rptDc.SetParameterValue("@Cstm", Convert.ToString(Value[3]));
                        rptDc.SetParameterValue("@DateNow", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                    }
                    break;
                case "DeliveryPlanningMonth":
                    {
                        rptDc.SetParameterValue("@yy", Convert.ToInt32(Value[0]));
                        rptDc.SetParameterValue("@mm", Convert.ToInt32(Value[1]));
                        rptDc.SetParameterValue("@mm2", Convert.ToInt32(Value[2]));
                        rptDc.SetParameterValue("@ItemNo", Convert.ToString(Value[3]));
                        rptDc.SetParameterValue("@Cstm", Convert.ToString(Value[4]));
                        rptDc.SetParameterValue("@DateNow", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                    }
                    break;
                case "ReportChangeJob":
                    {

                        rptDc.SetParameterValue("@DocNo", Convert.ToString(Value[0]));
                        rptDc.SetParameterValue("@DateNow", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                    }
                    break;
                case "Signature":
                    {
                        rptDc.SetParameterValue("@User", Convert.ToString(Value[0]));
                        rptDc.SetParameterValue("@Pass", Convert.ToString(Value[1]));
                        rptDc.SetParameterValue("@DataTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                    }
                    break;
                case "ListPurchaseAndPR":
                    {
                        rptDc.SetParameterValue("@Date1", DateTime.ParseExact(Value[0], "dd/MMM/yyyy", new CultureInfo("en-US")));
                        rptDc.SetParameterValue("@Date2", DateTime.ParseExact(Value[1], "dd/MMM/yyyy", new CultureInfo("en-US")).AddDays(1).AddMinutes(-1));
                        rptDc.SetParameterValue("@DocNo1", Convert.ToString(Value[2]));
                        rptDc.SetParameterValue("@DocNo2", Convert.ToString(Value[3]));
                        rptDc.SetParameterValue("@DateTime", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));
                    }
                    break;
            }
        }

        private void printForXPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printDialog1.UseEXDialog = false;
            DialogResult dr = printDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                //CRRReport.rptSourceX.VerifyDatabase();
                //CRRReport.rptSourceX.Refresh();
                if (fromdt.Equals(""))
                {
                    //CRRReport.rptSourceX = CryStal_pdf(_Dt, DATA);
                }
                else
                {
                    CRRReport.rptSourceX = CryStal_pdf(_Dt, DATA);
                }
                CRRReport.rptSourceX.PrintToPrinter(1, false, 0, 0);
                //System.Drawing.Printing.PrintDocument printDocument1 = new System.Drawing.Printing.PrintDocument();
                ////Get the Copy times
                //int nCopy = printDocument1.PrinterSettings.Copies;
                ////Get the number of Start Page
                //int sPage = printDocument1.PrinterSettings.FromPage;
                ////Get the number of End Page
                //int ePage = printDocument1.PrinterSettings.ToPage;
                //rptSourceX.PrintOptions.PrinterName = printDocument1.PrinterSettings.PrinterName;
                ////Start the printing process.  Provide details of the print job
                //rptSourceX.PrintToPrinter(nCopy, false, sPage, ePage);

            }
        }
    }
}
