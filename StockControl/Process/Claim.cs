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
    public partial class Claim : Telerik.WinControls.UI.RadRibbonForm
    {
        public Claim()
        {
            InitializeComponent();
        }
        public Claim(string Claim)
        {
            InitializeComponent();
            ClaimNo_t = Claim;
        }
        string ClaimNo_t = "";
     
        string Ac = "";
        DataTable dt_h = new DataTable();
        DataTable dt_d = new DataTable();

        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            HistoryView hw = new HistoryView(this.Name, txtClaimNo.Text);
            this.Cursor = Cursors.Default;
            hw.ShowDialog();
        }

        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }
        private void GETDTRow()
        {
            dt_h.Columns.Add(new DataColumn("id", typeof(int)));
            dt_h.Columns.Add(new DataColumn("ClaimNo", typeof(string)));
            dt_h.Columns.Add(new DataColumn("ClaimBy", typeof(string)));
            dt_h.Columns.Add(new DataColumn("ClaimDate", typeof(DateTime)));
            dt_h.Columns.Add(new DataColumn("CreateDate", typeof(DateTime)));
            dt_h.Columns.Add(new DataColumn("CreateBy", typeof(string)));
            dt_h.Columns.Add(new DataColumn("CodeNo", typeof(string)));
            dt_h.Columns.Add(new DataColumn("Qty", typeof(decimal)));
            dt_h.Columns.Add(new DataColumn("Remark", typeof(string)));
            dt_h.Columns.Add(new DataColumn("Status", typeof(string)));
            dt_h.Columns.Add(new DataColumn("UnitCost", typeof(decimal)));
            dt_h.Columns.Add(new DataColumn("Amount", typeof(decimal)));
            dt_h.Columns.Add(new DataColumn("Unit", typeof(string)));
            dt_h.Columns.Add(new DataColumn("RefReceiveID", typeof(int)));
            dt_h.Columns.Add(new DataColumn("StatusClaim", typeof(bool)));
           
          
   

        //dt_d.Columns.Add(new DataColumn("ShippingNo", typeof(string)));
        //dt_d.Columns.Add(new DataColumn("ShipType", typeof(string)));
        //dt_d.Columns.Add(new DataColumn("Seq", typeof(int)));
        //dt_d.Columns.Add(new DataColumn("CodeNo", typeof(string)));
        //dt_d.Columns.Add(new DataColumn("ItemNo", typeof(string)));
        //dt_d.Columns.Add(new DataColumn("ItemDescription", typeof(string)));
        //dt_d.Columns.Add(new DataColumn("QTY", typeof(decimal)));
        //dt_d.Columns.Add(new DataColumn("Remark", typeof(string)));
        //dt_d.Columns.Add(new DataColumn("LineName", typeof(string)));
        //dt_d.Columns.Add(new DataColumn("MachineName", typeof(string)));
        //dt_d.Columns.Add(new DataColumn("UnitShip", typeof(string)));
        //dt_d.Columns.Add(new DataColumn("PCSUnit", typeof(decimal)));
        //dt_d.Columns.Add(new DataColumn("SerialNo", typeof(string)));
        //dt_d.Columns.Add(new DataColumn("LotNo", typeof(string)));
        //dt_d.Columns.Add(new DataColumn("Calbit", typeof(bool)));
        //dt_d.Columns.Add(new DataColumn("ClearFlag", typeof(bool)));
        //dt_d.Columns.Add(new DataColumn("ClearDate", typeof(bool)));
        //dt_d.Columns.Add(new DataColumn("Status", typeof(string)));
        //dt_d.Columns.Add(new DataColumn("BarCode", typeof(Image)));


    }

        private void Unit_Load(object sender, EventArgs e)
        {
            //dgvData.ReadOnly = true;
            dgvData.AutoGenerateColumns = false;
            GETDTRow();

            DefaultItem();

            btnNew_Click(null, null);

            if (!ClaimNo_t.Equals(""))
            {
                btnNew.Enabled = true;
                txtClaimNo.Text = ClaimNo_t;
                DataLoad();
                Ac = "View";

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
            dt_d.Rows.Clear();
            dgvData.Rows.Clear();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {

                    try
                    {
                        int dgvNo = 0;
                        var r = (from h in db.tb_Claims
                                 //join d in db.tb_Receives on h.RefReceiveID equals d.ID
                                 join i in db.tb_Items on h.CodeNo equals i.CodeNo
                                 where //h.Status == "Waiting" //&& d.verticalID == VerticalID
                                    // h.Status != "Cancel" &&
                                     h.ClaimNo.Contains(txtClaimNo.Text)

                                 //&& (((h.CreateDate >= inclusiveStart
                                 // && h.CreateDate < exclusiveEnd)
                                 // && cbDate.Checked == true)
                                 //  || (cbDate.Checked == false)
                                 // )

                                 select new
                                 {
                                     CodeNo = h.CodeNo,
                                     ClaimNo = h.ClaimNo,
                                     ItemNo = i.ItemNo,
                                     ItemDescription = i.ItemDescription,
                                     QTY = h.Qty,
                                     Unit = h.Unit,
                                     PCSUnit = i.PCSUnit,
                                     UnitCost = h.UnitCost,
                                     CreateBy = h.CreateBy,
                                     CreateDate = h.CreateDate,
                                     ClaimBy = h.ClaimBy,
                                     ClaimDate = h.ClaimDate,
                                     Remark = h.Remark,
                                     id = h.id,
                                     RefRCID = h.RefReceiveID,
                                     Amount = h.Amount,
                                     StatusClaim = h.StatusClaim,
                                     Status = h.Status

                                 }
                        ).ToList();
                        if (r.Count > 0)
                        {
                            txtClaimNo.Text = r.FirstOrDefault().ClaimNo;
                            txtClaimName.Text = r.FirstOrDefault().ClaimBy;
                            dtClaimDate.Value = Convert.ToDateTime(r.FirstOrDefault().ClaimDate);

                            foreach (var vv in r)
                            {
                                dgvNo = dgvData.Rows.Count() + 1;
                                Add_Item(dgvNo, vv.CodeNo, vv.ItemNo, vv.ItemDescription, vv.QTY
                                    , vv.Unit, Convert.ToDecimal(vv.UnitCost), Convert.ToDecimal(vv.Amount)
                                    , vv.Remark, vv.id);
                            }





                            StockControl.dbClss.TSt(r.FirstOrDefault().Status);
                            if (StockControl.dbClss.TSt(r.FirstOrDefault().Status).Equals("Cancel"))
                            {
                                btnSave.Enabled = false;
                                btnDelete.Enabled = false;
                                btnView.Enabled = false;
                                btnEdit.Enabled = false;
                                lblStatus.Text = "Cancel";
                                dgvData.ReadOnly = true;
                                btnNew.Enabled = true;
                                btnDel_Item.Enabled = false;
                            }
                            //else if
                            //    (StockControl.dbClss.TSt(r.FirstOrDefault().Status).Equals("Process"))
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
                                (StockControl.dbClss.TSt(r.FirstOrDefault().Status).Equals("Completed"))

                            {
                                btnSave.Enabled = false;
                                btnDelete.Enabled = false;
                                btnView.Enabled = false;
                                btnEdit.Enabled = false;
                                lblStatus.Text = "Completed";
                                dgvData.ReadOnly = true;
                                btnNew.Enabled = true;
                                btnDel_Item.Enabled = false;
                            }
                            else
                            {
                                btnNew.Enabled = true;
                                btnSave.Enabled = true;
                                btnDelete.Enabled = true;
                                btnView.Enabled = true;
                                btnEdit.Enabled = true;
                                lblStatus.Text = StockControl.dbClss.TSt(r.FirstOrDefault().Status);
                                dgvData.ReadOnly = false;
                                btnDel_Item.Enabled = false;
                            }
                            dt_h = StockControl.dbClss.LINQToDataTable(r);

                        }
                        
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }


        }
        private void Add_Item(int Row, string CodeNo, string ItemNo
          , string ItemDescription, decimal QTY, string Unit
          ,  decimal UnitCost, decimal Amount
          , string Remark,int id)
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
                ee.Cells["QTY"].Value = QTY;
                ee.Cells["Unit"].Value = Unit;
                //ee.Cells["PCSUnit"].Value = PCSUnit;
                ee.Cells["UnitCost"].Value = UnitCost;
                ee.Cells["Amount"].Value = Amount;
                ee.Cells["Remark"].Value = Remark;
                ee.Cells["id"].Value = id;
               

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
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError("CreatePR", ex.Message + " : Add_Item", this.Name); }

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
            txtClaimNo.Text = "";

            txtCodeNo.Text = "";
            dtClaimDate.Value = DateTime.Now;
            txtCreateBy.Text = ClassLib.Classlib.User;
            txtClaimName.Text = ClassLib.Classlib.User;
            txtCreateDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            lblStatus.Text = "-";
            dgvData.Rows.Clear();
            dgvData.DataSource = null;
            dt_d.Rows.Clear();
            dt_h.Rows.Clear();
            txtTotal.Text = "0.00";
        }
        private void Enable_Status(bool ss, string Condition)
        {
            if (Condition.Equals("-") || Condition.Equals("New"))
            {
                txtCodeNo.Enabled = ss;
                //txtClaimName.Enabled = ss;
                //dtClaimDate.Enabled = ss;
                dgvData.ReadOnly = false;
                btnAdd_Row.Enabled = ss;
                btn_AddItem.Enabled = ss;
                btnDel_Item.Enabled = ss;
            }
            else if (Condition.Equals("View"))
            {
                txtCodeNo.Enabled = ss;
                txtClaimName.Enabled = ss;
                dtClaimDate.Enabled = ss;
                dgvData.ReadOnly = false;
                btnDel_Item.Enabled = ss;
                btnAdd_Row.Enabled = ss;
                btn_AddItem.Enabled = ss;
            }

            else if (Condition.Equals("Edit"))
            {
                txtCodeNo.Enabled = ss;
                //txtClaimName.Enabled = ss;
                //dtClaimDate.Enabled = ss;
                dgvData.ReadOnly = false;
                btnDel_Item.Enabled = ss;
                btnAdd_Row.Enabled = ss;
                btn_AddItem.Enabled = ss;
            }
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            btnDel_Item.Enabled = true;
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            btnEdit.Enabled = true;
            btnView.Enabled = true;
            btnSendClaim.Enabled = false;
            btnDelete.Enabled = true;

            ClearData();
            Enable_Status(true, "New");
            lblStatus.Text = "New";
            Ac = "New";

            // getมาไว้ก่อน แต่ยังไมได้ save
            txtClaimNo.Text = StockControl.dbClss.GetNo(16, 0);
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


                if (txtClaimName.Text.Equals(""))
                    err += "- “ผู้ทำรายการ:” เป็นค่าว่าง \n";
                //if (txtVendorNo.Text.Equals(""))
                //    err += "- “รหัสผู้ขาย:” เป็นค่าว่าง \n";
                if (dtClaimDate.Text.Equals(""))
                    err += "- “วันที่ทำรายการ:” เป็นค่าว่าง \n";

                if (dgvData.Rows.Count <= 0)
                    err += "- “รายการ:” เป็นค่าว่าง \n";
                int c = 0;
                foreach (GridViewRowInfo rowInfo in dgvData.Rows)
                {
                    if (rowInfo.IsVisible)
                    {
                       if(StockControl.dbClss.TInt(rowInfo.Cells["QTY"].Value)<=0)
                            err += "- “จำนวน:” น้อยกว่า 0 \n";

                        if (dbClss.TSt(rowInfo.Cells["Remark"].Value).Trim() == "")
                            err += "- “หมายเหตุ:” เป็นค่าว่าง \n";


                        c += 1;

                        //if (StockControl.dbClss.TInt(rowInfo.Cells["QTY"].Value) <= (0))
                        //{
                        //    err += "- “จำนวนเบิก:” ต้องมากกว่า 0 \n";
                        //}
                        //else if (StockControl.dbClss.TInt(rowInfo.Cells["QTY"].Value) != (0))
                        //{
                        //    c += 1;
                        //    //if (StockControl.dbClss.TSt(rowInfo.Cells["PRNo"].Value).Equals(""))
                        //    //    err += "- “เลขที่ PR:” เป็นค่าว่าง \n";
                        //    //if (StockControl.dbClss.TSt(rowInfo.Cells["TempNo"].Value).Equals(""))
                        //    //    err += "- “เลขที่อ้างอิงเอกสาร PRNo:” เป็นค่าว่าง \n";
                        //    if (StockControl.dbClss.TSt(rowInfo.Cells["CodeNo"].Value).Equals(""))
                        //        err += "- “รหัสทูล:” เป็นค่าว่าง \n";
                        //    if (StockControl.dbClss.TDe(rowInfo.Cells["QTY"].Value) > StockControl.dbClss.TDe(rowInfo.Cells["RemainQty"].Value))
                        //        err += "- “จำนวนเบิก:” มากกว่าจำนวนคงเหลือ \n";
                        //    if (StockControl.dbClss.TDe(rowInfo.Cells["UnitShip"].Value).Equals(""))
                        //        err += "- “หน่วย:” เป็นค่าว่าง \n";

                        //}
                    }
                }

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
            //using (DataClasses1DataContext db = new DataClasses1DataContext())
            //{
            //    var g = (from ix in db.tb_ShippingHs
            //             where ix.ShippingNo.Trim() == txtSHNo.Text.Trim() && ix.Status != "Cancel"
            //             //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
            //             select ix).ToList();
            //    if (g.Count > 0)  //มีรายการในระบบ
            //    {
            //        foreach (DataRow row in dt_h.Rows)
            //        {
            //            var gg = (from ix in db.tb_ShippingHs
            //                      where ix.ShippingNo.Trim() == txtSHNo.Text.Trim() && ix.Status != "Cancel"
            //                      //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
            //                      select ix).First();

            //            gg.UpdateBy = ClassLib.Classlib.User;
            //            gg.UpdateDate = DateTime.Now;
            //            dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไขโดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", txtSHNo.Text);
            //            if (StockControl.dbClss.TSt(gg.BarCode).Equals(""))
            //                gg.BarCode = StockControl.dbClss.SaveQRCode2D(txtSHNo.Text.Trim());

            //            if (!txtSHName.Text.Trim().Equals(row["ShipName"].ToString()))
            //            {
            //                gg.ShipName = txtSHName.Text;                           
            //                dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไขผู้เบิกสินค้า [" + txtSHName.Text.Trim() + " เดิม :" + row["ShipName"].ToString() + "]", txtSHNo.Text);
            //            }
            //            if (!txtJobCard.Text.Trim().Equals(row["JobCard"].ToString()))
            //            {
            //                gg.JobCard = txtJobCard.Text;
            //                dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไข JobCard [" + txtJobCard.Text.Trim() + " เดิม :" + row["JobCard"].ToString() + "]", txtSHNo.Text);
            //            }
            //            if (!txtTempJobCard.Text.Trim().Equals(row["TempJobCard"].ToString()))
            //            {
            //                gg.TempJobCard = txtTempJobCard.Text;
            //                dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไข TempJobCard [" + txtTempJobCard.Text.Trim() + " เดิม :" + row["TempJobCard"].ToString() + "]", txtSHNo.Text);
            //            }

            //            if (!txtRemark.Text.Trim().Equals(row["Remark"].ToString()))
            //            {
            //                gg.Remark = txtRemark.Text.Trim();
            //                dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไขหมายเหตุ [" + txtRemark.Text.Trim() + " เดิม :" + row["Remark"].ToString() + "]", txtSHNo.Text);
            //            }

            //            if (!dtRequire.Text.Trim().Equals(""))
            //            {
            //                string date1 = "";
            //                date1 = dtRequire.Value.ToString("yyyyMMdd", new CultureInfo("en-US"));
            //                string date2 = "";
            //                DateTime temp = DateTime.Now;
            //                if (!StockControl.dbClss.TSt(row["ShipDate"].ToString()).Equals(""))
            //                {

            //                    temp = Convert.ToDateTime(row["ShipDate"]);
            //                    date2 = temp.ToString("yyyyMMdd", new CultureInfo("en-US"));

            //                }
            //                if (!date1.Equals(date2))
            //                {
            //                    DateTime? RequireDate = DateTime.Now;
            //                    if (!dtRequire.Text.Equals(""))
            //                        RequireDate = dtRequire.Value;
            //                    gg.ShipDate = RequireDate;
            //                    dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไขวันที่เบิกสินค้า [" + dtRequire.Text.Trim() + " เดิม :" + temp.ToString() + "]", txtSHNo.Text);
            //                }
            //            }
            //            db.SubmitChanges();
            //        }
            //    }
            //    else //สร้างใหม่
            //    {
            //        byte[] barcode = null;
            //            //barcode = StockControl.dbClss.SaveQRCode2D(txtSHNo.Text.Trim());
            //        DateTime? UpdateDate = null;

            //        DateTime? RequireDate = DateTime.Now;
            //        if (!dtRequire.Text.Equals(""))
            //            RequireDate = dtRequire.Value;

            //        tb_ShippingH gg = new tb_ShippingH();
            //        gg.ShippingNo = txtSHNo.Text;
            //        gg.ShipDate = RequireDate;
            //        gg.UpdateBy = null;
            //        gg.UpdateDate = UpdateDate;
            //        gg.CreateBy = ClassLib.Classlib.User;
            //        gg.CreateDate = DateTime.Now;
            //        gg.ShipName = txtSHName.Text;
            //        gg.Remark = txtRemark.Text;
            //        gg.JobCard = txtJobCard.Text.Trim();
            //        gg.TempJobCard = txtTempJobCard.Text;
            //        gg.BarCode = barcode;
            //        gg.Status = "Completed";
            //        db.tb_ShippingHs.InsertOnSubmit(gg);
            //        db.SubmitChanges();

            //        dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "สร้าง การเบิกสินค้า [" + txtSHNo.Text.Trim() + "]", txtSHNo.Text);
            //    }
            //}
        }
        private void SaveDetail()
        {
            dgvData.EndEdit();

            DateTime? RequireDate = DateTime.Now;
            if (!dtClaimDate.Text.Equals(""))
                RequireDate = dtClaimDate.Value;
            int Seq = 0;
            DateTime? UpdateDate = null;
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                decimal UnitCost = 0;
                foreach (var g in dgvData.Rows)
                {
                    string SS = "";
                    if (g.IsVisible.Equals(true))
                    {
                        if (dbClss.TInt(g.Cells["id"].Value) == 0) // เอาเฉพาะรายการที่ไม่เป็น 0 
                        {

                            tb_Claim u = new tb_Claim();
                            u.ClaimNo = txtClaimNo.Text;
                            u.CodeNo = StockControl.dbClss.TSt(g.Cells["CodeNo"].Value);
                            u.Remark = StockControl.dbClss.TSt(g.Cells["Remark"].Value);
                            u.Qty = StockControl.dbClss.TDe(g.Cells["QTY"].Value);
                            u.Unit = StockControl.dbClss.TSt(g.Cells["Unit"].Value);
                            u.UnitCost = StockControl.dbClss.TDe(g.Cells["UnitCost"].Value);
                            u.Amount = StockControl.dbClss.TDe(g.Cells["Amount"].Value);
                            u.CreateBy = ClassLib.Classlib.User;
                            u.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                            u.ModifyBy = ClassLib.Classlib.User;
                            u.ModifyDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));

                            u.StatusClaim = false;
                            u.ClaimBy = txtClaimName.Text.Trim();
                            u.ClaimDate = Convert.ToDateTime(dtClaimDate.Value, new CultureInfo("en-US"));
                            u.Status = "Waiting";
                            db.tb_Claims.InsertOnSubmit(u);
                            db.SubmitChanges();

                            //if (StockControl.dbClss.TInt(g.Cells["id"].Value) >0)  //New ใหม่
                            //{

                            //db.sp_034_tb_Shipping_ADD_Claim(txtClaimNo.Text, StockControl.dbClss.TInt(g.Cells["id"].Value), StockControl.dbClss.TSt(g.Cells["CodeNo"].Value)
                            //    , StockControl.dbClss.TDe(g.Cells["QTY"].Value)
                            //    , StockControl.dbClss.TSt(g.Cells["Remark"].Value)
                            //    , "", "", "", "", "Completed", ClassLib.Classlib.User);

                            //C += 1;
                            dbClss.AddHistory(this.Name, "เพิ่มรายการClaim", "เพิ่มรายการClaim [" + StockControl.dbClss.TSt(g.Cells["CodeNo"].Value) + " จำนวน :" + StockControl.dbClss.TDe(g.Cells["QTY"].Value).ToString() + " " + StockControl.dbClss.TSt(g.Cells["Unit"].Value) + "]", txtClaimNo.Text);

                            //}


                        }
                        else
                        {

                            foreach (DataRow row in dt_h.Rows)
                            {
                                if (StockControl.dbClss.TInt(g.Cells["id"].Value) == StockControl.dbClss.TInt(row["id"]))
                                {
                                    var r = (from ix in db.tb_Claims
                                         where ix.ClaimNo.Trim() == txtClaimNo.Text.Trim()
                                         && ix.Status != "Cancel" && ix.Status != "Completed"
                                         && ix.StatusClaim == false
                                          && ix.id == dbClss.TInt(g.Cells["id"].Value)
                                         select ix).ToList();
                                    if (r.Count > 0)  //มีรายการในระบบ
                                    {
                                        foreach (var rr in r)
                                        {
                                            if (!StockControl.dbClss.TSt(g.Cells["CodeNo"].Value).Equals(row["CodeNo"].ToString()))
                                            {
                                                rr.CodeNo = StockControl.dbClss.TSt(g.Cells["CodeNo"].Value);
                                                dbClss.AddHistory(this.Name, "แก้ไขรายการClaim", "แก้ไขรหัสทูล [" + rr.CodeNo + "]", txtClaimNo.Text);
                                            }
                                            //rr.CodeNo = StockControl.dbClss.TSt(g.Cells["CodeNo"].Value);
                                            if (!StockControl.dbClss.TSt(g.Cells["Remark"].Value).Equals(row["Remark"].ToString()))
                                            {
                                                rr.Remark = StockControl.dbClss.TSt(g.Cells["Remark"].Value);
                                                dbClss.AddHistory(this.Name, "แก้ไขรายการClaim", "แก้ไขหมายเหตุ [" + rr.Remark + "]", txtClaimNo.Text);
                                            }
                                            //rr.Remark = StockControl.dbClss.TSt(g.Cells["Remark"].Value);
                                            if (!StockControl.dbClss.TDe(g.Cells["QTY"].Value).Equals(row["Qty"].ToString()))
                                            {
                                                rr.Qty = StockControl.dbClss.TDe(g.Cells["QTY"].Value);
                                                dbClss.AddHistory(this.Name, "แก้ไขรายการClaim", "แก้ไขจำนวน[" + rr.Qty.ToString() + "]", txtClaimNo.Text);
                                            }
                                            //rr.Qty = StockControl.dbClss.TDe(g.Cells["QTY"].Value);
                                            rr.Unit = StockControl.dbClss.TSt(g.Cells["Unit"].Value);
                                            if (!StockControl.dbClss.TDe(g.Cells["UnitCost"].Value).Equals(row["UnitCost"].ToString()))
                                            {
                                                rr.UnitCost = StockControl.dbClss.TDe(g.Cells["UnitCost"].Value);
                                                dbClss.AddHistory(this.Name, "แก้ไขรายการClaim", "แก้ไขราคา/หน่วย[" + rr.UnitCost.ToString() + "]", txtClaimNo.Text);
                                            }
                                            rr.UnitCost = StockControl.dbClss.TDe(g.Cells["UnitCost"].Value);
                                            rr.Amount = StockControl.dbClss.TDe(g.Cells["Amount"].Value);
                                            rr.ModifyBy = ClassLib.Classlib.User;
                                            rr.ModifyDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));

                                            rr.StatusClaim = false;
                                            rr.ClaimBy = txtClaimName.Text.Trim();
                                            rr.ClaimDate = Convert.ToDateTime(dtClaimDate.Value, new CultureInfo("en-US"));
                                            rr.Status = "Waiting";
                                            db.SubmitChanges();

                                            dbClss.AddHistory(this.Name, "แก้ไขรายการClaim", "แก้ไขรายการ Claim [" + StockControl.dbClss.TSt(g.Cells["CodeNo"].Value) + " จำนวน :" + StockControl.dbClss.TDe(g.Cells["QTY"].Value).ToString() + " " + StockControl.dbClss.TSt(g.Cells["Unit"].Value) + " ID : " + dbClss.TInt(g.Cells["id"].Value).ToString() + "]", txtClaimNo.Text);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (dbClss.TInt(dbClss.TInt(g.Cells["id"].Value)) > 0)
                        {
                            var r = (from ix in db.tb_Claims
                                     where ix.ClaimNo.Trim() == txtClaimNo.Text.Trim()
                                     && ix.Status != "Cancel" && ix.Status != "Completed" && ix.StatusClaim == false
                                      && ix.id == dbClss.TInt(g.Cells["id"].Value)
                                     select ix).ToList();
                            if (r.Count > 0)  //มีรายการในระบบ

                            {
                                foreach (var rr in r)
                                {
                                    rr.ModifyBy = ClassLib.Classlib.User;
                                    rr.ModifyDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                    rr.Status = "Cancel";
                                    rr.StatusClaim = false;
                                    db.SubmitChanges();

                                    dbClss.AddHistory(this.Name, "ลบรายการClaim", "ลบรายการClaim [" + StockControl.dbClss.TSt(g.Cells["CodeNo"].Value) + " จำนวน :" + StockControl.dbClss.TDe(g.Cells["QTY"].Value).ToString() + " " + StockControl.dbClss.TSt(g.Cells["Unit"].Value) + "]", txtClaimNo.Text);

                                }
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

                    if (Ac.Equals("New"))
                        txtClaimNo.Text = StockControl.dbClss.GetNo(16, 2);

                    if (!txtClaimNo.Text.Equals(""))
                    {
                        SaveDetail();
                        
                      
                        MessageBox.Show("บันทึกสำเร็จ!");
                        btnRefresh_Click(null, null);
                    }
                    else
                    {
                        MessageBox.Show("ไม่สามารถโหลดเลขที่เคลมได้ ติดต่อแผนก IT");
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
                    DateTime? AppDate = DateTime.Now;
                    int Seq = 0;



                    string CNNo = CNNo = StockControl.dbClss.GetNo(6, 2);
                    var g = (from ix in db.tb_Shippings
                                 //join i in db.tb_Items on ix.CodeNo equals i.CodeNo
                             where ix.ShippingNo.Trim() == txtClaimNo.Text.Trim() && ix.Status != "Cancel"

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
                            gg.CreateDate = DateTime.Now;
                            gg.DocNo = CNNo;
                            gg.RefNo = txtClaimNo.Text;
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
                    DateTime? AppDate = DateTime.Now;
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
                             where ix.ShippingNo.Trim() == txtClaimNo.Text.Trim() && ix.Status != "Cancel"

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
                                    gg.CreateDate = DateTime.Now;
                                    gg.DocNo = txtClaimNo.Text;
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
                                    gg.Flag_ClearTemp = 0; //0 คือ invoice,1 คือ Temp , 2 คือ clear temp แล้ว
                                    gg.Type_in_out = Type_in_out;
                                    gg.AmountCost = Amount;
                                    gg.UnitCost = UnitCost;
                                    gg.RemainQty = sum_Qty;
                                    gg.RemainUnitCost = 0;
                                    gg.RemainAmount = RemainAmount;
                                    gg.Avg = Avg;
                                    gg.TLCost = 0;
                                    gg.TLQty = 0;
                                    gg.ShipQty = 0;
                                    gg.Location = vv.Location;

                                    db.tb_Stocks.InsertOnSubmit(gg);
                                    db.SubmitChanges();

                                    dbClss.AddHistory(this.Name, "เบิกสินค้า", " เบิกสินค้าเลขที่ : " + txtClaimNo.Text + " เบิก : " + Category + " CodeNo : " + vv.CodeNo + " จำนวน : " + (-QTY).ToString() + " โดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", txtClaimNo.Text);

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
                                    gg.CreateDate = DateTime.Now;
                                    gg.DocNo = txtClaimNo.Text;
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
                                    gg.TLCost = 0;
                                    gg.TLQty = 0;
                                    gg.ShipQty = 0;
                                    gg.Location = vv.Location;

                                    db.tb_Stocks.InsertOnSubmit(gg);
                                    db.SubmitChanges();
                                    dbClss.AddHistory(this.Name, "เบิกสินค้า", " เบิกสินค้าเลขที่ : " + txtClaimNo.Text + " เบิก : " + Category + " CodeNo : " + vv.CodeNo + " จำนวน : " + (-QTY).ToString() + " โดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", txtClaimNo.Text);


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
            try
            {
                dgvData.EndEdit();
                if (e.RowIndex >= -1)
                {

                    //if (dgvData.Columns["QTY"].Index == e.ColumnIndex)
                    //{
                    //    decimal QTY = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["QTY"].Value), out QTY);
                    //    decimal RemainQty = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["RemainQty"].Value), out RemainQty);
                    //    if (QTY > RemainQty)
                    //    {
                    //        MessageBox.Show("ไม่สามารถรับเกินจำนวนคงเหลือได้");
                    //        e.Row.Cells["QTY"].Value = 0;
                    //    }
                    //}

                    if (dgvData.Columns["QTY"].Index == e.ColumnIndex
                        || dgvData.Columns["UnitCost"].Index == e.ColumnIndex
                        )
                    {
                        decimal QTY = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["QTY"].Value), out QTY);
                        decimal CostPerUnit = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["UnitCost"].Value), out CostPerUnit);
                        e.Row.Cells["Amount"].Value = QTY * CostPerUnit;
                        Cal_Amount();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
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
            

        }

        private void txtCodeNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {

                    Insert_data(txtCodeNo.Text);
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
                if (Convert.ToString(g.Cells["CodeNo"].Value).Equals(CodeNo))
                {
                    re = true;
                    MessageBox.Show("รหัสพาร์ทซ้ำ");
                    break;
                }
            }

            return re;
        }
        //private void Insert_data_New()
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
        //                         LineName = "",
        //                         Remark = "",
        //                         id = 0

        //                     }
        //            ).ToList();
        //            if (r.Count > 0)
        //            {
        //                dgvNo = dgvData.Rows.Count() + 1;

        //                foreach (var vv in r)
        //                {
        //                    //dgvData.Rows.Add(dgvNo.ToString(), vv.CodeNo, vv.ItemNo, vv.ItemDescription
        //                    //            , vv.RemainQty, vv.QTY, vv.UnitShip, vv.PCSUnit, vv.StandardCodt, vv.Amount,
        //                    //            vv.LotNo, vv.SerialNo, vv.MachineName, vv.LineName, vv.Remark, vv.id
        //                    //            );

        //                    Add_Item(dgvNo, vv.CodeNo, vv.ItemNo, vv.ItemDescription
        //                                , vv.RemainQty, vv.QTY, vv.UnitShip, dbClss.TDe(vv.PCSUnit), dbClss.TDe(vv.StandardCodt)
        //                                , vv.Amount, vv.LotNo, vv.SerialNo, vv.MachineName, vv.LineName, vv.Remark, vv.id);

        //                }
        //            }
        //            Cal_Amount();

        //        }
        //    }
        //}

     
        
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

                    //int id = 0;
                    //int.TryParse(StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["id"].Value), out id);
                    //if (id <= 0)
                    //    dgvData.Rows.Remove(dgvData.CurrentRow);

                    //else
                    //{
                        string CodeNo = "";
                        CodeNo = StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["CodeNo"].Value);
                        if (MessageBox.Show("ต้องการลบรายการ ( " + CodeNo + " ) ออกจากรายการ หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            dgvData.CurrentRow.IsVisible = false;
                        }
                    //}
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
                Claim_List sc = new Claim_List(txtClaimNo, txtCodeNo);
                this.Cursor = Cursors.Default;
                sc.ShowDialog();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();
                //LoadData

                string CodeNo = txtCodeNo.Text;
                string SHNo = txtClaimNo.Text;
                if (!txtClaimNo.Text.Equals(""))
                {
                    txtCodeNo.Text = "";

                    DataLoad();
                    Ac = "View";
                    btnDel_Item.Enabled = false;
                    btnSave.Enabled = false;
                    btnNew.Enabled = true;
                }
                //else
                //{
                //    btnDel_Item.Enabled = true;
                //    btnNew_Click(null, null);
                //    txtCodeNo.Text = CodeNo;

                //    Insert_data_New();
                //    txtCodeNo.Text = "";

                //}
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError("Shipping", ex.Message + " : radButtonElement1_Click", this.Name); }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                PrintPR a = new PrintPR(txtClaimNo.Text, txtClaimNo.Text, "Claim");
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
        private void Add()
        {
            try
            {

                this.Cursor = Cursors.WaitCursor;
                ListPart sc = new ListPart(txtCodeNo, "WIP-RM-Other", "Claim");
                this.Cursor = Cursors.Default;
                sc.ShowDialog();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();

                Insert_data(txtCodeNo.Text);
                txtCodeNo.Text = "";

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : Add", this.Name); }


            //try
            //{
            //    List<GridViewRowInfo> dgvRow_List = new List<GridViewRowInfo>();
            //    //dgvRow_List.Clear();
            //    Claim_ListAdd MS = new Claim_ListAdd(dgvRow_List);
            //    MS.ShowDialog();
            //    if (dgvRow_List.Count > 0)
            //    {
            //        string CodeNo = "";
            //        this.Cursor = Cursors.WaitCursor;
            //        using (DataClasses1DataContext db = new DataClasses1DataContext())
            //        {
            //            string ItemNo = "";
            //            string ItemDescription = "";                       
            //            decimal QTY = 0;                     
            //            string Unit = "";
            //            decimal UnitCost = 0;                      
            //            int id = 0;
            //            int Row = dgvData.Rows.Count() + 1;
            //            decimal Amount = 0;
            //            string Remark = "";
            //            foreach (GridViewRowInfo ee in dgvRow_List)
            //            {

            //                CodeNo = Convert.ToString(ee.Cells["CodeNo"].Value).Trim();
            //                if (!CodeNo.Equals("") && !check_Duppicate(CodeNo))
            //                {
            //                    ItemNo = Convert.ToString(ee.Cells["ItemNo"].Value).Trim();
            //                    ItemDescription = Convert.ToString(ee.Cells["ItemDesc"].Value).Trim();
            //                    QTY = Convert.ToDecimal(ee.Cells["Qty"].Value);
            //                    Unit = Convert.ToString(ee.Cells["Unit"].Value).Trim();
            //                    UnitCost = Convert.ToDecimal(ee.Cells["UnitCost"].Value);
            //                    Amount = Convert.ToDecimal(ee.Cells["Amount"].Value);
            //                    id = Convert.ToInt16(ee.Cells["Refsid"].Value);


            //                    dgvData.Rows.Add(Row.ToString(), CodeNo, ItemNo, ItemDescription, QTY, Unit, UnitCost, Amount, Remark, id,0);
            //                }
            //                else
            //                {
            //                    MessageBox.Show("รหัส ID Claim ซ้ำ");
            //                }
                            
            //            }
            //        }
            //    }
            //    //getTotal();           
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
            //finally { this.Cursor = Cursors.Default; }

        }
        private void Insert_data(string CodeNo)
        {

            try

            {
                if (!CodeNo.Equals(""))
                {
                    this.Cursor = Cursors.WaitCursor;
                    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    {
                        int No = 0;

                        string ItemNo = "";
                        string ItemDescription = "";
                        decimal QTY = 0;
                        //decimal RemainQty = 0;
                        string Unit = "";
                        decimal PCSUnit = 0;
                        decimal CostPerUnit = 0;
                        decimal Amount = 0;
                        int id = 0;
                        //string LotNo = "";
                        string Location = "";
                        string Remark = "";
                        int Row = dgvData.Rows.Count() + 1;
                        //int duppicate_CodeNo = 0;
                        //string Status = "Waiting";

                        var d1 = (from ix in db.tb_Items select ix)
                            .Where(a => a.CodeNo == CodeNo.Trim() && a.Status == "Active"

                            ).ToList();
                        if (d1.Count > 0)
                        {
                            var d = (from ix in db.tb_Items select ix)
                            .Where(a => a.CodeNo == CodeNo.Trim() && a.Status == "Active"

                            ).First();

                            ItemNo = d.ItemNo;
                            ItemDescription = d.ItemDescription;
                            //RemainQty = (Convert.ToDecimal(db.Cal_QTY(Convert.ToString(CodeNo), "Invoice", 0)));//Convert.ToDecimal(d.StockInv);
                            Unit = d.UnitBuy;
                            PCSUnit = Convert.ToDecimal(d.PCSUnit);
                            CostPerUnit = Convert.ToDecimal(d.StandardCost); // Convert.ToDecimal(dbClss.Get_Stock(CodeNo, "", "", "Avg"));//Convert.ToDecimal(d.StandardCost);
                            Location = d.Location;
                            No = dgvData.Rows.Count() + 1;
                            
                            if (!check_Duppicate(CodeNo))
                            {

                                dgvData.Rows.Add(Row.ToString(), CodeNo, ItemNo, ItemDescription, QTY, Unit, CostPerUnit,Amount, Remark, id);

                                //Add_Item(No, CodeNo, ItemNo, ItemDescription, RemainQty, QTY, Unit, PCSUnit, CostPerUnit, Amount, LotNo, Remark, "0", "", Location, "", "", "0");
                            }
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
                    if (StockControl.dbClss.TSt(rd1.Cells["CodeNo"].Value).Equals(CodeNo))
                        re = true;
                }
            }

            return re;

        }

        private void btnAdd_Row_Click(object sender, EventArgs e)
        {
            Add();
        }

        private void btn_AddItem_Click(object sender, EventArgs e)
        {
            Add();
        }

        private void btnView_Click_1(object sender, EventArgs e)
        {
            btnView.Enabled = false;
            btnEdit.Enabled = true;
            btnNew.Enabled = true;
            btnSave.Enabled = true;
            btnSendClaim.Enabled = true;

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
            btnSendClaim.Enabled = true;


            Enable_Status(true, "Edit");
            lblStatus.Text = "Edit";
            Ac = "Edit";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblStatus.Text != "Completed")
                {
                    lblStatus.Text = "Delete";
                    Ac = "Del";
                    if (MessageBox.Show("ต้องการลบรายการ ( " + txtClaimNo.Text + " ) หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        using (DataClasses1DataContext db = new DataClasses1DataContext())
                        {
                            var g = (from ix in db.tb_Claims
                                     where ix.ClaimNo.Trim() == txtClaimNo.Text.Trim()
                                     && ix.Status != "Cancel" && ix.Status != "Completed" 
                                     select ix).ToList();
                            if (g.Count > 0)  //มีรายการในระบบ
                            {
                                
                                foreach (var rr in g)
                                {

                                    rr.Status = "Cancel";
                                    rr.ModifyBy = ClassLib.Classlib.User;
                                    rr.ModifyDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                    rr.StatusClaim = false;
                                    db.SubmitChanges();

                                    dbClss.AddHistory(this.Name, "ลบรายการClaim", "ลบรายการClaim [" + rr.CodeNo + " จำนวน :" + rr.Qty.ToString() + " " + rr.Unit + "]", txtClaimNo.Text);

                                }
                                
                                db.SubmitChanges();
                                btnNew_Click(null, null);
                                Ac = "New";
                              
                            }
                            else // ไม่มีในระบบ
                            {
                                btnNew_Click(null, null);
                                Ac = "New";
                              
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

        private void btnSendClaim_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblStatus.Text != "Completed" && Ac == "Edit")
                {
                    lblStatus.Text = "Completed";
                    Ac = "Completed";
                    if (MessageBox.Show("ต้องการลบรายการ ( " + txtClaimNo.Text + " ) หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        using (DataClasses1DataContext db = new DataClasses1DataContext())
                        {
                            var g = (from ix in db.tb_Claims
                                     where ix.ClaimNo.Trim() == txtClaimNo.Text.Trim()
                                     && ix.Status != "Cancel" && ix.Status != "Completed"
                                     select ix).ToList();
                            if (g.Count > 0)  //มีรายการในระบบ
                            {

                                foreach (var rr in g)
                                {

                                    rr.Status = "Completed";
                                    rr.ModifyBy = ClassLib.Classlib.User;
                                    rr.ModifyDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                    rr.StatusClaim = true;
                                    db.SubmitChanges();

                                    dbClss.AddHistory(this.Name, "ส่งรายการClaim", "ส่งรายการClaim [" + rr.CodeNo + " จำนวน :" + rr.Qty.ToString() + " " + rr.Unit + "]", txtClaimNo.Text);

                                }

                                db.SubmitChanges();
                               
                            }
                           
                        }

                        MessageBox.Show("บันทึกสำเร็จ!");
                        btnRefresh_Click(null, null);
                    }
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
    }
}
