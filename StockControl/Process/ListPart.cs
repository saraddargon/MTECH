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

namespace StockControl
{
    public partial class ListPart : Telerik.WinControls.UI.RadRibbonForm
    {
        List<GridViewRowInfo> RetDT;
        public ListPart(string CodeNox)
        {
            InitializeComponent();
            CodeNo = CodeNox;
            //this.Text = "ประวัติ "+ Screen;
        }       
        Telerik.WinControls.UI.RadTextBox CodeNo_tt = new Telerik.WinControls.UI.RadTextBox();
        int screen = 0;
        string TypePart = "";
        string List_RetDT = "";
        public ListPart(Telerik.WinControls.UI.RadTextBox  CodeNox,string TypePartx,string Sc)
        {
            InitializeComponent();
            CodeNo_tt = CodeNox;
            TypePart = TypePartx;
            if (Sc == "Bom")
                screen = 3;
            else if (Sc == "CreateJob")
                screen = 3;
            else if (Sc == "ShipAVG")
                screen = 3;
            else
                screen = 1;
        }
        public ListPart(List<GridViewRowInfo> RetDT, string TypePartx, string Sc)
        {
            InitializeComponent();
            this.RetDT = RetDT;

            TypePart = TypePartx;
            if (Sc == "Bom")
                screen = 3;
            else if (Sc == "CreateJob")
                screen = 3;
            else if (Sc == "ShipAVG")
                screen = 3;
            else if (Sc == "Taking" || Sc== "AdjustStock")
                screen = 2;
            else
                screen = 1;

            List_RetDT = "ListTaking";
        }
        public ListPart(Telerik.WinControls.UI.RadTextBox CodeNox, string TypePartx, List<GridViewRowInfo> RetDT)
        {
            InitializeComponent();
            this.RetDT = RetDT;
            CodeNo_tt = CodeNox;
            TypePart = TypePartx;
            screen = 2;
        }
        public ListPart()
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

            Set_FindData();
            Set_dt_Print();
            LoadDefault();
            //radGridView1.ReadOnly = true;
            radGridView1.AutoGenerateColumns = false;
            DataLoad();
        }
        private void LoadDefault()
        {
            ddlLocation.DataSource = null;
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                ddlLocation.DisplayMember = "Location";
                ddlLocation.ValueMember = "Location";
               // ddlLocation.DataSource = db.tb_Locations.Where(s => s.Active == true && s.Status == "Completed").ToList();
                var g = (from ix in db.tb_Locations select ix).Where(s => s.Active == true && s.Status == "Completed").ToList();

                List<string> a = new List<string>();
                if(g.Count>0)
                {
                    foreach (var gg in g)
                        a.Add(gg.Location);
                }
                a.Add("");
                ddlLocation.DataSource = a;
                ddlLocation.Text = "";

            }
        }
        private void Set_FindData()
        {
            if (TypePart == "All" || TypePart =="")
            {
                ddlTypePart.Items.Add("");
                ddlTypePart.Items.Add("FG");
                ddlTypePart.Items.Add("WIP");
                ddlTypePart.Items.Add("RM");
                ddlTypePart.Items.Add("Other");


                ddlTypePart.Text = "";
            }
            else if (TypePart == "WIP-RM")
            {
                ddlTypePart.Items.Add("WIP");
                ddlTypePart.Items.Add("RM");

                ddlTypePart.Text = "RM";
            }
            else if (TypePart == "WIP-RM-Other")
            {
                ddlTypePart.Items.Add("WIP");
                ddlTypePart.Items.Add("RM");
                ddlTypePart.Items.Add("Other");

                ddlTypePart.Text = "RM";
            }

            else if (TypePart == "FG-WIP")
            {
                ddlTypePart.Items.Add("FG");
                ddlTypePart.Items.Add("WIP");

                ddlTypePart.Text = "FG";
            }
            else if (TypePart == "FG")
            {
                ddlTypePart.Items.Add("FG");
                ddlTypePart.Text = "FG";
            }
            else if (TypePart == "WIP")
            {
                ddlTypePart.Items.Add("WIP");
                ddlTypePart.Text = "WIP";
            }
            else if (TypePart == "RM")
            {
                ddlTypePart.Items.Add("RM");
                ddlTypePart.Text = "RM";
            }
           
            if(screen==2)
            {
                radButtonElement1.Text = "เพิ่มรายการ";
                radRibbonBarGroup1.Text = "Add Part";
            }
            if (screen == 3)
            {
                radButtonElement1.Text = "เลือกรายการ";
                radRibbonBarGroup1.Text = "Select Part";
            }

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
                    //dt = ClassLib.Classlib.LINQToDataTable(db.tb_Units.ToList());
                    //radGridView1.DataSource = db.tb_Histories.Where(s => s.ScreenName == ScreenSearch).OrderBy(o => o.CreateDate).ToList();
                    int c = 0;

                    //var g = (from ix in db.tb_Items select ix).Where(a => a.CodeNo.Contains(txtCodeNo.Text)
                    //    && a.ItemNo.Contains(txtPartName.Text)
                    //    && a.ItemDescription.Contains(txtDescription.Text)
                    //    && a.VendorItemName.Contains(txtVendorName.Text))
                    //    .ToList();


                    string Lo1 = dbClss.TSt(db.get_Location_No(1));
                    string Lo2 = dbClss.TSt(db.get_Location_No(2));
                    string Lo3 = dbClss.TSt(db.get_Location_No(3));
                    string Lo4 = dbClss.TSt( db.get_Location_No(4));

                    var g = (from ix in db.sp_014_Select_PartList(txtCodeNo.Text,txtPartName.Text                             
                             ,txtDescription.Text,"",txtVendorName.Text,ddlTypePart.Text,"",ddlLocation.Text) select ix).ToList();
                    if (g.Count > 0)
                    {
                        radGridView1.DataSource = g;

                        if (Lo1 == "")
                            radGridView1.Columns["QtyLocation11"].IsVisible = false;
                        if (Lo2 == "")
                            radGridView1.Columns["QtyLocation22"].IsVisible = false;
                        if (Lo3 == "")
                            radGridView1.Columns["QtyLocation33"].IsVisible = false;
                        if (Lo4 == "")
                            radGridView1.Columns["QtyLocation44"].IsVisible = false;

                        radGridView1.Columns["QtyLocation11"].HeaderText = Lo1;
                        radGridView1.Columns["QtyLocation22"].HeaderText = Lo2;
                        radGridView1.Columns["QtyLocation33"].HeaderText = Lo3;
                        radGridView1.Columns["QtyLocation44"].HeaderText = Lo4;

                        foreach (var x in radGridView1.Rows)
                        {
                            c += 1;
                            x.Cells["No"].Value = c;

                            x.Cells["QtyLocation11"].Value = 0;
                            x.Cells["QtyLocation22"].Value = 0;
                            x.Cells["QtyLocation33"].Value = 0;
                            x.Cells["QtyLocation44"].Value = 0;

                            //if (Lo1 == "")
                            //    x.Cells["QtyLocation11"].ColumnInfo.IsVisible = false;
                            //if (Lo2 == "")
                            //    x.Cells["QtyLocation22"].ColumnInfo.IsVisible = false;
                            //if (Lo3 == "")
                            //    x.Cells["QtyLocation33"].ColumnInfo.IsVisible = false;
                            //if (Lo4 == "")
                            //    x.Cells["QtyLocation44"].ColumnInfo.IsVisible = false;

                            //x.Cells["QtyLocation11"].ColumnInfo.HeaderText = Lo1;
                            //x.Cells["QtyLocation22"].ColumnInfo.HeaderText = Lo2;
                            //x.Cells["QtyLocation33"].ColumnInfo.HeaderText = Lo3;
                            //x.Cells["QtyLocation44"].ColumnInfo.HeaderText = Lo4;

                            if (dbClss.TSt(x.Cells["CodeNo"].Value) != "")
                            {
                                

                                var l = (from ix in db.sp_031_Location_Stock(dbClss.TSt(x.Cells["CodeNo"].Value), "") select ix).ToList();
                                if (l.Count > 0)
                                {
                                    foreach (var ll in l)
                                    {
                                        if (Lo1 == ll.Location)
                                        {
                                            x.Cells["QtyLocation11"].Value = ll.Qty;
                                        }
                                        else if (Lo2 == ll.Location)
                                            x.Cells["QtyLocation22"].Value = ll.Qty;
                                        else if (Lo3 == ll.Location)
                                            x.Cells["QtyLocation33"].Value = ll.Qty;
                                        else if (Lo4 == ll.Location)
                                            x.Cells["QtyLocation44"].Value = ll.Qty;
                                    }
                                }
                            }
                            
                        }
                    }


                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;


            //    radGridView1.DataSource = dt;
        }
        private bool CheckDuplicate(string code)
        {
            bool ck = false;

            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                int i = (from ix in db.tb_Units where ix.UnitCode == code select ix).Count();
                if (i > 0)
                    ck = false;
                else
                    ck = true;
            }
            return ck;
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
            if (screen == 2)
            {
                try
                {
                    radGridView1.EndEdit();

                    //foreach (var rd1 in radGridView1.Rows)
                    //{
                    //    if(StockControl.dbClss.TBo(rd1.Cells["S"].Value).Equals(true))
                    //    {
                    //        RetDT.Add(rd1);
                    //    }
                    //}
                    foreach (GridViewRowInfo rowinfo in radGridView1.Rows.Where(o => Convert.ToBoolean(o.Cells["S"].Value)))
                    {
                        RetDT.Add(rowinfo);
                    }

                    this.Close();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            else if (screen == 1 || screen == 3)
            {
                if (radGridView1.Rows.Count > 0)
                {
                    CodeNo_tt.Text = Convert.ToString(radGridView1.CurrentRow.Cells["CodeNo"].Value);
                    this.Close();
                }
            }
            else
            {
                this.Cursor = Cursors.WaitCursor;
                CreatePart sc = new CreatePart();
                this.Cursor = Cursors.Default;
                sc.ShowDialog();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void radGridView1_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex > -1)
                {

                    if (screen.Equals(1) //เปิดจากหน้า CreatePart
                    || screen.Equals(3)  // เปิดจากหน้า Bom โดยเลือกแค่ตัวเดียว
                    )
                    {
                        CodeNo_tt.Text = Convert.ToString(e.Row.Cells["CodeNo"].Value);
                        this.Close();
                    }
                    else if (screen.Equals(2))
                        return;
                    else
                    {
                        CreatePart sc = new CreatePart(Convert.ToString(e.Row.Cells["CodeNo"].Value));
                        this.Cursor = Cursors.Default;
                        sc.Show();
                    }
                }               
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        DataTable dt_ShelfTag = new DataTable();
        DataTable dt_Kanban = new DataTable();

        private void Set_dt_Print()
        {
            dt_ShelfTag.Columns.Add(new DataColumn("CodeNo", typeof(string)));
            dt_ShelfTag.Columns.Add(new DataColumn("PartDescription", typeof(string)));
            dt_ShelfTag.Columns.Add(new DataColumn("ShelfNo", typeof(string)));


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
        private void Print_Shelftag_datatable()
        {
            try
            {
                dt_ShelfTag.Rows.Clear();

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var g = (from ix in db.tb_Items select ix).Where(a => a.CodeNo == txtCodeNo.Text).ToList();
                    if (g.Count() > 0)
                    {
                        foreach (var gg in g)
                        {
                            dt_ShelfTag.Rows.Add(gg.CodeNo, gg.ItemDescription, gg.ShelfNo);
                        }
                        //DataTable DT =  StockControl.dbClss.LINQToDataTable(g);
                        //Reportx1 po = new Reportx1("Report_PurchaseRequest_Content1.rpt", DT, "FromDT");
                        //po.Show();

                        Report.Reportx1 op = new Report.Reportx1("002_BoxShelf_Part.rpt", dt_ShelfTag, "FromDL");
                        op.Show();
                    }
                    else
                        MessageBox.Show("not found.");
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void btn_PrintShelfTag_Click(object sender, EventArgs e)
        {
            try
            {

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    //delete ทิ้งก่อน
                    var deleteItem = (from ii in db.TempPrintShelfs where ii.UserName == ClassLib.Classlib.User select ii);
                    foreach (var d in deleteItem)
                    {
                        db.TempPrintShelfs.DeleteOnSubmit(d);
                        db.SubmitChanges();
                    }

                    int c = 0;
                    string CodeNo = "";
                    radGridView1.EndEdit();
                    //insert
                    foreach (var Rowinfo in radGridView1.Rows)
                    {
                        if (StockControl.dbClss.TBo(Rowinfo.Cells["S"].Value).Equals(true))
                        {
                            CodeNo = StockControl.dbClss.TSt(Rowinfo.Cells["CodeNo"].Value);
                            var g = (from ix in db.tb_Items select ix).Where(a => a.CodeNo == CodeNo).ToList();
                            if (g.Count() > 0)
                            {
                                
                                c += 1;
                                TempPrintShelf ps = new TempPrintShelf();
                                ps.UserName = ClassLib.Classlib.User;
                                ps.CodeNo = g.FirstOrDefault().CodeNo;
                                ps.PartDescription = g.FirstOrDefault().ItemDescription;
                                ps.PartNo = g.FirstOrDefault().ItemNo;
                                ps.ShelfNo = g.FirstOrDefault().ShelfNo;
                                ps.Max = Convert.ToDecimal(g.FirstOrDefault().MaximumStock);
                                ps.Min = Convert.ToDecimal(g.FirstOrDefault().MinimumStock);
                                ps.OrderPoint = Convert.ToDecimal(g.FirstOrDefault().ReOrderPoint);
                                db.TempPrintShelfs.InsertOnSubmit(ps);
                                db.SubmitChanges();
                            }
                        }

                    }
                    if (c > 0)
                    {
                        Report.Reportx1.Value = new string[2];
                        Report.Reportx1.Value[0] = ClassLib.Classlib.User;
                        Report.Reportx1.WReport = "002_BoxShelf_Part";
                        Report.Reportx1 op = new Report.Reportx1("002_BoxShelf_Part.rpt");
                        op.Show();
                    }
                    else
                        MessageBox.Show("not found.");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btn_Print_Barcode_Click(object sender, EventArgs e)
        {
            try
            {
                dt_Kanban.Rows.Clear();
                this.Cursor = Cursors.WaitCursor;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {


                    // Step 1 delete UserName
                    var deleteItem = (from ii in db.TempPrintKanbans where ii.UserName == ClassLib.Classlib.User select ii);
                    foreach (var d in deleteItem)
                    {
                        db.TempPrintKanbans.DeleteOnSubmit(d);
                        db.SubmitChanges();
                    }

                    // Step 2 Insert to Table

                    int c = 0;
                    string CodeNo = "";
                    radGridView1.EndEdit();
                    //insert
                    foreach (var Rowinfo in radGridView1.Rows)
                    {
                        if (StockControl.dbClss.TBo(Rowinfo.Cells["S"].Value).Equals(true))
                        {
                            CodeNo = StockControl.dbClss.TSt(Rowinfo.Cells["CodeNo"].Value);
                            var g = (from ix in db.tb_Items select ix).Where(a => a.CodeNo == CodeNo).ToList();
                            if (g.Count() > 0)
                            {
                                c += 1;
                                TempPrintKanban tm = new TempPrintKanban();
                                tm.UserName = ClassLib.Classlib.User;
                                tm.CodeNo = g.FirstOrDefault().CodeNo;
                                tm.PartDescription = g.FirstOrDefault().ItemDescription;
                                tm.PartNo = g.FirstOrDefault().ItemNo;
                                tm.VendorName = g.FirstOrDefault().VendorItemName;
                                tm.ShelfNo = g.FirstOrDefault().ShelfNo;
                                tm.GroupType = g.FirstOrDefault().GroupCode;
                                tm.Max = Convert.ToDecimal(g.FirstOrDefault().MaximumStock);
                                tm.Min = Convert.ToDecimal(g.FirstOrDefault().MinimumStock);
                                tm.ReOrderPoint = Convert.ToDecimal(g.FirstOrDefault().ReOrderPoint);
                                tm.ToolLife = Convert.ToDecimal(g.FirstOrDefault().Toollife);
                                tm.Location = g.FirstOrDefault().Location;
                                tm.TypePart = g.FirstOrDefault().TypePart;
                                byte[] barcode = StockControl.dbClss.SaveQRCode2D(g.FirstOrDefault().CodeNo);
                                tm.BarCode = barcode;
                                db.TempPrintKanbans.InsertOnSubmit(tm);
                                db.SubmitChanges();
                                this.Cursor = Cursors.Default;

                            }
                        }
                    }
                    if (c > 0)
                    {
                        Report.Reportx1.Value = new string[2];
                        Report.Reportx1.Value[0] = ClassLib.Classlib.User;
                        Report.Reportx1.WReport = "001_Kanban_Part";
                        Report.Reportx1 op = new Report.Reportx1("001_Kanban_Part.rpt");
                        op.Show();
                    }
                    else
                        MessageBox.Show("not found.");

                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;
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

        private void btnStockMovement_Click(object sender, EventArgs e)
        {

            string CodeNo = "";
            if (radGridView1.Rows.Count > 0)
                CodeNo = StockControl.dbClss.TSt(radGridView1.CurrentRow.Cells["CodeNo"].Value);

            PrintPR a = new PrintPR(CodeNo, CodeNo, "ReportStockMovement");
            a.ShowDialog();
        }

        private void radButtonElement3_Click(object sender, EventArgs e)
        {
            Calculate_Movement a = new Calculate_Movement();
            a.Show();
        }
    }
}
