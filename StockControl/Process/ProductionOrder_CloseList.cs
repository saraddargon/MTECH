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
    public partial class ProductionOrder_CloseList : Telerik.WinControls.UI.RadRibbonForm
    {
        int ts = 0;
        public string retDoc = "";
        public ProductionOrder_CloseList()
        {
            InitializeComponent();
        }
        public ProductionOrder_CloseList(int ts)
        {
            this.ts = ts;
            InitializeComponent();
        }



        Telerik.WinControls.UI.RadTextBox SHNo_tt = new Telerik.WinControls.UI.RadTextBox();
        Telerik.WinControls.UI.RadTextBox CodeNo_tt = new Telerik.WinControls.UI.RadTextBox();
        int screen = 0;
        DataTable dt = new DataTable();
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
        private void GETDTRow()
        {

            //dt.Columns.Add(new DataColumn("CodeNo", typeof(string)));
            //dt.Columns.Add(new DataColumn("ItemDescription", typeof(string)));
            //dt.Columns.Add(new DataColumn("Order", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("BackOrder", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("StockQty", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("UnitBuy", typeof(string)));
            //dt.Columns.Add(new DataColumn("PCSUnit", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("LeadTime", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("MaxStock", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("MinStock", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("VendorNo", typeof(string)));
            //dt.Columns.Add(new DataColumn("VendorName", typeof(string)));



        }
  
        private void Unit_Load(object sender, EventArgs e)
        {

            //radGridView1.ReadOnly = true;
            dgvData.AutoGenerateColumns = false;
            //GETDTRow();
            // DefaultItem();
            cbDate.Checked = true;
            dtFrom.Value = DateTime.Now;
            dtTo.Value = DateTime.Now;

            DataLoad();

         
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
        private void DataLoad()
        {
            dgvData.Rows.Clear();
            
            try
            {

                this.Cursor = Cursors.WaitCursor;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    string docNo = txtSHNo.Text.Trim();
                    string jobNo = txtJobNo.Text.Trim();
                    DateTime? dFrom = (cbDate.Checked) ? (DateTime?)dtFrom.Value.Date : null;
                    DateTime? dTo = (cbDate.Checked) ? (DateTime?)dtTo.Value.Date : null;
                    if (dTo != null)
                        dTo = dTo.Value.Date.AddDays(1).AddMinutes(-1);
                    var m = db.sp_082_CloseJobSpecial_SELECT(docNo, jobNo, dFrom, dTo).ToList();
                    dgvData.DataSource = null;
                    dgvData.AutoGenerateColumns = false;
                    dgvData.DataSource = m;

                    int rno = 1;
                    dgvData.Rows.ToList().ForEach(x =>
                    {
                        x.Cells["dgvNo"].Value = rno++;
                    });
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;


            //    radGridView1.DataSource = dt;
        }
      
        private void btnCancel_Click(object sender, EventArgs e)
        {
          
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            return;
            dgvData.ReadOnly = false;
            dgvData.AllowAddNewRow = false;
            dgvData.Rows.AddNew();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            dgvData.ReadOnly = false;
           // btnEdit.Enabled = false;
            btnPrint.Enabled = true;
            dgvData.AllowAddNewRow = false;
            //DataLoad();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvData.CurrentCell != null)
                {
                    if (ts == 0)
                    {
                        string a = dgvData.CurrentCell.RowInfo.Cells["DocNo"].Value.ToSt();
                        var p = new ProductionOrder_Close(a);
                        p.ShowDialog();
                        DataLoad();
                    }
                    else
                    {
                        retDoc = dgvData.CurrentCell.RowInfo.Cells["DocNo"].Value.ToSt();
                        this.Close();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                //radGridView1.Rows[e.RowIndex].Cells["dgvC"].Value = "T";
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

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                return;
                OpenFileDialog op = new OpenFileDialog();
                op.Filter = "Spread Sheet files (*.csv)|*.csv|All files (*.csv)|*.csv";
                if (op.ShowDialog() == DialogResult.OK)
                {


                    using (TextFieldParser parser = new TextFieldParser(op.FileName))
                    {
                        dt.Rows.Clear();
                        DateTime? d = null;
                        DateTime d1 = DateTime.Now;
                        parser.TextFieldType = FieldType.Delimited;
                        parser.SetDelimiters(",");
                        int a = 0;
                        int c = 0;
                        while (!parser.EndOfData)
                        {
                            //Processing row
                            a += 1;
                            DataRow rd = dt.NewRow();
                            // MessageBox.Show(a.ToString());
                            string[] fields = parser.ReadFields();
                            c = 0;
                            foreach (string field in fields)
                            {
                                c += 1;
                                //TODO: Process field

                                if (a > 1)
                                {
                                    if (c == 1)
                                        rd["ModelName"] = Convert.ToString(field).Trim();
                                    else if (c == 2)
                                        rd["ModelDescription"] = Convert.ToString(field);
                                    else if (c == 3)
                                        rd["ModelActive"] = Convert.ToBoolean(field);
                                    else if (c == 4)
                                        rd["LineName"] = Convert.ToString(field).Trim();
                                    else if (c == 5)
                                        rd["MCName"] = Convert.ToString(field);
                                    else if (c == 6)
                                        rd["Limit"] = Convert.ToBoolean(field);
                                    else if (c == 7)
                                    {
                                        if (DateTime.TryParse(Convert.ToString(field), out d1))
                                        {
                                            rd["ExpireDate"] = Convert.ToDateTime(field);

                                        }
                                        else
                                        {
                                            rd["ExpireDate"] = d;
                                        }
                                    }

                                }
                                else
                                {
                                    if (c == 1)
                                        rd["ModelName"] = "";
                                    else if (c == 2)
                                        rd["ModelDescription"] = "";
                                    else if (c == 3)
                                        rd["ModelActive"] = false;
                                    else if (c == 4)
                                        rd["LineName"] = "";
                                    else if (c == 5)
                                        rd["MCName"] = "";
                                    else if (c == 6)
                                        rd["Limit"] = false;
                                    else if (c == 7)
                                        rd["ExpireDate"] = d;




                                }


                            }
                            dt.Rows.Add(rd);

                        }
                    }
                    if (dt.Rows.Count > 0)
                    {

                        dbClss.AddHistory(this.Name, "Import", "Import file CSV in to System", "");
                        ImportData();
                        MessageBox.Show("Import Completed.");

                        DataLoad();
                    }

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dt.Rows.Clear(); }
        }

        private void ImportData()
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {

                    foreach (DataRow rd in dt.Rows)
                    {
                        if (!rd["ModelName"].ToString().Equals(""))
                        {


                            var x = (from ix in db.tb_Models where ix.ModelName == rd["ModelName"].ToString().Trim() select ix).FirstOrDefault();


                            DateTime? d = null;
                            DateTime d1 = DateTime.Now;
                            if (x == null)
                            {
                                tb_Model u = new tb_Model();
                                u.ModelName = rd["ModelName"].ToString().Trim();
                                u.ModelDescription = rd["ModelDescription"].ToString().Trim();
                                u.ModelActive = Convert.ToBoolean(rd["ModelActive"].ToString());
                                u.LineName = rd["LineName"].ToString().Trim();
                                u.MCName = rd["MCName"].ToString().Trim();
                                u.Limit = Convert.ToBoolean(rd["Limit"].ToString());
                                if (DateTime.TryParse(rd["ExpireDate"].ToString(), out d1))
                                {

                                    u.ExpireDate = Convert.ToDateTime(rd["ExpireDate"].ToString());
                                }
                                else
                                {
                                    u.ExpireDate = d;
                                }
                                db.tb_Models.InsertOnSubmit(u);
                                db.SubmitChanges();
                            }
                            else
                            {
                                x.ModelName = rd["ModelName"].ToString().Trim();
                                x.ModelDescription = rd["ModelDescription"].ToString().Trim();
                                x.ModelActive = Convert.ToBoolean(rd["ModelActive"].ToString());
                                x.LineName = rd["LineName"].ToString().Trim();
                                x.MCName = rd["MCName"].ToString().Trim();
                                x.Limit = Convert.ToBoolean(rd["Limit"].ToString());
                                if (DateTime.TryParse(rd["ExpireDate"].ToString(), out d1))
                                {

                                    x.ExpireDate = Convert.ToDateTime(rd["ExpireDate"].ToString());
                                }
                                else
                                {
                                    x.ExpireDate = d;
                                }


                                db.SubmitChanges();

                            }



                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("InportData", ex.Message, this.Name);
            }
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                string SHNo = "";
                if (dgvData.Rows.Count > 0)
                    SHNo = StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["ShippingNo"].Value);
                PrintPR a = new PrintPR(SHNo, SHNo, "Shipping");
                a.ShowDialog();
            }
            catch { }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void MasterTemplate_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if(dgvData.CurrentCell != null)
            {
                if (ts == 0)
                {
                    string a = dgvData.CurrentCell.RowInfo.Cells["DocNo"].Value.ToSt();
                    var p = new ProductionOrder_Close(a);
                    p.ShowDialog();
                    DataLoad();
                }
                else
                {
                    retDoc = dgvData.CurrentCell.RowInfo.Cells["DocNo"].Value.ToSt();
                    this.Close();
                }
            }
        }

        private void frezzRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvData.Rows.Count > 0)
                {

                    int Row = 0;
                    Row = dgvData.CurrentRow.Index;
                    dbClss.Set_Freeze_Row(dgvData, Row);


                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void frezzColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvData.Columns.Count > 0)
                {

                    int Col = 0;
                    Col = dgvData.CurrentColumn.Index;
                    dbClss.Set_Freeze_Column(dgvData, Col);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void unFrezzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                dbClss.Set_Freeze_UnColumn(dgvData);
                dbClss.Set_Freeze_UnRows(dgvData);


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
