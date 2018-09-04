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
    public partial class RoutingDetail : Telerik.WinControls.UI.RadRibbonForm
    {
        string RoutingNo = "";
        TypeAction tAction = TypeAction.Add;
        public RoutingDetail(string RoutingNo, TypeAction tAction)
        {
            InitializeComponent();
            this.RoutingNo = RoutingNo;
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

            if (tAction != TypeAction.Add)
            {
                DataLoad();
            }

            LoadDefualt();
            MyFont = new System.Drawing.Font(
                        "Tahoma", 9,
                        FontStyle.Italic,    // + obviously doesn't work, but what am I meant to do?
                        GraphicsUnit.Pixel);


            if (tAction == TypeAction.Add)
                NewClick();
            else if (tAction == TypeAction.Edit)
                EditClick();
            else if (tAction == TypeAction.View)
                ViewClick();
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
                Contact ct = new Contact(Convert.ToString(dgvData.Rows[row].Cells["VendorNo"].Value),
                    Convert.ToString(dgvData.Rows[row].Cells["VendorName"].Value));
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

                    var t = db.mh_RoutingTypes.ToList();
                    var com = dgvData.Columns["RoutingType"] as GridViewComboBoxColumn;
                    com.DisplayMember = "Name";
                    com.ValueMember = "id";
                    com.DataSource = t;
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

                var g = db.mh_Routings.Where(x => x.RoutingNo == RoutingNo && x.Active).OrderBy(x => x.RNo).ToList();
                DataTable dt2 = ClassLib.Classlib.LINQToDataTable(g);
                dgvData.DataSource = null;
                dgvData.DataSource = dt2;
                int ck = 0;
                foreach (var x in dgvData.Rows)
                {
                    if (row >= 0 && row == ck)
                    {
                        x.ViewInfo.CurrentRow = x;
                    }
                    ck += 1;
                }

                if (g.Count > 0)
                {
                    txtRoutingNo.Text = g.First().RoutingNo;
                    txtRoutingName.Text = g.First().RoutingName;
                    cbbUOM.SelectedValue = g.First().RoutingUOM;
                }
            }

            setRowNo();


            //    radGridView1.DataSource = dt;
        }
        private bool CheckDuplicate(string code)
        {
            bool ck = false;

            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                int i = (from ix in db.mh_GroupTypes where ix.GroupCode == code select ix).Count();
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

                string RoutingNo = txtRoutingNo.Text.Trim();
                string RoutingName = txtRoutingName.Text.Trim();
                int UOM = cbbUOM.SelectedValue.ToInt();
                if (RoutingNo == "")
                    RoutingNo = dbClss.GetNo(26, 2);
                txtRoutingNo.Text = RoutingNo;
                this.RoutingNo = RoutingNo;

                dgvData.EndEdit();
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    foreach (var g in dgvData.Rows)
                    {
                        if (Convert.ToString(g.Cells["dgvC"].Value).Equals("T"))
                        {
                            if (Convert.ToString(g.Cells["dgvCodeTemp"].Value).Equals(""))
                            {
                                var t = new mh_Routing();
                                t.RoutingNo = RoutingNo;
                                t.RoutingName = RoutingName;
                                t.RoutingUOM = UOM;
                                t.RoutingType = g.Cells["RoutingType"].Value.ToInt();
                                t.Description = g.Cells["Description"].Value.ToSt();
                                t.SetupTime = g.Cells["SetupTime"].Value.ToDecimal();
                                t.RunTime = g.Cells["RunTime"].Value.ToDecimal();
                                t.WaitTime = g.Cells["WaitTime"].Value.ToDecimal();
                                t.UnitCostPer = g.Cells["UnitCostPer"].Value.ToDecimal();
                                t.RNo = g.Cells["No"].Value.ToInt();

                                dbClss.AddHistory(this.Name, "เพิ่ม Routing", $"เพิ่ม Routing [{t.RoutingNo}]", "");
                                //dbClss.AddHistory(this.Name, "เพิ่มผู้ขาย", "เพิ่มผู้ขาย [" + gy.VendorName + "]", "");
                                db.mh_Routings.InsertOnSubmit(t);
                                db.SubmitChanges();
                                C += 1;
                            }
                            else
                            {
                                var t = db.mh_Routings.Where(x => x.id == g.Cells["dgvCodeTemp"].Value.ToInt()).First();
                                t.RoutingName = RoutingName;
                                t.RoutingUOM = UOM;
                                t.RoutingType = g.Cells["RoutingType"].Value.ToInt();
                                t.Description = g.Cells["Description"].Value.ToSt();
                                t.SetupTime = g.Cells["SetupTime"].Value.ToDecimal();
                                t.RunTime = g.Cells["RunTime"].Value.ToDecimal();
                                t.WaitTime = g.Cells["WaitTime"].Value.ToDecimal();
                                t.UnitCostPer = g.Cells["UnitCostPer"].Value.ToDecimal();
                                t.RNo = g.Cells["No"].Value.ToInt();

                                C += 1;
                                db.SubmitChanges();
                                dbClss.AddHistory(this.Name, "แก้ไข Routing", $"แก้ไข Routing [{t.RoutingNo}]", "");

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("เพิ่ม Routing", ex.Message, this.Name);
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
                    string CodeTemp = Convert.ToString(dgvData.Rows[row].Cells["dgvCodeTemp"].Value);
                    dgvData.EndEdit();
                    if (MessageBox.Show("ต้องการลบรายการหรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        using (DataClasses1DataContext db = new DataClasses1DataContext())
                        {
                            if (!CodeTemp.Equals(""))
                            {
                                var t = db.mh_Routings.Where(x => x.id == CodeTemp.ToInt()).ToList();
                                foreach (var d in t)
                                {
                                    //db.mh_Routings.DeleteOnSubmit(d);
                                    d.Active = false;
                                    dbClss.AddHistory(this.Name, "ลบ Routing", $"Delete Routing [{d.RoutingNo}]", "");
                                }
                                C += 1;

                                db.SubmitChanges();
                                dgvData.Rows.Remove(dgvData.Rows[row]);
                            }
                            else
                            {
                                dgvData.Rows.Remove(dgvData.Rows[row]);
                            }

                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("ลบ Routing", ex.Message, this.Name);
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
            dgvData.ReadOnly = false;
            dgvData.AllowAddNewRow = false;
            btnEdit.Enabled = false;
            btnView.Enabled = true;
            var rowe = dgvData.Rows.AddNew();
            rowe.Cells["No"].Value = rowe.Index + 1;

        }
        private void EditClick()
        {
            dgvData.ReadOnly = false;
            btnEdit.Enabled = false;
            btnView.Enabled = true;
            dgvData.AllowAddNewRow = false;

            txtRoutingName.ReadOnly = false;
            cbbUOM.ReadOnly = false;
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
                if (dgvData.Rows.Count <= 0)
                    err += "- “รายการ:” เป็นค่าว่าง \n";
                // int c = 0;
                if (txtRoutingName.Text.Trim() == "")
                    err += "- “Routing Name.:” เป็นค่าว่างไม่ได้ \n";
                if (cbbUOM.SelectedValue.ToInt() == 0)
                    err += "- “Unit of Measure.:” เป็นค่าว่างไม่ได้ \n";

                foreach (var g in dgvData.Rows)
                {
                    if (g.IsVisible)
                    {
                        if (Convert.ToString(g.Cells["RoutingType"].Value).Equals(""))
                            err += "- “Type.:” เป็นค่าว่างไม่ได้ \n";
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
            setRowNo();
            //DataLoad();

        }

        int row = -1;
        private void radGridView1_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            row = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                if (dgvData.Rows[e.RowIndex].Cells["dgvCodeTemp"].Value.ToInt() > 0)
                {
                    btnHolidays.Enabled = true;
                    btnWorkingDays.Enabled = true;
                }
                else
                {
                    btnHolidays.Enabled = false;
                    btnWorkingDays.Enabled = false;
                }
            }
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
            if (e.CellElement.ColumnInfo.Name == "Code")
            {
                e.CellElement.DrawFill = true;
                // e.CellElement.ForeColor = Color.Blue;
                e.CellElement.NumberOfColors = 1;
                e.CellElement.BackColor = Color.WhiteSmoke;
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
                //Contact ct = new Contact(Convert.ToString(radGridView1.Rows[row].Cells["VendorNo"].Value),
                //    Convert.ToString(radGridView1.Rows[row].Cells["VendorName"].Value));
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



    }
}
