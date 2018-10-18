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
            HistoryView hw = new HistoryView(this.Name, this.txtSPNo.Text);
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
            btnNew_Click(null, null);
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
                ClearData();
                var m = db.mh_ProductionOrder_CancelQties.Where(x => x.DocNo == docNo).FirstOrDefault();
                if (m != null)
                {
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnView.Enabled = true;
                    var j = db.mh_ProductionOrders.Where(x => x.JobNo == m.JobNo).FirstOrDefault();
                    if (j != null)
                    {
                        txtJobNo.Text = j.JobNo;
                        txtJobNo_TEMP.Text = j.JobNo;
                        txtFGNo.Text = j.FGNo;
                        txtFGName.Text = j.FGName;
                        txtQty.Text = m.Qty.ToSt();
                        txtJobQty.Text = m.JobQty.ToSt();
                        txtOutQty.Text = m.OutQty.ToSt();
                        txtUOM.Text = m.UOM;
                        txtidCstmPODt.Text = m.idCstmPODt.ToSt();
                        txtSPNo.Text = m.DocNo;
                        txtSeqStatus.Text = m.SeqStatus.ToSt();
                        txtRemark.Text = m.Remark;

                        cbR1.Checked = m.R1;
                        cbR2.Checked = m.R2;
                        cbR3.Checked = m.R3;
                        cbR4.Checked = m.R4;
                        cbR5.Checked = m.R5;
                        cbR6.Checked = m.R6;

                        txtM1.Text = m.M1;
                        txtM2.Text = m.M2;
                        txtM3.Text = m.M3;
                        txtM4.Text = m.M4;
                        txtM5.Text = m.M5;
                        txtM6.Text = m.M6;

                        btnView_Click(null, null);
                        if (!m.Active || txtSeqStatus.Text.ToInt() > 0)
                        {
                            btnEdit.Enabled = false;
                            btnDelete.Enabled = false;
                        }

                        if (!m.Active)
                            lbStatus.Text = "Inactive";
                        else if (m.SeqStatus == 0)
                            lbStatus.Text = "Waiting";
                        else if (m.SeqStatus == 1)
                            lbStatus.Text = "Waiting Approve";
                        else
                            lbStatus.Text = "Approved";
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
            btnEdit_Click(null, null);
            ClearData();
            //radGridView1.ReadOnly = false;
            //radGridView1.AllowAddNewRow = false;
            //radGridView1.Rows.AddNew();
            btnSave.Enabled = true;
            btnEdit.Enabled = false;
            btnView.Enabled = false;
            btnDelete.Enabled = false;

            lbStatus.Text = "New";
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

            txtJobNo.ReadOnly = false;
            txtQty.ReadOnly = false;
            txtRemark.ReadOnly = false;

            btnSave.Enabled = true;
            btnEdit.Enabled = false;
            btnView.Enabled = true;
            btnDelete.Enabled = true;

            cbR1.ReadOnly = false;
            cbR2.ReadOnly = false;
            cbR3.ReadOnly = false;
            cbR4.ReadOnly = false;
            cbR5.ReadOnly = false;
            cbR6.ReadOnly = false;
            txtR6.ReadOnly = false;

            txtM1.ReadOnly = false;
            txtM2.ReadOnly = false;
            txtM3.ReadOnly = false;
            txtM4.ReadOnly = false;
            txtM5.ReadOnly = false;
            txtM6.ReadOnly = false;

            lbStatus.Text = "Edit";
        }
        private void btnView_Click(object sender, EventArgs e)
        {
            txtJobNo.ReadOnly = true;
            txtQty.ReadOnly = true;
            txtRemark.ReadOnly = true;

            btnSave.Enabled = false;
            btnEdit.Enabled = true;
            btnView.Enabled = false;
            btnDelete.Enabled = true;

            cbR1.ReadOnly = true;
            cbR2.ReadOnly = true;
            cbR3.ReadOnly = true;
            cbR4.ReadOnly = true;
            cbR5.ReadOnly = true;
            cbR6.ReadOnly = true;
            txtR6.ReadOnly = true;

            txtM1.ReadOnly = true;
            txtM2.ReadOnly = true;
            txtM3.ReadOnly = true;
            txtM4.ReadOnly = true;
            txtM5.ReadOnly = true;
            txtM6.ReadOnly = true;

            lbStatus.Text = "View";
        }



        private bool Check_Save()
        {
            bool re = true;
            string err = "";
            try
            {
                if (txtSeqStatus.Text.ToInt() > 0)
                    err += "- สถานะไม่สามารถบันทึกได้.\n";
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
                        else if (txtQty.Value.ToDecimal() <= 0)
                            err += "- กรุุณากรอกจำนวนที่ต้องการ (Change Q'ty). \n";
                        else
                        {
                            var rc = db.mh_PackingDts.Where(x => x.idJob == m.id && x.Active)
                                .Join(db.mh_Packings.Where(x => x.Active)
                                , dt => dt.PackingNo
                                , hd => hd.PackingNo
                                , (dt, hd) => new { dt.Qty }).ToList();
                            var rcQ = 0.00m;
                            if (rc.Count > 0)
                                rcQ = rc.Sum(x => x.Qty);
                            var jobQ = txtJobQty.Text.ToDecimal();
                            var changeQ = txtQty.Value.ToDecimal();
                            if (changeQ < rcQ)
                                err += "- จำนวนที่ต้องการเปลี่ยนแปลง มีค่าน้อยกว่า จำนวนที่ถูกรับเข้าไปแล้ว.\n";

                            //var outQ = m.OutQty;
                            //var canQ = txtQty.Value.ToDecimal();
                            ////out Qty Waiting
                            //var d = db.mh_ProductionOrder_CancelQties.Where(x => x.SeqStatus != 2 && x.Active && x.JobNo == jNo).ToList();
                            //if (d.Count > 0)
                            //    canQ += d.Sum(x => x.Qty);
                            //if (canQ > outQ)
                            //    err += "- จำนวนยกเลิก มากกว่าจำนวนคงเหลือ.\n";
                            //else
                            //{
                            //    if (outQ - canQ <= 0) //จะปิด job
                            //    {
                            //        var rm = db.mh_ProductionOrderRMs.Where(x => x.Active && x.JobNo == jNo && x.OutQty < 0).ToList();
                            //        foreach (var r in rm)
                            //        {
                            //            if (r.OutQty + baseClass.GetQtyAccidenSlip(r.id) < 0)
                            //            {
                            //                err += $"- จำนวนเบิกใช้ 'วัตถุดิบ' เกินกำหนด กรุณาทำ Return RM หรือ Accident Slip กรณีไม่สามารถคืนวัตถุดิบได้ \n";
                            //                break;
                            //            }
                            //        }
                            //    }
                            //}
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
                        if (j == null)
                        {
                            baseClass.Error("- ไม่พบใบสั่งผลิต (Job Order Sheet No.).\n");
                            return;
                        }
                        //
                        bool newDoc = false;
                        if (m == null)
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
                                JobQty = txtJobQty.Text.ToDecimal(),
                                OutQty = txtOutQty.Text.ToDecimal(),
                                CutFG = false,
                                R1 = cbR1.Checked,
                                R2 = cbR2.Checked,
                                R3 = cbR3.Checked,
                                R4 = cbR4.Checked,
                                R5 = cbR5.Checked,
                                R6 = cbR6.Checked,
                                M1 = (cbR1.Checked) ? txtM1.Text : "",
                                M2 = (cbR2.Checked) ? txtM2.Text : "",
                                M3 = (cbR3.Checked) ? txtM3.Text : "",
                                M4 = (cbR4.Checked) ? txtM4.Text : "",
                                M5 = (cbR5.Checked) ? txtM5.Text : "",
                                M6 = (cbR6.Checked) ? txtM6.Text : "",
                                R6_Text = (cbR6.Checked) ? txtR6.Text : "",
                            };
                            newDoc = true;
                            db.mh_ProductionOrder_CancelQties.InsertOnSubmit(m);

                            AddHistory($"New Change Job Document", m.DocNo);
                        }
                        else
                        {
                            if (m.JobNo != txtJobNo.Text)
                            {
                                AddHistory($"Job Order No From {m.JobNo} to {txtJobNo.Text}", m.DocNo);
                                newDoc = true;
                            }
                            if (m.Qty != txtQty.Value.ToDecimal())
                            {
                                AddHistory($"Change Q'ty from {m.Qty} to {txtQty.Value.ToDecimal()}", m.DocNo);
                                newDoc = true;
                            }

                            m.JobNo = txtJobNo.Text;
                            m.Qty = txtQty.Value.ToDecimal();
                            m.R1 = cbR1.Checked;
                            m.R2 = cbR2.Checked;
                            m.R3 = cbR3.Checked;
                            m.R4 = cbR4.Checked;
                            m.R5 = cbR5.Checked;
                            m.R6 = cbR6.Checked;
                            m.M1 = (cbR1.Checked) ? txtM1.Text : "";
                            m.M2 = (cbR2.Checked) ? txtM2.Text : "";
                            m.M3 = (cbR3.Checked) ? txtM3.Text : "";
                            m.M4 = (cbR4.Checked) ? txtM4.Text : "";
                            m.M5 = (cbR5.Checked) ? txtM5.Text : "";
                            m.M6 = (cbR6.Checked) ? txtM6.Text : "";
                            m.Remark = txtRemark.Text;
                            m.R6_Text = (cbR6.Checked) ? txtR6.Text : "";
                        }

                        ////find new OutQty for save
                        //var fgQty = j.Qty;
                        //var outQty = j.OutQty;
                        if (newDoc)
                        {
                            var newQ = txtQty.Value.ToDecimal();
                            var recQ = 0.00m;

                            var rec = db.mh_PackingDts.Where(x => x.idJob == j.id && x.Active)
                                .Join(db.mh_Packings.Where(x => x.Active)
                                , dt => dt.PackingNo
                                , hd => hd.PackingNo
                                , (dt, hd) => new { hd, dt }).ToList();
                            if (rec.Count > 0) recQ = rec.Sum(x => x.dt.Qty);
                            m.OutQty = newQ - recQ;
                        }
                        

                        db.SubmitChanges();
                        txtSPNo.Text = m.DocNo;
                    }

                    DataLoad();
                    MessageBox.Show("บันทึกสำเร็จ.\n");

                    //ClearData();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }

        void AddHistory(string Detail, string DocNo)
        {
            dbClss.AddHistory(this.Name, "Change Job", Detail, DocNo);

        }

        private void ClearData()
        {
            txtJobNo.Text = "";
            txtJobNo_TEMP.Text = "";
            txtFGNo.Text = "";
            txtFGName.Text = "";
            txtQty.Value = 0;
            txtidCstmPODt.Text = "";
            txtUOM.Text = "";
            txtJobQty.Text = "";
            txtOutQty.Text = "";
            txtRemark.Text = "";
            btnNew.Enabled = true;
            btnSave.Enabled = true;
            btnDelete.Enabled = false;
            txtSeqStatus.Text = "";
            //lbStatus.Text = "New";

            cbR1.Checked = false;
            cbR2.Checked = false;
            cbR3.Checked = false;
            cbR4.Checked = false;
            cbR5.Checked = false;
            cbR6.Checked = false;
            txtR6.Text = "";
            txtM1.Text = "";
            txtM3.Text = "";
            txtM2.Text = "";
            txtM4.Text = "";
            txtM5.Text = "";
            txtM6.Text = "";

            cbR1_ToggleStateChanged(null, null);

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
                forLoadJob(pdlist.t_JobNo);
            }
        }
        void forLoadJob(string t_JobNo)
        {
            if (txtJobNo_TEMP.Text != t_JobNo)
            {
                txtJobNo.Text = t_JobNo;
                Loadjob();
            }
            else
            {
                string spNo = txtSPNo.Text;
                ClearData();
                txtSPNo.Text = spNo;
                DataLoad();
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
            //ReportChangeJob.rpt
            string SPNo = txtSPNo.Text.Trim();
            if (SPNo != "")
            {
                Report.Reportx1.Value = new string[1];
                Report.Reportx1.Value[0] = SPNo;
                Report.Reportx1.WReport = "ReportChangeJob";
                Report.Reportx1 op = new Report.Reportx1("ReportChangeJob.rpt");
                op.Show();
            }
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
                forLoadJob(txtJobNo.Text.Trim());
            }
        }
        void Loadjob()
        {
            using (var db = new DataClasses1DataContext())
            {
                string jobNo = txtJobNo.Text.Trim();
                txtJobNo.Text = "";
                txtFGNo.Text = "";
                txtFGName.Text = "";
                txtJobQty.Text = "0.00";
                txtUOM.Text = "";
                txtOutQty.Text = "0.00";
                txtQty.Value = 0;
                txtidCstmPODt.Text = "";
                //txtJobNo_TEMP.Text = "";
                var m = db.mh_ProductionOrders.Where(x => x.JobNo == jobNo && x.Active).FirstOrDefault();
                if (m != null)
                {
                    if (m.CloseJob)
                    {
                        baseClass.Warning("Job No. ถูกปิดไปแล้ว. \n");
                        txtJobNo.Text = "";
                        //txtJobNo_TEMP.Text = "";
                        return;
                    }
                    if (m.SeqStatus != 2)
                    {
                        baseClass.Warning("สถานะของ Job No. ยังไม่ Approved.\n");
                        txtJobNo.Text = "";
                        //txtJobNo_TEMP.Text = "";
                        return;
                    }

                    //var pk = db.mh_PackingDts.Where(x => x.Active && x.idJob == m.id)
                    //    .Join(db.mh_Packings.Where(x => x.Active)
                    //    , dt => dt.PackingNo
                    //    , hd => hd.PackingNo
                    //    , (dt, hd) => new { hd, dt }).ToList();
                    //if (pk.Count > 0)
                    //{
                    //    baseClass.Warning("- Job ถูกรับเข้าแล้ว.\n");
                    //    txtJobNo.Text = "";
                    //    return;
                    //}

                    txtJobNo.Text = jobNo;
                    //txtJobNo_TEMP.Text = txtJobNo.Text;
                    txtFGName.Text = m.FGName;
                    txtFGNo.Text = m.FGNo;
                    txtidCstmPODt.Text = m.RefDocId.ToSt();
                    txtUOM.Text = m.UOM;
                    txtJobQty.Text = m.Qty.ToString("0.00");
                    txtOutQty.Text = m.OutQty.ToSt();
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
                    var j = db.mh_ProductionOrders.Where(x => x.JobNo == m.JobNo && x.Active && !x.CloseJob).FirstOrDefault();
                    if (j != null)
                    {
                        m.Active = false;
                        m.UpdateDate = DateTime.Now;
                        m.UpdateBy = ClassLib.Classlib.User;

                        AddHistory("Delete Document", m.DocNo);

                        db.SubmitChanges();
                        btnDelete.Enabled = false;
                        baseClass.Info("Delete completed.\n");
                        lbStatus.Text = "Inactive";
                    }
                    else
                        baseClass.Warning("- ไม่พบเอกสารใบสั่งผลิต (Job Order Sheet).\n");
                }
            }
        }

        private void btnSendApprove_Click(object sender, EventArgs e)
        {
            if (txtSeqStatus.Text.ToInt() <= 1)
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

        private void radLabel4_Click(object sender, EventArgs e)
        {

        }

        private void cbR1_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            txtM1.Enabled = cbR1.Checked;
            txtM2.Enabled = cbR2.Checked;
            txtM3.Enabled = cbR3.Checked;
            txtM4.Enabled = cbR4.Checked;
            txtM5.Enabled = cbR5.Checked;
            txtM6.Enabled = cbR6.Checked;
            txtR6.Enabled = cbR6.Checked;
        }

    }
}
