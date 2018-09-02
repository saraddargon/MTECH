using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
namespace StockControl
{
    public partial class StockList : Telerik.WinControls.UI.RadRibbonForm
    {
        public StockList(string CodeNox)
        {
            InitializeComponent();
            CodeNo = CodeNox;
            //this.Text = "ประวัติ "+ Screen;
        }
        public StockList()
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

            dgvData.ReadOnly = true;
            dgvData.AutoGenerateColumns = false;
            DataLoad();

            LoadDefault();
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {

                var TypeCode = (from p in db.tb_Items
                                                select p.TypeCode).Distinct();
              

                //var g = (from i in db.tb_Items
                //         select new  
                //         {
                //             Type =  i.TypeCode    

                //         }).ToList();
               
                 
                    ddlType.DataSource = TypeCode;
                    ddlType.DisplayMember = "TypeCode";
                    ddlType.Text = "";

                   // ddlType.Items.Add("");//การ add ค่าเข้าไปต่อท้าย
              
                   
            }

        }
        private void LoadDefault()
        {
            ddlLocation.DataSource = null;
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                ddlLocation.DisplayMember = "Location";
                ddlLocation.ValueMember = "Location";
                // ddlLocation.DataSource = db.tb_Locations.Where(s => s.Active == true && s.Status == "Completed").ToList();
                var g = (from ix in db.tb_Locations select ix).Where(s => s.Active == true && s.Status == "Completed").ToList();

                List<string> a = new List<string>();
                if (g.Count > 0)
                {
                    foreach (var gg in g)
                        a.Add(gg.Location);
                }
                a.Add("");
                ddlLocation.DataSource = a;

            }
        }

        private void Load_Item()  
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    
                    var g = (from i in db.tb_Items
                             join b in db.tb_Vendors on i.VendorNo equals b.VendorNo
                             where 
                                  i.CodeNo.Contains(txtCodeNo.Text.Trim())
                                 && i.ItemNo.Contains(txtItemNo.Text.Trim())
                                 && i.ItemDescription.Contains(txtItemDescription.Text.Trim())
                                 && b.VendorName.Contains(txtVendorName.Text.Trim())
                                 && i.ShelfNo.Contains(ddlLocation.Text.Trim())
                                 && i.TypeCode.Contains(ddlType.Text)
                                 

                             select new
                             {

                                //x.Cells["StockInv"].Value = (Convert.ToDecimal(db.Cal_QTY(Convert.ToString(x.Cells["CodeNo"].Value), "Invoice", 0)));
                                //x.Cells["StockDL"].Value = (Convert.ToDecimal(db.Cal_QTY(Convert.ToString(x.Cells["CodeNo"].Value), "Temp", 0)));
                                //x.Cells["StockBackOrder"].Value = (Convert.ToDecimal(db.Cal_QTY(Convert.ToString(x.Cells["CodeNo"].Value), "BackOrder", 0)));


                                 CodeNo = i.CodeNo,
                                 ItemNo = i.ItemNo,
                                 ItemDescription = i.ItemDescription,
                                 StockQty = i.StockInv,//(Convert.ToDecimal(db.Cal_QTY(Convert.ToString(i.CodeNo), "Invoice", 0))), //StockControl.dbClss.TDe(i.StockInv),
                                 StockTemp = i.StockDL,//(Convert.ToDecimal(db.Cal_QTY(Convert.ToString(i.CodeNo), "Temp", 0))),// StockControl.dbClss.TDe(i.StockDL),
                                 ShelfNo = i.ShelfNo,
                                 QTY = 0,//StockControl.dbClss.TDe(s.QTY),
                                 GroupCode = i.GroupCode,
                                 TypeCode = i.TypeCode,
                                 StandardCost = Convert.ToDecimal(i.StandardCost),
                                 UnitBuy = i.UnitBuy,
                                 Amount = 0,//StockControl.dbClss.TDe(i.StockInv) * Convert.ToDecimal(i.StandardCost),
                                 VendorNo = i.VendorNo,
                                 VendorItemName =b.VendorName,
                                 Leadtime = i.Leadtime,
                                 MaximumStock = i.MaximumStock,
                                 MinimumStock = i.MinimumStock,
                                 ToolLife = i.Toollife,
                                 SD = i.SD,
                                 Status = i.Status,
                                 StopOrder = i.StopOrder

                             }).ToList();
                    
                    //var g = (from i in db.sp_015_Stock_List()
                    //dgvData.DataSource = g;
                    if (g.Count > 0)
                    {
                        foreach (var gg in g)
                        {
                            dgvData.Rows.Add("",
                                   
                                    gg.CodeNo,
                                    gg.ItemNo,
                                    gg.ItemDescription,
                                    gg.StockQty,
                                    gg.StockTemp,
                                    gg.ShelfNo,
                                    gg.QTY,
                                    gg.GroupCode,
                                    gg.TypeCode,
                                    gg.StandardCost,
                                    gg.UnitBuy,
                                    (Convert.ToDecimal(gg.StandardCost) * Convert.ToDecimal(gg.StockQty))
                                    + (Convert.ToDecimal(gg.StandardCost) * Convert.ToDecimal(gg.StockTemp)), //gg.Amount,
                                    gg.VendorNo,
                                    gg.VendorItemName,
                                    gg.Leadtime,
                                    gg.MaximumStock,
                                    gg.MinimumStock,
                                    gg.ToolLife,
                                    gg.SD,//      ค่าเบียงเบน
                                    gg.Status,
                                    gg.StopOrder);
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;
        }
        private void Load_Item2()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                   
                    var g = (from ix in db.sp_047_List_Check_Stock(txtCodeNo.Text,txtItemNo.Text,txtItemDescription.Text
                             ,"",txtVendorName.Text,"Active",ddlLocation.Text,ddlType.Text,ddlTypePart.Text) select ix).ToList();
                    if (g.Count > 0)
                    {
                        //string Lo1 = dbClss.TSt(db.get_Location_No(1));
                        //string Lo2 = dbClss.TSt(db.get_Location_No(2));
                        //string Lo3 = dbClss.TSt(db.get_Location_No(3));
                        //string Lo4 = dbClss.TSt(db.get_Location_No(4));

                        dgvData.DataSource = g;
                        dbClss.SetRowNo1(dgvData);

                        //if (Lo1 == "")
                        //    dgvData.Columns["QtyLocation1"].IsVisible = false;
                        //if (Lo2 == "")
                        //    dgvData.Columns["QtyLocation2"].IsVisible = false;
                        //if (Lo3 == "")
                        //    dgvData.Columns["QtyLocation3"].IsVisible = false;
                        //if (Lo4 == "")
                        //    dgvData.Columns["QtyLocation4"].IsVisible = false;

                        //dgvData.Columns["QtyLocation1"].HeaderText = Lo1;
                        //dgvData.Columns["QtyLocation2"].HeaderText = Lo2;
                        //dgvData.Columns["QtyLocation3"].HeaderText = Lo3;
                        //dgvData.Columns["QtyLocation4"].HeaderText = Lo4;

                        //foreach (var x in dgvData.Rows)
                        //{
                           
                        //    x.Cells["QtyLocation1"].Value = 0;
                        //    x.Cells["QtyLocation2"].Value = 0;
                        //    x.Cells["QtyLocation3"].Value = 0;
                        //    x.Cells["QtyLocation4"].Value = 0;

                        //    //if (Lo1 == "")
                        //    //    x.Cells["QtyLocation11"].ColumnInfo.IsVisible = false;
                        //    //if (Lo2 == "")
                        //    //    x.Cells["QtyLocation22"].ColumnInfo.IsVisible = false;
                        //    //if (Lo3 == "")
                        //    //    x.Cells["QtyLocation33"].ColumnInfo.IsVisible = false;
                        //    //if (Lo4 == "")
                        //    //    x.Cells["QtyLocation44"].ColumnInfo.IsVisible = false;

                        //    //x.Cells["QtyLocation11"].ColumnInfo.HeaderText = Lo1;
                        //    //x.Cells["QtyLocation22"].ColumnInfo.HeaderText = Lo2;
                        //    //x.Cells["QtyLocation33"].ColumnInfo.HeaderText = Lo3;
                        //    //x.Cells["QtyLocation44"].ColumnInfo.HeaderText = Lo4;

                        //    if (dbClss.TSt(x.Cells["CodeNo"].Value) != "")
                        //    {


                        //        var l = (from ix in db.sp_031_Location_Stock(dbClss.TSt(x.Cells["CodeNo"].Value), "") select ix).ToList();
                        //        if (l.Count > 0)
                        //        {
                        //            foreach (var ll in l)
                        //            {
                        //                if (Lo1 == ll.Location)
                        //                {
                        //                    x.Cells["QtyLocation1"].Value = ll.Qty;
                        //                }
                        //                else if (Lo2 == ll.Location)
                        //                    x.Cells["QtyLocation2"].Value = ll.Qty;
                        //                else if (Lo3 == ll.Location)
                        //                    x.Cells["QtyLocation3"].Value = ll.Qty;
                        //                else if (Lo4 == ll.Location)
                        //                    x.Cells["QtyLocation4"].Value = ll.Qty;
                        //            }
                        //        }
                        //    }

                        //}

                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;
        }
        private void Load_Receive()  //รับสินค้า
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var g = (from i in db.tb_Items
                             join r in db.tb_Receives on i.CodeNo equals r.CodeNo
                             join s in db.tb_Stock1s on r.RCNo equals s.RefNo

                             where s.Status == "Active" //&& d.verticalID == VerticalID
                                    && s.App == "Receive"
                                 && i.CodeNo.Contains(txtCodeNo.Text.Trim())
                                 && i.ItemNo.Contains(txtItemNo.Text.Trim())
                                 && i.ItemDescription.Contains(txtItemDescription.Text.Trim())
                                 && i.VendorItemName.Contains(txtVendorName.Text.Trim())
                                 && i.ShelfNo.Contains(ddlLocation.Text.Trim())

                             select new
                             {
                                 GroupCode = s.App,
                                 TypeCode = s.App,
                                 CodeNo = i.CodeNo,
                                 ItemNo = i.ItemNo,
                                 ItemDescription = i.ItemDescription,
                                 StockQty = StockControl.dbClss.TDe(i.StockInv),
                                 StockTemp = StockControl.dbClss.TDe(i.StockDL),
                                 ShelfNo = i.ShelfNo,
                                 QTY = StockControl.dbClss.TDe(s.QTY),
                                 StandardCost = Convert.ToDecimal(i.StandardCost),
                                 UnitBuy = i.UnitBuy,
                                 Amount = Convert.ToDecimal(s.QTY) * Convert.ToDecimal(i.StandardCost),
                                 VendorNo = i.VendorNo,
                                 VendorItemName = i.VendorItemName,
                                 Leadtime = i.Leadtime,
                                 MaximumStock = i.MaximumStock,
                                 MinimumStock = i.MinimumStock,
                                 ToolLife = i.Toollife,
                                 SD = i.SD,
                                 Status = i.Status,
                                 StopOrder = i.StopOrder

                             }).ToList();
                    //dgvData.DataSource = g;
                    if(g.Count>0)
                    {
                        foreach (var gg in g)
                        {
                            dgvData.Rows.Add("",
                                    gg.GroupCode,
                                    gg.TypeCode,
                                    gg.CodeNo,
                                    gg.ItemNo,
                                    gg.ItemDescription,
                                    gg.StockQty,
                                    gg.StockTemp,
                                    gg.ShelfNo,
                                    gg.QTY,
                                     Convert.ToDecimal(gg.StandardCost),
                                    gg.UnitBuy,
                                    gg.Amount,
                                    gg.VendorNo,
                                    gg.VendorItemName,
                                    gg.Leadtime,
                                    gg.MaximumStock,
                                    gg.MinimumStock,
                                    gg.ToolLife,
                                    gg.SD,//      ค่าเบียงเบน
                                    gg.Status,
                                    gg.StopOrder);
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;
        }
        private void Load_CancelReceive()  //ยกเลิกรับสินค้า
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                  
                    var g = (from i in db.tb_Items
                             join r in db.tb_Receive_Dels on i.CodeNo equals r.CodeNo
                             join s in db.tb_Stock1s on r.RCNo equals s.RefNo

                             where s.Status == "Active" //&& d.verticalID == VerticalID
                                    && s.App == "Cancel RC"
                                 && i.CodeNo.Contains(txtCodeNo.Text.Trim())
                                 && i.ItemNo.Contains(txtItemNo.Text.Trim())
                                 && i.ItemDescription.Contains(txtItemDescription.Text.Trim())
                                 && i.VendorItemName.Contains(txtVendorName.Text.Trim())
                                 && i.ShelfNo.Contains(ddlLocation.Text.Trim())

                             select new
                             {
                                 GroupCode = s.App,
                                 TypeCode = s.App,
                                 CodeNo = i.CodeNo,
                                 ItemNo = i.ItemNo,
                                 ItemDescription = i.ItemDescription,
                                 StockQty = StockControl.dbClss.TDe(i.StockInv),
                                 StockTemp = StockControl.dbClss.TDe(i.StockDL),
                                 ShelfNo = i.ShelfNo,
                                 QTY = StockControl.dbClss.TDe(s.QTY),
                                 StandardCost = Convert.ToDecimal(i.StandardCost),
                                 UnitBuy = i.UnitBuy,
                                 Amount = Convert.ToDecimal(s.QTY) * Convert.ToDecimal(i.StandardCost),
                                 VendorNo = i.VendorNo,
                                 VendorItemName = i.VendorItemName,
                                 Leadtime = i.Leadtime,
                                 MaximumStock = i.MaximumStock,
                                 MinimumStock = i.MinimumStock,
                                 ToolLife = i.Toollife,
                                 SD = i.SD,
                                 Status = i.Status,
                                 StopOrder = i.StopOrder

                             }).ToList();
                    //dgvData.DataSource = g;
                    if (g.Count > 0)
                    {
                        foreach (var gg in g)
                        {
                            dgvData.Rows.Add("",
                                    gg.GroupCode,
                                    gg.TypeCode,
                                    gg.CodeNo,
                                    gg.ItemNo,
                                    gg.ItemDescription,
                                    gg.StockQty,
                                    gg.StockTemp,
                                    gg.ShelfNo,
                                    gg.QTY,
                                    gg.StandardCost,
                                    gg.UnitBuy,
                                    gg.Amount,
                                    gg.VendorNo,
                                    gg.VendorItemName,
                                    gg.Leadtime,
                                    gg.MaximumStock,
                                    gg.MinimumStock,
                                    gg.ToolLife,
                                    gg.SD,//      ค่าเบียงเบน
                                    gg.Status,
                                    gg.StopOrder);
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;
        }
        private void Load_Shipping()  //เบิกสินค้า
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var g = (from i in db.tb_Items
                             join r in db.tb_Shippings on i.CodeNo equals r.CodeNo
                             join s in db.tb_Stock1s on r.ShippingNo equals s.RefNo

                             where s.Status == "Active" //&& d.verticalID == VerticalID
                                    && s.App == "Shipping"
                                 && i.CodeNo.Contains(txtCodeNo.Text.Trim())
                                 && i.ItemNo.Contains(txtItemNo.Text.Trim())
                                 && i.ItemDescription.Contains(txtItemDescription.Text.Trim())
                                 && i.VendorItemName.Contains(txtVendorName.Text.Trim())
                                 && i.ShelfNo.Contains(ddlLocation.Text.Trim())

                             select new
                             {

                                 GroupCode = s.App,
                                 TypeCode = s.App,
                                 CodeNo = i.CodeNo,
                                 ItemNo = i.ItemNo,
                                 ItemDescription = i.ItemDescription,
                                 StockQty = StockControl.dbClss.TDe(i.StockInv),
                                 StockTemp = StockControl.dbClss.TDe(i.StockDL),
                                 ShelfNo = i.ShelfNo,
                                 QTY = StockControl.dbClss.TDe(s.QTY),
                                 StandardCost = Convert.ToDecimal(i.StandardCost),
                                 UnitBuy = i.UnitBuy,
                                 Amount = Convert.ToDecimal(s.QTY) * Convert.ToDecimal(i.StandardCost),
                                 VendorNo = i.VendorNo,
                                 VendorItemName = i.VendorItemName,
                                 Leadtime = i.Leadtime,
                                 MaximumStock = i.MaximumStock,
                                 MinimumStock = i.MinimumStock,
                                 ToolLife = i.Toollife,
                                 SD = i.SD,
                                 Status = i.Status,
                                 StopOrder = i.StopOrder

                             }).ToList();
                    //dgvData.DataSource = g;
                    if (g.Count > 0)
                    {
                        foreach (var gg in g)
                        {
                            dgvData.Rows.Add("",
                                    gg.GroupCode,
                                    gg.TypeCode,
                                    gg.CodeNo,
                                    gg.ItemNo,
                                    gg.ItemDescription,
                                    gg.StockQty,
                                    gg.StockTemp,
                                    gg.ShelfNo,
                                    gg.QTY,
                                    gg.StandardCost,
                                    gg.UnitBuy,
                                    gg.Amount,
                                    gg.VendorNo,
                                    gg.VendorItemName,
                                    gg.Leadtime,
                                    gg.MaximumStock,
                                    gg.MinimumStock,
                                    gg.ToolLife,
                                    gg.SD,//      ค่าเบียงเบน
                                    gg.Status,
                                    gg.StopOrder);
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;
        }
        private void Load_CancelShipping()  //ยกเลิกเบิกสินค้า
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var g = (from i in db.tb_Items
                             join r in db.tb_Shippings on i.CodeNo equals r.CodeNo
                             join s in db.tb_Stock1s on r.ShippingNo equals s.RefNo

                             where s.Status == "Active" //&& d.verticalID == VerticalID
                                    && s.App == "Cancel SH"
                                    && r.Status == "Cancel"
                                 && i.CodeNo.Contains(txtCodeNo.Text.Trim())
                                 && i.ItemNo.Contains(txtItemNo.Text.Trim())
                                 && i.ItemDescription.Contains(txtItemDescription.Text.Trim())
                                 && i.VendorItemName.Contains(txtVendorName.Text.Trim())
                                 && i.Location.Contains(ddlLocation.Text.Trim())

                             select new
                             {
                                 GroupCode = s.App,
                                 TypeCode = s.App,
                                 CodeNo = i.CodeNo,
                                 ItemNo = i.ItemNo,
                                 ItemDescription = i.ItemDescription,
                                 StockQty = StockControl.dbClss.TDe(i.StockInv),
                                 StockTemp = StockControl.dbClss.TDe(i.StockDL),
                                 ShelfNo = i.ShelfNo,
                                 QTY = StockControl.dbClss.TDe(s.QTY),
                                 StandardCost = Convert.ToDecimal(i.StandardCost),
                                 UnitBuy = i.UnitBuy,
                                 Amount = Convert.ToDecimal(s.QTY) * Convert.ToDecimal(i.StandardCost),
                                 VendorNo = i.VendorNo,
                                 VendorItemName = i.VendorItemName,
                                 Leadtime = i.Leadtime,
                                 MaximumStock = i.MaximumStock,
                                 MinimumStock = i.MinimumStock,
                                 ToolLife = i.Toollife,
                                 SD = i.SD,
                                 Status = i.Status,
                                 StopOrder = i.StopOrder,
                                 TypePart = i.TypePart,
                                 Location = i.Location

                             }).ToList();
                    //dgvData.DataSource = g;
                    if (g.Count > 0)
                    {
                        foreach (var gg in g)
                        {
                            dgvData.Rows.Add("",
                                    gg.GroupCode,
                                    gg.TypeCode,
                                    gg.CodeNo,
                                    gg.ItemNo,
                                    gg.ItemDescription,
                                    gg.StockQty,
                                    gg.StockTemp,
                                    gg.ShelfNo,
                                    gg.QTY,
                                    gg.StandardCost,
                                    gg.UnitBuy,
                                    gg.Amount,
                                    gg.VendorNo,
                                    gg.VendorItemName,
                                    gg.Leadtime,
                                    gg.MaximumStock,
                                    gg.MinimumStock,
                                    gg.ToolLife,
                                    gg.SD,//      ค่าเบียงเบน
                                    gg.Status,
                                    gg.StopOrder
                                    );
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;
        }
        private void DataLoad()
        {
            dgvData.Rows.Clear();
            try

            {
                this.Cursor = Cursors.WaitCursor;

                //Load_Item();
                Load_Item2();

              

                int c = 0;
                foreach (var x in dgvData.Rows)
                {
                    c += 1;
                    x.Cells["dgvNo"].Value = c;

                  
                }
            }
            catch { }
            finally {this.Cursor = Cursors.Default; }    
        }
        private bool CheckDuplicate(string code)
        {
            bool ck = false;

            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                int i = (from ix in db.tb_Units where ix.UnitCode == code select ix).Count();
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

        private void btnNew_Click(object sender, EventArgs e)
        {
            dgvData.ReadOnly = false;
            dgvData.AllowAddNewRow = false;
            dgvData.Rows.AddNew();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            dgvData.ReadOnly = true;

            dgvData.AllowAddNewRow = false;
            DataLoad();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            dgvData.ReadOnly = false;

            dgvData.AllowAddNewRow = false;
            //DataLoad();
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

            DataLoad();

        }

        int row = -1;
        private void radGridView1_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            row = e.RowIndex;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            //  dbClss.ExportGridCSV(radGridView1);
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

        private void radButtonElement1_Click(object sender, EventArgs e)
        {
            try
            {
                //if (screen.Equals(1))
                //{
                //    if (!Convert.ToString(dgvData.CurrentRow.Cells["RCNo"].Value).Equals(""))
                //    {
                //        RCNo_tt.Text = Convert.ToString(dgvData.CurrentRow.Cells["RCNo"].Value);
                //        this.Close();
                //    }
                //    else
                //    {
                //        RCNo_tt.Text = Convert.ToString(dgvData.CurrentRow.Cells["RCNo"].Value);
                //        PRNo_tt.Text = Convert.ToString(dgvData.CurrentRow.Cells["PRNo"].Value);
                //        this.Close();
                //    }
                //}
                //else
                if(dgvData.Rows.Count>0)
                {
                    AdjustStock_Taking a = new AdjustStock_Taking("",
                        Convert.ToString(dgvData.CurrentRow.Cells["CodeNo"].Value)
                        ,"CheckStock");
                    a.ShowDialog();
                    //this.Close();
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvData_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex > -1)
                    radButtonElement1_Click(null, null);
                ////if (screen.Equals(1))
                ////{
                ////    if (!Convert.ToString(e.Row.Cells["RCNo"].Value).Equals(""))
                ////    {
                ////        RCNo_tt.Text = Convert.ToString(e.Row.Cells["RCNo"].Value);
                ////        this.Close();
                ////    }
                ////    else
                ////    {
                ////        RCNo_tt.Text = Convert.ToString(e.Row.Cells["RCNo"].Value);
                ////        PRNo_tt.Text = Convert.ToString(e.Row.Cells["PRNo"].Value);
                ////        this.Close();
                ////    }
                ////}
                ////else
                //try
                //{
                //    {
                //        AdjustStock a = new AdjustStock("",
                //            Convert.ToString(e.Row.Cells["CodeNo"].Value)
                //            ,"CheckStock");
                //        a.ShowDialog();
                //        //this.Close();
                //    }
                //}
                //catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void stockคงเหลอToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string CodeNo = "";
                if (dgvData.Rows.Count > 0)
                {
                    CodeNo = Convert.ToString(dgvData.CurrentRow.Cells["CodeNo"].Value);
                    this.Cursor = Cursors.WaitCursor;
                    Stock_List a = new Stock_List(CodeNo, "Invoice");
                    a.Show();
                }
                else
                    MessageBox.Show("ไม่พบรายการ");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;
        }

        private void tempคงเหลอToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string CodeNo = "";
                if (dgvData.Rows.Count > 0)
                {
                    CodeNo = Convert.ToString(dgvData.CurrentRow.Cells["CodeNo"].Value);
                    this.Cursor = Cursors.WaitCursor;
                    Stock_List a = new Stock_List(CodeNo, "Temp");
                    a.Show();
                }
                else
                    MessageBox.Show("ไม่พบรายการ");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;
        }

        private void backOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string CodeNo = "";
                if (dgvData.Rows.Count > 0)
                {
                    CodeNo = Convert.ToString(dgvData.CurrentRow.Cells["CodeNo"].Value);
                    this.Cursor = Cursors.WaitCursor;
                    Stock_List a = new Stock_List(CodeNo, "BackOrder");
                    a.Show();
                }
                else
                    MessageBox.Show("ไม่พบรายการ");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;
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

        private void radButtonElement2_Click(object sender, EventArgs e)
        {

        }
    }
}
