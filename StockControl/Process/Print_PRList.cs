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
using System.IO;
using OfficeOpenXml;
using Telerik.WinControls.Data;

namespace StockControl
{
    public partial class Print_PRList : Telerik.WinControls.UI.RadRibbonForm
    {
        public Print_PRList(string PR1xx, string PR2xx, string Typexx)
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
        public Print_PRList(Telerik.WinControls.UI.RadTextBox CodeNox)
        {
            InitializeComponent();
            CodeNo_tt = CodeNox;
            screen = 1;
        }
        public Print_PRList()
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
                //cbDate.Visible = false;
                //dtDate1.Visible = false;
                //dtDate2.Visible = false;
                //lblToDate.Visible = false;

                this.cboGroupType.AutoFilter = true;
                FilterDescriptor filter = new FilterDescriptor();
                filter.PropertyName = this.cboGroupType.DisplayMember;
                filter.Operator = FilterOperator.Contains;
                this.cboGroupType.AutoCompleteMode = AutoCompleteMode.Append;
                this.cboGroupType.EditorControl.MasterTemplate.FilterDescriptors.Add(filter);
                this.cboGroupType.BestFitColumns();
                cboGroupType.DisplayMember = "GroupCode";
                cboGroupType.ValueMember = "GroupCode";
                cboGroupType.DataSource = db.mh_GroupTypes.Where(s => s.GroupActive == true).ToList();
                cboGroupType.BestFitColumns();

                if (Type == "ListPurchaseAndPR")
                {
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
            //if (Type.Equals("ReceiveMonth"))
            //{
            //    Export_ReceiveMonth();
            //}
            //////  dbClss.ExportGridCSV(radGridView1);
            ////dbClss.ExportGridXlSX(radGridView1);
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
                    if (Type.Equals("ListPurchaseAndPR"))
                    {
                        Report.Reportx1.Value = new string[4];

                        Report.Reportx1.Value[0] = dtDate1.Value.Date.ToString("dd/MMM/yyyy");
                        Report.Reportx1.Value[1] = dtDate2.Value.Date.ToString("dd/MMM/yyyy");
                        Report.Reportx1.Value[2] = cboGroupType.Text;
                        Report.Reportx1.Value[3] = ddlStatus.Text;

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
