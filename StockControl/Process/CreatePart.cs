using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using Telerik.WinControls.Data;
using System.IO;
using System.Globalization;

namespace StockControl
{
    public partial class CreatePart : Telerik.WinControls.UI.RadRibbonForm
    {
        public CreatePart()
        {
            InitializeComponent();
          
        }
        string CodeNo = "";
        public CreatePart(string CodeNo)
        {
            InitializeComponent();
            this.CodeNo = CodeNo;
        }

        private int Cath01 = 9;
        DataTable dt_Import = new DataTable();
        DataTable dt = new DataTable();
        DataTable dt_Part = new DataTable();
        string Ac = "";
        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            HistoryView hw = new HistoryView(this.Name,txtInternalNo.Text);
            this.Cursor = Cursors.Default;
            hw.ShowDialog();
        }
        private void GETDTRow()
        {
            dt.Columns.Add(new DataColumn("DefaultNo", typeof(bool)));
            dt.Columns.Add(new DataColumn("VendorNo", typeof(string)));
            dt.Columns.Add(new DataColumn("ContactName", typeof(string)));
            dt.Columns.Add(new DataColumn("Tel", typeof(string)));
            dt.Columns.Add(new DataColumn("Fax", typeof(string)));
            dt.Columns.Add(new DataColumn("Email", typeof(string)));

            dt_Part = new DataTable();
            dt_Part.Columns.Add(new DataColumn("id", typeof(int)));
            dt_Part.Columns.Add(new DataColumn("InternalNo", typeof(string)));
            dt_Part.Columns.Add(new DataColumn("InternalName", typeof(string)));
            dt_Part.Columns.Add(new DataColumn("CustomerPartNo", typeof(string)));
            dt_Part.Columns.Add(new DataColumn("CustomerPartName", typeof(string)));
            dt_Part.Columns.Add(new DataColumn("GroupType", typeof(string)));
            dt_Part.Columns.Add(new DataColumn("Type", typeof(string)));
            dt_Part.Columns.Add(new DataColumn("InventoryGroup", typeof(string)));
            dt_Part.Columns.Add(new DataColumn("BaseUOM", typeof(string)));
            dt_Part.Columns.Add(new DataColumn("PurchaseUOM", typeof(string)));
            dt_Part.Columns.Add(new DataColumn("SalesUOM", typeof(string)));
            dt_Part.Columns.Add(new DataColumn("ConsumptionUOM", typeof(string)));
            dt_Part.Columns.Add(new DataColumn("VatType", typeof(string)));
            dt_Part.Columns.Add(new DataColumn("chkDrawing", typeof(bool)));
            dt_Part.Columns.Add(new DataColumn("Drawing", typeof(string)));
            dt_Part.Columns.Add(new DataColumn("ReplenishmentType", typeof(string)));
            dt_Part.Columns.Add(new DataColumn("InternalLeadTime", typeof(int)));
            dt_Part.Columns.Add(new DataColumn("ReorderType", typeof(string)));
            dt_Part.Columns.Add(new DataColumn("SafetyStock", typeof(decimal)));
            dt_Part.Columns.Add(new DataColumn("ReorderQty", typeof(decimal)));
            dt_Part.Columns.Add(new DataColumn("ReorderPoint", typeof(decimal)));
            dt_Part.Columns.Add(new DataColumn("MinimumQty", typeof(decimal)));
            dt_Part.Columns.Add(new DataColumn("MaximumQty", typeof(decimal)));
            dt_Part.Columns.Add(new DataColumn("BillOfMaterials", typeof(int)));
            dt_Part.Columns.Add(new DataColumn("Routing", typeof(int)));
            dt_Part.Columns.Add(new DataColumn("WorkCenter", typeof(int)));
            dt_Part.Columns.Add(new DataColumn("ItemUOM", typeof(int)));
            dt_Part.Columns.Add(new DataColumn("Active", typeof(bool)));
            dt_Part.Columns.Add(new DataColumn("VendorNo", typeof(string)));
            dt_Part.Columns.Add(new DataColumn("VendorName", typeof(string)));
            dt_Part.Columns.Add(new DataColumn("Timebucket", typeof(int)));
            dt_Part.Columns.Add(new DataColumn("BarCode", typeof(string)));
            dt_Part.Columns.Add(new DataColumn("CreateBy", typeof(string)));
            dt_Part.Columns.Add(new DataColumn("CreateDate", typeof(DateTime)));
            dt_Part.Columns.Add(new DataColumn("ModifyBy", typeof(string)));
            dt_Part.Columns.Add(new DataColumn("ModifyDate", typeof(DateTime)));



            //dt_Import
            dt_Import = new DataTable();
            dt_Import.Columns.Add(new DataColumn("id", typeof(int)));
            dt_Import.Columns.Add(new DataColumn("InternalNo", typeof(string)));
            dt_Import.Columns.Add(new DataColumn("InternalName", typeof(string)));
            dt_Import.Columns.Add(new DataColumn("CustomerPartNo", typeof(string)));
            dt_Import.Columns.Add(new DataColumn("CustomerPartName", typeof(string)));
            dt_Import.Columns.Add(new DataColumn("GroupType", typeof(string)));
            dt_Import.Columns.Add(new DataColumn("Type", typeof(string)));
            dt_Import.Columns.Add(new DataColumn("InventoryGroup", typeof(string)));
            dt_Import.Columns.Add(new DataColumn("BaseUOM", typeof(string)));
            dt_Import.Columns.Add(new DataColumn("PurchaseUOM", typeof(string)));
            dt_Import.Columns.Add(new DataColumn("SalesUOM", typeof(string)));
            dt_Import.Columns.Add(new DataColumn("ConsumptionUOM", typeof(string)));           
            dt_Import.Columns.Add(new DataColumn("VatType", typeof(string)));
            dt_Import.Columns.Add(new DataColumn("chkDrawing", typeof(bool)));
            dt_Import.Columns.Add(new DataColumn("Drawing", typeof(string)));          
            dt_Import.Columns.Add(new DataColumn("ReplenishmentType", typeof(string)));
            dt_Import.Columns.Add(new DataColumn("InternalLeadTime", typeof(int)));
            dt_Import.Columns.Add(new DataColumn("ReorderType", typeof(string)));
            dt_Import.Columns.Add(new DataColumn("SafetyStock", typeof(decimal)));
            dt_Import.Columns.Add(new DataColumn("ReorderQty", typeof(decimal)));
            dt_Import.Columns.Add(new DataColumn("ReorderPoint", typeof(decimal)));
            dt_Import.Columns.Add(new DataColumn("MinimumQty", typeof(decimal)));
            dt_Import.Columns.Add(new DataColumn("MaximumQty", typeof(decimal)));
            dt_Import.Columns.Add(new DataColumn("BillOfMaterials", typeof(int)));
            dt_Import.Columns.Add(new DataColumn("Routing", typeof(int)));     
            dt_Import.Columns.Add(new DataColumn("WorkCenter", typeof(int)));
            dt_Import.Columns.Add(new DataColumn("ItemUOM", typeof(int)));
            dt_Import.Columns.Add(new DataColumn("Active", typeof(bool)));
            dt_Import.Columns.Add(new DataColumn("VendorNo", typeof(string)));
            dt_Import.Columns.Add(new DataColumn("VendorName", typeof(string)));
            dt_Import.Columns.Add(new DataColumn("Timebucket", typeof(int)));        
            dt_Import.Columns.Add(new DataColumn("BarCode", typeof(string)));
            dt_Import.Columns.Add(new DataColumn("CreateBy", typeof(string)));
            dt_Import.Columns.Add(new DataColumn("CreateDate", typeof(DateTime)));
            dt_Import.Columns.Add(new DataColumn("ModifyBy", typeof(string)));
            dt_Import.Columns.Add(new DataColumn("ModifyDate", typeof(DateTime)));

           

        }
        private void Unit_Load(object sender, EventArgs e)
        {

            LoadPath_Dwg();
                
            //radGridView1.ReadOnly = true;
            //radGridView1.AutoGenerateColumns = false;
           

            txtCreateby.Text = ClassLib.Classlib.User;
            txtCreateDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            
            GETDTRow();
            Set_dt_Print();  // load data print

            LoadDefault();
            Cath01 = 9;

            Cleardata();
            if (!CodeNo.Equals(""))
            {
                txtInternalNo.Text = CodeNo;
                DataLoad();
                //View
                Enable_Status(false, "View");
                btnView.Enabled = false;
            }
            else
            {
                btnNew_Click(null, null);
                //New
                //Enable_Status(false, "-");
            }
        }
        private void Enable_Status(bool ss,string Condition)
        {
            if (Condition.Equals("-") || Condition.Equals("New"))
            {
                txtInternalNo.Enabled = ss;
                txtInternalName.Enabled = ss;
                txtCustomerPartName.Enabled = ss;
                cboGroupType.Enabled = ss;
                cboTypeCode.Enabled = ss;
                txtCustomerPartNo.Enabled = ss;
                cboVendorName.Enabled = ss;
                cboBaseUOM.Enabled = ss;
                ddlInventoryGroup.Enabled = ss;
                cboPurchaseUOM.Enabled = ss;
                cboSalseUOM.Enabled = ss;
                cboConsumptionUOM.Enabled = ss;
                ddlReplenishmentType.Enabled = ss;
                cboVatType.Enabled = ss;
                seInternalLeadTime.Enabled = ss;               
                ddlReOrderType.Enabled = ss;
                seMinimum.Enabled = ss;
                seMaximum.Enabled = ss;
                seReOrderPoint.Enabled = ss;
                seReOrderQty.Enabled = ss;
                seSafetyStock.Enabled = ss;
                seTimebucket.Enabled = ss;
                txtDrawing.Enabled = ss;

                btnAddDWG.Enabled = ss;
                btnDeleteDWG.Enabled = ss;
                chkGET.Checked = false;
                //btnGET.Enabled = true;
            }
            else if (Condition.Equals("View"))
            {
                txtInternalNo.Enabled = ss;
                txtInternalName.Enabled = ss;
                txtCustomerPartName.Enabled = ss;
                cboGroupType.Enabled = ss;
                cboTypeCode.Enabled = ss;
                txtCustomerPartNo.Enabled = ss;
                cboVendorName.Enabled = ss;
                cboBaseUOM.Enabled = ss;
                ddlInventoryGroup.Enabled = ss;
                cboPurchaseUOM.Enabled = ss;
                cboSalseUOM.Enabled = ss;
                cboConsumptionUOM.Enabled = ss;
                ddlReplenishmentType.Enabled = ss;
                cboVatType.Enabled = ss;
                seInternalLeadTime.Enabled = ss;
                ddlReOrderType.Enabled = ss;
                seMinimum.Enabled = ss;
                seMaximum.Enabled = ss;
                seReOrderPoint.Enabled = ss;
                seReOrderQty.Enabled = ss;
                seSafetyStock.Enabled = ss;
                seTimebucket.Enabled = ss;
                txtDrawing.Enabled = ss;

                btnAddDWG.Enabled = ss;
                btnDeleteDWG.Enabled = ss;
                btnGET.Enabled = false;
                chkGET.Checked = false;
            }
            else if (Condition.Equals("Edit"))
            {
                txtInternalNo.Enabled = false;
                cboGroupType.Enabled = false;
                txtInternalName.Enabled = ss;
                txtCustomerPartName.Enabled = ss;
                cboTypeCode.Enabled = ss;
                txtCustomerPartNo.Enabled = ss;
                cboVendorName.Enabled = ss;
                cboBaseUOM.Enabled = ss;
                ddlInventoryGroup.Enabled = ss;
                cboPurchaseUOM.Enabled = ss;
                cboSalseUOM.Enabled = ss;
                cboConsumptionUOM.Enabled = ss;
                ddlReplenishmentType.Enabled = ss;
                cboVatType.Enabled = ss;
                seInternalLeadTime.Enabled = ss;
                ddlReOrderType.Enabled = ss;
                seMinimum.Enabled = ss;
                seMaximum.Enabled = ss;
                seReOrderPoint.Enabled = ss;
                seReOrderQty.Enabled = ss;
                seSafetyStock.Enabled = ss;
                seTimebucket.Enabled = ss;
                txtDrawing.Enabled = ss;

                btnAddDWG.Enabled = ss;
                btnDeleteDWG.Enabled = ss;
                btnGET.Enabled = false;
                chkGET.Checked = false;
            }
        }
        private void LoadDefault()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                this.cboGroupType.AutoFilter = true;
                this.cboGroupType.DisplayMember = "GroupCode";
                FilterDescriptor filter = new FilterDescriptor();
                filter.PropertyName = this.cboGroupType.DisplayMember;
                filter.Operator = FilterOperator.Contains;
                this.cboGroupType.AutoCompleteMode = AutoCompleteMode.Append;
                this.cboGroupType.EditorControl.MasterTemplate.FilterDescriptors.Add(filter);
                this.cboGroupType.BestFitColumns();

                this.cboVendorName.AutoFilter = true;
                this.cboVendorName.AutoCompleteMode = AutoCompleteMode.Append;
                FilterDescriptor fi = new FilterDescriptor();
                fi.PropertyName = this.cboVendorName.ValueMember;
                fi.Operator = FilterOperator.StartsWith;
                this.cboVendorName.EditorControl.MasterTemplate.FilterDescriptors.Add(fi);
                
                cboVendorName.DisplayMember = "Name";
                cboVendorName.ValueMember = "No";
                cboVendorName.DataSource = db.mh_Vendors.Where(s => s.Active == true).ToList();
                cboVendorName.SelectedIndex = -1;
                cboVendorName.Text = "";


                this.cboVatType.AutoFilter = true;
                this.cboVatType.AutoCompleteMode = AutoCompleteMode.Append;
                FilterDescriptor vv = new FilterDescriptor();
                vv.PropertyName = this.cboVatType.ValueMember;
                vv.Operator = FilterOperator.StartsWith;
                this.cboVatType.EditorControl.MasterTemplate.FilterDescriptors.Add(vv);

                cboVatType.DisplayMember = "VatType";
                cboVatType.ValueMember = "VatType";
                cboVatType.DataSource = db.mh_VATTypes.Where(ab=>ab.Active==true).ToList();
                cboVatType.SelectedIndex = -1;
                cboVatType.Text = "";

                cboPurchaseUOM.DisplayMember = "UnitCode";
                cboPurchaseUOM.ValueMember = "UnitCode";
                cboPurchaseUOM.DataSource = db.mh_Units.Where(s => s.UnitActive == true).ToList();

                cboSalseUOM.DataSource = null;
                cboSalseUOM.DisplayMember = "UnitCode";
                cboSalseUOM.ValueMember = "UnitCode";
                cboSalseUOM.DataSource = db.mh_Units.Where(w => w.UnitActive == true).ToList();

                cboConsumptionUOM.DataSource = null;
                cboConsumptionUOM.DisplayMember = "UnitCode";
                cboConsumptionUOM.ValueMember = "UnitCode";
                cboConsumptionUOM.DataSource = db.mh_Units.Where(w => w.UnitActive == true).ToList();

                cboBaseUOM.DataSource = null;
                cboBaseUOM.DisplayMember = "UnitCode";
                cboBaseUOM.ValueMember = "UnitCode";
                cboBaseUOM.DataSource = db.mh_Units.Where(w => w.UnitActive == true).ToList();

                cboGroupType.DisplayMember = "GroupCode";
                cboGroupType.ValueMember = "GroupCode";
                cboGroupType.DataSource = db.mh_GroupTypes.Where(s => s.GroupActive == true).ToList();
                cboGroupType.BestFitColumns();
                try
                {

                    cboGroupType.SelectedIndex = 0;

                    if (!cboGroupType.Text.Equals(""))
                    {
                        DefaultType();
                    }
                }
                catch { }

              
                //ddlReOrderType.DisplayMember = "ShelfNo";
                //ddlReOrderType.ValueMember = "ShelfNo";
                //ddlReOrderType.DataSource = db.sp_045_ShelfNo_Select("").ToList();


            }
        }
        private void LoadPath_Dwg()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                var g = (from ix in db.tb_Paths
                         where ix.PathCode == "Drawing"
                         select ix).ToList();
                if (g.Count > 0)
                    lblPath.Text = StockControl.dbClss.TSt(g.FirstOrDefault().PathFile);
            }
        }
        private void DefaultType()
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    cboTypeCode.DataSource = null;
                    cboTypeCode.DisplayMember = "TypeCode";
                    cboTypeCode.ValueMember = "TypeCode";
                    cboTypeCode.DataSource = db.mh_Types.Where(t => t.TypeActive == true && t.GroupCode.Equals(cboGroupType.Text)).ToList();
                    
                    //cboTypeCode.SelectedIndex = 0;
                }
            }
            catch { }
        }
        private void DataLoad()
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var g = (from ix in db.mh_Items select ix).Where(a => a.InternalNo == txtInternalNo.Text).ToList();
                    if (g.Count() > 0)
                    {
                        txtInternalNo.Text = StockControl.dbClss.TSt(g.FirstOrDefault().InternalNo);
                        txtInternalName.Text = StockControl.dbClss.TSt(g.FirstOrDefault().InternalName);
                        txtCustomerPartName.Text = StockControl.dbClss.TSt(g.FirstOrDefault().CustomerPartName);
                        txtCustomerPartNo.Text = StockControl.dbClss.TSt(g.FirstOrDefault().CustomerPartNo);
                        cboGroupType.Text = StockControl.dbClss.TSt(g.FirstOrDefault().GroupType);
                        cboTypeCode.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Type);
                        cboVendorName.Text = StockControl.dbClss.TSt(g.FirstOrDefault().VendorName);
                        txtVendorNo.Text = StockControl.dbClss.TSt(g.FirstOrDefault().VendorNo);
                        ddlInventoryGroup.Text = StockControl.dbClss.TSt(g.FirstOrDefault().InventoryGroup);
                        cboBaseUOM.Text = StockControl.dbClss.TSt(g.FirstOrDefault().BaseUOM);
                        cboConsumptionUOM.Text = StockControl.dbClss.TSt(g.FirstOrDefault().ConsumptionUOM);
                        cboPurchaseUOM.Text = StockControl.dbClss.TSt(g.FirstOrDefault().PurchaseUOM);
                        cboSalseUOM.Text = StockControl.dbClss.TSt(g.FirstOrDefault().SalesUOM);
                        cboVatType.Text = StockControl.dbClss.TSt(g.FirstOrDefault().VatType);
                        ddlReplenishmentType.Text = StockControl.dbClss.TSt(g.FirstOrDefault().ReplenishmentType);
                        seInternalLeadTime.Value = StockControl.dbClss.TInt(g.FirstOrDefault().InternalLeadTime);
                       
                        ddlReOrderType.Text = StockControl.dbClss.TSt(g.FirstOrDefault().ReorderType);
                        seMaximum.Value = StockControl.dbClss.TDe(g.FirstOrDefault().MaximumQty);
                        seMinimum.Value = StockControl.dbClss.TDe(g.FirstOrDefault().MinimumQty);
                        seReOrderPoint.Value = StockControl.dbClss.TDe(g.FirstOrDefault().ReorderPoint);
                        seReOrderQty.Value = StockControl.dbClss.TDe(g.FirstOrDefault().ReorderQty);
                        seSafetyStock.Value = StockControl.dbClss.TDe(g.FirstOrDefault().SafetyStock);
                        seTimebucket.Value = StockControl.dbClss.TInt(g.FirstOrDefault().Timebucket);

                        txtDrawing.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Drawing);
                        chkDWG.Checked = StockControl.dbClss.TBo(g.FirstOrDefault().chkDrawing);

                        txtCreateby.Text = StockControl.dbClss.TSt(g.FirstOrDefault().CreateBy);
                        DateTime temp = Convert.ToDateTime(g.FirstOrDefault().CreateDate);
                        txtCreateDate.Text = temp.ToString("dd/MMM/yyyy");
                        txtUpdateBy.Text = StockControl.dbClss.TSt(g.FirstOrDefault().ModifyBy);
                        if (!txtUpdateBy.Text.Equals(""))
                        {
                            DateTime temp2 = Convert.ToDateTime(g.FirstOrDefault().ModifyDate);
                            txtUpdateDate.Text = temp2.ToString("dd/MMM/yyyy");
                        }
                        else
                            txtUpdateDate.Text = "";

                     
                        if (StockControl.dbClss.TBo(g.FirstOrDefault().Active).Equals(false))
                        {
                            btnSave.Enabled = false;
                            btnDelete.Enabled = false;
                            btnView.Enabled = false;
                            btnEdit.Enabled = false;
                            lbStatus.Text = "InActive";
                        }
                        else
                        {
                            btnSave.Enabled = true;
                            btnDelete.Enabled = true;
                            btnView.Enabled = true;
                            btnEdit.Enabled = true;
                            lbStatus.Text = "Active";
                        }
                        dt_Part = StockControl.dbClss.LINQToDataTable(g);
                    }

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
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

        
      
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {

            Cleardata();
            lbStatus.Text = "New";
            btnView.Enabled = true;
            btnEdit.Enabled = true;
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            //radGridView1.ReadOnly = false;
            //radGridView1.AllowAddNewRow = false;
            //radGridView1.Rows.AddNew();
            Enable_Status(true, "New");
            //btnGET.Enabled = true;
            chkGET.Enabled = true;

            Ac = "New";
            
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            Ac = "View";
            btnView.Enabled = false;
            btnEdit.Enabled = true;
            btnNew.Enabled = true;
            Enable_Status(false, "View");
            btnGET.Enabled = false;
            chkGET.Enabled = false;

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (txtInternalNo.Text.Equals(""))
            {
                MessageBox.Show("ไม่สามารถทำการแก้ไขรายการได้");
            }
            else
            {
                btnView.Enabled = true;
                btnEdit.Enabled = false;
                btnNew.Enabled = true;
                lbStatus.Text = "Edit";
                Enable_Status(true, "Edit");
                btnGET.Enabled = false;
                chkGET.Enabled = false;
                Ac = "Edit";
            }
        }
       
      
        private bool AddPart()
        {
            bool ck = false;
            int C = 0;
            try
            {

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    if (Ac.Equals("New"))  //New
                    {

                        string Temp_codeno = txtInternalNo.Text;
                        string temp_codeno2 = "";
                        if (chkGET.Checked.Equals(false))// ให้ระบบ Gen ให้
                        {
                            //if (txtCodeNo.Text.Length > 5)
                            //{
                            //    int c = txtCodeNo.Text.Length;

                            //    temp_codeno2 = Temp_codeno.Substring(5, c - 5);
                            //    txtCodeNo.Text = Get_CodeNo();
                            //    txtCodeNo.Text = txtCodeNo.Text + temp_codeno2;
                            //}
                            //else
                            txtInternalNo.Text = Get_CodeNo();
                            if (txtCustomerPartNo.Text == "")
                                txtCustomerPartNo.Text = txtInternalNo.Text;
                        }
                        //byte[] barcode = StockControl.dbClss.SaveQRCode2D(txtCodeNo.Text);
                        //byte[] barcode = null;
                        
                        //DateTime? UpdateDate = null;

                        mh_Item u = new mh_Item();
                        u.InternalNo = txtInternalNo.Text.Trim();
                        u.InternalName = txtInternalName.Text.Trim();
                        u.CustomerPartName = txtCustomerPartName.Text.Trim();
                        u.CustomerPartNo = txtCustomerPartNo.Text;
                        u.GroupType = cboGroupType.Text;
                        u.Type = cboTypeCode.Text;
                        u.InventoryGroup = ddlInventoryGroup.Text;
                        u.BaseUOM = cboBaseUOM.Text;
                        u.PurchaseUOM = cboPurchaseUOM.Text;
                        u.SalesUOM = cboSalseUOM.Text;
                        u.ConsumptionUOM = cboConsumptionUOM.Text;
                        u.VatType = cboVatType.Text;
                        u.ReplenishmentType = ddlReplenishmentType.Text;
                        u.InternalLeadTime = dbClss.TInt(seInternalLeadTime.Value);
                        u.ReorderType = ddlReOrderType.Text;
                        u.ReorderQty = dbClss.TDe(seReOrderQty.Value);
                        u.ReorderPoint = dbClss.TDe(seReOrderPoint);
                        u.MinimumQty = dbClss.TDe(seMinimum.Value);
                        u.MaximumQty = dbClss.TDe(seMaximum.Value);
                        u.SafetyStock = dbClss.TDe(seSafetyStock.Value);
                        u.ItemUOM = 0;
                        u.BillOfMaterials = 0;
                        u.Routing = 0;
                        u.WorkCenter = 0;
                        u.Active = true;
                        u.VendorNo = txtVendorNo.Text;
                        u.VendorName = cboVendorName.Text;
                        u.Timebucket = dbClss.TInt(seTimebucket.Value);                      
                        u.chkDrawing = chkDWG.Checked;
                        u.Drawing = txtDrawing.Text;
                        ///Save Drawing
                        if (chkDWG.Checked)
                        {
                            string tagetpart = lblPath.Text;
                            string FileName = lblTempAddFile.Text;
                            if (!System.IO.Directory.Exists(tagetpart))  //เช็คว่ามี partไฟล์เก็บหรือไม่ถ้าไม่ให้สร้างใหม่
                            {
                                System.IO.Directory.CreateDirectory(tagetpart);
                            }
                            //System.IO.File.Copy()

                            string File_temp = txtInternalNo.Text + "_" + ".pdf";//Path.GetExtension(AttachFile);  // IMG_IT-0123.jpg
                            File.Copy(FileName, tagetpart + File_temp, true);//ต้องทำเสมอ เป็นการ ก็อปปี้ Path เพื่อให้รูป มาว่างไว้ที่ path นี้ 

                            dbClss.AddHistory(this.Name, "เพิ่ม Drawing", "เพิ่มไฟล์ Drawing [" + txtInternalNo.Text.Trim() + "]", txtInternalNo.Text);
                        }

                        u.CreateBy = ClassLib.Classlib.User;
                        u.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                        db.mh_Items.InsertOnSubmit(u);
                        db.SubmitChanges();
                        C += 1;
                        dbClss.AddHistory(this.Name, "เพิ่มทูล", "Insert Part [" + u.InternalNo + "]", txtInternalNo.Text);
                    }
                    else  //Edit
                    {
                        foreach (DataRow row in dt_Part.Rows)
                        {
                            var g = (from ix in db.mh_Items
                                     where ix.InternalNo.Trim() == txtInternalNo.Text.Trim() && ix.Active == true
                                     select ix).ToList();
                            if (g.Count > 0)  //มีรายการในระบบ
                            {
                                var gg = (from ix in db.mh_Items
                                          where ix.InternalNo.Trim() == txtInternalNo.Text.Trim() && ix.Active == true
                                          select ix).First();
                                //gg.Status = "Active";

                                gg.ModifyBy = ClassLib.Classlib.User;
                                gg.ModifyDate = Convert.ToDateTime(DateTime.Now,new CultureInfo("en-US"));
                                dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไขโดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", txtInternalNo.Text);

                                //if(StockControl.dbClss.TSt(gg.BarCode).Equals(""))
                                //    gg.BarCode = StockControl.dbClss.SaveQRCode2D(txtCodeNo.Text);

                                if (!txtInternalName.Text.Trim().Equals(row["InternalName"].ToString()))
                                {
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไขชื่อทูล [ เดิม : " + row["InternalName"].ToString() +" ใหม่ : " + txtInternalName.Text.Trim() + "]", txtInternalNo.Text);
                                    gg.InternalName = txtInternalName.Text.Trim();
                                    
                                }
                                if (!txtCustomerPartNo.Text.Trim().Equals(row["CustomerPartNo"].ToString()))
                                {
                                    gg.CustomerPartNo = txtCustomerPartNo.Text.Trim();
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไขรหัสสินค้า [ เดิม : " + row["CustomerPartNo"].ToString() + " ใหม่ : " + txtCustomerPartNo.Text.Trim() + "]", txtInternalNo.Text);
                                }
                                if (!txtCustomerPartName.Text.Trim().Equals(row["CustomerPartName"].ToString()))
                                {
                                    gg.CustomerPartName = txtCustomerPartName.Text.Trim();
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไขชื่อสินค้า[ เดิม : " + row["CustomerPartName"].ToString() + " ใหม่ : " + txtCustomerPartName.Text.Trim() + "]", txtInternalNo.Text);
                                }
                                if (!cboGroupType.Text.Trim().Equals(row["GroupType"].ToString()))
                                {
                                    gg.GroupType = cboGroupType.Text.Trim();
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไขประเภทกลุ่มสินค้า [ เดิม : " + row["GroupType"].ToString() + " ใหม่ : " + cboGroupType.Text.Trim() + "]", txtInternalNo.Text);
                                }
                                if (!cboTypeCode.Text.Trim().Equals(row["Type"].ToString()))
                                {
                                    gg.Type = cboTypeCode.Text.Trim();
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไขประเภทสินค้า [ เดิม : " + row["Type"].ToString() + " ใหม่ : " + cboTypeCode.Text.Trim() + "]", txtInternalNo.Text);
                                }
                                if (!ddlInventoryGroup.Text.Trim().Equals(row["InventoryGroup"].ToString()))
                                {
                                    gg.InventoryGroup = ddlInventoryGroup.Text.Trim();
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไขประเภททูล [ เดิม : " + row["InventoryGroup"].ToString() + " ใหม่ : " + ddlInventoryGroup.Text.Trim() + "]", txtInternalNo.Text);
                                }
                                if (!cboVendorName.Text.Trim().Equals(row["VendorName"].ToString()))
                                {
                                    gg.VendorName = cboVendorName.Text.Trim();
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไขชื่อผู้ขาย [ เดิม : " + row["VendorName"].ToString() + " ใหม่ : " + cboVendorName.Text.Trim() + "]", txtInternalNo.Text);
                                }
                                if (!txtVendorNo.Text.Trim().Equals(row["VendorNo"].ToString()))
                                {
                                    gg.VendorNo = txtVendorNo.Text.Trim();
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไขชื่อผู้ขาย [ เดิม : " + row["VendorNo"].ToString() + " ใหม่ : " + txtVendorNo.Text.Trim() + "]", txtInternalNo.Text);
                                }
                                if (!cboBaseUOM.Text.Trim().Equals(row["BaseUOM"].ToString()))
                                {
                                    gg.BaseUOM = cboBaseUOM.Text.Trim();
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไขหน่วยพื้นฐาน [ เดิม : " + row["BaseUOM"].ToString() + " ใหม่ : " + cboBaseUOM.Text.Trim() + "]", txtInternalNo.Text);
                                }
                                if (!cboPurchaseUOM.Text.Trim().Equals(row["PurchaseUOM"].ToString()))
                                {
                                    gg.PurchaseUOM = cboPurchaseUOM.Text.Trim();
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไขหน่วยซื้อ [ เดิม : " + row["PurchaseUOM"].ToString() + " ใหม่ : " + cboPurchaseUOM.Text.Trim() + "]", txtInternalNo.Text);
                                }
                                if (!cboSalseUOM.Text.Trim().Equals(row["SalesUOM"].ToString()))
                                {
                                    gg.SalesUOM = cboSalseUOM.Text.Trim();
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไขหน่วยขาย [ เดิม : " + row["SalesUOM"].ToString() + " ใหม่ : " + cboSalseUOM.Text.Trim() + "]", txtInternalNo.Text);
                                }
                                if (!cboConsumptionUOM.Text.Trim().Equals(row["ConsumptionUOM"].ToString()))
                                {
                                    gg.ConsumptionUOM = cboConsumptionUOM.Text.Trim();
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไขหน่วยเบิก [ เดิม : " + row["ConsumptionUOM"].ToString() + " ใหม่ : " + cboConsumptionUOM.Text.Trim() + "]", txtInternalNo.Text);
                                }

                                if (!cboVatType.Text.Trim().Equals(row["VatType"].ToString()))
                                {
                                    gg.VatType = cboVatType.Text;
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไขประเภทภาษี [ เดิม : " + row["VatType"].ToString() + " ใหม่ : " + cboVatType.Text + "]", txtInternalNo.Text);
                                }
                                if (!ddlReplenishmentType.Text.Trim().Equals(row["ReplenishmentType"].ToString()))
                                {
                                    gg.ReplenishmentType = ddlReplenishmentType.Text;
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไขทดแทน [ เดิม : " + row["ReplenishmentType"].ToString() + " ใหม่ : " + ddlReplenishmentType.Text + "]", txtInternalNo.Text);
                                }                               
                                if (!seInternalLeadTime.Text.Trim().Equals(row["InternalLeadTime"].ToString()))
                                {
                                    int InternalLeadTime = dbClss.TInt(seInternalLeadTime.Value);
                                    gg.InternalLeadTime = InternalLeadTime;
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไขระยะเวลาเคลื่อนย้าย [ เดิม : " + row["InternalLeadTime"].ToString() + " ใหม่ : " + InternalLeadTime.ToString() + "]", txtInternalNo.Text);
                                }                             
                              
                                if (!seMaximum.Text.Trim().Equals(row["MaximumQty"].ToString()))
                                {
                                    decimal Maximum = 0; dbClss.TDe(seMaximum.Value);
                                    gg.MaximumQty = Maximum;
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไข MaximumStock [ เดิม : " + row["MaximumQty"].ToString() + " ใหม่ : " + Maximum.ToString() + "]", txtInternalNo.Text);
                                }
                                if (!seMinimum.Text.Trim().Equals(row["MinimumQty"].ToString()))
                                {
                                    decimal Minimum =  dbClss.TDe(seMinimum.Value);
                                    gg.MinimumQty = Minimum;
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไข MinimumStock [ เดิม : " + row["MinimumQty"].ToString() + " ใหม่ : " + Minimum.ToString() + "]", txtInternalNo.Text);
                                }
                                if (!ddlReOrderType.Text.Trim().Equals(row["ReorderType"].ToString()))
                                {
                                    gg.ReorderType = ddlReOrderType.Text;
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไขการวางผนการซื้อ [ เดิม : " + row["ReorderType"].ToString() + " ใหม่ : " + ddlReOrderType.ToString() + "]", txtInternalNo.Text);
                                }
                                if (!seReOrderPoint.Text.Trim().Equals(row["ReorderPoint"].ToString()))
                                {
                                    decimal ReorderPoint = dbClss.TDe(seReOrderPoint.Value);
                                    gg.ReorderPoint = ReorderPoint;
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไขจุดสั่งซื้อ [ เดิม : " + row["ReorderPoint"].ToString() + " ใหม่ : " + ReorderPoint.ToString() + "]", txtInternalNo.Text);
                                }
                                if (!seReOrderQty.Text.Trim().Equals(row["ReorderQty"].ToString()))
                                {
                                    decimal ReOrderQty = dbClss.TDe(seReOrderQty.Value);
                                    gg.ReorderQty = ReOrderQty;
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไขจำนวนสั่งซื้อ [ เดิม : " + row["ReorderQty"].ToString() + " ใหม่ : " + ReOrderQty.ToString() + "]", txtInternalNo.Text);
                                }
                                if (!seSafetyStock.Text.Trim().Equals(row["SafetyStock"].ToString()))
                                {
                                    decimal SafetyStock = dbClss.TDe(seSafetyStock.Value);
                                    gg.SafetyStock = SafetyStock;
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไขจำนวนสั่งซื้อ [ เดิม : " + row["SafetyStock"].ToString() + " ใหม่ : " + SafetyStock.ToString() + "]", txtInternalNo.Text);
                                }
                                if (!seTimebucket.Text.Trim().Equals(row["Timebucket"].ToString()))
                                {
                                    int Timebucket = dbClss.TInt(seTimebucket.Value);
                                    gg.Timebucket = Timebucket;
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไขกำหนดระยะเวลาที่: จะใช้ในการรวม order[ เดิม : " + row["Timebucket"].ToString() + " ใหม่ : " + Timebucket.ToString() + "]", txtInternalNo.Text);
                                }
                                if (!txtDrawing.Text.Trim().Equals(row["Drawing"].ToString()))
                                {
                                    gg.Drawing = txtDrawing.Text.Trim();
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไข Drawing No [ เดิม : " + row["Drawing"].ToString() + " ใหม่ : " + txtDrawing.Text.Trim() + "]", txtInternalNo.Text);
                                }
                                if (!chkDWG.Checked.ToString().Trim().Equals(row["chkDrawing"].ToString()))
                                {
                                    bool DWG = chkDWG.Checked;
                                    gg.chkDrawing = DWG;
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไข Drawing [" + chkDWG.Checked.ToString() + "]", txtInternalNo.Text);
                                }


                                C += 1;
                                db.SubmitChanges();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError(this.Name, ex.Message, this.Name);
            }

            if (C > 0)
                MessageBox.Show("บันทึกสำเร็จ!");

            return ck;
        }
        private bool Check_Save()
        {
            bool re = true;
            string err = "";
            try
            {
                //if (txtCodeNo.Text.Equals(""))
                //    err += " “รหัสพาร์ท:” เป็นค่าว่าง \n";
                if (txtInternalName.Text.Equals(""))
                    err += " “ชื่อทูล:” เป็นค่าว่าง \n";
                if (txtCustomerPartNo.Text.Equals(""))
                    txtCustomerPartNo.Text = txtInternalNo.Text;//err += "- “รหัสสินค้า:” เป็นค่าว่าง \n";
                if (txtCustomerPartName.Text.Equals(""))
                    txtCustomerPartName.Text = txtInternalName.Text;//err += "- “ชื่อสินค้า:” เป็นค่าว่าง \n";

                if (cboGroupType.Text.Equals(""))
                    err += "- “ประเภทกลุ่ม สินค้า:” เป็นค่าว่าง \n";
                if (cboTypeCode.Text.Equals(""))
                    err += "- “ประเภทสินค้า:” เป็นค่าว่าง \n";
                if (ddlInventoryGroup.Text.Equals(""))
                    err += "- “ประเภททูล:” เป็นค่าว่าง \n";
                if (cboVendorName.Text.Equals(""))
                    err += "- “ชื่อผู้ขาย:” เป็นค่าว่าง \n";
                if (txtVendorNo.Text.Equals(""))
                    err += "- “รหัสผู้ขาย:” เป็นค่าว่าง \n";
                if (cboBaseUOM.Text.Equals(""))
                    err += "- “หน่วยพื้นฐาน:” เป็นค่าว่าง \n";
                if (cboPurchaseUOM.Text.Equals(""))
                    err += "- “หน่วยซื้อ:” เป็นค่าว่าง \n";
                if (cboSalseUOM.Text.Equals(""))
                    err += "- “หน่วยขาย:” เป็นค่าว่าง \n";
                if (cboConsumptionUOM.Text.Equals(""))
                    err += "- “หน่วยเบิก:” เป็นค่าว่าง \n";
                if (cboVatType.Text.Equals(""))
                    err += "- “ประเภทภาษี:” เป็นค่าว่าง \n";
                if (ddlReplenishmentType.Text.Equals(""))
                    err += "- “ทดแทนด้วย:” เป็นค่าว่าง \n";

                if (ddlReOrderType.Text == "Minimum & Maximum Qty")
                {
                    if (seMaximum.Value.Equals(0))
                        err += "- “Maximum Stock:” น้อยกว่า 0 ไม่ได้ \n";
                    //if (seMinimum.Text.Equals(0))
                    //    err += "- “Minimum Stock:” เป็นค่าว่าง \n";
                }
                else if (ddlReOrderType.Text == "Fixed Reorder Qty")
                {
                    if (seReOrderPoint.Value.Equals(0))
                        err += "- “จุดสั่งซื้อ:” น้อยกว่า 0 ไม่ได้ \n";
                    if (seReOrderQty.Value.Equals(0))
                        err += "- “จำนวนที่สั่งซื้อ:” น้อยกว่า 0 ไม่ได้ \n";
                }
                   

                //---------------check codeno -------------------//
                if (Ac.Equals("New"))  //New
                {
                    if (chkGET.Checked)
                    {
                        if (txtInternalNo.Text.Trim().Equals(""))
                        {
                            err += " “รหัสทูล:” เป็นค่าว่าง \n";
                        }
                        else //เช็คว่า เลข Gen ด้านหน้าเป็น เลข Group เดียวกันหรือไม่ ถ้าไม่ใช่จะขึ้น Error
                        {
                            using (DataClasses1DataContext db = new DataClasses1DataContext())
                            {
                                string Temp_Running = "";
                                var I = (from ix in db.mh_GroupTypes select ix).Where(a => a.GroupCode == cboGroupType.Text).ToList();
                                if (I.Count > 0)
                                    Temp_Running = I.FirstOrDefault().Running;

                                if (!Temp_Running.Equals(""))
                                {
                                    string cut_string = "";
                                    cut_string = txtInternalNo.Text.Trim().Substring(0, 1);
                                    if (!cut_string.ToUpper().Equals(Temp_Running.ToUpper()))
                                        err += "- “รหัสทูล เริ่มต้นไม่ตรงกับประเภทกลุ่มสินค้า:”  \n";
                                    else//เช็คว่าเป็น CodeNo ที่มีในระบบหรือไม่ ถ้ามีแล้วจะ New เลขใหม่ไม่ได้ เพราะซ้ำ
                                    {
                                        var g1 = (from ix in db.tb_Items select ix).Where(a => a.CodeNo == txtInternalNo.Text.Trim()).ToList();
                                        if (g1.Count() > 0)
                                        {
                                            err += "- “รหัสทูล ซ้ำ:”มีรหัสทูล ในระบบแล้ว  \n";
                                        }
                                    }
                                }
                                //err += "- “ประเภทกลุ่ม สินค้า:” เป็นค่าว่าง \n";
                            }
                        }
                    }
                }
                //-----------------------------------------------//



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
            try
            {
                if (Check_Save())
                    return;
                else
                {
                    if (MessageBox.Show("ต้องการบันทึก ?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        AddPart();
                        DataLoad();
                       
                        Ac = "View";
                        btnView.Enabled = false;
                        btnEdit.Enabled = true;
                        btnNew.Enabled = true;
                        Enable_Status(false, "View");
                        chkGET.Enabled = false;
                        btnGET.Enabled = false;
                    }
                }
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                //radGridView1.Rows[e.RowIndex].Cells["dgvC"].Value = "T";
                //string check1 = Convert.ToString(radGridView1.Rows[e.RowIndex].Cells["VendorNo"].Value);
                //string TM= Convert.ToString(radGridView1.Rows[e.RowIndex].Cells["dgvCodeTemp"].Value);
                //if (!check1.Trim().Equals("") && TM.Equals(""))
                //{
                    
                //    if (!CheckDuplicate(check1.Trim()))
                //    {
                //        MessageBox.Show("ข้อมูล รหัสกลุ่มปรเภท ซ้ำ");
                //        radGridView1.Rows[e.RowIndex].Cells["GroupCode"].Value = "";
                //        radGridView1.Rows[e.RowIndex].Cells["GroupCode"].IsSelected = true;

                //    }
                //}
        

            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Unit_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
           // MessageBox.Show(e.KeyCode.ToString());
        }

        private void radGridView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
           // MessageBox.Show(e.KeyCode.ToString());

            if(e.KeyData==(Keys.Control|Keys.S))
            {
                if (MessageBox.Show("ต้องการบันทึก ?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //AddUnit();
                    //DataLoad();
                }
            }
        }

        private void Cleardata()
        {
            chkGET.Checked = true;
            btnGET.Enabled = false;
            txtInternalNo.Text = "";
            txtInternalName.Text = "";
            txtCustomerPartName.Text = "";
            txtCustomerPartNo.Text = "";
            cboGroupType.Text = "";
            cboTypeCode.Text = "";
            ddlInventoryGroup.Text = "";
            cboVendorName.Text = "";
            txtVendorNo.Text = "";
            cboBaseUOM.Text = "PCS";
            cboPurchaseUOM.Text = "PCS";
            cboSalseUOM.Text = "PCS";
            cboConsumptionUOM.Text = "PCS";
            cboVatType.Text = "";
            ddlReplenishmentType.Text = "Purchase";
            seInternalLeadTime.Value = 0;
            ddlReOrderType.Text = "";
            seMaximum.Value = 1;
            seMinimum.Value =0;
            seReOrderPoint.Value = 0;
            seReOrderQty.Value= 0;
            seSafetyStock.Value = 0;
            txtDrawing.Text = "";
            lbStatus.Text = "-";
            txtUpdateBy.Text = "";
            txtUpdateDate.Text = "";
            seTimebucket.Value = 0;


            txtCreateby.Text = ClassLib.Classlib.User;
            txtCreateDate.Text = Convert.ToDateTime( DateTime.Now,new CultureInfo("en-US")).ToString("dd/MMM/yyyy");

            chkDWG.Checked = false;
            lblTempAddFile.Text = "";
            dt_Part.Rows.Clear();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Ac = "Del";
                if (MessageBox.Show("ต้องการลบรายการ ( " + txtInternalNo.Text + " ) หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    {
                        var g = (from ix in db.mh_Items
                                 where ix.InternalNo.Trim() == txtInternalNo.Text.Trim() && ix.Active == true
                                 select ix).ToList();
                        if (g.Count > 0)  //มีรายการในระบบ
                        {
                                     var gg = (from ix in db.mh_Items
                                               where ix.InternalNo.Trim() == txtInternalNo.Text.Trim() && ix.Active == true
                                          select ix).First();
                               gg.Active = false;
                               gg.ModifyBy = ClassLib.Classlib.User;
                               gg.ModifyDate = Convert.ToDateTime(DateTime.Now,new CultureInfo("en-US"));
                            dbClss.AddHistory(this.Name  , "ลบทูล", "ลบทูล [" +txtInternalNo.Text.Trim() + "]", txtInternalNo.Text);                              
                            
                            db.SubmitChanges();
                            btnNew_Click(null, null);
                            btnSave.Enabled = true;
                            btnGET.Enabled = false;
                            chkGET.Checked = false;
                        }
                        else // ไม่มีในระบบ
                        {
                            //btnGET.Enabled = true;
                            chkGET.Checked = true;
                            Cleardata();
                            Enable_Status(true, "New");
                            btnSave.Enabled = true;
                        }
                    }

                    MessageBox.Show("ลบรายการ สำเร็จ!");
                }
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        int row = -1;
        private void radGridView1_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            row = e.RowIndex;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("รอฟอแมต์");
            return;

            //dbClss.ExportGridCSV(radGridView1);
            //dbClss.ExportGridXlSX(radGridView1);
            try
            {
                string tagetpart= System.IO.Path.GetTempPath();
                string Name = "Excel_001_Part_Export";
                string FileName = AppDomain.CurrentDomain.BaseDirectory + "Report\\Excel_001_Part_Export.xlsx";
               //string  FileOpen = Path.GetTempPath() + "Excel_001_Part_Export.xlsx";

                if (!System.IO.Directory.Exists(tagetpart))  //เช็คว่ามี partไฟล์เก็บหรือไม่ถ้าไม่ให้สร้างใหม่
                {
                    System.IO.Directory.CreateDirectory(tagetpart);
                }


                string File_temp = Name + "" + Path.GetExtension(FileName);  // IMG_IT-0123.jpg
                File.Copy(FileName, tagetpart + File_temp, true);//ต้องทำเสมอ เป็นการ ก็อปปี้ Path เพื่อให้รูป มาว่างไว้ที่ path นี้ 
                MessageBox.Show("Export Finished");
                System.Diagnostics.Process.Start(tagetpart + File_temp);

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void Import_1()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "Spread Sheet files (*.csv)|*.csv|All files (*.csv)|*.csv";
            if (op.ShowDialog() == DialogResult.OK)
            {

                using (TextFieldParser parser = new TextFieldParser(op.FileName))
                {
                    dt_Import.Rows.Clear();
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    int a = 0;
                    int c = 0;
                    while (!parser.EndOfData)
                    {
                        //Processing row
                        a += 1;
                        DataRow rd = dt_Import.NewRow();
                        //// MessageBox.Show(a.ToString());
                        string[] fields = parser.ReadFields();
                        c = 0;
                        foreach (string field in fields)
                        {
                            c += 1;
                            ////TODO: Process field
                            //    // MessageBox.Show(field);
                            if (a > 7)
                            {
                                if (c == 2)
                                    rd["CodeNo"] = Convert.ToString(field);
                                else if (c == 3)
                                    rd["ItemNo"] = StockControl.dbClss.TSt(field);
                                else if (c == 4)
                                    rd["ItemDescription"] = Convert.ToString(field);
                                else if (c == 5)
                                    rd["GroupCode"] = Convert.ToString(field);
                                else if (c == 6)
                                    rd["TypeCode"] = Convert.ToString(field);
                                else if (c == 7)
                                    rd["TypePart"] = Convert.ToString(field);
                                else if (c == 8)
                                    rd["VendorNo"] = Convert.ToString(field);
                                else if (c == 9)
                                    rd["VendorItemName"] = Convert.ToString(field);
                                else if (c == 10)
                                    rd["Maker"] = StockControl.dbClss.TSt(field);
                                else if (c == 11)
                                    rd["StandardCost"] = StockControl.dbClss.TDe(field);
                                else if (c == 12)
                                    rd["UnitBuy"] = Convert.ToString(field);
                                else if (c == 13)
                                    rd["UnitShip"] = Convert.ToString(field);
                                else if (c == 14)
                                    rd["PCSUnit"] = StockControl.dbClss.TDe(field);
                                else if (c == 15)
                                    rd["Leadtime"] = StockControl.dbClss.TDe(field);                               
                                else if (c == 16)
                                    rd["Replacement"] = Convert.ToString(field);
                                else if (c == 17)
                                    rd["StopOrder"] = StockControl.dbClss.TBo(field);                               
                                else if (c == 18)
                                    rd["MaximumStock"] = StockControl.dbClss.TDe(field);
                                else if (c == 19)
                                    rd["MinimumStock"] = StockControl.dbClss.TDe(field);
                                else if (c == 20)
                                    rd["Location"] = StockControl.dbClss.TSt(field);                              
                                else if (c == 21)
                                    rd["Toollife"] = StockControl.dbClss.TDe(field);
                                else if (c == 22)
                                    rd["Size"] = Convert.ToString(field);
                                else if (c == 23)
                                    rd["Remark"] = Convert.ToString(field);
                                else if (c == 24)
                                    rd["DWGNo"] = Convert.ToString(field);
                                else if (c == 25)
                                    rd["DWG"] = StockControl.dbClss.TBo(field);                              
                                else if (c == 26)
                                    rd["Status"] = Convert.ToString(field);
                                else if (c == 27)
                                    rd["BarCode"] = Convert.ToString(field);
                                else if (c == 28)
                                    rd["CreateBy"] = Convert.ToString(field);
                                else if (c == 29)
                                    rd["CreateDate"] = StockControl.dbClss.TDa(field);
                                //else if (c == 35)
                                //    rd["UpdateBy"] = Convert.ToString(field);
                                //else if (c == 36)
                                //    rd["UpdateDate"] = StockControl.dbClss.TDa(field);

                            }

                        }
                        dt_Import.Rows.Add(rd);
                    }
                }
                if (dt_Import.Rows.Count > 0)
                {
                    dbClss.AddHistory(this.Name  , "Import ทูล", "Import file CSV in to System", "Import ทูล");
                    //ImportData();
                    MessageBox.Show("Import Completed.");

                    //DataLoad();
                }

            }
        }
        private void btnImport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("รอฟอแมต์");
            return;
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "Spread Sheet files (*.csv)|*.csv|All files (*.csv)|*.csv";
            if (op.ShowDialog() == DialogResult.OK)
            {
                using (TextFieldParser parser = new TextFieldParser(op.FileName, Encoding.GetEncoding("windows-874")))
                {
                    this.Cursor = Cursors.WaitCursor;
                    //using (TextFieldParser parser = new TextFieldParser(op.FileName), Encoding.GetEncoding("windows-874")))
                    //{
                    dt_Import.Rows.Clear();
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    int a = 0;
                    int c = 0;

                    string CodeNo = "";
                    string ItemNo = "";
                    string ItemDescription = "";
                    string GroupCode = "";
                    string TypeCode = "";
                    string VendorNo = "";
                    string VendorItemName = "";
                    string Maker = "";
                    decimal StandardCost = 0;
                    string UnitBuy = "";
                    string UnitShip = "";
                    decimal PCSUnit = 0;
                    decimal Leadtime = 0;
                    string UseTacking = "";
                    string Replacement = "";
                    bool StopOrder = false;
                    string ShelfNo = "";
                    decimal MaximumStock = 0;
                    decimal MinimumStock = 0;
                    decimal SD = 0;
                    decimal ReOrderPoint = 0;
                    decimal Toollife = 0;
                    string Size = "";
                    string Remark = "";
                    string DWGNo = "";
                    bool DWG = false;
                    string CostingMethod = "";
                    string ItemGroup = "";
                    bool Critical = false;
                    decimal SafetyStock = 0;
                    string Status = "";
                    string BarCode = "";
                    string CreateBy = "";
                    string Location = "";
                    string TypePart = "";
                    DateTime? CreateDate = DateTime.Now;
                    string UpdateBy = "";
                    DateTime? UpdateDate = DateTime.Now;

                    while (!parser.EndOfData)
                    {
                        //Processing row
                        a += 1;
                        //DataRow rd = dt_Kanban.NewRow();
                        //// MessageBox.Show(a.ToString());
                        CodeNo = "";
                        ItemNo = "";
                        ItemDescription = "";
                        GroupCode = "";
                        TypeCode = "";
                        VendorNo = "";
                        VendorItemName = "";
                        Maker = "";
                        StandardCost = 0;
                        UnitBuy = "";
                        UnitShip = "";
                        PCSUnit = 0;
                        Leadtime = 0;
                        UseTacking = "";
                        Replacement = "";
                        StopOrder = false;
                        ShelfNo = "";
                        MaximumStock = 0;
                        MinimumStock = 0;
                        SD = 0;
                        ReOrderPoint = 0;
                        Toollife = 0;
                        Size = "";
                        Remark = "";
                        DWGNo = "";
                        DWG = false;
                        CostingMethod = "";
                        ItemGroup = "";
                        Critical = false;
                        SafetyStock = 0;
                        Status = "";
                        BarCode = "";
                        CreateBy = "";
                        CreateDate = DateTime.Now;
                        UpdateBy = "";
                        UpdateDate = DateTime.Now;
                        TypePart = "";
                        Location = "";
                        string[] fields = parser.ReadFields();
                        c = 0;
                        foreach (string field in fields)
                        {
                            c += 1;
                            ////TODO: Process field
                            //    // MessageBox.Show(field);
                            if (a > 7)
                            {
                                if (c == 3 && Convert.ToString(field).Equals(""))
                                {
                                    break;
                                }

                                if (c == 2)
                                    CodeNo = Convert.ToString(field);
                                else if (c == 3)
                                    ItemNo = StockControl.dbClss.TSt(field);
                                else if (c == 4)
                                    ItemDescription = Convert.ToString(field);
                                else if (c == 5)
                                    GroupCode = Convert.ToString(field);
                                else if (c == 6)
                                    TypeCode = Convert.ToString(field);
                                else if (c == 7)
                                    TypePart = Convert.ToString(field);
                                else if (c == 8)
                                    VendorNo = Convert.ToString(field);
                                else if (c == 9)
                                    VendorItemName = Convert.ToString(field);
                                else if (c == 10)
                                    Maker = Convert.ToString(field);
                                else if (c == 11)
                                    decimal.TryParse(Convert.ToString(field), out StandardCost); //StockControl.dbClss.TDe(field);
                                else if (c == 12)
                                    UnitBuy = Convert.ToString(field);
                                else if (c == 13)
                                    UnitShip = Convert.ToString(field);
                                else if (c == 14)
                                    decimal.TryParse(Convert.ToString(field), out PCSUnit); //StockControl.dbClss.TDe(field);
                                else if (c == 15)
                                    decimal.TryParse(Convert.ToString(field), out Leadtime);//= StockControl.dbClss.TDe(field);
                                //else if (c == 15)
                                //    UseTacking = Convert.ToString(field);
                                else if (c == 16)
                                    Replacement = Convert.ToString(field);
                                else if (c == 17)
                                    StopOrder = StockControl.dbClss.TBo(field);
                                //else if (c == 18)
                                //    ShelfNo = Convert.ToString(field);
                                else if (c == 18)
                                    decimal.TryParse(Convert.ToString(field), out MaximumStock);//= StockControl.dbClss.TDe(field);
                                else if (c == 19)
                                    decimal.TryParse(Convert.ToString(field), out MinimumStock);// = StockControl.dbClss.TDe(field);
                                else if (c == 20)
                                   Location = Convert.ToString(field);
                                //else if (c == 22)
                                //    decimal.TryParse(Convert.ToString(field), out ReOrderPoint);// = StockControl.dbClss.TDe(field);
                                else if (c == 21)
                                    ShelfNo = Convert.ToString(field);
                                else if (c == 22)
                                    decimal.TryParse(Convert.ToString(field), out Toollife);// = StockControl.dbClss.TDe(field);
                                else if (c == 23)
                                    Size = Convert.ToString(field);
                                else if (c == 24)
                                    Remark = Convert.ToString(field);
                                else if (c == 25)
                                    DWGNo = Convert.ToString(field);
                                else if (c == 26)
                                    DWG = StockControl.dbClss.TBo(field);
                                //else if (c == 28)
                                //    CostingMethod = Convert.ToString(field);
                                //else if (c == 29)
                                //    ItemGroup = Convert.ToString(field);
                                //else if (c == 30)
                                //    Critical = StockControl.dbClss.TBo(field);
                                //else if (c == 31)
                                //    decimal.TryParse(Convert.ToString(field), out SafetyStock);// = StockControl.dbClss.TDe(field);
                                else if (c == 27)
                                    Status = Convert.ToString(field);
                                else if (c == 28)
                                    BarCode = Convert.ToString(field);
                                else if (c == 29)
                                    CreateBy = Convert.ToString(field);
                                else if (c == 30 && !Convert.ToString(field).Equals(""))
                                    CreateDate = Convert.ToDateTime(StockControl.dbClss.TDa(field));
                                //else if (c == 36)
                                //    rd["UpdateBy"] = Convert.ToString(field);
                                //else if (c == 37)
                                //    rd["UpdateDate"] = StockControl.dbClss.TDa(field);

                            }
                           
                        }

                        if (!GroupCode.Equals("") && !TypeCode.Equals(""))
                        {
                            using (DataClasses1DataContext db = new DataClasses1DataContext())
                            {
                                var g = (from ix in db.tb_Items
                                         where ix.CodeNo.Trim().ToUpper() == CodeNo.Trim().ToUpper() //&& ix.Status == "Active"
                                         select ix).ToList();
                                if (g.Count <= 0)
                                {
                                    ////CodeNo = Get_CodeNo_GroupCode(GroupCode);
                                    //string Temp_codeno = CodeNo;
                                    //string temp_codeno2 = "";
                                    //if (CodeNo.Length > 5)
                                    //{
                                    //    int c1 = txtCodeNo.Text.Length;

                                    //    temp_codeno2 = Temp_codeno.Substring(5, c1 - 5);
                                    //    CodeNo = Get_CodeNo_GroupCode(GroupCode);
                                    //    CodeNo = CodeNo + temp_codeno2;
                                    //}
                                    //else
                                        CodeNo = Get_CodeNo_GroupCode(GroupCode);                                    
                                }
                            }
                        }
                        if (!TypePart.Equals(""))
                        {
                            if (TypePart.ToUpper() != "RM" && TypePart.ToUpper() != "FG" && TypePart.ToUpper() != "WIP"
                                && TypePart.ToUpper() != "Other"
                                )
                                TypePart = "";
                        }
                        if(!Status.Equals(""))
                        {
                            if (Status != "Active" && Status != "InActive")
                                Status = "Active";
                        }
                        if (!Location.Equals(""))
                        {
                            using (DataClasses1DataContext db = new DataClasses1DataContext())
                            {
                                var g = (from ix in db.tb_Locations
                                         where ix.Location.ToUpper().Trim() == Location.ToUpper().Trim() && ix.Status == "Completed" && ix.Active == true
                                         select ix).ToList();
                                if (g.Count > 0)
                                    Location = g.FirstOrDefault().Location;
                                else
                                    Location = "";
                            }
                            
                        }

                        //dt_Kanban.Rows.Add(rd);
                        if (CodeNo.ToString().Equals("") || ItemNo.ToString().Equals("")
                               || ItemDescription.ToString().Equals("") || GroupCode.ToString().Equals("")
                               || TypeCode.ToString().Equals("") || VendorNo.ToString().Equals("") 
                               || VendorItemName.ToString().Equals("")
                               || Maker.ToString().Equals("") || StandardCost.ToString().Equals("") 
                               || UnitBuy.ToString().Equals("")
                               || UnitShip.ToString().Equals("") || PCSUnit.ToString().ToString().Equals("") 
                               || Leadtime.ToString().Equals("")
                               || MaximumStock.ToString().Equals("") || MinimumStock.ToString().Equals("") 
                               || TypePart.ToString().Equals("") || Location.ToString().Equals("")
                               )
                        {
                            
                        }
                        else
                        {
                            //if (Status.Equals(""))
                            Status = "Active";
                            if (CreateBy.Equals(""))
                                CreateBy = ClassLib.Classlib.User;

                            Add_Item(CodeNo, ItemNo, ItemDescription, GroupCode, TypeCode, TypePart, VendorNo, VendorItemName
                                , Maker, StandardCost, UnitBuy, UnitShip, PCSUnit, Leadtime, Replacement, StopOrder, MaximumStock
                                , MinimumStock, Location, Toollife, Size, Remark, DWGNo, DWG, Status, BarCode
                                , CreateBy, Convert.ToDateTime(CreateDate), ShelfNo);

                            //dt_Import.Rows.Add(CodeNo, ItemNo, ItemDescription, GroupCode
                            //                               , TypeCode, UnitBuy, UnitShip, PCSUnit, ShelfNo, StandardCost,
                            //                               CostingMethod, ItemGroup, Replacement, VendorNo, VendorItemName, UseTacking
                            //                               , Critical, Leadtime, MaximumStock, MinimumStock
                            //                               , SafetyStock, ReOrderPoint, Status, StopOrder, Remark
                            //                               , Size, DWGNo, DWG, Maker, Toollife, SD
                            //                               , BarCode, CreateBy, CreateDate, UpdateBy, UpdateDate);
                        }
                    }


                }
                if (dt_Import.Rows.Count > 0)
                {
                    dbClss.AddHistory(this.Name, "Import ทูล", "Import file CSV in to System", "Import ทูล");
                    ImportData();
                    MessageBox.Show("Import Completed.");

                    //DataLoad();
                }
                //}
            }
            this.Cursor = Cursors.Default;

        }

        private void Add_Item(string CodeNo, string ItemNo, string ItemDescription
            , string GroupCode,string TypeCode,string TypePart,string VendorNo,string VendorItemName
            ,string Maker,decimal StandardCost,string UnitBuy,string UnitShip,decimal PCSUnit
            ,decimal Leadtime,string Replacement,bool StopOrder,decimal MaximumStock,decimal MinimumStock
            ,string Location,decimal Toollife,string Size,string Remark,string DWGNo,bool DWG,string Status,
            string BarCode,string CreateBy,DateTime CreateDate,string ShelfNo
            )
        {


            try
            {

                DataRow rd = dt_Import.NewRow();


                rd["CodeNo"] = CodeNo;

                rd["ItemNo"] = ItemNo;

                rd["ItemDescription"] = ItemDescription;

                rd["GroupCode"] = GroupCode;

                rd["TypeCode"] = TypeCode;

                rd["TypePart"] = TypePart;

                rd["VendorNo"] = VendorNo;

                rd["VendorItemName"] = VendorItemName;

                rd["Maker"] = Maker;

                rd["StandardCost"] = StandardCost;

                rd["UnitBuy"] = UnitBuy;

                rd["UnitShip"] = UnitShip;

                rd["PCSUnit"] = PCSUnit;

                rd["Leadtime"] = Leadtime;

                rd["Replacement"] = Replacement;

                rd["StopOrder"] = StopOrder;

                rd["MaximumStock"] = MaximumStock;

                rd["MinimumStock"] = MinimumStock;

                rd["Location"] = Location;

                rd["ShelfNo"] = ShelfNo;

                rd["Toollife"] = Toollife;

                rd["Size"] = Size;

                rd["Remark"] = Remark;

                rd["DWGNo"] = DWGNo;

                rd["DWG"] = DWG;

                rd["Status"] = Status;

                rd["BarCode"] = BarCode;

                rd["CreateBy"] = CreateBy;

                rd["CreateDate"] = CreateDate;



                dt_Import.Rows.Add(rd);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : Add_Item", this.Name); }

        }

        private void ImportData()
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                   
                    foreach (DataRow rd in dt_Import.Rows)
                    {
                        if (!rd["CodeNo"].ToString().Equals(""))
                        {

                            var g = (from ix in db.tb_Items
                                     where ix.CodeNo.Trim() == rd["CodeNo"].ToString().Trim() //&& ix.Status == "Active"
                                     select ix).ToList();
                            if (g.Count > 0)  //มีรายการในระบบ อัพเดต
                            {

                                var gg = (from ix in db.tb_Items
                                          where ix.CodeNo.Trim() == rd["CodeNo"].ToString().Trim()
                                          select ix).First();

                                gg.UpdateBy = rd["CreateBy"].ToString().Trim();
                                gg.UpdateDate = Convert.ToDateTime(rd["CreateDate"].ToString()); //DateTime.Now;
                                dbClss.AddHistory(this.Name  , "แก้ไข ทูล", " แก้ไข ทูล โดย Import โดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", rd["CodeNo"].ToString());

                                //if (StockControl.dbClss.TSt(gg.BarCode).Equals(""))
                                //    gg.BarCode = StockControl.dbClss.SaveQRCode2D(rd["CodeNo"].ToString());

                                gg.ItemNo = rd["ItemNo"].ToString().Trim();
                                gg.ItemDescription = rd["ItemDescription"].ToString().Trim();
                                gg.GroupCode = rd["GroupCode"].ToString().Trim();
                                gg.TypeCode = rd["TypeCode"].ToString().Trim();
                                gg.UnitBuy = rd["UnitBuy"].ToString().Trim();
                                gg.TypePart = rd["TypePart"].ToString().Trim();
                                gg.Location = rd["Location"].ToString().Trim();
                                gg.ShelfNo = rd["ShelfNo"].ToString().Trim();
                                gg.UnitShip = rd["UnitShip"].ToString().Trim();
                                decimal PCSUnit = 0; decimal.TryParse(rd["PCSUnit"].ToString(), out PCSUnit);
                                gg.PCSUnit = PCSUnit;
                                decimal StandardCost = 0; decimal.TryParse(rd["StandardCost"].ToString(), out StandardCost);
                                gg.StandardCost = StandardCost;
                                gg.Replacement = rd["Replacement"].ToString().Trim();
                                gg.VendorNo = rd["VendorNo"].ToString().Trim();
                                gg.VendorItemName = rd["VendorItemName"].ToString().Trim();
                                //gg.UseTacking = rd["UseTacking"].ToString().Trim();
                                decimal Leadtime = 0; decimal.TryParse(rd["Leadtime"].ToString(), out Leadtime);
                                gg.Leadtime = Leadtime;
                                decimal MaximumStock = 0; decimal.TryParse(rd["MaximumStock"].ToString(), out MaximumStock);
                                gg.MaximumStock = MaximumStock;
                                decimal MinimumStock = 0; decimal.TryParse(rd["MinimumStock"].ToString(), out MinimumStock);
                                gg.MinimumStock = MinimumStock;
                                decimal ReOrderPoint = 0; //decimal.TryParse(rd["ReOrderPoint"].ToString(), out ReOrderPoint);
                                gg.ReOrderPoint = ReOrderPoint;
                                bool StopOrder = StockControl.dbClss.TBo(rd["StopOrder"]);
                                gg.StopOrder = StopOrder;
                                gg.Remark = rd["Remark"].ToString().Trim();
                                gg.Size = rd["Size"].ToString();
                                gg.Maker = rd["Maker"].ToString().Trim();
                                decimal Toollife = 0; decimal.TryParse(rd["Toollife"].ToString(), out Toollife);
                                gg.Toollife = Toollife;
                                decimal SD = 0; //decimal.TryParse(rd["SD"].ToString(), out SD);
                                gg.SD = SD;
                                gg.DWGNo = rd["DWGNo"].ToString().Trim();
                                bool DWG = StockControl.dbClss.TBo(rd["DWG"]);
                                gg.DWG = DWG;

                                db.SubmitChanges();
                            }
                            else   // Add ใหม่
                            {
                                // byte[] barcode = StockControl.dbClss.SaveQRCode2D(rd["CodeNo"].ToString().Trim());
                                decimal StockDL = 0;
                                decimal StockInv = 0;
                                decimal StockBackOrder = 0;

                                decimal StandardCost = 0;
                                decimal MaximumStock = 0;
                                decimal MinimumStock = 0;
                                decimal SafetyStock = 0;
                                decimal ReOrderPoint = 0;
                                decimal SD = 0;
                                decimal Toollife = 0;
                                decimal Leadtime = 0;
                                bool Critical = false;
                                decimal PCSUnit = 0;
                                string CostingMethod = "";
                                string ItemGroup = "";
                                string UpdateBy = ClassLib.Classlib.User;
                                DateTime CreateDate = DateTime.Now;
                                UpdateBy = rd["CreateBy"].ToString().Trim();
                                CreateDate = Convert.ToDateTime(rd["CreateDate"].ToString()); //DateTime.Now;

                                decimal.TryParse(rd["StandardCost"].ToString(), out StandardCost);
                                decimal.TryParse(rd["MaximumStock"].ToString(), out MaximumStock);
                                decimal.TryParse(rd["MinimumStock"].ToString(), out MinimumStock);
                                //decimal.TryParse(rd["ReOrderPoint"].ToString(), out ReOrderPoint);
                                decimal.TryParse(rd["PCSUnit"].ToString(), out PCSUnit);
                                decimal.TryParse(rd["Leadtime"].ToString(), out Leadtime);
                                decimal.TryParse(rd["Toollife"].ToString(), out Toollife);
                                if (Toollife < 0)
                                    Toollife = 1;

                                //decimal.TryParse(rd["SD"].ToString(), out SD);

                                DateTime? UpdateDate = null;
                                
                                tb_Item u = new tb_Item();
                                u.CodeNo = rd["CodeNo"].ToString().Trim();
                                u.ItemNo = rd["ItemNo"].ToString().Trim();
                                u.ItemDescription = rd["ItemDescription"].ToString().Trim();
                                u.GroupCode = rd["GroupCode"].ToString();
                                u.TypeCode = rd["TypeCode"].ToString();
                                u.TypePart = rd["TypePart"].ToString();
                                u.Location = rd["Location"].ToString();
                                u.UnitBuy = rd["UnitBuy"].ToString();
                                u.VendorNo = rd["VendorNo"].ToString();
                                u.VendorItemName = rd["VendorItemName"].ToString().Trim();
                                u.Maker = rd["Maker"].ToString().Trim();
                                u.StandardCost = StandardCost;
                                u.UnitShip = rd["UnitShip"].ToString();
                                u.PCSUnit = PCSUnit;
                                u.Leadtime = Leadtime;
                                u.UseTacking = "";// rd["UseTacking"].ToString();
                                u.Replacement = rd["Replacement"].ToString();
                                u.StopOrder = StockControl.dbClss.TBo(rd["StopOrder"]);
                                u.ShelfNo =  rd["ShelfNo"].ToString();
                                u.MinimumStock = MinimumStock;
                                u.MaximumStock = MaximumStock;
                                u.SD = SD;
                                u.ReOrderPoint = ReOrderPoint;
                                u.Toollife = Toollife;
                                u.Size = rd["Size"].ToString();
                                u.Remark = rd["Remark"].ToString();
                                u.CreateBy = UpdateBy;
                                u.CreateDate = CreateDate;
                                u.UpdateDate = UpdateDate;
                                u.UpdateBy = "";
                                u.SafetyStock = SafetyStock;
                                u.Critical = Critical;
                                u.Status = rd["Status"].ToString();
                                u.CostingMethod = CostingMethod;
                                u.ItemGroup = ItemGroup;
                                u.BarCode = null;// barcode;
                                u.DWGNo = rd["DWGNo"].ToString();
                                u.DWG = StockControl.dbClss.TBo(rd["DWG"]);
                                u.StockDL = StockDL;
                                u.StockInv = StockInv;
                                u.StockBackOrder = StockBackOrder;
                                u.AvgCost = 0;

                                db.tb_Items.InsertOnSubmit(u);
                                db.SubmitChanges();

                                dbClss.AddHistory(this.Name  ,"เพิ่ม ทูล", "เพิ่ม ทูล โดย Import [" + u.CodeNo + "]", u.CodeNo);

                            }
                        }
                    }
                   
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message);
                dbClss.AddError("ImportData Part", ex.Message, this.Name);
            }
        }

        private void btnFilter1_Click(object sender, EventArgs e)
        {
           // radGridView1.EnableFiltering = true;
        }

        private void btnUnfilter1_Click(object sender, EventArgs e)
        {
           // radGridView1.EnableFiltering = false;
        }

        private void radMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboVendor_SelectedValueChanged(object sender, EventArgs e)
        {
           
        }

        private void txtVenderName_TextChanged(object sender, EventArgs e)
        {
            if (Cath01 == 0)
            {

                //VNDR = cboVendor.Text;
                //VNDRName = txtVenderName.Text;
                DataLoad();
            }
        }

        private void cboVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    //if(Cath01==0)
            //    //txtVenderName.Text = cboVendor.SelectedValue.ToString();
            //    if (!cboVendor.Text.Equals(""))
            //    {
            //        using (DataClasses1DataContext db = new DataClasses1DataContext())
            //        {
            //            var I = (from ix in db.tb_Vendors select ix).Where(a => a.VendorName == cboVendor.Text).ToList();
            //            if (I.Count > 0)
            //                txtVenderName.Text = I.FirstOrDefault().VendorNo;
            //        }
            //    }
            //}
            //catch { }
        }

        private void radGridView1_CellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
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
                    
                }
            }
        }

        private void radLabel25_Click(object sender, EventArgs e)
        {

        }

        private void radPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radDropDownList1_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {

        }

        private void radLabel27_Click(object sender, EventArgs e)
        {

        }

        private void radTextBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            if (!txtDrawing.Text.Trim().Equals(""))
            {

                //OpenFileDialog op = new OpenFileDialog();
                //op.Filter = "PDF files (*.pdf)|*.pdf";
                //op.FileName = txtDwgfile.Text;
                //op.ShowDialog();
                try
                {

                    OpenFileDialog op = new OpenFileDialog();
                    op.DefaultExt = "*.pdf";
                    op.AddExtension = true;
                    op.FileName = "";
                    op.Filter = "PDF files (*.pdf)|*.pdf";
                    this.Cursor = Cursors.WaitCursor;
                    if (op.ShowDialog() == DialogResult.OK)
                    {
                        string FileName = op.FileName;
                        string tagetpart = lblPath.Text;

                        if (!Ac.Equals("New")) // save ได้เรย
                        {
                            if (!System.IO.Directory.Exists(tagetpart))  //เช็คว่ามี partไฟล์เก็บหรือไม่ถ้าไม่ให้สร้างใหม่
                            {
                                System.IO.Directory.CreateDirectory(tagetpart);
                            }
                            //System.IO.File.Copy()

                            string File_temp = txtInternalNo.Text + "_" + ".pdf";//Path.GetExtension(AttachFile);  // IMG_IT-0123.jpg
                            File.Copy(FileName, tagetpart + File_temp, true);//ต้องทำเสมอ เป็นการ ก็อปปี้ Path เพื่อให้รูป มาว่างไว้ที่ path นี้ 

                            if(chkDWG.Checked)
                                dbClss.AddHistory(this.Name  , "แก้ไข Drawing", "แก้ไขไฟล์ Drawing [" + txtInternalNo.Text.Trim() + "]", txtInternalNo.Text);
                            else
                                dbClss.AddHistory(this.Name , "แก้ไข Drawing", "เพิ่มไฟล์ Drawing [" + txtInternalNo.Text.Trim() + "]", txtInternalNo.Text);


                            chkDWG.Checked = true;
                            using (DataClasses1DataContext db = new DataClasses1DataContext())
                            {
                                var g = (from ix in db.mh_Items
                                         where ix.InternalNo.Trim() == txtInternalNo.Text.Trim() && ix.Active == true
                                         select ix).ToList();
                                if (g.Count > 0)  //มีรายการในระบบ
                                {
                                    var gg = (from ix in db.mh_Items
                                              where ix.InternalNo.Trim() == txtInternalNo.Text.Trim() && ix.Active == true
                                              select ix).First();
                                    gg.chkDrawing = chkDWG.Checked;
                                    gg.Drawing = txtDrawing.Text;
                                    gg.ModifyBy = ClassLib.Classlib.User;
                                    gg.ModifyDate = Convert.ToDateTime(DateTime.Now,new CultureInfo("en-US"));
                                    db.SubmitChanges();
                                }
                            }

                        }
                        else
                        {
                            lblTempAddFile.Text = FileName;

                        }

                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError("CreatePart", ex.Message + " : radButton1_Click", this.Name); }
                finally { this.Cursor = Cursors.Default; }
            }
            else { MessageBox.Show("ต้องใส่ Drawing No.!"); }
            
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            if (chkDWG.Checked.Equals(true))
            {
                System.IO.File.Delete(lblPath.Text + txtInternalNo.Text + "_.pdf");
                chkDWG.Checked = false;


                dbClss.AddHistory(this.Name  , "ลบไฟล์ Drawing", "ลบไฟล์ Drawing [" + txtInternalNo.Text.Trim() + "]", txtInternalNo.Text);

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var g = (from ix in db.mh_Items
                             where ix.InternalNo.Trim() == txtInternalNo.Text.Trim() && ix.Active == true
                             select ix).ToList();
                    if (g.Count > 0)  //มีรายการในระบบ
                    {
                        var gg = (from ix in db.mh_Items
                                  where ix.InternalNo.Trim() == txtInternalNo.Text.Trim() && ix.Active == true
                                  select ix).First();
                        gg.chkDrawing = chkDWG.Checked;
                        gg.ModifyBy = ClassLib.Classlib.User;
                        gg.ModifyDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                        db.SubmitChanges();
                    }
                }
            }
            else
            {
                lblTempAddFile.Text = "";
            }
            MessageBox.Show("ลบไฟล์ Drawing เรียบร้อย");
        }

        private void radButtonElement1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Comming soon!");
            try
            {
                btnEdit.Enabled = true;
               
                btnNew.Enabled = true;
                
                Cleardata();
                Enable_Status(false, "View");
                

                this.Cursor = Cursors.WaitCursor;
                ListPart sc = new ListPart(txtInternalNo,"All","CreatePart");
                this.Cursor = Cursors.Default;
                sc.ShowDialog();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();
                //LoadData
                DataLoad();
                btnGET.Enabled = false;
                btnView.Enabled = false;
                chkGET.Enabled = false;
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError("CreatePart", ex.Message + " : radButtonElement1_Click", this.Name); }

        }

        private void btnGET_Click(object sender, EventArgs e)
        {
            try
            {
              if(!cboGroupType.Text.Trim().Equals(""))
                {
                    //txtCodeNo.Text = "I0001";
                    txtInternalNo.Text = Get_CodeNo();
                }
                else
                {
                    MessageBox.Show("ต้องเลือกประเภทกลุ่มก่อนเสมอ!!");
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : btnGET_Click", this.Name); }
        }
        private string Get_CodeNo()
        {
            string re = "";
            try
            {
                this.Cursor = Cursors.WaitCursor;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    string Temp_Running = "";
                    var I = (from ix in db.mh_GroupTypes select ix).Where(a => a.GroupCode == cboGroupType.Text).ToList();
                    if (I.Count > 0)
                        Temp_Running = I.FirstOrDefault().Running;

                    if (!Temp_Running.Equals(""))
                    {
                        var g = (from ix in db.mh_Items select ix).Where(a => a.InternalNo.Contains(Temp_Running)).OrderByDescending(b => b.InternalNo).ToList();
                        if (g.Count > 0)
                        {
                            //string temp = g.FirstOrDefault().CodeNo;
                            //int c =  Convert.ToInt32(g.FirstOrDefault().CodeNo.Substring(1, 4)) + 1;
                            int cou = Temp_Running.Length;
                            string CodeNo_t = g.FirstOrDefault().InternalNo;

                            int c = Convert.ToInt32(g.FirstOrDefault().InternalNo.Substring(cou, CodeNo_t.Length-cou)) + 1;

                            if (c.ToString().Count().Equals(1))
                                re = Temp_Running + "000"+ c.ToString();
                            else if (c.ToString().Count().Equals(2))
                                re = Temp_Running + "00" + c.ToString();
                            else if (c.ToString().Count().Equals(3))
                                re = Temp_Running + "0" + c.ToString(); 
                            else 
                                re = Temp_Running  + c.ToString();
                        }
                        else
                            re = Temp_Running + "0001";
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message+" : Get_CodeNo", this.Name); }
            this.Cursor = Cursors.Default;
            return re;
        }
        private string Get_CodeNo_GroupCode(string GroupCode)
        {
            string re = "";
            try
            {
                this.Cursor = Cursors.WaitCursor;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    string Temp_Running = "";
                    var I = (from ix in db.mh_GroupTypes select ix).Where(a => a.GroupCode == GroupCode).ToList();
                    if (I.Count > 0)
                        Temp_Running = I.FirstOrDefault().Running;

                    if (!Temp_Running.Equals(""))
                    {
                        var g = (from ix in db.mh_Items select ix).Where(a => a.InternalNo.Contains(Temp_Running)).OrderByDescending(b => b.InternalNo).ToList();
                        if (g.Count > 0)
                        {
                            //string temp = g.FirstOrDefault().CodeNo;
                            //int c =  Convert.ToInt32(g.FirstOrDefault().CodeNo.Substring(1, 4)) + 1;
                            int cou = Temp_Running.Length;
                            string CodeNo_t = g.FirstOrDefault().InternalNo;

                            int c = Convert.ToInt32(g.FirstOrDefault().InternalNo.Substring(cou, CodeNo_t.Length - cou)) + 1;


                            if (c.ToString().Count().Equals(1))
                                re = Temp_Running + "000" + c.ToString();
                            else if (c.ToString().Count().Equals(2))
                                re = Temp_Running + "00" + c.ToString();
                            else if (c.ToString().Count().Equals(3))
                                re = Temp_Running + "0" + c.ToString();
                            else
                                re = Temp_Running + c.ToString();

                        }
                        else
                            re = Temp_Running + "0001";
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : Get_CodeNo", this.Name); }
            this.Cursor = Cursors.Default;
            return re;
        }
        private void cboGroupType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DefaultType();
            if (cboTypeCode.Text.Equals(""))
                cboTypeCode.Text = cboGroupType.Text;
        }

        private void txtStandCost_KeyPress(object sender, KeyPressEventArgs e)
        {
            StockControl.dbClss.CheckDigitDecimal(e);
        }

        private void txtStandCost_Leave(object sender, EventArgs e)
        {
            //decimal temp = 0;
            //decimal.TryParse(txtStandCost.Text, out temp);
            //temp = decimal.Round(temp, 2);
            //txtStandCost.Text = (temp).ToString();
        }

        private void txtPCSUnit_KeyPress(object sender, KeyPressEventArgs e)
        {
            StockControl.dbClss.CheckDigitDecimal(e);
        }

        private void txtPCSUnit_Leave(object sender, EventArgs e)
        {
            //decimal temp = 0;
            //decimal.TryParse(txtPCSUnit.Text, out temp);
            //temp = decimal.Round(temp, 2);
            //txtPCSUnit.Text = (temp).ToString();
        }

        private void txtLeadTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            StockControl.dbClss.CheckDigitDecimal(e);
        }

        private void txtLeadTime_Leave(object sender, EventArgs e)
        {
            //decimal temp = 0;
            //decimal.TryParse(txtLeadTime.Text, out temp);
            //temp = decimal.Round(temp, 2);
            //txtLeadTime.Text = (temp).ToString();
        }

        private void txtMaximumStock_KeyPress(object sender, KeyPressEventArgs e)
        {
           
            StockControl.dbClss.CheckDigitDecimal(e);
        }

        private void txtMaximumStock_Leave(object sender, EventArgs e)
        {
            //decimal temp = 0;
            //decimal.TryParse(txtMaximumStock.Text, out temp);
            //temp = decimal.Round(temp, 2);
            //txtMaximumStock.Text = (temp).ToString();
        }

        private void txtMimimumStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            StockControl.dbClss.CheckDigitDecimal(e);
        }

        private void txtMimimumStock_Leave(object sender, EventArgs e)
        {
            //decimal temp = 0;
            //decimal.TryParse(txtMimimumStock.Text, out temp);
            //temp = decimal.Round(temp, 2);
            //txtMimimumStock.Text = (temp).ToString();
        }

        private void txtErrorLeadtime_KeyPress(object sender, KeyPressEventArgs e)
        {
            StockControl.dbClss.CheckDigitDecimal(e);
        }

        private void txtErrorLeadtime_Leave(object sender, EventArgs e)
        {
            //decimal temp = 0;
            //decimal.TryParse(txtErrorLeadtime.Text, out temp);
            //temp = decimal.Round(temp, 2);
            //txtErrorLeadtime.Text = (temp).ToString();
        }

        private void txtToolLife_KeyPress(object sender, KeyPressEventArgs e)
        {
            StockControl.dbClss.CheckDigitDecimal(e);
        }

        private void txtToolLife_Leave(object sender, EventArgs e)
        {
            //decimal temp = 0;
            //decimal.TryParse(txtToolLife.Text, out temp);
            //temp = decimal.Round(temp, 2);
            //txtToolLife.Text = (temp).ToString();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            string CodeNo = txtInternalNo.Text;
            Cleardata();
            Enable_Status(false, "View");
            txtInternalNo.Text = CodeNo;
            DataLoad();
            Ac = "View";
            btnGET.Enabled = false;
            chkGET.Enabled = false;
        }

        private void btnOpenDWG_Click(object sender, EventArgs e)
        {
            if (chkDWG.Checked.Equals(true))
            {
                System.Diagnostics.Process.Start(lblPath.Text+txtInternalNo.Text+"_.pdf");
            }
            else if (!lblTempAddFile.Text.Equals(""))  //กรณียังไม่ได้ save  
            {
                System.Diagnostics.Process.Start(lblTempAddFile.Text);
            }
            else
                MessageBox.Show("ไม่มีพบไฟล์ Drawing");
        }

        DataTable dt_ShelfTag = new DataTable();
        DataTable dt_Kanban = new DataTable();

        private void Set_dt_Print()
        {
            dt_ShelfTag.Columns.Add(new DataColumn("CodeNo", typeof(string)));
            dt_ShelfTag.Columns.Add(new DataColumn("PartDescription", typeof(string)));
            dt_ShelfTag.Columns.Add(new DataColumn("ShelfNo", typeof(string)));


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
        private void btnPrintShelfTAG_Click(object sender, EventArgs e)
        {
            try
            {
                dt_ShelfTag.Rows.Clear();

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var g = (from ix in db.tb_Items select ix).Where(a => a.CodeNo == txtInternalNo.Text).ToList();
                    if (g.Count() > 0)
                    {
                        //foreach(var gg in g)
                        //{
                        //    dt_ShelfTag.Rows.Add(gg.CodeNo, gg.ItemDescription, gg.ShelfNo);
                        //}
                        //DataTable DT =  StockControl.dbClss.LINQToDataTable(g);
                        //Reportx1 po = new Reportx1("Report_PurchaseRequest_Content1.rpt", DT, "FromDT");
                        //po.Show();
                        var deleteItem = (from ii in db.TempPrintShelfs where ii.UserName == ClassLib.Classlib.User select ii);
                        foreach (var d in deleteItem)
                        {
                            db.TempPrintShelfs.DeleteOnSubmit(d);
                            db.SubmitChanges();
                        }
                        TempPrintShelf ps = new TempPrintShelf();
                        ps.UserName = ClassLib.Classlib.User;
                        ps.CodeNo = g.FirstOrDefault().CodeNo;
                        ps.PartDescription = g.FirstOrDefault().ItemDescription;
                        ps.PartNo = g.FirstOrDefault().ItemNo;
                        ps.ShelfNo = g.FirstOrDefault().ShelfNo;
                        ps.Max = Convert.ToDecimal(g.FirstOrDefault().MaximumStock);
                        ps.Min = Convert.ToDecimal(g.FirstOrDefault().MinimumStock);
                        ps.OrderPoint = Convert.ToDecimal(g.FirstOrDefault().ReOrderPoint);
                        db.TempPrintShelfs.InsertOnSubmit(ps);
                        db.SubmitChanges();

                        Report.Reportx1.Value = new string[2];
                        Report.Reportx1.Value[0] = ClassLib.Classlib.User;
                        Report.Reportx1.WReport = "002_BoxShelf_Part";
                        Report.Reportx1 op = new Report.Reportx1("002_BoxShelf_Part.rpt");
                        op.Show();
                       
                    }
                    else
                        MessageBox.Show("not found.");
                }

            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnPrintBarCode_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    dt_Kanban.Rows.Clear();
            //    this.Cursor = Cursors.WaitCursor;
            //    using (DataClasses1DataContext db = new DataClasses1DataContext())
            //    {

            //        var g = (from ix in db.tb_Items select ix).Where(a => a.CodeNo == txtInternalNo.Text).ToList();
            //        if (g.Count() > 0)
            //        {
            //            // Step 1 delete UserName
            //            var deleteItem = (from ii in db.TempPrintKanbans where ii.UserName == ClassLib.Classlib.User select ii);
            //            foreach(var d in deleteItem)
            //            {
            //                db.TempPrintKanbans.DeleteOnSubmit(d);
            //                db.SubmitChanges();
            //            }

            //            // Step 2 Insert to Table
            //            TempPrintKanban tm = new TempPrintKanban();
            //            tm.UserName = ClassLib.Classlib.User;
            //            tm.CodeNo = g.FirstOrDefault().CodeNo;
            //            tm.PartDescription = g.FirstOrDefault().ItemDescription;
            //            tm.PartNo = g.FirstOrDefault().ItemNo;
            //            tm.VendorName = g.FirstOrDefault().VendorItemName;
            //            tm.ShelfNo = g.FirstOrDefault().ShelfNo;
            //            tm.GroupType = g.FirstOrDefault().GroupCode;
            //            tm.Max=Convert.ToDecimal(g.FirstOrDefault().MaximumStock);
            //            tm.Min= Convert.ToDecimal(g.FirstOrDefault().MinimumStock);
            //            tm.ReOrderPoint= Convert.ToDecimal(g.FirstOrDefault().ReOrderPoint);
            //            tm.ToolLife= Convert.ToDecimal(g.FirstOrDefault().Toollife);
            //            byte[] barcode = StockControl.dbClss.SaveQRCode2D(g.FirstOrDefault().CodeNo);
            //            tm.BarCode = barcode;
            //            tm.Location = ddlLocation.Text;
            //            tm.TypePart = ddlInventoryGroup.Text;

            //            db.TempPrintKanbans.InsertOnSubmit(tm);
            //            db.SubmitChanges();
            //            this.Cursor = Cursors.Default;
            //            // Step 3 Call Report
            //            Report.Reportx1.Value = new string[2];
            //            Report.Reportx1.Value[0] = ClassLib.Classlib.User;
            //            Report.Reportx1.WReport = "001_Kanban_Part";
            //            Report.Reportx1 op = new Report.Reportx1("001_Kanban_Part.rpt");
            //            op.Show();

            //            //foreach (var gg in g)
            //            //{
            //            //    dt_Kanban.Rows.Add(gg.CodeNo, gg.ItemNo, gg.ItemDescription, gg.ShelfNo, gg.Leadtime, gg.VendorItemName, gg.GroupCode, gg.Toollife, gg.MaximumStock, gg.MinimumStock,gg.ReOrderPoint, gg.BarCode);
            //            //}
            //            //DataTable DT =  StockControl.dbClss.LINQToDataTable(g);
            //            //Reportx1 po = new Reportx1("Report_PurchaseRequest_Content1.rpt", DT, "FromDT");
            //            //po.Show();


            //        }
            //        else
            //            MessageBox.Show("not found.");
            //    }

            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
            //this.Cursor = Cursors.Default;
        }

        private void txtMimimumStock_TextChanged(object sender, EventArgs e)
        {
            //txtReOrderPoint.Text = "0.00";
        }

        private void cboVendor_Leave(object sender, EventArgs e)
        {
            cboVendor_SelectedIndexChanged(null, null);
        }

        private void cboGroupType_Leave(object sender, EventArgs e)
        {
            cboGroupType_SelectedIndexChanged(null, null);
        }

        private void lblStock_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Stock_List a = new Stock_List(txtInternalNo.Text, "Invoice");
                a.Show();
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;
        }

        private void lblTempStock_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Stock_List a = new Stock_List(txtInternalNo.Text, "Temp");
                a.Show();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;
        }

        private void lblOrder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Stock_List a = new Stock_List(txtInternalNo.Text, "BackOrder");
                a.Show();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;
        }

        private void chkGET_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if (chkGET.Checked.Equals(true))
            {
                btnGET.Enabled = false;
                txtInternalNo.Enabled = true;
            }
            else
            {
                btnGET.Enabled = true;
                txtInternalNo.Enabled = false;
            }
        }

        private void btnPrintStockCard_Click(object sender, EventArgs e)
        {

            PrintPR a = new PrintPR(txtInternalNo.Text, txtInternalNo.Text, "ReportStockMovement");
            a.ShowDialog();
            
        }

        private void cboGroupType_ToolTipTextNeeded(object sender, Telerik.WinControls.ToolTipTextNeededEventArgs e)
        {
            //try
            //{
            //    //e.ToolTipText = cell.Value.ToString();

            //    string aa = cboGroupType.Select. ;//cboGroupType.SelectedItem.ToString();
            //    e.ToolTipText = aa;
            //}
            //catch { }
        }

        private void btnCal_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    db.sp_049_Cal_BalanceStock_List(txtInternalNo.Text, txtInternalNo.Text);
                    MessageBox.Show("คำนวณสำเร็จ");
                }
                }catch(Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void cboVendorName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if(Cath01==0)
                //txtVenderName.Text = cboVendor.SelectedValue.ToString();
                if (!cboVendorName.Text.Equals(""))
                {
                    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    {
                        var I = (from ix in db.mh_Vendors select ix).Where(a => a.Name == cboVendorName.Text).ToList();
                        if (I.Count > 0)
                            txtVendorNo.Text = I.FirstOrDefault().No;
                    }
                }
            }
            catch { }
        }

        private void cboVendorName_Leave(object sender, EventArgs e)
        {
            cboVendorName_SelectedIndexChanged(null, null);
        }
    }
}
