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
using ClassLib;
using Microsoft.VisualBasic;
using System.IO;

namespace StockControl
{
    public partial class ApproveList : Telerik.WinControls.UI.RadRibbonForm
    {
        public ApproveList(string CodeNox)
        {
            InitializeComponent();
            CodeNo = CodeNox;
            //this.Text = "ประวัติ "+ Screen;
        }
        Telerik.WinControls.UI.RadTextBox CodeNo_tt = new Telerik.WinControls.UI.RadTextBox();
        int screen = 0;
        public ApproveList(Telerik.WinControls.UI.RadTextBox CodeNox)
        {
            InitializeComponent();
            CodeNo_tt = CodeNox;
            screen = 1;
        }
        public ApproveList()
        {
            InitializeComponent();
        }

        string CodeNo = "";
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
            LoadDefault();
            dtDateFrom.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
            dtDateTo.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
            //Set_dt_Print();
            //radGridView1.ReadOnly = true;
            radGridView1.AutoGenerateColumns = false;
            DataLoad();
        }
        private void LoadDefault()
        {
            try
            {
                //List<string> Type = new List<string>();
                //Type.Add("");
                string UserID = ClassLib.Classlib.User;

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var g = (from ix in db.mh_ApproveSetups.Where(s => s.Active == 1 && s.UserID.ToUpper().Trim() == UserID.ToUpper().Trim())
                             select new { ix.ApproveType }).ToList();
                    if (g.Count > 0)
                    {
                        foreach (var gg in g)
                        {
                            ddlType.Items.Add(gg.ApproveType);
                        }

                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void DataLoad()
        {

            //dt.Rows.Clear();
            try
            {
                radGridView1.DataSource = null;
                this.Cursor = Cursors.WaitCursor;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    string dt1 = "";
                    string dt2 = "";
                    if (cbDate.Checked)
                    {
                        dt1 = Convert.ToDateTime(dtDateFrom.Value).ToString("yyyyMMdd");
                        dt2 = Convert.ToDateTime(dtDateTo.Value).ToString("yyyyMMdd");
                    }
                    radGridView1.DataSource = db.sp_059_ApproveList(txtDocNo.Text, ddlType.Text, ddlStatus.Text, Classlib.User);
                    dbClss.SetRowNo(radGridView1);
                }


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;

        }

        //private void DataLoad()
        //{

        //    //dt.Rows.Clear();
        //    try
        //    {
        //        radGridView1.DataSource = null;
        //        this.Cursor = Cursors.WaitCursor;
        //        using (DataClasses1DataContext db = new DataClasses1DataContext())
        //        {
        //            //dt = ClassLib.Classlib.LINQToDataTable(db.tb_Units.ToList());
        //            //radGridView1.DataSource = db.tb_Histories.Where(s => s.ScreenName == ScreenSearch).OrderBy(o => o.CreateDate).ToList();
        //            int c = 0;

        //            DateTime inclusiveStart = dtDateFrom.Value.Date;
        //            // Include the *whole* of the day indicated by searchEndDate
        //            DateTime exclusiveEnd = dtDateTo.Value.Date.AddDays(1);

        //            var g = (from ix in db.mh_PurchaseOrders select ix)
        //                .Where(a => a.VendorNo.Contains(txtVendorNo.Text)
        //                && (a.CHStatus == "Completed")
        //                && (a.CHNo != null)
        //                && a.CHNo.Contains(txtCHNo.Text)
        //                && a.PONo.Contains(txtDocNo.Text)
        //                && a.VendorName.Contains(txtVendorName.Text)
        //                && (((a.CHDate >= inclusiveStart
        //                           && a.CHDate < exclusiveEnd)
        //                           && cbDate.Checked == true)
        //                 || (cbDate.Checked == false)
        //                           )).ToList();
        //            if (g.Count > 0)
        //            {

        //                radGridView1.DataSource = g;
        //                foreach (var x in radGridView1.Rows)
        //                {
        //                    c += 1;
        //                    x.Cells["No"].Value = c;
        //                }
        //            }


        //        }
        //    }
        //    catch(Exception ex) { MessageBox.Show(ex.Message); }
        //    this.Cursor = Cursors.Default;


        //    //    radGridView1.DataSource = dt;
        //}
        private int compart_date(DateTime date1, DateTime date2)
        {
            int result = 0;
            //DateTime date1 = new DateTime(2009, 8, 1, 0, 0, 0);
            //DateTime date2 = new DateTime(2009, 8, 1, 12, 0, 0);
            result = DateTime.Compare(date1, date2);



            return result;
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            radGridView1.ReadOnly = false;
            radGridView1.AllowAddNewRow = false;
            radGridView1.Rows.AddNew();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            radGridView1.ReadOnly = true;

            radGridView1.AllowAddNewRow = false;
            DataLoad();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            radGridView1.ReadOnly = false;

            radGridView1.AllowAddNewRow = false;
            //DataLoad();
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
            //  dbClss.ExportGridCSV(radGridView1);
            dbClss.ExportGridXlSX(radGridView1);
        }



        private void btnFilter1_Click(object sender, EventArgs e)
        {
            radGridView1.EnableFiltering = true;
        }

        private void btnUnfilter1_Click(object sender, EventArgs e)
        {
            radGridView1.EnableFiltering = false;
        }

        private void radMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radButtonElement1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            radGridView1.EndEdit();
            if (baseClass.IsApprove())
            {
                using (var db = new DataClasses1DataContext())
                {
                    int id = 0;
                    string DocNo = "";
                    string Type = "";
                    int Seq = 0;
                    List<string> AppList = new List<string>();

                    foreach (var rd in radGridView1.Rows)
                    {
                        if (dbClss.TBo(rd.Cells["S"].Value))
                        {
                            DocNo = dbClss.TSt(rd.Cells["ApproveDocuNo"].Value);
                            Type = dbClss.TSt(rd.Cells["ApproveType"].Value);
                            id = dbClss.TInt(rd.Cells["id"].Value);

                            if ((dbClss.TSt(rd.Cells["Status"].Value) == "Waiting") || (dbClss.TSt(rd.Cells["Status"].Value) == "Reject"))
                                db.sp_064_mh_ApproveList_Update2(id, DocNo, Type, ClassLib.Classlib.User, "", "Approve");
                            ////Old-------------------------
                            //var g = (from ix in db.mh_ApproveLists
                            //         where ix.id == id && ix.Status == "Waiting"
                            //         select ix).ToList();
                            //if (g.Count > 0)
                            //{
                            //    var gg = (from ix in db.mh_ApproveLists
                            //              where ix.id == id && ix.Status == "Waiting"
                            //              select ix).First();
                            //    gg.Status = "Approved";
                            //    gg._By = ClassLib.Classlib.User;
                            //    gg._Date = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                            //    Seq = dbClss.TInt(gg.Seq);

                            //    db.SubmitChanges();

                            //    //คนที่อยู่ลำดับเดียวกัน
                            //    var u = (from ix in db.mh_ApproveLists
                            //             where ix.Seq == Seq && ix.Status == "Waiting"
                            //             && ix.ApproveDocuNo == DocNo
                            //             select ix).ToList();
                            //    if (u.Count > 0) 
                            //    {
                            //        foreach(var uu in u)
                            //        {
                            //            uu.Status = "Approved";
                            //            db.SubmitChanges();
                            //        }
                            //    }
                            //    db.SubmitChanges();


                            //    var a = (from ix in db.mh_ApproveLists
                            //             where ix.Status == "Cancel"
                            //              && ix.ApproveDocuNo == DocNo
                            //             select ix).ToList();
                            //    if (a.Count>0)  //มีรายการในระบบ
                            //    {
                            //        var c_approve = a.Where(ab => ab.Status == "Approved").ToList().Count();
                            //        var c_Waiting = a.Where(ab => ab.Status == "Waiting").ToList().Count();
                            //        if(c_Waiting<=0 && c_approve>0)
                            //            Update_Status(Type, DocNo);
                            //    }

                            //    dbClss.AddHistory(this.Name, "อนุมัติ Approve", "สร้าง Approve [" + DocNo + "]", DocNo);
                            //    baseClass.Info("Delete invoice complete.");
                            //}

                            ////Old-------------------------
                        }

                    }
                    DataLoad();
                    baseClass.Info("Approve complete.");
                }
            }

            this.Cursor = Cursors.Default;
            ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            ClassLib.Memory.Heap();
        }
        private void Update_Status(string Type, string DocNo)
        {
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    if (Type == "SaleOrder")
                    {
                        var p = db.mh_SaleOrders.Where(x => x.SONo == DocNo && x.Active).ToList();
                        if (p.Where(x => x.Active == true && (x.Status.ToSt() == "Waiting")).Count() > 0)
                        {
                            foreach (var pp in p)
                            {
                                //pp.Status = "Process";
                                pp.ApproveBy = Classlib.User;
                                pp.ApproveDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                            }
                            db.SubmitChanges();

                            baseClass.Info("Approve complete.");

                        }
                    }
                    else if (Type == "Plan")
                    {

                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void radGridView1_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            //try
            //{
            //    if (e.RowIndex > -1)
            //    {
            //        if (screen.Equals(1))
            //        {
            //            CodeNo_tt.Text = dbClss.TSt(e.Row.Cells["CHNo"].Value);
            //            this.Close();
            //        }
            //        else
            //        {
            //            ChargePO a = new ChargePO(dbClss.TSt(e.Row.Cells["CHNo"].Value));
            //            a.ShowDialog();
            //            //this.Close();
            //        }
            //    }
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        DataTable dt_Kanban = new DataTable();

        private void Set_dt_Print()
        {

            dt_Kanban.Columns.Add(new DataColumn("CodeNo", typeof(string)));
            dt_Kanban.Columns.Add(new DataColumn("PartNo", typeof(string)));
            dt_Kanban.Columns.Add(new DataColumn("PartDescription", typeof(string)));
            dt_Kanban.Columns.Add(new DataColumn("ShelfNo", typeof(string)));
            dt_Kanban.Columns.Add(new DataColumn("LeadTime", typeof(decimal)));
            dt_Kanban.Columns.Add(new DataColumn("VendorName", typeof(string)));
            dt_Kanban.Columns.Add(new DataColumn("GroupType", typeof(string)));
            dt_Kanban.Columns.Add(new DataColumn("ToolLife", typeof(decimal)));
            dt_Kanban.Columns.Add(new DataColumn("Max", typeof(decimal)));
            dt_Kanban.Columns.Add(new DataColumn("Min", typeof(decimal)));
            dt_Kanban.Columns.Add(new DataColumn("ReOrderPoint", typeof(decimal)));
            dt_Kanban.Columns.Add(new DataColumn("BarCode", typeof(Image)));

        }

        private void btn_Print_Barcode_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    dt_Kanban.Rows.Clear();

            //    using (DataClasses1DataContext db = new DataClasses1DataContext())
            //    {
            //        var g = (from ix in db.tb_Items select ix).Where(a => a.CodeNo == txtType.Text).ToList();
            //        if (g.Count() > 0)
            //        {
            //            foreach (var gg in g)
            //            {
            //                dt_Kanban.Rows.Add(gg.CodeNo, gg.ItemNo, gg.ItemDescription, gg.ShelfNo, gg.Leadtime, gg.VendorItemName, gg.GroupCode, gg.Toollife, gg.MaximumStock, gg.MinimumStock, gg.ReOrderPoint, gg.BarCode);
            //            }
            //            //DataTable DT =  StockControl.dbClss.LINQToDataTable(g);
            //            //Reportx1 po = new Reportx1("Report_PurchaseRequest_Content1.rpt", DT, "FromDT");
            //            //po.Show();

            //            Report.Reportx1 op = new Report.Reportx1("001_Kanban_Part.rpt", dt_Kanban, "FromDL");
            //            op.Show();
            //        }
            //        else
            //            MessageBox.Show("not found.");
            //    }

            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btn_PrintPR_Click(object sender, EventArgs e)
        {
            try
            {

                //dt_ShelfTag.Rows.Clear();
                string ApproveDocuNo = "";
                if (radGridView1.Rows.Count > 0)
                {
                    ApproveDocuNo = StockControl.dbClss.TSt(radGridView1.CurrentRow.Cells["ApproveDocuNo"].Value);

                    string Type = dbClss.TSt(radGridView1.CurrentRow.Cells["ApproveType"].Value);
                    if (Type == "Purchase Order")
                    {
                        //PrintPR a = new PrintPR(ApproveDocuNo, ApproveDocuNo, "PO");
                        //a.ShowDialog();
                        Report.Reportx1.Value = new string[2];
                        Report.Reportx1.Value[0] = ApproveDocuNo;
                        Report.Reportx1.Value[1] = ApproveDocuNo;
                        Report.Reportx1.WReport = "PO";
                        Report.Reportx1 op = new Report.Reportx1("PO.rpt");
                        op.Show();
                    }
                    else if (Type == "Job Req")
                    {
                        Report.Reportx1.Value = new string[1];
                        Report.Reportx1.Value[0] = ApproveDocuNo;
                        Report.Reportx1.WReport = "JOBNoList";
                        Report.Reportx1 op = new Report.Reportx1("JobOrderSheet3.rpt");
                        op.Show();
                    }
                    else if (Type == "Sale Order")
                    {
                        //PrintPR a = new PrintPR(ApproveDocuNo, ApproveDocuNo, "PR");
                        //a.ShowDialog();
                        Report.Reportx1.Value = new string[2];
                        Report.Reportx1.Value[0] = ApproveDocuNo;
                        Report.Reportx1.Value[1] = ApproveDocuNo;
                        Report.Reportx1.WReport = "SaleOrder";
                        Report.Reportx1 op = new Report.Reportx1("CustomerPO.rpt");
                        op.Show();
                    }
                    else if (Type == "Taking Stock")
                    {
                        Report.Reportx1.Value = new string[1];
                        Report.Reportx1.Value[0] = ApproveDocuNo;
                        Report.Reportx1.WReport = "CheckStockList";
                        Report.Reportx1 op = new Report.Reportx1("ReportCheckStock.rpt");
                        op.Show();
                    }
                    else if (Type == "Job Cancel" || Type == "Change Job")
                    {
                        Report.Reportx1.Value = new string[1];
                        Report.Reportx1.Value[0] = ApproveDocuNo;
                        Report.Reportx1.WReport = "ReportChangeJob";
                        Report.Reportx1 op = new Report.Reportx1("ReportChangeJob.rpt");
                        op.Show();
                    }
                    else if (Type == "Price List")
                    {
                        using (var db = new DataClasses1DataContext())
                        {
                            var m = db.mh_PriceLists.Where(x => x.PriceListCode == ApproveDocuNo).FirstOrDefault();
                            if (m != null)
                            {
                                if (m.AttachFile.ToSt() != "")
                                {
                                    try
                                    {
                                        System.Diagnostics.Process.Start(Path.Combine(baseClass.GetPathServer(PathCode.PriceList), m.AttachFile));
                                    }
                                    catch (Exception ex)
                                    {
                                        baseClass.Error(ex.Message);
                                    }
                                }
                                else
                                    baseClass.Warning("ไม่มีไฟล์แนบ.");
                            }
                            else
                            {
                                baseClass.Warning("ไม่พบเลขที่เอกสาร.");
                            }
                        }
                    }

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void radButtonElement2_Click(object sender, EventArgs e)
        {
            try
            {
                string PRNo = "";
                if (radGridView1.Rows.Count > 0)
                    PRNo = StockControl.dbClss.TSt(radGridView1.CurrentRow.Cells["PRNo"].Value);

                PrintPR a = new PrintPR(PRNo, PRNo, "PR");
                a.ShowDialog();
            }
            catch { }
        }

        private void frezzRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (radGridView1.Rows.Count > 0)
                {

                    int Row = 0;
                    Row = radGridView1.CurrentRow.Index;
                    dbClss.Set_Freeze_Row(radGridView1, Row);

                    //foreach (var rd in radGridView1.Rows)
                    //{
                    //    if (rd.Index <= Row)
                    //    {
                    //        radGridView1.Rows[rd.Index].PinPosition = PinnedRowPosition.Top;
                    //    }
                    //    else
                    //        break;
                    //}
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void frezzColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (radGridView1.Columns.Count > 0)
                {
                    int Col = 0;
                    Col = radGridView1.CurrentColumn.Index;
                    dbClss.Set_Freeze_Column(radGridView1, Col);

                    //foreach (var rd in radGridView1.Columns)
                    //{
                    //    if (rd.Index <= Col)
                    //    {
                    //        radGridView1.Columns[rd.Index].PinPosition = PinnedColumnPosition.Left;
                    //    }
                    //    else
                    //        break;
                    //}
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void unFrezzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                dbClss.Set_Freeze_UnColumn(radGridView1);
                dbClss.Set_Freeze_UnRows(radGridView1);
                //foreach (var rd in radGridView1.Rows)
                //{
                //    radGridView1.Rows[rd.Index].IsPinned = false;
                //}
                //foreach (var rd in radGridView1.Columns)
                //{
                //    radGridView1.Columns[rd.Index].IsPinned = false;                   
                //}

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void radButtonElement2_Click_1(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            radGridView1.EndEdit();
            if (baseClass.IsReject())
            {
                using (var db = new DataClasses1DataContext())
                {
                    int id = 0;
                    string DocNo = "";
                    string Type = "";
                    int Seq = 0;
                    List<string> AppList = new List<string>();

                    string GetMarkup = Interaction.InputBox("Reason For Reject", "Reason : ", "", 400, 250);
                    if (!GetMarkup.Trim().Equals(""))
                    {
                        foreach (var rd in radGridView1.Rows)
                        {
                            if (dbClss.TBo(rd.Cells["S"].Value))
                            {
                                DocNo = dbClss.TSt(rd.Cells["ApproveDocuNo"].Value);
                                Type = dbClss.TSt(rd.Cells["ApproveType"].Value);
                                id = dbClss.TInt(rd.Cells["id"].Value);
                                if ((dbClss.TSt(rd.Cells["Status"].Value) == "Waiting") || (dbClss.TSt(rd.Cells["Status"].Value) == "Completed") || (dbClss.TSt(rd.Cells["Status"].Value) == "Approved"))
                                    db.sp_064_mh_ApproveList_Update2(id, DocNo, Type, ClassLib.Classlib.User, GetMarkup, "Reject");
                            }

                        }
                        DataLoad();
                        baseClass.Info("Reject complete.");
                    }
                }
            }

            this.Cursor = Cursors.Default;
            ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            ClassLib.Memory.Heap();
        }
    }
}
