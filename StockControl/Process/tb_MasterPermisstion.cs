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
using Telerik.WinControls;
using Telerik.WinControls.Data;

namespace StockControl
{
    public partial class tb_MasterPermisstion : Telerik.WinControls.UI.RadRibbonForm
    {
        public tb_MasterPermisstion()
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
            dt.Columns.Add(new DataColumn("VendorNo", typeof(string)));
            dt.Columns.Add(new DataColumn("VendorName", typeof(string)));
            dt.Columns.Add(new DataColumn("Address", typeof(string)));
            dt.Columns.Add(new DataColumn("CRRNCY", typeof(string)));
            dt.Columns.Add(new DataColumn("Remark", typeof(string)));
            dt.Columns.Add(new DataColumn("Active", typeof(bool)));
            dt.Columns.Add(new DataColumn("ContactName", typeof(string)));
            dt.Columns.Add(new DataColumn("Tel", typeof(string)));
            dt.Columns.Add(new DataColumn("Fax", typeof(string)));
            dt.Columns.Add(new DataColumn("email", typeof(string)));
            
        }
        System.Drawing.Font MyFont;
        DataTable Screen = new DataTable();
        DataTable RootNode = new DataTable();
        DataTable dt_Permisstion = new DataTable();


        private void Unit_Load(object sender, EventArgs e)
        {
            Screen = new DataTable();
            Screen.Columns.Add(new DataColumn("LinkNode", typeof(string)));
            Screen.Columns.Add(new DataColumn("id", typeof(string)));

            RootNode = new DataTable();
            RootNode.Columns.Add(new DataColumn("RootName", typeof(string)));
            RootNode.Columns.Add(new DataColumn("LinkNode", typeof(string)));
            RootNode.Columns.Add(new DataColumn("id", typeof(string)));

            dt_Permisstion = new DataTable();
            dt_Permisstion.Columns.Add(new DataColumn("id", typeof(int)));
            dt_Permisstion.Columns.Add(new DataColumn("Refid_UserID", typeof(string)));
            dt_Permisstion.Columns.Add(new DataColumn("UserID", typeof(string)));
            dt_Permisstion.Columns.Add(new DataColumn("RootNode", typeof(string)));
            dt_Permisstion.Columns.Add(new DataColumn("LineNode", typeof(string)));
            dt_Permisstion.Columns.Add(new DataColumn("CreateDate", typeof(DateTime)));
            dt_Permisstion.Columns.Add(new DataColumn("CreateBy", typeof(string)));


            Load_Default();

            DataLoad();
            //LoadDefualt();
            MyFont = new System.Drawing.Font(
                            "Tahoma", 9,
                            FontStyle.Italic,    // + obviously doesn't work, but what am I meant to do?
                            GraphicsUnit.Pixel);

            ViewClick();
        }
        private void Load_Default()
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    this.cboRootNode.AutoFilter = true;
                    this.cboRootNode.DisplayMember = "RootName";
                    FilterDescriptor filter = new FilterDescriptor();
                    filter.PropertyName = this.cboRootNode.DisplayMember;
                    filter.Operator = FilterOperator.Contains;
                    this.cboRootNode.AutoCompleteMode = AutoCompleteMode.Append;
                    this.cboRootNode.EditorControl.MasterTemplate.FilterDescriptors.Add(filter);

                    cboRootNode.DisplayMember = "RootName";
                    cboRootNode.ValueMember = "LinkNode";

                    //RootNode.Rows.Clear();
                    //var G = (from ix in db.OpenForms
                    //         //where ix.RootName == cboRootNode.Text
                    //         select ix).ToList();
                    //if (G.Count() > 0)
                    //{
                    //    foreach (var gg in G)
                    //    {
                    //        RootNode.Rows.Add(gg.RootName,gg.LinkNode, gg.ID);
                    //    }
                    //    cboRootNode.DataSource = RootNode;
                    //    cboRootNode.SelectedIndex = 0;
                    //}
                    cboRootNode.DataSource = db.OpenForms.OrderBy(a=>a.NodeName).ToList();
                   
                    //try
                    //{
                    //    if (!cboRootNode.Text.Equals(""))
                    //    {
                    //        DefaultType();
                    //    }
                    //}
                    //catch { }

                    
                }
            }
            catch(Exception ex ){ MessageBox.Show(ex.Message); }
        }
        

        //private void DefaultType()
        //{
        //    try
        //    {
        //        using (DataClasses1DataContext db = new DataClasses1DataContext())
        //        {
        //            cboScreen.DataSource = null;
        //            cboScreen.DisplayMember = "LinkNode";
        //            cboScreen.ValueMember = "ID";
        //            //cboScreen.DataSource = db.OpenForms.Where(t => t.RootName.Equals(cboRootNode.Text)).ToList();

        //            Screen.Rows.Clear();

        //            var G = (from ix in db.OpenForms
        //                     where ix.RootName == cboRootNode.Text
        //                     select ix).ToList();
        //            if (G.Count() > 0)
        //            {
        //                foreach(var gg in G)
        //                {
        //                    Screen.Rows.Add(gg.LinkNode, gg.ID);
        //                }
        //                cboScreen.DataSource = Screen;
        //                cboScreen.SelectedIndex = 0;
        //            }

                    

        //        }
        //    }
        //    catch (Exception ex) { MessageBox.Show(ex.Message); }
        //}

        private void RMenu6_Click(object sender, EventArgs e)
        {
            //ลบ
            //throw new NotImplementedException();
            btnDelete_Click(sender, e);
        }

        private void RMenu5_Click(object sender, EventArgs e)
        {
            btnEdit_Click(sender, e);
        }

        private void RMenu4_Click(object sender, EventArgs e)
        {
            ////เพิ่มผู้ขาย
            //throw new NotImplementedException();
            btnNew_Click(sender, e);
        }

        private void RMenu3_Click(object sender, EventArgs e)
        {
            ////เพิ่มผุ้ติดต่อ
            //if (row >= 0)
            //{


            //    this.Cursor = Cursors.WaitCursor;
            //    Contact ct = new Contact(Convert.ToString(dgvData.Rows[row].Cells["VendorNo"].Value),
            //        Convert.ToString(dgvData.Rows[row].Cells["VendorName"].Value));
            //    this.Cursor = Cursors.Default;
            //    ct.ShowDialog();
            //    GC.Collect();
            //    GC.WaitForPendingFinalizers();

            //    ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            //    ClassLib.Memory.Heap();
            //}
        }

        private void RadMenuItem3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("menu");
           // throw new NotImplementedException();
        }

        private void LoadDefualt()
        {
            //try
            //{


            //    using (DataClasses1DataContext db = new DataClasses1DataContext())
            //    {
            //        var gt = (from ix in db.tb_CRRNCies select ix).ToList();
            //        GridViewComboBoxColumn comboBoxColumn = this.dgvData.Columns["CRRNCY"] as GridViewComboBoxColumn;
            //        comboBoxColumn.DisplayMember = "CRRNCY";
            //        comboBoxColumn.ValueMember = "CRRNCY";
            //        comboBoxColumn.DataSource = gt;
          
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    dbClss.AddError("CRRNCY", ex.Message, this.Name);
            //}
        }
        private void DataLoad()
        {
            //dt.Rows.Clear();
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
               
               // var g = (from ix in db.tb_Users select ix).ToList();
               //// DataTable dt2 = ClassLib.Classlib.LINQToDataTable(g);
               // dgvData.DataSource = g;
               // SetRowNo1(dgvData);
               // //int ck = 1;
               // //foreach (var x in dgvData.Rows)
               // //{
                    
               // //    x.Cells["dgvNo"].Value = ck;
                   
               // //    ck += 1;
               // //}

            }
            
        }
        private bool CheckDuplicate(string code)
        {
            bool ck = false;

            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                int i = (from ix in db.tb_GroupTypes where ix.GroupCode == code select ix).Count();
                if (i > 0)
                    ck = false;
                else
                    ck = true;
            }
            return ck;
        }
        private bool Check_Save()
        {
            bool re = true;
            string err = "";
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                   
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
        private void AddUnit()
        {
          
            int C = 0;

            if (Check_Save())
                return;
            try
            {
                
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    int Refid_UserID = 0;
                    string RootNode = "";
                    string LineNode = "";
                    string UserID = "";

                    if (txtScreen.Text.Trim() == "")
                        return;
                    
                    foreach (DataRow dr in dt_Permisstion.Rows)
                    {
                        foreach (Object obj in listBox1.SelectedItems)
                        {
                            // Refid_UserID = dbClss.TInt(dr["Refid_UserID"].ToString());
                            //RootNode = dr["RootNode"].ToString();
                            // LineNode = dr["LineNode"].ToString();
                            UserID = dr["UserID"].ToString();

                            var u = (from ix in db.tb_Users
                                     where ix.UserID.ToUpper().Trim() == UserID.ToUpper().Trim()
                                     select ix).ToList();
                            if (u.Count > 0)
                            {
                                Refid_UserID = Convert.ToInt16(u.FirstOrDefault().id);
                            }

                            if (Refid_UserID > 0)
                            {
                                var dd = (from ix in db.tb_Permisstions
                                          where ix.Refid_UserID == Refid_UserID
                                          select ix).ToList();
                                if (dd.Count > 0)
                                {

                                    dbClss.AddHistory(this.Name, "ลบ", "ลบ User Permisstions [ Screen : " + txtScreen.Text + " UserID : " + UserID + "]", "");

                                    db.tb_Permisstions.DeleteAllOnSubmit(dd);
                                    db.SubmitChanges();

                                    C += 1;
                                }
                            }
                        }
                    }

                    //foreach (var g in dgvData.Rows)
                    //{
                    //    if (g.IsVisible)
                    //    {
                    //        if (Convert.ToBoolean(g.Cells["dgvC"].Value))
                    //        {

                    //            if (Convert.ToInt16(g.Cells["id"].Value) <= 0)
                    //            {
                    //                var a = (from ix in db.tb_Users
                    //                         where ix.UserID.ToUpper() == Convert.ToString(g.Cells["UserID"].Value).Trim().ToUpper()
                    //                         select ix).ToList();
                    //                if (a.Count() <= 0)
                    //                {
                    //                    tb_User gy = new tb_User();
                    //                    gy.UserID = Convert.ToString(g.Cells["UserID"].Value).Trim();
                    //                    gy.UserName = Convert.ToString(g.Cells["UserName"].Value).Trim();
                    //                    gy.Department = Convert.ToString(g.Cells["Department"].Value).Trim();
                    //                    gy.Password = Convert.ToString(g.Cells["Password"].Value).Trim();
                    //                    gy.ModifyDate = DateTime.Now;
                    //                    gy.ModifyBy = ClassLib.Classlib.User;
                    //                    gy.CreateDate = DateTime.Now;
                    //                    gy.CreateBy = ClassLib.Classlib.User;
                    //                    gy.Status = Convert.ToBoolean(g.Cells["Status"].Value);

                    //                    db.tb_Users.InsertOnSubmit(gy);
                    //                    db.SubmitChanges();
                    //                    dbClss.AddHistory(this.Name, "เพิ่มUser", "เพิ่ม User [" + gy.UserID + " Department : " + gy.Department.ToString() + "]", "");
                    //                    C += 1;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                var a = (from ix in db.tb_Users
                    //                         where ix.id == Convert.ToInt16(g.Cells["id"].Value)
                    //                         select ix).ToList();
                    //                if (a.Count > 0)
                    //                {
                    //                    var unit1 = (from ix in db.tb_Users
                    //                                 where ix.id == Convert.ToInt16(g.Cells["id"].Value)
                    //                                 select ix).First();

                    //                    unit1.UserID = Convert.ToString(g.Cells["UserID"].Value).Trim();
                    //                    unit1.UserName = Convert.ToString(g.Cells["UserName"].Value).Trim();
                    //                    unit1.Password = Convert.ToString(g.Cells["Password"].Value).Trim();
                    //                    unit1.Department = Convert.ToString(g.Cells["Department"].Value);
                    //                    unit1.Status = Convert.ToBoolean(g.Cells["Status"].Value);
                    //                    unit1.ModifyDate = DateTime.Now;
                    //                    unit1.ModifyBy = ClassLib.Classlib.User;

                    //                    C += 1;

                    //                    db.SubmitChanges();
                    //                    dbClss.AddHistory(this.Name, "แก้ไข", "User [" + unit1.UserID + " Department : " + unit1.Department.ToString() + "]", "");
                    //                }
                    //            }
                    //        }
                    //    }
                    //    else //Delete
                    //    {

                    //        var dd = (from ix in db.tb_Departments
                    //                  where ix.id == Convert.ToInt16(g.Cells["id"].Value)
                    //                  select ix).ToList();
                    //        if (dd.Count > 0)
                    //        {

                    //            dbClss.AddHistory(this.Name, "ลบ", "ลบแผนก [" + dd.FirstOrDefault().Department + "]", "");

                    //            db.tb_Departments.DeleteAllOnSubmit(dd);
                    //            db.SubmitChanges();
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message);
                dbClss.AddError(this.Name, ex.Message, this.Name);
            }
            

            if (C > 0)
            {
                MessageBox.Show("บันทึกสำเร็จ!");
                DataLoad();
                ViewClick();
            }
        }
       
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DataLoad();
        }
        private void NewClick()
        {
            //dgvData.ReadOnly = false;
            //dgvData.AllowAddNewRow = false;
            btnEdit.Enabled = false;
            btnView.Enabled = true;
            //dgvData.Rows.AddNew();
            Ac = "New";
        }
        string Ac = "";
        int Edit = 0;
        private void EditClick()
        {
            //dgvData.ReadOnly = false;
            btnExclude.Enabled = true;
            btnInclude.Enabled = true;
            btnEdit.Enabled = false;
            btnView.Enabled = true;
            btnSave.Enabled = true;
            //dgvData.AllowAddNewRow = true;
            Ac = "Edit";
        }
        private void ViewClick()
        {
            //dgvData.ReadOnly = true;
            btnExclude.Enabled = false;
            btnInclude.Enabled = false;
            btnView.Enabled = false;
            btnEdit.Enabled = true;
            btnSave.Enabled = false;
            //dgvData.AllowAddNewRow = false;
            
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("ต้องการบันทึก ?","บันทึก",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
            {
                if (Ac == "Edit" || Ac == "New")
                {
                    //AddUnit();
                    MessageBox.Show("บันทึกสำเร็จ!");
                }
                else
                    MessageBox.Show("สถานะต้องเป็น New or Edit");
            }
        }

        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
               e.Row.Cells["dgvC"].Value = true;
                //string check1 = Convert.ToString(radGridView1.Rows[e.RowIndex].Cells["VendorName"].Value);
                //string TM= Convert.ToString(radGridView1.Rows[e.RowIndex].Cells["dgvCodeTemp2"].Value);
                //if (!check1.Trim().Equals("") && TM.Equals(""))
                //{

                //    if (!CheckDuplicate(check1.Trim()))
                //    {
                //        MessageBox.Show("ชื้อผู้ขายซ้ำ ซ้ำ");
                //        radGridView1.Rows[e.RowIndex].Cells["GroupCode"].Value = "";
                //        radGridView1.Rows[e.RowIndex].Cells["GroupCode"].IsSelected = true;

                //    }
                //}
                // if (dgvData.Columns["Department"].Index == e.ColumnIndex)
                //{
                //    var cc = e.Row.Cells["Department"];
                //    string DepartmentTemp = Convert.ToString(e.Row.Cells["Department"].Value);
                //    try
                //    {
                //        if (!DepartmentTemp.Equals(Department_Edit) && !Department_Edit.Equals(""))
                //        {
                //            (e.Row.Cells["Department"].Value) = Department_Edit;
                //            Department_Edit = "";
                //        }
                //    }
                //    catch { }
                //}

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        string Department_Edit = "";
        private void Unit_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
           // MessageBox.Show(e.KeyCode.ToString());
        }

        private void radGridView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // MessageBox.Show(e.KeyCode.ToString());

            if (e.KeyData == (Keys.Control | Keys.S))
            {
                if (MessageBox.Show("ต้องการบันทึก ?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    AddUnit();
                    DataLoad();
                }
            }
            //else if (e.KeyCode == Keys.Delete)
            //{
            //}

            //else if(e.KeyData == (Keys.Control | Keys.N))
            //{
            //    if (MessageBox.Show("ต้องการสร้างใหม่ ?", "สร้างใหม่", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //    {
            //        NewClick();
            //    }

            //}
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            
                //DeleteUnit();
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
           //dbClss.ExportGridXlSX(dgvData);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "Spread Sheet files (*.csv)|*.csv|All files (*.csv)|*.csv";
            if (op.ShowDialog() == DialogResult.OK)
            {


                using (TextFieldParser parser = new TextFieldParser(op.FileName))
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
                                    rd["VendorNo"] = Convert.ToString(field);
                                else if(c==2)
                                    rd["VendorName"] = Convert.ToString(field);
                                else if (c == 3)
                                    rd["Address"] = Convert.ToString(field);
                                else if (c == 4)
                                    rd["CRRNCY"] = Convert.ToString(field);
                                else if (c == 5)
                                    rd["Remark"] = Convert.ToString(field);
                                else if (c == 6)
                                    rd["Active"] = Convert.ToBoolean(field);

                            }
                            else
                            {
                                if (c == 1)
                                    rd["VendorNo"] = "";
                                else if (c == 2)
                                    rd["VendorName"] = "";
                                else if (c == 3)
                                    rd["Address"] = "";
                                else if (c == 4)
                                    rd["CRRNCY"] = "";
                                else if (c == 5)
                                    rd["Remark"] = "";
                                else if (c == 6)
                                    rd["Active"] = false;




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
            //try
            //{
            //    using (DataClasses1DataContext db = new DataClasses1DataContext())
            //    {
                   
            //        foreach (DataRow rd in dt.Rows)
            //        {
            //            if (!rd["VendorName"].ToString().Equals(""))
            //            {

            //                var x = (from ix in db.tb_Vendors where ix.VendorNo.ToLower().Trim() == rd["VendorNo"].ToString().ToLower().Trim() select ix).FirstOrDefault();

            //                if(x==null)
            //                {
                                
            //                    tb_Vendor ts = new tb_Vendor();
            //                    ts.VendorNo = dbClss.GetNo(1, 2);
            //                    ts.VendorName = Convert.ToString(rd["VendorName"].ToString());
            //                    ts.Address = Convert.ToString(rd["Address"].ToString());
            //                    ts.CRRNCY = Convert.ToString(rd["CRRNCY"].ToString());
            //                    ts.Remark = Convert.ToString(rd["Remark"].ToString());
            //                    ts.Active = Convert.ToBoolean(rd["Active"].ToString());
            //                    db.tb_Vendors.InsertOnSubmit(ts);
            //                    db.SubmitChanges();
            //                }
            //                else
            //                {
            //                    x.VendorName = Convert.ToString(rd["VendorName"].ToString());
            //                    x.Address = Convert.ToString(rd["Address"].ToString());
            //                    x.CRRNCY = Convert.ToString(rd["CRRNCY"].ToString());
            //                    x.Remark = Convert.ToString(rd["Remark"].ToString());
                               
            //                    x.Active = Convert.ToBoolean(rd["Active"].ToString());
            //                    db.SubmitChanges();

            //                }

                       
            //            }
            //        }
                   
            //    }
            //}
            //catch(Exception ex) { MessageBox.Show(ex.Message);
            //    dbClss.AddError(this.Name, ex.Message, this.Name);
            //}
        }

        private void btnFilter1_Click(object sender, EventArgs e)
        {
            //dgvData.EnableFiltering = true;
        }

        private void btnUnfilter1_Click(object sender, EventArgs e)
        {
            //dgvData.EnableFiltering = false;
        }

        private void radMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MasterTemplate_CellValueChanged(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
           //if(radGridView1.Columns["VendorName"].Index==e.ColumnIndex
           //     || radGridView1.Columns["VendorName"].Index == e.ColumnIndex
           //     || radGridView1.Columns["Address"].Index == e.ColumnIndex
           //     || radGridView1.Columns["CRRNCY"].Index == e.ColumnIndex
           //     || radGridView1.Columns["Remark"].Index == e.ColumnIndex
           //     )
           // {

           // }
        }

        private void MasterTemplate_RowFormatting(object sender, RowFormattingEventArgs e)
        {
            //if (e.RowElement.RowInfo.Cells["VendorNo"].Value == null)
            //{
            //    e.RowElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
            //    e.RowElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
            //    e.RowElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
            //}
            //else if(!e.RowElement.RowInfo.Cells["VendorNo"].Value.Equals(""))
            //{ 
            //    e.RowElement.DrawFill = true;
            //    // e.RowElement.GradientStyle = GradientStyles.Solid;
            //    e.RowElement.BackColor = Color.WhiteSmoke;
          
            //}
            //else
            //{
            //    e.RowElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
            //    e.RowElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
            //    e.RowElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
            //}
        }

        private void MasterTemplate_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            //if (e.CellElement.ColumnInfo.HeaderText == "รหัสผู้ขาย")
            //{
            //    if (e.CellElement.RowInfo.Cells["VendorNo"].Value != null)
            //    {
            //        if (!e.CellElement.RowInfo.Cells["VendorNo"].Value.Equals(""))
            //        {
            //            e.CellElement.DrawFill = true;
            //           // e.CellElement.ForeColor = Color.Blue;
            //            e.CellElement.NumberOfColors = 1;
            //            e.CellElement.BackColor = Color.WhiteSmoke;
            //        }
            //        else
            //        {
            //            e.CellElement.DrawFill = true;
            //            //e.CellElement.ForeColor = Color.Yellow;
            //            e.CellElement.NumberOfColors = 1;
            //            e.CellElement.BackColor = Color.WhiteSmoke;
            //        }
            //    }
            //}
            //else if (e.CellElement.ColumnInfo.HeaderText == "ผู้ติดต่อ"
            //    || e.CellElement.ColumnInfo.HeaderText == "เบอร์โทร"
            //    || e.CellElement.ColumnInfo.HeaderText == "เบอร์แฟกซ์"
            //    || e.CellElement.ColumnInfo.HeaderText == "อีเมล์"
            //    )
            //{
            //    if (e.CellElement.RowInfo.Cells["ContactName"].Value != null
            //        || e.CellElement.RowInfo.Cells["Tel"].Value != null
            //        || e.CellElement.RowInfo.Cells["FAX"].Value != null
            //        || e.CellElement.RowInfo.Cells["Email"].Value != null)
            //    {
            //        e.CellElement.DrawFill = true;
            //        // e.CellElement.ForeColor = Color.Blue;
            //        e.CellElement.NumberOfColors = 1;
            //        e.CellElement.BackColor = Color.WhiteSmoke;
            //        //if (!e.CellElement.RowInfo.Cells["ContactName"].Value.Equals("")
            //        //    )
            //        //{
            //        //    e.CellElement.DrawFill = true;
            //        //    // e.CellElement.ForeColor = Color.Blue;
            //        //    e.CellElement.NumberOfColors = 1;
            //        //    e.CellElement.BackColor = SystemColors.ButtonHighlight;
            //        //}
            //        //else
            //        //{
            //        //    //e.CellElement.DrawFill = true;
            //        //    ////e.CellElement.ForeColor = Color.Yellow;
            //        //    //e.CellElement.NumberOfColors = 1;
            //        //    //e.CellElement.BackColor = Color.WhiteSmoke;
            //        //}
            //    }
            //}
            //else
            //{
            //    e.CellElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
            //    e.CellElement.ResetValue(LightVisualElement.ForeColorProperty, ValueResetFlags.Local);
            //    e.CellElement.ResetValue(LightVisualElement.NumberOfColorsProperty, ValueResetFlags.Local);
            //    e.CellElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
            //}
        }

        private void radButtonElement1_Click(object sender, EventArgs e)
        {

        }

        private void MasterTemplate_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            try
            {
                //if (dgvData.Columns["dgvDel"].Index == e.ColumnIndex)  //dgvDel
                //    Delete_Item();
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void Delete_Item()
        {
            try
            {

                //if (dgvData.Rows.Count < 0)
                //    return;


                //if (Ac.Equals("New") || Ac.Equals("Edit"))
                //{
                //    this.Cursor = Cursors.WaitCursor;

                   
                //        int id = 0;
                //        int.TryParse(StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["id"].Value), out id);
                //    if (id <= 0)
                //        dgvData.Rows.Remove(dgvData.CurrentRow);

                //    else
                //    {
                //        string Department = "";
                //        Department = StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["Department"].Value);
                //        if (MessageBox.Show("ต้องการลบรายการ ( " + Department + " ) ออกจากรายการ หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //        {
                //            dgvData.CurrentRow.IsVisible = false;
                //        }
                //    }
                //        SetRowNo1(dgvData);
                //}
                //else
                //{
                //    MessageBox.Show("ไม่สามารถทำการลบรายการได้");
                //}
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

        private void dgvData_EditorRequired(object sender, EditorRequiredEventArgs e)
        {
            GridViewEditManager manager = sender as GridViewEditManager;
            // Assigning DropDownListAddEditor to the right column
            if (manager.GridViewElement.CurrentColumn.Name == "Department")
            {
                DropDownListAddEditor editor = new DropDownListAddEditor();
                editor.InputValueNotFound += new DropDownListAddEditor.InputValueNotFoundHandler(DropDownListAddEditor_InputValueNotFound);
                e.Editor = editor;
            }
          
        }
        internal class DropDownListAddEditor :
                 RadDropDownListEditor
        {
            protected GridDataCellElement cell;
            protected InputValueNotFoundArgs e;
            /// <summary>
            /// Event handler for missing values in item list of editor
            /// </summary>
            /// <param name="sender">Event source of type DropDownListAddEditor</param>
            /// <param name="e">Event arguments</param>
            public delegate void InputValueNotFoundHandler(object sender,
                                                           InputValueNotFoundArgs e);
            /// <summary>
            /// Event for missing values in item list of editor
            /// </summary>
            public event InputValueNotFoundHandler InputValueNotFound;
            /// <summary>
            /// Constructor
            /// </summary>
            public DropDownListAddEditor() :
                base()
            {
                // Nothing to do
            }
            public override bool EndEdit()
            {
                RadDropDownListEditorElement element = this.EditorElement as RadDropDownListEditorElement;
                string text = element.Text;
                RadListDataItem item = null;
                foreach (RadListDataItem entry in element.Items)
                {
                    if (entry.Text == text)
                    {
                        item = entry;
                        break;
                    }
                }
                if ((item == null) &&
                   (InputValueNotFound != null))
                {
                    // Get cell for handling CellEndEdit event
                    this.cell = (this.EditorManager as GridViewEditManager).GridViewElement.CurrentCell;
                    // Add event handling for setting value to cell
                    (this.OwnerElement as GridComboBoxCellElement).GridControl.CellEndEdit += new GridViewCellEventHandler(OnCellEndEdit);
                    this.e = new InputValueNotFoundArgs(element);
                    this.InputValueNotFound(this,
                                            this.e);
                }
                return base.EndEdit();
            }
            /// <summary>
            /// Puts added value into cell value
            /// </summary>
            /// <param name="sender">Event source of type GridViewEditManager</param>
            /// <param name="e">Event arguments</param>
            /// <remarks>Connected to GridView event CellEndEdit</remarks>
            protected void OnCellEndEdit(object sender,
                                         GridViewCellEventArgs e)
            {
                if (this.e != null)
                {
                    // Handle only added value, others by default handling of grid
                    if ((this.cell == (sender as GridViewEditManager).GridViewElement.CurrentCell) &&
                        this.e.ValueAdded)
                    {
                        e.Row.Cells[e.ColumnIndex].Value = this.e.Value;
                    }
                    this.e = null;
                }
            }
            /// <summary>
            /// Event arguments for InputValueNotFound
            /// </summary>
            public class InputValueNotFoundArgs :
                             EventArgs
            {
                /// <summary>
                /// Constructor
                /// </summary>
                /// <param name="editorElement">Editor assiciated element</param>
                internal protected InputValueNotFoundArgs(RadDropDownListEditorElement editorElement)
                {
                    this.EditorElement = editorElement;
                    this.Text = editorElement.Text;
                }
                /// <summary>
                /// Editor associated element 
                /// </summary>
                public RadDropDownListEditorElement EditorElement { get; protected set; }
                /// <summary>
                /// Input text with no match in drop down list
                /// </summary>
                public string Text { get; protected set; }
                /// <summary>
                /// Text related missing value
                /// </summary>
                /// <remarks>Has to be set during event processing</remarks>
                /// <seealso cref="ValueAdded"/>
                public object Value { get; set; }
                /// <summary>
                /// Missing value added
                /// </summary>
                /// <remarks>Set also the Value property</remarks>
                public bool ValueAdded { get; set; }
            }
        }
        private void DropDownListAddEditor_InputValueNotFound(object sender, DropDownListAddEditor.InputValueNotFoundArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(e.Text))
                {
                    List<string> values = e.EditorElement.DataSource as List<string>;
                    if (values == null)
                    {
                        List<string> aa = new List<string>();
                        e.EditorElement.DataSource = aa;
                        values = e.EditorElement.DataSource as List<string>;
                    }
                    if (!e.Text.Equals(""))
                    {
                        Department_Edit = e.Text;

                    }
                    values.Add(e.Text);
                    e.Value = e.Text;
                    e.ValueAdded = true;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void dgvData_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            RadMultiColumnComboBoxElement mccbEl = e.ActiveEditor as RadMultiColumnComboBoxElement;
            if (mccbEl != null)
            {
                mccbEl.DropDownSizingMode = SizingMode.UpDownAndRightBottom;
                mccbEl.DropDownMinSize = new Size(550, 300);
                mccbEl.DropDownMaxSize = new Size(550, 300);

                mccbEl.AutoSizeDropDownToBestFit = false;
                mccbEl.DropDownAnimationEnabled = false;
                mccbEl.AutoFilter = true;
                FilterDescriptor filterDescriptor = new FilterDescriptor(mccbEl.DisplayMember, FilterOperator.Contains, string.Empty);
                mccbEl.EditorControl.MasterTemplate.FilterDescriptors.Add(filterDescriptor);
            }
        }

        private void cboRootNode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboRootNode.Text != "")
                {
                    txtLinkNode.Text = cboRootNode.SelectedValue.ToString();


                    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    {
                        var G = (from ix in db.OpenForms
                                 where ix.RootName == cboRootNode.Text
                                    && ix.LinkNode == txtLinkNode.Text
                                 select ix).ToList();
                        if (G.Count() > 0)
                        {
                            txtScreen.Text = Convert.ToString(G.FirstOrDefault().TextNode);
                        }
                        LoadTextBox1();
                        LoadTextBox2();
                    }
                }
                else
                {
                    txtLinkNode.Text = "";
                    txtScreen.Text = "";
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void LoadTextBox1()
        {
            try
            {
                listBox1.Items.Clear();
                if (cboRootNode.Text != "" && txtLinkNode.Text != "")
                {
                    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    {
                        var G = (from ix in db.sp_029_Permisstion1(cboRootNode.Text, txtLinkNode.Text)
                                 select ix).ToList();
                        if (G.Count() > 0)
                        {
                            foreach (var GG in G)
                            {
                                listBox1.Items.Add(GG.UserID);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void LoadTextBox2()
        {
            try
            {
                listBox2.Items.Clear();
                dt_Permisstion.Rows.Clear();
                if (cboRootNode.Text != "" && txtLinkNode.Text != "")
                {
                    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    {
                        var G = (from ix in db.sp_030_Permisstion2(cboRootNode.Text, txtLinkNode.Text)
                                 select ix).ToList();
                        if (G.Count() > 0)
                        {
                            foreach(var GG in G)
                            {
                                listBox2.Items.Add(GG.UserID);
                            }
                            dt_Permisstion = StockControl.dbClss.LINQToDataTable(G);
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    MessageBox.Show(listBox1.SelectedItem.ToString());
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnInclude_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.Items.Count > 0)
                {
                    if (this.listBox1.SelectedIndex >= 0)
                    {
                        string UserID = listBox1.SelectedItem.ToString();
                        listBox2.Items.Add(listBox1.SelectedItem.ToString());
                        //if (this.listBox1.SelectedIndex >= 0)
                            this.listBox1.Items.RemoveAt(this.listBox1.SelectedIndex);

                        Edit = 1;

                        Add(UserID);

                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void Add(string UserID)
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    int Refid_UserID = 0;
                    string RootNode = "";
                    string LineNode = "";

                    if (txtScreen.Text.Trim() == "")
                        return;



                    var u = (from ix in db.tb_Users
                             where ix.UserID.ToUpper().Trim() == UserID.ToUpper().Trim()
                             select ix).ToList();
                    if (u.Count > 0)
                    {
                        Refid_UserID = Convert.ToInt16(u.FirstOrDefault().id);
                    }

                    if (Refid_UserID > 0)
                    {
                        var a = (from ix in db.tb_Permisstions
                                 where ix.Refid_UserID == Refid_UserID
                                    && ix.LineNode == txtLinkNode.Text.Trim()
                                    && ix.RootNode == cboRootNode.Text.Trim()
                                 select ix).ToList();
                        if (a.Count <= 0)
                        {
                            tb_Permisstion gy = new tb_Permisstion();
                            gy.Refid_UserID = Refid_UserID;
                            gy.RootNode = cboRootNode.Text.Trim();
                            gy.LineNode = txtLinkNode.Text.Trim();
                            gy.CreateBy = ClassLib.Classlib.User;                         
                            gy.CreateDate = DateTime.Now;                    

                            db.tb_Permisstions.InsertOnSubmit(gy);
                            db.SubmitChanges();
                            dbClss.AddHistory(this.Name, "เพิ่ม User Permisstion", "เพิ่ม User Permisstion RootNode : [" +gy.RootNode + " Screen : " + txtScreen.Text.ToString()+" UserID : " + UserID+ "]", "");
                         
                        }
                    }

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError(this.Name, ex.Message + "Add", this.Name);
            }

        }
        private void Del(string UserID)
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    int Refid_UserID = 0;
                    //string RootNode = "";
                    //string LineNode = "";

                    if (txtScreen.Text.Trim() == "")
                        return;



                    var u = (from ix in db.tb_Users
                             where ix.UserID.ToUpper().Trim() == UserID.ToUpper().Trim()
                             select ix).ToList();
                    if (u.Count > 0)
                    {
                        Refid_UserID = Convert.ToInt16(u.FirstOrDefault().id);
                    }

                    if (Refid_UserID > 0)
                    {
                        var dd = (from ix in db.tb_Permisstions
                                  where ix.Refid_UserID == Refid_UserID
                                   && ix.LineNode == txtLinkNode.Text.Trim()
                                    && ix.RootNode == cboRootNode.Text.Trim()
                                  select ix).ToList();
                        if (dd.Count > 0)
                        {
                            dbClss.AddHistory(this.Name, "ลบ", "ลบ User Permisstions [ Screen : " + txtScreen.Text + " UserID : " + UserID + "]", "");

                            db.tb_Permisstions.DeleteAllOnSubmit(dd);
                            db.SubmitChanges();


                        }
                    }

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError(this.Name, ex.Message + "Add", this.Name);
            }

        }

        private void btnExclude_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox2.Items.Count > 0)
                {
                    if (this.listBox2.SelectedIndex >= 0)
                    {
                        listBox1.Items.Add(listBox2.SelectedItem.ToString());
                        //if (this.listBox2.SelectedIndex >= 0)
                        Del(listBox2.SelectedItem.ToString());

                        this.listBox2.Items.RemoveAt(this.listBox2.SelectedIndex);

                        Edit = 1;

                        
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void cboRootNode_Click(object sender, EventArgs e)
        {
            //if(Edit != 0)
            //{
            //    if (MessageBox.Show("มีการแก้ไขเปลี่ยนแปลง Permisstion Screen"+ txtScreen.Text + "ต้องการบันทึกรายการก่อนหรือไม่ ?", "แก้ไข Permisstion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //    {
            //        AddUnit();
            //        Edit = 0;
            //    }
            //}
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            tb_MasterPermisstion_List a = new tb_MasterPermisstion_List();
            a.ShowDialog();
        }
    }
}
