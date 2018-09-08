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
using Telerik.WinControls;

namespace StockControl
{
    public partial class CreatePR : Telerik.WinControls.UI.RadRibbonForm
    {
        public CreatePR()
        {
            InitializeComponent();
        }
        public CreatePR(string TempNo)
        {
            InitializeComponent();
            TempNo_temp = TempNo;
        }
        public CreatePR(List<GridViewRowInfo> RetDT)
        {
            InitializeComponent();
            this.RetDT = RetDT;
        }
        //private int RowView = 50;
        //private int ColView = 10;
        //DataTable dt = new DataTable();
        List<GridViewRowInfo> RetDT;
        string TempNo_temp = "";
        DataTable dt_PRH = new DataTable();
        DataTable dt_PRD = new DataTable();
        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            HistoryView hw = new HistoryView(this.Name,txtPRNo.Text);
            this.Cursor = Cursors.Default;
            hw.ShowDialog();
        }

        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }
        private void GETDTRow()
        {
            dt_PRH.Columns.Add(new DataColumn("PRNo", typeof(string)));
            dt_PRH.Columns.Add(new DataColumn("TEMPNo", typeof(string)));
            //dt_PRH.Columns.Add(new DataColumn("VendorNo", typeof(string)));
            //dt_PRH.Columns.Add(new DataColumn("VendorName", typeof(string)));
            //dt_PRH.Columns.Add(new DataColumn("Address", typeof(string)));
            //dt_PRH.Columns.Add(new DataColumn("ContactName", typeof(string)));
            //dt_PRH.Columns.Add(new DataColumn("Tel", typeof(string)));
            //dt_PRH.Columns.Add(new DataColumn("Fax", typeof(string)));
            //dt_PRH.Columns.Add(new DataColumn("Email", typeof(string)));
            dt_PRH.Columns.Add(new DataColumn("RequestBy", typeof(string)));
            dt_PRH.Columns.Add(new DataColumn("Department", typeof(string)));
            dt_PRH.Columns.Add(new DataColumn("RequestDate", typeof(DateTime)));
            dt_PRH.Columns.Add(new DataColumn("HDRemark", typeof(string)));
            dt_PRH.Columns.Add(new DataColumn("Status", typeof(string)));
            dt_PRH.Columns.Add(new DataColumn("CreateDate", typeof(DateTime)));
            dt_PRH.Columns.Add(new DataColumn("CreateBy", typeof(string)));
            dt_PRH.Columns.Add(new DataColumn("UpdateDate", typeof(DateTime)));
            dt_PRH.Columns.Add(new DataColumn("UpdateBy", typeof(string)));
            dt_PRH.Columns.Add(new DataColumn("CRRNCY", typeof(string)));
            dt_PRH.Columns.Add(new DataColumn("Barcode", typeof(string)));
            dt_PRH.Columns.Add(new DataColumn("ClearBill", typeof(Boolean)));
            dt_PRH.Columns.Add(new DataColumn("RefDocument", typeof(string)));
            dt_PRH.Columns.Add(new DataColumn("Total", typeof(decimal)));

            dt_PRD.Columns.Add(new DataColumn("TempNo", typeof(string)));
            dt_PRD.Columns.Add(new DataColumn("PRNo", typeof(string)));
            dt_PRD.Columns.Add(new DataColumn("CodeNo", typeof(string)));
            dt_PRD.Columns.Add(new DataColumn("ItemName", typeof(string)));
            dt_PRD.Columns.Add(new DataColumn("ItemDesc", typeof(string)));
            dt_PRD.Columns.Add(new DataColumn("GroupCode", typeof(string)));
            dt_PRD.Columns.Add(new DataColumn("OrderQty", typeof(decimal)));
            dt_PRD.Columns.Add(new DataColumn("RemainQty", typeof(decimal)));
            dt_PRD.Columns.Add(new DataColumn("UOM", typeof(string)));
            dt_PRD.Columns.Add(new DataColumn("PCSUOM", typeof(decimal)));
            dt_PRD.Columns.Add(new DataColumn("Cost", typeof(decimal)));
            dt_PRD.Columns.Add(new DataColumn("Amount", typeof(decimal)));
            dt_PRD.Columns.Add(new DataColumn("DeliveryDate", typeof(DateTime)));
            dt_PRD.Columns.Add(new DataColumn("LineName", typeof(string)));
            dt_PRD.Columns.Add(new DataColumn("MCName", typeof(string)));
            dt_PRD.Columns.Add(new DataColumn("SerialNo", typeof(string)));
            dt_PRD.Columns.Add(new DataColumn("LotNo", typeof(string)));
            dt_PRD.Columns.Add(new DataColumn("Remark", typeof(string)));
            dt_PRD.Columns.Add(new DataColumn("SS", typeof(int)));
            dt_PRD.Columns.Add(new DataColumn("VATType", typeof(string)));
            dt_PRD.Columns.Add(new DataColumn("VendorNo", typeof(string)));
            dt_PRD.Columns.Add(new DataColumn("VendorName", typeof(string)));
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
               
                DefaultItem();
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
                        Enable_Status(false, "View");
                        btnView_Click(null, null);
                        txtTempNo.Text = TempNo_temp;
                        DataLoad();

                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        private void DefaultItem()
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    //cboVendorName.AutoCompleteMode = AutoCompleteMode.Append;
                    //cboVendorName.DisplayMember = "VendorName";
                    //cboVendorName.ValueMember = "VendorNo";
                    //cboVendorName.DataSource = (from ix in db.tb_Vendors.Where(s => s.Active == true)
                    //                            select new { ix.VendorNo, ix.VendorName }).ToList();
                    //cboVendorName.SelectedIndex = 0;
                    //var G = (from ix in db.mh_Vendors.Where(s => s.Active == true)
                    //                     select new { ix.No, ix.Name }).ToList();
                    //List<string> aaa = new List<string>();
                    //if (G.Count > 0)
                    //{
                    //    foreach (var gg in G)
                    //        aaa.Add(gg.No.Trim());
                    //}
                    //else
                    //{
                    //    aaa.Add("");
                    //}

                    GridViewMultiComboBoxColumn Comcol = (GridViewMultiComboBoxColumn)dgvData.Columns["dgvVendorNo"];
                    Comcol.DataSource = (from ix in db.mh_Vendors.Where(s => s.Active == true)
                                         select new { ix.No, ix.Name }).ToList();
                    //Comcol.DataSource = aaa;
                    Comcol.DisplayMember = "No";
                    Comcol.DropDownStyle = RadDropDownStyle.DropDown;

                    

                    GridViewMultiComboBoxColumn Uom = (GridViewMultiComboBoxColumn)dgvData.Columns["dgvUOM"];
                    Uom.DataSource = (from ix in db.mh_Units.Where(s => s.UnitActive == true)
                                         select new { ix.UnitCode }).ToList();
                    Uom.DisplayMember = "UnitCode";
                    Uom.DropDownStyle = RadDropDownStyle.DropDownList;


                    GridViewMultiComboBoxColumn Groupc = (GridViewMultiComboBoxColumn)dgvData.Columns["dgvGroupCode"];
                    Groupc.DataSource = (from ix in db.mh_GroupTypes.Where(s => s.GroupActive == true)
                                         select new { ix.GroupCode, ix.GroupName }).ToList();
                    //Comcol.DataSource = aaa;
                    Groupc.DisplayMember = "GroupCode";
                    Groupc.DropDownStyle = RadDropDownStyle.DropDown;


                    GridViewMultiComboBoxColumn vt = (GridViewMultiComboBoxColumn)dgvData.Columns["dgvVATType"];
                    vt.DataSource = (from ix in db.mh_VATTypes.Where(s => s.Active == true)
                                         select new { ix.VatType }).ToList();
                    //Comcol.DataSource = aaa;
                    vt.DisplayMember = "VatType";
                    vt.DropDownStyle = RadDropDownStyle.DropDown;



                    ddlDept.DataSource = (from ix in db.mh_Departments.Where(s => s.Status == true)
                                          select new { ix.Department }).ToList();
                    ddlDept.DisplayMember = "Department";
                    ddlDept.Text = "";




                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void LoadCurrency()
        {
            //try
            //{
            //    using (DataClasses1DataContext db = new DataClasses1DataContext())
            //    {
            //        var G = (from ix in db.tb_CRRNCies select ix).ToList();
            //        ddlCurrency.DataSource = G;
            //        ddlCurrency.DisplayMember = "CRRNCY";
            //        ddlCurrency.Text = "";

            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    dbClss.AddError("CRRNCY", ex.Message, this.Name);
            //}
        }
       
        private void DataLoad()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                dt_PRD.Rows.Clear();
                dt_PRH.Rows.Clear();
                int ck = 0;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var g = (from ix in db.mh_PurchaseRequests select ix)
                        .Where(a => a.TEMPNo == txtTempNo.Text.Trim()
                         && (a.Status != "Cancel")
                         ).ToList();
                    if (g.Count() > 0)
                    {

                        DateTime? temp_date = null;
                        
                        txtPRNo.Text = StockControl.dbClss.TSt(g.FirstOrDefault().PRNo);
                        txtTempNo.Text = StockControl.dbClss.TSt(g.FirstOrDefault().TEMPNo);
                        txtRefDocument.Text = StockControl.dbClss.TSt(g.FirstOrDefault().RefDocument);
                        txtRemarkHD.Text = StockControl.dbClss.TSt(g.FirstOrDefault().HDRemark);
                        ddlDept.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Department);
                        txtRequestBy.Text = StockControl.dbClss.TSt(g.FirstOrDefault().RequestBy);

                        if (!StockControl.dbClss.TSt(g.FirstOrDefault().RequestDate).Equals(""))
                            dtRequest.Value = Convert.ToDateTime((g.FirstOrDefault().RequestDate), new CultureInfo("en-US"));
                        else
                            dtRequest.Value = Convert.ToDateTime(temp_date, new CultureInfo("en-US"));
                        cbClearBill.Checked = StockControl.dbClss.TBo(g.FirstOrDefault().ClearBill);


                        dt_PRH = StockControl.dbClss.LINQToDataTable(g);

                        //Detail
                        var d = (from ix in db.mh_PurchaseRequestLines select ix)
                            .Where(a => a.TempNo == txtTempNo.Text.Trim() && a.SS == 1).ToList();
                        if (d.Count() > 0)
                        {
                            int c = 0;
                            dgvData.DataSource = d;
                            dt_PRD = StockControl.dbClss.LINQToDataTable(d);
                            string CodeNo = "";
                            foreach (var x in dgvData.Rows)
                            {
                                c += 1;
                                x.Cells["dgvNo"].Value = c;

                                if (StockControl.dbClss.TSt(x.Cells["dgvStatus"].Value) == "Completed"
                                    || StockControl.dbClss.TSt(x.Cells["dgvStatus"].Value) == "Process"
                                    )
                                {
                                    x.Cells["dgvCodeNo"].ReadOnly = true;
                                    x.Cells["dgvItemDesc"].ReadOnly = true;
                                    x.Cells["dgvItemName"].ReadOnly = true;
                                    x.Cells["dgvPCSUOM"].ReadOnly = true;
                                    x.Cells["dgvUOM"].ReadOnly = true;
                                    x.Cells["dgvVendorNo"].ReadOnly = true;
                                    x.Cells["dgvCost"].ReadOnly = true;
                                    x.Cells["dgvGroupCode"].ReadOnly = true;
                                    x.Cells["dgvRemark"].ReadOnly = true;
                                    x.Cells["dgvOrderQty"].ReadOnly = true;
                                    x.Cells["dgvVATType"].ReadOnly = true;
                                    
                                }
                                else
                                {
                                     CodeNo = dbClss.TSt(x.Cells["dgvCodeNo"].Value);
                                    var cc = (from ix in db.mh_Items select ix)
                                    .Where(a => a.InternalNo == CodeNo.Trim() && a.Active == true).ToList();

                                    if(cc.Count>0)
                                    {
                                        x.Cells["dgvCodeNo"].ReadOnly = true;
                                        x.Cells["dgvItemDesc"].ReadOnly = true;
                                        //x.Cells["dgvUOM"].ReadOnly = true;
                                        x.Cells["dgvItemName"].ReadOnly = true;
                                        //x.Cells["dgvCost"].ReadOnly = true;
                                        x.Cells["dgvGroupCode"].ReadOnly = true;
                                        x.Cells["dgvVATType"].ReadOnly = true;
                                        //x.Cells["dgvRemark"].ReadOnly = true;
                                        //x.Cells["dgvOrderQty"].ReadOnly = true;
                                    }
                                    else
                                    {
                                        x.Cells["dgvCodeNo"].ReadOnly = false;
                                        x.Cells["dgvItemDesc"].ReadOnly = false;
                                        x.Cells["dgvUOM"].ReadOnly = false;
                                        x.Cells["dgvItemName"].ReadOnly = false;
                                        x.Cells["dgvCost"].ReadOnly = false;
                                        x.Cells["dgvGroupCode"].ReadOnly = false;
                                        x.Cells["dgvVATType"].ReadOnly = false;
                                        //x.Cells["dgvRemark"].ReadOnly = false;
                                        //x.Cells["dgvOrderQty"].ReadOnly = false;
                                    }
                                }
                            }
                        }

                        //lblStatus.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Status);
                        if (StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Cancel"))
                        {
                            btnNew.Enabled = true;
                            btnSave.Enabled = false;
                            btnDelete.Enabled = false;
                            btnView.Enabled = false;
                            btnEdit.Enabled = false;
                            lblStatus.Text = "Cancel";
                            dgvData.ReadOnly = true;
                            btnAdd_Item.Enabled = false;
                            btnDel_Item.Enabled = false;
                            btnNewRow.Enabled = false;
                        }
                        else if
                            (StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Completed"))
                        {
                            btnSave.Enabled = false;
                            btnDelete.Enabled = false;
                            btnView.Enabled = false;
                            btnEdit.Enabled = false;
                            btnNew.Enabled = true;
                            lblStatus.Text = "Completed";
                            dgvData.ReadOnly = true;
                            btnAdd_Item.Enabled = false;
                            btnDel_Item.Enabled = false;
                            btnNewRow.Enabled = false;
                        }
                        else
                        {
                            
                            btnNew.Enabled = true;
                            btnSave.Enabled = true;
                            btnDelete.Enabled = true;
                            btnView.Enabled = false;
                            btnEdit.Enabled = true;
                            lblStatus.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Status);
                            dgvData.ReadOnly = false;
                            btnAdd_Item.Enabled = false;
                            btnDel_Item.Enabled = false;
                            btnNewRow.Enabled = false;
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
                Cal_total();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        private void Cal_total()
        {
            decimal Amount = 0;
            foreach (var g in dgvData.Rows)
            {
                if(g.IsVisible)
                    Amount += dbClss.TDe(g.Cells["dgvAmount"].Value);
            }
            txtTotal.Text = Amount.ToString("N2");
        }

        //private bool CheckDuplicate(string code, string Code2)
        //{
        //    bool ck = false;

        //    //using (DataClasses1DataContext db = new DataClasses1DataContext())
        //    //{
        //    //    int i = (from ix in db.tb_Models
        //    //             where ix.ModelName == code

        //    //             select ix).Count();
        //    //    if (i > 0)
        //    //        ck = false;
        //    //    else
        //    //        ck = true;
        //    //}

        //    return ck;
        //}
        private void SaveHerder()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                var g = (from ix in db.mh_PurchaseRequests
                         where ix.TEMPNo.Trim() == txtTempNo.Text.Trim() && ix.Status != "Cancel"
                         //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                         select ix).ToList();
                if (g.Count > 0)  //มีรายการในระบบ
                {
                    foreach (DataRow row in dt_PRH.Rows)
                    {

                        var gg = (from ix in db.mh_PurchaseRequests
                                  where ix.TEMPNo.Trim() == txtTempNo.Text.Trim() && ix.Status != "Cancel"
                                  //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                                  select ix).First();

                        //gg.Status = "Waiting";
                        //gg.TEMPNo = txtTempNo.Text;
                        gg.UpdateBy = ClassLib.Classlib.User;
                        gg.UpdateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                        dbClss.AddHistory(this.Name, "แก้ไข CreatePR", "แก้ไข CreatePR โดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", txtPRNo.Text);
                        gg.Total = dbClss.TDe(txtTotal.Text);
                        if (!txtPRNo.Text.Trim().Equals(row["PRNo"].ToString()))
                        {
                            gg.PRNo = txtPRNo.Text;

                            dbClss.AddHistory(this.Name, "แก้ไข CreatePR", "แก้ไขเลขที่ใบสั่งซื้อ [" + txtPRNo.Text.Trim() + "]", txtPRNo.Text);
                        }

                        //if (StockControl.dbClss.TSt(gg.Barcode).Equals(""))
                        //    gg.Barcode = StockControl.dbClss.SaveQRCode2D(txtPRNo.Text.Trim());
                        
                        if (!txtRefDocument.Text.Trim().Equals(row["RefDocument"].ToString()))
                        {
                            gg.RefDocument = txtRefDocument.Text.Trim();
                            dbClss.AddHistory(this.Name, "แก้ไข CreatePR", "แก้ไข RefDocument [" + txtRefDocument.Text.Trim() + "]", txtPRNo.Text);
                        }
                       
                        if (!cbClearBill.Checked.ToString().Equals(row["ClearBill"].ToString()))
                        {
                            gg.ClearBill = cbClearBill.Checked;
                            dbClss.AddHistory(this.Name, "แก้ไข CreatePR", "แก้ไขClearBill [" + cbClearBill.Checked.ToString() + "]", txtPRNo.Text);
                        }
                        if (!txtRequestBy.Text.ToString().Equals(row["RequestBy"].ToString()))
                        {
                            gg.RequestBy = txtRequestBy.Text;
                            dbClss.AddHistory(this.Name, "แก้ไข CreatePR", "แก้ไข ผู้ขอซื้อ [" + txtRequestBy.Text + "]", txtPRNo.Text);
                        }
                        if (!ddlDept.Text.ToString().Equals(row["Department"].ToString()))
                        {
                            gg.Department = ddlDept.Text;
                            dbClss.AddHistory(this.Name, "แก้ไข CreatePR", "แก้ไข ผู้ขอซื้อ [" + ddlDept.Text + "]", txtPRNo.Text);
                        }


                        if (!dtRequest.Text.Trim().Equals(""))
                        {
                            string date1 = "";
                            date1 = dtRequest.Value.ToString("yyyyMMdd", new CultureInfo("en-US"));
                            string date2 = "";
                            if (!StockControl.dbClss.TSt(row["RequestDate"].ToString()).Equals(""))
                            {
                                DateTime temp = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                temp = Convert.ToDateTime(row["RequestDate"]);
                                date2 = temp.ToString("yyyyMMdd", new CultureInfo("en-US"));

                            }
                            if (!date1.Equals(date2))
                            {
                                DateTime? RequestDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                if (!dtRequest.Text.Equals(""))
                                    RequestDate = dtRequest.Value;
                                gg.RequestDate = RequestDate;
                                dbClss.AddHistory(this.Name, "แก้ไข CreatePR", "แก้ไขวันที่ต้องการ [" + dtRequest.Text.Trim() + "]", txtPRNo.Text);

                            }

                        }
                        if (!txtRemarkHD.Text.Trim().Equals(row["HDRemark"].ToString()))
                        {
                            gg.HDRemark = txtRemarkHD.Text.Trim();
                            dbClss.AddHistory(this.Name, "แก้ไข CreatePR", "แก้ไขหมายเหตุ [" + txtRemarkHD.Text.Trim() + "]", txtPRNo.Text);
                        }
                        db.SubmitChanges();
                    }
                }
                else  // Add ใหม่
                {
                    byte[] barcode = null;
                    //if(!txtPRNo.Text.Equals(""))
                    //    barcode = StockControl.dbClss.SaveQRCode2D(txtPRNo.Text.Trim());

                    DateTime? UpdateDate = null;

                    mh_PurchaseRequest gg = new mh_PurchaseRequest();
                
                    gg.UpdateBy = null;
                    gg.UpdateDate = UpdateDate;
                    gg.CreateBy = ClassLib.Classlib.User;
                    gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    gg.RefDocument = txtRefDocument.Text;
                    gg.Barcode = barcode;
                    gg.PRNo = txtPRNo.Text;
                    gg.TEMPNo = txtTempNo.Text;
                    gg.ClearBill = cbClearBill.Checked;
                    gg.Department = ddlDept.Text.Trim();
                    gg.RequestBy = txtRequestBy.Text.Trim();

                    DateTime? RequireDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    if (!dtRequest.Text.Equals(""))
                        RequireDate = dtRequest.Value;

                    gg.RequestDate = RequireDate;
                    gg.HDRemark = txtRemarkHD.Text.Trim();
                    gg.Total = dbClss.TDe(txtTotal.Text);
                    gg.Status = "Waiting";

                    db.mh_PurchaseRequests.InsertOnSubmit(gg);
                    db.SubmitChanges();

                    dbClss.AddHistory(this.Name, "เพิ่ม CreatePR", "สร้าง PRNo [" + txtPRNo.Text.Trim() + ",เลขที่อ้างอิง :" + txtTempNo.Text + "]", txtPRNo.Text);

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

                            mh_PurchaseRequestLine u = new mh_PurchaseRequestLine();
                            u.PRNo = txtPRNo.Text;
                            u.TempNo = txtTempNo.Text;
                            u.CodeNo = StockControl.dbClss.TSt(g.Cells["dgvCodeNo"].Value);
                            u.ItemName = StockControl.dbClss.TSt(g.Cells["dgvItemName"].Value);
                            u.ItemDesc = StockControl.dbClss.TSt(g.Cells["dgvItemDesc"].Value);
                            u.GroupCode = StockControl.dbClss.TSt(g.Cells["dgvGroupCode"].Value);
                            u.OrderQty = StockControl.dbClss.TDe(g.Cells["dgvOrderQty"].Value);
                            u.PCSUOM = StockControl.dbClss.TDe(g.Cells["dgvPCSUOM"].Value);
                            u.UOM = StockControl.dbClss.TSt(g.Cells["dgvUOM"].Value);
                            u.Cost = StockControl.dbClss.TDe(g.Cells["dgvCost"].Value);
                            u.Amount = StockControl.dbClss.TDe(g.Cells["dgvAmount"].Value);
                            u.Remark = StockControl.dbClss.TSt(g.Cells["dgvRemark"].Value);
                            u.VATType = StockControl.dbClss.TSt(g.Cells["dgvVATType"].Value);
                            u.VendorNo = StockControl.dbClss.TSt(g.Cells["dgvVendorNo"].Value);
                            u.VendorName = StockControl.dbClss.TSt(g.Cells["dgvVendorName"].Value);

                            //u.LotNo = StockControl.dbClss.TSt(g.Cells["dgvLotNo"].Value);
                            //u.SerialNo = StockControl.dbClss.TSt(g.Cells["dgvSerialNo"].Value);
                            //u.MCName = StockControl.dbClss.TSt(g.Cells["dgvMCName"].Value);
                            //u.LineName = StockControl.dbClss.TSt(g.Cells["dgvLineName"].Value);
                            u.Status = "Waiting";

                            //if (!StockControl.dbClss.TSt(g.Cells["dgvDeliveryDate"].Value).Equals(""))
                            //    DeliveryDate = Convert.ToDateTime((g.Cells["dgvDeliveryDate"].Value));
                            //else
                            //    DeliveryDate = d;
                            //u.DeliveryDate = DeliveryDate;
                            
                            u.SS = 1;
                            db.mh_PurchaseRequestLines.InsertOnSubmit(u);
                            db.SubmitChanges();
                            //C += 1;
                            dbClss.AddHistory(this.Name, "เพิ่ม Item PR", "เพิ่มรายการ Create PR [" + u.CodeNo + "]", txtPRNo.Text);

                        }
                        else  // อัพเดต
                        {

                            if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) > 0)
                            {
                                foreach (DataRow row in dt_PRD.Rows)
                                {
                                    if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) == StockControl.dbClss.TInt(row["id"]))
                                    {
                                        var V = (from ix in db.mh_PurchaseRequestLines
                                                 where ix.TempNo.Trim() == txtTempNo.Text.Trim()
                                                 //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                                                 && ix.Status != "Completed" && ix.Status != "Process" && ix.Status != "Discon" && ix.Status != "Cancel"
                                                 && ix.id == StockControl.dbClss.TInt(g.Cells["dgvid"].Value)
                                                 select ix).ToList();

                                        if (V.Count > 0)  //มีรายการในระบบ
                                        {

                                            var u = (from ix in db.mh_PurchaseRequestLines
                                                     where ix.TempNo == txtTempNo.Text.Trim()
                                                     // && ix.TempNo == txtTempNo.Text
                                                     && ix.Status != "Completed" && ix.Status != "Process" && ix.Status != "Discon" && ix.Status != "Cancel"
                                                     && ix.id == StockControl.dbClss.TInt(g.Cells["dgvid"].Value)
                                                     select ix).First();


                                            dbClss.AddHistory(this.Name, "แก้ไขรายการ Item PR", "id :" + StockControl.dbClss.TSt(g.Cells["dgvid"].Value)
                                            + " CodeNo :" + StockControl.dbClss.TSt(g.Cells["dgvCodeNo"].Value)
                                            + " แก้ไขโดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", txtPRNo.Text);

                                            u.PRNo = txtPRNo.Text.Trim();

                                            if (!StockControl.dbClss.TSt(g.Cells["dgvCodeNo"].Value).Equals(row["CodeNo"].ToString()))
                                            {
                                                u.CodeNo = StockControl.dbClss.TSt(g.Cells["dgvCodeNo"].Value);
                                                dbClss.AddHistory(this.Name, "แก้ไข Item PR", "แก้ไขรหัสพาร์ท [" + u.CodeNo + "]", txtPRNo.Text);
                                            }

                                            u.ItemName = StockControl.dbClss.TSt(g.Cells["dgvItemName"].Value);
                                            u.ItemDesc = StockControl.dbClss.TSt(g.Cells["dgvItemDesc"].Value);
                                            if (!StockControl.dbClss.TSt(g.Cells["dgvVATType"].Value).Equals(row["VATType"].ToString()))
                                            {
                                                u.VATType = StockControl.dbClss.TSt(g.Cells["dgvVATType"].Value);
                                                dbClss.AddHistory(this.Name, "แก้ไข Item PR", "แก้ไขประเภทภาษี [" + u.VATType + "]", txtPRNo.Text);
                                            }
                                            if (!StockControl.dbClss.TSt(g.Cells["dgvGroupCode"].Value).Equals(row["GroupCode"].ToString()))
                                            {
                                                u.GroupCode = StockControl.dbClss.TSt(g.Cells["dgvGroupCode"].Value);
                                                dbClss.AddHistory(this.Name, "แก้ไข Item PR", "แก้ไขประเภทสินค้า [" + u.GroupCode + "]", txtPRNo.Text);
                                            }

                                            if (!StockControl.dbClss.TSt(g.Cells["dgvOrderQty"].Value).Equals(row["OrderQty"].ToString()))
                                            {
                                                decimal OrderQty = 0; decimal.TryParse(StockControl.dbClss.TSt(g.Cells["dgvOrderQty"].Value), out OrderQty);
                                                u.OrderQty = StockControl.dbClss.TDe(g.Cells["dgvOrderQty"].Value);
                                                dbClss.AddHistory(this.Name, "แก้ไข Item PR", "แก้ไขจำนวน [" + OrderQty.ToString() + "]", txtPRNo.Text);
                                            }

                                            u.PCSUOM = StockControl.dbClss.TDe(g.Cells["dgvPCSUOM"].Value);

                                            if (!StockControl.dbClss.TSt(g.Cells["dgvUOM"].Value).Equals(row["UOM"].ToString()))
                                            {
                                                u.UOM = StockControl.dbClss.TSt(g.Cells["dgvUnitCode"].Value);
                                                dbClss.AddHistory(this.Name, "แก้ไข Item PR", "แก้ไขหน่วย [" + u.UOM + "]", txtPRNo.Text);
                                            }

                                            u.Cost = StockControl.dbClss.TDe(g.Cells["dgvCost"].Value);
                                            u.Amount = StockControl.dbClss.TDe(g.Cells["dgvAmount"].Value);
                                            if (!StockControl.dbClss.TSt(g.Cells["dgvRemark"].Value).Equals(row["Remark"].ToString()))
                                            {
                                                u.Remark = StockControl.dbClss.TSt(g.Cells["dgvRemark"].Value);
                                                dbClss.AddHistory(this.Name, "แก้ไข Item PR", "แก้ไขวัตถุประสงค์ [" + u.Remark + "]", txtPRNo.Text);
                                            }
                                            if (!StockControl.dbClss.TSt(g.Cells["dgvVendorNo"].Value).Equals(row["VendorNo"].ToString()))
                                            {
                                                u.VendorNo = StockControl.dbClss.TSt(g.Cells["dgvVendorNo"].Value);
                                                u.VendorName = StockControl.dbClss.TSt(g.Cells["dgvVendorName"].Value);

                                                dbClss.AddHistory(this.Name, "แก้ไข Item PR", "แก้ไขผู้ขาย [" + u.VendorNo + "]", txtPRNo.Text);
                                            }
                                            //if (!StockControl.dbClss.TSt(g.Cells["dgvLotNo"].Value).Equals(row["LotNo"].ToString()))
                                            //{
                                            //    u.LotNo = StockControl.dbClss.TSt(g.Cells["dgvLotNo"].Value);
                                            //    dbClss.AddHistory(this.Name, "แก้ไข Item PR", "แก้ไข LotNo [" + u.LotNo + "]", txtPRNo.Text);
                                            //}
                                            //if (!StockControl.dbClss.TSt(g.Cells["dgvSerialNo"].Value).Equals(row["SerialNo"].ToString()))
                                            //{
                                            //    u.SerialNo = StockControl.dbClss.TSt(g.Cells["dgvSerialNo"].Value);
                                            //    dbClss.AddHistory(this.Name, "แก้ไข Item PR", "แก้ไขซีเรียล [" + u.SerialNo + "]", txtPRNo.Text);
                                            //}
                                            //if (!StockControl.dbClss.TSt(g.Cells["dgvMCName"].Value).Equals(row["MCName"].ToString()))
                                            //{
                                            //    u.MCName = StockControl.dbClss.TSt(g.Cells["dgvMCName"].Value);
                                            //    dbClss.AddHistory(this.Name, "แก้ไข Item PR", "แก้ไขชื่อ Machine [" + u.MCName + "]", txtPRNo.Text);
                                            //}
                                            //if (!StockControl.dbClss.TSt(g.Cells["dgvLineName"].Value).Equals(row["LineName"].ToString()))
                                            //{
                                            //    u.LineName = StockControl.dbClss.TSt(g.Cells["dgvLineName"].Value);
                                            //    dbClss.AddHistory(this.Name, "แก้ไข Item PR", "แก้ไขชื่อ Line [" + u.LineName + "]", txtPRNo.Text);
                                            //}


                                            //if (!StockControl.dbClss.TSt(g.Cells["dgvDeliveryDate"].Value).Equals(""))
                                            //    DeliveryDate = Convert.ToDateTime((g.Cells["dgvDeliveryDate"].Value));
                                            //else
                                            //    DeliveryDate = d;
                                            //u.DeliveryDate = DeliveryDate;


                                            u.SS = 1;


                                            //C += 1;
                                            db.SubmitChanges();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else //Del
                    {
                        if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) > 0)
                        {
                            var u = (from ix in db.mh_PurchaseRequestLines
                                     where ix.PRNo == txtPRNo.Text.Trim()
                                     //&& ix.TempNo == txtTempNo.Text 
                                     && ix.Status != "Completed" && ix.Status != "Process" && ix.Status != "Discon" && ix.Status != "Cancel"
                                     && ix.id == StockControl.dbClss.TInt(g.Cells["dgvid"].Value)
                                     select ix).First();
                            u.SS = 0;
                            u.Status = "Cancel";
                            dbClss.AddHistory(this.Name, "ลบ Item PR", "id :" + StockControl.dbClss.TSt(g.Cells["dgvid"].Value)
                                + " CodeNo :" + StockControl.dbClss.TSt(g.Cells["dgvCodeNo"].Value)
                                + " ลบโดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", txtPRNo.Text);


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
       
        private void btnCancel_Click(object sender, EventArgs e)
        {
           
        }
        private void Enable_Status(bool ss, string Condition)
        {
            if (Condition.Equals("-") || Condition.Equals("New"))
            {
                txtRefDocument.Enabled = ss;
                txtPRNo.Enabled = ss;
                dtRequest.Enabled = ss;
                dgvData.ReadOnly = false;
                txtRemarkHD.Enabled = ss;
                //txtCurrency.Enabled = ss;
                btnAdd_Item.Enabled = ss;
                btnDel_Item.Enabled = ss;
                btnNewRow.Enabled = ss;
                cbClearBill.Enabled = ss;
                ddlDept.Enabled = ss;
                txtRequestBy.Enabled = ss;
            }
            else if (Condition.Equals("View"))
            {
                txtPRNo.Enabled = ss;
                dtRequest.Enabled = ss;
                dgvData.ReadOnly = !ss;
                txtRemarkHD.Enabled = ss;
                btnAdd_Item.Enabled = ss;
                btnDel_Item.Enabled = ss;
                btnNewRow.Enabled = ss;
                txtRefDocument.Enabled = ss;
                cbClearBill.Enabled = ss;
                ddlDept.Enabled = ss;
                txtRequestBy.Enabled = ss;
            }
            else if (Condition.Equals("Edit"))
            {
                txtPRNo.Enabled = ss;
                txtRefDocument.Enabled = ss;
                dtRequest.Enabled = ss;
                dgvData.ReadOnly = !ss;
                txtRemarkHD.Enabled = ss;
                //txtCurrency.Enabled = ss;
                //txtVendorNo.Enabled = ss;
                btnAdd_Item.Enabled = ss;
                btnNewRow.Enabled = ss;
                btnDel_Item.Enabled = ss;
               
                cbClearBill.Enabled = ss;
                ddlDept.Enabled = ss;
                txtRequestBy.Enabled = ss;
            }
        }
       
        private void ClearData()
        {
            txtRefDocument.Text ="";
            txtPRNo.Text = "";
            txtTempNo.Text = "";
            //lblStatus.Text = "-";
            dtRequest.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
            ddlDept.Text = "";
            txtRequestBy.Text = ClassLib.Classlib.User;
            dgvData.Rows.Clear();
            dgvData.DataSource = null;
            txtRemarkHD.Text = "";
            cbClearBill.Checked = false;
            dt_PRH.Rows.Clear();
            dt_PRD.Rows.Clear();
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
           
            btnView.Enabled = true;
            btnEdit.Enabled = true;
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            btnDelete.Enabled = true;
            ClearData();
            Enable_Status(true, "New");
            lblStatus.Text = "New";
            Ac = "New";
            row = dgvData.Rows.Count - 1;
            if (row < 0)
                row = 0;
            //getมาไว้ก่อน แต่ยังไมได้ save 
            //txtTempNo.Text = StockControl.dbClss.GetNo(3, 0);

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
           

        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    int te = 0;

                    var p = (from ix in db.mh_PurchaseRequestLines
                             where ix.TempNo.Trim() == txtTempNo.Text.Trim()
                             && ix.Status != "Cancel"
                             && ix.SS == 1
                             
                             select ix).ToList();
                    if (p.Count > 0)
                    {
                        te = 1;
                    }

                    if (te == 1)
                    {
                        MessageBox.Show("ไม่สามารถทำการลบรายการได้ สถานะไม่ถูกต้อง");
                        return;
                    }


                    if (lblStatus.Text != "Completed" && lblStatus.Text != "Process")
                    {
                        lblStatus.Text = "Delete";
                        Ac = "Del";
                        if (MessageBox.Show("ต้องการลบรายการ ( " + txtPRNo.Text + " ) หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            this.Cursor = Cursors.WaitCursor;

                            var g = (from ix in db.mh_PurchaseRequests
                                     where ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                                     && ix.Status != "Cancel" && ix.Status != "Completed" && ix.Status != "Process" && ix.Status != "Discon"
                                     //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                                     select ix).ToList();
                            if (g.Count > 0)  //มีรายการในระบบ
                            {
                                var gg = (from ix in db.mh_PurchaseRequests
                                          where ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                                          //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                                          select ix).First();

                                try
                                {
                                    var s = (from ix in db.mh_PurchaseRequestLines
                                             where ix.TempNo.Trim() == txtTempNo.Text.Trim()
                                             //&& ix.Status =="Waiting"
                                             select ix).ToList();
                                    if (s.Count > 0)
                                    {
                                        foreach (var ss in s)
                                        {
                                            ss.SS = 0;
                                            ss.Status = "Cancel";
                                            db.SubmitChanges();

                                            ////update Stock backorder
                                            //db.sp_010_Update_StockItem(Convert.ToString(ss.CodeNo), "");
                                        }

                                    }
                                }
                                catch (Exception ex) { MessageBox.Show(ex.Message); }
                                //----------------------//


                                gg.Status = "Cancel";
                                gg.UpdateBy = ClassLib.Classlib.User;
                                gg.UpdateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));

                                dbClss.AddHistory(this.Name, "ลบ PR", "Delete PRNo [" + txtPRNo.Text.Trim() + "]", txtPRNo.Text);

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
              
                if (dtRequest.Text.Equals(""))
                    err += "- “วันที่ต้องการ:” เป็นค่าว่าง \n";
                if (ddlDept.Text.Trim().Equals(""))
                    err += "- “แผนก/ฝ่าย:” เป็นค่าว่าง \n";
                if (txtRequestBy.Text.Trim().Equals(""))
                    err += "- “ผู้ขอซื้อ:” เป็นค่าว่าง \n";


                if (dgvData.Rows.Count <= 0)
                    err += "- “รายการ:” เป็นค่าว่าง \n";
                foreach (GridViewRowInfo rowInfo in dgvData.Rows)
                {
                    if (rowInfo.IsVisible)
                    {
                        if (StockControl.dbClss.TSt(rowInfo.Cells["dgvCodeNo"].Value).Trim().Equals(""))
                            err += "- “รหัสทูล:” เป็นค่าว่าง \n";
                        if (StockControl.dbClss.TSt(rowInfo.Cells["dgvItemName"].Value).Trim().Equals(""))
                            err += "- “ชื่อทูล:” เป็นค่าว่าง \n";
                        if (StockControl.dbClss.TSt(rowInfo.Cells["dgvItemDesc"].Value).Trim().Equals(""))
                            err += "- “รายละเอียดทูล:” เป็นค่าว่าง \n";
                        if (StockControl.dbClss.TSt(rowInfo.Cells["dgvGroupCode"].Value).Trim().Equals(""))
                            err += "- “ประเภทกลุ่มสินค้า:” เป็นค่าว่าง \n";
                        if (StockControl.dbClss.TDe(rowInfo.Cells["dgvOrderQty"].Value) <= 0)
                            err += "- “จำนวน:” น้อยกว่า 0 \n";
                        if (StockControl.dbClss.TSt(rowInfo.Cells["dgvUOM"].Value).Trim().Equals(""))
                            err += "- “หน่วย:” เป็นค่าว่าง \n";
                        if (StockControl.dbClss.TDe(rowInfo.Cells["dgvPCSUOM"].Value) <=0)
                            err += "- “จำนวน/หน่วย:” น้อยกว่า 0 \n";
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
                dbClss.AddError("CreatePR", ex.Message, this.Name);
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
                                    if (!txtPRNo.Text.Equals(""))
                                    {

                                        var p = (from ix in db.mh_PurchaseRequests
                                                 where ix.PRNo.ToUpper().Trim() == txtPRNo.Text.Trim()
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
                                        txtPRNo.Text = StockControl.dbClss.GetNo(12, 2);

                                    txtTempNo.Text = StockControl.dbClss.GetNo(3, 2);

                                }


                                var ggg = (from ix in db.mh_PurchaseRequests
                                           where ix.TEMPNo.Trim() == txtTempNo.Text.Trim() //&& ix.Status != "Cancel"
                                           //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                                           select ix).ToList();
                                if (ggg.Count > 1)  //มีรายการในระบบ
                                {
                                    MessageBox.Show("เลขที่อ้างอิงถูกใช้แล้ว กรุณาสร้างเลขใหม่");
                                    return;
                                }
                            }

                            if (!txtTempNo.Text.Equals(""))
                            {

                                SaveHerder();
                                AddPR_d();

                                Ac = "View";
                                btnEdit.Enabled = true;
                                btnView.Enabled = false;
                                btnNew.Enabled = true;
                                Enable_Status(false, "View");

                                using (DataClasses1DataContext db = new DataClasses1DataContext())
                                {
                                    db.sp_023_PRHD_Cal_Status(txtTempNo.Text, txtPRNo.Text);
                                }
                                DataLoad();
                                
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
        //private void Insert_Stock_temp()
        //{
        //    //try
        //    //{

        //    //    using (DataClasses1DataContext db = new DataClasses1DataContext())
        //    //    {
                  
        //    //        var g = (from ix in db.tb_PurchaseRequestLines
        //    //                     //join i in db.tb_Items on ix.CodeNo equals i.CodeNo
        //    //                 where ix.TempNo.Trim() == txtTempNo.Text.Trim() && ix.SS == 1
        //    //                 select ix).ToList();
        //    //        if (g.Count > 0)
        //    //        {
        //    //            //insert Stock
        //    //            foreach (var vv in g)
        //    //            {
        //    //                db.sp_010_Update_StockItem(Convert.ToString(vv.CodeNo),"");
        //    //                //dbClss.Insert_StockTemp(vv.CodeNo, Convert.ToDecimal(vv.OrderQty), "PR_Temp", "Inv");
        //    //            }
        //    //        }
        //    //    }
        //    //}
        //    //catch (Exception ex) { MessageBox.Show(ex.Message); }
        //}
        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                dgvData.EndEdit();
                 if (e.RowIndex >= -1)
                {
                    if (dgvData.Columns["dgvCodeNo"].Index == e.ColumnIndex)
                    {
                        string CodeNo = dbClss.TSt(e.Row.Cells["dgvCodeNo"].Value);

                        int c = 0;
                        foreach (GridViewRowInfo rowInfo in dgvData.Rows)
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

                                    string ItemDescription = StockControl.dbClss.TSt(g.FirstOrDefault().InternalName);
                                    string GroupCode = StockControl.dbClss.TSt(g.FirstOrDefault().GroupType);
                                    decimal OrderQty = dbClss.TDe(e.Row.Cells["dgvCost"].Value);
                                    decimal PCSUOM = dbClss.Con_UOM(CodeNo, StockControl.dbClss.TSt(g.FirstOrDefault().PurchaseUOM));
                                    string UOM = StockControl.dbClss.TSt(g.FirstOrDefault().PurchaseUOM);
                                    //decimal StandardCost = 0;
                                    string Status = "Adding";
                                    string VATType = dbClss.TSt(g.FirstOrDefault().VatType);
                                    e.Row.Cells["dgvCodeNo"].Value = CodeNo;
                                    e.Row.Cells["dgvItemDesc"].Value = ItemDescription;
                                    e.Row.Cells["dgvGroupCode"].Value = GroupCode;
                                    //e.Row.Cells["dgvOrderQty"].Value = OrderQty;
                                    e.Row.Cells["dgvPCSUOM"].Value = PCSUOM;
                                    e.Row.Cells["dgvUOM"].Value = UOM;
                                    e.Row.Cells["dgvVATType"].Value = VATType;
                                    //e.Row.Cells["dgvCost"].Value = StandardCost;
                                    //e.Row.Cells["dgvAmount"].Value = OrderQty * StandardCost;
                                    //e.Row.Cells["dgvid"].Value = 0;
                                    e.Row.Cells["dgvStatus"].Value = Status;
                                    e.Row.Cells["dgvVendorNo"].Value = dbClss.TSt(g.FirstOrDefault().VendorNo);
                                    e.Row.Cells["dgvVendorName"].Value = dbClss.TSt(g.FirstOrDefault().VendorName);
                                    e.Row.Cells["dgvCodeNo"].ReadOnly = true;
                                    e.Row.Cells["dgvGroupCode"].ReadOnly = true;
                                    e.Row.Cells["dgvItemDesc"].ReadOnly = true;
                                    //e.Row.Cells["dgvUOM"].ReadOnly = true;
                                    //e.Row.Cells["dgvPCSUOM"].ReadOnly = true;
                                    e.Row.Cells["dgvVATType"].ReadOnly = true;

                                }
                                else
                                {
                                    string ItemDescription = "";
                                    string GroupCode = "";
                                    //decimal OrderQty = 0;
                                    decimal PCSUOM = 1;
                                    string UOM = "";
                                    //string Status = "Adding";
                                    string VATType = "";
                                    e.Row.Cells["dgvCodeNo"].Value = CodeNo;
                                    e.Row.Cells["dgvItemDesc"].Value = ItemDescription;
                                    e.Row.Cells["dgvGroupCode"].Value = GroupCode;
                                    //e.Row.Cells["dgvOrderQty"].Value = OrderQty;
                                    e.Row.Cells["dgvPCSUOM"].Value = PCSUOM;
                                    e.Row.Cells["dgvUOM"].Value = UOM;
                                    e.Row.Cells["dgvVATType"].Value = VATType;

                                    e.Row.Cells["dgvCodeNo"].ReadOnly = false;
                                    e.Row.Cells["dgvGroupCode"].ReadOnly = false;
                                    e.Row.Cells["dgvItemDesc"].ReadOnly = false;
                                    //e.Row.Cells["dgvUOM"].ReadOnly = false;
                                    e.Row.Cells["dgvVATType"].ReadOnly = false;
                                    //e.Row.Cells["dgvPCSUOM"].ReadOnly = false;
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
                        Cal_total();
                    }
                    else if (dgvData.Columns["dgvVendorNo"].Index == e.ColumnIndex)
                    {
                        string VendorNo = dbClss.TSt(e.Row.Cells["dgvVendorNo"].Value);

                        using (DataClasses1DataContext db = new DataClasses1DataContext())
                        {
                            var g = (from ix in db.mh_Vendors select ix)
                                .Where(a => a.No.ToUpper().Trim().Equals(VendorNo.ToUpper().Trim())).ToList();
                            if (g.Count > 0)
                            {
                                e.Row.Cells["dgvVendorName"].Value = dbClss.TSt(g.FirstOrDefault().Name);
                            }
                        }
                    }
                    else if (dgvData.Columns["dgvUOM"].Index == e.ColumnIndex)
                    {
                        string dgvUOM = dbClss.TSt(e.Row.Cells["dgvUOM"].Value);
                        string CodeNo = dbClss.TSt(e.Row.Cells["dgvCodeNo"].Value);
                        //decimal PCSUOM = dbClss.Con_UOM(CodeNo, dgvUOM);
                        e.Row.Cells["dgvPCSUOM"].Value = dbClss.Con_UOM(CodeNo, dgvUOM);
                        e.Row.Cells["dgvPCSUOM"].ReadOnly = false;
                        //using (DataClasses1DataContext db = new DataClasses1DataContext())
                        //{
                        //    var g = (from ix in db.mh_Items select ix)
                        //        .Where(a => a.InternalNo.ToUpper().Trim().Equals(CodeNo.ToUpper().Trim())).ToList();
                        //    if (g.Count > 0)
                        //    {
                        //        if (dgvUOM == dbClss.TSt(g.FirstOrDefault().PurchaseUOM))
                        //        {
                        //            e.Row.Cells["dgvPCSUOM"].Value = dbClss.Con_UOM(CodeNo, dgvUOM);
                        //            //e.Row.Cells["dgvPCSUOM"].ReadOnly = true;
                        //        }
                        //        //else
                        //        //{
                        //        //    e.Row.Cells["dgvPCSUOM"].ReadOnly = false;
                        //        //    e.Row.Cells["dgvPCSUOM"].Value = 0;
                        //        //}
                        //    }
                        //    else
                        //    {
                        //        e.Row.Cells["dgvPCSUOM"].ReadOnly = false;
                        //        e.Row.Cells["dgvPCSUOM"].Value = 0;
                        //    }
                        //}



                    }
                    //else if (dgvData.Columns["dgvGroupCode"].Index == e.ColumnIndex)
                    //{
                    //    string GroupCode = dbClss.TSt(e.Row.Cells["dgvGroupCode"].Value);

                    //    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    //    {
                    //        var g = (from ix in db.mh_GroupTypes select ix)
                    //            .Where(a => a.GroupCode.ToUpper().Trim().Equals(GroupCode.ToUpper().Trim())).ToList();
                    //        if (g.Count > 0)
                    //        {
                    //            e.Row.Cells["dgvGroupName"].Value = dbClss.TSt(g.FirstOrDefault().GroupName);
                    //        }
                    //    }
                    //}

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
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
                
                    List<GridViewRowInfo> dgvRow_List = new List<GridViewRowInfo>();
                    //dgvRow_List.Clear();
                    ListPart_CreatePR MS = new ListPart_CreatePR(dgvRow_List);
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
                    }
                
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
        private void Add_Part(string CodeNo, decimal OrderQty)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                int Row = 0; Row = dgvData.Rows.Count()+1;
                var g = (from ix in db.mh_Items select ix).Where(a => a.InternalNo.Contains(CodeNo)).ToList();
                if (g.Count > 0)
                {

                    //string CodeNo = StockControl.dbClss.TSt(g.FirstOrDefault().ItemNo);
                    string ItemName = dbClss.TSt(g.FirstOrDefault().InternalName);
                    string ItemDesc = StockControl.dbClss.TSt(g.FirstOrDefault().InternalDescription);
                    string UOM = StockControl.dbClss.TSt(g.FirstOrDefault().PurchaseUOM);
                    string GroupCode = StockControl.dbClss.TSt(g.FirstOrDefault().GroupType);
                    //decimal OrderQty = 0;
                    decimal PCSUOM = dbClss.Con_UOM(CodeNo, StockControl.dbClss.TSt(g.FirstOrDefault().PurchaseUOM));
                    bool Sys = true ;
                    
                    decimal Cost = 0;
                    decimal Amount = 0;
                    string VendorNo = StockControl.dbClss.TSt(g.FirstOrDefault().VendorNo);
                    string VendorName = StockControl.dbClss.TSt(g.FirstOrDefault().VendorName);
                    string Remark = "";
                    int id = 0;
                    string Status = "Adding";
                    string VatType = dbClss.TSt(g.FirstOrDefault().VatType);
                    Add_Item(Row, CodeNo, ItemName, ItemDesc, GroupCode
                      , OrderQty, UOM, PCSUOM, Cost, Amount, VendorNo, VendorName, Remark, VatType, id, Status, Sys);
                }
            }
        }
        private void ลบพารทToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
               
                if (dgvData.Rows.Count < 0)
                    return;


                if (Ac.Equals("New") || Ac.Equals("Edit"))
                {
                    this.Cursor = Cursors.WaitCursor;

                    if (StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["dgvStatus"].Value) != "Process"
                        && StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["dgvStatus"].Value) != "Completed")
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
                        SetRowNo1(dgvData);
                    }
                    else
                        MessageBox.Show("สถานะรายการไม่ถูกต้อง ไม่สามารถทำการลบรายการได้");
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
            //try
            //{
            //    using (DataClasses1DataContext db = new DataClasses1DataContext())
            //    {
            //        if (!cboVendorName.Text.Equals(""))
            //        {
            //            var I = (from ix in db.tb_Vendors select ix).Where(a => a.Active == true && a.VendorName.Equals(cboVendorName.Text)).ToList();
            //            if (I.Count > 0)
            //            {
            //                //StockControl.dbClss.TBo(a.Active).Equals(true)
            //                ddlCurrency.Text = I.FirstOrDefault().CRRNCY;
            //                txtAddress.Text = I.FirstOrDefault().Address;
            //                txtVendorNo.Text = I.FirstOrDefault().VendorNo;
            //                var g = (from ix in db.tb_VendorContacts select ix).Where(a => a.VendorNo.Equals(txtVendorNo.Text)).OrderByDescending(b => b.DefaultNo).ToList();
            //                if (g.Count > 0)
            //                {
            //                    txtContactName.Text = g.FirstOrDefault().ContactName;
            //                    txtTel.Text = g.FirstOrDefault().Tel;
            //                    txtFax.Text = g.FirstOrDefault().Fax;
            //                    txtEmail.Text = g.FirstOrDefault().Email;
            //                }
            //                else
            //                {
            //                    txtContactName.Text = "";
            //                    txtTel.Text = "";
            //                    txtFax.Text = "";
            //                    txtEmail.Text = "";
            //                }
            //                ddlCurrency_SelectedIndexChanged(null, null);
            //                txtVendorNo.ReadOnly = true;
            //                ddlCurrency.Enabled = false;
            //                ddlCurrency.BackColor = Color.WhiteSmoke;
            //                txtVendorNo.BackColor = Color.WhiteSmoke;

            //            }
            //            else
            //            {
            //                ddlCurrency.Text = "";
            //                txtRate.Text = "";
            //                txtAddress.Text = "";
            //                txtVendorNo.Text = "";
            //                txtContactName.Text = "";
            //                txtTel.Text = "";
            //                txtFax.Text = "";

            //                txtVendorNo.ReadOnly = false;
            //                ddlCurrency.Enabled = true;
            //                ddlCurrency.BackColor = Color.White;
            //                txtVendorNo.BackColor = Color.White;
            //            }
            //        }

            //    }
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
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
                CreatePR_List sc = new CreatePR_List(txtTempNo);
                this.Cursor = Cursors.Default;
                sc.ShowDialog();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();
                //LoadData
                DataLoad();
                Cal_total();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError("CreatePart", ex.Message + " : radButtonElement1_Click", this.Name); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            btnEdit.Enabled = true;
            btnView.Enabled = false;
            btnNew.Enabled = true;
            
            string TempNo = txtTempNo.Text;
            ClearData();
            Enable_Status(false, "View");
            txtTempNo.Text = TempNo;
            DataLoad();
            Ac = "View";
        }

        private void txtPRNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && !txtPRNo.Text.Equals(""))
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var g = (from ix in db.mh_PurchaseRequests select ix)
                        .Where(a => a.PRNo == txtPRNo.Text.Trim()
                        && (a.Status != "Cancel")
                        ).ToList();
                    if (g.Count() > 0)
                    {
                        txtTempNo.Text = StockControl.dbClss.TSt(g.FirstOrDefault().TEMPNo);
                        btnView_Click(null, null);
                        DataLoad();
                    }
                }

            }
        }
        private void CreatePR_from_WaitingPR()
        {
            //try
            //{
            //    if (RetDT.Count > 0)
            //    {
            //        string CodeNo = "";
            //        this.Cursor = Cursors.WaitCursor;
            //        string VendorNo = "";
            //        foreach (GridViewRowInfo ee in RetDT)
            //        {
            //            VendorNo = Convert.ToString(ee.Cells["VendorNo"].Value).Trim();
            //            if(!VendorNo.Equals(""))
            //            {
            //                using (DataClasses1DataContext db = new DataClasses1DataContext())
            //                {
            //                    var I = (from ix in db.tb_Vendors select ix).Where(a => a.Active == true 
            //                    && a.VendorNo.Equals(VendorNo)).ToList();
            //                    if (I.Count > 0)
            //                    {
            //                        //StockControl.dbClss.TBo(a.Active).Equals(true)
            //                        ddlCurrency.Text = I.FirstOrDefault().CRRNCY;
            //                        txtAddress.Text = I.FirstOrDefault().Address;
            //                        txtVendorNo.Text = I.FirstOrDefault().VendorNo;
            //                        cboVendorName.Text = I.FirstOrDefault().VendorName;
            //                        var g = (from ix in db.tb_VendorContacts select ix).Where(a => a.VendorNo.Equals(txtVendorNo.Text)).OrderByDescending(b => b.DefaultNo).ToList();
            //                        if (g.Count > 0)
            //                        {
            //                            txtContactName.Text = g.FirstOrDefault().ContactName;
            //                            txtTel.Text = g.FirstOrDefault().Tel;
            //                            txtFax.Text = g.FirstOrDefault().Fax;
            //                            txtEmail.Text = g.FirstOrDefault().Email;
                                        
            //                        }
            //                    }
            //                }

            //            }

            //            CodeNo = Convert.ToString(ee.Cells["CodeNo"].Value).Trim();
            //            if (!CodeNo.Equals(""))
            //            {
            //                Add_Part(CodeNo,StockControl.dbClss.TInt(ee.Cells["Order"].Value));

            //            }
                        
            //        }
            //    }
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
            //finally { this.Cursor = Cursors.Default; }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                PrintPR a = new PrintPR(txtPRNo.Text,txtPRNo.Text,"PR");
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

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {

                int Row = 0; Row = dgvData.Rows.Count() + 1;
                string CodeNo = "";
                string ItemName = "";
                string ItemDesc = "";
                string UOM = "PCS";
                string GroupCode = "";
                decimal OrderQty = 0;
                decimal PCSUOM = 1;
                decimal Cost = 0;
                decimal Amount = 0;
                string VendorNo = "";
                string VendorName = "";
                string Remark = "";
                int id = 0;
                string Status = "Adding";
                string VatType = "";
                Add_Item(Row, CodeNo, ItemName,ItemDesc, GroupCode
                    , OrderQty, UOM, PCSUOM, Cost, Amount, VendorNo, VendorName, Remark, VatType, id, Status,false);


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        private void Add_Item(int Row,string CodeNo,string ItemName, string ItemDesc
            , string GroupCode, decimal OrderQty, string UOM, decimal PCSUOM,decimal Cost,decimal Amount
            ,string VendorNo, string VendorName,string Remark,string VatType, int id, string Status,bool Sys)
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

                              
                ee.Cells["dgvNo"].Value =Row.ToString();             
                ee.Cells["dgvCodeNo"].Value = CodeNo;
                ee.Cells["dgvItemName"].Value = ItemName;
                ee.Cells["dgvItemDesc"].Value = ItemDesc;                          
                ee.Cells["dgvGroupCode"].Value = GroupCode;
                ee.Cells["dgvOrderQty"].Value = OrderQty;
                ee.Cells["dgvUOM"].Value = UOM;
                ee.Cells["dgvPCSUOM"].Value = PCSUOM;
                ee.Cells["dgvCost"].Value = Cost;
                ee.Cells["dgvAmount"].Value = 1* Cost;
                ee.Cells["dgvRemark"].Value = "";
                ee.Cells["dgvVendorNo"].Value = VendorNo;
                ee.Cells["dgvVendorName"].Value = VendorName;             
                ee.Cells["dgvid"].Value = id;
                ee.Cells["dgvStatus"].Value = Status;
                ee.Cells["dgvVATType"].Value = VatType;
                //if (!statuss.Equals("Completed") || !statuss.Equals("Process")) //|| (!dbclass.TBo(ApproveFlag) && dbclass.TSt(status) != "Reject"))
                //    dgvData.ReadOnly = false;
                //if (statuss == "Del")
                //    ee.IsVisible = false;


                if (Sys)
                {
                    ee.Cells["dgvCodeNo"].ReadOnly = true;
                    ee.Cells["dgvItemName"].ReadOnly = true;
                    //ee.Cells["dgvUOM"].ReadOnly = true;
                    ee.Cells["dgvItemDesc"].ReadOnly = true;
                    ee.Cells["dgvGroupCode"].ReadOnly = true;
                    //ee.Cells["dgvPCSUOM"].ReadOnly = true;
                }
               
                ////if (lblStatus.Text.Equals("Completed"))//|| lbStatus.Text.Equals("Reject"))
                ////    dgvData.AllowAddNewRow = false;
                ////else
                ////    dgvData.AllowAddNewRow = true;

                ////dbclass.SetRowNo1(dgvData);
            }            
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : Add_Item", this.Name); }

        }

        private void ddlCurrency_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            //try
            //{
            //    using (DataClasses1DataContext db = new DataClasses1DataContext())
            //    {
            //        if (ddlCurrency.Text != "")
            //        {
            //            var g = (from ix in db.tb_CRRNCies select ix)
            //                .Where(a => a.CRRNCY == ddlCurrency.Text.Trim()

            //                ).ToList();
            //            if (g.Count() > 0)
            //            {
            //                txtRate.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Rate);
            //            }
            //            else
            //                txtRate.Text = "";
            //        }
            //        else
            //            txtRate.Text = "";
            //    }
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void ddlFactory_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            //if (ddlFactory.Text == "Factory 1")
            //    txtTempNo.Text = StockControl.dbClss.GetNo(3, 0);
            //else if (ddlFactory.Text == "Factory 2")
            //    txtTempNo.Text = StockControl.dbClss.GetNo(22, 0);
        }

        private void dgvData_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            //try
            //{
            //    GridSpinEditor editor = e.ActiveEditor as GridSpinEditor;
            //    if (editor != null)
            //    {
            //        GridSpinEditorElement element = editor.EditorElement as GridSpinEditorElement;
            //        element.InterceptArrowKeys = false;
            //        element.KeyDown -= new KeyEventHandler(element_KeyDown);
            //        element.KeyDown += new KeyEventHandler(element_KeyDown);
            //    }
            //}
            //catch { }

        }
        private void element_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Up)
            {
                dgvData.GridNavigator.SelectPreviousRow(1);
            }
            if (e.KeyData == Keys.Down)
            {
                dgvData.GridNavigator.SelectNextRow(1);
            }
        }

        private void dgvData_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            try
            {
                if (e.Column.Name.Equals("dgvUOM"))
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
                else if (e.Column.Name.Equals("dgvVendorNo"))
                {
                    /////////////มีการ เคลียร์ การ Add ก่อน แล้วค่อย Add ใหม่////////////////
                    //Row = e.RowIndex;
                    RadMultiColumnComboBoxElement Comcol = (RadMultiColumnComboBoxElement)e.ActiveEditor;
                    Comcol.Columns.Clear();

                    //RadMultiColumnComboBoxElement Comcol = (RadMultiColumnComboBoxElement)e.ActiveEditor;
                    Comcol.DropDownSizingMode = SizingMode.UpDownAndRightBottom;
                    Comcol.DropDownWidth = 350;
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
                        HeaderText = "VendorNo",
                        Name = "No",
                        FieldName = "No",
                        Width = 100,
                        AllowFiltering = true,
                        ReadOnly = false
                    }

                   );
                    Comcol.EditorControl.Columns.Add(new GridViewTextBoxColumn
                    {
                        HeaderText = "VendorName",
                        Name = "Name",
                        FieldName = "Name",
                        Width = 170
                    });

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
                else if (e.Column.Name.Equals("dgvVATType"))
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
                        HeaderText = "VAT Type",
                        Name = "VatType",
                        FieldName = "VatType",
                        Width = 130,
                        AllowFiltering = true,
                        ReadOnly = false
                    }
                   );
                   

                }

            }
            catch { }
        }
    }
    
}
