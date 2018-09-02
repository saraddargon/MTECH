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
    public partial class Shipping_FG : Telerik.WinControls.UI.RadRibbonForm
    {
        public Shipping_FG()
        {
            InitializeComponent();
        }
        public Shipping_FG(string SHNo)
        {
            InitializeComponent();
            SHNo_t = SHNo;
        }
        public Shipping_FG(string SHNo,string CodeNo)
        {
            InitializeComponent();
            SHNo_t = SHNo;
            CodeNo_t = CodeNo;
        }
        string SHNo_t = "";
        string CodeNo_t = "";
        string Ac = "";
        DataTable dt_h = new DataTable();
       // DataTable dt_d = new DataTable();

        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            HistoryView hw = new HistoryView(this.Name, txtSHFG.Text);
            this.Cursor = Cursors.Default;
            hw.ShowDialog();
        }

        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }
        private void GETDTRow()
        {
            dt_h.Columns.Add(new DataColumn("id", typeof(int)));
            dt_h.Columns.Add(new DataColumn("SHNo", typeof(string)));
            dt_h.Columns.Add(new DataColumn("SHBy", typeof(string)));
            dt_h.Columns.Add(new DataColumn("SHDate", typeof(DateTime)));
            dt_h.Columns.Add(new DataColumn("CreateBy", typeof(string)));
            dt_h.Columns.Add(new DataColumn("CreateDate", typeof(DateTime)));
            dt_h.Columns.Add(new DataColumn("CSTM_Name", typeof(string)));
            dt_h.Columns.Add(new DataColumn("CSTM_Address", typeof(string)));
            dt_h.Columns.Add(new DataColumn("Remark", typeof(string)));
            dt_h.Columns.Add(new DataColumn("JobCard", typeof(string)));
            dt_h.Columns.Add(new DataColumn("TempJobCard", typeof(string)));
            dt_h.Columns.Add(new DataColumn("Status", typeof(string)));
            dt_h.Columns.Add(new DataColumn("BarCode", typeof(string)));
            
        }

        private void Unit_Load(object sender, EventArgs e)
        {
            //dgvData.ReadOnly = true;
           // dgvData.AutoGenerateColumns = false;
            GETDTRow();

            DefaultItem();

            btnNew_Click(null, null);

            if (!SHNo_t.Equals(""))
            {
                btnNew.Enabled = true;
                txtSHFG.Text = SHNo_t;              
                DataLoad();
                Ac = "View";
                Enable_Status(false, "View");

            }
            else if (!CodeNo_t.Equals(""))
            {
                btnNew.Enabled = true;
                txtJobCard_Barcode.Text = CodeNo_t;
                Insert_data();
                txtJobCard_Barcode.Text = "";
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
            
            try
            {

                this.Cursor = Cursors.WaitCursor;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {

                    try
                    {

                        var g = (from i in db.tb_Shipping_JobCards
                                 join s in db.tb_JobCards on i.TempJobCard equals s.TempJobCard

                                 where i.Status != "Cancel"
                                    // && i.Status != "Completed"
                                    && i.SHNo.Trim() == txtSHFG.Text.Trim()

                                 select new
                                 {
                                     CodeNo = s.CodeNo,
                                     ItemNo = s.ItemName,
                                     ItemDescription = s.ItemDesc,
                                     id = i.id,
                                     JobCard = i.JobCard.Trim(),
                                     TempJobCard = i.TempJobCard.Trim(),
                                     CSTM_Name = i.CSTM_Name,
                                     CSTM_Address = i.CSTM_Address,
                                     SHDate = i.SHDate,
                                     SHBy = i.SHBy,
                                     Qty = s.Qty,
                                     RemainQty = i.Qty,
                                     RemainQty2 = s.RemainQty,
                                     LotNo = s.LotNo,
                                     Unit = s.Unit,
                                     Remark = i.Remark,
                                     Status = i.Status,
                                     CreateBy = i.CreateBy,
                                     CreateDate = i.CreateDate,
                                     UnitCost = i.UnitCost,
                                     Amount = i.Amount,
                                     TypeJobCard = s.Type

                                 }
                            ).ToList();

                        //var g = (from ix in db.tb_Shipping_JobCards select ix)
                        //    .Where(a => a.SHNo == txtSHFG.Text.Trim()).ToList();
                        if (g.Count() > 0)
                        {
                            DateTime? temp_date = null;

                            txtJobCard.Text = dbClss.TSt(g.FirstOrDefault().JobCard).Trim();
                            txtTempJobCard.Text = dbClss.TSt(g.FirstOrDefault().TempJobCard).Trim();
                            txtSHName.Text = dbClss.TSt(g.FirstOrDefault().SHBy);
                            txtCSTM_Name.Text = dbClss.TSt(g.FirstOrDefault().CSTM_Name);
                            txtCSTM_Address.Text = dbClss.TSt(g.FirstOrDefault().CSTM_Address);
                            txtCodeNo.Text = dbClss.TSt(g.FirstOrDefault().CodeNo);
                            txtItemDescription.Text = dbClss.TSt(g.FirstOrDefault().ItemDescription);
                            txtItemName.Text = dbClss.TSt(g.FirstOrDefault().ItemNo);
                            txtUnitCost.Text = dbClss.TDe(g.FirstOrDefault().UnitCost).ToString("N2");
                            txtCost.Text = dbClss.TDe(g.FirstOrDefault().Amount).ToString("N2");
                            txtOrderQty.Text = dbClss.TDe(g.FirstOrDefault().Qty).ToString("N2");
                            txtRemainQty.Text = dbClss.TDe(g.FirstOrDefault().RemainQty).ToString("N2");
                            txtRemark.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Remark);
                            txtQty.Text = dbClss.TDe(g.FirstOrDefault().RemainQty2).ToString("N2");

                            if (!StockControl.dbClss.TSt(g.FirstOrDefault().SHDate).Equals(""))
                                dtSHDate.Value = Convert.ToDateTime(g.FirstOrDefault().SHDate,new CultureInfo("en-US"));
                            else
                                dtSHDate.Value = Convert.ToDateTime(temp_date,new CultureInfo("en-US"));


                            txtCreateBy.Text = StockControl.dbClss.TSt(g.FirstOrDefault().CreateBy);                           
                            if (!StockControl.dbClss.TSt(g.FirstOrDefault().CreateDate).Equals(""))                         
                               txtCreateDate.Text = Convert.ToDateTime(g.FirstOrDefault().CreateDate).ToString("dd/MMM/yyyy");                            
                            else
                                txtCreateDate.Text = "";

                            
                            lblStatus.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Status);
                            if (StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Cancel"))
                            {
                                btnSave.Enabled = false;
                                //btnDelete.Enabled = false;
                                //btnView.Enabled = false;
                                //btnEdit.Enabled = false;
                                lblStatus.Text = "Cancel";
                               // dgvData.ReadOnly = true;
                                btnNew.Enabled = true;
                                btnDel_Item.Enabled = false;
                            }
                            else if
                                (StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Process"))
                            {
                                btnSave.Enabled = false;
                                //btnDelete.Enabled = false;
                                //btnView.Enabled = false;
                                //btnEdit.Enabled = false;
                                lblStatus.Text = "Process";
                                //dgvData.ReadOnly = false;
                                btnNew.Enabled = true;
                                btnDel_Item.Enabled = false;
                            }
                            else if
                                (StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Completed"))
                            {
                                btnSave.Enabled = false;
                                //btnDelete.Enabled = false;
                                //btnView.Enabled = false;
                                //btnEdit.Enabled = false;
                                lblStatus.Text = "Completed";
                                //dgvData.ReadOnly = true;
                                btnNew.Enabled = true;
                                btnDel_Item.Enabled = false;
                            }
                            else
                            {
                                btnNew.Enabled = true;
                                btnSave.Enabled = true;
                                //btnDelete.Enabled = true;
                                //btnView.Enabled = true;
                                //btnEdit.Enabled = true;
                                lblStatus.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Status);
                                //dgvData.ReadOnly = false;
                                btnDel_Item.Enabled = false;
                            }
                            dt_h = StockControl.dbClss.LINQToDataTable(g);

                            if(StockControl.dbClss.TSt(g.FirstOrDefault().TypeJobCard)=="Claim")
                            {
                                btnSave.Enabled = false;
                                btnDiscon.Enabled = false;
                                //btnDelete.Enabled = false;
                                //btnView.Enabled = false;
                                //btnEdit.Enabled = false;
                                //dgvData.ReadOnly = true;
                                btnNew.Enabled = true;
                                btnDel_Item.Enabled = false;
                            }
                           
                        }
                        else
                        {
                            MessageBox.Show("ไม่พบเลขเอกสาร");
                            btnNew_Click(null, null);
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
            
            txtSHFG.Text = "";
            txtRemark.Text = "";
            txtJobCard_Barcode.Text = "";
            dtSHDate.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
            txtCSTM_Address.Text = "";
            txtCSTM_Name.Text = "";
            txtCreateBy.Text = ClassLib.Classlib.User;
            txtSHName.Text = ClassLib.Classlib.User;
            txtCreateDate.Text = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy");
            lblStatus.Text = "-";
            txtCodeNo.Text = "";
            txtItemDescription.Text = "";
            txtItemName.Text = "";
            txtJobCard.Text = "";
            txtidJobCard.Text = "0";
            txtTempJobCard.Text = "";
            txtRemainQty.Text = "0.00";
            txtUnitCost.Text = "0.00";
            txtCost.Text = "0.00";
        }
      private void Enable_Status(bool ss,string Condition)
        {
            if (Condition.Equals("-") || Condition.Equals("New"))
            {
                txtJobCard_Barcode.Enabled = ss;
                txtSHName.Enabled = ss;
                txtCSTM_Name.Enabled = ss;
                txtCSTM_Address.Enabled = ss;
                //txtRCNo.Enabled = ss;
                txtRemark.Enabled = ss;
                //txtTempNo.Enabled = ss;
                dtSHDate.Enabled = ss;
                //dgvData.ReadOnly = false;
                btnDel_Item.Enabled = ss;


            }
            else if (Condition.Equals("View"))
            {
                txtJobCard_Barcode.Enabled = ss;
                txtSHName.Enabled = ss;
                txtCSTM_Name.Enabled = ss;
                txtCSTM_Address.Enabled = ss;
                txtRemark.Enabled = ss;
                txtCSTM_Name.Enabled = ss;
                txtCSTM_Address.Enabled = ss;
                dtSHDate.Enabled = ss;
                //dgvData.ReadOnly = false;
                txtRemark.Enabled = ss;
                btnDel_Item.Enabled = ss;
            }

            else if (Condition.Equals("Edit"))
            {
                txtJobCard_Barcode.Enabled = ss;
                txtSHName.Enabled = ss;
                txtCSTM_Name.Enabled = ss;
                txtCSTM_Address.Enabled = ss;
                txtRemark.Enabled = ss;
                //txtTempNo.Enabled = ss;
                dtSHDate.Enabled = ss;
                //dgvData.ReadOnly = false;
                txtRemark.Enabled = ss;
                btnDel_Item.Enabled = ss;
            }
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            btnDel_Item.Enabled = true;
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            btnDiscon.Enabled = true;
            ClearData();
            Enable_Status(true, "New");
            lblStatus.Text = "New";
            Ac = "New";
            txtQty.Text = "0.00";
            txtCost.Text = "0.00";
            txtUnitCost.Text = "0.00";
            txtOrderQty.Text = "0.00";
                 
           // getมาไว้ก่อน แต่ยังไมได้ save
            txtSHFG.Text = StockControl.dbClss.GetNo(15, 0);
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

                if (txtSHName.Text.Equals(""))
                    err += "- “ผู้เบิกสินค้า:” เป็นค่าว่าง \n";
                if (txtJobCard.Text.Equals(""))
                    err += "- “Job card:” เป็นค่าว่าง \n";
                if (txtTempJobCard.Text.Equals(""))
                    err += "- “Temp Job card:” เป็นค่าว่าง \n";
                if (txtSHName.Text.Equals(""))
                    err += "- “ผู้เบิกสินค้า:” เป็นค่าว่าง \n";
                if (txtCSTM_Name.Text.Equals(""))
                    err += "- “ชื่อลูกค้า:” เป็นค่าว่าง \n";
                if (txtCSTM_Address.Text.Equals(""))
                    err += "- “ที่อยู่:” เป็นค่าว่าง \n";
                if (txtCodeNo.Text.Equals(""))
                    err += "- “รหัสสินค้า:” เป็นค่าว่าง \n";
                if (dtSHDate.Text.Equals(""))
                    err += "- “วันที่เบิกสินค้า:” เป็นค่าว่าง \n";

                if(dbClss.TDe(txtRemainQty.Text)<=0)
                    err += "- “ระบุจำนวนที่จะปิดใบงานต้องมากกว่า 0 ”  \n";
                if (dbClss.TDe(txtRemainQty.Text)> dbClss.TDe(txtQty.Text))
                    err += "- “ไม่สามารถปิดจำนวนใบงานมากกว่าจำนวนสั่งผลิตคงเหลือได้:”  \n";


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
                var g = (from ix in db.tb_Shipping_JobCards
                         where ix.SHNo.Trim() == txtSHFG.Text.Trim() 
                         && ix.Status != "Cancel"
                         && ix.Status != "Completed"
                         select ix).ToList();
                if (g.Count > 0)  //มีรายการในระบบ
                {
                    foreach (DataRow row in dt_h.Rows)
                    {
                        
                        var gg = (from ix in db.tb_Shipping_JobCards
                                  where ix.SHNo.Trim() == txtSHFG.Text.Trim()
                                 && ix.Status != "Cancel"
                                  && ix.Status != "Completed"
                                  select ix).First();

                        gg.CreateBy = ClassLib.Classlib.User;
                        gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                        dbClss.AddHistory(this.Name, "แก้ไขขายสินค้า", "แก้ไขโดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", txtSHFG.Text);

                        //if (StockControl.dbClss.TSt(gg.Barcode).Equals(""))
                        //    gg.Barcode = StockControl.dbClss.SaveQRCode2D(txtSHFG.Text.Trim());

                        if (!txtSHName.Text.Trim().Equals(row["SHBy"].ToString()))
                        {
                            gg.SHBy = txtSHName.Text;                           
                            dbClss.AddHistory(this.Name, "แก้ไขขายสินค้า", "แก้ไขผู้เบิกสินค้า [" + txtSHName.Text.Trim() + " เดิม :" + row["SHBy"].ToString() + "]", txtSHFG.Text);
                        }
                        if (!txtCSTM_Name.Text.Trim().Equals(row["CSTM_Name"].ToString()))
                        {
                            gg.CSTM_Name = txtCSTM_Name.Text;
                            dbClss.AddHistory(this.Name, "แก้ไขขายสินค้า", "แก้ไขชื่อลูกค้า [" + txtCSTM_Name.Text.Trim() + " เดิม :" + row["CSTM_Name"].ToString() + "]", txtSHFG.Text);
                        }
                        if (!txtCSTM_Address.Text.Trim().Equals(row["CSTM_Address"].ToString()))
                        {
                            gg.CSTM_Address = txtCSTM_Address.Text;
                            dbClss.AddHistory(this.Name, "แก้ไขขายสินค้า", "แก้ไขที่อยู่ [" + txtCSTM_Address.Text.Trim() + " เดิม :" + row["CSTM_Address"].ToString() + "]", txtSHFG.Text);
                        }
                        if (!txtJobCard.Text.Trim().Equals(row["JobCard"].ToString()))
                        {
                            gg.JobCard = txtJobCard.Text;
                            dbClss.AddHistory(this.Name, "แก้ไขขายสินค้า", "แก้ไขJobCard [" + txtJobCard.Text.Trim() + " เดิม :" + row["JobCard"].ToString() + "]", txtSHFG.Text);
                        }
                        if (!txtTempJobCard.Text.Trim().Equals(row["TempJobCard"].ToString()))
                        {
                            gg.TempJobCard = txtTempJobCard.Text;
                            dbClss.AddHistory(this.Name, "แก้ไขขายสินค้า", "แก้ไขJobCard [" + txtTempJobCard.Text.Trim() + " เดิม :" + row["TempJobCard"].ToString() + "]", txtSHFG.Text);
                        }

                        gg.UnitCost = dbClss.TDe(txtUnitCost.Text);
                        gg.Amount = dbClss.TDe(txtCost.Text);
                        gg.Qty = dbClss.TDe(txtRemainQty.Text);


                        if (!txtRemark.Text.Trim().Equals(row["Remark"].ToString()))
                        {
                            gg.Remark = txtRemark.Text.Trim();
                            dbClss.AddHistory(this.Name, "แก้ไขขายสินค้า", "แก้ไขหมายเหตุ [" + txtRemark.Text.Trim() + " เดิม :" + row["Remark"].ToString() + "]", txtSHFG.Text);
                        }
                      
                        if (!dtSHDate.Text.Trim().Equals(""))
                        {
                            string date1 = "";
                            date1 = dtSHDate.Value.ToString("yyyyMMdd", new CultureInfo("en-US"));
                            string date2 = "";
                            DateTime temp = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                            if (!StockControl.dbClss.TSt(row["SHDate"].ToString()).Equals(""))
                            {
                                temp = Convert.ToDateTime(row["SHDate"]);
                                date2 = temp.ToString("yyyyMMdd", new CultureInfo("en-US"));

                            }
                            if (!date1.Equals(date2))
                            {
                                DateTime? RequireDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                if (!dtSHDate.Text.Equals(""))
                                    RequireDate = dtSHDate.Value;
                                gg.SHDate = RequireDate;
                                dbClss.AddHistory(this.Name, "แก้ไขขายสินค้า", "แก้ไขวันที่เบิกสินค้า [" + dtSHDate.Text.Trim() + " เดิม :" + temp.ToString() + "]", txtSHFG.Text);
                            }
                        }
                        gg.Status = "Completed";
                        db.SubmitChanges();


                        var j = (from ix in db.tb_JobCards
                                 where ix.TempJobCard.Trim() == txtTempJobCard.Text.Trim()
                                 && ix.Status != "Cancel"
                                  && ix.Status != "Completed"
                                  && ix.Status != "Discon"
                                  && ix.Type != "Claim"
                                 select ix).ToList();
                        if (j.Count > 0)
                        {
                            var jj = (from ix in db.tb_JobCards
                                     where ix.TempJobCard.Trim() == txtTempJobCard.Text.Trim()
                                     && ix.Status != "Cancel"
                                      && ix.Status != "Completed"
                                      && ix.Status != "Discon"
                                      && ix.Type != "Claim"
                                      select ix).First();
                            
                            if ((dbClss.TDe(txtQty.Text) - dbClss.TDe(txtRemainQty.Text)) <= 0)
                            {
                                jj.Status = "Partial";
                                jj.UnitCost = dbClss.TDe(txtUnitCost.Text);
                                jj.Amount = dbClss.TDe(txtCost.Text);
                                jj.RemainQty = (dbClss.TDe(txtQty.Text) - dbClss.TDe(txtRemainQty.Text));
                            }
                            else
                            {
                                jj.Status = "Completed";
                                jj.UnitCost = dbClss.TDe(txtUnitCost.Text);
                                jj.Amount = dbClss.TDe(txtCost.Text);
                                jj.RemainQty = (dbClss.TDe(txtQty.Text) - dbClss.TDe(txtRemainQty.Text));
                            }

                            db.SubmitChanges();
                            dbClss.AddHistory(this.Name, "แก้ไขใบร้องขอการผลิต", "ส่งงานการขอการผลิต [" + txtTempJobCard.Text.Trim() + "]", txtTempJobCard.Text);
                        }


                    }
                }
                else //สร้างใหม่
                {
                    byte[] barcode = null;
                    //barcode = StockControl.dbClss.SaveQRCode2D(txtSHFG.Text.Trim());
                    //DateTime? UpdateDate = null;

                    DateTime? SHDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    if (!dtSHDate.Text.Equals(""))
                        SHDate = dtSHDate.Value;

                    tb_Shipping_JobCard gg = new tb_Shipping_JobCard();
                    gg.SHNo = txtSHFG.Text;
                    gg.SHDate = SHDate;
                    gg.SHBy = txtSHName.Text.Trim();               
                    gg.CreateBy = ClassLib.Classlib.User;
                    gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    gg.CSTM_Name = txtCSTM_Name.Text;
                    gg.CSTM_Address = txtCSTM_Address.Text;
                    gg.JobCard = txtJobCard.Text;
                    gg.TempJobCard = txtTempJobCard.Text;
                    gg.Status = "Completed";
                    gg.Remark = txtRemark.Text;
                    gg.UnitCost = dbClss.TDe(txtUnitCost.Text);
                    gg.Amount = dbClss.TDe(txtCost.Text);
                    gg.Qty = dbClss.TDe(txtRemainQty.Text);

                    gg.Barcode = barcode;
                    db.tb_Shipping_JobCards.InsertOnSubmit(gg);
                    db.SubmitChanges();

                    dbClss.AddHistory(this.Name, "CloseJob", "สร้างการปิดใบสั่งผลิตสินค้า [" + txtSHFG.Text.Trim() + "]", txtSHFG.Text);

                    var j = (from ix in db.tb_JobCards
                             where ix.TempJobCard.Trim() == txtTempJobCard.Text.Trim()
                             && ix.Status != "Cancel"
                              && ix.Status != "Completed"
                              && ix.Status != "Discon"
                              && ix.Type !="Claim"
                             select ix).ToList();
                    if (j.Count > 0)
                    {
                        var jj = (from ix in db.tb_JobCards
                                  where ix.TempJobCard.Trim() == txtTempJobCard.Text.Trim()
                                  && ix.Status != "Cancel"
                                   && ix.Status != "Completed"
                                   && ix.Status != "Discon"
                                   && ix.Type != "Claim"
                                  select ix).First();
                       
                        if ((dbClss.TDe(txtQty.Text) > dbClss.TDe(txtRemainQty.Text)))
                        {
                            jj.Status = "Partial";
                            jj.UnitCost = dbClss.TDe(txtUnitCost.Text);
                            jj.Amount = dbClss.TDe(txtCost.Text);
                            jj.RemainQty = (dbClss.TDe(txtQty.Text) - dbClss.TDe(txtRemainQty.Text));
                        }
                        else
                        {
                            jj.Status = "Completed";
                            jj.UnitCost = dbClss.TDe(txtUnitCost.Text);
                            jj.Amount = dbClss.TDe(txtCost.Text);
                            jj.RemainQty = (dbClss.TDe(txtQty.Text) - dbClss.TDe(txtRemainQty.Text));
                        }
                        db.SubmitChanges();

                        //Add Stock
                        InsertStock_new_Receive();
                        OutStock();

                        dbClss.AddHistory(this.Name, "แก้ไขใบร้องขอการผลิต", "ส่งงานการขอการผลิต [" + txtTempJobCard.Text.Trim() + "]", txtTempJobCard.Text);
                    }

                }
            }
        }
        //private decimal Cal_Job()
        //{
        //    decimal re = 0;
        //    try
        //    {
                
        //        using (DataClasses1DataContext db = new DataClasses1DataContext())
        //        {
        //            decimal Amount = 0;
        //            var g = (from ix in db.sp_035_Cal_Cost_JobCard(txtJobCard.Text, txtTempJobCard.Text, dbClss.TDe(txtRemainQty.Text), "", "") select ix).ToList();
        //            if (g.Count > 0)
        //            {
        //                foreach (var gg in g)
        //                {
        //                    Amount += dbClss.TDe(gg.Amount);
        //                }

        //                if (dbClss.TDe(txtQty.Text) > 0)
        //                    re = Math.Round(( (Amount / dbClss.TDe(txtQty.Text))),2);
                        
        //            }
        //        }
        //    }
        //    catch (Exception ex) { MessageBox.Show(ex.Message); }
        //    return re;
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
                    decimal Avg = 0;
                    decimal UnitCost = 0;
                    decimal sum_Remain = 0;
                    decimal sum_Qty = 0;
                    //if (rdoInvoice.IsChecked)
                    //{
                        Category = "Invoice";
                        Type = "รับด้วยใบ Invoice";
                        Flag_ClearTemp = 0;
                    //}
                    //else
                    //{
                    //    Category = "Temp";
                    //    Type = "ใบส่งของชั่วคราว";
                    //    Flag_ClearTemp = 1;
                    //}

                    var g = (from ix in db.tb_Shipping_JobCards
                                 join i in db.tb_JobCards on ix.TempJobCard equals i.TempJobCard
                                
                                 //join t in db.tb_Items on i.CodeNo equals t.CodeNo
                             where ix.SHNo.Trim() == txtSHFG.Text.Trim() && ix.Status != "Cancel"
                             select new
                             {
                                 CodeNo = i.CodeNo,
                                 ItemNo =i.ItemName,
                                 ItemDescription = i.ItemDesc,
                                 TempJobCard = ix.TempJobCard,
                                 JobCard = ix.JobCard,                                
                                 QTY = ix.Qty,
                                 UnitCost = ix.UnitCost,
                                 Amount = ix.Amount
                                 ,id = i.id    
                                 
                             }).ToList();
                    if (g.Count > 0)
                    {
                        //insert Stock

                        foreach (var vv in g)
                        {
                            Seq += 1;

                            Amount = dbClss.TDe(vv.Amount);
                            UnitCost = dbClss.TDe(vv.UnitCost);

                            // Amount = Convert.ToDecimal(vv.QTY) * Convert.ToDecimal(vv.UnitCost);
                            //UnitCost = Convert.ToDecimal(vv.UnitCost);
                            string ShelfNo = "";
                            string Location = "";
                            var l = (from ix in db.tb_Items
                                     where ix.CodeNo.Trim().ToUpper() == vv.CodeNo.Trim().ToUpper()
                                     && ix.Status != "Cancel"
                                     select ix).ToList();
                            if (l.Count >0)
                            {
                                Location = dbClss.TSt(l.FirstOrDefault().Location);
                                ShelfNo = Convert.ToString(l.FirstOrDefault().ShelfNo);
                            }

                            //แบบที่ 1 จะไป sum ใหม่
                            RemainQty = (Convert.ToDecimal(db.Cal_QTY_Remain_Location(vv.CodeNo, "", 0,Location)));
                            //แบบที่ 2 จะไปดึงล่าสุดมา
                            //RemainQty = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainQty"));
                            sum_Remain = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainAmount",Location))
                                + Amount;

                            sum_Qty = RemainQty + Convert.ToDecimal(vv.QTY);

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
                                RemainUnitCost = Math.Round((Math.Abs(RemainAmount) / Math.Abs(sum_Qty)), 2);

                            tb_Stock gg = new tb_Stock();
                            gg.AppDate = AppDate;
                            gg.Seq = Seq;
                            gg.App = "CloseJob";
                            gg.Appid = Seq;
                            gg.CreateBy = ClassLib.Classlib.User;
                            gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                            gg.DocNo = txtSHFG.Text;
                            gg.RefNo = vv.JobCard;
                            gg.CodeNo = txtCodeNo.Text.Trim();
                            gg.Type = Type;
                            gg.QTY = Convert.ToDecimal(vv.QTY);
                            gg.Inbound = Convert.ToDecimal(vv.QTY);
                            gg.Outbound = 0;
                            gg.Type_i = 1;  //Receive = 1,Cancel Receive 2,Shipping = 3,Cancel Shipping = 4,Adjust stock = 5,ClearTemp = 6
                            gg.Category = Category;
                            gg.Refid = vv.id;
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
                            gg.TLQty = Convert.ToDecimal(vv.QTY);
                            gg.ShipQty = 0;
                            gg.RefJobCode = txtJobCard.Text;
                            gg.RefTempJobCode = txtTempJobCard.Text;
                            gg.RefidJobCode = dbClss.TInt(txtidJobCard.Text);
                            gg.Location = Location;
                            gg.ShelfNo = ShelfNo;

                            //ต้องไม่ใช่ Item ที่มีในระบบ
                            var c = (from ix in db.tb_Items
                                     where ix.CodeNo.Trim().ToUpper() == vv.CodeNo.Trim().ToUpper()
                                     && ix.Status != "Cancel"
                                     select ix).ToList();
                            if (c.Count <= 0)
                            {
                                gg.TLQty = 0;
                                gg.ShipQty = Convert.ToDecimal(vv.QTY);
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
        private void OutStock()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                string DocNo = "";
                DocNo = StockControl.dbClss.GetNo(17, 2);
                decimal Qty = dbClss.TDe(txtRemainQty.Text);
                int RefidJobNo = 0;

                if (DocNo != "" && Qty > 0)
                {
                    db.sp_036_Sell_FG(DocNo
                        , txtCodeNo.Text
                                   , Qty, ""
                                   , "", ""
                                   , "", ""
                                   , "Completed", ClassLib.Classlib.User
                                   , ""//txtJobCard.Text.Trim()
                                   , ""//txtTempJobCard.Text.Trim()
                                   , RefidJobNo
                                   , ""
                                   );
                }
            }
        }
        private void SaveDetail()
        {
            //dgvData.EndEdit();
           
            //DateTime? RequireDate = DateTime.Now;
            //if (!dtRequire.Text.Equals(""))
            //    RequireDate = dtRequire.Value;
            //int Seq = 0;
            //DateTime? UpdateDate = null;
            //using (DataClasses1DataContext db = new DataClasses1DataContext())
            //{
            //    decimal UnitCost = 0;
            //    foreach (var g in dgvData.Rows)
            //    {
            //        string SS = "";
            //        if (g.IsVisible.Equals(true))
            //        {
            //            if (StockControl.dbClss.TInt(g.Cells["QTY"].Value) != (0)) // เอาเฉพาะรายการที่ไม่เป็น 0 
            //            {
            //                if (StockControl.dbClss.TInt(g.Cells["id"].Value) <= 0)  //New ใหม่
            //                {

            //                    //decimal RemainQty = 0;

            //                    UnitCost = StockControl.dbClss.TDe(g.Cells["StandardCost"].Value);//Convert.ToDecimal(dbClss.Get_Stock(StockControl.dbClss.TSt(g.Cells["CodeNo"].Value), "", "", "Avg"));
            //                    Seq += 1;
            //                    tb_Shipping u = new tb_Shipping();
            //                    u.ShippingNo = txtSHNo.Text.Trim();
            //                    u.CodeNo = StockControl.dbClss.TSt(g.Cells["CodeNo"].Value);                              
            //                    u.ItemNo = StockControl.dbClss.TSt(g.Cells["ItemNo"].Value);
            //                    u.ItemDescription = StockControl.dbClss.TSt(g.Cells["ItemDescription"].Value);
            //                    u.QTY = StockControl.dbClss.TDe(g.Cells["QTY"].Value);
            //                    u.PCSUnit = StockControl.dbClss.TDe(g.Cells["PCSUnit"].Value);
            //                    u.UnitShip = StockControl.dbClss.TSt(g.Cells["UnitShip"].Value);                              
            //                    u.Remark = StockControl.dbClss.TSt(g.Cells["Remark"].Value);
            //                    u.LotNo = StockControl.dbClss.TSt(g.Cells["LotNo"].Value);
            //                    u.SerialNo = StockControl.dbClss.TSt(g.Cells["SerialNo"].Value);
            //                    u.MachineName = StockControl.dbClss.TSt(g.Cells["MachineName"].Value);
            //                    u.LineName = StockControl.dbClss.TSt(g.Cells["LineName"].Value);
            //                    u.Calbit = false;
            //                    u.ClearFlag = false;
            //                    u.ClearDate = UpdateDate;
            //                    u.Seq = Seq;
            //                    u.Status = "Completed";
            //                    u.UnitCost = UnitCost;
            //                    db.tb_Shippings.InsertOnSubmit(u);
            //                    db.SubmitChanges();
                                
            //                    //C += 1;
            //                    dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "เพิ่มรายการเบิก [" + u.CodeNo + " จำนวนเบิก :" + u.QTY.ToString() +" "+u.UnitShip+ "]", txtSHNo.Text);
                                
            //                }
            //                else
            //                {
            //                    if (StockControl.dbClss.TInt(g.Cells["id"].Value) > 0)
            //                    {
            //                        foreach (DataRow row in dt_d.Rows)
            //                        {
            //                            var u = (from ix in db.tb_Shippings
            //                                     where ix.id == Convert.ToInt32(g.Cells["id"])
            //                                         && ix.ShippingNo == txtSHNo.Text
            //                                         && ix.CodeNo == StockControl.dbClss.TSt(g.Cells["CodeNo"].Value)
            //                                     select ix).First();
                                        

            //                            dbClss.AddHistory(this.Name, "แก้ไขการเบิก", " แก้ไขรายการเบิก id :" + StockControl.dbClss.TSt(g.Cells["id"].Value)
            //                           + " CodeNo :" + StockControl.dbClss.TSt(g.Cells["CodeNo"].Value)
            //                           + " แก้ไขโดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", txtSHNo.Text);

            //                            //u.Seq = Seq;

            //                            if (!StockControl.dbClss.TSt(g.Cells["CodeNo"].Value).Equals(row["CodeNo"].ToString()))
            //                            {
            //                                u.CodeNo = StockControl.dbClss.TSt(g.Cells["CodeNo"].Value);
            //                                dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไขรหัสพาร์ท [" + u.CodeNo + "]", txtSHNo.Text);
            //                            }
            //                            if (!StockControl.dbClss.TSt(g.Cells["QTY"].Value).Equals(row["QTY"].ToString()))
            //                            {
            //                                decimal QTY = 0; decimal.TryParse(StockControl.dbClss.TSt(g.Cells["QTY"].Value), out QTY);
            //                                u.QTY = StockControl.dbClss.TDe(g.Cells["QTY"].Value);
            //                                dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไขจำนวนเบิก [" + QTY.ToString() + "]", txtSHNo.Text);
            //                            }
            //                            if (!StockControl.dbClss.TSt(g.Cells["UnitShip"].Value).Equals(row["UnitShip"].ToString()))
            //                            {
            //                                u.UnitShip = StockControl.dbClss.TSt(g.Cells["UnitShip"].Value);
            //                                dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไขหน่วย [" + u.UnitShip + "]", txtSHNo.Text);
            //                            }
            //                            if (!StockControl.dbClss.TSt(g.Cells["PCSUnit"].Value).Equals(row["PCSUnit"].ToString()))
            //                            {
            //                                u.PCSUnit = StockControl.dbClss.TDe(g.Cells["PCSUnit"].Value);
            //                                dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไขจำนวน/หน่วย [" + u.PCSUnit + "]", txtSHNo.Text);
            //                            }
            //                            if (!StockControl.dbClss.TSt(g.Cells["LotNo"].Value).Equals(row["LotNo"].ToString()))
            //                            {
            //                                u.LotNo = StockControl.dbClss.TSt(g.Cells["LotNo"].Value);
            //                                dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไข LotNo [" + u.LotNo + "]", txtSHNo.Text);
            //                            }
            //                            if (!StockControl.dbClss.TSt(g.Cells["SerialNo"].Value).Equals(row["SerialNo"].ToString()))
            //                            {
            //                                u.SerialNo = StockControl.dbClss.TSt(g.Cells["SerialNo"].Value);
            //                                dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไข ซีเรียล [" + u.SerialNo + "]", txtSHNo.Text);
            //                            }
            //                            if (!StockControl.dbClss.TSt(g.Cells["MachineName"].Value).Equals(row["MachineName"].ToString()))
            //                            {
            //                                u.MachineName = StockControl.dbClss.TSt(g.Cells["MachineName"].Value);
            //                                dbClss.AddHistory(this.Name + "แก้ไขการเบิก", "แก้ไขรายการเบิก", "แก้ไข ชื่อ Machine [" + u.MachineName + "]", txtSHNo.Text);
            //                            }
            //                            if (!StockControl.dbClss.TSt(g.Cells["LineName"].Value).Equals(row["LineName"].ToString()))
            //                            {
            //                                u.LineName = StockControl.dbClss.TSt(g.Cells["LineName"].Value);
            //                                dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไข ชื่อ Line [" + u.LineName + "]", txtSHNo.Text);
            //                            }
            //                            if (!StockControl.dbClss.TSt(g.Cells["Remark"].Value).Equals(row["Remark"].ToString()))
            //                            {
            //                                u.Remark = StockControl.dbClss.TSt(g.Cells["Remark"].Value);
            //                                dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไขวัตถุประสงค์ [" + u.Remark + "]", txtSHNo.Text);
            //                            }
                                        
            //                            u.Status = "Completed";      
            //                            db.SubmitChanges();
                                        
            //                        }
            //                    }
            //                }

            //            }
            //        }
            //    }
            //}
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
                        txtSHFG.Text = StockControl.dbClss.GetNo(15, 2);

                    if (!txtSHFG.Text.Equals(""))
                    {
                        btnCal_Click(null, null);
                        SaveHerder();
                        //SaveDetail();


                        DataLoad();
                        btnNew.Enabled = true;
                        btnDel_Item.Enabled = false;
                        btnSave.Enabled = false;


                        MessageBox.Show("บันทึกสำเร็จ!");
                        //btnRefresh_Click(null,null);
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
                             where ix.ShippingNo.Trim() == txtSHFG.Text.Trim() && ix.Status != "Cancel"

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
                            gg.RefNo = txtSHFG.Text;
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
        private void InsertStock_new()
        {
            try
            {

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    DateTime? CalDate = null;
                    DateTime? AppDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    int Seq = 0;
                    string Type = "Shipping";
                    string Category = ""; //Temp,Invoice
                    decimal Cost = 0;
                   // int Flag_ClearTemp = 0;
                    decimal Qty_Inv = 0;
                    decimal Qty_DL = 0;
                    decimal Qty_Remain = 0;
                    decimal QTY = 0;
                    decimal QTY_temp = 0;

                    string Type_in_out = "Out";
                    decimal RemainQty = 0;
                    decimal Amount = 0;
                    decimal RemainAmount = 0;
                    decimal Avg = 0;
                    decimal UnitCost = 0;
                    decimal sum_Remain = 0;
                    decimal sum_Qty = 0;

                    //string CNNo = CNNo = StockControl.dbClss.GetNo(6, 2);
                    var g = (from ix in db.tb_Shippings
                                 //join i in db.tb_Items on ix.CodeNo equals i.CodeNo
                             where ix.ShippingNo.Trim() == txtSHFG.Text.Trim() && ix.Status != "Cancel"

                             select ix).ToList();
                    if (g.Count > 0)
                    {
                        //insert Stock

                        foreach (var vv in g)
                        {
                            Seq += 1;

                            QTY = Convert.ToDecimal(vv.QTY);
                            QTY_temp = 0;
                            Qty_Remain = (Convert.ToDecimal(db.Cal_QTY(vv.CodeNo, "", 0)));  //sum ทั้งหมด
                            Qty_Inv = (Convert.ToDecimal(db.Cal_QTY(vv.CodeNo, "Invoice", 0))); //sum เฉพาะ Invoice
                            Qty_DL = (Convert.ToDecimal(db.Cal_QTY(vv.CodeNo, "Temp", 0))); // sum เฉพาะ DL
                            
                            if (QTY <= Qty_Remain)
                            {                               

                                if (Qty_Inv >= QTY) //ถ้า จำนวน remain มีมากกว่าจำนวนที่จะลบ
                                {
                                    UnitCost = Convert.ToDecimal(vv.UnitCost);
                                    //if (UnitCost <= 0)
                                    //    UnitCost = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "Avg"));

                                    Amount = (-QTY) * UnitCost;

                                    //แบบที่ 1 จะไป sum ใหม่
                                    RemainQty = (Convert.ToDecimal(db.Cal_QTY_Remain_Location(vv.CodeNo, "", 0,vv.Location)));
                                    //แบบที่ 2 จะไปดึงล่าสุดมา
                                    //RemainQty = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainQty"));
                                    sum_Remain = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainAmount",vv.Location))
                                        + Amount;

                                    sum_Qty = RemainQty + (-QTY);
                                    Avg = UnitCost;//sum_Remain / sum_Qty;
                                    RemainAmount = sum_Remain;


                                    Category = "Invoice";
                                    tb_Stock gg = new tb_Stock();
                                    gg.AppDate = AppDate;
                                    gg.Seq = Seq;
                                    gg.App = "Shipping";
                                    gg.Appid = Seq;
                                    gg.CreateBy = ClassLib.Classlib.User;
                                    gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                    gg.DocNo = txtSHFG.Text;
                                    gg.RefNo = "";
                                    gg.CodeNo = vv.CodeNo;
                                    gg.Type = Type;
                                    gg.QTY = -Convert.ToDecimal(QTY);
                                    gg.Inbound = 0;
                                    gg.Outbound = -Convert.ToDecimal(QTY);
                                    gg.Type_i = 3;  //Receive = 1,Cancel Receive 2,Shipping = 3,Cancel Shipping = 4,Adjust stock = 5,ClearTemp = 6
                                    gg.Category = Category;
                                    gg.Refid = vv.id;
                                    
                                    gg.CalDate = CalDate;
                                    gg.Status = "Active";
                                    gg.Flag_ClearTemp =0; //0 คือ invoice,1 คือ Temp , 2 คือ clear temp แล้ว
                                    gg.Type_in_out = Type_in_out;
                                    gg.AmountCost = Amount;
                                    gg.UnitCost = UnitCost;
                                    gg.RemainQty = sum_Qty;
                                    gg.RemainUnitCost = 0;
                                    gg.RemainAmount = RemainAmount;
                                    gg.Avg = Avg;


                                    db.tb_Stocks.InsertOnSubmit(gg);
                                    db.SubmitChanges();

                                    dbClss.AddHistory(this.Name, "เบิกสินค้า", " เบิกสินค้าเลขที่ : " + txtSHFG.Text + " เบิก : " + Category + " CodeNo : " + vv.CodeNo + " จำนวน : " + (-QTY).ToString() + " โดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", txtSHFG.Text);

                                }
                                else
                                {
                                    QTY_temp = QTY - Qty_Inv;

                                    UnitCost = Convert.ToDecimal(vv.UnitCost);
                                    //if (UnitCost <= 0)
                                    //    UnitCost = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "Avg"));
                                    
                                    Amount = (-QTY) * UnitCost;

                                    //แบบที่ 1 จะไป sum ใหม่
                                    RemainQty = (Convert.ToDecimal(db.Cal_QTY_Remain_Location(vv.CodeNo, "", 0,vv.Location)));
                                    //แบบที่ 2 จะไปดึงล่าสุดมา
                                    //RemainQty = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainQty"));
                                    sum_Remain = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainAmount",vv.Location))
                                        + Amount;

                                    sum_Qty = RemainQty + (-QTY);
                                    Avg = UnitCost;//sum_Remain / sum_Qty;
                                    RemainAmount = sum_Remain;

                                    Category = "Temp";
                                    tb_Stock gg = new tb_Stock();
                                    gg.AppDate = AppDate;
                                    gg.Seq = Seq;
                                    gg.App = "Shipping";
                                    gg.Appid = Seq;
                                    gg.CreateBy = ClassLib.Classlib.User;
                                    gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                    gg.DocNo = txtSHFG.Text;
                                    gg.RefNo = "";
                                    gg.CodeNo = vv.CodeNo;
                                    gg.Type = Type;
                                    gg.QTY = -Convert.ToDecimal(QTY);
                                    gg.Inbound = 0;
                                    gg.Outbound = -Convert.ToDecimal(QTY);
                                    gg.Type_i = 3;  //Receive = 1,Cancel Receive 2,Shipping = 3,Cancel Shipping = 4,Adjust stock = 5,ClearTemp = 6
                                    gg.Category = Category;
                                    gg.Refid = vv.id;
                                    
                                    gg.CalDate = CalDate;
                                    gg.Status = "Active";
                                    gg.Flag_ClearTemp = 1; //0 คือ invoice,1 คือ Temp , 2 คือ clear temp แล้ว
                                    gg.Type_in_out = Type_in_out;
                                    gg.AmountCost = Amount;
                                    gg.UnitCost = UnitCost;
                                    gg.RemainQty = sum_Qty;
                                    gg.RemainUnitCost = 0;
                                    gg.RemainAmount = RemainAmount;
                                    gg.Avg = Avg;

                                    db.tb_Stocks.InsertOnSubmit(gg);
                                    db.SubmitChanges();
                                    dbClss.AddHistory(this.Name, "เบิกสินค้า", " เบิกสินค้าเลขที่ : " + txtSHFG.Text + " เบิก : " + Category + " CodeNo : " + vv.CodeNo + " จำนวน : " + (-QTY).ToString() + " โดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", txtSHFG.Text);


                                    //// --Stock ใน Invoice ไม่พอ ต้องเอาที่ Temp มา

                                    //UnitCost = Convert.ToDecimal(vv.UnitCost);
                                    //if (UnitCost <= 0)
                                    //    UnitCost = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "Avg"));
                                    
                                    //Amount = (-QTY_temp) * UnitCost;

                                    ////แบบที่ 1 จะไป sum ใหม่
                                    //RemainQty = (Convert.ToDecimal(db.Cal_QTY(vv.CodeNo, "", 0)));
                                    ////แบบที่ 2 จะไปดึงล่าสุดมา
                                    ////RemainQty = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainQty"));
                                    //sum_Remain = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainAmount"))
                                    //    + Amount;
                                    //sum_Qty = RemainQty + (-QTY_temp);
                                    //Avg = UnitCost;//sum_Remain / sum_Qty;
                                    //RemainAmount = sum_Remain;

                                    //Category = "Invoice";
                                    //tb_Stock aa = new tb_Stock();
                                    //aa.AppDate = AppDate;
                                    //aa.Seq = Seq;
                                    //aa.App = "Shipping";
                                    //aa.Appid = Seq;
                                    //aa.CreateBy = ClassLib.Classlib.User;
                                    //aa.CreateDate = DateTime.Now;
                                    //aa.DocNo = txtSHNo.Text;
                                    //aa.RefNo = "";
                                    //aa.CodeNo = vv.CodeNo;
                                    //aa.Type = Type;
                                    //aa.QTY = -Convert.ToDecimal(QTY_temp);
                                    //aa.Inbound = 0;
                                    //aa.Outbound = -Convert.ToDecimal(QTY_temp);
                                    //aa.Type_i = 3;  //Receive = 1,Cancel Receive 2,Shipping = 3,Cancel Shipping = 4,Adjust stock = 5,ClearTemp = 6
                                    //aa.Category = Category;
                                    //aa.Refid = vv.id;
                                   
                                    //aa.CalDate = CalDate;
                                    //aa.Status = "Active";
                                    //aa.Flag_ClearTemp = 1; //0 คือ invoice,1 คือ Temp , 2 คือ clear temp แล้ว
                                    //aa.Type_in_out = Type_in_out;
                                    //aa.AmountCost = Amount;
                                    //aa.UnitCost = UnitCost;
                                    //aa.RemainQty = sum_Qty;
                                    //aa.RemainUnitCost = 0;
                                    //aa.RemainAmount = RemainAmount;
                                    //aa.Avg = Avg;

                                    //db.tb_Stocks.InsertOnSubmit(aa);
                                    //db.SubmitChanges();
                                    //dbClss.AddHistory(this.Name, "เบิกสินค้า", " เบิกสินค้าเลขที่ : " + txtSHNo.Text + " เบิก : " + Category + " CodeNo : " + vv.CodeNo + " จำนวน : " + (-QTY_temp).ToString() + " โดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", txtSHNo.Text);

                                }

                            }

                            //update Stock เข้า item
                            db.sp_010_Update_StockItem(Convert.ToString(vv.CodeNo), "");
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
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
            DataLoad();
            btnDel_Item.Enabled = false;
            btnSave.Enabled = false;
            btnNew.Enabled = true;
           
        }

        private void txtCodeNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {

                    Insert_data();
                    txtJobCard_Barcode.Text = "";

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
        private void Insert_data()
        {
            if (!txtJobCard_Barcode.Text.Equals("") )
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                   
                    var r = (from i in db.tb_JobCards
                                 //join s in db.tb_Stocks on i.CodeNo equals s.RefNo

                             where i.Status != "Cancel" 
                                && i.Status != "Completed"
                                && i.Status != "Discon"
                                && i.JobCard == txtJobCard_Barcode.Text
                                && i.Type != "Claim"
                             //&& h.VendorNo.Contains(VendorNo_ss)
                             select new
                             {
                                 CodeNo = i.CodeNo,
                                 ItemNo = i.ItemName,
                                 ItemDescription = i.ItemDesc,
                                 id = i.id,                                 
                                 JobCard = i.JobCard.Trim(),
                                 TempJobCard= i.TempJobCard.Trim(),
                                 CSTM_Name = i.CustomerName,
                                 CSTM_Addredd = i.Address,
                                 Duedate = i.Duedate,
                                 RemainQty = i.RemainQty,
                                 Qty = i.Qty,
                                 LotNo= i.LotNo,
                                 Unit = i.Unit,
                                 Remark = i.Remark,
                                 Status = i.Status

                             }
                    ).ToList();
                    if (r.Count > 0)
                    {
                        txtJobCard.Text = dbClss.TSt(r.FirstOrDefault().JobCard);
                        txtTempJobCard.Text = dbClss.TSt(r.FirstOrDefault().TempJobCard);
                        txtCSTM_Name.Text = dbClss.TSt(r.FirstOrDefault().CSTM_Name);
                        txtCSTM_Address.Text = dbClss.TSt(r.FirstOrDefault().CSTM_Addredd);
                        txtRemark.Text = dbClss.TSt(r.FirstOrDefault().Remark);
                        txtCodeNo.Text = dbClss.TSt(r.FirstOrDefault().CodeNo);
                        txtItemDescription.Text = dbClss.TSt(r.FirstOrDefault().ItemDescription);
                        txtItemName.Text = dbClss.TSt(r.FirstOrDefault().ItemNo);
                        if(dbClss.TDe(r.FirstOrDefault().RemainQty)>0)
                        {
                            txtQty.Text =  (dbClss.TDe(r.FirstOrDefault().RemainQty)).ToString("N2");
                            txtRemainQty.Text =(dbClss.TDe(r.FirstOrDefault().RemainQty)).ToString("N2");
                        }
                        else
                        {
                            txtQty.Text = (dbClss.TDe(r.FirstOrDefault().Qty)).ToString("N2");
                            txtRemainQty.Text = (dbClss.TDe(r.FirstOrDefault().Qty)).ToString("N2");
                        }
                        
                      
                        txtOrderQty.Text = dbClss.TDe(r.FirstOrDefault().Qty).ToString("N2");
                        txtCost.Text = "0.00";
                        txtUnitCost.Text = "0.00";
                        txtidJobCard.Text = dbClss.TSt(r.FirstOrDefault().id).ToString();

                        //if(dbClss.TSt(r.FirstOrDefault().Duedate)!="")
                        //    dtSHDate.Value = Convert.ToDateTime(r.FirstOrDefault().Duedate);

                    }

                    //Cal_Amount();

                }
            }
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
                Shipping_FG_List sc = new Shipping_FG_List(txtSHFG);
                this.Cursor = Cursors.Default;
                sc.ShowDialog();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();
                //LoadData
                
                string SHNo = txtSHFG.Text;
                if (!txtSHFG.Text.Equals(""))
                {
                    txtJobCard_Barcode.Text = "";

                    DataLoad();
                    Ac = "View";
                    btnDel_Item.Enabled = false;
                    btnSave.Enabled = false;
                    btnNew.Enabled = true;
                }
                
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : radButtonElement1_Click", this.Name); }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                PrintPR a = new PrintPR(txtSHFG.Text, txtSHFG.Text, "Shipping");
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

        private void btn_JobCard_Click(object sender, EventArgs e)
        {

        }

        private void btnCal_Click(object sender, EventArgs e)
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    decimal Amount = 0;
                    var g = (from ix in db.sp_040_Cal_Cost_JobCard_All(txtJobCard.Text,txtTempJobCard.Text,dbClss.TDe(txtRemainQty.Text),"","") select ix).ToList();
                    if(g.Count>0)
                    {
                        foreach(var gg in g)
                        {
                            Amount += dbClss.TDe(gg.Amount);
                        }

                        if(dbClss.TDe(txtRemainQty.Text)>0 && Amount>0)
                            txtUnitCost.Text = (Amount / dbClss.TDe(txtRemainQty.Text)).ToString("N2");

                        txtCost.Text = Amount.ToString("N2");
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void txtRemainQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            StockControl.dbClss.CheckDigitDecimal(e);
        }

        private void btnDiscon_Click(object sender, EventArgs e)
        {
            try
            {

                if (Ac.Equals("New") && txtTempJobCard.Text.Trim() !="")// || Ac.Equals("Edit"))
                {
                    //if (Check_Save())
                    //    return;
                    if (MessageBox.Show("ต้องการบันทึก ?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.Cursor = Cursors.WaitCursor;

                        using (DataClasses1DataContext db = new DataClasses1DataContext())
                        {
                            var j = (from ix in db.tb_JobCards
                                     where ix.TempJobCard.Trim() == txtTempJobCard.Text.Trim()
                                     && ix.Status != "Cancel"
                                      && ix.Status != "Completed"
                                      && ix.Status != "Discon"
                                      && ix.Type != "Claim"
                                     select ix).ToList();
                            if (j.Count > 0)
                            {
                                var jj = (from ix in db.tb_JobCards
                                          where ix.TempJobCard.Trim() == txtTempJobCard.Text.Trim()
                                          && ix.Status != "Cancel"
                                           && ix.Status != "Completed"
                                           && ix.Status != "Discon"
                                           && ix.Type != "Claim"
                                          select ix).First();

                                jj.Status = "Discon";
                                jj.RemainQty = 0;

                                db.SubmitChanges();
                                dbClss.AddHistory(this.Name, "แก้ไขใบร้องขอการผลิต", "ยกเลิกการผลิตงาน [" + txtTempJobCard.Text.Trim() + "]", txtTempJobCard.Text);
                            }
                        }
                        //if (!txtSHFG.Text.Equals(""))
                        //{
                        //SaveHerder();
                        //SaveDetail();


                        //DataLoad();
                        //btnNew.Enabled = true;
                        //btnDel_Item.Enabled = false;
                        //btnSave.Enabled = false;

                        MessageBox.Show("บันทึกสำเร็จ!");
                        //btnRefresh_Click(null,null);
                        //}
                        //else
                        //{
                        //    MessageBox.Show("ไม่สามารถโหลดเลขที่รับสินค้าได้ ติดต่อแผนก IT");
                        //}
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
            
    }
}
