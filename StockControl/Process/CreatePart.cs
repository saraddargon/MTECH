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
using Telerik.WinControls;
using Telerik.WinControls.UI;

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


        DataTable dt_HD_Bom = new DataTable();
        DataTable dt_DT_Bom = new DataTable();

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
            dt_Part.Columns.Add(new DataColumn("InternalDescription", typeof(string)));
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
            dt_Part.Columns.Add(new DataColumn("MaximumInventory", typeof(decimal)));
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
            dt_Part.Columns.Add(new DataColumn("Inspection", typeof(bool)));
            dt_Part.Columns.Add(new DataColumn("Location", typeof(string)));
            dt_Part.Columns.Add(new DataColumn("Taking", typeof(bool)));
            dt_Part.Columns.Add(new DataColumn("ShelfNo", typeof(bool)));
            dt_Part.Columns.Add(new DataColumn("StandardPrice", typeof(decimal)));
            dt_Part.Columns.Add(new DataColumn("StandardCost", typeof(decimal)));
         
            //dt_Import
            dt_Import = new DataTable();
            dt_Import.Columns.Add(new DataColumn("id", typeof(int)));
            dt_Import.Columns.Add(new DataColumn("InternalNo", typeof(string)));
            dt_Import.Columns.Add(new DataColumn("InternalName", typeof(string)));
            dt_Import.Columns.Add(new DataColumn("InternalDescription", typeof(string)));
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
            dt_Import.Columns.Add(new DataColumn("MaximumInventory", typeof(decimal)));
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
            dt_Import.Columns.Add(new DataColumn("Inspection", typeof(bool)));
            dt_Import.Columns.Add(new DataColumn("Location", typeof(string)));
            dt_Import.Columns.Add(new DataColumn("Taking", typeof(bool)));
            dt_Import.Columns.Add(new DataColumn("ShelfNo", typeof(bool)));
            dt_Import.Columns.Add(new DataColumn("StandardPrice", typeof(decimal)));
            dt_Import.Columns.Add(new DataColumn("StandardCost", typeof(bool)));


            dt_HD_Bom.Columns.Add(new DataColumn("id", typeof(string)));
            dt_HD_Bom.Columns.Add(new DataColumn("PartNo", typeof(string)));
            dt_HD_Bom.Columns.Add(new DataColumn("BomNo", typeof(string)));
            dt_HD_Bom.Columns.Add(new DataColumn("Year_", typeof(string)));
            dt_HD_Bom.Columns.Add(new DataColumn("Month_", typeof(string)));
            dt_HD_Bom.Columns.Add(new DataColumn("Description", typeof(string)));
            dt_HD_Bom.Columns.Add(new DataColumn("Status", typeof(string)));
            dt_HD_Bom.Columns.Add(new DataColumn("Remark", typeof(string)));
            dt_HD_Bom.Columns.Add(new DataColumn("StartDate", typeof(DateTime)));
            dt_HD_Bom.Columns.Add(new DataColumn("EndDate", typeof(DateTime)));
            dt_HD_Bom.Columns.Add(new DataColumn("CreateBy", typeof(string)));
            dt_HD_Bom.Columns.Add(new DataColumn("CreateDate", typeof(DateTime)));
            dt_HD_Bom.Columns.Add(new DataColumn("ModifyBy", typeof(string)));
            dt_HD_Bom.Columns.Add(new DataColumn("ModifyDate", typeof(DateTime)));
            dt_HD_Bom.Columns.Add(new DataColumn("ShelfNo", typeof(DateTime)));


            dt_DT_Bom.Columns.Add(new DataColumn("id", typeof(string)));
            dt_DT_Bom.Columns.Add(new DataColumn("PartNo", typeof(string)));
            dt_DT_Bom.Columns.Add(new DataColumn("BomNo", typeof(string)));
            dt_DT_Bom.Columns.Add(new DataColumn("Year_", typeof(string)));
            dt_DT_Bom.Columns.Add(new DataColumn("Month_", typeof(string)));
            dt_DT_Bom.Columns.Add(new DataColumn("Line", typeof(int)));
            dt_DT_Bom.Columns.Add(new DataColumn("Component", typeof(string)));
            dt_DT_Bom.Columns.Add(new DataColumn("Qty", typeof(decimal)));
            dt_DT_Bom.Columns.Add(new DataColumn("Unit", typeof(string)));
            dt_DT_Bom.Columns.Add(new DataColumn("PCSUnit", typeof(decimal)));
            dt_DT_Bom.Columns.Add(new DataColumn("UnitCost", typeof(decimal)));
            dt_DT_Bom.Columns.Add(new DataColumn("Cost", typeof(decimal)));
        }
        private void Unit_Load(object sender, EventArgs e)
        {
            radPageView1.Pages[1].Item.Visibility = ElementVisibility.Collapsed;

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
                txtInternalDesc.Enabled = ss;
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
                seMaximumInventory.Enabled = ss;
                cbInspaction.Enabled = ss;
                cboLocation.Enabled = ss;
                cbTakingLot.Enabled = ss;
                txtShelfNo.Enabled = ss;
                txtBom.Enabled = ss;
                cboBOM.Enabled = ss;
                txtVersion.Enabled = ss;
                cboRouting.Enabled = ss;
                seStandardPrice.Enabled = ss;
                seStandardCost.Enabled = ss;

                btnAddDWG.Enabled = ss;
                btnDeleteDWG.Enabled = ss;
                chkGET.Checked = false;
                //btnGET.Enabled = true;

                btnAdd_Item.Enabled = ss;
                btnDel_Item.Enabled = ss;

            }
            else if (Condition.Equals("View"))
            {
                txtInternalDesc.Enabled = ss;
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
                seMaximumInventory.Enabled = ss;
                cbInspaction.Enabled = ss;
                cboLocation.Enabled = ss;
                cbTakingLot.Enabled = ss;
                txtShelfNo.Enabled = ss;
                txtBom.Enabled = ss;
                cboBOM.Enabled = ss;
                txtVersion.Enabled = ss;
                cboRouting.Enabled = ss;
                seStandardPrice.Enabled = ss;
                seStandardCost.Enabled = ss;

                btnAddDWG.Enabled = ss;
                btnDeleteDWG.Enabled = ss;
                btnGET.Enabled = false;
                chkGET.Checked = false;

                btnAdd_Item.Enabled = ss;
                btnDel_Item.Enabled = ss;
            }
            else if (Condition.Equals("Edit"))
            {
                txtInternalDesc.Enabled = ss;
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
                seMaximumInventory.Enabled = ss;
                cbInspaction.Enabled = ss;
                cboLocation.Enabled = ss;
                cbTakingLot.Enabled = ss;
                txtShelfNo.Enabled = ss;
                txtBom.Enabled = ss;
                cboBOM.Enabled = ss;
                txtVersion.Enabled = ss;
                cboRouting.Enabled = ss;
                seStandardPrice.Enabled = ss;
                seStandardCost.Enabled = ss;

                btnAddDWG.Enabled = ss;
                btnDeleteDWG.Enabled = ss;
                btnGET.Enabled = false;
                chkGET.Checked = false;

                btnAdd_Item.Enabled = ss;
                btnDel_Item.Enabled = ss;
            }
        }
        private void LoadDefault()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
               

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

                this.cboRouting.AutoFilter = true;
                this.cboRouting.AutoCompleteMode = AutoCompleteMode.Append;
                FilterDescriptor ro = new FilterDescriptor();
                ro.PropertyName = this.cboRouting.ValueMember;
                ro.Operator = FilterOperator.StartsWith;
                this.cboRouting.EditorControl.MasterTemplate.FilterDescriptors.Add(ro);

                cboRouting.DisplayMember = "RoutingNo";
                cboRouting.ValueMember = "RoutingNo";
                cboRouting.DataSource = db.mh_Routings.Where(s => s.Active == true).ToList();
                cboRouting.SelectedIndex = -1;
                cboRouting.Text = "";

                this.cboBOM.AutoFilter = true;
                this.cboBOM.AutoCompleteMode = AutoCompleteMode.Append;
                FilterDescriptor bb = new FilterDescriptor();
                bb.PropertyName = this.cboBOM.ValueMember;
                bb.Operator = FilterOperator.StartsWith;
                this.cboBOM.EditorControl.MasterTemplate.FilterDescriptors.Add(bb);
                cboBOM.DisplayMember = "BomNo";
                cboBOM.ValueMember = "BomNo";
                cboBOM.DataSource = db.tb_BomHDs.Where(s => s.Status !="Cancel").ToList();
                cboBOM.SelectedIndex = -1;
                cboBOM.Text = "";

                this.cboLocation.AutoFilter = true;
                this.cboLocation.AutoCompleteMode = AutoCompleteMode.Append;
                FilterDescriptor lo = new FilterDescriptor();
                lo.PropertyName = this.cboLocation.ValueMember;
                lo.Operator = FilterOperator.StartsWith;
                this.cboLocation.EditorControl.MasterTemplate.FilterDescriptors.Add(lo);

                cboLocation.DisplayMember = "Code";
                cboLocation.ValueMember = "Name";
                cboLocation.DataSource = db.mh_Locations.Where(s => s.Active == true).ToList();
                cboLocation.SelectedIndex = -1;
                cboLocation.Text = "";


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
                try
                {

                    cboGroupType.SelectedIndex = 0;

                    if (!cboGroupType.Text.Equals(""))
                    {
                        DefaultType();
                    }
                }
                catch { }

                try
                {
                    GridViewMultiComboBoxColumn Uom = (GridViewMultiComboBoxColumn)dgvBom.Columns["dgvUnit"];
                    Uom.DataSource = (from ix in db.mh_Units.Where(s => s.UnitActive == true)
                                      select new { ix.UnitCode }).ToList();
                    Uom.DisplayMember = "UnitCode";
                    Uom.DropDownStyle = RadDropDownStyle.DropDown;
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
                    var g = (from ix in db.mh_Items select ix)
                        .Where(a => a.InternalNo == txtInternalNo.Text).ToList();
                    if (g.Count() > 0)
                    {
                        txtInternalNo.Text = StockControl.dbClss.TSt(g.FirstOrDefault().InternalNo);
                        txtInternalName.Text = StockControl.dbClss.TSt(g.FirstOrDefault().InternalName);
                        txtInternalDesc.Text = StockControl.dbClss.TSt(g.FirstOrDefault().InternalDescription);
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
                        seMaximumInventory.Value = StockControl.dbClss.TInt(g.FirstOrDefault().MaximumInventory);
                        cbInspaction.Checked = dbClss.TBo(g.FirstOrDefault().Inspection);

                        cboLocation.Text = dbClss.TSt(g.FirstOrDefault().Location);
                        ddlReOrderType.Text = StockControl.dbClss.TSt(g.FirstOrDefault().ReorderType);
                        seMaximum.Value = StockControl.dbClss.TDe(g.FirstOrDefault().MaximumQty);
                        seMinimum.Value = StockControl.dbClss.TDe(g.FirstOrDefault().MinimumQty);
                        seReOrderPoint.Value = StockControl.dbClss.TDe(g.FirstOrDefault().ReorderPoint);
                        seReOrderQty.Value = StockControl.dbClss.TDe(g.FirstOrDefault().ReorderQty);
                        seSafetyStock.Value = StockControl.dbClss.TDe(g.FirstOrDefault().SafetyStock);
                        seTimebucket.Value = StockControl.dbClss.TInt(g.FirstOrDefault().Timebucket);
                        cbTakingLot.Checked = dbClss.TBo(g.FirstOrDefault().TakingLot);
                        txtShelfNo.Text = dbClss.TSt(g.FirstOrDefault().ShelfNo);
                        seStandardCost.Value = dbClss.TDe(g.FirstOrDefault().StandardCost);
                        seStandardPrice.Value = dbClss.TDe(g.FirstOrDefault().StandardPrice);

                        if (dbClss.TInt(g.FirstOrDefault().Routing)>0)
                        {
                            txtidRouting.Text = dbClss.TSt(g.FirstOrDefault().Routing);

                            var I = (from ix in db.mh_Routings select ix).Where(a => a.id == dbClss.TInt(g.FirstOrDefault().Routing)).ToList();
                            if (I.Count > 0)
                                cboRouting.Text = dbClss.TSt(I.FirstOrDefault().RoutingNo);
                        }
                        if (dbClss.TInt(g.FirstOrDefault().BillOfMaterials) > 0)
                        {
                            txtidBOM.Text = dbClss.TSt(g.FirstOrDefault().BillOfMaterials);

                            var I = (from ix in db.tb_BomHDs select ix).Where(a => a.id == dbClss.TInt(g.FirstOrDefault().BillOfMaterials)).ToList();
                            if (I.Count > 0)
                            {
                                txtBom.Text = dbClss.TSt(I.FirstOrDefault().BomNo);
                                cboBOM.Text = dbClss.TSt(I.FirstOrDefault().BomNo);
                            }
                        }

                       

                        //if(txtBom.Text !="")
                        //{
                        //    DataLoad_BOM();
                        //}

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
                            txtInternalNo.Enabled = false;
                            btnSave.Enabled = false;
                            btnDelete.Enabled = false;
                            btnView.Enabled = false;
                            btnEdit.Enabled = false;
                            lbStatus.Text = "InActive";
                            btnNew.Enabled = true;
                        }
                        else
                        {
                            txtInternalNo.Enabled = false;
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
            txtInternalNo.Enabled = true;
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

                        //string Temp_codeno = txtInternalNo.Text;
                        //string temp_codeno2 = "";
                        //if (chkGET.Checked.Equals(false))// ให้ระบบ Gen ให้
                        //{
                        //    //if (txtCodeNo.Text.Length > 5)
                        //    //{
                        //    //    int c = txtCodeNo.Text.Length;

                        //    //    temp_codeno2 = Temp_codeno.Substring(5, c - 5);
                        //    //    txtCodeNo.Text = Get_CodeNo();
                        //    //    txtCodeNo.Text = txtCodeNo.Text + temp_codeno2;
                        //    //}
                        //    //else
                        //    txtInternalNo.Text = Get_CodeNo();
                            
                        //}

                        if (txtCustomerPartNo.Text == "")
                            txtCustomerPartNo.Text = txtInternalNo.Text;

                        byte[] barcode = StockControl.dbClss.SaveQRCode2D(txtInternalNo.Text.Trim());
                        //byte[] barcode = null;

                        //DateTime? UpdateDate = null;

                        mh_Item u = new mh_Item();
                        u.BarCode = barcode;
                        u.InternalNo = txtInternalNo.Text.Trim();
                        u.InternalName = txtInternalName.Text.Trim();
                        u.InternalDescription = txtInternalDesc.Text.Trim();
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
                        u.ReorderPoint = dbClss.TDe(seReOrderPoint.Value);
                        u.MinimumQty = dbClss.TDe(seMinimum.Value);
                        u.MaximumQty = dbClss.TDe(seMaximum.Value);
                        u.MaximumInventory = dbClss.TDe(seMaximumInventory);
                        u.SafetyStock = dbClss.TDe(seSafetyStock.Value);
                        u.ItemUOM = 0;
                        u.BillOfMaterials = dbClss.TInt(txtidBOM.Text);
                        u.Routing = dbClss.TInt(txtidRouting.Text);
                        u.WorkCenter = 0;
                        u.Active = true;
                        u.VendorNo = txtVendorNo.Text;
                        u.VendorName = cboVendorName.Text;
                        u.Timebucket = dbClss.TInt(seTimebucket.Value);                      
                        u.chkDrawing = chkDWG.Checked;
                        u.Drawing = txtDrawing.Text;
                        u.Inspection = dbClss.TBo(cbInspaction.Checked);
                        u.TakingLot = dbClss.TBo(cbTakingLot.Checked);
                        u.Location = cboLocation.Text;
                        u.ShelfNo = txtShelfNo.Text.Trim();
                        u.StandardCost = dbClss.TDe(seStandardCost.Value);
                        u.StandardPrice = dbClss.TDe(seStandardPrice.Value);
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

                        //Insert UOM
                        InsertUOM_New(txtInternalNo.Text.Trim(),cboBaseUOM.Text,1);
                        InsertUOM_New(txtInternalNo.Text.Trim(), cboSalseUOM.Text, 1);
                        InsertUOM_New(txtInternalNo.Text.Trim(), cboPurchaseUOM.Text, 1);
                        InsertUOM_New(txtInternalNo.Text.Trim(), cboConsumptionUOM.Text, 1);

                    }
                    else  //Edit
                    {
                        foreach (DataRow row in dt_Part.Rows)
                        {
                            var g = (from ix in db.mh_Items
                                     where ix.InternalNo.Trim() == txtInternalNo.Text.Trim() 
                                     //&& ix.Active == true
                                     select ix).ToList();
                            if (g.Count > 0)  //มีรายการในระบบ
                            {
                                var gg = (from ix in db.mh_Items
                                          where ix.InternalNo.Trim() == txtInternalNo.Text.Trim() 
                                          //&& ix.Active == true
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
                                if (!txtInternalDesc.Text.Trim().Equals(row["InternalDescription"].ToString()))
                                {
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไขรายละเอียด [ เดิม : " + row["InternalDescription"].ToString() + " ใหม่ : " + txtInternalDesc.Text.Trim() + "]", txtInternalNo.Text);
                                    gg.InternalDescription = txtInternalDesc.Text.Trim();
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
                                if (!seStandardCost.Text.Trim().Equals(row["StandardCost"].ToString()))
                                {
                                    decimal StandardCost = dbClss.TDe(seStandardCost.Value);
                                    gg.StandardCost = StandardCost;
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไข StandardCost [ เดิม : " + row["StandardCost"].ToString() + " ใหม่ : " + seStandardCost.ToString() + "]", txtInternalNo.Text);
                                }
                                if (!seStandardPrice.Text.Trim().Equals(row["StandardPrice"].ToString()))
                                {
                                    decimal StandardPrice = dbClss.TDe(seStandardPrice.Value);
                                    gg.StandardPrice = StandardPrice;
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไข StandardPrice [ เดิม : " + row["StandardPrice"].ToString() + " ใหม่ : " + seStandardPrice.ToString() + "]", txtInternalNo.Text);
                                }
                               
                                if (!cbTakingLot.Checked.Equals(row["TakingLot"].ToString()))
                                {
                                    gg.TakingLot = dbClss.TBo(cbTakingLot.Checked);
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไข TakingLot [ เดิม : " + row["TakingLot"].ToString() + " ใหม่ : " + cbTakingLot.Checked.ToString() + "]", txtInternalNo.Text);
                                }
                                if (!cboLocation.Text.Equals(row["Location"].ToString()))
                                {
                                    gg.Location = cboLocation.Text;
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไข Location [ เดิม : " + row["Location"].ToString() + " ใหม่ : " + cboLocation.Text + "]", txtInternalNo.Text);
                                }
                                if (!txtShelfNo.Text.Equals(row["ShelfNo"].ToString()))
                                {
                                    gg.ShelfNo = txtShelfNo.Text;
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไข ShelfNo [ เดิม : " + row["ShelfNo"].ToString() + " ใหม่ : " + txtShelfNo.Text + "]", txtInternalNo.Text);
                                }
                                if (!cbInspaction.Checked.ToString().Equals(row["Inspection"].ToString()))
                                {
                                    gg.Inspection = dbClss.TBo(cbInspaction.Checked);
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไข Inspection [ เดิม : " + row["Inspection"].ToString() + " ใหม่ : " + cbInspaction.ToString() + "]", txtInternalNo.Text);
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
                                if (!seMaximumInventory.Text.Trim().Equals(row["MaximumInventory"].ToString()))
                                {
                                    decimal MaximumInventory = dbClss.TDe(seMaximumInventory.Value);
                                    gg.MaximumInventory = MaximumInventory;
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไข MaximumInventory [ เดิม : " + row["MaximumInventory"].ToString() + " ใหม่ : " + MaximumInventory.ToString() + "]", txtInternalNo.Text);
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
                                if (!txtidBOM.Text.Trim().Equals(row["BillOfMaterials"].ToString()))
                                {
                                    gg.BillOfMaterials = dbClss.TInt(txtidBOM.Text);
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไข BillOfMaterials No [ เดิม : " + row["BillOfMaterials"].ToString() + " ใหม่ : " + txtidBOM.Text.Trim() + "]", txtInternalNo.Text);
                                }
                                if (!txtidRouting.Text.Trim().Equals(row["Routing"].ToString()))
                                {
                                    gg.Routing = dbClss.TInt(txtidRouting.Text);
                                    dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไข Routing No [ เดิม : " + row["Routing"].ToString() + " ใหม่ : " + txtidRouting.Text.Trim() + "]", txtInternalNo.Text);
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
        private void InsertUOM_New(string ItemNo1,string UOM,decimal PCSUnit)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                var g1 = (from ix in db.mh_ItemUOMs
                          where ix.ItemNo.Trim().ToUpper() == ItemNo1.Trim().ToUpper()
                          && ix.UOMCode.Trim().ToUpper() == UOM.Trim().ToUpper()
                          //&& 
                          //ix.id == Convert.ToInt16(g.Cells["id"].Value)
                          select ix).ToList();
                if (g1.Count <= 0)
                {
                    var gg = (from ix in db.mh_ItemUOMs
                              where ix.ItemNo.Trim().ToUpper() == ItemNo1.Trim().ToUpper()
                                && ix.UOMCode.Trim().ToUpper() == UOM.Trim().ToUpper()
                              select ix).ToList();
                    if (gg.Count <= 0)
                    {
                        mh_ItemUOM u = new mh_ItemUOM();
                        u.QuantityPer = PCSUnit;
                        u.ItemNo = ItemNo1.Trim();
                        u.UOMCode = UOM;
                        u.Active = true;
                        u.Remark = "";
                        u.CreateBy = ClassLib.Classlib.User;
                        u.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                        db.mh_ItemUOMs.InsertOnSubmit(u);
                        db.SubmitChanges();
                        dbClss.AddHistory(this.Name, "เพิ่ม", "Insert Item UOM [ ItemNo:" + u.ItemNo + ", UOMCode:" + u.UOMCode + "]", "");
                    }
                }

              
            }
        }
        private bool Check_Save()
        {
            bool re = true;
            string err = "";
            try
            {
                if (txtInternalNo.Text.Trim().Equals(""))
                    err += " “รหัสทูล:” เป็นค่าว่าง \n";
                else
                {
                    if (Ac.Equals("New"))  //New
                    {
                        if (!txtInternalNo.Text.Equals(""))
                        {
                            using (DataClasses1DataContext db = new DataClasses1DataContext())
                            {
                                var g = (from ix in db.mh_Items select ix)
                                    .Where(a => a.InternalNo.Trim().ToUpper() == txtInternalNo.Text.Trim().ToUpper()).ToList();
                                if (g.Count() > 0)
                                {
                                    err += " “รหัสทูล:” ซ้ำในระบบ \n";
                                }
                            }
                        }
                    }
                }
                if (txtInternalName.Text.Equals(""))
                    err += " “ชื่อทูล:” เป็นค่าว่าง \n";
                if (txtInternalDesc.Text.Equals(""))
                    err += " “รายละเอียด:” เป็นค่าว่าง \n";
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

                if (ddlInventoryGroup.Text == "RM" || ddlInventoryGroup.Text == "SEMI")
                {
                    if (cboVendorName.Text.Equals(""))
                        err += "- “ชื่อผู้ขาย:” เป็นค่าว่าง \n";
                    if (txtVendorNo.Text.Equals(""))
                        err += "- “รหัสผู้ขาย:” เป็นค่าว่าง \n";
                }
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
                if (cboLocation.Text.Equals(""))
                    err += "- “Default Location:” เป็นค่าว่าง \n";
                if (txtShelfNo.Text.Equals(""))
                    err += "- “Default Shelf No.:” เป็นค่าว่าง \n";


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
                   

                ////---------------check codeno -------------------//
                //if (Ac.Equals("New"))  //New
                //{
                //    if (chkGET.Checked)
                //    {
                //        if (txtInternalNo.Text.Trim().Equals(""))
                //        {
                //            err += " “รหัสทูล:” เป็นค่าว่าง \n";
                //        }
                //        else //เช็คว่า เลข Gen ด้านหน้าเป็น เลข Group เดียวกันหรือไม่ ถ้าไม่ใช่จะขึ้น Error
                //        {
                //            using (DataClasses1DataContext db = new DataClasses1DataContext())
                //            {
                //                string Temp_Running = "";
                //                var I = (from ix in db.mh_GroupTypes select ix).Where(a => a.GroupCode == cboGroupType.Text).ToList();
                //                if (I.Count > 0)
                //                    Temp_Running = I.FirstOrDefault().Running;

                //                if (!Temp_Running.Equals(""))
                //                {
                //                    string cut_string = "";
                //                    cut_string = txtInternalNo.Text.Trim().Substring(0, 1);
                //                    if (!cut_string.ToUpper().Equals(Temp_Running.ToUpper()))
                //                        err += "- “รหัสทูล เริ่มต้นไม่ตรงกับประเภทกลุ่มสินค้า:”  \n";
                //                    else//เช็คว่าเป็น CodeNo ที่มีในระบบหรือไม่ ถ้ามีแล้วจะ New เลขใหม่ไม่ได้ เพราะซ้ำ
                //                    {
                //                        var g1 = (from ix in db.mh_Items select ix).Where(a => a.InternalNo == txtInternalNo.Text.Trim()).ToList();
                //                        if (g1.Count() > 0)
                //                        {
                //                            err += "- “รหัสทูล ซ้ำ:”มีรหัสทูล ในระบบแล้ว  \n";
                //                        }
                //                    }
                //                }
                //                //err += "- “ประเภทกลุ่ม สินค้า:” เป็นค่าว่าง \n";
                //            }
                //        }
                //    }
                //}
                ////-----------------------------------------------//



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
                        if (txtInternalNo.Text.Trim() == "")
                            return;
                        //int temp_item = 0;
                        //if (Ac.Equals("New"))  //New
                        //{
                        //    using (DataClasses1DataContext db = new DataClasses1DataContext())
                        //    {
                        //        var g = (from ix in db.mh_Items
                        //                 where ix.InternalNo.Trim().ToUpper() == txtInternalNo.Text.Trim().ToUpper()
                        //                 select ix).ToList();
                        //        {
                        //            temp_item = 1; 
                        //        }
                        //    }
                        //}
                        //if (temp_item == 1)
                        //{
                        //    if (MessageBox.Show("รายการที่ทำการสร้างใหม่ เคยมีในระบบแล้วต้องการทีจะนำกลับมาใช้ใหม่ ใช่หรือไม่?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        //    {
                        //        Ac = "Edit";
                        //    }
                        //    else
                        //        return;
                        //}

                        AddPart();
                       // AddBom();
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
        private void AddBom()
        {
            if (ddlInventoryGroup.Text == "FG" || ddlInventoryGroup.Text == "SEMI" || dbClss.TInt(txtidBOM.Text)> 0)
            {
                SaveHerder_BOM();
                AddPR_d_Bom();
            }

        }
        private void SaveHerder_BOM()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                int idBom = 0;
                idBom = dbClss.TInt(txtidBOM.Text);

                var g = (from ix in db.tb_BomHDs
                         where ix.id == idBom
                         //ix.PartNo.Trim().ToUpper() == txtInternalNo.Text.Trim().ToUpper()
                         //&& ix.BomNo.Trim().ToUpper() == txtBom.Text.Trim().ToUpper()

                         select ix).ToList();
                if (g.Count > 0)  //มีรายการในระบบ
                {
                    foreach (DataRow row in dt_HD_Bom.Rows)
                    {

                        var gg = (from ix in db.tb_BomHDs
                                  where ix.id == idBom
                                  //ix.PartNo.Trim().ToUpper() == txtInternalNo.Text.Trim().ToUpper()
                                  //&& ix.BomNo.Trim().ToUpper() == txtBom.Text.Trim().ToUpper()

                                  select ix).First();

                        //gg.Status = "Waiting";
                        //gg.TEMPNo = txtTempNo.Text;
                        gg.ModifyBy = ClassLib.Classlib.User;
                        gg.ModifyDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));

                        dbClss.AddHistory(this.Name, "แก้ไข Bom", "แก้ไข Bom โดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy", new CultureInfo("en-US")) + "]", txtBom.Text.Trim() + "-" + txtInternalNo.Text.Trim());

                        //var i = (from ix in db.mh_Items select ix).Where(a => a.InternalNo.Equals(txtInternalNo.Text.Trim().ToUpper())).ToList();
                        //if (i.Count > 0)
                        //{
                        gg.PartDescription = txtInternalDesc.Text; // dbClss.TSt(i.FirstOrDefault().InternalDescription);//txtDescription.Text;
                        gg.PartName = txtInternalName.Text; // dbClss.TSt(i.FirstOrDefault().InternalName); //txtPartName.Text;
                                                            //}
                                                            //if (StockControl.dbClss.TSt(gg.Barcode).Equals(""))
                                                            //    gg.Barcode = StockControl.dbClss.SaveQRCode2D(txtPRNo.Text.Trim());
                        int ver = dbClss.TInt(txtVersion.Text) + 1;
                        gg.Version = ver.ToString();
                        gg.Status = "Process";
                     
                        db.SubmitChanges();
                    }
                }
                else  // Add ใหม่
                {
                    //byte[] barcode = null;
                    //if (!txtPRNo.Text.Equals(""))
                    //    barcode = StockControl.dbClss.SaveQRCode2D(txtPRNo.Text.Trim());


                    tb_BomHD gg = new tb_BomHD();
                    gg.ModifyBy = ClassLib.Classlib.User;
                    gg.ModifyDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    gg.CreateBy = ClassLib.Classlib.User;
                    gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    gg.PartNo = txtInternalNo.Text.Trim();
                    gg.BomNo = txtBom.Text.Trim().ToUpper();
                    gg.PartDescription = txtInternalDesc.Text.Trim();
                    gg.PartName = txtInternalName.Text.Trim();
                    gg.Remark = "";// txtRemarkHD.Text.Trim();
                    gg.Version = txtVersion.Text.Trim();
                    gg.Status = "Process";
                    //if (!dtBegin.Text.Trim().Equals("") && !dtEnd.Text.Trim().Equals(""))
                    //{
                    //    gg.StartDate = Convert.ToDateTime(dtBegin.Value, new CultureInfo("en-US"));
                    //    gg.EndDate = Convert.ToDateTime(dtEnd.Value, new CultureInfo("en-US"));
                    //}
                    db.tb_BomHDs.InsertOnSubmit(gg);
                    db.SubmitChanges();

                    dbClss.AddHistory(this.Name, "เพิ่ม Bom", "สร้าง Bom No. : [ " + txtBom.Text.Trim() + " ,Part No. : :" + txtInternalNo.Text + " ] ", txtBom.Text.Trim() + "-" + txtInternalNo.Text.Trim());

                }
            }
        }
        private bool AddPR_d_Bom()
        {

            bool ck = false;
            //int C = 0;
            //try
            //{


            dgvBom.EndEdit();
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                int line_t = 50;
                string ComponentName = "";
                string ComponentDescription = "";
                foreach (var g in dgvBom.Rows)
                {
                    if (g.IsVisible.Equals(true))
                    {
                        // line_t += 50;
                        DateTime? d = null;
                        var i = (from ix in db.mh_Items select ix)
                            .Where(a => a.InternalNo.Contains(StockControl.dbClss.TSt(g.Cells["dgvComponent"].Value))).ToList();
                        if (i.Count > 0)
                        {
                            ComponentName = dbClss.TSt(i.FirstOrDefault().InternalName);
                            ComponentDescription = dbClss.TSt(i.FirstOrDefault().InternalDescription);
                        }

                        if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) <= 0)  //New ใหม่
                        {

                            tb_BomDT u = new tb_BomDT();
                            u.PartNo = txtInternalNo.Text.Trim();
                            u.BomNo = txtBom.Text.Trim().ToUpper();

                            u.Line = line_t;
                            u.Component = StockControl.dbClss.TSt(g.Cells["dgvComponent"].Value);
                            u.ComponentDescription = StockControl.dbClss.TSt(g.Cells["dgvComponentDescription"].Value);
                            u.ComponentName = StockControl.dbClss.TSt(g.Cells["dgvComponentName"].Value);
                            u.Qty = StockControl.dbClss.TDe(g.Cells["dgvQty"].Value);
                            u.Unit = StockControl.dbClss.TSt(g.Cells["dgvUnit"].Value);
                            u.UnitCost = StockControl.dbClss.TDe(g.Cells["dgvUnitCost"].Value);
                            u.Cost = StockControl.dbClss.TDe(g.Cells["dgvCost"].Value);
                            u.PCSUnit = dbClss.TDe(g.Cells["dgvPCSUnit"].Value);
                            db.tb_BomDTs.InsertOnSubmit(u);
                            db.SubmitChanges();
                            //C += 1;
                            dbClss.AddHistory(this.Name, "เพิ่ม Item Bom", "เพิ่มรายการ Component [" + u.Component + "]", txtBom.Text.Trim() + "-" + txtInternalNo.Text.Trim());

                            line_t += 50;
                        }
                        else  // อัพเดต
                        {

                            if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) > 0)
                            {
                                foreach (DataRow row in dt_DT_Bom.Rows)
                                {
                                    if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) == StockControl.dbClss.TInt(row["id"]))
                                    {
                                        //var u = (from ix in db.tb_BomDTs
                                        //         where ix.PartNo.ToUpper() == txtPartNo.Text.Trim().ToUpper()
                                        //         && ix.BomNo.ToUpper() == txtBomNo.Text.Trim().ToUpper()
                                        //         // && ix.TempNo == txtTempNo.Text
                                        //         && ix.id == StockControl.dbClss.TInt(g.Cells["dgvid"].Value)
                                        //         select ix).First();

                                        dbClss.AddHistory(this.Name, "แก้ไขรายการ Item Bom", "id :" + StockControl.dbClss.TSt(g.Cells["dgvid"].Value)
                                        + " BomNo :" + txtBom.Text.ToUpper()
                                        + " ,PartNo :" + txtInternalNo.Text
                                        + " แก้ไขโดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy", new CultureInfo("en-US")) + "]"
                                        , txtBom.Text.Trim() + "-" + txtInternalNo.Text.Trim());

                                        //u.PartNo = txtPartNo.Text.Trim();
                                        //u.BomNo = txtBomNo.Text.Trim().ToUpper();

                                        if (!StockControl.dbClss.TSt(g.Cells["dgvComponent"].Value).Equals(row["Component"].ToString()))
                                        {
                                            //u.Component = StockControl.dbClss.TSt(g.Cells["dgvComponent"].Value);
                                            dbClss.AddHistory(this.Name, "แก้ไข Item Bom", "แก้ไข Component [" + StockControl.dbClss.TSt(g.Cells["dgvComponent"].Value) + "]", txtBom.Text.Trim() + "-" + txtInternalNo.Text.Trim());
                                        }

                                        //u.Line = line_t;                         
                                        decimal Qty = 0; decimal.TryParse(StockControl.dbClss.TSt(g.Cells["dgvQty"].Value), out Qty);
                                        if (!StockControl.dbClss.TSt(g.Cells["dgvQty"].Value).Equals(row["Qty"].ToString()))
                                        {
                                            //u.Qty = StockControl.dbClss.TDe(g.Cells["dgvQty"].Value);
                                            dbClss.AddHistory(this.Name, "แก้ไข Item Bom", "แก้ไขจำนวน [" + Qty.ToString() + "]", txtBom.Text.Trim() + "-" + txtInternalNo.Text.Trim());
                                        }
                                        //u.PCSUnit = StockControl.dbClss.TDe(g.Cells["dgvPCSUnit"].Value);

                                        if (!StockControl.dbClss.TSt(g.Cells["dgvUnit"].Value).Equals(row["Unit"].ToString()))
                                        {
                                            // u.Unit = StockControl.dbClss.TSt(g.Cells["dgvUnit"].Value);
                                            dbClss.AddHistory(this.Name, "แก้ไข Item Bom", "แก้ไขหน่วย [" + StockControl.dbClss.TSt(g.Cells["dgvUnit"].Value) + "]", txtBom.Text.Trim() + "-" + txtInternalNo.Text.Trim());
                                        }

                                        //u.UnitCost = StockControl.dbClss.TDe(g.Cells["dgvUnitCost"].Value);
                                        //u.Cost = StockControl.dbClss.TDe(g.Cells["dgvCost"].Value);

                                        db.sp_020_Bom_DT_ADD(StockControl.dbClss.TInt(g.Cells["dgvid"].Value)
                                            , txtInternalNo.Text.Trim(), txtBom.Text.Trim(), null, null, line_t
                                            , StockControl.dbClss.TSt(g.Cells["dgvComponent"].Value)
                                            , ComponentName//StockControl.dbClss.TSt(g.Cells["dgvComponentName"].Value)
                                            , ComponentDescription// StockControl.dbClss.TSt(g.Cells["dgvComponentDescription"].Value)
                                            , Qty
                                            , StockControl.dbClss.TSt(g.Cells["dgvUnit"].Value)
                                            , StockControl.dbClss.TDe(g.Cells["dgvUnitCost"].Value)
                                            , StockControl.dbClss.TDe(g.Cells["dgvCost"].Value)
                                            , StockControl.dbClss.TDe(g.Cells["dgvPCSUnit"].Value)
                                            , ClassLib.Classlib.User

                                            );
                                        //db.SubmitChanges();
                                        line_t += 50;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else //Del
                    {
                        if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) > 0)
                        {
                            db.sp_019_Bom_DT_Del(StockControl.dbClss.TInt(g.Cells["dgvid"].Value), txtBom.Text.Trim(), txtInternalNo.Text.Trim(), StockControl.dbClss.TSt(g.Cells["dgvComponent"].Value), ClassLib.Classlib.User);

                            db.SubmitChanges();
                        }
                    }
                }
            }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    dbClss.AddError("CreatePR", ex.Message, this.Name);
            //}

            //if (C > 0)
            //    MessageBox.Show("บันทึกสำเร็จ!");

            return ck;
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
            seMaximumInventory.Value = 0;
            txtInternalDesc.Text = "";
            cbInspaction.Checked = false;
            cbTakingLot.Checked = false;
            cboLocation.Text = "";
            txtShelfNo.Text = "";
            txtBom.Text = "";
            cboBOM.Text = "";
            cboBOM.SelectedIndex = -1;
            txtVersion.Text = "";
            cboRouting.SelectedIndex = -1;
            cboRouting.Text = "";
            dgvBom.Rows.Clear();
            txtidRouting.Text = "0";
            txtidBOM.Text = "0";
            seStandardCost.Value = 0;
            seStandardPrice.Value = 0;

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
                            gg.ModifyDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                            dbClss.AddHistory(this.Name, "ลบทูล", "ลบทูล [" + txtInternalNo.Text.Trim() + "]", txtInternalNo.Text);

                            //Delete Bom
                            db.sp_018_Bom_Del(0, txtBom.Text, txtInternalNo.Text, ClassLib.Classlib.User);


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
                                var g = (from ix in db.mh_Items
                                         where ix.InternalNo.Trim().ToUpper() == CodeNo.Trim().ToUpper() //&& ix.Status == "Active"
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
                                var g = (from ix in db.mh_Locations
                                         where ix.Code.ToUpper().Trim() == Location.ToUpper().Trim() && ix.Active ==true && ix.Active == true
                                         select ix).ToList();
                                if (g.Count > 0)
                                    Location = g.FirstOrDefault().Code;
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

                            var g = (from ix in db.mh_Items
                                     where ix.InternalNo.Trim() == rd["InternalNo"].ToString().Trim() //&& ix.Status == "Active"
                                     select ix).ToList();
                            if (g.Count > 0)  //มีรายการในระบบ อัพเดต
                            {

                                var gg = (from ix in db.mh_Items
                                          where ix.InternalNo.Trim() == rd["InternalNo"].ToString().Trim()
                                          select ix).First();

                                gg.ModifyBy = rd["ModifyBy"].ToString().Trim();
                                gg.ModifyDate = Convert.ToDateTime(rd["ModifyDate"].ToString()); //DateTime.Now;
                                dbClss.AddHistory(this.Name  , "แก้ไข ทูล", " แก้ไข ทูล โดย Import โดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", rd["CodeNo"].ToString());

                                //if (StockControl.dbClss.TSt(gg.BarCode).Equals(""))
                                //    gg.BarCode = StockControl.dbClss.SaveQRCode2D(rd["CodeNo"].ToString());

                                //gg.ItemNo = rd["ItemNo"].ToString().Trim();
                                gg.InternalName = rd["InternalName"].ToString().Trim();
                                gg.InventoryGroup = rd["InventoryGroup"].ToString().Trim();
                                gg.GroupType = rd["GroupType"].ToString().Trim();
                                gg.PurchaseUOM = rd["PurchaseUOM"].ToString().Trim();
                                gg.SalesUOM = rd["SalesUOM"].ToString().Trim();
                                gg.ConsumptionUOM = rd["ConsumptionUOM"].ToString().Trim();
                                gg.BaseUOM = rd["BaseUOM"].ToString().Trim();
                                gg.CustomerPartName = rd["CustomerPartName"].ToString().Trim();
                                gg.CustomerPartNo = rd["CustomerPartNo"].ToString().Trim();
                                gg.Type = rd["Type"].ToString().Trim();
                                gg.VatType = rd["VatType"].ToString().Trim();
                                gg.chkDrawing = dbClss.TBo(rd["chkDrawing"]);
                                gg.Drawing = rd["Drawing"].ToString().Trim();
                                gg.VendorNo = rd["VendorNo"].ToString().Trim();
                                gg.VendorName = rd["VendorName"].ToString().Trim();
                                //gg.UseTacking = rd["UseTacking"].ToString().Trim();
                                int Timebucket = 0; int.TryParse(rd["Timebucket"].ToString(), out Timebucket);
                                gg.Timebucket = Timebucket;
                                decimal MaximumQty = 0; decimal.TryParse(rd["MaximumQty"].ToString(), out MaximumQty);
                                gg.MaximumQty = MaximumQty;
                                decimal MinimumQty = 0; decimal.TryParse(rd["MinimumQty"].ToString(), out MinimumQty);
                                gg.MinimumQty = MinimumQty;
                                decimal ReOrderPoint = 0; decimal.TryParse(rd["ReOrderPoint"].ToString(), out ReOrderPoint);
                                gg.ReorderPoint = ReOrderPoint;
                                decimal ReorderQty = 0; decimal.TryParse(rd["ReorderQty"].ToString(), out ReorderQty);
                                gg.ReorderQty = ReorderQty;
                                decimal SafetyStock = 0; decimal.TryParse(rd["SafetyStock"].ToString(), out SafetyStock);
                                gg.SafetyStock = SafetyStock;
                                gg.Active = dbClss.TBo(rd["Active"]);
                                gg.InternalLeadTime = dbClss.TInt(rd["InternalLeadTime"]);
                                db.SubmitChanges();
                            }
                            else   // Add ใหม่
                            {
                                // byte[] barcode = StockControl.dbClss.SaveQRCode2D(rd["CodeNo"].ToString().Trim());
                                //decimal StockDL = 0;
                                //decimal StockInv = 0;
                                //decimal StockBackOrder = 0;

                                //decimal StandardCost = 0;
                                //decimal MaximumStock = 0;
                                //decimal MinimumStock = 0;
                                //decimal SafetyStock = 0;
                                //decimal ReOrderPoint = 0;
                                //decimal SD = 0;
                                //decimal Toollife = 0;
                                //decimal Leadtime = 0;
                                //bool Critical = false;
                                //decimal PCSUnit = 0;
                                //string CostingMethod = "";
                                //string ItemGroup = "";
                                //string UpdateBy = ClassLib.Classlib.User;
                                //DateTime CreateDate = DateTime.Now;
                                //UpdateBy = rd["CreateBy"].ToString().Trim();
                                //CreateDate = Convert.ToDateTime(rd["CreateDate"].ToString()); //DateTime.Now;

                                //decimal.TryParse(rd["StandardCost"].ToString(), out StandardCost);
                                //decimal.TryParse(rd["MaximumStock"].ToString(), out MaximumStock);
                                //decimal.TryParse(rd["MinimumStock"].ToString(), out MinimumStock);
                                ////decimal.TryParse(rd["ReOrderPoint"].ToString(), out ReOrderPoint);
                                //decimal.TryParse(rd["PCSUnit"].ToString(), out PCSUnit);
                                //decimal.TryParse(rd["Leadtime"].ToString(), out Leadtime);
                                //decimal.TryParse(rd["Toollife"].ToString(), out Toollife);
                                //if (Toollife < 0)
                                //    Toollife = 1;

                                //decimal.TryParse(rd["SD"].ToString(), out SD);

                                DateTime? UpdateDate = null;
                                
                                mh_Item gg = new mh_Item();
                                gg.InternalName = rd["InternalName"].ToString().Trim();
                                gg.InventoryGroup = rd["InventoryGroup"].ToString().Trim();
                                gg.GroupType = rd["GroupType"].ToString().Trim();
                                gg.PurchaseUOM = rd["PurchaseUOM"].ToString().Trim();
                                gg.SalesUOM = rd["SalesUOM"].ToString().Trim();
                                gg.ConsumptionUOM = rd["ConsumptionUOM"].ToString().Trim();
                                gg.BaseUOM = rd["BaseUOM"].ToString().Trim();
                                gg.CustomerPartName = rd["CustomerPartName"].ToString().Trim();
                                gg.CustomerPartNo = rd["CustomerPartNo"].ToString().Trim();
                                gg.Type = rd["Type"].ToString().Trim();
                                gg.VatType = rd["VatType"].ToString().Trim();
                                gg.chkDrawing = dbClss.TBo(rd["chkDrawing"]);
                                gg.Drawing = rd["Drawing"].ToString().Trim();
                                gg.VendorNo = rd["VendorNo"].ToString().Trim();
                                gg.VendorName = rd["VendorName"].ToString().Trim();
                                //gg.UseTacking = rd["UseTacking"].ToString().Trim();
                                int Timebucket = 0; int.TryParse(rd["Timebucket"].ToString(), out Timebucket);
                                gg.Timebucket = Timebucket;
                                decimal MaximumQty = 0; decimal.TryParse(rd["MaximumQty"].ToString(), out MaximumQty);
                                gg.MaximumQty = MaximumQty;
                                decimal MinimumQty = 0; decimal.TryParse(rd["MinimumQty"].ToString(), out MinimumQty);
                                gg.MinimumQty = MinimumQty;
                                decimal ReOrderPoint = 0; decimal.TryParse(rd["ReOrderPoint"].ToString(), out ReOrderPoint);
                                gg.ReorderPoint = ReOrderPoint;
                                decimal ReorderQty = 0; decimal.TryParse(rd["ReorderQty"].ToString(), out ReorderQty);
                                gg.ReorderQty = ReorderQty;
                                decimal SafetyStock = 0; decimal.TryParse(rd["SafetyStock"].ToString(), out SafetyStock);
                                gg.SafetyStock = SafetyStock;
                                gg.Active = dbClss.TBo(rd["Active"]);
                                gg.InternalLeadTime = dbClss.TInt(rd["InternalLeadTime"]);

                                db.mh_Items.InsertOnSubmit(gg);
                                db.SubmitChanges();

                                dbClss.AddHistory(this.Name  ,"เพิ่ม ทูล", "เพิ่ม ทูล โดย Import [" + gg.InternalNo + "]", gg.InternalNo);

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
            //if(ddlInventoryGroup.Text=="FG"||ddlInventoryGroup.Text=="SEMI")
            //{
            //    radPageView1.Pages[1].Item.Visibility = ElementVisibility.Visible;
            //    cboRouting.Enabled = true;
            //}
            //else
            //{
            //    radPageView1.Pages[1].Item.Visibility = ElementVisibility.Collapsed;
            //    cboRouting.Enabled = false;
            //    cboRouting.SelectedIndex = -1;
            //    cboRouting.Text = "";
            //    txtBom.Text = "";
            //    txtVersion.Text = "";
            //    txtidBOM.Text = "0";
            //    txtidRouting.Text = "0";
            //}
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
                    var g = (from ix in db.mh_Items select ix).Where(a => a.InternalNo == txtInternalNo.Text).ToList();
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
                        ps.CodeNo = g.FirstOrDefault().InternalNo;
                        ps.PartDescription = g.FirstOrDefault().InternalDescription;
                        ps.PartNo = g.FirstOrDefault().InternalName;
                        ps.ShelfNo = "";// g.FirstOrDefault().ShelfNo;
                        ps.Max = Convert.ToDecimal(g.FirstOrDefault().MaximumQty);
                        ps.Min = Convert.ToDecimal(g.FirstOrDefault().MinimumQty);
                        ps.OrderPoint = Convert.ToDecimal(g.FirstOrDefault().ReorderPoint);
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
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnPrintBarCode_Click(object sender, EventArgs e)
        {
            try
            {
                dt_Kanban.Rows.Clear();
                this.Cursor = Cursors.WaitCursor;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {

                    var g = (from ix in db.mh_Items select ix).Where(a => a.InternalNo == txtInternalNo.Text).ToList();
                    if (g.Count() > 0)
                    {
                        // Step 1 delete UserName
                        var deleteItem = (from ii in db.TempPrintKanbans where ii.UserName == ClassLib.Classlib.User select ii);
                        foreach (var d in deleteItem)
                        {
                            db.TempPrintKanbans.DeleteOnSubmit(d);
                            db.SubmitChanges();
                        }

                        // Step 2 Insert to Table
                        TempPrintKanban tm = new TempPrintKanban();
                        tm.UserName = ClassLib.Classlib.User;
                        tm.CodeNo = g.FirstOrDefault().InternalNo;
                        tm.PartDescription = g.FirstOrDefault().InternalDescription;
                        tm.PartNo = g.FirstOrDefault().InternalName;
                        tm.VendorName = g.FirstOrDefault().VendorName;
                        tm.ShelfNo = "";// g.FirstOrDefault().ShelfNo;
                        tm.GroupType = g.FirstOrDefault().GroupType;
                        tm.Max = Convert.ToDecimal(g.FirstOrDefault().MaximumQty);
                        tm.Min = Convert.ToDecimal(g.FirstOrDefault().MinimumQty);
                        tm.ReOrderPoint = Convert.ToDecimal(g.FirstOrDefault().ReorderPoint);
                        tm.ToolLife = 1;// Convert.ToDecimal(g.FirstOrDefault().Toollife);
                        byte[] barcode = StockControl.dbClss.SaveQRCode2D(g.FirstOrDefault().InternalNo);
                        tm.BarCode = barcode;
                        tm.Location = cboLocation.Text;
                        tm.TypePart = ddlInventoryGroup.Text;

                        db.TempPrintKanbans.InsertOnSubmit(tm);
                        db.SubmitChanges();
                        this.Cursor = Cursors.Default;
                        // Step 3 Call Report
                        Report.Reportx1.Value = new string[2];
                        Report.Reportx1.Value[0] = ClassLib.Classlib.User;
                        Report.Reportx1.WReport = "001_Kanban_Part";
                        Report.Reportx1 op = new Report.Reportx1("001_Kanban_Part.rpt");
                        op.Show();

                        //foreach (var gg in g)
                        //{
                        //    dt_Kanban.Rows.Add(gg.CodeNo, gg.ItemNo, gg.ItemDescription, gg.ShelfNo, gg.Leadtime, gg.VendorItemName, gg.GroupCode, gg.Toollife, gg.MaximumStock, gg.MinimumStock,gg.ReOrderPoint, gg.BarCode);
                        //}
                        //DataTable DT =  StockControl.dbClss.LINQToDataTable(g);
                        //Reportx1 po = new Reportx1("Report_PurchaseRequest_Content1.rpt", DT, "FromDT");
                        //po.Show();


                    }
                    else
                        MessageBox.Show("not found.");
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;
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

        private void seTimebucket_ValueChanged(object sender, EventArgs e)
        {

        }

        private void radLabel45_Click(object sender, EventArgs e)
        {

        }

        private void txtBom_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter)
            {
                DataLoad_BOM();
            }
        }
        private void DataLoad_BOM()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                dt_DT_Bom.Rows.Clear();
                dt_HD_Bom.Rows.Clear();
                int ck = 0;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var g = (from a in db.tb_BomHDs
                             join b in db.mh_Items on a.PartNo equals b.InternalNo
                             where (a.Status != "Cancel")
                             //&& (b.Active == true)
                             && (a.BomNo == (txtBom.Text.Trim()))
                             && (a.PartNo == txtInternalNo.Text.Trim())

                             select new
                             {
                                 PartName = a.PartName,
                                 PartDesc = a.PartDescription,
                                 TypePart = b.InventoryGroup,
                                 Remark = a.Remark,
                                 Description = a.Description,
                                 Year_ = a.Year_,
                                 Month_ = a.Month_,
                                 CreateBy = a.CreateBy,
                                 CreateDate = a.CreateDate,
                                 ModifyBy = a.ModifyBy,
                                 ModifyDate = a.ModifyDate,
                                 StartDate = a.StartDate,
                                 EndDate = a.EndDate,
                                 ReplenishmentType = b.ReplenishmentType
                                 ,
                                 GroupType = b.GroupType,
                                 BomNo = a.BomNo,
                                 PartNo = a.PartNo,
                                 Status = a.Status,
                                 Version = a.Version,
                                 id = a.id

                             }//.Where(ab => ab.VendorNo.Contains(Vendorno))
                              ).ToList();
                    if (g.Count() > 0)
                    {

                        dt_HD_Bom = StockControl.dbClss.LINQToDataTable(g);

                        txtVersion.Text = dbClss.TSt(g.FirstOrDefault().Version);
                        txtidBOM.Text = dbClss.TSt(g.FirstOrDefault().id);

                        var d = (from ix in db.tb_BomDTs select ix)
                            .Where(a => a.PartNo == txtInternalNo.Text.Trim()
                            && a.BomNo == txtBom.Text.Trim()
                            ).ToList();
                        if (d.Count() > 0)
                        {
                            int c = 0;
                            dgvBom.DataSource = d;
                            dt_DT_Bom = StockControl.dbClss.LINQToDataTable(d);
                            foreach (var x in dgvBom.Rows)
                            {
                                c += 1;
                                x.Cells["dgvNo"].Value = c;

                                var i2 = (from ix in db.mh_Items select ix).Where(a => a.InternalNo == StockControl.dbClss.TSt(x.Cells["dgvComponent"].Value)).ToList();
                                if (i2.Count() > 0)
                                {
                                    x.Cells["dgvGroupType"].Value = StockControl.dbClss.TSt(i2.FirstOrDefault().InventoryGroup);
                                    x.Cells["dgvType"].Value = StockControl.dbClss.TSt(i2.FirstOrDefault().InventoryGroup);
                                    x.Cells["dgvReplenishmentType"].Value = StockControl.dbClss.TSt(i2.FirstOrDefault().ReplenishmentType);

                                }
                            }
                        }

                        //lblStatus.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Status);
                        if (StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Cancel"))
                        {
                            dgvBom.ReadOnly = true;
                        }
                        else if
                            (StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Completed"))
                        {
                            dgvBom.ReadOnly = true;
                        }
                        else
                        {
                            dgvBom.ReadOnly = false;
                        }

                        foreach (var x in dgvBom.Rows)
                        {
                            if (row >= 0 && row == ck && dgvBom.Rows.Count > 0)
                            {
                                x.ViewInfo.CurrentRow = x;
                            }
                            ck += 1;
                        }
                    }
                    else
                    {
                        txtidBOM.Text = "0";
                        txtVersion.Text = "";
                        dgvBom.Rows.Clear();
                    }                 
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void btnAdd_Item_Click(object sender, EventArgs e)
        {
            try
            {
                if (!txtInternalNo.Text.Equals("") && !txtBom.Text.Equals(""))
                {
                    List<GridViewRowInfo> dgvRow_List = new List<GridViewRowInfo>();
                    //dgvRow_List.Clear();                      
                    ListPart MS = new ListPart(txtInternalNo, "SEMI-RM", dgvRow_List);
                    //ListPart_CreatePR MS = new ListPart_CreatePR(dgvRow_List, txtVendorNo.Text);
                    MS.ShowDialog();
                    if (dgvRow_List.Count > 0)
                    {
                        string CodeNo = "";
                        this.Cursor = Cursors.WaitCursor;
                        decimal OrderQty = 1;
                        foreach (GridViewRowInfo ee in dgvRow_List)
                        {
                            CodeNo = Convert.ToString(ee.Cells["InternalNo"].Value).Trim();
                            if (!CodeNo.Equals("") && !check_Duppicate(CodeNo))
                            {
                                Add_Part(CodeNo, OrderQty);
                            }
                            else
                            {
                                MessageBox.Show("รหัสพาร์ท ซ้ำ");
                            }
                        }
                    }
                }
                else
                    MessageBox.Show("เลือกรหัสทูล หรือ รหัสบอม ก่อน !!!");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void btnDel_Item_Click(object sender, EventArgs e)
        {
            try
            {

                if (dgvBom.Rows.Count <= 0)
                    return;
                if (Ac.Equals("New") || Ac.Equals("Edit"))
                {
                    this.Cursor = Cursors.WaitCursor;

                    int id = 0;
                    int.TryParse(StockControl.dbClss.TSt(dgvBom.CurrentRow.Cells["dgvid"].Value), out id);
                    if (id <= 0)
                        dgvBom.Rows.Remove(dgvBom.CurrentRow);

                    else
                    {
                        string CodeNo = ""; StockControl.dbClss.TSt(dgvBom.CurrentRow.Cells["dgvCompanent"]);
                        if (MessageBox.Show("ต้องการลบรายการ ( " + CodeNo + " ) ออกจากรายการ หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            dgvBom.CurrentRow.IsVisible = false;
                        }
                    }
                    SetRowNo1(dgvBom);
                }
                else
                {
                    MessageBox.Show("ไม่สามารถทำการลบรายการได้");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        public static void SetRowNo1(RadGridView Grid)//เลขลำดับ
        {
            int i = 1;
            Grid.Rows.Where(o => o.IsVisible).ToList().ForEach(o =>
            {
                o.Cells["dgvNo"].Value = i;
                i++;
            });
        }
        private bool check_Duppicate(string CodeNo)
        {
            bool re = false;
            foreach (var rd1 in dgvBom.Rows)
            {
                if (rd1.IsVisible.Equals(true))
                {
                    if (StockControl.dbClss.TSt(rd1.Cells["dgvComponent"].Value).Trim().ToUpper().Equals(CodeNo.Trim().ToUpper()))
                        re = true;
                }
            }

            return re;

        }
        private void Add_Part(string CodeNo, decimal OrderQty)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                int Row = 0; Row = dgvBom.Rows.Count() + 1;
                var g = (from ix in db.mh_Items select ix).Where(a => a.InternalNo.Contains(CodeNo)).ToList();
                if (g.Count > 0)
                {
                    decimal PCSUnit = dbClss.Con_UOM(StockControl.dbClss.TSt(g.FirstOrDefault().InternalNo)
                            , StockControl.dbClss.TSt(g.FirstOrDefault().ConsumptionUOM));

                    //dgvBom.Rows.Add(Row.ToString()
                    //    , txtInternalNo.Text.Trim()
                    //    , CodeNo
                    //    //, StockControl.dbClss.TSt(g.FirstOrDefault().InternalNo)
                    //     , dbClss.TSt(g.FirstOrDefault().InternalName)
                    //     , dbClss.TSt(g.FirstOrDefault().InternalDescription)
                    //     , dbClss.TSt(g.FirstOrDefault().GroupType)
                    //    , StockControl.dbClss.TSt(g.FirstOrDefault().InventoryGroup)
                    //    , OrderQty
                    //    //, StockControl.dbClss.TDe(g.FirstOrDefault().PCSUnit)
                    //    , StockControl.dbClss.TSt(g.FirstOrDefault().ConsumptionUOM)
                    //    , PCSUnit
                    //    , 0
                    //    , 0//1 * StockControl.dbClss.TDe(g.FirstOrDefault().StandardCost)
                    //    , 0
                    //    , 0
                    //    , dbClss.TSt(g.FirstOrDefault().ReplenishmentType)
                    //    );

                    Add_Item(Row.ToString(), txtInternalNo.Text.Trim()
                        , CodeNo
                        , dbClss.TSt(g.FirstOrDefault().InternalName)
                         , dbClss.TSt(g.FirstOrDefault().InternalDescription)
                         , dbClss.TSt(g.FirstOrDefault().GroupType)
                         , StockControl.dbClss.TSt(g.FirstOrDefault().InventoryGroup)
                         , OrderQty
                         , StockControl.dbClss.TSt(g.FirstOrDefault().ConsumptionUOM)
                         , PCSUnit
                         , 0, 0, 0, 0, dbClss.TSt(g.FirstOrDefault().ReplenishmentType)
                         );
                }
            }
        }
        private void Add_Item(string No, string dgvPartNo, string dgvComponent
        , string dgvComponentName,string dgvComponentDescription,string dgvGroupType
            ,string dgvType
            , decimal dgvQty,  string dgvUnit, decimal dgvPCSUnit
       , decimal dgvUnitCost, decimal dgvCost, int dgvid, int dgvLine, string dgvReplenishmentType
    
        )
        {
            try
            {
                int rowindex = -1;
                GridViewRowInfo ee;
                if (rowindex == -1)
                {
                    ee = dgvBom.Rows.AddNew();
                }
                else
                    ee = dgvBom.Rows[rowindex];

                ee.Cells["dgvNo"].Value = No.ToString();
                ee.Cells["dgvPartNo"].Value = dgvPartNo;
                ee.Cells["dgvComponent"].Value = dgvComponent;
                ee.Cells["dgvComponentName"].Value = dgvComponentName;
                ee.Cells["dgvComponentDescription"].Value = dgvComponentDescription;
                ee.Cells["dgvGroupType"].Value = dgvGroupType;
                ee.Cells["dgvType"].Value = dgvType;
                ee.Cells["dgvQty"].Value = dgvQty;
                ee.Cells["dgvUnit"].Value = dgvUnit;
                ee.Cells["dgvPCSUnit"].Value = dgvPCSUnit;
                ee.Cells["dgvUnitCost"].Value = dgvUnitCost;
                ee.Cells["dgvCost"].Value = dgvCost;
                ee.Cells["dgvid"].Value = dgvid;
                ee.Cells["dgvLine"].Value = dgvLine;
                ee.Cells["dgvReplenishmentType"].Value = dgvReplenishmentType;
              
                
                ////if (lblStatus.Text.Equals("Completed"))//|| lbStatus.Text.Equals("Reject"))
                ////    dgvData.AllowAddNewRow = false;
                ////else
                ////    dgvData.AllowAddNewRow = true;

                ////dbclass.SetRowNo1(dgvData);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : Add_Item", this.Name); }

        }

        private void cboRouting_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if(Cath01==0)
                //txtVenderName.Text = cboVendor.SelectedValue.ToString();
                if (!cboVendorName.Text.Equals(""))
                {
                    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    {
                        var I = (from ix in db.mh_Routings select ix).Where(a => a.RoutingNo == cboRouting.Text).ToList();
                        if (I.Count > 0)
                            txtidRouting.Text = dbClss.TSt(I.FirstOrDefault().id);
                    }
                }
            }
            catch { }
        }

        private void cboRouting_Leave(object sender, EventArgs e)
        {
            cboRouting_SelectedIndexChanged(null, null);
        }

        private void txtBom_Leave(object sender, EventArgs e)
        {
            DataLoad_BOM();
        }

        private void dgvBom_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            try
            {
                if (e.Column.Name.Equals("dgvUnit"))
                {
                    /////////////มีการ เคลียร์ การ Add ก่อน แล้วค่อย Add ใหม่////////////////
                    //Row = e.RowIndex;
                    RadMultiColumnComboBoxElement Comcol = (RadMultiColumnComboBoxElement)e.ActiveEditor;
                    Comcol.Columns.Clear();

                    //RadMultiColumnComboBoxElement Comcol = (RadMultiColumnComboBoxElement)e.ActiveEditor;
                    Comcol.DropDownSizingMode = SizingMode.UpDownAndRightBottom;
                    Comcol.DropDownWidth = 150;
                    Comcol.DropDownHeight = 150;
                    //Comcol.EditorControl.BestFitColumns(BestFitColumnMode.AllCells);
                    Comcol.EditorControl.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                    //ปรับอัตโนมัติ
                    //Comcol.EditorControl.AutoGenerateColumns = false;
                    //Comcol.BestFitColumns(true, true);
                    Comcol.AutoFilter = true;

                    //Comcol.EditorControl.AllowAddNewRow = true;
                    Comcol.EditorControl.EnableFiltering = true;
                    Comcol.EditorControl.ReadOnly = false;
                    Comcol.ClearFilter();


                    //Comcol.DisplayMember = "ItemNo";
                    //Comcol.ValueMember = "ItemNo";

                    // //----------------------------- ปรับโดยกำหนดเอง
                    Comcol.EditorControl.Columns.Add(new GridViewTextBoxColumn
                    {
                        HeaderText = "UOM",
                        Name = "UnitCode",
                        FieldName = "UnitCode",
                        Width = 100,
                        AllowFiltering = true,
                        ReadOnly = false
                    }
                   );
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void dgvBom_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            try
            {
                dgvBom.EndEdit();
                if (e.RowIndex >= -1)
                {

                    if (dgvBom.Columns["dgvQty"].Index == e.ColumnIndex
                        || dgvBom.Columns["dgvUnitCost"].Index == e.ColumnIndex
                        )
                    {
                        decimal OrderQty = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["dgvQty"].Value), out OrderQty);
                        decimal StandardCost = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["dgvUnitCost"].Value), out StandardCost);
                        e.Row.Cells["dgvCost"].Value = OrderQty * StandardCost;
                    }

                    else if (dgvBom.Columns["dgvUnit"].Index == e.ColumnIndex)
                    {
                        string dgvUOM = dbClss.TSt(e.Row.Cells["dgvUnit"].Value);
                        string CodeNo = dbClss.TSt(e.Row.Cells["dgvComponent"].Value);
                        decimal PCSUOM = dbClss.Con_UOM(CodeNo, dgvUOM);
                        e.Row.Cells["dgvPCSUnit"].Value = dbClss.Con_UOM(CodeNo, dgvUOM);
                        //using (DataClasses1DataContext db = new DataClasses1DataContext())
                        //{
                        //    var g = (from ix in db.mh_Items select ix)
                        //        .Where(a => a.InternalNo.ToUpper().Trim().Equals(CodeNo.ToUpper().Trim())).ToList();
                        //    if (g.Count > 0)
                        //    {
                        //        if (dgvUOM == dbClss.TSt(g.FirstOrDefault().PurchaseUOM))
                        //        {
                        //            e.Row.Cells["dgvPCSUnit"].Value = dbClss.Con_UOM(CodeNo, dgvUOM);
                        //            //e.Row.Cells["dgvPCSUnit"].ReadOnly = true;
                        //        }                           
                        //    }

                        //}
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void radButtonElement2_Click(object sender, EventArgs e)
        {
            tb_Master_ItemUOM a = new tb_Master_ItemUOM();
            a.Show();
        }

        private void cboBOM_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if(Cath01==0)
                //txtVenderName.Text = cboVendor.SelectedValue.ToString();
                if (!cboBOM.Text.Equals(""))
                {
                    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    {
                        var I = (from ix in db.tb_BomHDs select ix).Where(a => a.PartNo == txtInternalNo.Text.Trim() && a.BomNo == cboBOM.Text).ToList();
                        if (I.Count > 0)
                            txtidBOM.Text = dbClss.TSt(I.FirstOrDefault().id);
                        else
                        {
                            txtidBOM.Text = "0";
                            txtVersion.Text = "";
                        }
                    }
                }
                else
                    txtidBOM.Text = "0";
            }
            catch { }
        }

        private void radButtonElement3_Click(object sender, EventArgs e)
        {
            Bom_List a = new Bom_List();
            a.ShowDialog();

        }
    }
}
