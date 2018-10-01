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
    public partial class WorkCentersSub : Telerik.WinControls.UI.RadRibbonForm
    {
        int WorkId = 0;
        TypeAction tAction = TypeAction.Add;
        public WorkCentersSub(int WorkId, TypeAction tAction)
        {
            InitializeComponent();
            this.WorkId = WorkId;
            this.tAction = tAction;
        }

        private void Unit_Load(object sender, EventArgs e)
        {
            RMenu4.Click += RMenu4_Click;
            RMenu5.Click += RMenu5_Click;
            RMenu6.Click += RMenu6_Click;
            dgvData.ReadOnly = true;
            dgvData.AutoGenerateColumns = false;
            LoadDefualt();

            if (tAction != TypeAction.Add)
            {
                DataLoad();
            }


            if (tAction == TypeAction.Add)
                NewClick();
            else if (tAction == TypeAction.Edit)
                EditClick();
            else if (tAction == TypeAction.View)
                ViewClick();
        }


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


        private void RMenu6_Click(object sender, EventArgs e)
        {
            //Delete Process
            //throw new NotImplementedException();
            DeleteRow();
        }
        void DeleteRow()
        {
            //if (dgvData.Rows.Count <= 1)
            //{
            //    baseClass.Warning("Cannot remove last Row.\n");
            //    return;
            //}

            if (dgvData.CurrentCell != null)
            {
                int id = dgvData.CurrentCell.RowInfo.Cells["id"].Value.ToInt();
                if (id > 0)
                {
                    using (var db = new DataClasses1DataContext())
                    {
                        var w = db.mh_WorkCenterSubs.Where(x => x.id == id).First();
                        w.Active = false;
                        db.SubmitChanges();

                        decimal sumCost = 0.00m;
                        decimal sumCapa = 0.00m;
                        int workId = txtWorkId.Text.ToInt();
                        if (WorkId > 0)
                        {
                            var ww = db.mh_WorkCenterSubs.Where(x => x.idWorkCenter == workId && x.Active).ToList();
                            foreach (var item in ww)
                            {
                                sumCost += item.CostPerUOM;
                                sumCapa += item.Capacity;
                            }

                            var whd = db.mh_WorkCenters.Where(x => x.id == workId).First();
                            whd.Capacity = sumCapa;
                            whd.CostPerUOM = sumCost;
                            db.SubmitChanges();
                        }
                    }
                }

                dgvData.Rows.Remove(dgvData.CurrentCell.RowInfo);

                baseClass.Info("Delete complete.");
            }

            setRowNo();
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


        void setRowNo()
        {
            dgvData.Rows.ToList().ForEach(x =>
            {
                x.Cells["No"].Value = x.Index + 1;
            });
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

                    var t = db.mh_WorkCenterTypes.ToList();
                    var com = dgvData.Columns["WType"] as GridViewComboBoxColumn;
                    com.DisplayMember = "WorkType";
                    com.ValueMember = "id";
                    com.DataSource = t;

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
                cbbCalendar.SelectedValue = w.Calendar;
                txtDescription.Text = w.Decription;

                var ws = db.mh_WorkCenterSubs.Where(x => x.idWorkCenter == WorkId && x.Active).ToList();
                dgvData.AutoGenerateColumns = false;
                dgvData.DataSource = null;
                dgvData.DataSource = ws;
            }

            setRowNo();
        }

        private bool AddUnit()
        {
            bool ck = false;
            int C = 0;
            try
            {
                dgvData.EndEdit();
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    //work Center
                    int idWork = txtWorkId.Text.ToInt();
                    string workNo = txtWorkNo.Text.Trim();
                    string workName = txtWorkName.Text.Trim();
                    int UOM = cbbUOM.SelectedValue.ToInt();
                    int Cal = cbbCalendar.SelectedValue.ToInt();
                    string Desc = txtDescription.Text.Trim();
                    bool newDoc = false;
                    var work = db.mh_WorkCenters.Where(x => x.id == idWork).FirstOrDefault();
                    if (work == null)
                    {
                        work = new mh_WorkCenter();
                        //workNo = dbClss.GetNo(25, 2);
                        dbClss.AddHistory(this.Name, "WorkCenter", $"New Work Center {workNo}", workNo);
                        newDoc = true;
                    }

                    if (!newDoc)
                    {
                        if (work.WorkCenterNo != workNo) dbClss.AddHistory(this.Name, "WorkCenter", $"Work Center No. from {work.WorkCenterNo} to {workNo}", workNo);
                        if (work.WorkCenterName != workName) dbClss.AddHistory(this.Name, "WorkCenter", $"Work Center Name from {work.WorkCenterName} to {workName}", workNo);
                        if (work.UOM != UOM) dbClss.AddHistory(this.Name, "WorkCenter", $"UOM from {work.UOM} to {UOM}", workNo);
                        if (work.Calendar != Cal) dbClss.AddHistory(this.Name, "WorkCenter", $"Calendar from {work.Calendar} to {Cal}", workNo);
                        if (work.Decription != Desc) dbClss.AddHistory(this.Name, "WorkCenter", $"Description from {work.Decription} to {Desc}", workNo);
                    }

                    work.WorkCenterNo = workNo;
                    work.WorkCenterName = workName;
                    work.UOM = UOM;
                    work.Calendar = Cal;
                    work.Decription = Desc;
                    work.Active = true;
                    if (idWork == 0)
                        db.mh_WorkCenters.InsertOnSubmit(work);
                    db.SubmitChanges();

                    idWork = work.id;
                    WorkId = work.id;


                    //Sub-Work Centers
                    decimal sumCapa = 0.00m;
                    decimal sumCost = 0.00m;
                    foreach (var item in dgvData.Rows)
                    {
                        newDoc = false;
                        decimal CostPerUOM = item.Cells["CostPerUOM"].Value.ToDecimal();
                        decimal Capacity = item.Cells["Capacity"].Value.ToDecimal();
                        sumCapa += Capacity;
                        sumCost += CostPerUOM;

                        //if (item.Cells["dgvC"].Value.ToSt() == "") continue;

                        int id = item.Cells["id"].Value.ToInt();
                        string SubWorkNo = item.Cells["SubWorkNo"].Value.ToSt();
                        //if (SubWorkNo == "")
                        //    SubWorkNo = dbClss.GetNo(27, 2);
                        string SubWorkName = item.Cells["SubWorkName"].Value.ToSt();
                        int WType = item.Cells["WType"].Value.ToInt();
                        string Description = item.Cells["Description"].Value.ToSt();
                        bool active = true;

                        var ws = db.mh_WorkCenterSubs.Where(x => x.id == id).FirstOrDefault();
                        if (ws == null)
                        {
                            ws = new mh_WorkCenterSub();
                            newDoc = true;
                            dbClss.AddHistory(this.Name, "WorkCenter", $"New Sub-Workcenter from {SubWorkNo}", workNo);
                        }

                        if (!newDoc)
                        {
                            if (SubWorkNo != ws.SubWorkNo) dbClss.AddHistory(this.Name, "WorkCenter", $"Sub-Work No from {ws.SubWorkNo} to {SubWorkNo}", workNo);
                            if (SubWorkName != ws.SubWorkName) dbClss.AddHistory(this.Name, "WorkCenter", $"Sub-Work Name from {ws.SubWorkName} to {SubWorkName}", workNo);
                            if (UOM != ws.UOM) dbClss.AddHistory(this.Name, "WorkCenter", $"Sub-UOM from {ws.UOM} to {UOM}", workNo);
                            if (WType != ws.WType) dbClss.AddHistory(this.Name, "WorkCenter", $"Work Center Type from {ws.WType} to {WType}", workNo);
                            if (CostPerUOM != ws.CostPerUOM) dbClss.AddHistory(this.Name, "WorkCenter", $"Cost:UOM from {ws.CostPerUOM} to {CostPerUOM}", workNo);
                            if (Capacity != ws.Capacity) dbClss.AddHistory(this.Name, "WorkCenter", $"Capacity from {ws.Capacity} to {Capacity}", workNo);
                            if (Description != ws.Description) dbClss.AddHistory(this.Name, "WorkCenter", $"Description from {ws.Description} to {Description}", workNo);
                        }

                        ws.idWorkCenter = idWork;
                        ws.SubWorkName = SubWorkName;
                        ws.SubWorkNo = SubWorkNo;
                        ws.UOM = UOM;
                        ws.WType = WType;
                        ws.Active = active;
                        ws.CostPerUOM = CostPerUOM;
                        ws.Capacity = Capacity;
                        ws.Description = Description;

                        if (id == 0)
                            db.mh_WorkCenterSubs.InsertOnSubmit(ws);
                        db.SubmitChanges();
                        C++;
                    }

                    //sum Capacity & Cost Per
                    var w = db.mh_WorkCenters.Where(x => x.id == WorkId).FirstOrDefault();
                    if (w != null && dgvData.Rows.Count > 0)
                    {
                        w.CostPerUOM = sumCost;
                        w.Capacity = sumCapa;
                        db.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("Add Sub-Work center", ex.Message, this.Name);
            }

            if (C > 0)
                MessageBox.Show("Save complete.!");

            return ck;
        }
        private bool DeleteUnit()
        {
            bool ck = false;

            int C = 0;
            try
            {

                dgvData.EndEdit();
                int wid = txtWorkId.Text.ToInt();
                if (wid > 0)
                {
                    if (MessageBox.Show("Do you want to Delete Work Center ?", "Delete Sub-Work", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        using (DataClasses1DataContext db = new DataClasses1DataContext())
                        {
                            var m = db.mh_WorkCenters.Where(x => x.id == wid).First();
                            m.Active = false;
                            db.SubmitChanges();
                            C++;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("Delete Work Centers", ex.Message, this.Name);
            }

            if (C > 0)
            {
                //row = row - 1;
                MessageBox.Show("Delete complete.!");
                DataLoad();
            }




            return ck;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DataLoad();
        }
        void setEnabled(bool s)
        {
            s = !s;
            txtWorkName.ReadOnly = s;
            cbbUOM.ReadOnly = s;
            cbbCalendar.ReadOnly = s;
            txtDescription.ReadOnly = s;
            dgvData.ReadOnly = s;
        }
        private void NewClick()
        {
            dgvData.ReadOnly = false;
            dgvData.AllowAddNewRow = false;
            btnEdit.Enabled = true;
            btnView.Enabled = true;
            var rowe = dgvData.Rows.AddNew();
            rowe.Cells["No"].Value = rowe.Index + 1;
            setEnabled(true);

        }
        private void EditClick()
        {
            dgvData.ReadOnly = false;
            btnEdit.Enabled = false;
            btnView.Enabled = true;
            dgvData.AllowAddNewRow = false;

            txtWorkNo.ReadOnly = false;
            txtWorkName.ReadOnly = false;
            cbbUOM.ReadOnly = false;

            setEnabled(true);
        }
        private void ViewClick()
        {
            dgvData.ReadOnly = true;
            btnView.Enabled = false;
            btnEdit.Enabled = true;
            dgvData.AllowAddNewRow = false;

            txtWorkNo.ReadOnly = true;
            txtWorkName.ReadOnly = true;
            cbbUOM.ReadOnly = true;
            cbbUOM.SelectedIndex = -1;

            setEnabled(false);
            DataLoad();
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

                dgvData.EndEdit();

                using (var db = new DataClasses1DataContext())
                {
                    if (txtWorkNo.Text == "")
                        err += "- Work id is empty.\n";
                    else
                    {
                        int idw = txtWorkId.Text.ToInt();
                        string workcode = txtWorkNo.Text;
                        var d = db.mh_WorkCenters.Where(x => x.id != idw && x.Active && x.WorkCenterNo == workcode).ToList();
                        if (d.Count > 0)
                        {
                            err += "- Work center no. is dupplicate.\n";
                        }
                    }


                    if (dgvData.Rows.Count <= 0)
                        err += "- Sub-Work is empty.\n";
                    if (txtWorkName.Text.Trim() == "")
                        err += "- Work name is empty.\n";
                    if (cbbCalendar.SelectedValue.ToInt() <= 0)
                        err += "- Calendar is empty.\n";
                    if (cbbUOM.SelectedValue.ToInt() <= 0)
                        err += "- Unit of Measure is empty.\n";

                    foreach (var e in dgvData.Rows)
                    {
                        int ids = e.Cells["id"].Value.ToInt();
                        string subworkno = e.Cells["SubWorkNo"].Value.ToSt();
                        if (subworkno == "")
                            err += "- Sub-Work no is empty.\n";
                        else
                        {
                            var sb = db.mh_WorkCenterSubs.Where(x => x.id != ids && x.Active && x.SubWorkNo == subworkno).ToList();
                            if (sb.Count > 0)
                            {
                                err += "- Sub-Work No is dupplicate.\n";
                                break;
                            }
                        }
                        if (e.Cells["SubWorkName"].Value.ToSt().Trim() == "")
                            err += "- Sub-Work Name is empty.\n";
                        if (e.Cells["WType"].Value.ToInt() == 0)
                            err += "- Work Type is empty.\n";
                        if (e.Cells["Capacity"].Value.ToDecimal() <= 0)
                            err += "- Capacity is empty.\n";



                        if (err != "")
                            break;
                    }
                }


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

            if (MessageBox.Show("ต้องการบันทึก ?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                AddUnit();
                DataLoad();
            }
        }

        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                dgvData.Rows[e.RowIndex].Cells["dgvC"].Value = "T";
                //string check1 = Convert.ToString(radGridView1.Rows[e.RowIndex].Cells["VendorName"].Value);
                //string TM= Convert.ToString(radGridView1.Rows[e.RowIndex].Cells["dgvCodeTemp2"].Value);
                //if (!check1.Trim().Equals("") && TM.Equals(""))
                //{

                //    if (!CheckDuplicate(check1.Trim()))
                //    {
                //        MessageBox.Show("ชื้อผู้ขายซ้ำ ซ้ำ");
                //        radGridView1.Rows[e.RowIndex].Cells["GroupCode"].Value = "";
                //        radGridView1.Rows[e.RowIndex].Cells["GroupCode"].IsSelected = true;

                //    }
                //}


            }
            catch (Exception ex) { }
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {

            DeleteUnit();
            setRowNo();
            //DataLoad();

        }

        int row = -1;
        private void radGridView1_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            row = e.RowIndex;


        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            //dbClss.ExportGridCSV(radGridView1);
            dbClss.ExportGridXlSX(dgvData);
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
            if (dgvData.CurrentCell != null)
            {
                throw new NotImplementedException();

                this.Cursor = Cursors.WaitCursor;

                var ct = new WorkingDay(dgvData.Rows[row].Cells["Code"].Value.ToInt());
                this.Cursor = Cursors.Default;
                ct.ShowDialog();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();
            }
        }

        private void btnHolidays_Click(object sender, EventArgs e)
        {

            if (row >= 0)
            {
                this.Cursor = Cursors.WaitCursor;
                //Contact ct = new Contact(Convert.ToString(radGridView1.Rows[row].Cells["VendorNo"].Value),
                //    Convert.ToString(radGridView1.Rows[row].Cells["VendorName"].Value));
                var ct = new Holiday(dgvData.Rows[row].Cells["Code"].Value.ToInt());
                this.Cursor = Cursors.Default;
                ct.ShowDialog();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();
            }
        }

        private void dgvData_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                e.Row.Cells["dgvC"].Value = "T";
            }
        }
    }
}
