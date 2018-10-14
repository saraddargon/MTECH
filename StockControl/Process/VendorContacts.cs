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
using System.IO;

namespace StockControl
{
    public partial class VendorContacts : Telerik.WinControls.UI.RadRibbonForm
    {
        TypeAction tAction = TypeAction.Add;
        public VendorContacts()
        {
            InitializeComponent();
        }
        public VendorContacts(int idVendor, TypeAction tAction)
        {
            InitializeComponent();
            txtid.Text = idVendor.ToSt();
            this.tAction = tAction;
        }

        //private int RowView = 50;
        //private int ColView = 10;
        DataTable dt = new DataTable();
        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            HistoryView hw = new HistoryView(this.Name, txtNo.Text);
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
                var op = txtAttachFile.Dialog as OpenFileDialog;
                op.Filter = "(*.pdf)|*.pdf";

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
                    com1.DataSource = db.mh_VendorGroups.ToList();

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
                    var g = db.mh_Vendors.Where(x => x.id == id).FirstOrDefault();
                    txtNo.Text = g.No;
                    txtBranchCode.Text = g.BranchCode;
                    txtName.Text = g.Name;
                    txtAddress.Text = g.Address;
                    txtShippingAddress.Text = g.ReceivingAddress;
                    cbbCustomerGroup.SelectedValue = g.VendorGroup;
                    txtShippingTime.Value = g.ShippingTime;
                    txtAttachFile.Value = g.AttachFile;
                    cbbVatGroup.SelectedValue = g.VatGroup;
                    cbbCurrency.SelectedValue = g.DefaultCurrency;
                    cbVatRegis.Checked = g.VATRegistration;
                    cbPriceIncVat.Checked = g.PriceIncludeingVat;
                    cbActive.Checked = g.Active;

                    var m = db.mh_VendorContacts.Where(w => w.VendorId == id && w.Active).ToList();
                    dgvData.AutoGenerateColumns = false;
                    dgvData.DataSource = null;
                    dgvData.DataSource = m;

                    var a = db.mh_VendorItems.Where(x => x.VendorId == id && x.Active).ToList();
                    dgvData2.AutoGenerateColumns = false;
                    dgvData2.DataSource = null;
                    dgvData2.DataSource = a;
                    setRowNo();

                    if (tAction == TypeAction.View)
                        setEdit(false);
                    else
                        setEdit(true);
                }
                else
                    setEdit(true);
            }

            if (tAction == TypeAction.Add)
                txtNo.ReadOnly = false;
            else
                txtNo.ReadOnly = true;


            //    radGridView1.DataSource = dt;
        }

        void setEdit(bool ss)
        {
            //txtNo.ReadOnly = !ss;
            txtBranchCode.ReadOnly = !ss;
            txtName.ReadOnly = !ss;
            txtAddress.ReadOnly = !ss;
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
                bool newDoc = false;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    int id = txtid.Text.ToInt();
                    var vndr = db.mh_Vendors.Where(x => x.id == id).FirstOrDefault();
                    if (vndr == null)
                    {
                        vndr = new mh_Vendor();
                        db.mh_Vendors.InsertOnSubmit(vndr);
                        newDoc = true;

                        dbClss.AddHistory(this.Name, "Vendor", $"New Vendor {txtNo.Text}", vndr.No);
                    }

                    if (!newDoc)
                    {
                        string vNo = txtNo.Text;
                        if (vndr.No != txtNo.Text.Trim()) dbClss.AddHistory(this.Name, "Vendor", $"Vendor No. from {vndr.No} to {txtNo.Text}", vNo);
                        if (vndr.BranchCode != txtBranchCode.Text.Trim()) dbClss.AddHistory(this.Name, "Vendor", $"Branch Code from {vndr.BranchCode} to {txtBranchCode.Text}", vNo);
                        if (vndr.Name != txtName.Text.Trim()) dbClss.AddHistory(this.Name, "Vendor", $"Vendor Name from {vndr.Name} to {txtName.Text}", vNo);
                        if (vndr.Address != txtAddress.Text.Trim()) dbClss.AddHistory(this.Name, "Vendor", $"Address from {vndr.Address} to {txtAddress.Text}", vNo);
                        if (vndr.ShippingTime != txtShippingTime.Value.ToInt()) dbClss.AddHistory(this.Name, "Vendor", $"Shipping time from {vndr.ShippingTime} to {txtShippingTime.Value.ToInt()}", vNo);
                        if (vndr.VATRegistration != cbVatRegis.Checked) dbClss.AddHistory(this.Name, "Vendor", $"VAT Regis. from {vndr.VATRegistration} to {cbVatRegis.Checked}", vNo);
                        if (vndr.PriceIncludeingVat != cbPriceIncVat.Checked) dbClss.AddHistory(this.Name, "Vendor", $"Price Inc. Vat from {vndr.PriceIncludeingVat} to {cbPriceIncVat.Checked}", vNo);
                        if (vndr.ReceivingAddress != txtShippingAddress.Text.Trim()) dbClss.AddHistory(this.Name, "Vendor", $"Receive Address from {vndr.ReceivingAddress} to {txtShippingAddress.Text}", vNo);
                        if (vndr.VendorGroup != cbbCustomerGroup.SelectedValue.ToInt()) dbClss.AddHistory(this.Name, "Vendor", $"Vendor Group from {vndr.VendorGroup} to {cbbCustomerGroup.SelectedValue.ToInt()}", vNo);
                        if (vndr.VatGroup != cbbVatGroup.SelectedValue.ToInt()) dbClss.AddHistory(this.Name, "Vendor", $"Vat Group from {vndr.VatGroup} to {cbbVatGroup.SelectedValue.ToInt()}", vNo);
                        if (vndr.DefaultCurrency != cbbCurrency.SelectedValue.ToInt()) dbClss.AddHistory(this.Name, "Vendor", $"Default Currency from {vndr.DefaultCurrency} to {cbbCurrency.SelectedValue.ToInt()}", vNo);
                        if (vndr.Active != cbActive.Checked) dbClss.AddHistory(this.Name, "Vendor", $"Status Active from {vndr.Active} to {cbActive.Checked}", vNo);
                        if (txtAttachFile.Value.ToSt() != "")
                        {
                            if (vndr.AttachFile != Path.GetFileName(txtAttachFile.Value.ToSt()))
                                dbClss.AddHistory(this.Name, "Vendor", $"Attach file from {vndr.AttachFile} to {Path.GetFileName(txtAttachFile.Value.ToSt())}", vNo);
                        }
                    }

                    vndr.No = txtNo.Text.Trim();
                    vndr.BranchCode = txtBranchCode.Text;
                    vndr.Name = txtName.Text.Trim();
                    vndr.Address = txtAddress.Text.Trim();
                    vndr.ShippingTime = txtShippingTime.Value.ToInt();
                    vndr.VATRegistration = cbVatRegis.Checked;
                    vndr.PriceIncludeingVat = cbPriceIncVat.Checked;
                    vndr.ReceivingAddress = txtShippingAddress.Text.Trim();
                    vndr.VendorGroup = cbbCustomerGroup.SelectedValue.ToInt();
                    vndr.VatGroup = cbbVatGroup.SelectedValue.ToInt();
                    vndr.DefaultCurrency = cbbCurrency.SelectedValue.ToInt();
                    vndr.Active = cbActive.Checked;
                    db.SubmitChanges();
                    //file
                    string fullPath = txtAttachFile.Value.ToSt();
                    string fName = "";
                    try
                    {
                        if (Path.GetFileName(fullPath) != fullPath)
                        {
                            try
                            {
                                fName = vndr.id.ToSt() + "_" + Path.GetFileName(fullPath);
                                File.Copy(fullPath, Path.Combine(baseClass.GetPathServer(PathCode.Vendor), fName), true);
                                fullPath = fName;
                            }
                            catch (Exception ex) { baseClass.Error(ex.Message); fName = ""; }

                        }
                    }
                    catch (Exception ex)
                    {
                        baseClass.Error(ex.Message);
                    }
                    vndr.AttachFile = fName;
                    db.SubmitChanges();

                    txtid.Text = vndr.id.ToSt();

                    foreach (var c in dgvData.Rows)
                    {
                        newDoc = false;
                        if (c.Cells["dgvC"].Value.ToSt() == "") continue;

                        int idDT = c.Cells["id"].Value.ToInt();
                        var con = db.mh_VendorContacts.Where(x => x.id == idDT).FirstOrDefault();
                        if (con == null)
                        {
                            con = new mh_VendorContact();
                            newDoc = true;
                            dbClss.AddHistory(this.Name, "Vendor", $"New Customer Contact {c.Cells["ContactName"].Value.ToSt()}", txtNo.Text);
                        }

                        if (!newDoc)
                        {
                            if (con.Def != c.Cells["Def"].Value.ToBool()) dbClss.AddHistory(this.Name, "Vendor", $"Default from {con.Def} to {c.Cells["Def"].Value.ToBool()}", txtNo.Text);
                            if (con.ContactName != c.Cells["ContactName"].Value.ToSt()) dbClss.AddHistory(this.Name, "Vendor", $"Contact Name from {con.ContactName} to {c.Cells["ContactName"].Value.ToSt()}", txtNo.Text);
                            if (con.Tel != c.Cells["Tel"].Value.ToSt()) dbClss.AddHistory(this.Name, "Vendor", $"Telephone from {con.Tel} to {c.Cells["Tel"].Value.ToSt()}", txtNo.Text);
                            if (con.Fax != c.Cells["Fax"].Value.ToSt()) dbClss.AddHistory(this.Name, "Vendor", $"Fax from {con.Fax} to {c.Cells["Fax"].Value.ToSt()}", txtNo.Text);
                            if (con.Email != c.Cells["Email"].Value.ToSt()) dbClss.AddHistory(this.Name, "Vendor", $"Email from {con.Email} to {c.Cells["Email"].Value.ToSt()}", txtNo.Text);
                        }

                        con.Def = c.Cells["Def"].Value.ToBool();
                        con.VendorId = vndr.id;
                        con.ContactName = c.Cells["ContactName"].Value.ToSt();
                        con.Tel = c.Cells["Tel"].Value.ToSt();
                        con.Fax = c.Cells["Fax"].Value.ToSt();
                        con.Email = c.Cells["Email"].Value.ToSt();
                        con.Active = true;
                        if (idDT <= 0)
                            db.mh_VendorContacts.InsertOnSubmit(con);
                        if (con.Def)
                        {
                            vndr.ContactName = con.ContactName;
                            vndr.PhoneNo = con.Tel;
                            vndr.FaxNo = con.Fax;
                            vndr.Email = con.Email;
                        }
                        db.SubmitChanges();
                    }

                    foreach (var c in dgvData2.Rows)
                    {
                        newDoc = false;
                        int idDt = c.Cells["id"].Value.ToInt();
                        string Item = c.Cells["Item"].Value.ToSt();
                        string Desc = c.Cells["Description"].Value.ToSt();
                        var v = db.mh_VendorItems.Where(x => x.id == idDt).FirstOrDefault();
                        if (v == null)
                        {
                            v = new mh_VendorItem();
                            v.VendorId = vndr.id;
                            db.mh_VendorItems.InsertOnSubmit(v);

                            newDoc = true;
                            dbClss.AddHistory(this.Name, "Vendor", $"New Catagory {Item}", txtNo.Text);
                        }

                        if (!newDoc)
                        {
                            if (v.Item != Item) dbClss.AddHistory(this.Name, "Vendor", $"Item from {v.Item} to {Item}", txtNo.Text);
                            if (v.Description != Desc) dbClss.AddHistory(this.Name, "Vendor", $"Description from {v.Description} to {Desc}", txtNo.Text);
                        }

                        v.Item = Item;
                        v.Description = Desc;
                        v.Active = true;
                        db.SubmitChanges();
                    }

                    tAction = TypeAction.Edit;
                    DataLoad();

                    if (dgvData.Rows.Count == 1)
                    {
                        int idDT = dgvData.Rows[0].Cells["id"].Value.ToInt();
                        dgvData.Rows[0].Cells["Def"].Value = true;
                        var m = db.mh_VendorContacts.Where(x => x.id == idDT).FirstOrDefault();
                        m.Def = true;
                        db.SubmitChanges();
                    }
                }

                baseClass.Info("Save complete.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("Add Vendor Contact", ex.Message, this.Name);
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
                string name = dgvData.CurrentCell.RowInfo.Cells["ContactName"].Value.ToSt();
                dgvData.EndEdit();
                if (MessageBox.Show("Do you want to Delete ( " + name + " ) ?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (id > 0)
                    {
                        using (DataClasses1DataContext db = new DataClasses1DataContext())
                        {
                            var v = db.mh_Vendors.Where(x => x.id == id).FirstOrDefault();
                            if (v != null)
                            {
                                v.Active = false;
                                db.SubmitChanges();

                                dbClss.AddHistory(this.Name, "Vendor", "Delete Vendor", v.No);
                            }
                        }
                    }

                    baseClass.Info("Delete complete.\n");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("Delete Vendor Contact", ex.Message, this.Name);
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

                if (txtNo.Text.Trim() == "")
                    err += "- Vendor no is empty.\n";
                else
                {
                    int id = txtid.Text.ToInt();
                    string vndrNo = txtNo.Text.Trim();
                    using (var db = new DataClasses1DataContext())
                    {
                        var m = db.mh_Vendors.Where(x => x.Active && x.id != id && x.No == vndrNo).ToList();
                        if (m.Count > 0)
                            err += "- Vendor no is dupplicate.\n";
                    }
                }
                if (txtName.Text.Trim().Equals(""))
                    err += "- Vendor name is empty.\n";
                if (txtBranchCode.Text.Trim().Equals(""))
                    err += "- Branch code is empty.\n";
                if (txtAddress.Text.Trim().Equals(""))
                    err += "- Address is empty.\n";
                if (txtShippingAddress.Text.Trim().Equals(""))
                    err += "- Receive Address is empty.\n";
                if (cbbCustomerGroup.SelectedValue.ToInt() == 0)
                    err += "- Vendor Group is empty.\n";
                if (cbbVatGroup.SelectedValue.ToInt() == 0)
                    err += "- Vat Group is empty.\n";
                if (cbbCurrency.SelectedValue.ToInt() == 0)
                    err += "- Currency is empty.\n";
                if (dgvData.Rows.Count == 0)
                {
                    err += "- Vendor contact is empty.\n";
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

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            dgvData.Rows.AddNew();
        }

        private void btnDeleteRow_Click(object sender, EventArgs e)
        {
            if (dgvData.CurrentCell == null) return;
            var rowe = dgvData.CurrentCell.RowInfo;
            int idDt = rowe.Cells["id"].Value.ToInt();
            if (idDt > 0)
            {
                using (var db = new DataClasses1DataContext())
                {
                    var m = db.mh_VendorContacts.Where(x => x.id == idDt).FirstOrDefault();
                    if (m != null)
                    {
                        m.Active = true;
                        db.SubmitChanges();
                    }
                }
            }
            dgvData.Rows.Remove(rowe);
        }

        private void btnAddRow2_Click(object sender, EventArgs e)
        {
            dgvData2.Rows.AddNew();
            setRowNo();
        }
        private void btnDeleteRow2_Click(object sender, EventArgs e)
        {
            if (dgvData2.CurrentCell == null) return;
            var rowe = dgvData2.CurrentCell.RowInfo;
            int idDt = rowe.Cells["id"].Value.ToInt();
            if (idDt > 0)
            {
                using (var db = new DataClasses1DataContext())
                {
                    var m = db.mh_VendorItems.Where(x => x.id == idDt).FirstOrDefault();
                    if (m != null)
                    {
                        m.Active = false;
                        db.SubmitChanges();
                    }
                }
            }
            dgvData2.Rows.Remove(rowe);
            setRowNo();
        }
        void setRowNo()
        {
            int rno = 1;
            dgvData2.Rows.ToList().ForEach(x =>
            {
                x.Cells["RNo"].Value = rno++;
            });
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (baseClass.Question("Do you want to 'Delete Attach file' ?"))
                DelAttachFile();
        }
        void DelAttachFile()
        {
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    int idv = txtid.Text.ToInt();
                    var m = db.mh_Vendors.Where(x => x.id == idv).FirstOrDefault();
                    if (m != null)
                    {
                        if (m.AttachFile.ToSt() != "")
                        {
                            m.AttachFile = "";
                            db.SubmitChanges();
                            baseClass.Info("Delete Attach file complete.\n");
                            dbClss.AddHistory(this.Name, "Vendor", "Delete Attach file", txtNo.Text);
                        }
                    }
                    txtAttachFile.Value = "";
                }
            }
            catch (Exception ex)
            {
                baseClass.Error(ex.Message);
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAttachFile.Value.ToSt() != "")
                {
                    string apath = txtAttachFile.Value.ToSt();
                    if (Path.GetFileName(txtAttachFile.Value.ToSt()) == Path.GetFileName(txtAttachFile.Value.ToSt()))
                        apath = Path.Combine(baseClass.GetPathServer(PathCode.Vendor), txtAttachFile.Value.ToSt());
                    System.Diagnostics.Process.Start(apath);
                }
            }
            catch (Exception ex)
            {
                baseClass.Error(ex.Message);
            }
        }
    }
}
