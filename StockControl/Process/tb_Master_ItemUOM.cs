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
using Telerik.WinControls.UI;
using Telerik.WinControls;
using Telerik.WinControls.Data;

namespace StockControl
{
    public partial class tb_Master_ItemUOM : Telerik.WinControls.UI.RadRibbonForm
    {
        public tb_Master_ItemUOM()
        {
            InitializeComponent();
        }

        //private int RowView = 50;
        //private int ColView = 10;
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
            dt.Columns.Add(new DataColumn("UnitCode", typeof(string)));
            dt.Columns.Add(new DataColumn("UnitDetail", typeof(string)));
            dt.Columns.Add(new DataColumn("UnitActive", typeof(bool)));
            dt.Columns.Add(new DataColumn("id", typeof(bool)));
        }
        private void Unit_Load(object sender, EventArgs e)
        {
            RMenu3.Click += RMenu3_Click;
            RMenu4.Click += RMenu4_Click;
            RMenu5.Click += RMenu5_Click;
            RMenu6.Click += RMenu6_Click;
            radGridView1.ReadOnly = true;
            radGridView1.AutoGenerateColumns = false;
            GETDTRow();

            Default();
            DataLoad();
        }
        private void Default()
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

                    GridViewMultiComboBoxColumn col = (GridViewMultiComboBoxColumn)radGridView1.Columns["ItemNo"];
                    col.DataSource = (from ix in db.mh_Items.Where(s => Convert.ToBoolean(s.Active.Equals(true)))
                                      select new { ix.InternalNo,ix.InternalName }).ToList();

                    col.DisplayMember = "InternalNo";
                    col.ValueMember = "InternalNo";
                    col.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDown;
                    col.FilteringMode = GridViewFilteringMode.DisplayMember;

                    col.AutoSizeMode = BestFitColumnMode.DisplayedDataCells;
                    col.TextAlignment = ContentAlignment.MiddleCenter;
                    col.DropDownStyle = RadDropDownStyle.DropDownList;

                }
                catch { }
                try
                {

                    GridViewMultiComboBoxColumn col = (GridViewMultiComboBoxColumn)radGridView1.Columns["UOMCode"];
                    col.DataSource = (from ix in db.mh_Units.Where(s => Convert.ToBoolean(s.UnitActive.Equals(true)))
                                      select new { ix.UnitCode }).ToList();

                    col.DisplayMember = "UnitCode";
                    col.ValueMember = "UnitCode";
                    col.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDown;
                    col.FilteringMode = GridViewFilteringMode.DisplayMember;

                    col.AutoSizeMode = BestFitColumnMode.DisplayedDataCells;
                    col.TextAlignment = ContentAlignment.MiddleCenter;
                    col.DropDownStyle = RadDropDownStyle.DropDownList;

                }
                catch { }
            }
        }

        private void RMenu6_Click(object sender, EventArgs e)
        {
           
            DeleteUnit();
            DataLoad();
        }

        private void RMenu5_Click(object sender, EventArgs e)
        {
            EditClick();
        }

        private void RMenu4_Click(object sender, EventArgs e)
        {
            ViewClick();
        }

        private void RMenu3_Click(object sender, EventArgs e)
        {
            NewClick();

        }

        private void DataLoad()
        {
            //dt.Rows.Clear();
            int ck = 0;
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                //dt = ClassLib.Classlib.LINQToDataTable(db.tb_Units.ToList());
                radGridView1.DataSource = db.mh_ItemUOMs.ToList();// dt;
                //foreach(var x in radGridView1.Rows)
                //{

                    //x.Cells["TempVatType"].Value = x.Cells["VatType"].Value.ToString();
                    //x.Cells["VatType"].ReadOnly = true;
                    //if(row>=0 && row==ck && radGridView1.Rows.Count > 0)
                    //{

                    //    x.ViewInfo.CurrentRow = x;
                        
                    //}
                    //ck += 1;
                //}
               
            }


            //    radGridView1.DataSource = dt;
        }
        private bool CheckDuplicate(string code)
        {
            bool ck = false;

            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                int i = (from ix in db.mh_VATTypes where ix.VatType == code select ix).Count();
                if (i > 0)
                    ck = false;
                else
                    ck = true;
            }
            return ck;
        }

        private bool AddUnit()
        {
            bool ck = false;
            int C = 0;
            try
            {
                radGridView1.EndEdit();
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    foreach (var g in radGridView1.Rows)
                    {
                        if (!Convert.ToString(g.Cells["ItemNo"].Value).Equals("")
                            && !Convert.ToString(g.Cells["UOMCode"].Value).Equals("")
                            && g.IsVisible)
                        {
                            if (Convert.ToString(g.Cells["dgvC"].Value).Equals("T"))
                            {

                                var g1 = (from ix in db.mh_ItemUOMs
                                          where ix.ItemNo == Convert.ToString(g.Cells["ItemNo"].Value)
                                          && ix.UOMCode == Convert.ToString(g.Cells["UOMCode"].Value)
                                          //&& 
                                          //ix.id == Convert.ToInt16(g.Cells["id"].Value)
                                          select ix).ToList();
                                if (g1.Count > 0)
                                //if(Convert.ToInt16(g.Cells["id"].Value)==0)
                                //if (Convert.ToString(g.Cells["TempVatType"].Value).Equals(""))
                                {

                                    var gg = (from ix in db.mh_ItemUOMs
                                              where ix.ItemNo == Convert.ToString(g.Cells["ItemNo"].Value)
                                                && ix.UOMCode == Convert.ToString(g.Cells["UOMCode"].Value)
                                              //ix.id == Convert.ToInt16(g.Cells["id"].Value)
                                              select ix).ToList();
                                    if (gg.Count > 0)
                                    {
                                        var unit1 = (from ix in db.mh_ItemUOMs
                                                     where ix.ItemNo == Convert.ToString(g.Cells["ItemNo"].Value)
                                                 && ix.UOMCode == Convert.ToString(g.Cells["UOMCode"].Value)
                                                     //ix.id == Convert.ToInt16(g.Cells["id"].Value)
                                                     select ix).First();
                                        //unit1.VatType = Convert.ToString(g.Cells["VatType"].Value);
                                        //u.VatGroup = Convert.ToString(g.Cells["VatGroup"].Value);
                                        unit1.QuantityPer = dbClss.TDe(g.Cells["QuantityPer"].Value);
                                        unit1.ItemNo =dbClss.TSt(g.Cells["ItemNo"].Value);
                                        unit1.UOMCode = dbClss.TSt(g.Cells["UOMCode"].Value);
                                        unit1.Active = Convert.ToBoolean(g.Cells["Active"].Value);
                                        unit1.Remark = Convert.ToString(g.Cells["Remark"].Value);
                                        unit1.CreateBy = ClassLib.Classlib.User;
                                        unit1.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));

                                        C += 1;

                                        db.SubmitChanges();
                                        dbClss.AddHistory(this.Name, "แก้ไข", "Update Item UOM [ ItemNo:" + unit1.ItemNo + ", UOMCode:" + unit1.UOMCode + "]", "");
                                    }
                                }
                                else
                                {

                                    mh_ItemUOM u = new mh_ItemUOM();
                                    u.QuantityPer = dbClss.TDe(g.Cells["QuantityPer"].Value);
                                    u.ItemNo = dbClss.TSt(g.Cells["ItemNo"].Value);
                                    u.UOMCode = dbClss.TSt(g.Cells["UOMCode"].Value);
                                    u.Active = Convert.ToBoolean(g.Cells["Active"].Value);
                                    u.Remark = Convert.ToString(g.Cells["Remark"].Value);
                                    u.CreateBy = ClassLib.Classlib.User;
                                    u.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                    db.mh_ItemUOMs.InsertOnSubmit(u);
                                    db.SubmitChanges();
                                    C += 1;
                                    dbClss.AddHistory(this.Name, "เพิ่ม", "Insert Item UOM  [ ItemNo:" + u.ItemNo + ", UOMCode:" + u.UOMCode + "]", "");


                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message);
                dbClss.AddError(this.Name, ex.Message, this.Name);
            }

            if (C > 0)
                MessageBox.Show("บันทึกสำเร็จ!");

            return ck;
        }
        private bool DeleteUnit()
        {
            bool ck = false;
         
            int C = 0;
            try
            {
                
                if (row >= 0)
                {
                    int id = Convert.ToInt16(radGridView1.Rows[row].Cells["id"].Value);
                    string CodeDelete = Convert.ToString(radGridView1.Rows[row].Cells["ItemNo"].Value);
                    string VatGroup = Convert.ToString(radGridView1.Rows[row].Cells["UOMCode"].Value);
                    radGridView1.EndEdit();
                    if (MessageBox.Show("ต้องการลบรายการ ( Internal No :"+ CodeDelete+ " UOM Code : "+ VatGroup + " ) หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        using (DataClasses1DataContext db = new DataClasses1DataContext())
                        {

                            if (!CodeDelete.Equals(""))
                            {
                                if (!VatGroup.Equals(""))
                                {

                                    var unit1 = (from ix in db.mh_ItemUOMs
                                                 where //ix.VatType == CodeDelete
                                                 ix.id == id
                                                 select ix).ToList();
                                    foreach (var d in unit1)
                                    {
                                        db.mh_ItemUOMs.DeleteOnSubmit(d);
                                        dbClss.AddHistory(this.Name, "ลบ Unit", "Delete Item UOM [ ItemNo:" + d.ItemNo+ ", UOMCode:" + d.UOMCode+"]","");
                                    }
                                    C += 1;


                                    db.SubmitChanges();
                                }
                            }

                        }
                    }
                }
            }

            catch (Exception ex) { MessageBox.Show(ex.Message);
                dbClss.AddError(this.Name, ex.Message, this.Name);
            }

            if (C > 0)
            {
                radGridView1.CurrentRow.IsVisible = false;
                row = row - 1;
                MessageBox.Show("ลบรายการ สำเร็จ!");
            }
              
            return ck;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DataLoad();
        }
        private void NewClick()
        {
            radGridView1.ReadOnly = false;
            radGridView1.AllowAddNewRow = false;
            btnEdit.Enabled = false;
            btnView.Enabled = true;
            radGridView1.Rows.AddNew();
        }
        string Ac = "";
        private void EditClick()
        {
            radGridView1.ReadOnly = false;
            btnEdit.Enabled = false;
            btnView.Enabled = true;
            radGridView1.AllowAddNewRow = false;
            Ac = "Edit";
        }
        private void ViewClick()
        {
            radGridView1.ReadOnly = true;
            btnView.Enabled = false;
            btnEdit.Enabled = true;
            radGridView1.AllowAddNewRow = false;
            DataLoad();
            Ac = "View";
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            NewClick();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            ViewClick();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

            EditClick();
        }
        private void Saveclick()
        {
            if (MessageBox.Show("ต้องการบันทึก ?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                AddUnit();
                DataLoad();
                ViewClick();
            }
        }
        private void DeleteClick()
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Saveclick();
            
        }


        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                radGridView1.Rows[e.RowIndex].Cells["dgvC"].Value = "T";
                //string check1 = Convert.ToString(radGridView1.Rows[e.RowIndex].Cells["VatType"].Value);
                //string TM= Convert.ToString(radGridView1.Rows[e.RowIndex].Cells["TempVatType"].Value);
                //if (!check1.Trim().Equals("") && TM.Equals(""))
                //{                    
                //    if (!CheckDuplicate(check1.Trim()))
                //    {
                //        MessageBox.Show("ข้อมูล รหัสหน่วย ซ้ำ");
                //        radGridView1.Rows[e.RowIndex].Cells["VatType"].Value = "";
                //        radGridView1.Rows[e.RowIndex].Cells["VatType"].IsSelected = true;

                //    }
                //}
        

            }
            catch(Exception ex) { }
        }

        private void Unit_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {


        }

        private void radGridView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

            if (e.KeyData == (Keys.Control | Keys.S))
            {
                if (MessageBox.Show("ต้องการบันทึก ?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    AddUnit();
                    DataLoad();
                }
            }
            else if (e.KeyData == (Keys.Control | Keys.N))
            {
                if (MessageBox.Show("ต้องการสร้างใหม่ ?", "สร้างใหม่", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    NewClick();
                }
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            
                DeleteUnit();
                DataLoad();
            
        }

        int row = -1;
        private void radGridView1_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            row = e.RowIndex;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            //dbClss.ExportGridCSV(radGridView1);
           dbClss.ExportGridXlSX(radGridView1);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("don't coding");
            return;

            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "Spread Sheet files (*.csv)|*.csv|All files (*.csv)|*.csv";
            if (op.ShowDialog() == DialogResult.OK)
            {

                using (TextFieldParser parser = new TextFieldParser(op.FileName, Encoding.GetEncoding("windows-874")))
                //using (TextFieldParser parser = new TextFieldParser(op.FileName))
                {
                    dt.Rows.Clear();
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
                            //MessageBox.Show(field);
                            if (a>1)
                            {
                                if(c==1)
                                    rd["UnitCode"] = Convert.ToString(field);
                                else if(c==2)
                                    rd["UnitDetail"] = Convert.ToString(field);
                                else if(c==3)
                                    rd["UnitActive"] = Convert.ToBoolean(field);

                            }
                            else
                            {
                                if (c == 1)
                                    rd["UnitCode"] = "";
                                else if (c == 2)
                                    rd["UnitDetail"] = "";
                                else if (c == 3)
                                    rd["UnitActive"] = false;




                            }

                            //
                            //rd[""] = "";
                            //rd[""]
                        }
                        dt.Rows.Add(rd);

                    }
                }
                if(dt.Rows.Count>0)
                {
                    dbClss.AddHistory(this.Name, "Import", "Import file CSV in to System", "");
                    ImportData();
                    MessageBox.Show("Import Completed.");

                    DataLoad();
                }
               
            }
        }

        private void ImportData()
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                   
                    foreach (DataRow rd in dt.Rows)
                    {
                        if (!rd["UnitCode"].ToString().Equals(""))
                        {

                            var x = (from ix in db.mh_Units where ix.UnitCode.ToLower().Trim() == rd["UnitCode"].ToString().ToLower().Trim() select ix).FirstOrDefault();

                            if(x==null)
                            {
                                mh_Unit ts = new mh_Unit();
                                ts.UnitCode = Convert.ToString(rd["UnitCode"].ToString());
                                ts.UnitDetail = Convert.ToString(rd["UnitDetail"].ToString());
                                ts.UnitActive = Convert.ToBoolean(rd["UnitActive"].ToString());
                                db.mh_Units.InsertOnSubmit(ts);
                                db.SubmitChanges();
                            }
                            else
                            {
                                x.UnitDetail = Convert.ToString(rd["UnitDetail"].ToString());
                                x.UnitActive = Convert.ToBoolean(rd["UnitActive"].ToString());
                                db.SubmitChanges();

                            }

                       
                        }
                    }
                   
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message);
                dbClss.AddError("InportData", ex.Message, this.Name);
            }
        }

        private void btnFilter1_Click(object sender, EventArgs e)
        {
            radGridView1.EnableFiltering = true;
        }

        private void btnUnfilter1_Click(object sender, EventArgs e)
        {
            radGridView1.EnableFiltering = false;
        }

        private void radMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MasterTemplate_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                if (Ac == "Edit" || Ac == "New")
                {
                    if (radGridView1.Columns["dgvDel"].Index == e.ColumnIndex)  //dgvDel
                        Delete_Item();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void Delete_Item()
        {
            try
            {

                if (radGridView1.Rows.Count < 0)
                    return;


                if (Ac.Equals("New") || Ac.Equals("Edit"))
                {
                    this.Cursor = Cursors.WaitCursor;


                    int id = 0;
                    int.TryParse(StockControl.dbClss.TSt(radGridView1.CurrentRow.Cells["id"].Value), out id);
                    if (id <= 0)
                        radGridView1.Rows.Remove(radGridView1.CurrentRow);

                    else
                    {
                        //string UnitCode = "";
                        //UnitCode = StockControl.dbClss.TSt(radGridView1.CurrentRow.Cells["UnitCode"].Value);
                        //if (MessageBox.Show("ต้องการลบรายการ ( " + UnitCode + " ) ออกจากรายการ หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        //{
                            DeleteUnit();
                            //radGridView1.CurrentRow.IsVisible = false;
                        //}
                    }
                }
                else
                {
                    MessageBox.Show("ไม่สามารถทำการลบรายการได้");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void MasterTemplate_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {

            RadMultiColumnComboBoxElement mccbEl = e.ActiveEditor as RadMultiColumnComboBoxElement;
            if (mccbEl != null)
            {
                mccbEl.DropDownSizingMode = SizingMode.UpDownAndRightBottom;
                mccbEl.DropDownMinSize = new Size(400, 200);
                mccbEl.DropDownMaxSize = new Size(400, 200);

                mccbEl.AutoSizeDropDownToBestFit = false;
                mccbEl.DropDownAnimationEnabled = false;
                mccbEl.AutoFilter = true;
                FilterDescriptor filterDescriptor = new FilterDescriptor(mccbEl.DisplayMember, FilterOperator.Contains, string.Empty);
                mccbEl.EditorControl.MasterTemplate.FilterDescriptors.Add(filterDescriptor);
            }
        }

        private void MasterTemplate_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            try
            {
                if (e.Column.Name.Equals("ItemNo"))
                {
                    /////////////มีการ เคลียร์ การ Add ก่อน แล้วค่อย Add ใหม่////////////////
                    //Row = e.RowIndex;
                    RadMultiColumnComboBoxElement Comcol = (RadMultiColumnComboBoxElement)e.ActiveEditor;
                    Comcol.Columns.Clear();

                    //RadMultiColumnComboBoxElement Comcol = (RadMultiColumnComboBoxElement)e.ActiveEditor;
                    Comcol.DropDownSizingMode = SizingMode.UpDownAndRightBottom;
                    Comcol.DropDownWidth = 400;
                    Comcol.DropDownHeight = 200;
                    //Comcol.EditorControl.BestFitColumns(BestFitColumnMode.AllCells);
                    Comcol.EditorControl.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                    //ปรับอัตโนมัติ
                    //Comcol.EditorControl.AutoGenerateColumns = false;
                    //Comcol.BestFitColumns(true, true);
                    Comcol.AutoFilter = true;

                    //Comcol.EditorControl.AllowAddNewRow = true;
                    Comcol.EditorControl.EnableFiltering = true;
                    Comcol.EditorControl.ReadOnly = false;
                    Comcol.ClearFilter();


                    //Comcol.DisplayMember = "ItemNo";
                    //Comcol.ValueMember = "ItemNo";

                    // //----------------------------- ปรับโดยกำหนดเอง
                    Comcol.EditorControl.Columns.Add(new GridViewTextBoxColumn
                    {
                        HeaderText = "InternalNo",
                        Name = "InternalNo",
                        FieldName = "InternalNo",
                        Width = 150,
                        AllowFiltering = true,
                        ReadOnly = false
                    }
                   );
                    Comcol.EditorControl.Columns.Add(new GridViewTextBoxColumn
                    {
                        HeaderText = "InternalName",
                        Name = "InternalName",
                        FieldName = "InternalName",
                        Width = 250,
                        AllowFiltering = true,
                        ReadOnly = false
                    }
                   );
                }
                else if (e.Column.Name.Equals("UOMCode"))
                {
                    /////////////มีการ เคลียร์ การ Add ก่อน แล้วค่อย Add ใหม่////////////////
                    //Row = e.RowIndex;
                    RadMultiColumnComboBoxElement Comcol = (RadMultiColumnComboBoxElement)e.ActiveEditor;
                    Comcol.Columns.Clear();

                    //RadMultiColumnComboBoxElement Comcol = (RadMultiColumnComboBoxElement)e.ActiveEditor;
                    Comcol.DropDownSizingMode = SizingMode.UpDownAndRightBottom;
                    Comcol.DropDownWidth = 150;
                    Comcol.DropDownHeight = 200;
                    //Comcol.EditorControl.BestFitColumns(BestFitColumnMode.AllCells);
                    Comcol.EditorControl.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                    //ปรับอัตโนมัติ
                    //Comcol.EditorControl.AutoGenerateColumns = false;
                    //Comcol.BestFitColumns(true, true);
                    Comcol.AutoFilter = true;

                    //Comcol.EditorControl.AllowAddNewRow = true;
                    Comcol.EditorControl.EnableFiltering = true;
                    Comcol.EditorControl.ReadOnly = false;
                    Comcol.ClearFilter();


                    //Comcol.DisplayMember = "ItemNo";
                    //Comcol.ValueMember = "ItemNo";

                    // //----------------------------- ปรับโดยกำหนดเอง
                    Comcol.EditorControl.Columns.Add(new GridViewTextBoxColumn
                    {
                        HeaderText = "UOM",
                        Name = "UnitCode",
                        FieldName = "UnitCode",
                        Width = 100,
                        AllowFiltering = true,
                        ReadOnly = false
                    }
                   );
                }

            }
            catch { }
        }
    }
}
