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
using Telerik.WinControls;
using Telerik.WinControls.Data;

namespace StockControl
{
    public partial class Receive : Telerik.WinControls.UI.RadRibbonForm
    {
        public Receive()
        {
            InitializeComponent();
        }
        public Receive(string RCNo,string PRNo)
        {
            InitializeComponent();
            RCNo_L = RCNo;
            PRNo_L = PRNo;
        }
        string Ac = "";
        string RCNo_L = "";
        string PRNo_L = "";
        DataTable dt_RCH = new DataTable();
        DataTable dt_RCD = new DataTable();
        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            HistoryView hw = new HistoryView(this.Name, txtRCNo.Text);
            this.Cursor = Cursors.Default;
            hw.ShowDialog();
        }

        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }
        private void GETDTRow()
        {
            dt_RCH.Columns.Add(new DataColumn("RCNo", typeof(string)));
            dt_RCH.Columns.Add(new DataColumn("TempNo", typeof(string)));
            dt_RCH.Columns.Add(new DataColumn("Status", typeof(string)));
            dt_RCH.Columns.Add(new DataColumn("VendorNo", typeof(string)));
            dt_RCH.Columns.Add(new DataColumn("VendorName", typeof(string)));
            dt_RCH.Columns.Add(new DataColumn("RemarkHD", typeof(string)));
            dt_RCH.Columns.Add(new DataColumn("InvoiceNo", typeof(string)));
            dt_RCH.Columns.Add(new DataColumn("Type", typeof(string)));          
            dt_RCH.Columns.Add(new DataColumn("RCDate", typeof(DateTime)));
            dt_RCH.Columns.Add(new DataColumn("TypeReceive",typeof(string)));

            dt_RCD.Columns.Add(new DataColumn("CodeNo", typeof(string)));
            dt_RCD.Columns.Add(new DataColumn("ItemNo", typeof(string)));
            dt_RCD.Columns.Add(new DataColumn("ItemDescription", typeof(string)));
            dt_RCD.Columns.Add(new DataColumn("QTY", typeof(decimal)));
            dt_RCD.Columns.Add(new DataColumn("RemainQty", typeof(decimal)));
            dt_RCD.Columns.Add(new DataColumn("Unit", typeof(string)));
            dt_RCD.Columns.Add(new DataColumn("PCSUnit", typeof(decimal)));
            dt_RCD.Columns.Add(new DataColumn("CostPerUnit", typeof(decimal)));
            dt_RCD.Columns.Add(new DataColumn("Amount", typeof(decimal)));
            dt_RCD.Columns.Add(new DataColumn("CRRNCY", typeof(string)));
            dt_RCD.Columns.Add(new DataColumn("LotNo", typeof(string)));
            dt_RCD.Columns.Add(new DataColumn("SerialNo", typeof(string)));
            dt_RCD.Columns.Add(new DataColumn("ShelfNo", typeof(string)));
            dt_RCD.Columns.Add(new DataColumn("Remark", typeof(string)));
            dt_RCD.Columns.Add(new DataColumn("TempNo", typeof(string)));
            dt_RCD.Columns.Add(new DataColumn("PRNo", typeof(string)));
            dt_RCD.Columns.Add(new DataColumn("RCNo", typeof(string)));
            dt_RCD.Columns.Add(new DataColumn("InvoiceNo", typeof(string)));
            dt_RCD.Columns.Add(new DataColumn("ID", typeof(int)));
            dt_RCD.Columns.Add(new DataColumn("PRID", typeof(int)));
            dt_RCD.Columns.Add(new DataColumn("Location", typeof(int)));


        }
        //int crow = 99;
        private void Unit_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                btnNew_Click(null, null);
                dgvData.AutoGenerateColumns = false;
                GETDTRow();

                DefaultItem();

                //Test
                txtDocNo.Text = "SMT1-20180009";
                Insert_data_PR();
                txtDocNo.Text = "";

                if (!RCNo_L.Equals(""))
                {
                    btnNew.Enabled = true;
                    txtRCNo.Text = RCNo_L;
                    txtDocNo.Text = "";
                    DataLoad();
                    Ac = "View";
                }
                else if(!PRNo_L.Equals(""))
                {
                    btnNew.Enabled = true;
                    txtDocNo.Text = PRNo_L;
                    Insert_data_PR();
                    txtDocNo.Text = "";
                }

               
                
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        private void DefaultItem()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                //cboVendor.AutoCompleteMode = AutoCompleteMode.Append;
                //cboVendor.DisplayMember = "VendorName";
                //cboVendor.ValueMember = "VendorNo";
                //cboVendor.DataSource = (from ix in db.tb_Vendors.Where(s => s.Active == true)
                //                        select new { ix.VendorNo,ix.VendorName,ix.CRRNCY }).ToList();
                //cboVendor.SelectedIndex = 0;
                try
                {
                    
                    GridViewMultiComboBoxColumn col = (GridViewMultiComboBoxColumn)dgvData.Columns["Location"];
                    col.DataSource = (from ix in db.mh_Locations.Where(s => Convert.ToBoolean(s.Active.Equals(true)) && s.Active==true)
                                      select new { ix.Code,ix.Name }).ToList();
                    
                    col.DisplayMember = "Code";
                    col.ValueMember = "Code";
                    col.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDown;
                    col.FilteringMode = GridViewFilteringMode.DisplayMember;

                    col.AutoSizeMode = BestFitColumnMode.DisplayedDataCells;
                    col.TextAlignment = ContentAlignment.MiddleCenter;
                    col.DropDownStyle = RadDropDownStyle.DropDownList;
                   
                }
                catch { }
                try
                {
                    GridViewMultiComboBoxColumn Uom = (GridViewMultiComboBoxColumn)dgvData.Columns["Unit"];
                    Uom.DataSource = (from ix in db.mh_Units.Where(s => s.UnitActive == true)
                                      select new { ix.UnitCode }).ToList();
                    Uom.DisplayMember = "UnitCode";
                    Uom.DropDownStyle = RadDropDownStyle.DropDown;
                }
                catch { }
                try
                {
                    var a = (from ix in db.sp_045_ShelfNo_Select("") select ix).ToList();
                    //if (g.Count > 0)
                    //{
                    //GridViewMultiComboBoxColumn col = (GridViewMultiComboBoxColumn)dgvData.Columns["ShelfNo"];

                    //col.DataSource = g;

                    //col.DisplayMember = "ShelfNo";
                    //col.ValueMember = "ShelfNo";
                    //col.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDown;
                    //col.FilteringMode = GridViewFilteringMode.DisplayMember;

                    GridViewComboBoxColumn comboColumn = (GridViewComboBoxColumn)dgvData.Columns["ShelfNo"];

                    List<string> aaa = new List<string>();
                    aaa.AddRange(a.Select(o => o.ShelfNo));
                    comboColumn.DataSource = aaa;
                }
                catch { }



                //this.dgvData.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                //this.dgvData.CellEditorInitialized += radGridView1_CellEditorInitialized;
            }
        }
        private void DataLoad()
        {
            
            dt_RCD.Rows.Clear();
            dt_RCH.Rows.Clear();
            try
            {

                this.Cursor = Cursors.WaitCursor;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {

                    try
                    {
                        var g = (from ix in db.tb_ReceiveHs select ix).Where(a => a.RCNo == txtRCNo.Text.Trim()).ToList();
                        if (g.Count() > 0)
                        {
                            DateTime? temp_date = null;
                            txtVendorNo.Text = StockControl.dbClss.TSt(g.FirstOrDefault().VendorNo);
                            txtVendorName.Text = StockControl.dbClss.TSt(g.FirstOrDefault().VendorName);
                            txtRemark.Text = StockControl.dbClss.TSt(g.FirstOrDefault().RemarkHD);
                            //ddlTypeReceive.Text = StockControl.dbClss.TSt(g.FirstOrDefault().TypeReceive);


                            if (StockControl.dbClss.TSt(g.FirstOrDefault().Type).Equals("รับด้วยใบ Invoice"))
                            {
                                rdoInvoice.IsChecked = true;
                                txtInvoiceNo.Text = StockControl.dbClss.TSt(g.FirstOrDefault().InvoiceNo);
                                dtInvoiceDate.Value = Convert.ToDateTime(g.FirstOrDefault().InvoiceDate, new CultureInfo("en-US"));
                            }
                            else //ใบส่งของชั่วคราว
                            {
                                rdoDL.IsChecked = true;
                                txtDLNo.Text = StockControl.dbClss.TSt(g.FirstOrDefault().InvoiceNo);
                            }
                            if (!StockControl.dbClss.TSt(g.FirstOrDefault().RCDate).Equals(""))
                                dtRequire.Value = Convert.ToDateTime(g.FirstOrDefault().RCDate,new CultureInfo("en-US"));
                            else
                                dtRequire.Value = Convert.ToDateTime(temp_date,new CultureInfo("en-US"));
                           

                            txtReceiveBy.Text = StockControl.dbClss.TSt(g.FirstOrDefault().CreateBy);
                            if(!StockControl.dbClss.TSt(g.FirstOrDefault().UpdateBy).Equals(""))
                                txtReceiveBy.Text = StockControl.dbClss.TSt(g.FirstOrDefault().UpdateBy);
                            if (!StockControl.dbClss.TSt(g.FirstOrDefault().CreateDate).Equals(""))
                            {
                                if(!StockControl.dbClss.TSt(g.FirstOrDefault().UpdateDate).Equals(""))
                                    txtReceiveDate.Text = Convert.ToDateTime(g.FirstOrDefault().UpdateDate).ToString("dd/MMM/yyyy");
                                else
                                    txtReceiveDate.Text = Convert.ToDateTime(g.FirstOrDefault().CreateDate).ToString("dd/MMM/yyyy");
                            }
                            else
                                txtReceiveDate.Text = "";

                            //lblStatus.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Status);
                            if (StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Cancel"))
                            {
                                btnSave.Enabled = false;
                                //btnDelete.Enabled = false;
                                //btnView.Enabled = false;
                                //btnEdit.Enabled = false;
                                lblStatus.Text = "Cancel";
                                dgvData.ReadOnly = true;
                                btnNew.Enabled = true;
                            }
                            else if
                                (StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Partial"))
                            {
                                btnSave.Enabled = false;
                                //btnDelete.Enabled = false;
                                //btnView.Enabled = false;
                                //btnEdit.Enabled = false;
                                lblStatus.Text = "Partial";
                                dgvData.ReadOnly = false;
                                btnNew.Enabled = true;
                            }
                            else if
                                (StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Completed") 
                                || StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Discon")
                                )
                            {
                                btnSave.Enabled = false;
                                //btnDelete.Enabled = false;
                                //btnView.Enabled = false;
                                //btnEdit.Enabled = false;
                                lblStatus.Text = "Completed";
                                dgvData.ReadOnly = true;
                                btnNew.Enabled = true;
                            }
                            else
                            {
                                btnSave.Enabled = true;
                                //btnDelete.Enabled = true;
                                //btnView.Enabled = true;
                                //btnEdit.Enabled = true;
                                lblStatus.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Status);
                                dgvData.ReadOnly = false;
                                btnNew.Enabled = true;
                            }
                            dt_RCH = StockControl.dbClss.LINQToDataTable(g);

                            //Detail
                            var d = (from ix in db.tb_Receives select ix)
                            .Where(a => a.RCNo == txtRCNo.Text.Trim() 
                                && a.Status != "Cancel").ToList();
                            if (d.Count() > 0)
                            {
                                int c = 0;
                                dgvData.DataSource = d;
                                dt_RCD = StockControl.dbClss.LINQToDataTable(d);
                                string SS = "";
                                foreach (var x in dgvData.Rows)
                                {
                                    c += 1;
                                    x.Cells["dgvNo"].Value = c;

                                    if (Convert.ToString(x.Cells["Status"].Value).Equals("Partial"))
                                    {
                                        x.Cells["QTY"].ReadOnly = false;
                                        x.Cells["Unit"].ReadOnly = false;
                                        x.Cells["PCSUnit"].ReadOnly = false;
                                        x.Cells["CostPerUnit"].ReadOnly = false;
                                        x.Cells["Remark"].ReadOnly = false;
                                        x.Cells["ShelfNo"].ReadOnly = false;
                                        x.Cells["Location"].ReadOnly = false;
                                    }
                                    else if (Convert.ToString(x.Cells["Status"].Value).Equals("Completed")
                                        || Convert.ToString(x.Cells["Status"].Value).Equals("Discon"))
                                    {
                                        x.Cells["QTY"].ReadOnly = true;
                                        x.Cells["Unit"].ReadOnly = true;
                                        x.Cells["PCSUnit"].ReadOnly = true;
                                        x.Cells["CostPerUnit"].ReadOnly = true;
                                        x.Cells["Remark"].ReadOnly = true;
                                        x.Cells["ShelfNo"].ReadOnly = true;
                                        x.Cells["Location"].ReadOnly = true;
                                    }
                                }

                                Cal_Amount();
                            }
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
            catch { }
            finally { this.Cursor = Cursors.Default; }


            //    radGridView1.DataSource = dt;
        }
        
        private bool CheckDuplicate(string code, string Code2)
        {
            bool ck = false;

            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                int i = (from ix in db.tb_Models
                         where ix.ModelName == code

                         select ix).Count();
                if (i > 0)
                    ck = false;
                else
                    ck = true;
            }

            return ck;
        }
        private void ClearData()
        {
            txtInvoiceNo.Text = "";
            txtDLNo.Text = "";
            txtDLNo.Enabled = false;
            txtRCNo.Text = "";
            //ddlTypeReceive.Text = "";
            dtRequire.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
            txtReceiveBy.Text = ClassLib.Classlib.User;
            txtReceiveDate.Text = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy");
            txtRemark.Text = "";
            txtVendorName.Text = "";
            txtVendorNo.Text = "";
            txtDocNo.Text = "";
            rdoInvoice.IsChecked = true;
            dtInvoiceDate.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
            dgvData.Rows.Clear();
            dgvData.DataSource = null;
            dt_RCD.Rows.Clear();
            dt_RCH.Rows.Clear();
            txtTotal.Text = "";
        }
        private void Enable_Status(bool ss, string Condition)
        {
            if (Condition.Equals("-") || Condition.Equals("New"))
            {
                txtDocNo.Enabled = ss;
                //txtVendorName.Enabled = ss;
                //txtRCNo.Enabled = ss;
                txtRemark.Enabled = ss;
                //txtTempNo.Enabled = ss;
                dtRequire.Enabled = ss;
                dgvData.ReadOnly = false;
                rdoInvoice.Enabled = ss;
                rdoDL.Enabled = ss;
                //ddlTypeReceive.Enabled = ss;

            }
            else if (Condition.Equals("View"))
            {
                txtDocNo.Enabled = ss;
                //txtVendorName.Enabled = ss;
                //txtRCNo.Enabled = ss;
                txtRemark.Enabled = ss;
                //txtTempNo.Enabled = ss;
                dtRequire.Enabled = ss;
                dgvData.ReadOnly = false;
                rdoInvoice.Enabled = ss;
                rdoDL.Enabled = ss;
            }
            
            else if (Condition.Equals("Edit"))
            {
                txtDocNo.Enabled = ss;
                //txtVendorName.Enabled = ss;
                //txtRCNo.Enabled = ss;
                txtRemark.Enabled = ss;
                //txtTempNo.Enabled = ss;
                dtRequire.Enabled = ss;
                dgvData.ReadOnly = false;
                rdoInvoice.Enabled = ss;
                rdoDL.Enabled = ss;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {

            btnNew.Enabled = false;
            btnSave.Enabled = true;
            ClearData();
            Enable_Status(true, "New");
            lblStatus.Text = "New";
            Ac = "New";

            //getมาไว้ก่อน แต่ยังไมได้ save 
            txtRCNo.Text = StockControl.dbClss.GetNo(4, 0);
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            //radGridView1.ReadOnly = true;
            //btnView.Enabled = false;
            //btnEdit.Enabled = true;
            //radGridView1.AllowAddNewRow = false;
            //DataLoad();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //radGridView1.ReadOnly = false;
            //btnEdit.Enabled = false;
            //btnView.Enabled = true;
            //radGridView1.AllowAddNewRow = false;
            //DataLoad();
        }

        private bool Check_Save()
        {
            bool re = true;
            string err = "";
            try
            {
                //if (txtCodeNo.Text.Equals(""))
                //    err += " “รหัสพาร์ท:” เป็นค่าว่าง \n";
                //if (txtRCNo.Text.Equals(""))
                //    err += " “เลขที่รับสินค้า:” เป็นค่าว่าง \n";
                if (txtVendorNo.Text.Equals(""))
                    err += "- “ผู้ขาย:” เป็นค่าว่าง \n";
                //if (txtVendorNo.Text.Equals(""))
                //    err += "- “รหัสผู้ขาย:” เป็นค่าว่าง \n";
                if (dtRequire.Text.Equals(""))
                    err += "- “วันที่รับสินค้า:” เป็นค่าว่าง \n";
                if (rdoInvoice.IsChecked)
                {
                    if (txtInvoiceNo.Text.Equals(""))
                        err += "- “Invoice No:” เป็นค่าว่าง \n";
                    if(dtInvoiceDate.Text.Equals(""))
                        err += "- “Invoice Date:” เป็นค่าว่าง \n";
                }
                if(rdoDL.IsChecked)
                {
                    if (txtDLNo.Text.Equals(""))
                        err += "- “DL No:” เป็นค่าว่าง \n";
                }
                if (dgvData.Rows.Count <= 0)
                    err += "- “รายการ:” เป็นค่าว่าง \n";
                int c = 0;
                foreach (GridViewRowInfo rowInfo in dgvData.Rows)
                {
                    if (rowInfo.IsVisible)
                    {
                        if (StockControl.dbClss.TInt(rowInfo.Cells["QTY"].Value) != (0))
                        {
                            c += 1;
                            if (StockControl.dbClss.TSt(rowInfo.Cells["PRNo"].Value).Equals(""))
                                err += "- “เลขที่ PR:” เป็นค่าว่าง \n";
                            if (StockControl.dbClss.TSt(rowInfo.Cells["TempNo"].Value).Equals(""))
                                err += "- “เลขที่อ้างอิงเอกสาร PRNo:” เป็นค่าว่าง \n";
                            if (StockControl.dbClss.TSt(rowInfo.Cells["CodeNo"].Value).Equals(""))
                                err += "- “รหัสทูล:” เป็นค่าว่าง \n";
                            if (StockControl.dbClss.TDe(rowInfo.Cells["QTY"].Value) <= 0)
                                err += "- “จำนวนรับ:” น้อยกว่า 0 \n";
                            if (StockControl.dbClss.TDe(rowInfo.Cells["CostPerUnit"].Value) <= 0)
                                err += "- “ราคา/หน่วย:” น้อยกว่า 0 \n";
                            if (StockControl.dbClss.TDe(rowInfo.Cells["Unit"].Value).Equals(""))
                                err += "- “หน่วย:” เป็นค่าว่าง \n";
                            if (StockControl.dbClss.TDe(rowInfo.Cells["PCSUnit"].Value) <= 0)
                                err += "- “จำนวน/หน่วย:” น้อยกว่า 0 \n";
                            if (StockControl.dbClss.TSt(rowInfo.Cells["Location"].Value).Equals(""))
                                err += "- “สถานที่เก็บ:” เป็นค่าว่าง \n";
                            if (StockControl.dbClss.TSt(rowInfo.Cells["ShelfNo"].Value).Equals(""))
                                err += "- “ชึ้นเก็บ Shelf No.:” เป็นค่าว่าง \n";

                        }
                    }
                }

                if (c <= 0)
                    err += "- “กรุณาระบุจำนวนที่จะรับสินค้า:” เป็นค่าว่าง \n";


                if (!err.Equals(""))
                    MessageBox.Show(err);
                else
                    re = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("Receive", ex.Message, this.Name);
            }

            return re;
        }
        private void SaveHerder()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                var g = (from ix in db.tb_ReceiveHs
                         where ix.RCNo.Trim() == txtRCNo.Text.Trim() && ix.Status != "Cancel"
                         //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                         select ix).ToList();
                if (g.Count > 0)  //มีรายการในระบบ
                {
                    foreach (DataRow row in dt_RCH.Rows)
                    {
                        var gg = (from ix in db.tb_ReceiveHs
                                  where ix.RCNo.Trim() == txtRCNo.Text.Trim() && ix.Status != "Cancel"
                                  //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                                  select ix).First();

                        gg.UpdateBy = ClassLib.Classlib.User;
                        gg.UpdateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                        dbClss.AddHistory(this.Name , "แก้ไข Receive", "แก้ไขโดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", txtRCNo.Text.Trim());

                        //if (StockControl.dbClss.TSt(gg.Barcode).Equals(""))
                        //    gg.Barcode = StockControl.dbClss.SaveQRCode2D(txtRCNo.Text.Trim());

                        if (!txtVendorNo.Text.Trim().Equals(row["VendorNo"].ToString()))
                        {
                            gg.VendorName = txtVendorName.Text;
                            gg.VendorNo = txtVendorNo.Text.Trim();
                            dbClss.AddHistory(this.Name , "แก้ไข Receive", "แก้ไขรหัสผู้ขาย [" + txtVendorNo.Text.Trim() + " เดิม :" + row["VendorNo"].ToString() + "]", txtRCNo.Text.Trim());
                        }
                        //if (!ddlTypeReceive.Text.Trim().Equals(row["TypeReceive"].ToString()))
                        //{
                        gg.TypeReceive = "PO";
                        //    dbClss.AddHistory(this.Name, "แก้ไข Receive", "แก้ไขประเภทรับ[" + ddlTypeReceive.Text.Trim() + " เดิม :" + row["TypeReceive"].ToString() + "]", txtRCNo.Text.Trim());
                        //}
                        if (!txtInvoiceNo.Text.Trim().Equals(row["InvoiceNo"].ToString()))
                        {
                            gg.InvoiceDate = Convert.ToDateTime(dtInvoiceDate.Value, new CultureInfo("en-US"));
                            gg.InvoiceNo = txtInvoiceNo.Text.Trim();
                            gg.TempNo = txtInvoiceNo.Text.Trim();
                            dbClss.AddHistory(this.Name , "แก้ไข Receive", "แก้ไขInvoice No[" + txtInvoiceNo.Text.Trim() + " เดิม :" + row["InvoiceNo"].ToString() + "]", txtRCNo.Text.Trim());
                        }
                        if (!txtDLNo.Text.Trim().Equals(row["TempNo"].ToString()))
                        {
                            gg.TempNo = txtDLNo.Text.Trim();
                            gg.InvoiceNo = txtDLNo.Text.Trim();
                            dbClss.AddHistory(this.Name, "แก้ไข Receive", "แก้ไข DL No[" + txtDLNo.Text.Trim() + " เดิม :" + row["TempNo"].ToString() + "]", txtRCNo.Text.Trim());
                        }
                        if (!txtRemark.Text.Trim().Equals(row["RemarkHD"].ToString()))
                        {
                            gg.RemarkHD = txtRemark.Text.Trim();
                            dbClss.AddHistory(this.Name , "แก้ไข Receive", "แก้ไขหมายเหตุ [" + txtRemark.Text.Trim() + " เดิม :" + row["RemarkHD"].ToString() + "]", txtRCNo.Text.Trim());
                        }
                        string Type = "";
                        if (rdoInvoice.IsChecked)
                            Type = "รับด้วยใบ Invoice";
                        else
                            Type = "ใบส่งของชั่วคราว";

                        if (!Type.Equals(row["Type"].ToString()))
                        {
                            gg.Type = Type;
                            dbClss.AddHistory(this.Name, "แก้ไข Receive", "แก้ไขประเภทการรับสินค้า [" + Type + " เดิม :" + row["Type"].ToString() + "]", txtRCNo.Text.Trim());
                        }

                        if (!dtRequire.Text.Trim().Equals(""))
                        {
                            string date1 = "";
                            date1 = dtRequire.Value.ToString("yyyyMMdd", new CultureInfo("en-US"));
                            string date2 = "";
                            DateTime temp = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")); 
                            if (!StockControl.dbClss.TSt(row["RCDate"].ToString()).Equals(""))
                            {
                                
                                temp = Convert.ToDateTime(row["RCDate"]);
                                date2 = temp.ToString("yyyyMMdd", new CultureInfo("en-US"));
                            }
                            if (!date1.Equals(date2))
                            {
                                DateTime? RequireDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                if (!dtRequire.Text.Equals(""))
                                    RequireDate = dtRequire.Value;
                                gg.RCDate = RequireDate;
                                dbClss.AddHistory(this.Name, "แก้ไข Receive", "แก้ไขวันที่รับสินค้า [" + dtRequire.Text.Trim() + " เดิม :" + temp.ToString() + "]", txtRCNo.Text.Trim());
                            }
                        }
                        db.SubmitChanges();
                    }
                }
                else //สร้างใหม่
                {
                    byte[] barcode = null;
                        //barcode = StockControl.dbClss.SaveQRCode2D(txtRCNo.Text.Trim());
                    DateTime? UpdateDate = null;

                    DateTime? RequireDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    if (!dtRequire.Text.Equals(""))
                        RequireDate = dtRequire.Value;

                    tb_ReceiveH gg = new tb_ReceiveH();
                    gg.RCNo = txtRCNo.Text;
                    gg.RCDate = RequireDate;
                    gg.UpdateBy = null;
                    gg.UpdateDate = UpdateDate;
                    gg.CreateBy = ClassLib.Classlib.User;
                    gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    gg.VendorName = txtVendorName.Text;
                    gg.VendorNo = txtVendorNo.Text.Trim();
                    gg.RemarkHD = txtRemark.Text;
                    gg.TypeReceive = "PO";
                    string Type = "";
                    if (rdoInvoice.IsChecked)
                    {
                        Type = "รับด้วยใบ Invoice";
                        gg.InvoiceNo = txtInvoiceNo.Text;
                        gg.Flag_Temp = false;
                        gg.InvoiceDate = Convert.ToDateTime(dtInvoiceDate.Value, new CultureInfo("en-US"));
                    }
                    else
                    {
                        gg.InvoiceNo = txtDLNo.Text;
                        gg.TempNo = txtDLNo.Text;
                        Type = "ใบส่งของชั่วคราว";
                        gg.Flag_Temp = true;
                    }
                    
                    gg.Type = Type;
                    gg.Barcode = barcode;
                    gg.Status = Cal_Status();
                    db.tb_ReceiveHs.InsertOnSubmit(gg);
                    db.SubmitChanges();

                    dbClss.AddHistory(this.Name , "สร้าง Receive", "สร้าง Receive [" + txtRCNo.Text.Trim() + ",เลขที่ Invoice No or DL No: " + txtInvoiceNo.Text + "]", txtRCNo.Text.Trim());
                }
            }
        }
        private string Cal_Status()
        {
            string re = "Completed";
          
            decimal QTY = 0;
            decimal RemainQty = 0;
            foreach (var rd1 in dgvData.Rows)
            {
                QTY = StockControl.dbClss.TDe(rd1.Cells["QTY"].Value);
                if (QTY != 0)
                {
                    RemainQty = StockControl.dbClss.TDe(rd1.Cells["RemainQty"].Value);
                    if (QTY < RemainQty)
                    {
                        re = "Partial";
                        break;
                    }
                }
               
            }
            return re;
        }
        private void SaveDetail()
        {
            dgvData.EndEdit();
            
                string RCNo = txtRCNo.Text;
                DateTime? RequireDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
            if (!dtRequire.Text.Equals(""))
                    RequireDate = dtRequire.Value;
                int Seq = 0;
                DateTime? UpdateDate = null;
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                foreach (var g in dgvData.Rows)
                {
                    string SS = "";
                    string LotNo = "";
                    if (g.IsVisible.Equals(true))
                    {
                        if (StockControl.dbClss.TInt(g.Cells["QTY"].Value) != (0)) // เอาเฉพาะรายการที่ไม่เป็น 0 
                        {
                            if (StockControl.dbClss.TInt(g.Cells["ID"].Value) <= 0)  //New ใหม่
                            {
                                string Temp = "";
                                string PRNo = ""; ;
                                int PRID = 0;
                                decimal RemainQty = 0;
                                LotNo = StockControl.dbClss.TSt(g.Cells["LotNo"].Value);
                                if(LotNo=="")
                                {
                                    LotNo = dbClss.Get_Lot(DateTime.Now.ToString("yyyyMMdd"));
                                }
                                Seq += 1;
                                tb_Receive u = new tb_Receive();
                                u.PRNo = StockControl.dbClss.TSt(g.Cells["PRNo"].Value);
                                u.TempNo = StockControl.dbClss.TSt(g.Cells["TempNo"].Value);
                                u.CodeNo = StockControl.dbClss.TSt(g.Cells["CodeNo"].Value);
                                u.ItemNo = StockControl.dbClss.TSt(g.Cells["ItemNo"].Value);
                                u.ItemDescription = StockControl.dbClss.TSt(g.Cells["ItemDescription"].Value);
                                u.RemainQty = StockControl.dbClss.TDe(g.Cells["RemainQty"].Value) -
                                                StockControl.dbClss.TDe(g.Cells["QTY"].Value);

                                u.QTY = StockControl.dbClss.TDe(g.Cells["QTY"].Value);
                                u.PCSUnit = StockControl.dbClss.TDe(g.Cells["PCSUnit"].Value);
                                u.Unit = StockControl.dbClss.TSt(g.Cells["Unit"].Value);
                                u.Remark = StockControl.dbClss.TSt(g.Cells["Remark"].Value);
                                u.LotNo = LotNo; //StockControl.dbClss.TSt(g.Cells["LotNo"].Value);
                                u.SerialNo = StockControl.dbClss.TSt(g.Cells["SerialNo"].Value);
                                u.CRRNCY = StockControl.dbClss.TSt(g.Cells["CRRNCY"].Value);
                                u.RCNo = RCNo;                               
                                u.PRID = StockControl.dbClss.TInt(g.Cells["PRID"].Value);
                                u.ShelfNo = StockControl.dbClss.TSt(g.Cells["ShelfNo"].Value);
                                u.TypeReceive = StockControl.dbClss.TSt(g.Cells["TypeReceive"].Value);
                                u.Location = StockControl.dbClss.TSt(g.Cells["Location"].Value);
                                u.Rate = dbClss.TDe(g.Cells["Rate"].Value);
                                u.idCSTMPODt = dbClss.TInt(g.Cells["idCSTMPODt"].Value);

                                if (rdoDL.IsChecked)
                                {
                                    u.InvoiceNo = txtDLNo.Text;
                                    u.TempInvNo = txtDLNo.Text;
                                    u.CostPerUnit = 0;
                                    u.Amount = 0;
                                }
                                else if (rdoInvoice.IsChecked)
                                {
                                    u.InvoiceNo = txtInvoiceNo.Text;
                                    u.Amount = StockControl.dbClss.TDe(g.Cells["Amount"].Value);
                                    u.CostPerUnit = StockControl.dbClss.TDe(g.Cells["CostPerUnit"].Value);
                                }
                                string BaseUOM = "";
                                decimal BasePCSUnit = 1;
                                    var be = (from ix in db.mh_Items select ix)
                                        .Where(a => a.InternalNo.Trim().ToUpper().Equals(StockControl.dbClss.TSt(g.Cells["CodeNo"].Value).Trim().ToUpper())                                    

                                        ).ToList();
                                if (be.Count > 0)
                                {
                                    var ins = (from ix in db.mh_Items select ix)
                                        .Where(a => a.InternalNo.Trim().ToUpper().Equals(StockControl.dbClss.TSt(g.Cells["CodeNo"].Value).Trim().ToUpper())
                                        ).FirstOrDefault();
                                    ins.StandardCost = dbClss.TDe(u.CostPerUnit);

                                    BaseUOM = dbClss.TSt(be.FirstOrDefault().BaseUOM);
                                    BasePCSUnit = dbClss.Con_UOM((StockControl.dbClss.TSt(g.Cells["CodeNo"].Value).Trim().ToUpper()), BaseUOM);
                                }
                                if (BaseUOM == "" || BasePCSUnit <= 0)
                                {
                                    BasePCSUnit = 1;
                                    BaseUOM = "PCS";
                                }

                                u.PCSUnit_Base = BasePCSUnit;
                                u.BaseUOM = BaseUOM;
                                u.RCDate = RequireDate;
                                u.Seq = Seq;

                                if (u.RemainQty > 0)
                                    u.Status = "Partial";
                                else
                                {
                                    SS = "Completed";
                                    u.Status = "Completed";
                                }
                                u.CreateBy = ClassLib.Classlib.User;
                                u.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                u.UpdateBy = null;
                                u.CreateDate = UpdateDate;

                                Temp = u.TempNo;
                                PRNo = u.PRNo;
                                PRID = StockControl.dbClss.TInt(g.Cells["PRID"].Value);
                                RemainQty = Convert.ToDecimal(u.RemainQty);
                                
                                db.tb_Receives.InsertOnSubmit(u);
                                db.SubmitChanges();

                                //// update Remainใน tb_receive ที่เป็น PRID เดียวกัน ทั้งหมด
                                update_remainqty_Receive(PRNo, Temp, PRID, RemainQty);

                                ////หมายถึงรับสินค้าครบหมดแล้ว ให้ไป เปลี่ยนสถาะ เป็น Completed ทุกตัวที่เป็น PRID เดียวกัน
                                if (!SS.Equals(""))
                                    Save_Status_Receive(PRNo, Temp, PRID, RemainQty);

                                //C += 1;
                                dbClss.AddHistory(this.Name , "เพิ่ม Receive", "เพิ่มรายการ Receive [" + u.CodeNo + " จำนวนรับ :" + u.QTY.ToString() +" หน่วย :"+u.Unit + " จำนวนคงเหลือ :" + u.RemainQty.ToString() + "]", txtRCNo.Text.Trim());
                                
                            }
                               
                        }
                    }
                }
            }
        }
       
        private int Get_RCid(string TypeReceive,int RefPRPO_id,string RefPRPO)
        {
            int re = 0;
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                var v = (from ix in db.tb_Receives
                         where ix.PRID == RefPRPO_id
                              && ix.PRNo == RefPRPO
                              && StockControl.dbClss.TSt( ix.TypeReceive) == TypeReceive
                              && ix.Status  != "Cancel"
                              && ix.RCNo == txtRCNo.Text
                         select ix).OrderByDescending(a => a.ID).ToList();
                if (v.Count > 0)
                {
                    //update รายการใน PR
                    var p = (from ix in db.tb_Receives
                             where ix.PRID == RefPRPO_id
                                  && ix.PRNo == RefPRPO
                                  && StockControl.dbClss.TSt(ix.TypeReceive) == TypeReceive
                                  && ix.Status != "Cancel"
                                  && ix.RCNo == txtRCNo.Text
                             select ix).OrderByDescending(a => a.ID).First();
                    {
                        re = StockControl.dbClss.TInt(p.ID);
                    }
                }
            }
             return re;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Ac.Equals("New"))// || Ac.Equals("Edit"))
                {
                    if (Check_Save())
                        return;

                    string Crr = "";
                    foreach (var g in dgvData.Rows)
                    {
                        Crr= StockControl.dbClss.TSt(g.Cells["CRRNCY"].Value);
                        if (Crr != "THB")
                            break;
                    }
                    if(Crr !="THB")
                    {
                        if (MessageBox.Show("บางรายการเป็นเงินต่างประเทศ คุณเปลี่ยนจำนวนเงินเป็น Rate THB แล้ว ต้องการทำรายการต่อใช่หรือไม่ ?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                        }
                        else
                            return;
                    }
                    if (MessageBox.Show("ต้องการบันทึก ?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.Cursor = Cursors.WaitCursor;

                        if (Ac.Equals("New"))
                            txtRCNo.Text = StockControl.dbClss.GetNo(4, 2);

                        if (!txtRCNo.Text.Equals(""))
                        {
                            SaveHerder();
                            SaveDetail();

                            MessageBox.Show("บันทึกสำเร็จ!");

                            DataLoad();
                            btnNew.Enabled = true;
                            txtDocNo.Enabled = false;

                            //insert Stock1
                            //Insert_Stock1();

                            //insert stock
                            InsertStock_new_Receive();

                            //if (ddlTypeReceive.Text == "PR")
                            //    InsertStock_new_Ship();
                            //else if (ddlTypeReceive.Text == "PO")  // กรณีที่ Item นั่นไม่มีในระบบให้ Ship ออกทันที                            
                            InsertStock_new_Ship_Other_PO();


                            //ปรับ Status PR and PO และ Remain
                            Change_StatusPR_PO();
                            
                        }
                        else
                        {
                            MessageBox.Show("ไม่สามารถโหลดเลขที่รับสินค้าได้ ติดต่อแผนก IT");
                        }
                    }
                }
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        //private void Insert_Stock1()
        //{
        //    try
        //    {
                
        //        using (DataClasses1DataContext db = new DataClasses1DataContext())
        //        {
        //            DateTime? CalDate = null;
        //            DateTime? AppDate = DateTime.Now;
        //            int Seq = 0;
        //            string Type = "";
        //            if (rdoInvoice.IsChecked)
        //                Type = "รับด้วยใบ Invoice";
        //            else
        //                Type = "ใบส่งของชั่วคราว";

        //            decimal Cost = 0;


        //            string CNNo = CNNo = StockControl.dbClss.GetNo(6, 2);
        //            var g = (from ix in db.tb_Receives
        //                         //join i in db.tb_Items on ix.CodeNo equals i.CodeNo
        //                     where ix.RCNo.Trim() == txtRCNo.Text.Trim() && ix.Status != "Cancel"
                             
        //                     select ix).ToList();
        //            if (g.Count > 0)
        //            {
        //                //insert Stock

        //                foreach (var vv in g)
        //                {
        //                    Seq += 1;

        //                    tb_Stock1 gg = new tb_Stock1();
        //                    gg.AppDate = AppDate;
        //                    gg.Seq = Seq;
        //                    gg.App = "Receive";
        //                    gg.Appid = Seq;
        //                    gg.CreateBy = ClassLib.Classlib.User;
        //                    gg.CreateDate = DateTime.Now;
        //                    gg.DocNo = CNNo;
        //                    gg.RefNo = txtRCNo.Text;
        //                    gg.Type = Type;
        //                    gg.QTY = Convert.ToDecimal(vv.QTY);
        //                    gg.Inbound = Convert.ToDecimal(vv.QTY);
        //                    gg.Outbound = 0;

        //                    if(rdoDL.IsChecked)
        //                    {
        //                        gg.UnitCost = 0;
        //                        gg.AmountCost = 0;
        //                    }
        //                    else
        //                    {
        //                        gg.AmountCost = Convert.ToDecimal(vv.QTY) * Convert.ToDecimal(vv.CostPerUnit);
        //                        gg.UnitCost = Convert.ToDecimal(vv.CostPerUnit);
        //                    }
        //                    gg.RemainQty = 0;
        //                    gg.RemainUnitCost = 0;
        //                    gg.RemainAmount = 0;
        //                    gg.CalDate = CalDate;
        //                    gg.Status = "Active";

                           
        //                    db.tb_Stock1s.InsertOnSubmit(gg);
        //                    db.SubmitChanges();

        //                    //tb_Items inv,DL
        //                    dbClss.Insert_Stock(vv.CodeNo, Convert.ToDecimal(vv.QTY), "Receive", "Inv");
        //                    //tb_Items temp
        //                    dbClss.Insert_StockTemp(vv.CodeNo, (Convert.ToDecimal(vv.QTY)), "RC_Temp", "Inv");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex) { MessageBox.Show(ex.Message); }
        //}
        private void InsertStock_new_Receive()
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    DateTime? CalDate = null;
                    DateTime? AppDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    int Seq = 0;
                    string Type = "";
                    string Category = "";
                    int Flag_ClearTemp = 0;
                    string Type_in_out = "In";
                    decimal RemainQty = 0;
                    decimal Amount = 0;
                    decimal RemainAmount = 0;
                    decimal RemainUnitCost = 0;
                    //decimal Avg = 0;
                    decimal UnitCost = 0;
                    decimal sum_Remain = 0;
                    decimal sum_Qty = 0;
                    decimal BasePCSUnit = 0;
                    if (rdoInvoice.IsChecked)
                    {
                        Category = "Invoice";
                        Type = "รับด้วยใบ Invoice";
                        Flag_ClearTemp = 0;
                    }
                    else
                    {
                        Category = "Temp";
                        Type = "ใบส่งของชั่วคราว";
                        Flag_ClearTemp = 1;
                    }
                  
                    var g = (from ix in db.tb_Receives
                                 //join i in db.tb_Items on ix.CodeNo equals i.CodeNo
                             where ix.RCNo.Trim() == txtRCNo.Text.Trim() && ix.Status != "Cancel"

                             select ix).ToList();
                    if (g.Count > 0)
                    {
                        //insert Stock
                        foreach (var vv in g)
                        {
                            Seq += 1;
                            BasePCSUnit = dbClss.TDe(vv.PCSUnit_Base);
                            if (rdoDL.IsChecked)
                            {
                                Amount = 0;
                                UnitCost = 0;
                            }
                            else
                            {
                                // ไมได้คุณ BasePCSUnit
                                //Amount = Math.Round(( Convert.ToDecimal(vv.QTY) * Convert.ToDecimal(vv.CostPerUnit)),2);
                                //UnitCost = Math.Round((Amount/ (Convert.ToDecimal(vv.QTY) * Convert.ToDecimal(vv.PCSUnit))),2);

                                Amount = Math.Round((Convert.ToDecimal(vv.QTY) * Convert.ToDecimal(vv.CostPerUnit)), 2);
                                UnitCost = Math.Round((Amount / (Convert.ToDecimal(vv.QTY) * Convert.ToDecimal(vv.PCSUnit) * BasePCSUnit )), 2);

                            }
                            //แบบที่ 1 จะไป sum ใหม่
                            RemainQty = (Convert.ToDecimal(db.Cal_QTY_Remain_Location(vv.CodeNo, "", 0, vv.Location, dbClss.TInt(vv.idCSTMPODt))));
                            //แบบที่ 2 จะไปดึงล่าสุดมา
                            //RemainQty = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainQty"));
                            sum_Remain = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainAmount",vv.Location,dbClss.TInt(vv.idCSTMPODt)))
                                + Amount;

                            sum_Qty = RemainQty + Math.Round(((Convert.ToDecimal(vv.QTY)* Convert.ToDecimal(vv.PCSUnit) * BasePCSUnit)),2);
                            ////Avg = sum_Remain / sum_Qty;
                            //if (sum_Qty > 0)
                            //    Avg = sum_Remain / sum_Qty;
                            //else
                            //    Avg = 0;
                            ////RemainAmount = sum_Qty * Avg;
                            ////Avg = UnitCost;//sum_Remain / sum_Qty;
                            RemainAmount = sum_Remain;
                            if (sum_Qty <= 0)
                                RemainUnitCost = 0;
                            else
                                RemainUnitCost = Math.Round(( Math.Abs(RemainAmount) / Math.Abs(sum_Qty)),2);

                            tb_Stock gg = new tb_Stock();
                            gg.AppDate = AppDate;
                            gg.Seq = Seq;
                            gg.App = "Receive";
                            gg.Appid = Seq;
                            gg.CreateBy = ClassLib.Classlib.User;
                            gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                            gg.DocNo = txtRCNo.Text;
                            gg.RefNo =vv.PRNo;
                            gg.CodeNo = vv.CodeNo;
                            gg.Type = Type;
                            gg.QTY = Math.Round(( Convert.ToDecimal(vv.QTY)* Convert.ToDecimal(vv.PCSUnit) * BasePCSUnit), 2);
                            gg.Inbound = Math.Round(( Convert.ToDecimal(vv.QTY) * Convert.ToDecimal(vv.PCSUnit) * BasePCSUnit), 2);
                            gg.Outbound = 0;
                            gg.Type_i = 1;  //Receive = 1,Cancel Receive 2,Shipping = 3,Cancel Shipping = 4,Adjust stock = 5,ClearTemp = 6
                            gg.Category = Category;
                            gg.Refid = vv.ID;
                            gg.Type_in_out = Type_in_out;
                            gg.AmountCost = Amount;
                            gg.UnitCost = UnitCost;
                            gg.RemainQty = sum_Qty;
                            gg.RemainUnitCost = RemainUnitCost;
                            gg.RemainAmount = RemainAmount;
                            gg.Avg = 0;// Avg;
                            gg.CalDate = CalDate;
                            gg.Status = "Active";
                            gg.Flag_ClearTemp = Flag_ClearTemp;   //0 คือ invoice,1 คือ Temp , 2 คือ clear temp แล้ว
                            gg.TLCost = Amount;
                            gg.TLQty = Math.Round(( Convert.ToDecimal(vv.QTY) * Convert.ToDecimal(vv.PCSUnit) * BasePCSUnit),2);
                            gg.ShipQty = 0;
                            gg.Location = vv.Location;
                            gg.ShelfNo = vv.ShelfNo;
                            gg.LotNo = vv.LotNo;
                            gg.idCSTMPODt = vv.idCSTMPODt;

                            //ต้องไม่ใช่ Item ที่มีในระบบ
                            var c = (from ix in db.mh_Items
                                     where ix.InternalNo.Trim().ToUpper() == vv.CodeNo.Trim().ToUpper() && ix.Active ==true
                                     select ix).ToList();
                            if (c.Count <= 0)
                            {
                                gg.TLQty = 0;
                                gg.ShipQty = Math.Round(( Convert.ToDecimal(vv.QTY) * Convert.ToDecimal(vv.PCSUnit) * BasePCSUnit),2);
                            }

                            db.tb_Stocks.InsertOnSubmit(gg);
                            db.SubmitChanges();

                            //update Stock เข้า item
                            db.sp_010_Update_StockItem(Convert.ToString(vv.CodeNo), "");
                            
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void InsertStock_new_Ship()
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    string SHNo = "";// StockControl.dbClss.GetNo(5, 2);
                    DateTime? CalDate = null;
                    DateTime? AppDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    int Seq = 0;
                    string Type = "Shipping";
                    string Category = "";
                    int Flag_ClearTemp = 0;
                    string Type_in_out = "Out";
                    decimal RemainQty = 0;
                    decimal Amount = 0;
                    decimal RemainAmount = 0;
                    decimal Avg = 0;
                    decimal UnitCost = 0;
                    decimal sum_Remain = 0;
                    decimal sum_Qty = 0;
                    decimal RemainUnitCost = 0;
                    decimal QTY = 0;
                    if (rdoInvoice.IsChecked)
                    {
                        Category = "Invoice";
                        //Type = "รับด้วยใบ Invoice";
                        Flag_ClearTemp = 0;
                    }
                    else
                    {
                        Category = "Temp";
                        //Type = "ใบส่งของชั่วคราว";
                        Flag_ClearTemp = 1;
                    }

                    var g = (from ix in db.tb_Receives
                                 //join i in db.mh_Items on ix.CodeNo equals i.CodeNo
                             where ix.RCNo.Trim() == txtRCNo.Text.Trim() && ix.Status != "Cancel"

                             select ix).ToList();
                    if (g.Count > 0)
                    {
                        //insert Stock
                        foreach (var vv in g)
                        {
                            //ต้องไม่ใช่ Item ที่มีในระบบ
                            var c = (from ix in db.mh_Items
                                     where ix.InternalNo.Trim().ToUpper() == vv.CodeNo.Trim().ToUpper() && ix.Active ==true
                                     select ix).ToList();
                            if (c.Count <= 0)
                            {
                                if (SHNo == "")
                                {
                                    SHNo = StockControl.dbClss.GetNo(5, 2);
                                    Ship_HD(SHNo, ClassLib.Classlib.User, "", "");
                                }

                                Seq += 1;
                                QTY = -Convert.ToDecimal(vv.QTY);

                                if (rdoDL.IsChecked)
                                {
                                    Amount = 0;
                                    UnitCost = 0;
                                }
                                else
                                {
                                    Amount = QTY * Convert.ToDecimal(vv.CostPerUnit);
                                    //UnitCost = Convert.ToDecimal(vv.CostPerUnit);
                                    UnitCost = Math.Round((Amount / (Convert.ToDecimal(vv.QTY) * Convert.ToDecimal(vv.PCSUnit))), 2);

                                }

                                Amount = (QTY) * UnitCost;

                                //แบบที่ 1 จะไป sum ใหม่
                                RemainQty = (Convert.ToDecimal(db.Cal_QTY_Remain_Location(vv.CodeNo, "", 0,vv.Location,dbClss.TInt(vv.idCSTMPODt))));
                                //แบบที่ 2 จะไปดึงล่าสุดมา
                                //RemainQty = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainQty"));
                                sum_Remain = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainAmount",vv.Location,dbClss.TInt(vv.idCSTMPODt)))
                                    + Amount;

                                sum_Qty = RemainQty + (QTY * Convert.ToDecimal(vv.PCSUnit));
                                //Avg = sum_Remain / sum_Qty;                            
                                //RemainAmount = sum_Qty * Avg;
                                Avg = UnitCost;//sum_Remain / sum_Qty;
                                RemainAmount = sum_Remain;
                                if (sum_Qty <= 0)
                                    RemainUnitCost = 0;
                                else
                                    RemainUnitCost = Math.Round((Math.Abs(RemainAmount) / Math.Abs(sum_Qty)), 2);


                                //-----------------------------------
                                tb_Shipping u = new tb_Shipping();
                                u.ShippingNo = SHNo;
                                u.CodeNo = vv.CodeNo;
                                u.ItemNo = vv.ItemNo;
                                u.ItemDescription = vv.ItemDescription;
                                u.QTY = StockControl.dbClss.TDe(vv.QTY) * Convert.ToDecimal(vv.PCSUnit);
                                u.PCSUnit = StockControl.dbClss.TDe(vv.PCSUnit);
                                u.UnitShip = StockControl.dbClss.TSt(vv.Unit);
                                u.Remark = vv.Remark;
                                u.LotNo = vv.LotNo;
                                u.SerialNo = vv.SerialNo;
                                u.MachineName = "";
                                u.LineName = "";
                                u.Calbit = false;
                                u.ClearFlag = false;
                                u.ClearDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                u.Seq = Seq;
                                u.Status = "Completed";
                                u.UnitCost = UnitCost;
                                u.idCSTMPODt = vv.idCSTMPODt;
                                db.tb_Shippings.InsertOnSubmit(u);
                                db.SubmitChanges();

                                //-----------------------------------

                                tb_Stock gg = new tb_Stock();
                                gg.AppDate = AppDate;
                                gg.Seq = Seq;
                                gg.App = "Shipping";
                                gg.Appid = Seq;
                                gg.CreateBy = ClassLib.Classlib.User;
                                gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                gg.DocNo = SHNo;
                                gg.RefNo = vv.PRNo;
                                gg.CodeNo = vv.CodeNo;
                                gg.Type = Type;
                                gg.QTY = QTY * Convert.ToDecimal(vv.PCSUnit);
                                gg.Inbound = 0;
                                gg.Outbound = QTY * Convert.ToDecimal(vv.PCSUnit);
                                gg.Type_i = 3;  //Receive = 1,Cancel Receive 2,Shipping = 3,Cancel Shipping = 4,Adjust stock = 5,ClearTemp = 6
                                gg.Category = Category;
                                gg.Refid = vv.ID;
                                
                                gg.CalDate = CalDate;
                                gg.Status = "Active";
                                gg.Flag_ClearTemp = Flag_ClearTemp; //0 คือ invoice,1 คือ Temp , 2 คือ clear temp แล้ว
                                gg.Type_in_out = Type_in_out;
                                gg.AmountCost = Amount;
                                gg.UnitCost = UnitCost;
                                gg.RemainQty = sum_Qty;
                                gg.RemainUnitCost = RemainUnitCost;
                                gg.RemainAmount = RemainAmount;
                                gg.Avg = 0;//Avg;
                                gg.TLCost = Math.Abs(Amount);
                                gg.TLQty = 0;
                                gg.ShipQty = Math.Abs(QTY) * Convert.ToDecimal(vv.PCSUnit);
                                gg.RefShipid = 0;
                                gg.Location = vv.Location;
                                gg.ShelfNo = vv.ShelfNo;
                                gg.idCSTMPODt = vv.idCSTMPODt;

                                var ab = (from ix in db.tb_Shippings
                                         where ix.ShippingNo.Trim().ToUpper() == SHNo.Trim().ToUpper() && ix.Status != "Cancel"
                                            && ix.CodeNo.Trim().ToUpper() == vv.CodeNo.Trim().ToUpper()
                                         select ix).OrderByDescending(aa => aa.id).ToList();
                                if (ab.Count > 0)
                                {
                                    gg.RefShipid = Convert.ToInt16(ab.FirstOrDefault().id);
                                }

                                    db.tb_Stocks.InsertOnSubmit(gg);
                                db.SubmitChanges();
                                

                                dbClss.AddHistory(this.Name, "เบิกสินค้า", " เบิกสินค้าเลขที่ : " + SHNo + " เบิก : " + Category + " CodeNo : " + vv.CodeNo + " จำนวน : " + (QTY).ToString() + " โดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", SHNo);
                                
                                //update Stock เข้า item
                                db.sp_010_Update_StockItem(Convert.ToString(vv.CodeNo), "");
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void InsertStock_new_Ship_Other_PO()
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    string SHNo = "";
                    DateTime? CalDate = null;
                    DateTime? AppDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    int Seq = 0;
                    string Type = "Shipping";
                    string Category = "";
                    int Flag_ClearTemp = 0;
                    string Type_in_out = "Out";
                    decimal RemainQty = 0;
                    decimal Amount = 0;
                    decimal RemainAmount = 0;
                    decimal Avg = 0;
                    decimal UnitCost = 0;
                    decimal sum_Remain = 0;
                    decimal sum_Qty = 0;
                    decimal RemainUnitCost = 0;
                    decimal QTY = 0;
                    decimal BasePCSUnit = 1;
                    if (rdoInvoice.IsChecked)
                    {
                        Category = "Invoice";
                        //Type = "รับด้วยใบ Invoice";
                        Flag_ClearTemp = 0;
                    }
                    else
                    {
                        Category = "Temp";
                        //Type = "ใบส่งของชั่วคราว";
                        Flag_ClearTemp = 1;
                    }

                    var g = (from ix in db.tb_Receives
                                 //join i in db.mh_Items on ix.CodeNo equals i.CodeNo
                             where ix.RCNo.Trim() == txtRCNo.Text.Trim() && ix.Status != "Cancel"

                             select ix).ToList();
                    if (g.Count > 0)
                    {
                        //insert Stock
                        foreach (var vv in g)
                        {
                            //ต้องไม่ใช่ Item ที่มีในระบบ
                            BasePCSUnit = dbClss.TDe(vv.PCSUnit_Base);
                            var c = (from ix in db.mh_Items
                                     where ix.InternalNo.Trim().ToUpper() == vv.CodeNo.Trim().ToUpper() && ix.Active==true
                                     select ix).ToList();
                            if (c.Count <= 0)
                            {
                                if (SHNo == "")
                                {
                                    SHNo = StockControl.dbClss.GetNo(5, 2);
                                    Ship_HD(SHNo, ClassLib.Classlib.User, "", "");
                                }                               
                                
                                Seq += 1;
                                QTY = -Convert.ToDecimal(vv.QTY);

                                if (rdoDL.IsChecked)
                                {
                                    Amount = 0;
                                    UnitCost = 0;
                                }
                                else
                                {
                                    Amount = QTY * Convert.ToDecimal(vv.CostPerUnit);
                                    //UnitCost = Convert.ToDecimal(vv.CostPerUnit);
                                    UnitCost = Math.Round((Amount / (Convert.ToDecimal(vv.QTY) * Convert.ToDecimal(vv.PCSUnit) * BasePCSUnit)), 2);

                                }

                                Amount = (QTY) * UnitCost ;
                                //แบบที่ 1 จะไป sum ใหม่
                                RemainQty = (Convert.ToDecimal(db.Cal_QTY_Remain_Location(vv.CodeNo, "", 0,vv.Location,dbClss.TInt(vv.idCSTMPODt))));
                                //แบบที่ 2 จะไปดึงล่าสุดมา
                                //RemainQty = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainQty"));
                                sum_Remain = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainAmount",vv.Location,dbClss.TInt(vv.idCSTMPODt)))
                                    + Amount;

                                sum_Qty = RemainQty + Math.Round(((QTY*dbClss.TDe(vv.PCSUnit) * BasePCSUnit)),2);
                                //Avg = sum_Remain / sum_Qty;
                                //RemainAmount = sum_Qty * Avg;
                                Avg = UnitCost;
                                RemainAmount = sum_Remain;
                                if (sum_Qty <= 0)
                                    RemainUnitCost = 0;
                                else
                                    RemainUnitCost = Math.Round((Math.Abs(RemainAmount) / Math.Abs(sum_Qty)), 2);

                                //-----------------------------------
                                tb_Shipping u = new tb_Shipping();
                                u.ShippingNo = SHNo;
                                u.CodeNo = vv.CodeNo;
                                u.ItemNo = vv.ItemNo;
                                u.ItemDescription = vv.ItemDescription;
                                u.QTY =Math.Round(( StockControl.dbClss.TDe(vv.QTY) * dbClss.TDe(vv.PCSUnit) * BasePCSUnit),2);
                                u.PCSUnit = StockControl.dbClss.TDe(vv.PCSUnit);
                                u.UnitShip = StockControl.dbClss.TSt(vv.Unit);
                                u.Remark = vv.Remark;
                                u.LotNo = vv.LotNo;
                                u.SerialNo = vv.SerialNo;
                                u.MachineName = "";
                                u.LineName = "";
                                u.Calbit = false;
                                u.ClearFlag = false;
                                u.ClearDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                u.Seq = Seq;
                                u.Status = "Completed";
                                u.UnitCost = UnitCost;
                                u.Location = vv.Location;
                                u.BasePCSUOM = vv.PCSUnit_Base;
                                u.BaseUOM = vv.BaseUOM;
                                u.idCSTMPODt = vv.idCSTMPODt;

                                db.tb_Shippings.InsertOnSubmit(u);
                                db.SubmitChanges();

                                //-----------------------------------
                                tb_Stock gg = new tb_Stock();
                                gg.AppDate = AppDate;
                                gg.Seq = Seq;
                                gg.App = "Shipping";
                                gg.Appid = Seq;
                                gg.CreateBy = ClassLib.Classlib.User;
                                gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                gg.DocNo = SHNo;
                                gg.RefNo = vv.PRNo;
                                gg.CodeNo = vv.CodeNo;
                                gg.Type = Type;
                                gg.QTY = Math.Round((QTY * dbClss.TDe(vv.PCSUnit) * BasePCSUnit),2);
                                gg.Inbound = 0;
                                gg.Outbound = Math.Round((QTY * dbClss.TDe(vv.PCSUnit) * BasePCSUnit),2);
                                gg.Type_i = 3;  //Receive = 1,Cancel Receive 2,Shipping = 3,Cancel Shipping = 4,Adjust stock = 5,ClearTemp = 6
                                gg.Category = Category;
                                gg.Refid = vv.ID;

                                gg.CalDate = CalDate;
                                gg.Status = "Active";
                                gg.Flag_ClearTemp = Flag_ClearTemp; //0 คือ invoice,1 คือ Temp , 2 คือ clear temp แล้ว
                                gg.Type_in_out = Type_in_out;
                                gg.AmountCost = Amount;
                                gg.UnitCost = UnitCost;
                                gg.RemainQty = sum_Qty;
                                gg.RemainUnitCost = 0;
                                gg.RemainAmount = 0;//RemainAmount;
                                gg.Avg = 0;//Avg;
                                gg.TLQty = 0;
                                gg.TLCost = Math.Abs(Amount);
                                gg.ShipQty = Math.Round((Math.Abs(QTY) * dbClss.TDe(vv.PCSUnit) * BasePCSUnit),2);
                                gg.RefShipid = 0;
                                gg.Location = vv.Location;
                                gg.ShelfNo = vv.ShelfNo;
                                gg.idCSTMPODt = vv.idCSTMPODt;

                                var ab = (from ix in db.tb_Shippings
                                          where ix.ShippingNo.Trim().ToUpper() == SHNo.Trim().ToUpper() && ix.Status != "Cancel"
                                             && ix.CodeNo.Trim().ToUpper() == vv.CodeNo.Trim().ToUpper()
                                          select ix).OrderByDescending(aa => aa.id).ToList();
                                if (ab.Count > 0)
                                {
                                    gg.RefShipid = Convert.ToInt16(ab.FirstOrDefault().id);
                                }

                                db.tb_Stocks.InsertOnSubmit(gg);
                                db.SubmitChanges();

                                dbClss.AddHistory(this.Name, "เบิกสินค้า", " เบิกสินค้าเลขที่ : " + SHNo + " เบิก : " + Category + " CodeNo : " + vv.CodeNo + " จำนวน : " + (QTY).ToString() + " โดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", SHNo);

                                //update Stock เข้า item
                                db.sp_010_Update_StockItem(Convert.ToString(vv.CodeNo), "");
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void Ship_HD(string SHNo,string ShipName,string Jobcard,string TempJobCard)
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    byte[] barcode = StockControl.dbClss.SaveQRCode2D(SHNo);
                    DateTime? UpdateDate = null;

                    DateTime? RequireDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    if (!dtRequire.Text.Equals(""))
                        RequireDate = dtRequire.Value;

                    tb_ShippingH gg = new tb_ShippingH();
                    gg.ShippingNo = SHNo;
                    gg.ShipDate = RequireDate;
                    gg.UpdateBy = null;
                    gg.UpdateDate = UpdateDate;
                    gg.CreateBy = ClassLib.Classlib.User;
                    gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    gg.ShipName = ShipName;
                    gg.Remark = txtRemark.Text;
                    gg.JobCard = Jobcard;
                    gg.TempJobCard = TempJobCard;
                    gg.BarCode = barcode;
                    gg.Status = "Completed";
                    gg.Remark = "ShipOther";
                    db.tb_ShippingHs.InsertOnSubmit(gg);
                    db.SubmitChanges();

                    dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "สร้าง การเบิกสินค้า [" + SHNo + "]", SHNo);
                }
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void Change_StatusPR_PO()
        {
            try
            {

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {


                    var g = (from ix in db.tb_Receives
                                 //join i in db.tb_Items on ix.CodeNo equals i.CodeNo
                             where ix.RCNo.Trim() == txtRCNo.Text.Trim() && ix.Status != "Cancel"

                             select ix).ToList();
                    if (g.Count > 0)
                    {
                        //insert Stock

                        foreach (var vv in g)
                        {
                            //if (ddlTypeReceive.Text == "PR")
                            //{
                            //    var v = (from ix in db.tb_PurchaseRequestLines
                            //             where ix.id == StockControl.dbClss.TInt(vv.PRID)
                            //                && ix.SS != 0
                            //             select ix).ToList();

                            //    if (v.Count > 0)
                            //    {
                            //        //update รายการใน PR
                            //        var p = (from ix in db.tb_PurchaseRequestLines
                            //                 where ix.id == StockControl.dbClss.TInt(vv.PRID)
                            //                    && ix.SS != 0
                            //                 select ix).First();

                            //        if (StockControl.dbClss.TDe(vv.QTY) > StockControl.dbClss.TDe(p.RemainQty))
                            //            p.RemainQty = 0;
                            //        else
                            //            p.RemainQty = StockControl.dbClss.TDe(p.RemainQty) - StockControl.dbClss.TDe(vv.QTY);

                            //        p.Status = "Completed";
                            //        p.RCNo = txtRCNo.Text;
                            //        p.RefRCid = vv.ID;// StockControl.dbClss.TInt(Get_RCid(ddlTypeReceive.Text, p.id, p.PRNo));

                            //        dbClss.AddHistory(this.Name, "รับรายการสินค้า Receive", "ID :" + StockControl.dbClss.TSt(vv.ID)
                            //              + " CodeNo :" + StockControl.dbClss.TSt(vv.CodeNo)
                            //              + " แก้ไขโดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", StockControl.dbClss.TSt(vv.PRNo));

                            //        db.SubmitChanges();

                            //        //ถ้าเป็น PR ให้รับเข้าแล้ว Ship ออกทันที

                            //        //update Stock เข้า item
                            //        db.sp_010_Update_StockItem(Convert.ToString(p.CodeNo), "");

                            //        //Update status pr herder
                            //        db.sp_023_PRHD_Cal_Status(p.TempNo, p.PRNo);
                            //    }
                            //}
                            //else if (ddlTypeReceive.Text == "PO")
                            {
                                var v = (from ix in db.mh_PurchaseOrderDetails
                                         where ix.id == StockControl.dbClss.TInt(vv.PRID)
                                            && ix.SS != 0
                                         select ix).ToList();
                                if (v.Count > 0)
                                {
                                    //update รายการใน PR
                                    var p = (from ix in db.mh_PurchaseOrderDetails
                                             where ix.id == StockControl.dbClss.TInt(vv.PRID)
                                                && ix.SS != 0
                                             select ix).First();
                                    if (StockControl.dbClss.TDe(vv.QTY) > StockControl.dbClss.TDe(p.BackOrder))
                                        p.BackOrder = 0;
                                    else
                                        p.BackOrder = StockControl.dbClss.TDe(p.BackOrder) - StockControl.dbClss.TDe(vv.QTY);

                                    dbClss.AddHistory(this.Name, "รับรายการสินค้า Receive", "ID :" + StockControl.dbClss.TSt(vv.ID)
                                          + " CodeNo :" + StockControl.dbClss.TSt(vv.CodeNo)
                                          + " แก้ไขโดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", StockControl.dbClss.TSt(vv.PRNo));

                                    p.RefRCNo = txtRCNo.Text;
                                    p.RefRCid = vv.ID;//StockControl.dbClss.TInt(Get_RCid(ddlTypeReceive.Text, p.id, p.PRNo));

                                    db.SubmitChanges();

                                    //update Stock เข้า item
                                    db.sp_010_Update_StockItem(Convert.ToString(p.CodeNo), "");

                                    //Update status pr herder
                                    db.sp_022_POHD_Cal_Status(p.TempPNo, p.PONo);
                                }
                            }


                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void update_remainqty_Receive(string PRNo, string TempNo, int PRID, decimal RemainQty)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                //var u = (from ix in db.tb_Receives
                //         where
                //              //ix.TempNo == TempNo
                //              ix.PRNo == PRNo
                //             && ix.PRID == PRID
                //         select ix).ToList();

                //if (u.Count > 0)
                //{
                //    foreach (var gg in u)
                //    {
                //       gg.RemainQty = RemainQty;

                //        db.SubmitChanges();
                //        //dbClss.AddHistory(this.Name + txtRCNo.Text, "รับสินค้าครบแล้ว Receive", "รับสินค้าครบแล้ว Receive [ PRNo : " + u.PRNo + " TempNo : " + TempNo + " PRID : " + PRID.ToString() + "]", "");
                //    }
                //    //dbClss.AddHistory(this.Name + txtRCNo.Text, "รับสินค้าครบแล้ว Receive", "รับสินค้าครบแล้ว Receive [ PRNo : " + PRNo + " TempNo : " + TempNo + " PRID : " + PRID.ToString() + "]", "");
                //}
                db.sp_003_Cal_Receive_Eemain(PRID, PRNo, TempNo, RemainQty);

            }
        }
        private void Save_Status_Receive(string PRNo,string TempNo,int PRID,decimal RemainQty)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.sp_004_Update_Receive_Remain(PRID, PRNo, TempNo, RemainQty);

                //string RCNo = "";
                //var e = (from ix in db.tb_Receives
                //         where
                //              //ix.TempNo == TempNo
                //              //ix.PRNo == PRNo
                //              ix.PRID == PRID
                //         select ix).ToList();

                //if (e.Count > 0)
                //{
                //    RCNo = Convert.ToString(e.FirstOrDefault().RCNo);

                //    foreach (var gg in e)
                //    {
                //        gg.Status = "Completed";
                //        gg.RemainQty = 0;

                //        db.SubmitChanges();
                //        //dbClss.AddHistory(this.Name + txtRCNo.Text, "รับสินค้าครบแล้ว Receive", "รับสินค้าครบแล้ว Receive [ PRNo : " + u.PRNo + " TempNo : " + TempNo + " PRID : " + PRID.ToString() + "]", "");
                //    }
                //    dbClss.AddHistory(this.Name + txtRCNo.Text, "รับสินค้าครบแล้ว Receive", "รับสินค้าครบแล้ว Receive [ PRNo : " + PRNo + " TempNo : " + TempNo + " PRID : " + PRID.ToString() + "]", "");
                //}

                ////เช็คและ update ทั้ง RC
                //var r = (from ix in db.tb_Receives
                //         where
                //              ix.RCNo == RCNo
                //              && ix.RemainQty ==0
                //         select ix).ToList();
                //if(r.Count>0)
                //{
                //    foreach (var gg in r)
                //    {
                //        gg.Status = "Completed";
                //        gg.RemainQty = 0;
                //        db.SubmitChanges();
                //        //dbClss.AddHistory(this.Name + txtRCNo.Text, "รับสินค้าครบแล้ว Receive", "รับสินค้าครบแล้ว Receive [ PRNo : " + u.PRNo + " TempNo : " + TempNo + " PRID : " + PRID.ToString() + "]", "");
                //    }

                //    var h = (from ix in db.tb_ReceiveHs
                //             where
                //                  ix.RCNo == RCNo
                //              select ix).First();
                //    h.Status = "Completed";
                //    db.SubmitChanges();

                //}


            }
        }
        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                /*gvData.Rows[e.RowIndex].Cells["dgvC"].Value = "T";*/
                dgvData.EndEdit();
                if (e.RowIndex >= -1)
                {

                    if (dgvData.Columns["QTY"].Index == e.ColumnIndex)
                    {
                        decimal QTY = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["QTY"].Value), out QTY);
                        decimal RemainQty = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["RemainQty"].Value), out RemainQty);
                        if (QTY > RemainQty)
                        {
                            MessageBox.Show("ไม่สามารถรับเกินจำนวนคงเหลือได้");
                            e.Row.Cells["QTY"].Value = 0;
                        }
                    }
                    else if (dgvData.Columns["ShelfNo"].Index == e.ColumnIndex)
                    {
                        var cc = e.Row.Cells["ShelfNo"];
                        string CategoryTemp = Convert.ToString(e.Row.Cells["ShelfNo"].Value);
                        try
                        {
                            if (!CategoryTemp.Equals(ShelfNo_Edit) && !ShelfNo_Edit.Equals(""))
                            {
                                (e.Row.Cells["ShelfNo"].Value) = ShelfNo_Edit;
                                ShelfNo_Edit = "";
                            }
                        }
                        catch { }
                    }
                    else if (dgvData.Columns["Unit"].Index == e.ColumnIndex)
                    {
                        string dgvUOM = dbClss.TSt(e.Row.Cells["Unit"].Value);
                        string CodeNo = dbClss.TSt(e.Row.Cells["CodeNo"].Value);
                        e.Row.Cells["PCSUnit"].Value = dbClss.Con_UOM(CodeNo, dgvUOM);
                        ////decimal PCSUOM = 0;//dbClss.Con_UOM(CodeNo, dgvUOM);
                        //using (DataClasses1DataContext db = new DataClasses1DataContext())
                        //{
                        //    var g = (from ix in db.mh_Items select ix)
                        //        .Where(a => a.InternalNo.ToUpper().Trim().Equals(CodeNo.ToUpper().Trim())).ToList();
                        //    if (g.Count > 0)
                        //    {
                        //        if (dgvUOM == dbClss.TSt(g.FirstOrDefault().ConsumptionUOM))
                        //        {
                        //            e.Row.Cells["PCSUnit"].Value = dbClss.Con_UOM(CodeNo, dgvUOM);
                        //            //e.Row.Cells["dgvPCSUnit"].ReadOnly = true;
                        //        }
                        //        else
                        //        {
                        //            e.Row.Cells["PCSUnit"].ReadOnly = false;
                        //            e.Row.Cells["PCSUnit"].Value = 0;
                        //        }
                        //    }
                        //    //else
                        //    //{
                        //    //    e.Row.Cells["PCSUnit"].ReadOnly = false;
                        //    //    e.Row.Cells["PCSUnit"].Value = 0;
                        //    //}
                        //}
                    }
                    if (dgvData.Columns["QTY"].Index == e.ColumnIndex
                        || dgvData.Columns["CostPerUnit"].Index == e.ColumnIndex
                        )
                    {
                        decimal QTY = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["QTY"].Value), out QTY);
                        decimal CostPerUnit = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["CostPerUnit"].Value), out CostPerUnit);
                        //decimal Rate = 1; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["Rate"].Value), out Rate);
                        //if (Rate <= 0)
                        //    Rate = 1;
                        //CostPerUnit = CostPerUnit * Rate;
                        e.Row.Cells["Amount"].Value = QTY * CostPerUnit;
                        Cal_Amount();
                    }
                   
                }
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
            }
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

        private void txtModelName_TextChanged(object sender, EventArgs e)
        {
            DataLoad();
        }

      
        private void radLabel5_Click(object sender, EventArgs e)
        {

        }

        private void radLabel2_Click(object sender, EventArgs e)
        {

        }

        private void radTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPRNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if(e.KeyChar == 13)
                {
                    

                    //if (ddlTypeReceive.Text =="PR")
                    //    Insert_data_PR();
                    //else if(ddlTypeReceive.Text=="PO")
                        Insert_data_PO();

                    //if (dgvData.Rows.Count() > 0)
                    //    ddlTypeReceive.Enabled = false;
                    //else
                    //    ddlTypeReceive.Enabled = true;

                    txtDocNo.Text = "";
                    
                }
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void Insert_data_PR()
        {
            //if (!txtDocNo.Text.Equals(""))
            //{
            //    using (DataClasses1DataContext db = new DataClasses1DataContext())
            //    {

            //        var p = (from ix in db.tb_PurchaseRequestLines select ix)
            //                   .Where(a => a.PRNo == txtDocNo.Text.Trim() && a.SS == 1
            //                   && (Convert.ToDecimal(a.RemainQty) > Convert.ToDecimal(0.00))
            //                   && ((a.PoNo) == "" || a.PoNo == null)
            //                   && ((a.RefPOid) == 0 || a.RefPOid == null)
                               
            //                   ).ToList();
            //        if(p.Count <=0)
            //        {
            //            MessageBox.Show("ไม่พบรายการ");
            //            return;
            //        }

            //        int No = 0;
            //        string CodeNo = "";
            //        string ItemNo = "";
            //        string ItemDescription = "";
            //        decimal QTY = 0;
            //        decimal RemainQty = 0;
            //        string Unit = "";
            //        decimal PCSUnit = 0;
            //        decimal CostPerUnit = 0;
            //        decimal Amount = 0;
            //        string CRRNCY = "";
            //        string LotNo = "";
            //        string SerialNo = "";
            //        string Remark = "";
            //        string PRNo = "";
            //        string RCNo = "";
            //        string TempNo = "";
            //        string InvoiceNo = "";
            //        if (rdoInvoice.IsChecked)
            //            InvoiceNo = txtInvoiceNo.Text;
            //        else
            //            InvoiceNo = txtDLNo.Text;

            //        int duppicate_vendor = 0;
            //        string Status = "Waiting";
            //        int ID = 0;
            //        int PRID = 0;
            //        string ShelfNo = "";
            //        string Location = "";

            //        var g = (from ix in db.tb_PurchaseRequests select ix)
            //            .Where(a => a.PRNo == txtDocNo.Text.Trim()
            //            && (Convert.ToString(a.Status) != "Cancel")
            //            ).ToList();
            //        if (g.Count() > 0)
            //        {
            //            if (txtVendorNo.Text.Equals(""))
            //            {
            //                txtVendorName.Text = StockControl.dbClss.TSt(g.FirstOrDefault().VendorName);
            //                txtVendorNo.Text = StockControl.dbClss.TSt(g.FirstOrDefault().VendorNo);
            //                //txtTempNo.Text = StockControl.dbClss.TSt(g.FirstOrDefault().TEMPNo);
                            
            //            }
            //            else
            //            {
            //                if (!txtVendorNo.Text.Equals(StockControl.dbClss.TSt(g.FirstOrDefault().VendorNo)))
            //                {
            //                    MessageBox.Show("ไม่สามารถรับสินค้าต่างผู้ขายได้");
            //                    duppicate_vendor = 1;
            //                }
            //            }

            //            CRRNCY = StockControl.dbClss.TSt(g.FirstOrDefault().CRRNCY);

            //            if (duppicate_vendor <=0)
            //            {
                            

            //                var d = (from ix in db.tb_PurchaseRequestLines select ix)
            //                    .Where(a => a.PRNo == txtDocNo.Text.Trim() && a.SS == 1 
            //                    && (Convert.ToDecimal(a.RemainQty) > Convert.ToDecimal(0.00))
            //                    && (Convert.ToString(a.PoNo) == "" || a.PoNo == null)
            //                    && (Convert.ToInt16(a.RefPOid) == 0 || a.RefPOid == null)
            //                    )
                                
            //                    .ToList();
            //                if (d.Count() > 0)

            //                {
            //                    foreach (var gg in d)
            //                    {
            //                        No = dgvData.Rows.Count() + 1;

            //                        CodeNo = StockControl.dbClss.TSt(gg.CodeNo);
            //                        if (!check_Duppicate(CodeNo))
            //                        {
            //                            TempNo = StockControl.dbClss.TSt(gg.TempNo);

            //                            ItemNo = StockControl.dbClss.TSt(gg.ItemName);
            //                            ItemDescription = StockControl.dbClss.TSt(gg.ItemDesc);
            //                            QTY = 0;//StockControl.dbClss.TDe(gg.OrderQty);
            //                                    //RemainQty ต้อง Cal ใหม่ ว่ารับเข้าทั้งหมดเท่าแล้วค่อยเอามาหักลบกัน
            //                            RemainQty = StockControl.dbClss.TDe(gg.RemainQty);
            //                            Unit = StockControl.dbClss.TSt(gg.UnitCode);
            //                            // จำนวนต่อหน่วย
            //                            PCSUnit = StockControl.dbClss.TDe(gg.PCSUnit);
            //                            // ราคาต่อหน่วย
            //                            CostPerUnit = StockControl.dbClss.TDe(gg.StandardCost);
            //                            if (rdoDL.IsChecked)
            //                                CostPerUnit = 0;
            //                            Amount = QTY * CostPerUnit;
            //                            //CRRNCY = CRRNCY;  //มาจาก herder
            //                            LotNo = StockControl.dbClss.TSt(gg.LotNo);
            //                            SerialNo = StockControl.dbClss.TSt(gg.SerialNo);
            //                            Remark = "";
            //                            PRNo = txtDocNo.Text;
            //                            RCNo = "";
            //                            PRID = StockControl.dbClss.TInt(gg.id);

            //                            if (StockControl.dbClss.TDe(gg.OrderQty)
            //                                    == StockControl.dbClss.TDe(gg.RemainQty))
            //                                Status = "Waiting";
            //                            else
            //                                Status = "Partial";


            //                            //dgvData.Rows.Add(No.ToString(), Status, CodeNo, ItemNo, ItemDescription, QTY, RemainQty, Unit
            //                            //    , PCSUnit, CostPerUnit, Amount, CRRNCY, LotNo, SerialNo, ShelfNo, Remark, TempNo, PRNo, RCNo, InvoiceNo
            //                            //    , ID.ToString(), PRID.ToString(),ddlTypeReceive.Text
            //                            //    );

            //                            var l = (from ix in db.tb_Items select ix)
            //                                    .Where(a => a.CodeNo == CodeNo.Trim()
            //                                    && (Convert.ToString(a.Status) != "Cancel")
            //                                    ).ToList();
            //                            if(l.Count>0)
            //                            {
            //                                Location = dbClss.TSt(l.FirstOrDefault().Location);
            //                                ShelfNo = dbClss.TSt(l.FirstOrDefault().ShelfNo);
            //                            }

            //                            Add_Item(No.ToString(), Status, CodeNo, ItemNo
            //                               , ItemDescription, QTY, RemainQty, Unit
            //                               , PCSUnit, CostPerUnit, Amount, CRRNCY, LotNo
            //                               , SerialNo, ShelfNo, Location, Remark, TempNo, PRNo, RCNo, InvoiceNo
            //                               , ID.ToString(), PRID.ToString(), ddlTypeReceive.Text
            //                               );

            //                        }
            //                    }
            //                }
            //                Cal_Amount();
            //            }
            //            duppicate_vendor = 0;
            //        }
    
            //    }
            //}
        }
        private void Insert_data_PO()
        {
            if (!txtDocNo.Text.Equals(""))
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {

                    var p = (from ix in db.mh_PurchaseOrderDetails select ix)
                               .Where
                               (a => a.PONo == txtDocNo.Text.Trim() && a.SS == 1
                               && (Convert.ToDecimal(a.BackOrder) > Convert.ToDecimal(0.00))
                              
                               ).ToList();
                    if (p.Count <= 0)
                    {
                        MessageBox.Show("ไม่พบรายการ");
                        return;
                    }

                    int No = 0;
                    string CodeNo = "";
                    string ItemNo = "";
                    string ItemDescription = "";
                    decimal QTY = 0;
                    decimal RemainQty = 0;
                    string Unit = "";
                    decimal PCSUnit = 0;
                    decimal CostPerUnit = 0;
                    decimal Amount = 0;
                    string CRRNCY = "";
                    string LotNo = "";
                    string SerialNo = "";
                    string Remark = "";
                    string PRNo = "";
                    string RCNo = "";
                    string TempNo = "";
                    string InvoiceNo = "";
                    if (rdoInvoice.IsChecked)
                        InvoiceNo = txtInvoiceNo.Text;
                    else
                        InvoiceNo = txtDLNo.Text;

                    int duppicate_vendor = 0;
                    string Status = "Waiting";
                    int ID = 0;
                    int PRID = 0;
                    string ShelfNo = "";
                    string Location = "";
                    decimal Rate = 1;
                    int idCSTMPODt = 0;



                    var g = (from ix in db.mh_PurchaseOrders select ix)
                        .Where(a => a.PONo == txtDocNo.Text.Trim()
                        && a.Status != "Cancel"
                        && Convert.ToInt16(a.SeqStatus)==2
                        ).ToList();
                    if (g.Count() > 0)
                    {
                        if (txtVendorNo.Text.Equals(""))
                        {
                            txtVendorName.Text = StockControl.dbClss.TSt(g.FirstOrDefault().VendorName);
                            txtVendorNo.Text = StockControl.dbClss.TSt(g.FirstOrDefault().VendorNo);
                            //txtTempNo.Text = StockControl.dbClss.TSt(g.FirstOrDefault().TEMPNo);

                        }
                        else
                        {
                            if (!txtVendorNo.Text.Equals(StockControl.dbClss.TSt(g.FirstOrDefault().VendorNo)))
                            {
                                MessageBox.Show("ไม่สามารถรับสินค้าต่างผู้ขายได้");
                                duppicate_vendor = 1;
                            }
                        }

                        CRRNCY = StockControl.dbClss.TSt(g.FirstOrDefault().CRRNCY);

                        if (duppicate_vendor <= 0)
                        {


                            var d = (from ix in db.mh_PurchaseOrderDetails select ix)
                                .Where(a => a.PONo == txtDocNo.Text.Trim() && a.SS == 1
                                && (Convert.ToDecimal(a.BackOrder) > Convert.ToDecimal(0.00))
                                )

                                .ToList();
                            if (d.Count() > 0)

                            {
                                foreach (var gg in d)
                                {
                                    No = dgvData.Rows.Count() + 1;

                                    CodeNo = StockControl.dbClss.TSt(gg.CodeNo);
                                    if (!check_Duppicate(gg.id))
                                    {
                                        TempNo = StockControl.dbClss.TSt(gg.TempPNo);

                                        ItemNo = StockControl.dbClss.TSt(gg.ItemName);
                                        ItemDescription = StockControl.dbClss.TSt(gg.ItemDesc);
                                        QTY = 0;//StockControl.dbClss.TDe(gg.OrderQty);
                                                //RemainQty ต้อง Cal ใหม่ ว่ารับเข้าทั้งหมดเท่าแล้วค่อยเอามาหักลบกัน
                                        RemainQty = StockControl.dbClss.TDe(gg.BackOrder);
                                        Unit = StockControl.dbClss.TSt(gg.Unit);
                                        // จำนวนต่อหน่วย
                                        PCSUnit = StockControl.dbClss.TDe(gg.PCSUnit);
                                        // ราคาต่อหน่วย
                                        CostPerUnit = StockControl.dbClss.TDe(gg.CHUnitCost);
                                        if (CostPerUnit <= 0)
                                            CostPerUnit = StockControl.dbClss.TDe(gg.Cost);
                                        //if (rdoDL.IsChecked)
                                        //    CostPerUnit = 0;
                                        //Rate = dbClss.TDe(gg.Rate);
                                        //CostPerUnit = Math.Round((Rate * CostPerUnit),2);

                                         Amount = Math.Round((QTY * CostPerUnit), 2);
                                        //CRRNCY = CRRNCY;  //มาจาก herder
                                        LotNo = StockControl.dbClss.TSt(gg.LotNo);
                                        SerialNo = StockControl.dbClss.TSt(gg.SerialNo);
                                        Remark = "";
                                        PRNo = txtDocNo.Text;
                                        RCNo = "";
                                        PRID = StockControl.dbClss.TInt(gg.id);
                                        idCSTMPODt = dbClss.TInt(gg.idCSTMPODt);

                                        if (StockControl.dbClss.TDe(gg.OrderQty)
                                                == StockControl.dbClss.TDe(gg.BackOrder))
                                            Status = "Waiting";
                                        else
                                            Status = "Partial";


                                        //dgvData.Rows.Add(No.ToString(), Status, CodeNo, ItemNo
                                        //    , ItemDescription, QTY, RemainQty, Unit
                                        //    , PCSUnit, CostPerUnit, Amount, CRRNCY, LotNo
                                        //    , SerialNo, ShelfNo, Location, Remark, TempNo, PRNo, RCNo, InvoiceNo
                                        //    , ID.ToString(), PRID.ToString(),ddlTypeReceive.Text
                                        //    );
                                        var l = (from ix in db.mh_Items select ix)
                                                .Where(a => a.InternalNo == CodeNo.Trim()
                                                && (a.Active == true)
                                                ).ToList();
                                        if (l.Count > 0)
                                        {
                                            Location = dbClss.TSt(l.FirstOrDefault().Location);
                                            ShelfNo = dbClss.TSt(l.FirstOrDefault().ShelfNo);
                                        }
                                        Add_Item(No.ToString(), Status, CodeNo, ItemNo
                                           , ItemDescription, QTY, RemainQty, Unit
                                           , PCSUnit, CostPerUnit, Amount, CRRNCY, LotNo
                                           , SerialNo, ShelfNo, Location, Remark, TempNo, PRNo, RCNo, InvoiceNo
                                           , ID.ToString(), PRID.ToString(), ""//ddlTypeReceive.Text
                                           ,Rate
                                           , idCSTMPODt
                                           );
                                    }
                                }
                            }
                            Cal_Amount();
                        }
                        duppicate_vendor = 0;
                    }
                    else
                        MessageBox.Show("ไม่พบรายการ เช็คสถานะ");
                }
            }
        }
        private void Add_Item(string No, string Status, string CodeNo,string ItemNo
           , string ItemDescription, decimal QTY, decimal RemainQty, string Unit, decimal PCSUnit
          , decimal CostPerUnit, decimal Amount, string CRRNCY, string LotNo, string SerialNo
          ,string ShelfNo,string Location, string Remark,string TempNo,string PRNo,string RCNo
          ,string InvoiceNo,string ID,string PRID,string TypeReceive,decimal Rate,int idCSTMPODt
           )                     
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

                ee.Cells["dgvNo"].Value = No.ToString();
                ee.Cells["Status"].Value = Status;
                ee.Cells["CodeNo"].Value = CodeNo;
                ee.Cells["ItemNo"].Value = ItemNo;
                ee.Cells["ItemDescription"].Value = ItemDescription;
                ee.Cells["QTY"].Value = QTY;
                ee.Cells["RemainQty"].Value = RemainQty;
                ee.Cells["Unit"].Value = Unit;
                ee.Cells["PCSUnit"].Value = PCSUnit;
                ee.Cells["CostPerUnit"].Value = CostPerUnit;
                ee.Cells["Amount"].Value = Amount;
                ee.Cells["CRRNCY"].Value = CRRNCY;
                ee.Cells["LotNo"].Value = LotNo;
                ee.Cells["SerialNo"].Value = SerialNo;
                ee.Cells["ShelfNo"].Value = ShelfNo;
                ee.Cells["Location"].Value = Location;
                ee.Cells["Remark"].Value = Remark;
                ee.Cells["TempNo"].Value = TempNo;
                ee.Cells["PRNo"].Value = PRNo;
                ee.Cells["RCNo"].Value = RCNo;
                ee.Cells["InvoiceNo"].Value = InvoiceNo;
                ee.Cells["ID"].Value = ID;
                ee.Cells["PRID"].Value = PRID;
                ee.Cells["TypeReceive"].Value = TypeReceive;
                ee.Cells["Rate"].Value = Rate;
                ee.Cells["idCSTMPODt"].Value = idCSTMPODt;

                //if (GroupCode != "Other")
                //{
                //    ee.Cells["dgvCodeNo"].ReadOnly = true;
                //    ee.Cells["dgvItemName"].ReadOnly = true;
                //    ee.Cells["dgvItemDesc"].ReadOnly = true;


                //    ee.Cells["dgvPCSUnit"].ReadOnly = true;
                //    //ee.Cells["dgvUnitCode"].ReadOnly = true;
                //    //ee.Cells["dgvStandardCost"].ReadOnly = true;
                //}
                //else
                //{
                //    ee.Cells["dgvCodeNo"].ReadOnly = false;
                //    ee.Cells["dgvItemName"].ReadOnly = false;
                //    ee.Cells["dgvItemDesc"].ReadOnly = false;

                //    ee.Cells["dgvPCSUnit"].ReadOnly = false;
                //    //ee.Cells["dgvUnitCode"].ReadOnly = false;
                //    //ee.Cells["dgvStandardCost"].ReadOnly = false;
                //}

                ////if (lblStatus.Text.Equals("Completed"))//|| lbStatus.Text.Equals("Reject"))
                ////    dgvData.AllowAddNewRow = false;
                ////else
                ////    dgvData.AllowAddNewRow = true;

                ////dbclass.SetRowNo1(dgvData);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : Add_Item", this.Name); }

        }
        private void Cal_Amount()
        {
            if(dgvData.Rows.Count() >0)
            {
                decimal Amount = 0;
                decimal Total = 0;
                foreach (var rd1 in dgvData.Rows)
                {
                    Amount = StockControl.dbClss.TDe(rd1.Cells["Amount"].Value);
                    Total += Amount;
                }
                txtTotal.Text = Total.ToString("###,###,##0.00");
            }
        }
        private bool check_Duppicate(int id)
        {
            bool re = false;
            foreach (var rd1 in dgvData.Rows)
            {
                if (StockControl.dbClss.TInt(rd1.Cells["PRID"].Value).Equals(id))
                    re = true;
            }
            
                return re;

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //btnEdit.Enabled = true;
            //btnView.Enabled = false;
            btnNew.Enabled = true;

            string PR = txtRCNo.Text;
            ClearData();
            Enable_Status(false, "View");
            txtRCNo.Text = PR;
            DataLoad();
            btnSave.Enabled = false;
            Ac = "View";
        }

        private void btnListITem_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = false;
                //btnEdit.Enabled = true;
                //btnView.Enabled = false;
                btnNew.Enabled = true;
                ClearData();
                Enable_Status(false, "View");

                this.Cursor = Cursors.WaitCursor;
                ReceiveList sc = new ReceiveList(txtRCNo,txtDocNo);
                this.Cursor = Cursors.Default;
                sc.ShowDialog();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();
                //LoadData

                string PRNo = txtDocNo.Text;
                string RCNo = txtRCNo.Text;
                if (!txtRCNo.Text.Equals(""))
                {
                    txtDocNo.Text = "";
                    
                    DataLoad();
                    Ac = "View";
                }
                else
                {
                   
                    btnNew_Click(null, null);
                    txtDocNo.Text = PRNo;

                    Insert_data_PR();
                    txtDocNo.Text = "";

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError("CreatePart", ex.Message + " : radButtonElement1_Click", this.Name); }

        }

        private void rdoDL_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if(rdoDL.IsChecked)
            {
                txtDLNo.Enabled = true;
                txtInvoiceNo.Enabled = false;
            }
        }

        private void rdoInvoice_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (rdoInvoice.IsChecked)
            {
                txtDLNo.Enabled = false;
                txtInvoiceNo.Enabled = true;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                PrintPR a = new PrintPR(txtRCNo.Text, txtRCNo.Text, "Receive");
                a.ShowDialog();

                //using (DataClasses1DataContext db = new DataClasses1DataContext())
                //{
                //    var g = (from ix in db.sp_R003_ReportReceive(txtRCNo.Text, DateTime.Now) select ix).ToList();
                //    if (g.Count() > 0)
                //    {

                //        Report.Reportx1.Value = new string[2];
                //        Report.Reportx1.Value[0] = txtRCNo.Text;
                //        Report.Reportx1.WReport = "ReportReceive";
                //        Report.Reportx1 op = new Report.Reportx1("ReportReceive.rpt");
                //        op.Show();

                //    }
                //    else
                //        MessageBox.Show("not found.");
                //}

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void dgvData_Click(object sender, EventArgs e)
        {

        }

        private void ddlTypeReceive_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            //if (dgvData.Rows.Count() > 0)
            //    ddlTypeReceive.Enabled = false;
            //else
            //    ddlTypeReceive.Enabled = true;
        }

        private void MasterTemplate_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            try
            {
                if (e.Column.Name.Equals("Location"))
                {
                    /////////////มีการ เคลียร์ การ Add ก่อน แล้วค่อย Add ใหม่////////////////
                    //Row = e.RowIndex;
                    RadMultiColumnComboBoxElement Comcol = (RadMultiColumnComboBoxElement)e.ActiveEditor;
                    Comcol.Columns.Clear();

                    //RadMultiColumnComboBoxElement Comcol = (RadMultiColumnComboBoxElement)e.ActiveEditor;
                    Comcol.DropDownSizingMode = SizingMode.UpDownAndRightBottom;
                    Comcol.DropDownWidth = 300;
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
                        HeaderText = "รหัส",
                        Name = "Code",
                        FieldName = "Code",
                        Width = 80,
                        AllowFiltering = true,
                        ReadOnly = false
                    }
                   );
                    Comcol.EditorControl.Columns.Add(new GridViewTextBoxColumn
                    {
                        HeaderText = "ชื่อ",
                        Name = "Name",
                        FieldName = "Name",
                        Width = 150,
                        AllowFiltering = true,
                        ReadOnly = false
                    }
                   );
                    // Comcol.EditorControl.Columns.Add(new GridViewTextBoxColumn
                    // {
                    //     HeaderText = "Description",
                    //     Name = "Description",
                    //     FieldName = "Description",
                    //     Width = 300,
                    //     AllowFiltering = true,
                    //     ReadOnly = false

                    // }
                    //);


                    //dgvDataDetail.CellEditorInitialized += MasterTemplate_CellEditorInitialized;

                }
                if (e.Column.Name.Equals("Unit"))
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
            catch { }

        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            PrintPR a = new PrintPR(txtRCNo.Text, txtRCNo.Text, "ReceiveMonth");
            a.ShowDialog();
        }

        private void MasterTemplate_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {

            RadMultiColumnComboBoxElement mccbEl = e.ActiveEditor as RadMultiColumnComboBoxElement;
            if (mccbEl != null)
            {
                mccbEl.DropDownSizingMode = SizingMode.UpDownAndRightBottom;
                mccbEl.DropDownMinSize = new Size(300, 200);
                mccbEl.DropDownMaxSize = new Size(300, 200);

                mccbEl.AutoSizeDropDownToBestFit = false;
                mccbEl.DropDownAnimationEnabled = false;
                mccbEl.AutoFilter = true;
                FilterDescriptor filterDescriptor = new FilterDescriptor(mccbEl.DisplayMember, FilterOperator.Contains, string.Empty);
                mccbEl.EditorControl.MasterTemplate.FilterDescriptors.Add(filterDescriptor);
            }
        }

        private void MasterTemplate_EditorRequired(object sender, EditorRequiredEventArgs e)
        {
            GridViewEditManager manager = sender as GridViewEditManager;
            // Assigning DropDownListAddEditor to the right column

            if (manager.GridViewElement.CurrentColumn.Name == "ShelfNo")
            {
                DropDownListAddEditor editor = new DropDownListAddEditor();
                editor.InputValueNotFound += new DropDownListAddEditor.InputValueNotFoundHandler(DropDownListAddEditor_InputValueNotFoundCategory_Edit);
                e.Editor = editor;
            }
        }
        string ShelfNo_Edit = "";
        private void DropDownListAddEditor_InputValueNotFoundCategory_Edit(object sender, DropDownListAddEditor.InputValueNotFoundArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(e.Text))
                {
                    List<string> values = e.EditorElement.DataSource as List<string>;
                    if (values == null)
                    {
                        List<string> aa = new List<string>();
                        e.EditorElement.DataSource = aa;
                        values = e.EditorElement.DataSource as List<string>;
                    }
                    if (!e.Text.Equals(""))
                        ShelfNo_Edit = e.Text;
                    values.Add(e.Text);
                    e.Value = e.Text;
                    e.ValueAdded = true;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        internal class DropDownListAddEditor :
                RadDropDownListEditor
        {
            protected GridDataCellElement cell;
            protected InputValueNotFoundArgs e;
            /// <summary>
            /// Event handler for missing values in item list of editor
            /// </summary>
            /// <param name="sender">Event source of type DropDownListAddEditor</param>
            /// <param name="e">Event arguments</param>
            public delegate void InputValueNotFoundHandler(object sender,
                                                           InputValueNotFoundArgs e);
            /// <summary>
            /// Event for missing values in item list of editor
            /// </summary>
            public event InputValueNotFoundHandler InputValueNotFound;
            /// <summary>
            /// Constructor
            /// </summary>
            public DropDownListAddEditor() :
                base()
            {
                // Nothing to do
            }
            public override bool EndEdit()
            {
                RadDropDownListEditorElement element = this.EditorElement as RadDropDownListEditorElement;
                string text = element.Text;
                RadListDataItem item = null;
                foreach (RadListDataItem entry in element.Items)
                {
                    if (entry.Text == text)
                    {
                        item = entry;
                        break;
                    }
                }
                if ((item == null) &&
                   (InputValueNotFound != null))
                {
                    // Get cell for handling CellEndEdit event
                    this.cell = (this.EditorManager as GridViewEditManager).GridViewElement.CurrentCell;
                    // Add event handling for setting value to cell
                    (this.OwnerElement as GridComboBoxCellElement).GridControl.CellEndEdit += new GridViewCellEventHandler(OnCellEndEdit);
                    this.e = new InputValueNotFoundArgs(element);
                    this.InputValueNotFound(this,
                                            this.e);
                }
                return base.EndEdit();
            }
            /// <summary>
            /// Puts added value into cell value
            /// </summary>
            /// <param name="sender">Event source of type GridViewEditManager</param>
            /// <param name="e">Event arguments</param>
            /// <remarks>Connected to GridView event CellEndEdit</remarks>
            protected void OnCellEndEdit(object sender,
                                         GridViewCellEventArgs e)
            {
                if (this.e != null)
                {
                    // Handle only added value, others by default handling of grid
                    if ((this.cell == (sender as GridViewEditManager).GridViewElement.CurrentCell) &&
                        this.e.ValueAdded)
                    {
                        e.Row.Cells[e.ColumnIndex].Value = this.e.Value;
                    }
                    this.e = null;
                }
            }
            /// <summary>
            /// Event arguments for InputValueNotFound
            /// </summary>
            public class InputValueNotFoundArgs :
                             EventArgs
            {
                /// <summary>
                /// Constructor
                /// </summary>
                /// <param name="editorElement">Editor assiciated element</param>
                internal protected InputValueNotFoundArgs(RadDropDownListEditorElement editorElement)
                {
                    this.EditorElement = editorElement;
                    this.Text = editorElement.Text;
                }
                /// <summary>
                /// Editor associated element 
                /// </summary>
                public RadDropDownListEditorElement EditorElement { get; protected set; }
                /// <summary>
                /// Input text with no match in drop down list
                /// </summary>
                public string Text { get; protected set; }
                /// <summary>
                /// Text related missing value
                /// </summary>
                /// <remarks>Has to be set during event processing</remarks>
                /// <seealso cref="ValueAdded"/>
                public object Value { get; set; }
                /// <summary>
                /// Missing value added
                /// </summary>
                /// <remarks>Set also the Value property</remarks>
                public bool ValueAdded { get; set; }
            }
        }

        private void openPRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvData.Rows.Count <= 0)
                    return;

                string TempNo1 = dbClss.TSt(dgvData.CurrentRow.Cells["TempNo"].Value);

                if (dbClss.TSt(dgvData.CurrentRow.Cells["PRNo"].Value) != "") //&& ddlTypeReceive.Text =="PR")
                {
                    //string TEmpPR = "";
                    //using (DataClasses1DataContext db = new DataClasses1DataContext())
                    //{
                    //    var g = (from ix in db.tb_PurchaseRequests select ix)
                    //   .Where(a => a.PRNo == dbClss.TSt(dgvData.CurrentRow.Cells["PRNo"].Value)
                    //    && (a.Status != "Cancel")
                    //    ).ToList();
                    //    if (g.Count() > 0)
                    //    {
                    //        TEmpPR = dbClss.TSt(g.FirstOrDefault().TEMPNo);
                    //    }
                    //}
                    if (TempNo1 != "")
                    {
                        CreatePR op = new CreatePR(TempNo1);
                        op.ShowDialog();
                    }
                }
                else if (dbClss.TSt(dgvData.CurrentRow.Cells["PRNo"].Value) != "")// && ddlTypeReceive.Text == "PO")
                {
                    //string TEmpPR = "";
                    //using (DataClasses1DataContext db = new DataClasses1DataContext())
                    //{
                    //    var g = (from ix in db.tb_PurchaseOrders select ix)
                    //   .Where(a => a.PONo == dbClss.TSt(dgvData.CurrentRow.Cells["PRNo"].Value)
                    //    && (a.Status != "Cancel")
                    //    ).ToList();
                    //    if (g.Count() > 0)
                    //    {
                    //        TEmpPR = dbClss.TSt(g.FirstOrDefault().TempPNo);
                    //    }
                    //}
                    if (TempNo1 != "")
                    {
                        CreatePO op = new CreatePO(TempNo1);
                        op.ShowDialog();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
