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

namespace StockControl
{
    public partial class ProductionOrder_Cancel : Telerik.WinControls.UI.RadRibbonForm
    {
        public ProductionOrder_Cancel()
        {
            InitializeComponent();
        }
        public ProductionOrder_Cancel(string tNo)
        {
            InitializeComponent();
            txtSPNo.Text = tNo;
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

        private void Unit_Load(object sender, EventArgs e)
        {
            // txtCNNo.Text = StockControl.dbClss.GetNo(6, 0);
            string tNo = txtSPNo.Text;
            ClearData();
            if (tNo != "")
            {
                txtSPNo.Text = tNo;
                DataLoad();
            }
        }

        void DataLoad()
        {
            using (var db = new DataClasses1DataContext())
            {
                string docNo = txtSPNo.Text.Trim();
                var m = db.mh_ProductionOrder_CancelQties.Where(x => x.DocNo == docNo).FirstOrDefault();
                if (m != null)
                {
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    var j = db.mh_ProductionOrders.Where(x => x.JobNo == m.JobNo).FirstOrDefault();
                    if (j != null)
                    {
                        txtJobNo.Text = j.JobNo;
                        txtFGNo.Text = j.FGNo;
                        txtFGName.Text = j.FGName;
                        txtQty.Text = m.Qty.ToSt();
                        txtOutQty.Text = j.OutQty.ToSt();
                        txtidCstmPODt.Text = m.idCstmPODt.ToSt();
                        txtSPNo.Text = m.DocNo;
                        txtSeqStatus.Text = m.SeqStatus.ToSt();
                        txtRemark.Text = m.Remark;
                        if (m.SeqStatus == 0)
                            lbStatus.Text = "Waiting";
                        else if (m.SeqStatus == 1)
                            lbStatus.Text = "Waiting Approve";
                        else
                            lbStatus.Text = "Approved";

                        if (!m.Active)
                            btnDelete.Enabled = false;
                        else
                            btnDelete.Enabled = true;
                    }
                }
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearData();
            //radGridView1.ReadOnly = false;
            //radGridView1.AllowAddNewRow = false;
            //radGridView1.Rows.AddNew();
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
                if (txtJobNo.Text == "")
                    err += "- กรุณาใส่ Job No.\n";
                using (var db = new DataClasses1DataContext())
                {
                    string jNo = txtJobNo.Text.Trim();
                    var m = db.mh_ProductionOrders.Where(x => x.JobNo == jNo).FirstOrDefault();
                    if (m == null)
                        err += "- ไม่พบ Job no.\n";
                    else
                    {
                        if (m.CloseJob)
                            err += "- สถานะของ Job ปิดแล้ว.\n";
                        else if (m.OutQty < txtQty.Value.ToDecimal())
                            err += "- จำนวนยกเลิก มากกว่าจำนวนคงเหลือ.\n";
                        else if (txtQty.Value.ToDecimal() <= 0)
                            err += "- กรุุณากรอกจำนวนที่ต้องการยกเลิก. \n";
                        else
                        {
                            var outQ = m.OutQty;
                            var canQ = txtQty.Value.ToDecimal();
                            //out Qty Waiting
                            var d = db.mh_ProductionOrder_CancelQties.Where(x => x.SeqStatus != 2 && x.Active && x.JobNo == jNo).ToList();
                            if (d.Count > 0)
                                canQ += d.Sum(x => x.Qty);
                            if (canQ > outQ)
                                err += "- จำนวนยกเลิก มากกว่าจำนวนคงเหลือ.\n";

                            var pk = db.mh_PackingDts.Where(x => x.Active && x.idJob == m.id)
                                .Join(db.mh_Packings.Where(x => x.Active)
                                , dt => dt.PackingNo
                                , hd => hd.PackingNo
                                , (dt, hd) => new { hd, dt }).ToList();
                            if (pk.Count > 0)
                                err += "- Job ถูกรับเข้าแล้ว.\n";
                        }
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

                if (MessageBox.Show("Do you want to Save ?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Cursor = Cursors.WaitCursor;
                    //txtSPNo.Text = StockControl.dbClss.GetNo(40, 2);
                    using (var db = new DataClasses1DataContext())
                    {
                        string spNo = txtSPNo.Text;
                        string jobNo = txtJobNo.Text.Trim();
                        var m = db.mh_ProductionOrder_CancelQties.Where(x => x.DocNo == spNo).FirstOrDefault();
                        var j = db.mh_ProductionOrders.Where(x => x.JobNo == jobNo).FirstOrDefault();
                        if (m == null && j != null)
                        {
                            m = new mh_ProductionOrder_CancelQty
                            {
                                Active = true,
                                CreateBy = ClassLib.Classlib.User,
                                CreateDate = DateTime.Now,
                                DocNo = dbClss.GetNo(43, 2),
                                idCstmPODt = txtidCstmPODt.Text.ToInt(),
                                JobNo = txtJobNo.Text.Trim(),
                                UpdateBy = ClassLib.Classlib.User,
                                UpdateDate = DateTime.Now,
                                Remark = txtRemark.Text,
                                DocDate = DateTime.Now,
                                PCSUnit = j.PCSUnit,
                                Qty = txtQty.Value.ToDecimal(),
                                SeqStatus = 0,
                                UOM = j.UOM,
                            };
                            db.mh_ProductionOrder_CancelQties.InsertOnSubmit(m);
                            db.SubmitChanges();

                            //j.OutQty -= m.Qty;
                            //j.CloseJob = true;
                            j.UpdateBy = ClassLib.Classlib.User;
                            j.UpdateDate = DateTime.Now;
                            db.SubmitChanges();

                            txtSPNo.Text = m.DocNo;
                        }
                    }

                    DataLoad();
                    MessageBox.Show("บันทึกสำเร็จ.\n");

                    //ClearData();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void ClearData()
        {
            txtJobNo.Text = "";
            txtFGNo.Text = "";
            txtFGName.Text = "";
            txtQty.Value = 0;
            txtidCstmPODt.Text = "";
            txtOutQty.Text = "";
            txtRemark.Text = "";
            btnNew.Enabled = true;
            btnSave.Enabled = true;
            btnDelete.Enabled = false;
            txtSeqStatus.Text = "";
            lbStatus.Text = "New";
            txtSPNo.Text = StockControl.dbClss.GetNo(43, 0);
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
            var pdlist = new ProductionOrder_List(3);
            pdlist.ShowDialog();
            if (pdlist.t_JobNo != "")
            {
                txtJobNo.Text = pdlist.t_JobNo;
                Loadjob();
            }
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


        private void btnPrint_Click(object sender, EventArgs e)
        {

        }

        private void btnListItem_Click(object sender, EventArgs e)
        {
            //ShippingCancelList a = new ShippingCancelList();
            //a.Show();
            var p = new ProductionOrder_CancelList(1);
            p.ShowDialog();
            if (p.retDoc != "")
            {
                ClearData();
                txtSPNo.Text = p.retDoc;
                DataLoad();
            }
        }

        private void txtJobNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Loadjob();
            }
        }
        void Loadjob()
        {
            using (var db = new DataClasses1DataContext())
            {
                string jobNo = txtJobNo.Text.Trim();
                txtFGName.Text = "";
                txtFGNo.Text = "";
                txtQty.Text = "";
                txtidCstmPODt.Text = "";
                txtOutQty.Text = "";
                //txtRemark.Text = "";
                var m = db.mh_ProductionOrders.Where(x => x.JobNo == jobNo && x.Active).FirstOrDefault();
                if (m != null)
                {
                    if (m.CloseJob)
                    {
                        baseClass.Warning("Job No. ถูกปิดไปแล้ว. \n");
                        return;
                    }
                    if (m.SeqStatus != 2)
                    {
                        baseClass.Warning("สถานะของ Job No. ยังไม่ Approve.\n");
                        return;
                    }

                    var pk = db.mh_PackingDts.Where(x => x.Active && x.idJob == m.id)
                        .Join(db.mh_Packings.Where(x => x.Active)
                        , dt => dt.PackingNo
                        , hd => hd.PackingNo
                        , (dt, hd) => new { hd, dt }).ToList();
                    if (pk.Count > 0)
                    {
                        baseClass.Warning("- Job ถูกรับเข้าแล้ว.\n");
                        return;
                    }

                    txtFGName.Text = m.FGName;
                    txtFGNo.Text = m.FGNo;
                    //txtQty.Text = m.Qty.ToSt();
                    txtOutQty.Text = m.OutQty.ToSt();
                    txtidCstmPODt.Text = m.RefDocId.ToSt();
                    txtQty.Focus();
                }
                else
                {
                    baseClass.Warning("ไม่พบ Job No.\n");
                }
            }
        }

        bool ChkDel()
        {
            bool retval = true;
            string mssg = "";

            if (txtSeqStatus.Text.ToInt() > 0)
                mssg += "- สถานะเอกสารไม่สามารถ ลบได้.\n";

            if (mssg != "")
            {
                retval = false;
                baseClass.Warning(mssg);
            }
            return retval;
        }
        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (ChkDel() && baseClass.Question("Do you want to 'Delete' ?"))
                DelDoc();
        }
        void DelDoc()
        {
            using (var db = new DataClasses1DataContext())
            {
                string docNo = txtSPNo.Text.Trim();
                var m = db.mh_ProductionOrder_CancelQties.Where(x => x.DocNo == docNo).FirstOrDefault();
                if (m != null)
                {
                    var j = db.mh_ProductionOrders.Where(x => x.JobNo == m.JobNo && x.Active && x.CloseJob).FirstOrDefault();
                    if (j != null)
                    {
                        m.Active = false;
                        m.UpdateDate = DateTime.Now;
                        m.UpdateBy = ClassLib.Classlib.User;

                        //j.Qty += j.Qty;
                        //j.CloseJob = false;
                        j.UpdateBy = ClassLib.Classlib.User;
                        j.UpdateDate = DateTime.Now;
                        db.SubmitChanges();
                        btnDelete.Enabled = false;
                        baseClass.Info("Delete completed.\n");
                    }
                }
            }
        }

        private void btnSendApprove_Click(object sender, EventArgs e)
        {
            if (txtSeqStatus.Text.ToInt() == 0)
            {
                using (var db = new DataClasses1DataContext())
                {
                    if (baseClass.IsSendApprove())
                    {
                        db.sp_062_mh_ApproveList_Add(txtSPNo.Text.Trim(), "Job Cancel", ClassLib.Classlib.User);
                        MessageBox.Show("Send complete.");
                        DataLoad();
                    }
                }
            }
            else
                baseClass.Warning("สถานะไม่สามารถส่ง Approve ได้.\n");
        }
    }
}
