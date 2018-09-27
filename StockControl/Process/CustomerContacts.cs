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
        public CustomerContacts(int idCust, TypeAction tAction)
        {
            InitializeComponent();
            txtid.Text = idCust.ToSt();
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

            LoadDefualt();
            DataLoad();
            if (tAction == TypeAction.View)
            {
                ViewClick();
            }
            else if (tAction == TypeAction.Edit)
            {
                EditClick();
            }
            else
            {
                setEdit(true);
            }
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
                if (txtid.Text.ToInt() != 0 && tAction != TypeAction.Add)
                {
                    int id = txtid.Text.ToInt();
                    var g = db.mh_Customers.Where(x => x.id == id).FirstOrDefault();
                    txtid.Text = g.id.ToSt();
                    txtNo.Text = g.No.ToSt();
                    txtBranchCOde.Text = g.BranchCode;
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
                    var m = db.mh_CustomerContacts.Where(w => w.idCustomer == id && w.Active).ToList();
                    dgvData.AutoGenerateColumns = false;
                    dgvData.DataSource = null;
                    dgvData.DataSource = m;

                    if (tAction == TypeAction.View)
                        setEdit(false);
                    else
                        setEdit(true);
                }
                else
                    setEdit(true);
            }

            txtNo.ReadOnly = tAction != TypeAction.Add;
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
            //txtNo.ReadOnly = !ss;
            txtBranchCOde.ReadOnly = !ss;
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
                    bool newDoc = false;
                    int id = txtid.Text.ToInt();
                    var cstm = db.mh_Customers.Where(x => x.id == id).FirstOrDefault();
                    if(cstm == null)
                    {
                        newDoc = true;
                        cstm = new mh_Customer();
                        db.mh_Customers.InsertOnSubmit(cstm);

                        dbClss.AddHistory(this.Name, "Customer Contact", $"New Customer {txtNo.Text}", txtNo.Text);
                    }

                    if (!newDoc)
                    {
                        //dbClss.AddHistory(this.Name, "Customer Contact", $" {} to {}", cstm.No);
                        if(cstm.BranchCode != txtBranchCOde.Text) dbClss.AddHistory(this.Name, "Customer Contact", $"Branchcode {cstm.BranchCode} to {txtBranchCOde.Text}", cstm.No);
                        if(cstm.No != txtNo.Text) dbClss.AddHistory(this.Name, "Customer Contact", $"Customer No {cstm.No} to {txtNo.Text}", cstm.No);
                        if(cstm.Name != txtName.Text) dbClss.AddHistory(this.Name, "Customer Contact", $"Customer Name {cstm.Name} to {txtName.Text}", cstm.No);
                        if(cstm.Address != txtAddress.Text) dbClss.AddHistory(this.Name, "Customer Contact", $"Address {cstm.Address} to {txtAddress.Text}", cstm.No);
                        if(cstm.CreditLimit != txtCreditLimit.Value.ToDecimal()) dbClss.AddHistory(this.Name, "Customer Contact", $"Credit Limit {cstm.CreditLimit} to {txtCreditLimit.Value.ToDecimal()}", cstm.No);
                        if(cstm.ShippingTime != txtShippingTime.Value.ToInt()) dbClss.AddHistory(this.Name, "Customer Contact", $"Shipping Time {cstm.ShippingTime} to {txtShippingTime.Value.ToInt()}", cstm.No);
                        if(cstm.AttachFile != txtAttachFile.Value.ToSt()) dbClss.AddHistory(this.Name, "Customer Contact", $"Attach file {cstm.AttachFile} to {txtAttachFile.Value.ToSt()}", cstm.No);
                        if(cstm.VATRegistration != cbVatRegis.Checked) dbClss.AddHistory(this.Name, "Customer Contact", $"Vat Regis. {cstm.VATRegistration} to {cbVatRegis.Checked}", cstm.No);
                        if(cstm.PriceIncludeingVat != cbPriceIncVat.Checked) dbClss.AddHistory(this.Name, "Customer Contact", $"Price Inc. Vat {cstm.PriceIncludeingVat} to {cbPriceIncVat.Checked}", cstm.No);
                        if(cstm.ShippingAddress != txtShippingAddress.Text) dbClss.AddHistory(this.Name, "Customer Contact", $"Shipping Address {cstm.ShippingAddress} to {txtShippingAddress.Text}", cstm.No);
                        if(cstm.CustomerGroup != cbbCustomerGroup.SelectedValue.ToInt()) dbClss.AddHistory(this.Name, "Customer Contact", $"Customer Group {cstm.CustomerGroup} to {cbbCustomerGroup.SelectedValue.ToInt()}", cstm.No);
                        if(cstm.VatGroup != cbbVatGroup.SelectedValue.ToInt()) dbClss.AddHistory(this.Name, "Customer Contact", $"Vat Group {cstm.VatGroup} to {cbbVatGroup.SelectedValue.ToInt()}", cstm.No);
                        if(cstm.DefaultCurrency != cbbCurrency.SelectedValue.ToInt()) dbClss.AddHistory(this.Name, "Customer Contact", $"Currency {cstm.DefaultCurrency} to {cbbCurrency.SelectedValue.ToInt()}", cstm.No);
                        if(cstm.Active != cbActive.Checked) dbClss.AddHistory(this.Name, "Customer Contact", $"Status Customer {cbActive.Checked}", cstm.No);
                    }

                    cstm.BranchCode = txtBranchCOde.Text;
                    cstm.No = txtNo.Text.Trim();
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
                    db.SubmitChanges();

                    foreach (var c in dgvData.Rows)
                    {
                        if (c.Cells["dgvC"].Value.ToSt() == "") continue;

                        bool newC = false;
                        int idDt = c.Cells["id"].Value.ToInt();
                        var con = db.mh_CustomerContacts.Where(x => x.id == id).FirstOrDefault();
                        if (con == null)
                        {
                            newC = true;
                            con = new mh_CustomerContact();
                            db.mh_CustomerContacts.InsertOnSubmit(con);
                            dbClss.AddHistory(this.Name, "Customer Contact", $"New Contact {c.Cells["ContactName"].Value.ToSt()}", cstm.No);
                        }

                        if (!newC)
                        {

                        }

                        con.idCustomer = cstm.id;
                        con.Def = c.Cells["Def"].Value.ToBool();
                        con.id = 0;
                        con.ContactName = c.Cells["ContactName"].Value.ToSt();
                        con.Tel = c.Cells["Tel"].Value.ToSt();
                        con.Fax = c.Cells["Fax"].Value.ToSt();
                        con.Email = c.Cells["Email"].Value.ToSt();
                        con.Active = true;
                        db.SubmitChanges();
                    }

                    tAction = TypeAction.Edit;
                    DataLoad();

                    if (dgvData.Rows.Count == 1)
                    {
                        int idDt = dgvData.Rows[0].Cells["id"].Value.ToInt();
                        dgvData.Rows[0].Cells["Def"].Value = true;
                        var m = db.mh_CustomerContacts.Where(x => x.id == idDt).FirstOrDefault();
                        if(m != null)
                            m.Def = true;
                        db.SubmitChanges();
                    }
                }

                baseClass.Info("Save complete.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("เพิ่มผู้ซื้อ", ex.Message, this.Name);
            }

            //if (C > 0)
            //    MessageBox.Show("บันทึกสำเร็จ!");

            return ck;
        }
        private bool DeleteUnit()
        {
            bool ck = false;

            int C = 0;
            try
            {
                int id = txtid.Text.ToInt();
                using (var db = new DataClasses1DataContext())
                {
                    var cstm = db.mh_Customers.Where(x => x.id == id).FirstOrDefault();
                    if(cstm != null)
                    {
                        cstm.Active = false;
                        db.SubmitChanges();
                    }

                    baseClass.Info("Delete complete.\n");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("Delete Cusomter Contact", ex.Message, this.Name);
            }
            


            return ck;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DataLoad();
        }
        private void NewClick()
        {
            if (tAction != TypeAction.View)
            {
                dgvData.ReadOnly = false;
                dgvData.AllowAddNewRow = false;
                btnEdit.Enabled = false;
                btnView.Enabled = true;
                btnSave.Enabled = true;
                setEdit(true);
                if (txtNo.Text != "")
                    lblStatus.Text = "Edit";
                else
                    lblStatus.Text = "New";
                dgvData.Rows.AddNew();
            }
        }
        private void EditClick()
        {
            dgvData.ReadOnly = false;
            btnEdit.Enabled = false;
            btnView.Enabled = true;
            btnSave.Enabled = true;
            dgvData.AllowAddNewRow = false;
            setEdit(true);
            lblStatus.Text = "Edit";
            //tAction = TypeAction.Edit;
        }
        private void ViewClick()
        {
            dgvData.ReadOnly = true;
            btnView.Enabled = false;
            btnEdit.Enabled = true;
            btnSave.Enabled = false;
            dgvData.AllowAddNewRow = false;
            setEdit(false);
            lblStatus.Text = "View";
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

                if (txtNo.Text.Trim().Equals(""))
                    err += "- Customer no is empty.\n";
                else
                {
                    using (var db = new DataClasses1DataContext())
                    {
                        string cno = txtNo.Text.Trim();
                        int id = txtid.Text.ToInt();
                        var cstm = db.mh_Customers.Where(x => x.No == cno && x.Active && x.id != id).ToList();
                        if (cstm.Count > 0)
                            err += "- Customer no. is dupplication.\n";
                    }
                }
                if (txtName.Text.Trim().Equals(""))
                    err += "- Customer name is empty.\n";
                if (txtBranchCOde.Text.Trim().Equals(""))
                    err += "- Branch code is empty.\n";
                if (txtAddress.Text.Trim().Equals(""))
                    err += "- Address is empty.\n";
                if (txtShippingAddress.Text.Trim().Equals(""))
                    err += "- Shipping Address is empty.\n";
                if (cbbCustomerGroup.SelectedValue.ToInt() == 0)
                    err += "- Customer Group is empty.\n";
                if (cbbVatGroup.SelectedValue.ToInt() == 0)
                    err += "- Vat Group is empty.\n";
                if (cbbCurrency.SelectedValue.ToInt() == 0)
                    err += "- Currency is empty.\n";
                if(dgvData.Rows.Count == 0)
                {
                    err += "- Contact data is empty.\n";
                }
                else
                {
                    if (dgvData.Rows.Where(x => x.Cells["Def"].Value.ToBool()).Count() == 0)
                        err += "- Not set Default Contact.\n";
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
                EditClick();
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

        private void dgvData_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.Column.Name.Equals("Def"))
                {
                    if (!e.Row.Cells["Def"].Value.ToBool())
                    {
                        dgvData.Rows.Where(x => x.Index != e.RowIndex).ToList().ForEach(x =>
                          {
                              x.Cells["Def"].Value = false;
                              x.Cells["dgvC"].Value = "T";
                          });
                    }

                }
            }
        }

        private void btnDeleteRow_Click(object sender, EventArgs e)
        {
            dgvData.EndEdit();

            if (dgvData.CurrentCell == null) return;
            var rowe = dgvData.CurrentCell.RowInfo;

            if (rowe.Cells["Def"].Value.ToBool())
            {
                baseClass.Warning("Cannot Remove Default Contact.\n");
                return;
            }

            int idDt = rowe.Cells["id"].Value.ToInt();
            if (idDt > 0)
            {
                using (var db = new DataClasses1DataContext())
                {
                    var m = db.mh_CustomerContacts.Where(x => x.id == idDt).FirstOrDefault();
                    if(m != null)
                    {
                        m.Active = false;
                        db.SubmitChanges();
                    }
                }
            }
            dgvData.Rows.Remove(rowe);
        }

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            var rowe = dgvData.Rows.AddNew();
        }


    }
}
