﻿using System;
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
    public partial class RoutingDetail : Telerik.WinControls.UI.RadRibbonForm
    {
        int RoutingId = 0;
        TypeAction tAction = TypeAction.Add;
        public RoutingDetail(int RoutingId, TypeAction tAction)
        {
            InitializeComponent();
            this.RoutingId = RoutingId;
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
        private void GETDTRow()
        {
            dt.Columns.Add("No", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Address", typeof(string));
            dt.Columns.Add("VATRegistration", typeof(bool));
            dt.Columns.Add("PhoneNo", typeof(string));
            dt.Columns.Add("FaxNo", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("ContactName", typeof(string));
            dt.Columns.Add("VendorGroup", typeof(int));
            dt.Columns.Add("VatGroup", typeof(int));
            dt.Columns.Add("DefaultCurrency", typeof(int));
            dt.Columns.Add("ShippingTime", typeof(int));
            dt.Columns.Add("PriceIncludeingVat", typeof(bool));
            dt.Columns.Add("ReceiveAddress", typeof(string));
            dt.Columns.Add("AttachFile", typeof(string));

        }
        System.Drawing.Font MyFont;
        private void Unit_Load(object sender, EventArgs e)
        {
            RMenu4.Click += RMenu4_Click;
            RMenu5.Click += RMenu5_Click;
            RMenu6.Click += RMenu6_Click;
            dgvData.ReadOnly = true;
            dgvData.AutoGenerateColumns = false;
            GETDTRow();
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

        private void RMenu6_Click(object sender, EventArgs e)
        {
            //Delete Process
            //throw new NotImplementedException();
            DeleteRow();
        }
        void DeleteRow()
        {
            if (dgvData.CurrentCell != null)
            {
                int id = dgvData.CurrentCell.RowInfo.Cells["dgvCodetemp"].Value.ToInt();
                if (id > 0)
                {
                    using (var db = new DataClasses1DataContext())
                    {
                        var w = db.mh_RoutingDTs.Where(x => x.id == id).First();
                        w.Active = false;
                        db.SubmitChanges();
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
            NewClick();
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


                    var ww = dgvData.Columns["WorkCenter"] as GridViewMultiComboBoxColumn;
                    var tt = db.mh_WorkCenters.Where(x => x.Active).Select(x => new { x.id, x.WorkCenterNo, x.WorkCenterName }).ToList();
                    ww.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    ww.DisplayMember = "WorkCenterNo";
                    ww.ValueMember = "id";
                    ww.DataSource = tt;

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
            //dt.Rows.Clear();
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {

                var g = db.mh_Routings.Where(x => x.id == RoutingId && x.Active).FirstOrDefault();
                txtid.Text = g.id.ToSt();
                txtRoutingNo.Text = g.RoutingNo;
                txtRoutingName.Text = g.RoutingName.ToSt();
                cbbUOM.SelectedValue = g.RoutingUOM;
                txtDescription.Text = g.Description;

                var dt = db.mh_RoutingDTs.Where(x => x.RoutingId == RoutingId && x.Active).OrderBy(x => x.RNo).ToList();
                DataTable dt2 = ClassLib.Classlib.LINQToDataTable(dt);
                dgvData.DataSource = null;
                dgvData.DataSource = dt2;

            }

            setRowNo();


            //    radGridView1.DataSource = dt;
        }

        private bool AddUnit()
        {
            bool ck = false;
            int C = 0;
            try
            {
                int Rid = txtid.Text.ToInt();
                string RoutingNo = txtRoutingNo.Text.Trim();
                string RoutingName = txtRoutingName.Text.Trim();
                int UOM = cbbUOM.SelectedValue.ToInt();
                string Description = txtDescription.Text.Trim();

                dgvData.EndEdit();
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    //HD
                    var hd = db.mh_Routings.Where(x => x.id == Rid).FirstOrDefault();
                    if (hd == null)
                    {
                        hd = new mh_Routing();
                        db.mh_Routings.InsertOnSubmit(hd);
                    }
                    hd.RoutingNo = RoutingNo;
                    hd.RoutingName = RoutingName;
                    hd.Description = Description;
                    hd.RoutingUOM = UOM;
                    hd.Active = true;
                    db.SubmitChanges();
                    txtid.Text = hd.id.ToSt();
                    RoutingId = hd.id;

                    //DT
                    foreach (var g in dgvData.Rows)
                    {
                        if (g.Cells["dgvC"].Value.ToSt().Equals("T"))
                        {
                            int id = g.Cells["dgvCodetemp"].Value.ToInt();
                            var t = db.mh_RoutingDTs.Where(x => x.id == id).FirstOrDefault();
                            if (t == null)
                            {
                                t = new mh_RoutingDT();
                                db.mh_RoutingDTs.InsertOnSubmit(t);
                            }
                            //var t = new mh_RoutingDT();
                            t.RoutingId = RoutingId;
                            t.RNo = g.Cells["No"].Value.ToInt();
                            t.idWorkCenter = g.Cells["WorkCenter"].Value.ToInt();
                            t.Description = g.Cells["Description"].Value.ToSt();
                            t.SetupTime = g.Cells["SetupTime"].Value.ToDecimal();
                            t.RunTime = g.Cells["RunTime"].Value.ToDecimal();
                            t.WaitTime = g.Cells["WaitTime"].Value.ToDecimal();
                            t.UnitCost = g.Cells["UnitCost"].Value.ToDecimal();
                            t.Active = true;

                            db.SubmitChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("Add Routing", ex.Message, this.Name);
            }

            MessageBox.Show("Save complete.!");

            return ck;
        }
        private bool DeleteUnit()
        {
            bool ck = false;

            int C = 0;
            try
            {
                string CodeTemp = txtRoutingNo.Text.Trim();
                dgvData.EndEdit();
                if (MessageBox.Show("Do you want to Delete Routing ?", "Delete Routing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    {
                        if (!CodeTemp.Equals(""))
                        {
                            var t = db.mh_Routings.Where(x => x.RoutingNo == CodeTemp).ToList();
                            foreach (var d in t)
                            {
                                //db.mh_Routings.DeleteOnSubmit(d);
                                d.Active = false;
                                dbClss.AddHistory(this.Name, "Delete Routing", $"Delete Routing [{d.RoutingNo}]", "");
                            }
                            C += 1;

                            db.SubmitChanges();
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("Delete Routing", ex.Message, this.Name);
            }

            if (C > 0)
            {
                row = row - 1;
                MessageBox.Show("Delete complete.!");
            }




            return ck;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DataLoad();
        }
        private void NewClick()
        {
            dgvData.ReadOnly = false;
            dgvData.AllowAddNewRow = false;
            btnEdit.Enabled = false;
            btnView.Enabled = true;
            var rowe = dgvData.Rows.AddNew();
            rowe.Cells["No"].Value = rowe.Index + 1;
            rowe.Cells["RunTime"].Value = 1;

        }
        private void EditClick()
        {
            dgvData.ReadOnly = false;
            btnEdit.Enabled = false;
            btnView.Enabled = true;
            dgvData.AllowAddNewRow = false;

            txtRoutingName.ReadOnly = false;
            cbbUOM.ReadOnly = false;
            txtRoutingNo.ReadOnly = false;
        }
        private void ViewClick()
        {
            dgvData.ReadOnly = true;
            btnView.Enabled = false;
            btnEdit.Enabled = true;
            dgvData.AllowAddNewRow = false;

            txtRoutingName.ReadOnly = true;
            cbbUOM.ReadOnly = true;
            cbbUOM.SelectedIndex = -1;
            txtRoutingNo.ReadOnly = true;

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

                if (txtRoutingNo.Text == "")
                    err += "- Routing No is empty.\n";
                else
                {
                    using (var db = new DataClasses1DataContext())
                    {
                        int id = txtid.Text.ToInt();
                        string routingNo = txtRoutingNo.Text.Trim();
                        var m = db.mh_Routings.Where(x => x.id != id && x.Active && x.RoutingNo == routingNo).ToList();
                        if (m.Count > 0)
                        {
                            err += "- Routing No. is Dupplicate.\n";
                        }
                    }
                }
                if (txtRoutingName.Text.Trim() == "")
                    err += "- Routing Name is empty.\n";
                if (cbbUOM.SelectedValue.ToInt() == 0)
                    err += "- Unit of Measure is empty.\n";
                foreach (var ee in dgvData.Rows)
                {
                    if (ee.Cells["WorkCenter"].Value.ToInt() == 0)
                        err += "- Work center is empty.\n";

                    if (err != "")
                        break;
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

            if (MessageBox.Show("Do you want to Save ?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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

                if (e.Column.Name.Equals("WorkCenter"))
                {
                    int id = e.Value.ToInt();
                    using (var db = new DataClasses1DataContext())
                    {
                        var t = db.mh_WorkCenters.Where(x => x.id == id).FirstOrDefault();
                        if (t != null)
                            e.Row.Cells["Description"].Value = t.WorkCenterName;
                    }
                }


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

        private void MasterTemplate_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            if (e.Column.Name.Equals("WorkCenter"))
            {
                RadMultiColumnComboBoxElement a = e.ActiveEditor as RadMultiColumnComboBoxElement;
                a.EditorControl.Columns["id"].IsVisible = false;
                a.EditorControl.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                a.EditorControl.Columns["WorkCenterNo"].MaxWidth = 120;
                a.EditorControl.ShowRowHeaderColumn = false;
                a.DropDownMinSize = new Size(300, 200);
                a.DropDownSizingMode = SizingMode.UpDownAndRightBottom;
            }
        }
    }
}
