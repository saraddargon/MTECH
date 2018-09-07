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
    public partial class CustomerContacts : Telerik.WinControls.UI.RadRibbonForm
    {
        TypeAction tAction = TypeAction.Add;
        public CustomerContacts()
        {
            InitializeComponent();
        }
        public CustomerContacts(string CustomerNo, TypeAction tAction)
        {
            InitializeComponent();
            txtNo.Text = CustomerNo;
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
            //dt.Columns.Add(new DataColumn("VendorNo", typeof(string)));
            //dt.Columns.Add(new DataColumn("VendorName", typeof(string)));
            //dt.Columns.Add(new DataColumn("Address", typeof(string)));
            //dt.Columns.Add(new DataColumn("CRRNCY", typeof(string)));
            //dt.Columns.Add(new DataColumn("Remark", typeof(string)));
            //dt.Columns.Add(new DataColumn("Active", typeof(bool)));
            //dt.Columns.Add(new DataColumn("ContactName", typeof(string)));
            //dt.Columns.Add(new DataColumn("Tel", typeof(string)));
            //dt.Columns.Add(new DataColumn("Fax", typeof(string)));
            //dt.Columns.Add(new DataColumn("email", typeof(string)));
            dt.Columns.Add("No", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Address", typeof(string));
            dt.Columns.Add("VATRegistration", typeof(bool));
            dt.Columns.Add("PhoneNo", typeof(string));
            dt.Columns.Add("FaxNo", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("ContactName", typeof(string));
            dt.Columns.Add("CreditLimit", typeof(decimal));
            dt.Columns.Add("CustomerGroup", typeof(int));
            dt.Columns.Add("VatGroup", typeof(int));
            dt.Columns.Add("DefaultCurrency", typeof(int));
            dt.Columns.Add("ShippingTime", typeof(int));
            dt.Columns.Add("PriceIncludeingVat", typeof(bool));
            dt.Columns.Add("ShippingAddress", typeof(string));
            dt.Columns.Add("AttachFile", typeof(string));

        }

        private void Unit_Load(object sender, EventArgs e)
        {
            dgvData.ReadOnly = true;
            dgvData.AutoGenerateColumns = false;
            GETDTRow();
            
            DataLoad();
            LoadDefualt();
        }
        private void LoadDefualt()
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var gt = (from ix in db.mh_CRRNCies select ix).ToList();
                    var comboBoxColumn = cbbCurrency;
                    comboBoxColumn.DisplayMember = "CRRNCY";
                    comboBoxColumn.ValueMember = "id";
                    comboBoxColumn.DataSource = gt;


                    var com1 = cbbCustomerGroup;
                    com1.DisplayMember = "Value";
                    com1.ValueMember = "id";
                    com1.DataSource = db.mh_CustomerGroups.ToList();

                    var com2 = cbbVatGroup;
                    com2.DisplayMember = "Value";
                    com2.ValueMember = "id";
                    com2.DataSource = db.mh_VatGroups.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("CRRNCY", ex.Message, this.Name);
            }
        }

        private void DataLoad()
        {
            //dt.Rows.Clear();
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                if (txtNo.Text != "" && tAction != TypeAction.Add)
                {
                    string cstmNo = txtNo.Text;
                    var g = db.mh_Customers.Where(x => x.No == cstmNo).FirstOrDefault();
                    txtName.Text = g.Name;
                    txtAddress.Text = g.Address;
                    txtShippingAddress.Text = g.ShippingAddress;
                    txtCreditLimit.Value = g.CreditLimit;
                    cbbCustomerGroup.SelectedValue = g.CustomerGroup;
                    txtShippingTime.Value = g.ShippingTime;
                    txtAttachFile.Value = g.AttachFile;
                    cbbVatGroup.SelectedValue = g.VatGroup;
                    cbbCurrency.SelectedValue = g.DefaultCurrency;
                    cbVatRegis.Checked = g.VATRegistration;
                    cbPriceIncVat.Checked = g.PriceIncludeingVat;
                    cbActive.Checked = g.Active;
                    var m = db.mh_CustomerContacts.Where(w => w.CustomerNo == cstmNo && w.Active).ToList();
                    dgvData.AutoGenerateColumns = false;
                    dgvData.DataSource = null;
                    dgvData.DataSource = m;

                    setEdit(false);
                }
                else
                    setEdit(true);
            }


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

        void setEdit(bool ss)
        {
            txtName.ReadOnly = !ss;
            txtAddress.ReadOnly = !ss;
            txtCreditLimit.ReadOnly = !ss;
            txtShippingTime.ReadOnly = !ss;
            txtAttachFile.ReadOnly = !ss;
            btnDel.Enabled = ss;
            cbVatRegis.ReadOnly = !ss;
            cbPriceIncVat.ReadOnly = !ss;
            txtShippingAddress.ReadOnly = !ss;
            cbbCustomerGroup.ReadOnly = !ss;
            cbbVatGroup.ReadOnly = !ss;
            cbbCurrency.ReadOnly = !ss;
            cbActive.ReadOnly = !ss;
            dgvData.ReadOnly = !ss;
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
                    string cstmno = txtNo.Text.Trim();
                    var cstm = new mh_Customer();
                    if (tAction == TypeAction.Add)
                        cstmno = dbClss.GetNo(24, 2);
                    else
                        cstm = db.mh_Customers.Where(x => x.No == cstmno).FirstOrDefault();
                    txtNo.Text = cstmno;
                    cstm.No = cstmno;
                    cstm.Name = txtName.Text.Trim();
                    cstm.Address = txtAddress.Text.Trim();
                    cstm.CreditLimit = txtCreditLimit.Value.ToDecimal();
                    cstm.ShippingTime = txtShippingTime.Value.ToInt();
                    cstm.AttachFile = txtAttachFile.Value.ToSt();
                    cstm.VATRegistration = cbVatRegis.Checked;
                    cstm.PriceIncludeingVat = cbPriceIncVat.Checked;
                    cstm.ShippingAddress = txtShippingAddress.Text.Trim();
                    cstm.CustomerGroup = cbbCustomerGroup.SelectedValue.ToInt();
                    cstm.VatGroup = cbbVatGroup.SelectedValue.ToInt();
                    cstm.DefaultCurrency = cbbCurrency.SelectedValue.ToInt();
                    cstm.Active = cbActive.Checked;
                    if (tAction == TypeAction.Add)
                        db.mh_Customers.InsertOnSubmit(cstm);
                    db.SubmitChanges();

                    foreach (var c in dgvData.Rows)
                    {
                        int id = c.Cells["id"].Value.ToInt();
                        var con = db.mh_CustomerContacts.Where(x => x.id == id).FirstOrDefault();
                        if (con == null)
                            con = new mh_CustomerContact();
                        con.Def = c.Cells["Def"].Value.ToBool();
                        con.CustomerNo = cstmno;
                        con.ContactName = c.Cells["ContactName"].Value.ToSt();
                        con.Tel = c.Cells["Tel"].Value.ToSt();
                        con.Fax = c.Cells["Fax"].Value.ToSt();
                        con.Email = c.Cells["Email"].Value.ToSt();
                        if (id <= 0)
                            db.mh_CustomerContacts.InsertOnSubmit(con);
                        db.SubmitChanges();
                    }
                }

                baseClass.Info("Save complete.");
                DataLoad();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("เพิ่มผู้ซื้อ", ex.Message, this.Name);
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
                    string CodeDelete = Convert.ToString(dgvData.Rows[row].Cells["VendorNo"].Value);
                    string CodeTemp = Convert.ToString(dgvData.Rows[row].Cells["dgvCodeTemp"].Value);
                    dgvData.EndEdit();
                    if (MessageBox.Show("ต้องการลบรายการ ( " + CodeDelete + " ) หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        using (DataClasses1DataContext db = new DataClasses1DataContext())
                        {

                            if (!CodeDelete.Equals(""))
                            {
                                if (!CodeTemp.Equals(""))
                                {

                                    //var unit1 = (from ix in db.tb_Vendors
                                    //             where ix.VendorNo == CodeDelete
                                    //             select ix).ToList();
                                    //foreach (var d in unit1)
                                    //{
                                    //    db.tb_Vendors.DeleteOnSubmit(d);
                                    //    dbClss.AddHistory(this.Name, "ลบผู้ขาย", "Delete Vendor [" + d.VendorName + "]", "");
                                    //}
                                    var unit1 = db.mh_Customers.Where(x => x.No == CodeDelete).ToList();
                                    foreach (var d in unit1)
                                    {
                                        db.mh_Customers.DeleteOnSubmit(d);
                                        dbClss.AddHistory(this.Name, "ลบผู้ซื้อ", "Delete Customer [" + d.Name + "]", "");
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
                dbClss.AddError("ลบผู้ซื้อ", ex.Message, this.Name);
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
            setEdit(true);
            dgvData.Rows.AddNew();
        }
        private void EditClick()
        {
            dgvData.ReadOnly = false;
            btnEdit.Enabled = false;
            btnView.Enabled = true;
            dgvData.AllowAddNewRow = false;
            setEdit(true);
            //tAction = TypeAction.Edit;
        }
        private void ViewClick()
        {
            dgvData.ReadOnly = true;
            btnView.Enabled = false;
            btnEdit.Enabled = true;
            dgvData.AllowAddNewRow = false;
            setEdit(false);
            //tAction = TypeAction.View;
            //DataLoad();
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
                //if (dgvData.Rows.Count <= 0)
                //    err += "- “รายการ:” เป็นค่าว่าง \n";
                // int c = 0;

                if (txtName.Text.Trim().Equals(""))
                    err += "- Customer name is empty.\n";
                if (cbbCustomerGroup.SelectedValue.ToInt() == 0)
                    err += "- Customer Group is empty.\n";
                if (cbbVatGroup.SelectedValue.ToInt() == 0)
                    err += "- Vat Group is empty.\n";
                if (cbbCurrency.SelectedValue.ToInt() == 0)
                    err += "- Currency is empty.\n";


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
        
        private void radButtonElement1_Click(object sender, EventArgs e)
        {

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

        private void radGridView1_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            //EditClick();
        }
    }
}
