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
    public partial class Shipping_AvgJobCard : Telerik.WinControls.UI.RadRibbonForm
    {
        public Shipping_AvgJobCard()
        {
            InitializeComponent();
        }
        public Shipping_AvgJobCard(string YYYY,string MM,string CodeNo)
        {
            InitializeComponent();
            YYYY_t = YYYY;
            MM_t = MM;
            CodeNo_t = CodeNo;
        }
        string YYYY_t = "";
        string MM_t = "";
        string CodeNo_t = "";
        string Ac = "";
        DataTable dt_h = new DataTable();
        DataTable dt_d = new DataTable();

        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            HistoryView hw = new HistoryView(this.Name, txtCodeNo.Text);
            this.Cursor = Cursors.Default;
            hw.ShowDialog();
        }

        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }
        private void GETDTRow()
        {
            dt_h.Columns.Add(new DataColumn("id", typeof(int)));
            dt_h.Columns.Add(new DataColumn("CodeNo", typeof(string)));
            dt_h.Columns.Add(new DataColumn("YYYY", typeof(string)));
            dt_h.Columns.Add(new DataColumn("MM", typeof(string)));
            dt_h.Columns.Add(new DataColumn("CreateDate", typeof(DateTime)));
            dt_h.Columns.Add(new DataColumn("CreateBy", typeof(string)));
            dt_h.Columns.Add(new DataColumn("Qty", typeof(decimal)));
            dt_h.Columns.Add(new DataColumn("Cost", typeof(decimal)));
            dt_h.Columns.Add(new DataColumn("Remark", typeof(string)));
            dt_h.Columns.Add(new DataColumn("Status", typeof(string)));
            //dt_h.Columns.Add(new DataColumn("YYYY", typeof(string)));
            //dt_h.Columns.Add(new DataColumn("MM", typeof(string)));


            dt_d.Columns.Add(new DataColumn("JobCard", typeof(string)));
            dt_d.Columns.Add(new DataColumn("TempJobCard", typeof(string)));
            dt_d.Columns.Add(new DataColumn("CodeNo", typeof(string)));
            dt_d.Columns.Add(new DataColumn("ItemNo", typeof(string)));
            dt_d.Columns.Add(new DataColumn("RefidJobCard", typeof(int)));
            dt_d.Columns.Add(new DataColumn("id", typeof(int)));
            dt_d.Columns.Add(new DataColumn("UnitCost", typeof(decimal)));
            dt_d.Columns.Add(new DataColumn("Amount", typeof(decimal)));
            dt_d.Columns.Add(new DataColumn("Qty", typeof(decimal)));
            dt_d.Columns.Add(new DataColumn("Remark", typeof(string)));
            dt_d.Columns.Add(new DataColumn("Unit", typeof(string)));
            dt_d.Columns.Add(new DataColumn("PCSUnit", typeof(decimal)));
            dt_d.Columns.Add(new DataColumn("YYYY", typeof(string)));
            dt_d.Columns.Add(new DataColumn("MM", typeof(string)));
            dt_d.Columns.Add(new DataColumn("Status", typeof(string)));

        }

        private void Unit_Load(object sender, EventArgs e)
        {
            //dgvData.ReadOnly = true;
            dgvData.AutoGenerateColumns = false;
            GETDTRow();

            DefaultItem();
            
            btnNew_Click(null, null);

            ddlMonth.Text = Convert.ToDateTime( DateTime.Now,new CultureInfo("en-US")).ToString("MM");
            ddlYear.Text = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("yyyy");

            if (!MM_t.Equals("") && !YYYY_t.Equals("") && !CodeNo_t.Equals(""))
            {
                btnNew.Enabled = true;
                txtCodeNo.Text = CodeNo_t;
                ddlMonth.Text = MM_t;
                ddlYear.Text = YYYY_t;
                txtJobCard_Barcode.Text = "";
                DataLoad();
                Enable_Status(true, "View");
                Ac = "View";

            }
            //else if (!CodeNo_t.Equals(""))
            //{
            //    btnNew.Enabled = true;
            //    txtJobCard_Barcode.Text = CodeNo_t;
            //    Insert_data_New();
            //    txtJobCard_Barcode.Text = "";
            //}

        }
        private void DefaultItem()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                ddlYear.DataSource = null;
                ddlYear.DisplayMember = "YYYY";
                ddlYear.ValueMember = "YYYY";

                var g = (from ix in db.sp_Select_Year() select ix).ToList();
                ddlYear.DataSource = g;
                ddlYear.SelectedIndex = 0;

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

            if (ddlMonth.Text != "" && ddlYear.Text != ""&& txtCodeNo.Text.Trim() != "")
            {

                dt_h.Rows.Clear();
                dt_d.Rows.Clear();
                try
                {

                    this.Cursor = Cursors.WaitCursor;
                    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    {

                        var g = (from ix in db.tb_Shipping_JobCardAvgHs select ix)
                            .Where(a => a.YYYY == ddlYear.Text.Trim()
                            && a.MM == ddlMonth.Text.Trim()
                            && a.CodeNo == txtCodeNo.Text.Trim()
                        ).ToList();
                        if (g.Count() > 0)
                        {
                            //DateTime? temp_date = null;
                            txtQty.Text = StockControl.dbClss.TDe(g.FirstOrDefault().Qty).ToString("N2");
                            txtCost.Text = StockControl.dbClss.TDe(g.FirstOrDefault().Cost).ToString("N2");
                            lblStatus.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Status);
                            txtCreateBy.Text = StockControl.dbClss.TSt(g.FirstOrDefault().CreateBy);

                            if (!StockControl.dbClss.TSt(g.FirstOrDefault().CreateDate).Equals(""))
                                txtCreateDate.Text = Convert.ToDateTime(g.FirstOrDefault().CreateDate).ToString("dd/MMM/yyyy");
                            else
                                txtCreateDate.Text = "";


                            var t = (from ix in db.tb_Items select ix)
                            .Where(a => a.CodeNo == txtCodeNo.Text && a.TypePart != "FG")
                            .ToList();
                            if (g.Count() > 0)
                            {
                                txtItemNo.Text = StockControl.dbClss.TSt(t.FirstOrDefault().ItemNo);
                                txtItemDescription.Text = StockControl.dbClss.TSt(t.FirstOrDefault().ItemDescription);

                            }

                            if (StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Cancel"))
                            {
                                btnSave.Enabled = false;
                                btnDelete.Enabled = false;
                                btnView.Enabled = false;
                                btnEdit.Enabled = false;
                                lblStatus.Text = "Cancel";
                                dgvData.ReadOnly = true;
                                btnNew.Enabled = true;
                                btnDel_Item.Enabled = false;
                                //txtCodeNo.Enabled = false;
                                btnAdd_Item.Enabled = false;
                            }
                            //else if
                            //    (StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Process"))
                            //{
                            //    btnSave.Enabled = false;
                            //    //btnDelete.Enabled = false;
                            //    //btnView.Enabled = false;
                            //    //btnEdit.Enabled = false;
                            //    lblStatus.Text = "Process";
                            //    dgvData.ReadOnly = false;
                            //    btnNew.Enabled = true;
                            //    btnDel_Item.Enabled = false;
                            //}
                            else if
                                (StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Completed"))

                            {
                                btnSave.Enabled = false;
                                btnDelete.Enabled = false;
                                btnView.Enabled = false;
                                btnEdit.Enabled = false;
                                lblStatus.Text = "Completed";
                                dgvData.ReadOnly = true;
                                btnNew.Enabled = true;
                                btnDel_Item.Enabled = false;
                                //txtCodeNo.Enabled = false;
                                btnAdd_Item.Enabled = false;
                            }
                            else
                            {
                                btnNew.Enabled = true;
                                btnSave.Enabled = true;
                                btnDelete.Enabled = true;
                                btnView.Enabled = true;
                                btnEdit.Enabled = true;
                                lblStatus.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Status);
                                dgvData.ReadOnly = false;
                                btnDel_Item.Enabled = false;
                                //txtCodeNo.Enabled = false;
                                btnAdd_Item.Enabled = true;
                            }
                            dt_h = StockControl.dbClss.LINQToDataTable(g);

                            //Detail
                            var d = (from ix in db.tb_Shipping_JobCardAvgs select ix)
                             .Where(a => a.YYYY == ddlYear.Text.Trim()
                            && a.MM == ddlMonth.Text.Trim()
                            && a.CodeNo == txtCodeNo.Text.Trim()
                                && a.Status != "Cancel").ToList();
                            if (d.Count() > 0)
                            {
                                int c = 0;
                                dgvData.DataSource = d;
                                dt_d = StockControl.dbClss.LINQToDataTable(d);

                                int id = 0;
                                foreach (var x in dgvData.Rows)
                                {
                                    c += 1;
                                    x.Cells["dgvNo"].Value = c;

                                    var r = (from i in db.tb_JobCards
                                                 //join s in db.tb_Stocks on i.CodeNo equals s.RefNo
                                             where //i.Status == "Waiting" //&& d.verticalID == VerticalID
                                               i.JobCard == dbClss.TSt(x.Cells["JobCard"].Value)
                                              && i.TempJobCard == dbClss.TSt(x.Cells["TempJobCard"].Value)

                                             select new
                                             {
                                                 CodeNo = i.CodeNo,
                                                 ItemName = i.ItemName,
                                                 TempJobCard = i.TempJobCard,
                                                 CustomerName = i.CustomerName,
                                                 //UnitCost = 0,//Convert.ToDecimal(dbClss.Get_Stock(i.CodeNo, "", "", "Avg")),//i.StandardCost
                                                 JobCard = i.JobCard,
                                                 Qty = i.Qty
                                             }
                                    ).ToList();
                                    if (r.Count > 0)
                                    {

                                        x.Cells["CodeNo"].Value = r.FirstOrDefault().CodeNo;
                                        //x.Cells["JobCard"].Value = r.FirstOrDefault().JobCard;
                                        // x.Cells["TempJobCard"].Value = r.FirstOrDefault().TempJobCard;
                                        x.Cells["CustomerName"].Value = r.FirstOrDefault().CustomerName;
                                        x.Cells["ItemName"].Value = r.FirstOrDefault().ItemName;
                                        //x.Cells["Qty"].Value = r.FirstOrDefault().Qty;
                                    }
                                }
                            }
                            //Cal_Amount();
                        }
                        else
                        {
                            string YYYY = ddlYear.Text;
                            string MM = ddlMonth.Text;
                            string Codno = txtCodeNo.Text;
                            btnNew_Click(null, null);
                            txtCodeNo.Text = Codno;
                            ddlMonth.Text = MM;
                            ddlYear.Text = YYYY;
                            if (txtCodeNo.Text != "")
                            {
                                var c = (from ix in db.tb_Items select ix)
                                   .Where(a => a.CodeNo == txtCodeNo.Text && a.TypePart != "FG")
                                   .ToList();
                                if (c.Count() > 0)
                                {
                                    txtItemNo.Text = StockControl.dbClss.TSt(c.FirstOrDefault().ItemNo);
                                    txtItemDescription.Text = StockControl.dbClss.TSt(c.FirstOrDefault().ItemDescription);
                                }
                            }
                        }

                    }

                }
                catch { }
                finally { this.Cursor = Cursors.Default; }
            }
            else
            {
                string YYYY = ddlYear.Text;
                string MM = ddlMonth.Text;
                btnNew_Click(null, null);
                ddlMonth.Text = MM;
                ddlYear.Text = YYYY;
            }
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
            txtCodeNo.Text = "";
            txtJobCard_Barcode.Text = "";
            txtCreateBy.Text = ClassLib.Classlib.User;
            txtItemNo.Text = "";
            txtItemDescription.Text = "";
            ddlMonth.Text = "";
            ddlYear.Text = "";
            //ddlMonth.Text = DateTime.Now.ToString("MM");
            //ddlYear.Text = DateTime.Now.ToString("yyyy");
            txtCreateDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            lblStatus.Text = "-";
            dgvData.Rows.Clear();
            txtTotal.Text = "0.00";
            txtCost.Text = "0.00";
            txtQty.Text = "0.00";
        }
        private void Enable_Status(bool ss,string Condition)
        {
            if (Condition.Equals("-") || Condition.Equals("New"))
            {
                txtJobCard_Barcode.Enabled = ss;
                //ddlYear.ReadOnly = !ss;
                //ddlMonth.ReadOnly = !ss;
                txtCodeNo.Enabled = ss;
                dgvData.ReadOnly = false;
                btnDel_Item.Enabled = ss;
                txtCost.Enabled = ss;
                txtQty.Enabled = ss;

            }
            else if (Condition.Equals("View"))
            {
                txtJobCard_Barcode.Enabled = ss;
                //ddlYear.ReadOnly = !ss;
                //ddlMonth.ReadOnly = !ss;
                //ddlYear.Enabled = ss;
                //ddlMonth.Enabled = ss;

                txtCodeNo.Enabled = ss;
                dgvData.ReadOnly = false;
                btnDel_Item.Enabled = ss;
                txtCost.Enabled = ss;
                txtQty.Enabled = ss;
            }

            else if (Condition.Equals("Edit"))
            {
                txtJobCard_Barcode.Enabled = ss;
                //ddlYear.ReadOnly = !ss;
                //ddlMonth.ReadOnly = !ss;
                //ddlYear.Enabled = ss;
                //ddlMonth.Enabled = ss;

                txtCodeNo.Enabled = ss;
                dgvData.ReadOnly = false;
                btnDel_Item.Enabled = ss;
                txtCost.Enabled = ss;
                txtQty.Enabled = ss;
            }
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            btnDel_Item.Enabled = true;
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            btnAdd_Item.Enabled = true;
            btnDelete.Enabled = true;
            btnView.Enabled = true;
            btnEdit.Enabled = true;
            ClearData();
            Enable_Status(true, "New");
            lblStatus.Text = "New";
            Ac = "New";

      
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

        private bool Check_Save()
        {
            bool re = true;
            string err = "";
            try
            {
               

                if (ddlYear.Text.Equals(""))
                    err += "- “ปี:” เป็นค่าว่าง \n";
                if (ddlMonth.Text.Equals(""))
                    err += "- “เดือน:” เป็นค่าว่าง \n";
                if (txtCodeNo.Text.Equals(""))
                    err += "- “รหัสทูล:” เป็นค่าว่าง \n";
                if (dgvData.Rows.Count <= 0)
                    err += "- “รายการ:” เป็นค่าว่าง \n";
                //int c = 0;
                //foreach (GridViewRowInfo rowInfo in dgvData.Rows)
                //{
                //    if (rowInfo.IsVisible)
                //    {
                //        if (StockControl.dbClss.TInt(rowInfo.Cells["QTY"].Value) <= (0))
                //        {
                //            err += "- “จำนวนเบิก:” ต้องมากกว่า 0 \n";
                //        }
                //        else if (StockControl.dbClss.TInt(rowInfo.Cells["QTY"].Value) != (0))
                //        {
                //            c += 1;
                //            //if (StockControl.dbClss.TSt(rowInfo.Cells["PRNo"].Value).Equals(""))
                //            //    err += "- “เลขที่ PR:” เป็นค่าว่าง \n";
                //            //if (StockControl.dbClss.TSt(rowInfo.Cells["TempNo"].Value).Equals(""))
                //            //    err += "- “เลขที่อ้างอิงเอกสาร PRNo:” เป็นค่าว่าง \n";
                //            if (StockControl.dbClss.TSt(rowInfo.Cells["CodeNo"].Value).Equals(""))
                //                err += "- “รหัสทูล:” เป็นค่าว่าง \n";
                //            if (StockControl.dbClss.TDe(rowInfo.Cells["QTY"].Value) > StockControl.dbClss.TDe(rowInfo.Cells["RemainQty"].Value))
                //                err += "- “จำนวนเบิก:” มากกว่าจำนวนคงเหลือ \n";
                //            if (StockControl.dbClss.TDe(rowInfo.Cells["UnitShip"].Value).Equals(""))
                //                err += "- “หน่วย:” เป็นค่าว่าง \n";

                //        }
                //    }
                //}

                //if (c <= 0)
                //    err += "- “กรุณาระบุจำนวนที่จะเบิกสินค้า:” เป็นค่าว่าง \n";


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
                var g = (from ix in db.tb_Shipping_JobCardAvgHs select ix)
                           .Where(a => a.YYYY == ddlYear.Text.Trim()
                           && a.MM == ddlMonth.Text.Trim()
                           && a.CodeNo == txtCodeNo.Text.Trim()
                           && a.Status != "Completed").ToList();
                if (g.Count > 0)  //มีรายการในระบบ
                {
                    foreach (DataRow row in dt_h.Rows)
                    {
                       
                        var gg = (from ix in db.tb_Shipping_JobCardAvgHs select ix)
                           .Where(a => a.YYYY == ddlYear.Text.Trim()
                           && a.MM == ddlMonth.Text.Trim()
                           && a.CodeNo == txtCodeNo.Text.Trim()
                           && a.Status != "Completed").First();
                        
                        gg.CreateBy = ClassLib.Classlib.User;
                        gg.CreateDate = DateTime.Now;

                        dbClss.AddHistory(this.Name, "แก้ไขการเบิกFG", "แก้ไขโดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", txtCodeNo.Text + "-" + ddlYear.Text + ddlMonth.Text);

                        //if (StockControl.dbClss.TSt(gg.BarCode).Equals(""))
                        //    gg.BarCode = StockControl.dbClss.SaveQRCode2D(txtSHNo.Text.Trim());

                        if (!txtCost.Text.Trim().Equals(row["Cost"].ToString()))
                        {
                            decimal Cost = 0; decimal.TryParse(txtCost.Text, out Cost);
                            gg.Cost = Cost;
                            dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไข ราคา[" + Cost.ToString() + "]", txtCodeNo.Text + "-" + ddlYear.Text + ddlMonth.Text);
                        }
                        if (!txtQty.Text.Trim().Equals(row["Qty"].ToString()))
                        {
                            decimal Qty = 0; decimal.TryParse(txtQty.Text, out Qty);
                            gg.Qty = Qty;
                            dbClss.AddHistory(this.Name, "แก้ไข ทูล", "แก้ไข จำนวน[" + Qty.ToString() + "]", txtCodeNo.Text + "-" + ddlYear.Text + ddlMonth.Text);
                        }
                        
                        db.SubmitChanges();
                    }
                }
                else //สร้างใหม่
                {
                    //byte[] barcode = null;
                    ////barcode = StockControl.dbClss.SaveQRCode2D(txtSHNo.Text.Trim());
                    //DateTime? UpdateDate = null;

                    tb_Shipping_JobCardAvgH gg = new tb_Shipping_JobCardAvgH();
                    gg.YYYY = ddlYear.Text;
                    gg.MM = ddlMonth.Text;
                    gg.CreateBy = ClassLib.Classlib.User;
                    gg.CreateDate = DateTime.Now;
                    gg.Qty = dbClss.TDe(txtQty.Text);
                    gg.Cost = dbClss.TDe(txtCost.Text);
                    gg.CodeNo = txtCodeNo.Text;
                    gg.Status = "Process";
                   
                    db.tb_Shipping_JobCardAvgHs.InsertOnSubmit(gg);
                    db.SubmitChanges();

                    dbClss.AddHistory(this.Name, "แก้ไขการเบิกFG Avg", "สร้าง การเบิกสินค้า FG Avg [ Qty : " + gg.Qty.ToString() +" Cost : " + gg.Cost.ToString() + "]", txtCodeNo.Text + "-" + ddlYear.Text + ddlMonth.Text);
                }
            }
        }
        private void SaveDetail()
        {
            dgvData.EndEdit();

            //DateTime? RequireDate = DateTime.Now;
            //if (!dtRequire.Text.Equals(""))
            //    RequireDate = dtRequire.Value;
            //int Seq = 0;
            //DateTime? UpdateDate = null;
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                
                foreach (var g in dgvData.Rows)
                {
                    string SS = "";
                    if (g.IsVisible.Equals(true))
                    {

                        if (StockControl.dbClss.TInt(g.Cells["id"].Value) <= 0)  //New ใหม่
                        {

                            tb_Shipping_JobCardAvg u = new tb_Shipping_JobCardAvg();
                            u.YYYY = ddlYear.Text;
                            u.CodeNo = txtCodeNo.Text;
                            u.TempJobCard = StockControl.dbClss.TSt(g.Cells["TempJobCard"].Value);
                            u.MM = ddlMonth.Text;
                            u.JobCard = StockControl.dbClss.TSt(g.Cells["JobCard"].Value);
                            u.Unit = StockControl.dbClss.TSt(g.Cells["Unit"].Value);
                            u.UnitCost = StockControl.dbClss.TDe(g.Cells["UnitCost"].Value);
                            //u.Qty = StockControl.dbClss.TDe(g.Cells["Qty"].Value);
                            //u.PCSUnit = StockControl.dbClss.TDe(g.Cells["PCSUnit"].Value);
                            //u.Amount = StockControl.dbClss.TDe(g.Cells["Amount"].Value);
                            u.Remark = StockControl.dbClss.TSt(g.Cells["Remark"].Value);
                            u.RefidJobCard = StockControl.dbClss.TInt(g.Cells["RefidJobCard"].Value);
                            u.Status = "Waiting";
                            u.CreateBy = ClassLib.Classlib.User;
                            u.CreateDate = DateTime.Now;
                            u.Qty = StockControl.dbClss.TDe(g.Cells["Qty"].Value);
                            db.tb_Shipping_JobCardAvgs.InsertOnSubmit(u);
                            db.SubmitChanges();

                            dbClss.AddHistory(this.Name, "แก้ไขการเบิกFG Avg", "เพิ่มรายการเบิก FG Avg  [" + StockControl.dbClss.TSt(g.Cells["JobCard"].Value) + "]", txtCodeNo.Text + "-" + ddlYear.Text + ddlMonth.Text);

                        }

                        else
                        {
                            if (StockControl.dbClss.TInt(g.Cells["id"].Value) > 0)
                            {
                                foreach (DataRow row in dt_d.Rows)
                                {
                                    if (StockControl.dbClss.TInt(g.Cells["id"].Value) == StockControl.dbClss.TInt(row["id"]))
                                    {
                                        var d = (from ix in db.tb_Shipping_JobCardAvgs select ix)
                                              .Where(a => a.YYYY == ddlYear.Text.Trim()
                                              && a.MM == ddlMonth.Text.Trim()
                                              && a.CodeNo == txtCodeNo.Text.Trim()
                                              && a.Status != "Cancel"
                                              && a.id == StockControl.dbClss.TInt(g.Cells["id"].Value)
                                              ).ToList();
                                        if (d.Count > 0)
                                        {
                                            var u = (from ix in db.tb_Shipping_JobCardAvgs select ix)
                                             .Where(a => a.YYYY == ddlYear.Text.Trim()
                                             && a.MM == ddlMonth.Text.Trim()
                                             && a.CodeNo == txtCodeNo.Text.Trim()
                                             && a.Status != "Cancel"
                                             && a.id == StockControl.dbClss.TInt(g.Cells["id"].Value)
                                              ).First();

                                            u.CreateDate = DateTime.Now;
                                            u.CreateBy = ClassLib.Classlib.User;
                                            dbClss.AddHistory(this.Name, "แก้ไขการเบิกFG Avg", " แก้ไขรายการเบิก id :" + StockControl.dbClss.TSt(g.Cells["id"].Value)
                                           + " JobCard :" + StockControl.dbClss.TSt(g.Cells["JobCard"].Value)
                                           + " แก้ไขโดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", txtCodeNo.Text + "-" + ddlYear.Text + ddlMonth.Text);

                                            if (!txtCodeNo.Text.Trim().Equals(row["CodeNo"].ToString()))
                                            {
                                                u.CodeNo = txtCodeNo.Text.Trim();
                                                dbClss.AddHistory(this.Name, "แก้ไขการเบิกFG Avg", "แก้ไขรหัสทูล [" + u.CodeNo + "]", txtCodeNo.Text + "-" + ddlYear.Text + ddlMonth.Text);
                                            }
                                            //if (!StockControl.dbClss.TSt(g.Cells["Qty"].Value).Equals(row["Qty"].ToString()))
                                            //{
                                            //    decimal Qty = 0; decimal.TryParse(StockControl.dbClss.TSt(g.Cells["Qty"].Value), out Qty);
                                            //    u.Qty = StockControl.dbClss.TDe(g.Cells["Qty"].Value);
                                            //    dbClss.AddHistory(this.Name, "แก้ไขการเบิกFG Avg", "แก้ไขจำนวนเบิก [" + Qty.ToString() + "]", txtCodeNo.Text + "-" + ddlYear.Text + ddlMonth.Text);
                                            //}
                                            //if (!StockControl.dbClss.TSt(g.Cells["Unit"].Value).Equals(row["Unit"].ToString()))
                                            //{
                                            //    u.Unit = StockControl.dbClss.TSt(g.Cells["Unit"].Value);
                                            //    dbClss.AddHistory(this.Name, "แก้ไขการเบิกFG Avg", "แก้ไขหน่วย [" + u.Unit + "]", txtCodeNo.Text + "-" + ddlYear.Text + ddlMonth.Text);
                                            //}
                                            if (!StockControl.dbClss.TSt(g.Cells["UnitCost"].Value).Equals(row["UnitCost"].ToString()))
                                            {
                                                u.Qty = StockControl.dbClss.TDe(g.Cells["Qty"].Value);
                                                decimal UnitCost = 0; decimal.TryParse(StockControl.dbClss.TSt(g.Cells["UnitCost"].Value), out UnitCost);
                                                u.UnitCost = StockControl.dbClss.TDe(g.Cells["UnitCost"].Value);
                                                dbClss.AddHistory(this.Name, "แก้ไขการเบิกFG Avg", "แก้ไขจำนวนเบิก [" + UnitCost.ToString() + "]", txtCodeNo.Text + "-" + ddlYear.Text + ddlMonth.Text);
                                            }
                                            if (!StockControl.dbClss.TSt(g.Cells["JobCard"].Value).Equals(row["JobCard"].ToString()))
                                            {
                                                u.JobCard = StockControl.dbClss.TSt(g.Cells["JobCard"].Value);
                                                dbClss.AddHistory(this.Name, "แก้ไขการเบิกFG Avg", "แก้ไข JobCard [" + u.JobCard + "]", txtCodeNo.Text + "-" + ddlYear.Text + ddlMonth.Text);
                                            }
                                            if (!StockControl.dbClss.TSt(g.Cells["TempJobCard"].Value).Equals(row["TempJobCard"].ToString()))
                                            {
                                                u.TempJobCard = StockControl.dbClss.TSt(g.Cells["TempJobCard"].Value);
                                                dbClss.AddHistory(this.Name, "แก้ไขการเบิกFG Avg", "แก้ไข TempJobCard [" + u.TempJobCard + "]", txtCodeNo.Text + "-" + ddlYear.Text + ddlMonth.Text);
                                            }
                                            if (!StockControl.dbClss.TSt(g.Cells["RefidJobCard"].Value).Equals(row["RefidJobCard"].ToString()))
                                            {
                                                u.RefidJobCard = StockControl.dbClss.TInt(g.Cells["RefidJobCard"].Value);
                                                dbClss.AddHistory(this.Name, "แก้ไขการเบิกFG Avg", "แก้ไข RefidJobCard [" + u.RefidJobCard.ToString() + "]", txtCodeNo.Text + "-" + ddlYear.Text + ddlMonth.Text);
                                            }

                                            //if (!StockControl.dbClss.TSt(g.Cells["PCSUnit"].Value).Equals(row["PCSUnit"].ToString()))
                                            //{
                                            //    u.PCSUnit = StockControl.dbClss.TDe(g.Cells["PCSUnit"].Value);
                                            //    dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไขจำนวน/หน่วย [" + u.PCSUnit + "]", txtSHNo.Text);
                                            //}

                                            if (!StockControl.dbClss.TSt(g.Cells["Remark"].Value).Equals(row["Remark"].ToString()))
                                            {
                                                u.Remark = StockControl.dbClss.TSt(g.Cells["Remark"].Value);
                                                dbClss.AddHistory(this.Name, "แก้ไขการเบิกFG Avg", "แก้ไขวัตถุประสงค์ [" + u.Remark + "]", txtCodeNo.Text + "-" + ddlYear.Text + ddlMonth.Text);
                                            }

                                            u.Status = "Waiting";
                                            db.SubmitChanges();
                                        }
                                    }
                                }
                            }
                        }

                    }
                    else  //Delete
                    {
                        if (StockControl.dbClss.TInt(g.Cells["id"].Value) > 0)
                        {
                            var d = (from ix in db.tb_Shipping_JobCardAvgs select ix)
                                                  .Where(a => a.YYYY == ddlYear.Text.Trim()
                                                  && a.MM == ddlMonth.Text.Trim()
                                                  && a.CodeNo == txtCodeNo.Text.Trim()
                                                  && a.Status != "Cancel"
                                                  && a.id == StockControl.dbClss.TInt(g.Cells["id"].Value)
                                                  ).ToList();
                            if (d.Count > 0)
                            {
                                //delete
                                db.tb_Shipping_JobCardAvgs.DeleteAllOnSubmit(d);
                                db.SubmitChanges();
                            }
                        }
                    }
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

                    //if (Ac.Equals("New"))
                    //    txtSHNo.Text = StockControl.dbClss.GetNo(5, 2);

                    if (!txtCodeNo.Text.Equals("") && !ddlMonth.Text.Equals("") && !ddlYear.Text.Equals(""))
                    {
                        btnCal_Click(null, null);//Cal ก่อน Save
                        SaveHerder();
                        SaveDetail();
                        
                        //DataLoad();
                        btnNew.Enabled = true;
                        btnDel_Item.Enabled = false;

                        ////insert sotck
                        //InsertStock_new();

                        MessageBox.Show("บันทึกสำเร็จ!");
                        btnRefresh_Click(null, null);
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
            //try
            //{

            //    using (DataClasses1DataContext db = new DataClasses1DataContext())
            //    {
            //        DateTime? CalDate = null;
            //        DateTime? AppDate = DateTime.Now;
            //        int Seq = 0;
                    
                    

            //        string CNNo = CNNo = StockControl.dbClss.GetNo(6, 2);
            //        var g = (from ix in db.tb_Shippings
            //                     //join i in db.tb_Items on ix.CodeNo equals i.CodeNo
            //                 where ix.ShippingNo.Trim() == txtSHNo.Text.Trim() && ix.Status != "Cancel"

            //                 select ix).ToList();
            //        if (g.Count > 0)
            //        {
            //            //insert Stock

            //            foreach (var vv in g)
            //            {
            //                Seq += 1;

            //                tb_Stock1 gg = new tb_Stock1();
            //                gg.AppDate = AppDate;
            //                gg.Seq = Seq;
            //                gg.App = "Shipping";
            //                gg.Appid = Seq;
            //                gg.CreateBy = ClassLib.Classlib.User;
            //                gg.CreateDate = DateTime.Now;
            //                gg.DocNo = CNNo;
            //                gg.RefNo = txtSHNo.Text;
            //                gg.Type = "Ship";
            //                gg.QTY = -Convert.ToDecimal(vv.QTY);
            //                gg.Inbound = 0;
            //                gg.Outbound = -Convert.ToDecimal(vv.QTY); ;
            //                gg.AmountCost = (-Convert.ToDecimal(vv.QTY)) * get_cost(vv.CodeNo);
            //                gg.UnitCost = get_cost(vv.CodeNo);
            //                gg.RemainQty = 0;
            //                gg.RemainUnitCost = 0;
            //                gg.RemainAmount = 0;
            //                gg.CalDate = CalDate;
            //                gg.Status = "Active";

            //                db.tb_Stock1s.InsertOnSubmit(gg);
            //                db.SubmitChanges();

            //                dbClss.Insert_Stock(vv.CodeNo, (-Convert.ToDecimal(vv.QTY)), "Shipping", "Inv");


            //            }
            //        }
            //    }
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void InsertStock_new()
        {
            //try
            //{

            //    using (DataClasses1DataContext db = new DataClasses1DataContext())
            //    {
            //        DateTime? CalDate = null;
            //        DateTime? AppDate = DateTime.Now;
            //        int Seq = 0;
            //        string Type = "Shipping";
            //        string Category = ""; //Temp,Invoice
            //        decimal Cost = 0;
            //       // int Flag_ClearTemp = 0;
            //        decimal Qty_Inv = 0;
            //        decimal Qty_DL = 0;
            //        decimal Qty_Remain = 0;
            //        decimal QTY = 0;
            //        decimal QTY_temp = 0;

            //        string Type_in_out = "Out";
            //        decimal RemainQty = 0;
            //        decimal Amount = 0;
            //        decimal RemainAmount = 0;
            //        decimal Avg = 0;
            //        decimal UnitCost = 0;
            //        decimal sum_Remain = 0;
            //        decimal sum_Qty = 0;

            //        //string CNNo = CNNo = StockControl.dbClss.GetNo(6, 2);
            //        var g = (from ix in db.tb_Shippings
            //                     //join i in db.tb_Items on ix.CodeNo equals i.CodeNo
            //                 where ix.ShippingNo.Trim() == txtSHNo.Text.Trim() && ix.Status != "Cancel"

            //                 select ix).ToList();
            //        if (g.Count > 0)
            //        {
            //            //insert Stock

            //            foreach (var vv in g)
            //            {
            //                Seq += 1;

            //                QTY = Convert.ToDecimal(vv.QTY);
            //                QTY_temp = 0;
            //                Qty_Remain = (Convert.ToDecimal(db.Cal_QTY(vv.CodeNo, "", 0)));  //sum ทั้งหมด
            //                Qty_Inv = (Convert.ToDecimal(db.Cal_QTY(vv.CodeNo, "Invoice", 0))); //sum เฉพาะ Invoice
            //                Qty_DL = (Convert.ToDecimal(db.Cal_QTY(vv.CodeNo, "Temp", 0))); // sum เฉพาะ DL
                            
            //                if (QTY <= Qty_Remain)
            //                {                               

            //                    if (Qty_Inv >= QTY) //ถ้า จำนวน remain มีมากกว่าจำนวนที่จะลบ
            //                    {
            //                        UnitCost = Convert.ToDecimal(vv.UnitCost);
            //                        //if (UnitCost <= 0)
            //                        //    UnitCost = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "Avg"));

            //                        Amount = (-QTY) * UnitCost;

            //                        //แบบที่ 1 จะไป sum ใหม่
            //                        RemainQty = (Convert.ToDecimal(db.Cal_QTY(vv.CodeNo, "", 0)));
            //                        //แบบที่ 2 จะไปดึงล่าสุดมา
            //                        //RemainQty = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainQty"));
            //                        sum_Remain = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainAmount"))
            //                            + Amount;

            //                        sum_Qty = RemainQty + (-QTY);
            //                        Avg = UnitCost;//sum_Remain / sum_Qty;
            //                        RemainAmount = sum_Remain;


            //                        Category = "Invoice";
            //                        tb_Stock gg = new tb_Stock();
            //                        gg.AppDate = AppDate;
            //                        gg.Seq = Seq;
            //                        gg.App = "Shipping";
            //                        gg.Appid = Seq;
            //                        gg.CreateBy = ClassLib.Classlib.User;
            //                        gg.CreateDate = DateTime.Now;
            //                        gg.DocNo = txtSHNo.Text;
            //                        gg.RefNo = "";
            //                        gg.CodeNo = vv.CodeNo;
            //                        gg.Type = Type;
            //                        gg.QTY = -Convert.ToDecimal(QTY);
            //                        gg.Inbound = 0;
            //                        gg.Outbound = -Convert.ToDecimal(QTY);
            //                        gg.Type_i = 3;  //Receive = 1,Cancel Receive 2,Shipping = 3,Cancel Shipping = 4,Adjust stock = 5,ClearTemp = 6
            //                        gg.Category = Category;
            //                        gg.Refid = vv.id;
                                    
            //                        gg.CalDate = CalDate;
            //                        gg.Status = "Active";
            //                        gg.Flag_ClearTemp =0; //0 คือ invoice,1 คือ Temp , 2 คือ clear temp แล้ว
            //                        gg.Type_in_out = Type_in_out;
            //                        gg.AmountCost = Amount;
            //                        gg.UnitCost = UnitCost;
            //                        gg.RemainQty = sum_Qty;
            //                        gg.RemainUnitCost = 0;
            //                        gg.RemainAmount = RemainAmount;
            //                        gg.Avg = Avg;
            //                        gg.TLCost = 0;
            //                        gg.TLQty = 0;
            //                        gg.ShipQty = 0;

            //                        db.tb_Stocks.InsertOnSubmit(gg);
            //                        db.SubmitChanges();

            //                        dbClss.AddHistory(this.Name, "เบิกสินค้า", " เบิกสินค้าเลขที่ : " + txtSHNo.Text + " เบิก : " + Category + " CodeNo : " + vv.CodeNo + " จำนวน : " + (-QTY).ToString() + " โดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", txtSHNo.Text);

            //                    }
            //                    else
            //                    {
            //                        QTY_temp = QTY - Qty_Inv;

            //                        UnitCost = Convert.ToDecimal(vv.UnitCost);
            //                        //if (UnitCost <= 0)
            //                        //    UnitCost = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "Avg"));
                                    
            //                        Amount = (-QTY) * UnitCost;

            //                        //แบบที่ 1 จะไป sum ใหม่
            //                        RemainQty = (Convert.ToDecimal(db.Cal_QTY(vv.CodeNo, "", 0)));
            //                        //แบบที่ 2 จะไปดึงล่าสุดมา
            //                        //RemainQty = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainQty"));
            //                        sum_Remain = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainAmount"))
            //                            + Amount;

            //                        sum_Qty = RemainQty + (-QTY);
            //                        Avg = UnitCost;//sum_Remain / sum_Qty;
            //                        RemainAmount = sum_Remain;

            //                        Category = "Temp";
            //                        tb_Stock gg = new tb_Stock();
            //                        gg.AppDate = AppDate;
            //                        gg.Seq = Seq;
            //                        gg.App = "Shipping";
            //                        gg.Appid = Seq;
            //                        gg.CreateBy = ClassLib.Classlib.User;
            //                        gg.CreateDate = DateTime.Now;
            //                        gg.DocNo = txtSHNo.Text;
            //                        gg.RefNo = "";
            //                        gg.CodeNo = vv.CodeNo;
            //                        gg.Type = Type;
            //                        gg.QTY = -Convert.ToDecimal(QTY);
            //                        gg.Inbound = 0;
            //                        gg.Outbound = -Convert.ToDecimal(QTY);
            //                        gg.Type_i = 3;  //Receive = 1,Cancel Receive 2,Shipping = 3,Cancel Shipping = 4,Adjust stock = 5,ClearTemp = 6
            //                        gg.Category = Category;
            //                        gg.Refid = vv.id;
                                    
            //                        gg.CalDate = CalDate;
            //                        gg.Status = "Active";
            //                        gg.Flag_ClearTemp = 1; //0 คือ invoice,1 คือ Temp , 2 คือ clear temp แล้ว
            //                        gg.Type_in_out = Type_in_out;
            //                        gg.AmountCost = Amount;
            //                        gg.UnitCost = UnitCost;
            //                        gg.RemainQty = sum_Qty;
            //                        gg.RemainUnitCost = 0;
            //                        gg.RemainAmount = RemainAmount;
            //                        gg.Avg = Avg;
            //                        gg.TLCost = 0;
            //                        gg.TLQty = 0;
            //                        gg.ShipQty = 0;

            //                        db.tb_Stocks.InsertOnSubmit(gg);
            //                        db.SubmitChanges();
            //                        dbClss.AddHistory(this.Name, "เบิกสินค้า", " เบิกสินค้าเลขที่ : " + txtSHNo.Text + " เบิก : " + Category + " CodeNo : " + vv.CodeNo + " จำนวน : " + (-QTY).ToString() + " โดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", txtSHNo.Text);


            //                        //// --Stock ใน Invoice ไม่พอ ต้องเอาที่ Temp มา

            //                        //UnitCost = Convert.ToDecimal(vv.UnitCost);
            //                        //if (UnitCost <= 0)
            //                        //    UnitCost = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "Avg"));
                                    
            //                        //Amount = (-QTY_temp) * UnitCost;

            //                        ////แบบที่ 1 จะไป sum ใหม่
            //                        //RemainQty = (Convert.ToDecimal(db.Cal_QTY(vv.CodeNo, "", 0)));
            //                        ////แบบที่ 2 จะไปดึงล่าสุดมา
            //                        ////RemainQty = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainQty"));
            //                        //sum_Remain = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainAmount"))
            //                        //    + Amount;
            //                        //sum_Qty = RemainQty + (-QTY_temp);
            //                        //Avg = UnitCost;//sum_Remain / sum_Qty;
            //                        //RemainAmount = sum_Remain;

            //                        //Category = "Invoice";
            //                        //tb_Stock aa = new tb_Stock();
            //                        //aa.AppDate = AppDate;
            //                        //aa.Seq = Seq;
            //                        //aa.App = "Shipping";
            //                        //aa.Appid = Seq;
            //                        //aa.CreateBy = ClassLib.Classlib.User;
            //                        //aa.CreateDate = DateTime.Now;
            //                        //aa.DocNo = txtSHNo.Text;
            //                        //aa.RefNo = "";
            //                        //aa.CodeNo = vv.CodeNo;
            //                        //aa.Type = Type;
            //                        //aa.QTY = -Convert.ToDecimal(QTY_temp);
            //                        //aa.Inbound = 0;
            //                        //aa.Outbound = -Convert.ToDecimal(QTY_temp);
            //                        //aa.Type_i = 3;  //Receive = 1,Cancel Receive 2,Shipping = 3,Cancel Shipping = 4,Adjust stock = 5,ClearTemp = 6
            //                        //aa.Category = Category;
            //                        //aa.Refid = vv.id;
                                   
            //                        //aa.CalDate = CalDate;
            //                        //aa.Status = "Active";
            //                        //aa.Flag_ClearTemp = 1; //0 คือ invoice,1 คือ Temp , 2 คือ clear temp แล้ว
            //                        //aa.Type_in_out = Type_in_out;
            //                        //aa.AmountCost = Amount;
            //                        //aa.UnitCost = UnitCost;
            //                        //aa.RemainQty = sum_Qty;
            //                        //aa.RemainUnitCost = 0;
            //                        //aa.RemainAmount = RemainAmount;
            //                        //aa.Avg = Avg;

            //                        //db.tb_Stocks.InsertOnSubmit(aa);
            //                        //db.SubmitChanges();
            //                        //dbClss.AddHistory(this.Name, "เบิกสินค้า", " เบิกสินค้าเลขที่ : " + txtSHNo.Text + " เบิก : " + Category + " CodeNo : " + vv.CodeNo + " จำนวน : " + (-QTY_temp).ToString() + " โดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", txtSHNo.Text);

            //                    }

            //                }

            //                //update Stock เข้า item
            //                db.sp_010_Update_StockItem(Convert.ToString(vv.CodeNo), "");
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
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

                    //Insert_data(); ของเก่า
                    //New
                    Insert_data_New();
                    txtJobCard_Barcode.Text = "";

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
            if (!txtJobCard_Barcode.Text.Equals("") && !Duppicate(txtJobCard_Barcode.Text))
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    //int No = 0;
                    //string CodeNo = "";
                    //string ItemNo = "";
                    //string ItemDescription = "";
                    //decimal QTY = 0;
                    //decimal RemainQty = 0;
                    //string UnitShip = "";
                    //decimal PCSUnit = 0;
                    //decimal StandardCost = 0;
                    //decimal Amount = 0;
                    ////string CRRNCY = "";
                    //string LotNo = "";
                    //string SerialNo = "";
                    //string Remark = "";
                    //string MachineName = "";
                    // string Status = "Waiting";
                    //int id = 0;
                    int dgvNo = 0;

                    var r = (from i in db.tb_JobCards
                                 //join s in db.tb_Stocks on i.CodeNo equals s.RefNo
                             where (i.Status == "Waiting" || i.Status == "Partial") //&& d.verticalID == VerticalID

                                && i.JobCard == txtJobCard_Barcode.Text
                             //&& h.VendorNo.Contains(VendorNo_ss)
                             select new
                             {
                                 CodeNo = i.CodeNo,
                                 ItemName = i.ItemName,
                                 TempJobCard = i.TempJobCard,
                                 CustomerName = i.CustomerName,
                                 UnitCost = 0,//Convert.ToDecimal(dbClss.Get_Stock(i.CodeNo, "", "", "Avg")),//i.StandardCost
                                 JobCard = i.JobCard,
                                 Remark = "",
                                 id = 0,
                                 RefidJobCard = i.id,
                                 PCSUnit = 1,
                                 Unit = i.Unit,
                                 Qty =0

                             }
                    ).ToList();
                    if (r.Count > 0)
                    {
                        dgvNo = dgvData.Rows.Count() + 1;

                        foreach (var vv in r)
                        {
                            Add_Item(dgvNo, vv.CodeNo, vv.JobCard, vv.TempJobCard
                                        , vv.CustomerName, vv.ItemName, vv.UnitCost, vv.Remark,vv.id,vv.RefidJobCard,vv.PCSUnit,vv.Unit,vv.Qty);

                        }
                    }
                    btnCal_Click(null,null);

                }
            }
        }

        private void Add_Item(int Row, string CodeNo, string JobCard
            , string TempJobCard ,string CustomerName,string ItemName
           , decimal UnitCost
            ,string Remark,int id,int RefidJobCard,decimal PCSUnit,string Unit,decimal Qty)
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
                ee.Cells["JobCard"].Value = JobCard;
                ee.Cells["TempJobCard"].Value = TempJobCard;
                ee.Cells["CustomerName"].Value = CustomerName;
                ee.Cells["ItemName"].Value = ItemName;
                ee.Cells["UnitCost"].Value = UnitCost;
                ee.Cells["Remark"].Value = Remark;
                ee.Cells["id"].Value = id;
                ee.Cells["RefidJobCard"].Value = RefidJobCard;
                ee.Cells["PCSUnit"].Value = PCSUnit;
                ee.Cells["Qty"].Value = Qty;


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
        //private void Insert_data()
        //{
        //    if (!txtCodeNo.Text.Equals("") && !Duppicate(txtCodeNo.Text))
        //    {
        //        using (DataClasses1DataContext db = new DataClasses1DataContext())
        //        {
        //            //int No = 0;
        //            //string CodeNo = "";
        //            //string ItemNo = "";
        //            //string ItemDescription = "";
        //            //decimal QTY = 0;
        //            //decimal RemainQty = 0;
        //            //string UnitShip = "";
        //            //decimal PCSUnit = 0;
        //            //decimal StandardCost = 0;
        //            //decimal Amount = 0;
        //            ////string CRRNCY = "";
        //            //string LotNo = "";
        //            //string SerialNo = "";
        //            //string Remark = "";
        //            //string MachineName = "";
        //            // string Status = "Waiting";
        //            //int id = 0;
        //            int dgvNo = 0;

        //            var r = (from i in db.tb_Items
        //                         //join s in db.tb_Stocks on i.CodeNo equals s.RefNo

        //                     where i.Status == "Active" //&& d.verticalID == VerticalID
        //                        && i.CodeNo == txtCodeNo.Text
        //                     //&& h.VendorNo.Contains(VendorNo_ss)
        //                     select new
        //                     {
        //                         CodeNo = i.CodeNo,
        //                         ItemNo = i.ItemNo,
        //                         ItemDescription = i.ItemDescription,
        //                         RemainQty = (Convert.ToDecimal(db.Cal_QTY(i.CodeNo, "", 0))),
        //                         UnitShip = i.UnitShip,
        //                         PCSUnit = i.PCSUnit,
        //                         StandardCodt = i.StandardCost,//Convert.ToDecimal(dbClss.Get_Stock(i.CodeNo, "", "", "Avg")),//i.StandardCost
        //                         Amount = 0,
        //                         QTY = 0,
        //                         LotNo = "",
        //                         SerialNo = "",
        //                         MachineName = "",
        //                         LineName ="",
        //                         Remark ="",
        //                         id = 0
                                 
        //                     }
        //            ).ToList();
        //            if (r.Count > 0)
        //            {
        //                dgvNo = dgvData.Rows.Count() + 1;

        //                foreach (var vv in r)
        //                {
        //                    dgvData.Rows.Add(dgvNo.ToString(),vv.CodeNo, vv.ItemNo, vv.ItemDescription
        //                                , vv.RemainQty, vv.QTY, vv.UnitShip, vv.PCSUnit, vv.StandardCodt, vv.Amount,
        //                                vv.LotNo, vv.SerialNo, vv.MachineName, vv.LineName, vv.Remark, vv.id
        //                                );
        //                }

        //            }
                    
        //            Cal_Amount();
                     
        //        }
        //    }
        //}

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
                        string JobCard = "";
                        JobCard = StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["JobCard"].Value);
                        if (MessageBox.Show("ต้องการลบรายการ ( " + JobCard + " ) ออกจากรายการ หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                Shipping_AvgJobCard_List sc = new Shipping_AvgJobCard_List(ddlYear,ddlMonth,txtCodeNo);
                this.Cursor = Cursors.Default;
                sc.ShowDialog();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();
                //LoadData

                //string Year = ddlYear.Text;
                //string Month = ddlMonth.Text;
                //string CodeNo = txtCodeNo.Text;

                if (!ddlYear.Text.Equals("") && !ddlMonth.Text.Equals("") && !txtCodeNo.Text.Equals(""))
                {
                    txtJobCard_Barcode.Text = "";

                    DataLoad();
                    Ac = "View";
                    Enable_Status(true, "View");
                    btnDel_Item.Enabled = false;
                    btnSave.Enabled = false;
                    btnNew.Enabled = true;
                }
             
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : btnListItem_Click", this.Name); }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintPR a = new PrintPR(txtCodeNo.Text, txtCodeNo.Text, "ShippingAVG");
            a.ShowDialog();

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
            //                    txtTempJobCard.Text = dbClss.TSt(p.FirstOrDefault().TempJobCard);
            //                else if (dbClss.TSt(p.FirstOrDefault().Status) != "Completed")
            //                {
            //                    txtTempJobCard.Text = "";
            //                    txtJobCard.Text = "";
            //                    MessageBox.Show("ใบงานการผลิตดังกล่าวถูกปิดไปแล้ว กรุณาระบุใบงานการผลิตใหม่");
            //                }

            //            }
            //            else
            //            {
            //                txtJobCard.Text = "";
            //                txtTempJobCard.Text = "";
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
            //}
        }

        private void btnAdd_Item_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                ListPart sc = new ListPart(txtCodeNo, "WIP-RM-Other", "ShipAVG");
                this.Cursor = Cursors.Default;
                sc.ShowDialog();



                Load_CodeNO(txtCodeNo.Text);
                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();

            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void Load_CodeNO(string CodeNo)
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    if (ddlMonth.Text != "" && ddlYear.Text != "" && txtCodeNo.Text != "")
                        DataLoad();
                    else
                    {

                        var g = (from ix in db.tb_Items select ix)
                            .Where(a => a.CodeNo == txtCodeNo.Text && a.TypePart != "FG")
                            .ToList();
                        if (g.Count() > 0)
                        {
                            txtItemNo.Text = StockControl.dbClss.TSt(g.FirstOrDefault().ItemNo);
                            txtItemDescription.Text = StockControl.dbClss.TSt(g.FirstOrDefault().ItemDescription);
                            
                        }
                        else
                        {
                            txtItemDescription.Text = "";
                            txtItemNo.Text = "";
                            txtCodeNo.Text = "";
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnCal_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // foreach(var g in dgvData.Rows)
                dgvData.EndEdit();
                decimal Cost = 0;
                decimal.TryParse(txtCost.Text, out Cost);
                decimal Qty = 0;
                decimal.TryParse(txtQty.Text, out Qty);
                
                decimal UnitCost = 0;
                
                int c = 0;
                decimal JobC = 0;
                foreach (GridViewRowInfo rowInfo in dgvData.Rows)
                {
                    if(rowInfo.IsVisible)
                        c += 1;
                }

                if (Qty > 0)
                {
                    if(c>0)
                        JobC = Math.Round((c / Qty),2);

                    UnitCost = Math.Round((Cost / Qty), 2);
                }

                foreach (GridViewRowInfo rowInfo in dgvData.Rows)
                {
                    rowInfo.Cells["Qty"].Value = JobC;
                    rowInfo.Cells["UnitCost"].Value = UnitCost;
                }


               
                
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : btnCal_Click", this.Name); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void txtQty_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter)
                {
                    decimal temp = 0;
                    decimal.TryParse(txtQty.Text, out temp);
                    temp = decimal.Round(temp, 2);
                    txtQty.Text = (temp).ToString("N2");
                }
            }
            catch { }
        }

        private void txtCost_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter)
                {
                    decimal temp = 0;
                    decimal.TryParse(txtCost.Text, out temp);
                    temp = decimal.Round(temp, 2);
                    txtCost.Text = (temp).ToString("N2");
                }
            }
            catch { }
        }

        private void txtCost_KeyPress(object sender, KeyPressEventArgs e)
        {
            StockControl.dbClss.CheckDigitDecimal(e);
        }

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            StockControl.dbClss.CheckDigitDecimal(e);
        }

        private void ddlYear_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            DataLoad();
        }

        private void ddlMonth_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            DataLoad();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblStatus.Text != "Completed")
                {
                    lblStatus.Text = "Delete";
                    Ac = "Del";
                    if (MessageBox.Show("ต้องการลบรายการ หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        using (DataClasses1DataContext db = new DataClasses1DataContext())
                        {
                            var g = (from ix in db.tb_Shipping_JobCardAvgHs select ix)
                            .Where(a => a.YYYY == ddlYear.Text.Trim()
                            && a.MM == ddlMonth.Text.Trim()
                            && a.CodeNo == txtCodeNo.Text.Trim()
                            && a.Status != "Completed"

                                ).ToList();
                            if (g.Count > 0)  //มีรายการในระบบ
                            {


                                //detail
                                var d = (from ix in db.tb_Shipping_JobCardAvgs select ix)
                                    .Where(a => a.YYYY == ddlYear.Text.Trim()
                                    && a.MM == ddlMonth.Text.Trim()
                                    && a.CodeNo == txtCodeNo.Text.Trim()
                                    && a.Status != "Cancel").ToList();
                                if (d.Count() > 0)
                                {

                                    foreach (var vvd in d)
                                    {
                                        dbClss.AddHistory(this.Name, "ลบ ShipAVG", "Delete ShipAVG [ CodeNo : " + txtCodeNo.Text.Trim() +" Job Card : "+ vvd.JobCard + " Year : " + ddlYear.Text + " Month : " + ddlMonth.Text + "]", txtCodeNo.Text + ddlYear.Text + ddlMonth.Text);
                                    }
                                    
                                    //delete
                                    db.tb_Shipping_JobCardAvgs.DeleteAllOnSubmit(d);
                                    db.SubmitChanges();
                                }

                                
                                //delete 
                                db.tb_Shipping_JobCardAvgHs.DeleteAllOnSubmit(g);
                                db.SubmitChanges();

                                dbClss.AddHistory(this.Name, "ลบ ShipAVG", "Delete ShipAVG [ CodeNo : " + txtCodeNo.Text.Trim() +" Year : " + ddlYear.Text +" Month : "+ ddlMonth.Text + "]", txtCodeNo.Text+ddlYear.Text+ddlMonth.Text);
                                
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

        private void txtQty_Leave(object sender, EventArgs e)
        {
            try
            {
                decimal Qty = dbClss.TDe(txtQty.Text);
                txtQty.Text = Qty.ToString("###,###,###,##0.00");
            }
            catch { }
        }

        private void txtCost_Leave(object sender, EventArgs e)
        {
            try
            {
                decimal Cost = dbClss.TDe(txtCost.Text);
                txtCost.Text = Cost.ToString("###,###,###,##0.00");
            }
            catch { }
        }

        private void btnSumqty_Click(object sender, EventArgs e)
        {
            if (txtCodeNo.Text != "" && ddlMonth.Text != "" && ddlYear.Text != "")
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    txtQty.Text = dbClss.TDe(db.Cal_Ship_Qty(txtCodeNo.Text, ddlYear.Text, ddlMonth.Text)).ToString("N2");
                    txtCost.Text = dbClss.TDe(db.Cal_Ship_Cost(txtCodeNo.Text, ddlYear.Text, ddlMonth.Text)).ToString("N2");

                }
            }
            else
            {
                MessageBox.Show("กรุณาระบุ ปี เดือนและรหัสทูล");
            }
        }
    }
}
