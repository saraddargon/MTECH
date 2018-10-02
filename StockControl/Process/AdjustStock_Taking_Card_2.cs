using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;
using ClassLib;
using Microsoft.VisualBasic;

namespace StockControl
{
    public partial class AdjustStock_Taking_Card_2 : Telerik.WinControls.UI.RadRibbonForm
    {
        public AdjustStock_Taking_Card_2(string CodeNox)
        {
            InitializeComponent();
            CodeNo = CodeNox;
            //this.Text = "ประวัติ "+ Screen;
        }
        Telerik.WinControls.UI.RadTextBox CodeNo_tt = new Telerik.WinControls.UI.RadTextBox();
        int screen = 0;
        public AdjustStock_Taking_Card_2(Telerik.WinControls.UI.RadTextBox  CodeNox)
        {
            InitializeComponent();
            CodeNo_tt = CodeNox;
            screen = 1;
        }
        public AdjustStock_Taking_Card_2()
        {
            InitializeComponent();
        }

        string CodeNo = "";
        //private int RowView = 50;
        //private int ColView = 10;
        //DataTable dt = new DataTable();
        private void radMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }
        private void GETDTRow()
        {
            //dt.Columns.Add(new DataColumn("UnitCode", typeof(string)));
            //dt.Columns.Add(new DataColumn("UnitDetail", typeof(string)));
            //dt.Columns.Add(new DataColumn("UnitActive", typeof(bool)));
        }
        private void Unit_Load(object sender, EventArgs e)
        {
           
        }
       
        private void btnCancel_Click(object sender, EventArgs e)
        {
          
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            lblStatus.Visible = false;
            txtListNo.Text = "";
            txtListNo1.Text = "";
            btnCheckData.Enabled = true;
            btnCompareData.Enabled = true;
            btnGetList.Enabled = true;
            btnGetPrevious.Enabled = true;
            btnInput.Enabled = true;
            btnCompareData.Enabled = true;
            btnCompleted.Enabled = true;
            btnPrintReport.Enabled = true;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
           
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
           
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {

        }

        private void Unit_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // MessageBox.Show(e.KeyCode.ToString());
        }

        private void radGridView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

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
            ////  dbClss.ExportGridCSV(radGridView1);
            //dbClss.ExportGridXlSX(radGridView1);
        }



        private void btnFilter1_Click(object sender, EventArgs e)
        {
            //radGridView1.EnableFiltering = true;
        }

        private void btnUnfilter1_Click(object sender, EventArgs e)
        {
            //radGridView1.EnableFiltering = false;
        }

        private void radMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radButtonElement1_Click(object sender, EventArgs e)
        {
           
        }
        private void Update_Status(string Type, string DocNo)
        {
          
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
        }

        private void radGridView1_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
          
            
        }
       
       
        private void btn_Print_Barcode_Click(object sender, EventArgs e)
        {
           
        }

        private void btn_PrintPR_Click(object sender, EventArgs e)
        {
            //try
            //{
                
            //    //dt_ShelfTag.Rows.Clear();
            //    string PRNo = "";
            //    if(radGridView1.Rows.Count > 0)
            //        PRNo = StockControl.dbClss.TSt(radGridView1.CurrentRow.Cells["PRNo"].Value);

            //    PrintPR a = new PrintPR(PRNo, PRNo,"PR");
            //    a.ShowDialog();


            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void radButtonElement2_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    string PRNo = "";
            //    if (radGridView1.Rows.Count > 0)
            //        PRNo = StockControl.dbClss.TSt(radGridView1.CurrentRow.Cells["PRNo"].Value);

            //    PrintPR a = new PrintPR(PRNo, PRNo, "PR");
            //    a.ShowDialog();
            //}
            //catch { }
        }

        private void frezzRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (radGridView1.Rows.Count > 0)
            //    {

            //        int Row = 0;
            //        Row = radGridView1.CurrentRow.Index;
            //        dbClss.Set_Freeze_Row(radGridView1, Row);

            //        //foreach (var rd in radGridView1.Rows)
            //        //{
            //        //    if (rd.Index <= Row)
            //        //    {
            //        //        radGridView1.Rows[rd.Index].PinPosition = PinnedRowPosition.Top;
            //        //    }
            //        //    else
            //        //        break;
            //        //}
            //    }
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void frezzColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (radGridView1.Columns.Count > 0)
            //    {
            //        int Col = 0;
            //        Col = radGridView1.CurrentColumn.Index;
            //        dbClss.Set_Freeze_Column(radGridView1, Col);

            //        //foreach (var rd in radGridView1.Columns)
            //        //{
            //        //    if (rd.Index <= Col)
            //        //    {
            //        //        radGridView1.Columns[rd.Index].PinPosition = PinnedColumnPosition.Left;
            //        //    }
            //        //    else
            //        //        break;
            //        //}
            //    }
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void unFrezzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{

            //    dbClss.Set_Freeze_UnColumn(radGridView1);
            //    dbClss.Set_Freeze_UnRows(radGridView1);
            //    //foreach (var rd in radGridView1.Rows)
            //    //{
            //    //    radGridView1.Rows[rd.Index].IsPinned = false;
            //    //}
            //    //foreach (var rd in radGridView1.Columns)
            //    //{
            //    //    radGridView1.Columns[rd.Index].IsPinned = false;                   
            //    //}

            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void radButtonElement2_Click_1(object sender, EventArgs e)
        {
            //this.Cursor = Cursors.WaitCursor;
            //radGridView1.EndEdit();
            //if (baseClass.IsReject())
            //{
            //    using (var db = new DataClasses1DataContext())
            //    {
            //        int id = 0;
            //        string DocNo = "";
            //        string Type = "";
            //        int Seq = 0;
            //        List<string> AppList = new List<string>();

            //        string GetMarkup = Interaction.InputBox("Reason For Reject", "Reason : ", "", 400, 250);
            //        if (!GetMarkup.Trim().Equals(""))
            //        {
            //            foreach (var rd in radGridView1.Rows)
            //            {
            //                if (dbClss.TBo(rd.Cells["S"].Value))
            //                {
            //                    DocNo = dbClss.TSt(rd.Cells["ApproveDocuNo"].Value);
            //                    Type = dbClss.TSt(rd.Cells["ApproveType"].Value);
            //                    id = dbClss.TInt(rd.Cells["id"].Value);
            //                    if ((dbClss.TSt(rd.Cells["Status"].Value) == "Waiting") || (dbClss.TSt(rd.Cells["Status"].Value) == "Completed") || (dbClss.TSt(rd.Cells["Status"].Value) == "Approved"))
            //                        db.sp_064_mh_ApproveList_Update2(id, DocNo, Type, ClassLib.Classlib.User, GetMarkup, "Reject");
            //                }

            //            }
            //            DataLoad();
            //            baseClass.Info("Reject complete.");
            //        }
            //    }
            //}

            //this.Cursor = Cursors.Default;
            //ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            //ClassLib.Memory.Heap();
        }

        private void btnCheckData_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new DataClasses1DataContext())
                {

                    int Temp = 0;
                    var ck = db.mh_CheckStock_Lists.Where(x => x.CheckStatus == "Waiting").ToList();
                    if (ck.Count > 0)
                    {
                        MessageBox.Show("มี ListNo ค้างเลขที่ : " + dbClss.TSt(ck.FirstOrDefault().CheckNo));
                        //lblStatus.Text = dbClss.TSt(ck.FirstOrDefault().CheckStatus);

                        txtListNo.Text = dbClss.TSt(ck.FirstOrDefault().CheckNo);
                        txtListNo1.Text = dbClss.TSt(ck.FirstOrDefault().CheckNo);
                        btnCheckData.Enabled = false;
                        btnGetList.Enabled = false;
                    }
                    else
                    {
                        txtListNo.Text = "";
                        txtListNo1.Text = "";
                        btnCheckData.Enabled = true;
                        btnGetList.Enabled = true;
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnGetList_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtListNo.Text != "")
                    return;

                using (var db = new DataClasses1DataContext())
                {
                    if (MessageBox.Show("ต้องการสร้าง List Check Stock หรือไม่? \n ", "ListStock", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        int Temp = 0;
                        var ck = db.mh_CheckStock_Lists.Where(x => x.CheckStatus == "Waiting").ToList();
                        if (ck.Count > 0)
                        {
                            if (MessageBox.Show("รายการเช็คสต็อกบางรายการยังไม่ถูกปิด ต้องการปิดและสร้างรายการเช็คสต็อกใหม่หรือไม่ ?", "Check Stock", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                Temp = 0;
                                var g = (from ix in db.mh_CheckStock_Lists select ix)
                               .Where(a => a.CheckStatus == "Waiting").OrderByDescending(ab => ab.id).ToList();
                                if (g.Count > 0)
                                {
                                    foreach (var ss in g)
                                    {
                                        ss.CheckStatus = "Cancel";
                                        db.SubmitChanges();
                                    }
                                }
                            }
                            else
                            {
                                Temp = 1;
                                return;
                            }
                        }
                        if (Temp == 0)
                        {
                            db.sp_074_CheckStock_List(ClassLib.Classlib.User);
                            MessageBox.Show("สร้างรายการเช็คสต็อกสำเร็จ.");

                            var g = (from ix in db.mh_CheckStock_Lists select ix)
                                .Where(a => a.CheckStatus == "Waiting").OrderByDescending(ab => ab.id).ToList();
                            if (g.Count() > 0)
                            {
                                txtListNo.Text = dbClss.TSt(g.FirstOrDefault().CheckNo);
                                txtListNo1.Text = dbClss.TSt(g.FirstOrDefault().CheckNo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnGetPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                  

                    this.Cursor = Cursors.WaitCursor;
                    AdjustStock_Taking_List sc = new AdjustStock_Taking_List(txtListNo);
                    this.Cursor = Cursors.Default;
                    sc.ShowDialog();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                    ClassLib.Memory.Heap();
                    //LoadData                    
                    string ADNo = txtListNo.Text;
                    if (!txtListNo.Text.Equals(""))
                    {
                        txtListNo1.Text = txtListNo.Text;
                        //using (var db = new DataClasses1DataContext())
                        //{
                        //    var ck = db.mh_CheckStock_Lists.Where(x => x.CheckStatus == "Waiting").ToList();
                        //    if (ck.Count > 0)
                        //    {

                        //    }
                        //}
                    }
                    else
                    {
                        btnNew_Click(null, null);
                    }

                }
                catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : radButtonElement1_Click", this.Name); }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnInput_Click(object sender, EventArgs e)
        {
            AdjustStock_Taking a = new AdjustStock_Taking(txtListNo1.Text);
            a.ShowDialog();
        }

        private void btnCompleted_Click(object sender, EventArgs e)
        {

            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {

                var c = (from ix in db.mh_CheckStock_Lists select ix)
                        .Where(a => a.CheckStatus == "Waiting"
                        //&& a.SeqStatus == 2
                        && a.CheckNo.Trim().ToUpper() == txtListNo1.Text.Trim().ToUpper()).ToList();
                if (c.Count > 0)
                {
                    if (MessageBox.Show("คุณต้องทำ Set ข้อมุล ListNo นี้ให้ Completed ใช่หรือไม่? \n ", "Completed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        foreach (var ss in c)
                        {
                            ss.CheckStatus = "Completed";
                            db.SubmitChanges();
                        }
                        MessageBox.Show("completed.");
                        btnNew_Click(null, null);
                    }
                }
                else
                    MessageBox.Show("can't update.");
            }
            
        }

        private void btnCompareData_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("ต้องการเปรียบเทีบบสต็อกที่ทำการนับมากับระบบ ใช่หรือไม่?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var c = (from ix in db.mh_CheckStock_Lists select ix)
                     .Where(a => a.CheckStatus == "Waiting" 
                     &&  a.CheckNo.Trim().ToUpper() == txtListNo1.Text.Trim().ToUpper() 
                     && Convert.ToBoolean(a.InputOK) == false).ToList();
                    if (c.Count > 0)
                    {
                        foreach (var ss in c)
                        {
                            //ss.InputQty = StockControl.dbClss.TDe(g.Cells["QTYTrue"].Value);
                            //ss.CompareQty = StockControl.dbClss.TDe(g.Cells["CompareQty"].Value);
                            //ss.Cost = StandardCost;//StockControl.dbClss.TDe(g.Cells["StandardCost"].Value);
                            //ss.Amount = StockControl.dbClss.TDe(g.Cells["Amount"].Value);
                            //ss.Remark = dbClss.TSt(g.Cells["Remark"].Value);
                            //ss.Type = dbClss.TSt(g.Cells["Type"].Value);
                            //ss.UOM = StockControl.dbClss.TSt(g.Cells["Unit"].Value);
                            //ss.PCSUnit = StockControl.dbClss.TDe(g.Cells["PCSUnit"].Value);
                            ////ss.CheckStatus = "Completed";
                            ss.InputOK = true;
                            db.SubmitChanges();
                        }
                    }
                }
            }
        }

        private void btnUpdateStockTaking_Click(object sender, EventArgs e)
        {

            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                var c = (from ix in db.mh_CheckStock_Lists select ix)
                 .Where(a => a.CheckStatus == "Waiting"
                 && a.SeqStatus == 2
                 && Convert.ToString(a.ApproveBy) != ""
                 && a.CheckNo.Trim().ToUpper() == txtListNo1.Text.Trim().ToUpper()
                 && Convert.ToBoolean(a.InputOK) == true).ToList();
                if (c.Count > 0)
                {
                    if (MessageBox.Show("ต้องการอัพเดตสต็อก ใช่หรือไม่?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        bool re = SaveHerder();
                        if (re)
                        {
                            SaveDetail();
                            MessageBox.Show("update stock complete.");
                        }
                    }
                }
                else
                    MessageBox.Show("can't update.");

            }
        }
        private bool SaveHerder()
        {
            bool re = false;
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                var g = (from ix in db.tb_StockAdjustHs
                         where ix.ADNo.Trim() == txtListNo1.Text.Trim() && ix.Status != "Cancel"
                         //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                         select ix).ToList();
                if (g.Count <= 0)  //มีรายการในระบบ
                //{
                //    foreach (DataRow row in dt_ADH.Rows)
                //    {
                //        var gg = (from ix in db.tb_StockAdjustHs
                //                  where ix.ADNo.Trim() == txtADNo.Text.Trim() && ix.Status != "Cancel"
                //                  //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                //                  select ix).First();

                //        gg.UpdateBy = ClassLib.Classlib.User;
                //        gg.UpdateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                //        dbClss.AddHistory(this.Name, "ปรับสต็อก", "แก้ไขโดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", txtADNo.Text);

                //        if (StockControl.dbClss.TSt(gg.BarCode).Equals(""))
                //            gg.BarCode = StockControl.dbClss.SaveQRCode2D(txtADNo.Text.Trim());


                //        if (!txtAdjustBy.Text.Trim().Equals(row["ADBy"].ToString()))
                //        {
                //            gg.ADBy = txtAdjustBy.Text.Trim();
                //            dbClss.AddHistory(this.Name, "ปรับสต็อก", "แก้ไข ผู้ร้องขอ[" + txtAdjustBy.Text.Trim() + " เดิม :" + row["ADBy"].ToString() + "]", txtADNo.Text);
                //        }
                //        if (!txtRemark.Text.Trim().Equals(row["Remark"].ToString()))
                //        {
                //            gg.Remark = txtRemark.Text.Trim();
                //            dbClss.AddHistory(this.Name, "ปรับสต็อก", "แก้ไขหมายเหตุ [" + txtRemark.Text.Trim() + " เดิม :" + row["Remark"].ToString() + "]", txtADNo.Text);
                //        }


                //        if (!dtRequire.Text.Trim().Equals(""))
                //        {
                //            string date1 = "";
                //            date1 = dtRequire.Value.ToString("yyyyMMdd", new CultureInfo("en-US"));
                //            string date2 = "";
                //            DateTime temp = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                //            if (!StockControl.dbClss.TSt(row["ADDate"].ToString()).Equals(""))
                //            {

                //                temp = Convert.ToDateTime(row["ADDate"]);
                //                date2 = temp.ToString("yyyyMMdd", new CultureInfo("en-US"));

                //            }
                //            if (!date1.Equals(date2))
                //            {
                //                DateTime? RequireDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                //                if (!dtRequire.Text.Equals(""))
                //                    RequireDate = dtRequire.Value;
                //                gg.ADDate = RequireDate;
                //                dbClss.AddHistory(this.Name, "ปรับสต็อก", "แก้ไขวันที่ปรับสต็อกสินค้า [" + dtRequire.Text.Trim() + " เดิม :" + temp.ToString() + "]", txtADNo.Text);
                //            }
                //        }
                //        db.SubmitChanges();
                //    }
                //}
                //else //สร้างใหม่
                {
                    var c = (from ix in db.mh_CheckStock_Lists select ix)
                    .Where(a => a.CheckStatus == "Waiting"
                    && a.SeqStatus == 2
                    && Convert.ToString(a.ApproveBy) != ""
                    && a.CheckNo.Trim().ToUpper() == txtListNo1.Text.Trim().ToUpper()
                    && Convert.ToBoolean(a.InputOK) == true).ToList();
                    if (c.Count > 0)
                    {
                        re = true;

                        byte[] barcode = null;
                        barcode = StockControl.dbClss.SaveQRCode2D(c.FirstOrDefault().CheckNo.Trim());
                        DateTime? UpdateDate = null;

                        DateTime? RequireDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                        if (!c.FirstOrDefault().CheckDate.ToSt().Equals(""))
                            RequireDate = Convert.ToDateTime(c.FirstOrDefault().CheckDate);

                        tb_StockAdjustH gg = new tb_StockAdjustH();
                        gg.ADNo = c.FirstOrDefault().CheckNo.Trim();
                        gg.ADBy = ClassLib.Classlib.User;
                        gg.ADDate = RequireDate;
                        gg.UpdateBy = null;
                        gg.UpdateDate = UpdateDate;
                        gg.CreateBy = ClassLib.Classlib.User;
                        gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                        gg.Remark = "";
                        gg.BarCode = barcode;
                        gg.Status = "Completed";
                        db.tb_StockAdjustHs.InsertOnSubmit(gg);
                        db.SubmitChanges();

                        dbClss.AddHistory(this.Name, "ปรับสต็อก", "สร้าง การปรับสต็อกสินค้า [" + c.FirstOrDefault().CheckNo.Trim() + "]", c.FirstOrDefault().CheckNo.Trim());
                    }
                }
            }
            return re;
        }
        private void SaveDetail()
        {


            string ADNo = txtListNo1.Text.Trim().ToUpper();
            DateTime? RequireDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
            //if (!dtRequire.Text.Equals(""))
            //    RequireDate = dtRequire.Value;

            int Seq = 0;
            DateTime? UpdateDate = null;
            string LotNo = "";

            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                string SS = "";
                int id = 0;

                DateTime? CalDate = null;
                DateTime? AppDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
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
                Category = "Adjust";
                Type = "Adjust";
                Flag_ClearTemp = 5;
                decimal Qty = 0;
                string Location = "";
                string ShelfNo = "";
                var c = (from ix in db.mh_CheckStock_Lists select ix)
                     .Where(a => a.CheckStatus == "Waiting"
                     && a.SeqStatus == 2
                     && Convert.ToString(a.ApproveBy) != ""
                     && a.CheckNo.Trim().ToUpper() == ADNo.Trim().ToUpper()
                     && Convert.ToBoolean(a.InputOK) == true).ToList();
                if (c.Count > 0)
                {
                    foreach (var ss in c)
                    {
                        id = StockControl.dbClss.TInt(ss.id);

                        if (StockControl.dbClss.TDe(ss.InputQty) != (0)) // เอาเฉพาะรายการที่ไม่เป็น 0 
                        {
                            //if (StockControl.dbClss.TInt(g.Cells["id"].Value) <= 0)  //New ใหม่
                            //{

                            LotNo = dbClss.Get_Lot(DateTime.Now.ToString("yyyyMMdd"));
                            //StockControl.dbClss.TSt(g.Cells["LotNo"].Value);
                            //if (LotNo == "")
                            //{                                                                                                                                    
                            //LotNo = dbClss.Get_Lot(DateTime.Now.ToString("yyyyMMdd"));
                            //    }
                            Seq += 1;
                            tb_StockAdjust u = new tb_StockAdjust();
                            u.AdjustNo = ADNo;

                            u.CodeNo = StockControl.dbClss.TSt(ss.InternalNo);
                            var i = (from ix in db.mh_Items
                                     where ix.InternalNo.Trim() == StockControl.dbClss.TSt(ss.InternalNo).Trim() && ix.Active == true
                                     select ix).ToList();
                            if (i.Count > 0)
                            {
                                u.ItemNo = StockControl.dbClss.TSt(i.FirstOrDefault().InternalName);
                                u.ItemDescription = StockControl.dbClss.TSt(i.FirstOrDefault().InternalDescription);
                                u.Location = StockControl.dbClss.TSt(i.FirstOrDefault().Location);
                                Location = StockControl.dbClss.TSt(i.FirstOrDefault().Location);
                                u.ShelfNo = StockControl.dbClss.TSt(i.FirstOrDefault().ShelfNo);
                                ShelfNo = StockControl.dbClss.TSt(i.FirstOrDefault().ShelfNo);
                            }
                            u.Qty = StockControl.dbClss.TDe(ss.InputQty);
                            u.StandardCost = StockControl.dbClss.TDe(ss.Cost);
                            u.PCSUnit = StockControl.dbClss.TDe(ss.PCSUnit);
                            u.Unit = StockControl.dbClss.TSt(ss.UOM);
                            u.Amount = StockControl.dbClss.TDe(ss.Amount);
                            u.Reason = StockControl.dbClss.TSt(ss.Remark);
                            u.LotNo = LotNo;
                            u.StockType = "Taking";
                            u.Seq = Seq;
                            u.Status = "Completed";
                            u.CreateBy = ClassLib.Classlib.User;
                            u.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                            u.RefidJobCard = 0;
                            u.RefJobCard = "";
                            u.RefTempJobCard = "";
                            u.idCSTMPODt = 0;
                            db.tb_StockAdjusts.InsertOnSubmit(u);
                            db.SubmitChanges();
                            dbClss.AddHistory(this.Name, "ปรับสต็อก", "เพิ่มรายการ ปรับสต็อก [" + u.CodeNo + " จำนวนปรับปรุง :" + u.Qty.ToString() + " จำนวนก่อนปรับปรุง " + StockControl.dbClss.TSt(ss.StockQty) + "]", ADNo);


                            decimal StandardCost = 0;
                            StandardCost = dbClss.TDe(ss.Cost); //Math.Round((StockControl.dbClss.TDe(gg.Amount) / StockControl.dbClss.TDe(g.Cells["QTYTrue"].Value)), 2);

                            Qty = StockControl.dbClss.TDe(ss.InputQty);
                            if (Qty > 0)
                            {
                                Seq += 1;
                                BasePCSUnit = 1;
                                Amount = StockControl.dbClss.TDe(ss.Amount);
                                UnitCost = Math.Round((Amount / Qty), 2);//Math.Round((Amount / (Qty * StockControl.dbClss.TDe(g.Cells["PCSUnit"].Value) * BasePCSUnit)), 2);

                                //แบบที่ 1 จะไป sum ใหม่
                                RemainQty = (Convert.ToDecimal(db.Cal_QTY_Remain_Location(StockControl.dbClss.TSt(ss.InternalNo), "Free", 0, Location, 0)));
                                sum_Remain = Convert.ToDecimal(dbClss.Get_Stock(StockControl.dbClss.TSt(ss.InternalNo), "", "", "RemainAmount", Location, 0))
                                    + Amount;

                                sum_Qty = RemainQty + Qty;//Math.Round(((Qty * StockControl.dbClss.TDe(g.Cells["PCSUnit"].Value) * BasePCSUnit)), 2);
                                RemainAmount = sum_Remain;
                                if (sum_Qty <= 0)
                                    RemainUnitCost = 0;
                                else
                                    RemainUnitCost = Math.Round((Math.Abs(RemainAmount) / Math.Abs(sum_Qty)), 2);

                                tb_Stock gg = new tb_Stock();
                                gg.AppDate = AppDate;
                                gg.Seq = Seq;
                                gg.App = "Adjust";
                                gg.Appid = Seq;
                                gg.CreateBy = ClassLib.Classlib.User;
                                gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                gg.DocNo = ss.CheckNo;
                                gg.RefNo = ss.CheckNo;
                                gg.CodeNo = ss.InternalNo;
                                gg.Type = Type;
                                gg.QTY = Qty;//Math.Round((Qty * StockControl.dbClss.TDe(g.Cells["PCSUnit"].Value) * BasePCSUnit), 2);
                                gg.Inbound = Qty;//Math.Round((Qty * StockControl.dbClss.TDe(g.Cells["PCSUnit"].Value) * BasePCSUnit), 2);
                                gg.Outbound = 0;
                                gg.Type_i = 1;  //Receive = 1,Cancel Receive 2,Shipping = 3,Cancel Shipping = 4,Adjust stock = 5,ClearTemp = 6
                                gg.Category = Category;
                                gg.Refid = 0;
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
                                gg.TLQty = Qty;//Math.Round((Qty * StockControl.dbClss.TDe(g.Cells["PCSUnit"].Value) * BasePCSUnit), 2);
                                gg.ShipQty = 0;
                                gg.Location = Location;
                                gg.ShelfNo = ShelfNo;
                                gg.LotNo = LotNo;
                                gg.idCSTMPODt = 0;
                                gg.Free = true;

                                //ต้องไม่ใช่ Item ที่มีในระบบ
                                var cc = (from ix in db.mh_Items
                                          where ix.InternalNo.Trim().ToUpper() == ss.InternalNo.Trim().ToUpper() && ix.Active == true
                                          select ix).ToList();
                                if (cc.Count <= 0)
                                {
                                    gg.TLQty = 0;
                                    gg.ShipQty = Qty;//Math.Round((Qty * StockControl.dbClss.TDe(g.Cells["PCSUnit"].Value) * BasePCSUnit), 2);
                                }

                                db.tb_Stocks.InsertOnSubmit(gg);
                                db.SubmitChanges();
                            }
                            else if (Qty < 0) //ปรับออก
                            {
                                Qty = Math.Abs(Qty);
                                //db.sp_041_tb_Adjust_Stock(txtADNo.Text, vv.CodeNo, Qty, ClassLib.Classlib.User,"","",0, Location,dbClss.TInt(vv.idCSTMPODt));
                                db.sp_057_Cut_Stock(ss.CheckNo, StockControl.dbClss.TSt(ss.InternalNo).Trim().ToUpper(), Qty, ClassLib.Classlib.User, "", "", 0, Location, "Adjust", "Adjust", 5, 0, 0, "", 0);

                            }

                            //update Stock เข้า item
                            db.sp_010_Update_StockItem(StockControl.dbClss.TSt(ss.InternalNo).Trim().ToUpper(), "");
                        }
                        //else
                        //{
                        //    var c = (from ix in db.mh_CheckStock_Lists select ix)
                        //             .Where(a => a.CheckStatus == "Waiting" && a.id == id).ToList();
                        //    if (c.Count > 0)
                        //    {
                        //        foreach (var ss in c)
                        //        {
                        //            ss.CheckStatus = "Completed";
                        //            db.SubmitChanges();
                        //        }
                        //    }
                        //}

                    }
                }
                var up = (from ix in db.mh_CheckStock_Lists select ix)
                    .Where(a => a.CheckStatus == "Waiting"
                    && a.SeqStatus == 2
                    && Convert.ToString(a.ApproveBy) != ""
                    && a.CheckNo.Trim().ToUpper() == ADNo.Trim().ToUpper()
                    //&& Convert.ToBoolean(a.InputOK) == true
                    )                    .ToList();
                if (c.Count > 0)
                {
                    foreach (var ss in c)
                    {
                        ss.CheckStatus = "Completed";
                        db.SubmitChanges();
                    }
                }
            }
        }

        private void btnSendApprove_Click(object sender, EventArgs e)
        {
            try
            {
                
                    using (var db = new DataClasses1DataContext())
                    {
                        var c = (from ix in db.mh_CheckStock_Lists select ix)
                        .Where(a => a.CheckStatus == "Waiting"
                        && a.CheckNo.Trim().ToUpper() == txtListNo1.Text.Trim().ToUpper()
                        && Convert.ToBoolean(a.InputOK) == true
                        ).ToList();
                    if (c.Count > 0)
                    {
                        if (MessageBox.Show("ต้องการส่งขออนุมัติปรับสต็อก ใช่หรือไม่?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            if (baseClass.IsSendApprove())
                            {
                                db.sp_062_mh_ApproveList_Add(txtListNo1.Text.Trim(), "Taking Stock", Classlib.User);
                                MessageBox.Show("Send approve complete.");
                            }
                        }
                    }
                    else
                        MessageBox.Show("can't send Approve.");
                    
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
