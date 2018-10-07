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
    public partial class PriceList : Telerik.WinControls.UI.RadRibbonForm
    {
        public PriceList()
        {
            InitializeComponent();
        }
        public PriceList(string SHNo)
        {
            InitializeComponent();
            SHNo_t = SHNo;
        }
       
        string SHNo_t = "";
        string CodeNo_t = "";
        string Ac = "";
        DataTable dt_h = new DataTable();

        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            HistoryView hw = new HistoryView(this.Name, txtPriceListCode.Text);
            this.Cursor = Cursors.Default;
            hw.ShowDialog();
        }

        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }
        private void GETDTRow()
        {
            dt_h.Columns.Add(new DataColumn("InternalNo", typeof(string)));
            dt_h.Columns.Add(new DataColumn("PriceListCode", typeof(string)));
            dt_h.Columns.Add(new DataColumn("UnitPrice", typeof(decimal)));
            dt_h.Columns.Add(new DataColumn("StartDate", typeof(DateTime)));
            dt_h.Columns.Add(new DataColumn("EndDate", typeof(DateTime)));
            dt_h.Columns.Add(new DataColumn("CreateDate", typeof(DateTime)));
            dt_h.Columns.Add(new DataColumn("CreateBy", typeof(string)));
            dt_h.Columns.Add(new DataColumn("ModifyDate", typeof(DateTime)));
            dt_h.Columns.Add(new DataColumn("ModifyBy", typeof(string)));
            dt_h.Columns.Add(new DataColumn("SendApproveBy", typeof(string)));
            dt_h.Columns.Add(new DataColumn("SendApproveDate", typeof(DateTime)));
            dt_h.Columns.Add(new DataColumn("ApproveBy", typeof(string)));
            dt_h.Columns.Add(new DataColumn("ApproveDate", typeof(DateTime)));
            dt_h.Columns.Add(new DataColumn("SeqStatus", typeof(int)));
            dt_h.Columns.Add(new DataColumn("Active", typeof(bool)));
            dt_h.Columns.Add(new DataColumn("Status", typeof(string)));
            dt_h.Columns.Add(new DataColumn("Remark", typeof(string)));


        }

        private void Unit_Load(object sender, EventArgs e)
        {
           
            GETDTRow();   
            //DefaultItem();
            
            btnNew_Click(null, null);

            if (!SHNo_t.Equals(""))
            {
                btnNew.Enabled = true;
                txtPriceListCode.Text = SHNo_t;
                DataLoad();
                Ac = "View";               
            }          
        }
        private void DefaultItem()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {               
              
            }
        }
        private void DataLoad()
        {
            string temp = txtPriceListCode.Text;
            ClearData();
            txtPriceListCode.Text = temp;

            dt_h.Rows.Clear();
            try
            {

                this.Cursor = Cursors.WaitCursor;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {

                    try
                    {
                        var g = (from ix in db.mh_PriceLists select ix)
                            .Where(a => a.PriceListCode == txtPriceListCode.Text.Trim()).ToList();
                        if (g.Count() > 0)
                        {
                            dt_h = StockControl.dbClss.LINQToDataTable(g);

                            txtInternalNo.Text = dbClss.TSt(g.FirstOrDefault().InternalNo);
                            seUnitPrice.Value = dbClss.TDe(g.FirstOrDefault().UnitPrice);
                            DateTime? a = null;
                            if (dbClss.TSt(g.FirstOrDefault().StartDate) != "")
                                dtStartDate.Value = Convert.ToDateTime(g.FirstOrDefault().StartDate);
                            else
                                dtStartDate.Value = a.Value;

                            if (dbClss.TSt(g.FirstOrDefault().EndDate) != "")
                                dtStartDate.Value = Convert.ToDateTime(g.FirstOrDefault().EndDate);
                            else
                                dtStartDate.Value = a.Value;

                            lblStatus.Text = dbClss.TSt(g.FirstOrDefault().Status);
                            txtCreateBy.Text = dbClss.TSt(g.FirstOrDefault().CreateBy);
                            txtCreateDate.Text = Convert.ToDateTime(g.FirstOrDefault().CreateDate).ToString("dd/MMM/yyyy");
                            txtPriceListCode.Enabled = false;
                            txtRemark.Text = dbClss.TSt(g.FirstOrDefault().Remark);

                            
                            if (lblStatus.Text == "Approve")
                            {
                                btnSendApprove.Enabled = false;
                                btnSave.Enabled = false;
                                btnEdit.Enabled = false;
                            }
                            else if  (lblStatus.Text == "Waiting" || lblStatus.Text == "Waiting Approve")
                            {
                                btnEdit.Enabled = true;
                                btnSave.Enabled = true;
                                btnSendApprove.Enabled = true;
                                btnView.Enabled = false;
                               
                            }
                            btnNew.Enabled = true;

                        }
                        else
                            txtPriceListCode.Enabled = true;
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }

                }
            }
            catch { }
            finally { this.Cursor = Cursors.Default; }
            

        }
        private bool CheckDuplicate(string code)
        {
            bool ck = false;

            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                int i = (from ix in db.mh_PriceLists
                         where ix.PriceListCode.ToUpper().Trim() == code.ToUpper().Trim()
                         && ix.Status != "Cancel"
                         select ix).Count();
                if (i > 0)
                    ck = true;
                else
                    ck = false;
            }

            return ck;
        }

        private void ClearData()
        {
            dtEndDate.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
            dtStartDate.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
            txtCreateBy.Text = ClassLib.Classlib.User;
            seUnitPrice.Value = 0;
            txtPriceListCode.Text = "";
            txtInternalNo.Text = "";
            txtCreateDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            txtRemark.Text = "";

            lblStatus.Text = "-";
        }
      private void Enable_Status(bool ss,string Condition)
        {
            if (Condition.Equals("-") || Condition.Equals("New"))
            {
                txtPriceListCode.Enabled = ss;
                seUnitPrice.Enabled = ss;
                txtInternalNo.Enabled = ss;
                dtStartDate.Enabled = ss;
                dtEndDate.Enabled = ss;
                btnDel_Item.Enabled = ss;
            }
            else if (Condition.Equals("View"))
            {
                txtPriceListCode.Enabled = ss;
                seUnitPrice.Enabled = ss;
                txtInternalNo.Enabled = ss;
                dtStartDate.Enabled = ss;
                dtEndDate.Enabled = ss;
                btnDel_Item.Enabled = ss;
            }

            else if (Condition.Equals("Edit"))
            {
                //txtPriceListCode.Enabled = ss;
                seUnitPrice.Enabled = ss;
                //txtInternalNo.Enabled = ss;
                btnAdd_Item.Enabled = false;
                dtStartDate.Enabled = ss;
                dtEndDate.Enabled = ss;
                btnDel_Item.Enabled = ss;
            }
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            btnEdit.Enabled = true;
            btnDel_Item.Enabled = true;
            btnNew.Enabled = true;
            btnSave.Enabled = true;
            btnView.Enabled = false;
            ClearData();
            Enable_Status(true, "New");
            lblStatus.Text = "New";
            Ac = "New";

            // getมาไว้ก่อน แต่ยังไมได้ save
            txtPriceListCode.Text = StockControl.dbClss.GetNo(41, 0);
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            btnView.Enabled = false;
            btnEdit.Enabled = true;
            btnNew.Enabled = true;
            btnSave.Enabled = true;

            Enable_Status(false, "View");
            lblStatus.Text = "View";
            Ac = "View";
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            btnView.Enabled = true;
            btnEdit.Enabled = false;
            btnNew.Enabled = true;
            btnSave.Enabled = true;
            btnDel.Enabled = true;

            Enable_Status(true, "Edit");
            //lblStatus.Text = "Edit";
            Ac = "Edit";
        }

        private bool Check_Save()
        {
            bool re = true;
            string err = "";
            try
            {

                //if (txtPriceListCode.Text.Equals(""))
                //    err += "- “เลขที่อ้างอิง:” เป็นค่าว่าง \n";
                //else
                if (!txtPriceListCode.Text.Equals(""))
                {
                    if(Ac=="New")
                    {
                        if(CheckDuplicate(txtPriceListCode.Text))
                            err += "- “เลขทีอ้างอิงซ้ำ” \n";
                    }
                }
                if (txtInternalNo.Text.Equals(""))
                    err += "- “รหัสสินค้า:” เป็นค่าว่าง \n";
                if (dtStartDate.Text.Equals(""))
                    err += "- “วันที่เริมใช้:” เป็นค่าว่าง \n";

                if (dtEndDate.Text.Equals(""))
                    err += "- “วันทีสิ้นสุด:” เป็นค่าว่าง \n";


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
        private void SaveHerder()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                var g = (from ix in db.mh_PriceLists
                         where ix.PriceListCode.Trim() == txtPriceListCode.Text.Trim() && ix.Status != "Cancel"
                         select ix).ToList();
                if (g.Count > 0)  //มีรายการในระบบ
                {
                    foreach (DataRow row in dt_h.Rows)
                    {
                        var gg = (from ix in db.mh_PriceLists
                                  where ix.PriceListCode.Trim() == txtPriceListCode.Text.Trim() && ix.Status != "Cancel"
                                  select ix).First();

                        //gg.CreateBy = ClassLib.Classlib.User;
                        //gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                        gg.ModifyBy = ClassLib.Classlib.User;
                        gg.ModifyDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));

                        dbClss.AddHistory(this.Name, "แก้ไข PriceList", "แก้ไขโดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", txtPriceListCode.Text);
                        //if (StockControl.dbClss.TSt(gg.BarCode).Equals(""))
                        //    gg.BarCode = StockControl.dbClss.SaveQRCode2D(txtPriceListCode.Text.Trim());

                        if (!txtInternalNo.Text.Trim().Equals(row["InternalNo"].ToString()))
                        {
                            gg.InternalNo = txtInternalNo.Text;                           
                            dbClss.AddHistory(this.Name, "แก้ไข PriceList", "แก้ไขรหัสสินค้า [" + txtInternalNo.Text.Trim() + " เดิม :" + row["InternalNo"].ToString() + "]", txtPriceListCode.Text);
                        }
                        if (!seUnitPrice.Text.Trim().Equals(row["UnitPrice"].ToString()))
                        {
                            gg.UnitPrice = dbClss.TDe(seUnitPrice.Value);
                            dbClss.AddHistory(this.Name, "แก้ไข PriceList", "แก้ไขราคาสินค้า [" + seUnitPrice.Text.Trim() + " เดิม :" + row["UnitPrice"].ToString() + "]", txtPriceListCode.Text);
                        }
                        if (!dtStartDate.Value.ToString("yyyyMMdd").Equals(Convert.ToDateTime(row["StartDate"]).ToString("yyyyMMdd")))
                        {
                            gg.StartDate = Convert.ToDateTime(dtStartDate.Value,new CultureInfo("en-US"));
                            dbClss.AddHistory(this.Name, "แก้ไข PriceList", "แก้ไขวันทีเริ่มใช [" + dtStartDate.Text.Trim() + " เดิม :" + row["StartDate"].ToString() + "]", txtPriceListCode.Text);
                        }
                        if (!dtEndDate.Value.ToString("yyyyMMdd").Equals(Convert.ToDateTime(row["EndDate"]).ToString("yyyyMMdd")))
                        {
                            gg.EndDate = Convert.ToDateTime(dtEndDate.Value, new CultureInfo("en-US"));
                            dbClss.AddHistory(this.Name, "แก้ไข PriceList", "แก้ไขวันทีสิ้นสุด [" + dtEndDate.Text.Trim() + " เดิม :" + row["EndDate"].ToString() + "]", txtPriceListCode.Text);
                        }
                        if (!txtRemark.Text.Trim().Equals(row["Remark"].ToString()))
                        {
                            gg.Remark = txtRemark.Text;
                            dbClss.AddHistory(this.Name, "แก้ไข PriceList", "แก้ไขหมายเหตุ [" + txtRemark.Text.Trim() + " เดิม :" + row["Remark"].ToString() + "]", txtPriceListCode.Text);
                        }
                        db.SubmitChanges();
                    }
                }
                else //สร้างใหม่
                {
                    //byte[] barcode = null;
                    //barcode = StockControl.dbClss.SaveQRCode2D(txtPriceListCode.Text.Trim());

                    DateTime? UpdateDate = null;

                    mh_PriceList gg = new mh_PriceList();                   
                    gg.PriceListCode = txtPriceListCode.Text;
                    gg.StartDate = Convert.ToDateTime(dtStartDate.Value, new CultureInfo("en-US"));
                    gg.EndDate = Convert.ToDateTime(dtEndDate.Value, new CultureInfo("en-US"));
                    gg.ModifyBy = ClassLib.Classlib.User;
                    gg.ModifyDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    gg.CreateBy = ClassLib.Classlib.User;
                    gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    gg.InternalNo = txtInternalNo.Text.Trim();
                    gg.Remark = txtRemark.Text;
                    gg.UnitPrice = dbClss.TDe(seUnitPrice.Value);

                    gg.Status = "Waiting";
                 
                    db.mh_PriceLists.InsertOnSubmit(gg);
                    db.SubmitChanges();

                    dbClss.AddHistory(this.Name, "แก้ไข PriceList", "สร้าง การเบิกสินค้า [" + txtPriceListCode.Text.Trim() + "]", txtPriceListCode.Text);
                }
            }
        }
     
        private void btnSave_Click(object sender, EventArgs e)
        {
            
                if (Ac.Equals("New") || Ac.Equals("Edit"))
                {
                    if (Check_Save())
                        return;
                    if (MessageBox.Show("ต้องการบันทึก ?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.Cursor = Cursors.WaitCursor;

                    if (Ac.Equals("New")  && txtPriceListCode.Text=="")
                        txtPriceListCode.Text = StockControl.dbClss.GetNo(41, 2);

                    if (!txtPriceListCode.Text.Equals(""))
                    {
                        SaveHerder();
                      
                        
                        DataLoad();
                        btnNew.Enabled = true;
                        //btnDel_Item.Enabled = false;
                        
                        MessageBox.Show("บันทึกสำเร็จ!");
                        btnRefresh_Click(null,null);
                    }
                    else
                    {
                        MessageBox.Show("ไม่สามารถโหลดเลขที่เอกสารได้ ติดต่อแผนก IT");
                    }
                    }
                }
        }
        
        
      
      
        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            //try
            //{
               
            //    if (e.RowIndex >= -1)
            //    {

            //        if (dgvData.Columns["QTY"].Index == e.ColumnIndex)
            //        {

            //            if (dbClss.TSt(e.Row.Cells["UnitShip"].Value) == "")
            //            {
            //                e.Row.Cells["QTY"].Value = 0;
            //                MessageBox.Show("หน่วยเบิกเป็นค่าว่าง");
            //            }
            //            else
            //            {
            //                //Cal Remain Qty

            //                //string dgvUOM = dbClss.TSt(e.Row.Cells["UnitShip"].Value);
            //                string CodeNo = dbClss.TSt(e.Row.Cells["CodeNo"].Value);
            //                decimal PCSUnit = dbClss.TDe(e.Row.Cells["PCSUnit"].Value);
            //                string BaseUOM = dbClss.TSt(e.Row.Cells["BaseUOM"].Value);
            //                decimal BasePCSUOM = dbClss.Con_UOM(CodeNo, BaseUOM);

            //                //using (DataClasses1DataContext db = new DataClasses1DataContext())
            //                //{
            //                //    var g = (from ix in db.mh_Items select ix).Where(a => a.InternalNo == CodeNo).ToList();
            //                //    if (g.Count() > 0)
            //                //    {
            //                //        BaseUom = dbClss.TSt(g.FirstOrDefault().BaseUOM);
            //                //        BasePCSUOM = dbClss.Con_UOM(CodeNo, BaseUom);
            //                //    }
            //                //}

            //                decimal QTY = 0;
            //                decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["QTY"].Value), out QTY);
            //                decimal RemainQty = 0;
            //                decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["RemainQty"].Value), out RemainQty);
            //                if (BasePCSUOM <= 0)
            //                    BasePCSUOM = 1;

            //                decimal Temp = 0;
            //                //Temp = BasePCSUOM * PCSUnit * QTY;

            //                Temp = Check_RemainStock(CodeNo, PCSUnit, BaseUOM, BasePCSUOM, QTY, RemainQty);

            //                if (Temp > RemainQty)
            //                {
            //                    MessageBox.Show("ไม่สามารถเบิกเกินจำนวนคงเหลือได้");
            //                    e.Row.Cells["QTY"].Value = 0;
            //                    QTY = 0;
            //                }


            //                if (QTY > 0)
            //                {
            //                    // ใช้ 0 เพราะต้องการเอาราคาล่าสุดของ stock free
            //                    // ใช้ -1 คือเอาทุกรายการรับ
            //                    int idCSTMPODt = 0;// dbClss.TInt(txtidCSTMPODt.Text);
            //                    int Free = 0;
            //                    if (idCSTMPODt > 0) Free = 0;
            //                    //else if (idCSTMPODt == 0) Free = 1;

            //                    e.Row.Cells["StandardCost"].Value = Get_UnitCostFIFO(dbClss.TSt(e.Row.Cells["CodeNo"].Value), QTY, dbClss.TSt(e.Row.Cells["Location"].Value), idCSTMPODt, Free);
            //                }
            //            }
            //        }

            //        if (dgvData.Columns["QTY"].Index == e.ColumnIndex
            //            || dgvData.Columns["StandardCost"].Index == e.ColumnIndex
            //            )
            //        {
            //            decimal QTY = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["QTY"].Value), out QTY);
            //            decimal CostPerUnit = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["StandardCost"].Value), out CostPerUnit);
            //            e.Row.Cells["Amount"].Value = QTY * CostPerUnit;
            //            Cal_Amount();
            //        }
            //        else if (dgvData.Columns["UnitShip"].Index == e.ColumnIndex)
            //        {
            //            string dgvUOM = dbClss.TSt(e.Row.Cells["UnitShip"].Value);
            //            string CodeNo = dbClss.TSt(e.Row.Cells["CodeNo"].Value);
            //            e.Row.Cells["PCSUnit"].Value = dbClss.Con_UOM(CodeNo, dgvUOM);
                        
            //            //Cal Remain Qty
            //            decimal PCSUnit = dbClss.TDe(e.Row.Cells["PCSUnit"].Value);
            //            string BaseUOM = dbClss.TSt(e.Row.Cells["BaseUOM"].Value);
            //            decimal BasePCSUOM  = dbClss.Con_UOM(CodeNo, BaseUOM);

            //            decimal QTY = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["QTY"].Value), out QTY);
            //            decimal RemainQty = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["RemainQty"].Value), out RemainQty);
            //            if (BasePCSUOM <= 0)
            //                BasePCSUOM = 1;

            //            decimal Temp = 0;
            //            //Temp = BasePCSUOM * PCSUnit * QTY;

            //            Temp = Check_RemainStock(CodeNo, PCSUnit, BaseUOM, BasePCSUOM, QTY, RemainQty);

            //            if (Temp > RemainQty)
            //            {
            //                MessageBox.Show("ไม่สามารถเบิกเกินจำนวนคงเหลือได้");
            //                e.Row.Cells["QTY"].Value = 0;
            //                QTY = 0;
            //            }

            //        }
            //        else if (dgvData.Columns["Location"].Index == e.ColumnIndex)
            //        {
            //            using (DataClasses1DataContext db = new DataClasses1DataContext())
            //            {
            //                e.Row.Cells["RemainQty"].Value = (Convert.ToDecimal(db.Cal_QTY_Remain_Location(Convert.ToString(e.Row.Cells["CodeNo"].Value), "SafetyStock", 0, Convert.ToString(e.Row.Cells["Location"].Value),0)));


            //                string dgvUOM = dbClss.TSt(e.Row.Cells["UnitShip"].Value);
            //                string CodeNo = dbClss.TSt(e.Row.Cells["CodeNo"].Value);
            //                e.Row.Cells["PCSUnit"].Value = dbClss.Con_UOM(CodeNo, dgvUOM);

            //                //Cal Remain Qty
            //                decimal PCSUnit = dbClss.TDe(e.Row.Cells["PCSUnit"].Value);
            //                string BaseUOM = dbClss.TSt(e.Row.Cells["BaseUOM"].Value);
            //                decimal BasePCSUOM  = dbClss.Con_UOM(CodeNo, BaseUOM);

            //                decimal QTY = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["QTY"].Value), out QTY);
            //                decimal RemainQty = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["RemainQty"].Value), out RemainQty);
            //                if (BasePCSUOM <= 0)
            //                    BasePCSUOM = 1;

            //                decimal Temp = 0;
            //                //Temp = BasePCSUOM * PCSUnit * QTY;

            //                Temp = Check_RemainStock(CodeNo, PCSUnit, BaseUOM, BasePCSUOM, QTY, RemainQty);

            //                if (Temp > RemainQty)
            //                {
            //                    MessageBox.Show("ไม่สามารถเบิกเกินจำนวนคงเหลือได้");
            //                    e.Row.Cells["QTY"].Value = 0;
            //                    QTY = 0;
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
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
            //if (dgvData.Rows.Count() > 0)
            //{
            //    decimal Amount = 0;
            //    decimal Total = 0;
            //    foreach (var rd1 in dgvData.Rows)
            //    {
            //        Amount = StockControl.dbClss.TDe(rd1.Cells["Amount"].Value);
            //        Total += Amount;
            //    }
            //    txtTotal.Text = Total.ToString("###,###,##0.00");
            //}
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
            //dbClss.ExportGridXlSX(dgvData);
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
            btnView_Click(null, null);
            DataLoad();
           
           
        }

        private void txtCodeNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    //Insert_data_New_Location();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
      
   
   
      
    
        private void dgvData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                btnDel_Item_Click(null, null);
        }

        private void btnDel_Item_Click(object sender, EventArgs e)
        {
            try
            {
                //if (Ac.Equals("New"))// || Ac.Equals("Edit"))
                //{
                //    this.Cursor = Cursors.WaitCursor;

                //    int id = 0;
                //    int.TryParse(StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["id"].Value), out id);
                //    if (id <= 0)
                //        dgvData.Rows.Remove(dgvData.CurrentRow);

                //    else
                //    {
                //        string CodeNo = "";
                //        CodeNo = StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["CodeNo"].Value);
                //        if (MessageBox.Show("ต้องการลบรายการ ( " + CodeNo + " ) ออกจากรายการ หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //        {
                //            dgvData.CurrentRow.IsVisible = false;
                //        }
                //    }
                //    SetRowNo1(dgvData);
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

                this.Cursor = Cursors.WaitCursor;
                PriceList_List sc = new PriceList_List(txtPriceListCode,"PriceList");
                this.Cursor = Cursors.Default;
                sc.ShowDialog();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();
               
                if (!txtPriceListCode.Text.Equals(""))
                {

                    DataLoad();
                    Ac = "View";
                    btnDel_Item.Enabled = false;
                    btnSave.Enabled = false;
                    btnNew.Enabled = true;
                }
                
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : radButtonElement1_Click", this.Name); }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                PrintPR a = new PrintPR(txtPriceListCode.Text, txtPriceListCode.Text, "Shipping");
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
            //using (DataClasses1DataContext db = new DataClasses1DataContext())
            //{
            //    var p = (from ix in db.mh_ProductionOrders select ix)
            //         .Where
            //         (a => a.JobNo.Trim().ToUpper() == txtJobCard.Text.Trim().ToUpper() && a.Active ==true

            //         ).ToList();
            //    if (p.Count > 0)
            //    {
            //        //if (dbClss.TSt(p.FirstOrDefault().Status) != "Completed")
            //        //{
            //            //txtTempJobCard.Text = dbClss.TSt(p.FirstOrDefault().TempJobCard);
            //            txtRefidJobNo.Text = dbClss.TSt(p.FirstOrDefault().id);
            //            txtidCSTMPODt.Text = dbClss.TInt(p.FirstOrDefault().RefDocId).ToString();
            //        //}
            //        //else if (dbClss.TSt(p.FirstOrDefault().Status) != "Completed")
            //        //{
            //        //    txtTempJobCard.Text = "";
            //        //    txtJobCard.Text = "";
            //        //    txtRefidJobNo.Text = "0";
            //        //    MessageBox.Show("ใบงานการผลิตดังกล่าวถูกปิดไปแล้ว กรุณาระบุใบงานการผลิตใหม่");
            //        //}

            //    }
            //    else
            //    {
            //        txtJobCard.Text = "";
            //        txtTempJobCard.Text = "";
            //        txtRefidJobNo.Text = "0";
            //    }
            //}
        }

        private void txtJobCard_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //try
            //{
            //    if (txtJobCard.Text.Trim() == "")
            //        return;

            //    if (e.KeyValue == 13 || e.KeyValue == 9)
            //    {
            //        Add_JobCard();
            //    }
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void cbShipforJob_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            //if(cbShipforJob.Checked)
            //{
            //    txtJobCard.ReadOnly = false;
            //}
            //else
            //{
            //    txtJobCard.ReadOnly = true;
            //    txtJobCard.Text = "";
            //    txtTempJobCard.Text = "";
            //    txtRefidJobNo.Text = "0";
            //}
        }

        private void txtJobCard_Leave(object sender, EventArgs e)
        {
            Add_JobCard();
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
                    Comcol.DropDownWidth = 350;
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
                        HeaderText = "สถานที่เก็บ",
                        Name = "Location",
                        FieldName = "Location",
                        Width = 100,
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
                mccbEl.DropDownMinSize = new Size(300, 100);
                mccbEl.DropDownMaxSize = new Size(300, 100);

                mccbEl.AutoSizeDropDownToBestFit = false;
                mccbEl.DropDownAnimationEnabled = false;
                mccbEl.AutoFilter = true;
                FilterDescriptor filterDescriptor = new FilterDescriptor(mccbEl.DisplayMember, FilterOperator.Contains, string.Empty);
                mccbEl.EditorControl.MasterTemplate.FilterDescriptors.Add(filterDescriptor);
            }
        }

        private void radButtonElement1_Click(object sender, EventArgs e)
        {
            PrintPR a = new PrintPR(txtPriceListCode.Text, txtPriceListCode.Text, "ShippingToDay");
            a.ShowDialog();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (baseClass.IsDel($"Do you want to Delete shipping: {txtPriceListCode.Text} ?"))
                {
                    using (var db = new DataClasses1DataContext())
                    {
                        
                        //var g = (from ix in db.tb_Shipping_FGs
                        //         where ix.RefNo == txtPriceListCode.Text && ix.Active == true
                        //         select ix).ToList();
                        //if (g.Count > 0)
                        //{
                        //    foreach (var gg in g)
                        //    {
                        //        var f = (from ix in db.mh_ShipmentDTs
                        //                 where ix.id == Convert.ToInt16(gg.Refid_dt)
                        //                 && ix.OutInv < ix.Qty
                        //                 select ix).ToList();
                        //        if (f.Count > 0)
                        //        {
                        //            temp += 1;
                        //            break;
                        //        }
                        //    }
                        //    if (temp > 0)
                        //    {
                        //        MessageBox.Show("บางรายการถูกนำไปสร้างใบแจ้งหนี้ (Invoice) แล้ว ไม่สามารถทำรายการได้");
                        //        return;
                        //    }                          
                                
                        //    }                        
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnSendApprove_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    if (lblStatus.Text == "Waiting" || Ac == "Edit" || Ac=="New"
                        )
                    {
                        if (baseClass.IsSendApprove())
                        {
                            db.sp_062_mh_ApproveList_Add(txtPriceListCode.Text.Trim(), "Price List", ClassLib.Classlib.User);
                            MessageBox.Show("Send complete.");
                            btnRefresh_Click(null, null);
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnAdd_Item_Click(object sender, EventArgs e)
        {
            try
            {

                this.Cursor = Cursors.WaitCursor;
                List<GridViewRowInfo> dgvRow_List = new List<GridViewRowInfo>();
                ListPart sc = new ListPart(dgvRow_List, "FG-SEMI", "AdjustStock");

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
                        CodeNo = Convert.ToString(ee.Cells["InternalNo"].Value).Trim();
                        txtInternalNo.Text = CodeNo;
                        if (txtInternalNo.Text != "")
                        {
                            Insert_data(txtInternalNo.Text);
                        }

                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : btnAdd_Item_Click", this.Name); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void txtPriceListCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    DataLoad();
              

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
                        
                        var d1 = (from ix in db.mh_Items select ix)
                            .Where(a => a.InternalNo == CodeNo.Trim() && a.Active == true

                            ).ToList();
                        if (d1.Count > 0)
                        {
                            var d = (from ix in db.mh_Items select ix)
                            .Where(a => a.InternalNo == CodeNo.Trim() && a.Active == true

                            ).First();
                            txtInternalNo.Text = d.InternalNo;
                       
                        }
                        else
                            txtInternalNo.Text = "";
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void txtInternalNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {

                    Insert_data(txtInternalNo.Text);


                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
