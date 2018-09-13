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
using ClassLib;

namespace StockControl
{
    public partial class ProductionOrder : Telerik.WinControls.UI.RadRibbonForm
    {
        string t_JobNo = "";

        bool demo = false;
        public ProductionOrder()
        {
            InitializeComponent();
        }
        public ProductionOrder(string JobNo)
        {
            InitializeComponent();
            this.t_JobNo = JobNo;
        }

        List<GridViewRowInfo> RetDT;
        string TempNo_temp = "";
        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            HistoryView hw = new HistoryView(this.Name, "txtPONo.Text");
            this.Cursor = Cursors.Default;
            hw.ShowDialog();
        }

        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }
        private void GETDTRow()
        {

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

                LoadDefault();
                var m = t_JobNo;
                ClearData();
                btnNew_Click(null, null);
                t_JobNo = m;

                if (t_JobNo != "")
                    DataLoad();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void LoadDefault()
        {
            using (var db = new DataClasses1DataContext())
            {

            }
        }

        private void DataLoad(bool warningMssg = true)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int ck = 0;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var t = db.mh_ProductionOrders.Where(x => x.Active && x.JobNo == t_JobNo).FirstOrDefault();
                    if (t != null)
                    {
                        txtJobNo.Text = t.JobNo;
                        txtRefDocNo.Text = t.RefDocNo;
                        txtRefDocId.Text = t.RefDocId.ToSt();
                        txtFGNo.Text = t.FGNo;
                        txtFGName.Text = t.FGName;
                        txtStartingDate.Text = t.StartingDate.ToDtTimeString();
                        txtEndingDate.Text = t.EndingDate.ToDtTimeString();
                        txtReqDate.Text = t.ReqDate.ToDtString();
                        txtUOM.Text = t.UOM;
                        txtPCSUnit.Value = t.PCSUnit;
                        txtOutQty.Value = t.OutQty;
                        txtCreateDate.Text = t.CreateDate.ToDtString();
                        txtCreateBy.Text = t.CreateBy;
                        txtLotNo.Text = t.LotNo;

                        //dt
                        var dts = db.mh_ProductionOrderRMs.Where(x => x.JobNo == t.JobNo && x.Active).ToList();
                        foreach (var dt in dts)
                        {
                            addRow(dt.id, dt.ItemNo, dt.ItemName, dt.Qty, dt.UOM, dt.PCSUnit
                                , dt.RemQty, dt.GroupType, dt.Type, dt.InvGroup);
                        }

                        SetRowNo1(dgvData);
                        btnView_Click(null, null);
                    }
                    else if (warningMssg)
                        baseClass.Warning("Job Orders not found.!!");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void addRow(int id, string itemNo, string itemName, decimal qty, string uOM, decimal pCSUnit
            , decimal remQty, string groupType, string type, string invGroup)
        {
            var rowe = dgvData.Rows.AddNew();
            rowe.Cells["RNo"].Value = rowe.Index + 1;
            rowe.Cells["id"].Value = id;
            rowe.Cells["ItemNo"].Value = itemNo;
            rowe.Cells["ItemName"].Value = itemName;
            rowe.Cells["Qty"].Value = qty;
            rowe.Cells["UOM"].Value = uOM;
            rowe.Cells["PCSUnit"].Value = pCSUnit;
            rowe.Cells["RemQty"].Value = remQty;
            rowe.Cells["GroupType"].Value = groupType;
            rowe.Cells["Type"].Value = type;
            rowe.Cells["InvGroup"].Value = invGroup;
            rowe.Cells["dgvC"].Value = "T";
        }

        //

        private void Enable_Status(bool ss, string Condition)
        {
            if (Condition.Equals("-") || Condition.Equals("New"))
            {
            }
            else if (Condition.Equals("View"))
            {
            }
            else if (Condition.Equals("Edit"))
            {
            }
        }

        private void ClearData()
        {
            txtJobNo.Text = "";
            txtRefDocId.Text = "0";
            txtRefDocNo.Text = "";
            txtFGNo.Text = "";
            txtFGName.Text = "";
            txtReqDate.Text = "";
            txtFGQty.Value = 0;
            txtUOM.Text = "";
            txtPCSUnit.Value = 0;
            txtStartingDate.Text = "";
            txtEndingDate.Text = "";
            txtOutQty.Value = 0;
            txtLotNo.Text = "";
            txtCreateBy.Text = Classlib.User;
            txtCreateDate.Text = DateTime.Now.ToDtString();

            using (var db = new DataClasses1DataContext())
            {
                var d = DateTime.Now.Date;
                var m = db.mh_LotFGs.Where(x => x.LotDate == d).FirstOrDefault();
                if (m != null)
                    txtLotNo.Text = m.LotNo;
            }
            //

            dgvData.DataSource = null;
            dgvData.Rows.Clear();

            t_JobNo = "";
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            btnView.Enabled = true;
            btnEdit.Enabled = true;
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            btnDelete.Enabled = true;

            btnAdd_Row.Enabled = true;
            btnDel_Item.Enabled = true;
            btnAddPart.Enabled = true;

            dgvData.ReadOnly = false;

            ClearData();
            Enable_Status(true, "New");
            lblStatus.Text = "New";
            Ac = "New";
            row = dgvData.Rows.Count - 1;
            if (row < 0)
                row = 0;

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            btnView.Enabled = false;
            btnEdit.Enabled = true;
            btnNew.Enabled = true;
            btnSave.Enabled = true;

            btnAdd_Row.Enabled = false;
            btnDel_Item.Enabled = false;
            btnAddPart.Enabled = false;

            dgvData.ReadOnly = true;

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

            btnAdd_Row.Enabled = true;
            btnDel_Item.Enabled = true;
            btnAddPart.Enabled = true;

            dgvData.ReadOnly = false;

            Enable_Status(true, "Edit");
            lblStatus.Text = "Edit";
            Ac = "Edit";

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtFGNo.Text == "") return;
            if (Math.Round(txtFGQty.Value.ToDecimal() * txtPCSUnit.Value.ToDecimal(), 2) != txtOutQty.Value.ToDecimal())
            {
                baseClass.Warning("Status Process cannot Delete.\n");
                return;
            }
            if (dgvData.Rows.Where(x => x.Cells["Qty"].Value.ToDecimal() 
                * x.Cells["PCSUnit"].Value.ToDecimal() != x.Cells["RemQty"].Value.ToDecimal()).Count() > 0)
            {
                baseClass.Warning("RM or SEMI alreday shipped into this 'Job', Please cancel shipping RM.\n");
                return;
            }

            if (baseClass.IsDel("Do you want to 'Delete' ?"))
                DeleteE();
        }
        void DeleteE()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                string jobNo = txtJobNo.Text;
                using (var db = new DataClasses1DataContext())
                {
                    var m = db.mh_ProductionOrders.Where(x => x.JobNo == jobNo && x.Active).FirstOrDefault();
                    if(m != null)
                    {
                        m.Active = false;
                        m.UpdateDate = DateTime.Now;
                        m.UpdateBy = ClassLib.Classlib.User;

                        var po = db.mh_CustomerPODTs.Where(x => x.id == m.RefDocId).FirstOrDefault();
                        if(po != null)
                        {
                            //po.OutPlan += (m.Qty * m.PCSUnit) - m.OutQty;
                            po.OutPlan = Math.Round(m.Qty * m.PCSUnit, 3); //Full Return Qty
                            po.Status = baseClass.setCustomerPOStatus(po);
                            db.SubmitChanges();

                            baseClass.Info("Delete complete.\n");
                        }
                    }
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

                if (!err.Equals(""))
                    MessageBox.Show(err);
                else
                    re = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("CreatePO", ex.Message, this.Name);
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
                    else if (baseClass.IsSave())
                        SaveE();
                }
                else
                    MessageBox.Show("Can save only status 'New' or 'Edit'");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        void SaveE()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    string jobNo = txtJobNo.Text;
                    bool newJob = false;
                    //Hd
                    var m = db.mh_ProductionOrders.Where(x => x.JobNo == jobNo).FirstOrDefault();
                    if (m == null)
                    {
                        m = new mh_ProductionOrder();
                        m.CreateBy = ClassLib.Classlib.User;
                        m.CreateDate = DateTime.Now;
                        m.JobDate = DateTime.Now.Date;
                        m.JobNo = dbClss.GetNo(29, 2);
                        db.mh_ProductionOrders.InsertOnSubmit(m);
                        newJob = true;
                    }
                    m.Active = true;
                    m.EndingDate = txtEndingDate.Text.ToDateTime().Value;
                    m.FGName = txtFGName.Text;
                    m.FGNo = txtFGNo.Text;
                    m.LotNo = txtLotNo.Text;
                    m.Qty = txtFGQty.Value.ToDecimal();
                    m.PCSUnit = txtPCSUnit.Value.ToDecimal();
                    m.OutQty = m.Qty * m.PCSUnit;
                    m.RefDocId = txtRefDocId.Text.ToInt();
                    m.RefDocNo = txtRefDocNo.Text;
                    m.ReqDate = txtReqDate.Text.ToDateTime().Value;
                    m.StartingDate = txtStartingDate.Text.ToDateTime().Value;
                    m.UOM = txtUOM.Text;
                    m.UpdateBy = ClassLib.Classlib.User;
                    m.UpdateDate = DateTime.Now;

                    //update Customer P/O [Only New Job] - Out Plan
                    if (newJob)
                    {
                        var po = db.mh_CustomerPODTs.Where(x => x.id == m.RefDocId).FirstOrDefault();
                        if (po != null)
                        {
                            //po.OutPlan -= m.OutQty;
                            po.OutPlan = 0;//Full Ref Customer P/O
                            po.Status = baseClass.setCustomerPOStatus(po);
                            db.SubmitChanges();
                        }
                    }

                    t_JobNo = m.JobNo;
                    //dt
                    foreach (var item in dgvData.Rows)
                    {
                        int id = item.Cells["id"].Value.ToInt();
                        var d = db.mh_ProductionOrderRMs.Where(x => x.id == id).FirstOrDefault();
                        if (d == null)
                        {
                            d = new mh_ProductionOrderRM();
                            db.mh_ProductionOrderRMs.InsertOnSubmit(d);
                        }
                        d.Active = true;
                        d.ItemNo = item.Cells["ItemNo"].Value.ToSt();
                        d.ItemName = item.Cells["ItemName"].Value.ToSt();
                        d.Qty = item.Cells["Qty"].Value.ToDecimal();
                        d.UOM = item.Cells["UOM"].Value.ToSt();
                        d.PCSUnit = item.Cells["PCSUnit"].Value.ToDecimal();
                        d.RemQty = item.Cells["RemQty"].Value.ToDecimal();
                        d.GroupType = item.Cells["GroupType"].Value.ToSt();
                        d.Type = item.Cells["Type"].Value.ToSt();
                        d.InvGroup = item.Cells["InvGroup"].Value.ToSt();
                        db.SubmitChanges();
                    }
                }

                var docno = t_JobNo;
                baseClass.Info("Save complete(s).");
                ClearData();
                t_JobNo = docno;
                DataLoad();
            }
            catch (Exception ex)
            {
                baseClass.Error(ex.Message);
            }
            finally { this.Cursor = Cursors.Default; }
        }

        string beginItem = "";
        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                dgvData.EndEdit();
                if (e.RowIndex >= -1)
                {
                    var itemNo = e.Row.Cells["Item"].Value.ToSt();
                    if (e.Column.Name.Equals("Amount") || e.Column.Name.Equals("Qty"))
                    {
                        if (e.Row.Cells["Qty"].Value.ToDecimal() > 0)
                        {
                            var m = Math.Round(e.Row.Cells["Amount"].Value.ToDecimal() / e.Row.Cells["Qty"].Value.ToDecimal(), 2);
                            dgvData.Rows[e.RowIndex].Cells["PricePerUnit"].Value = m;
                        }
                        else
                            dgvData.Rows[e.RowIndex].Cells["PricePerUnit"].Value = 0;

                    }
                    else if (e.Column.Name.Equals("Item"))
                    {
                        using (var db = new DataClasses1DataContext())
                        {
                            var t = db.mh_Items.Where(x => x.InternalNo.Equals(itemNo)).FirstOrDefault();
                            if (t == null)
                            {
                                baseClass.Warning($"Item no. ({itemNo}) not found.!!");
                                e.Row.Cells["Item"].Value = beginItem;
                                return;
                            }
                            var tU = db.mh_ItemUOMs.Where(x => x.ItemNo == t.InternalNo && x.UOMCode == t.BaseUOM).FirstOrDefault();
                            var pcsunit = (tU != null) ? tU.QuantityPer : 1;

                            //set Tool
                            if (beginItem == "")
                            {
                                //addRow(e.RowIndex
                                //    , DateTime.Now, t.InternalNo, t.InternalName
                                //    , 1, t.BaseUOM, pcsunit, 0, 0
                                //    , 1, 1, "", 0, "Waiting");
                            }
                            else
                            {
                                e.Row.Cells["ItemName"].Value = t.InternalName;
                                e.Row.Cells["Unit"].Value = t.BaseUOM;
                                e.Row.Cells["PCSUnit"].Value = pcsunit;
                            }

                            //
                            SetRowNo1(dgvData);
                        }
                    }
                    else if (e.Column.Name.Equals("Unit"))
                    {
                        var unit = e.Row.Cells["Unit"].Value.ToSt();
                        using (var db = new DataClasses1DataContext())
                        {
                            var u = db.mh_ItemUOMs.Where(x => x.ItemNo == itemNo && x.UOMCode == unit).FirstOrDefault();
                            var pcsunit = (u != null) ? u.QuantityPer : 1;

                            e.Row.Cells["PCSUnit"].Value = pcsunit;
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void MasterTemplate_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            if (e.RowIndex >= -1)
            {
                string itemNo = e.Row.Cells["Item"].Value.ToSt();
                if (e.Column.Name.Equals("Unit"))
                {
                    using (var db = new DataClasses1DataContext())
                    {
                        var unit = db.mh_ItemUOMs.Where(x => x.ItemNo == itemNo).ToList();
                        unit = unit.Where(x => x.Active.ToBool()).ToList();
                        var c1 = dgvData.Columns["Unit"] as GridViewComboBoxColumn;
                        c1.ValueMember = "UOMCode";
                        c1.DisplayMember = "UOMCode";
                        c1.DataSource = unit;
                    }
                }
                else if (e.Column.Name.Equals("Item"))
                {
                    beginItem = itemNo;
                }
            }
        }




        private void radGridView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // MessageBox.Show(e.KeyCode.ToString());

            //if (e.KeyData == (Keys.Control | Keys.S))
            //{
            //    btnSave_Click(null, null);
            //}
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

        private void radPanel1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void ลบพารทToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                //if (dgvData.Rows.Count < 0)
                //    return;


                //if (Ac.Equals("New") || Ac.Equals("Edit"))
                //{
                //    this.Cursor = Cursors.WaitCursor;


                //    if (dgvData.CurrentRow.Cells["Status"].Value.ToSt() == "Waiting")
                //    {

                //        int id = 0;
                //        int.TryParse(StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["id"].Value), out id);
                //        if (id <= 0)
                //            dgvData.Rows.Remove(dgvData.CurrentRow);

                //        else
                //        {
                //            row = dgvData.CurrentRow.Index;
                //            btnDelete_Click(null, null);
                //        }
                //        CallTotal();
                //        //getTotal();
                //        SetRowNo1(dgvData);
                //    }
                //    else
                //        MessageBox.Show("ไม่สามารถทำการลบรายการได้ สถานะไม่ถูกต้อง");
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
                o.Cells["RNo"].Value = i;
                i++;
            });
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
                var pd = new ProductionOrder_List();
                this.Cursor = Cursors.Default;
                pd.ShowDialog();
                //if (pol.PONo != "" && pol.CstmNo != "")
                //{
                //    t_PONo = pol.PONo;
                //    t_CustomerNo = pol.CstmNo;
                //    //LoadData
                //    DataLoad();
                //}


                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError("CreatePart", ex.Message + " : radButtonElement1_Click", this.Name); }
            finally { this.Cursor = Cursors.Default; }



        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            btnEdit.Enabled = true;
            btnView.Enabled = false;
            btnNew.Enabled = true;
            ClearData();
            Enable_Status(false, "View");
            DataLoad();
            Ac = "View";
        }


        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                PrintPR a = new PrintPR(txtJobNo.Text, txtJobNo.Text, "ReportProductionOrder");
                a.ShowDialog();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnAdd_Row_Click(object sender, EventArgs e)
        {
            try
            {
                dgvData.Rows.AddNew();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        


        private void CallTotal()
        {
            try
            {
                decimal amnt = 0.00m;
                foreach (var item in dgvData.Rows.Where(x => x.IsVisible))
                {
                    amnt += item.Cells["Amount"].Value.ToDecimal();
                }

            }
            catch (Exception ex) { MessageBox.Show("err2: " + ex.Message); }
        }


        private void btnAddPart_Click(object sender, EventArgs e)
        {
            AddPartE();
        }
        void AddPartE()
        {
            var m = new List<GridViewRowInfo>();
            var selP = new ListPart_CreatePR(m);
            selP.ShowDialog();
            if (m.Count() > 0)
            {
                using (var db = new DataClasses1DataContext())
                {
                    foreach (var item in m)
                    {
                        var itemNo = item.Cells["CodeNo"].Value.ToSt();
                        var t = db.mh_Items.Where(x => x.InternalNo == itemNo).FirstOrDefault();
                        if (t == null)
                        {
                            baseClass.Warning($"Item ({itemNo} not found.!!!");
                            return;
                        }

                        var tU = db.mh_ItemUOMs.Where(x => x.ItemNo == itemNo && x.UOMCode == t.BaseUOM).FirstOrDefault();
                        decimal u = (tU != null) ? tU.QuantityPer : 1;

                        var rowE = dgvData.Rows.AddNew();
                        //addRow(rowE.Index, DateTime.Now, itemNo, t.InternalName
                        //    , 1, t.BaseUOM, u, 0, 0, 0, 0
                        //    , "", 0, "Waiting");
                    }
                    SetRowNo1(dgvData);

                }
            }
        }

        private void btnRecal_Click(object sender, EventArgs e)
        {
            baseClass.Info("Comming Soon...");
        }
    }
}
