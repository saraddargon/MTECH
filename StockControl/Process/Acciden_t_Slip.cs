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
using ClassLib;

namespace StockControl
{
    public partial class Acciden_t_Slip : Telerik.WinControls.UI.RadRibbonForm
    {
        public Acciden_t_Slip()
        {
            InitializeComponent();
        }
        public Acciden_t_Slip(string SHNo)
        {
            InitializeComponent();
            SHNo_t = SHNo;
           
        }
        public Acciden_t_Slip(string SHNo,string CodeNo)
        {
            InitializeComponent();
            SHNo_t = SHNo;
            CodeNo_t = CodeNo;
        }
        string SHNo_t = "";
        string CodeNo_t = "";
        string Ac = "";
        DataTable dt_h = new DataTable();
        DataTable dt_d = new DataTable();

        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            HistoryView hw = new HistoryView(this.Name, txtSHNo.Text);
            this.Cursor = Cursors.Default;
            hw.ShowDialog();
        }

        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }
        private void GETDTRow()
        {
            dt_h.Columns.Add(new DataColumn("ShippingNo", typeof(string)));
            dt_h.Columns.Add(new DataColumn("ShipName", typeof(string)));
            dt_h.Columns.Add(new DataColumn("ShipDate", typeof(DateTime)));
            dt_h.Columns.Add(new DataColumn("CreateDate", typeof(DateTime)));
            dt_h.Columns.Add(new DataColumn("CreateBy", typeof(string)));
            dt_h.Columns.Add(new DataColumn("UpdateDate", typeof(DateTime)));
            dt_h.Columns.Add(new DataColumn("UpdateBy", typeof(string)));
            dt_h.Columns.Add(new DataColumn("Remark", typeof(string)));
            dt_h.Columns.Add(new DataColumn("Status", typeof(string)));
            dt_h.Columns.Add(new DataColumn("JobCard", typeof(string)));
            dt_h.Columns.Add(new DataColumn("TempJobCard", typeof(string)));
            dt_h.Columns.Add(new DataColumn("Location", typeof(string)));
            dt_h.Columns.Add(new DataColumn("ToLocation", typeof(string)));

            dt_d.Columns.Add(new DataColumn("ShippingNo", typeof(string)));
            dt_d.Columns.Add(new DataColumn("ShipType", typeof(string)));
            dt_d.Columns.Add(new DataColumn("Seq", typeof(int)));          
            dt_d.Columns.Add(new DataColumn("CodeNo", typeof(string)));
            dt_d.Columns.Add(new DataColumn("ItemNo", typeof(string)));
            dt_d.Columns.Add(new DataColumn("ItemDescription", typeof(string)));
            dt_d.Columns.Add(new DataColumn("QTY", typeof(decimal)));
            dt_d.Columns.Add(new DataColumn("Remark", typeof(string)));
            dt_d.Columns.Add(new DataColumn("LineName", typeof(string)));
            dt_d.Columns.Add(new DataColumn("MachineName", typeof(string)));
            dt_d.Columns.Add(new DataColumn("UnitShip", typeof(string)));
            dt_d.Columns.Add(new DataColumn("PCSUnit", typeof(decimal)));
            dt_d.Columns.Add(new DataColumn("SerialNo", typeof(string)));
            dt_d.Columns.Add(new DataColumn("LotNo", typeof(string)));
            dt_d.Columns.Add(new DataColumn("Calbit", typeof(bool)));
            dt_d.Columns.Add(new DataColumn("ClearFlag", typeof(bool)));
            dt_d.Columns.Add(new DataColumn("ClearDate", typeof(bool)));
            dt_d.Columns.Add(new DataColumn("Status", typeof(string)));
            dt_d.Columns.Add(new DataColumn("BarCode", typeof(Image)));
            dt_d.Columns.Add(new DataColumn("Location", typeof(string)));
            dt_d.Columns.Add(new DataColumn("ToLocation", typeof(string)));
            dt_d.Columns.Add(new DataColumn("idProductionOrderRM", typeof(string)));
            

        }

        private void Unit_Load(object sender, EventArgs e)
        {
            //dgvData.ReadOnly = true;
            dgvData.AutoGenerateColumns = false;
            GETDTRow();
   
            DefaultItem();
            
            btnNew_Click(null, null);
            
            //Add_JobCard();

            if (!SHNo_t.Equals(""))
            {
                btnNew.Enabled = true;
                txtSHNo.Text = SHNo_t;
              
                DataLoad();
                Ac = "View";
               
            }
            //else if (!CodeNo_t.Equals(""))
            //{
            //    btnNew.Enabled = true;
            //    txtCodeNo.Text = CodeNo_t;
            //    Insert_data_New();
            //    txtCodeNo.Text = "";
            //}

        }
        private void DefaultItem()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {



                //try
                //{

                //    GridViewMultiComboBoxColumn col = (GridViewMultiComboBoxColumn)dgvData.Columns["Location"];
                //    col.DataSource = (from ix in db.mh_Locations.Where(s => Convert.ToBoolean(s.Active.Equals(true)) && s.Active == true)
                //                      select new { ix.Code, ix.Name }).ToList();

                //    col.DisplayMember = "Code";
                //    col.ValueMember = "Code";
                //    col.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDown;
                //    col.FilteringMode = GridViewFilteringMode.DisplayMember;

                //    col.AutoSizeMode = BestFitColumnMode.DisplayedDataCells;
                //    col.TextAlignment = ContentAlignment.MiddleCenter;
                //    col.DropDownStyle = RadDropDownStyle.DropDownList;

                //}
                try
                {
                    GridViewMultiComboBoxColumn Uom = (GridViewMultiComboBoxColumn)dgvData.Columns["UnitShip"];
                    Uom.DataSource = (from ix in db.mh_Units.Where(s => s.UnitActive == true)
                                      select new { ix.UnitCode }).ToList();
                    Uom.DisplayMember = "UnitCode";
                    Uom.DropDownStyle = RadDropDownStyle.DropDown;
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
          
            dt_h.Rows.Clear();
            dt_d.Rows.Clear();
            try
            {

                this.Cursor = Cursors.WaitCursor;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {

                    try
                    {
                        var g = (from ix in db.tb_ShippingHs select ix).Where(a => a.ShippingNo == txtSHNo.Text.Trim()).ToList();
                        if (g.Count() > 0)
                        {
                            DateTime? temp_date = null;
                            txtRemark.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Remark);
                            
                            //txtTempJobCard.Text = StockControl.dbClss.TSt(g.FirstOrDefault().TempJobCard);
                            txtJobCard.Text = StockControl.dbClss.TSt(g.FirstOrDefault().JobCard);
                            //txtLocation.Text = dbClss.TSt(g.FirstOrDefault().ToLocation);

                            if (txtJobCard.Text != "")
                            {
                                cbShipforJob.Checked = true;
                                var p = (from ix in db.mh_ProductionOrders select ix)
                                    .Where
                                    (a => a.JobNo.Trim().ToUpper() == txtJobCard.Text.Trim().ToUpper() && a.Active == true
                                    ).ToList();
                                if (p.Count > 0)
                                {
                                    txtRefidJobNo.Text = dbClss.TSt(p.FirstOrDefault().id);
                                }
                            }
                            else
                                cbShipforJob.Checked = false;

                            if (!StockControl.dbClss.TSt(g.FirstOrDefault().ShipDate).Equals(""))
                                dtRequire.Value = Convert.ToDateTime(g.FirstOrDefault().ShipDate);
                            else
                                dtRequire.Value = Convert.ToDateTime(temp_date);


                            txtCreateBy.Text = StockControl.dbClss.TSt(g.FirstOrDefault().CreateBy);
                            if (!StockControl.dbClss.TSt(g.FirstOrDefault().UpdateBy).Equals(""))
                                txtCreateBy.Text = StockControl.dbClss.TSt(g.FirstOrDefault().UpdateBy);
                            if (!StockControl.dbClss.TSt(g.FirstOrDefault().CreateDate).Equals(""))
                            {
                                if (!StockControl.dbClss.TSt(g.FirstOrDefault().UpdateDate).Equals(""))
                                    txtCreateDate.Text = Convert.ToDateTime(g.FirstOrDefault().UpdateDate).ToString("dd/MMM/yyyy");
                                else
                                    txtCreateDate.Text = Convert.ToDateTime(g.FirstOrDefault().CreateDate).ToString("dd/MMM/yyyy");
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
                                btnDel_Item.Enabled = false;
                            }
                            else if
                                (StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Process"))
                            {
                                btnSave.Enabled = false;
                                //btnDelete.Enabled = false;
                                //btnView.Enabled = false;
                                //btnEdit.Enabled = false;
                                lblStatus.Text = "Process";
                                dgvData.ReadOnly = false;
                                btnNew.Enabled = true;
                                btnDel_Item.Enabled = false;
                            }
                            else if
                                (StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Completed"))
                                
                            {
                                btnSave.Enabled = false;
                                //btnDelete.Enabled = false;
                                //btnView.Enabled = false;
                                //btnEdit.Enabled = false;
                                lblStatus.Text = "Completed";
                                dgvData.ReadOnly = true;
                                btnNew.Enabled = true;
                                btnDel_Item.Enabled = false;
                            }
                            else
                            {
                                btnNew.Enabled = true;
                                btnSave.Enabled = true;
                                //btnDelete.Enabled = true;
                                //btnView.Enabled = true;
                                //btnEdit.Enabled = true;
                                lblStatus.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Status);
                                dgvData.ReadOnly = false;
                                btnDel_Item.Enabled = false;
                            }
                            dt_h = StockControl.dbClss.LINQToDataTable(g);

                            //Detail
                            var d = (from ix in db.tb_Shippings select ix)
                            .Where(a => a.ShippingNo == txtSHNo.Text.Trim()
                                && a.Status != "Cancel").ToList();
                            if (d.Count() > 0)
                            {
                                int c = 0;
                                dgvData.DataSource = d;
                                dt_d = StockControl.dbClss.LINQToDataTable(d);

                                //int id = 0;
                                foreach (var x in dgvData.Rows)
                                {
                                    c += 1;
                                    x.Cells["dgvNo"].Value = c;

                                    var p = (from ix in db.mh_Items select ix)
                                         .Where
                                         (a => a.InternalNo.Trim().ToUpper() == Convert.ToString(x.Cells["CodeNo"].Value).Trim().ToUpper()

                                         ).ToList();
                                    if (p.Count > 0)
                                    {
                                        x.Cells["GroupType"].Value = dbClss.TSt(p.FirstOrDefault().GroupType);
                                        x.Cells["Type"].Value = dbClss.TSt(p.FirstOrDefault().InventoryGroup);
                                    }

                                    x.Cells["QtyUsed"].Value = dbClss.TDe( db.Get_ShipQty(Convert.ToString(x.Cells["CodeNo"].Value),txtJobCard.Text));

                                    //id = Convert.ToInt32(x.Cells["id"].Value);
                                    var s = (from ix in db.tb_Stocks select ix)
                                   .Where(a => a.DocNo == txtSHNo.Text.Trim()
                                       //&& a.Refid == id)
                                       && a.Location == Convert.ToString(x.Cells["Location"].Value)
                                       && a.CodeNo == Convert.ToString(x.Cells["CodeNo"].Value)).OrderByDescending(ab => ab.id)

                                       .FirstOrDefault();
                                    if (s != null)
                                    {
                                        x.Cells["RemainQty"].Value = Convert.ToDecimal(s.RemainQty);
                                        x.Cells["UnitCost"].Value = Convert.ToDecimal(s.UnitCost);
                                        x.Cells["Amount"].Value = Math.Abs(Convert.ToDecimal(s.UnitCost) * Convert.ToDecimal(x.Cells["QtyShip"].Value));//Math.Abs(Convert.ToDecimal(s.AmountCost));
                                    }
                                }
                            }
                            Cal_Amount();
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

        private void ClearData()
        {
            ddlType.Text = "";
            cbShipforJob.Checked = true;
            txtJobCard.Text = "";
            //txtTempJobCard.Text = "";
            txtRefidJobNo.Text = "0";
            txtLocation.Text = "Production";
            txtJobCard.ReadOnly = false;

            txtSHNo.Text = "";
            txtRemark.Text = "";
            //txtCodeNo.Text = "";
            dtRequire.Value = DateTime.Now;
            txtCreateBy.Text = ClassLib.Classlib.User;
            txtSHName.Text = ClassLib.Classlib.User;
            txtCreateDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            lblStatus.Text = "-";
            dgvData.Rows.Clear();
            txtTotal.Text = "0.00";
            
        }
      private void Enable_Status(bool ss,string Condition)
        {
            if (Condition.Equals("-") || Condition.Equals("New"))
            {
                txtSHName.Enabled = ss;
                txtRemark.Enabled = ss;
                dtRequire.Enabled = ss;
                dgvData.ReadOnly = false;
                btnDel_Item.Enabled = ss;
                txtJobCard.Enabled = ss;

            }
            else if (Condition.Equals("View"))
            {
                txtSHName.Enabled = ss;
                txtRemark.Enabled = ss;
                dtRequire.Enabled = ss;
                dgvData.ReadOnly = false;
                txtRemark.Enabled = ss;
                btnDel_Item.Enabled = ss;
                txtJobCard.Enabled = ss;
            }

            else if (Condition.Equals("Edit"))
            {
                txtSHName.Enabled = ss;
                txtRemark.Enabled = ss;
                dtRequire.Enabled = ss;
                dgvData.ReadOnly = false;
                txtRemark.Enabled = ss;
                btnDel_Item.Enabled = ss;
                txtJobCard.Enabled = ss;
            }
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

           // getมาไว้ก่อน แต่ยังไมได้ save
            txtSHNo.Text = StockControl.dbClss.GetNo(35, 0);
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
                if (cbShipforJob.Checked)
                {
                    if (txtJobCard.Text.Equals(""))
                        err += "- “DocumentNo:” เป็นค่าว่าง \n";
                    //if (txtTempJobCard.Text.Equals(""))
                    //    err += "- “เลขที่อ้างอิง TempNo:” เป็นค่าว่าง \n";
                }
                else
                {
                    txtJobCard.Text = "";
                    //txtTempJobCard.Text = "";
                    txtRefidJobNo.Text = "0";
                }

                if (txtSHName.Text.Equals(""))
                    err += "- “ผู้เบิกสินค้า:” เป็นค่าว่าง \n";
                if (dtRequire.Text.Equals(""))
                    err += "- “วันที่เบิกสินค้า:” เป็นค่าว่าง \n";
                //if (txtLocation.Text.Equals(""))
                //    err += "- “สถานที่ต้องการย้ายสินค้าไปเก็บ(Location):” เป็นค่าว่าง \n";


                if (dgvData.Rows.Count <= 0)
                    err += "- “รายการ:” เป็นค่าว่าง \n";
                int c = 0;

                string CodeNo = "";
                decimal PCSUnit = 1;
                string BaseUOM = "";
                decimal BasePCSUOM = 1;
                bool cnk = false;
                decimal QTY = 0;
                decimal RemainQty = 0;

                foreach (GridViewRowInfo rowInfo in dgvData.Rows)
                {
                    if (rowInfo.IsVisible)
                    {


                        //if (StockControl.dbClss.TInt(rowInfo.Cells["QtyShip"].Value) <= (0))
                        //{
                        //    err += "- “จำนวนเบิก:” ต้องมากกว่า 0 \n";
                        //}
                        //else 
                        if (StockControl.dbClss.TInt(rowInfo.Cells["QtyShip"].Value) != (0))
                        {
                            c += 1;

                            CodeNo = "";
                            PCSUnit = 1;
                            BaseUOM = "";
                            BasePCSUOM = 1;
                            cnk = false;
                            QTY = 0;
                            RemainQty = 0;

                            if (StockControl.dbClss.TSt(rowInfo.Cells["CodeNo"].Value).Equals(""))
                                err += "- “รหัสทูล:” เป็นค่าว่าง \n";
                            else
                            {
                                CodeNo = StockControl.dbClss.TSt(rowInfo.Cells["CodeNo"].Value);
                                PCSUnit = StockControl.dbClss.TDe(rowInfo.Cells["PCSUnit"].Value);
                                BaseUOM = StockControl.dbClss.TSt(rowInfo.Cells["BaseUOM"].Value);
                                BasePCSUOM = dbClss.TDe(rowInfo.Cells["BasePCSUOM"].Value); //dbClss.Con_UOM(CodeNo, BaseUOM);
                               
                                QTY = StockControl.dbClss.TDe(rowInfo.Cells["QtyShip"].Value);
                                RemainQty = StockControl.dbClss.TDe(rowInfo.Cells["RemainQty"].Value);

                                if (BasePCSUOM <= 0)
                                    BasePCSUOM = 1;

                                decimal Temp = 0;
                                Temp = BasePCSUOM * PCSUnit * QTY;

                                if (Temp > RemainQty)
                                {
                                    cnk = true;
                                    err += "- “จำนวนเบิก:” มากกว่าจำนวนคงเหลือ \n";
                                }

                            }

                            if (cnk.Equals(false))
                            {
                                if (StockControl.dbClss.TDe(rowInfo.Cells["QtyShip"].Value) > StockControl.dbClss.TDe(rowInfo.Cells["RemainQty"].Value))
                                    err += "- “จำนวนเบิก:” มากกว่าจำนวนคงเหลือ \n";
                            }

                            if (StockControl.dbClss.TDe(rowInfo.Cells["UnitShip"].Value).Equals(""))
                                err += "- “หน่วย:” เป็นค่าว่าง \n";
                            if (StockControl.dbClss.TSt(rowInfo.Cells["Location"].Value).Equals(""))
                                err += "- “สถานที่เก็บในรายการ:” เป็นค่าว่าง \n";


                        }
                    }
                }

                if (c <= 0)
                    err += "- “กรุณาระบุจำนวนที่จะเบิกสินค้า:” เป็นค่าว่าง \n";


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
        private void SaveHerder_Old()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                var g = (from ix in db.tb_ShippingHs
                         where ix.ShippingNo.Trim() == txtSHNo.Text.Trim() && ix.Status != "Cancel"
                         //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                         select ix).ToList();
                if (g.Count > 0)  //มีรายการในระบบ
                {
                    foreach (DataRow row in dt_h.Rows)
                    {
                        var gg = (from ix in db.tb_ShippingHs
                                  where ix.ShippingNo.Trim() == txtSHNo.Text.Trim() && ix.Status != "Cancel"
                                  //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                                  select ix).First();

                        gg.UpdateBy = ClassLib.Classlib.User;
                        gg.UpdateDate = DateTime.Now;
                        dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไขโดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", txtSHNo.Text);
                        if (StockControl.dbClss.TSt(gg.BarCode).Equals(""))
                            gg.BarCode = StockControl.dbClss.SaveQRCode2D(txtSHNo.Text.Trim());

                        if (!txtSHName.Text.Trim().Equals(row["ShipName"].ToString()))
                        {
                            gg.ShipName = txtSHName.Text;
                            dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไขผู้เบิกสินค้า [" + txtSHName.Text.Trim() + " เดิม :" + row["ShipName"].ToString() + "]", txtSHNo.Text);
                        }
                        if (!txtJobCard.Text.Trim().Equals(row["JobCard"].ToString()))
                        {
                            gg.JobCard = txtJobCard.Text;
                            dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไข JobCard [" + txtJobCard.Text.Trim() + " เดิม :" + row["JobCard"].ToString() + "]", txtSHNo.Text);
                        }
                        //if (!txtTempJobCard.Text.Trim().Equals(row["TempJobCard"].ToString()))
                        //{
                        //    gg.TempJobCard = txtTempJobCard.Text;
                        //    dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไข TempJobCard [" + txtTempJobCard.Text.Trim() + " เดิม :" + row["TempJobCard"].ToString() + "]", txtSHNo.Text);
                        //}

                        if (!txtRemark.Text.Trim().Equals(row["Remark"].ToString()))
                        {
                            gg.Remark = txtRemark.Text.Trim();
                            dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไขหมายเหตุ [" + txtRemark.Text.Trim() + " เดิม :" + row["Remark"].ToString() + "]", txtSHNo.Text);
                        }
                        if (!txtLocation.Text.Trim().Equals(row["ToLocation"].ToString()))
                        {
                            gg.ToLocation = txtLocation.Text.Trim();
                            dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไขสถานทีเก็บ [" + txtLocation.Text.Trim() + " เดิม :" + row["ToLocation"].ToString() + "]", txtSHNo.Text);
                        }
                        if (!dtRequire.Text.Trim().Equals(""))
                        {
                            string date1 = "";
                            date1 = dtRequire.Value.ToString("yyyyMMdd", new CultureInfo("en-US"));
                            string date2 = "";
                            DateTime temp = DateTime.Now;
                            if (!StockControl.dbClss.TSt(row["ShipDate"].ToString()).Equals(""))
                            {

                                temp = Convert.ToDateTime(row["ShipDate"]);
                                date2 = temp.ToString("yyyyMMdd", new CultureInfo("en-US"));

                            }
                            if (!date1.Equals(date2))
                            {
                                DateTime? RequireDate = DateTime.Now;
                                if (!dtRequire.Text.Equals(""))
                                    RequireDate = dtRequire.Value;
                                gg.ShipDate = RequireDate;
                                dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไขวันที่เบิกสินค้า [" + dtRequire.Text.Trim() + " เดิม :" + temp.ToString() + "]", txtSHNo.Text);
                            }
                        }
                        db.SubmitChanges();
                    }
                }
                else //สร้างใหม่
                {
                    byte[] barcode = null;
                    //barcode = StockControl.dbClss.SaveQRCode2D(txtSHNo.Text.Trim());
                    DateTime? UpdateDate = null;

                    DateTime? RequireDate = DateTime.Now;
                    if (!dtRequire.Text.Equals(""))
                        RequireDate = dtRequire.Value;

                    tb_ShippingH gg = new tb_ShippingH();
                    gg.ShippingNo = txtSHNo.Text;
                    gg.ShipDate = RequireDate;
                    gg.UpdateBy = null;
                    gg.UpdateDate = UpdateDate;
                    gg.CreateBy = ClassLib.Classlib.User;
                    gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    gg.ShipName = txtSHName.Text;
                    gg.Remark = txtRemark.Text;
                    gg.JobCard = txtJobCard.Text.Trim();
                    gg.TempJobCard = "";
                    gg.BarCode = barcode;
                    gg.Status = "Completed";
                    gg.ToLocation = txtLocation.Text;
                    db.tb_ShippingHs.InsertOnSubmit(gg);
                    db.SubmitChanges();

                    dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "สร้าง การเบิกสินค้า [" + txtSHNo.Text.Trim() + "]", txtSHNo.Text);
                }
            }
        }
        private void SaveDetail_Old()
        {
            dgvData.EndEdit();

            DateTime? RequireDate = DateTime.Now;
            if (!dtRequire.Text.Equals(""))
                RequireDate = dtRequire.Value;
            //int Seq = 0;
            //DateTime? UpdateDate = null;
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                //decimal UnitCost = 0;
                foreach (var g in dgvData.Rows)
                {
                    string SS = "";
                    if (g.IsVisible.Equals(true))
                    {
                        if (StockControl.dbClss.TInt(g.Cells["QtyShip"].Value) != (0)) // เอาเฉพาะรายการที่ไม่เป็น 0 
                        {
                            if (StockControl.dbClss.TInt(g.Cells["id"].Value) <= 0)  //New ใหม่
                            {
                                int RefidJobNo = dbClss.TInt(txtRefidJobNo.Text);
                                string BaseUOM = dbClss.TSt(g.Cells["BaseUOM"].Value);
                                decimal BasePCSUOM = dbClss.TDe(g.Cells["BasePCSUOM"].Value);//dbClss.Con_UOM(StockControl.dbClss.TSt(g.Cells["CodeNo"].Value), BaseUOM);

                                //เบิกจาก WH
                                db.sp_071_Accident_Slip(txtSHNo.Text.Trim(), StockControl.dbClss.TSt(g.Cells["CodeNo"].Value)
                                    , dbClss.TDe(g.Cells["QtyPlan"].Value)
                                    , StockControl.dbClss.TDe(g.Cells["QtyShip"].Value), StockControl.dbClss.TSt(g.Cells["Remark"].Value)
                                    , StockControl.dbClss.TSt(g.Cells["LineName"].Value), StockControl.dbClss.TSt(g.Cells["MachineName"].Value)
                                    , StockControl.dbClss.TSt(g.Cells["SerialNo"].Value)
                                    , StockControl.dbClss.TSt(g.Cells["LotNo"].Value)
                                    , "Completed", ClassLib.Classlib.User
                                    , txtJobCard.Text.Trim()
                                    , ""//txtTempJobCard.Text.Trim()
                                    , RefidJobNo
                                    , dbClss.TSt(g.Cells["Location"].Value)
                                    , txtLocation.Text
                                    , BaseUOM
                                    , BasePCSUOM
                                    , dbClss.TSt(g.Cells["UnitShip"].Value)
                                    , StockControl.dbClss.TDe(g.Cells["PCSUnit"].Value)
                                    , dbClss.TInt(g.Cells["idCSTMPODt"].Value)
                                    , dbClss.TInt(g.Cells["idProductionOrderRM"].Value)
                                    , 0
                                    );


                            }
                        }
                    }
                }
            }
        }
        private void SaveHerder()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                var g = (from ix in db.mh
                         where ix.ShippingNo.Trim() == txtSHNo.Text.Trim() && ix.Status != "Cancel"
                         //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                         select ix).ToList();
                if (g.Count > 0)  //มีรายการในระบบ
                {
                    foreach (DataRow row in dt_h.Rows)
                    {
                        var gg = (from ix in db.tb_ShippingHs
                                  where ix.ShippingNo.Trim() == txtSHNo.Text.Trim() && ix.Status != "Cancel"
                                  //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                                  select ix).First();

                        gg.UpdateBy = ClassLib.Classlib.User;
                        gg.UpdateDate = DateTime.Now;
                        dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไขโดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", txtSHNo.Text);
                        if (StockControl.dbClss.TSt(gg.BarCode).Equals(""))
                            gg.BarCode = StockControl.dbClss.SaveQRCode2D(txtSHNo.Text.Trim());

                        if (!txtSHName.Text.Trim().Equals(row["ShipName"].ToString()))
                        {
                            gg.ShipName = txtSHName.Text;                           
                            dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไขผู้เบิกสินค้า [" + txtSHName.Text.Trim() + " เดิม :" + row["ShipName"].ToString() + "]", txtSHNo.Text);
                        }
                        if (!txtJobCard.Text.Trim().Equals(row["JobCard"].ToString()))
                        {
                            gg.JobCard = txtJobCard.Text;                            
                            dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไข JobCard [" + txtJobCard.Text.Trim() + " เดิม :" + row["JobCard"].ToString() + "]", txtSHNo.Text);
                        }
                        //if (!txtTempJobCard.Text.Trim().Equals(row["TempJobCard"].ToString()))
                        //{
                        //    gg.TempJobCard = txtTempJobCard.Text;
                        //    dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไข TempJobCard [" + txtTempJobCard.Text.Trim() + " เดิม :" + row["TempJobCard"].ToString() + "]", txtSHNo.Text);
                        //}

                        if (!txtRemark.Text.Trim().Equals(row["Remark"].ToString()))
                        {
                            gg.Remark = txtRemark.Text.Trim();
                            dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไขหมายเหตุ [" + txtRemark.Text.Trim() + " เดิม :" + row["Remark"].ToString() + "]", txtSHNo.Text);
                        }
                        if (!txtLocation.Text.Trim().Equals(row["ToLocation"].ToString()))
                        {
                            gg.ToLocation = txtLocation.Text.Trim();
                            dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไขสถานทีเก็บ [" + txtLocation.Text.Trim() + " เดิม :" + row["ToLocation"].ToString() + "]", txtSHNo.Text);
                        }
                        if (!dtRequire.Text.Trim().Equals(""))
                        {
                            string date1 = "";
                            date1 = dtRequire.Value.ToString("yyyyMMdd", new CultureInfo("en-US"));
                            string date2 = "";
                            DateTime temp = DateTime.Now;
                            if (!StockControl.dbClss.TSt(row["ShipDate"].ToString()).Equals(""))
                            {

                                temp = Convert.ToDateTime(row["ShipDate"]);
                                date2 = temp.ToString("yyyyMMdd", new CultureInfo("en-US"));

                            }
                            if (!date1.Equals(date2))
                            {
                                DateTime? RequireDate = DateTime.Now;
                                if (!dtRequire.Text.Equals(""))
                                    RequireDate = dtRequire.Value;
                                gg.ShipDate = RequireDate;
                                dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "แก้ไขวันที่เบิกสินค้า [" + dtRequire.Text.Trim() + " เดิม :" + temp.ToString() + "]", txtSHNo.Text);
                            }
                        }
                        db.SubmitChanges();
                    }
                }
                else //สร้างใหม่
                {
                    byte[] barcode = null;
                        //barcode = StockControl.dbClss.SaveQRCode2D(txtSHNo.Text.Trim());
                    DateTime? UpdateDate = null;

                    DateTime? RequireDate = DateTime.Now;
                    if (!dtRequire.Text.Equals(""))
                        RequireDate = dtRequire.Value;

                    tb_ShippingH gg = new tb_ShippingH();
                    gg.ShippingNo = txtSHNo.Text;
                    gg.ShipDate = RequireDate;
                    gg.UpdateBy = null;
                    gg.UpdateDate = UpdateDate;
                    gg.CreateBy = ClassLib.Classlib.User;
                    gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    gg.ShipName = txtSHName.Text;
                    gg.Remark = txtRemark.Text;
                    gg.JobCard = txtJobCard.Text.Trim();
                    gg.TempJobCard = "";
                    gg.BarCode = barcode;
                    gg.Status = "Completed";
                    gg.ToLocation = txtLocation.Text;
                    db.tb_ShippingHs.InsertOnSubmit(gg);
                    db.SubmitChanges();

                    dbClss.AddHistory(this.Name, "แก้ไขการเบิก", "สร้าง การเบิกสินค้า [" + txtSHNo.Text.Trim() + "]", txtSHNo.Text);
                }
            }
        }
        private void SaveDetail()
        {
            dgvData.EndEdit();
           
            DateTime? RequireDate = DateTime.Now;
            if (!dtRequire.Text.Equals(""))
                RequireDate = dtRequire.Value;
            //int Seq = 0;
            //DateTime? UpdateDate = null;
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                //decimal UnitCost = 0;
                foreach (var g in dgvData.Rows)
                {
                    string SS = "";
                    if (g.IsVisible.Equals(true))
                    {
                        if (StockControl.dbClss.TInt(g.Cells["QtyShip"].Value) != (0)) // เอาเฉพาะรายการที่ไม่เป็น 0 
                        {
                            if (StockControl.dbClss.TInt(g.Cells["id"].Value) <= 0)  //New ใหม่
                            {
                                int RefidJobNo = dbClss.TInt(txtRefidJobNo.Text);
                                string BaseUOM = dbClss.TSt(g.Cells["BaseUOM"].Value);
                                decimal BasePCSUOM = dbClss.TDe(g.Cells["BasePCSUOM"].Value);//dbClss.Con_UOM(StockControl.dbClss.TSt(g.Cells["CodeNo"].Value), BaseUOM);

                                //เบิกจาก WH
                                db.sp_071_Accident_Slip(txtSHNo.Text.Trim(), StockControl.dbClss.TSt(g.Cells["CodeNo"].Value)
                                    , dbClss.TDe(g.Cells["QtyPlan"].Value)
                                    , StockControl.dbClss.TDe(g.Cells["QtyShip"].Value), StockControl.dbClss.TSt(g.Cells["Remark"].Value)
                                    , StockControl.dbClss.TSt(g.Cells["LineName"].Value), StockControl.dbClss.TSt(g.Cells["MachineName"].Value)
                                    , StockControl.dbClss.TSt(g.Cells["SerialNo"].Value)
                                    , StockControl.dbClss.TSt(g.Cells["LotNo"].Value)
                                    , "Completed", ClassLib.Classlib.User
                                    , txtJobCard.Text.Trim()
                                    , ""//txtTempJobCard.Text.Trim()
                                    , RefidJobNo
                                    , dbClss.TSt(g.Cells["Location"].Value)
                                    , txtLocation.Text
                                    , BaseUOM
                                    , BasePCSUOM
                                    , dbClss.TSt(g.Cells["UnitShip"].Value)
                                    , StockControl.dbClss.TDe(g.Cells["PCSUnit"].Value)
                                    , dbClss.TInt(g.Cells["idCSTMPODt"].Value)
                                    , dbClss.TInt(g.Cells["idProductionOrderRM"].Value)
                                    , 0
                                    );
                                

                            }
                        }
                    }
                }
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            if (Ac.Equals("New"))// || Ac.Equals("Edit"))
            {
                if (Check_Save())
                    return;
                if (MessageBox.Show("ต้องการบันทึก ?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Cursor = Cursors.WaitCursor;

                    if (Ac.Equals("New"))
                        txtSHNo.Text = StockControl.dbClss.GetNo(35, 2);

                    if (!txtSHNo.Text.Equals(""))
                    {
                        SaveHerder();
                        SaveDetail();

                        DataLoad();
                        btnNew.Enabled = true;
                        btnDel_Item.Enabled = false;

                        ////insert sotck
                        //InsertStock_new();

                        MessageBox.Show("บันทึกสำเร็จ!");
                        btnRefresh_Click(null, null);
                    }
                    else
                    {
                        MessageBox.Show("ไม่สามารถโหลดเลขที่เอกสารได้ ติดต่อแผนก IT");
                    }
                }
            }
        }
      

        private decimal get_cost(string Code)
        {
            decimal re = 0;
            //using (DataClasses1DataContext db = new DataClasses1DataContext())
            //{
            //    var g = (from ix in db.tb_Items
            //             where ix.CodeNo == Code && ix.Status == "Active"
            //             select ix).First();
            //    re = Convert.ToDecimal(g.StandardCost);

            //}
            return re;
        }
        private void Insert_Stock()
        {
            //try
            //{

            //    using (DataClasses1DataContext db = new DataClasses1DataContext())
            //    {
            //        DateTime? CalDate = null;
            //        DateTime? AppDate = DateTime.Now;
            //        int Seq = 0;
                    
                    

            //        string CNNo = CNNo = StockControl.dbClss.GetNo(6, 2);
            //        var g = (from ix in db.tb_Shippings
            //                     //join i in db.tb_Items on ix.CodeNo equals i.CodeNo
            //                 where ix.ShippingNo.Trim() == txtSHNo.Text.Trim() && ix.Status != "Cancel"

            //                 select ix).ToList();
            //        if (g.Count > 0)
            //        {
            //            //insert Stock

            //            foreach (var vv in g)
            //            {
            //                Seq += 1;

            //                tb_Stock1 gg = new tb_Stock1();
            //                gg.AppDate = AppDate;
            //                gg.Seq = Seq;
            //                gg.App = "Shipping";
            //                gg.Appid = Seq;
            //                gg.CreateBy = ClassLib.Classlib.User;
            //                gg.CreateDate = DateTime.Now;
            //                gg.DocNo = CNNo;
            //                gg.RefNo = txtSHNo.Text;
            //                gg.Type = "Ship";
            //                gg.QTY = -Convert.ToDecimal(vv.QTY);
            //                gg.Inbound = 0;
            //                gg.Outbound = -Convert.ToDecimal(vv.QTY); ;
            //                gg.AmountCost = (-Convert.ToDecimal(vv.QTY)) * get_cost(vv.CodeNo);
            //                gg.UnitCost = get_cost(vv.CodeNo);
            //                gg.RemainQty = 0;
            //                gg.RemainUnitCost = 0;
            //                gg.RemainAmount = 0;
            //                gg.CalDate = CalDate;
            //                gg.Status = "Active";

            //                db.tb_Stock1s.InsertOnSubmit(gg);
            //                db.SubmitChanges();

            //                dbClss.Insert_Stock(vv.CodeNo, (-Convert.ToDecimal(vv.QTY)), "Shipping", "Inv");


            //            }
            //        }
            //    }
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void InsertStock_new()
        {
            try
            {

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    DateTime? CalDate = null;
                    DateTime? AppDate = DateTime.Now;
                    int Seq = 0;
                    string Type = "Shipping";
                    string Category = ""; //Temp,Invoice
                    decimal Cost = 0;
                   // int Flag_ClearTemp = 0;
                    decimal Qty_Inv = 0;
                    decimal Qty_DL = 0;
                    decimal Qty_Remain = 0;
                    decimal QTY = 0;
                    decimal QTY_temp = 0;

                    string Type_in_out = "Out";
                    decimal RemainQty = 0;
                    decimal Amount = 0;
                    decimal RemainAmount = 0;
                    decimal Avg = 0;
                    decimal UnitCost = 0;
                    decimal sum_Remain = 0;
                    decimal sum_Qty = 0;
                    int idCSTMPODt = 0;
                    //string CNNo = CNNo = StockControl.dbClss.GetNo(6, 2);
                    var g = (from ix in db.tb_Shippings
                                 //join i in db.tb_Items on ix.CodeNo equals i.CodeNo
                             where ix.ShippingNo.Trim() == txtSHNo.Text.Trim() && ix.Status != "Cancel"

                             select ix).ToList();
                    if (g.Count > 0)
                    {
                        //insert Stock

                        foreach (var vv in g)
                        {
                            Seq += 1;

                            QTY = Convert.ToDecimal(vv.QTY);
                            QTY_temp = 0;
                            Qty_Remain = (Convert.ToDecimal(db.Cal_QTY(vv.CodeNo, "", 0)));  //sum ทั้งหมด
                            Qty_Inv = (Convert.ToDecimal(db.Cal_QTY(vv.CodeNo, "Invoice", 0))); //sum เฉพาะ Invoice
                            Qty_DL = (Convert.ToDecimal(db.Cal_QTY(vv.CodeNo, "Temp", 0))); // sum เฉพาะ DL
                            
                            if (QTY <= Qty_Remain)
                            {                               

                                if (Qty_Inv >= QTY) //ถ้า จำนวน remain มีมากกว่าจำนวนที่จะลบ
                                {
                                    UnitCost = Convert.ToDecimal(vv.UnitCost);
                                    //if (UnitCost <= 0)
                                    //    UnitCost = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "Avg"));

                                    Amount = (-QTY) * UnitCost;
                                    string Category1 = "Invoice";
                                    if (dbClss.TInt(vv.idCSTMPODt) > 0) Category1 = "CstmPOID";
                                    else if (dbClss.TInt(vv.idCSTMPODt) == 0) Category1 = "SafetyStock";

                                    //แบบที่ 1 จะไป sum ใหม่
                                    RemainQty = (Convert.ToDecimal(db.Cal_QTY_Remain_Location(vv.CodeNo, "", 0,vv.Location,dbClss.TInt(vv.idCSTMPODt))));
                                    //แบบที่ 2 จะไปดึงล่าสุดมา
                                    //RemainQty = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainQty"));
                                    sum_Remain = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainAmount",vv.Location, dbClss.TInt(vv.idCSTMPODt)))
                                        + Amount;

                                    sum_Qty = RemainQty + (-QTY);
                                    Avg = UnitCost;//sum_Remain / sum_Qty;
                                    RemainAmount = sum_Remain;


                                    Category = "Invoice";
                                    tb_Stock gg = new tb_Stock();
                                    gg.AppDate = AppDate;
                                    gg.Seq = Seq;
                                    gg.App = "Shipping";
                                    gg.Appid = Seq;
                                    gg.CreateBy = ClassLib.Classlib.User;
                                    gg.CreateDate = DateTime.Now;
                                    gg.DocNo = txtSHNo.Text;
                                    gg.RefNo = "";
                                    gg.CodeNo = vv.CodeNo;
                                    gg.Type = Type;
                                    gg.QTY = -Convert.ToDecimal(QTY);
                                    gg.Inbound = 0;
                                    gg.Outbound = -Convert.ToDecimal(QTY);
                                    gg.Type_i = 3;  //Receive = 1,Cancel Receive 2,Shipping = 3,Cancel Shipping = 4,Adjust stock = 5,ClearTemp = 6
                                    gg.Category = Category;
                                    gg.Refid = vv.id;
                                    
                                    gg.CalDate = CalDate;
                                    gg.Status = "Active";
                                    gg.Flag_ClearTemp =0; //0 คือ invoice,1 คือ Temp , 2 คือ clear temp แล้ว
                                    gg.Type_in_out = Type_in_out;
                                    gg.AmountCost = Amount;
                                    gg.UnitCost = UnitCost;
                                    gg.RemainQty = sum_Qty;
                                    gg.RemainUnitCost = 0;
                                    gg.RemainAmount = RemainAmount;
                                    gg.Avg = Avg;
                                    gg.TLCost = 0;
                                    gg.TLQty = 0;
                                    gg.ShipQty = 0;

                                    db.tb_Stocks.InsertOnSubmit(gg);
                                    db.SubmitChanges();

                                    dbClss.AddHistory(this.Name, "เบิกสินค้า", " เบิกสินค้าเลขที่ : " + txtSHNo.Text + " เบิก : " + Category + " CodeNo : " + vv.CodeNo + " จำนวน : " + (-QTY).ToString() + " โดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", txtSHNo.Text);

                                }
                                else
                                {
                                    QTY_temp = QTY - Qty_Inv;

                                    UnitCost = Convert.ToDecimal(vv.UnitCost);
                                    //if (UnitCost <= 0)
                                    //    UnitCost = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "Avg"));
                                    
                                    Amount = (-QTY) * UnitCost;
                                    string Category1 = "Invoice";
                                    if (dbClss.TInt(vv.idCSTMPODt) > 0) Category1 = "CstmPOID";
                                    else if (dbClss.TInt(vv.idCSTMPODt) == 0) Category1 = "SafetyStock";

                                    //แบบที่ 1 จะไป sum ใหม่
                                    RemainQty = (Convert.ToDecimal(db.Cal_QTY_Remain_Location(vv.CodeNo, Category1, 0,vv.Location,dbClss.TInt(vv.idCSTMPODt))));
                                    //แบบที่ 2 จะไปดึงล่าสุดมา
                                    //RemainQty = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainQty"));
                                    sum_Remain = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainAmount",vv.Location, dbClss.TInt(vv.idCSTMPODt)))
                                        + Amount;

                                    sum_Qty = RemainQty + (-QTY);
                                    Avg = UnitCost;//sum_Remain / sum_Qty;
                                    RemainAmount = sum_Remain;

                                    Category = "Temp";
                                    tb_Stock gg = new tb_Stock();
                                    gg.AppDate = AppDate;
                                    gg.Seq = Seq;
                                    gg.App = "Shipping";
                                    gg.Appid = Seq;
                                    gg.CreateBy = ClassLib.Classlib.User;
                                    gg.CreateDate = DateTime.Now;
                                    gg.DocNo = txtSHNo.Text;
                                    gg.RefNo = "";
                                    gg.CodeNo = vv.CodeNo;
                                    gg.Type = Type;
                                    gg.QTY = -Convert.ToDecimal(QTY);
                                    gg.Inbound = 0;
                                    gg.Outbound = -Convert.ToDecimal(QTY);
                                    gg.Type_i = 3;  //Receive = 1,Cancel Receive 2,Shipping = 3,Cancel Shipping = 4,Adjust stock = 5,ClearTemp = 6
                                    gg.Category = Category;
                                    gg.Refid = vv.id;
                                    
                                    gg.CalDate = CalDate;
                                    gg.Status = "Active";
                                    gg.Flag_ClearTemp = 1; //0 คือ invoice,1 คือ Temp , 2 คือ clear temp แล้ว
                                    gg.Type_in_out = Type_in_out;
                                    gg.AmountCost = Amount;
                                    gg.UnitCost = UnitCost;
                                    gg.RemainQty = sum_Qty;
                                    gg.RemainUnitCost = 0;
                                    gg.RemainAmount = RemainAmount;
                                    gg.Avg = Avg;
                                    gg.TLCost = 0;
                                    gg.TLQty = 0;
                                    gg.ShipQty = 0;

                                    db.tb_Stocks.InsertOnSubmit(gg);
                                    db.SubmitChanges();
                                    dbClss.AddHistory(this.Name, "เบิกสินค้า", " เบิกสินค้าเลขที่ : " + txtSHNo.Text + " เบิก : " + Category + " CodeNo : " + vv.CodeNo + " จำนวน : " + (-QTY).ToString() + " โดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", txtSHNo.Text);


                                    //// --Stock ใน Invoice ไม่พอ ต้องเอาที่ Temp มา

                                    //UnitCost = Convert.ToDecimal(vv.UnitCost);
                                    //if (UnitCost <= 0)
                                    //    UnitCost = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "Avg"));
                                    
                                    //Amount = (-QTY_temp) * UnitCost;

                                    ////แบบที่ 1 จะไป sum ใหม่
                                    //RemainQty = (Convert.ToDecimal(db.Cal_QTY(vv.CodeNo, "", 0)));
                                    ////แบบที่ 2 จะไปดึงล่าสุดมา
                                    ////RemainQty = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainQty"));
                                    //sum_Remain = Convert.ToDecimal(dbClss.Get_Stock(vv.CodeNo, "", "", "RemainAmount"))
                                    //    + Amount;
                                    //sum_Qty = RemainQty + (-QTY_temp);
                                    //Avg = UnitCost;//sum_Remain / sum_Qty;
                                    //RemainAmount = sum_Remain;

                                    //Category = "Invoice";
                                    //tb_Stock aa = new tb_Stock();
                                    //aa.AppDate = AppDate;
                                    //aa.Seq = Seq;
                                    //aa.App = "Shipping";
                                    //aa.Appid = Seq;
                                    //aa.CreateBy = ClassLib.Classlib.User;
                                    //aa.CreateDate = DateTime.Now;
                                    //aa.DocNo = txtSHNo.Text;
                                    //aa.RefNo = "";
                                    //aa.CodeNo = vv.CodeNo;
                                    //aa.Type = Type;
                                    //aa.QTY = -Convert.ToDecimal(QTY_temp);
                                    //aa.Inbound = 0;
                                    //aa.Outbound = -Convert.ToDecimal(QTY_temp);
                                    //aa.Type_i = 3;  //Receive = 1,Cancel Receive 2,Shipping = 3,Cancel Shipping = 4,Adjust stock = 5,ClearTemp = 6
                                    //aa.Category = Category;
                                    //aa.Refid = vv.id;
                                   
                                    //aa.CalDate = CalDate;
                                    //aa.Status = "Active";
                                    //aa.Flag_ClearTemp = 1; //0 คือ invoice,1 คือ Temp , 2 คือ clear temp แล้ว
                                    //aa.Type_in_out = Type_in_out;
                                    //aa.AmountCost = Amount;
                                    //aa.UnitCost = UnitCost;
                                    //aa.RemainQty = sum_Qty;
                                    //aa.RemainUnitCost = 0;
                                    //aa.RemainAmount = RemainAmount;
                                    //aa.Avg = Avg;

                                    //db.tb_Stocks.InsertOnSubmit(aa);
                                    //db.SubmitChanges();
                                    //dbClss.AddHistory(this.Name, "เบิกสินค้า", " เบิกสินค้าเลขที่ : " + txtSHNo.Text + " เบิก : " + Category + " CodeNo : " + vv.CodeNo + " จำนวน : " + (-QTY_temp).ToString() + " โดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", txtSHNo.Text);

                                }

                            }

                            //update Stock เข้า item
                            db.sp_010_Update_StockItem(Convert.ToString(vv.CodeNo), "");
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                dgvData.EndEdit();
                if (e.RowIndex >= -1)
                {

                    if (dgvData.Columns["QtyShip"].Index == e.ColumnIndex)
                    {

                        if (dbClss.TSt(e.Row.Cells["UnitShip"].Value) == "")
                        {
                            e.Row.Cells["QtyShip"].Value = 0;
                            e.Row.Cells["Qty"].Value = 0;
                            MessageBox.Show("หน่วยเบิกเป็นค่าว่าง");
                        }
                        else
                        {
                            //Cal Remain Qty

                            //string dgvUOM = dbClss.TSt(e.Row.Cells["UnitShip"].Value);
                            string CodeNo = dbClss.TSt(e.Row.Cells["CodeNo"].Value);
                            decimal PCSUnit = dbClss.TDe(e.Row.Cells["PCSUnit"].Value);
                            //string BaseUOM = dbClss.TSt(e.Row.Cells["BaseUOM"].Value);
                            decimal BasePCSUOM = dbClss.TDe(e.Row.Cells["BasePCSUOM"].Value);// dbClss.Con_UOM(CodeNo, BaseUOM);

                            //using (DataClasses1DataContext db = new DataClasses1DataContext())
                            //{
                            //    var g = (from ix in db.mh_Items select ix).Where(a => a.InternalNo == CodeNo).ToList();
                            //    if (g.Count() > 0)
                            //    {
                            //        BaseUom = dbClss.TSt(g.FirstOrDefault().BaseUOM);
                            //        BasePCSUOM = dbClss.Con_UOM(CodeNo, BaseUom);
                            //    }
                            //}

                            decimal QTY = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["QtyShip"].Value), out QTY);
                            decimal RemainQty = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["RemainQty"].Value), out RemainQty);
                            if (BasePCSUOM <= 0)
                                BasePCSUOM = 1;

                            decimal Temp = 0;
                            Temp = BasePCSUOM * PCSUnit * QTY;

                            //Temp = Check_RemainStock(CodeNo, PCSUnit, BaseUOM, BasePCSUOM, QTY, RemainQty);

                            if (Temp > RemainQty)
                            {
                                MessageBox.Show("ไม่สามารถเบิกเกินจำนวนคงเหลือได้");
                                e.Row.Cells["QtyShip"].Value = 0;
                                QTY = 0;
                                Temp = 0;
                            }

                            if (QTY > 0)
                            {
                                int idCSTMPODt = dbClss.TInt(e.Row.Cells["idCSTMPODt"].Value);
                                int Free = 0;//---1;
                                if (idCSTMPODt > 0) Free = 0;
                                //else if (idCSTMPODt == 0) Free = 1;

                                e.Row.Cells["UnitCost"].Value = Get_UnitCostFIFO(dbClss.TSt(e.Row.Cells["CodeNo"].Value), QTY, dbClss.TSt(e.Row.Cells["Location"].Value), idCSTMPODt, Free);
                            }
                            e.Row.Cells["Qty"].Value = Math.Round(Temp, 2);
                        }
                    }

                    if (dgvData.Columns["QtyShip"].Index == e.ColumnIndex
                        || dgvData.Columns["UnitCost"].Index == e.ColumnIndex
                        )
                    {
                        decimal QTY = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["QtyShip"].Value), out QTY);
                        decimal CostPerUnit = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["UnitCost"].Value), out CostPerUnit);
                        e.Row.Cells["Amount"].Value = QTY * CostPerUnit;
                        Cal_Amount();
                    }
                    else if (dgvData.Columns["UnitShip"].Index == e.ColumnIndex)
                    {
                        string dgvUOM = dbClss.TSt(e.Row.Cells["UnitShip"].Value);
                        string CodeNo = dbClss.TSt(e.Row.Cells["CodeNo"].Value);
                        e.Row.Cells["PCSUnit"].Value = dbClss.Con_UOM(CodeNo, dgvUOM);
                        
                        //Cal Remain Qty
                        decimal PCSUnit = dbClss.TDe(e.Row.Cells["PCSUnit"].Value);
                        //string BaseUOM = dbClss.TSt(e.Row.Cells["BaseUOM"].Value);
                        decimal BasePCSUOM  = dbClss.TDe(e.Row.Cells["BasePCSUOM"].Value);//dbClss.Con_UOM(CodeNo, BaseUOM);

                        decimal QTY = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["QtyShip"].Value), out QTY);
                        decimal RemainQty = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["RemainQty"].Value), out RemainQty);
                        if (BasePCSUOM <= 0)
                            BasePCSUOM = 1;

                        decimal Temp = 0;
                        Temp = BasePCSUOM * PCSUnit * QTY;

                        //Temp = Check_RemainStock(CodeNo, PCSUnit, BaseUOM, BasePCSUOM, QTY, RemainQty);

                        if (Temp > RemainQty)
                        {
                            MessageBox.Show("ไม่สามารถเบิกเกินจำนวนคงเหลือได้");
                            e.Row.Cells["QtyShip"].Value = 0;
                            QTY = 0;
                        }
                        e.Row.Cells["Qty"].Value = Math.Round(Temp, 2);

                    }
                    else if (dgvData.Columns["Location"].Index == e.ColumnIndex)
                    {
                        using (DataClasses1DataContext db = new DataClasses1DataContext())
                        {
                            int idCSTMPODt = dbClss.TInt(e.Row.Cells["idCSTMPODt"].Value);
                            string Category = "Invoice";
                            if (idCSTMPODt > 0) Category = "CstmPOID";
                            else if (idCSTMPODt == 0) Category = "SafetyStock";

                            e.Row.Cells["RemainQty"].Value = (Convert.ToDecimal(db.Cal_QTY_Remain_Location(Convert.ToString(e.Row.Cells["CodeNo"].Value)
                                , Category, 0, Convert.ToString(e.Row.Cells["Location"].Value), idCSTMPODt)));


                            string dgvUOM = dbClss.TSt(e.Row.Cells["UnitShip"].Value);
                            string CodeNo = dbClss.TSt(e.Row.Cells["CodeNo"].Value);
                            e.Row.Cells["PCSUnit"].Value = dbClss.Con_UOM(CodeNo, dgvUOM);

                            //Cal Remain Qty
                            decimal PCSUnit = dbClss.TDe(e.Row.Cells["PCSUnit"].Value);
                            string BaseUOM = dbClss.TSt(e.Row.Cells["BaseUOM"].Value);
                            decimal BasePCSUOM = dbClss.TDe(e.Row.Cells["BasePCSUOM"].Value);// dbClss.Con_UOM(CodeNo, BaseUOM);

                            decimal QTY = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["QtyShip"].Value), out QTY);
                            decimal RemainQty = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["RemainQty"].Value), out RemainQty);
                            if (BasePCSUOM <= 0)
                                BasePCSUOM = 1;

                            decimal Temp = 0;
                            Temp = BasePCSUOM * PCSUnit * QTY;

                            //Temp = Check_RemainStock(CodeNo, PCSUnit, BaseUOM, BasePCSUOM, QTY, RemainQty);

                            if (Temp > RemainQty)
                            {
                                MessageBox.Show("ไม่สามารถเบิกเกินจำนวนคงเหลือได้");
                                e.Row.Cells["QtyShip"].Value = 0;
                                QTY = 0;
                            }
                            e.Row.Cells["Qty"].Value = Math.Round(Temp, 2);
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private decimal Check_RemainStock(string InternalNo,decimal PCSUnit,string BaseUnit,decimal BasePCSUnit,decimal QTY, decimal RemainQty)
        {
            decimal re = 0;

            //string dgvUOM = dbClss.TSt(e.Row.Cells["UnitShip"].Value);
            string CodeNo = InternalNo;
            string BaseUOM = BaseUnit;
            decimal BasePCSUOM = BasePCSUnit;
            
            if (BasePCSUOM <= 0)
                BasePCSUOM = 1;

           
            re = BasePCSUOM * PCSUnit * QTY;



            return re;
        }
        private decimal Get_UnitCostFIFO(string CodeNo, decimal Qty,string Location,int idCSTMPODt,int Free)
        {
            decimal re = 0;
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                re = dbClss.TDe(db.Get_AvgCost_FIFO(CodeNo, Qty, Location, idCSTMPODt,Free));
            }
            return re;
        }
        private void Cal_Amount()
        {
            if (dgvData.Rows.Count() > 0)
            {
                decimal Amount = 0;
                decimal Total = 0;
                foreach (var rd1 in dgvData.Rows)
                {
                    Amount = StockControl.dbClss.TDe(rd1.Cells["Amount"].Value);
                    Total += Amount;
                }
                txtTotal.Text = Total.ToString("###,###,##0.00");
            }
        }
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
           
        }

        //private void cboYear_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        //{
            
        //}

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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            DataLoad();
            btnDel_Item.Enabled = false;
            btnSave.Enabled = false;
            btnNew.Enabled = true;
           
        }

        private void txtCodeNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
 
                    //Insert_data_New();
                    Insert_data_New_Location();
                    //txtCodeNo.Text = "";

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private bool Duppicate(string CodeNo)
        {
            bool re = false;
            dgvData.EndEdit();
            foreach (var g in dgvData.Rows)
            {
                if(Convert.ToString(g.Cells["CodeNo"].Value).Equals(CodeNo))
                {
                    re = true;
                    MessageBox.Show("รหัสพาร์ทซ้ำ");
                    break;
                }
            }

            return re;
        }
        private void Insert_data_New()
        {
            //if(ddlLocation.Text.Trim() =="")
            //{
            //    MessageBox.Show("เลือกสถานที่เก็บสินค้า !");
            //    return;
            //}
            //if (!txtCodeNo.Text.Equals("") && !Duppicate(txtCodeNo.Text))
            //{
            //    using (DataClasses1DataContext db = new DataClasses1DataContext())
            //    {
            //        //int No = 0;
            //        //string CodeNo = "";
            //        //string ItemNo = "";
            //        //string ItemDescription = "";
            //        //decimal QTY = 0;
            //        //decimal RemainQty = 0;
            //        //string UnitShip = "";
            //        //decimal PCSUnit = 0;
            //        //decimal StandardCost = 0;
            //        //decimal Amount = 0;
            //        ////string CRRNCY = "";
            //        //string LotNo = "";
            //        //string SerialNo = "";
            //        //string Remark = "";
            //        //string MachineName = "";
            //        // string Status = "Waiting";
            //        //int id = 0;
            //        int dgvNo = 0;
            //        var r = (from i in db.tb_Items
            //                     //join s in db.tb_Stocks on i.CodeNo equals s.RefNo
            //                 where i.Status == "Active" //&& d.verticalID == VerticalID
            //                    && i.CodeNo == txtCodeNo.Text
            //                 //&& h.VendorNo.Contains(VendorNo_ss)
            //                 select new
            //                 {
            //                     CodeNo = i.CodeNo,
            //                     ItemNo = i.ItemNo,
            //                     ItemDescription = i.ItemDescription,
            //                     RemainQty = (Convert.ToDecimal(db.Cal_QTY(i.CodeNo, "", 0))),
            //                     UnitShip = i.UnitShip,
            //                     PCSUnit = i.PCSUnit,
            //                     StandardCodt = i.StandardCost,//Convert.ToDecimal(dbClss.Get_Stock(i.CodeNo, "", "", "Avg")),//i.StandardCost
            //                     Amount = 0,
            //                     QTY = 0,
            //                     LotNo = "",
            //                     SerialNo = "",
            //                     MachineName = "",
            //                     LineName = "",
            //                     Remark = "",
            //                     id = 0,
            //                     Location = ""

            //                 }
            //        ).ToList();
            //        if (r.Count > 0)
            //        {
            //            dgvNo = dgvData.Rows.Count() + 1;

            //            foreach (var vv in r)
            //            {
            //                //dgvData.Rows.Add(dgvNo.ToString(), vv.CodeNo, vv.ItemNo, vv.ItemDescription
            //                //            , vv.RemainQty, vv.QTY, vv.UnitShip, vv.PCSUnit, vv.StandardCodt, vv.Amount,
            //                //            vv.LotNo, vv.SerialNo, vv.MachineName, vv.LineName, vv.Remark, vv.id
            //                //            );

            //                Add_Item(dgvNo, vv.CodeNo, vv.ItemNo, vv.ItemDescription
            //                            , vv.RemainQty, vv.QTY, vv.UnitShip, dbClss.TDe(vv.PCSUnit), dbClss.TDe(vv.StandardCodt)
            //                            , vv.Amount,vv.LotNo, vv.SerialNo, vv.MachineName, vv.LineName, vv.Remark, vv.id
            //                            ,vv.Location);

            //            }
            //        }
            //        Cal_Amount();

            //    }
            //}
        }
        private void Insert_data_New_Location_None()
        {

            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {

                string JobCard = txtJobCard.Text;
                string Refid = txtRefidJobNo.Text;

                btnNew_Click(null, null);
                txtJobCard.Text = JobCard;
                txtRefidJobNo.Text = Refid;

                int dgvNo = 0;
                var r = (from ix in db.sp_051_Job_list_2(JobCard,ddlType.Text)
                         select ix).ToList();
                if (r.Count > 0)
                {

                    foreach (var vv in r)
                    {
                        dgvNo = dgvData.Rows.Count() + 1;
                        //PCSBaseUOM = dbClss.Con_UOM(vv.CodeNo, vv.BaseUOM);
                        //if (PCSBaseUOM <= 0)
                        //    PCSBaseUOM = 1;
                        //AC_PCSUnit = dbClss.TDe(vv.PCSUnit) * PCSBaseUOM;

                        Add_Item(dgvNo, vv.CodeNo, vv.ItemNo, vv.ItemDescriptioin
                                        , dbClss.TDe(vv.RemainQty), vv.QtyPlan, vv.QtyShip
                                        , vv.UnitShip, dbClss.TDe(vv.PCSUnit)
                                        , dbClss.TDe(vv.UnitCost)
                                        , vv.Amount, vv.LotNo, vv.SerialNo, vv.MachineName, vv.LineName, vv.Remark, 0
                                        , vv.Location, vv.BaseUOM, dbClss.TDe(vv.BasePCSUOM), dbClss.TDe(vv.QtyUsed)
                                        , vv.GroupType, vv.Type, dbClss.TInt(vv.idCSTMPODt), dbClss.TInt(vv.id)
                                        , vv.UnitPlan, vv.UnitUsed
                                        , Math.Round((dbClss.TDe(vv.QtyShip) * dbClss.TDe(vv.PCSUnit)), 2)
                                        );

                    }
                }
                Cal_Amount();

            }
        }
        private void Insert_data_New_Location()
        {

            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {

                string JobCard = txtJobCard.Text;
                //string TempJob = txtTempJobCard.Text;
                string  Refid = txtRefidJobNo.Text  ;
                //string loca = txtLocation.Text;

                btnNew_Click(null, null);
                txtJobCard.Text = JobCard;

                //txtTempJobCard.Text = TempJob;
                txtRefidJobNo.Text = Refid;
                //txtLocation.Text = loca;
                
                int dgvNo = 0;
                var r = (from ix in db.sp_051_Job_list_2(JobCard,ddlType.Text)
                         select ix).ToList();
                if (r.Count > 0)
                {

                    foreach (var vv in r)
                    {
                        dgvNo = dgvData.Rows.Count() + 1;
                        //PCSBaseUOM = dbClss.Con_UOM(vv.CodeNo, vv.BaseUOM);
                        //if (PCSBaseUOM <= 0)
                        //    PCSBaseUOM = 1;
                        //AC_PCSUnit = dbClss.TDe(vv.PCSUnit) * PCSBaseUOM;
                       
                        Add_Item(dgvNo, vv.CodeNo, vv.ItemNo, vv.ItemDescriptioin
                                        , dbClss.TDe( vv.RemainQty), dbClss.TDe(vv.QtyPlan), dbClss.TDe(vv.QtyShip)
                                        , vv.UnitShip, dbClss.TDe(vv.PCSUnit)
                                        , dbClss.TDe(vv.UnitCost)
                                        , vv.Amount, vv.LotNo, vv.SerialNo, vv.MachineName, vv.LineName, vv.Remark,0
                                        , vv.Location, vv.BaseUOM, dbClss.TDe(vv.BasePCSUOM),dbClss.TDe(vv.QtyUsed)
                                        ,vv.GroupType,vv.Type, dbClss.TInt(vv.idCSTMPODt),dbClss.TInt(vv.id)
                                        , vv.UnitPlan, vv.UnitUsed
                                        , Math.Round((dbClss.TDe(vv.QtyShip) * dbClss.TDe(vv.PCSUnit)), 2)
                                        );

                    }
                }
                Cal_Amount();

            }
        }
        private void Add_Item(int Row, string CodeNo, string ItemNo
            , string ItemDescription,decimal RemainQty, decimal QtyPlan, decimal QtyShip, string UnitShip, decimal PCSUnit
           , decimal StandardCost,decimal Amount,string LotNo,string SerialNo,string MachineName,string LineName
            ,string Remark,int id,string Location,string BaseUOM,decimal BasePCSUOM
            ,decimal QtyUsed,string GroupType,string Type, int idCSTMPODt,int idProductionOrderRM
            , string UnitPlan, string UnitUsed, decimal Qty
            )
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
                ee.Cells["QtyPlan"].Value = QtyPlan;
                ee.Cells["QtyShip"].Value = QtyShip;
                ee.Cells["UnitShip"].Value = UnitShip;
                ee.Cells["PCSUnit"].Value = PCSUnit;
                ee.Cells["UnitCost"].Value = StandardCost;
                ee.Cells["Amount"].Value = Amount;
                ee.Cells["LotNo"].Value = LotNo;
                ee.Cells["SerialNo"].Value = SerialNo;
                ee.Cells["MachineName"].Value = MachineName;
                ee.Cells["LineName"].Value = LineName;
                ee.Cells["Remark"].Value = Remark;
                ee.Cells["id"].Value = id;
                ee.Cells["Location"].Value = Location;
                ee.Cells["BaseUOM"].Value = BaseUOM;
                ee.Cells["BasePCSUOM"].Value = BasePCSUOM;
                ee.Cells["QtyUsed"].Value = QtyUsed;
                ee.Cells["GroupType"].Value = GroupType;
                ee.Cells["Type"].Value = Type;
                ee.Cells["idCSTMPODt"].Value = idCSTMPODt;
                ee.Cells["idProductionOrderRM"].Value = idProductionOrderRM;
                ee.Cells["UnitPlan"].Value = UnitPlan;
                ee.Cells["UnitUsed"].Value = UnitUsed;
                ee.Cells["Qty"].Value = Qty;

                //if (GroupCode != "Other")
                //{
                //    ee.Cells["dgvCodeNo"].ReadOnly = true;
                //    ee.Cells["dgvItemName"].ReadOnly = true;
                //    ee.Cells["dgvItemDesc"].ReadOnly = true;


                //    ee.Cells["dgvPCSUnit"].ReadOnly = true;
                //    //ee.Cells["dgvUnitCode"].ReadOnly = true;
                //    //ee.Cells["dgvStandardCost"].ReadOnly = true;
                //}
                //else
                //{
                //    ee.Cells["dgvCodeNo"].ReadOnly = false;
                //    ee.Cells["dgvItemName"].ReadOnly = false;
                //    ee.Cells["dgvItemDesc"].ReadOnly = false;

                //    ee.Cells["dgvPCSUnit"].ReadOnly = false;
                //    //ee.Cells["dgvUnitCode"].ReadOnly = false;
                //    //ee.Cells["dgvStandardCost"].ReadOnly = false;
                //}

                ////if (lblStatus.Text.Equals("Completed"))//|| lbStatus.Text.Equals("Reject"))
                ////    dgvData.AllowAddNewRow = false;
                ////else
                ////    dgvData.AllowAddNewRow = true;

                ////dbclass.SetRowNo1(dgvData);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : Add_Item", this.Name); }

        }
        //private void Insert_data()
        //{
        //    if (!txtCodeNo.Text.Equals("") && !Duppicate(txtCodeNo.Text))
        //    {
        //        using (DataClasses1DataContext db = new DataClasses1DataContext())
        //        {
        //            //int No = 0;
        //            //string CodeNo = "";
        //            //string ItemNo = "";
        //            //string ItemDescription = "";
        //            //decimal QTY = 0;
        //            //decimal RemainQty = 0;
        //            //string UnitShip = "";
        //            //decimal PCSUnit = 0;
        //            //decimal StandardCost = 0;
        //            //decimal Amount = 0;
        //            ////string CRRNCY = "";
        //            //string LotNo = "";
        //            //string SerialNo = "";
        //            //string Remark = "";
        //            //string MachineName = "";
        //            // string Status = "Waiting";
        //            //int id = 0;
        //            int dgvNo = 0;

        //            var r = (from i in db.tb_Items
        //                         //join s in db.tb_Stocks on i.CodeNo equals s.RefNo

        //                     where i.Status == "Active" //&& d.verticalID == VerticalID
        //                        && i.CodeNo == txtCodeNo.Text
        //                     //&& h.VendorNo.Contains(VendorNo_ss)
        //                     select new
        //                     {
        //                         CodeNo = i.CodeNo,
        //                         ItemNo = i.ItemNo,
        //                         ItemDescription = i.ItemDescription,
        //                         RemainQty = (Convert.ToDecimal(db.Cal_QTY(i.CodeNo, "", 0))),
        //                         UnitShip = i.UnitShip,
        //                         PCSUnit = i.PCSUnit,
        //                         StandardCodt = i.StandardCost,//Convert.ToDecimal(dbClss.Get_Stock(i.CodeNo, "", "", "Avg")),//i.StandardCost
        //                         Amount = 0,
        //                         QTY = 0,
        //                         LotNo = "",
        //                         SerialNo = "",
        //                         MachineName = "",
        //                         LineName ="",
        //                         Remark ="",
        //                         id = 0
                                 
        //                     }
        //            ).ToList();
        //            if (r.Count > 0)
        //            {
        //                dgvNo = dgvData.Rows.Count() + 1;

        //                foreach (var vv in r)
        //                {
        //                    dgvData.Rows.Add(dgvNo.ToString(),vv.CodeNo, vv.ItemNo, vv.ItemDescription
        //                                , vv.RemainQty, vv.QTY, vv.UnitShip, vv.PCSUnit, vv.StandardCodt, vv.Amount,
        //                                vv.LotNo, vv.SerialNo, vv.MachineName, vv.LineName, vv.Remark, vv.id
        //                                );
        //                }

        //            }
                    
        //            Cal_Amount();
                     
        //        }
        //    }
        //}

        private void dgvData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                btnDel_Item_Click(null, null);
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
                        string CodeNo = "";
                        CodeNo = StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["CodeNo"].Value);
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

        private void btnListItem_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = false;
                //btnEdit.Enabled = true;
                //btnView.Enabled = false;
                btnNew.Enabled = true;
                ClearData();
                Enable_Status(false, "View");
                RadTextBox txtCodeNo = new RadTextBox();
                txtCodeNo.Text = "";

                this.Cursor = Cursors.WaitCursor;
                Accident_Slip_List sc = new Accident_Slip_List(txtSHNo, txtCodeNo,"Accident_Slip");
                this.Cursor = Cursors.Default;
                sc.ShowDialog();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();
                //LoadData

                string CodeNo = txtCodeNo.Text;
                string SHNo = txtSHNo.Text;
                if (!txtSHNo.Text.Equals(""))
                {
                    txtCodeNo.Text = "";

                    DataLoad();
                    Ac = "View";
                    btnDel_Item.Enabled = false;
                    btnSave.Enabled = false;
                    btnNew.Enabled = true;
                }
                else
                {
                    btnDel_Item.Enabled = true;
                    btnNew_Click(null, null);
                    txtCodeNo.Text = CodeNo;

                    Insert_data_New();
                    txtCodeNo.Text = "";

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : radButtonElement1_Click", this.Name); }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                PrintPR a = new PrintPR(txtSHNo.Text, txtSHNo.Text, "ReportAccidentSlip");
                a.ShowDialog();

                //using (DataClasses1DataContext db = new DataClasses1DataContext())
                //{
                //    var g = (from ix in db.sp_R004_ReportShipping(txtSHNo.Text, DateTime.Now) select ix).ToList();
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
        private void Add_JobCard()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                var p = (from ix in db.mh_ProductionOrderRMs select ix)
                         .Where
                         (a => a.JobNo.Trim().ToUpper() == txtJobCard.Text.Trim().ToUpper() && a.Active == true

                         ).ToList();
                if (p.Count > 0)
                {
                    if (dbClss.TBo(p.FirstOrDefault().Active) == true)
                    {
                        txtRefidJobNo.Text = dbClss.TSt(p.FirstOrDefault().id);                      
                    }
                    else if (dbClss.TBo(p.FirstOrDefault().Active) == false)
                    {
                        txtJobCard.Text = "";
                        txtRefidJobNo.Text = "0";
                        MessageBox.Show("ใบงานการผลิตดังกล่าวถูกปิดไปแล้ว กรุณาระบุใบงานการผลิตใหม่");
                        return;
                    }

                }
                else
                {
                    txtJobCard.Text = "";
                    txtRefidJobNo.Text = "0";
                    return;
                }

                if (ddlType.Text=="None")
                {
                    Insert_data_New_Location_None();
                }
                else if (ddlType.Text == "CutStock")
                {
                    Insert_data_New_Location();
                }
            }
        }

        private void txtJobCard_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            try
            {
                
                if (e.KeyValue == 13 || e.KeyValue == 9)
                {
                    if (txtJobCard.Text.Trim() == "")
                        return;

                    if (ddlType.Text.Trim() == "")
                    {
                        MessageBox.Show("เลือกประเภท !!!");
                        return;
                    }

                    Add_JobCard();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void cbShipforJob_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if(cbShipforJob.Checked)
            {
                txtJobCard.ReadOnly = false;
            }
            else
            {
                txtJobCard.ReadOnly = true;
                txtJobCard.Text = "";
                //txtTempJobCard.Text = "";
                txtRefidJobNo.Text = "0";
            }
        }

        private void txtJobCard_Leave(object sender, EventArgs e)
        {
           // Add_JobCard();
        }

        private void txtJobCard_TextChanged(object sender, EventArgs e)
        {
            //if (txtJobCard.Text != "")
            //    cbShipforJob.Checked = true;
            //else
            //    cbShipforJob.Checked = false;
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
                    Comcol.DropDownWidth = 150;
                    Comcol.DropDownHeight = 150;
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
                        HeaderText = "รหัส",
                        Name = "Code",
                        FieldName = "Code",
                        Width = 80,
                        AllowFiltering = true,
                        ReadOnly = false
                    }
                   );
                    Comcol.EditorControl.Columns.Add(new GridViewTextBoxColumn
                    {
                        HeaderText = "ชื่อ",
                        Name = "Name",
                        FieldName = "Name",
                        Width = 150,
                        AllowFiltering = true,
                        ReadOnly = false
                    }
                   );

                    //dgvDataDetail.CellEditorInitialized += MasterTemplate_CellEditorInitialized;

                }
                if (e.Column.Name.Equals("UnitShip"))
                {
                    /////////////มีการ เคลียร์ การ Add ก่อน แล้วค่อย Add ใหม่////////////////
                    //Row = e.RowIndex;
                    RadMultiColumnComboBoxElement Comcol = (RadMultiColumnComboBoxElement)e.ActiveEditor;
                    Comcol.Columns.Clear();

                    //RadMultiColumnComboBoxElement Comcol = (RadMultiColumnComboBoxElement)e.ActiveEditor;
                    Comcol.DropDownSizingMode = SizingMode.UpDownAndRightBottom;
                    Comcol.DropDownWidth = 150;
                    Comcol.DropDownHeight = 150;
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

        private void MasterTemplate_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            RadMultiColumnComboBoxElement mccbEl = e.ActiveEditor as RadMultiColumnComboBoxElement;
            if (mccbEl != null)
            {
                mccbEl.DropDownSizingMode = SizingMode.UpDownAndRightBottom;
                mccbEl.DropDownMinSize = new Size(200, 100);
                mccbEl.DropDownMaxSize = new Size(200, 100);

                mccbEl.AutoSizeDropDownToBestFit = false;
                mccbEl.DropDownAnimationEnabled = false;
                mccbEl.AutoFilter = true;
                FilterDescriptor filterDescriptor = new FilterDescriptor(mccbEl.DisplayMember, FilterOperator.Contains, string.Empty);
                mccbEl.EditorControl.MasterTemplate.FilterDescriptors.Add(filterDescriptor);
            }
        }

        private void radButtonElement1_Click(object sender, EventArgs e)
        {
            PrintPR a = new PrintPR(txtSHNo.Text, txtSHNo.Text, "ShippingToDay");
            a.ShowDialog();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("ต้องการยกเลิกรายการ ( " + txtSHNo.Text + " ) หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    {

                        MessageBox.Show("รอเช็คเรื่องการปิดจอบแล้วจะ ลบไมไม่ได้");
                        return;

                        var unit1 = (from ix in db.mh_ProductionOrders
                                     where ix.Active == true
                                        && ix.JobNo.Trim().ToUpper() == txtJobCard.Text.Trim().ToUpper()
                                     select ix).ToList();
                        foreach (var d in unit1)
                        {
                            string CancelNo = StockControl.dbClss.GetNo(36, 2);
                            db.sp_073_Accident_Slip_Del(CancelNo, txtSHNo.Text, txtJobCard.Text, Classlib.User, Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")));

                            baseClass.Info("Delete accident slip complete.");
                            dbClss.AddHistory(this.Name, "Delete accident slip", "Delete accident slip : [" + txtSHNo.Text + "]", "");

                            btnNew_Click(null, null);
                        }


                    }
                }
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
