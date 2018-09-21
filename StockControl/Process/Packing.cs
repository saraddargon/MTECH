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
    public partial class Packing : Telerik.WinControls.UI.RadRibbonForm
    {
        public Packing()
        {
            InitializeComponent();
        }
        public Packing(string PackingNo)
        {
            InitializeComponent();
            txtPackingNo.Text = PackingNo;
        }
        string Ac = "";

        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            HistoryView hw = new HistoryView(this.Name, txtPackingNo.Text);
            this.Cursor = Cursors.Default;
            hw.ShowDialog();
        }

        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }

        //int crow = 99;
        private void Unit_Load(object sender, EventArgs e)
        {
            try
            {
                var pk = txtPackingNo.Text;
                this.Cursor = Cursors.WaitCursor;
                btnNew_Click(null, null);
                dgvData.AutoGenerateColumns = false;

                DefaultItem();

                if (pk != null)
                {
                    txtPackingNo.Text = pk;
                    DataLoad();
                }


                dgvData.Columns.ToList().ForEach(x =>
                {
                    x.ReadOnly = x.Name != "Qty";
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
                        string pk = txtPackingNo.Text.Trim();
                        var m = db.mh_Packings.Where(x => x.PackingNo == pk).FirstOrDefault();
                        if (m != null)
                        {
                            txtPackingNo.Text = m.PackingNo;
                            dtPackingDate.Value = m.PackingDate;
                            txtRemark.Text = m.Remark;

                            dgvData.DataSource = null;
                            dgvData.Rows.Clear();
                            var dt = db.mh_PackingDts.Where(x => x.Active && x.PackingNo == pk).ToList();
                            foreach (var item in dt)
                            {
                                addRow(item.id, item.ItemNo, item.ItemName, item.Qty, item.UOM, item.PCSUnit
                                    , item.UnitPrice, item.LotNo, item.Location, item.ShelfNo, item.Remark, item.RefNo
                                    , item.CustomerPONo, item.idJob, item.idCstmPODt);
                            }
                            setRowNo();

                            if (!m.Active)
                                lblStatus.Text = "Cancel";

                            btnSave.Enabled = false;
                            btnDelete.Enabled = true;
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
            catch { }
            finally { this.Cursor = Cursors.Default; }


            //    radGridView1.DataSource = dt;
        }

        private void ClearData()
        {
            txtInvoiceNo.Text = "";
            txtDLNo.Text = "";
            txtDLNo.Enabled = false;
            txtPackingNo.Text = "";
            //ddlTypeReceive.Text = "";
            dtPackingDate.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
            txtReceiveBy.Text = ClassLib.Classlib.User;
            txtReceiveDate.Text = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).ToString("dd/MMM/yyyy");
            txtRemark.Text = "";
            txtVendorName.Text = "";
            txtVendorNo.Text = "";
            txtJobNo.Text = "";
            rdoInvoice.IsChecked = true;
            dtInvoiceDate.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
            dgvData.Rows.Clear();
            dgvData.DataSource = null;
            txtidPk.Text = "";

            txtTotal.Text = "";
            txtJobNo.Focus();
        }
        private void Enable_Status(bool ss, string Condition)
        {
            if (Condition.Equals("-") || Condition.Equals("New"))
            {
                txtJobNo.Enabled = ss;
                //txtVendorName.Enabled = ss;
                //txtRCNo.Enabled = ss;
                txtRemark.Enabled = ss;
                //txtTempNo.Enabled = ss;
                dtPackingDate.Enabled = ss;
                dgvData.ReadOnly = false;
                rdoInvoice.Enabled = ss;
                rdoDL.Enabled = ss;
                //ddlTypeReceive.Enabled = ss;

            }
            else if (Condition.Equals("View"))
            {
                txtJobNo.Enabled = ss;
                //txtVendorName.Enabled = ss;
                //txtRCNo.Enabled = ss;
                txtRemark.Enabled = ss;
                //txtTempNo.Enabled = ss;
                dtPackingDate.Enabled = ss;
                dgvData.ReadOnly = false;
                rdoInvoice.Enabled = ss;
                rdoDL.Enabled = ss;
            }

            else if (Condition.Equals("Edit"))
            {
                txtJobNo.Enabled = ss;
                //txtVendorName.Enabled = ss;
                //txtRCNo.Enabled = ss;
                txtRemark.Enabled = ss;
                //txtTempNo.Enabled = ss;
                dtPackingDate.Enabled = ss;
                dgvData.ReadOnly = false;
                rdoInvoice.Enabled = ss;
                rdoDL.Enabled = ss;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            btnDelete.Enabled = false;
            ClearData();
            Enable_Status(true, "New");
            lblStatus.Text = "New";
            Ac = "New";

            ////getมาไว้ก่อน แต่ยังไมได้ save 
            //txtPackingNo.Text = StockControl.dbClss.GetNo(4, 0);
        }

        //SaveE
        private bool ChkSaveE()
        {
            bool re = true;
            string err = "";
            try
            {
                //if (txtVendorNo.Text.Equals(""))
                //    err += "- “รหัสผู้ขาย:” เป็นค่าว่าง \n";
                if (dtPackingDate.Text.Equals(""))
                    err += "- “PackingDate:” is empty. \n";

                if (dgvData.Rows.Count <= 0)
                    err += "- “Data item:” is empty \n";
                var idCal = new List<int>();
                foreach (GridViewRowInfo r in dgvData.Rows)
                {
                    string JobNo = r.Cells["RefNo"].Value.ToSt();
                    if (r.Cells["Qty"].Value.ToDecimal() <= 0)
                    {
                        err += $"- “{JobNo}:” Receive Q'ty is 0 \n";
                        break;
                    }

                    int idJob = r.Cells["idJob"].Value.ToInt();
                    var qty = dgvData.Rows.Where(x => x.Cells["idJob"].Value.ToInt() == idJob).Sum(x => x.Cells["Qty"].Value.ToDecimal());
                    var outQty = r.Cells["OutQty"].Value.ToDecimal();
                    if (qty > outQty)
                    {
                        err += $"- “{JobNo}:” Receive Q'ty more than Out Q'ty \n";
                        break;
                    }
                }


                if (!err.Equals(""))
                    baseClass.Warning(err);
                else
                    re = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("Packing", ex.Message, this.Name);
            }

            return re;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Ac.Equals("New"))// || Ac.Equals("Edit"))
                {
                    if (ChkSaveE())
                        return;

                    if (MessageBox.Show("Do you want to 'Save' ?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        SaveE();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        void SaveE()
        {
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    string PKNo = (txtidPk.Text.ToInt() > 0) ? txtPackingNo.Text.Trim() : "";
                    var m = db.mh_Packings.Where(x => x.PackingNo == PKNo).FirstOrDefault();
                    if (m == null)
                    {
                        m = new mh_Packing();
                        m.CreateBy = ClassLib.Classlib.User;
                        m.CreateDate = DateTime.Now;
                        m.PackingNo = dbClss.GetNo(32, 2);
                        db.mh_Packings.InsertOnSubmit(m);
                    }
                    m.Remark = txtRemark.Text;
                    m.Active = true;
                    m.UpdateDate = DateTime.Now;
                    m.UpdateBy = ClassLib.Classlib.User;
                    m.PackingDate = dtPackingDate.Value.Date;
                    db.SubmitChanges(); //Save Header

                    //DT
                    foreach (var item in dgvData.Rows)
                    {
                        bool newItem = false;
                        int id = item.Cells["id"].Value.ToInt();
                        int idJob = item.Cells["idJob"].Value.ToInt();
                        string jobNo = item.Cells["RefNo"].Value.ToSt();
                        var dt = db.mh_PackingDts.Where(x => x.id == id).FirstOrDefault();
                        if (dt == null)
                        {
                            dt = new mh_PackingDt();
                            db.mh_PackingDts.InsertOnSubmit(dt);
                            newItem = true;
                        }
                        dt.PackingNo = m.PackingNo;
                        dt.ItemNo = item.Cells["ItemNo"].Value.ToSt();
                        dt.ItemName = item.Cells["ItemName"].Value.ToSt();
                        dt.Qty = item.Cells["Qty"].Value.ToDecimal();
                        dt.UOM = item.Cells["UOM"].Value.ToSt();
                        dt.PCSUnit = item.Cells["PCSUnit"].Value.ToDecimal();
                        dt.Amount = item.Cells["Amount"].Value.ToDecimal();
                        //find Rem amount when Receive Qty == Out Qty
                        var prod = db.mh_ProductionOrders.Where(x => x.id == idJob).FirstOrDefault();
                        if (prod == null) continue;
                        if (dt.Qty == prod.OutQty)
                        {
                            var sumProd = db.mh_ProductionOrderRMs.Where(x => x.JobNo == jobNo && x.Active).Sum(x => x.CostOverall);
                            var costAll = prod.CostOverhead + sumProd; //Cost All in Job (Cost Overhead + RM Cost)
                            var costTaking = 0.00m;
                            var packall = db.mh_Packings.Where(x => x.Active)
                                        .Join(db.mh_PackingDts.Where(x => x.Active && x.idJob == idJob)
                                        , pk_hd => pk_hd.PackingNo
                                        , pk_dt => pk_dt.PackingNo
                                        , (pk_hd, pk_dt) => new { pk_hd, pk_dt }).ToList();
                            if (packall.Count() > 0)
                                costTaking = packall.Sum(x => x.pk_dt.Amount);
                            //costRem
                            dt.Amount = costAll - costTaking;
                        }
                        dt.UnitPrice = Math.Round(dt.Amount / dt.Qty, 2);

                        dt.LotNo = item.Cells["LotNo"].Value.ToSt();
                        dt.ShelfNo = item.Cells["ShelfNo"].Value.ToSt();
                        dt.Remark = item.Cells["Remark"].Value.ToSt();
                        dt.RefNo = jobNo;
                        dt.CustomerPONo = item.Cells["CustomerPONo"].Value.ToSt();
                        dt.idJob = idJob;
                        dt.idCstmPODt = item.Cells["idCstmPODt"].Value.ToInt();
                        dt.Active = true;

                        db.SubmitChanges(); //Save Detail

                        //update OutQty -- Job
                        var job = db.mh_ProductionOrders.Where(x => x.id == idJob).FirstOrDefault();
                        if (job != null)
                        {
                            job.OutQty -= dt.Qty;
                            if (job.OutQty < 0)
                                job.OutQty = 0;
                        }
                        //Stock
                        if (newItem)
                        {
                            var s = new tb_Stock();
                            s.AppDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                            s.Seq = item.Index + 1;
                            s.App = "Receive";
                            s.Appid = item.Index + 1;
                            s.CreateBy = ClassLib.Classlib.User;
                            s.CreateDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
                            s.DocNo = txtPackingNo.Text;
                            s.RefNo = job.JobNo; //JobNo --> mh_ProductionOrder
                            s.CodeNo = item.Cells["ItemNo"].Value.ToSt();
                            s.Type = "Receive By Job";
                            s.QTY = Math.Round(item.Cells["Qty"].Value.ToDecimal() * item.Cells["PCSUnit"].Value.ToDecimal(), 2);
                            s.Inbound = s.QTY;
                            s.Outbound = 0;
                            s.Type_i = 1;
                            s.Category = "Invoice";
                            s.Refid = dt.id; //id mh_PackingDt
                            s.Type_in_out = "In";
                            s.AmountCost = dt.Amount;
                            if (s.AmountCost > 0)
                                s.UnitCost = Math.Round(s.QTY.ToDecimal() / s.AmountCost.ToDecimal(), 2);
                            else
                                s.UnitCost = 0;

                            decimal RemainQty = (Convert.ToDecimal(db.Cal_QTY_Remain_Location(s.CodeNo, "", 0, "Warehouse")));
                            decimal sum_Remain = Convert.ToDecimal(dbClss.Get_Stock(s.CodeNo, "", "", "RemainAmount", "Warehouse")) + s.AmountCost.ToDecimal();
                            decimal sum_Qty = RemainQty.ToDecimal() + s.QTY.ToDecimal();
                            var RemainAmount = sum_Remain;
                            decimal RemainUnitCost = 0.00m;
                            if (sum_Qty <= 0)
                                RemainUnitCost = 0;
                            else
                                RemainUnitCost = Math.Round((Math.Abs(RemainAmount) / Math.Abs(sum_Qty)), 2);
                            s.RemainQty = sum_Qty;
                            s.RemainUnitCost = RemainUnitCost;
                            s.RemainAmount = RemainAmount;
                            s.Avg = 0;
                            s.CalDate = null;
                            s.Status = "Active";
                            s.Flag_ClearTemp = 0;
                            s.TLCost = s.AmountCost;
                            s.TLQty = s.QTY;
                            s.ShipQty = 0;
                            s.Location = "Warehouse";
                            s.ShelfNo = item.Cells["ShelfNo"].Value.ToSt();
                            //ต้องไม่ใช่ Item ที่มีในระบบ
                            var c = (from ix in db.mh_Items
                                     where ix.InternalNo.Trim().ToUpper() == s.CodeNo.Trim().ToUpper() && ix.Active
                                     select ix).ToList();
                            if (c.Count <= 0)
                            {
                                s.TLQty = 0;
                                s.ShipQty = s.QTY;
                            }

                            db.tb_Stocks.InsertOnSubmit(s);
                            db.SubmitChanges();

                            //update Stock เข้า item
                            db.sp_010_Update_StockItem(Convert.ToString(s.CodeNo), "");
                        }

                        db.SubmitChanges();
                    }

                    ClearData();
                    txtPackingNo.Text = m.PackingNo;
                    DataLoad();
                    baseClass.Info("Save complete.\n");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //Begin Edit ------ End Edit
        decimal beginQty = 0.00m;
        private void MasterTemplate_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            try
            {
                beginQty = 0.00m;
                if (e.Row.Cells["OutQty"].Value.ToDecimal() <= 0)
                    e.Cancel = true;
                else if (e.Column.Name.Equals("Qty"))
                {
                    beginQty = e.Row.Cells["Qty"].Value.ToDecimal();
                }
            }
            catch { }

        }
        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                /*gvData.Rows[e.RowIndex].Cells["dgvC"].Value = "T";*/
                dgvData.EndEdit();
                if (e.RowIndex >= -1)
                {
                    if (e.Column.Name.Equals("Qty"))
                    {
                        if (e.Row.Cells["Qty"].Value.ToDecimal() > e.Row.Cells["OutQty"].Value.ToDecimal())
                        {
                            baseClass.Warning("- Cannot Receive Q'ty > Out Qty.\n");
                            e.Row.Cells["Qty"].Value = beginQty;
                        }
                    }
                    calAmnt();
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


        private void radLabel5_Click(object sender, EventArgs e)
        {

        }

        private void radTextBox2_TextChanged(object sender, EventArgs e)
        {

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
                    txtJobNo.Focus();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        void AddJob(string JobNo)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //if (dgvData.Rows.Where(x => x.Cells["RefNo"].Value.ToSt().Equals(JobNo)).Count() > 0)
                //{
                //    baseClass.Warning($"- Job no {JobNo} is already in list.\n");
                //    return;
                //}

                using (var db = new DataClasses1DataContext())
                {
                    var m = db.mh_ProductionOrders.Where(x => x.Active && x.JobNo == JobNo).FirstOrDefault();
                    if (m != null)
                    {
                        if (m.SeqStatus != 2)
                        {
                            baseClass.Warning($"- Job no {JobNo} status not Approved.\n");
                            return;
                        }
                        if (m.OutQty <= 0)
                        {
                            baseClass.Warning($"- Job no {JobNo} is already Completed.\n");
                            return;
                        }

                        var tool = db.mh_Items.Where(x => x.InternalNo == m.FGNo).FirstOrDefault();
                        var podt = db.mh_CustomerPODTs.Where(x => x.id == m.RefDocId).FirstOrDefault();
                        var pohd = db.mh_CustomerPOs.Where(x => x.id == podt.idCustomerPO).FirstOrDefault();

                        var costAll = m.CostOverhead;
                        var dt = db.mh_ProductionOrderRMs.Where(x => x.JobNo == m.JobNo && x.Active).ToList();
                        costAll += dt.Sum(x => x.CostOverall);
                        var costPer = Math.Round(costAll / m.Qty, 2);

                        addRow(0, m.FGNo, m.FGName, 0, m.UOM, m.PCSUnit, costPer
                            , m.LotNo, "Warehouse", tool.ShelfNo, "", JobNo, pohd.CustomerPONo
                            , m.id, m.RefDocId);
                        setRowNo();
                        calAmnt();
                    }
                    else
                        baseClass.Warning($"- Job no {JobNo} not found.\n");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { this.Cursor = Cursors.Default; }
        }
        private void addRow(int id, string itemNo, string itemName, decimal qty, string uOM, decimal pCSUnit
            , decimal costPer, string lotNo, string LocationItem, string shelfNo, string Remark
            , string jobNo, string customerPONo, int idJob, int idCstmPOdt)
        {
            var row = dgvData.Rows.AddNew();
            row.Cells["id"].Value = id;
            row.Cells["ItemNo"].Value = itemNo;
            row.Cells["ItemName"].Value = itemName;
            row.Cells["Qty"].Value = qty;
            row.Cells["UOM"].Value = uOM;
            row.Cells["PCSUnit"].Value = pCSUnit;
            row.Cells["UnitPrice"].Value = costPer;
            row.Cells["Amount"].Value = Math.Round(costPer * qty, 2);
            row.Cells["LotNo"].Value = lotNo;
            row.Cells["Location"].Value = LocationItem;
            row.Cells["ShelfNo"].Value = shelfNo;
            row.Cells["Remark"].Value = Remark;
            row.Cells["RefNo"].Value = jobNo;
            row.Cells["CustomerPONo"].Value = customerPONo;
            row.Cells["idJob"].Value = idJob;
            row.Cells["idCstmPOdt"].Value = idCstmPOdt;

            //find OutQty
            using (var db = new DataClasses1DataContext())
            {
                var m = db.mh_ProductionOrders.Where(x => x.id == idJob).FirstOrDefault();
                var outQty = qty;
                if (m != null)
                    outQty = m.OutQty;

                row.Cells["OutQty"].Value = outQty;
            }

        }
        void setRowNo()
        {
            foreach (var item in dgvData.Rows)
            {
                item.Cells["dgvNo"].Value = item.Index + 1;
            }
        }
        void calAmnt()
        {
            var amnt = 0.00m;
            dgvData.Rows.ToList().ForEach(x =>
            {
                x.Cells["Amount"].Value = Math.Round(x.Cells["Qty"].Value.ToDecimal() * x.Cells["UnitPrice"].Value.ToDecimal(), 2);
                amnt += x.Cells["Amount"].Value.ToDecimal();
            });
            txtTotal.Text = amnt.ToDecimal().ToString("#,0.00");
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //btnEdit.Enabled = true;
            //btnView.Enabled = false;
            btnNew.Enabled = true;

            string PR = txtPackingNo.Text;
            ClearData();
            Enable_Status(false, "View");
            txtPackingNo.Text = PR;
            DataLoad();
            Ac = "View";
        }

        private void btnListITem_Click(object sender, EventArgs e)
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
                var sc = new Packing_List(2);
                this.Cursor = Cursors.Default;
                sc.ShowDialog();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                if(sc.t_PackingNo != "")
                {
                    txtPackingNo.Text = sc.t_PackingNo;
                    DataLoad();
                }

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError("CreatePart", ex.Message + " : radButtonElement1_Click", this.Name); }

        }

        private void rdoDL_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (rdoDL.IsChecked)
            {
                txtDLNo.Enabled = true;
                txtInvoiceNo.Enabled = false;
            }
        }

        private void rdoInvoice_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (rdoInvoice.IsChecked)
            {
                txtDLNo.Enabled = false;
                txtInvoiceNo.Enabled = true;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                PrintPR a = new PrintPR(txtPackingNo.Text, txtPackingNo.Text, "Receive");
                a.ShowDialog();

                //using (DataClasses1DataContext db = new DataClasses1DataContext())
                //{
                //    var g = (from ix in db.sp_R003_ReportReceive(txtRCNo.Text, DateTime.Now) select ix).ToList();
                //    if (g.Count() > 0)
                //    {

                //        Report.Reportx1.Value = new string[2];
                //        Report.Reportx1.Value[0] = txtRCNo.Text;
                //        Report.Reportx1.WReport = "ReportReceive";
                //        Report.Reportx1 op = new Report.Reportx1("ReportReceive.rpt");
                //        op.Show();

                //    }
                //    else
                //        MessageBox.Show("not found.");
                //}

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void dgvData_Click(object sender, EventArgs e)
        {

        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            PrintPR a = new PrintPR(txtPackingNo.Text, txtPackingNo.Text, "ReceiveMonth");
            a.ShowDialog();
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
            GridViewEditManager manager = sender as GridViewEditManager;
            // Assigning DropDownListAddEditor to the right column

            if (manager.GridViewElement.CurrentColumn.Name == "ShelfNo")
            {
                DropDownListAddEditor editor = new DropDownListAddEditor();
                editor.InputValueNotFound += new DropDownListAddEditor.InputValueNotFoundHandler(DropDownListAddEditor_InputValueNotFoundCategory_Edit);
                e.Editor = editor;
            }
        }
        string ShelfNo_Edit = "";
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


        private void btnDeleteRow_Click(object sender, EventArgs e)
        {
            if (dgvData.CurrentCell != null)
            {
                if (dgvData.CurrentCell.RowInfo.Cells["id"].Value.ToInt() == 0)
                {
                    dgvData.Rows.Remove(dgvData.CurrentCell.RowInfo);
                    setRowNo();
                    calAmnt();
                }
                else
                    baseClass.Error("Status Cannot Delete.\n");
            }
        }

        bool chkDelE()
        {
            bool ret = true;
            string mssg = "";

            using (var db = new DataClasses1DataContext())
            {
                foreach (var item in dgvData.Rows)
                {
                    int id = item.Cells["id"].Value.ToInt();
                    //var m = db.tb_Stocks.Where(x=>x.Refid == id && x.RefNo == txtPackingNo.Text).tol)
                }
            }

            if(mssg != "")
            {
                ret = false;
                baseClass.Warning(mssg);
            }
            return ret;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (chkDelE() && baseClass.Question("Do you want to 'Delete' ?"))
                DeleteE();
        }
        void DeleteE()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {

            }
            catch (Exception ex)
            {
                baseClass.Error(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void txtPackingNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string pk = txtPackingNo.Text;
                ClearData();
                txtPackingNo.Text = pk;
                DataLoad();
            }
        }

        private void lblStatus_TextChanged(object sender, EventArgs e)
        {
            if (lblStatus.Text == "Cancel")
                lblStatus.ForeColor = Color.Red;
            else
                lblStatus.ForeColor = Color.FromArgb(0, 192, 0);
        }
    }
}
