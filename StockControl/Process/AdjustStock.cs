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
using Telerik.WinControls;
using Telerik.WinControls.Data;

namespace StockControl
{
    public partial class AdjustStock : Telerik.WinControls.UI.RadRibbonForm
    {
        public AdjustStock()
        {
            InitializeComponent();
        }

        public AdjustStock(string ADNo, string CodeNo,string Screen)
        {
            InitializeComponent();
            ADNo_tt = ADNo;
            CodeNo_tt = CodeNo;
            this.Screen = Screen;
        }
        string Screen = "";
        string ADNo_tt = "";
        string CodeNo_tt = "";
        string Ac = "";
        DataTable dt_ADH = new DataTable();
        DataTable dt_ADD = new DataTable();

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
            dt_ADD.Columns.Add(new DataColumn("AdjustNo", typeof(string)));
            dt_ADD.Columns.Add(new DataColumn("Seq", typeof(string)));
            dt_ADD.Columns.Add(new DataColumn("StockType", typeof(string)));
            dt_ADD.Columns.Add(new DataColumn("CodeNo", typeof(string)));
            dt_ADD.Columns.Add(new DataColumn("ItemNo", typeof(string)));
            dt_ADD.Columns.Add(new DataColumn("ItemDescription", typeof(string)));
            dt_ADD.Columns.Add(new DataColumn("RemainQty", typeof(decimal)));
            dt_ADD.Columns.Add(new DataColumn("QTY", typeof(decimal)));
            dt_ADD.Columns.Add(new DataColumn("Unit", typeof(string)));
            dt_ADD.Columns.Add(new DataColumn("PCSUnit", typeof(decimal)));
            dt_ADD.Columns.Add(new DataColumn("StandardCost", typeof(decimal)));
            dt_ADD.Columns.Add(new DataColumn("Amount", typeof(decimal)));
            dt_ADD.Columns.Add(new DataColumn("LotNo", typeof(string)));
            dt_ADD.Columns.Add(new DataColumn("Remark", typeof(string)));
            dt_ADD.Columns.Add(new DataColumn("id", typeof(int)));
            dt_ADD.Columns.Add(new DataColumn("RefJobCard", typeof(string)));
            dt_ADD.Columns.Add(new DataColumn("RefTempJobCard", typeof(string)));
            dt_ADD.Columns.Add(new DataColumn("RefidJobCard", typeof(string)));


            dt_ADH.Columns.Add(new DataColumn("id", typeof(int)));
            dt_ADH.Columns.Add(new DataColumn("ADNo", typeof(string)));
            dt_ADH.Columns.Add(new DataColumn("ADDate", typeof(DateTime)));
            dt_ADH.Columns.Add(new DataColumn("ADBy", typeof(string)));
            dt_ADH.Columns.Add(new DataColumn("Remark", typeof(string)));
            dt_ADH.Columns.Add(new DataColumn("CreateDate", typeof(DateTime)));
            dt_ADH.Columns.Add(new DataColumn("CreateBy", typeof(string)));
            dt_ADH.Columns.Add(new DataColumn("Status", typeof(string)));
            dt_ADH.Columns.Add(new DataColumn("UpdateDate", typeof(DateTime)));
            dt_ADH.Columns.Add(new DataColumn("UpdateBy", typeof(string)));
            dt_ADH.Columns.Add(new DataColumn("BarCode", typeof(Image)));
            


        }

        private void Unit_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                btnNew_Click(null, null);
                dgvData.AutoGenerateColumns = false;
                GETDTRow();

                DefaultItem();

                if (!ADNo_tt.Equals(""))
                {
                    btnNew.Enabled = true;
                    txtADNo.Text = ADNo_tt;
                    txtCodeNo.Text = "";
                    DataLoad();
                    Ac = "View";
                }
                else if (!CodeNo_tt.Equals(""))
                {
                    btnNew.Enabled = true;
                    txtCodeNo.Text = CodeNo_tt;
                    Insert_data(txtCodeNo.Text);
                    txtCodeNo.Text = "";
                }


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
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

                    GridViewMultiComboBoxColumn col = (GridViewMultiComboBoxColumn)dgvData.Columns["Location"];
                    col.DataSource = (from ix in db.tb_Locations.Where(s => Convert.ToBoolean(s.Active.Equals(true)) && s.Status == "Completed")
                                      select new { ix.Location }).ToList();

                    col.DisplayMember = "Location";
                    col.ValueMember = "Location";
                    col.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDown;
                    col.FilteringMode = GridViewFilteringMode.DisplayMember;

                    col.AutoSizeMode = BestFitColumnMode.DisplayedDataCells;
                    col.TextAlignment = ContentAlignment.MiddleCenter;
                    col.DropDownStyle = RadDropDownStyle.DropDownList;

                }
                catch { }

                try
                {
                    var a = (from ix in db.sp_045_ShelfNo_Select("") select ix).ToList();
                    //if (g.Count > 0)
                    //{
                    //GridViewMultiComboBoxColumn col = (GridViewMultiComboBoxColumn)dgvData.Columns["ShelfNo"];

                    //col.DataSource = g;

                    //col.DisplayMember = "ShelfNo";
                    //col.ValueMember = "ShelfNo";
                    //col.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDown;
                    //col.FilteringMode = GridViewFilteringMode.DisplayMember;

                    GridViewComboBoxColumn comboColumn = (GridViewComboBoxColumn)dgvData.Columns["ShelfNo"];

                    List<string> aaa = new List<string>();
                    aaa.AddRange(a.Select(o => o.ShelfNo));                    
                    comboColumn.DataSource = aaa;

                    ////}                 

                }
                catch { }
                //this.dgvData.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                //this.dgvData.CellEditorInitialized += radGridView1_CellEditorInitialized;
            }
        }
        private void DataLoad()
        {

            dt_ADH.Rows.Clear();
            dt_ADD.Rows.Clear();
            dgvData.Rows.Clear();
            dgvData.DataSource = null;
            try
            {

                this.Cursor = Cursors.WaitCursor;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {

                    try
                    {
                        var g = (from ix in db.tb_StockAdjustHs select ix).Where(a => a.ADNo == txtADNo.Text.Trim()).ToList();
                        if (g.Count() > 0)
                        {
                            DateTime? temp_date = null;
                           
                            txtRemark.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Remark);
                            if (!StockControl.dbClss.TSt(g.FirstOrDefault().ADDate).Equals(""))
                                dtRequire.Value = Convert.ToDateTime(g.FirstOrDefault().ADDate,new CultureInfo("en-US"));
                            else
                                dtRequire.Value = Convert.ToDateTime(temp_date);
                           

                            txtAdjustBy.Text = StockControl.dbClss.TSt(g.FirstOrDefault().ADBy);
                            txtCreateBy.Text = StockControl.dbClss.TSt(g.FirstOrDefault().CreateBy);
                            if (!StockControl.dbClss.TSt(g.FirstOrDefault().CreateDate).Equals(""))
                            {
                                    txtCreateDate.Text = Convert.ToDateTime(g.FirstOrDefault().CreateDate,new CultureInfo("en-US")).ToString("dd/MMM/yyyy");
                            }
                            else
                                txtCreateDate.Text = "";

                            //lblStatus.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Status);
                            if (StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Cancel"))
                            {
                                btnSave.Enabled = false;
                                //btnDelete.Enabled = false;
                                //btnView.Enabled = false;
                                //btnEdit.Enabled = false;
                                lblStatus.Text = "Cancel";
                                dgvData.ReadOnly = true;
                                btnNew.Enabled = true;
                            }
                           
                            else if
                                (StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Completed")
                                
                                )
                            {
                                btnSave.Enabled = false;
                                //btnDelete.Enabled = false;
                                //btnView.Enabled = false;
                                //btnEdit.Enabled = false;
                                lblStatus.Text = "Completed";
                                dgvData.ReadOnly = true;
                                btnNew.Enabled = true;
                            }
                            else
                            {
                                btnSave.Enabled = true;
                                //btnDelete.Enabled = true;
                                //btnView.Enabled = true;
                                //btnEdit.Enabled = true;
                                lblStatus.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Status);
                                dgvData.ReadOnly = false;
                                btnNew.Enabled = true;
                            }
                            dt_ADH = StockControl.dbClss.LINQToDataTable(g);


                            int dgvNo = 0;
                            //detail
                            var r = (from d in db.tb_StockAdjusts
                                     join i in db.tb_Items on d.CodeNo equals i.CodeNo
                                     where d.AdjustNo == txtADNo.Text

                                        && d.Status != "Cancel"

                                     select new
                                     {
                                         CodeNo = d.CodeNo,
                                         S = false,
                                         ItemNo = d.ItemNo,
                                         ItemDescription = d.ItemDescription,
                                        
                                         QTY = d.Qty,
                                         
                                         RemainQty = (Convert.ToDecimal(db.Cal_QTY(d.CodeNo, "", 0))),// i.StockInv,
                                         Unit = d.Unit,
                                         PCSUnit = d.PCSUnit,
                                         MaxStock = i.MaximumStock,
                                         MinStock = i.MinimumStock,
                                         StandardCost = d.StandardCost,//i.StandardCost,
                                         Amount =d.Amount,
                                         LotNo = d.LotNo,
                                         Remark = d.Reason,
                                         id = d.id,
                                         Location = d.Location,
                                         ShelfNo = d.ShelfNo
                                         ,RefJobCard = d.RefJobCard
                                         ,RefTempJobCard = d.RefTempJobCard
                                         ,RefidJobCard = d.RefidJobCard
                                     }
                            ).ToList();
                            if (r.Count > 0)
                            {
                                dgvNo = dgvData.Rows.Count() + 1;

                                foreach (var vv in r)
                                {
                                    //dgvData.Rows.Add(dgvNo.ToString(),
                                    //    vv.CodeNo,
                                    //    vv.ItemNo,
                                    //    vv.ItemDescription,
                                    //    vv.RemainQty,
                                    //    vv.QTY,
                                    //    vv.Unit,
                                    //    vv.PCSUnit,
                                    //    vv.StandardCost,
                                    //    vv.Amount,
                                    //    vv.LotNo,
                                    //    vv.Remark,
                                    //    vv.id,
                                    //    vv.ShelfNo,
                                    //    vv.Location
                                    //    ,vv.RefJobCard
                                    //    ,vv.RefTempJobCard
                                    //    ,vv.RefidJobCard
                                    //    );

                                    Add_Item(dgvNo, vv.CodeNo, vv.ItemNo, vv.ItemDescription, vv.RemainQty, dbClss.TDe( vv.QTY), vv.Unit, dbClss.TDe(vv.PCSUnit), dbClss.TDe(vv.StandardCost), dbClss.TDe(vv.Amount), vv.LotNo, vv.Remark,vv.id.ToString(),vv.ShelfNo, vv.Location, vv.RefJobCard,vv.RefTempJobCard, vv.RefidJobCard.ToString());
                                }

                            }
                            

                            ////Detail  แบบที่ สอง
                            //var d = (from ix in db.tb_StockAdjusts select ix)
                            //.Where(a => a.AdjustNo == txtADNo.Text.Trim()
                            //    && a.Status != "Cancel").ToList();
                            //if (d.Count() > 0)
                            //{
                            //    int c = 0;
                            //    dgvData.DataSource = d;
                                
                            //    dt_ADD = StockControl.dbClss.LINQToDataTable(d);
                            //    string SS = "";
                            //    foreach (var x in dgvData.Rows)
                            //    {
                            //        c += 1;
                            //        x.Cells["dgvNo"].Value = c;

                            //        //if (Convert.ToString(x.Cells["Status"].Value).Equals("Waiting"))
                            //        //{
                            //        //    x.Cells["QTY"].ReadOnly = false;
                            //        //    x.Cells["Unit"].ReadOnly = false;
                            //        //    x.Cells["PCSUnit"].ReadOnly = false;
                            //        //    x.Cells["StandardCost"].ReadOnly = false;
                            //        //    x.Cells["Remark"].ReadOnly = false;
                            //        //    x.Cells["LotNo"].ReadOnly = false;
                            //        //    x.Cells["Remark"].ReadOnly = false;
                            //        //}
                            //        //else if (Convert.ToString(x.Cells["Status"].Value).Equals("Completed")
                            //        //    || Convert.ToString(x.Cells["Status"].Value).Equals("Cancel")
                            //        //    )
                                        
                            //        //{
                            //        //    x.Cells["QTY"].ReadOnly = true;
                            //        //    x.Cells["Unit"].ReadOnly = true;
                            //        //    x.Cells["PCSUnit"].ReadOnly = true;
                            //        //    x.Cells["StandardCost"].ReadOnly = true;
                            //        //    x.Cells["Remark"].ReadOnly = true;
                            //        //    x.Cells["LotNo"].ReadOnly = true;
                            //        //    x.Cells["Remark"].ReadOnly = true;
                            //        //}
                            //    }
                            //}
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
            catch { }
            finally { this.Cursor = Cursors.Default; }


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

      
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DataLoad();
        }
        private void ClearData()
        {
            txtADNo.Text = "";
            txtAdjustBy.Text = ClassLib.Classlib.User;

            dtRequire.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
            txtCreateBy.Text = ClassLib.Classlib.User;
            txtCreateDate.Text = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy");
            txtRemark.Text = "";
           
            txtCodeNo.Text = "";
            
            dgvData.Rows.Clear();
            dgvData.DataSource = null;
            dt_ADH.Rows.Clear();
            dt_ADD.Rows.Clear();


        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            btnDel_Item.Enabled = true;
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            ClearData();
            Enable_Status(true, "New");
            lblStatus.Text = "New";
            Ac = "New";

            //getมาไว้ก่อน แต่ยังไมได้ save 
            txtADNo.Text = StockControl.dbClss.GetNo(7, 0);
        }
        private void Enable_Status(bool ss, string Condition)
        {
            if (Condition.Equals("-") || Condition.Equals("New"))
            {
                txtCodeNo.Enabled = ss;
                //txtVendorName.Enabled = ss;
                //txtRCNo.Enabled = ss;
                txtRemark.Enabled = ss;
                //txtTempNo.Enabled = ss;
                dtRequire.Enabled = ss;
                dgvData.ReadOnly = false;
               

            }
            else if (Condition.Equals("View"))
            {
                txtCodeNo.Enabled = ss;
                //txtVendorName.Enabled = ss;
                //txtRCNo.Enabled = ss;
                txtRemark.Enabled = ss;
                //txtTempNo.Enabled = ss;
                dtRequire.Enabled = ss;
                dgvData.ReadOnly = false;
                
            }

            else if (Condition.Equals("Edit"))
            {
                txtCodeNo.Enabled = ss;
                //txtVendorName.Enabled = ss;
                //txtRCNo.Enabled = ss;
                txtRemark.Enabled = ss;
                //txtTempNo.Enabled = ss;
                dtRequire.Enabled = ss;
                dgvData.ReadOnly = false;
               
            }
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
                //if (txtCodeNo.Text.Equals(""))
                //    err += " “รหัสพาร์ท:” เป็นค่าว่าง \n";
                //if (txtRCNo.Text.Equals(""))
                //    err += " “เลขที่รับสินค้า:” เป็นค่าว่าง \n";
                if (txtAdjustBy.Text.Equals(""))
                    err += "- “ผู้ร้องขอ:” เป็นค่าว่าง \n";
                //if (txtVendorNo.Text.Equals(""))
                //    err += "- “รหัสผู้ขาย:” เป็นค่าว่าง \n";
                if (dtRequire.Text.Equals(""))
                    err += "- “วันที่รับสินค้า:” เป็นค่าว่าง \n";
               
                if (dgvData.Rows.Count <= 0)
                    err += "- “รายการ:” เป็นค่าว่าง \n";
                int c = 0;
                foreach (GridViewRowInfo rowInfo in dgvData.Rows)
                {
                    if (rowInfo.IsVisible)
                    {
                        //if (StockControl.dbClss.TInt(rowInfo.Cells["QTY"].Value) != (0))
                        //{
                        c += 1;

                        if (StockControl.dbClss.TSt(rowInfo.Cells["CodeNo"].Value).Equals(""))
                            err += "- “รหัสพาร์ท:” เป็นค่าว่าง \n";
                        //if (StockControl.dbClss.TDe(rowInfo.Cells["QTY"].Value) <= 0)
                        //    err += "- “จำนวนรับ:” น้อยกว่า 0 \n";
                        if (StockControl.dbClss.TDe(rowInfo.Cells["Unit"].Value).Equals(""))
                            err += "- “หน่วย:” เป็นค่าว่าง \n";
                        if (StockControl.dbClss.TDe(rowInfo.Cells["Location"].Value).Equals(""))
                            err += "- “สถานที่เก็บ:” เป็นค่าว่าง \n";
                        //}
                    }
                }

                if (c <= 0)
                    err += "- “กรุณาระบุจำนวนที่จะปรับสต็อกสินค้า:” เป็นค่าว่าง \n";


                if (!err.Equals(""))
                    MessageBox.Show(err);
                else
                    re = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("AdjustStock", ex.Message, this.Name);
            }

            return re;
        }
        private void Save_Adjust()
        {
            if (Check_Save())
                return;
            if (MessageBox.Show("ต้องการบันทึก ?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Cursor = Cursors.WaitCursor;

                if (Ac.Equals("New"))
                    txtADNo.Text = StockControl.dbClss.GetNo(7, 2);

                if (!txtADNo.Text.Equals("")) //&& Ac.Equals("New"))
                {

                    SaveHerder();
                    SaveDetail();
                    string ADNo = txtADNo.Text;
                    ClearData();
                    txtADNo.Text = ADNo;


                    DataLoad();
                    btnNew.Enabled = true;
                    btnDel_Item.Enabled = false;

                    Insert_Stock_new();

                    MessageBox.Show("บันทึกสำเร็จ!");
                    DataLoad();
                }
                else
                {
                    MessageBox.Show("ไม่สามารถโหลดเลขที่รับสินค้าได้ ติดต่อแผนก IT");
                }
            }
        }
      
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Ac.Equals("New"))// || Ac.Equals("Edit"))
                {
                        Save_Adjust();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        private void Insert_Stock_new()
        {
            try
            {

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    DateTime? CalDate = null;
                    DateTime? AppDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    int Seq = 0;
                    string Type_in_out = "In";
                    decimal RemainQty = 0;
                    decimal Amount = 0;
                    decimal RemainAmount = 0;
                    decimal Avg = 0;
                    decimal UnitCost = 0;
                    decimal sum_Remain = 0;
                    decimal sum_Qty = 0;
                    decimal RemainUnitCost = 0;

                    //string Type = "";
                    string Category = "Invoice"; //Temp,Invoice
                   
                    var g = (from ix in db.tb_StockAdjusts
                                 //join i in db.tb_Items on ix.CodeNo equals i.CodeNo
                             where ix.AdjustNo.Trim() == txtADNo.Text.Trim() && ix.Status != "Cancel"

                             select ix).ToList();
                    if (g.Count > 0)
                    {
                        //insert Stock

                        foreach (var vv in g)
                        {
                            Seq += 1;

                            if (Convert.ToDecimal(vv.Qty) < 0)
                            {
                                //Ship out Stock
                                db.sp_041_tb_Adjust_Stock(txtADNo.Text, vv.CodeNo, Math.Abs(dbClss.TDe(vv.Qty)), ClassLib.Classlib.User, vv.RefJobCard, vv.RefTempJobCard, vv.RefidJobCard,vv.Location);
                            }
                            else
                            {
                                tb_Stock gg = new tb_Stock();
                                gg.CodeNo = vv.CodeNo;
                                gg.AppDate = AppDate;
                                gg.Seq = Seq;
                                gg.App = "Adjust";
                                gg.Appid = Seq;
                                gg.CreateBy = ClassLib.Classlib.User;
                                gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                gg.DocNo = txtADNo.Text;
                                gg.RefNo = "";
                                gg.Type = "Adjust";
                                gg.QTY = Convert.ToDecimal(vv.Qty) * dbClss.TDe(vv.PCSUnit);
                                gg.Location = vv.Location;
                                gg.ShelfNo = vv.ShelfNo;
                                //if (Convert.ToDecimal(vv.Qty) < 0)
                                //{
                                //    gg.Outbound = Convert.ToDecimal(vv.Qty);
                                //    gg.Inbound = 0;
                                //    Type_in_out = "Out";

                                //    UnitCost = Convert.ToDecimal(vv.StandardCost);//Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "Avg"));
                                //    Amount = Convert.ToDecimal(vv.Qty) * UnitCost;

                                //    //แบบที่ 1 จะไป sum ใหม่
                                //    RemainQty = (Convert.ToDecimal(db.Cal_QTY(vv.CodeNo, "", 0)));
                                //    //แบบที่ 2 จะไปดึงล่าสุดมา
                                //    //RemainQty = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainQty"));
                                //    sum_Remain = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainAmount"))
                                //        + Amount;

                                //    sum_Qty = RemainQty + Convert.ToDecimal(vv.Qty);
                                //    //Avg = UnitCost;//sum_Remain / sum_Qty;
                                //    RemainAmount = sum_Remain;
                                //    if (sum_Qty <= 0)
                                //        RemainUnitCost = 0;
                                //    else
                                //        RemainUnitCost = Math.Round((Math.Abs(RemainAmount) / Math.Abs(sum_Qty)), 2);


                                //    gg.TLCost = Math.Abs(Amount);
                                //    gg.TLQty = 0;
                                //    gg.ShipQty = Math.Abs(Convert.ToDecimal(vv.Qty));
                                //}
                                //else
                                {
                                    gg.Inbound = Convert.ToDecimal(vv.Qty)*dbClss.TDe(vv.PCSUnit);
                                    gg.Outbound = 0;
                                    Type_in_out = "In";

                                    Amount = Convert.ToDecimal(vv.Qty) * Convert.ToDecimal(vv.StandardCost);
                                    UnitCost = Math.Round((Amount / (Convert.ToDecimal(vv.Qty)*dbClss.TDe(vv.PCSUnit))),2);

                                    //แบบที่ 1 จะไป sum ใหม่
                                    RemainQty = (Convert.ToDecimal(db.Cal_QTY_Remain_Location(vv.CodeNo, "", 0,vv.Location)));
                                    //แบบที่ 2 จะไปดึงล่าสุดมา
                                    //RemainQty = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainQty"));

                                    sum_Remain = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainAmount",vv.Location))
                                        + Amount;

                                    sum_Qty = RemainQty + ( Convert.ToDecimal(vv.Qty) * dbClss.TDe(vv.PCSUnit));
                                    //Avg = sum_Remain / sum_Qty;
                                    //RemainAmount = sum_Qty * Avg;
                                    RemainAmount = sum_Remain;
                                    if (sum_Qty <= 0)
                                        RemainUnitCost = 0;
                                    else
                                        RemainUnitCost = Math.Round((Math.Abs(RemainAmount) / Math.Abs(sum_Qty)), 2);


                                    gg.TLCost = Math.Abs(Amount);
                                    gg.TLQty = Math.Abs(Convert.ToDecimal(vv.Qty) * dbClss.TDe(vv.PCSUnit));
                                    gg.ShipQty = 0;
                                }


                                gg.CalDate = CalDate;
                                gg.Status = "Active";

                                gg.Type_i = 5;  //Receive = 1,Cancel Receive 2,Shipping = 3,Cancel Shipping = 4,Adjust stock = 5,ClearTemp = 6
                                gg.Category = Category;
                                gg.Refid = 0;
                                gg.RefidAD = vv.id;

                                gg.Flag_ClearTemp = 0;   //0 คือ invoice,1 คือ Temp , 2 คือ clear temp แล้ว

                                gg.Type_in_out = Type_in_out;
                                gg.AmountCost = Amount;
                                gg.UnitCost = UnitCost;
                                gg.RemainQty = sum_Qty;
                                gg.RemainUnitCost = RemainUnitCost;
                                gg.RemainAmount = RemainAmount;
                                gg.Avg = 0;// Avg;                            
                                gg.RefidJobCode = vv.RefidJobCard;
                                gg.RefJobCode = vv.RefJobCard;
                                gg.RefTempJobCode = vv.RefTempJobCard;

                                db.tb_Stocks.InsertOnSubmit(gg);
                                db.SubmitChanges();

                                //update item
                                //dbClss.Insert_Stock(vv.CodeNo, (Convert.ToDecimal(vv.Qty)), "Adjust", "Inv");

                                //update Stock เข้า item
                                db.sp_010_Update_StockItem(Convert.ToString(vv.CodeNo), "");
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    
        private void Insert_Stock()
        {
            try
            {

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    DateTime? CalDate = null;
                    DateTime? AppDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    int Seq = 0;



                    string CNNo = CNNo = StockControl.dbClss.GetNo(6, 2);
                    var g = (from ix in db.tb_StockAdjusts
                                 //join i in db.tb_Items on ix.CodeNo equals i.CodeNo
                             where ix.AdjustNo.Trim() == txtADNo.Text.Trim() && ix.Status != "Cancel"

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
                            gg.App = "Adjust";
                            gg.Appid = Seq;
                            gg.CreateBy = ClassLib.Classlib.User;
                            gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                            gg.DocNo = CNNo;
                            gg.RefNo = txtADNo.Text;
                            gg.Type = "Adjust";
                            gg.QTY = Convert.ToDecimal(vv.Qty);
                            if (Convert.ToDecimal(vv.Qty) < 0)
                            {
                                gg.Outbound = Convert.ToDecimal(vv.Qty);
                                gg.Inbound = 0;
                            }
                            else
                            {
                                gg.Inbound = Convert.ToDecimal(vv.Qty);
                                gg.Outbound = 0;
                            }

                            gg.AmountCost = (Convert.ToDecimal(vv.Qty)) * get_cost(vv.CodeNo);
                            gg.UnitCost = get_cost(vv.CodeNo);
                            gg.RemainQty = 0;
                            gg.RemainUnitCost = 0;
                            gg.RemainAmount = 0;
                            gg.CalDate = CalDate;
                            gg.Status = "Active";

                            db.tb_Stock1s.InsertOnSubmit(gg);
                            db.SubmitChanges();

                            dbClss.Insert_Stock(vv.CodeNo, (Convert.ToDecimal(vv.Qty)), "Adjust", "Inv");


                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
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
        private void SaveHerder()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                var g = (from ix in db.tb_StockAdjustHs
                         where ix.ADNo.Trim() == txtADNo.Text.Trim() && ix.Status != "Cancel"
                         //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                         select ix).ToList();
                if (g.Count > 0)  //มีรายการในระบบ
                {
                    foreach (DataRow row in dt_ADH.Rows)
                    {
                        var gg = (from ix in db.tb_StockAdjustHs
                                  where ix.ADNo.Trim() == txtADNo.Text.Trim() && ix.Status != "Cancel"
                                  //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                                  select ix).First();

                        gg.UpdateBy = ClassLib.Classlib.User;
                        gg.UpdateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                        dbClss.AddHistory(this.Name,"ปรับสต็อก", "แก้ไขโดย [" + ClassLib.Classlib.User + " วันที่ :" + Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy") + "]", txtADNo.Text);

                        if (StockControl.dbClss.TSt(gg.BarCode).Equals(""))
                            gg.BarCode = StockControl.dbClss.SaveQRCode2D(txtADNo.Text.Trim());

                        
                        if (!txtAdjustBy.Text.Trim().Equals(row["ADBy"].ToString()))
                        {
                            gg.ADBy = txtAdjustBy.Text.Trim();
                            dbClss.AddHistory(this.Name, "ปรับสต็อก", "แก้ไข ผู้ร้องขอ[" + txtAdjustBy.Text.Trim() + " เดิม :" + row["ADBy"].ToString() + "]", txtADNo.Text);
                        }
                        if (!txtRemark.Text.Trim().Equals(row["Remark"].ToString()))
                        {
                            gg.Remark = txtRemark.Text.Trim();
                            dbClss.AddHistory(this.Name , "ปรับสต็อก", "แก้ไขหมายเหตุ [" + txtRemark.Text.Trim() + " เดิม :" + row["Remark"].ToString() + "]", txtADNo.Text);
                        }
                       
                        
                        if (!dtRequire.Text.Trim().Equals(""))
                        {
                            string date1 = "";
                            date1 = dtRequire.Value.ToString("yyyyMMdd", new CultureInfo("en-US"));
                            string date2 = "";
                            DateTime temp = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                            if (!StockControl.dbClss.TSt(row["ADDate"].ToString()).Equals(""))
                            {

                                temp = Convert.ToDateTime(row["ADDate"]);
                                date2 = temp.ToString("yyyyMMdd", new CultureInfo("en-US"));

                            }
                            if (!date1.Equals(date2))
                            {
                                DateTime? RequireDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                if (!dtRequire.Text.Equals(""))
                                    RequireDate = dtRequire.Value;
                                gg.ADDate = RequireDate;
                                dbClss.AddHistory(this.Name, "ปรับสต็อก", "แก้ไขวันที่ปรับสต็อกสินค้า [" + dtRequire.Text.Trim() + " เดิม :" + temp.ToString() + "]", txtADNo.Text);
                            }
                        }
                        db.SubmitChanges();
                    }
                }
                else //สร้างใหม่
                {
                    byte[] barcode = null;
                    //barcode = StockControl.dbClss.SaveQRCode2D(txtADNo.Text.Trim());
                    DateTime? UpdateDate = null;

                    DateTime? RequireDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    if (!dtRequire.Text.Equals(""))
                        RequireDate = dtRequire.Value;

                    tb_StockAdjustH gg = new tb_StockAdjustH();
                    gg.ADNo = txtADNo.Text;
                    gg.ADBy = txtAdjustBy.Text;
                    gg.ADDate = RequireDate;
                    gg.UpdateBy = null;
                    gg.UpdateDate = UpdateDate;
                    gg.CreateBy = ClassLib.Classlib.User;
                    gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    gg.Remark = txtRemark.Text;
                    gg.BarCode = barcode;
                    gg.Status = "Completed";
                    db.tb_StockAdjustHs.InsertOnSubmit(gg);
                    db.SubmitChanges();

                    dbClss.AddHistory(this.Name , "ปรับสต็อก", "สร้าง การปรับสต็อกสินค้า [" + txtADNo.Text.Trim() + "]", txtADNo.Text);
                }
            }
        }
        private void SaveDetail()
        {
            dgvData.EndEdit();

            string ADNo = txtADNo.Text;
            DateTime? RequireDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
            if (!dtRequire.Text.Equals(""))
                RequireDate = dtRequire.Value;
            int Seq = 0;
            DateTime? UpdateDate = null;
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                foreach (var g in dgvData.Rows)
                {
                    string SS = "";
                    if (g.IsVisible.Equals(true))
                    {
                        if (StockControl.dbClss.TInt(g.Cells["QTY"].Value) != (0)) // เอาเฉพาะรายการที่ไม่เป็น 0 
                        {
                            if (StockControl.dbClss.TInt(g.Cells["id"].Value) <= 0)  //New ใหม่
                            {
                               
                                Seq += 1;
                                tb_StockAdjust u = new tb_StockAdjust();
                                u.AdjustNo = txtADNo.Text;
                               
                                u.CodeNo = StockControl.dbClss.TSt(g.Cells["CodeNo"].Value);
                                u.ItemNo = StockControl.dbClss.TSt(g.Cells["ItemNo"].Value);
                                u.ItemDescription = StockControl.dbClss.TSt(g.Cells["ItemDescription"].Value);
                                u.Qty = StockControl.dbClss.TDe(g.Cells["QTY"].Value);
                                u.PCSUnit = StockControl.dbClss.TDe(g.Cells["PCSUnit"].Value);
                                u.Unit = StockControl.dbClss.TSt(g.Cells["Unit"].Value);
                                u.Amount = StockControl.dbClss.TDe(g.Cells["Amount"].Value);
                                u.Reason = StockControl.dbClss.TSt(g.Cells["Remark"].Value);                              
                                u.LotNo = StockControl.dbClss.TSt(g.Cells["LotNo"].Value);                               
                                //u.RCDate = RequireDate;
                                u.Seq = Seq;
                                u.Status = "Completed";
                                u.StandardCost = StockControl.dbClss.TDe(g.Cells["StandardCost"].Value);

                                u.CreateBy = ClassLib.Classlib.User;
                                u.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                                u.Location = StockControl.dbClss.TSt(g.Cells["Location"].Value);
                                u.ShelfNo = StockControl.dbClss.TSt(g.Cells["ShelfNo"].Value);

                                u.RefidJobCard = StockControl.dbClss.TInt(g.Cells["RefidJobCard"].Value);
                                u.RefJobCard = StockControl.dbClss.TSt(g.Cells["RefJobCard"].Value);
                                u.RefTempJobCard = StockControl.dbClss.TSt(g.Cells["RefTempJobCard"].Value);
                                
                                db.tb_StockAdjusts.InsertOnSubmit(u);
                                db.SubmitChanges();

                                ////// update Remainใน tb_receive ที่เป็น PRID เดียวกัน ทั้งหมด
                                //update_remainqty_Receive(PRNo, Temp, PRID, RemainQty);

                                //////หมายถึงรับสินค้าครบหมดแล้ว ให้ไป เปลี่ยนสถาะ เป็น Completed ทุกตัวที่เป็น PRID เดียวกัน
                                //if (!SS.Equals(""))
                                //    Save_Status_Receive(PRNo, Temp, PRID, RemainQty);

                                //C += 1;
                                dbClss.AddHistory(this.Name , "ปรับสต็อก", "เพิ่มรายการ ปรับสต็อก [" + u.CodeNo + " จำนวนรับ :" + u.Qty.ToString()  + "]", txtADNo.Text);
                                
                            }
                           

                        }
                    }
                }
            }
        }
        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                dgvData.EndEdit();
                if (e.RowIndex >= -1)
                {
                    //if (dgvData.Columns["CodeNo"].Index == e.ColumnIndex)
                    //{

                    //}
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

                    if (dgvData.Columns["RefJobCard"].Index == e.ColumnIndex)
                    {
                        string JobCard = dbClss.TSt(e.Row.Cells["RefJobCard"].Value);
                        if (JobCard != "")
                        {
                            using (DataClasses1DataContext db = new DataClasses1DataContext())
                            {
                                var g = (from ix in db.tb_JobCards select ix)
                               .Where(a => a.JobCard == JobCard.Trim()).ToList();
                                if (g.Count() > 0)
                                {
                                    e.Row.Cells["RefTempJobCard"].Value = dbClss.TSt(g.FirstOrDefault().TempJobCard);
                                    e.Row.Cells["RefidJobCard"].Value = dbClss.TInt(g.FirstOrDefault().id);
                                }
                                else
                                {
                                    e.Row.Cells["RefJobCard"].Value = "";
                                    e.Row.Cells["RefTempJobCard"].Value = "";
                                    e.Row.Cells["RefidJobCard"].Value = 0;
                                }
                            }
                        }
                        else
                        {
                            e.Row.Cells["RefJobCard"].Value = "";
                            e.Row.Cells["RefTempJobCard"].Value = "";
                            e.Row.Cells["RefidJobCard"].Value = 0;
                        }
                    }
                    else if (dgvData.Columns["ShelfNo"].Index == e.ColumnIndex)
                    {
                        var cc = e.Row.Cells["ShelfNo"];
                        string CategoryTemp = Convert.ToString(e.Row.Cells["ShelfNo"].Value);
                        try
                        {
                            if (!CategoryTemp.Equals(ShelfNo_Edit) && !ShelfNo_Edit.Equals(""))
                            {
                                (e.Row.Cells["ShelfNo"].Value) = ShelfNo_Edit;
                                ShelfNo_Edit = "";
                            }
                        }
                        catch { }
                    }

                    else if (dgvData.Columns["QTY"].Index == e.ColumnIndex
                        || dgvData.Columns["StandardCost"].Index == e.ColumnIndex
                        )
                    {
                        decimal QTY = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["QTY"].Value), out QTY);
                        decimal CostPerUnit = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["StandardCost"].Value), out CostPerUnit);

                        //if (QTY<0)
                        //{
                        //    CostPerUnit =  Convert.ToDecimal(dbClss.Get_Stock(StockControl.dbClss.TSt(e.Row.Cells["CodeNo"].Value), "", "", "Avg"));
                        //    e.Row.Cells["StandardCost"].Value = CostPerUnit;
                        //}


                        e.Row.Cells["Amount"].Value = Math.Round(QTY * CostPerUnit);
                        //Cal_Amount();
                    }
                    else if (dgvData.Columns["Location"].Index == e.ColumnIndex)
                    {
                        using (DataClasses1DataContext db = new DataClasses1DataContext())
                        {
                            e.Row.Cells["RemainQty"].Value = (Convert.ToDecimal(db.Cal_QTY_Remain_Location(Convert.ToString(e.Row.Cells["CodeNo"].Value), "Invoice", 0, Convert.ToString(e.Row.Cells["Location"].Value))));
                        }
                    }
                }

            }
            catch (Exception ex) { }
        }
        string ShelfNo_Edit = "";

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

        private void cboYear_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            
        }

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
                        decimal RemainQty = 0;
                        string Unit = "";
                        decimal PCSUnit = 0;
                        decimal CostPerUnit = 0;
                        decimal Amount = 0;
                        //string CRRNCY = "";
                        string LotNo = "";
                        string Location = "";
                        string Remark = "";
                        string ShelfNo = "";
                        int duppicate_CodeNo = 0;
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
                            RemainQty = (Convert.ToDecimal(db.Cal_QTY_Remain_Location(Convert.ToString(CodeNo), "Invoice", 0,Convert.ToString(d.Location))));//Convert.ToDecimal(d.StockInv);
                            Unit = d.UnitBuy;
                            PCSUnit = Convert.ToDecimal(d.PCSUnit);
                            CostPerUnit = Convert.ToDecimal(d.StandardCost); // Convert.ToDecimal(dbClss.Get_Stock(CodeNo, "", "", "Avg"));//Convert.ToDecimal(d.StandardCost);
                            Location = d.Location;
                            No = dgvData.Rows.Count() + 1;
                            ShelfNo = d.ShelfNo;
                            if (!check_Duppicate(CodeNo))
                            {
                                //dgvData.Rows.Add(No,
                                //                    CodeNo,
                                //                    ItemNo,
                                //                    ItemDescription,
                                //                    RemainQty,
                                //                    QTY,
                                //                    Unit,
                                //                    PCSUnit,
                                //                    CostPerUnit,
                                //                    Amount,
                                //                    LotNo,
                                //                    Remark,
                                //                    "0"
                                //                    ,""
                                //                    , Location
                                //                    , ""
                                //                    ,""
                                //                    ,"0"
                                //                    );

                                Add_Item(No, CodeNo, ItemNo, ItemDescription, RemainQty, QTY, Unit, PCSUnit, CostPerUnit, Amount, LotNo, Remark, "0", ShelfNo, Location, "", "", "0");
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        private void Add_Item(int Row, string CodeNo, string ItemNo
            , string ItemDescription ,decimal RemainQty
           , decimal QTY, string Unit,decimal PCSUnit, decimal StandardCost,decimal Amount,string LotNo,string Remark
            ,string id ,string ShelfNo,string Location,string RefJobCard,string RefTempJobCard,string RefidJobCard)
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
                ee.Cells["RemainQty"].Value = RemainQty;
                ee.Cells["QTY"].Value = QTY;
                ee.Cells["PCSUnit"].Value = PCSUnit;
                ee.Cells["Unit"].Value = Unit;
                ee.Cells["StandardCost"].Value = StandardCost;
                ee.Cells["Amount"].Value = Amount;//Math.Round((OrderQty * StandardCost), 2);
                ee.Cells["id"].Value = id;
                ee.Cells["LotNo"].Value = LotNo;
                ee.Cells["Remark"].Value = Remark;
                ee.Cells["Location"].Value = Location;
                ee.Cells["RefJobCard"].Value = RefJobCard;
                ee.Cells["RefTempJobCard"].Value = RefTempJobCard;
                ee.Cells["RefidJobCard"].Value = RefidJobCard;
                ee.Cells["ShelfNo"].Value = ShelfNo;

                //dbclass.SetRowNo1(dgvData);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : Add_Item", this.Name); }

        }

        private bool check_Duppicate(string CodeNo)
        {
            bool re = false;
            foreach (var rd1 in dgvData.Rows)
            {
                if (StockControl.dbClss.TSt(rd1.Cells["CodeNo"].Value).Equals(CodeNo))
                    re = true;
            }

            return re;

        }

        private void btnListitem_Click(object sender, EventArgs e)
        {
            try
            {
                btnDel_Item.Enabled = false;
                btnSave.Enabled = false;
                //btnEdit.Enabled = true;
                //btnView.Enabled = false;
                btnNew.Enabled = true;
                ClearData();
                Enable_Status(false, "View");

                this.Cursor = Cursors.WaitCursor;
                AdjustStock_List sc = new AdjustStock_List(txtADNo, txtCodeNo);
                this.Cursor = Cursors.Default;
                sc.ShowDialog();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();
                //LoadData

                string CodeNo = txtCodeNo.Text;
                string ADNo = txtADNo.Text;
                if (!txtADNo.Text.Equals(""))
                {
                    txtCodeNo.Text = "";
                    DataLoad();
                    Ac = "View";
                    
                }
                else
                {

                    btnNew_Click(null, null);
                    txtCodeNo.Text = CodeNo;

                    Insert_data(txtCodeNo.Text);
                    txtCodeNo.Text = "";

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : radButtonElement1_Click", this.Name); }

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

                    int id = 0;
                    int.TryParse(StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["id"].Value), out id);
                    if (id <= 0)
                        dgvData.Rows.Remove(dgvData.CurrentRow);

                    else
                    {
                        string CodeNo = ""; CodeNo = StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["CodeNo"].Value);
                        if (MessageBox.Show("ต้องการลบรายการ ( " + CodeNo + " ) ออกจากรายการ หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            dgvData.CurrentRow.IsVisible = false;
                        }
                    }
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

        private void radButtonElement1_Click(object sender, EventArgs e)
        {
            try
            {
                PrintPR a = new PrintPR(txtADNo.Text, txtADNo.Text, "AdjustStock");
                a.ShowDialog();

                //using (DataClasses1DataContext db = new DataClasses1DataContext())
                //{
                //    var g = (from ix in db.sp_R004_ReportShipping(txtSHNo.Text, Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"))) select ix).ToList();
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

    

        private void btnAdd_Item_Click(object sender, EventArgs e)
        {
            try
            {
              
                this.Cursor = Cursors.WaitCursor;
                List<GridViewRowInfo> dgvRow_List = new List<GridViewRowInfo>();
                ListPart sc = new ListPart(dgvRow_List, "All", "AdjustStock");
                
                sc.ShowDialog();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();

              
                if (dgvRow_List.Count > 0)
                {
                    string CodeNo = "";
                   
                    foreach (GridViewRowInfo ee in dgvRow_List)
                    {
                        CodeNo = Convert.ToString(ee.Cells["CodeNo"].Value).Trim();
                        txtCodeNo.Text = CodeNo;
                        if (txtCodeNo.Text != "")
                        {
                            Insert_data(txtCodeNo.Text);
                        }
                        txtCodeNo.Text = "";

                    }
                }


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : btnAdd_Item_Click", this.Name); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void MasterTemplate_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            try
            {
                if (e.Column.Name.Equals("Location"))
                {
                    /////////////มีการ เคลียร์ การ Add ก่อน แล้วค่อย Add ใหม่////////////////
                    //Row = e.RowIndex;
                    RadMultiColumnComboBoxElement Comcol = (RadMultiColumnComboBoxElement)e.ActiveEditor;
                    Comcol.Columns.Clear();

                    //RadMultiColumnComboBoxElement Comcol = (RadMultiColumnComboBoxElement)e.ActiveEditor;
                    Comcol.DropDownSizingMode = SizingMode.UpDownAndRightBottom;
                    Comcol.DropDownWidth = 100;
                    Comcol.DropDownHeight = 80;
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
                        HeaderText = "สถานที่เก็บ",
                        Name = "Location",
                        FieldName = "Location",
                        Width = 100,
                        AllowFiltering = true,
                        ReadOnly = false
                    }
                   );
                    // Comcol.EditorControl.Columns.Add(new GridViewTextBoxColumn
                    // {
                    //     HeaderText = "Description",
                    //     Name = "Description",
                    //     FieldName = "Description",
                    //     Width = 300,
                    //     AllowFiltering = true,
                    //     ReadOnly = false

                    // }
                    //);


                    //dgvDataDetail.CellEditorInitialized += MasterTemplate_CellEditorInitialized;

                }
            }
            catch { }
        }

        private void MasterTemplate_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {

            RadMultiColumnComboBoxElement mccbEl = e.ActiveEditor as RadMultiColumnComboBoxElement;
            if (mccbEl != null)
            {
                mccbEl.DropDownSizingMode = SizingMode.UpDownAndRightBottom;
                mccbEl.DropDownMinSize = new Size(150, 100);
                mccbEl.DropDownMaxSize = new Size(150, 100);

                mccbEl.AutoSizeDropDownToBestFit = false;
                mccbEl.DropDownAnimationEnabled = false;
                mccbEl.AutoFilter = true;
                FilterDescriptor filterDescriptor = new FilterDescriptor(mccbEl.DisplayMember, FilterOperator.Contains, string.Empty);
                mccbEl.EditorControl.MasterTemplate.FilterDescriptors.Add(filterDescriptor);
            }

        }

        private void MasterTemplate_EditorRequired(object sender, EditorRequiredEventArgs e)
        {
            GridViewEditManager manager = sender as GridViewEditManager;
            // Assigning DropDownListAddEditor to the right column
           
             if (manager.GridViewElement.CurrentColumn.Name == "ShelfNo")
            {
                DropDownListAddEditor editor = new DropDownListAddEditor();
                editor.InputValueNotFound += new DropDownListAddEditor.InputValueNotFoundHandler(DropDownListAddEditor_InputValueNotFoundCategory_Edit);
                e.Editor = editor;
            }
           
        }
        private void DropDownListAddEditor_InputValueNotFoundCategory_Edit(object sender, DropDownListAddEditor.InputValueNotFoundArgs e)
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
                        ShelfNo_Edit = e.Text;
                    values.Add(e.Text);
                    e.Value = e.Text;
                    e.ValueAdded = true;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
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
    }
}
