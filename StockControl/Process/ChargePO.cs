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
using Microsoft.VisualBasic;
namespace StockControl
{
    public partial class ChargePO : Telerik.WinControls.UI.RadRibbonForm
    {
        public ChargePO()
        {
            InitializeComponent();
        }
        public ChargePO(string CHNo)
        {
            InitializeComponent();
            CHNo_temp = CHNo;
        }
        public ChargePO(List<GridViewRowInfo> RetDT)
        {
            InitializeComponent();
            this.RetDT = RetDT;
        }
        //private int RowView = 50;
        //private int ColView = 10;
        //DataTable dt = new DataTable();
        List<GridViewRowInfo> RetDT;
        string CHNo_temp = "";
        DataTable dt_POHD = new DataTable();
        DataTable dt_PODT = new DataTable();
        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            HistoryView hw = new HistoryView(this.Name,txtCHNo.Text);
            this.Cursor = Cursors.Default;
            hw.ShowDialog();
        }

        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }
        private void GETDTRow()
        {
            dt_POHD.Columns.Add(new DataColumn("CHNo", typeof(string)));
            dt_POHD.Columns.Add(new DataColumn("CHBy", typeof(string)));
            dt_POHD.Columns.Add(new DataColumn("CHDate", typeof(DateTime)));
            dt_POHD.Columns.Add(new DataColumn("CHStatus", typeof(string)));
            dt_POHD.Columns.Add(new DataColumn("CHRemark", typeof(string)));
            dt_POHD.Columns.Add(new DataColumn("id", typeof(decimal)));
            dt_POHD.Columns.Add(new DataColumn("TempPNo", typeof(decimal)));
            
            dt_PODT.Columns.Add(new DataColumn("id", typeof(string)));
            dt_PODT.Columns.Add(new DataColumn("CHNo", typeof(string)));
            dt_PODT.Columns.Add(new DataColumn("PONo", typeof(string)));
            dt_PODT.Columns.Add(new DataColumn("CHFlag", typeof(bool)));
            dt_PODT.Columns.Add(new DataColumn("CHCost", typeof(string)));
            dt_PODT.Columns.Add(new DataColumn("CHTotal", typeof(string)));
            dt_PODT.Columns.Add(new DataColumn("TempPNo", typeof(string)));


        }

        string Ac = "";
        private void Unit_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                //dgvData.ReadOnly = true;
                dgvData.AutoGenerateColumns = false;
                GETDTRow();
              
                ClearData();
                btnNew_Click(null, null);

                if (RetDT != null)
                {

                    if (RetDT.Count > 0)
                    {
                        btnNew_Click(null, null);
                        CreatePR_from_WaitingPR();
                    }
                }
                else
                {
                    if (!CHNo_temp.Equals(""))
                    {
                        Enable_Status(false, "View");
                        btnView_Click(null, null);
                        //txtTempNo.Text = TempNo_temp;
                        DataLoad();
                        
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
    
      
        private void DataLoad()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                dt_PODT.Rows.Clear();
                dt_POHD.Rows.Clear();
                int ck = 0;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var g = (from ix in db.tb_ChargePOHs select ix)
                        .Where(a => a.CHNo == txtCHNo.Text.Trim()
                         && (a.Status != "Cancel")
                         ).ToList();
                    if (g.Count() > 0)
                    {
                        txtCost.Text = StockControl.dbClss.TSt(g.FirstOrDefault().CHCost);
                        txtRemarkHD.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Remark);
                        // dt_POHD = StockControl.dbClss.LINQToDataTable(g);

                        ////Detail
                        var d = (from i in db.tb_ChargePODs
                                 join s in db.tb_PurchaseOrderDetails on i.RefidPO equals s.id
                                 where i.Status != "Cancel"
                                    && i.CHNo.Trim() == txtCHNo.Text.Trim()
                                 select new
                                 {
                                     CHNo = s.CHNo,
                                     RefPO = i.RefPO,
                                     RefidPO = i.RefidPO,
                                     CHUnitCost = i.CHUnitCost,
                                     CHCost = i.CHCost,
                                     CHTotal = i.CHTotal,
                                     CodeNo = s.CodeNo,
                                     ItemName = s.ItemName,
                                     ItemDesc = s.ItemDesc,
                                     GroupCode = s.GroupCode,
                                     id = i.id,
                                     TempPNo = s.TempPNo,
                                     Unit = s.Unit,
                                     OrderQty = s.OrderQty,
                                     //Cost = s.Cost,
                                     //Cost = (Math.Round((Convert.ToDecimal(s.Amount)), 2) - Math.Round((Convert.ToDecimal(s.DiscountAmount)), 2)) / Convert.ToDecimal(s.OrderQty)
                                     //Cost = 0,
                                     AmountTemp = s.Amount,
                                     DiscountAmount = s.DiscountAmount
                                    //Amount = 100,// Math.Round(( dbClss.TDe( s.Cost) * dbClss.TDe( s.OrderQty)),2),
                                    ,
                                     Status = i.Status

                                 }
                           ).ToList();
                        if(d.Count>0)
                        {
                            dgvData.DataSource = d;
                        }

                        //var d = (from ix in db.tb_PurchaseOrderDetails select ix)
                        //    .Where(a => a.CHNo == txtCHNo.Text.Trim() && a.SS == 1).ToList();
                        //if (d.Count() > 0)
                        //{
                        int c = 0;
                        //    dgvData.DataSource = d;
                        //    dt_PODT = StockControl.dbClss.LINQToDataTable(d);
                        dgvData.EndEdit();
                        foreach (var x in dgvData.Rows)
                        {
                            c += 1;
                            x.Cells["dgvNo"].Value = c;

                            x.Cells["Cost"].Value = (Math.Round((Convert.ToDecimal(x.Cells["AmountTemp"].Value)), 2) - Math.Round((Convert.ToDecimal(x.Cells["DiscountAmount"].Value)), 2)) / Convert.ToDecimal(x.Cells["OrderQty"].Value);

                            //x.Cells["Amount"].Value = Math.Round((dbClss.TDe(x.Cells["OrderQty"].Value) * dbClss.TDe(x.Cells["Cost"].Value)), 2);
                            decimal am = dbClss.TDe(x.Cells["OrderQty"].Value) * dbClss.TDe(x.Cells["Cost"].Value);
                           
                            x.Cells["Amount"].Value = am;

                            if(dbClss.TDe(x.Cells["CHCost"].Value)>0)
                                x.Cells["dgvFlag"].Value = true;
                        }
                        //}

                        //lblStatus.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Status);
                        if (StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Cancel"))
                        {
                            btnNew.Enabled = true;
                            btnSave.Enabled = false;
                            btnDelete.Enabled = false;
                            btnView.Enabled = false;
                            btnEdit.Enabled = false;
                            lblStatus.Text = "Cancel";
                            dgvData.ReadOnly = true;
                            btnAdd_Item.Enabled = false;
                            btnAdd_Part.Enabled = false;
                            btnAdd_Row.Enabled = false;
                            btnDel_Item.Enabled = false;
                            btnCal.Enabled = false;
                            btn_AddItem.Enabled = false;
                        }
                        else if
                            (StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Completed")
                            || StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Process")
                            )
                        {
                            btnSave.Enabled = false;
                            btnDelete.Enabled = true;
                            btnView.Enabled = false;
                            btnEdit.Enabled = false;
                            btnNew.Enabled = true;
                            lblStatus.Text = "Completed";
                            dgvData.ReadOnly = true;
                            btnAdd_Item.Enabled = false;
                            btnAdd_Part.Enabled = false;
                            btnAdd_Row.Enabled = false;
                            btnDel_Item.Enabled = false;
                            btnCal.Enabled = false;
                            btn_AddItem.Enabled = false;
                        }
                        else
                        {
                            btnNew.Enabled = true;
                            btnSave.Enabled = true;
                            btnDelete.Enabled = true;
                            btnView.Enabled = false;
                            btnEdit.Enabled = true;
                            lblStatus.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Status);
                            dgvData.ReadOnly = false;
                            btnAdd_Item.Enabled = false;
                            btnAdd_Part.Enabled = false;
                            btnAdd_Row.Enabled = false;
                            btnDel_Item.Enabled = false;
                            btnCal.Enabled = true;
                            btn_AddItem.Enabled = true;
                        }

                        foreach (var x in dgvData.Rows)
                        {
                            if (row >= 0 && row == ck && dgvData.Rows.Count > 0)
                            {
                                x.ViewInfo.CurrentRow = x;
                            }
                            ck += 1;
                        }

                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        private bool CheckDuplicate(string code, string Code2)
        {
            bool ck = false;

            //using (DataClasses1DataContext db = new DataClasses1DataContext())
            //{
            //    int i = (from ix in db.tb_Models
            //             where ix.ModelName == code

            //             select ix).Count();
            //    if (i > 0)
            //        ck = false;
            //    else
            //        ck = true;
            //}

            return ck;
        }
        private void Save()
        {
            dgvData.EndEdit();
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                //Herder---------------------------------

                var l = (from ix in db.tb_ChargePOHs
                         where ix.CHNo == txtCHNo.Text.Trim()
                         select ix).ToList();
                if (l.Count > 0)
                {
                    var u = (from ix in db.tb_ChargePOHs
                             where
                            ix.CHNo == txtCHNo.Text.Trim()
                            && ix.Status != "Cancel" && ix.Status != "Completed"
                             select ix).First();
                    u.CHBy = ClassLib.Classlib.User;
                    u.CHDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    u.CHCost = StockControl.dbClss.TDe(txtCost.Text);
                    u.Status = "Completed";
                    u.Remark = txtRemarkHD.Text;
                    db.SubmitChanges();
                    dbClss.AddHistory(this.Name, "แก้ไข ChargPO", "แก้ไข Addition charge : CHCost [" + u.CHCost.ToString() + "]", txtCHNo.Text);
                    
                }
                else  //New
                {
                    tb_ChargePOH gg = new tb_ChargePOH();
                    gg.CHNo = txtCHNo.Text;
                    gg.CHBy = ClassLib.Classlib.User;
                    gg.CHDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    gg.CHCost = StockControl.dbClss.TDe(txtCost.Text);
                    gg.Status = "Completed";
                    gg.Remark = txtRemarkHD.Text;
                    db.tb_ChargePOHs.InsertOnSubmit(gg);
                    db.SubmitChanges();

                    dbClss.AddHistory(this.Name, "เพิ่ม ChargPO", "สร้าง ChargPO [" + gg.CHCost.ToString() + "]", txtCHNo.Text);

                }

                List<string> Temp = new List<string>();

                //--------Detail-----------
                foreach (var g in dgvData.Rows)
                {
                    if (StockControl.dbClss.TInt(g.Cells["id"].Value) > 0)
                    {
                        var d = (from ix in db.tb_ChargePODs
                                 where ix.CHNo == txtCHNo.Text.Trim()
                                 && ix.id == dbClss.TInt(g.Cells["id"].Value)
                                 select ix).ToList();
                        if (d.Count > 0)
                        {
                            var dd = (from ix in db.tb_ChargePODs
                                     where ix.CHNo == txtCHNo.Text.Trim()
                                     && ix.id == dbClss.TInt(g.Cells["id"].Value)
                                     select ix).First();
                            dd.RefidPO = dbClss.TInt(g.Cells["RefidPO"].Value);
                            dd.RefPO = dbClss.TSt(g.Cells["RefPO"].Value);
                            dd.CHUnitCost = dbClss.TDe(g.Cells["CHUnitCost"].Value);
                            dd.CHCost = dbClss.TDe(g.Cells["CHCost"].Value);
                            dd.CHTotal = dbClss.TDe(g.Cells["CHTotal"].Value);
                            dd.Status = "Completed";
                            db.SubmitChanges();
                            dbClss.AddHistory(this.Name, "แก้ไข ChargPO", "แก้ไข ChargPO ID [" + " POID : " + dd.RefidPO.ToString() + " CHTotal : " + dd.CHTotal.ToString() + " CHCost : " + dd.CHCost.ToString() + " CHUnitCost : " + dd.CHUnitCost.ToString() + "]", txtCHNo.Text);

                        }
                    }
                    else  //NEw
                    {
                        tb_ChargePOD gg = new tb_ChargePOD();
                        gg.CHNo = txtCHNo.Text;
                        gg.RefidPO = dbClss.TInt(g.Cells["RefidPO"].Value);
                        gg.RefPO = dbClss.TSt(g.Cells["RefPO"].Value);
                        gg.CHUnitCost = dbClss.TDe(g.Cells["CHUnitCost"].Value);
                        gg.CHCost = dbClss.TDe(g.Cells["CHCost"].Value);
                        gg.CHTotal = dbClss.TDe(g.Cells["CHTotal"].Value);
                        gg.Status = "Completed";
                        
                        db.tb_ChargePODs.InsertOnSubmit(gg);
                        db.SubmitChanges();

                        dbClss.AddHistory(this.Name, "เพิ่ม ChargPO", "สร้าง ChargPO ID ["+" POID : " + gg.RefidPO.ToString() +" CHTotal : "+gg.CHTotal.ToString()+ " CHCost : "+ gg.CHCost.ToString() + " CHUnitCost : " + gg.CHUnitCost.ToString() + "]", txtCHNo.Text);

                        
                        Temp.Add(dbClss.TSt(g.Cells["RefPO"].Value));
                        
                    }
                }

                //Update PO ว่าใส่ราคาส่วนเพิ่มแล้ว 
                //
                foreach (var T in Temp.Distinct())
                {
                    var g1 = (from ix in db.tb_PurchaseOrders
                              where ix.PONo == dbClss.TSt(T) //ix.CHNo.Trim() == txtCHNo.Text.Trim() && ix.Status != "Cancel"       //  && ix.CHStatus != "Completed"
                               && (ix.Status != "Cancel")
                              select ix).ToList();
                    if (g1.Count > 0)
                    {
                        //-----------Herder------------------
                        var gg = (from ix in db.tb_PurchaseOrders
                                  where ix.PONo == dbClss.TSt(T) //ix.CHNo.Trim() == txtCHNo.Text.Trim() && ix.Status != "Cancel"
                                   && (ix.Status != "Cancel")
                                  select ix).First();

                        gg.CHBy = ClassLib.Classlib.User;
                        gg.CHDate = Convert.ToDateTime(DateTime.Now,new CultureInfo("en-US"));
                        gg.CHStatus = "Completed";
                        gg.CHNo = txtCHNo.Text;
                        gg.CHCost = dbClss.TDe(txtCost.Text);
                        gg.CHRemark = txtRemarkHD.Text.Trim();
                        
                        db.SubmitChanges();

                        //update PODetail
                        db.sp_042_Cal_POCost(gg.PONo);
                        
                    }
                }
            }
        }
        private void Cal_updateStack()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var g = (from ix in db.tb_ChargePODs
                             where ix.CHNo == txtCHNo.Text.Trim()
                             select ix).ToList();
                    if (g.Count > 0)
                    {
                        List<string> Temp = new List<string>();

                        foreach (var gg in g)
                        {
                            db.sp_043_Cal_Stock(dbClss.TInt(gg.RefidPO));

                            Temp.Add(dbClss.TSt(gg.RefPO));

                        }

                        if (Temp.Count>0)
                        {
                            foreach (var T in Temp.Distinct())
                            {
                                string CodeNo = "";
                                var g1 = (from ix in db.tb_PurchaseOrderDetails
                                          where ix.PONo == dbClss.TSt(T) //ix.CHNo.Trim() == txtCHNo.Text.Trim() && ix.Status != "Cancel"       //  && ix.CHStatus != "Completed"
                                           && (ix.SS == 1)
                                          select ix).ToList();
                                if (g1.Count > 0)
                                {
                                    foreach (var g2 in g1)
                                    {
                                        CodeNo = dbClss.TSt(g2.CodeNo);
                                        db.sp_044_Cal_BalanceStock(CodeNo);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        private void AddPR_d()
        {
          
          
            //int chk = 0;
            dgvData.EndEdit();

            List<string> Temp = new List<string>();

            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                foreach (var g in dgvData.Rows)
                {
                    if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) > 0)
                    {
                        //foreach (DataRow row in dt_PODT.Rows)
                        //{
                        //    if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) > dbClss.TInt(row["id"]))
                        //    {
                        //        chk = 1;

                        var l = (from ix in db.tb_PurchaseOrderDetails
                                 where
                                  ix.id == StockControl.dbClss.TInt(g.Cells["dgvid"].Value)
                                 select ix).ToList();
                        if (l.Count > 0)
                        {
                            Temp.Add(l.FirstOrDefault().TempPNo);

                            var u = (from ix in db.tb_PurchaseOrderDetails
                                     where
                                      ix.id == StockControl.dbClss.TInt(g.Cells["dgvid"].Value)
                                     select ix).First();

                            dbClss.AddHistory(this.Name, "แก้ไขรายการ Item PO", " แก้ไข Additional charge id :" + StockControl.dbClss.TSt(g.Cells["dgvid"].Value)
                            + " CodeNo :" + StockControl.dbClss.TSt(g.Cells["dgvCodeNo"].Value)
                            + " แก้ไขโดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy") + "]", u.PONo);

                            u.CHNo = txtCHNo.Text.Trim();
                            //u.CodeNo = StockControl.dbClss.TSt(g.Cells["dgvCodeNo"].Value);
                            dbClss.AddHistory(this.Name, "แก้ไข Item PO", "แก้ไข Additional charge : CHNo [" + u.CHNo + "]", u.PONo);

                            u.CHFlag = StockControl.dbClss.TBo(g.Cells["dgvFlag"].Value);
                            dbClss.AddHistory(this.Name, "แก้ไข Item PO", "แก้ไข Addition charge : CHFlag [" + u.CHFlag.ToString() + "]", u.PONo);


                            decimal CHCost = 0; decimal.TryParse(StockControl.dbClss.TSt(g.Cells["dgvCHCost"].Value), out CHCost);
                            u.CHCost = StockControl.dbClss.TDe(g.Cells["dgvCHCost"].Value);
                            dbClss.AddHistory(this.Name, "แก้ไข Item PO", "แก้ไข Addition charge : CHCost [" + u.CHCost.ToString() + "]", u.PONo);

                            decimal CHTotal = 0; decimal.TryParse(StockControl.dbClss.TSt(g.Cells["dgvCHTotal"].Value), out CHTotal);
                            u.CHTotal = StockControl.dbClss.TDe(g.Cells["dgvCHTotal"].Value);
                            dbClss.AddHistory(this.Name, "แก้ไข Item PO", "แก้ไข Addition charge : CHTotal [" + u.CHTotal.ToString() + "]", u.PONo);


                            db.SubmitChanges();
                        }
                    }
                }



                foreach (var T in Temp.Distinct())
                {
                    var g1 = (from ix in db.tb_PurchaseOrders
                              where ix.TempPNo == dbClss.TSt(T) //ix.CHNo.Trim() == txtCHNo.Text.Trim() && ix.Status != "Cancel"
                                                                //  && ix.CHStatus != "Completed"
                               && (ix.Status != "Cancel")
                              select ix).ToList();
                    if (g1.Count > 0)
                    {
                        //-----------Herder------------------
                        var gg = (from ix in db.tb_PurchaseOrders
                                  where ix.TempPNo == dbClss.TSt(T) //ix.CHNo.Trim() == txtCHNo.Text.Trim() && ix.Status != "Cancel"
                                  //&& ix.CHStatus != "Completed"
                                   && (ix.Status != "Cancel")
                                  select ix).First();

                        gg.CHBy = ClassLib.Classlib.User;
                        gg.CHDate = DateTime.Now;
                        gg.CHStatus = "Completed";
                        gg.CHNo = txtCHNo.Text;
                        gg.CHCost = dbClss.TDe(txtCost.Text);
                        gg.CHRemark = txtRemarkHD.Text.Trim();

                        db.SubmitChanges();
                    }
                }
            }
          
        }
       
        private void btnCancel_Click(object sender, EventArgs e)
        {
           
        }
        private void Enable_Status(bool ss, string Condition)
        {
            if (Condition.Equals("-") || Condition.Equals("New"))
            {
                txtCost.Enabled= ss;
                dgvData.ReadOnly = false;
                txtRemarkHD.Enabled = ss;
                //txtCurrency.Enabled = ss;
                btnAdd_Item.Enabled = ss;
                btnAdd_Part.Enabled = ss;
                btnAdd_Row.Enabled = ss;
                btnDel_Item.Enabled = ss;
                btnCal.Enabled = ss;
                btn_AddItem.Enabled = ss;
             
            }
            else if (Condition.Equals("View"))
            {
                txtCost.Enabled = ss;
                dgvData.ReadOnly = !ss;
                txtRemarkHD.Enabled = ss;
                btnAdd_Item.Enabled = ss;
                btnAdd_Part.Enabled = ss;
                btnAdd_Row.Enabled = ss;
                btnDel_Item.Enabled = ss;
                btnCal.Enabled = ss;
                btn_AddItem.Enabled = ss;

            }
            else if (Condition.Equals("Edit"))
            {
                txtCost.Enabled = ss;
                dgvData.ReadOnly = !ss;
                txtRemarkHD.Enabled = ss;
                btnAdd_Item.Enabled = ss;
                btnAdd_Part.Enabled = ss;
                btnAdd_Row.Enabled = ss;
                btnDel_Item.Enabled = ss;
                btnCal.Enabled = ss;
                btn_AddItem.Enabled = ss;

            }
        }
       
        private void ClearData()
        {
            txtCost.Text = "0.00";
            txtCHNo.Text = "";
            dgvData.Rows.Clear();
            dgvData.DataSource = null;
            txtRemarkHD.Text = "";
            dt_POHD.Rows.Clear();
            dt_PODT.Rows.Clear();
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            btnView.Enabled = true;
            btnEdit.Enabled = false;
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            btnDelete.Enabled = true;
         
            ClearData();
            Enable_Status(true, "New");

            lblStatus.Text = "New";
            Ac = "New";
            row = dgvData.Rows.Count - 1;
            if (row < 0)
                row = 0;
            //getมาไว้ก่อน แต่ยังไมได้ save 
            txtCHNo.Text = StockControl.dbClss.GetNo(14, 0);

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
            btnDelete.Enabled = true;
            
            Enable_Status(true, "Edit");
            lblStatus.Text = "Edit";
            Ac = "Edit";

        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "Delete";
                Ac = "Del";
                if (MessageBox.Show("ต้องการลบรายการ ( " + txtCHNo.Text + " ) หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Cursor = Cursors.WaitCursor;
                    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    {
                        var g = (from ix in db.tb_ChargePOHs
                                 where ix.CHNo.Trim() == txtCHNo.Text.Trim() && ix.Status != "Cancel"
                                 //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                                 select ix).ToList();
                        if (g.Count > 0)  //มีรายการในระบบ
                        {

                            List<string> Temp = new List<string>();

                            var gg = (from ix in db.tb_ChargePOHs
                                      where ix.CHNo.Trim() == txtCHNo.Text.Trim()
                                      //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                                      select ix).First();
                            
                            try
                            {
                                var s = (from ix in db.tb_ChargePODs
                                         where ix.CHNo.Trim() == txtCHNo.Text.Trim()

                                         select ix).ToList();
                                if (s.Count > 0)
                                {
                                    foreach (var ss in s)
                                    {
                                        ss.Status = "Cancel";
                                        db.SubmitChanges();

                                        Temp.Add(dbClss.TSt(ss.RefPO));                                        
                                    }
                                }
                            }
                            catch (Exception ex) { MessageBox.Show(ex.Message); }
                            //----------------------//


                            gg.Status = "Cancel";
                            //gg.UpdateBy = ClassLib.Classlib.User;
                            //gg.UpdateDate = DateTime.Now;

                            dbClss.AddHistory(this.Name, "ลบ ChargePO", "Delete ChargePO [" + txtCHNo.Text.Trim() + "]", txtCHNo.Text);

                            db.SubmitChanges();

                            Update_PO(Temp);
                            Cal_updateStack();


                            btnNew_Click(null, null);
                            Ac = "New";
                            btnSave.Enabled = true;
                        }
                        else // ไม่มีในระบบ
                        {
                            btnNew_Click(null, null);
                            Ac = "New";
                            btnSave.Enabled = true;
                        }
                    }

                    MessageBox.Show("ลบรายการ สำเร็จ!");
                    row = row - 1;
                    if (dgvData.Rows.Count <= 0)
                        row = -1;
                }


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }

        }
        private void Update_PO(List<string> Temp)
        {
            //Update PO ว่าใส่ราคาส่วนเพิ่มแล้ว 
            //
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                foreach (var T in Temp.Distinct())
                {
                    var g1 = (from ix in db.tb_PurchaseOrders
                              where ix.PONo == dbClss.TSt(T) //ix.CHNo.Trim() == txtCHNo.Text.Trim() && ix.Status != "Cancel"       //  && ix.CHStatus != "Completed"
                               && (ix.Status != "Cancel")
                              select ix).ToList();
                    if (g1.Count > 0)
                    {
                        //-----------Herder------------------
                        var gg = (from ix in db.tb_PurchaseOrders
                                  where ix.PONo == dbClss.TSt(T) //ix.CHNo.Trim() == txtCHNo.Text.Trim() && ix.Status != "Cancel"
                                   && (ix.Status != "Cancel")
                                  select ix).First();

                        //gg.CHBy = ClassLib.Classlib.User;
                        //gg.CHDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                        gg.CHStatus = "Waiting";
                        gg.CHNo = null;
                        gg.CHCost = null;
                        gg.CHRemark = txtRemarkHD.Text.Trim();

                        db.SubmitChanges();

                        //update PODetail
                        db.sp_042_Cal_POCost(gg.PONo);
                    }
                }
            }
        }
        private bool Check_Save()
        {
            bool re = true;
            //string err = "";
            //try
            //{
            //    //if (txtCodeNo.Text.Equals(""))
            //    //    err += " “รหัสพาร์ท:” เป็นค่าว่าง \n";
            //    //if (txtPRNo.Text.Equals(""))
            //    //    err += " “เลขที่ใบขอสั่งซื้อ:” เป็นค่าว่าง \n";
            //    if (cboVendorName.Text.Equals(""))
            //        err += "- “เลือกผู้ขาย:” เป็นค่าว่าง \n";
            //    if (txtVendorNo.Text.Equals(""))
            //        err += "- “รหัสผู้ขาย:” เป็นค่าว่าง \n";
            //    if (ddlCurrency.Text.Equals(""))
            //        err += "- “สกุลเงิน:” เป็นค่าว่าง \n";
            //    if (txtContactName.Text.Equals(""))
            //        err += "- “ผู้ติดต่อ:” เป็นค่าว่าง \n";
            //    if (txtAddress.Text.Equals(""))
            //        err += "- “ที่อยู่:” เป็นค่าว่าง \n";
            //    if (txtTel.Text.Equals(""))
            //        err += "- “เบอร์โทร:” เป็นค่าว่าง \n";
            //    if (dtDuedate.Text.Equals(""))
            //        err += "- “วันที่ต้องการ:” เป็นค่าว่าง \n";

            //    if(dgvData.Rows.Count<=0)
            //        err += "- “รายการ:” เป็นค่าว่าง \n";
            //    foreach (GridViewRowInfo rowInfo in dgvData.Rows)
            //    {
            //        if (rowInfo.IsVisible)
            //        {
            //           if(StockControl.dbClss.TSt(rowInfo.Cells["dgvCodeNo"].Value).Equals(""))
            //               err += "- “รหัสทูล:” เป็นค่าว่าง \n";
            //            if (StockControl.dbClss.TSt(rowInfo.Cells["dgvItemName"].Value).Equals(""))
            //                err += "- “ชื่อทูล:” เป็นค่าว่าง \n";
            //            if (StockControl.dbClss.TSt(rowInfo.Cells["dgvItemDesc"].Value).Equals(""))
            //                err += "- “รายละเอียดทูล:” เป็นค่าว่าง \n";
            //            if (StockControl.dbClss.TSt(rowInfo.Cells["dgvGroupCode"].Value).Equals(""))
            //                err += "- “กลุ่มสินค้า:” เป็นค่าว่าง \n";                       
            //            if (StockControl.dbClss.TDe(rowInfo.Cells["dgvOrderQty"].Value)<=0)
            //                err += "- “จำนวน:” น้อยกว่า 0 \n";
            //            if(StockControl.dbClss.TDe(rowInfo.Cells["dgvUnit"].Value).Equals(""))
            //                err += "- “หน่วย:” เป็นค่าว่าง \n";
            //            if (StockControl.dbClss.TDe(rowInfo.Cells["dgvPCSUnit"].Value) <=0)
            //                err += "- “จำนวน:หน่วย:” เป็นค่าว่าง \n";

            //        }
            //    }


            //     if (!err.Equals(""))
            //        MessageBox.Show(err);
            //    else
            //        re = false;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    dbClss.AddError("CreatePO", ex.Message, this.Name);
            //}

            return re;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Ac.Equals("New") || Ac.Equals("Edit"))
                {
                    //if (Check_Save())
                    //    return;
                    //else
                    {

                        if (MessageBox.Show("ต้องการบันทึก ?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            this.Cursor = Cursors.WaitCursor;

                            using (DataClasses1DataContext db = new DataClasses1DataContext())
                            {
                                if (Ac.Equals("New"))
                                {
                                    
                                    txtCHNo.Text = StockControl.dbClss.GetNo(14, 2);
                                }


                                var ggg = (from ix in db.tb_ChargePOHs
                                           where ix.CHNo.Trim() == txtCHNo.Text.Trim() //&& ix.Status != "Cancel"
                                           //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                                           select ix).ToList();
                                if (ggg.Count > 1)  //มีรายการในระบบ
                                {
                                    MessageBox.Show("เลขที่อ้างอิงถูกใช้แล้ว กรุณาสร้างเลขใหม่");
                                    return;
                                }
                            }

                            if (!txtCHNo.Text.Equals(""))
                            {

                                Save();
                                Cal_updateStack();
                                //AddPR_d();

                                Ac = "View";
                                btnEdit.Enabled = true;
                                btnView.Enabled = false;
                                btnNew.Enabled = true;
                                Enable_Status(false, "View");

                                DataLoad();

                                ////insert Stock temp
                                //Insert_Stock_temp();

                                MessageBox.Show("บันทึกสำเร็จ!");

                            }
                        }
                    }
                }
                else
                    MessageBox.Show("สถานะต้องเป็น New หรือ Edit เท่านั่น");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        private void Insert_Stock_temp()
        {
            try
            {

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                  
                    var g = (from ix in db.tb_PurchaseRequestLines
                                 //join i in db.tb_Items on ix.CodeNo equals i.CodeNo
                             where ix.TempNo.Trim() == txtCHNo.Text.Trim() && ix.SS == 1
                             select ix).ToList();
                    if (g.Count > 0)
                    {
                        //insert Stock
                        foreach (var vv in g)
                        {
                            db.sp_010_Update_StockItem(Convert.ToString(vv.CodeNo),"");
                            //dbClss.Insert_StockTemp(vv.CodeNo, Convert.ToDecimal(vv.OrderQty), "PR_Temp", "Inv");
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

                    if (dgvData.Columns["OrderQty"].Index == e.ColumnIndex
                        || dgvData.Columns["Cost"].Index == e.ColumnIndex
                        )
                    {
                        decimal OrderQty = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["OrderQty"].Value), out OrderQty);
                        decimal StandardCost = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["Cost"].Value), out StandardCost);
                        e.Row.Cells["Amount"].Value = OrderQty * StandardCost;
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Unit_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // MessageBox.Show(e.KeyCode.ToString());
        }

        private void radGridView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // MessageBox.Show(e.KeyCode.ToString());

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

        private void radLabel5_Click(object sender, EventArgs e)
        {

        }

        private void เพมพารทToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (!txtVendorNo.Text.Equals(""))
            //    {
            //        List<GridViewRowInfo> dgvRow_List = new List<GridViewRowInfo>();
            //        //dgvRow_List.Clear();
            //        CreatePR_List_2 MS = new CreatePR_List_2(dgvRow_List, txtVendorNo.Text);
            //        MS.ShowDialog();
            //        if (dgvRow_List.Count > 0)
            //        {
            //            string CodeNo = "";
            //            this.Cursor = Cursors.WaitCursor;
                       
            //            string ItemName = "";
            //            string ItemDescription = "";
            //            string GroupCode = "Other";
            //            decimal OrderQty = 0;
            //            decimal PCSUnit = 1;
            //            string Unit = "PCS";
            //            decimal Cost = 0;
            //            string Status = "Adding";
            //            string PRNO = "";
            //            int Refid = 0;
            //            int id = 0;
            //            int Row = dgvData.Rows.Count() + 1;
            //            foreach (GridViewRowInfo ee in dgvRow_List)
            //            {
            //                CodeNo = Convert.ToString(ee.Cells["CodeNo"].Value).Trim();
            //                ItemName = Convert.ToString(ee.Cells["ItemNo"].Value).Trim();
            //                ItemDescription = Convert.ToString(ee.Cells["ItemDesc"].Value).Trim();
            //                GroupCode = Convert.ToString(ee.Cells["GroupCode"].Value).Trim();
            //                OrderQty = StockControl.dbClss.TDe(ee.Cells["OrderQty"].Value);
            //                PCSUnit = StockControl.dbClss.TDe(ee.Cells["PCSUnit"].Value);
            //                Unit = StockControl.dbClss.TSt(ee.Cells["UnitCode"].Value);
            //                Cost = StockControl.dbClss.TDe(ee.Cells["Cost"].Value);
            //                PRNO = StockControl.dbClss.TSt(ee.Cells["TempNo"].Value);
            //                Refid = StockControl.dbClss.TInt(ee.Cells["id"].Value);

            //                Add_Item(Row, CodeNo, ItemName, ItemDescription, GroupCode, OrderQty, PCSUnit, Unit, Cost, PRNO, Status, Refid, id);

            //            }
            //        }
            //    }
            //    else
            //        MessageBox.Show("เลือกผู้ขายก่อน !!!");
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
            //finally { this.Cursor = Cursors.Default; }
        }
        private bool check_Duppicate(string PONo,string CodeNo,int id)
        {
            bool re = false;
            foreach (var rd1 in dgvData.Rows)
            {
                if (rd1.IsVisible.Equals(true))
                {
                    if (StockControl.dbClss.TSt(rd1.Cells["RefPO"].Value).Equals(PONo)
                        && StockControl.dbClss.TSt(rd1.Cells["CodeNo"].Value).Equals(CodeNo)
                        && StockControl.dbClss.TInt(rd1.Cells["RefidPO"].Value).Equals(id)
                        )
                        re = true;
                }
            }

            return re;

        }
        private void Add_Part(string CodeNo,int OrderQty)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                int Row = 0; Row = dgvData.Rows.Count()+1;
                var g = (from ix in db.tb_Items select ix).Where(a => a.CodeNo.Contains(CodeNo)).ToList();
                if (g.Count > 0)
                {
                    //dgvData.Rows.Add(Row.ToString(), CodeNo,
                    //    StockControl.dbClss.TSt(g.FirstOrDefault().ItemNo)
                    //    , StockControl.dbClss.TSt(g.FirstOrDefault().ItemDescription)
                    //    , StockControl.dbClss.TSt(g.FirstOrDefault().GroupCode)
                    //    , OrderQty
                    //    , StockControl.dbClss.TDe(g.FirstOrDefault().PCSUnit)
                    //    , StockControl.dbClss.TSt(g.FirstOrDefault().UnitBuy)
                    //    , StockControl.dbClss.TDe(g.FirstOrDefault().StandardCost)
                    //    , 1 * StockControl.dbClss.TDe(g.FirstOrDefault().StandardCost)
                    //    , ""
                    //    , "" //Lotno
                    //    , "" //SerialNo
                    //    , "" //MCName
                    //    , "" //LineName
                    //    , DateTime.Now
                    //    ,0.0 // RemainQty
                    //    ,0
                    //    );


                }
            }
        }
        private void ลบพารทToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
               
                if (dgvData.Rows.Count < 0)
                    return;


                if (Ac.Equals("New") || Ac.Equals("Edit"))
                {
                    this.Cursor = Cursors.WaitCursor;
                    
                    int id = 0;
                    int.TryParse(StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["id"].Value), out id);
                    if (id <= 0)
                    {
                        ////dgvData.Rows.Remove(dgvData.CurrentRow);
                        //string PONo = dbClss.TSt(dgvData.CurrentRow.Cells["RefPO"].Value);
                        //foreach (GridViewRowInfo rowInfo in dgvData.Rows)
                        //{
                        //   if(dbClss.TSt(rowInfo.Cells["RefPO"].Value) == PONo)
                        //    {
                        //        rowInfo.Delete();
                        //    }
                        //}


                        if (dbClss.ToDel())
                        {
                            string PONo = dbClss.TSt(dgvData.CurrentRow.Cells["RefPO"].Value);
                            List<GridViewRowInfo> eelist = new List<GridViewRowInfo>();
                            foreach (GridViewRowInfo rowInfo in dgvData.Rows)//datagridview save ที่ละแถว
                            {
                                if (dbClss.TSt(rowInfo.Cells["RefPO"].Value) == PONo)
                                    eelist.Add(rowInfo);
                            }

                            foreach (var ee in eelist)
                                dgvData.Rows.Remove(ee);
                        }
                    }
                    else
                    {
                        MessageBox.Show("ไม่สามารถทำรายการได้");
                    }

                    //else
                    //{
                    //    string CodeNo = "";
                    //    CodeNo = StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["CodeNo"]);
                    //    if (MessageBox.Show("ต้องการลบรายการ ( " + CodeNo + " ) ออกจากรายการ หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    //    {
                    //        dgvData.CurrentRow.IsVisible = false;
                    //    }
                    //}
                    btnCal_Click(null, null);
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
        private void cboVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void MasterTemplate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                ลบพารทToolStripMenuItem_Click(null, null);
        }

        private void btnListItem_Click(object sender, EventArgs e)
        {
            ////DataLoad();
            try
            {
                btnEdit.Enabled = true;
                btnView.Enabled = false;
                btnNew.Enabled = true;
                ClearData();
                Ac = "View";
                Enable_Status(false, "View");

                this.Cursor = Cursors.WaitCursor;
                ChargePO_List sc = new ChargePO_List(txtCHNo);
                this.Cursor = Cursors.Default;
                sc.ShowDialog();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();
                //LoadData
                DataLoad();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : btnListItem_Click", this.Name); }
            finally { this.Cursor = Cursors.Default; }

          

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            btnEdit.Enabled = true;
            btnView.Enabled = false;
            btnNew.Enabled = true;
            
            string TempNo = txtCHNo.Text;
            ClearData();
            Enable_Status(false, "View");
            txtCHNo.Text = TempNo;
            DataLoad();
            Ac = "View";
        }

       
        private void CreatePR_from_WaitingPR()
        {
            //try
            //{
            //    if (RetDT.Count > 0)
            //    {
            //        string CodeNo = "";
            //        this.Cursor = Cursors.WaitCursor;
            //        string VendorNo = "";
            //        foreach (GridViewRowInfo ee in RetDT)
            //        {
            //            VendorNo = Convert.ToString(ee.Cells["VendorNo"].Value).Trim();
            //            if(!VendorNo.Equals(""))
            //            {
            //                using (DataClasses1DataContext db = new DataClasses1DataContext())
            //                {
            //                    var I = (from ix in db.tb_Vendors select ix).Where(a => a.Active == true 
            //                    && a.VendorNo.Equals(VendorNo)).ToList();
            //                    if (I.Count > 0)
            //                    {
            //                        //StockControl.dbClss.TBo(a.Active).Equals(true)
            //                        ddlCurrency.Text = I.FirstOrDefault().CRRNCY;
            //                        txtAddress.Text = I.FirstOrDefault().Address;
            //                        txtVendorNo.Text = I.FirstOrDefault().VendorNo;
            //                        cboVendorName.Text = I.FirstOrDefault().VendorName;
            //                        var g = (from ix in db.tb_VendorContacts select ix).Where(a => a.VendorNo.Equals(txtVendorNo.Text)).OrderByDescending(b => b.DefaultNo).ToList();
            //                        if (g.Count > 0)
            //                        {
            //                            txtContactName.Text = g.FirstOrDefault().ContactName;
            //                            txtTel.Text = g.FirstOrDefault().Tel;
            //                            txtFax.Text = g.FirstOrDefault().Fax;
            //                            txtEmail.Text = g.FirstOrDefault().Email;
                                        
            //                        }
            //                    }
            //                }

            //            }

            //            CodeNo = Convert.ToString(ee.Cells["CodeNo"].Value).Trim();
            //            if (!CodeNo.Equals(""))
            //            {
            //                Add_Part(CodeNo,StockControl.dbClss.TInt(ee.Cells["Order"].Value));

            //            }
                        
            //        }
            //    }
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
            //finally { this.Cursor = Cursors.Default; }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    PrintPR a = new PrintPR(txtPONo.Text,txtPONo.Text,"PR");
            //    a.ShowDialog();

            //    //using (DataClasses1DataContext db = new DataClasses1DataContext())
            //    //{
            //    //    var g = (from ix in db.sp_R002_ReportPR(txtPRNo.Text,DateTime.Now) select ix).ToList();
            //    //    if (g.Count() > 0)
            //    //    {

            //    //        Report.Reportx1.Value = new string[2];
            //    //        Report.Reportx1.Value[0] = txtPRNo.Text;
            //    //        Report.Reportx1.WReport = "ReportPR";
            //    //        Report.Reportx1 op = new Report.Reportx1("ReportPR.rpt");
            //    //        op.Show();

            //    //    }
            //    //    else
            //    //        MessageBox.Show("not found.");
            //    //}

            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void cboVendorName_Leave(object sender, EventArgs e)
        {
            cboVendor_SelectedIndexChanged(null, null);
        }

        private void radButtonElement3_Click(object sender, EventArgs e)
        {
            string RefPO = "";
            string TempNo = txtCHNo.Text;
            if (!txtCHNo.Text.Equals(""))
            {
                string GetMarkup = Interaction.InputBox("ใส่เลขที่ P/O ใหม่!", "P/O New : ", "", 400, 250);
                if (!GetMarkup.Trim().Equals(""))
                {
                    RefPO = GetMarkup;
                    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    {
                        db.sp_UpdatePO(TempNo, RefPO);
                    }
                    MessageBox.Show("Update Completed.");
                    btnRefresh_Click(sender, e);
                }
            }
        }

        private void btnAdd_Row_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (!txtVendorNo.Text.Equals(""))
            //    {
            //        int Row = 0; Row = dgvData.Rows.Count() + 1;
            //        string CodeNo = "";
            //        string ItemName = "";
            //        string ItemDescription = "";
            //        string GroupCode = "Other";
            //        decimal OrderQty = 0;
            //        decimal PCSUnit = 1;
            //        string Unit = "PCS";
            //        decimal Cost = 0;
            //        string Status = "Adding";
            //        string PRNO = "";
            //        int Refid = 0;
            //        int id = 0;
            //        Add_Item(Row, CodeNo, ItemName, ItemDescription, GroupCode, OrderQty, PCSUnit, Unit, Cost,PRNO,Status,Refid,id);

            //    }
            //    else
            //        MessageBox.Show("เลือกผู้ขายก่อน !!!");
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
            //finally { this.Cursor = Cursors.Default; }
        }
        private void Add_Item(int Row, string CodeNo, string ItemNo, string ItemDescription, string GroupCode, decimal OrderQty, decimal PCSUnit
           , string UnitBuy, decimal StandardCost,string PRNo,string Status,int Refid,int id)
        {
            //dgvData.Rows.Add(Row.ToString(), CodeNo,
            //        ItemNo
            //        , ItemDescription
            //        , GroupCode
            //        , OrderQty
            //        , PCSUnit
            //        , UnitBuy
            //        , StandardCost
            //        , 1 * StandardCost
            //        , ""
            //        , "" //Lotno
            //        , "" //SerialNo
            //        , "" //MCName
            //        , "" //LineName
            //        , DateTime.Now
            //        , 0.0 // RemainQty
            //        , 0
            //        );

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
                ee.Cells["dgvCodeNo"].Value = CodeNo;
                ee.Cells["dgvItemName"].Value = ItemNo;
                ee.Cells["dgvItemDesc"].Value = ItemDescription;
                ee.Cells["dgvGroupCode"].Value = GroupCode;
                ee.Cells["dgvOrderQty"].Value = OrderQty;
                ee.Cells["dgvPCSUnit"].Value = PCSUnit;
                ee.Cells["dgvUnit"].Value = UnitBuy;
                ee.Cells["dgvCost"].Value = StandardCost;
                ee.Cells["dgvAmount"].Value = 1 * StandardCost;
                ee.Cells["dgvPRNo"].Value = PRNo;
                ee.Cells["dgvPRItem"].Value = Refid;
                ee.Cells["dgvStatus"].Value = Status;
                ee.Cells["dgvid"].Value = 0;
                
                //if (!statuss.Equals("Completed") || !statuss.Equals("Process")) //|| (!dbclass.TBo(ApproveFlag) && dbclass.TSt(status) != "Reject"))
                //    dgvData.ReadOnly = false;
                //if (statuss == "Del")
                //    ee.IsVisible = false;


                if (GroupCode != "Other" || PRNo != "")
                {
                    ee.Cells["dgvCodeNo"].ReadOnly = true;
                    ee.Cells["dgvItemName"].ReadOnly = true;
                    ee.Cells["dgvItemDesc"].ReadOnly = true;


                    ee.Cells["dgvPCSUnit"].ReadOnly = true;
                    ee.Cells["dgvUnit"].ReadOnly = true;
                    ee.Cells["dgvCost"].ReadOnly = true;
                }
                else
                {
                    ee.Cells["dgvCodeNo"].ReadOnly = false;
                    ee.Cells["dgvItemName"].ReadOnly = false;
                    ee.Cells["dgvItemDesc"].ReadOnly = false;

                    ee.Cells["dgvPCSUnit"].ReadOnly = false;
                    ee.Cells["dgvUnit"].ReadOnly = false;
                    ee.Cells["dgvCost"].ReadOnly = false;
                }

                //if (lblStatus.Text.Equals("Completed"))//|| lbStatus.Text.Equals("Reject"))
                //    dgvData.AllowAddNewRow = false;
                //else
                //    dgvData.AllowAddNewRow = true;

                //dbclass.SetRowNo1(dgvData);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : Add_Item", this.Name); }

        }
        private void ddlCurrency_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            
        }

        private void btnAdd_Part_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (!txtVendorNo.Text.Equals(""))
            //    {
            //        List<GridViewRowInfo> dgvRow_List = new List<GridViewRowInfo>();
            //        //dgvRow_List.Clear();
            //        ListPart_CreatePR MS = new ListPart_CreatePR(dgvRow_List, txtVendorNo.Text);
            //        MS.ShowDialog();
            //        if (dgvRow_List.Count > 0)
            //        {
            //            string CodeNo = "";
            //            this.Cursor = Cursors.WaitCursor;
            //            decimal OrderQty = 1;
            //            foreach (GridViewRowInfo ee in dgvRow_List)
            //            {
            //                CodeNo = Convert.ToString(ee.Cells["CodeNo"].Value).Trim();
            //                if (!CodeNo.Equals("") && !check_Duppicate(CodeNo))
            //                {
            //                    Add_Part(CodeNo, OrderQty);
            //                }
            //                else
            //                {
            //                    MessageBox.Show("รหัสพาร์ท ซ้ำ");
            //                }
            //            }
            //        }
            //    }
            //    else
            //        MessageBox.Show("เลือกผู้ขายก่อน !!!");
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
            //finally { this.Cursor = Cursors.Default; }
        }
        private void Add_Part(string CodeNo, decimal OrderQty)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                int Row = 0; Row = dgvData.Rows.Count() + 1;
                var g = (from ix in db.tb_Items select ix).Where(a => a.CodeNo.Contains(CodeNo)).ToList();
                if (g.Count > 0)
                {
                    //dgvData.Rows.Add(Row.ToString(), CodeNo,
                    //    StockControl.dbClss.TSt(g.FirstOrDefault().ItemNo)
                    //    , StockControl.dbClss.TSt(g.FirstOrDefault().ItemDescription)
                    //    , StockControl.dbClss.TSt(g.FirstOrDefault().GroupCode)
                    //    , OrderQty
                    //    , StockControl.dbClss.TDe(g.FirstOrDefault().PCSUnit)
                    //    , StockControl.dbClss.TSt(g.FirstOrDefault().UnitBuy)
                    //    , StockControl.dbClss.TDe(g.FirstOrDefault().StandardCost)
                    //    , 1 * StockControl.dbClss.TDe(g.FirstOrDefault().StandardCost)
                    //    , ""
                    //    , "" //Lotno
                    //    , "" //SerialNo
                    //    , "" //MCName
                    //    , "" //LineName
                    //    , DateTime.Now
                    //    ,0.0 // RemainQty
                    //    ,0
                    //    );   

                    //string CodeNo = "";
                    string ItemNo = StockControl.dbClss.TSt(g.FirstOrDefault().ItemNo);
                    string ItemDescription = StockControl.dbClss.TSt(g.FirstOrDefault().ItemDescription);
                    string GroupCode = StockControl.dbClss.TSt(g.FirstOrDefault().GroupCode);
                    //int OrderQty = 0;
                    decimal PCSUnit = StockControl.dbClss.TDe(g.FirstOrDefault().PCSUnit);
                    string UnitBuy = StockControl.dbClss.TSt(g.FirstOrDefault().UnitBuy);
                    decimal StandardCost = StockControl.dbClss.TDe(g.FirstOrDefault().StandardCost);
                    
                    string Status = "Adding";
                    string PRNO = "";
                    int Refid = 0;
                    int id = 0;
                    

                    Add_Item(Row, CodeNo, ItemNo, ItemDescription, GroupCode, OrderQty
                        , PCSUnit, UnitBuy, StandardCost, PRNO, Status, Refid, id);

                }
            }
        }

        private void txtCost_KeyPress(object sender, KeyPressEventArgs e)
        {
            StockControl.dbClss.CheckDigitDecimal(e);
        }

        private void txtCost_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
           
            //decimal temp = 0;
            //decimal.TryParse(txtCost.Text, out temp);
            //temp = decimal.Round(temp, 2);
            //txtCost.Text = (temp).ToString("N2");
        }

        private void btn_AddItem_Click(object sender, EventArgs e)
        {
            try
            {
               
                    List<GridViewRowInfo> dgvRow_List = new List<GridViewRowInfo>();
                //dgvRow_List.Clear();
                ChargePO_List_2 MS = new ChargePO_List_2(dgvRow_List);
                    MS.ShowDialog();
                if (dgvRow_List.Count > 0)
                {
                    string CodeNo = "";
                    this.Cursor = Cursors.WaitCursor;
                    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    {
                        string ItemName = "";
                        string ItemDescription = "";
                        string GroupCode = "Other";
                        decimal OrderQty = 0;
                        decimal PCSUnit = 1;
                        string Unit = "PCS";
                        decimal Cost = 0;
                        string Status = "Adding";
                        string PONo = "";
                        bool Flag = false;
                        int id = 0;
                        int Row = dgvData.Rows.Count() + 1;
                        decimal CHCost = 0;
                        decimal CHTotal = 0;
                        string TempPNo = "";
                        //DateTime? DeliveryDate = null;
                        foreach (GridViewRowInfo ee in dgvRow_List)
                        {
                            PONo = StockControl.dbClss.TSt(ee.Cells["PONo"].Value);
                            TempPNo = StockControl.dbClss.TSt(ee.Cells["TempPNo"].Value);


                            var d = (from ix in db.tb_PurchaseOrderDetails select ix)
                              .Where(a => a.TempPNo == TempPNo.Trim() && a.SS == 1).ToList();
                            if (d.Count() > 0)
                            {
                                foreach (var x in d)
                                {

                                    Row = dgvData.Rows.Count() + 1;
                                    CodeNo = Convert.ToString(x.CodeNo).Trim();
                                    ItemName = Convert.ToString(x.ItemName).Trim();
                                    ItemDescription = Convert.ToString(x.ItemDesc).Trim();
                                    GroupCode = Convert.ToString(x.GroupCode).Trim();
                                    OrderQty = StockControl.dbClss.TDe(x.OrderQty);
                                    PCSUnit = StockControl.dbClss.TDe(x.PCSUnit);
                                    Unit = StockControl.dbClss.TSt(x.Unit);
                                    //Cost = StockControl.dbClss.TDe(x.Cost);
                                    Cost =( Math.Round((StockControl.dbClss.TDe(x.Amount)),2)- Math.Round((StockControl.dbClss.TDe(x.DiscountAmount)), 2))/dbClss.TDe(x.OrderQty);


                                    Flag = StockControl.dbClss.TBo(x.CHFlag);
                                    CHCost = StockControl.dbClss.TDe(x.CHCost);
                                    CHTotal = StockControl.dbClss.TDe(x.CHTotal);

                                    id = StockControl.dbClss.TInt(x.id);
                                    //DeliveryDate = Convert.ToDateTime(ee.Cells["dgvDeliveryDate"].Value);
                                    if (!PONo.Equals("") && !check_Duppicate(PONo, CodeNo,id))
                                    {
                                        Add_Item(Row, CodeNo, ItemName, ItemDescription, GroupCode, OrderQty
                                        , PCSUnit, Unit, Cost, PONo, Status, id, Flag, CHCost, CHTotal, TempPNo);
                                    }
                                }
                            }
                        }
                    }
                }
                    //getTotal();           
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        private void Add_Item(int Row, string CodeNo, string ItemNo, string ItemDescription, string GroupCode, decimal OrderQty
            , decimal PCSUnit, string UnitBuy, decimal StandardCost, string PONo, string Status, int id,bool Flag
            ,decimal CHCost,decimal CHTotal,string TempPNo)
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
                ee.Cells["dgvFlag"].Value = Flag;

                ee.Cells["RefPO"].Value = PONo;
                ee.Cells["CodeNo"].Value = CodeNo;
                
                ee.Cells["ItemName"].Value = ItemNo;
                ee.Cells["ItemDesc"].Value = ItemDescription;
                ee.Cells["GroupCode"].Value = GroupCode;
                ee.Cells["OrderQty"].Value = OrderQty;
                ee.Cells["PCSUnit"].Value = PCSUnit;
                ee.Cells["Unit"].Value = UnitBuy;
                ee.Cells["Cost"].Value = StandardCost;
                ee.Cells["Amount"].Value =Math.Round((OrderQty * StandardCost),2);
               
                //ee.Cells["dgvStatus"].Value = Status;
                ee.Cells["RefidPO"].Value = id;
                ee.Cells["CHCost"].Value = CHCost;
                ee.Cells["CHTotal"].Value = CHTotal;
                ee.Cells["CHUnitCost"].Value = StandardCost;
                ee.Cells["TempPNo"].Value = TempPNo;
                ee.Cells["id"].Value = 0;


                //if (dbClss.TSt(DeliveryDate) != "")
                //    ee.Cells["dgvDeliveryDate"].Value = DeliveryDate;



                //if (GroupCode != "Other" || PRNo != "")
                //{
                //    ee.Cells["dgvCodeNo"].ReadOnly = true;
                //    ee.Cells["dgvItemName"].ReadOnly = true;
                //    ee.Cells["dgvItemDesc"].ReadOnly = true;

                //    ee.Cells["dgvPCSUnit"].ReadOnly = true;
                //    //ee.Cells["dgvUnit"].ReadOnly = true;
                //    //ee.Cells["dgvCost"].ReadOnly = true;
                //}
                //else
                //{
                //    ee.Cells["dgvCodeNo"].ReadOnly = false;
                //    ee.Cells["dgvItemName"].ReadOnly = false;
                //    ee.Cells["dgvItemDesc"].ReadOnly = false;

                //    ee.Cells["dgvPCSUnit"].ReadOnly = false;
                //    //ee.Cells["dgvUnit"].ReadOnly = false;
                //    //ee.Cells["dgvCost"].ReadOnly = false;
                //}

                //if (Refid > 0)
                //{
                //    ee.Cells["dgvCodeNo"].ReadOnly = true;
                //    ee.Cells["dgvItemName"].ReadOnly = true;
                //    ee.Cells["dgvItemDesc"].ReadOnly = true;
                //    ee.Cells["dgvPCSUnit"].ReadOnly = true;
                //    //ee.Cells["dgvUnit"].ReadOnly = true;
                //    //ee.Cells["dgvCost"].ReadOnly = true;
                //    ee.Cells["dgvOrderQty"].ReadOnly = true;
                //}

                //dbclass.SetRowNo1(dgvData);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : Add_Item", this.Name); }

        }


        private void MasterTemplate_Click(object sender, EventArgs e)
        {

        }

        private void btnCal_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // foreach(var g in dgvData.Rows)
                dgvData.EndEdit();
                decimal CostImport = 0;
                decimal.TryParse(txtCost.Text, out CostImport);
                decimal SumCHCost = 0;
                decimal TempCost = 0;
                int count = dgvData.Rows.Where(a=> Convert.ToBoolean(a.Cells["dgvFlag"].Value)).Count();
                int c = 0;

                decimal Amount = 0;
                foreach (GridViewRowInfo rowInfo in dgvData.Rows)
                {
                    if (Convert.ToBoolean(rowInfo.Cells["dgvFlag"].Value))
                    {
                        Amount += dbClss.TDe(rowInfo.Cells["Amount"].Value);
                    }

                    rowInfo.Cells["CHCost"].Value = 0;
                    rowInfo.Cells["CHTotal"].Value = 0;
                }

                foreach (GridViewRowInfo rowInfo in dgvData.Rows)//.Where(a => Convert.ToBoolean(a.Cells["dgvFlag"].Value) == true))
                {
                    if (Convert.ToBoolean(rowInfo.Cells["dgvFlag"].Value))
                    {
                        c += 1;

                        if (c == count)
                        {
                            rowInfo.Cells["CHCost"].Value = Math.Round((CostImport - SumCHCost), 2);
                            rowInfo.Cells["CHTotal"].Value = Math.Round((CostImport - SumCHCost), 2) + dbClss.TDe(rowInfo.Cells["Amount"].Value);

                            if (dbClss.TDe(rowInfo.Cells["OrderQty"].Value) > 0)
                                rowInfo.Cells["CHUnitCost"].Value = Math.Round((dbClss.TDe(rowInfo.Cells["CHTotal"].Value) / dbClss.TDe(rowInfo.Cells["OrderQty"].Value)), 2);
                            else
                                rowInfo.Cells["CHUnitCost"].Value = dbClss.TDe(rowInfo.Cells["Cost"].Value);
                        }
                        else
                        {
                            if (Amount > 0)
                                TempCost = (dbClss.TDe(rowInfo.Cells["Amount"].Value) * CostImport) / Amount;

                            TempCost = Math.Round(TempCost, 2);

                            rowInfo.Cells["CHCost"].Value = TempCost;
                            rowInfo.Cells["CHTotal"].Value = Math.Round((TempCost + dbClss.TDe(rowInfo.Cells["Amount"].Value)), 2);

                            if (dbClss.TDe(rowInfo.Cells["OrderQty"].Value) > 0)
                                rowInfo.Cells["CHUnitCost"].Value = Math.Round((dbClss.TDe(rowInfo.Cells["CHTotal"].Value) / dbClss.TDe(rowInfo.Cells["OrderQty"].Value)), 2);
                            else
                                rowInfo.Cells["CHUnitCost"].Value = dbClss.TDe(rowInfo.Cells["Cost"].Value);
                            
                        }
                        SumCHCost += TempCost;
                    }
                    else
                    {
                        rowInfo.Cells["CHTotal"].Value = dbClss.TDe(rowInfo.Cells["CHCost"].Value)+ dbClss.TDe(rowInfo.Cells["Amount"].Value);
                        rowInfo.Cells["CHUnitCost"].Value = dbClss.TDe(rowInfo.Cells["Cost"].Value);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : btnCal_Click", this.Name); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void txtCost_Leave(object sender, EventArgs e)
        {
            decimal temp = 0;
            decimal.TryParse(txtCost.Text, out temp);
            temp = decimal.Round(temp, 2);
            txtCost.Text = (temp).ToString("N2");
        }
    }
}
