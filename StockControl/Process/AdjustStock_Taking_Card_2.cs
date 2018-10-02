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
                //if (txtListNo.Text == "")
                //{
                //    using (var db = new DataClasses1DataContext())
                //    {
                //        var g = (from ix in db.mh_CheckStock_Lists select ix)
                //                  .OrderByDescending(ab => ab.id).ToList();
                //        if (g.Count > 0)
                //        {

                //        }
                //    }
                //}
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
                if (MessageBox.Show("คุณต้องทำ Set ข้อมุล ListNo นี้ให้ Completed ใช่หรือไม่? \n ", "Completed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var c = (from ix in db.mh_CheckStock_Lists select ix)
                            .Where(a => a.CheckStatus == "Waiting" && a.CheckNo.Trim().ToUpper() == txtListNo1.Text.Trim().ToUpper()).ToList();
                    if (c.Count > 0)
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
            }
        }
    }
}
