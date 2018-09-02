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
using System.Globalization;

namespace StockControl
{
    public partial class Stock_Location : Telerik.WinControls.UI.RadRibbonForm
    {
        List<GridViewRowInfo> RetDT;
        public Stock_Location(Telerik.WinControls.UI.RadTextBox CodeNox)
        {
            InitializeComponent();
            CodeNo_tt = CodeNox;
            TypePart = "All";
            //this.Text = "ประวัติ "+ Screen;
        }
        public Stock_Location(Telerik.WinControls.UI.RadTextBox CodeNox
            , Telerik.WinControls.UI.RadTextBox Locataionxx, Telerik.WinControls.UI.RadTextBox Qtyxx)
        {
            InitializeComponent();
            CodeNo_tt = CodeNox;
            TypePart = "All";
            Locataion_tt = Locataionxx;
            Qty_tt = Qtyxx;
            screen = 1;
            //this.Text = "ประวัติ "+ Screen;
        }
        Telerik.WinControls.UI.RadTextBox CodeNo_tt = new Telerik.WinControls.UI.RadTextBox();
        Telerik.WinControls.UI.RadTextBox Locataion_tt = new Telerik.WinControls.UI.RadTextBox();
        Telerik.WinControls.UI.RadTextBox Qty_tt = new Telerik.WinControls.UI.RadTextBox();

        int screen = 0;
        string TypePart = "";
       
        public Stock_Location(Telerik.WinControls.UI.RadTextBox CodeNox, string TypePartx, List<GridViewRowInfo> RetDT)
        {
            InitializeComponent();
            this.RetDT = RetDT;
            CodeNo_tt = CodeNox;
            TypePart = TypePartx;
            screen = 2;
        }
        public Stock_Location()
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
            //ddlTypePart.Text = "RM";
            //Set_dt_Print();
            //radGridView1.ReadOnly = true;
            dgvData.AutoGenerateColumns = false;
            DataLoad();
            LoadDefault();

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
                if (g.Count > 0)
                {
                    foreach (var gg in g)
                        a.Add(gg.Location);
                }
                a.Add("");
                ddlLocation.DataSource = a;

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

            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                
                try
                {

                    GridViewMultiComboBoxColumn col = (GridViewMultiComboBoxColumn)dgvData.Columns["Location"];
                    col.DataSource = (from ix in db.tb_Locations.Where(s => Convert.ToBoolean(s.Active.Equals(true)) && s.Status == "Completed")
                                      select new { ix.Location }).ToList();

                    col.DisplayMember = "Location";
                    col.ValueMember = "Location";
                    col.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDown;
                    col.FilteringMode = GridViewFilteringMode.DisplayMember;

                    col.AutoSizeMode = BestFitColumnMode.DisplayedDataCells;
                    col.TextAlignment = ContentAlignment.MiddleCenter;
                    col.DropDownStyle = RadDropDownStyle.DropDownList;

                }
                catch { }

            }

            //else if (TypePart == "WIP-RM")
            //{
            //    ddlTypePart.Items.Add("WIP");
            //    ddlTypePart.Items.Add("RM");

            //    ddlTypePart.Text = "RM";
            //}
            //else if (TypePart == "FG-WIP")
            //{
            //    ddlTypePart.Items.Add("FG");
            //    ddlTypePart.Items.Add("WIP");

            //    ddlTypePart.Text = "FG";
            //}
            //else if (TypePart == "FG")
            //{
            //    ddlTypePart.Items.Add("FG");
            //    ddlTypePart.Text = "FG";
            //}
            //else if (TypePart == "WIP")
            //{
            //    ddlTypePart.Items.Add("WIP");
            //    ddlTypePart.Text = "WIP";
            //}
            //else if (TypePart == "RM")
            //{
            //    ddlTypePart.Items.Add("RM");
            //    ddlTypePart.Text = "RM";
            //}

            //if(screen==2)
            //{
            //    radButtonElement1.Text = "เพิ่มรายการ";
            //    radRibbonBarGroup1.Text = "Add Part";
            //}
            //if (screen == 3)
            //{
            //    radButtonElement1.Text = "เลือกรายการ";
            //    radRibbonBarGroup1.Text = "Select Part";
            //}

        }
        private void DataLoad()
        {
            //dt.Rows.Clear();
            try
            {
                dgvData.DataSource = null;
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


                    var g = (from ix in db.sp_032_Location_Stock_List(txtCodeNo.Text,txtPartName.Text,txtDescription.Text,ddlLocation.Text,ddlTypePart.Text) select ix).ToList();
                    if (g.Count > 0)
                    {
                        dgvData.DataSource = g;
                        foreach (var x in dgvData.Rows)
                        {
                            c += 1;
                            x.Cells["No"].Value = c;
                            //       // x.Cells["StockInv"].Value = (Convert.ToDecimal(db.Cal_QTY(Convert.ToString(x.Cells["CodeNo"].Value), "Invoice", 0)));
                            //       // x.Cells["StockDL"].Value = (Convert.ToDecimal(db.Cal_QTY(Convert.ToString(x.Cells["CodeNo"].Value), "Temp", 0)));
                            //       // x.Cells["StockBackOrder"].Value = (Convert.ToDecimal(db.Cal_QTY(Convert.ToString(x.Cells["CodeNo"].Value), "BackOrder", 0)));

                            //    }
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
            dgvData.ReadOnly = false;
            dgvData.AllowAddNewRow = false;
            dgvData.Rows.AddNew();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            dgvData.ReadOnly = true;

            dgvData.AllowAddNewRow = false;
            DataLoad();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            dgvData.ReadOnly = false;

            dgvData.AllowAddNewRow = false;
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
            dbClss.ExportGridXlSX(dgvData);
        }



        private void btnFilter1_Click(object sender, EventArgs e)
        {
            dgvData.EnableFiltering = true;
        }

        private void btnUnfilter1_Click(object sender, EventArgs e)
        {
            dgvData.EnableFiltering = false;
        }

        private void radMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radButtonElement1_Click(object sender, EventArgs e)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                int c = 0;
                string DocNo = StockControl.dbClss.GetNo(19, 2);
                if (DocNo != "")
                {
                    foreach (var g in dgvData.Rows)
                    {

                        if (g.IsVisible.Equals(true) && dbClss.TBo(g.Cells["S"].Value) == true)
                        {
                            var r = (from ix in db.tb_Stocks
                                     where ix.id == dbClss.TInt(g.Cells["id"].Value) && ix.Status != "Cancel"
                                     select ix).ToList();
                            if (r.Count > 0)  //มีรายการในระบบ
                            {
                                c = 1;

                                var gg = (from ix in db.tb_Stocks
                                          where ix.id == dbClss.TInt(g.Cells["id"].Value) && ix.Status != "Cancel"
                                          //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                                          select ix).First();

                                //gg.UpdateBy = ClassLib.Classlib.User;
                                //gg.UpdateDate = DateTime.Now;
                                //dbClss.AddHistory(this.Name, "แก้ไข Receive", "แก้ไขโดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", txtRCNo.Text.Trim());

                                gg.Location = dbClss.TSt(g.Cells["Location"].Value);
                                gg.RemarkLocation = dbClss.TSt(g.Cells["RemarkLocation"].Value);

                                tb_Move_Location mm = new tb_Move_Location();
                                mm.DocNo = DocNo;
                                mm.CreateBy = ClassLib.Classlib.User;
                                mm.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                mm.Location_New = dbClss.TSt(g.Cells["Location"].Value);
                                mm.CodeNo = dbClss.TSt(g.Cells["CodeNo"].Value);
                                mm.Location_Old = dbClss.TSt(g.Cells["TempLocation"].Value);
                                mm.Remark = dbClss.TSt(g.Cells["RemarkLocation"].Value);
                                mm.Refid = dbClss.TInt(g.Cells["id"].Value);
                                db.tb_Move_Locations.InsertOnSubmit(mm);
                                db.SubmitChanges();

                                dbClss.AddHistory(this.Name, "แก้ไข Stock", "แก้ไขสถานที่จัดเก็บ [" + gg.CodeNo + " " + gg.Location + "]", gg.CodeNo);

                            }
                        }
                    }

                    MessageBox.Show("บันทึกสำเร็จ!");

                    DataLoad();
                }
            }
                // if (screen==1)
                //{
                //    CodeNo_tt.Text = Convert.ToString(dgvData.CurrentRow.Cells["CodeNo"].Value);
                //    Locataion_tt.Text = Convert.ToString(dgvData.CurrentRow.Cells["Location"].Value);
                //    Qty_tt.Text = Convert.ToString(dgvData.CurrentRow.Cells["Qty"].Value);

                //    this.Close();
                //}
                //else
                //{
                //    this.Cursor = Cursors.WaitCursor;
                //    MoveStock_Location sc = new MoveStock_Location(Convert.ToString(dgvData.CurrentRow.Cells["CodeNo"].Value), Convert.ToString(dgvData.CurrentRow.Cells["Location"].Value), Convert.ToDecimal(dgvData.CurrentRow.Cells["Qty"].Value));
                //    this.Cursor = Cursors.Default;
                //    sc.ShowDialog();
                //    GC.Collect();
                //    GC.WaitForPendingFinalizers();

                //    ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                //    ClassLib.Memory.Heap();
                //}
            }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void radGridView1_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            //try
            //{
            //    radButtonElement1_Click(null, null);

            //        //if (screen.Equals(1) //เปิดจากหน้า CreatePart
            //        //|| screen.Equals(3)  // เปิดจากหน้า Bom โดยเลือกแค่ตัวเดียว
            //        //)
            //        //{
            //        //    CodeNo_tt.Text = Convert.ToString(e.Row.Cells["CodeNo"].Value);
            //        //    this.Close();
            //        //}
                   
            //        //else
            //        //{
            //        //    CreatePart sc = new CreatePart(Convert.ToString(e.Row.Cells["CodeNo"].Value));
            //        //    this.Cursor = Cursors.Default;
            //        //    sc.Show();
            //        //}                
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
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
                    dgvData.EndEdit();
                    //insert
                    foreach (var Rowinfo in dgvData.Rows)
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
                        dgvData.EndEdit();
                        //insert
                        foreach (var Rowinfo in dgvData.Rows)
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
                if (dgvData.Rows.Count > 0)
                {
                    
                    int Row = 0;
                    Row = dgvData.CurrentRow.Index;
                    dbClss.Set_Freeze_Row(dgvData, Row);

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

                dbClss.Set_Freeze_UnColumn(dgvData);
                dbClss.Set_Freeze_UnRows(dgvData);
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
                if (dgvData.Columns.Count > 0)
                {
                    int Col = 0;
                    Col = dgvData.CurrentColumn.Index;
                    dbClss.Set_Freeze_Column(dgvData, Col);

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

        private void dgvData_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            try
            {
                if (e.Column.Name.Equals("Location"))
                {
                    /////////////มีการ เคลียร์ การ Add ก่อน แล้วค่อย Add ใหม่////////////////
                    //Row = e.RowIndex;
                    RadMultiColumnComboBoxElement Comcol = (RadMultiColumnComboBoxElement)e.ActiveEditor;
                    Comcol.Columns.Clear();

                    //RadMultiColumnComboBoxElement Comcol = (RadMultiColumnComboBoxElement)e.ActiveEditor;
                    Comcol.DropDownSizingMode = SizingMode.UpDownAndRightBottom;
                    Comcol.DropDownWidth = 150;
                    Comcol.DropDownHeight = 150;
                    //Comcol.EditorControl.BestFitColumns(BestFitColumnMode.AllCells);
                    Comcol.EditorControl.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                    //ปรับอัตโนมัติ
                    //Comcol.EditorControl.AutoGenerateColumns = false;
                    //Comcol.BestFitColumns(true, true);
                    Comcol.AutoFilter = true;

                    //Comcol.EditorControl.AllowAddNewRow = true;
                    Comcol.EditorControl.EnableFiltering = true;
                    Comcol.EditorControl.ReadOnly = false;
                    Comcol.ClearFilter();


                    //Comcol.DisplayMember = "ItemNo";
                    //Comcol.ValueMember = "ItemNo";

                    // //----------------------------- ปรับโดยกำหนดเอง
                    Comcol.EditorControl.Columns.Add(new GridViewTextBoxColumn
                    {
                        HeaderText = "สถานที่เก็บ",
                        Name = "Location",
                        FieldName = "Location",
                        Width = 100,
                        AllowFiltering = true,
                        ReadOnly = false
                    }
                   );
                    // Comcol.EditorControl.Columns.Add(new GridViewTextBoxColumn
                    // {
                    //     HeaderText = "Description",
                    //     Name = "Description",
                    //     FieldName = "Description",
                    //     Width = 300,
                    //     AllowFiltering = true,
                    //     ReadOnly = false

                    // }
                    //);


                    //dgvDataDetail.CellEditorInitialized += MasterTemplate_CellEditorInitialized;

                }
            }
            catch { }
        }

        private void dgvData_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            try
            {
                /*gvData.Rows[e.RowIndex].Cells["dgvC"].Value = "T";*/
                dgvData.EndEdit();
                if (e.RowIndex >= -1)
                {

                    if (dgvData.Columns["Location"].Index == e.ColumnIndex)
                    {
                        e.Row.Cells["S"].Value = true;
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
