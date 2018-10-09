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
    public partial class ProductionOrder_Close : Telerik.WinControls.UI.RadRibbonForm
    {
        public ProductionOrder_Close()
        {
            InitializeComponent();
        }
        public ProductionOrder_Close(string tNo)
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
            if(tNo != "")
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
                txtSPNo.Text = docNo;
                var m = db.mh_ProductionOrder_CloseSpecials.Where(x => x.DocNo == docNo).FirstOrDefault();
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
                        txtQty.Text = j.Qty.ToSt();
                        txtOutQty.Text = j.OutQty.ToSt();
                        txtidCstmPODt.Text = j.RefDocId.ToSt();
                        txtSPNo.Text = m.DocNo;
                        txtRemark.Text = m.Remark;

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
                    err += "- Please input Job no.\n";
                using (var db = new DataClasses1DataContext())
                {
                    string jNo = txtJobNo.Text.Trim();
                    var m = db.mh_ProductionOrders.Where(x => x.JobNo == jNo).FirstOrDefault();
                    if (m == null)
                        err += "- Job no. not found.\n";
                    else
                    {
                        if (m.CloseJob)
                            err += "- Job is already closed.\n";
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
                        var m = db.mh_ProductionOrder_CloseSpecials.Where(x => x.DocNo == spNo).FirstOrDefault();
                        var j = db.mh_ProductionOrders.Where(x => x.JobNo == jobNo).FirstOrDefault();
                        if (m == null && j != null)
                        {
                            m = new mh_ProductionOrder_CloseSpecial
                            {
                                Active = true,
                                CreateBy = ClassLib.Classlib.User,
                                CreateDate = DateTime.Now,
                                DocNo = dbClss.GetNo(40, 2),
                                idCSTMPOdt = txtidCstmPODt.Text.ToInt(),
                                JobNo = txtJobNo.Text.Trim(),
                                UpdateBy = ClassLib.Classlib.User,
                                UpdateDate = DateTime.Now,
                                Remark = txtRemark.Text,
                                Qty = j.OutQty,
                                PCSUnit = j.PCSUnit,
                                UOM = j.UOM,
                            };
                            db.mh_ProductionOrder_CloseSpecials.InsertOnSubmit(m);
                            db.SubmitChanges();

                            j.OutQty = 0;
                            j.CloseJob = true;
                            j.UpdateBy = ClassLib.Classlib.User;
                            j.UpdateDate = DateTime.Now;
                            db.SubmitChanges();

                        }
                    }

                    DataLoad();
                    MessageBox.Show("Close Job complete.\n");

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
            txtQty.Text = "";
            txtidCstmPODt.Text = "";
            txtOutQty.Text = "";
            txtRemark.Text = "";
            btnSave.Enabled = true;
            btnDelete.Enabled = false;
            txtSPNo.Text = StockControl.dbClss.GetNo(40, 0);
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
            var p = new ProductionOrder_CloseList(1);
            p.ShowDialog();
            if(p.retDoc != "")
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
                var m = db.mh_ProductionOrders.Where(x => x.JobNo == jobNo && !x.CloseJob && x.Active).FirstOrDefault();
                if (m != null)
                {
                    txtFGName.Text = m.FGName;
                    txtFGNo.Text = m.FGNo;
                    //txtQty.Text = m.Qty.ToSt();
                    txtQty.Text = m.OutQty.ToSt();
                    txtOutQty.Text = m.OutQty.ToSt();
                    txtidCstmPODt.Text = m.RefDocId.ToSt();
                }
                else
                    baseClass.Warning("Job not found.\n");
            }
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (baseClass.Question("Do you want to 'Delete' ?"))
                DelDoc();
        }
        void DelDoc()
        {
            using (var db = new DataClasses1DataContext())
            {
                string docNo = txtSPNo.Text.Trim();
                var m = db.mh_ProductionOrder_CloseSpecials.Where(x => x.DocNo == docNo).FirstOrDefault();
                if (m != null)
                {
                    var j = db.mh_ProductionOrders.Where(x => x.JobNo == m.JobNo && x.Active && x.CloseJob).FirstOrDefault();
                    if (j != null)
                    {
                        m.Active = false;
                        m.UpdateDate = DateTime.Now;
                        m.UpdateBy = ClassLib.Classlib.User;

                        j.OutQty += m.Qty;
                        j.CloseJob = false;
                        j.UpdateBy = ClassLib.Classlib.User;
                        j.UpdateDate = DateTime.Now;
                        db.SubmitChanges();
                        btnDelete.Enabled = false;
                        baseClass.Info("Delete complete.\n");
                    }
                }
            }
        }
    }
}
