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

namespace StockControl
{
    public partial class WorkCentersDetail : Telerik.WinControls.UI.RadRibbonForm
    {
        int WorkId = 0;
        TypeAction tAction = TypeAction.Add;
        public WorkCentersDetail(int WorkId, TypeAction tAction)
        {
            InitializeComponent();
            this.WorkId = WorkId;
            this.tAction = tAction;
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

        private void Unit_Load(object sender, EventArgs e)
        {
            RMenu4.Click += RMenu4_Click;
            RMenu5.Click += RMenu5_Click;
            RMenu6.Click += RMenu6_Click;
            LoadDefualt();

            if (tAction != TypeAction.Add)
            {
                DataLoad();
            }

            if (tAction == TypeAction.Edit || tAction == TypeAction.Add)
                EditClick();
            else if (tAction == TypeAction.View)
                ViewClick();

        }

        private void RMenu6_Click(object sender, EventArgs e)
        {
            //Delete Process
            //throw new NotImplementedException();
            DeleteRow();
        }

        private void RMenu5_Click(object sender, EventArgs e)
        {
            //not do anything
        }

        private void RMenu4_Click(object sender, EventArgs e)
        {
            //Add Workcenter to process
            //throw new NotImplementedException();
            btnNew_Click(null, null);
        }
        void DeleteRow()
        {

        }


        private void LoadDefualt()
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var unit = db.mh_UnitTimes.ToList();
                    cbbUOM.DisplayMember = "Name";
                    cbbUOM.ValueMember = "id";
                    cbbUOM.DataSource = unit;

                    cbbCalendar.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    var cal = db.mh_Calendars.Where(x => x.Active).ToList();
                    cbbCalendar.DisplayMember = "Description";
                    cbbCalendar.ValueMember = "id";
                    cbbCalendar.DataSource = cal;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("Default", ex.Message, this.Name);
            }
        }
        private void DataLoad()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                var w = db.mh_WorkCenters.Where(x => x.id == WorkId).First();
                txtWorkId.Text = WorkId.ToSt();
                txtWorkNo.Text = w.WorkCenterNo;
                txtWorkName.Text = w.WorkCenterName.ToSt();
                cbbUOM.SelectedValue = w.UOM;
                txtCostPer.Value = w.CostPerUOM;
                txtCapa.Value = w.Capacity;
                cbbCalendar.SelectedValue = w.Calendar;
                txtDescription.Text = w.Decription;
            }
        }

        private bool AddUnit()
        {
            bool ck = false;
            int C = 0;
            try
            {

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    int idWork = txtWorkId.Text.ToInt();
                    string workNo = txtWorkNo.Text;
                    string workName = txtWorkName.Text.Trim();
                    int UOM = cbbUOM.SelectedValue.ToInt();
                    decimal costPer = txtCostPer.Value.ToDecimal();
                    decimal capa = txtCapa.Value.ToDecimal();
                    int cal = cbbCalendar.SelectedValue.ToInt();
                    string desc = txtDescription.Text;

                    var w = db.mh_WorkCenters.Where(x => x.id == idWork).FirstOrDefault();
                    if(w == null)
                    {
                        w = new mh_WorkCenter();
                        workNo = dbClss.GetNo(25, 2);
                    }
                    w.WorkCenterNo = workNo;
                    w.WorkCenterName = workName;
                    w.UOM = UOM;
                    w.CostPerUOM = costPer;
                    w.Capacity = capa;
                    w.Calendar = cal;
                    w.Decription = desc;

                    db.SubmitChanges();

                    WorkId = w.id;
                    C++;
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("Add Work center", ex.Message, this.Name);
            }

            if (C > 0)
            {
                MessageBox.Show("Save complete.!");
                DataLoad();
            }

            return ck;
        }
        private bool DeleteUnit()
        {
            bool ck = false;

            int C = 0;
            try
            {
                int idw = txtWorkId.Text.ToInt();
                if (idw <= 0) return false;
                if (MessageBox.Show("Do you want to Delete Work Center ?", "Delete Sub-Work", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    {                        
                        var t = db.mh_WorkCenters.Where(x => x.id == idw).ToList();
                        foreach(var tt in t)
                        {
                            tt.Active = false;
                            C++;
                        }
                        db.SubmitChanges();
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("Delete Work", ex.Message, this.Name);
            }

            if (C > 0)
            {
                row = row - 1;
                MessageBox.Show("Delete complete.!");
                DataLoad();
            }




            return ck;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DataLoad();
        }
        private void NewClick()
        {
            //btnEdit.Enabled = true;
            //btnView.Enabled = true;
        }
        private void EditClick()
        {
            btnEdit.Enabled = false;
            btnView.Enabled = true;

            txtWorkName.ReadOnly = false;
            cbbUOM.ReadOnly = false;
            txtCostPer.ReadOnly = false;
            txtCapa.ReadOnly = false;
            cbbCalendar.ReadOnly = false;
            txtDescription.ReadOnly = false;
        }
        private void ViewClick()
        {
            btnEdit.Enabled = true;
            btnView.Enabled = false;

            txtWorkName.ReadOnly = true;
            cbbUOM.ReadOnly = true;
            txtCostPer.ReadOnly = true;
            txtCapa.ReadOnly = true;
            cbbCalendar.ReadOnly = true;
            txtDescription.ReadOnly = true;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            NewClick();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            ViewClick();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditClick();
        }
        private bool Check_Save()
        {
            bool re = true;
            string err = "";
            try
            {
                if (txtWorkName.Text.Trim() == "")
                    err += "- Work Name is empty.\n";
                if (cbbUOM.SelectedValue.ToInt() <= 0)
                    err += "- Unit of Measure is empty.\n";
                if (txtCapa.Value.ToDecimal() <= 0)
                    err += "- Capacity is empty.\n";
                if (cbbCalendar.SelectedValue.ToInt() == 0)
                    err += "- Calendar is empty.\n";

                if (!err.Equals(""))
                    MessageBox.Show(err);
                else
                    re = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError(this.Name, ex.Message, this.Name);
            }

            return re;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Check_Save())
                return;

            if (MessageBox.Show("Do you want to Save ?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                AddUnit();
                DataLoad();
            }
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {

            DeleteUnit();
            //DataLoad();

        }

        int row = -1;

        private void btnExport_Click(object sender, EventArgs e)
        {
            //dbClss.ExportGridCSV(radGridView1);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "Spread Sheet files (*.csv)|*.csv|All files (*.csv)|*.csv";
            if (op.ShowDialog() == DialogResult.OK)
            {


                using (TextFieldParser parser = new TextFieldParser(op.FileName))
                {
                    dt.Rows.Clear();
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    int a = 0;
                    int c = 0;
                    while (!parser.EndOfData)
                    {
                        //Processing row
                        a += 1;
                        DataRow rd = dt.NewRow();
                        // MessageBox.Show(a.ToString());
                        string[] fields = parser.ReadFields();
                        c = 0;
                        foreach (string field in fields)
                        {
                            c += 1;
                            //TODO: Process field
                            //MessageBox.Show(field);
                            if (a > 1)
                            {
                                if (c == 1)
                                    rd["VendorNo"] = Convert.ToString(field);
                                else if (c == 2)
                                    rd["VendorName"] = Convert.ToString(field);
                                else if (c == 3)
                                    rd["Address"] = Convert.ToString(field);
                                else if (c == 4)
                                    rd["CRRNCY"] = Convert.ToString(field);
                                else if (c == 5)
                                    rd["Remark"] = Convert.ToString(field);
                                else if (c == 6)
                                    rd["Active"] = Convert.ToBoolean(field);

                            }
                            else
                            {
                                if (c == 1)
                                    rd["VendorNo"] = "";
                                else if (c == 2)
                                    rd["VendorName"] = "";
                                else if (c == 3)
                                    rd["Address"] = "";
                                else if (c == 4)
                                    rd["CRRNCY"] = "";
                                else if (c == 5)
                                    rd["Remark"] = "";
                                else if (c == 6)
                                    rd["Active"] = false;




                            }

                            //
                            //rd[""] = "";
                            //rd[""]
                        }
                        dt.Rows.Add(rd);

                    }
                }
                if (dt.Rows.Count > 0)
                {
                    dbClss.AddHistory(this.Name, "Import", "Import file CSV in to System", "");
                    ImportData();
                    MessageBox.Show("Import Completed.");

                    DataLoad();
                }

            }
        }

        private void ImportData()
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {

                    foreach (DataRow rd in dt.Rows)
                    {
                        if (!rd["VendorName"].ToString().Equals(""))
                        {

                            var x = (from ix in db.tb_Vendors where ix.VendorNo.ToLower().Trim() == rd["VendorNo"].ToString().ToLower().Trim() select ix).FirstOrDefault();
                            //var x = db.mh_Vendors.Where(x => x.No.ToLower() == rd["VendorNo"].ToSt().ToLower().Trim()).FirstOrDefault();

                            if (x == null)
                            {

                                tb_Vendor ts = new tb_Vendor();
                                ts.VendorNo = dbClss.GetNo(1, 2);
                                ts.VendorName = Convert.ToString(rd["VendorName"].ToString());
                                ts.Address = Convert.ToString(rd["Address"].ToString());
                                ts.CRRNCY = Convert.ToString(rd["CRRNCY"].ToString());
                                ts.Remark = Convert.ToString(rd["Remark"].ToString());
                                ts.Active = Convert.ToBoolean(rd["Active"].ToString());
                                db.tb_Vendors.InsertOnSubmit(ts);
                                db.SubmitChanges();

                            }
                            else
                            {
                                x.VendorName = Convert.ToString(rd["VendorName"].ToString());
                                x.Address = Convert.ToString(rd["Address"].ToString());
                                x.CRRNCY = Convert.ToString(rd["CRRNCY"].ToString());
                                x.Remark = Convert.ToString(rd["Remark"].ToString());
                                x.Active = Convert.ToBoolean(rd["Active"].ToString());
                                db.SubmitChanges();
                            }


                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("InportData", ex.Message, this.Name);
            }
        }

        private void btnFilter1_Click(object sender, EventArgs e)
        {
        }

        private void btnUnfilter1_Click(object sender, EventArgs e)
        {
        }

        private void radMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
