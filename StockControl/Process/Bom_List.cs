using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;

namespace StockControl
{
    public partial class Bom_List : Telerik.WinControls.UI.RadRibbonForm
    {
        public Bom_List(string CodeNox)
        {
            InitializeComponent();
            CodeNo = CodeNox;
            //this.Text = "ประวัติ "+ Screen;
        }
        Telerik.WinControls.UI.RadTextBox CodeNo_tt = new Telerik.WinControls.UI.RadTextBox();
        Telerik.WinControls.UI.RadTextBox BomNo_tt = new Telerik.WinControls.UI.RadTextBox();

        int screen = 0;
        string TypePart = "";
        public Bom_List(Telerik.WinControls.UI.RadTextBox  CodeNox
              , Telerik.WinControls.UI.RadTextBox BomNox
            , string TypePartx)
        {
            InitializeComponent();
            CodeNo_tt = CodeNox;
            BomNo_tt = BomNox;
            TypePart = TypePartx;
            screen = 1;
        }
        public Bom_List()
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

            dtDateFrom.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
            dtDateTo.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
            Set_dt_Print();
            //radGridView1.ReadOnly = true;
            radGridView1.AutoGenerateColumns = false;

            txtCodeNo.Text = CodeNo_tt.Text;
            txtBomNo.Text = BomNo_tt.Text;

            DataLoad();
        }
        private void Set_FindData()
        {
            if (TypePart == "All" || TypePart == "")
            {
                ddlTypePart.Items.Add("");
                ddlTypePart.Items.Add("FG");
                ddlTypePart.Items.Add("WIP");
                ddlTypePart.Items.Add("RM");

                ddlTypePart.Text = "";
            }
            else if (TypePart == "WIP-RM")
            {
                ddlTypePart.Items.Add("WIP");
                ddlTypePart.Items.Add("RM");

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
            if(screen ==1)
            {
                radButtonElement1.Text = "เลือก Bom";
                radRibbonBarGroup1.Text = "Select Bom";
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
                  
                   
                    string DTBegin_s = "";
                    string DTEnd_s = "";

                    if (cbDate.Checked)
                    {
                        if (dtDateFrom.Text == "" || dtDateTo.Text == "")
                        {
                            DTBegin_s = DateTime.Today.ToString("yyyyMMdd", new CultureInfo("en-US"));
                            DTEnd_s = DateTime.Today.ToString("yyyyMMdd", new CultureInfo("en-US"));
                        }
                        else
                        {
                            DTBegin_s = dtDateFrom.Value.ToString("yyyyMMdd", new CultureInfo("en-US"));
                            DTEnd_s = dtDateTo.Value.ToString("yyyyMMdd", new CultureInfo("en-US"));
                        }
                    }

                    var g = (from ix in db.sp_017_Select_BomHD(txtBomNo.Text,txtCodeNo.Text,"","","",ddlTypePart.Text,"", DTBegin_s, DTEnd_s) select ix)
                        //.Where(a => a.VendorNo.Contains(txtBomNo.Text)
                        //&& (a.Status != "Cancel")
                        ////&& a.TEMPNo.Contains(txtTempNo.Text)
                        //&& a.PRNo.Contains(txtCodeNo.Text)
                        ////&& a.VendorName.Contains(txtVendorName.Text)
                        //&& (a.CreateDate >= inclusiveStart
                        //        && a.CreateDate < exclusiveEnd)
                        // )
                        .ToList();
                    if (g.Count > 0)
                    {
                        radGridView1.DataSource = g;
                        foreach (var x in radGridView1.Rows)
                        {
                            c += 1;
                            x.Cells["No"].Value = c;
                        }
                    }
                    
                       
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;


            //    radGridView1.DataSource = dt;
        }
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
            try
            {
                if (screen.Equals(1))
                {

                    if (radGridView1.Rows.Count <= 0)
                        return;

                    CodeNo_tt.Text = Convert.ToString(radGridView1.CurrentRow.Cells["PartNo"].Value);
                    BomNo_tt.Text = Convert.ToString(radGridView1.CurrentRow.Cells["BomNo"].Value);
                    this.Close();
                }
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    Bom sc = new Bom();
                    this.Cursor = Cursors.Default;
                    sc.ShowDialog();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                    ClassLib.Memory.Heap();
                }
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
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
                    if (screen.Equals(1))
                    {
                        CodeNo_tt.Text = Convert.ToString(e.Row.Cells["PartNo"].Value);
                        BomNo_tt.Text = Convert.ToString(e.Row.Cells["BomNo"].Value);
                        this.Close();
                    }
                    else
                    {
                        Bom a = new Bom(Convert.ToString(e.Row.Cells["PartNo"].Value)
                            , Convert.ToString(e.Row.Cells["BomNo"].Value)
                            );
                        a.ShowDialog();
                        //this.Close();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
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
            try
            {
                dt_Kanban.Rows.Clear();

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var g = (from ix in db.tb_Items select ix).Where(a => a.CodeNo == txtBomNo.Text).ToList();
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
                string BomNo = "";
                string PartNo = "";

                if (radGridView1.Rows.Count <= 0)
                    return;

                BomNo = StockControl.dbClss.TSt(radGridView1.CurrentRow.Cells["BomNo"].Value);
                PartNo = StockControl.dbClss.TSt(radGridView1.CurrentRow.Cells["PartNo"].Value);


                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var g = (from ix in db.sp_R015_Report_Bom(PartNo, BomNo, "", "", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"))) select ix).ToList();
                    if (g.Count() > 0)
                    {

                        Report.Reportx1.Value = new string[4];
                        Report.Reportx1.Value[0] = PartNo;
                        Report.Reportx1.Value[1] = BomNo;
                        Report.Reportx1.Value[2] = "";
                        Report.Reportx1.Value[3] = "";
                        Report.Reportx1.WReport = "Bom";
                        Report.Reportx1 op = new Report.Reportx1("Bom.rpt");
                        op.Show();

                    }
                    else
                        MessageBox.Show("not found.");
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
    }
}
