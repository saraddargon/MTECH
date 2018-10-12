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
    public partial class PrintFGCard : Telerik.WinControls.UI.RadRibbonForm
    {
        public PrintFGCard()
        {
            InitializeComponent();
        }

        string Ac = "";
        string RCNo_L = "";
        string PRNo_L = "";
        DataTable dt_RCH = new DataTable();
        DataTable dt_RCD = new DataTable();
        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.Cursor = Cursors.Default;
        }

        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }
        private void GETDTRow()
        {
            dt_RCH.Columns.Add(new DataColumn("RCNo", typeof(string)));
            dt_RCH.Columns.Add(new DataColumn("TempNo", typeof(string)));
            dt_RCH.Columns.Add(new DataColumn("Status", typeof(string)));
            dt_RCH.Columns.Add(new DataColumn("VendorNo", typeof(string)));
            dt_RCH.Columns.Add(new DataColumn("VendorName", typeof(string)));
            dt_RCH.Columns.Add(new DataColumn("RemarkHD", typeof(string)));
            dt_RCH.Columns.Add(new DataColumn("InvoiceNo", typeof(string)));
            dt_RCH.Columns.Add(new DataColumn("Type", typeof(string)));
            dt_RCH.Columns.Add(new DataColumn("RCDate", typeof(DateTime)));
            dt_RCH.Columns.Add(new DataColumn("TypeReceive", typeof(string)));

            dt_RCD.Columns.Add(new DataColumn("CodeNo", typeof(string)));
            dt_RCD.Columns.Add(new DataColumn("ItemNo", typeof(string)));
            dt_RCD.Columns.Add(new DataColumn("ItemDescription", typeof(string)));
            dt_RCD.Columns.Add(new DataColumn("Qty", typeof(decimal)));
            dt_RCD.Columns.Add(new DataColumn("RemainQty", typeof(decimal)));
            dt_RCD.Columns.Add(new DataColumn("Unit", typeof(string)));
            dt_RCD.Columns.Add(new DataColumn("PCSUnit", typeof(decimal)));
            dt_RCD.Columns.Add(new DataColumn("CostPerUnit", typeof(decimal)));
            dt_RCD.Columns.Add(new DataColumn("Amount", typeof(decimal)));
            dt_RCD.Columns.Add(new DataColumn("CRRNCY", typeof(string)));
            dt_RCD.Columns.Add(new DataColumn("LotNo", typeof(string)));
            dt_RCD.Columns.Add(new DataColumn("SerialNo", typeof(string)));
            dt_RCD.Columns.Add(new DataColumn("ShelfNo", typeof(string)));
            dt_RCD.Columns.Add(new DataColumn("Remark", typeof(string)));
            dt_RCD.Columns.Add(new DataColumn("TempNo", typeof(string)));
            dt_RCD.Columns.Add(new DataColumn("PRNo", typeof(string)));
            dt_RCD.Columns.Add(new DataColumn("RCNo", typeof(string)));
            dt_RCD.Columns.Add(new DataColumn("InvoiceNo", typeof(string)));
            dt_RCD.Columns.Add(new DataColumn("ID", typeof(int)));
            dt_RCD.Columns.Add(new DataColumn("PRID", typeof(int)));
            dt_RCD.Columns.Add(new DataColumn("Location", typeof(int)));


        }
        //int crow = 99;
        private void Unit_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                btnNew_Click(null, null);
                dgvData.AutoGenerateColumns = false;
                GETDTRow();

                DefaultItem();

                dgvData.Columns.ToList().ForEach(x =>
                {
                    if (x.Name == "S")
                        x.ReadOnly = false;
                    else if (x.Name == "Qty")
                        x.ReadOnly = false;
                    else if (x.Name == "TagCount")
                        x.ReadOnly = false;
                    else if (x.Name == "ProductionDate")
                        x.ReadOnly = false;
                    else
                        x.ReadOnly = true;
                });

                txtJobNo.Focus();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        private void DefaultItem()
        {

        }
        private void DataLoad()
        {
            try
            {

                this.Cursor = Cursors.WaitCursor;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {

                    try
                    {

                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
            catch { }
            finally { this.Cursor = Cursors.Default; }


            //    radGridView1.DataSource = dt;
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
            dgvData.Rows.Clear();
            dgvData.DataSource = null;
            txtJobNo.Text = "";
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearData();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }
        //private void Insert_Stock1()
        //{
        //    try
        //    {

        //        using (DataClasses1DataContext db = new DataClasses1DataContext())
        //        {
        //            DateTime? CalDate = null;
        //            DateTime? AppDate = DateTime.Now;
        //            int Seq = 0;
        //            string Type = "";
        //            if (rdoInvoice.IsChecked)
        //                Type = "รับด้วยใบ Invoice";
        //            else
        //                Type = "ใบส่งของชั่วคราว";

        //            decimal Cost = 0;


        //            string CNNo = CNNo = StockControl.dbClss.GetNo(6, 2);
        //            var g = (from ix in db.tb_Receives
        //                         //join i in db.tb_Items on ix.CodeNo equals i.CodeNo
        //                     where ix.RCNo.Trim() == txtRCNo.Text.Trim() && ix.Status != "Cancel"

        //                     select ix).ToList();
        //            if (g.Count > 0)
        //            {
        //                //insert Stock

        //                foreach (var vv in g)
        //                {
        //                    Seq += 1;

        //                    tb_Stock1 gg = new tb_Stock1();
        //                    gg.AppDate = AppDate;
        //                    gg.Seq = Seq;
        //                    gg.App = "Receive";
        //                    gg.Appid = Seq;
        //                    gg.CreateBy = ClassLib.Classlib.User;
        //                    gg.CreateDate = DateTime.Now;
        //                    gg.DocNo = CNNo;
        //                    gg.RefNo = txtRCNo.Text;
        //                    gg.Type = Type;
        //                    gg.QTY = Convert.ToDecimal(vv.QTY);
        //                    gg.Inbound = Convert.ToDecimal(vv.QTY);
        //                    gg.Outbound = 0;

        //                    if(rdoDL.IsChecked)
        //                    {
        //                        gg.UnitCost = 0;
        //                        gg.AmountCost = 0;
        //                    }
        //                    else
        //                    {
        //                        gg.AmountCost = Convert.ToDecimal(vv.QTY) * Convert.ToDecimal(vv.CostPerUnit);
        //                        gg.UnitCost = Convert.ToDecimal(vv.CostPerUnit);
        //                    }
        //                    gg.RemainQty = 0;
        //                    gg.RemainUnitCost = 0;
        //                    gg.RemainAmount = 0;
        //                    gg.CalDate = CalDate;
        //                    gg.Status = "Active";


        //                    db.tb_Stock1s.InsertOnSubmit(gg);
        //                    db.SubmitChanges();

        //                    //tb_Items inv,DL
        //                    dbClss.Insert_Stock(vv.CodeNo, Convert.ToDecimal(vv.QTY), "Receive", "Inv");
        //                    //tb_Items temp
        //                    dbClss.Insert_StockTemp(vv.CodeNo, (Convert.ToDecimal(vv.QTY)), "RC_Temp", "Inv");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex) { MessageBox.Show(ex.Message); }
        //}


        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                /*gvData.Rows[e.RowIndex].Cells["dgvC"].Value = "T";*/
                dgvData.EndEdit();
                if (e.RowIndex >= -1)
                {

                    if (dgvData.Columns["Qty"].Index == e.ColumnIndex)
                    {
                        decimal QTY = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["Qty"].Value), out QTY);
                        decimal RemainQty = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["RemainQty"].Value), out RemainQty);
                        if (QTY > RemainQty)
                        {
                            MessageBox.Show("ไม่สามารถรับเกินจำนวนคงเหลือได้");
                            e.Row.Cells["Qty"].Value = 0;
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
                    else if (dgvData.Columns["Unit"].Index == e.ColumnIndex)
                    {
                        string dgvUOM = dbClss.TSt(e.Row.Cells["Unit"].Value);
                        string CodeNo = dbClss.TSt(e.Row.Cells["CodeNo"].Value);
                        e.Row.Cells["PCSUnit"].Value = dbClss.Con_UOM(CodeNo, dgvUOM);
                        ////decimal PCSUOM = 0;//dbClss.Con_UOM(CodeNo, dgvUOM);
                        //using (DataClasses1DataContext db = new DataClasses1DataContext())
                        //{
                        //    var g = (from ix in db.mh_Items select ix)
                        //        .Where(a => a.InternalNo.ToUpper().Trim().Equals(CodeNo.ToUpper().Trim())).ToList();
                        //    if (g.Count > 0)
                        //    {
                        //        if (dgvUOM == dbClss.TSt(g.FirstOrDefault().ConsumptionUOM))
                        //        {
                        //            e.Row.Cells["PCSUnit"].Value = dbClss.Con_UOM(CodeNo, dgvUOM);
                        //            //e.Row.Cells["dgvPCSUnit"].ReadOnly = true;
                        //        }
                        //        else
                        //        {
                        //            e.Row.Cells["PCSUnit"].ReadOnly = false;
                        //            e.Row.Cells["PCSUnit"].Value = 0;
                        //        }
                        //    }
                        //    //else
                        //    //{
                        //    //    e.Row.Cells["PCSUnit"].ReadOnly = false;
                        //    //    e.Row.Cells["PCSUnit"].Value = 0;
                        //    //}
                        //}
                    }
                    if (dgvData.Columns["Qty"].Index == e.ColumnIndex
                        || dgvData.Columns["CostPerUnit"].Index == e.ColumnIndex
                        )
                    {
                        decimal QTY = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["Qty"].Value), out QTY);
                        decimal CostPerUnit = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["CostPerUnit"].Value), out CostPerUnit);
                        //decimal Rate = 1; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["Rate"].Value), out Rate);
                        //if (Rate <= 0)
                        //    Rate = 1;
                        //CostPerUnit = CostPerUnit * Rate;
                        e.Row.Cells["Amount"].Value = QTY * CostPerUnit;
                        Cal_Amount();
                    }

                }
            }
            catch (Exception ex) { }
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

        private void txtPRNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    AddJob(txtJobNo.Text);
                    txtJobNo.Text = "";
                    setRowNo();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        void AddJob(string JobNo)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    //var m = db.mh_ProductionOrders.Where(x => x.Active && x.OutQty > 0 && x.JobNo == JobNo).FirstOrDefault();
                    //if (m != null)
                    //{
                    //    var tool = db.mh_Items.Where(x => x.InternalNo == m.FGNo).FirstOrDefault();
                    //    var podt = db.mh_CustomerPODTs.Where(x => x.id == m.RefDocId).FirstOrDefault();
                    //    var pohd = db.mh_CustomerPOs.Where(x => x.id == podt.idCustomerPO).FirstOrDefault();

                    //    setRowNo();
                    //}
                    var m = db.sp_065_PrintTag_JobNo(JobNo).ToList();
                    foreach (var mm in m)
                    {
                        addRow(DateTime.Now, mm.ItemNo, mm.ItemName, mm.Qty, mm.UOM, mm.PCSUnit, mm.UnitPrice, mm.Amount
                            , mm.LotNo, mm.Location, mm.ShelfNo, mm.JobNo, mm.CustomerPONo, mm.idCustomerPO
                            , mm.idJob, mm.idCstmPODt, mm.Type, mm.GroupType, mm.InvGroup);
                    }
                    setRowNo();
                    txtJobNo.Text = "";
                    txtJobNo.Focus();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { this.Cursor = Cursors.Default; }
        }

        private void addRow(DateTime ProdDate, string itemNo, string itemName, decimal qty, string uOM, decimal pCSUnit, decimal unitPrice, decimal? amount
            , string lotNo, string location, string shelfNo, string jobNo, string customerPONo, int idCustomerPO, int idJob, int idCstmPODt
            , string Type, string GroupType, string InvGroup)
        {
            var rowe = dgvData.Rows.AddNew();
            rowe.Cells["ProductionDate"].Value = ProdDate;
            rowe.Cells["ItemNo"].Value = itemNo;
            rowe.Cells["ItemName"].Value = itemName;
            rowe.Cells["Qty"].Value = qty;
            rowe.Cells["UOM"].Value = uOM;
            rowe.Cells["PCSUnit"].Value = pCSUnit;
            rowe.Cells["UnitPrice"].Value = unitPrice;
            rowe.Cells["Amount"].Value = amount;
            rowe.Cells["LotNo"].Value = lotNo;
            rowe.Cells["Location"].Value = location;
            rowe.Cells["ShelfNo"].Value = shelfNo;
            rowe.Cells["JobNo"].Value = jobNo;
            rowe.Cells["CustomerPONo"].Value = customerPONo;
            rowe.Cells["idCustomerPO"].Value = idCustomerPO;
            rowe.Cells["idJob"].Value = idJob;
            rowe.Cells["idCstmPODt"].Value = idCstmPODt;
            rowe.Cells["Type"].Value = Type;
            rowe.Cells["GroupType"].Value = GroupType;
            rowe.Cells["InvGroup"].Value = InvGroup;
            rowe.Cells["TagCount"].Value = 1;
        }

        void setRowNo()
        {
            foreach (var item in dgvData.Rows)
            {
                item.Cells["dgvNo"].Value = item.Index + 1;
            }
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
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void btnListITem_Click(object sender, EventArgs e)
        {


        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (dgvData.Rows.Where(x => x.Cells["S"].Value.ToBool()).Count() > 0)
                {
                    var rowlist = dgvData.Rows.Where(x => x.Cells["S"].Value.ToBool()).ToList();
                    
                    if(rowlist.Where(x=>x.Cells["TagCount"].Value.ToDecimal() <= 0).Count() > 0)
                    {
                        baseClass.Warning("Tag cannot less than 1.");
                        return;
                    }

                    using (var db = new DataClasses1DataContext())
                    {
                        var l = db.mh_ProductTAGs.Where(x => x.UserID == ClassLib.Classlib.User).ToList();
                        db.mh_ProductTAGs.DeleteAllOnSubmit(l);
                        db.SubmitChanges();
                        //
                        int rNo = 0;
                        foreach (var row in rowlist)
                        {
                            string JobNo = row.Cells["JobNo"].Value.ToSt();
                            decimal Qty = row.Cells["Qty"].Value.ToDecimal();
                            string printDate = DateTime.Now.ToString("ddMMyyyy");
                            int TagCount = row.Cells["TagCount"].Value.ToInt();
                            decimal q1 = Math.Round(Qty / TagCount, 0);
                            decimal q2 = Qty - Math.Round(q1 * (TagCount - 1), 0);
                            var temp_tagCount = TagCount;
                            int ofTag = 0;
                            while (temp_tagCount > 0)
                            {
                                ofTag++;
                                rNo++;
                                bool last = temp_tagCount == 1;

                                string qr = $"{JobNo},{((last) ? q2 : q1)},{printDate},{ofTag}";
                                byte[] qrCode = dbClss.SaveQRCode2D(qr);
                                
                                var m = new mh_ProductTAG
                                {
                                    UserID = ClassLib.Classlib.User,
                                    PartNo = row.Cells["ItemNo"].Value.ToSt(),
                                    Seq = rNo,
                                    PartName = row.Cells["ItemName"].Value.ToSt(),
                                    LotNo = row.Cells["LotNo"].Value.ToSt(),
                                    Qty = (last) ? q2 : q1,
                                    QRCode = qrCode,
                                    Machine = "",
                                    BOMNo = "",
                                    OFTAG = ofTag.ToSt(),
                                };
                                db.mh_ProductTAGs.InsertOnSubmit(m);
                                temp_tagCount--;
                            }
                            db.SubmitChanges();

                            //Save Production Date
                            DateTime ProductionDate = row.Cells["ProductionDate"].Value.ToDateTime().Value.Date;
                            var p = db.mh_ProductionOrders.Where(x => x.JobNo == JobNo).FirstOrDefault();
                            if(p != null)
                            {
                                p.ProductionDate = ProductionDate;
                                db.SubmitChanges();
                            }
                        }

                        if(rNo > 0)
                        {
                            Report.Reportx1.Value = new string[2];
                            Report.Reportx1.Value[0] = ""; //BomNo
                            Report.Reportx1.Value[1] = ClassLib.Classlib.User; //USERID
                            Report.Reportx1.WReport = "TagFG";
                            Report.Reportx1 op = new Report.Reportx1("FG_TAG.rpt");
                            op.Show();
                        }
                    }
                }
                else
                    baseClass.Warning("Please select Data.\n");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void dgvData_Click(object sender, EventArgs e)
        {

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
                    Comcol.DropDownWidth = 300;
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
                if (e.Column.Name.Equals("Unit"))
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

        private void btnExcel_Click(object sender, EventArgs e)
        {
            //PrintPR a = new PrintPR(txtPackingNo.Text, txtPackingNo.Text, "ReceiveMonth");
            //a.ShowDialog();
        }

        private void MasterTemplate_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {

            RadMultiColumnComboBoxElement mccbEl = e.ActiveEditor as RadMultiColumnComboBoxElement;
            if (mccbEl != null)
            {
                mccbEl.DropDownSizingMode = SizingMode.UpDownAndRightBottom;
                mccbEl.DropDownMinSize = new Size(300, 200);
                mccbEl.DropDownMaxSize = new Size(300, 200);

                mccbEl.AutoSizeDropDownToBestFit = false;
                mccbEl.DropDownAnimationEnabled = false;
                mccbEl.AutoFilter = true;
                FilterDescriptor filterDescriptor = new FilterDescriptor(mccbEl.DisplayMember, FilterOperator.Contains, string.Empty);
                mccbEl.EditorControl.MasterTemplate.FilterDescriptors.Add(filterDescriptor);
            }
        }

        private void MasterTemplate_EditorRequired(object sender, EditorRequiredEventArgs e)
        {
        }
        string ShelfNo_Edit = "";


        private void openPRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvData.Rows.Count <= 0)
                    return;

                string TempNo1 = dbClss.TSt(dgvData.CurrentRow.Cells["TempNo"].Value);

                if (dbClss.TSt(dgvData.CurrentRow.Cells["PRNo"].Value) != "") //&& ddlTypeReceive.Text =="PR")
                {
                    //string TEmpPR = "";
                    //using (DataClasses1DataContext db = new DataClasses1DataContext())
                    //{
                    //    var g = (from ix in db.tb_PurchaseRequests select ix)
                    //   .Where(a => a.PRNo == dbClss.TSt(dgvData.CurrentRow.Cells["PRNo"].Value)
                    //    && (a.Status != "Cancel")
                    //    ).ToList();
                    //    if (g.Count() > 0)
                    //    {
                    //        TEmpPR = dbClss.TSt(g.FirstOrDefault().TEMPNo);
                    //    }
                    //}
                    if (TempNo1 != "")
                    {
                        CreatePR op = new CreatePR(TempNo1);
                        op.ShowDialog();
                    }
                }
                else if (dbClss.TSt(dgvData.CurrentRow.Cells["PRNo"].Value) != "")// && ddlTypeReceive.Text == "PO")
                {
                    //string TEmpPR = "";
                    //using (DataClasses1DataContext db = new DataClasses1DataContext())
                    //{
                    //    var g = (from ix in db.tb_PurchaseOrders select ix)
                    //   .Where(a => a.PONo == dbClss.TSt(dgvData.CurrentRow.Cells["PRNo"].Value)
                    //    && (a.Status != "Cancel")
                    //    ).ToList();
                    //    if (g.Count() > 0)
                    //    {
                    //        TEmpPR = dbClss.TSt(g.FirstOrDefault().TempPNo);
                    //    }
                    //}
                    if (TempNo1 != "")
                    {
                        CreatePO op = new CreatePO(TempNo1);
                        op.ShowDialog();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnDeleteRow_Click(object sender, EventArgs e)
        {
            if (dgvData.CurrentCell != null)
            {
                dgvData.Rows.Remove(dgvData.CurrentCell.RowInfo);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            dbClss.ExportGridXlSX(dgvData);
        }
    }
}
