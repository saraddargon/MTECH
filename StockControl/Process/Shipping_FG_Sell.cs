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
using Telerik.WinControls.Data;
using Telerik.WinControls;

namespace StockControl
{
    public partial class Shipping_FG_Sell : Telerik.WinControls.UI.RadRibbonForm
    {
        public Shipping_FG_Sell()
        {
            InitializeComponent();
        }
        public Shipping_FG_Sell(string SHNo,string CodeNo)
        {
            InitializeComponent();
            SHNo_t = SHNo;
            CodeNo_t = CodeNo;
        }
        string SHNo_t = "";
        string CodeNo_t = "";
        string Ac = "";
        DataTable dt_h = new DataTable();
        DataTable dt_d = new DataTable();

        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            HistoryView hw = new HistoryView(this.Name, txtDocNo.Text);
            this.Cursor = Cursors.Default;
            hw.ShowDialog();
        }

        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }
        private void GETDTRow()
        {
            dt_h.Columns.Add(new DataColumn("id", typeof(int)));
            dt_h.Columns.Add(new DataColumn("DocNo", typeof(string)));
            dt_h.Columns.Add(new DataColumn("DocNoDate", typeof(DateTime)));
            dt_h.Columns.Add(new DataColumn("CreateDate", typeof(DateTime)));
            dt_h.Columns.Add(new DataColumn("CreateBy", typeof(string)));
            dt_h.Columns.Add(new DataColumn("DocBy", typeof(string)));
            dt_h.Columns.Add(new DataColumn("Remark", typeof(string)));
            dt_h.Columns.Add(new DataColumn("Status", typeof(string)));
            dt_h.Columns.Add(new DataColumn("CSTM_Name", typeof(string)));
            dt_h.Columns.Add(new DataColumn("CSTM_Address", typeof(string)));
            dt_h.Columns.Add(new DataColumn("Barcode", typeof(Image)));
            dt_h.Columns.Add(new DataColumn("Invoice", typeof(string)));
            



            dt_d.Columns.Add(new DataColumn("id", typeof(string)));
            dt_d.Columns.Add(new DataColumn("DocNo", typeof(string)));       
            dt_d.Columns.Add(new DataColumn("CodeNo", typeof(string)));
            dt_d.Columns.Add(new DataColumn("ItemNo", typeof(string)));
            dt_d.Columns.Add(new DataColumn("ItemDescription", typeof(string)));
            dt_d.Columns.Add(new DataColumn("Qty", typeof(decimal)));
            dt_d.Columns.Add(new DataColumn("Remark", typeof(string)));
            dt_d.Columns.Add(new DataColumn("PCSUnit", typeof(decimal)));
            dt_d.Columns.Add(new DataColumn("UnitShip", typeof(decimal)));
            dt_d.Columns.Add(new DataColumn("UnitCost", typeof(decimal)));
            dt_d.Columns.Add(new DataColumn("Amount", typeof(decimal)));
            dt_d.Columns.Add(new DataColumn("Status", typeof(string)));
            dt_d.Columns.Add(new DataColumn("BarCode", typeof(Image)));

            
        }

        private void Unit_Load(object sender, EventArgs e)
        {
            //dgvData.ReadOnly = true;
            dgvData.AutoGenerateColumns = false;
            GETDTRow();
   
            DefaultItem();
            
            btnNew_Click(null, null);

            if (!SHNo_t.Equals(""))
            {
                btnNew.Enabled = true;
                txtDocNo.Text = SHNo_t;
                txtCodeNo.Text = "";
                DataLoad();
                Ac = "View";
               
            }
            else if (!CodeNo_t.Equals(""))
            {
                btnNew.Enabled = true;
                txtCodeNo.Text = CodeNo_t;
                Insert_data_New();
                txtCodeNo.Text = "";
            }

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
                    col.DataSource = (from ix in db.tb_Locations.Where(s => Convert.ToBoolean(s.Active.Equals(true)) && s.Status == "Completed")
                                      select new { ix.Location }).ToList();

                    col.DisplayMember = "Location";
                    col.ValueMember = "Location";
                    col.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDown;
                    col.FilteringMode = GridViewFilteringMode.DisplayMember;

                    col.AutoSizeMode = BestFitColumnMode.DisplayedDataCells;
                    col.TextAlignment = ContentAlignment.MiddleCenter;
                    col.DropDownStyle = RadDropDownStyle.DropDownList;
                }
                catch { }

                //col.TextAlignment = ContentAlignment.MiddleCenter;
                //col.Name = "CodeNo";
                //this.radGridView1.Columns.Add(col);

                //this.radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

                //this.radGridView1.CellEditorInitialized += radGridView1_CellEditorInitialized;
            }
        }
        private void DataLoad()
        {

            dt_h.Rows.Clear();
            dt_d.Rows.Clear();
            try
            {

                this.Cursor = Cursors.WaitCursor;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {

                    try
                    {
                        var g = (from ix in db.tb_Sell_FGs select ix).Where(a => a.DocNo == txtDocNo.Text.Trim()).ToList();
                        if (g.Count() > 0)
                        {
                            DateTime? temp_date = null;
                            txtRemark.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Remark);
                            txtCSTM_Address.Text = StockControl.dbClss.TSt(g.FirstOrDefault().CSTM_Address);
                            txtCSTM_Name.Text = StockControl.dbClss.TSt(g.FirstOrDefault().CSTM_Name);
                            txtDocName.Text = StockControl.dbClss.TSt(g.FirstOrDefault().DocBy);
                            txtInvoice.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Invoice);

                            if (!StockControl.dbClss.TSt(g.FirstOrDefault().DocNoDate).Equals(""))
                                dtDocDate.Value = Convert.ToDateTime(g.FirstOrDefault().DocNoDate,new CultureInfo("en-US"));
                            else
                                dtDocDate.Value = Convert.ToDateTime(temp_date,new CultureInfo("en-US"));

                            
                            txtCreateBy.Text = StockControl.dbClss.TSt(g.FirstOrDefault().CreateBy);
                          
                            if (!StockControl.dbClss.TSt(g.FirstOrDefault().CreateDate).Equals(""))
                             txtCreateDate.Text = Convert.ToDateTime(g.FirstOrDefault().CreateDate,new CultureInfo("en-US")).ToString("dd/MMM/yyyy");
                            
                            else
                                txtCreateDate.Text = "";

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
                                btnDel_Item.Enabled = false;
                                btnAdd_Part.Enabled = false;
                            }
                           
                            else if
                                (StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Completed"))

                            {
                                btnSave.Enabled = false;
                                //btnDelete.Enabled = false;
                                //btnView.Enabled = false;
                                //btnEdit.Enabled = false;
                                lblStatus.Text = "Completed";
                                dgvData.ReadOnly = true;
                                btnNew.Enabled = true;
                                btnDel_Item.Enabled = false;
                                btnAdd_Part.Enabled = false;
                            }
                            else
                            {
                                btnNew.Enabled = true;
                                btnSave.Enabled = true;
                                //btnDelete.Enabled = true;
                                //btnView.Enabled = true;
                                //btnEdit.Enabled = true;
                                lblStatus.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Status);
                                dgvData.ReadOnly = false;
                                btnDel_Item.Enabled = false;
                                btnAdd_Part.Enabled = false;
                            }
                            dt_h = StockControl.dbClss.LINQToDataTable(g);

                            //Detail

                            var d = (from i in db.tb_Sell_FG_Details
                                         //join s in db.tb_Stocks on i.CodeNo equals s.RefNo
                                     where i.Status != "Cancel" //&& d.verticalID == VerticalID
                                        && i.DocNo == txtDocNo.Text.Trim()

                                     //var d = (from ix in db.tb_Sell_FG_Details select ix)
                                     //.Where (a => a.DocNo == txtDocNo.Text.Trim()
                                     //    && a.Status != "Cancel")
                                     select new
                                     {
                                         CodeNo = i.CodeNo,
                                         //ItemNo= "",
                                         //ItemDescription = "a",
                                         Qty = i.Qty,
                                         //RemainQty = 0,
                                         StandardCost = i.UnitCost,//Convert.ToDecimal(dbClss.Get_Stock(i.CodeNo, "", "", "Avg")),//i.StandardCost
                                         Amount = i.Amount,
                                         Remark = i.Remark,
                                         id = i.id
                                         //PCSUnit = 1,
                                         //UnitShip = ""

                                 }).ToList();
                            if (d.Count() > 0)
                            {
                                int c = 0;
                                dgvData.DataSource = d;
                                dt_d = StockControl.dbClss.LINQToDataTable(d);
                                dgvData.EndEdit();
                                int id = 0;
                                foreach (var x in dgvData.Rows)
                                {
                                    c += 1;
                                    x.Cells["dgvNo"].Value = c;

                                    id = Convert.ToInt32(x.Cells["id"].Value);

                                    var s = (from ix in db.tb_Stocks select ix)
                                       .Where(a => a.DocNo == txtDocNo.Text.Trim()
                                           //&& a.Refid == id)
                                           && a.CodeNo == Convert.ToString(x.Cells["CodeNo"].Value)).OrderByDescending(ab => ab.id)
                                           .FirstOrDefault();
                                    if (s != null)
                                    {
                                        x.Cells["RemainQty"].Value = Convert.ToDecimal(s.RemainQty);
                                        //x.Cells["StandardCost"].Value = Convert.ToDecimal(s.UnitCost);
                                        //x.Cells["Amount"].Value = Math.Abs(Convert.ToDecimal(s.UnitCost) * Convert.ToDecimal(x.Cells["QTY"].Value));//Math.Abs(Convert.ToDecimal(s.AmountCost));
                                    }
                                    var t = (from ix in db.tb_Items select ix)
                                      .Where(a => a.CodeNo == Convert.ToString(x.Cells["CodeNo"].Value))
                                          .FirstOrDefault();
                                    if (t != null)
                                    {
                                        x.Cells["ItemNo"].Value = dbClss.TSt(t.ItemNo);
                                        x.Cells["ItemDescription"].Value = dbClss.TSt(t.ItemDescription);
                                        x.Cells["UnitShip"].Value = dbClss.TSt(t.UnitShip);
                                        x.Cells["PCSUnit"].Value = dbClss.TDe(t.PCSUnit);

                                    }
                                }
                            }
                            Cal_Amount();
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }

                }
            }
            catch { }
            finally { this.Cursor = Cursors.Default; }


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
            txtCSTM_Name.Text = "";
            txtCSTM_Address.Text = "";
            txtInvoice.Text = "";
            txtDocNo.Text = "";
            txtRemark.Text = "";
            txtCodeNo.Text = "";
            dtDocDate.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
            txtCreateBy.Text = ClassLib.Classlib.User;
            txtDocName.Text = ClassLib.Classlib.User;
            txtCreateDate.Text = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy");
            lblStatus.Text = "-";
            dgvData.Rows.Clear();
            dgvData.DataSource = null;
            dt_d.Rows.Clear();
            dt_h.Rows.Clear();
            txtTotal.Text = "0.00";
        }
      private void Enable_Status(bool ss,string Condition)
        {
            if (Condition.Equals("-") || Condition.Equals("New"))
            {
                txtCodeNo.Enabled = ss;
                txtDocName.Enabled = ss;
                //txtRCNo.Enabled = ss;
                txtRemark.Enabled = ss;
                //txtTempNo.Enabled = ss;
                dtDocDate.Enabled = ss;
                dgvData.ReadOnly = false;
                btnDel_Item.Enabled = ss;
                txtCSTM_Address.Enabled = ss;
                txtCSTM_Name.Enabled = ss;
                txtInvoice.Enabled = ss;

            }
            else if (Condition.Equals("View"))
            {
                txtInvoice.Enabled = ss;
                txtCodeNo.Enabled = ss;
                txtDocName.Enabled = ss;
                //txtRCNo.Enabled = ss;
                txtRemark.Enabled = ss;
                //txtTempNo.Enabled = ss;
                dtDocDate.Enabled = ss;
                dgvData.ReadOnly = false;
                txtRemark.Enabled = ss;
                btnDel_Item.Enabled = ss;
                txtCSTM_Address.Enabled = ss;
                txtCSTM_Name.Enabled = ss;
            }

            else if (Condition.Equals("Edit"))
            {
                txtInvoice.Enabled = ss;
                txtCodeNo.Enabled = ss;
                txtDocName.Enabled = ss;
                //txtRCNo.Enabled = ss;
                txtRemark.Enabled = ss;
                //txtTempNo.Enabled = ss;
                dtDocDate.Enabled = ss;
                dgvData.ReadOnly = false;
                txtRemark.Enabled = ss;
                btnDel_Item.Enabled = ss;
                txtCSTM_Address.Enabled = ss;
                txtCSTM_Name.Enabled = ss;
            }
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            btnDel_Item.Enabled = true;
            btnAdd_Part.Enabled = true;
            btnNew.Enabled = false;
            btnSave.Enabled = true;

            ClearData();
            Enable_Status(true, "New");
            lblStatus.Text = "New";
            Ac = "New";

           // getมาไว้ก่อน แต่ยังไมได้ save
            txtDocNo.Text = StockControl.dbClss.GetNo(17, 0);
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
                

                if (txtDocName.Text.Equals(""))
                    err += "- “ผู้ขายสินค้า:” เป็นค่าว่าง \n";
                if (dtDocDate.Text.Equals(""))
                    err += "- “วันที่ขายสินค้า:” เป็นค่าว่าง \n";
                if (txtInvoice.Text.Equals(""))
                    err += "- “เลขที่ Invoice / อ้างอิง:” เป็นค่าว่าง \n";
                if (txtCSTM_Name.Text.Equals(""))
                    err += "- “ชื่อลูกค้า:” เป็นค่าว่าง \n";
                //if (txtCSTM_Address.Text.Equals(""))
                //    err += "- “ที่อยู่:” เป็นค่าว่าง \n";

                if (dgvData.Rows.Count <= 0)
                    err += "- “รายการ:” เป็นค่าว่าง \n";
                int c = 0;
                foreach (GridViewRowInfo rowInfo in dgvData.Rows)
                {
                    if (rowInfo.IsVisible)
                    {
                        if (StockControl.dbClss.TInt(rowInfo.Cells["QTY"].Value) <= (0))
                        {
                            err += "- “จำนวนสินค้า:” ต้องมากกว่า 0 \n";
                        }
                        else  if (StockControl.dbClss.TInt(rowInfo.Cells["QTY"].Value) != (0))
                        {
                            c += 1;
                            
                            if (StockControl.dbClss.TSt(rowInfo.Cells["CodeNo"].Value).Equals(""))
                                err += "- “รหัสสินค้า:” เป็นค่าว่าง \n";
                            if (StockControl.dbClss.TDe(rowInfo.Cells["QTY"].Value) > StockControl.dbClss.TDe(rowInfo.Cells["RemainQty"].Value))
                                err += "- “จำนวนสินค้าที่ขาย:” มากกว่าจำนวนคงเหลือ \n";
                            //if (StockControl.dbClss.TDe(rowInfo.Cells["UnitShip"].Value).Equals(""))
                            //    err += "- “หน่วย:” เป็นค่าว่าง \n";

                        }
                        else if (StockControl.dbClss.TSt(rowInfo.Cells["Location"].Value)=="")
                        {
                            err += "- “สถานที่เก็บ:” เป็นค่าว่าง \n";
                        }
                    }
                }

                if (c <= 0)
                    err += "- “กรุณาระบุจำนวนที่จะเบิกสินค้า:” เป็นค่าว่าง \n";


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
        private void SaveHerder()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                var g = (from ix in db.tb_Sell_FGs
                         where ix.DocNo.Trim() == txtDocNo.Text.Trim() && ix.Status != "Cancel"
                         //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                         select ix).ToList();
                if (g.Count > 0)  //มีรายการในระบบ
                {
                    foreach (DataRow row in dt_h.Rows)
                    {
                        var gg = (from ix in db.tb_Sell_FGs
                                  where ix.DocNo.Trim() == txtDocNo.Text.Trim() && ix.Status != "Cancel"
                                  //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                                  select ix).First();

                        gg.CreateBy = ClassLib.Classlib.User;
                        gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                        dbClss.AddHistory(this.Name, "แก้ไขการขายสินค้า", "แก้ไขโดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", txtDocNo.Text);
                        //if (StockControl.dbClss.TSt(gg.BarCode).Equals(""))
                        //    gg.BarCode = StockControl.dbClss.SaveQRCode2D(txtSHNo.Text.Trim());

                        if (!txtDocName.Text.Trim().Equals(row["DocBy"].ToString()))
                        {
                            gg.DocBy = txtDocName.Text;
                            dbClss.AddHistory(this.Name, "แก้ไขการขายสินค้า", "แก้ไขผู้ขายสินค้า [" + txtDocName.Text.Trim() + " เดิม :" + row["DocBy"].ToString() + "]", txtDocNo.Text);
                        }

                        if (!txtInvoice.Text.Trim().Equals(row["Invoice"].ToString()))
                        {
                            gg.Invoice = txtInvoice.Text.Trim();
                            dbClss.AddHistory(this.Name, "แก้ไขการขายสินค้า", "แก้ไขเลขที่ Invoice /อ้างอิง [" + txtInvoice.Text.Trim() + " เดิม :" + row["Invoice"].ToString() + "]", txtDocNo.Text);
                        }
                      

                        if (!txtCSTM_Name.Text.Trim().Equals(row["CSTM_Name"].ToString()))
                        {
                            gg.CSTM_Name = txtCSTM_Name.Text;
                            dbClss.AddHistory(this.Name, "แก้ไขการขายสินค้า", "แก้ไขชื่อลูกค้า [" + txtCSTM_Name.Text.Trim() + " เดิม :" + row["CSTM_Name"].ToString() + "]", txtDocNo.Text);
                        }
                        if (!txtCSTM_Address.Text.Trim().Equals(row["CSTM_Address"].ToString()))
                        {
                            gg.CSTM_Address = txtCSTM_Address.Text;
                            dbClss.AddHistory(this.Name, "แก้ไขการขายสินค้า", "แก้ไขที่อยู่ [" + txtCSTM_Address.Text.Trim() + " เดิม :" + row["CSTM_Address"].ToString() + "]", txtDocNo.Text);
                        }

                        if (!txtRemark.Text.Trim().Equals(row["Remark"].ToString()))
                        {
                            gg.Remark = txtRemark.Text.Trim();
                            dbClss.AddHistory(this.Name, "แก้ไขการขายสินค้า", "แก้ไขหมายเหตุ [" + txtRemark.Text.Trim() + " เดิม :" + row["Remark"].ToString() + "]", txtDocNo.Text);
                        }

                        if (!dtDocDate.Text.Trim().Equals(""))
                        {
                            string date1 = "";
                            date1 = dtDocDate.Value.ToString("yyyyMMdd", new CultureInfo("en-US"));
                            string date2 = "";
                            DateTime temp = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                            if (!StockControl.dbClss.TSt(row["DocNoDate"].ToString()).Equals(""))
                            {

                                temp = Convert.ToDateTime(row["DocNoDate"]);
                                date2 = temp.ToString("yyyyMMdd", new CultureInfo("en-US"));

                            }
                            if (!date1.Equals(date2))
                            {
                                DateTime? RequireDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                if (!dtDocDate.Text.Equals(""))
                                    RequireDate = dtDocDate.Value;
                                gg.DocNoDate = RequireDate;
                                dbClss.AddHistory(this.Name, "แก้ไขการขายสินค้า", "แก้ไขวันที่ขายสินค้า [" + dtDocDate.Text.Trim() + " เดิม :" + temp.ToString() + "]", txtDocNo.Text);
                            }
                        }
                        db.SubmitChanges();
                    }
                }
                else //สร้างใหม่
                {
                    byte[] barcode = null;
                    //barcode = StockControl.dbClss.SaveQRCode2D(txtSHNo.Text.Trim());
                    //DateTime? UpdateDate = null;

                    DateTime? DocDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    if (!dtDocDate.Text.Equals(""))
                        DocDate = dtDocDate.Value;

                    tb_Sell_FG gg = new tb_Sell_FG();
                    gg.CSTM_Name = txtCSTM_Name.Text;
                    gg.CSTM_Address = txtCSTM_Address.Text;
                    gg.DocBy = txtDocName.Text;
                    gg.DocNoDate = DocDate;
                    gg.CreateBy = ClassLib.Classlib.User;
                    gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    gg.DocNo = txtDocNo.Text;
                    gg.Remark = txtRemark.Text;           
                    gg.Barcode = barcode;
                    gg.Status = "Completed";
                    gg.Invoice = txtInvoice.Text.Trim();

                    db.tb_Sell_FGs.InsertOnSubmit(gg);
                    db.SubmitChanges();

                    dbClss.AddHistory(this.Name, "การสินค้า", "สร้าง การขายสินค้า [" + txtDocNo.Text.Trim() + "]", txtDocNo.Text);
                }
            }
        }
        private void SaveDetail()
        {
            dgvData.EndEdit();

            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                //decimal UnitCost = 0;
                //string LineName = "";
                //string SerialNo = "";
                //string MachineName = "";
                //string LotNo = "";

                foreach (var g in dgvData.Rows)
                {
                    string SS = "";
                    if (g.IsVisible.Equals(true))
                    {
                        if (StockControl.dbClss.TInt(g.Cells["QTY"].Value) != (0)) // เอาเฉพาะรายการที่ไม่เป็น 0 
                        {
                            if (StockControl.dbClss.TInt(g.Cells["id"].Value) <= 0)  //New ใหม่
                            {
                                //int RefidJobNo = 0;// dbClss.TInt(txtRefidJobNo.Text);

                                tb_Sell_FG_Detail gg = new tb_Sell_FG_Detail();
                                gg.DocNo = txtDocNo.Text;
                                gg.Qty = (StockControl.dbClss.TDe(g.Cells["QTY"].Value));
                                gg.CodeNo = StockControl.dbClss.TSt(g.Cells["CodeNo"].Value);
                                gg.UnitCost = StockControl.dbClss.TDe(g.Cells["StandardCost"].Value) ;
                                gg.Amount = StockControl.dbClss.TDe(g.Cells["Amount"].Value);
                                gg.Location = StockControl.dbClss.TSt(g.Cells["Location"].Value);
                                gg.Remark = StockControl.dbClss.TSt(g.Cells["Remark"].Value);

                                gg.Status = "Completed";
                                db.tb_Sell_FG_Details.InsertOnSubmit(gg);
                                db.SubmitChanges();

                                dbClss.AddHistory(this.Name, "การสินค้า", "สร้าง การขายสินค้า [" + (StockControl.dbClss.TSt(g.Cells["CodeNo"].Value)) + " Qty : " + (StockControl.dbClss.TDe(g.Cells["QTY"].Value)).ToString() +" Amount : "+ StockControl.dbClss.TDe(g.Cells["Amount"].Value).ToString()+ "]", txtDocNo.Text);
                                
                            }
                        }
                    }
                }
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            if (Ac.Equals("New"))// || Ac.Equals("Edit"))
            {
                if (Check_Save())
                    return;
                if (MessageBox.Show("ต้องการบันทึก ?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Cursor = Cursors.WaitCursor;

                    if (Ac.Equals("New"))
                        txtDocNo.Text = StockControl.dbClss.GetNo(17, 2);

                    if (!txtDocNo.Text.Equals(""))
                    {
                        SaveHerder();
                        SaveDetail();

                        //add stock
                        InsertStock_new();


                        DataLoad();
                        btnNew.Enabled = true;
                        btnDel_Item.Enabled = false;
                        btnAdd_Part.Enabled = false;
                        btnSave.Enabled = false;
                        btnAdd_Part.Enabled = false;
                        //btnRefresh_Click(null, null);
                        MessageBox.Show("บันทึกสำเร็จ!");

                    }
                    else
                    {
                        MessageBox.Show("ไม่สามารถโหลดเลขที่รับสินค้าได้ ติดต่อแผนก IT");
                    }
                }
            }
        }
        
        private decimal get_cost(string Code)
        {
            decimal re = 0;
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                var g = (from ix in db.tb_Items
                         where ix.CodeNo == Code && ix.Status == "Active"
                         select ix).First();
                re = Convert.ToDecimal(g.StandardCost);

            }
            return re;
        }
       
        private void InsertStock_new()
        {
            try
            {

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    //string CNNo = CNNo = StockControl.dbClss.GetNo(6, 2);
                    var g = (from ix in db.tb_Sell_FG_Details
                                 //join i in db.tb_Items on ix.CodeNo equals i.CodeNo
                             where ix.DocNo.Trim() == txtDocNo.Text.Trim() && ix.Status != "Cancel"

                             select ix).ToList();
                    if (g.Count > 0)
                    {
                        //insert Stock
                        string LineName = "";
                        string MachineName = "";
                        string LotNo = "";
                        string SerialNo = "";
                        int RefidJobNo = 0;
                        foreach (var vv in g)
                        {
                            //RefidJobNo = vv.id;
                            
                            db.sp_036_Sell_FG(txtDocNo.Text.Trim(), StockControl.dbClss.TSt(vv.CodeNo)
                                , StockControl.dbClss.TDe(vv.Qty), StockControl.dbClss.TSt(vv.Remark)
                                , LineName, MachineName
                                , SerialNo, LotNo
                                , "Completed", ClassLib.Classlib.User
                                , ""//txtJobCard.Text.Trim()
                                , ""//txtTempJobCard.Text.Trim()
                                , RefidJobNo
                                ,vv.Location
                                );

                            //update Stock เข้า item
                            //db.sp_010_Update_StockItem(Convert.ToString(vv.CodeNo), "");
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                dgvData.EndEdit();
                if (e.RowIndex >= -1)
                {

                    if (dgvData.Columns["QTY"].Index == e.ColumnIndex)
                    {
                        decimal QTY = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["QTY"].Value), out QTY);
                        decimal RemainQty = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["RemainQty"].Value), out RemainQty);
                        if (QTY > RemainQty)
                        {
                            MessageBox.Show("ไม่สามารถเบิกขายเกินจำนวนคงเหลือได้");
                            e.Row.Cells["QTY"].Value = 0;
                        }

                        e.Row.Cells["StandardCost"].Value = Get_UnitCostFIFO(dbClss.TSt(e.Row.Cells["CodeNo"].Value),QTY, dbClss.TSt(e.Row.Cells["Location"].Value));
                    }

                    if (dgvData.Columns["QTY"].Index == e.ColumnIndex
                        || dgvData.Columns["StandardCost"].Index == e.ColumnIndex
                        )
                    {
                        decimal QTY = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["QTY"].Value), out QTY);
                        decimal CostPerUnit = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["StandardCost"].Value), out CostPerUnit);
                        e.Row.Cells["Amount"].Value = QTY * CostPerUnit;
                        Cal_Amount();
                    }
                    else if (dgvData.Columns["Location"].Index == e.ColumnIndex)
                    {
                        using (DataClasses1DataContext db = new DataClasses1DataContext())
                        {
                            e.Row.Cells["RemainQty"].Value = (Convert.ToDecimal(db.Cal_QTY_Remain_Location(Convert.ToString(e.Row.Cells["CodeNo"].Value), "Invoice", 0, Convert.ToString(e.Row.Cells["Location"].Value))));
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private decimal Get_UnitCostFIFO(string CodeNo,decimal Qty,string Location)
        {
            decimal re = 0;
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                re = dbClss.TDe(db.Get_AvgCost_FIFO(CodeNo, Qty, Location));
            }
            return re;
        }
        private void Cal_Amount()
        {
            if (dgvData.Rows.Count() > 0)
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
        private void Unit_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // MessageBox.Show(e.KeyCode.ToString());
        }

        private void radGridView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
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

        private void radGridView1_Click(object sender, EventArgs e)
        {

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

        //private void cboYear_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        //{
            
        //}

        private void radLabel5_Click(object sender, EventArgs e)
        {

        }

        private void radLabel2_Click(object sender, EventArgs e)
        {

        }

        private void radTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void radLabel4_Click(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            string Docno = txtDocNo.Text;
            btnNew_Click(null, null);
            txtDocNo.Text = Docno;
            DataLoad();
            btnDel_Item.Enabled = false;
            btnSave.Enabled = false;
            btnNew.Enabled = true;
            btnAdd_Part.Enabled = false;
        }

        private void txtCodeNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {

                    //Insert_data(); ของเก่า
                    //New
                    Insert_data_New();
                    txtCodeNo.Text = "";

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private bool Duppicate(string CodeNo)
        {
            bool re = false;
            dgvData.EndEdit();
            foreach (var g in dgvData.Rows)
            {
                if(Convert.ToString(g.Cells["CodeNo"].Value).Equals(CodeNo))
                {
                    re = true;
                    MessageBox.Show("รหัสพาร์ทซ้ำ");
                    break;
                }
            }

            return re;
        }
        private void Insert_data_New()
        {
            if (!txtCodeNo.Text.Equals("") && !Duppicate(txtCodeNo.Text))
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    
                    int dgvNo = 0;

                    var r = (from i in db.tb_Items
                                 //join s in db.tb_Stocks on i.CodeNo equals s.RefNo
                             where i.Status == "Active" //&& d.verticalID == VerticalID
                                && i.CodeNo == txtCodeNo.Text
                                && i.TypePart == "FG"
                                && i.StockInv > 0
                             //&& h.VendorNo.Contains(VendorNo_ss)
                             select new
                             {

                                 CodeNo = i.CodeNo,
                                 ItemNo = i.ItemNo,
                                 ItemDesc = i.ItemDescription,
                                 QTY = 0,
                                 //Qty = i.StockInv,
                                 RemainQty = (Convert.ToDecimal(db.Cal_QTY_Remain_Location(i.CodeNo, "", 0,i.Location))),
                                 //Qty1 = (Convert.ToDecimal(db.Cal_QTY(i.CodeNo, "", 0))),
                                 UnitCost = i.StandardCost,
                                 Amount = 0,
                                 Unit = i.UnitShip,                               
                                 Status = i.Status,
                                 CreateBy = i.CreateBy,
                                 CreateDate = i.CreateDate,
                                 Location = i.Location,
                                                
                                 PCSUnit = i.PCSUnit,
                                 StandardCodt = i.StandardCost,//Convert.ToDecimal(dbClss.Get_Stock(i.CodeNo, "", "", "Avg")),//i.StandardCost
                                                                                              
                                 Remark = "",
                                 id = 0

                             }
                    ).ToList();
                    if (r.Count > 0)
                    {
                        dgvNo = dgvData.Rows.Count() + 1;

                        foreach (var vv in r)
                        {
                            
                            Add_Item(dgvNo, vv.CodeNo, vv.ItemNo, vv.ItemDesc
                                        , vv.RemainQty, vv.QTY, vv.Unit, dbClss.TDe(vv.PCSUnit), dbClss.TDe(vv.StandardCodt)
                                        , vv.Amount, vv.Remark, vv.id,vv.Location);

                        }
                    }
                    Cal_Amount();

                }
            }
        }

        private void Add_Item(int Row, string CodeNo, string ItemNo
            , string ItemDescription,decimal RemainQty, decimal QTY,string UnitShip, decimal PCSUnit
           , decimal StandardCost,decimal Amount
            ,string Remark,int id,string Location)
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
                ee.Cells["CodeNo"].Value = CodeNo;
                ee.Cells["ItemNo"].Value = ItemNo;
                ee.Cells["ItemDescription"].Value = ItemDescription;
                ee.Cells["RemainQty"].Value = RemainQty;
                ee.Cells["QTY"].Value = QTY;
                ee.Cells["UnitShip"].Value = UnitShip;
                ee.Cells["PCSUnit"].Value = PCSUnit;
                ee.Cells["StandardCost"].Value = StandardCost;
                ee.Cells["Amount"].Value = Amount;              
                ee.Cells["Remark"].Value = Remark;
                ee.Cells["id"].Value = id;
                ee.Cells["Location"].Value = Location;
              


                ////if (lblStatus.Text.Equals("Completed"))//|| lbStatus.Text.Equals("Reject"))
                ////    dgvData.AllowAddNewRow = false;
                ////else
                ////    dgvData.AllowAddNewRow = true;

                ////dbclass.SetRowNo1(dgvData);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : Add_Item", this.Name); }

        }
     
        private void dgvData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                btnDel_Item_Click(null, null);
        }

        private void btnDel_Item_Click(object sender, EventArgs e)
        {
            try
            {

                if (dgvData.Rows.Count < 0)
                    return;


                if (Ac.Equals("New"))// || Ac.Equals("Edit"))
                {
                    this.Cursor = Cursors.WaitCursor;

                    int id = 0;
                    int.TryParse(StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["id"].Value), out id);
                    if (id <= 0)
                        dgvData.Rows.Remove(dgvData.CurrentRow);

                    else
                    {
                        string CodeNo = "";
                        CodeNo = StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["CodeNo"].Value);
                        if (MessageBox.Show("ต้องการลบรายการ ( " + CodeNo + " ) ออกจากรายการ หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            dgvData.CurrentRow.IsVisible = false;
                        }
                    }
                    SetRowNo1(dgvData);
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

        private void btnListItem_Click(object sender, EventArgs e)
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
                Shipping_FG_Sell_List2 sc = new Shipping_FG_Sell_List2(txtDocNo);
                this.Cursor = Cursors.Default;
                sc.ShowDialog();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();
                //LoadData

              
                string SHNo = txtDocNo.Text;
                if (!txtDocNo.Text.Equals(""))
                {
                    DataLoad();
                    Ac = "View";
                    btnDel_Item.Enabled = false;
                    btnSave.Enabled = false;
                    btnNew.Enabled = true;
                    btnAdd_Part.Enabled = false;
                }
                else
                {
                    btnDel_Item.Enabled = true;
                    btnNew_Click(null, null);
                   
                    btnAdd_Part.Enabled = true;
                    Insert_data_New();
                    txtCodeNo.Text = "";

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : radButtonElement1_Click", this.Name); }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                PrintPR a = new PrintPR(txtDocNo.Text, txtDocNo.Text, "SellFG");
                a.ShowDialog();

                //using (DataClasses1DataContext db = new DataClasses1DataContext())
                //{
                //    var g = (from ix in db.sp_R004_ReportShipping(txtSHNo.Text, Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));) select ix).ToList();
                //    if (g.Count() > 0)
                //    {

                //        Report.Reportx1.Value = new string[2];
                //        Report.Reportx1.Value[0] = txtSHNo.Text;
                //        Report.Reportx1.WReport = "ReportShipping";
                //        Report.Reportx1 op = new Report.Reportx1("ReportShipping.rpt");
                //        op.Show();

                //    }
                //    else
                //        MessageBox.Show("not found.");
                //}

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void txtJobCard_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //try
            //{
            //    if (txtJobCard.Text.Trim() == "")
            //        return;

            //    if (e.KeyValue == 13 || e.KeyValue == 9)
            //    {
            //        using (DataClasses1DataContext db = new DataClasses1DataContext())
            //        {
            //            var p = (from ix in db.tb_JobCards select ix)
            //                 .Where
            //                 (a => a.JobCard.Trim().ToUpper() == txtJobCard.Text.Trim().ToUpper() && a.Status != "Cancel"
                           
            //                 ).ToList();
            //            if (p.Count > 0)
            //            {
            //                if (dbClss.TSt(p.FirstOrDefault().Status) != "Completed")
            //                {
            //                    txtTempJobCard.Text = dbClss.TSt(p.FirstOrDefault().TempJobCard);
            //                    txtRefidJobNo.Text = dbClss.TSt(p.FirstOrDefault().id);
            //                }
            //                else if (dbClss.TSt(p.FirstOrDefault().Status) != "Completed")
            //                {
            //                    txtTempJobCard.Text = "";
            //                    txtJobCard.Text = "";
            //                    txtRefidJobNo.Text = "0";
            //                    MessageBox.Show("ใบงานการผลิตดังกล่าวถูกปิดไปแล้ว กรุณาระบุใบงานการผลิตใหม่");
            //                }

            //            }
            //            else
            //            {
            //                txtJobCard.Text = "";
            //                txtTempJobCard.Text = "";
            //                txtRefidJobNo.Text = "0";
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void cbShipforJob_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            //if(cbShipforJob.Checked)
            //{
            //    txtJobCard.ReadOnly = false;
            //}
            //else
            //{
            //    txtJobCard.ReadOnly = true;
            //    txtJobCard.Text = "";
            //    txtTempJobCard.Text = "";
            //    txtRefidJobNo.Text = "0";
            //}
        }

        private void btnAdd_Part_Click(object sender, EventArgs e)
        {
            try
            {
                
                    List<GridViewRowInfo> dgvRow_List = new List<GridViewRowInfo>();
                    //dgvRow_List.Clear();
                    Shipping_FG_Sell_List MS = new Shipping_FG_Sell_List(dgvRow_List);
                    MS.ShowDialog();
                if (dgvRow_List.Count > 0)
                {
                    string CodeNo = "";
                    this.Cursor = Cursors.WaitCursor;
                    //decimal OrderQty = 1;
                    foreach (GridViewRowInfo ee in dgvRow_List)
                    {
                        CodeNo = Convert.ToString(ee.Cells["CodeNo"].Value).Trim();
                        if (!CodeNo.Equals(""))// && !check_Duppicate(CodeNo))
                        {
                            txtCodeNo.Text = CodeNo;
                            Insert_data_New();
                        }
                        else
                        {
                            MessageBox.Show("รหัสพาร์ท ซ้ำ");
                        }
                    }
                    //getTotal();
                }
             
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
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
                    Comcol.DropDownWidth = 100;
                    Comcol.DropDownHeight = 80;
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
                        HeaderText = "สถานที่เก็บ",
                        Name = "Location",
                        FieldName = "Location",
                        Width = 100,
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
            }
            catch { }
        }

        private void MasterTemplate_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {

            RadMultiColumnComboBoxElement mccbEl = e.ActiveEditor as RadMultiColumnComboBoxElement;
            if (mccbEl != null)
            {
                mccbEl.DropDownSizingMode = SizingMode.UpDownAndRightBottom;
                mccbEl.DropDownMinSize = new Size(150, 100);
                mccbEl.DropDownMaxSize = new Size(150, 100);

                mccbEl.AutoSizeDropDownToBestFit = false;
                mccbEl.DropDownAnimationEnabled = false;
                mccbEl.AutoFilter = true;
                FilterDescriptor filterDescriptor = new FilterDescriptor(mccbEl.DisplayMember, FilterOperator.Contains, string.Empty);
                mccbEl.EditorControl.MasterTemplate.FilterDescriptors.Add(filterDescriptor);
            }
        }
    }
}
