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
    public partial class Return_FG : Telerik.WinControls.UI.RadRibbonForm
    {
        public Return_FG()
        {
            InitializeComponent();
        }

        //private int RowView = 50;
        //private int ColView = 10;
        //DataTable dt = new DataTable();
        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            HistoryView hw = new HistoryView(this.Name);
            this.Cursor = Cursors.Default;
            hw.ShowDialog();
        }

        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }
        //private void GETDTRow()
        //{

        //    dt.Columns.Add(new DataColumn("CodeNo", typeof(string)));
        //    dt.Columns.Add(new DataColumn("ItemDescription", typeof(string)));
        //    dt.Columns.Add(new DataColumn("Order", typeof(decimal)));
        //    dt.Columns.Add(new DataColumn("BackOrder", typeof(decimal)));
        //    dt.Columns.Add(new DataColumn("StockQty", typeof(decimal)));
        //    dt.Columns.Add(new DataColumn("UnitBuy", typeof(string)));
        //    dt.Columns.Add(new DataColumn("PCSUnit", typeof(decimal)));
        //    dt.Columns.Add(new DataColumn("LeadTime", typeof(decimal)));
        //    dt.Columns.Add(new DataColumn("MaxStock", typeof(decimal)));
        //    dt.Columns.Add(new DataColumn("MinStock", typeof(decimal)));
        //    dt.Columns.Add(new DataColumn("VendorNo", typeof(string)));
        //    dt.Columns.Add(new DataColumn("VendorName", typeof(string)));



        //}

        private void Unit_Load(object sender, EventArgs e)
        {

            // txtCNNo.Text = StockControl.dbClss.GetNo(6, 0);
            btnNew_Click(null, null);
        }
        private void DefaultItem()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                //cboVendor.AutoCompleteMode = AutoCompleteMode.Append;
                //cboVendor.DisplayMember = "VendorName";
                //cboVendor.ValueMember = "VendorNo";
                //cboVendor.DataSource =(from ix in db.tb_Vendors.Where(s => s.Active == true) select new { ix.VendorNo,ix.VendorName}).ToList();
                //cboVendor.SelectedIndex = -1;

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

        private void Enable_Status(bool ss, string Condition)
        {
            if (Condition.Equals("-") || Condition.Equals("New"))
            {
                txtCodeNo.Enabled = ss;
                btnJobCard_Old.Enabled = ss;
                btnJobCard_New.Enabled = ss;
                txtQtyClaim.ReadOnly = false;
                txtClaimName.Enabled = ss;
                dtClaimDate.Enabled = ss;
                btnCal.Enabled = ss;
            }
            else if (Condition.Equals("View"))
            {
                txtCodeNo.Enabled = ss;
                btnJobCard_Old.Enabled = ss;
                btnJobCard_New.Enabled = ss;
                txtQtyClaim.ReadOnly = true;
                txtClaimName.Enabled = ss;
                dtClaimDate.Enabled = ss;
                btnCal.Enabled = ss;
            }

            else if (Condition.Equals("Edit"))
            {
                txtCodeNo.Enabled = ss;
                btnJobCard_Old.Enabled = ss;
                btnJobCard_New.Enabled = ss;
                txtQtyClaim.ReadOnly = false;
                txtClaimName.Enabled = ss;
                dtClaimDate.Enabled = ss;
                btnCal.Enabled = ss;
            }
        }
        string Ac = "";
        private void btnNew_Click(object sender, EventArgs e)
        {
            btnReturntoCustomer.Enabled = false;
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            btnDelete.Enabled = true;

            ClearData();
            Enable_Status(true, "New");
            lblStatus.Text = "New";
            Ac = "New";
            txtQtyClaim.ReadOnly = false; //เปิดให้พิมพ์จำนวนเองได้
            // getมาไว้ก่อน แต่ยังไมได้ save
            txtClaimNo.Text = StockControl.dbClss.GetNo(18, 0);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // radGridView1.ReadOnly = false;
            //// btnEdit.Enabled = false;
            // btnView.Enabled = true;
            // radGridView1.AllowAddNewRow = false;
            //DataLoad();
        }
        private bool Check_Save()
        {
            bool re = true;
            string err = "";
            try
            {
                if (txtDocNo.Text.Equals(""))
                {
                    err += "- “เลขที่ใบขาย :” เป็นค่าว่าง \n";
                }
                if (txtCodeNo.Text.Equals(""))
                {
                    err += "- “รหัสสินค้า :” เป็นค่าว่าง \n";
                }
                if (txtRemark.Text.Equals(""))
                {
                    err += "- “เหตุผลการเคลม :” เป็นค่าว่าง \n";
                }

                if (txtTempJobCard_New.Text.Equals(""))
                {
                    err += "- “Temp Job Card (เลขที่ใบผลิตงานใหม่) :” เป็นค่าว่าง \n";
                }
                else if (txtJobCard_New.Text.Equals(""))
                {
                    err += "- “Job Card (เลขที่ใบผลิตงานใหม่) :” เป็นค่าว่าง \n";
                }
                else if ( dbClss.TDe(txtQtyClaim.Text)> dbClss.TDe(txtQty.Text))
                {
                    err += "- “จำนวนสินค้าที่เคลม :” มากกว่าสินค้าขายไม่ได้ \n";
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
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                btnCal_Click(null, null);
                if (Check_Save())
                    return;

                if (MessageBox.Show("ต้องการบันทึก ?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Cursor = Cursors.WaitCursor;


                    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    {

                        var g = (from i in db.tb_Claim_FG_Details
                                 join s in db.tb_Claim_FGs on i.ClaimNo equals s.ClaimNo
                                 join t in db.tb_Items on i.CodeNo equals t.CodeNo

                                 where i.Status != "Cancel" && s.Status != "Cancel"
                                    && i.ClaimNo == (txtClaimNo.Text.Trim())
                                 //&& i.CodeNo == (txtCodeNo.Text.Trim())

                                 select new
                                 {
                                     ClaimNo = i.ClaimNo,
                                     CodeNo = i.CodeNo,
                                     ItemNo = t.ItemNo,
                                     ItemDesc = t.ItemDescription,
                                     id = i.id,
                                     Qty = i.Qty,
                                     UnitCost = i.UnitCost,
                                     DocNo = i.DocNo,
                                     Unit = t.UnitShip,
                                     Refid = i.Refid,
                                     Status = s.Status,
                                     RefJobCard = i.RefJobCard,
                                     RefTempJobCard = i.RefTempJobCard,
                                     Return = i.Return,
                                     ReturnBy = i.ReturnBy,
                                     ReturnDate = i.ReturnDate,
                                     RefDocNo = s.RefDocNo,
                                     Amount = i.Amount,
                                     Remark = i.Remark,
                                     ClaimBy = s.ClaimBy,
                                     ClaimDate = s.ClaimDate

                                 }
                           ).ToList();
                        if (g.Count > 0)
                        {
                            MessageBox.Show("เลขที่ใบเคลม(Return FG) ถูกใช้แล้ว กรุณากดสร้างรายการใหม่.");
                            return;
                        }
                        else
                        {

                            if (Ac.Equals("New"))
                            {
                                txtClaimNo.Text = StockControl.dbClss.GetNo(18, 2);

                                //Cal Cost ใหม่ก่อนค่อยบันทึก

                                if (txtJobCard_New.Text != "")
                                {
                                    Save(1); // Save จากเลข Refer ที่ผู้ใช้สร้างเอง
                                    //Create List Jobcard Refer
                                    db.sp_038_tb_Claim_FG_ReferJobCard_ADD(txtClaimNo.Text, txtCodeNo.Text, txtDocNo.Text);

                                }
                                             //else
                                             //    Save(2);//ระบบสร้างให้ Auto



                                int id = 0;
                                var s1 = (from ix in db.tb_Claim_FG_Details
                                              //join i in db.tb_Items on ix.CodeNo equals i.CodeNo
                                          where ix.ClaimNo.Trim() == txtClaimNo.Text.Trim()
                                          && ix.CodeNo.Trim() == txtCodeNo.Text.Trim()
                                          && ix.Status != "Cancel"
                                          select ix).ToList();
                                if (s1.Count > 0)
                                {
                                    id = dbClss.TInt(s1.FirstOrDefault().id);
                                }
                                if (id > 0)
                                {


                                    //----------บันทึก Refer Claim---------
                                    var f = (from i in db.tb_Sell_FG_Details
                                                 //join s in db.tb_Sell_FGs on i.DocNo equals s.DocNo
                                                 // join t in db.tb_Items on i.CodeNo equals t.CodeNo

                                             where i.Status != "Cancel"
                                                 && i.DocNo == (txtDocNo.Text.Trim())
                                                 && i.CodeNo == (txtCodeNo.Text.Trim())
                                                 && i.id == dbClss.TInt(txtReftid.Text)
                                             select new
                                             {
                                                 id = i.id,
                                                 RefidClaim = i.RefidClaim,
                                                 RefidClaim2 = i.RefidClaim_2,
                                                 RefidClaim3 = i.RefidClaim_3,
                                                 RefidClaim4 = i.RefidClaim_4,
                                                 RefidClaim5 = i.RefidClaim_5
                                             }
                                ).ToList();
                                    if (f.Count > 0)
                                    {
                                        var ff = (from ix in db.tb_Sell_FG_Details
                                                  where ix.Status != "Cancel"
                                                 && ix.DocNo == (txtDocNo.Text.Trim())
                                                 && ix.CodeNo == (txtCodeNo.Text.Trim())
                                                 && ix.id == dbClss.TInt(txtReftid.Text)
                                                  select ix).First();

                                        if (ff.RefidClaim == null || ff.RefidClaim == 0)
                                            ff.RefidClaim = id;
                                        else if (ff.RefidClaim_2 == null || ff.RefidClaim_2 == 0)
                                            ff.RefidClaim_2 = id;
                                        else if (ff.RefidClaim_3 == null || ff.RefidClaim_3 == 0)
                                            ff.RefidClaim_3 = id;
                                        else if (ff.RefidClaim_4 == null || ff.RefidClaim_4 == 0)
                                            ff.RefidClaim_4 = id;
                                        else if (ff.RefidClaim_5 == null || ff.RefidClaim_5 == 0)
                                            ff.RefidClaim_5 = id;


                                        dbClss.AddHistory(this.Name, "แก้ไขใบร้องขอการผลิต", "รับงานคืน เลขที่ Refer [" + txtJobCard_New.Text.Trim() + " TempJobCard : " + txtTempJobCard_New.Text + " จำนวนเคลม : " + txtQtyClaim.Text + "]", txtDocNo.Text);
                                        dbClss.AddHistory(this.Name, "แก้ไขใบร้องขอการผลิต", "สร้าง เคลมสินค้า เลขที่ใบขาย [" + txtDocNo.Text.Trim() + " CodeNo : " + txtCodeNo.Text + " จำนวนเคลม : " + txtQtyClaim.Text + "]", txtClaimNo.Text);

                                        db.SubmitChanges();
                                    }
                                }
                            }
                        }
                    }
                    btnRefresh_Click(null, null);
                    MessageBox.Show("บันทึกสำเร็จ!");
                    btnSave.Enabled = false;
                    btnNew.Enabled = true;
                    //btnDelete.Enabled = true;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        private void Save(int c)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                decimal UnitCost = dbClss.TDe(txtUnitCost_New.Text);
                decimal Amount = UnitCost * dbClss.TDe(txtQtyClaim.Text);

                if (c == 1)
                {
                    tb_Claim_FG gg = new tb_Claim_FG();
                    gg.ClaimNo = txtClaimNo.Text;
                    gg.ClaimBy = ClassLib.Classlib.User;
                    gg.ClaimDate = dtClaimDate.Value;
                    gg.RefDocNo = txtDocNo.Text;
                    gg.Remark = txtRemark.Text;
                    gg.CreateBy = ClassLib.Classlib.User;
                    gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    gg.Status = "Process";
                    db.tb_Claim_FGs.InsertOnSubmit(gg);
                    db.SubmitChanges();


                    tb_Claim_FG_Detail dd = new tb_Claim_FG_Detail();
                    dd.ClaimNo = txtClaimNo.Text;
                    dd.DocNo = txtDocNo.Text;
                    dd.CodeNo = txtCodeNo.Text;
                    dd.Refid = dbClss.TInt(txtReftid.Text);
                    dd.Qty = dbClss.TDe(txtQtyClaim.Text);
                    dd.UnitCost = UnitCost;
                    dd.Amount = Amount;
                    dd.RefJobCard = txtJobCard_New.Text;
                    dd.RefTempJobCard = txtTempJobCard_New.Text;
                    dd.Remark = txtRemark.Text;
                    dd.Status = "Process";
                    dd.Additional_Amount = dbClss.TDe(txtCal_Amount_New.Text);
                    dd.Additional_CostUnit = dbClss.TDe(txtCal_UnitCost_New.Text);

                    db.tb_Claim_FG_Details.InsertOnSubmit(dd);
                    db.SubmitChanges();
                }
                //else if(c==2)
                //{

                //}
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

        private void ClearData()
        {
            txtSHNo.Text = "";
            txtDocNo.Text = "";
            txtQty.Text = "0.00";
            txtUnit.Text = "0.00";
            txtCodeNo.Text = "";
            txtReftid.Text = "";
            txtItemDescription.Text = "";
            txtItemNo.Text = "";
            lblStatus.Text = "New";
            txtJobCard_New.Text = "";
            txtTempJobCard_New.Text = "";
            //txtCodeNo_New.Text = "";
            txtid_New.Text = "";
            txtQtyClaim.Text = "0.00";
            txtUnitCost.Text = "0.00";
            txtUnitCost_New.Text = "0.00";
            txtUnitClaim.Text = "";
            txtClaimName.Text = ClassLib.Classlib.User;
            dtClaimDate.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
            Ac = "";
            txtRemark.Text = "";
            txtCal_Amount_New.Text = "0.00";
            txtCal_UnitCost_New.Text = "0.00";
        }
        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                // radGridView1.Rows[e.RowIndex].Cells["dgvC"].Value = "T";
                //string TM1 = Convert.ToString(radGridView1.Rows[e.RowIndex].Cells["ModelName"].Value);
                ////string TM2 = Convert.ToString(radGridView1.Rows[e.RowIndex].Cells["MMM"].Value);
                //string Chk = Convert.ToString(radGridView1.Rows[e.RowIndex].Cells["dgvCodeTemp"].Value);
                //if (Chk.Equals("") && !TM1.Equals(""))
                //{

                //    if (!CheckDuplicate(TM1, Chk))
                //    {
                //        MessageBox.Show("ข้อมูล รายการซ้า");
                //        radGridView1.Rows[e.RowIndex].Cells["ModelName"].Value = "";
                //        //  radGridView1.Rows[e.RowIndex].Cells["MMM"].Value = "";
                //        //  radGridView1.Rows[e.RowIndex].Cells["UnitCode"].IsSelected = true;

                //    }
                //}


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

            //if (e.KeyData == (Keys.Control | Keys.S))
            //{
            //    if (MessageBox.Show("ต้องการบันทึก ?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //    {
            //        return;
            //        AddUnit();
            //        DataLoad();
            //    }
            //}
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        int row = -1;
        private void radGridView1_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            row = e.RowIndex;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            //dbClss.ExportGridCSV(radGridView1);
            // dbClss.ExportGridXlSX(radGridView1);
        }


        private void btnFilter1_Click(object sender, EventArgs e)
        {
            //  radGridView1.EnableFiltering = true;
        }

        private void btnUnfilter1_Click(object sender, EventArgs e)
        {
            // radGridView1.EnableFiltering = false;
        }

        private void radMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {

        }

        private void radGridView1_Click(object sender, EventArgs e)
        {

        }

        private void chkActive_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {

        }

        private void radButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
                //btnSave.Enabled = false;
                //btnEdit.Enabled = true;
                //btnView.Enabled = false;
                btnNew.Enabled = true;

                txtDocNo.Text = "";
                txtUnit.Text = "";
                txtCodeNo.Text = "";
                txtReftid.Text = "";
                txtItemDescription.Text = "";
                txtItemNo.Text = "";
                txtQty.Text = "0.00";
                txtUnitCost.Text = "0.00";


                //Enable_Status(false, "View");

                this.Cursor = Cursors.WaitCursor;
                Return_FG_ListADD sc = new Return_FG_ListADD(txtDocNo, txtReftid, txtCodeNo, txtItemNo, txtItemDescription, txtQty, txtUnit, txtUnitCost);
                this.Cursor = Cursors.Default;
                sc.ShowDialog();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();
                //LoadData

                //string SHNo = txtDocNo.Text;
                //if (!txtDocNo.Text.Equals(""))
                //{

                //    Load_Shipping_FG_Old();
                //   // DataLoad();
                //    //Ac = "View";
                //    //btnDel_Item.Enabled = false;
                //    //btnSave.Enabled = false;
                //    btnNew.Enabled = true;
                //}

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : radButton1_Click_1", this.Name); }

        }
        private void Load_Shipping_FG_Old()
        {
            //try
            //{
            //    using (DataClasses1DataContext db = new DataClasses1DataContext())
            //    {

            //        var g = (from i in db.tb_Shipping_JobCards
            //                 join s in db.tb_JobCards on i.TempJobCard equals s.TempJobCard

            //                 where i.Status != "Cancel"
            //                    // && i.Status != "Completed"
            //                    && i.SHNo.Trim() == txtDocNo.Text.Trim()
            //                    && s.RefRT_JobCard == null
            //                 select new
            //                 {
            //                     SHNo = i.SHNo,
            //                     CodeNo = s.CodeNo,
            //                     ItemNo = s.ItemName,
            //                     ItemDescription = s.ItemDesc,
            //                     id = i.id,
            //                     JobCard = i.JobCard.Trim(),
            //                     TempJobCard = i.TempJobCard.Trim(),
            //                     CSTM_Name = i.CSTM_Name,
            //                     CSTM_Address = i.CSTM_Address,
            //                     SHDate = i.SHDate,
            //                     SHBy = i.SHBy,
            //                     Qty = s.Qty,
            //                     LotNo = s.LotNo,
            //                     Unit = s.Unit,
            //                     Remark = i.Remark,
            //                     Status = i.Status,
            //                     CreateBy = i.CreateBy,
            //                     CreateDate = i.CreateDate

            //                 }
            //               ).ToList();

            //        //var g = (from ix in db.tb_Shipping_JobCards select ix)
            //        //    .Where(a => a.SHNo == txtSHFG.Text.Trim()).ToList();
            //        if (g.Count() > 0)
            //        {
            //            //DateTime? temp_date = null;

            //            txtid.Text = dbClss.TSt(g.FirstOrDefault().id).Trim();
            //            txtTempJobCard.Text = dbClss.TSt(g.FirstOrDefault().TempJobCard).Trim();
            //            txtJobCard.Text = dbClss.TSt(g.FirstOrDefault().JobCard);
            //            txtCodeNo.Text = dbClss.TSt(g.FirstOrDefault().CodeNo);
            //            txtItemNo.Text = dbClss.TSt(g.FirstOrDefault().ItemNo);
            //            txtItemDescription.Text = dbClss.TSt(g.FirstOrDefault().ItemDescription);
            //            txtDocNo.Text = dbClss.TSt(g.FirstOrDefault().SHNo);

            //        }
            //        else
            //        {
            //            txtid.Text = dbClss.TSt(0).Trim();
            //            txtTempJobCard.Text = "";
            //            txtJobCard.Text = "";
            //            txtCodeNo.Text = "";
            //            txtItemNo.Text = "";
            //            txtItemDescription.Text = "";
            //            txtDocNo.Text = "";
            //            MessageBox.Show("สถานะไม่ถูกต้อง หรือรายการดังกล่าวถูกทำรายการรับคืนเรียบร้อยแล้ว");
            //        }


            //    }
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : radButton1_Click_1", this.Name); }

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

        }

        private void radPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cboModelName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (crow == 0)
            //    DataLoad();
        }

        private void cboYear_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {

        }

        private void radCheckBox1_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            //if(radCheckBox1.Checked)
            //{
            //    foreach(var rd in radGridView1.Rows)
            //    {
            //        rd.Cells["S"].Value = true;
            //    }
            //}else
            //{
            //    foreach (var rd in radGridView1.Rows)
            //    {
            //        rd.Cells["S"].Value = false;
            //    }
            //}
        }

        private void radTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void ddlType_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            //string temp = ddlType.Text;
            //ClearData();
            //ddlType.Text = temp;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }

        private void btnListItem_Click(object sender, EventArgs e)
        {
            Return_FG_List a = new Return_FG_List(txtClaimNo,txtCodeNo);
            a.ShowDialog();
            if(txtClaimNo.Text !="")
            {
                btnRefresh_Click(null, null);
            }
            else
            {
                btnNew_Click(null, null);
            }
        }

        private void btnJobCard_New_Click(object sender, EventArgs e)
        {

            try
            {
                //btnSave.Enabled = false;
                //btnEdit.Enabled = true;
                //btnView.Enabled = false;
                btnNew.Enabled = true;

                txtJobCard_New.Text = "";
                txtTempJobCard_New.Text = "";
                txtQtyClaim.Text = "0.00";
                txtid_New.Text = "";
                txtUnit.Text = "";
                txtUnitCost_New.Text = "";


                this.Cursor = Cursors.WaitCursor;
                CreateJob_List sc = new CreateJob_List(txtJobCard_New, txtTempJobCard_New, txtid_New, txtQtyClaim, txtUnitClaim);
                this.Cursor = Cursors.Default;
                this.Cursor = Cursors.Default;
                sc.ShowDialog();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();

                //decimal Unitcost = dbClss.TDe(txtUnitCost.Text);
                //decimal qty_new = dbClss.TDe(txtQtyClaim.Text);
                txtUnitCost_New.Text = txtUnitCost.Text;
                txtQtyClaim.ReadOnly = true;

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var g = (from ix in db.tb_JobCards select ix)
                  .Where(a => a.Status != "Cancel"
                  && a.JobCard==(txtJobCard_New.Text.Trim())
                  && a.TempJobCard==(txtTempJobCard_New.Text.Trim())
                  
                 ).ToList();
                    if(g.Count>0)
                    {
                        if(dbClss.TSt(g.FirstOrDefault().Status)=="Completed"
                            || dbClss.TSt(g.FirstOrDefault().Status) == "Discon")
                        {
                            MessageBox.Show("ไม่สามารถใช้เลขที่ใบสั่งผลิตสินค้านี้ได้ เนื่องจากถูกปิดไปแล้ว");
                            txtJobCard_New.Text = "";
                            txtTempJobCard_New.Text = "";
                            txtid_New.Text = "0";
                            txtQtyClaim.Text = "0";
                            txtUnitClaim.Text = "";
                        }
                    }
                }
                //LoadData

                //string SHNo = txtTempJobCard_New.Text;
                //if (!txtTempJobCard_New.Text.Equals(""))
                //{

                //    Load_JobCard();
                //    // DataLoad();
                //    //Ac = "View";
                //    //btnDel_Item.Enabled = false;
                //    //btnSave.Enabled = false;
                //    btnNew.Enabled = true;
                //}

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : btnJobCard_New_Click", this.Name); }


        }
        private void Load_JobCard()
        {
            //try
            //{
            //    using (DataClasses1DataContext db = new DataClasses1DataContext())
            //    {

            //        var g = (from i in db.tb_JobCards

            //                 where i.Status != "Cancel"
            //                    && i.Status != "Completed"
            //                    && i.TempJobCard.Trim() == txtTempJobCard_New.Text.Trim()
            //                    && i.RefRT_JobCard == null
            //                 select new
            //                 {
            //                     id = i.id,
            //                     TempJobCard = i.TempJobCard,
            //                     JobCard = i.JobCard,
            //                     CodeNo = i.CodeNo
            //                     ,ItemNo = i.ItemName
            //                     ,ItemDesc = i.ItemDesc

            //                 }
            //               ).ToList();

            //        //var g = (from ix in db.tb_Shipping_JobCards select ix)
            //        //    .Where(a => a.SHNo == txtSHFG.Text.Trim()).ToList();
            //        if (g.Count() > 0)
            //        {
            //            //DateTime? temp_date = null;

            //            txtid_New.Text = dbClss.TSt(g.FirstOrDefault().id).Trim();
            //            txtTempJobCard_New.Text = dbClss.TSt(g.FirstOrDefault().TempJobCard).Trim();
            //            txtJobCard_New.Text = dbClss.TSt(g.FirstOrDefault().JobCard);
            //            txtCodeNo_New.Text = dbClss.TSt(g.FirstOrDefault().CodeNo);
            //            txtItemNo_New.Text = dbClss.TSt(g.FirstOrDefault().ItemNo);
            //            txtItemDesc_New.Text = dbClss.TSt(g.FirstOrDefault().ItemDesc);

            //        }
            //        else
            //        {
            //            txtid_New.Text = dbClss.TSt(0).Trim();
            //            txtTempJobCard_New.Text = "";
            //            txtJobCard_New.Text = "";
            //            txtCodeNo_New.Text = "";
            //            txtItemNo_New.Text = "";
            //            txtItemDesc_New.Text = "";

            //            MessageBox.Show("สถานะไม่ถูกต้อง หรือรายการดังกล่าวถูกเบิกสินค้าขายเรียบร้อยแล้ว");
            //        }


            //    }
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : radButton1_Click_1", this.Name); }

        }

        private void txtQtyClaim_KeyPress(object sender, KeyPressEventArgs e)
        {
            StockControl.dbClss.CheckDigitDecimal(e);
        }

        private void btnReturntoCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                if (!lblStatus.Text.Equals("Completed"))
                {
                    
                    btnCal_Click(null, null);

                    SaveReturnFG_to_CSTM();
                    btnRefresh_Click(null, null);

                    MessageBox.Show("บันทึกสำเร็จ!");
                    btnSave.Enabled = false;
                    btnNew.Enabled = true;
                    btnDelete.Enabled = false;
                    btnReturntoCustomer.Enabled = false;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : btnReturntoCustomer_Click", this.Name); }


        }
        private void SaveReturnFG_to_CSTM()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                if (txtSHNo.Text == "")
                    txtSHNo.Text = StockControl.dbClss.GetNo(15, 2);

                if (!txtSHNo.Text.Equals(""))
                {
                    var g = (from ix in db.tb_Shipping_JobCards
                             where ix.SHNo.Trim() == txtSHNo.Text
                             && ix.Status != "Cancel"
                              && ix.Status != "Completed"
                             select ix).ToList();
                    if (g.Count > 0)  //มีรายการในระบบ
                    {

                        var gg = (from ix in db.tb_Shipping_JobCards
                                  where ix.SHNo.Trim() == txtSHNo.Text.Trim()
                                 && ix.Status != "Cancel"
                                  && ix.Status != "Completed"
                                  select ix).First();

                        gg.CreateBy = ClassLib.Classlib.User;
                        gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                        dbClss.AddHistory(this.Name, "แก้ไขขายสินค้า", "แก้ไขโดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", txtSHNo.Text);

                        //gg.UnitCost = dbClss.TDe(txtUnitCost_New.Text); // dbClss.TDe(txtCal_UnitCost_New.Text);
                        //gg.Amount = Math.Round((dbClss.TDe(txtUnitCost_New.Text) * dbClss.TDe(txtQtyClaim.Text)),2);
                        //gg.Qty = dbClss.TDe(txtQtyClaim.Text);

                        gg.UnitCost =  dbClss.TDe(txtCal_UnitCost_New.Text);
                        gg.Amount = Math.Round((dbClss.TDe(txtCal_UnitCost_New.Text) * dbClss.TDe(txtQtyClaim.Text)), 2);
                        gg.Qty = dbClss.TDe(txtQtyClaim.Text);

                        gg.SHDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                        //dbClss.AddHistory(this.Name, "แก้ไขขายสินค้า", "แก้ไขวันที่เบิกสินค้า [" + DateTime.Now.ToString().Trim() + " เดิม :" + temp.ToString() + "]", txtSHNo.Text);


                        gg.Status = "Completed";
                        db.SubmitChanges();


                        var j = (from ix in db.tb_JobCards
                                 where ix.TempJobCard.Trim() == txtTempJobCard_New.Text.Trim()
                                 && ix.Status != "Cancel"
                                  && ix.Status != "Completed"
                                  && ix.Status != "Discon"
                                  && ix.Type == "Claim"
                                 select ix).ToList();
                        if (j.Count > 0)
                        {
                            var jj = (from ix in db.tb_JobCards
                                      where ix.TempJobCard.Trim() == txtTempJobCard_New.Text.Trim()
                                      && ix.Status != "Cancel"
                                       && ix.Status != "Completed"
                                       && ix.Status != "Discon"
                                       && ix.Type == "Claim"
                                      select ix).First();

                            
                            jj.Status = "Completed";
                            jj.RemainQty = 0;
                            jj.UnitCost = dbClss.TDe(txtCal_UnitCost_New.Text);
                            jj.Amount =  Math.Round((dbClss.TDe(txtCal_UnitCost_New.Text) * dbClss.TDe(txtQtyClaim.Text)), 2);
                            db.SubmitChanges();
                            dbClss.AddHistory(this.Name, "แก้ไขใบร้องขอการเคลม", "ส่งงานสินค้าเคลมคืน [" + txtTempJobCard_New.Text.Trim() + "]", txtTempJobCard_New.Text);
                        }
                    }
                    else //สร้างใหม่
                    {
                        byte[] barcode = null;
                        //barcode = StockControl.dbClss.SaveQRCode2D(txtSHFG.Text.Trim());
                        //DateTime? UpdateDate = null;

                        string CSTM_Name = "";
                        string CSTM_Address = "";
                        DateTime? SHDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                        var cc = (from ix in db.tb_JobCards select ix)
                          .Where(a => a.TempJobCard == txtTempJobCard_New.Text.Trim()).ToList();
                        if (cc.Count() > 0)
                        {
                            CSTM_Name = dbClss.TSt(cc.FirstOrDefault().CustomerName);
                            CSTM_Address = dbClss.TSt(cc.FirstOrDefault().Address);
                        }
                        
                        tb_Shipping_JobCard gg = new tb_Shipping_JobCard();
                        gg.SHNo = txtSHNo.Text;
                        gg.SHDate = SHDate;
                        gg.SHBy = ClassLib.Classlib.User;
                        gg.CreateBy = ClassLib.Classlib.User;
                        gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                        gg.CSTM_Name = CSTM_Name;
                        gg.CSTM_Address = CSTM_Address;
                        gg.JobCard = txtJobCard_New.Text;
                        gg.TempJobCard = txtTempJobCard_New.Text;
                        gg.Status = "Completed";
                        gg.Remark = "";
                        gg.UnitCost = dbClss.TDe(txtCal_UnitCost_New.Text);
                        gg.Amount = Math.Round((dbClss.TDe(txtCal_UnitCost_New.Text) * dbClss.TDe(txtQtyClaim.Text)), 2);//dbClss.TDe(txtCal_Amount_New.Text);
                        gg.Qty = dbClss.TDe(txtQtyClaim.Text);

                        gg.Barcode = barcode;
                        db.tb_Shipping_JobCards.InsertOnSubmit(gg);
                        db.SubmitChanges();

                        dbClss.AddHistory(this.Name, "แก้ไขส่งสินค้าเคลมคืน", "ส่งงานสินค้าเคลมคืน [" + txtSHNo.Text.Trim() + "]", txtSHNo.Text);

                        var j = (from ix in db.tb_JobCards
                                 where ix.TempJobCard.Trim() == txtTempJobCard_New.Text.Trim()
                                 && ix.Status != "Cancel"
                                  && ix.Status != "Completed"
                                  && ix.Status != "Discon"
                                  && ix.Type == "Claim"
                                 select ix).ToList();
                        if (j.Count > 0)
                        {
                            var jj = (from ix in db.tb_JobCards
                                      where ix.TempJobCard.Trim() == txtTempJobCard_New.Text.Trim()
                                      && ix.Status != "Cancel"
                                       && ix.Status != "Completed"
                                       && ix.Status != "Discon"
                                       && ix.Type == "Claim"
                                      select ix).First();

                           
                            jj.Status = "Completed";
                            jj.RemainQty = 0;
                            jj.UnitCost = dbClss.TDe(txtCal_UnitCost_New.Text);
                            jj.Amount = Math.Round((dbClss.TDe(txtCal_UnitCost_New.Text) * dbClss.TDe(txtQtyClaim.Text)), 2);

                            db.SubmitChanges();
                            dbClss.AddHistory(this.Name, "แก้ไขใบร้องขอการผลิต", "ส่งงานการขอการผลิต [" + txtTempJobCard_New.Text.Trim() + "]", txtTempJobCard_New.Text);
                        }


                        var s1 = (from ix in db.tb_Claim_FGs
                                      //join i in db.tb_Items on ix.CodeNo equals i.CodeNo
                                  where ix.ClaimNo.Trim() == txtClaimNo.Text.Trim()
                                  && ix.Status != "Cancel" &&  ix.Status != "Completed"
                                  select ix).ToList();
                        if (s1.Count > 0)
                        {
                            foreach (var aa in s1)
                            {
                                aa.Status = "Completed";
                                
                                db.SubmitChanges();
                            }
                        }
                        var s3 = (from ix in db.tb_Claim_FG_ReferJobCards
                                      //join i in db.tb_Items on ix.CodeNo equals i.CodeNo
                                  where ix.ClaimNo.Trim() == txtClaimNo.Text.Trim()
                                  && ix.CodeNo.Trim() == txtCodeNo.Text.Trim()
                                  && ix.Status != "Cancel" && ix.Status != "Completed"
                                  select ix).ToList();
                        if (s3.Count > 0)
                        {
                            foreach (var aa in s3)
                            {
                                aa.Status = "Completed";
                                db.SubmitChanges();
                            }
                        }

                        var s2 = (from ix in db.tb_Claim_FG_Details
                                      //join i in db.tb_Items on ix.CodeNo equals i.CodeNo
                                  where ix.ClaimNo.Trim() == txtClaimNo.Text.Trim()
                                  && ix.CodeNo.Trim() == txtCodeNo.Text.Trim()
                                  && ix.Status != "Cancel" && ix.Status != "Completed"
                                  select ix).ToList();
                        if (s2.Count > 0)
                        {
                           

                            foreach (var aa in s2)
                            {
                                aa.Status = "Completed";
                                aa.Return = true;
                                aa.ReturnBy = ClassLib.Classlib.User;
                                aa.ReturnDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                
                                aa.Additional_CostUnit = dbClss.TDe(txtCal_UnitCost_New.Text);
                                aa.Additional_Amount = Math.Round((dbClss.TDe(txtCal_UnitCost_New.Text) * dbClss.TDe(txtQtyClaim.Text)), 2);

                                aa.UnitCost_New = dbClss.TDe(txtCal_UnitCost_New.Text) + dbClss.TDe( txtUnitCost_New.Text);
                                aa.Amount_New = dbClss.TDe(txtQtyClaim.Text) * (dbClss.TDe(txtCal_UnitCost_New.Text) + dbClss.TDe(txtUnitCost_New.Text));
                                db.SubmitChanges();

                                dbClss.AddHistory(this.Name, "แก้ไขใบร้องขอการผลิต", "ส่งงานการขอการผลิต [" + txtCodeNo.Text.Trim() + "]", txtClaimNo.Text);

                            }
                        }

                    }
                }
            }
        }

        private void btnCal_Click(object sender, EventArgs e)
        {
            try
            {
                ////Cal ก่อน Save
                //decimal Qty = dbClss.TDe(txtQtyClaim.Text);
                //txtCal_Amount_New.Text = "";
                //txtCal_UnitCost_New.Text = "";

                try
                {
                    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    {
                        decimal Amount = 0;
                        //var g = (from ix in db.sp_040_Cal_Cost_JobCard_All(txtJobCard_New.Text, txtTempJobCard_New.Text, dbClss.TDe(txtQtyClaim.Text), "", "") select ix).ToList();
                        //if (g.Count > 0)
                        //{
                        //    foreach (var gg in g)
                        //    {
                        //        Amount += dbClss.TDe(gg.Amount);
                        //    }

                        //    if (dbClss.TDe(txtQtyClaim.Text) > 0 && Amount > 0)
                        //        txtCal_UnitCost_New.Text = (Amount / dbClss.TDe(txtQtyClaim.Text)).ToString("N2");

                        //    txtCal_Amount_New.Text = Amount.ToString("N2");
                        //}




                        var g1 = (from ix in db.sp_035_Cal_Cost_JobCard(txtJobCard_New.Text, txtTempJobCard_New.Text, dbClss.TDe(txtQtyClaim.Text), "", "") select ix).ToList();
                        if (g1.Count > 0)
                        {
                            foreach (var gg in g1)
                            {
                                Amount += dbClss.TDe(gg.Amount);
                            }

                            if (dbClss.TDe(txtQtyClaim.Text) > 0)
                            {
                                //decimal temp = dbClss.TDe(txtUnitCost_New.Text);
                                //Amount += temp;

                                //txtUnitCost_New.Text = (Amount / dbClss.TDe(txtQtyClaim.Text)).ToString("N2"); 
                                txtCal_UnitCost_New.Text = (Amount / dbClss.TDe(txtQtyClaim.Text)).ToString("N2");
                            }
                            txtCal_Amount_New.Text = Amount.ToString("N2");
                        }
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : btnReturntoCustomer_Click", this.Name); }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var g = (from i in db.tb_Claim_FG_Details
                             join s in db.tb_Claim_FGs on i.ClaimNo equals s.ClaimNo
                             join t in db.tb_Items on i.CodeNo equals t.CodeNo
                            
                             where i.Status != "Cancel" && s.Status != "Cancel"
                                && i.ClaimNo == (txtClaimNo.Text.Trim())
                             //&& i.CodeNo == (txtCodeNo.Text.Trim())

                             select new
                             {
                                 ClaimNo = i.ClaimNo,
                                 CodeNo = i.CodeNo,
                                 ItemNo = t.ItemNo,
                                 ItemDesc = t.ItemDescription,
                                 id = i.id,
                                 Qty = i.Qty,
                                 UnitCost = i.UnitCost,

                                 DocNo = i.DocNo,
                                 Unit = t.UnitShip,
                                 Refid = i.Refid,
                                 Status = s.Status,
                                 RefJobCard = i.RefJobCard,
                                 RefTempJobCard = i.RefTempJobCard,
                                 Return = i.Return,
                                 ReturnBy = i.ReturnBy,
                                 ReturnDate = i.ReturnDate,
                                 RefDocNo = s.RefDocNo,
                                 Amount = i.Amount,
                                 Remark = i.Remark,
                                 ClaimBy = s.ClaimBy,
                                 ClaimDate = s.ClaimDate,
                                 Cal_UnitCost_New = i.UnitCost_New,
                                 Cal_Amount_New = i.Amount_New

                                 

                             }
                              ).ToList();
                    if (g.Count > 0)
                    {
                        txtQtyClaim.Text = dbClss.TDe(g.FirstOrDefault().Qty).ToString("N2");
                        //txtUnitCost_New.Text = dbClss.TDe(g.FirstOrDefault().UnitCost).ToString("N2");
                        txtUnitCost.Text = dbClss.TDe(g.FirstOrDefault().UnitCost).ToString("N2");

                        txtCal_UnitCost_New.Text = dbClss.TDe(g.FirstOrDefault().Cal_UnitCost_New).ToString("N2");
                        txtCal_Amount_New.Text = dbClss.TDe(g.FirstOrDefault().Cal_Amount_New).ToString("N2");
                        //txtid_New.Text = dbClss.TSt(g.FirstOrDefault().Refid);

                        txtClaimNo.Text = dbClss.TSt(g.FirstOrDefault().ClaimNo);
                        txtDocNo.Text = dbClss.TSt(g.FirstOrDefault().RefDocNo);
                        txtCodeNo.Text = dbClss.TSt(g.FirstOrDefault().CodeNo);
                        txtReftid.Text = dbClss.TSt(g.FirstOrDefault().Refid);
                        txtJobCard_New.Text = dbClss.TSt(g.FirstOrDefault().RefJobCard);
                        txtTempJobCard_New.Text = dbClss.TSt(g.FirstOrDefault().RefTempJobCard);
                        txtItemDescription.Text = dbClss.TSt(g.FirstOrDefault().ItemDesc);
                        txtItemNo.Text = dbClss.TSt(g.FirstOrDefault().ItemNo);
                        txtUnit.Text = dbClss.TSt(g.FirstOrDefault().Unit);
                        txtUnitClaim.Text = dbClss.TSt(g.FirstOrDefault().Unit);

                        decimal temp = 0;
                        var r = (from ix in db.tb_Claim_FG_ReferJobCards select ix).Where(a => a.Status != "Cancel" && a.ClaimNo.Trim().ToUpper() == txtClaimNo.Text.Trim().ToUpper()) .ToList();
                        if (r.Count > 0)
                        {
                            foreach(var rr in r)
                            {
                                temp += dbClss.TDe(rr.UnitCost);
                            }
                            txtUnitCost_New.Text = temp.ToString("N2");
                        }

                            lblStatus.Text = dbClss.TSt(g.FirstOrDefault().Status);
                        //Enable_Status(true, "New");
                        if (lblStatus.Text =="Completed")
                        {
                            btnSave.Enabled = false;
                            btnNew.Enabled = true;
                            btnReturntoCustomer.Enabled = false;

                            txtCodeNo.Enabled = false;
                            btnJobCard_Old.Enabled = false;
                            btnJobCard_New.Enabled = false;
                            txtQtyClaim.ReadOnly = true;
                            txtClaimName.Enabled = false;
                            dtClaimDate.Enabled = false;
                            btnCal.Enabled = false;
                            btnDelete.Enabled = false;
                        }
                        else if (lblStatus.Text == "Process")
                        {
                            btnSave.Enabled = false;
                            btnNew.Enabled = true;
                            btnReturntoCustomer.Enabled = true;

                            txtCodeNo.Enabled = false;
                            btnJobCard_Old.Enabled = false;
                            btnJobCard_New.Enabled = false;
                            txtQtyClaim.ReadOnly = true;
                            txtClaimName.Enabled = false;
                            dtClaimDate.Enabled = false;
                            btnCal.Enabled = true;
                            btnDelete.Enabled = true;
                        }
                        else
                        {
                            btnSave.Enabled = false;
                            btnNew.Enabled = true;
                            btnReturntoCustomer.Enabled = true;

                            txtCodeNo.Enabled = false;
                            btnJobCard_Old.Enabled = false;
                            btnJobCard_New.Enabled = false;
                            txtQtyClaim.ReadOnly = true;
                            txtClaimName.Enabled = false;
                            dtClaimDate.Enabled = false;
                            btnCal.Enabled = true;
                            btnDelete.Enabled = true;
                        }

                            var gg = (from i in db.tb_Sell_FG_Details
                                 join s in db.tb_Sell_FGs on i.DocNo equals s.DocNo
                                 //join t in db.tb_Items on i.CodeNo equals t.CodeNo

                                 where i.Status != "Cancel"
                                     && i.DocNo.Contains(txtDocNo.Text.Trim())
                                     && i.CodeNo.Contains(txtCodeNo.Text.Trim())
                                     && i.id == dbClss.TInt(txtReftid.Text)
                                 select new
                                 {
                                     DocNo = i.DocNo,
                                     CodeNo = i.CodeNo,
                                     //ItemNo = t.ItemNo,
                                     //ItemDesc = t.ItemDescription,
                                     id = i.id,
                                     CSTM_Name = s.CSTM_Name,
                                     CSTM_Address = s.CSTM_Address,
                                     Qty = i.Qty,
                                     //Unit = t.UnitShip,
                                     Remark = i.Remark,
                                     Status = i.Status,
                                     DocBy = s.DocBy,
                                     DocNoDate = s.DocNoDate,
                                     UnitCost = i.UnitCost,
                                     Amount = i.Amount

                                 }
                           ).ToList();
                        if(gg.Count>0)
                        {
                            txtQty.Text = dbClss.TDe(gg.FirstOrDefault().Qty).ToString("N2");
                            txtUnitCost.Text = dbClss.TDe(gg.FirstOrDefault().UnitCost).ToString("N2");

                        }

                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : btnRefresh_Click", this.Name); }

        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (!lblStatus.Text.Equals("Completed"))
                {

                    Save_Delete();


                    btnNew_Click(null, null);
                    MessageBox.Show("บันทึกสำเร็จ!");
                    btnSave.Enabled = false;
                    btnNew.Enabled = true;
                    btnDelete.Enabled = false;
                    btnReturntoCustomer.Enabled = false;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : btnDelete_Click_1", this.Name); }

        }
        private void Save_Delete()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {

                int id = 0;
                var c = (from ix in db.tb_Claim_FG_Details
                              //join i in db.tb_Items on ix.CodeNo equals i.CodeNo
                          where ix.ClaimNo.Trim() == txtClaimNo.Text.Trim()
                          && ix.CodeNo.Trim() == txtCodeNo.Text.Trim()
                          && ix.Status != "Cancel"
                          select ix).ToList();
                if (c.Count > 0)
                {
                    id = dbClss.TInt(c.FirstOrDefault().id);
                }

                //----------บันทึก Refer Claim---------
                var f = (from i in db.tb_Sell_FG_Details
                             //join s in db.tb_Sell_FGs on i.DocNo equals s.DocNo
                             // join t in db.tb_Items on i.CodeNo equals t.CodeNo

                         where i.Status != "Cancel"
                             && i.DocNo == (txtDocNo.Text.Trim())
                             && i.CodeNo == (txtCodeNo.Text.Trim())
                             && i.id == dbClss.TInt(txtReftid.Text)
                         select new
                         {
                             id = i.id,
                             RefidClaim = i.RefidClaim,
                             RefidClaim2 = i.RefidClaim_2,
                             RefidClaim3 = i.RefidClaim_3,
                             RefidClaim4 = i.RefidClaim_4,
                             RefidClaim5 = i.RefidClaim_5
                         }
                   ).ToList();
                if (f.Count > 0)
                {
                    var ff = (from ix in db.tb_Sell_FG_Details
                              where ix.Status != "Cancel"
                             && ix.DocNo == (txtDocNo.Text.Trim())
                             && ix.CodeNo == (txtCodeNo.Text.Trim())
                             && ix.id == dbClss.TInt(txtReftid.Text)
                              select ix).First();

                    if (ff.RefidClaim != null && ff.RefidClaim != 0)
                        ff.RefidClaim = null;
                    else if (ff.RefidClaim_2 != null && ff.RefidClaim_2 != 0)
                        ff.RefidClaim_2 = null;
                    else if (ff.RefidClaim_3 != null && ff.RefidClaim_3 != 0)
                        ff.RefidClaim_3 = null;
                    else if (ff.RefidClaim_4 != null && ff.RefidClaim_4 != 0)
                        ff.RefidClaim_4 = null;
                    else if (ff.RefidClaim_5 != null && ff.RefidClaim_5 != 0)
                        ff.RefidClaim_5 = null;


                    dbClss.AddHistory(this.Name, "ลบรายการเคลม", "ลบรายการเคลมรับงานคืน เลขที่ Refer [" + txtJobCard_New.Text.Trim() + " TempJobCard : " + txtTempJobCard_New.Text + " จำนวนเคลม : " + txtQtyClaim.Text + "]", txtDocNo.Text);
                    dbClss.AddHistory(this.Name, "ลบรายการเคลม", "ลบรายการเคลมสินค้า เลขที่ใบขาย [" + txtDocNo.Text.Trim() + " CodeNo : " + txtCodeNo.Text + " จำนวนเคลม : " + txtQtyClaim.Text + "]", txtClaimNo.Text);

                    db.SubmitChanges();
                }


                var s1 = (from ix in db.tb_Claim_FGs
                              //join i in db.tb_Items on ix.CodeNo equals i.CodeNo
                          where ix.ClaimNo.Trim() == txtClaimNo.Text.Trim()
                          && ix.Status != "Cancel" && ix.Status != "Completed"
                          select ix).ToList();
                if (s1.Count > 0)
                {
                    foreach (var aa in s1)
                    {
                        aa.Status = "Cancel";

                        db.SubmitChanges();
                    }
                }

                var s3 = (from ix in db.tb_Claim_FG_ReferJobCards
                              //join i in db.tb_Items on ix.CodeNo equals i.CodeNo
                          where ix.ClaimNo.Trim() == txtClaimNo.Text.Trim()
                          //&& ix.CodeNo.Trim() == txtCodeNo.Text.Trim()
                          //&& ix.Status != "Cancel" && ix.Status != "Completed"
                          select ix).ToList();
                if (s3.Count > 0)
                {
                    foreach (var ab in s3)
                    {
                        ab.Status = "Cancel";
                        db.SubmitChanges();
                    }
                }

                var s2 = (from ix in db.tb_Claim_FG_Details
                              //join i in db.tb_Items on ix.CodeNo equals i.CodeNo
                          where ix.ClaimNo.Trim() == txtClaimNo.Text.Trim()
                          && ix.CodeNo.Trim() == txtCodeNo.Text.Trim()
                          && ix.Status != "Cancel" && ix.Status != "Completed"
                          select ix).ToList();
                if (s2.Count > 0)
                {
                    foreach (var aa in s2)
                    {
                        aa.Status = "Cancel";
                        aa.Return = false;
                        //aa.ReturnBy = ClassLib.Classlib.User;
                        //aa.ReturnDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                        db.SubmitChanges();

                        dbClss.AddHistory(this.Name, "ลบรายการเคลม", "ลบรายการเคลม [" + txtCodeNo.Text.Trim() + "]", txtClaimNo.Text);

                    }
                }

               

            }
        }

    }
}
