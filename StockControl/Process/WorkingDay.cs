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
    public partial class WorkingDay : Telerik.WinControls.UI.RadRibbonForm
    {
        public WorkingDay()
        {
            InitializeComponent();
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
            RMenu3.Click += RMenu3_Click;
            RMenu4.Click += RMenu4_Click;
            RMenu5.Click += RMenu5_Click;
            RMenu6.Click += RMenu6_Click;
            radGridView1.ReadOnly = true;
            radGridView1.AutoGenerateColumns = false;
            GETDTRow();
            //for (int i = 0; i <= RowView; i++)
            //{
            //    DataRow rd = dt.NewRow();
            //    rd["UnitCode"] = "";
            //    rd["UnitDetail"] = "";
            //    rd["UnitActive"] = false;
            //    dt.Rows.Add(rd);
            //}


            DataLoad();
            LoadDefualt();
            MyFont = new System.Drawing.Font(
    "Tahoma", 9,
    FontStyle.Italic,    // + obviously doesn't work, but what am I meant to do?
    GraphicsUnit.Pixel);
        }

        private void RMenu6_Click(object sender, EventArgs e)
        {
            //ลบ
            //throw new NotImplementedException();
            btnDelete_Click(sender, e);
        }

        private void RMenu5_Click(object sender, EventArgs e)
        {
            btnEdit_Click(sender, e);
        }

        private void RMenu4_Click(object sender, EventArgs e)
        {
            ////เพิ่มผู้ขาย
            //throw new NotImplementedException();
            btnNew_Click(sender, e);
        }

        private void RMenu3_Click(object sender, EventArgs e)
        {
            //เพิ่มผุ้ติดต่อ
            if (row >= 0)
            {


                this.Cursor = Cursors.WaitCursor;
                Contact ct = new Contact(Convert.ToString(radGridView1.Rows[row].Cells["VendorNo"].Value),
                    Convert.ToString(radGridView1.Rows[row].Cells["VendorName"].Value));
                this.Cursor = Cursors.Default;
                ct.ShowDialog();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();
            }
        }

        private void RadMenuItem3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("menu");
            // throw new NotImplementedException();
        }

        Dictionary<int, string> DayOfWeek = new Dictionary<int, string>();
        private void LoadDefualt()
        {
            try
            {
                DayOfWeek.Clear();
                DayOfWeek.Add(0, "Monday");
                DayOfWeek.Add(1, "Tueday");
                DayOfWeek.Add(2, "Wednesday");
                DayOfWeek.Add(3, "Thursday");
                DayOfWeek.Add(4, "Friday");
                DayOfWeek.Add(5, "Saturday");
                DayOfWeek.Add(6, "Sunday");

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    //var gt = (from ix in db.mh_CRRNCies select ix).ToList();
                    //GridViewComboBoxColumn comboBoxColumn = this.radGridView1.Columns["DefaultCrrncy"] as GridViewComboBoxColumn;
                    //comboBoxColumn.DisplayMember = "CRRNCY";
                    //comboBoxColumn.ValueMember = "id";
                    //comboBoxColumn.DataSource = gt;

                    var com = radGridView1.Columns["Day"] as GridViewComboBoxColumn;
                    com.DisplayMember = "Value";
                    com.ValueMember = "Key";
                    com.DataSource = DayOfWeek;
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

                var g = db.mh_WorkingDays.ToList();
                DataTable dt2 = ClassLib.Classlib.LINQToDataTable(g);
                radGridView1.DataSource = dt2;
                int ck = 0;
                foreach (var x in radGridView1.Rows)
                {
                    if (row >= 0 && row == ck)
                    {
                        x.ViewInfo.CurrentRow = x;
                    }
                    ck += 1;
                }
            }


            //    radGridView1.DataSource = dt;
        }
        private bool CheckDuplicate(string code)
        {
            bool ck = false;

            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                int i = (from ix in db.tb_GroupTypes where ix.GroupCode == code select ix).Count();
                if (i > 0)
                    ck = false;
                else
                    ck = true;
            }
            return ck;
        }

        private bool AddUnit()
        {
            bool ck = false;
            int C = 0;
            try
            {


                radGridView1.EndEdit();
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    foreach (var g in radGridView1.Rows)
                    {
                        if (Convert.ToString(g.Cells["dgvC"].Value).Equals("T"))
                        {
                            if (Convert.ToString(g.Cells["dgvCodeTemp"].Value).Equals(""))
                            {
                                var t = new mh_WorkingDay();
                                t.Day = g.Cells["Day"].Value.ToInt();
                                t.EndingTime = g.Cells["EndTime"].Value.ToSt();
                                t.NoOfWorkHours = g.Cells["WorkHours"].Value.ToDecimal();
                                t.ShiftCode = g.Cells["ShiftCode"].Value.ToSt();
                                t.StartingTime = g.Cells["StartTime"].Value.ToSt();
                                
                                dbClss.AddHistory(this.Name, "เพิ่มวันทำงาน", "เพิ่มวันทำงาน [" + t.Day + $":{t.ShiftCode}]", "");
                                //dbClss.AddHistory(this.Name, "เพิ่มผู้ขาย", "เพิ่มผู้ขาย [" + gy.VendorName + "]", "");
                                db.mh_WorkingDays.InsertOnSubmit(t);
                                db.SubmitChanges();
                                C += 1;
                            }
                            else
                            {
                                var t = db.mh_WorkingDays.Where(x => x.id == g.Cells["dgvCodeTemp"].Value.ToInt()).First();
                                t.Day = g.Cells["Day"].Value.ToInt();
                                t.EndingTime = g.Cells["EndTime"].Value.ToSt();
                                t.NoOfWorkHours = g.Cells["WorkHours"].Value.ToDecimal();
                                t.ShiftCode = g.Cells["ShiftCode"].Value.ToSt();
                                t.StartingTime = g.Cells["StartTime"].Value.ToSt();

                                C += 1;
                                db.SubmitChanges();
                                dbClss.AddHistory(this.Name, "แก้ไขวันทำงาน", "แก้ไขวันทำงาน [" + t.Day + $":{t.ShiftCode}]", "");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("เพิ่มวันทำงาน", ex.Message, this.Name);
            }

            if (C > 0)
                MessageBox.Show("บันทึกสำเร็จ!");

            return ck;
        }
        private bool DeleteUnit()
        {
            bool ck = false;

            int C = 0;
            try
            {

                if (row >= 0)
                {
                    string CodeDelete = radGridView1.Rows[row].Cells["Day"].Value.ToSt(); //Convert.ToString(radGridView1.Rows[row].Cells["VendorNo"].Value);
                    string CodeTemp = Convert.ToString(radGridView1.Rows[row].Cells["dgvCodeTemp"].Value);
                    radGridView1.EndEdit();
                    if (MessageBox.Show("ต้องการลบรายการ ( " + CodeDelete + " ) หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        using (DataClasses1DataContext db = new DataClasses1DataContext())
                        {

                            if (!CodeDelete.Equals(""))
                            {
                                if (!CodeTemp.Equals(""))
                                {
                                    //var unit1 = db.mh_Vendors.Where(x => x.No == CodeDelete).ToList();
                                    //foreach (var d in unit1)
                                    //{
                                    //    db.mh_Vendors.DeleteOnSubmit(d);
                                    //    dbClss.AddHistory(this.Name, "ลบผู้ขาย", "Delete Vendor [" + d.Name + "]", "");
                                    //}
                                    var t = db.mh_WorkingDays.Where(x => x.id == CodeTemp.ToInt()).ToList();
                                    foreach(var d in t)
                                    {
                                        db.mh_WorkingDays.DeleteOnSubmit(d);
                                        dbClss.AddHistory(this.Name, "ลบวันทำงาน", $"Delete Working Day [{d.Day}:{d.ShiftCode}]", "");
                                    }
                                    C += 1;



                                    db.SubmitChanges();
                                }
                            }

                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("ลบวันทำงาน", ex.Message, this.Name);
            }

            if (C > 0)
            {
                row = row - 1;
                MessageBox.Show("ลบรายการ สำเร็จ!");
            }




            return ck;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DataLoad();
        }
        private void NewClick()
        {
            radGridView1.ReadOnly = false;
            radGridView1.AllowAddNewRow = false;
            btnEdit.Enabled = false;
            btnView.Enabled = true;
            radGridView1.Rows.AddNew();
        }
        private void EditClick()
        {
            radGridView1.ReadOnly = false;
            btnEdit.Enabled = false;
            btnView.Enabled = true;
            radGridView1.AllowAddNewRow = false;
        }
        private void ViewClick()
        {
            radGridView1.ReadOnly = true;
            btnView.Enabled = false;
            btnEdit.Enabled = true;
            radGridView1.AllowAddNewRow = false;
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

                radGridView1.EndEdit();
                if (radGridView1.Rows.Count <= 0)
                    err += "- “รายการ:” เป็นค่าว่าง \n";
                // int c = 0;

                foreach (var g in radGridView1.Rows)
                {
                    if (g.IsVisible)
                    {
                        if (Convert.ToString(g.Cells["Day"].Value).Equals(""))
                            err += "- “วัน:” เป็นค่าว่างไม่ได้ \n";
                        if (Convert.ToString(g.Cells["WorkHours"].Value).Equals(""))
                            err += "- “จำนวนชม.:” เป็นค่าว่างไม่ได้ \n";
                        if (Convert.ToString(g.Cells["ShiftCode"].Value).Equals(""))
                            err += "- “กะทำงาน:” เป็นค่าว่างไม่ได้ \n";
                        if (Convert.ToString(g.Cells["StartTime"].Value).Equals(""))
                            err += "- “เวลาเริ่ม:” เป็นค่าว่างไม่ได้ \n";
                        if (Convert.ToString(g.Cells["EndTime"].Value).Equals(""))
                            err += "- “เวลาสิ้นสุด:” เป็นค่าว่างไม่ได้ \n";
                        if(g.Cells["StartTime"].Value.ToSt().Contains("_"))
                            err += "- “เวลาเริ่ม:” กรอกข้อมูลไม่ถูกต้อง \n";
                        if (g.Cells["EndTime"].Value.ToSt().Contains("_"))
                            err += "- “เวลาสิ้นสุด:” กรอกข้อมูลไม่ถูกต้อง \n";
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
                radGridView1.Rows[e.RowIndex].Cells["dgvC"].Value = "T";
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

        private void Unit_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // MessageBox.Show(e.KeyCode.ToString());
        }

        private void radGridView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // MessageBox.Show(e.KeyCode.ToString());

            if (e.KeyData == (Keys.Control | Keys.S))
            {
                btnSave_Click(null, null);
                //if (MessageBox.Show("ต้องการบันทึก ?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //{
                //    AddUnit();
                //    DataLoad();
                //}
            }
            else if (e.KeyData == (Keys.Control | Keys.N))
            {
                if (MessageBox.Show("ต้องการสร้างใหม่ ?", "สร้างใหม่", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    NewClick();
                }

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            DeleteUnit();
            DataLoad();

        }

        int row = -1;
        private void radGridView1_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            row = e.RowIndex;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            //dbClss.ExportGridCSV(radGridView1);
            dbClss.ExportGridXlSX(radGridView1);
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

        private void MasterTemplate_CellValueChanged(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            //if(radGridView1.Columns["VendorName"].Index==e.ColumnIndex
            //     || radGridView1.Columns["VendorName"].Index == e.ColumnIndex
            //     || radGridView1.Columns["Address"].Index == e.ColumnIndex
            //     || radGridView1.Columns["CRRNCY"].Index == e.ColumnIndex
            //     || radGridView1.Columns["Remark"].Index == e.ColumnIndex
            //     )
            // {

            // }
            if (e.Column.Name == "DefaultCrrncy")
            {
                var ee = e.Value;
            }
            else if (e.Column.Name == "VendorGroup")
            {
                var ee = e.Value;
                var eee = e.Value.ToInt();
            }
        }

        private void MasterTemplate_RowFormatting(object sender, RowFormattingEventArgs e)
        {
            //if (e.RowElement.RowInfo.Cells["VendorNo"].Value == null)
            //{
            //    e.RowElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
            //    e.RowElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
            //    e.RowElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
            //}
            //else if(!e.RowElement.RowInfo.Cells["VendorNo"].Value.Equals(""))
            //{ 
            //    e.RowElement.DrawFill = true;
            //    // e.RowElement.GradientStyle = GradientStyles.Solid;
            //    e.RowElement.BackColor = Color.WhiteSmoke;

            //}
            //else
            //{
            //    e.RowElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
            //    e.RowElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
            //    e.RowElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
            //}
        }

        private void MasterTemplate_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement.ColumnInfo.HeaderText == "รหัสผู้ขาย")
            {
                if (e.CellElement.RowInfo.Cells["VendorNo"].Value != null)
                {
                    if (!e.CellElement.RowInfo.Cells["VendorNo"].Value.Equals(""))
                    {
                        e.CellElement.DrawFill = true;
                        // e.CellElement.ForeColor = Color.Blue;
                        e.CellElement.NumberOfColors = 1;
                        e.CellElement.BackColor = Color.WhiteSmoke;
                    }
                    else
                    {
                        e.CellElement.DrawFill = true;
                        //e.CellElement.ForeColor = Color.Yellow;
                        e.CellElement.NumberOfColors = 1;
                        e.CellElement.BackColor = Color.WhiteSmoke;
                    }
                }
            }
            else if (e.CellElement.ColumnInfo.HeaderText == "ผู้ติดต่อ"
                || e.CellElement.ColumnInfo.HeaderText == "เบอร์โทร"
                || e.CellElement.ColumnInfo.HeaderText == "เบอร์แฟกซ์"
                || e.CellElement.ColumnInfo.HeaderText == "อีเมล์"
                )
            {
                if (e.CellElement.RowInfo.Cells["ContactName"].Value != null
                    || e.CellElement.RowInfo.Cells["PhoneNo"].Value != null
                    || e.CellElement.RowInfo.Cells["FaxNo"].Value != null
                    || e.CellElement.RowInfo.Cells["Email"].Value != null)
                {
                    e.CellElement.DrawFill = true;
                    // e.CellElement.ForeColor = Color.Blue;
                    e.CellElement.NumberOfColors = 1;
                    e.CellElement.BackColor = Color.WhiteSmoke;
                    //if (!e.CellElement.RowInfo.Cells["ContactName"].Value.Equals("")
                    //    )
                    //{
                    //    e.CellElement.DrawFill = true;
                    //    // e.CellElement.ForeColor = Color.Blue;
                    //    e.CellElement.NumberOfColors = 1;
                    //    e.CellElement.BackColor = SystemColors.ButtonHighlight;
                    //}
                    //else
                    //{
                    //    //e.CellElement.DrawFill = true;
                    //    ////e.CellElement.ForeColor = Color.Yellow;
                    //    //e.CellElement.NumberOfColors = 1;
                    //    //e.CellElement.BackColor = Color.WhiteSmoke;
                    //}
                }
            }
            else
            {
                e.CellElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(LightVisualElement.ForeColorProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(LightVisualElement.NumberOfColorsProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
            }
        }

        private void radButtonElement1_Click(object sender, EventArgs e)
        {

            if (row >= 0)
            {


                this.Cursor = Cursors.WaitCursor;
                Contact ct = new Contact(Convert.ToString(radGridView1.Rows[row].Cells["VendorNo"].Value),
                    Convert.ToString(radGridView1.Rows[row].Cells["VendorName"].Value));
                this.Cursor = Cursors.Default;
                ct.ShowDialog();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();
            }
        }
    }
}
