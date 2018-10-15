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
using System.Globalization;
using Microsoft.VisualBasic;
using Telerik.WinControls.Data;
using Telerik.WinControls;

namespace StockControl
{
    public partial class CreatePO : Telerik.WinControls.UI.RadRibbonForm
    {
        public CreatePO()
        {
            InitializeComponent();
        }
        public CreatePO(string TempNo)
        {
            InitializeComponent();
            TempNo_temp = TempNo;
        }
        public CreatePO(List<GridViewRowInfo> RetDT)
        {
            InitializeComponent();
            this.RetDT = RetDT;
        }
        //private int RowView = 50;
        //private int ColView = 10;
        //DataTable dt = new DataTable();
        List<GridViewRowInfo> RetDT;
        string TempNo_temp = "";
        DataTable dt_POHD = new DataTable();
        DataTable dt_PODT = new DataTable();
        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            HistoryView hw = new HistoryView(this.Name, txtPONo.Text);
            this.Cursor = Cursors.Default;
            hw.ShowDialog();
        }

        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }
        private void GETDTRow()
        {
            dt_POHD.Columns.Add(new DataColumn("PONo", typeof(string)));
            dt_POHD.Columns.Add(new DataColumn("TempPNo", typeof(string)));
            dt_POHD.Columns.Add(new DataColumn("VendorNo", typeof(string)));
            dt_POHD.Columns.Add(new DataColumn("VendorName", typeof(string)));
            dt_POHD.Columns.Add(new DataColumn("Address", typeof(string)));
            dt_POHD.Columns.Add(new DataColumn("ContactName", typeof(string)));
            dt_POHD.Columns.Add(new DataColumn("Tel", typeof(string)));
            dt_POHD.Columns.Add(new DataColumn("Fax", typeof(string)));
            dt_POHD.Columns.Add(new DataColumn("Email", typeof(string)));
            dt_POHD.Columns.Add(new DataColumn("ClearBill", typeof(bool)));
            dt_POHD.Columns.Add(new DataColumn("Duedate", typeof(DateTime)));
            dt_POHD.Columns.Add(new DataColumn("Remark", typeof(string)));
            dt_POHD.Columns.Add(new DataColumn("Status", typeof(string)));
            dt_POHD.Columns.Add(new DataColumn("CreateDate", typeof(DateTime)));
            dt_POHD.Columns.Add(new DataColumn("CreateBy", typeof(string)));
            dt_POHD.Columns.Add(new DataColumn("ModifyDate", typeof(DateTime)));
            dt_POHD.Columns.Add(new DataColumn("ModifyBy", typeof(string)));
            dt_POHD.Columns.Add(new DataColumn("CRRNCY", typeof(string)));
            dt_POHD.Columns.Add(new DataColumn("Barcode", typeof(string)));
            dt_POHD.Columns.Add(new DataColumn("Quotation", typeof(string)));
            dt_POHD.Columns.Add(new DataColumn("Rev", typeof(int)));
            dt_POHD.Columns.Add(new DataColumn("ApproveDate", typeof(DateTime)));
            dt_POHD.Columns.Add(new DataColumn("ApproveBy", typeof(string)));
            dt_POHD.Columns.Add(new DataColumn("ConfirmDate", typeof(DateTime)));
            dt_POHD.Columns.Add(new DataColumn("ConfirmBy", typeof(string)));
            dt_POHD.Columns.Add(new DataColumn("Total", typeof(decimal)));
            dt_POHD.Columns.Add(new DataColumn("Discount", typeof(decimal)));
            dt_POHD.Columns.Add(new DataColumn("Discpct", typeof(decimal)));
            dt_POHD.Columns.Add(new DataColumn("AfterDiscount", typeof(decimal)));
            dt_POHD.Columns.Add(new DataColumn("vat", typeof(decimal)));
            dt_POHD.Columns.Add(new DataColumn("VatDetail", typeof(decimal)));
            dt_POHD.Columns.Add(new DataColumn("GrandTotal", typeof(decimal)));
            dt_POHD.Columns.Add(new DataColumn("id", typeof(int)));
            dt_POHD.Columns.Add(new DataColumn("VATGroup", typeof(string)));
            dt_POHD.Columns.Add(new DataColumn("VATType", typeof(string)));
            dt_POHD.Columns.Add(new DataColumn("Version", typeof(string)));

            dt_PODT.Columns.Add(new DataColumn("id", typeof(string)));
            dt_PODT.Columns.Add(new DataColumn("TempPNo", typeof(string)));
            dt_PODT.Columns.Add(new DataColumn("PONo", typeof(string)));
            dt_PODT.Columns.Add(new DataColumn("PRNo", typeof(string)));
            dt_PODT.Columns.Add(new DataColumn("CodeNo", typeof(string)));
            dt_PODT.Columns.Add(new DataColumn("ItemName", typeof(string)));
            dt_PODT.Columns.Add(new DataColumn("ItemDesc", typeof(string)));
            dt_PODT.Columns.Add(new DataColumn("GroupCode", typeof(string)));
            dt_PODT.Columns.Add(new DataColumn("OrderQty", typeof(decimal)));
            dt_PODT.Columns.Add(new DataColumn("BackOrder", typeof(decimal)));
            dt_PODT.Columns.Add(new DataColumn("Unit", typeof(string)));
            dt_PODT.Columns.Add(new DataColumn("PCSUnit", typeof(decimal)));
            dt_PODT.Columns.Add(new DataColumn("Cost", typeof(decimal)));
            dt_PODT.Columns.Add(new DataColumn("Rate", typeof(decimal)));
            dt_PODT.Columns.Add(new DataColumn("Amount", typeof(decimal)));
            dt_PODT.Columns.Add(new DataColumn("RealCost", typeof(decimal)));
            dt_PODT.Columns.Add(new DataColumn("PRItem", typeof(string)));
            dt_PODT.Columns.Add(new DataColumn("LineName", typeof(string)));
            dt_PODT.Columns.Add(new DataColumn("MCName", typeof(string)));
            dt_PODT.Columns.Add(new DataColumn("SerialNo", typeof(string)));
            dt_PODT.Columns.Add(new DataColumn("LotNo", typeof(string)));
            dt_PODT.Columns.Add(new DataColumn("Remark", typeof(string)));
            dt_PODT.Columns.Add(new DataColumn("SS", typeof(int)));
            dt_PODT.Columns.Add(new DataColumn("idCSTMPODt", typeof(int)));


        }

        string Ac = "";
        private void Unit_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                //dgvData.ReadOnly = true;
                dgvData.AutoGenerateColumns = false;
                GETDTRow();
                //LoadRunningPO();
                LoadCurrency();
                LoadVendor();

                ClearData();
                btnNew_Click(null, null);

                if (RetDT != null)
                {

                    if (RetDT.Count > 0)
                    {
                        btnNew_Click(null, null);
                        CreatePR_from_WaitingPR();
                    }
                }
                else
                {
                    if (!TempNo_temp.Equals(""))
                    {
                        txtTempNo.Text = TempNo_temp;
                        Enable_Status(false, "View");
                        btnView_Click(null, null);
                        //txtTempNo.Text = TempNo_temp;
                        DataLoad();

                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        private void LoadVendor()
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
                //cboVendorName.MultiColumnComboBoxElement.DropDownWidth = 200;
                //cboVendorName.MultiColumnComboBoxElement.EditorControl.Columns["No"].Width = 80;
                //cboVendorName.MultiColumnComboBoxElement.EditorControl.Columns["Name"].Width = 100;
                //cboVendorName.DropDownSizingMode = SizingMode.UpDownAndRightBottom;
                //cboVendorName.DropDownMinSize = new Size(200, 200);                
                cboVendorName.DataSource = db.mh_Vendors.Where(s => s.Active == true).ToList();
                cboVendorName.SelectedIndex = -1;
                cboVendorName.Text = "";

                //this.cboVendorName.AutoFilter = true;
                //this.cboVendorName.AutoCompleteMode = AutoCompleteMode.Append;
                //FilterDescriptor fi = new FilterDescriptor();
                //fi.PropertyName = this.cboVendorName.ValueMember;
                //fi.Operator = FilterOperator.StartsWith;
                //this.cboVendorName.EditorControl.MasterTemplate.FilterDescriptors.Add(fi);

                //cboVendorName.AutoCompleteMode = AutoCompleteMode.Append;
                //cboVendorName.DisplayMember = "VendorName";
                //cboVendorName.ValueMember = "VendorNo";              

                //var g = (from ix in db.mh_Vendors
                //         //join i in db.mh_CRRNCies on ix.DefaultCurrency equals i.id
                //         where ix.Active == true

                //         select new
                //         {                           
                //             VendorNo = ix.No,
                //             VendorName = ix.Name  
                //             //,Currency = i.Crrncy 
                //             //,Rate = i.Rate                        
                //         }
                //            ).ToList();
                //cboVendorName.DataSource = g;
                //cboVendorName.SelectedIndex = -1;
                //cboVendorName.Text = "";

                this.cboVatGroup.AutoFilter = true;
                this.cboVatGroup.AutoCompleteMode = AutoCompleteMode.Append;
                FilterDescriptor fg = new FilterDescriptor();
                fg.PropertyName = this.cboVatGroup.ValueMember;
                fg.Operator = FilterOperator.StartsWith;
                this.cboVatGroup.EditorControl.MasterTemplate.FilterDescriptors.Add(fg);

                cboVatGroup.AutoCompleteMode = AutoCompleteMode.Append;
                cboVatGroup.DisplayMember = "VatGroup";
                cboVatGroup.ValueMember = "id";

                var vg = (from ix in db.mh_VATSetups
                          where ix.Active == true
                          select new
                          {
                              VatGroup = ix.VatGroup,
                              VatType = ix.VatType
                              ,
                              Rate = ix.Vat_
                              ,
                              id = ix.id
                          }
                            ).ToList();
                cboVatGroup.DataSource = vg;
                cboVatGroup.SelectedIndex = 0;
                cboVatGroup.Text = "";

                try
                {

                    ddlCurrency.DataSource = (from ix in db.mh_CRRNCies.Where(s => s.Active == true)
                                              select new { ix.Crrncy }).ToList();
                    ddlCurrency.DisplayMember = "Crrncy";
                    ddlCurrency.Text = "";

                    ////GridViewMultiComboBoxColumn col = (GridViewMultiComboBoxColumn)radGridView1.Columns["CodeNo"];
                    ////col.DataSource = (from ix in db.mh_CRRNCies.Where(s => s.Active.Equals(true))
                    ////                  select new { ix.CodeNo, ix.ItemDescription }).ToList();
                    //col.DisplayMember = "CodeNo";
                    //col.ValueMember = "CodeNo";

                    //col.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDown;
                    //col.FilteringMode = GridViewFilteringMode.DisplayMember;
                    //col.AutoSizeMode = BestFitColumnMode.DisplayedDataCells;
                }
                catch (Exception ex) { dbClss.AddError("CRRNCY", ex.Message, this.Name); }


                GridViewMultiComboBoxColumn Groupc = (GridViewMultiComboBoxColumn)dgvData.Columns["dgvGroupCode"];
                Groupc.DataSource = (from ix in db.mh_GroupTypes.Where(s => s.GroupActive == true)
                                     select new { ix.GroupCode, ix.GroupName }).ToList();
                //Comcol.DataSource = aaa;
                Groupc.DisplayMember = "GroupCode";
                Groupc.DropDownStyle = RadDropDownStyle.DropDown;


                GridViewMultiComboBoxColumn Uom = (GridViewMultiComboBoxColumn)dgvData.Columns["dgvUnit"];
                Uom.DataSource = (from ix in db.mh_Units.Where(s => s.UnitActive == true)
                                  select new { ix.UnitCode }).ToList();
                Uom.DisplayMember = "UnitCode";
                Uom.DropDownStyle = RadDropDownStyle.DropDown;



                //col.TextAlignment = ContentAlignment.MiddleCenter;
                //col.Name = "CodeNo";
                //this.radGridView1.Columns.Add(col);

                //this.radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                //this.radGridView1.CellEditorInitialized += radGridView1_CellEditorInitialized;
            }
        }
        private void LoadCurrency()
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var G = (from ix in db.tb_CRRNCies select ix).ToList();
                    ddlCurrency.DataSource = G;
                    ddlCurrency.DisplayMember = "CRRNCY";
                    ddlCurrency.Text = "";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("CRRNCY", ex.Message, this.Name);
            }
        }
        //private void LoadRunningPO()
        //{
        //    try
        //    {
        //        using (DataClasses1DataContext db = new DataClasses1DataContext())
        //        {
        //            var G = (from ix in db.sp_048_Running_PO() select ix).ToList();
        //            ddlFactory.DataSource = G;
        //            ddlFactory.DisplayMember = "Location";
        //            ddlFactory.Text = "";

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        dbClss.AddError("CRRNCY", ex.Message, this.Name);
        //    }

        //}
        private void DataLoad()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                dt_PODT.Rows.Clear();
                dt_POHD.Rows.Clear();
                int ck = 0;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var g = (from ix in db.mh_PurchaseOrders select ix)
                        .Where(a => a.TempPNo == txtTempNo.Text.Trim()
                         && (a.Status != "Cancel")
                         ).ToList();
                    if (g.Count() > 0)
                    {

                        DateTime? temp_date = null;
                        cboVatType.Text = dbClss.TSt(g.FirstOrDefault().VATType);
                        cboVatGroup.Text = dbClss.TSt(g.FirstOrDefault().VATGroup);
                        txtPONo.Text = StockControl.dbClss.TSt(g.FirstOrDefault().PONo);
                        //txtTempNo.Text = StockControl.dbClss.TSt(g.FirstOrDefault().TEMPNo);
                        txtVendorNo.Text = StockControl.dbClss.TSt(g.FirstOrDefault().VendorNo);
                        cboVendorName.Text = StockControl.dbClss.TSt(g.FirstOrDefault().VendorName);
                        txtTel.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Tel);
                        txtContactName.Text = StockControl.dbClss.TSt(g.FirstOrDefault().ContactName);
                        ddlCurrency.Text = StockControl.dbClss.TSt(g.FirstOrDefault().CRRNCY);
                        txtFax.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Fax);
                        txtEmail.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Email);
                        txtAddress.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Address);
                        txtRemarkHD.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Remark);
                        cbClearBill.Checked = StockControl.dbClss.TBo(g.FirstOrDefault().ClearBill);
                        txtVattax.Text = StockControl.dbClss.TInt(g.FirstOrDefault().VatTax).ToString("##,###,##0.00");
                        lbTotalOrder.Text = StockControl.dbClss.TDe(g.FirstOrDefault().GrandTotal).ToString("##,###,##0.00");
                        txtLessPoDiscountAmount.Text = StockControl.dbClss.TDe(g.FirstOrDefault().Discount).ToString("##,###,##0.00");
                        txtLessPoDiscountAmountPersen.Text = StockControl.dbClss.TDe(g.FirstOrDefault().Discpct).ToString("##,###,##0.00");
                        txtAfterDiscount.Text = StockControl.dbClss.TDe(g.FirstOrDefault().AfterDiscount).ToString("##,###,##0.00");
                        lbOrderSubtotal.Text = StockControl.dbClss.TDe(g.FirstOrDefault().Total).ToString("##,###,##0.00");
                        txtVat.Text = StockControl.dbClss.TDe(g.FirstOrDefault().vat).ToString("##,###,##0.00");
                        txtSeqStatus.Text = dbClss.TInt(g.FirstOrDefault().SeqStatus).ToSt();
                        txtVersion.Text = dbClss.TSt(g.FirstOrDefault().Version);
                        txtQuotation.Text = dbClss.TSt(g.FirstOrDefault().Quotation);

                        cbvatDetail.Checked = StockControl.dbClss.TBo(g.FirstOrDefault().VatDetail);
                        if (StockControl.dbClss.TDe(txtVat.Text) > 0)
                            cbvat.Checked = true;
                        else
                            cbvat.Checked = false;


                        if (!StockControl.dbClss.TSt(g.FirstOrDefault().Duedate).Equals(""))
                            dtDuedate.Value = Convert.ToDateTime((g.FirstOrDefault().Duedate), new CultureInfo("en-US"));
                        else
                            dtDuedate.Value = Convert.ToDateTime(temp_date);




                        dt_POHD = StockControl.dbClss.LINQToDataTable(g);

                        //Detail
                        var d = (from ix in db.mh_PurchaseOrderDetails select ix)
                            .Where(a => a.TempPNo == txtTempNo.Text.Trim() && a.SS == 1).ToList();
                        if (d.Count() > 0)
                        {
                            int c = 0;
                            dgvData.DataSource = d;
                            dt_PODT = StockControl.dbClss.LINQToDataTable(d);

                            if (d.FirstOrDefault().DF.Equals(4))
                            {
                                LastDiscount = true;
                                lastDiscountAmount = false;

                            }
                            else if (d.FirstOrDefault().DF.Equals(5))
                            {
                                LastDiscount = true;
                                lastDiscountAmount = true;
                            }
                            if (LastDiscount)
                            {
                                if (lastDiscountAmount)
                                {
                                    CallDiscontLast(true);
                                    CallSumDiscountLast(true);
                                }
                                else
                                {
                                    CallDiscontLast(false);
                                    CallSumDiscountLast(false);
                                }
                            }
                            
                            foreach (var x in dgvData.Rows)
                            {
                                c += 1;
                                x.Cells["dgvNo"].Value = c;
                                x.Cells["dgvBackOrder2"].Value = x.Cells["dgvBackOrder"].Value;
                                x.Cells["dgvOrderQty2"].Value = x.Cells["dgvOrderQty"].Value;

                                x.Cells["dgvAmount"].Value = dbClss.TDe(x.Cells["dgvOrderQty"].Value) * dbClss.TDe(x.Cells["dgvCost"].Value);

                                if (dbClss.TDe(x.Cells["dgvBackOrder"].Value) == dbClss.TDe(x.Cells["dgvOrderQty"].Value))
                                {
                                    x.Cells["dgvStatus"].Value = "Waiting";

                                    //ถ้าบางรายการรับเข้าแล้วจะแก้ไข Item หรือ cost ไม่ได้ แก้ไขได้เฉพาะ Qty
                                    //เพราะ PO นั่นถูกอนุมัติเรียบร้อยแล้ว
                                    
                                    if (StockControl.dbClss.TSt(g.FirstOrDefault().Status) == "Process")
                                    {
                                        x.Cells["dgvOrderQty"].ReadOnly = false;
                                        x.Cells["dgvCodeNo"].ReadOnly = true;
                                        x.Cells["dgvItemName"].ReadOnly = true;
                                        x.Cells["dgvItemDesc"].ReadOnly = true;
                                        x.Cells["dgvUnit"].ReadOnly = true;
                                        x.Cells["dgvCost"].ReadOnly = true;
                                        x.Cells["dgvPCSUnit"].ReadOnly = true;
                                        x.Cells["dgvGroupCode"].ReadOnly = true;
                                    }

                                    if (dbClss.TInt(x.Cells["dgvPRItem"].Value) > 0)
                                    {
                                        x.Cells["dgvOrderQty"].ReadOnly = true;
                                        x.Cells["dgvCodeNo"].ReadOnly = true;
                                        x.Cells["dgvItemName"].ReadOnly = true;
                                        x.Cells["dgvItemDesc"].ReadOnly = true;
                                        x.Cells["dgvUnit"].ReadOnly = true;
                                        x.Cells["dgvCost"].ReadOnly = true;
                                        x.Cells["dgvPCSUnit"].ReadOnly = true;
                                        x.Cells["dgvGroupCode"].ReadOnly = true;
                                        //x.Cells["dgvDiscountAmount"].ReadOnly = true;
                                    }
                                    x.Cells["dgvChangeQty"].ReadOnly = true;                                    
                                }
                                else if (dbClss.TDe(x.Cells["dgvBackOrder"].Value) <= dbClss.TDe(x.Cells["dgvOrderQty"].Value)
                                    && dbClss.TDe(x.Cells["dgvBackOrder"].Value) != 0)
                                {
                                    x.Cells["dgvStatus"].Value = "Partial";
                                    x.Cells["dgvOrderQty"].ReadOnly = true;
                                    x.Cells["dgvCodeNo"].ReadOnly = true;
                                    x.Cells["dgvItemName"].ReadOnly = true;
                                    x.Cells["dgvItemDesc"].ReadOnly = true;
                                    x.Cells["dgvUnit"].ReadOnly = true;
                                    x.Cells["dgvCost"].ReadOnly = true;
                                    x.Cells["dgvPCSUnit"].ReadOnly = true;
                                    x.Cells["dgvDiscountAmount"].ReadOnly = true;

                                    x.Cells["dgvChangeQty"].ReadOnly = false;
                                    
                                }
                                else
                                {
                                    if (dbClss.TDe(x.Cells["dgvDiscon"].Value) > 0)
                                    {
                                        x.Cells["dgvStatus"].Value = "Discon";
                                        x.Cells["dgvDiscon_B"].Value = true;
                                        x.Cells["dgvChangeQty"].ReadOnly = false;
                                    
                                    }
                                    else
                                    {
                                        x.Cells["dgvStatus"].Value = "Full";
                                        x.Cells["dgvChangeQty"].ReadOnly = true;
                                      
                                    }

                                    x.Cells["dgvOrderQty"].ReadOnly = true;
                                    x.Cells["dgvOrderQty"].ReadOnly = true;
                                    x.Cells["dgvCodeNo"].ReadOnly = true;
                                    x.Cells["dgvItemName"].ReadOnly = true;
                                    x.Cells["dgvItemDesc"].ReadOnly = true;
                                    x.Cells["dgvUnit"].ReadOnly = true;
                                    x.Cells["dgvCost"].ReadOnly = true;
                                    x.Cells["dgvPCSUnit"].ReadOnly = true;
                                    x.Cells["dgvDiscountAmount"].ReadOnly = true;
                                }
                            }
                        }

                        CalTAX1();
                        CallTotal();
                        btnCal_Click(null, null);
                        if (cbvatDetail.Checked)
                            getTotal();

                        string Status = StockControl.dbClss.TSt(g.FirstOrDefault().Status);
                        int SeqStatus = 0;
                        SeqStatus = dbClss.TInt(txtSeqStatus.Text);

                        if (Status == "Waiting" && SeqStatus == 1)
                            Status = "Waiting Approve";
                        else if (Status == "Waiting" && SeqStatus == 2)
                            Status = "Approved";
                        //lblStatus.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Status);
                        if (Status.Equals("Cancel"))
                        {
                            //ddlFactory.Enabled = false;
                            btnNew.Enabled = true;
                            btnSave.Enabled = false;
                            btnDelete.Enabled = false;
                            btnView.Enabled = false;
                            btnEdit.Enabled = false;
                            lblStatus.Text = "Cancel";
                            dgvData.ReadOnly = true;
                            btnAdd_Item.Enabled = false;
                            btnAdd_Part.Enabled = false;
                            btnAdd_Row.Enabled = false;
                            btnDel_Item.Enabled = false;
                            btnSendApprove.Enabled = false;
                        }
                        else if
                            (Status.Equals("Completed")
                            || Status.Equals("Process")
                            || Status.Equals("Waiting Approve")
                            || Status.Equals("Approved")
                            )
                        {
                            //ddlFactory.Enabled = false;
                            btnSave.Enabled = false;
                            btnDelete.Enabled = false;
                            btnView.Enabled = false;
                            btnEdit.Enabled = false;
                            btnNew.Enabled = true;
                            lblStatus.Text = Status;// StockControl.dbClss.TSt(g.FirstOrDefault().Status);
                            dgvData.ReadOnly = true;
                            btnAdd_Item.Enabled = false;
                            btnAdd_Part.Enabled = false;
                            btnAdd_Row.Enabled = false;
                            btnDel_Item.Enabled = false;
                            btnSendApprove.Enabled = false;

                            if (lblStatus.Text == "Process")
                            {
                                dgvData.ReadOnly = false;
                                btnDiscon.Enabled = true;
                                btnDiscon_Item.Enabled = true;
                                btnchangeQty.Enabled = true;
                                btnDel_Item.Enabled = true;
                                txtAfterDiscount.Enabled = false;
                                txtLessPoDiscountAmount.Enabled = false;
                                txtLessPoDiscountAmountPersen.Enabled = false;
                            }
                        }
                        else
                        {
                            //ddlFactory.Enabled = false;
                            btnNew.Enabled = true;
                            btnSave.Enabled = true;
                            btnDelete.Enabled = true;
                            btnView.Enabled = false;
                            btnEdit.Enabled = true;
                            lblStatus.Text = Status;//StockControl.dbClss.TSt(g.FirstOrDefault().Status);
                            dgvData.ReadOnly = false;
                            btnAdd_Item.Enabled = false;
                            btnAdd_Part.Enabled = false;
                            btnAdd_Row.Enabled = false;
                            btnDel_Item.Enabled = false;
                            btnSendApprove.Enabled = true;
                            txtAfterDiscount.Enabled = true;
                            txtLessPoDiscountAmount.Enabled = true;
                            txtLessPoDiscountAmountPersen.Enabled = true;
                        }
                        foreach (var x in dgvData.Rows)
                        {
                            if (row >= 0 && row == ck && dgvData.Rows.Count > 0)
                            {
                                x.ViewInfo.CurrentRow = x;
                            }
                            ck += 1;
                        }

                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        private bool CheckDuplicate(string code, string Code2)
        {
            bool ck = false;

            //using (DataClasses1DataContext db = new DataClasses1DataContext())
            //{
            //    int i = (from ix in db.tb_Models
            //             where ix.ModelName == code

            //             select ix).Count();
            //    if (i > 0)
            //        ck = false;
            //    else
            //        ck = true;
            //}

            return ck;
        }
        private void SaveHerder()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                var g = (from ix in db.mh_PurchaseOrders
                         where ix.TempPNo.Trim() == txtTempNo.Text.Trim()
                         && ix.Status != "Cancel"
                         //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                         select ix).ToList();
                if (g.Count > 0)  //มีรายการในระบบ
                {
                    foreach (DataRow row in dt_POHD.Rows)
                    {

                        var gg = (from ix in db.mh_PurchaseOrders
                                  where ix.TempPNo.Trim() == txtTempNo.Text.Trim() && ix.Status != "Cancel"
                                  //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                                  select ix).First();

                        //gg.Status = "Waiting";
                        //gg.TEMPNo = txtTempNo.Text;
                        gg.ModifyBy = ClassLib.Classlib.User;
                        gg.ModifyDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));


                        dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไข CreatePO โดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", txtPONo.Text);

                        if (!txtPONo.Text.Trim().Equals(row["PONo"].ToString()))
                        {
                            gg.PONo = txtPONo.Text;

                            dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขเลขที่ใบสั่งซื้อ [" + txtPONo.Text.Trim() + "]", txtPONo.Text);
                        }

                        if (StockControl.dbClss.TSt(gg.Barcode).Equals(""))
                            gg.Barcode = StockControl.dbClss.SaveQRCode2D(txtPONo.Text.Trim());

                        if (!cboVatType.Text.Trim().Equals(row["VATType"].ToString()))
                        {
                            gg.VATType = cboVatType.Text;
                            dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขประเภทภาษี [" + cboVatType.Text.Trim() + "]", txtPONo.Text);
                        }
                        if (!cboVatGroup.Text.Trim().Equals(row["VATGroup"].ToString()))
                        {
                            gg.VATGroup = cboVatGroup.Text;
                            dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขกลุ่มภาษี [" + cboVatGroup.Text.Trim() + "]", txtPONo.Text);
                        }
                        if (!txtVendorNo.Text.Trim().Equals(row["VendorNo"].ToString()))
                        {
                            gg.VendorName = cboVendorName.Text;
                            gg.VendorNo = txtVendorNo.Text.Trim();
                            dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขรหัสผู้ขาย [" + txtVendorNo.Text.Trim() + "]", txtPONo.Text);
                        }
                        if (!txtQuotation.Text.Trim().Equals(row["Quotation"].ToString()))
                        {
                            gg.Quotation = txtQuotation.Text.Trim();
                            dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขเลขที่เสนอราคา [" + txtQuotation.Text.Trim() + "]", txtPONo.Text);
                        }
                        if (!cbClearBill.Checked.ToString().Equals(row["ClearBill"].ToString()))
                        {
                            gg.ClearBill = cbClearBill.Checked;
                            dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไข ClearBill [" + cbClearBill.Checked.ToString() + "]", txtPONo.Text);
                        }
                        if (!ddlCurrency.Text.Trim().Equals(row["CRRNCY"].ToString()))
                        {
                            gg.CRRNCY = ddlCurrency.Text.Trim();
                            decimal Rate = dbClss.TDe(txtRate.Text);
                            if (Rate <= 0) Rate = 1;
                            gg.Rate = Rate;
                            dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขสกุลเงิน [" + ddlCurrency.Text.Trim() + "]", txtPONo.Text);
                        }
                        if (!txtContactName.Text.Trim().Equals(row["ContactName"].ToString()))
                        {
                            gg.ContactName = txtContactName.Text.Trim();
                            dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขผู้ติดต่อ [" + txtContactName.Text.Trim() + "]", txtPONo.Text);
                        }
                        if (!txtAddress.Text.Trim().Equals(row["Address"].ToString()))
                        {
                            gg.Address = txtAddress.Text.Trim();
                            dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขที่อยู่ [" + txtAddress.Text.Trim() + "]", txtPONo.Text);
                        }
                        if (!txtTel.Text.Trim().Equals(row["Tel"].ToString()))
                        {
                            gg.Tel = txtTel.Text.Trim();
                            dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขเบอร์โทร [" + txtTel.Text.Trim() + "]", txtPONo.Text);
                        }
                        if (!txtFax.Text.Trim().Equals(row["Fax"].ToString()))
                        {
                            gg.Fax = txtFax.Text.Trim();
                            dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขเบอร์แฟกซ์ [" + txtFax.Text.Trim() + "]", txtPONo.Text);
                        }

                        if (!txtEmail.Text.Trim().Equals(row["Email"].ToString()))
                        {
                            gg.Email = txtEmail.Text.Trim();
                            dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขอีเมลล์ [" + txtEmail.Text.Trim() + "]", txtPONo.Text);
                        }
                        if (!txtVersion.Text.Trim().Equals(row["Version"].ToString()))
                        {
                            gg.Version = dbClss.TSt(txtVersion.Text);
                            dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไข Rev. [" + txtVersion.Text.Trim() + "]", txtPONo.Text);
                        }

                        //if (!txtVat.Text.Trim().Equals(row["vat"].ToString()))
                        //{
                        //    gg.vat = StockControl.dbClss.TDe(txtVat.Text);
                        //    dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไข Vat(%) [" + txtVat.Text.Trim() + "]", txtPONo.Text);
                        //}
                        //if (!txtVattax.Text.Trim().Equals(row["VatTax"].ToString()))
                        //{
                        //    gg.VatTax = StockControl.dbClss.TDe(txtVattax.Text);
                        //    dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขราคา Vat [" + txtVattax.Text.Trim() + "]", txtPONo.Text);
                        //}
                        //if (!cbvatDetail.Checked.ToString().Equals(row["VatDetail"].ToString()))
                        //{
                        //    gg.VatDetail = StockControl.dbClss.TBo(cbvatDetail.Checked);
                        //    dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขรวม Vat จากรายการ [" + cbvatDetail.Checked.ToString() + "]", txtPONo.Text);
                        //}
                        //if (!txtDiscount.Text.Equals(row["Discount"].ToString()))
                        //{
                        //    gg.Discount = StockControl.dbClss.TDe(txtDiscount.Text);
                        //    dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขส่วนลด [" + txtDiscount.Text + "]", txtPONo.Text);
                        //}
                        //if (!txtAfterDiscount.Text.Equals(row["AfterDiscount"].ToString()))
                        //{
                        //    gg.AfterDiscount = StockControl.dbClss.TDe(txtAfterDiscount.Text);
                        //    dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขราคาหลังหักส่วนลด [" + txtAfterDiscount.Text + "]", txtPONo.Text);
                        //}
                        //if (!txtTotal.Text.Equals(row["Total"].ToString()))
                        //{
                        //    gg.Total = StockControl.dbClss.TDe(txtTotal.Text);
                        //    dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขผลรวม [" + txtTotal.Text + "]", txtPONo.Text);
                        //}
                        //if (!txtGrandTotal.Text.Equals(row["GrandTotal"].ToString()))
                        //{
                        //    gg.GrandTotal = StockControl.dbClss.TDe(txtGrandTotal.Text);
                        //    dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขราคาสุทธิ [" + txtGrandTotal.Text + "]", txtPONo.Text);
                        //}
                        gg.vat = StockControl.dbClss.TDe(txtVat.Text);
                        gg.VatTax = StockControl.dbClss.TDe(txtVattax.Text);
                        gg.VatDetail = StockControl.dbClss.TBo(cbvatDetail.Checked);
                        gg.Discount = StockControl.dbClss.TDe(txtLessPoDiscountAmount.Text);
                        gg.Discpct = StockControl.dbClss.TDe(txtLessPoDiscountAmountPersen.Text);
                        gg.AfterDiscount = StockControl.dbClss.TDe(txtAfterDiscount.Text);
                        gg.Total = StockControl.dbClss.TDe(lbOrderSubtotal.Text);
                        gg.GrandTotal = StockControl.dbClss.TDe(lbTotalOrder.Text);
                        gg.Usefixunit = StockControl.dbClss.TBo(cbUsefixunit.Checked);
                        gg.CHStatus = "Waiting";


                        if (!dtDuedate.Text.Trim().Equals(""))
                        {
                            string date1 = "";
                            date1 = dtDuedate.Value.ToString("yyyyMMdd", new CultureInfo("en-US"));
                            string date2 = "";
                            if (!StockControl.dbClss.TSt(row["Duedate"].ToString()).Equals(""))
                            {
                                DateTime temp = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                temp = Convert.ToDateTime(row["Duedate"]);
                                date2 = temp.ToString("yyyyMMdd", new CultureInfo("en-US"));

                            }
                            if (!date1.Equals(date2))
                            {
                                DateTime? RequireDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                if (!dtDuedate.Text.Equals(""))
                                    RequireDate = dtDuedate.Value;
                                gg.Duedate = RequireDate;
                                dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขวันที่ต้องการ [" + dtDuedate.Text.Trim() + "]", txtPONo.Text);

                            }

                        }
                        if (!txtRemarkHD.Text.Trim().Equals(row["Remark"].ToString()))
                        {
                            gg.Remark = txtRemarkHD.Text.Trim();
                            dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขหมายเหตุ [" + txtRemarkHD.Text.Trim() + "]", txtPONo.Text);
                        }


                        db.SubmitChanges();
                    }
                }
                else  // Add ใหม่
                {
                    byte[] barcode = null;
                    if (!txtPONo.Text.Equals(""))
                        barcode = StockControl.dbClss.SaveQRCode2D(txtPONo.Text.Trim());
                    //DateTime? UpdateDate = null;

                    mh_PurchaseOrder gg = new mh_PurchaseOrder();
                    //gg.LocationRunning = ddlFactory.Text;
                    gg.ModifyBy = ClassLib.Classlib.User;
                    gg.ModifyDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    gg.CreateBy = ClassLib.Classlib.User;
                    gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    gg.VendorName = cboVendorName.Text;
                    gg.VendorNo = txtVendorNo.Text.Trim();
                    gg.VATGroup = cboVatGroup.Text;
                    gg.VATType = cboVatType.Text;
                    gg.Address = txtAddress.Text.Trim();
                    gg.Tel = txtTel.Text.Trim();
                    gg.Fax = txtFax.Text.Trim();
                    gg.ContactName = txtContactName.Text.Trim();
                    gg.Email = txtEmail.Text.Trim();
                    gg.Barcode = barcode;
                    gg.PONo = txtPONo.Text.Trim();
                    gg.TempPNo = txtTempNo.Text;
                    gg.Quotation = txtQuotation.Text.Trim();
                    gg.ClearBill = cbClearBill.Checked;
                    decimal Rate = dbClss.TDe(txtRate.Text);
                    if (Rate <= 0) Rate = 1;
                    gg.Rate = Rate;
                    gg.vat = StockControl.dbClss.TDe(txtVat.Text);
                    gg.VatDetail = StockControl.dbClss.TBo(cbvatDetail.Checked);
                    gg.Total = StockControl.dbClss.TDe(lbOrderSubtotal.Text);
                    gg.GrandTotal = StockControl.dbClss.TDe(lbTotalOrder.Text);
                    gg.AfterDiscount = StockControl.dbClss.TDe(txtAfterDiscount.Text);
                    gg.Discount = StockControl.dbClss.TDe(txtLessPoDiscountAmount.Text);
                    gg.Discpct = StockControl.dbClss.TDe(txtLessPoDiscountAmountPersen.Text);
                    gg.VatTax = StockControl.dbClss.TDe(txtVattax.Text);
                    gg.Usefixunit = StockControl.dbClss.TBo(cbUsefixunit.Checked);
                    gg.Version = dbClss.TSt(txtVersion.Text);

                    DateTime? Duedate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    if (!dtDuedate.Text.Equals(""))
                        Duedate = dtDuedate.Value;
                    else
                        dtDuedate.Value = Duedate.Value;


                    gg.Duedate = Duedate;
                    gg.Remark = txtRemarkHD.Text.Trim();
                    gg.CRRNCY = ddlCurrency.Text.Trim();
                    gg.Status = "Waiting";
                    gg.CHStatus = "Waiting";
                    gg.SeqStatus = 0;

                    db.mh_PurchaseOrders.InsertOnSubmit(gg);
                    db.SubmitChanges();

                    dbClss.AddHistory(this.Name, "เพิ่ม CreatePO", "สร้าง PONo [" + txtPONo.Text.Trim() + ",เลขที่อ้างอิง :" + txtTempNo.Text + "]", txtPONo.Text);

                }
            }
        }
        private bool AddPR_d()
        {

            bool ck = false;
            //int C = 0;
            //try
            //{


            dgvData.EndEdit();
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                foreach (var g in dgvData.Rows)
                {
                    if (g.IsVisible.Equals(true))
                    {
                        DateTime? d = null;
                        DateTime? DeliveryDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                        if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) <= 0)  //New ใหม่
                        {

                            mh_PurchaseOrderDetail u = new mh_PurchaseOrderDetail();
                            u.PONo = txtPONo.Text;
                            u.TempPNo = txtTempNo.Text;
                            u.CodeNo = StockControl.dbClss.TSt(g.Cells["dgvCodeNo"].Value);
                            u.ItemName = StockControl.dbClss.TSt(g.Cells["dgvItemName"].Value);
                            u.ItemDesc = StockControl.dbClss.TSt(g.Cells["dgvItemDesc"].Value);
                            u.GroupCode = StockControl.dbClss.TSt(g.Cells["dgvGroupCode"].Value);
                            u.OrderQty = StockControl.dbClss.TDe(g.Cells["dgvOrderQty"].Value);
                            u.PCSUnit = StockControl.dbClss.TDe(g.Cells["dgvPCSUnit"].Value);
                            u.Unit = StockControl.dbClss.TSt(g.Cells["dgvUnit"].Value);
                            u.PRItem = StockControl.dbClss.TInt(g.Cells["dgvPRItem"].Value);
                            //u.RealCost = StockControl.dbClss.TDe(g.Cells["dgvRealCost"].Value);
                            u.Cost = StockControl.dbClss.TDe(g.Cells["dgvCost"].Value);
                            //u.Remark = StockControl.dbClss.TSt(g.Cells["dgvRemark"].Value);
                            //u.LotNo = StockControl.dbClss.TSt(g.Cells["dgvLotNo"].Value);
                            //u.SerialNo = StockControl.dbClss.TSt(g.Cells["dgvSerialNo"].Value);
                            //u.MCName = StockControl.dbClss.TSt(g.Cells["dgvMCName"].Value);
                            //u.LineName = StockControl.dbClss.TSt(g.Cells["dgvLineName"].Value);
                            u.PRNo = StockControl.dbClss.TSt(g.Cells["dgvPRNo"].Value);
                            u.BackOrder = StockControl.dbClss.TDe(g.Cells["dgvBackOrder"].Value);
                            u.Amount = dbClss.TDe(g.Cells["dgvAmount"].Value);
                            u.Discount = dbClss.TDe(g.Cells["dgvDiscount"].Value);
                            u.DiscountAmount = dbClss.TDe(g.Cells["dgvDiscountAmount"].Value);
                            u.ExtendedCost = dbClss.TDe(g.Cells["dgvExtendedCost"].Value);
                            u.DF = dbClss.TInt(g.Cells["dgvDF"].Value);
                            DeliveryDate = dtDuedate.Value;
                            if (!StockControl.dbClss.TSt(g.Cells["dgvDeliveryDate"].Value).Equals(""))
                                DeliveryDate = StockControl.dbClss.TDa(g.Cells["dgvDeliveryDate"].Value);
                            decimal Rate = 0; decimal.TryParse(StockControl.dbClss.TSt(txtRate.Text), out Rate);
                            if (Rate >= 0)
                                Rate = 1;
                            u.Rate = Rate;

                            u.DeliveryDate = DeliveryDate;
                            u.SS = 1;
                            u.idCSTMPODt = dbClss.TInt(g.Cells["idCSTMPODt"].Value);
                            db.mh_PurchaseOrderDetails.InsertOnSubmit(u);
                            db.SubmitChanges();
                            //C += 1;
                            dbClss.AddHistory(this.Name, "เพิ่ม Item PO", "เพิ่มรายการ Create PO [" + u.CodeNo + "]", txtPONo.Text);

                        }
                        else  // อัพเดต
                        {

                            if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) > 0
                                && StockControl.dbClss.TSt(g.Cells["dgvStatus"].Value) != "Full"
                                && StockControl.dbClss.TSt(g.Cells["dgvStatus"].Value) != "Partial"
                                )
                            {
                                foreach (DataRow row in dt_PODT.Rows)
                                {
                                    if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) == StockControl.dbClss.TInt(row["id"]))
                                    {
                                        var u = (from ix in db.mh_PurchaseOrderDetails
                                                 where ix.TempPNo == txtTempNo.Text.Trim()
                                                 // && ix.TempNo == txtTempNo.Text                                             
                                                 && ix.id == StockControl.dbClss.TInt(g.Cells["dgvid"].Value)
                                                 select ix).First();

                                        dbClss.AddHistory(this.Name, "แก้ไขรายการ Item PO", "id :" + StockControl.dbClss.TSt(g.Cells["dgvid"].Value)
                                        + " CodeNo :" + StockControl.dbClss.TSt(g.Cells["dgvCodeNo"].Value)
                                        + " แก้ไขโดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", txtPONo.Text);

                                        u.PONo = txtPONo.Text.Trim();
                                        u.Amount = dbClss.TDe(g.Cells["dgvAmount"].Value);
                                        u.Discount = dbClss.TDe(g.Cells["dgvDiscount"].Value);
                                        u.DiscountAmount = dbClss.TDe(g.Cells["dgvDiscountAmount"].Value);
                                        u.ExtendedCost = dbClss.TDe(g.Cells["dgvExtendedCost"].Value);
                                        u.DF = dbClss.TInt(g.Cells["dgvDF"].Value);

                                        if (!StockControl.dbClss.TSt(g.Cells["dgvCodeNo"].Value).Equals(row["CodeNo"].ToString()))
                                        {
                                            u.CodeNo = StockControl.dbClss.TSt(g.Cells["dgvCodeNo"].Value);
                                            dbClss.AddHistory(this.Name, "แก้ไข Item PO", "แก้ไขรหัสทูล [" + u.CodeNo + "]", txtPONo.Text);
                                        }

                                        if (!StockControl.dbClss.TSt(g.Cells["dgvItemName"].Value).Equals(row["ItemName"].ToString()))
                                        {
                                            u.ItemName = StockControl.dbClss.TSt(g.Cells["dgvItemName"].Value);
                                            dbClss.AddHistory(this.Name, "แก้ไข Item PO", "แก้ไขชื่อทูล [" + u.ItemName + "]", txtPONo.Text);
                                        }
                                        if (!StockControl.dbClss.TSt(g.Cells["dgvItemDesc"].Value).Equals(row["ItemDesc"].ToString()))
                                        {
                                            u.ItemDesc = StockControl.dbClss.TSt(g.Cells["dgvItemDesc"].Value);
                                            dbClss.AddHistory(this.Name, "แก้ไข Item PO", "แก้ไขรายละเอียดทูล [" + u.ItemDesc + "]", txtPONo.Text);
                                        }
                                        u.GroupCode = StockControl.dbClss.TSt(g.Cells["dgvGroupCode"].Value);

                                        if (!StockControl.dbClss.TSt(g.Cells["dgvOrderQty"].Value).Equals(row["OrderQty"].ToString()))
                                        {
                                            decimal OrderQty = 0; decimal.TryParse(StockControl.dbClss.TSt(g.Cells["dgvOrderQty"].Value), out OrderQty);
                                            u.OrderQty = StockControl.dbClss.TDe(g.Cells["dgvOrderQty"].Value);
                                            u.BackOrder = OrderQty;
                                            dbClss.AddHistory(this.Name, "แก้ไข Item PO", "แก้ไขจำนวน [" + u.OrderQty.ToString() + "]", txtPONo.Text);
                                        }

                                        //if (!StockControl.dbClss.TSt(g.Cells["dgvBackOrder"].Value).Equals(row["BackOrder"].ToString()))
                                        //{
                                        //    decimal BackOrder = 0; decimal.TryParse(StockControl.dbClss.TSt(g.Cells["dgvBackOrder"].Value), out BackOrder);
                                        //    u.BackOrder = StockControl.dbClss.TDe(g.Cells["dgvBackOrder"].Value);
                                        //    dbClss.AddHistory(this.Name, "แก้ไข Item PO", "แก้ไขจำนวน BackOrder [" + u.BackOrder.ToString() + "]", txtPONo.Text);
                                        //}
                                        if (!StockControl.dbClss.TSt(g.Cells["dgvPCSUnit"].Value).Equals(row["PCSUnit"].ToString()))
                                        {
                                            u.PCSUnit = StockControl.dbClss.TDe(g.Cells["dgvPCSUnit"].Value);
                                            decimal dgvPCSUnit = 0; decimal.TryParse(StockControl.dbClss.TSt(g.Cells["dgvPCSUnit"].Value), out dgvPCSUnit);
                                            dbClss.AddHistory(this.Name, "แก้ไข Item PO", "แก้ไขจำนวน:หน่วย [" + u.PCSUnit.ToString() + "]", txtPONo.Text);
                                        }

                                        if (!StockControl.dbClss.TSt(g.Cells["dgvUnit"].Value).Equals(row["Unit"].ToString()))
                                        {
                                            u.Unit = StockControl.dbClss.TSt(g.Cells["dgvUnit"].Value);
                                            dbClss.AddHistory(this.Name, "แก้ไข Item PR", "แก้ไขหน่วย [" + u.Unit + "]", txtPONo.Text);
                                        }

                                        if (!StockControl.dbClss.TSt(g.Cells["dgvCost"].Value).Equals(row["Cost"].ToString()))
                                        {
                                            u.Cost = StockControl.dbClss.TDe(g.Cells["dgvCost"].Value);
                                            decimal dgvCost = 0; decimal.TryParse(StockControl.dbClss.TSt(g.Cells["dgvCost"].Value), out dgvCost);
                                            dbClss.AddHistory(this.Name, "แก้ไข Item PO", "แก้ไขราคา [" + u.Cost.ToString() + "]", txtPONo.Text);
                                        }
                                        if (!StockControl.dbClss.TSt(g.Cells["dgvPRNo"].Value).Equals(row["PRNo"].ToString()))
                                        {
                                            u.PRNo = StockControl.dbClss.TSt(g.Cells["dgvPRNo"].Value);
                                            u.PRItem = StockControl.dbClss.TInt(g.Cells["dgvPRItem"].Value);
                                            dbClss.AddHistory(this.Name, "แก้ไข Item PR", "แก้ไขเลขที่อ้างอิ่ง [" + u.PRNo + "]", txtPONo.Text);
                                        }
                                        if (!StockControl.dbClss.TSt(txtRate.Text).Equals(row["Rate"].ToString()))
                                        {
                                            decimal Rate = 0; decimal.TryParse(StockControl.dbClss.TSt(txtRate.Text), out Rate);
                                            if (Rate >= 0)
                                                Rate = 1;
                                            u.Rate = Rate;

                                            dbClss.AddHistory(this.Name, "แก้ไข Item Rate", "แก้ไข Rate [" + u.Rate.ToString() + "]", txtPONo.Text);
                                        }
                                        //if (!StockControl.dbClss.TSt(g.Cells["dgvPRItem"].Value).Equals(row["PRItem"].ToString()))
                                        //{
                                        //    u.PRItem = StockControl.dbClss.TInt(g.Cells["dgvPRItem"].Value);
                                        //    decimal PRItem = 0; decimal.TryParse(StockControl.dbClss.TSt(g.Cells["dgvPRItem"].Value), out PRItem);
                                        //    dbClss.AddHistory(this.Name, "แก้ไข Item PO", "แก้ไข id เลขอ้างอิ่งอ้างอิ่ง [" + u.PRItem.ToString() + "]", txtPONo.Text);
                                        //}

                                        //if (!StockControl.dbClss.TSt(g.Cells["dgvLotNo"].Value).Equals(row["LotNo"].ToString()))
                                        //{
                                        //    u.LotNo = StockControl.dbClss.TSt(g.Cells["dgvLotNo"].Value);
                                        //    dbClss.AddHistory(this.Name, "แก้ไข Item PR", "แก้ไข LotNo [" + u.LotNo + "]", txtPONo.Text);
                                        //}
                                        //if (!StockControl.dbClss.TSt(g.Cells["dgvSerialNo"].Value).Equals(row["SerialNo"].ToString()))
                                        //{
                                        //    u.SerialNo = StockControl.dbClss.TSt(g.Cells["dgvSerialNo"].Value);
                                        //    dbClss.AddHistory(this.Name, "แก้ไข Item PR", "แก้ไขซีเรียล [" + u.SerialNo + "]", txtPONo.Text);
                                        //}
                                        //if (!StockControl.dbClss.TSt(g.Cells["dgvMCName"].Value).Equals(row["MCName"].ToString()))
                                        //{
                                        //    u.MCName = StockControl.dbClss.TSt(g.Cells["dgvMCName"].Value);
                                        //    dbClss.AddHistory(this.Name, "แก้ไข Item PR", "แก้ไขชื่อ Machine [" + u.MCName + "]", txtPONo.Text);
                                        //}
                                        //if (!StockControl.dbClss.TSt(g.Cells["dgvLineName"].Value).Equals(row["LineName"].ToString()))
                                        //{
                                        //    u.LineName = StockControl.dbClss.TSt(g.Cells["dgvLineName"].Value);
                                        //    dbClss.AddHistory(this.Name, "แก้ไข Item PR", "แก้ไขชื่อ Line [" + u.LineName + "]", txtPONo.Text);
                                        //}

                                        if (!StockControl.dbClss.TSt(g.Cells["dgvDeliveryDate"].Value).Equals(""))
                                            DeliveryDate = Convert.ToDateTime((g.Cells["dgvDeliveryDate"].Value));
                                        else
                                            DeliveryDate = dtDuedate.Value;
                                        u.DeliveryDate = DeliveryDate;
                                        u.idCSTMPODt = dbClss.TInt(g.Cells["idCSTMPODt"].Value);

                                        u.SS = 1;
                                        //C += 1;
                                        db.SubmitChanges();
                                    }
                                }
                            }
                        }

                    }
                    else //Del
                    {
                        if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) > 0)
                        {
                            var u = (from ix in db.mh_PurchaseOrderDetails
                                     where //ix.PRNo == txtPONo.Text.Trim() 
                                           //&& ix.TempNo == txtTempNo.Text 
                                      ix.id == StockControl.dbClss.TInt(g.Cells["dgvid"].Value)
                                     select ix).First();
                            u.SS = 0;

                            dbClss.AddHistory(this.Name, "ลบ Item PO", "id :" + StockControl.dbClss.TSt(g.Cells["dgvid"].Value)
                                + " CodeNo :" + StockControl.dbClss.TSt(g.Cells["dgvCodeNo"].Value)
                                + " ลบโดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", txtPONo.Text);

                            db.SubmitChanges();


                            //ปรับสถานะ PR เป็น Waiting

                            var p = (from ix in db.mh_PurchaseRequestLines
                                     where ix.RefPOid == StockControl.dbClss.TInt(g.Cells["dgvid"].Value)
                                      //&& ix.TempNo == txtTempNo.Text 
                                      && ix.id == StockControl.dbClss.TInt(g.Cells["dgvPRItem"].Value)
                                     select ix).First();
                            p.Status = "Waiting";
                            //p.RemainQty = p.OrderQty;
                            p.PoNo = "";
                            p.RefPOid = 0;



                            dbClss.AddHistory(this.Name, "ปรับสถานะ Item PR", "ลบ PO จาก POid :" + StockControl.dbClss.TSt(g.Cells["dgvid"].Value)
                                + " PONo :" + txtPONo.Text.Trim()
                                + " ปรับโดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", StockControl.dbClss.TSt(g.Cells["dgvPRNo"].Value));

                            db.SubmitChanges();

                            db.sp_023_PRHD_Cal_Status(p.TempNo, p.PRNo);

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

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }
        private void Enable_Status(bool ss, string Condition)
        {
            if (Condition.Equals("-") || Condition.Equals("New"))
            {
                txtPONo.Enabled = ss;
                cboVendorName.Enabled = ss;
                txtTel.Enabled = ss;
                txtFax.Enabled = ss;
                txtContactName.Enabled = ss;
                dtDuedate.Enabled = ss;
                dgvData.ReadOnly = false;
                txtRemarkHD.Enabled = ss;
                cboVatGroup.Enabled = ss;
                cboVatType.Enabled = ss;
                //txtCurrency.Enabled = ss;
                btnAdd_Item.Enabled = ss;
                btnAdd_Part.Enabled = ss;
                btnAdd_Row.Enabled = ss;
                btnDel_Item.Enabled = ss;
                //txtVendorNo.Enabled = ss;
                txtEmail.Enabled = ss;
                txtAddress.Enabled = ss;
                txtQuotation.Enabled = ss;
                cbClearBill.Enabled = ss;
                txtLessPoDiscountAmount.Enabled = ss;
                cbvat.Enabled = ss;
                cbvatDetail.Enabled = ss;
                txtVattax.Enabled = ss;
                txtVersion.Enabled = ss;
                txtLessPoDiscountAmountPersen.Enabled = ss;
                txtAfterDiscount.Enabled = ss;
            }
            else if (Condition.Equals("View"))
            {
                txtPONo.Enabled = ss;
                cboVendorName.Enabled = ss;
                txtTel.Enabled = ss;
                txtFax.Enabled = ss;
                txtContactName.Enabled = ss;
                dtDuedate.Enabled = ss;
                dgvData.ReadOnly = !ss;
                txtRemarkHD.Enabled = ss;
                cboVatGroup.Enabled = ss;
                cboVatType.Enabled = ss;
                //txtCurrency.Enabled = ss;
                btnAdd_Item.Enabled = ss;
                btnAdd_Part.Enabled = ss;
                btnAdd_Row.Enabled = ss;
                btnDel_Item.Enabled = ss;
                //txtVendorNo.Enabled = ss;
                txtEmail.Enabled = ss;
                txtAddress.Enabled = ss;
                txtQuotation.Enabled = ss;
                cbClearBill.Enabled = ss;
                txtLessPoDiscountAmount.Enabled = ss;
                cbvat.Enabled = ss;
                cbvatDetail.Enabled = ss;
                txtVattax.Enabled = ss;
                txtVersion.Enabled = ss;
                txtLessPoDiscountAmountPersen.Enabled = ss;
                txtAfterDiscount.Enabled = ss;
            }
            else if (Condition.Equals("Edit"))
            {
                txtPONo.Enabled = ss;
                cboVendorName.Enabled = ss;
                txtTel.Enabled = ss;
                txtFax.Enabled = ss;
                txtContactName.Enabled = ss;
                dtDuedate.Enabled = ss;
                dgvData.ReadOnly = !ss;
                txtRemarkHD.Enabled = ss;
                cboVatGroup.Enabled = ss;
                cboVatType.Enabled = ss;
                //txtCurrency.Enabled = ss;
                //txtVendorNo.Enabled = ss;
                btnAdd_Item.Enabled = ss;
                btnAdd_Part.Enabled = ss;
                btnAdd_Row.Enabled = ss;
                btnDel_Item.Enabled = ss;
                txtEmail.Enabled = ss;
                txtAddress.Enabled = ss;
                txtQuotation.Enabled = ss;
                cbClearBill.Enabled = ss;
                txtLessPoDiscountAmount.Enabled = ss;
                cbvat.Enabled = ss;
                cbvatDetail.Enabled = ss;
                txtVattax.Enabled = ss;
                txtVersion.Enabled = ss;
                txtLessPoDiscountAmountPersen.Enabled = ss;
                txtAfterDiscount.Enabled = ss;
            }
        }

        private void ClearData()
        {
            //ddlFactory.Text = "";
            txtSeqStatus.Text = "0";
            txtPONo.Text = "";
            cboVendorName.Text = "";
            txtTempNo.Text = "";
            txtTel.Text = "";
            txtFax.Text = "";
            txtAddress.Text = "";
            txtContactName.Text = "";
            //lblStatus.Text = "-";
            dtDuedate.Value = DateTime.Now;
            dgvData.Rows.Clear();
            dgvData.DataSource = null;
            txtRemarkHD.Text = "";
            ddlCurrency.Text = "";
            txtRate.Text = "";
            txtVendorNo.Text = "";
            txtEmail.Text = "";
            cbClearBill.Checked = false;
            lbOrderSubtotal.Text = "";
            txtPlusExcededTax.Text = "";
            txtLessPoDiscountAmountPersen.Text = "";
            txtLessPoDiscountAmount.Text = "";
            txtTotalsumDiscount.Text = "";
            txtAfterDiscount.Text = "";
            txtVat.Text = "";
            lbTotalOrder.Text = "0.00";
            cbvat.Checked = false;
            txtVattax.Text = "7";
            cboVatType.Text = "";
            cboVatGroup.Text = "";
            cbvatDetail.Checked = false;
            txtQuotation.Text = "";
            txtVersion.Text = "";

            dt_POHD.Rows.Clear();
            dt_PODT.Rows.Clear();
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            //ddlFactory.Enabled = true;
            btnView.Enabled = true;
            btnEdit.Enabled = true;
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            btnDelete.Enabled = true;
            btnDiscon.Enabled = false;
            btnDiscon_Item.Enabled = false;
            btnchangeQty.Enabled = false;
            btnSendApprove.Enabled = true;
            ClearData();
            Enable_Status(true, "New");
            lblStatus.Text = "New";
            Ac = "New";
            row = dgvData.Rows.Count - 1;
            if (row < 0)
                row = 0;
            //getมาไว้ก่อน แต่ยังไมได้ save 
            //txtTempNo.Text = StockControl.dbClss.GetNo(10, 0);

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            btnView.Enabled = false;
            btnEdit.Enabled = true;
            btnNew.Enabled = true;
            btnSave.Enabled = true;

            Enable_Status(false, "View");
            lblStatus.Text = "View";
            Ac = "View";
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            btnView.Enabled = true;
            btnEdit.Enabled = false;
            btnNew.Enabled = true;
            btnSave.Enabled = true;
            btnDelete.Enabled = true;


            Enable_Status(true, "Edit");
            lblStatus.Text = "Edit";
            Ac = "Edit";

            int te = 0;
            foreach (var g in dgvData.Rows)
            {
                if ((dbClss.TSt(g.Cells["dgvStatus"].Value) == "Partial")
                    || (dbClss.TSt(g.Cells["dgvStatus"].Value) == "Full")
                    || (dbClss.TSt(g.Cells["dgvStatus"].Value) == "Discon")
                    )
                {
                    te = 1;
                    break;
                }
            }
            if (te == 1)
            {
                btnDelete.Enabled = false;
            }


        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    int te = 0;
                    foreach (var g in dgvData.Rows)
                    {
                        if ((dbClss.TSt(g.Cells["dgvStatus"].Value) == "Partial")
                            || (dbClss.TSt(g.Cells["dgvStatus"].Value) == "Full")
                            || (dbClss.TSt(g.Cells["dgvStatus"].Value) == "Discon")
                            )
                        {
                            te = 1;
                            break;
                        }
                    }
                    if (te == 1)
                    {
                        MessageBox.Show("ไม่สามารถทำการลบรายการได้ สถานะไม่ถูกต้อง");
                        return;
                    }

                    if (lblStatus.Text != "Completed" && lblStatus.Text != "Process"
                        && lblStatus.Text != "Approved"
                        && lblStatus.Text != "Waiting Approve")
                    {
                        lblStatus.Text = "Delete";
                        Ac = "Del";
                        if (MessageBox.Show("ต้องการลบรายการ ( " + txtPONo.Text + " ) หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            this.Cursor = Cursors.WaitCursor;

                            var g = (from ix in db.mh_PurchaseOrders
                                     where ix.TempPNo.Trim() == txtTempNo.Text.Trim()
                                     && ix.Status != "Cancel" && ix.Status != "Completed" && ix.Status != "Process"
                                     && ix.SeqStatus == 0
                                     select ix).ToList();
                            if (g.Count > 0)  //มีรายการในระบบ
                            {
                                var gg = (from ix in db.mh_PurchaseOrders
                                          where ix.TempPNo.Trim() == txtTempNo.Text.Trim()
                                          //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                                          select ix).First();

                                try
                                {
                                    var s = (from ix in db.mh_PurchaseOrderDetails
                                             where ix.TempPNo.Trim() == txtTempNo.Text.Trim()
                                             && ix.SS == 1
                                             select ix).ToList();
                                    if (s.Count > 0)
                                    {
                                        foreach (var ss in s)
                                        {
                                            Del_PO(ss.id, StockControl.dbClss.TInt(ss.PRItem), ss.PRNo);

                                            ss.SS = 0;
                                            db.SubmitChanges();
                                            //update Stock backorder
                                            db.sp_010_Update_StockItem(Convert.ToString(ss.CodeNo), "BackOrder");
                                        }

                                    }
                                }
                                catch (Exception ex) { MessageBox.Show(ex.Message); }
                                //----------------------//


                                gg.Status = "Cancel";
                                gg.ModifyBy = ClassLib.Classlib.User;
                                gg.ModifyDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));

                                dbClss.AddHistory(this.Name, "ลบ PO", "Delete PONo [" + txtPONo.Text.Trim() + "]", txtPONo.Text);

                                dbClss.Delete_ApproveList(txtPONo.Text.Trim());

                                db.SubmitChanges();
                                btnNew_Click(null, null);
                                Ac = "New";
                                btnSave.Enabled = true;


                            }
                            else // ไม่มีในระบบ
                            {
                                btnNew_Click(null, null);
                                Ac = "New";
                                btnSave.Enabled = true;
                            }
                        }

                        MessageBox.Show("ลบรายการ สำเร็จ!");
                        row = row - 1;
                        if (dgvData.Rows.Count <= 0)
                            row = -1;
                    }
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }

        }
        private void Del_PO(int id, int PRItem, string PRNo)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                //ปรับสถานะ PR เป็น Waiting

                var s = (from ix in db.mh_PurchaseRequestLines
                         where ix.RefPOid == id && ix.id == PRItem
                         select ix).ToList();
                if (s.Count > 0)
                {
                    //var p = (from ix in db.tb_PurchaseRequestLines
                    //     where dbClss.TInt( ix.RefPOid) == id
                    //      //&& ix.TempNo == txtTempNo.Text 
                    //      && dbClss.TInt(ix.id) == PRItem
                    //     select ix).First();
                    foreach (var p in s)
                    {
                        p.Status = "Waiting";

                        //p.RemainQty = p.OrderQty;
                        p.PoNo = "";
                        p.RefPOid = 0;

                        dbClss.AddHistory(this.Name, "ปรับสถานะ Item PR", "ลบ PO จาก POid :" + StockControl.dbClss.TSt(id)
                            + " PONo :" + txtPONo.Text.Trim()
                            + " ปรับโดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", PRNo);

                        db.SubmitChanges();

                        //update status pr
                        db.sp_023_PRHD_Cal_Status(p.TempNo, p.PRNo);
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
                //if (txtCodeNo.Text.Equals(""))
                //    err += " “รหัสพาร์ท:” เป็นค่าว่าง \n";
                //if (txtPRNo.Text.Equals(""))
                //    err += " “เลขที่ใบขอสั่งซื้อ:” เป็นค่าว่าง \n";
                //if (ddlFactory.Text.Equals(""))
                //    err += "- “ประเภทใบสั่งซื้อ:” เป็นค่าว่าง \n";
                if (cboVendorName.Text.Equals(""))
                    err += "- “เลือกผู้ขาย:” เป็นค่าว่าง \n";
                if (txtVendorNo.Text.Equals(""))
                    err += "- “รหัสผู้ขาย:” เป็นค่าว่าง \n";
                if (ddlCurrency.Text.Equals(""))
                    err += "- “สกุลเงิน:” เป็นค่าว่าง \n";
                if (txtContactName.Text.Equals(""))
                    err += "- “ผู้ติดต่อ:” เป็นค่าว่าง \n";
                //if (txtAddress.Text.Equals(""))
                //    err += "- “ที่อยู่:” เป็นค่าว่าง \n";
                if (txtTel.Text.Equals(""))
                    err += "- “เบอร์โทร:” เป็นค่าว่าง \n";
                if (dtDuedate.Text.Equals(""))
                    err += "- “วันที่ต้องการ:” เป็นค่าว่าง \n";
                if (cboVatGroup.Text.Equals(""))
                    err += "- “กลุ่มภาษี:” เป็นค่าว่าง \n";
                if (cboVatType.Text.Equals(""))
                    err += "- “ประเภทภาษี:” เป็นค่าว่าง \n";


                if (dgvData.Rows.Count <= 0)
                    err += "- “รายการ:” เป็นค่าว่าง \n";
                foreach (GridViewRowInfo rowInfo in dgvData.Rows)
                {
                    if (rowInfo.IsVisible)
                    {
                        if (StockControl.dbClss.TSt(rowInfo.Cells["dgvCodeNo"].Value).Equals(""))
                            err += "- “รหัสทูล:” เป็นค่าว่าง \n";
                        if (StockControl.dbClss.TSt(rowInfo.Cells["dgvItemName"].Value).Equals(""))
                            err += "- “ชื่อทูล:” เป็นค่าว่าง \n";
                        if (StockControl.dbClss.TSt(rowInfo.Cells["dgvItemDesc"].Value).Equals(""))
                            err += "- “รายละเอียดทูล:” เป็นค่าว่าง \n";
                        if (StockControl.dbClss.TSt(rowInfo.Cells["dgvGroupCode"].Value).Equals(""))
                            err += "- “กลุ่มสินค้า:” เป็นค่าว่าง \n";
                        if (StockControl.dbClss.TDe(rowInfo.Cells["dgvOrderQty"].Value) <= 0)
                            err += "- “จำนวน:” น้อยกว่า 0 \n";
                        if (StockControl.dbClss.TDe(rowInfo.Cells["dgvUnit"].Value).Equals(""))
                            err += "- “หน่วย:” เป็นค่าว่าง \n";
                        if (StockControl.dbClss.TDe(rowInfo.Cells["dgvPCSUnit"].Value) <= 0)
                            err += "- “จำนวน:หน่วย:” เป็นค่าว่าง \n";

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
                dbClss.AddError("CreatePO", ex.Message, this.Name);
            }

            return re;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Ac.Equals("New") || Ac.Equals("Edit"))
                {
                    if (Check_Save())
                        return;
                    else
                    {

                        if (MessageBox.Show("ต้องการบันทึก ?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            this.Cursor = Cursors.WaitCursor;

                            using (DataClasses1DataContext db = new DataClasses1DataContext())
                            {
                                if (Ac.Equals("New"))
                                {
                                    //ถ้ามีการใส่เลขที่ PR เช็คดูว่ามีการใส่เลขนี้แล้วหรือไม่ ถ้ามีให้ใส่เลขอื่น
                                    if (!txtPONo.Text.Equals(""))
                                    {

                                        var p = (from ix in db.mh_PurchaseOrders
                                                 where ix.PONo.ToUpper().Trim() == txtPONo.Text.Trim()
                                                 && ix.Status != "Cancel"
                                                 //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                                                 select ix).ToList();
                                        if (p.Count > 0)  //มีรายการในระบบ
                                        {
                                            MessageBox.Show("เลขที่ใบสั่งซื้อถูกใช้ไปแล้ว กรุณาใส่เลขใหม่");
                                            return;
                                        }
                                    }
                                    else

                                        txtPONo.Text = StockControl.dbClss.GetNo(11, 2);
                                    txtTempNo.Text = StockControl.dbClss.GetNo(10, 2);
                                }

                                var ggg = (from ix in db.mh_PurchaseOrders
                                           where ix.TempPNo.Trim() == txtTempNo.Text.Trim() //&& ix.Status != "Cancel"
                                           //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                                           select ix).ToList();
                                if (ggg.Count > 1)  //มีรายการในระบบ
                                {
                                    MessageBox.Show("เลขที่อ้างอิงถูกใช้แล้ว กรุณาสร้างเลขใหม่");
                                    return;
                                }
                            }

                            if (!txtTempNo.Text.Equals("")
                                && !txtPONo.Text.Equals(""))
                            {
                                //Calก่อน save
                                //getTotal();
                                LessPoDiscountAmount_KeyPress((char)13);
                                if (cbvatDetail.Checked)
                                    getTotal();

                                SaveHerder();
                                AddPR_d();



                                Ac = "View";
                                btnEdit.Enabled = true;
                                btnView.Enabled = false;
                                btnNew.Enabled = true;
                                Enable_Status(false, "View");


                                //Calculate Status
                                using (DataClasses1DataContext db = new DataClasses1DataContext())
                                {
                                    db.sp_022_POHD_Cal_Status(txtTempNo.Text, txtPONo.Text);

                                    //update cost receive PODetail
                                    db.sp_042_Cal_POCost(txtPONo.Text);
                                }

                                DataLoad();

                                //change status pr
                                Chage_status_Pr();

                                ////insert Stock temp
                                Insert_Stock_temp();

                                MessageBox.Show("บันทึกสำเร็จ!");

                            }
                        }
                    }
                }
                else
                    MessageBox.Show("สถานะต้องเป็น New หรือ Edit เท่านั่น");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        private void Insert_Stock_temp()
        {
            try
            {

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {

                    var g = (from ix in db.mh_PurchaseOrderDetails
                             join h in db.mh_PurchaseOrders on ix.TempPNo equals h.TempPNo
                             where ix.TempPNo.Trim() == txtTempNo.Text.Trim()
                                && ix.SS == 1 && h.Status != "Cancel"

                             select ix).ToList();
                    if (g.Count > 0)
                    {
                        //update BackOrder 
                        foreach (var vv in g)
                        {
                            db.sp_010_Update_StockItem(Convert.ToString(vv.CodeNo), "BackOrder");
                            //dbClss.Insert_StockTemp(vv.CodeNo, Convert.ToDecimal(vv.OrderQty), "PR_Temp", "Inv");
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void Chage_status_Pr()
        {
            ////ปรับสถานะ PR เป็น Waiting

            try
            {

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    foreach (var g in dgvData.Rows)
                    {
                        if (g.IsVisible.Equals(true))
                        {
                            var v = (from ix in db.mh_PurchaseRequestLines
                                     where //ix.RefPOid == StockControl.dbClss.TInt(g.Cells["dgvid"].Value)
                                           // ix.TempNo == txtTempNo.Text 
                                        ix.id == StockControl.dbClss.TInt(g.Cells["dgvPRItem"].Value)
                                     select ix).ToList();
                            if (v.Count > 0)
                            {
                                var p = (from ix in db.mh_PurchaseRequestLines
                                         where //ix.RefPOid == StockControl.dbClss.TInt(g.Cells["dgvid"].Value)
                                               // ix.TempNo == txtTempNo.Text 
                                            ix.id == StockControl.dbClss.TInt(g.Cells["dgvPRItem"].Value)
                                         select ix).First();
                                p.Status = "Completed";
                                //p.RemainQty = 0;
                                p.PoNo = txtPONo.Text.Trim();
                                p.RefPOid = StockControl.dbClss.TInt(g.Cells["dgvid"].Value);

                                dbClss.AddHistory(this.Name, "ปรับสถานะ Item PR", "สร้าง PO จาก PRid :" + StockControl.dbClss.TSt(g.Cells["dgvPRItem"].Value)
                                    + " PONo :" + txtPONo.Text.Trim()
                                    + " ปรับโดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", StockControl.dbClss.TSt(g.Cells["dgvPRNo"].Value));

                                db.SubmitChanges();

                                db.sp_023_PRHD_Cal_Status(p.TempNo, p.PRNo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }


        }
        bool LastDiscount = false;
        bool lastDiscountAmount = false;
        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                dgvData.EndEdit();
                if (e.RowIndex >= -1)
                {
                    decimal UnitCost = 0;
                    decimal Qty = 0;
                    decimal PA = 0;
                    decimal ExtendedCost = 0;
                    int cal = 0;
                    int A = 0;

                    if (dgvData.Columns["dgvCodeNo"].Index == e.ColumnIndex)
                    {
                        string CodeNo = dbClss.TSt(e.Row.Cells["dgvCodeNo"].Value);

                        int c = 0;
                        foreach (GridViewRowInfo rowInfo in dgvData.Rows)//datagridview save ที่ละแถว
                        {
                            if (rowInfo.IsVisible.Equals(true))
                            {
                                if (rowInfo.Index != e.RowIndex)
                                {
                                    if (StockControl.dbClss.TSt(rowInfo.Cells["dgvCodeNo"].Value).Equals(CodeNo))
                                    {
                                        c += 1;
                                        break;
                                    }
                                }
                            }
                        }

                        if (c > 0)
                        {
                            MessageBox.Show("รายการซ้ำ");
                            e.Row.Cells["dgvCodeNo"].Value = "";
                            CodeNo = "";
                            return;
                        }
                        
                        if (CodeNo != "" && c <= 0)
                        {
                            using (DataClasses1DataContext db = new DataClasses1DataContext())
                            {
                                var g = (from ix in db.mh_Items select ix).Where(a => a.InternalNo.ToUpper().Trim().Equals(CodeNo.ToUpper().Trim())).ToList();
                                if (g.Count > 0)
                                {

                                    string ItemNo = StockControl.dbClss.TSt(g.FirstOrDefault().InternalName);
                                    string ItemDescription = StockControl.dbClss.TSt(g.FirstOrDefault().InternalName);
                                    string GroupCode = StockControl.dbClss.TSt(g.FirstOrDefault().GroupType);
                                    int OrderQty = 0;
                                    decimal PCSUnit = dbClss.Con_UOM(CodeNo, StockControl.dbClss.TSt(g.FirstOrDefault().PurchaseUOM));
                                    string UnitBuy = StockControl.dbClss.TSt(g.FirstOrDefault().PurchaseUOM);
                                    decimal StandardCost = dbClss.TDe(g.FirstOrDefault().StandardCost);
                                    string Status = "Adding";
                                    string PRNO = "";
                                    int Refid = 0;
                                    int id = 0;
                                    DateTime? DeliveryDate = null;

                                    e.Row.Cells["dgvCodeNo"].Value = CodeNo;
                                    e.Row.Cells["dgvItemName"].Value = ItemNo;
                                    e.Row.Cells["dgvItemDesc"].Value = ItemDescription;
                                    e.Row.Cells["dgvGroupCode"].Value = GroupCode;
                                    e.Row.Cells["dgvOrderQty"].Value = OrderQty;
                                    e.Row.Cells["dgvPCSUnit"].Value = PCSUnit;
                                    e.Row.Cells["dgvUnit"].Value = UnitBuy;
                                    e.Row.Cells["dgvCost"].Value = StandardCost;
                                    e.Row.Cells["dgvAmount"].Value = OrderQty * StandardCost;
                                    e.Row.Cells["dgvPRNo"].Value = PRNO;
                                    e.Row.Cells["dgvPRItem"].Value = Refid;
                                    e.Row.Cells["dgvStatus"].Value = Status;
                                    e.Row.Cells["dgvid"].Value = id;
                                    e.Row.Cells["dgvBackOrder"].Value = OrderQty;
                                    //if (dbClss.TSt(DeliveryDate) != "")
                                    e.Row.Cells["dgvDeliveryDate"].Value = DeliveryDate;

                                    e.Row.Cells["dgvCodeNo"].ReadOnly = true;
                                    e.Row.Cells["dgvItemName"].ReadOnly = true;
                                    e.Row.Cells["dgvItemDesc"].ReadOnly = true;
                                    //e.Row.Cells["dgvPCSUnit"].ReadOnly = true;
                                    e.Row.Cells["dgvGroupCode"].ReadOnly = true;
                                    //e.Row.Cells["dgvUnit"].ReadOnly = true;

                                }
                            }
                        }
                    }
                    else if (dgvData.Columns["dgvOrderQty"].Index == e.ColumnIndex
                        || dgvData.Columns["dgvCost"].Index == e.ColumnIndex
                        )
                    {
                        decimal OrderQty = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["dgvOrderQty"].Value), out OrderQty);
                        decimal StandardCost = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["dgvCost"].Value), out StandardCost);
                        e.Row.Cells["dgvAmount"].Value = OrderQty * StandardCost;

                        if (StockControl.dbClss.TSt(e.Row.Cells["dgvStatus"].Value) == "Adding")
                            e.Row.Cells["dgvBackOrder"].Value = e.Row.Cells["dgvOrderQty"].Value;


                        decimal.TryParse(Convert.ToString(dgvData.Rows[e.RowIndex].Cells["dgvOrderQty"].Value), out Qty);
                        decimal.TryParse(Convert.ToString(dgvData.Rows[e.RowIndex].Cells["dgvCost"].Value), out UnitCost);
                        decimal.TryParse(Convert.ToString(dgvData.Rows[e.RowIndex].Cells["dgvDiscountAmount"].Value), out PA);
                        decimal.TryParse(Convert.ToString(dgvData.Rows[e.RowIndex].Cells["dgvAmount"].Value), out ExtendedCost);
                        cal = 1;

                        //getTotal();
                    }
                    else if (dgvData.Columns["dgvUnit"].Index == e.ColumnIndex)
                    {
                        string dgvUOM = dbClss.TSt(e.Row.Cells["dgvUnit"].Value);
                        string CodeNo = dbClss.TSt(e.Row.Cells["dgvCodeNo"].Value);
                        //decimal PCSUOM = dbClss.Con_UOM(CodeNo, dgvUOM);
                        e.Row.Cells["dgvPCSUnit"].Value = dbClss.Con_UOM(CodeNo, dgvUOM);
                        e.Row.Cells["dgvPCSUnit"].ReadOnly = false;
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
                        //        else
                        //        {
                        //            e.Row.Cells["dgvPCSUnit"].ReadOnly = false;
                        //            e.Row.Cells["dgvPCSUnit"].Value = 0;
                        //        }
                        //    }
                        //    else
                        //    {
                        //e.Row.Cells["dgvPCSUnit"].ReadOnly = false;
                        //        e.Row.Cells["dgvPCSUnit"].Value = 0;
                        //    }
                        //}
                    }
                    else if (dgvData.Columns["dgvDiscount"].Index == e.ColumnIndex)
                    {
                        cal = 1;
                        A = 1;
                        LastDiscount = false;
                        //discount %
                    }
                    else if (dgvData.Columns["dgvDiscountAmount"].Index == e.ColumnIndex)
                    {
                        //discount amount
                        LastDiscount = false;
                        cal = 1;
                        A = 2;

                    }
                    else if (dgvData.Columns["dgvChangeQty"].Index == e.ColumnIndex)
                    {
                        string Status = StockControl.dbClss.TSt(e.Row.Cells["dgvStatus"].Value);
                        if (Status == "Partial")
                        {
                            decimal OrderQty2 = 0;
                            decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["dgvOrderQty2"].Value), out OrderQty2);
                            decimal BackOrder2 = 0;
                            decimal.TryParse(Convert.ToString(dgvData.Rows[e.RowIndex].Cells["dgvBackOrder2"].Value), out BackOrder2);
                            decimal ChangeQty = 0;
                            decimal.TryParse(Convert.ToString(dgvData.Rows[e.RowIndex].Cells["dgvChangeQty"].Value), out ChangeQty);
                            if (ChangeQty <= BackOrder2)
                            {
                                e.Row.Cells["dgvBackOrder"].Value = BackOrder2 - ChangeQty;
                                e.Row.Cells["dgvOrderQty"].Value = OrderQty2 - ChangeQty;
                            }
                            else
                            {
                                MessageBox.Show("ไม่สามารถทำการลดจำนวนสั่งซื้อเกินจำนวนคงเหลือการสั่งซื้อได้");
                                e.Row.Cells["dgvBackOrder"].Value = BackOrder2;
                                e.Row.Cells["dgvChangeQty"].Value = 0;
                                e.Row.Cells["dgvOrderQty"].Value = e.Row.Cells["dgvOrderQty2"].Value;
                            }
                            cal = 1;
                        }                        
                    }

                        if (A > 0)
                    {
                        decimal PC = 0;
                        decimal AM = 0;
                        cal = 1;

                        decimal.TryParse(Convert.ToString(dgvData.Rows[e.RowIndex].Cells["dgvOrderQty"].Value), out Qty);
                        decimal.TryParse(Convert.ToString(dgvData.Rows[e.RowIndex].Cells["dgvCost"].Value), out UnitCost);
                        decimal.TryParse(Convert.ToString(dgvData.Rows[e.RowIndex].Cells["dgvDiscount"].Value), out PC);
                        decimal.TryParse(Convert.ToString(dgvData.Rows[e.RowIndex].Cells["dgvDiscountAmount"].Value), out PA);


                        //calculate Discount

                        if (A == 1 && PC > 0) //%
                        {
                            AM = (Qty * UnitCost);
                            dgvData.Rows[e.RowIndex].Cells["dgvDF"].Value = 1;
                            dgvData.Rows[e.RowIndex].Cells["dgvDiscountAmount"].Value = (AM * PC / 100);
                            dgvData.Rows[e.RowIndex].Cells["dgvDiscountExt"].Value = (AM * PC / 100);
                        }
                        else if (A == 2 && PA > 0) //AM
                        {
                            AM = (Qty * UnitCost);
                            dgvData.Rows[e.RowIndex].Cells["dgvDF"].Value = 2;
                            dgvData.Rows[e.RowIndex].Cells["dgvDiscount"].Value = (PA / AM) * 100;
                            dgvData.Rows[e.RowIndex].Cells["dgvDiscountExt"].Value = PA;
                        }



                        if (PA > (UnitCost * Qty))
                        {
                            MessageBox.Show("ส่วนลดเกิน ยอด Amount!!!");
                            dgvData.Rows[e.RowIndex].Cells["dgvDiscount"].Value = 0;
                            dgvData.Rows[e.RowIndex].Cells["dgvDiscountAmount"].Value = 0;
                            dgvData.Rows[e.RowIndex].Cells["dgvDiscountExt"].Value = 0;
                        }

                    }

                    if (cal > 0)
                    {

                        //dgvDataOrder.Rows[e.RowIndex].Cells["dgvExtendedCost"].Value = UnitCost;
                        //dgvDataOrder.Rows[e.RowIndex].Cells["dgvAmount"].Value = (Qty*UnitCost);
                        //dgvDataOrder.Rows[e.RowIndex].Cells["dgvNetofTAX"].Value = (Qty * UnitCost)-PA;
                        //dgvDataOrder.Rows[e.RowIndex].Cells["dgvVatAmount"].Value = ((Qty * UnitCost)-PA) * vat / 100;
                        //dgvDataOrder.Rows[e.RowIndex].Cells["dgvSubTotal"].Value = ((Qty * UnitCost)-PA) * vat / 100 + ((Qty * UnitCost)-PA);
                        //CallListDiscount();

                        if (LastDiscount)
                        {
                            if (lastDiscountAmount)
                            {
                                CallDiscontLast(true);
                                CallSumDiscountLast(true);
                            }
                            else
                            {
                                CallDiscontLast(false);
                                CallSumDiscountLast(false);
                            }
                        }
                        else
                        {
                            CallListDiscount();
                        }

                        CallTotal();

                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void CallListDiscount()
        {
            decimal UnitCost = 0;
            decimal ExtendedCost = 0;
            decimal Qty = 0;
            decimal PA = 0;
            decimal PR = 0;
            decimal SumP = 0;
            decimal SumA = 0;
            int DF = 0;
            try
            {
                dgvData.EndEdit();
                foreach (var r2 in dgvData.Rows)
                {
                    UnitCost = 0;
                    Qty = 0;
                    PA = 0;
                    PR = 0;

                    decimal.TryParse(Convert.ToString(r2.Cells["dgvOrderQty"].Value), out Qty);
                    decimal.TryParse(Convert.ToString(r2.Cells["dgvCost"].Value), out UnitCost);
                    decimal.TryParse(Convert.ToString(r2.Cells["dgvDiscountAmount"].Value), out PA);
                    decimal.TryParse(Convert.ToString(r2.Cells["dgvDiscount"].Value), out PR);
                    decimal.TryParse(Convert.ToString(r2.Cells["dgvAmount"].Value), out ExtendedCost);

                    //MessageBox.Show(" % "+PR.ToString());
                    if (PR > 0)
                    {
                        if (Convert.ToInt32(r2.Cells["dgvDF"].Value).Equals(1))
                        {
                            //MessageBox.Show(" x% " + PR.ToString());
                            PA = (ExtendedCost * PR / 100);
                            r2.Cells["dgvDiscountAmount"].Value = (ExtendedCost * PR / 100);
                            r2.Cells["dgvDiscountExt"].Value = (ExtendedCost * PR / 100);
                        }
                        else
                        {
                            //MessageBox.Show(" z% " + PR.ToString());
                            //PA = ((Qty * UnitCost) * PR / 100);
                            r2.Cells["dgvDiscount"].Value = (PA / ExtendedCost) * 100;
                            r2.Cells["dgvDiscountAmount"].Value = PA;
                            r2.Cells["dgvDiscountExt"].Value = PA;

                        }
                    }
                    else
                    {
                        PA = 0;
                    }

                    r2.Cells["dgvNetofTAX"].Value = ExtendedCost - PA;

                    SumP += ExtendedCost;
                    SumA += PA;
                }

                txtLessPoDiscountAmountPersen.Text = ((SumA / SumP) * 100).ToString("##0.00");
                txtLessPoDiscountAmount.Text = (SumA).ToString("###,###,##0.00");
            }
            catch { }
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
            }
        }



        int row = -1;
        private void radGridView1_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            row = e.RowIndex;
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

        private void radButton1_Click(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void chkActive_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {

        }

        private void radButton1_Click_1(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void radGridView1_CellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            //if (e.CellElement.ColumnInfo.Name == "ModelName")
            //{
            //    if (e.CellElement.RowInfo.Cells["ModelName"].Value != null)
            //    {
            //        if (!e.CellElement.RowInfo.Cells["ModelName"].Value.Equals(""))
            //        {
            //            e.CellElement.DrawFill = true;
            //            // e.CellElement.ForeColor = Color.Blue;
            //            e.CellElement.NumberOfColors = 1;
            //            e.CellElement.BackColor = Color.WhiteSmoke;
            //        }

            //    }
            //}
        }

        private void txtModelName_TextChanged(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void radPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cboModelName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void radLabel5_Click(object sender, EventArgs e)
        {

        }

        private void เพมพารทToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!txtVendorNo.Text.Equals(""))
                {
                    List<GridViewRowInfo> dgvRow_List = new List<GridViewRowInfo>();
                    //dgvRow_List.Clear();
                    CreatePR_List_2 MS = new CreatePR_List_2(dgvRow_List, txtVendorNo.Text, "CreatePO");
                    MS.ShowDialog();
                    if (dgvRow_List.Count > 0)
                    {
                        string CodeNo = "";
                        this.Cursor = Cursors.WaitCursor;

                        string ItemName = "";
                        string ItemDescription = "";
                        string GroupCode = "Other";
                        decimal OrderQty = 0;
                        decimal PCSUnit = 1;
                        string Unit = "PCS";
                        decimal Cost = 0;
                        string Status = "Adding";
                        string PRNO = "";
                        int Refid = 0;
                        int id = 0;
                        int Row = dgvData.Rows.Count() + 1;
                        DateTime? DeliveryDate = null;
                        int idCSTMPODt = 0;
                        foreach (GridViewRowInfo ee in dgvRow_List)
                        {
                            CodeNo = Convert.ToString(ee.Cells["CodeNo"].Value).Trim();
                            ItemName = Convert.ToString(ee.Cells["ItemName"].Value).Trim();
                            ItemDescription = Convert.ToString(ee.Cells["ItemDesc"].Value).Trim();
                            GroupCode = Convert.ToString(ee.Cells["GroupCode"].Value).Trim();
                            OrderQty = StockControl.dbClss.TDe(ee.Cells["OrderQty"].Value);
                            PCSUnit = StockControl.dbClss.TDe(ee.Cells["PCSUnit"].Value);
                            Unit = StockControl.dbClss.TSt(ee.Cells["UnitCode"].Value);
                            Cost = StockControl.dbClss.TDe(ee.Cells["Cost"].Value);
                            PRNO = StockControl.dbClss.TSt(ee.Cells["TempNo"].Value);
                            Refid = StockControl.dbClss.TInt(ee.Cells["id"].Value);
                            //DeliveryDate = Convert.ToDateTime(ee.Cells["dgvDeliveryDate"].Value);
                            idCSTMPODt = dbClss.TInt(ee.Cells["idCSTMPODt"].Value);

                            Add_Item(Row, CodeNo, ItemName, ItemDescription, GroupCode, OrderQty
                                , PCSUnit, Unit, Cost, PRNO, DeliveryDate, Status, Refid, id, true, idCSTMPODt);

                        }

                    }
                    //getTotal();
                    CallTotal();
                }
                else
                    MessageBox.Show("เลือกประเภทใบสั่งซื้อ หรือ ผู้ขายก่อน !!!");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        private bool check_Duppicate(string CodeNo)
        {
            bool re = false;
            foreach (var rd1 in dgvData.Rows)
            {
                if (rd1.IsVisible.Equals(true))
                {
                    if (StockControl.dbClss.TSt(rd1.Cells["dgvCodeNo"].Value).Equals(CodeNo))
                        re = true;
                }
            }

            return re;

        }
        private void Add_Part(string CodeNo, int OrderQty)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                int Row = 0; Row = dgvData.Rows.Count() + 1;
                var g = (from ix in db.mh_Items select ix).Where(a => a.InternalNo.Contains(CodeNo)).ToList();
                if (g.Count > 0)
                {
                    //dgvData.Rows.Add(Row.ToString(), CodeNo,
                    //    StockControl.dbClss.TSt(g.FirstOrDefault().ItemNo)
                    //    , StockControl.dbClss.TSt(g.FirstOrDefault().ItemDescription)
                    //    , StockControl.dbClss.TSt(g.FirstOrDefault().GroupCode)
                    //    , OrderQty
                    //    , StockControl.dbClss.TDe(g.FirstOrDefault().PCSUnit)
                    //    , StockControl.dbClss.TSt(g.FirstOrDefault().UnitBuy)
                    //    , StockControl.dbClss.TDe(g.FirstOrDefault().StandardCost)
                    //    , 1 * StockControl.dbClss.TDe(g.FirstOrDefault().StandardCost)
                    //    , ""
                    //    , "" //Lotno
                    //    , "" //SerialNo
                    //    , "" //MCName
                    //    , "" //LineName
                    //    , DateTime.Now
                    //    ,0.0 // RemainQty
                    //    ,0
                    //    );


                }
            }
        }
        private void ลบพารทToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                if (dgvData.Rows.Count < 0)
                    return;

                string Status = StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["dgvStatus"].Value);

                if (Ac.Equals("New") || Ac.Equals("Edit") || (lblStatus.Text=="Process") && Status == "Waiting")
                {
                    this.Cursor = Cursors.WaitCursor;

                    if (Status == "Waiting" || Status == "Adding")
                    {

                        int id = 0;
                        int.TryParse(StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["dgvid"].Value), out id);
                        if (id <= 0)
                            dgvData.Rows.Remove(dgvData.CurrentRow);

                        else
                        {
                            string CodeNo = "";
                            CodeNo = StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["dgvCodeNo"].Value);
                            if (MessageBox.Show("ต้องการลบรายการ ( " + CodeNo + " ) ออกจากรายการ หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                dgvData.CurrentRow.IsVisible = false;
                            }
                        }
                        CallTotal();
                        //getTotal();
                        SetRowNo1(dgvData);
                    }
                    else
                        MessageBox.Show("ไม่สามารถทำการลบรายการได้ สถานะไม่ถูกต้อง");
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
        private void cboVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    if (!cboVendorName.Text.Equals(""))
                    {
                        var I = (from ix in db.mh_Vendors select ix)
                            .Where(a => a.Active == true && a.Name.Equals(cboVendorName.Text)).ToList();
                        if (I.Count > 0)
                        {
                            //StockControl.dbClss.TBo(a.Active).Equals(true)
                            //ddlCurrency.Text = I.FirstOrDefault().CRRNCY;
                            txtAddress.Text = I.FirstOrDefault().Address;
                            txtVendorNo.Text = I.FirstOrDefault().No;
                            //var g = (from ix in db.tb_VendorContacts select ix).Where(a => a.VendorNo.Equals(txtVendorNo.Text)).OrderByDescending(b => b.DefaultNo).ToList();
                            //if (g.Count > 0)
                            //{
                            txtContactName.Text = I.FirstOrDefault().ContactName;
                            txtTel.Text = I.FirstOrDefault().PhoneNo;
                            txtFax.Text = I.FirstOrDefault().FaxNo;
                            txtEmail.Text = I.FirstOrDefault().Email;

                            var v = (from ix in db.mh_VatGroups select ix)
                            .Where(a => a.Active == true && a.id.Equals(dbClss.TInt(I.FirstOrDefault().VatGroup))).ToList();
                            if (v.Count > 0)
                            {
                                cboVatGroup.Text = dbClss.TSt(v.FirstOrDefault().Value);
                            }

                            //}
                            //else
                            //{
                            //    txtContactName.Text = "";
                            //    txtTel.Text = "";
                            //    txtFax.Text = "";
                            //    txtEmail.Text = "";
                            //}
                            int id_cr = dbClss.TInt(I.FirstOrDefault().DefaultCurrency);
                            if (id_cr > 0)
                            {
                                var cr = (from ix in db.mh_CRRNCies select ix)
                                    .Where(a => a.id.Equals(id_cr)).ToList();
                                if (cr.Count > 0)
                                {
                                    ddlCurrency.Text = dbClss.TSt(cr.FirstOrDefault().Crrncy);
                                    ddlCurrency_SelectedIndexChanged(null, null);
                                }
                                else
                                {
                                    ddlCurrency.Text = "";
                                    txtRate.Text = "0";
                                }
                            }
                            else
                            {
                                ddlCurrency.Text = "";
                                txtRate.Text = "0";
                            }

                            txtVendorNo.ReadOnly = true;
                            ddlCurrency.Enabled = false;
                            ddlCurrency.BackColor = Color.WhiteSmoke;
                            txtVendorNo.BackColor = Color.WhiteSmoke;
                        }
                        else
                        {
                            ddlCurrency.Text = "";
                            txtRate.Text = "";
                            txtAddress.Text = "";
                            txtVendorNo.Text = "";
                            txtContactName.Text = "";
                            txtTel.Text = "";
                            txtFax.Text = "";
                            cboVatType.Text = "";
                            cboVatGroup.Text = "";

                            txtVendorNo.ReadOnly = false;
                            ddlCurrency.Enabled = true;
                            ddlCurrency.BackColor = Color.White;
                            txtVendorNo.BackColor = Color.White;
                        }
                    }

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void MasterTemplate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                ลบพารทToolStripMenuItem_Click(null, null);
        }

        private void btnListItem_Click(object sender, EventArgs e)
        {
            ////DataLoad();
            try
            {
                btnEdit.Enabled = true;
                btnView.Enabled = false;
                btnNew.Enabled = true;
                ClearData();
                Ac = "View";
                Enable_Status(false, "View");

                this.Cursor = Cursors.WaitCursor;
                CreatePO_List sc = new CreatePO_List(txtTempNo);
                this.Cursor = Cursors.Default;
                sc.ShowDialog();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();
                //LoadData
                DataLoad();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError("CreatePart", ex.Message + " : radButtonElement1_Click", this.Name); }
            finally { this.Cursor = Cursors.Default; }



        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            btnEdit.Enabled = true;
            btnView.Enabled = false;
            btnNew.Enabled = true;
            btnDiscon.Enabled = false;
            btnDiscon_Item.Enabled = false;
            btnchangeQty.Enabled = false;
            string TempNo = txtTempNo.Text;
            ClearData();
            Enable_Status(false, "View");
            txtTempNo.Text = TempNo;
            DataLoad();
            Ac = "View";
        }

        private void txtPRNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && !txtPONo.Text.Equals(""))
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var g = (from ix in db.mh_PurchaseOrders select ix)
                        .Where(a => a.PONo == txtPONo.Text.Trim()
                        && (a.Status != "Cancel")
                        ).ToList();
                    if (g.Count() > 0)
                    {
                        txtTempNo.Text = StockControl.dbClss.TSt(g.FirstOrDefault().TempPNo);
                        btnView_Click(null, null);
                        DataLoad();
                    }
                }

            }
        }
        private void CreatePR_from_WaitingPR()
        {
            try
            {
                if (RetDT.Count > 0)
                {
                    string CodeNo = "";
                    this.Cursor = Cursors.WaitCursor;
                    string VendorNo = "";
                    foreach (GridViewRowInfo ee in RetDT)
                    {
                        VendorNo = Convert.ToString(ee.Cells["VendorNo"].Value).Trim();
                        if (!VendorNo.Equals(""))
                        {
                            using (DataClasses1DataContext db = new DataClasses1DataContext())
                            {
                                var I = (from ix in db.tb_Vendors select ix).Where(a => a.Active == true
                                && a.VendorNo.Equals(VendorNo)).ToList();
                                if (I.Count > 0)
                                {
                                    //StockControl.dbClss.TBo(a.Active).Equals(true)
                                    ddlCurrency.Text = I.FirstOrDefault().CRRNCY;
                                    txtAddress.Text = I.FirstOrDefault().Address;
                                    txtVendorNo.Text = I.FirstOrDefault().VendorNo;
                                    cboVendorName.Text = I.FirstOrDefault().VendorName;
                                    var g = (from ix in db.tb_VendorContacts select ix).Where(a => a.VendorNo.Equals(txtVendorNo.Text)).OrderByDescending(b => b.DefaultNo).ToList();
                                    if (g.Count > 0)
                                    {
                                        txtContactName.Text = g.FirstOrDefault().ContactName;
                                        txtTel.Text = g.FirstOrDefault().Tel;
                                        txtFax.Text = g.FirstOrDefault().Fax;
                                        txtEmail.Text = g.FirstOrDefault().Email;

                                    }
                                }
                            }

                        }

                        CodeNo = Convert.ToString(ee.Cells["CodeNo"].Value).Trim();
                        if (!CodeNo.Equals(""))
                        {
                            Add_Part(CodeNo, StockControl.dbClss.TInt(ee.Cells["Order"].Value));

                        }

                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                PrintPR a = new PrintPR(txtPONo.Text, txtPONo.Text, "PO");
                a.ShowDialog();

                //using (DataClasses1DataContext db = new DataClasses1DataContext())
                //{
                //    var g = (from ix in db.sp_R002_ReportPR(txtPRNo.Text,DateTime.Now) select ix).ToList();
                //    if (g.Count() > 0)
                //    {

                //        Report.Reportx1.Value = new string[2];
                //        Report.Reportx1.Value[0] = txtPRNo.Text;
                //        Report.Reportx1.WReport = "ReportPR";
                //        Report.Reportx1 op = new Report.Reportx1("ReportPR.rpt");
                //        op.Show();

                //    }
                //    else
                //        MessageBox.Show("not found.");
                //}

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void cboVendorName_Leave(object sender, EventArgs e)
        {
            cboVendor_SelectedIndexChanged(null, null);
        }

        private void radButtonElement3_Click(object sender, EventArgs e)
        {
            string RefPO = "";
            string TempNo = txtTempNo.Text;
            if (!txtTempNo.Text.Equals(""))
            {
                string GetMarkup = Interaction.InputBox("ใส่เลขที่ P/O ใหม่!", "P/O New : ", "", 400, 250);
                if (!GetMarkup.Trim().Equals(""))
                {
                    RefPO = GetMarkup;
                    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    {
                        db.sp_UpdatePO(TempNo, RefPO);
                    }
                    MessageBox.Show("Update Completed.");
                    btnRefresh_Click(sender, e);
                }
            }
        }

        private void btnAdd_Row_Click(object sender, EventArgs e)
        {
            try
            {
                if (!txtVendorNo.Text.Equals(""))
                {
                    int Row = 0; Row = dgvData.Rows.Count() + 1;
                    string CodeNo = "";
                    string ItemName = "";
                    string ItemDescription = "";
                    string GroupCode = "";
                    decimal OrderQty = 0;
                    decimal PCSUnit = 1;
                    string Unit = "PCS";
                    decimal Cost = 0;
                    string Status = "Adding";
                    string PRNO = "";
                    int Refid = 0;
                    int id = 0;
                    DateTime? dgvDeliveryDate = null;
                    Add_Item(Row, CodeNo, ItemName, ItemDescription, GroupCode, OrderQty, PCSUnit
                        , Unit, Cost, PRNO, dgvDeliveryDate, Status, Refid, id, false, 0);

                }
                else
                    MessageBox.Show("เลือกผู้ขายก่อน !!!");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        private void Add_Item(int Row, string CodeNo, string ItemNo, string ItemDescription, string GroupCode, decimal OrderQty, decimal PCSUnit
           , string UnitBuy, decimal StandardCost, string PRNo, DateTime? DeliveryDate
            , string Status, int Refid, int id, bool Sys, int idCSTMPODt)
        {
            try
            {
                int rowindex = -1;
                GridViewRowInfo ee;
                if (rowindex == -1)
                {
                    ee = dgvData.Rows.AddNew();
                }
                else
                    ee = dgvData.Rows[rowindex];


                ee.Cells["dgvNo"].Value = Row.ToString();
                ee.Cells["dgvCodeNo"].Value = CodeNo;
                ee.Cells["dgvItemName"].Value = ItemNo;
                ee.Cells["dgvItemDesc"].Value = ItemDescription;
                ee.Cells["dgvGroupCode"].Value = GroupCode;
                ee.Cells["dgvOrderQty"].Value = OrderQty;
                ee.Cells["dgvPCSUnit"].Value = PCSUnit;
                ee.Cells["dgvUnit"].Value = UnitBuy;
                ee.Cells["dgvCost"].Value = StandardCost;
                ee.Cells["dgvAmount"].Value = OrderQty * StandardCost;
                ee.Cells["dgvPRNo"].Value = PRNo;
                ee.Cells["dgvPRItem"].Value = Refid;
                ee.Cells["dgvStatus"].Value = Status;
                ee.Cells["dgvid"].Value = 0;
                ee.Cells["dgvBackOrder"].Value = OrderQty;
                ee.Cells["idCSTMPODt"].Value = idCSTMPODt;

                if (dbClss.TSt(DeliveryDate) != "")
                    ee.Cells["dgvDeliveryDate"].Value = DeliveryDate;

                //if (!statuss.Equals("Completed") || !statuss.Equals("Process")) //|| (!dbclass.TBo(ApproveFlag) && dbclass.TSt(status) != "Reject"))
                //    dgvData.ReadOnly = false;
                //if (statuss == "Del")
                //    ee.IsVisible = false;

                if (Sys)
                {
                    ee.Cells["dgvCodeNo"].ReadOnly = true;
                    //ee.Cells["dgvUnit"].ReadOnly = true;
                    ee.Cells["dgvItemDesc"].ReadOnly = true;
                    ee.Cells["dgvGroupCode"].ReadOnly = true;
                    ee.Cells["dgvItemName"].ReadOnly = true;
                    //ee.Cells["dgvPCSUnit"].ReadOnly = true;
                }

                if (Refid > 0)
                {
                    ee.Cells["dgvCodeNo"].ReadOnly = true;
                    ee.Cells["dgvItemName"].ReadOnly = true;
                    ee.Cells["dgvItemDesc"].ReadOnly = true;
                    ee.Cells["dgvPCSUnit"].ReadOnly = true;
                    ee.Cells["dgvOrderQty"].ReadOnly = true;
                    ee.Cells["dgvGroupCode"].ReadOnly = true;
                    ee.Cells["dgvUnit"].ReadOnly = true;
                }
                ee.Cells["dgvChangeQty"].ReadOnly = true;

                //dbclass.SetRowNo1(dgvData);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError("CreatePO", ex.Message + " : Add_Item", this.Name); }

        }
        private void ddlCurrency_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    if (ddlCurrency.Text != "")
                    {
                        var g = (from ix in db.mh_CRRNCies select ix)
                            .Where(a => a.Crrncy == ddlCurrency.Text.Trim()

                            ).ToList();
                        if (g.Count() > 0)
                        {
                            txtRate.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Rate);
                        }
                        else
                            txtRate.Text = "";
                    }
                    else
                        txtRate.Text = "";
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnAdd_Part_Click(object sender, EventArgs e)
        {
            try
            {
                if (!txtVendorNo.Text.Equals(""))
                {
                    List<GridViewRowInfo> dgvRow_List = new List<GridViewRowInfo>();
                    //dgvRow_List.Clear();
                    ListPart_CreatePR MS = new ListPart_CreatePR(dgvRow_List, txtVendorNo.Text);
                    MS.ShowDialog();
                    if (dgvRow_List.Count > 0)
                    {
                        string CodeNo = "";
                        this.Cursor = Cursors.WaitCursor;
                        decimal OrderQty = 1;
                        foreach (GridViewRowInfo ee in dgvRow_List)
                        {
                            CodeNo = Convert.ToString(ee.Cells["CodeNo"].Value).Trim();
                            if (!CodeNo.Equals("") && !check_Duppicate(CodeNo))
                            {
                                Add_Part(CodeNo, OrderQty);
                            }
                            else
                            {
                                MessageBox.Show("รหัสพาร์ท ซ้ำ");
                            }
                        }
                        //getTotal();
                        CallTotal();
                    }
                }
                else
                    MessageBox.Show("เลือกผู้ขายก่อน !!!");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        private void Add_Part(string CodeNo, decimal OrderQty)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                int Row = 0; Row = dgvData.Rows.Count() + 1;
                var g = (from ix in db.mh_Items select ix).Where(a => a.InternalNo.Contains(CodeNo)).ToList();
                if (g.Count > 0)
                {


                    //string CodeNo = "";
                    string ItemNo = StockControl.dbClss.TSt(g.FirstOrDefault().InternalName);
                    string ItemDescription = StockControl.dbClss.TSt(g.FirstOrDefault().InternalDescription);
                    string GroupCode = StockControl.dbClss.TSt(g.FirstOrDefault().GroupType);
                    //int OrderQty = 0;
                    decimal PCSUnit = dbClss.Con_UOM(CodeNo, StockControl.dbClss.TSt(g.FirstOrDefault().PurchaseUOM));
                    string UnitBuy = StockControl.dbClss.TSt(g.FirstOrDefault().PurchaseUOM);
                    decimal StandardCost = StockControl.dbClss.TDe(g.FirstOrDefault().StandardCost);

                    string Status = "Adding";
                    string PRNO = "";
                    int Refid = 0;
                    int id = 0;
                    DateTime? dgvDeliveryDate = null;

                    Add_Item(Row, CodeNo, ItemNo, ItemDescription, GroupCode, OrderQty
                        , PCSUnit, UnitBuy, StandardCost, PRNO, dgvDeliveryDate, Status, Refid, id, true, 0);

                }
            }
        }

        private void cbvat_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (cbvat.Checked.Equals(false))
            {
                txtVattax.Text = "0";
            }
            else
            {
                cboVatType_SelectedIndexChanged(null, null);
                // txtVattax.Text = "7";
            }
            LessPoDiscountAmount_KeyPress((char)13);
            //getTotal();
        }
        private void cbvatDetail_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            LessPoDiscountAmount_KeyPress((char)13);
            getTotal();
        }
        private void getTotal()
        {
            try
            {
                dgvData.EndEdit();
                if (dgvData.Rows.Count > 0)
                {
                    double Total = 0;
                    double Discount = 0;
                    double afDiscount = 0;
                    double vat = 0;
                    //  double Grantotal = 0;
                    double VatDetail = 0;
                    double TotalSum = 0;
                    double.TryParse(txtLessPoDiscountAmount.Text, out Discount);
                    double vat1x = 7;
                    double.TryParse(txtVattax.Text, out vat1x);

                    double vat2x = vat1x + 100;
                    double vat3x = vat2x / 100;
                    double vat4x = vat3x - 1;

                    double FxCost = 0;


                    foreach (GridViewRowInfo rd in dgvData.Rows)
                    {
                        if (rd.IsVisible)
                        {
                            //if (cbUsefixunit.Checked)
                            //{
                            //    FxCost = 0;
                            //    try
                            //    {
                            //        //MessageBox.Show("xxx");
                            //        // double.TryParse(Convert.ToString(rd.Cells["dgvFxAmount"].Value), out FxCost);
                            //        Total += Convert.ToDouble(rd.Cells["dgvFxAmount"].Value);
                            //        VatDetail += Convert.ToDouble(rd.Cells["dgvFxAmount"].Value) * vat4x;
                            //    }
                            //    catch (Exception ex) { MessageBox.Show("EX01." + ex.Message); }
                            //}
                            //else
                            //{

                            //Total += Convert.ToDouble(rd.Cells["dgvAmount"].Value);
                            //VatDetail += Convert.ToDouble(rd.Cells["dgvAmount"].Value) * vat4x;

                            Total += Convert.ToDouble(rd.Cells["dgvExtendedCost"].Value);
                            VatDetail += Convert.ToDouble(rd.Cells["dgvExtendedCost"].Value) * vat4x;

                            //}
                        }
                    }

                    TotalSum = Total;
                    ///////////////////////
                    // MessageBox.Show(Total.ToString()+":"+VatDetail.ToString() +":"+(Total-VatDetail));
                    if (cbvatDetail.Checked)
                    {

                        //Total = Math.Round((Total - VatDetail),2, MidpointRounding.ToEven);
                        double.TryParse(txtLessPoDiscountAmount.Text, out Discount);
                        if (Discount > 0)
                        {
                            //Total = System.Math.Floor((Total / 1.07) * 100) / 100;                           
                            lbOrderSubtotal.Text = (TotalSum).ToString("##,###,##0.00");
                        }
                        else
                        {
                            Total = System.Math.Floor((Total / vat3x) * 100) / 100;
                            lbOrderSubtotal.Text = (Total).ToString("##,###,##0.00");
                        }
                    }
                    else
                    {
                        lbOrderSubtotal.Text = Total.ToString("##,###,##0.00");
                    }
                    ////////////////////////////
                    if (cbvatDetail.Checked)
                    {
                        if (Discount > 0)
                        {
                            //สำหรับส่วนลด หลักจากรวม vat ในรายการ
                            afDiscount = System.Math.Floor(((Total - Discount) * 100 / vat2x) * 100) / 100;
                            txtAfterDiscount.Text = afDiscount.ToString("##,###,##0.00");
                        }
                        else
                        {

                            afDiscount = (Total - Discount);
                            txtAfterDiscount.Text = afDiscount.ToString("##,###,##0.00");
                        }
                    }
                    else
                    {
                        afDiscount = (Total - Discount);
                        txtAfterDiscount.Text = afDiscount.ToString("##,###,##0.00");
                    }
                    ////////////////////////////
                    if (cbvat.Checked)
                    {
                        vat = afDiscount * vat4x;
                        txtVat.Text = vat.ToString("##,###,##0.00");

                    }
                    else
                    {
                        if (cbvatDetail.Checked)
                        {
                            vat = afDiscount * vat4x;
                            txtVat.Text = vat.ToString("##,###,##0.00");
                        }
                        else
                        {
                            vat = 0;
                            txtVat.Text = "0.00";
                        }
                    }

                    lbTotalOrder.Text = (afDiscount + vat).ToString("##,###,##0.00");
                    //if (chkVATDetail.Checked)
                    //{
                    //    if (Discount > 0)
                    //    {
                    //        txtGrandTotal.Text = (Total - Discount).ToString("##,###,##0.00"); 
                    //    }
                    //}
                    //////////หาทศนิยม///////////////
                    if (cbvatDetail.Checked)
                    {
                        double Total2 = 0;
                        double vat2 = 0;
                        double GrandTx = 0;
                        double Pus = 0;
                        double.TryParse(lbOrderSubtotal.Text, out Total2);
                        double.TryParse(txtVattax.Text, out vat2);
                        double.TryParse(lbTotalOrder.Text, out GrandTx);
                        if (Discount > 0)
                        {
                            if (GrandTx != (TotalSum - Discount))
                            {
                                Pus = Math.Abs(GrandTx - (TotalSum - Discount));
                                // Total = Total + Pus;                               
                                txtAfterDiscount.Text = (afDiscount + Pus).ToString("##,###,##0.00");
                                lbTotalOrder.Text = (afDiscount + vat + Pus).ToString("##,###,##0.00");
                            }
                        }
                        else
                        {
                            if (GrandTx != (TotalSum))
                            {
                                Pus = Math.Abs(GrandTx - (TotalSum));
                                Total = Total + Pus;
                                lbOrderSubtotal.Text = (Total).ToString("##,###,##0.00");
                                txtAfterDiscount.Text = (afDiscount + Pus).ToString("##,###,##0.00");
                                lbTotalOrder.Text = (afDiscount + vat + Pus).ToString("##,###,##0.00");
                            }
                        }
                    }

                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void txtDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {

            try
            {
                StockControl.dbClss.CheckDigitDecimal(e);
                LessPoDiscountAmount_KeyPress(e.KeyChar);
            }
            catch { }
        }

        private void txtVat_KeyPress(object sender, KeyPressEventArgs e)
        {
            StockControl.dbClss.CheckDigitDecimal(e);
        }

        private void txtDiscount_Leave(object sender, EventArgs e)
        {
            //decimal temp = 0;
            //decimal.TryParse(txtLessPoDiscountAmount.Text, out temp);
            //temp = decimal.Round(temp, 2);
            //txtLessPoDiscountAmount.Text = (temp).ToString();
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            getTotal();
        }

        private void txtVat_TextChanged(object sender, EventArgs e)
        {
            //getTotal();
            LessPoDiscountAmount_KeyPress((char)13);
        }

        private void btnDiscon_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblStatus.Text != "Completed"
                    && lblStatus.Text != "Approved"
                    && lblStatus.Text != "Waiting Approve")
                {
                    lblStatus.Text = "Discon";
                    Ac = "Discon";
                    int cc = 0;
                    if (MessageBox.Show("ต้องการยกเลิกรายการ ( " + txtPONo.Text + " ) หรือไม่ ?", "ยกเลิกรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        btnCal_Click(null, null);

                        this.Cursor = Cursors.WaitCursor;
                        using (DataClasses1DataContext db = new DataClasses1DataContext())
                        {

                            foreach (var g in dgvData.Rows)
                            {
                                if (g.IsVisible.Equals(true) && StockControl.dbClss.TBo(g.Cells["dgvDiscon_B"].Value))
                                {

                                    if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) > 0
                                        && StockControl.dbClss.TSt(g.Cells["dgvStatus"].Value) != "Full"
                                        && StockControl.dbClss.TSt(g.Cells["dgvStatus"].Value) == "Partial"
                                        )
                                    {
                                        foreach (DataRow row in dt_PODT.Rows)
                                        {
                                            if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) == StockControl.dbClss.TInt(row["id"]))
                                            {
                                                var u = (from ix in db.mh_PurchaseOrderDetails
                                                         where ix.TempPNo == txtTempNo.Text.Trim()
                                                         // && ix.TempNo == txtTempNo.Text                                             
                                                         && ix.id == StockControl.dbClss.TInt(g.Cells["dgvid"].Value)
                                                         && ix.BackOrder > 0
                                                         select ix).First();

                                                u.Discon = dbClss.TDe(u.Discon) + u.BackOrder;
                                                u.BackOrder = 0;

                                                cc += 1;

                                                db.SubmitChanges();
                                                dbClss.AddHistory(this.Name, "แก้ไข Item PO", "แก้ไขยกเลิกการรับส่วนที่เหลือ [" + u.CodeNo.ToString() + "]", txtPONo.Text);

                                                db.sp_010_Update_StockItem(StockControl.dbClss.TSt(g.Cells["dgvCodeNo"].Value), "BackOrder");
                                            }
                                        }
                                    }
                                }
                            }
                            //Calculate Status
                            db.sp_022_POHD_Cal_Status(txtTempNo.Text, txtPONo.Text);

                        }

                        if (cc > 0)
                            MessageBox.Show("บันทึกรายการ สำเร็จ!");
                        btnRefresh_Click(null, null);

                    }
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void btnDiscon_Item_Click(object sender, EventArgs e)
        {
            try
            {

                if (dgvData.Rows.Count < 0)
                    return;


                if (lblStatus.Text == "Process")
                {
                    this.Cursor = Cursors.WaitCursor;

                    if (StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["dgvStatus"].Value) == "Partial"
                        && StockControl.dbClss.TDe(dgvData.CurrentRow.Cells["dgvBackOrder"].Value) > 0)
                    {

                        int id = 0;
                        int.TryParse(StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["dgvid"].Value), out id);
                        if (id <= 0)
                            dgvData.Rows.Remove(dgvData.CurrentRow);

                        else
                        {
                            string CodeNo = "";
                            CodeNo = StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["dgvCodeNo"].Value);
                            if (MessageBox.Show("ต้องการยกเลิกรายการ ( " + CodeNo + " ) ออกจากรายการ หรือไม่ ?", "ยกเลิกรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                dgvData.CurrentRow.Cells["dgvDiscon_B"].Value = true;
                            }
                        }
                        getTotal();
                        SetRowNo1(dgvData);
                    }
                    else
                        MessageBox.Show("ไม่สามารถทำการยกเลิกรายการได้ สถานะไม่ถูกต้อง");
                }
                else
                {
                    MessageBox.Show("ไม่สามารถทำการลบรายการได้");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void openPRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvData.Rows.Count <= 0)
                    return;

                if (dbClss.TSt(dgvData.CurrentRow.Cells["dgvPRNo"].Value) != "")
                {
                    string TEmpPR = dbClss.TSt(dgvData.CurrentRow.Cells["dgvPRNo"].Value);
                    //using (DataClasses1DataContext db = new DataClasses1DataContext())
                    //{
                    //    var g = (from ix in db.tb_PurchaseRequests select ix)
                    //   .Where(a => a.PRNo == dbClss.TSt(dgvData.CurrentRow.Cells["dgvPRNo"].Value)
                    //    && (a.Status != "Cancel")
                    //    ).ToList();
                    //    if (g.Count() > 0)
                    //    {
                    //        TEmpPR = dbClss.TSt(g.FirstOrDefault().TEMPNo);
                    //    }
                    //}
                    if (TEmpPR != "")
                    {
                        CreatePR op = new CreatePR(TEmpPR);
                        op.ShowDialog();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnCal_Click(object sender, EventArgs e)
        {
            try
            {
                LessPoDiscountAmount_KeyPress((char)13);
            }
            catch { }
        }
        private void CallDiscontLast(bool am)
        {
            try
            {
                decimal TaxBase = 0;
                decimal Amount = 0;
                decimal DisP = 0;
                decimal DisA = 0;

                decimal.TryParse(lbOrderSubtotal.Text, out TaxBase);
                decimal.TryParse(txtLessPoDiscountAmount.Text, out DisA);
                decimal.TryParse(txtLessPoDiscountAmountPersen.Text, out DisP);
                //decimal SumDis = 0;
                dgvData.EndEdit();
                foreach (var r2 in dgvData.Rows)
                {
                    if (r2.IsVisible)
                    {
                        Amount = 0;
                        decimal.TryParse(Convert.ToString(r2.Cells["dgvAmount"].Value), out Amount);
                        if (!am) // Persent
                        {
                            //r2.Cells["dgvdiscount"].Value = (Amount*DisP) / 100;
                            r2.Cells["dgvDF"].Value = 4;
                            r2.Cells["dgvDiscountAmount"].Value = ((Amount * DisP) / 100);
                            r2.Cells["dgvDiscountExt"].Value = ((Amount * DisP) / 100);
                            r2.Cells["dgvDiscount"].Value = (((Amount * DisP) / 100) / Amount) * 100;
                            // SumDis += ((Amount * DisP) / 100);
                        }
                        else // Amount
                        {
                            // MessageBox.Show("xx" + TaxBase+","+Amount);

                            r2.Cells["dgvDF"].Value = 5;
                            r2.Cells["dgvDiscountAmount"].Value = ((Amount * DisA) / TaxBase);
                            r2.Cells["dgvDiscountExt"].Value = ((Amount * DisA) / TaxBase);
                            r2.Cells["dgvDiscount"].Value = (((Amount * DisA) / TaxBase) / Amount) * 100;
                        }
                    }
                }

            }
            catch { }
        }

        private void txtDiscount_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                LessPoDiscountAmount_KeyPress((char)13);
            }
        }
        private void LessPoDiscountAmount_KeyPress(char keys)
        {
            LastDiscount = true;
            lastDiscountAmount = true;
            //EditData = true;
            if (keys == 13) // Discount Amount
            {
                try
                {
                    //คือเมื่อมีการกด Enter ให้ทำการคำนวณ
                    CallDiscontLast(true);
                    //CallListDiscount();
                    //CallListDiscount();
                    CallTotal();
                    CallSumDiscountLast(true);
                }
                catch { }
            }
            if (cbvatDetail.Checked)
                getTotal();
        }
        private void CallSumDiscountLast(bool am)
        {
            try
            {
                decimal UnitCost = 0;
                decimal ExtendedCost = 0;
                decimal Qty = 0;
                decimal PA = 0;
                decimal PR = 0;
                decimal SumP = 0;
                decimal SumA = 0;
                dgvData.EndEdit();
                foreach (var r2 in dgvData.Rows)
                {
                    if (r2.IsVisible)
                    {
                        UnitCost = 0;
                        Qty = 0;
                        PA = 0;
                        PR = 0;

                        decimal.TryParse(Convert.ToString(r2.Cells["dgvOrderQty"].Value), out Qty);
                        decimal.TryParse(Convert.ToString(r2.Cells["dgvCost"].Value), out UnitCost);
                        decimal.TryParse(Convert.ToString(r2.Cells["dgvDiscountAmount"].Value), out PA);
                        decimal.TryParse(Convert.ToString(r2.Cells["dgvDiscount"].Value), out PR);
                        decimal.TryParse(Convert.ToString(r2.Cells["dgvAmount"].Value), out ExtendedCost);

                        SumP += ExtendedCost;
                        SumA += PA;
                    }
                }
                if (am)
                {
                    txtLessPoDiscountAmountPersen.Text = ((SumA / SumP) * 100).ToString("##0.00");
                }
                else
                {

                    txtLessPoDiscountAmount.Text = (SumA).ToString("###,###,##0.00");
                }
            }
            catch { }
        }
        private void CallTotal()
        {
            try
            {
                decimal UnitCost = 0;
                decimal ExtendedCost = 0;
                decimal Qty = 0;
                decimal PA = 0;
                //decimal PR = 0;
                decimal RC = 0;
                //bool hanfix = false;
                foreach (var r2 in dgvData.Rows)
                {
                    if (r2.IsVisible)
                    {
                        UnitCost = 0;
                        Qty = 0;
                        PA = 0;
                        //if (Convert.ToBoolean(r2.Cells["dgvHandFix"].Value))
                        //{
                        //    //1365.85376
                        //    r2.Cells["dgvUnitCost"].Value = 0;
                        //    // txtUnitCost.Text = (ExtendedCost / Qty).ToString("##,###,###,###,##0.000000");
                        //    // decimal.TryParse(txtUnitCost.Text, out Cost);
                        //    // MessageBox.Show(Cost.ToString());
                        //}
                        decimal.TryParse(Convert.ToString(r2.Cells["dgvOrderQty"].Value), out Qty);
                        decimal.TryParse(Convert.ToString(r2.Cells["dgvCost"].Value), out UnitCost);
                        decimal.TryParse(Convert.ToString(r2.Cells["dgvAmount"].Value), out ExtendedCost);
                        decimal.TryParse(Convert.ToString(r2.Cells["dgvDiscountAmount"].Value), out PA);

                        //decimal.TryParse(Convert.ToString(r2.Cells["dgvReceiveQty"].Value), out RC);

                        //if (Convert.ToBoolean(r2.Cells["dgvHandFix"].Value))
                        //{
                        //    //1365.85376
                        //    r2.Cells["dgvCost"].Value = Convert.ToDecimal(Math.Round(Convert.ToDecimal((ExtendedCost / Qty)), 6, MidpointRounding.AwayFromZero));
                        //    decimal.TryParse(Convert.ToString(r2.Cells["dgvCost"].Value), out UnitCost);
                        //    // txtUnitCost.Text = (ExtendedCost / Qty).ToString("##,###,###,###,##0.000000");
                        //    // decimal.TryParse(txtUnitCost.Text, out Cost);
                        //    // MessageBox.Show(Cost.ToString());
                        //}



                        r2.Cells["dgvDiscountExt"].Value = PA;
                        //if (Convert.ToString(r2.Cells["dgvStatus"].Value).Equals("Discon"))
                        //{
                        //    r2.Cells["dgvOutStanding"].Value = 0;
                        //}
                        //else
                        //{
                        //    r2.Cells["dgvOutStanding"].Value = Qty - RC;
                        //}

                        ExtendedCost = (Qty * UnitCost);

                        //if (chkCutDecimal.Checked)
                        //{
                        //    ExtendedCost = Math.Floor(ExtendedCost * 100) / 100;
                        //    //MessageBox.Show(ExtendedCost.ToString());
                        //}

                        r2.Cells["dgvExtendedCost"].Value = Convert.ToDecimal(Math.Round(Convert.ToDecimal(UnitCost), 2, MidpointRounding.AwayFromZero));
                        r2.Cells["dgvAmount"].Value = ExtendedCost;
                        r2.Cells["dgvNetofTAX"].Value = ExtendedCost - PA;


                        string a = dbClss.TSt(ExtendedCost - PA);
                        decimal aa = 0;
                        aa = Convert.ToDecimal(Math.Round(Convert.ToDecimal(a), 6, MidpointRounding.AwayFromZero));



                        r2.Cells["dgvNetofTAX"].Value = ExtendedCost - PA;//Convert.ToDecimal(Math.Round(Convert.ToDecimal(a), 6, MidpointRounding.AwayFromZero));

                        //string b =dbclass.TSt((((Qty * UnitCost) - PA) * vat) / 100);
                        r2.Cells["dgvVatAmount"].Value = (ExtendedCost - PA) * dbClss.TDe(txtVattax.Text) / 100;
                        //r2.Cells["dgvVatAmount"].Value = Convert.ToDecimal(Math.Round(Convert.ToDecimal(b), 6, MidpointRounding.AwayFromZero));

                        r2.Cells["dgvSubTotal"].Value = (ExtendedCost - PA) + ((ExtendedCost - PA) * dbClss.TDe(txtVattax.Text) / 100);

                    }


                    //  dgvDataOrder.EndEdit();

                    decimal Sumtotal = 0;
                    decimal Total = 0;
                    decimal SumTotal2 = 0;
                    //string Currency = "THB";
                    foreach (var rd in dgvData.Rows)
                    {
                        if (rd.IsVisible)
                        {
                            Total = 0;
                            decimal.TryParse(Convert.ToString(rd.Cells["dgvAmount"].Value), out Total);
                            Sumtotal += Total;
                            SumTotal2 += (Convert.ToDecimal(rd.Cells["dgvAmount"].Value) - Convert.ToDecimal(rd.Cells["dgvDiscountExt"].Value));
                            //Currency = (Convert.ToString(rd.Cells["dgvCurrency"].Value));
                        }
                    }
                    txtTotalsumDiscount.Text = (SumTotal2).ToString("###,###,##0.00");
                    lbOrderSubtotal.Text = (Sumtotal).ToString("###,###,##0.00");
                }
                //lbCurrency1.Text = Currency;
                //CalTAX1();
                CalSubtotal();
            }
            catch (Exception ex) { MessageBox.Show("err2: " + ex.Message); }
        }
        private void CalSubtotal()
        {
            try
            {
                //lbOrderSubtotal.Text = txtTotalOrder.Text;
                txtAfterDiscount.Text = txtTotalsumDiscount.Text;

                lbTotalOrder.Text = "";
                //txtTotalTax.Text = txtTotalTax_Taxes.Text;

                decimal netAmount = 0;
                decimal PlusTax = 0;
                decimal.TryParse(txtAfterDiscount.Text, out netAmount);
                txtPlusExcededTax.Text = ((netAmount * dbClss.TDe(txtVattax.Text)) / 100).ToString("N2");
                txtVat.Text = txtPlusExcededTax.Text;
                decimal.TryParse(txtPlusExcededTax.Text, out PlusTax);
                lbTotalOrder.Text = (netAmount + PlusTax).ToString("###,###,##0.00");

            }
            catch { }
        }

        private void txtLessPoDiscountAmountPersen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {

                LessPoDiscountAmountPersen_KeyPress((char)13);

            }


        }
        private void LessPoDiscountAmountPersen_KeyPress(char Keys)
        {
            LastDiscount = true;
            lastDiscountAmount = false;
            //EditData = true;
            if (Keys == 13)  // Discount %
            {
                try
                {
                    //คือเมื่อมีการกด Enter ให้ทำการคำนวณ
                    CallDiscontLast(false);
                    // CallListDiscount();                    
                    CallTotal();
                    CallSumDiscountLast(false);



                }
                catch { }
            }
            if (cbvatDetail.Checked)
                getTotal();
        }

        private void txtLessPoDiscountAmountPersen_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                dbClss.CheckDigitDecimal(e);
                LessPoDiscountAmountPersen_KeyPress((char)13);
            }
            catch { }
        }


        private void CalTAX1()
        {
            try
            {

                ////decimal TaxAmount = 0;
                //decimal TaxBase = 0;
                //decimal RateA = 0;
                //decimal.TryParse(txtRate.Text, out RateA);
                //decimal.TryParse(txtTotalsumDiscount.Text, out TaxBase);
                //decimal SumTax = 0;

                //foreach (var rd in dgvDataTaxes.Rows)
                //{
                //    RateA = 0;

                //    rd.Cells["dgvTaxBase"].Value = TaxBase * 1;
                //    rd.Cells["dgvTaxAmount"].Value = TaxBase * RateA / 100;
                //    SumTax += (TaxBase * RateA / 100);
                //}


                //txtTotalTax_Taxes.Text = (SumTax).ToString("###,###,##0.00");
                CalSubtotal();
            }
            catch (Exception ex) { MessageBox.Show("CalTax : " + ex.Message); }
        }

        private void ddlFactory_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            txtTempNo.Text = StockControl.dbClss.GetNo(10, 0);
        }

        private void cboVatGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    if (!cboVatGroup.Text.Equals(""))
                    {
                        this.cboVatType.AutoFilter = true;
                        this.cboVatType.AutoCompleteMode = AutoCompleteMode.Append;
                        FilterDescriptor fi = new FilterDescriptor();
                        fi.PropertyName = this.cboVatType.ValueMember;
                        fi.Operator = FilterOperator.StartsWith;
                        this.cboVatType.EditorControl.MasterTemplate.FilterDescriptors.Add(fi);

                        cboVatType.DataSource = null;
                        cboVatType.AutoCompleteMode = AutoCompleteMode.Append;
                        cboVatType.DisplayMember = "VatType";
                        cboVatType.ValueMember = "VatType";
                        string Temp = "";
                        var vg = (from ix in db.mh_VATSetups
                                  where ix.Active == true
                                  && ix.VatGroup == cboVatGroup.Text
                                  select new
                                  {
                                      VatType = ix.VatType,
                                      Rate = ix.Vat_
                                  }
                                    ).ToList();
                        cboVatType.DataSource = vg;
                        cboVatType.SelectedIndex = 0;
                        //cboVatType.Text = "";        

                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void cboVatGroup_Leave(object sender, EventArgs e)
        {
            cboVatGroup_SelectedIndexChanged(null, null);
        }

        private void cboVatType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    if (!cboVatGroup.Text.Equals("") && cboVatType.Text != "")
                    {
                        var I = (from ix in db.mh_VATSetups select ix)
                            .Where(a => a.Active == true
                            && a.VatGroup == cboVatGroup.Text
                            && a.VatType == cboVatType.Text
                            ).ToList();
                        if (I.Count > 0)
                        {
                            txtVattax.Text = dbClss.TDe(I.FirstOrDefault().Vat_).ToString();
                            cbvat.Checked = true;
                        }
                    }
                    else
                    {
                        txtVattax.Text = "0";
                        cbvat.Checked = false;
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void cboVatType_Leave(object sender, EventArgs e)
        {
            cboVatType_SelectedIndexChanged(null, null);
        }

        private void MasterTemplate_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
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

                else if (e.Column.Name.Equals("dgvGroupCode"))
                {
                    /////////////มีการ เคลียร์ การ Add ก่อน แล้วค่อย Add ใหม่////////////////
                    //Row = e.RowIndex;
                    RadMultiColumnComboBoxElement Comcol = (RadMultiColumnComboBoxElement)e.ActiveEditor;
                    Comcol.Columns.Clear();

                    //RadMultiColumnComboBoxElement Comcol = (RadMultiColumnComboBoxElement)e.ActiveEditor;
                    Comcol.DropDownSizingMode = SizingMode.UpDownAndRightBottom;
                    Comcol.DropDownWidth = 370;
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
                        HeaderText = "GroupCode",
                        Name = "GroupCode",
                        FieldName = "GroupCode",
                        Width = 150,
                        AllowFiltering = true,
                        ReadOnly = false
                    }
                   );
                    Comcol.EditorControl.Columns.Add(new GridViewTextBoxColumn
                    {
                        HeaderText = "GroupName",
                        Name = "GroupName",
                        FieldName = "GroupName",
                        Width = 200
                    });

                }


            }
            catch { }
        }

        private void btnSendApprove_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    if (lblStatus.Text == "Waiting")
                    {
                        if (baseClass.IsSendApprove())
                        {
                            db.sp_062_mh_ApproveList_Add(txtPONo.Text.Trim(), "Purchase Order", ClassLib.Classlib.User);
                            MessageBox.Show("Send complete.");
                            btnRefresh_Click(null, null);
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void ปรบจำนวนToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnchangeQty_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblStatus.Text != "Completed"
                    && lblStatus.Text != "Approved"
                    && lblStatus.Text != "Waiting Approve")
                {
                    //lblStatus.Text = "Discon";
                    Ac = "Discon";
                    int cc = 0;
                    if (MessageBox.Show("ต้องการปรับจำนวนสั่งซื้อรายการ ( " + txtPONo.Text + " ) หรือไม่ ?", "ยกเลิกรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        btnCal_Click(null, null);

                        this.Cursor = Cursors.WaitCursor;

                        using (DataClasses1DataContext db = new DataClasses1DataContext())
                        {
                            //Herder
                            var h = (from ix in db.mh_PurchaseOrders
                                     where ix.TempPNo.Trim() == txtTempNo.Text.Trim()
                                     && ix.Status != "Cancel"
                                     && ix.Status != "Completed"
                                     select ix).ToList();
                            if (h.Count > 0)  //มีรายการในระบบ
                            {
                                foreach (DataRow row in dt_POHD.Rows)
                                {

                                    var gg = (from ix in db.mh_PurchaseOrders
                                              where ix.TempPNo.Trim() == txtTempNo.Text.Trim() 
                                              && ix.Status != "Cancel"
                                               && ix.Status != "Completed"
                                              select ix).First();
                                    gg.ModifyBy = ClassLib.Classlib.User;
                                    gg.ModifyDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));                                    
                                    dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไข CreatePO โดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", txtPONo.Text);

                                    if (!txtPONo.Text.Trim().Equals(row["PONo"].ToString()))
                                    {
                                        gg.PONo = txtPONo.Text;
                                        dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขเลขที่ใบสั่งซื้อ [" + txtPONo.Text.Trim() + "]", txtPONo.Text);

                                        if (StockControl.dbClss.TSt(gg.Barcode).Equals(""))
                                            gg.Barcode = StockControl.dbClss.SaveQRCode2D(txtPONo.Text.Trim());
                                    }
                                    if (StockControl.dbClss.TSt(gg.Barcode).Equals(""))
                                        gg.Barcode = StockControl.dbClss.SaveQRCode2D(txtPONo.Text.Trim());

                                    //if (!cboVatType.Text.Trim().Equals(row["VATType"].ToString()))
                                    //{
                                    //    gg.VATType = cboVatType.Text;
                                    //    dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขประเภทภาษี [" + cboVatType.Text.Trim() + "]", txtPONo.Text);
                                    //}
                                    //if (!cboVatGroup.Text.Trim().Equals(row["VATGroup"].ToString()))
                                    //{
                                    //    gg.VATGroup = cboVatGroup.Text;
                                    //    dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขกลุ่มภาษี [" + cboVatGroup.Text.Trim() + "]", txtPONo.Text);
                                    //}
                                    //if (!txtVendorNo.Text.Trim().Equals(row["VendorNo"].ToString()))
                                    //{
                                    //    gg.VendorName = cboVendorName.Text;
                                    //    gg.VendorNo = txtVendorNo.Text.Trim();
                                    //    dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขรหัสผู้ขาย [" + txtVendorNo.Text.Trim() + "]", txtPONo.Text);
                                    //}
                                    //if (!txtQuotation.Text.Trim().Equals(row["Quotation"].ToString()))
                                    //{
                                    //    gg.Quotation = txtQuotation.Text.Trim();
                                    //    dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขเลขที่เสนอราคา [" + txtQuotation.Text.Trim() + "]", txtPONo.Text);
                                    //}
                                    //if (!cbClearBill.Checked.ToString().Equals(row["ClearBill"].ToString()))
                                    //{
                                    //    gg.ClearBill = cbClearBill.Checked;
                                    //    dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไข ClearBill [" + cbClearBill.Checked.ToString() + "]", txtPONo.Text);
                                    //}
                                    //if (!ddlCurrency.Text.Trim().Equals(row["CRRNCY"].ToString()))
                                    //{
                                    //    gg.CRRNCY = ddlCurrency.Text.Trim();
                                    //    decimal Rate = dbClss.TDe(txtRate.Text);
                                    //    if (Rate <= 0) Rate = 1;
                                    //    gg.Rate = Rate;
                                    //    dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขสกุลเงิน [" + ddlCurrency.Text.Trim() + "]", txtPONo.Text);
                                    //}
                                    //if (!txtContactName.Text.Trim().Equals(row["ContactName"].ToString()))
                                    //{
                                    //    gg.ContactName = txtContactName.Text.Trim();
                                    //    dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขผู้ติดต่อ [" + txtContactName.Text.Trim() + "]", txtPONo.Text);
                                    //}
                                    //if (!txtAddress.Text.Trim().Equals(row["Address"].ToString()))
                                    //{
                                    //    gg.Address = txtAddress.Text.Trim();
                                    //    dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขที่อยู่ [" + txtAddress.Text.Trim() + "]", txtPONo.Text);
                                    //}
                                    //if (!txtTel.Text.Trim().Equals(row["Tel"].ToString()))
                                    //{
                                    //    gg.Tel = txtTel.Text.Trim();
                                    //    dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขเบอร์โทร [" + txtTel.Text.Trim() + "]", txtPONo.Text);
                                    //}
                                    //if (!txtFax.Text.Trim().Equals(row["Fax"].ToString()))
                                    //{
                                    //    gg.Fax = txtFax.Text.Trim();
                                    //    dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขเบอร์แฟกซ์ [" + txtFax.Text.Trim() + "]", txtPONo.Text);
                                    //}

                                    //if (!txtEmail.Text.Trim().Equals(row["Email"].ToString()))
                                    //{
                                    //    gg.Email = txtEmail.Text.Trim();
                                    //    dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขอีเมลล์ [" + txtEmail.Text.Trim() + "]", txtPONo.Text);
                                    //}
                                    if (!txtVersion.Text.Trim().Equals(row["Version"].ToString()))
                                    {
                                        gg.Version = dbClss.TSt(txtVersion.Text);
                                        dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไข Rev. [" + txtVersion.Text.Trim() + "]", txtPONo.Text);
                                    }
                                    
                                    gg.vat = StockControl.dbClss.TDe(txtVat.Text);
                                    gg.VatTax = StockControl.dbClss.TDe(txtVattax.Text);
                                    gg.VatDetail = StockControl.dbClss.TBo(cbvatDetail.Checked);
                                    gg.Discount = StockControl.dbClss.TDe(txtLessPoDiscountAmount.Text);
                                    gg.Discpct = StockControl.dbClss.TDe(txtLessPoDiscountAmountPersen.Text);
                                    gg.AfterDiscount = StockControl.dbClss.TDe(txtAfterDiscount.Text);
                                    gg.Total = StockControl.dbClss.TDe(lbOrderSubtotal.Text);
                                    gg.GrandTotal = StockControl.dbClss.TDe(lbTotalOrder.Text);
                                    gg.Usefixunit = StockControl.dbClss.TBo(cbUsefixunit.Checked);
                                    //gg.CHStatus = "Waiting";
                                    //if (!dtDuedate.Text.Trim().Equals(""))
                                    //{
                                    //    string date1 = "";
                                    //    date1 = dtDuedate.Value.ToString("yyyyMMdd", new CultureInfo("en-US"));
                                    //    string date2 = "";
                                    //    if (!StockControl.dbClss.TSt(row["Duedate"].ToString()).Equals(""))
                                    //    {
                                    //        DateTime temp = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                    //        temp = Convert.ToDateTime(row["Duedate"]);
                                    //        date2 = temp.ToString("yyyyMMdd", new CultureInfo("en-US"));

                                    //    }
                                    //    if (!date1.Equals(date2))
                                    //    {
                                    //        DateTime? RequireDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                    //        if (!dtDuedate.Text.Equals(""))
                                    //            RequireDate = dtDuedate.Value;
                                    //        gg.Duedate = RequireDate;
                                    //        dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขวันที่ต้องการ [" + dtDuedate.Text.Trim() + "]", txtPONo.Text);

                                    //    }

                                    //}
                                    if (!txtRemarkHD.Text.Trim().Equals(row["Remark"].ToString()))
                                    {
                                        gg.Remark = txtRemarkHD.Text.Trim();
                                        dbClss.AddHistory(this.Name, "แก้ไข CreatePO", "แก้ไขหมายเหตุ [" + txtRemarkHD.Text.Trim() + "]", txtPONo.Text);
                                    }


                                    db.SubmitChanges();
                                }
                            }


                            //Detail
                            decimal ChangeQty = 0;
                            foreach (var g in dgvData.Rows)
                            {
                                if (g.IsVisible.Equals(true) && StockControl.dbClss.TDe(g.Cells["dgvChangeQty"].Value) > 0)
                                {

                                    if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) > 0
                                        && StockControl.dbClss.TSt(g.Cells["dgvStatus"].Value) != "Full"
                                        && StockControl.dbClss.TSt(g.Cells["dgvStatus"].Value) == "Partial"
                                        )
                                    {
                                        foreach (DataRow row in dt_PODT.Rows)
                                        {
                                            ChangeQty = StockControl.dbClss.TDe(g.Cells["dgvChangeQty"].Value);

                                            if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) == StockControl.dbClss.TInt(row["id"]))
                                            {
                                                var u_c = (from ix in db.mh_PurchaseOrderDetails
                                                           where ix.TempPNo == txtTempNo.Text.Trim()
                                                           // && ix.TempNo == txtTempNo.Text                                             
                                                           && ix.id == StockControl.dbClss.TInt(g.Cells["dgvid"].Value)
                                                           && ix.BackOrder > 0
                                                           select ix).ToList();
                                                if (u_c.Count > 0)
                                                {
                                                    var u = (from ix in db.mh_PurchaseOrderDetails
                                                             where ix.TempPNo == txtTempNo.Text.Trim()
                                                             // && ix.TempNo == txtTempNo.Text                                             
                                                             && ix.id == StockControl.dbClss.TInt(g.Cells["dgvid"].Value)
                                                             && ix.BackOrder > 0
                                                             select ix).First();

                                                    u.Discon = dbClss.TDe(u.Discon) + ChangeQty;
                                                    u.BackOrder = dbClss.TDe(u.BackOrder) - ChangeQty;
                                                    if (dbClss.TDe(u.BackOrder) <= 0)
                                                        u.BackOrder = 0;
                                                    u.OrderQty = StockControl.dbClss.TDe(g.Cells["dgvOrderQty"].Value);

                                                    cc += 1;

                                                    db.SubmitChanges();
                                                    dbClss.AddHistory(this.Name, "แก้ไข Item PO", "แก้ไขยกเลิกการรับส่วนที่เหลือ [" + u.CodeNo.ToString() + "]", txtPONo.Text);

                                                    db.sp_010_Update_StockItem(StockControl.dbClss.TSt(g.Cells["dgvCodeNo"].Value), "BackOrder");
                                                }
                                            }
                                        }
                                    }
                                }
                                else if(g.IsVisible && StockControl.dbClss.TSt(g.Cells["dgvStatus"].Value) == "Waiting")
                                {

                                    if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) > 0
                                        && StockControl.dbClss.TSt(g.Cells["dgvStatus"].Value) != "Full"
                                        && StockControl.dbClss.TSt(g.Cells["dgvStatus"].Value) != "Partial"
                                        )
                                    {
                                        DateTime? d = null;
                                        DateTime? DeliveryDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                        foreach (DataRow row in dt_PODT.Rows)
                                        {
                                            if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) == StockControl.dbClss.TInt(row["id"]))
                                            {
                                                var u_c = (from ix in db.mh_PurchaseOrderDetails
                                                         where ix.TempPNo == txtTempNo.Text.Trim()
                                                         // && ix.TempNo == txtTempNo.Text                                             
                                                         && ix.id == StockControl.dbClss.TInt(g.Cells["dgvid"].Value)
                                                         select ix).ToList();
                                                if (u_c.Count > 0)
                                                {
                                                    var u = (from ix in db.mh_PurchaseOrderDetails
                                                             where ix.TempPNo == txtTempNo.Text.Trim()
                                                             // && ix.TempNo == txtTempNo.Text                                             
                                                             && ix.id == StockControl.dbClss.TInt(g.Cells["dgvid"].Value)
                                                             select ix).First();

                                                    dbClss.AddHistory(this.Name, "แก้ไขรายการ Item PO", "id :" + StockControl.dbClss.TSt(g.Cells["dgvid"].Value)
                                                    + " CodeNo :" + StockControl.dbClss.TSt(g.Cells["dgvCodeNo"].Value)
                                                    + " แก้ไขโดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", txtPONo.Text);

                                                    u.PONo = txtPONo.Text.Trim();
                                                    u.Amount = dbClss.TDe(g.Cells["dgvAmount"].Value);
                                                    u.Discount = dbClss.TDe(g.Cells["dgvDiscount"].Value);
                                                    u.DiscountAmount = dbClss.TDe(g.Cells["dgvDiscountAmount"].Value);
                                                    u.ExtendedCost = dbClss.TDe(g.Cells["dgvExtendedCost"].Value);
                                                    u.DF = dbClss.TInt(g.Cells["dgvDF"].Value);

                                                    if (!StockControl.dbClss.TSt(g.Cells["dgvCodeNo"].Value).Equals(row["CodeNo"].ToString()))
                                                    {
                                                        u.CodeNo = StockControl.dbClss.TSt(g.Cells["dgvCodeNo"].Value);
                                                        dbClss.AddHistory(this.Name, "แก้ไข Item PO", "แก้ไขรหัสทูล [" + u.CodeNo + "]", txtPONo.Text);
                                                    }

                                                    if (!StockControl.dbClss.TSt(g.Cells["dgvItemName"].Value).Equals(row["ItemName"].ToString()))
                                                    {
                                                        u.ItemName = StockControl.dbClss.TSt(g.Cells["dgvItemName"].Value);
                                                        dbClss.AddHistory(this.Name, "แก้ไข Item PO", "แก้ไขชื่อทูล [" + u.ItemName + "]", txtPONo.Text);
                                                    }
                                                    if (!StockControl.dbClss.TSt(g.Cells["dgvItemDesc"].Value).Equals(row["ItemDesc"].ToString()))
                                                    {
                                                        u.ItemDesc = StockControl.dbClss.TSt(g.Cells["dgvItemDesc"].Value);
                                                        dbClss.AddHistory(this.Name, "แก้ไข Item PO", "แก้ไขรายละเอียดทูล [" + u.ItemDesc + "]", txtPONo.Text);
                                                    }
                                                    u.GroupCode = StockControl.dbClss.TSt(g.Cells["dgvGroupCode"].Value);

                                                    if (!StockControl.dbClss.TSt(g.Cells["dgvOrderQty"].Value).Equals(row["OrderQty"].ToString()))
                                                    {
                                                        decimal OrderQty = 0; decimal.TryParse(StockControl.dbClss.TSt(g.Cells["dgvOrderQty"].Value), out OrderQty);
                                                        u.OrderQty = StockControl.dbClss.TDe(g.Cells["dgvOrderQty"].Value);
                                                        u.BackOrder = OrderQty;
                                                        dbClss.AddHistory(this.Name, "แก้ไข Item PO", "แก้ไขจำนวน [" + u.OrderQty.ToString() + "]", txtPONo.Text);
                                                    }
                                                    if (!StockControl.dbClss.TSt(g.Cells["dgvPCSUnit"].Value).Equals(row["PCSUnit"].ToString()))
                                                    {
                                                        u.PCSUnit = StockControl.dbClss.TDe(g.Cells["dgvPCSUnit"].Value);
                                                        decimal dgvPCSUnit = 0; decimal.TryParse(StockControl.dbClss.TSt(g.Cells["dgvPCSUnit"].Value), out dgvPCSUnit);
                                                        dbClss.AddHistory(this.Name, "แก้ไข Item PO", "แก้ไขจำนวน:หน่วย [" + u.PCSUnit.ToString() + "]", txtPONo.Text);
                                                    }

                                                    if (!StockControl.dbClss.TSt(g.Cells["dgvUnit"].Value).Equals(row["Unit"].ToString()))
                                                    {
                                                        u.Unit = StockControl.dbClss.TSt(g.Cells["dgvUnit"].Value);
                                                        dbClss.AddHistory(this.Name, "แก้ไข Item PR", "แก้ไขหน่วย [" + u.Unit + "]", txtPONo.Text);
                                                    }

                                                    if (!StockControl.dbClss.TSt(g.Cells["dgvCost"].Value).Equals(row["Cost"].ToString()))
                                                    {
                                                        u.Cost = StockControl.dbClss.TDe(g.Cells["dgvCost"].Value);
                                                        decimal dgvCost = 0; decimal.TryParse(StockControl.dbClss.TSt(g.Cells["dgvCost"].Value), out dgvCost);
                                                        dbClss.AddHistory(this.Name, "แก้ไข Item PO", "แก้ไขราคา [" + u.Cost.ToString() + "]", txtPONo.Text);
                                                    }
                                                    if (!StockControl.dbClss.TSt(g.Cells["dgvPRNo"].Value).Equals(row["PRNo"].ToString()))
                                                    {
                                                        u.PRNo = StockControl.dbClss.TSt(g.Cells["dgvPRNo"].Value);
                                                        u.PRItem = StockControl.dbClss.TInt(g.Cells["dgvPRItem"].Value);
                                                        dbClss.AddHistory(this.Name, "แก้ไข Item PR", "แก้ไขเลขที่อ้างอิ่ง [" + u.PRNo + "]", txtPONo.Text);
                                                    }
                                                    if (!StockControl.dbClss.TSt(txtRate.Text).Equals(row["Rate"].ToString()))
                                                    {
                                                        decimal Rate = 0; decimal.TryParse(StockControl.dbClss.TSt(txtRate.Text), out Rate);
                                                        if (Rate >= 0)
                                                            Rate = 1;
                                                        u.Rate = Rate;

                                                        dbClss.AddHistory(this.Name, "แก้ไข Item Rate", "แก้ไข Rate [" + u.Rate.ToString() + "]", txtPONo.Text);
                                                    }


                                                    if (!StockControl.dbClss.TSt(g.Cells["dgvDeliveryDate"].Value).Equals(""))
                                                        DeliveryDate = Convert.ToDateTime((g.Cells["dgvDeliveryDate"].Value));
                                                    else
                                                        DeliveryDate = dtDuedate.Value;
                                                    u.DeliveryDate = DeliveryDate;
                                                    u.idCSTMPODt = dbClss.TInt(g.Cells["idCSTMPODt"].Value);

                                                    u.SS = 1;
                                                    //C += 1;
                                                    db.SubmitChanges();
                                                }
                                            }
                                        }
                                    }
                                }
                                else if(g.IsVisible.Equals(false) && StockControl.dbClss.TSt(g.Cells["dgvStatus"].Value) == "Waiting") //Cancel
                                {

                                    if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) > 0)
                                    {
                                        

                                        var u = (from ix in db.mh_PurchaseOrderDetails
                                                 where //ix.PRNo == txtPONo.Text.Trim() 
                                                       //&& ix.TempNo == txtTempNo.Text 
                                                  ix.id == StockControl.dbClss.TInt(g.Cells["dgvid"].Value)
                                                 select ix).First();
                                        u.SS = 0;

                                        dbClss.AddHistory(this.Name, "ลบ Item PO", "id :" + StockControl.dbClss.TSt(g.Cells["dgvid"].Value)
                                            + " CodeNo :" + StockControl.dbClss.TSt(g.Cells["dgvCodeNo"].Value)
                                            + " ลบโดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", txtPONo.Text);

                                        db.SubmitChanges();

                                        //ปรับสถานะ PR เป็น Waiting
                                        var p = (from ix in db.mh_PurchaseRequestLines
                                                 where ix.RefPOid == StockControl.dbClss.TInt(g.Cells["dgvid"].Value)
                                                  //&& ix.TempNo == txtTempNo.Text 
                                                  && ix.id == StockControl.dbClss.TInt(g.Cells["dgvPRItem"].Value)
                                                 select ix).First();
                                        p.Status = "Waiting";
                                        //p.RemainQty = p.OrderQty;
                                        p.PoNo = "";
                                        p.RefPOid = 0;
                                        dbClss.AddHistory(this.Name, "ปรับสถานะ Item PR", "ลบ PO จาก POid :" + StockControl.dbClss.TSt(g.Cells["dgvid"].Value)
                                            + " PONo :" + txtPONo.Text.Trim()
                                            + " ปรับโดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", StockControl.dbClss.TSt(g.Cells["dgvPRNo"].Value));

                                        db.SubmitChanges();

                                        db.sp_023_PRHD_Cal_Status(p.TempNo, p.PRNo);
                                    }
                                }
                            }
                            //Calculate Status
                            db.sp_022_POHD_Cal_Status(txtTempNo.Text, txtPONo.Text);

                        }

                        if (cc > 0)
                            MessageBox.Show("บันทึกรายการ สำเร็จ!");
                        btnRefresh_Click(null, null);

                    }
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
    }
}
