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
using Telerik.WinControls.Data;

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
            LoadDefault();
            Set_FindData();
            Set_dt_Print();
            //LoadDefault();
            //radGridView1.ReadOnly = true;
            radGridView1.AutoGenerateColumns = false;
            DataLoad();
        }
        private void LoadDefault()
        {
            cboLocation.DataSource = null;
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                //ddlLocation.DisplayMember = "Location";
                //ddlLocation.ValueMember = "Location";
                //// ddlLocation.DataSource = db.tb_Locations.Where(s => s.Active == true && s.Status == "Completed").ToList();
                //var g = (from ix in db.mh_Locations select ix).Where(s => s.Active == true && s.Status == "Completed").ToList();

                //List<string> a = new List<string>();
                //if (g.Count > 0)
                //{
                //    foreach (var gg in g)
                //        a.Add(gg.Location);
                //}
                //a.Add("");
                //ddlLocation.DataSource = a;
                //ddlLocation.Text = "";


                this.cboLocation.AutoFilter = true;
                this.cboLocation.AutoCompleteMode = AutoCompleteMode.Append;
                FilterDescriptor lo = new FilterDescriptor();
                lo.PropertyName = this.cboLocation.ValueMember;
                lo.Operator = FilterOperator.StartsWith;
                this.cboLocation.EditorControl.MasterTemplate.FilterDescriptors.Add(lo);

                cboLocation.DisplayMember = "Code";
                cboLocation.ValueMember = "Name";
                cboLocation.DataSource = db.mh_Locations.Where(s => s.Active == true).ToList();
                cboLocation.SelectedIndex = -1;
                cboLocation.Text = "";


            }
        }
        private void Set_FindData()
        {
            if (TypePart == "All" || TypePart =="")
            {
                ddlTypePart.Items.Add("");
                ddlTypePart.Items.Add("FG");
                ddlTypePart.Items.Add("SEMI");
                ddlTypePart.Items.Add("RM");
                //ddlTypePart.Items.Add("Other");


                ddlTypePart.Text = "";
            }
            else if (TypePart == "SEMI-RM")
            {
                ddlTypePart.Items.Add("SEMI");
                ddlTypePart.Items.Add("RM");

                ddlTypePart.Text = "RM";
            }
            else if (TypePart == "SEMI-RM-Other")
            {
                ddlTypePart.Items.Add("SEMI");
                ddlTypePart.Items.Add("RM");
                ddlTypePart.Items.Add("Other");

                ddlTypePart.Text = "RM";
            }

            else if (TypePart == "FG-SEMI")
            {
                ddlTypePart.Items.Add("FG");
                ddlTypePart.Items.Add("SEMI");

                ddlTypePart.Text = "FG";
            }
            else if (TypePart == "FG")
            {
                ddlTypePart.Items.Add("FG");
                ddlTypePart.Text = "FG";
            }
            else if (TypePart == "SEMI")
            {
                ddlTypePart.Items.Add("SEMI");
                ddlTypePart.Text = "SEMI";
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
                    //int c = 0;

                    var g = (from ix in db.mh_Items select ix)
                        .Where(a => a.InternalNo.Contains(txtCodeNo.Text)
                        && a.InternalName.Contains(txtPartName.Text)
                        && a.VendorName.Contains(txtVendorName.Text)
                        && a.InventoryGroup.Contains(ddlTypePart.Text)
                        && a.Location.Contains(cboLocation.Text)
                        && a.Active==true
                        )
                        .ToList();
                    if(g.Count>0)
                    {
                        radGridView1.DataSource = g;

                        decimal RemainStock = 0;
                        //string Location = "";
                        foreach (var x in radGridView1.Rows)
                        {
                            //Location = dbClss.TSt(x.Cells["Location"].Value);
                            RemainStock = (Convert.ToDecimal(db.Cal_QTY_Remain_Location(Convert.ToString(x.Cells["InternalNo"].Value), "Free", 0, Convert.ToString(x.Cells["Location"].Value),0)));
                            x.Cells["RemainStock"].Value = RemainStock;
                        }
                            dbClss.SetRowNo1(radGridView1);
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
                int i = (from ix in db.mh_Units where ix.UnitCode == code select ix).Count();
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
                    CodeNo_tt.Text = Convert.ToString(radGridView1.CurrentRow.Cells["InternalNo"].Value);
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
                        CodeNo_tt.Text = Convert.ToString(e.Row.Cells["InternalNo"].Value);
                        this.Close();
                    }
                    else if (screen.Equals(2))
                        return;
                    else
                    {
                        CreatePart sc = new CreatePart(Convert.ToString(e.Row.Cells["InternalNo"].Value));
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
                            CodeNo = StockControl.dbClss.TSt(Rowinfo.Cells["InternalNo"].Value);
                            var g = (from ix in db.mh_Items select ix).Where(a => a.InternalNo == CodeNo).ToList();
                            if (g.Count() > 0)
                            {
                                
                                c += 1;
                                TempPrintShelf ps = new TempPrintShelf();
                                ps.UserName = ClassLib.Classlib.User;
                                ps.CodeNo = g.FirstOrDefault().InternalNo;
                                ps.PartDescription = g.FirstOrDefault().InternalDescription;
                                ps.PartNo = g.FirstOrDefault().InternalName;
                                ps.ShelfNo = "";// g.FirstOrDefault().ShelfNo;
                                ps.Max = Convert.ToDecimal(g.FirstOrDefault().MaximumQty);
                                ps.Min = Convert.ToDecimal(g.FirstOrDefault().MinimumQty);
                                ps.OrderPoint = Convert.ToDecimal(g.FirstOrDefault().ReorderPoint);
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
                            CodeNo = StockControl.dbClss.TSt(Rowinfo.Cells["InternalNo"].Value);
                            var g = (from ix in db.mh_Items select ix).Where(a => a.InternalNo == CodeNo).ToList();
                            if (g.Count() > 0)
                            {
                                c += 1;
                                TempPrintKanban tm = new TempPrintKanban();
                                tm.UserName = ClassLib.Classlib.User;
                                tm.CodeNo = g.FirstOrDefault().InternalNo;
                                tm.PartDescription = g.FirstOrDefault().InternalDescription;
                                tm.PartNo = g.FirstOrDefault().InternalName;
                                tm.VendorName = g.FirstOrDefault().VendorName;
                                tm.ShelfNo = "";// g.FirstOrDefault().ShelfNo;
                                tm.GroupType = g.FirstOrDefault().GroupType;
                                tm.Max = Convert.ToDecimal(g.FirstOrDefault().MaximumQty);
                                tm.Min = Convert.ToDecimal(g.FirstOrDefault().MinimumQty);
                                tm.ReOrderPoint = Convert.ToDecimal(g.FirstOrDefault().ReorderPoint);
                                tm.ToolLife = 1;// Convert.ToDecimal(g.FirstOrDefault().Toollife);
                                tm.Location = g.FirstOrDefault().Location;
                                tm.TypePart = g.FirstOrDefault().InventoryGroup;
                                byte[] barcode = StockControl.dbClss.SaveQRCode2D(g.FirstOrDefault().InternalNo);
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
                CodeNo = StockControl.dbClss.TSt(radGridView1.CurrentRow.Cells["InternalNo"].Value);

            PrintPR a = new PrintPR(CodeNo, CodeNo, "ReportStockMovement");
            a.ShowDialog();
        }

        private void radButtonElement3_Click(object sender, EventArgs e)
        {
            Calculate_Movement a = new Calculate_Movement();
            a.Show();
        }

        private void radButtonElement5_Click(object sender, EventArgs e)
        {
            Report.Reportx1.Value = new string[1];
            Report.Reportx1.Value[0] = ddlTypePart.Text;
            Report.Reportx1.WReport = "ItemList";
            Report.Reportx1 op = new Report.Reportx1("ReportItemList.rpt");
            op.Show();
        }
    }
}
