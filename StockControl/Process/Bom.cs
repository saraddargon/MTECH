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
using Telerik.WinControls;

namespace StockControl
{
    public partial class Bom : Telerik.WinControls.UI.RadRibbonForm
    {
        public Bom()
        {
            InitializeComponent();
        }
        public Bom(string PartNox,string BomNox)
        {
            InitializeComponent();
            PartNo_temp = PartNox;
            BomNo_temp = BomNox;

        }
        public Bom(List<GridViewRowInfo> RetDT)
        {
            InitializeComponent();
            this.RetDT = RetDT;
        }
        //private int RowView = 50;
        //private int ColView = 10;
        //DataTable dt = new DataTable();
        List<GridViewRowInfo> RetDT;
        string PartNo_temp = "";
        string BomNo_temp = "";

        DataTable dt_HD = new DataTable();
        DataTable dt_DT = new DataTable();
        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            HistoryView hw = new HistoryView(this.Name,txtPartNo.Text);
            this.Cursor = Cursors.Default;
            hw.ShowDialog();
        }

        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }
        private void GETDTRow()
        {
            dt_HD.Columns.Add(new DataColumn("id", typeof(string)));
            dt_HD.Columns.Add(new DataColumn("PartNo", typeof(string)));
            dt_HD.Columns.Add(new DataColumn("BomNo", typeof(string)));
            dt_HD.Columns.Add(new DataColumn("Year_", typeof(string)));
            dt_HD.Columns.Add(new DataColumn("Month_", typeof(string)));
            dt_HD.Columns.Add(new DataColumn("Description", typeof(string)));
            dt_HD.Columns.Add(new DataColumn("Status", typeof(string)));
            dt_HD.Columns.Add(new DataColumn("Remark", typeof(string)));
            dt_HD.Columns.Add(new DataColumn("StartDate", typeof(DateTime)));
            dt_HD.Columns.Add(new DataColumn("EndDate", typeof(DateTime)));
            dt_HD.Columns.Add(new DataColumn("CreateBy", typeof(string)));
            dt_HD.Columns.Add(new DataColumn("CreateDate", typeof(DateTime)));
            dt_HD.Columns.Add(new DataColumn("ModifyBy", typeof(string)));
            dt_HD.Columns.Add(new DataColumn("ModifyDate", typeof(DateTime)));
            dt_HD.Columns.Add(new DataColumn("ShelfNo", typeof(DateTime)));


            dt_DT.Columns.Add(new DataColumn("id", typeof(string)));
            dt_DT.Columns.Add(new DataColumn("PartNo", typeof(string)));
            dt_DT.Columns.Add(new DataColumn("BomNo", typeof(string)));
            dt_DT.Columns.Add(new DataColumn("Year_", typeof(string)));
            dt_DT.Columns.Add(new DataColumn("Month_", typeof(string)));
            dt_DT.Columns.Add(new DataColumn("Line", typeof(int)));
            dt_DT.Columns.Add(new DataColumn("Component", typeof(string)));
            dt_DT.Columns.Add(new DataColumn("Qty", typeof(decimal)));
            dt_DT.Columns.Add(new DataColumn("Unit", typeof(string)));
            dt_DT.Columns.Add(new DataColumn("PCSUnit", typeof(decimal)));
            dt_DT.Columns.Add(new DataColumn("UnitCost", typeof(decimal)));
            dt_DT.Columns.Add(new DataColumn("Cost", typeof(decimal)));
           

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
                DefaultItem();
               
                ClearData();
                btnNew_Click(null, null);

                //if (RetDT != null)
                //{

                //    if (RetDT.Count > 0)
                //    {
                //        btnNew_Click(null, null);
                //        CreatePR_from_WaitingPR();
                //    }
                //}
                //else
                //{
                if (!PartNo_temp.Equals("") && !BomNo_temp.Equals(""))
                {
                    Enable_Status(false, "View");
                    btnView_Click(null, null);
                    txtPartNo.Text = PartNo_temp;
                    txtBomNo.Text = BomNo_temp;
                    DataLoad();

                }
                //}
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        private void DefaultItem()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                GridViewMultiComboBoxColumn Uom = (GridViewMultiComboBoxColumn)dgvData.Columns["dgvUnit"];
                Uom.DataSource = (from ix in db.mh_Units.Where(s => s.UnitActive == true)
                                  select new { ix.UnitCode }).ToList();
                Uom.DisplayMember = "UnitCode";
                Uom.DropDownStyle = RadDropDownStyle.DropDown;

                //    cboVendorName.AutoCompleteMode = AutoCompleteMode.Append;
                //    cboVendorName.DisplayMember = "VendorName";
                //    cboVendorName.ValueMember = "VendorNo";
                //    cboVendorName.DataSource = (from ix in db.tb_Vendors.Where(s => s.Active == true)
                //                            select new { ix.VendorNo,ix.VendorName,ix.CRRNCY }).ToList();
                //    cboVendorName.SelectedIndex = 0;


                //    try
                //    {



                //        //GridViewMultiComboBoxColumn col = (GridViewMultiComboBoxColumn)radGridView1.Columns["CodeNo"];
                //        //col.DataSource = (from ix in db.tb_Items.Where(s => s.Status.Equals("Active")) select new { ix.CodeNo, ix.ItemDescription }).ToList();
                //        //col.DisplayMember = "CodeNo";
                //        //col.ValueMember = "CodeNo";

                //        //col.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDown;
                //        //col.FilteringMode = GridViewFilteringMode.DisplayMember;

                //        // col.AutoSizeMode = BestFitColumnMode.DisplayedDataCells;
                //    }
                //    catch { }

                //    //col.TextAlignment = ContentAlignment.MiddleCenter;
                //    //col.Name = "CodeNo";
                //    //this.radGridView1.Columns.Add(col);

                //    //this.radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

                //    //this.radGridView1.CellEditorInitialized += radGridView1_CellEditorInitialized;
            }
        }
        private void DataLoad()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                dt_DT.Rows.Clear();
                dt_HD.Rows.Clear();
                int ck = 0;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var g = (from a in db.tb_BomHDs
                             join b in db.mh_Items on a.PartNo equals b.InternalNo //&& b.Active == true
                             where (a.Status != "Cancel")
                             //&& (b.Active == true)
                             && (a.BomNo == (txtBomNo.Text.Trim()))
                             && (a.PartNo == txtPartNo.Text.Trim())

                             select new
                             {
                                 BomNo = a.BomNo,
                                 PartNo = a.PartNo,
                                 PartName = a.PartName,
                                 PartDesc = a.PartDescription,
                                 TypePart = b.InventoryGroup,
                                 Remark = a.Remark,
                                 Description = a.Description,
                                 Year_ = a.Year_,
                                 Month_ = a.Month_,
                                 CreateBy = a.CreateBy,
                                 CreateDate = a.CreateDate,
                                 ModifyBy = a.ModifyBy,
                                 ModifyDate = a.ModifyDate,
                                 StartDate = a.StartDate,
                                 EndDate = a.EndDate,
                                 Status = a.Status,
                                 ReplenishmentType = b.ReplenishmentType
                                 , GroupType = b.GroupType
                                 , Version = a.Version

                             }//.Where(ab => ab.VendorNo.Contains(Vendorno))
                              ).ToList();


                    //var g = (from ix in db.tb_BomHDs
                    //         select ix)
                    //    .Where(a => a.PartNo == txtPartNo.Text.Trim()
                    //     && (a.BomNo == txtBomNo.Text.Trim())
                    //     && (a.Status != "Cancel")
                    //     ).ToList();
                    if (g.Count() > 0)
                    {

                        DateTime? temp_date = null;
                        txtPartNo.Text = StockControl.dbClss.TSt(g.FirstOrDefault().PartNo);

                        //var i = (from ix in db.tb_Items select ix).Where(a => a.CodeNo == txtPartNo.Text).ToList();
                        //if (i.Count() > 0)
                        //{
                        //    txtPartName.Text = StockControl.dbClss.TSt(i.FirstOrDefault().ItemNo);
                        //    txtTypePart.Text = StockControl.dbClss.TSt(i.FirstOrDefault().TypePart);
                        //}
                        txtDescription.Text = dbClss.TSt(g.FirstOrDefault().PartDesc);
                        txtPartName.Text = StockControl.dbClss.TSt(g.FirstOrDefault().PartName);
                        txtTypePart.Text = StockControl.dbClss.TSt(g.FirstOrDefault().TypePart);

                        txtRemarkHD.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Remark);
                        //txtDescription.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Description);
                        txtBomNo.Text = StockControl.dbClss.TSt(g.FirstOrDefault().BomNo).ToUpper();
                        txtYear_.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Year_);
                        txtMonth_.Text = StockControl.dbClss.TSt(g.FirstOrDefault().Month_);
                        txtVersion.Text = dbClss.TSt(g.FirstOrDefault().Version);

                        txtCreateby.Text = StockControl.dbClss.TSt(g.FirstOrDefault().CreateBy);
                        DateTime temp = Convert.ToDateTime(g.FirstOrDefault().CreateDate,new CultureInfo("en-US"));
                        txtCreateDate.Text = temp.ToString("dd/MMM/yyyy");
                       
                        if (!StockControl.dbClss.TSt(g.FirstOrDefault().ModifyBy).Equals(""))
                        {
                            DateTime temp2 = Convert.ToDateTime(g.FirstOrDefault().ModifyDate,new CultureInfo("en-US"));
                            txtCreateDate.Text = temp2.ToString("dd/MMM/yyyy");
                            txtCreateby.Text = StockControl.dbClss.TSt(g.FirstOrDefault().ModifyBy);
                        }
                        //else
                        //    txtCreateDate.Text = "";

                        if (!StockControl.dbClss.TSt(g.FirstOrDefault().StartDate).Equals(""))
                            dtBegin.Value = Convert.ToDateTime(g.FirstOrDefault().StartDate,new CultureInfo("en-US"));
                        else
                            dtBegin.Value = Convert.ToDateTime(temp_date);

                        if (!StockControl.dbClss.TSt(g.FirstOrDefault().EndDate).Equals(""))
                            dtEnd.Value = Convert.ToDateTime(g.FirstOrDefault().EndDate,new CultureInfo("en-US"));
                        else
                            dtEnd.Value = Convert.ToDateTime(temp_date);


                        dt_HD = StockControl.dbClss.LINQToDataTable(g);

                        // จบ Herder  -----------------------



                        //Detail

                        //var d = (from a in db.tb_BomDTs
                        //         join b in db.tb_Items on a.Component equals b.CodeNo
                        //         where 
                        //         (b.Status == "Active")
                        //         && (a.BomNo == (txtBomNo.Text.Trim()))
                        //         && (a.PartNo == txtPartNo.Text.Trim())

                        //         select new
                        //         {
                        //             id = a.id,
                        //             BomNo = a.BomNo,
                        //             PartNo = StockControl.dbClss.TSt(a.PartNo),
                        //             //PartName = b.ItemNo,
                        //             //ItemNo = b.ItemNo,
                        //             ComponentName = StockControl.dbClss.TSt(b.ItemNo),
                        //             //TypePart = b.TypePart,
                        //             Type = StockControl.dbClss.TSt(b.TypePart),
                        //             Line = StockControl.dbClss.TInt(a.Line),
                        //             Component = StockControl.dbClss.TSt(a.Component),
                        //             Year_ = a.Year_,
                        //             Month_ = a.Month_,
                        //             Qty = StockControl.dbClss.TDe(a.Qty),
                        //             Unit = StockControl.dbClss.TSt(a.Unit),
                        //             UnitCost = StockControl.dbClss.TDe(a.UnitCost),
                        //             Cost = StockControl.dbClss.TDe(a.Cost),                                    

                        //         }//.Where(ab => ab.VendorNo.Contains(Vendorno))
                        //     ).ToList();


                        var d = (from ix in db.tb_BomDTs select ix)
                            .Where(a => a.PartNo == txtPartNo.Text.Trim()
                            && a.BomNo == txtBomNo.Text.Trim()
                            ).ToList();
                        if (d.Count() > 0)
                        {
                            int c = 0;
                            dgvData.DataSource = d;
                            dt_DT = StockControl.dbClss.LINQToDataTable(d);
                            foreach (var x in dgvData.Rows)
                            {
                                c += 1;
                                x.Cells["dgvNo"].Value = c;

                                var i2 = (from ix in db.mh_Items select ix).Where(a => a.InternalNo == StockControl.dbClss.TSt(x.Cells["dgvComponent"].Value)).ToList();
                                if (i2.Count() > 0)
                                {
                                    x.Cells["dgvGroupType"].Value = StockControl.dbClss.TSt(i2.FirstOrDefault().InventoryGroup);
                                    x.Cells["dgvType"].Value = StockControl.dbClss.TSt(i2.FirstOrDefault().InventoryGroup);
                                    x.Cells["dgvReplenishmentType"].Value = StockControl.dbClss.TSt(i2.FirstOrDefault().ReplenishmentType);

                                }
                            }
                        }

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
                            btnDel_Item.Enabled = false;
                        }
                        else if
                            (StockControl.dbClss.TSt(g.FirstOrDefault().Status).Equals("Completed"))
                        {
                            btnSave.Enabled = false;
                            btnDelete.Enabled = false;
                            btnView.Enabled = false;
                            btnEdit.Enabled = false;
                            btnNew.Enabled = true;
                            lblStatus.Text = "Completed";
                            dgvData.ReadOnly = true;
                            btnAdd_Item.Enabled = false;
                            btnDel_Item.Enabled = false;
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
                            btnDel_Item.Enabled = false;
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
                    else if(txtPartNo.Text.Trim() != "")
                    {
                        var i = (from ix in db.mh_Items select ix).Where(a => a.InternalNo == txtPartNo.Text && (a.InventoryGroup =="FG" || a.InventoryGroup =="SEMI" )).ToList();
                        if (i.Count() > 0)
                        {
                            txtPartName.Text = StockControl.dbClss.TSt(i.FirstOrDefault().InternalName);
                            txtTypePart.Text = StockControl.dbClss.TSt(i.FirstOrDefault().InventoryGroup);
                            txtDescription.Text = dbClss.TSt(i.FirstOrDefault().InternalDescription);
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
        private void SaveHerder()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                var g = (from ix in db.tb_BomHDs
                         where ix.PartNo.Trim().ToUpper() == txtPartNo.Text.Trim().ToUpper()
                         && ix.BomNo.Trim().ToUpper() == txtBomNo.Text.Trim().ToUpper()
                         //&& ix.Status != "Cancel"
                         ////&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                         select ix).ToList();
                if (g.Count > 0)  //มีรายการในระบบ
                {
                    foreach (DataRow row in dt_HD.Rows)
                    {

                        var gg = (from ix in db.tb_BomHDs
                                  where ix.PartNo.Trim().ToUpper() == txtPartNo.Text.Trim().ToUpper()
                                  && ix.BomNo.Trim().ToUpper() == txtBomNo.Text.Trim().ToUpper()
                                  //&& ix.Status != "Cancel"
                                  ////&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                                  select ix).First();

                        //gg.Status = "Waiting";
                        //gg.TEMPNo = txtTempNo.Text;
                        gg.ModifyBy = ClassLib.Classlib.User;
                        gg.ModifyDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));

                        dbClss.AddHistory(this.Name, "แก้ไข Bom", "แก้ไข Bom โดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy", new CultureInfo("en-US")) + "]", txtBomNo.Text.Trim() + "-"+txtPartNo.Text.Trim());

                        var i = (from ix in db.mh_Items select ix).Where(a => a.InternalNo.Equals(txtPartNo.Text.Trim().ToUpper())).ToList();
                        if (i.Count > 0)
                        {
                            gg.PartDescription = dbClss.TSt(i.FirstOrDefault().InternalDescription);//txtDescription.Text;
                            gg.PartName = dbClss.TSt(i.FirstOrDefault().InternalName); //txtPartName.Text;
                        }
                        //if (StockControl.dbClss.TSt(gg.Barcode).Equals(""))
                        //    gg.Barcode = StockControl.dbClss.SaveQRCode2D(txtPRNo.Text.Trim());

                        int ver = dbClss.TInt(txtVersion.Text) +1;
                        gg.Version = ver.ToString();
                        gg.Status = "Process";
                        
                        if (!dtBegin.Text.Trim().Equals("") && !dtEnd.Text.Trim().Equals(""))
                        {
                           
                            gg.StartDate = Convert.ToDateTime(dtBegin.Value, new CultureInfo("en-US"));
                            gg.EndDate = Convert.ToDateTime(dtEnd.Value, new CultureInfo("en-US"));

                            dbClss.AddHistory(this.Name, "แก้ไข Bom", "แก้ไขวันเริ่มและสิ้นสุด [ วันที่เริ่ม : " + dtBegin.Text.Trim() +" วันที่สิ้นสุด : "+ dtEnd.Text.Trim() + "]", txtBomNo.Text.Trim() + "-" + txtPartNo.Text.Trim());

                            //}
                        }
                        if (!txtRemarkHD.Text.Trim().Equals(row["Remark"].ToString()))
                        {
                            gg.Remark = txtRemarkHD.Text.Trim();
                            dbClss.AddHistory(this.Name, "แก้ไข Bom", "แก้ไขหมายเหตุ [" + txtRemarkHD.Text.Trim() + "]", txtBomNo.Text.Trim() + "-" + txtPartNo.Text.Trim());
                        }
                        db.SubmitChanges();
                    }
                }
                else  // Add ใหม่
                {
                    //byte[] barcode = null;
                    //if (!txtPRNo.Text.Equals(""))
                    //    barcode = StockControl.dbClss.SaveQRCode2D(txtPRNo.Text.Trim());
                   

                    tb_BomHD gg = new tb_BomHD();
                    gg.ModifyBy = ClassLib.Classlib.User;
                    gg.ModifyDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    gg.CreateBy = ClassLib.Classlib.User;
                    gg.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                    gg.PartNo = txtPartNo.Text.Trim();
                    gg.BomNo = txtBomNo.Text.Trim().ToUpper();
                    gg.PartDescription = txtDescription.Text.Trim();
                    gg.PartName = txtPartName.Text.Trim();
                    gg.Remark = txtRemarkHD.Text.Trim();
                    gg.Version = txtVersion.Text.Trim();
                    gg.Status = "Process";
                    if (!dtBegin.Text.Trim().Equals("") && !dtEnd.Text.Trim().Equals(""))
                    {
                        gg.StartDate = Convert.ToDateTime(dtBegin.Value, new CultureInfo("en-US"));
                        gg.EndDate = Convert.ToDateTime(dtEnd.Value, new CultureInfo("en-US"));
                    }
                    db.tb_BomHDs.InsertOnSubmit(gg);
                    db.SubmitChanges();

                    dbClss.AddHistory(this.Name, "เพิ่ม Bom", "สร้าง Bom No. : [ " + txtBomNo.Text.Trim() + " ,Part No. : :" + txtPartNo.Text + " ] ", txtBomNo.Text.Trim() + "-" + txtPartNo.Text.Trim());

                }
            }
        }
        private bool AddPR_d()
        {

            bool ck = false;
            //int C = 0;
            //try
            //{


            dgvData.EndEdit();
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                int line_t = 50;
                string ComponentName = "";
                string ComponentDescription = "";
                foreach (var g in dgvData.Rows)
                {
                    if (g.IsVisible.Equals(true))
                    {
                       // line_t += 50;
                        DateTime? d = null;
                        var i = (from ix in db.mh_Items select ix).Where(a => a.InternalNo.Contains(StockControl.dbClss.TSt(g.Cells["dgvComponent"].Value))).ToList();
                        if (i.Count > 0)
                        {
                            ComponentName = dbClss.TSt(i.FirstOrDefault().InternalName);
                            ComponentDescription = dbClss.TSt(i.FirstOrDefault().InternalDescription);

                        }

                        if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) <= 0)  //New ใหม่
                        {
                            
                                tb_BomDT u = new tb_BomDT();
                            u.PartNo = txtPartNo.Text.Trim();
                            u.BomNo = txtBomNo.Text.Trim().ToUpper();
                            
                            u.Line = line_t;
                            u.Component = StockControl.dbClss.TSt(g.Cells["dgvComponent"].Value);
                            u.ComponentDescription = StockControl.dbClss.TSt(g.Cells["dgvComponentDescription"].Value);
                            u.ComponentName = StockControl.dbClss.TSt(g.Cells["dgvComponentName"].Value);
                            u.Qty = StockControl.dbClss.TDe(g.Cells["dgvQty"].Value);
                            u.Unit = StockControl.dbClss.TSt(g.Cells["dgvUnit"].Value);
                            u.UnitCost = StockControl.dbClss.TDe(g.Cells["dgvUnitCost"].Value);
                            u.Cost = StockControl.dbClss.TDe(g.Cells["dgvCost"].Value);
                            u.PCSUnit = dbClss.TDe(g.Cells["dgvPCSUnit"].Value);
                            db.tb_BomDTs.InsertOnSubmit(u);
                            db.SubmitChanges();
                            //C += 1;
                            dbClss.AddHistory(this.Name, "เพิ่ม Item Bom", "เพิ่มรายการ Component [" + u.Component + "]", txtBomNo.Text.Trim() + "-" + txtPartNo.Text.Trim());

                            line_t += 50;
                        }
                        else  // อัพเดต
                        {

                            if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) > 0)
                            {
                                foreach (DataRow row in dt_DT.Rows)
                                {
                                    if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) == StockControl.dbClss.TInt(row["id"]))
                                    {
                                        //var u = (from ix in db.tb_BomDTs
                                        //         where ix.PartNo.ToUpper() == txtPartNo.Text.Trim().ToUpper()
                                        //         && ix.BomNo.ToUpper() == txtBomNo.Text.Trim().ToUpper()
                                        //         // && ix.TempNo == txtTempNo.Text
                                        //         && ix.id == StockControl.dbClss.TInt(g.Cells["dgvid"].Value)
                                        //         select ix).First();

                                        dbClss.AddHistory(this.Name, "แก้ไขรายการ Item Bom", "id :" + StockControl.dbClss.TSt(g.Cells["dgvid"].Value)
                                        + " BomNo :" + txtBomNo.Text.ToUpper()
                                        + " ,PartNo :" + txtPartNo.Text
                                        + " แก้ไขโดย [" + ClassLib.Classlib.User + " วันที่ :" + DateTime.Now.ToString("dd/MMM/yyyy", new CultureInfo("en-US")) + "]"
                                        , txtBomNo.Text.Trim() + "-" + txtPartNo.Text.Trim());

                                        //u.PartNo = txtPartNo.Text.Trim();
                                        //u.BomNo = txtBomNo.Text.Trim().ToUpper();

                                        if (!StockControl.dbClss.TSt(g.Cells["dgvComponent"].Value).Equals(row["Component"].ToString()))
                                        {
                                            //u.Component = StockControl.dbClss.TSt(g.Cells["dgvComponent"].Value);
                                            dbClss.AddHistory(this.Name, "แก้ไข Item Bom", "แก้ไข Component [" + StockControl.dbClss.TSt(g.Cells["dgvComponent"].Value) + "]", txtBomNo.Text.Trim() + "-" + txtPartNo.Text.Trim());
                                        }

                                        //u.Line = line_t;                         
                                        decimal Qty = 0; decimal.TryParse(StockControl.dbClss.TSt(g.Cells["dgvQty"].Value), out Qty);
                                        if (!StockControl.dbClss.TSt(g.Cells["dgvQty"].Value).Equals(row["Qty"].ToString()))
                                        {
                                            //u.Qty = StockControl.dbClss.TDe(g.Cells["dgvQty"].Value);
                                            dbClss.AddHistory(this.Name, "แก้ไข Item Bom", "แก้ไขจำนวน [" + Qty.ToString() + "]", txtBomNo.Text.Trim() + "-" + txtPartNo.Text.Trim());
                                        }
                                        //u.PCSUnit = StockControl.dbClss.TDe(g.Cells["dgvPCSUnit"].Value);

                                        if (!StockControl.dbClss.TSt(g.Cells["dgvUnit"].Value).Equals(row["Unit"].ToString()))
                                        {
                                            // u.Unit = StockControl.dbClss.TSt(g.Cells["dgvUnit"].Value);
                                            dbClss.AddHistory(this.Name, "แก้ไข Item Bom", "แก้ไขหน่วย [" + StockControl.dbClss.TSt(g.Cells["dgvUnit"].Value) + "]", txtBomNo.Text.Trim() + "-" + txtPartNo.Text.Trim());
                                        }

                                        //u.UnitCost = StockControl.dbClss.TDe(g.Cells["dgvUnitCost"].Value);
                                        //u.Cost = StockControl.dbClss.TDe(g.Cells["dgvCost"].Value);

                                        db.sp_020_Bom_DT_ADD(StockControl.dbClss.TInt(g.Cells["dgvid"].Value)
                                            , txtPartNo.Text.Trim(), txtBomNo.Text.Trim(), null, null, line_t
                                            , StockControl.dbClss.TSt(g.Cells["dgvComponent"].Value)
                                            , ComponentName//StockControl.dbClss.TSt(g.Cells["dgvComponentName"].Value)
                                            , ComponentDescription// StockControl.dbClss.TSt(g.Cells["dgvComponentDescription"].Value)
                                            , Qty
                                            , StockControl.dbClss.TSt(g.Cells["dgvUnit"].Value)
                                            , StockControl.dbClss.TDe(g.Cells["dgvUnitCost"].Value)
                                            , StockControl.dbClss.TDe(g.Cells["dgvCost"].Value)
                                            , StockControl.dbClss.TDe(g.Cells["dgvPCSUnit"].Value)
                                            , ClassLib.Classlib.User
                                            
                                            );
                                        //db.SubmitChanges();
                                         line_t += 50;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else //Del
                    {
                        if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) > 0)
                        {
                            db.sp_019_Bom_DT_Del(StockControl.dbClss.TInt(g.Cells["dgvid"].Value), txtBomNo.Text.Trim(), txtPartNo.Text.Trim(), StockControl.dbClss.TSt(g.Cells["dgvComponent"].Value), ClassLib.Classlib.User);

                            db.SubmitChanges();
                        }
                    }
                }
            }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    dbClss.AddError("CreatePR", ex.Message, this.Name);
            //}

            //if (C > 0)
            //    MessageBox.Show("บันทึกสำเร็จ!");

            return ck;
        }
       
        private void btnCancel_Click(object sender, EventArgs e)
        {
           
        }
        private void Enable_Status(bool ss, string Condition)
        {
            if (Condition.Equals("-") || Condition.Equals("New"))
            {
                txtPartNo.Enabled = ss;
                txtBomNo.Enabled = ss;
                //txtDescription.Enabled = ss;
                dtBegin.Enabled = ss;
                dtEnd.Enabled = ss;
                dgvData.ReadOnly = false;
                txtRemarkHD.Enabled = ss;              
                btnAdd_Item.Enabled = ss;
                btnDel_Item.Enabled = ss;
                txtVersion.Enabled = ss;
               
            }
            else if (Condition.Equals("View"))
            {
                txtPartNo.Enabled = ss;
                txtBomNo.Enabled = ss;
                //txtDescription.Enabled = ss;
                dtBegin.Enabled = ss;
                dtEnd.Enabled = ss;               
                dgvData.ReadOnly = !ss;
                txtRemarkHD.Enabled = ss; 
                btnAdd_Item.Enabled = ss;
                btnDel_Item.Enabled = ss;
                txtVersion.Enabled = ss;
            }
            else if (Condition.Equals("Edit"))
            {
                txtPartNo.Enabled = ss;
                txtBomNo.Enabled = ss;
               // txtDescription.Enabled = ss;
                dtBegin.Enabled = ss;
                dtEnd.Enabled = ss;               
                dgvData.ReadOnly = !ss;
                txtRemarkHD.Enabled = ss;              
                btnAdd_Item.Enabled = ss;
                btnDel_Item.Enabled = ss;
                txtVersion.Enabled = ss;
            }
        }
       
        private void ClearData()
        {
            txtPartNo.Text = "";
            txtBomNo.Text = "";
            txtDescription.Text = "";
            txtPartName.Text = "";
            txtTypePart.Text = "";
            dtBegin.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
            dtEnd.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
            dtBegin.SetToNullValue();
            dtEnd.SetToNullValue();
            dgvData.Rows.Clear();
            dgvData.DataSource = null;
            txtRemarkHD.Text = "";
            dt_HD.Rows.Clear();
            dt_DT.Rows.Clear();
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            btnView.Enabled = true;
            btnEdit.Enabled = true;
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
            ////getมาไว้ก่อน แต่ยังไมได้ save 
            //txtTempNo.Text = StockControl.dbClss.GetNo(3, 0);

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
                if (MessageBox.Show("ต้องการลบรายการ ( Bom No. : " + txtBomNo.Text + " Part No. " + txtPartNo.Text + " ) หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Cursor = Cursors.WaitCursor;
                    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    {
                        db.sp_018_Bom_Del(0, txtBomNo.Text, txtPartNo.Text, ClassLib.Classlib.User);
                        
                        //dbClss.AddHistory(this.Name, "ลบ Bom", "Delete Bom [ Bom No. : " + txtBomNo.Text.Trim()+" Part No. : "+txtPartNo.Text + "]", txtBomNo.Text+"_"+txtPartNo.Text);

                        db.SubmitChanges();
                        btnNew_Click(null, null);
                        Ac = "New";
                        btnSave.Enabled = true;

                    }

                    MessageBox.Show("ลบรายการ สำเร็จ!");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }

        }
        private bool Check_Save()
        {
            bool re = true;
            string err = "";
            try
            {
                //if (txtCodeNo.Text.Equals(""))
                //    err += " “รหัสพาร์ท:” เป็นค่าว่าง \n";
                //if (txtPRNo.Text.Equals(""))
                //    err += " “เลขที่ใบขอสั่งซื้อ:” เป็นค่าว่าง \n";
                if (txtPartNo.Text.Equals(""))
                    err += "- “รหัสทูล:” เป็นค่าว่าง \n";
                if (txtTypePart.Text.Equals(""))
                    err += "- “ประเภททูล:” เป็นค่าว่าง \n";
                else if (txtTypePart.Text != "")
                {
                    if(txtTypePart.Text != "FG" && txtTypePart.Text != "SEMI")
                    err += "- “ประเภททูล:” ต้องเป็น FG หรือ SEMI เท่านั่น \n";
                }
                if (txtBomNo.Text.Equals(""))
                    err += "- “รหัสบอม:” เป็นค่าว่าง \n";
                //if (dtRequire.Text.Equals(""))
                //    err += "- “วันที่ต้องการ:” เป็นค่าว่าง \n";

                if (!dtBegin.Text.Equals("") || !dtEnd.Text.Equals(""))
                {
                    if (!dtEnd.Text.Equals(""))
                    {
                        if (Convert.ToDateTime(dtBegin.Value, new CultureInfo("en-US")).CompareTo(Convert.ToDateTime(dtEnd.Value, new CultureInfo("en-US"))) > 0)
                            err += "- “วันที่เริ่ม มากกว่า วันที่สิ้นสุด” ไม่ได้ \n";
                    }
                }
                else if(!dtBegin.Text.Equals(""))
                {
                    dtEnd.Value = dtBegin.Value;
                }
                //else
                //{

                //    //err += "- “วันที่เริ่ม หรือ วันที่สิ้นสุด” เป็นค่าว่าง \n";

                //    ////DateTime? temp = null;
                //    //dtBegin.SetToNullValue();
                //    //dtEnd.SetToNullValue();
                //    ////dtBegin.Value = temp.Value;
                //    ////dtEnd.Value = temp.Value;
                //    ////dtBegin.Text = "";
                //    ////dtEnd.Text = "";
                //}


                if (dgvData.Rows.Count <= 0)
                    err += "- “รายการ:” เป็นค่าว่าง \n";
                foreach (GridViewRowInfo rowInfo in dgvData.Rows)
                {
                    if (rowInfo.IsVisible)
                    {
                        if (StockControl.dbClss.TSt(rowInfo.Cells["dgvComponent"].Value).Equals(""))
                            err += "- “รหัสทูล(Component):” เป็นค่าว่าง \n";
                        if (StockControl.dbClss.TDe(rowInfo.Cells["dgvQty"].Value) <= 0)
                            err += "- “จำนวน:” น้อยกว่า 0 \n";
                        if (StockControl.dbClss.TSt(rowInfo.Cells["dgvUnit"].Value).Equals(""))
                            err += "- “หน่วย:” เป็นค่าว่าง \n";
                        if (StockControl.dbClss.TDe(rowInfo.Cells["dgvPCSUnit"].Value) <= 0)
                            err += "- “จำนวน/หน่วย:” น้อยกว่า 0 \n";
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
                if (Ac.Equals("New") || Ac.Equals("Edit"))
                {
                    if (Check_Save())
                        return;
                    else
                    {

                        if (MessageBox.Show("ต้องการบันทึก ?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            this.Cursor = Cursors.WaitCursor;

                            using (DataClasses1DataContext db = new DataClasses1DataContext())
                            {
                                if (Ac.Equals("New"))
                                {
                                    //ถ้ามีการใส่เลขที่ PR เช็คดูว่ามีการใส่เลขนี้แล้วหรือไม่ ถ้ามีให้ใส่เลขอื่น
                                    if (!txtPartNo.Text.Trim().Equals("") && !txtBomNo.Text.Trim().Equals(""))
                                    {

                                        var p = (from ix in db.tb_BomHDs
                                                 where ix.PartNo.ToUpper().Trim() == txtPartNo.Text.Trim() 
                                                 && ix.BomNo.ToUpper().Trim() == txtBomNo.Text.Trim()
                                                 //&& ix.Status != "Cancel"
                                                 //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                                                 select ix).ToList();
                                        if (p.Count > 0)  //มีรายการในระบบ
                                        {
                                            MessageBox.Show("เลขที่ใบสั่งซื้อถูกใช้ไปแล้ว กรุณาใส่เลขใหม่");
                                            return;
                                        }
                                    }
                                    //txtTempNo.Text = StockControl.dbClss.GetNo(3, 2);
                                }
                                //var ggg = (from ix in db.tb_PurchaseRequests
                                //           where ix.TEMPNo.Trim() == txtTempNo.Text.Trim() //&& ix.Status != "Cancel"
                                //           //&& ix.TEMPNo.Trim() == txtTempNo.Text.Trim()
                                //           select ix).ToList();
                                //if (ggg.Count > 1)  //มีรายการในระบบ
                                //{
                                //    MessageBox.Show("เลขที่อ้างอิงถูกใช้แล้ว กรุณาสร้างเลขใหม่");
                                //    return;
                                //}
                            }

                            if (!txtPartNo.Text.Trim().Equals("") && !txtBomNo.Text.Trim().Equals(""))
                            {

                                SaveHerder();
                                AddPR_d();

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
        //private void Insert_Stock_temp()
        //{
        //    //try
        //    //{

        //    //    using (DataClasses1DataContext db = new DataClasses1DataContext())
        //    //    {
                  
        //    //        var g = (from ix in db.tb_PurchaseRequestLines
        //    //                     //join i in db.tb_Items on ix.CodeNo equals i.CodeNo
        //    //                 where ix.TempNo.Trim() == txtTempNo.Text.Trim() && ix.SS == 1
        //    //                 select ix).ToList();
        //    //        if (g.Count > 0)
        //    //        {
        //    //            //insert Stock
        //    //            foreach (var vv in g)
        //    //            {
        //    //                db.sp_010_Update_StockItem(Convert.ToString(vv.CodeNo),"");
        //    //                //dbClss.Insert_StockTemp(vv.CodeNo, Convert.ToDecimal(vv.OrderQty), "PR_Temp", "Inv");
        //    //            }
        //    //        }
        //    //    }
        //    //}
        //    //catch (Exception ex) { MessageBox.Show(ex.Message); }
        //}
        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                dgvData.EndEdit();
                if (e.RowIndex >= -1)
                {

                    if (dgvData.Columns["dgvQty"].Index == e.ColumnIndex
                        || dgvData.Columns["dgvUnitCost"].Index == e.ColumnIndex
                        )
                    {
                        decimal OrderQty = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["dgvQty"].Value), out OrderQty);
                        decimal StandardCost = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["dgvUnitCost"].Value), out StandardCost);
                        e.Row.Cells["dgvCost"].Value = OrderQty * StandardCost;
                    }

                    else if (dgvData.Columns["dgvUnit"].Index == e.ColumnIndex)
                    {
                        string dgvUOM = dbClss.TSt(e.Row.Cells["dgvUnit"].Value);
                        string CodeNo = dbClss.TSt(e.Row.Cells["dgvComponent"].Value);
                        decimal PCSUOM = dbClss.Con_UOM(CodeNo, dgvUOM);
                        e.Row.Cells["dgvPCSUnit"].Value = dbClss.Con_UOM(CodeNo, dgvUOM);
                        //using (DataClasses1DataContext db = new DataClasses1DataContext())
                        //{
                        //    var g = (from ix in db.mh_Items select ix)
                        //        .Where(a => a.InternalNo.ToUpper().Trim().Equals(CodeNo.ToUpper().Trim())).ToList();
                        //    if (g.Count > 0)
                        //    {
                        //        if (dgvUOM == dbClss.TSt(g.FirstOrDefault().PurchaseUOM))
                        //        {
                        //            e.Row.Cells["dgvPCSUnit"].Value = dbClss.Con_UOM(CodeNo, dgvUOM);
                        //            //e.Row.Cells["dgvPCSUnit"].ReadOnly = true;
                        //        }                           
                        //    }

                        //}
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
            try
            {
                if (!txtPartNo.Text.Equals("") && !txtBomNo.Text.Equals(""))
                {
                    List<GridViewRowInfo> dgvRow_List = new List<GridViewRowInfo>();
                    //dgvRow_List.Clear();              
                    ListPart MS = new ListPart(txtPartNo, "SEMI-RM", dgvRow_List);
                    //ListPart_CreatePR MS = new ListPart_CreatePR(dgvRow_List, txtVendorNo.Text);
                    MS.ShowDialog();
                    if (dgvRow_List.Count > 0)
                    {
                        string CodeNo = "";
                        this.Cursor = Cursors.WaitCursor;
                        decimal OrderQty = 1;
                        foreach (GridViewRowInfo ee in dgvRow_List)
                        {
                            CodeNo = Convert.ToString(ee.Cells["InternalNo"].Value).Trim();
                            if (!CodeNo.Equals("") && !check_Duppicate(CodeNo))
                            {
                                Add_Part(CodeNo, OrderQty);
                            }
                            else
                            {
                                MessageBox.Show("รหัสพาร์ท ซ้ำ");
                            }
                        }
                    }
                }
                else
                    MessageBox.Show("เลือกรหัสทูล หรือ รหัสบอม ก่อน !!!");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }

        }
        private bool check_Duppicate(string CodeNo)
        {
            bool re = false;
            foreach (var rd1 in dgvData.Rows)
            {
                if (rd1.IsVisible.Equals(true))
                {
                    if (StockControl.dbClss.TSt(rd1.Cells["dgvComponent"].Value).Trim().ToUpper().Equals(CodeNo.Trim().ToUpper()))
                        re = true;
                }
            }

            return re;

        }
        private void Add_Part(string CodeNo,decimal OrderQty)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                int Row = 0; Row = dgvData.Rows.Count()+1;
                var g = (from ix in db.mh_Items select ix).Where(a => a.InternalNo.Contains(CodeNo)).ToList();
                if (g.Count > 0)
                {
                    decimal PCSUnit = dbClss.Con_UOM(StockControl.dbClss.TSt(g.FirstOrDefault().InternalNo)
                            , StockControl.dbClss.TSt(g.FirstOrDefault().ConsumptionUOM));

                    //dgvData.Rows.Add(Row.ToString()
                    //    , txtPartNo.Text.Trim()
                    //    , CodeNo
                    //     //StockControl.dbClss.TSt(g.FirstOrDefault().InternalNo)
                    //     , dbClss.TSt(g.FirstOrDefault().InternalName)
                    //     , dbClss.TSt(g.FirstOrDefault().InternalDescription)
                    //     , dbClss.TSt(g.FirstOrDefault().GroupType)
                    //    , StockControl.dbClss.TSt(g.FirstOrDefault().InventoryGroup)
                    //    , OrderQty
                    //    //, StockControl.dbClss.TDe(g.FirstOrDefault().PCSUnit)
                    //    , StockControl.dbClss.TSt(g.FirstOrDefault().ConsumptionUOM)
                    //    , PCSUnit
                    //    , 0//1 * StockControl.dbClss.TDe(g.FirstOrDefault().StandardCost)
                    //    , 0
                    //    , 0
                    //    ,0
                    //    , dbClss.TSt(g.FirstOrDefault().ReplenishmentType)
                    //    );

                    Add_Item(Row.ToString(), CodeNo, dbClss.TSt(g.FirstOrDefault().InternalNo)
                       , dbClss.TSt(g.FirstOrDefault().InternalName)
                        , dbClss.TSt(g.FirstOrDefault().InternalDescription)
                        , dbClss.TSt(g.FirstOrDefault().GroupType)
                        , StockControl.dbClss.TSt(g.FirstOrDefault().InventoryGroup)
                        , OrderQty
                        , StockControl.dbClss.TSt(g.FirstOrDefault().ConsumptionUOM)
                        , PCSUnit
                        , 0, 0, 0, 0, dbClss.TSt(g.FirstOrDefault().ReplenishmentType)
                        );
                }
            }
        }
        private void Add_Item(string No, string dgvPartNo, string dgvComponent
      , string dgvComponentName, string dgvComponentDescription, string dgvGroupType
          , string dgvType
          , decimal dgvQty, string dgvUnit, decimal dgvPCSUnit
     , decimal dgvUnitCost, decimal dgvCost, int dgvid, int dgvLine, string dgvReplenishmentType

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

                ee.Cells["dgvNo"].Value = No.ToString();
                ee.Cells["dgvPartNo"].Value = dgvPartNo;
                ee.Cells["dgvComponent"].Value = dgvComponent;
                ee.Cells["dgvComponentName"].Value = dgvComponentName;
                ee.Cells["dgvComponentDescription"].Value = dgvComponentDescription;
                ee.Cells["dgvGroupType"].Value = dgvGroupType;
                ee.Cells["dgvType"].Value = dgvType;
                ee.Cells["dgvQty"].Value = dgvQty;
                ee.Cells["dgvUnit"].Value = dgvUnit;
                ee.Cells["dgvPCSUnit"].Value = dgvPCSUnit;
                ee.Cells["dgvUnitCost"].Value = dgvUnitCost;
                ee.Cells["dgvCost"].Value = dgvCost;
                ee.Cells["dgvid"].Value = dgvid;
                ee.Cells["dgvLine"].Value = dgvLine;
                ee.Cells["dgvReplenishmentType"].Value = dgvReplenishmentType;


                ////if (lblStatus.Text.Equals("Completed"))//|| lbStatus.Text.Equals("Reject"))
                ////    dgvData.AllowAddNewRow = false;
                ////else
                ////    dgvData.AllowAddNewRow = true;

                ////dbclass.SetRowNo1(dgvData);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError(this.Name, ex.Message + " : Add_Item", this.Name); }

        }
        private void ลบพารทToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
               
                if (dgvData.Rows.Count <= 0)
                    return;


                if (Ac.Equals("New") || Ac.Equals("Edit"))
                {
                    this.Cursor = Cursors.WaitCursor;
                    
                    int id = 0;
                    int.TryParse(StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["dgvid"].Value), out id);
                    if (id <= 0)
                        dgvData.Rows.Remove(dgvData.CurrentRow);
                    
                    else
                    {
                        string CodeNo = ""; StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["dgvCompanent"]);
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
        private void cboVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    using (DataClasses1DataContext db = new DataClasses1DataContext())
            //    {
            //        if (!cboVendorName.Text.Equals(""))
            //        {
            //            var I = (from ix in db.tb_Vendors select ix).Where(a => a.Active == true && a.VendorName.Equals(cboVendorName.Text)).ToList();
            //            if (I.Count > 0)
            //            {
            //                //StockControl.dbClss.TBo(a.Active).Equals(true)
            //                txtCurrency.Text = I.FirstOrDefault().CRRNCY;
            //                txtAddress.Text = I.FirstOrDefault().Address;
            //                txtVendorNo.Text = I.FirstOrDefault().VendorNo;
            //                var g = (from ix in db.tb_VendorContacts select ix).Where(a => a.VendorNo.Equals(txtVendorNo.Text)).OrderByDescending(b => b.DefaultNo).ToList();
            //                if (g.Count > 0)
            //                {
            //                    txtContactName.Text = g.FirstOrDefault().ContactName;
            //                    txtTel.Text = g.FirstOrDefault().Tel;
            //                    txtFax.Text = g.FirstOrDefault().Fax;
            //                    txtEmail.Text = g.FirstOrDefault().Email;
            //                }
            //                else
            //                {
            //                    txtContactName.Text = "";
            //                    txtTel.Text = "";
            //                    txtFax.Text = "";
            //                    txtEmail.Text = "";
            //                }
            //            }
            //            else
            //            {
            //                txtCurrency.Text = "";
            //                txtAddress.Text = "";
            //                txtVendorNo.Text = "";
            //                txtContactName.Text = "";
            //                txtTel.Text = "";
            //                txtFax.Text = "";
            //            }
            //        }

            //    }
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void MasterTemplate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                ลบพารทToolStripMenuItem_Click(null, null);
        }

        private void btnListItem_Click(object sender, EventArgs e)
        {
            try
            {
                btnEdit.Enabled = true;
                btnNew.Enabled = true;
                //Cleardata();
                btnNew_Click(null, null);

                

                this.Cursor = Cursors.WaitCursor;
                Bom_List sc = new Bom_List(txtPartNo, txtBomNo, "FG-SEMI");
                this.Cursor = Cursors.Default;
                sc.ShowDialog();
                GC.Collect();
                GC.WaitForPendingFinalizers();


                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();

                if(txtPartNo.Text != "" && txtBomNo.Text != "")
                    Enable_Status(false, "View");
                else
                    Enable_Status(false, "New");

                DataLoad();
                //btnGET.Enabled = false;
                btnView.Enabled = false;
                //chkGET.Enabled = false;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError("Bom", ex.Message + " : btnFind_Click", this.Name); }


            //////DataLoad();
            //try
            //{
            //    btnEdit.Enabled = true;
            //    btnView.Enabled = false;
            //    btnNew.Enabled = true;
            //    ClearData();
            //    Ac = "View";
            //    Enable_Status(false, "View");

            //    this.Cursor = Cursors.WaitCursor;
            //    CreatePR_List sc = new CreatePR_List(txtTempNo);
            //    this.Cursor = Cursors.Default;
            //    sc.ShowDialog();
            //    GC.Collect();
            //    GC.WaitForPendingFinalizers();

            //    ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            //    ClassLib.Memory.Heap();
            //    //LoadData
            //    DataLoad();
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError("CreatePart", ex.Message + " : radButtonElement1_Click", this.Name); }
            //finally { this.Cursor = Cursors.Default; }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            btnEdit.Enabled = true;
            btnView.Enabled = false;
            btnNew.Enabled = true;

            string Temp1 = txtBomNo.Text;
            string Tmep2 = txtPartNo.Text;
             ClearData();
            Enable_Status(false, "View");

            txtBomNo.Text = Temp1;
            txtPartNo.Text = Tmep2;

            DataLoad();
            Ac = "View";
        }

        private void txtPRNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == 13 && !txtPartNo.Text.Equals(""))
            //{
            //    string PartT = txtPartNo.Text;
            //    btnNew_Click(null, null);
            //    txtPartNo.Text = PartT;
            //    Load_Part();
            //}
        }
        private void Load_Part()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var ga = (from ix in db.mh_Items select ix).Where(a => a.InternalNo.Contains(txtPartNo.Text.Trim())  
                        && (a.InventoryGroup == "FG" || a.InventoryGroup == "SEMI" )).ToList();
                    if (ga.Count > 0)
                    {
                        txtDescription.Text = dbClss.TSt(ga.FirstOrDefault().InternalDescription);
                        txtPartName.Text = StockControl.dbClss.TSt(ga.FirstOrDefault().InternalName);
                        txtTypePart.Text = StockControl.dbClss.TSt(ga.FirstOrDefault().InventoryGroup);
                    }
                }
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
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
            //                        txtCurrency.Text = I.FirstOrDefault().CRRNCY;
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
            try
            {
                string BomNo = txtBomNo.Text.Trim();
                string PartNo = txtPartNo.Text.Trim();

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var g = (from ix in db.sp_R015_Report_Bom(PartNo,BomNo,"","", Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"))) select ix).ToList();
                    if (g.Count() > 0)
                    {

                        Report.Reportx1.Value = new string[4];
                        Report.Reportx1.Value[0] = PartNo;
                        Report.Reportx1.Value[1] = BomNo;
                        Report.Reportx1.Value[2] ="";
                        Report.Reportx1.Value[3] = "";
                        Report.Reportx1.WReport = "Bom";
                        Report.Reportx1 op = new Report.Reportx1("Bom.rpt");
                        op.Show();

                    }
                    else
                        MessageBox.Show("not found.");
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void cboVendorName_Leave(object sender, EventArgs e)
        {
            cboVendor_SelectedIndexChanged(null, null);
        }

        private void radButtonElement3_Click(object sender, EventArgs e)
        {
            //string RefPO = "";
            //string TempNo = txtTempNo.Text;
            //if (!txtTempNo.Text.Equals(""))
            //{
            //    string GetMarkup = Interaction.InputBox("ใส่เลขที่ P/O ใหม่!", "P/O New : ", "", 400, 250);
            //    if (!GetMarkup.Trim().Equals(""))
            //    {
            //        RefPO = GetMarkup;
            //        using (DataClasses1DataContext db = new DataClasses1DataContext())
            //        {
            //            db.sp_UpdatePO(TempNo, RefPO);
            //        }
            //        MessageBox.Show("Update Completed.");
            //        btnRefresh_Click(sender, e);
            //    }
            //}

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //btnEdit.Enabled = true;
                //btnNew.Enabled = true;
                ////Cleardata();
                string PartT = txtPartNo.Text;
                btnNew_Click(null, null);
                txtPartNo.Text = PartT;
                //Enable_Status(false, "View");

                this.Cursor = Cursors.WaitCursor;
                ListPart sc = new ListPart(txtPartNo, "FG-SEMI","Bom");
                this.Cursor = Cursors.Default;
                sc.ShowDialog();

                Load_Part();

                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();
                //DataLoad();
                //btnGET.Enabled = false;
                btnView.Enabled = false;
                //chkGET.Enabled = false;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError("Bom", ex.Message + " : button1_Click", this.Name); }

        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                //btnEdit.Enabled = true;
                //btnNew.Enabled = true;
                //Cleardata();
                // btnNew_Click(null, null);

                // Enable_Status(false, "View");
                //List<GridViewRowInfo> dgvRow_List = new List<GridViewRowInfo>();

                this.Cursor = Cursors.WaitCursor;              

                Bom_List sc = new Bom_List(txtPartNo,txtBomNo, "FG-SEMI");
                this.Cursor = Cursors.Default;
                sc.ShowDialog();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();

                DataLoad();

                //btnGET.Enabled = false;
                //btnView.Enabled = false;
                //chkGET.Enabled = false;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError("Bom", ex.Message + " : btnFind_Click", this.Name); }

        }

       

        private void txtBomNo_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //กรณีที่ Key Item and bom ทั้งคู่
            if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter)
            {
                if (!txtPartNo.Text.Trim().Equals("") && !txtBomNo.Text.Trim().Equals(""))
                {
                    string PartT = txtPartNo.Text;
                    string BomT = txtBomNo.Text;
                    btnNew_Click(null, null);
                    txtPartNo.Text = PartT;
                    txtBomNo.Text = BomT;

                    DataLoad();
                }
                else
                    txtBomNo.Text = txtBomNo.Text.Trim().ToUpper();
            }
        }

        private void txtPartNo_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //กรณีที่ Key Item and bom ทั้งคู่
            if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter)
            {
                if (!txtPartNo.Text.Trim().Equals("") && !txtBomNo.Text.Trim().Equals(""))
                {
                    string PartT = txtPartNo.Text;
                    string BomT = txtBomNo.Text;
                    btnNew_Click(null, null);
                    txtPartNo.Text = PartT;
                    txtBomNo.Text = BomT;

                    DataLoad();
                }
                else if (!txtPartNo.Text.Trim().Equals(""))
                {
                    string PartT = txtPartNo.Text;
                    btnNew_Click(null, null);
                    txtPartNo.Text = PartT;
                    Load_Part();
                }
            }
        }

        private void txtBomNo_Leave(object sender, EventArgs e)
        {
            txtBomNo.Text = txtBomNo.Text.Trim().ToUpper();
            ////กรณีที่ Key Item and bom ทั้งคู่

            //    if (!txtPartNo.Text.Trim().Equals("") && !txtBomNo.Text.Trim().Equals(""))
            //    {
            //        string PartT = txtPartNo.Text;
            //        string BomT = txtBomNo.Text;
            //        btnNew_Click(null, null);
            //        txtPartNo.Text = PartT;
            //        txtBomNo.Text = BomT;

            //        DataLoad();
            //    }

            //กรณีที่ Key Item and bom ทั้งคู่
           
                if (!txtPartNo.Text.Trim().Equals("") && !txtBomNo.Text.Trim().Equals(""))
                {
                    string PartT = txtPartNo.Text;
                    string BomT = txtBomNo.Text;
                    btnNew_Click(null, null);
                    txtPartNo.Text = PartT;
                    txtBomNo.Text = BomT;

                    DataLoad();
                }
               
            

        }

        private void MasterTemplate_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            try
            {
                if (e.Column.Name.Equals("dgvUnit"))
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
            }catch(Exception ex) { MessageBox.Show(ex.Message); }

            }
    }
}
