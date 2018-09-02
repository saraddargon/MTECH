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

namespace StockControl
{
    public partial class CreateJob : Telerik.WinControls.UI.RadRibbonForm
    {
        public CreateJob()
        {
            InitializeComponent();
        }
        public CreateJob(string JobCard,string TempJobCard)
        {
            InitializeComponent();
            JobCard_t = JobCard;
            TempJobCard_t = TempJobCard;
        }
        string JobCard_t = "";
        string TempJobCard_t = "";
        string Ac = "";
        DataTable dt_h = new DataTable();
        //DataTable dt_d = new DataTable();

        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            HistoryView hw = new HistoryView(this.Name, txtTempJobCard.Text);
            this.Cursor = Cursors.Default;
            hw.ShowDialog();
        }

        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }
        private void GETDTRow()
        {
            dt_h.Columns.Add(new DataColumn("id", typeof(int)));
            dt_h.Columns.Add(new DataColumn("JobCard", typeof(string)));
            dt_h.Columns.Add(new DataColumn("TempJobCard", typeof(string)));
            dt_h.Columns.Add(new DataColumn("CustomerName", typeof(string)));
            dt_h.Columns.Add(new DataColumn("Address", typeof(string)));
            dt_h.Columns.Add(new DataColumn("CodeNo", typeof(string)));
            dt_h.Columns.Add(new DataColumn("ItemName", typeof(string)));
            dt_h.Columns.Add(new DataColumn("ItemDesc", typeof(string)));
            dt_h.Columns.Add(new DataColumn("Qty", typeof(decimal)));
            dt_h.Columns.Add(new DataColumn("LotNo", typeof(string)));
            dt_h.Columns.Add(new DataColumn("Unit", typeof(string)));
            

            dt_h.Columns.Add(new DataColumn("Duedate", typeof(DateTime)));
            dt_h.Columns.Add(new DataColumn("PODate", typeof(DateTime)));
            dt_h.Columns.Add(new DataColumn("CreateBy", typeof(string)));
            dt_h.Columns.Add(new DataColumn("ModifyBy", typeof(string)));
            dt_h.Columns.Add(new DataColumn("ModifyDate", typeof(DateTime)));
            dt_h.Columns.Add(new DataColumn("CreateDate", typeof(DateTime)));
          
            dt_h.Columns.Add(new DataColumn("Remark", typeof(string)));
            dt_h.Columns.Add(new DataColumn("Status", typeof(string)));
            dt_h.Columns.Add(new DataColumn("BarCode", typeof(Image)));
            dt_h.Columns.Add(new DataColumn("Type", typeof(string)));


        }

        private void Unit_Load(object sender, EventArgs e)
        {
            //dgvData.ReadOnly = true;
            dgvData.AutoGenerateColumns = false;
            GETDTRow();

            DefaultItem();

            btnNew_Click(null, null);

            if (!TempJobCard_t.Equals(""))
            {
                btnNew.Enabled = true;
                txtTempJobCard.Text = TempJobCard_t;

                Enable_Status(false, "View");
                btnView_Click_1(null, null);

                DataLoad();

               

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

                    

                    //GridViewMultiComboBoxColumn col = (GridViewMultiComboBoxColumn)radGridView1.Columns["CodeNo"];
                    //col.DataSource = (from ix in db.tb_Items.Where(s => s.Status.Equals("Active")) select new { ix.CodeNo, ix.ItemDescription }).ToList();
                    //col.DisplayMember = "CodeNo";
                    //col.ValueMember = "CodeNo";

                    //col.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDown;
                    //col.FilteringMode = GridViewFilteringMode.DisplayMember;

                    // col.AutoSizeMode = BestFitColumnMode.DisplayedDataCells;
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
           // dt_d.Rows.Clear();
            try
            {

                this.Cursor = Cursors.WaitCursor;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {

                    try
                    {
                        var g = (from ix in db.tb_JobCards select ix)
                            .Where(a => a.TempJobCard == txtTempJobCard.Text.Trim()).ToList();
                        if (g.Count() > 0)
                        {
                            DateTime? temp_date = null;
                            txtRemark.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Remark);

                            if (!StockControl.dbClss.TSt(g.FirstOrDefault().Duedate).Equals(""))
                                dtDuedate.Value = Convert.ToDateTime((g.FirstOrDefault().Duedate),new CultureInfo("en-US"));
                            else
                                dtDuedate.Value = Convert.ToDateTime((temp_date),new CultureInfo("en-US"));

                            if (!StockControl.dbClss.TSt(g.FirstOrDefault().PODate).Equals(""))
                                dtPOdate.Value = Convert.ToDateTime((g.FirstOrDefault().PODate),new CultureInfo("en-US"));
                            else
                                dtPOdate.Value = Convert.ToDateTime(temp_date);

                            txtJobCard.Text = StockControl.dbClss.TSt(g.FirstOrDefault().JobCard);
                            txtCustomerName.Text = StockControl.dbClss.TSt(g.FirstOrDefault().CustomerName);
                            txtAddress.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Address);
                            txtCodeNo.Text = StockControl.dbClss.TSt(g.FirstOrDefault().CodeNo);
                            txtItemDesc.Text = StockControl.dbClss.TSt(g.FirstOrDefault().ItemDesc);
                            txtItemName.Text = StockControl.dbClss.TSt(g.FirstOrDefault().ItemName);
                            txtUnit.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Unit);
                            txtQty.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Qty);
                            txtLotNo.Text = StockControl.dbClss.TSt(g.FirstOrDefault().LotNo);
                            ddlType.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Type);
                            txtAmount.Text = StockControl.dbClss.TDe(g.FirstOrDefault().Amount).ToString("N2");
                            txtUnitCost.Text = StockControl.dbClss.TDe(g.FirstOrDefault().UnitCost).ToString("N2");

                            txtCreateBy.Text = StockControl.dbClss.TSt(g.FirstOrDefault().CreateBy);
                            if (!StockControl.dbClss.TSt(g.FirstOrDefault().ModifyBy).Equals(""))
                                txtCreateBy.Text = StockControl.dbClss.TSt(g.FirstOrDefault().ModifyBy);
                            if (!StockControl.dbClss.TSt(g.FirstOrDefault().CreateDate).Equals(""))
                            {
                                if (!StockControl.dbClss.TSt(g.FirstOrDefault().ModifyDate).Equals(""))
                                    txtCreateDate.Text = Convert.ToDateTime((g.FirstOrDefault().ModifyDate),new CultureInfo("en-US")).ToString("dd/MMM/yyyy");
                                else
                                    txtCreateDate.Text = Convert.ToDateTime((g.FirstOrDefault().CreateDate),new CultureInfo("en-US")).ToString("dd/MMM/yyyy");
                            }
                            else
                                txtCreateDate.Text = "";

                            //Get Cost Calim or Cost Addition
                            Get_CostCalim();

                            //lblStatus.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Status);
                            if (StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Cancel"))
                            {
                                btnSave.Enabled = false;
                                btnDelete.Enabled = false;
                                btnView.Enabled = false;
                                btnEdit.Enabled = false;
                                lblStatus.Text = "Cancel";
                                dgvData.ReadOnly = true;
                                btnNew.Enabled = true;
                                //btnDel_Item.Enabled = false;
                                ddlType.ReadOnly = true;
                                btnCal.Enabled = false;
                            }
                            else if
                                (StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Partial"))
                            {

                                btnSave.Enabled = false;
                                btnDelete.Enabled = false;
                                btnView.Enabled = false;
                                btnEdit.Enabled = false;
                                lblStatus.Text = "Partial";
                                dgvData.ReadOnly = true;
                                btnNew.Enabled = true;
                                ddlType.ReadOnly = true;
                                //btnDel_Item.Enabled = false;
                                btnCal.Enabled = true;
                            }
                            else if
                                (StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Completed")
                                || StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Discon")
                                //|| StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Partial")                               
                                )

                            {
                                btnSave.Enabled = false;
                                btnDelete.Enabled = false;
                                btnView.Enabled = false;
                                btnEdit.Enabled = false;
                                lblStatus.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Status);
                                dgvData.ReadOnly = true;
                                btnNew.Enabled = true;
                                //btnDel_Item.Enabled = false;
                                ddlType.ReadOnly = true;
                                btnCal.Enabled = false;
                            }
                            else
                            {
                                btnNew.Enabled = true;
                                btnSave.Enabled = false;
                                btnDelete.Enabled = false;
                                btnView.Enabled = false;
                                btnEdit.Enabled = true;
                                lblStatus.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Status);
                                dgvData.ReadOnly = false;
                                ddlType.ReadOnly = false;
                                btnCal.Enabled = true;
                            }
                            dt_h = StockControl.dbClss.LINQToDataTable(g);


                            //shipping
                            var d = (from a in db.tb_Shippings
                                     join b in db.tb_ShippingHs on a.ShippingNo equals b.ShippingNo
                                     join c in db.tb_Items on a.CodeNo equals c.CodeNo

                                     where (a.Status != "Cancel")
                                     && (b.Status != "Cancel")
                                     && (b.JobCard == (txtJobCard.Text.Trim()))
                                     //&& (b.TempJobCard == txtTempJobCard.Text.Trim())
                                     select new
                                     {
                                         CodeNo = a.CodeNo,
                                         ItemName = a.ItemNo,
                                         ItemDesc = a.ItemDescription,
                                         GroupCode = c.GroupCode,
                                         QTY = a.QTY,
                                         Cost = a.UnitCost,
                                         Amount = a.QTY * a.UnitCost,
                                         Unit = a.UnitShip,
                                         PCSUnit = a.PCSUnit,
                                         LotNo = a.LotNo,
                                         Remark = a.Remark,
                                         ShipBy = b.ShipName,
                                         ShipDate = b.ShipDate,
                                         PONo = "",
                                         PRNo = "",
                                         Status = ""
                                         

                                     }//.Where(ab => ab.VendorNo.Contains(Vendorno))
                             ).ToList();

                            //var d = (from ix in db.tb_Shippings select ix)
                            //.Where(a => a.ShippingNo == txtSHNo.Text.Trim()
                            //    && a.Status != "Cancel").ToList();
                            if (d.Count() > 0)
                            {
                                int c = 0;
                                dgvData.DataSource = d;
                                //dt_d = StockControl.dbClss.LINQToDataTable(d);

                                int id = 0;
                                foreach (var x in dgvData.Rows)
                                {
                                    c += 1;
                                    x.Cells["dgvNo"].Value = c;
                                }
                            }

                            //Ship AVG
                            //shipping
                            dgvShipAVG.Rows.Clear();
                            var av = (from a in db.tb_Shipping_JobCardAvgs
                                     //join b in db.tb_Shipping_JobCardAvgHs on a.ShippingNo equals b.ShippingNo
                                     join c in db.tb_Items on a.CodeNo equals c.CodeNo

                                     where (a.Status != "Cancel")
                                    
                                     && (a.JobCard == (txtJobCard.Text.Trim()))
                                     && (a.TempJobCard == (txtTempJobCard.Text.Trim()))
                                     
                                      select new
                                     {
                                         CodeNo = a.CodeNo,
                                         ItemName = c.ItemNo,
                                         ItemDesc = c.ItemDescription,
                                         GroupCode = c.GroupCode,
                                         QTY = a.Qty,
                                         UnitCost = a.UnitCost,
                                         Amount = a.Qty * a.UnitCost,
                                         Unit = c.UnitShip,
                                         PCSUnit = a.PCSUnit,
                                         id = a.id,
                                         Remark = a.Remark,
                                         YYYY = a.YYYY,
                                         MM = a.MM,
                                         Status = a.Status


                                     }//.Where(ab => ab.VendorNo.Contains(Vendorno))
                             ).ToList();

                            //var d = (from ix in db.tb_Shippings select ix)
                            //.Where(a => a.ShippingNo == txtSHNo.Text.Trim()
                            //    && a.Status != "Cancel").ToList();
                            if (av.Count() > 0)
                            {
                                int c = 0;
                                dgvShipAVG.DataSource = av;
                                //dt_d = StockControl.dbClss.LINQToDataTable(d);

                                //int id = 0;
                                foreach (var x in dgvShipAVG.Rows)
                                {
                                    c += 1;
                                    x.Cells["dgvNo"].Value = c;
                                }
                            }

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
            txtJobCard.Text = "";
            txtTempJobCard.Text = "";
            txtRemark.Text = "";
            txtCodeNoBarcode.Text = "";
            dtDuedate.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
            dtPOdate.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
            dtPOdate.SetToNullValue();
            dtDuedate.SetToNullValue();
            ddlType.Text = "Normal";
            txtCreateBy.Text = ClassLib.Classlib.User;
           // txtSHName.Text = ClassLib.Classlib.User;
            txtCreateDate.Text = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy");
            lblStatus.Text = "-";
            txtQty.Text = "";
            txtCustomerName.Text = "";
            txtAddress.Text = "";
            txtCodeNo.Text = "";
            txtUnit.Text = "";
            txtLotNo.Text = "";
            txtItemDesc.Text = "";
            txtItemName.Text = "";
            dgvData.Rows.Clear();
            dgvData.DataSource = null;
            dgvShipAVG.Rows.Clear();
            dgvShipAVG.DataSource = null;
            txtUnitCost.Text = "0.00";
            txtAmount.Text = "0.00";
            txtCal_Amount_New.Text = "0.00";
            txtCal_UnitCost_New.Text = "0.00";
            dt_h.Rows.Clear();
            
        }
      private void Enable_Status(bool ss,string Condition)
        {
            if (Condition.Equals("-") || Condition.Equals("New"))
            {
                txtCodeNoBarcode.Enabled = ss;
                txtJobCard.Enabled = ss;
                txtCustomerName.Enabled = ss;
                txtRemark.Enabled = ss;
                txtAddress.Enabled = ss;
                dtDuedate.Enabled = ss;
                /*dgvData.ReadOnly = false;*/
                //.Enabled = ss;
                dtDuedate.Enabled = ss;
                dtPOdate.Enabled = ss;
                txtCodeNo.Enabled = ss;
                txtQty.Enabled = ss;
                txtLotNo.Enabled = ss;
            }
            else if (Condition.Equals("View"))
            {
                txtCodeNoBarcode.Enabled = ss;
                txtJobCard.Enabled = ss;
                txtCustomerName.Enabled = ss;
                txtRemark.Enabled = ss;
                txtAddress.Enabled = ss;
                dtDuedate.Enabled = ss;
                /*dgvData.ReadOnly = false;*/
                //btnDel_Item.Enabled = ss;
                dtDuedate.Enabled = ss;
                dtPOdate.Enabled = ss;
                txtCodeNo.Enabled = ss;
                txtQty.Enabled = ss;
                txtLotNo.Enabled = ss;
            }

            else if (Condition.Equals("Edit"))
            {
                txtCodeNoBarcode.Enabled = ss;
                txtJobCard.Enabled = ss;
                txtCustomerName.Enabled = ss;
                txtRemark.Enabled = ss;
                txtAddress.Enabled = ss;
                dtDuedate.Enabled = ss;
                /*dgvData.ReadOnly = false;*/
                //btnDel_Item.Enabled = ss;
                dtDuedate.Enabled = ss;
                dtPOdate.Enabled = ss;
                txtCodeNo.Enabled = ss;
                txtQty.Enabled = ss;
                txtLotNo.Enabled = ss;
            }
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            btnDelete.Enabled = true;
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            btnView.Enabled = true;
            btnEdit.Enabled = false;

            ClearData();
            Enable_Status(true, "New");
            lblStatus.Text = "New";
            Ac = "New";

           // getมาไว้ก่อน แต่ยังไมได้ save
            txtTempJobCard.Text = StockControl.dbClss.GetNo(13, 0);
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
                if (ddlType.Text.Equals(""))
                    err += "- “ประเภท:” เป็นค่าว่าง \n";
                if (txtJobCard.Text.Equals(""))
                    err += "- “Job card:” เป็นค่าว่าง \n";
                if (txtCustomerName.Text.Equals(""))
                    err += "- “ชื่อลูกค้า:” เป็นค่าว่าง \n";
                if (txtAddress.Text.Equals(""))
                    err += "- “ที่อยู่:” เป็นค่าว่าง \n";
                if (dtDuedate.Text.Equals(""))
                    err += "- “วันที่ส่งสินค้า:” เป็นค่าว่าง \n";
                if (dtPOdate.Text.Equals(""))
                    err += "- “วันที่สั่งสินค้า:” เป็นค่าว่าง \n";
                if (txtCodeNo.Text.Equals(""))
                    err += "- “รหัสสินค้า:” เป็นค่าว่าง \n";
                if (txtQty.Text.Equals(""))
                    err += "- “จำนวนสินค้า:” เป็นค่าว่าง \n";
                else
                {
                    if(dbClss.TDe(txtQty.Text)<0)
                        err += "- “จำนวนสินค้า น้อยกว่า 0 :” ไม่ได้ \n";
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
        private void SaveHerder()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                var g = (from ix in db.tb_JobCards
                         where ix.TempJobCard.Trim() == txtTempJobCard.Text.Trim()                             
                            && ix.Status != "Cancel"
                            && ix.Status != "Completed"
                            && ix.Status != "Discon"
                            && ix.Status != "Partial"

                         select ix).ToList();
                if (g.Count > 0)  //มีรายการในระบบ
                {
                    foreach (DataRow row in dt_h.Rows)
                    {
                        var gg = (from ix in db.tb_JobCards
                                  where ix.TempJobCard.Trim() == txtTempJobCard.Text.Trim() 
                                  && ix.Status != "Cancel"
                                   && ix.Status != "Completed"
                                   && ix.Status != "Partial"
                                   && ix.Status != "Discon"
                                  select ix).First();

                        gg.ModifyBy = ClassLib.Classlib.User;
                        gg.ModifyDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                        dbClss.AddHistory(this.Name, "แก้ไขใบร้องขอการผลิต", "แก้ไขโดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", txtTempJobCard.Text);

                        //if (StockControl.dbClss.TSt(gg.BarCode).Equals(""))
                        //    gg.BarCode = StockControl.dbClss.SaveQRCode2D(txtTempJobCard.Text.Trim());


                        if (!txtJobCard.Text.Trim().Equals(row["JobCard"].ToString()))
                        {
                            gg.JobCard = txtJobCard.Text.Trim();
                            dbClss.AddHistory(this.Name, "แก้ไขใบร้องขอการผลิต", "แก้ไข Job Card [" + txtJobCard.Text.Trim() + " เดิม :" + row["JobCard"].ToString() + "]", txtTempJobCard.Text);
                        }
                        if (!txtCustomerName.Text.Trim().Equals(row["CustomerName"].ToString()))
                        {
                            gg.CustomerName = txtCustomerName.Text.Trim();
                            dbClss.AddHistory(this.Name, "แก้ไขใบร้องขอการผลิต", "แก้ไข ชื่อลูกค้า [" + txtCustomerName.Text.Trim() + " เดิม :" + row["CustomerName"].ToString() + "]", txtTempJobCard.Text);
                        }
                        if (!txtAddress.Text.Trim().Equals(row["Address"].ToString()))
                        {
                            gg.Address = txtAddress.Text.Trim();
                            dbClss.AddHistory(this.Name, "แก้ไขใบร้องขอการผลิต", "แก้ไข ที่อยู่ลูกค้า [" + txtAddress.Text.Trim() + " เดิม :" + row["Address"].ToString() + "]", txtTempJobCard.Text);
                        }

                        
                      
                        if (!dtDuedate.Text.Trim().Equals(""))
                        {
                            string date1 = "";
                            date1 = dtDuedate.Value.ToString("yyyyMMdd", new CultureInfo("en-US"));
                            string date2 = "";
                            DateTime temp = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                            if (!StockControl.dbClss.TSt(row["Duedate"].ToString()).Equals(""))
                            {

                                temp = Convert.ToDateTime(row["Duedate"]);
                                date2 = temp.ToString("yyyyMMdd", new CultureInfo("en-US"));

                            }
                            if (!date1.Equals(date2))
                            {
                                DateTime? Duedate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                if (!dtDuedate.Text.Equals(""))
                                    Duedate = dtDuedate.Value;
                                gg.Duedate = Duedate;
                                dbClss.AddHistory(this.Name, "แก้ไขใบร้องขอการผลิต", "แก้ไข วันที่ส่งสินค้า [" + dtDuedate.Text.Trim() + " เดิม :" + temp.ToString() + "]", txtTempJobCard.Text);
                            }
                        }

                        if (!dtPOdate.Text.Trim().Equals(""))
                        {
                            string date1 = "";
                            date1 = dtPOdate.Value.ToString("yyyyMMdd", new CultureInfo("en-US"));
                            string date2 = "";
                            DateTime temp = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                            if (!StockControl.dbClss.TSt(row["PODate"].ToString()).Equals(""))
                            {

                                temp = Convert.ToDateTime(row["PODate"]);
                                date2 = temp.ToString("yyyyMMdd", new CultureInfo("en-US"));

                            }
                            if (!date1.Equals(date2))
                            {
                                DateTime? PODate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                if (!dtPOdate.Text.Equals(""))
                                    PODate = dtPOdate.Value;
                                gg.PODate = PODate;
                                dbClss.AddHistory(this.Name, "แก้ไขใบร้องขอการผลิต", "แก้ไข วันที่สั่งสินค้า(PODate) [" + dtPOdate.Text.Trim() + " เดิม :" + temp.ToString() + "]", txtTempJobCard.Text);
                            }
                        }


                        if (!txtCodeNo.Text.Trim().Equals(row["CodeNo"].ToString()))
                        {
                            gg.CodeNo = txtCodeNo.Text.Trim();
                            gg.ItemDesc = txtItemDesc.Text.Trim();
                            gg.ItemName = txtItemName.Text.Trim();
                            gg.Unit = txtUnit.Text.Trim();
                            dbClss.AddHistory(this.Name, "แก้ไขใบร้องขอการผลิต", "แก้ไข รหัสสินค้า [" + txtCodeNo.Text.Trim() + " เดิม :" + row["CodeNo"].ToString() + "]", txtTempJobCard.Text);
                        }
                        if (!txtQty.Text.Trim().Equals(row["Qty"].ToString()))
                        {
                            gg.Qty = dbClss.TDe( txtQty.Text);
                            gg.RemainQty = dbClss.TDe(txtQty.Text);
                            dbClss.AddHistory(this.Name, "แก้ไขใบร้องขอการผลิต", "แก้ไข จำนวนสินค้า [" + txtQty.Text.Trim() + " เดิม :" + row["Qty"].ToString() + "]", txtTempJobCard.Text);
                        }
                        if (!txtLotNo.Text.Trim().Equals(row["LotNo"].ToString()))
                        {
                            gg.LotNo = txtLotNo.Text.Trim();
                            dbClss.AddHistory(this.Name, "แก้ไขใบร้องขอการผลิต", "แก้ไข ลอต(Lot) [" + txtLotNo.Text.Trim() + " เดิม :" + row["LotNo"].ToString() + "]", txtTempJobCard.Text);
                        }
                        if (!txtRemark.Text.Trim().Equals(row["Remark"].ToString()))
                        {
                            gg.Remark = txtRemark.Text.Trim();
                            dbClss.AddHistory(this.Name, "แก้ไขใบร้องขอการผลิต", "แก้ไข หมายเหตุ [" + txtRemark.Text.Trim() + " เดิม :" + row["Remark"].ToString() + "]", txtTempJobCard.Text);
                        }
                        if (!ddlType.Text.Trim().Equals(row["Type"].ToString()))
                        {
                            gg.Type = ddlType.Text;
                            dbClss.AddHistory(this.Name, "แก้ไขใบร้องขอการผลิต", "แก้ไข ประเภท [" + ddlType.Text.Trim() + " เดิม :" + row["Type"].ToString() + "]", txtTempJobCard.Text);
                        }

                        gg.UnitCost = dbClss.TDe(txtUnitCost.Text);
                        gg.Amount = dbClss.TDe(txtAmount.Text);
                        

                        db.SubmitChanges();
                    }
                }
                else //สร้างใหม่
                {
                    byte[] barcode = null;
                        //barcode = StockControl.dbClss.SaveQRCode2D(txtTempJobCard.Text.Trim());
                    //DateTime? UpdateDate = null;

                    DateTime? Duedate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    if (!dtDuedate.Text.Equals(""))
                        Duedate = dtDuedate.Value;
                    DateTime? PODate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    if (!dtPOdate.Text.Equals(""))
                        PODate = dtPOdate.Value;


                    tb_JobCard gg = new tb_JobCard();
                    gg.TempJobCard = txtTempJobCard.Text;
                    gg.JobCard = txtJobCard.Text.Trim();
                    gg.ModifyBy = ClassLib.Classlib.User;               
                    gg.ModifyDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    gg.CreateBy = ClassLib.Classlib.User;
                    gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    gg.CustomerName = txtCustomerName.Text;
                    gg.Address = txtAddress.Text;
                    gg.CodeNo = txtCodeNo.Text;
                    gg.ItemName = txtItemName.Text;
                    gg.ItemDesc = txtItemDesc.Text;
                    gg.Unit = txtUnit.Text;
                    gg.PODate = PODate;
                    gg.Duedate = Duedate;
                    gg.LotNo = txtLotNo.Text;
                    gg.Remark = txtRemark.Text;
                    gg.Qty = dbClss.TDe(txtQty.Text);
                    gg.RemainQty = dbClss.TDe(txtQty.Text);
                    gg.BarCode = barcode;
                    gg.Status = "Waiting";
                    gg.UnitCost = dbClss.TDe(txtUnitCost.Text);
                    gg.Amount = dbClss.TDe(txtAmount.Text);
                    gg.Type = ddlType.Text;

                    db.tb_JobCards.InsertOnSubmit(gg);
                    db.SubmitChanges();

                    dbClss.AddHistory(this.Name, "แก้ไขใบร้องขอการผลิต", "สร้างใบร้องขอการผลิต [" + txtTempJobCard.Text.Trim() + "]", txtTempJobCard.Text);
                }
            }
        }
       
        private void btnSave_Click(object sender, EventArgs e)
        {

            if (Ac.Equals("New") || Ac.Equals("Edit"))
            {
                if (Check_Save())
                    return;
                if (MessageBox.Show("ต้องการบันทึก ?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Cursor = Cursors.WaitCursor;

                    if (Ac.Equals("New"))
                    {

                        //ถ้ามีการใส่เลขที่ PR เช็คดูว่ามีการใส่เลขนี้แล้วหรือไม่ ถ้ามีให้ใส่เลขอื่น
                        if (!txtJobCard.Text.Equals(""))
                        {
                            using (DataClasses1DataContext db = new DataClasses1DataContext())
                            {
                                var p = (from ix in db.tb_JobCards
                                         where ix.JobCard.ToUpper().Trim() == txtJobCard.Text.Trim().ToUpper()
                                         && ix.Status != "Cancel"
                                         //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                                         select ix).ToList();
                                if (p.Count > 0)  //มีรายการในระบบ
                                {
                                    MessageBox.Show("เลข Job Card ถูกใช้ไปแล้ว  กรุณาใส่เลขใหม่");
                                    return;
                                }
                            }
                        }

                        txtTempJobCard.Text = StockControl.dbClss.GetNo(13, 2);
                        
                    }
                    if (!txtTempJobCard.Text.Equals(""))
                    {
                        SaveHerder();
                       

                        DataLoad();
                        btnNew.Enabled = true;
                        //btnDel_Item.Enabled = false;
                        

                        MessageBox.Show("บันทึกสำเร็จ!");
                        btnRefresh_Click(null, null);
                    }
                    else
                    {
                        MessageBox.Show("ไม่สามารถโหลดเลขที่รับสินค้าได้ ติดต่อแผนก IT");
                    }
                }
            }
            else
                MessageBox.Show("สถานะต้องเป็น New หรือ Edit เท่านั่น");
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
        private void Insert_Stock()
        {
            try
            {

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    DateTime? CalDate = null;
                    DateTime? AppDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    int Seq = 0;
                    
                    

                    string CNNo = CNNo = StockControl.dbClss.GetNo(6, 2);
                    var g = (from ix in db.tb_Shippings
                                 //join i in db.tb_Items on ix.CodeNo equals i.CodeNo
                             where ix.ShippingNo.Trim() == txtTempJobCard.Text.Trim() && ix.Status != "Cancel"

                             select ix).ToList();
                    if (g.Count > 0)
                    {
                        //insert Stock

                        foreach (var vv in g)
                        {
                            Seq += 1;

                            tb_Stock1 gg = new tb_Stock1();
                            gg.AppDate = AppDate;
                            gg.Seq = Seq;
                            gg.App = "Shipping";
                            gg.Appid = Seq;
                            gg.CreateBy = ClassLib.Classlib.User;
                            gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                            gg.DocNo = CNNo;
                            gg.RefNo = txtTempJobCard.Text;
                            gg.Type = "Ship";
                            gg.QTY = -Convert.ToDecimal(vv.QTY);
                            gg.Inbound = 0;
                            gg.Outbound = -Convert.ToDecimal(vv.QTY); ;
                            gg.AmountCost = (-Convert.ToDecimal(vv.QTY)) * get_cost(vv.CodeNo);
                            gg.UnitCost = get_cost(vv.CodeNo);
                            gg.RemainQty = 0;
                            gg.RemainUnitCost = 0;
                            gg.RemainAmount = 0;
                            gg.CalDate = CalDate;
                            gg.Status = "Active";

                            db.tb_Stock1s.InsertOnSubmit(gg);
                            db.SubmitChanges();

                            dbClss.Insert_Stock(vv.CodeNo, (-Convert.ToDecimal(vv.QTY)), "Shipping", "Inv");


                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        //private void InsertStock_new()
        //{
        //    try
        //    {

        //        using (DataClasses1DataContext db = new DataClasses1DataContext())
        //        {
        //            DateTime? CalDate = null;
        //            DateTime? AppDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
        //            int Seq = 0;
        //            string Type = "Shipping";
        //            string Category = ""; //Temp,Invoice
        //            decimal Cost = 0;
        //           // int Flag_ClearTemp = 0;
        //            decimal Qty_Inv = 0;
        //            decimal Qty_DL = 0;
        //            decimal Qty_Remain = 0;
        //            decimal QTY = 0;
        //            decimal QTY_temp = 0;

        //            string Type_in_out = "Out";
        //            decimal RemainQty = 0;
        //            decimal Amount = 0;
        //            decimal RemainAmount = 0;
        //            decimal Avg = 0;
        //            decimal UnitCost = 0;
        //            decimal sum_Remain = 0;
        //            decimal sum_Qty = 0;

        //            //string CNNo = CNNo = StockControl.dbClss.GetNo(6, 2);
        //            var g = (from ix in db.tb_Shippings
        //                         //join i in db.tb_Items on ix.CodeNo equals i.CodeNo
        //                     where ix.ShippingNo.Trim() == txtTempJobCard.Text.Trim() && ix.Status != "Cancel"

        //                     select ix).ToList();
        //            if (g.Count > 0)
        //            {
        //                //insert Stock

        //                foreach (var vv in g)
        //                {
        //                    Seq += 1;

        //                    QTY = Convert.ToDecimal(vv.QTY);
        //                    QTY_temp = 0;
        //                    Qty_Remain = (Convert.ToDecimal(db.Cal_QTY(vv.CodeNo, "", 0)));  //sum ทั้งหมด
        //                    Qty_Inv = (Convert.ToDecimal(db.Cal_QTY(vv.CodeNo, "Invoice", 0))); //sum เฉพาะ Invoice
        //                    Qty_DL = (Convert.ToDecimal(db.Cal_QTY(vv.CodeNo, "Temp", 0))); // sum เฉพาะ DL
                            
        //                    if (QTY <= Qty_Remain)
        //                    {                               

        //                        if (Qty_Inv >= QTY) //ถ้า จำนวน remain มีมากกว่าจำนวนที่จะลบ
        //                        {
        //                            UnitCost = Convert.ToDecimal(vv.UnitCost);
        //                            //if (UnitCost <= 0)
        //                            //    UnitCost = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "Avg"));

        //                            Amount = (-QTY) * UnitCost;

        //                            //แบบที่ 1 จะไป sum ใหม่
        //                            RemainQty = (Convert.ToDecimal(db.Cal_QTY(vv.CodeNo, "", 0)));
        //                            //แบบที่ 2 จะไปดึงล่าสุดมา
        //                            //RemainQty = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainQty"));
        //                            sum_Remain = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainAmount",))
        //                                + Amount;

        //                            sum_Qty = RemainQty + (-QTY);
        //                            Avg = UnitCost;//sum_Remain / sum_Qty;
        //                            RemainAmount = sum_Remain;


        //                            Category = "Invoice";
        //                            tb_Stock gg = new tb_Stock();
        //                            gg.AppDate = AppDate;
        //                            gg.Seq = Seq;
        //                            gg.App = "Shipping";
        //                            gg.Appid = Seq;
        //                            gg.CreateBy = ClassLib.Classlib.User;
        //                            gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
        //                            gg.DocNo = txtTempJobCard.Text;
        //                            gg.RefNo = "";
        //                            gg.CodeNo = vv.CodeNo;
        //                            gg.Type = Type;
        //                            gg.QTY = -Convert.ToDecimal(QTY);
        //                            gg.Inbound = 0;
        //                            gg.Outbound = -Convert.ToDecimal(QTY);
        //                            gg.Type_i = 3;  //Receive = 1,Cancel Receive 2,Shipping = 3,Cancel Shipping = 4,Adjust stock = 5,ClearTemp = 6
        //                            gg.Category = Category;
        //                            gg.Refid = vv.id;
                                    
        //                            gg.CalDate = CalDate;
        //                            gg.Status = "Active";
        //                            gg.Flag_ClearTemp =0; //0 คือ invoice,1 คือ Temp , 2 คือ clear temp แล้ว
        //                            gg.Type_in_out = Type_in_out;
        //                            gg.AmountCost = Amount;
        //                            gg.UnitCost = UnitCost;
        //                            gg.RemainQty = sum_Qty;
        //                            gg.RemainUnitCost = 0;
        //                            gg.RemainAmount = RemainAmount;
        //                            gg.Avg = Avg;


        //                            db.tb_Stocks.InsertOnSubmit(gg);
        //                            db.SubmitChanges();

        //                            dbClss.AddHistory(this.Name, "เบิกสินค้า", " เบิกสินค้าเลขที่ : " + txtTempJobCard.Text + " เบิก : " + Category + " CodeNo : " + vv.CodeNo + " จำนวน : " + (-QTY).ToString() + " โดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", txtTempJobCard.Text);

        //                        }
        //                        else
        //                        {
        //                            QTY_temp = QTY - Qty_Inv;

        //                            UnitCost = Convert.ToDecimal(vv.UnitCost);
        //                            //if (UnitCost <= 0)
        //                            //    UnitCost = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "Avg"));
                                    
        //                            Amount = (-QTY) * UnitCost;

        //                            //แบบที่ 1 จะไป sum ใหม่
        //                            RemainQty = (Convert.ToDecimal(db.Cal_QTY(vv.CodeNo, "", 0)));
        //                            //แบบที่ 2 จะไปดึงล่าสุดมา
        //                            //RemainQty = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainQty"));
        //                            sum_Remain = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainAmount"))
        //                                + Amount;

        //                            sum_Qty = RemainQty + (-QTY);
        //                            Avg = UnitCost;//sum_Remain / sum_Qty;
        //                            RemainAmount = sum_Remain;

        //                            Category = "Temp";
        //                            tb_Stock gg = new tb_Stock();
        //                            gg.AppDate = AppDate;
        //                            gg.Seq = Seq;
        //                            gg.App = "Shipping";
        //                            gg.Appid = Seq;
        //                            gg.CreateBy = ClassLib.Classlib.User;
        //                            gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
        //                            gg.DocNo = txtTempJobCard.Text;
        //                            gg.RefNo = "";
        //                            gg.CodeNo = vv.CodeNo;
        //                            gg.Type = Type;
        //                            gg.QTY = -Convert.ToDecimal(QTY);
        //                            gg.Inbound = 0;
        //                            gg.Outbound = -Convert.ToDecimal(QTY);
        //                            gg.Type_i = 3;  //Receive = 1,Cancel Receive 2,Shipping = 3,Cancel Shipping = 4,Adjust stock = 5,ClearTemp = 6
        //                            gg.Category = Category;
        //                            gg.Refid = vv.id;
                                    
        //                            gg.CalDate = CalDate;
        //                            gg.Status = "Active";
        //                            gg.Flag_ClearTemp = 1; //0 คือ invoice,1 คือ Temp , 2 คือ clear temp แล้ว
        //                            gg.Type_in_out = Type_in_out;
        //                            gg.AmountCost = Amount;
        //                            gg.UnitCost = UnitCost;
        //                            gg.RemainQty = sum_Qty;
        //                            gg.RemainUnitCost = 0;
        //                            gg.RemainAmount = RemainAmount;
        //                            gg.Avg = Avg;

        //                            db.tb_Stocks.InsertOnSubmit(gg);
        //                            db.SubmitChanges();
        //                            dbClss.AddHistory(this.Name, "เบิกสินค้า", " เบิกสินค้าเลขที่ : " + txtTempJobCard.Text + " เบิก : " + Category + " CodeNo : " + vv.CodeNo + " จำนวน : " + (-QTY).ToString() + " โดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", txtTempJobCard.Text);


        //                            //// --Stock ใน Invoice ไม่พอ ต้องเอาที่ Temp มา

        //                            //UnitCost = Convert.ToDecimal(vv.UnitCost);
        //                            //if (UnitCost <= 0)
        //                            //    UnitCost = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "Avg"));

        //                            //Amount = (-QTY_temp) * UnitCost;

        //                            ////แบบที่ 1 จะไป sum ใหม่
        //                            //RemainQty = (Convert.ToDecimal(db.Cal_QTY(vv.CodeNo, "", 0)));
        //                            ////แบบที่ 2 จะไปดึงล่าสุดมา
        //                            ////RemainQty = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainQty"));
        //                            //sum_Remain = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainAmount"))
        //                            //    + Amount;
        //                            //sum_Qty = RemainQty + (-QTY_temp);
        //                            //Avg = UnitCost;//sum_Remain / sum_Qty;
        //                            //RemainAmount = sum_Remain;

        //                            //Category = "Invoice";
        //                            //tb_Stock aa = new tb_Stock();
        //                            //aa.AppDate = AppDate;
        //                            //aa.Seq = Seq;
        //                            //aa.App = "Shipping";
        //                            //aa.Appid = Seq;
        //                            //aa.CreateBy = ClassLib.Classlib.User;
        //                            //aa.CreateDate = Convert.ToDateTime( DateTime.Now, new CultureInfo("en-US"));
        //                            //aa.DocNo = txtSHNo.Text;
        //                            //aa.RefNo = "";
        //                            //aa.CodeNo = vv.CodeNo;
        //                            //aa.Type = Type;
        //                            //aa.QTY = -Convert.ToDecimal(QTY_temp);
        //                            //aa.Inbound = 0;
        //                            //aa.Outbound = -Convert.ToDecimal(QTY_temp);
        //                            //aa.Type_i = 3;  //Receive = 1,Cancel Receive 2,Shipping = 3,Cancel Shipping = 4,Adjust stock = 5,ClearTemp = 6
        //                            //aa.Category = Category;
        //                            //aa.Refid = vv.id;

        //                            //aa.CalDate = CalDate;
        //                            //aa.Status = "Active";
        //                            //aa.Flag_ClearTemp = 1; //0 คือ invoice,1 คือ Temp , 2 คือ clear temp แล้ว
        //                            //aa.Type_in_out = Type_in_out;
        //                            //aa.AmountCost = Amount;
        //                            //aa.UnitCost = UnitCost;
        //                            //aa.RemainQty = sum_Qty;
        //                            //aa.RemainUnitCost = 0;
        //                            //aa.RemainAmount = RemainAmount;
        //                            //aa.Avg = Avg;

        //                            //db.tb_Stocks.InsertOnSubmit(aa);
        //                            //db.SubmitChanges();
        //                            //dbClss.AddHistory(this.Name, "เบิกสินค้า", " เบิกสินค้าเลขที่ : " + txtSHNo.Text + " เบิก : " + Category + " CodeNo : " + vv.CodeNo + " จำนวน : " + (-QTY_temp).ToString() + " โดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", txtSHNo.Text);

        //                        }

        //                    }

        //                    //update Stock เข้า item
        //                    db.sp_010_Update_StockItem(Convert.ToString(vv.CodeNo), "");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex) { MessageBox.Show(ex.Message); }
        //}
        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            //try
            //{
            //    dgvData.EndEdit();
            //    if (e.RowIndex >= -1)
            //    {

            //        if (dgvData.Columns["QTY"].Index == e.ColumnIndex)
            //        {
            //            decimal QTY = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["QTY"].Value), out QTY);
            //            decimal RemainQty = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["RemainQty"].Value), out RemainQty);
            //            if (QTY > RemainQty)
            //            {
            //                MessageBox.Show("ไม่สามารถรับเกินจำนวนคงเหลือได้");
            //                e.Row.Cells["QTY"].Value = 0;
            //            }
            //        }

            //        if (dgvData.Columns["QTY"].Index == e.ColumnIndex
            //            || dgvData.Columns["StandardCost"].Index == e.ColumnIndex
            //            )
            //        {
            //            decimal QTY = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["QTY"].Value), out QTY);
            //            decimal CostPerUnit = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["StandardCost"].Value), out CostPerUnit);
            //            e.Row.Cells["Amount"].Value = QTY * CostPerUnit;
            //            Cal_Amount();
            //        }
            //    }
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void Cal_Amount()
        {
            //if (dgvData.Rows.Count() > 0)
            //{
            //    decimal Amount = 0;
            //    decimal Total = 0;
            //    foreach (var rd1 in dgvData.Rows)
            //    {
            //        Amount = StockControl.dbClss.TDe(rd1.Cells["Amount"].Value);
            //        Total += Amount;
            //    }
            //    txtTotal.Text = Total.ToString("###,###,##0.00");
            //}
        }
        private void Unit_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // MessageBox.Show(e.KeyCode.ToString());
        }

        private void radGridView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }



        int row = -1;
        private void radGridView1_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            row = e.RowIndex;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ////dbClss.ExportGridCSV(radGridView1);
            //dbClss.ExportGridXlSX(dgvData);
        }

      
        private void btnFilter1_Click(object sender, EventArgs e)
        {
            //dgvData.EnableFiltering = true;
        }

        private void btnUnfilter1_Click(object sender, EventArgs e)
        {
            //dgvData.EnableFiltering = false;
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
            btnView.Enabled = false;
            btnEdit.Enabled = true;
            btnNew.Enabled = true;
            btnSave.Enabled = false;
            btnDelete.Enabled = false;

            Enable_Status(false, "View");
            //lblStatus.Text = "View";
            Ac = "View";
            
            DataLoad();
            
           
        }

        private void txtCodeNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    txtCodeNo.Text = txtCodeNoBarcode.Text;
                    Load_CodeNO(txtCodeNo.Text);
                    txtCodeNoBarcode.Text = "";

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private bool Duppicate(string CodeNo)
        {
            bool re = false;
            //dgvData.EndEdit();
            //foreach (var g in dgvData.Rows)
            //{
            //    if(Convert.ToString(g.Cells["CodeNo"].Value).Equals(CodeNo))
            //    {
            //        re = true;
            //        MessageBox.Show("รหัสพาร์ทซ้ำ");
            //        break;
            //    }
            //}

            return re;
        }
       

        private void dgvData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                btnDel_Item_Click(null, null);
        }

        private void btnDel_Item_Click(object sender, EventArgs e)
        {
            //try
            //{

            //    if (dgvData.Rows.Count < 0)
            //        return;


            //    if (Ac.Equals("New"))// || Ac.Equals("Edit"))
            //    {
            //        this.Cursor = Cursors.WaitCursor;

            //        int id = 0;
            //        int.TryParse(StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["id"].Value), out id);
            //        if (id <= 0)
            //            dgvData.Rows.Remove(dgvData.CurrentRow);

            //        else
            //        {
            //            string CodeNo = ""; StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["CodeNo"]);
            //            if (MessageBox.Show("ต้องการลบรายการ ( " + CodeNo + " ) ออกจากรายการ หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //            {
            //                dgvData.CurrentRow.IsVisible = false;
            //            }
            //        }
            //        SetRowNo1(dgvData);
            //    }
            //    else
            //    {
            //        MessageBox.Show("ไม่สามารถทำการลบรายการได้");
            //    }
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
            //finally { this.Cursor = Cursors.Default; }
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
                CreateJob_List sc = new CreateJob_List(txtJobCard, txtTempJobCard);
                this.Cursor = Cursors.Default;
                sc.ShowDialog();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();
                //LoadData

                string JobCard = txtJobCard.Text;
                string TempJobCard = txtTempJobCard.Text;
                if (!txtTempJobCard.Text.Equals("") && !txtJobCard.Text.Equals(""))
                {
                    txtCodeNoBarcode.Text = "";

                    DataLoad();
                    Ac = "View";
                    //btnDel_Item.Enabled = false;
                    btnSave.Enabled = false;
                    btnNew.Enabled = true;
                }
                
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError("Shipping", ex.Message + " : radButtonElement1_Click", this.Name); }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                PrintPR a = new PrintPR(txtJobCard.Text, txtJobCard.Text, "JobCard");
                a.ShowDialog();

                //using (DataClasses1DataContext db = new DataClasses1DataContext())
                //{
                //    var g = (from ix in db.sp_R004_ReportShipping(txtSHNo.Text, DateTime.Now) select ix).ToList();
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

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            StockControl.dbClss.CheckDigitDecimal(e);
        }

        private void txtQty_Leave(object sender, EventArgs e)
        {
            decimal temp = 0;
            decimal.TryParse(txtQty.Text, out temp);
            temp = decimal.Round(temp, 2);
            txtQty.Text = (temp).ToString();
        }

        private void txtCodeNo_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if(e.KeyValue == 9 || e.KeyValue == 13)
            {
                Load_CodeNO(txtCodeNo.Text);
            }
        }
        private void Load_CodeNO(string CodeNo)
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var g = (from ix in db.tb_Items select ix)
                        .Where(a => a.CodeNo == txtCodeNo.Text && (a.TypePart =="FG" || a.TypePart =="WIP") )
                        .ToList();
                    if (g.Count() > 0)
                    {
                        txtItemName.Text = StockControl.dbClss.TSt(g.FirstOrDefault().ItemNo);
                        txtItemDesc.Text = StockControl.dbClss.TSt(g.FirstOrDefault().ItemDescription);
                        txtUnit.Text = StockControl.dbClss.TSt(g.FirstOrDefault().UnitShip);
                    }
                    else
                    {
                        txtItemDesc.Text = "";
                        txtItemName.Text = "";
                        txtUnit.Text = "";
                        txtCodeNo.Text = "";
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ListPart sc = new ListPart(txtCodeNo, "FG-WIP", "CreateJob");
            this.Cursor = Cursors.Default;
            sc.ShowDialog();
            Load_CodeNO(txtCodeNo.Text);
            GC.Collect();
            GC.WaitForPendingFinalizers();

            ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            ClassLib.Memory.Heap();
        }

        private void btnView_Click_1(object sender, EventArgs e)
        {
            btnView.Enabled = false;
            btnEdit.Enabled = true;
            btnNew.Enabled = true;
            btnSave.Enabled = false;
            //btnDelete.Enabled = false;

            Enable_Status(false, "View");
            lblStatus.Text = "View";
            Ac = "View";
        }

        private void btnEdit_Click_1(object sender, EventArgs e)
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
                if (lblStatus.Text != "Completed" && lblStatus.Text != "Process" && lblStatus.Text != "Discon" && lblStatus.Text != "Partial")
                {
                    lblStatus.Text = "Delete";
                    Ac = "Del";
                    if (MessageBox.Show("ต้องการลบรายการ ( " + txtJobCard.Text + " ) หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        using (DataClasses1DataContext db = new DataClasses1DataContext())
                        {
                            var g = (from ix in db.tb_JobCards
                                     where ix.TempJobCard.Trim() == txtTempJobCard.Text.Trim()
                                     && ix.Status != "Cancel" 
                                     && ix.Status != "Completed" 
                                     && ix.Status != "Process"
                                     && ix.Status != "Discon"
                                     && ix.Status != "Partial"
                                     select ix).ToList();
                            if (g.Count > 0)  //มีรายการในระบบ
                            {
                                var gg = (from ix in db.tb_JobCards
                                          where ix.TempJobCard.Trim() == txtTempJobCard.Text.Trim()
                                           && ix.Status != "Cancel" && ix.Status != "Completed" && ix.Status != "Process"
                                           && ix.Status != "Discon"
                                           && ix.Status != "Partial"
                                          select ix).First();

                                

                                gg.Status = "Cancel";
                                gg.ModifyBy = ClassLib.Classlib.User;
                                gg.ModifyDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));

                                dbClss.AddHistory(this.Name, "ลบ JobCard", "Delete JobCard [" + txtJobCard.Text.Trim() + "]", txtTempJobCard.Text);


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

        private void radLabel28_Click(object sender, EventArgs e)
        {

        }

        private void btnCal_Click(object sender, EventArgs e)
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    decimal Amount = 0;
                    var g = (from ix in db.sp_035_Cal_Cost_JobCard(txtJobCard.Text, txtTempJobCard.Text, dbClss.TDe(txtQty.Text), "", "") select ix).ToList();
                    if (g.Count > 0)
                    {
                        foreach (var gg in g)
                        {
                            Amount += dbClss.TDe(gg.Amount);
                        }

                        if (dbClss.TDe(txtQty.Text) > 0)
                            txtUnitCost.Text = (Amount / dbClss.TDe(txtQty.Text)).ToString("N2");

                        txtAmount.Text = Amount.ToString("N2");
                    }

                    Get_CostCalim();
                   
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void Get_CostCalim()
        {
            try
            {
                decimal Amount = 0;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {

                    //var c = (from ix in db.sp_046_Sum_STDAdditionCost(txtJobCard.Text, txtTempJobCard.Text, "", "", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"))) select ix).ToList();
                    // if (c.Count > 0)
                    //{
                    //    Amount = 0;
                    //    foreach (var gg in c)
                    //    {
                    //        Amount += dbClss.TDe(gg.Amount);
                    //    }
                    //    Amount += dbClss.TDe(txtAmount.Text);

                    //    if (dbClss.TDe(txtQty.Text) > 0)
                    //        txtCal_UnitCost_New.Text = (Amount / dbClss.TDe(txtQty.Text)).ToString("N2");

                    //    txtCal_Amount_New.Text = Amount.ToString("N2");
                    //}

                    Amount = dbClss.TDe(db.Cal_Claim(txtJobCard.Text, txtTempJobCard.Text, "", ""));
                    if (Amount > 0)
                    {
                        Amount += dbClss.TDe(txtAmount.Text);

                        if (dbClss.TDe(txtQty.Text) > 0)
                            txtCal_UnitCost_New.Text = (Amount / dbClss.TDe(txtQty.Text)).ToString("N2");

                        txtCal_Amount_New.Text = Amount.ToString("N2");
                    }
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void btnPrintCost_Click(object sender, EventArgs e)
        {
            try
            {
                string JobCard = txtJobCard.Text;
                string TempJobCard = txtTempJobCard.Text;
                string YYYY = "";
                string MM = "";
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var g = (from ix in db.sp_R013_Report_Jobcard_Cost(JobCard, TempJobCard,YYYY,MM, Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"))) select ix).ToList();
                    if (g.Count() > 0)
                    {
                        Report.Reportx1.Value = new string[4];
                        Report.Reportx1.Value[0] = JobCard;
                        Report.Reportx1.Value[1] = TempJobCard;
                        Report.Reportx1.Value[2] = YYYY;
                        Report.Reportx1.Value[3] = MM;
                        Report.Reportx1.WReport = "JobCard_Cost";
                        Report.Reportx1 op = new Report.Reportx1("JobCard_Cost3.rpt");
                        op.Show();
                    }
                    else
                        MessageBox.Show("not found.");
                }
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
