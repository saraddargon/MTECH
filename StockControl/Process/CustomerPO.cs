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
    public partial class CustomerPO : Telerik.WinControls.UI.RadRibbonForm
    {
        public CustomerPO()
        {
            InitializeComponent();
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

                using (var db = new DataClasses1DataContext())
                {
                    var unit = db.mh_Units.ToList();
                    unit = unit.Where(x => x.UnitActive.ToBool()).ToList();
                    var c1 = dgvData.Columns["Unit"] as GridViewComboBoxColumn;
                    c1.ValueMember = "id";
                    c1.DisplayMember = "UnitCode";
                    c1.DataSource = unit;

                    var cus = db.mh_Customers.Where(x => x.Active).Select(x=>new { x.No, x.Name }).ToList();
                    //cus[0].No;
                    //cus[0].Name;
                    cbbCSTM.MultiColumnComboBoxElement.AutoSizeDropDownToBestFit = true;
                    cbbCSTM.DisplayMember = "Name";
                    cbbCSTM.ValueMember = "No";
                    cbbCSTM.MultiColumnComboBoxElement.DataSource = cus;
                    cbbCSTM.SelectedIndex = -1;

                }

                ClearData();
                btnNew_Click(null, null);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        private void DataLoad()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int ck = 0;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
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
            cbbCSTM.SelectedIndex = -1;
            txtPONo.Text = "";
            dtOrderDate.Value = DateTime.Today;
            dgvData.DataSource = null;
            txtRemark.Text = "";
            txtTotal.Text = (0).ToMoney();
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            btnView.Enabled = true;
            btnEdit.Enabled = true;
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            btnDelete.Enabled = true;
            btnDiscon.Enabled = false;

            btnAdd_Row.Enabled = true;
            btnDel_Item.Enabled = true;

            ClearData();
            Enable_Status(true, "New");
            lblStatus.Text = "New";
            Ac = "New";
            row = dgvData.Rows.Count - 1;
            if (row < 0)
                row = 0;
            //getมาไว้ก่อน แต่ยังไมได้ save 
            //txtTempNo.Text = StockControl.dbClss.GetNo(10, 0);

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            btnView.Enabled = false;
            btnEdit.Enabled = true;
            btnNew.Enabled = true;
            btnSave.Enabled = true;

            btnAdd_Row.Enabled = false;
            btnDel_Item.Enabled = false;

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


            Enable_Status(true, "Edit");
            lblStatus.Text = "Edit";
            Ac = "Edit";

            int te = 0;
            foreach (var g in dgvData.Rows)
            {
                if ((dbClss.TSt(g.Cells["dgvStatus"].Value) == "Partial")
                    || (dbClss.TSt(g.Cells["dgvStatus"].Value) == "Full")
                    || (dbClss.TSt(g.Cells["dgvStatus"].Value) == "Discon")
                    )
                {
                    te = 1;
                    break;
                }
            }
            if (te == 1)
            {
                btnDelete.Enabled = false;
            }


        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {

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
                if(cbbCSTM.SelectedValue.ToSt() == "" || txtCSTMNo.Text == "")
                    err += " “Customer:” is empty \n";
                if(txtPONo.Text.Trim() == "")
                    err += " “P/O No.:” is empty \n";
                if (dgvData.Rows.Where(x => x.IsVisible).Count() < 1)
                    err += " “Items:” is empty \n";


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
                    else
                        SaveE();
                }
                else
                    MessageBox.Show("สถานะต้องเป็น New หรือ Edit เท่านั่น");
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
                    foreach (var item in dgvData.Rows)
                    {
                        int id = item.Cells["id"].Value.ToInt();
                        var t = db.mh_CustomerPOs.Where(x => x.id == id).FirstOrDefault();
                        if(t != null)
                        {
                            //edit
                            t.Active = item.IsVisible;
                        }
                        else if(t.Active)
                        {
                            //add
                            t = new mh_CustomerPO();
                            t.Active = true;
                            t.Amount = item.Cells["Amount"].Value.ToDecimal();
                            t.id = 0;
                            t.CreateBy = "";
                            t.CreateDate = DateTime.Now;
                            t.UpdateBy = "";
                            t.UpdateDate = DateTime.Now;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                baseClass.Error(ex.Message);
            }
            finally { this.Cursor = Cursors.Default; }
        }


        bool LastDiscount = false;
        bool lastDiscountAmount = false;
        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                dgvData.EndEdit();
                if (e.RowIndex >= -1)
                {
                    decimal UnitCost = 0;
                    decimal Qty = 0;
                    decimal PA = 0;
                    decimal ExtendedCost = 0;
                    int cal = 0;
                    int A = 0;

                    if (dgvData.Columns["dgvCodeNo"].Index == e.ColumnIndex)
                    {
                        string CodeNo = dbClss.TSt(e.Row.Cells["dgvCodeNo"].Value);

                        int c = 0;
                        foreach (GridViewRowInfo rowInfo in dgvData.Rows)//datagridview save ที่ละแถว
                        {
                            if (rowInfo.IsVisible.Equals(true))
                            {
                                if (rowInfo.Index != e.RowIndex)
                                {
                                    if (StockControl.dbClss.TSt(rowInfo.Cells["dgvCodeNo"].Value).Equals(CodeNo))
                                    {
                                        c += 1;
                                        break;
                                    }
                                }
                            }
                        }

                        if (c > 0)
                        {
                            MessageBox.Show("รายการซ้ำ");
                            e.Row.Cells["dgvCodeNo"].Value = "";
                            CodeNo = "";
                            return;
                        }


                        if (CodeNo != "" && c <= 0)
                        {
                            using (DataClasses1DataContext db = new DataClasses1DataContext())
                            {
                                var g = (from ix in db.tb_Items select ix).Where(a => a.CodeNo.ToUpper().Trim().Equals(CodeNo.ToUpper().Trim())).ToList();
                                if (g.Count > 0)
                                {

                                    string ItemNo = StockControl.dbClss.TSt(g.FirstOrDefault().ItemNo);
                                    string ItemDescription = StockControl.dbClss.TSt(g.FirstOrDefault().ItemDescription);
                                    string GroupCode = StockControl.dbClss.TSt(g.FirstOrDefault().GroupCode);
                                    int OrderQty = 0;
                                    decimal PCSUnit = StockControl.dbClss.TDe(g.FirstOrDefault().PCSUnit);
                                    string UnitBuy = StockControl.dbClss.TSt(g.FirstOrDefault().UnitBuy);
                                    decimal StandardCost = StockControl.dbClss.TDe(g.FirstOrDefault().StandardCost);

                                    string Status = "Adding";
                                    string PRNO = "";
                                    int Refid = 0;
                                    int id = 0;
                                    DateTime? DeliveryDate = null;

                                    e.Row.Cells["dgvCodeNo"].Value = CodeNo;
                                    e.Row.Cells["dgvItemName"].Value = ItemNo;
                                    e.Row.Cells["dgvItemDesc"].Value = ItemDescription;
                                    e.Row.Cells["dgvGroupCode"].Value = GroupCode;
                                    e.Row.Cells["dgvOrderQty"].Value = OrderQty;
                                    e.Row.Cells["dgvPCSUnit"].Value = PCSUnit;
                                    e.Row.Cells["dgvUnit"].Value = UnitBuy;
                                    e.Row.Cells["dgvCost"].Value = StandardCost;
                                    e.Row.Cells["dgvAmount"].Value = OrderQty * StandardCost;
                                    e.Row.Cells["dgvPRNo"].Value = PRNO;
                                    e.Row.Cells["dgvPRItem"].Value = Refid;
                                    e.Row.Cells["dgvStatus"].Value = Status;
                                    e.Row.Cells["dgvid"].Value = id;
                                    e.Row.Cells["dgvBackOrder"].Value = OrderQty;
                                    //if (dbClss.TSt(DeliveryDate) != "")
                                    e.Row.Cells["dgvDeliveryDate"].Value = DeliveryDate;

                                    e.Row.Cells["dgvCodeNo"].ReadOnly = true;
                                    e.Row.Cells["dgvItemName"].ReadOnly = true;
                                    e.Row.Cells["dgvItemDesc"].ReadOnly = true;
                                    e.Row.Cells["dgvPCSUnit"].ReadOnly = true;
                                    //ee.Cells["dgvUnit"].ReadOnly = true;
                                    //ee.Cells["dgvCost"].ReadOnly = true;
                                }
                            }
                        }
                    }
                    else if (dgvData.Columns["dgvOrderQty"].Index == e.ColumnIndex
                        || dgvData.Columns["dgvCost"].Index == e.ColumnIndex
                        )
                    {
                        decimal OrderQty = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["dgvOrderQty"].Value), out OrderQty);
                        decimal StandardCost = 0; decimal.TryParse(StockControl.dbClss.TSt(e.Row.Cells["dgvCost"].Value), out StandardCost);
                        e.Row.Cells["dgvAmount"].Value = OrderQty * StandardCost;

                        if (StockControl.dbClss.TSt(e.Row.Cells["dgvStatus"].Value) == "Adding")
                            e.Row.Cells["dgvBackOrder"].Value = e.Row.Cells["dgvOrderQty"].Value;


                        decimal.TryParse(Convert.ToString(dgvData.Rows[e.RowIndex].Cells["dgvOrderQty"].Value), out Qty);
                        decimal.TryParse(Convert.ToString(dgvData.Rows[e.RowIndex].Cells["dgvCost"].Value), out UnitCost);
                        decimal.TryParse(Convert.ToString(dgvData.Rows[e.RowIndex].Cells["dgvDiscountAmount"].Value), out PA);
                        decimal.TryParse(Convert.ToString(dgvData.Rows[e.RowIndex].Cells["dgvAmount"].Value), out ExtendedCost);
                        cal = 1;

                        //getTotal();
                    }
                    else if (dgvData.Columns["dgvDiscount"].Index == e.ColumnIndex)
                    {
                        cal = 1;
                        A = 1;
                        LastDiscount = false;
                        //discount %
                    }
                    else if (dgvData.Columns["dgvDiscountAmount"].Index == e.ColumnIndex)
                    {
                        //discount amount
                        LastDiscount = false;
                        cal = 1;
                        A = 2;

                    }

                    if (A > 0)
                    {
                        decimal PC = 0;
                        decimal AM = 0;
                        cal = 1;

                        decimal.TryParse(Convert.ToString(dgvData.Rows[e.RowIndex].Cells["dgvOrderQty"].Value), out Qty);
                        decimal.TryParse(Convert.ToString(dgvData.Rows[e.RowIndex].Cells["dgvCost"].Value), out UnitCost);
                        decimal.TryParse(Convert.ToString(dgvData.Rows[e.RowIndex].Cells["dgvDiscount"].Value), out PC);
                        decimal.TryParse(Convert.ToString(dgvData.Rows[e.RowIndex].Cells["dgvDiscountAmount"].Value), out PA);


                        //calculate Discount

                        if (A == 1 && PC > 0) //%
                        {
                            AM = (Qty * UnitCost);
                            dgvData.Rows[e.RowIndex].Cells["dgvDF"].Value = 1;
                            dgvData.Rows[e.RowIndex].Cells["dgvDiscountAmount"].Value = (AM * PC / 100);
                            dgvData.Rows[e.RowIndex].Cells["dgvDiscountExt"].Value = (AM * PC / 100);
                        }
                        else if (A == 2 && PA > 0) //AM
                        {
                            AM = (Qty * UnitCost);
                            dgvData.Rows[e.RowIndex].Cells["dgvDF"].Value = 2;
                            dgvData.Rows[e.RowIndex].Cells["dgvDiscount"].Value = (PA / AM) * 100;
                            dgvData.Rows[e.RowIndex].Cells["dgvDiscountExt"].Value = PA;
                        }



                        if (PA > (UnitCost * Qty))
                        {
                            MessageBox.Show("ส่วนลดเกิน ยอด Amount!!!");
                            dgvData.Rows[e.RowIndex].Cells["dgvDiscount"].Value = 0;
                            dgvData.Rows[e.RowIndex].Cells["dgvDiscountAmount"].Value = 0;
                            dgvData.Rows[e.RowIndex].Cells["dgvDiscountExt"].Value = 0;
                        }

                    }
                    if (cal > 0)
                    {

                        //dgvDataOrder.Rows[e.RowIndex].Cells["dgvExtendedCost"].Value = UnitCost;
                        //dgvDataOrder.Rows[e.RowIndex].Cells["dgvAmount"].Value = (Qty*UnitCost);
                        //dgvDataOrder.Rows[e.RowIndex].Cells["dgvNetofTAX"].Value = (Qty * UnitCost)-PA;
                        //dgvDataOrder.Rows[e.RowIndex].Cells["dgvVatAmount"].Value = ((Qty * UnitCost)-PA) * vat / 100;
                        //dgvDataOrder.Rows[e.RowIndex].Cells["dgvSubTotal"].Value = ((Qty * UnitCost)-PA) * vat / 100 + ((Qty * UnitCost)-PA);
                        //CallListDiscount();

                        if (LastDiscount)
                        {
                            if (lastDiscountAmount)
                            {
                                CallDiscontLast(true);
                                CallSumDiscountLast(true);
                            }
                            else
                            {
                                CallDiscontLast(false);
                                CallSumDiscountLast(false);
                            }
                        }
                        else
                        {
                            CallListDiscount();
                        }

                        CallTotal();

                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void CallListDiscount()
        {
            decimal UnitCost = 0;
            decimal ExtendedCost = 0;
            decimal Qty = 0;
            decimal PA = 0;
            decimal PR = 0;
            decimal SumP = 0;
            decimal SumA = 0;
            int DF = 0;
            try
            {
                dgvData.EndEdit();
                foreach (var r2 in dgvData.Rows)
                {
                }

            }
            catch { }
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

        private void radPanel1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void เพมพารทToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
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
                    if (StockControl.dbClss.TSt(rd1.Cells["dgvCodeNo"].Value).Equals(CodeNo))
                        re = true;
                }
            }

            return re;

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

                    if (StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["dgvStatus"].Value) == "Waiting"
                        || StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["dgvStatus"].Value) == "Adding")
                    {

                        int id = 0;
                        int.TryParse(StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["dgvid"].Value), out id);
                        if (id <= 0)
                            dgvData.Rows.Remove(dgvData.CurrentRow);

                        else
                        {
                            string CodeNo = "";
                            CodeNo = StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["dgvCodeNo"].Value);
                            if (MessageBox.Show("ต้องการลบรายการ ( " + CodeNo + " ) ออกจากรายการ หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                dgvData.CurrentRow.IsVisible = false;
                            }
                        }
                        CallTotal();
                        //getTotal();
                        SetRowNo1(dgvData);
                    }
                    else
                        MessageBox.Show("ไม่สามารถทำการลบรายการได้ สถานะไม่ถูกต้อง");
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
                //CreatePO_List sc = new CreatePO_List(txtTempNo);
                this.Cursor = Cursors.Default;
                //sc.ShowDialog();



                GC.Collect();
                GC.WaitForPendingFinalizers();

                ClassLib.Memory.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                ClassLib.Memory.Heap();
                //LoadData
                DataLoad();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError("CreatePart", ex.Message + " : radButtonElement1_Click", this.Name); }
            finally { this.Cursor = Cursors.Default; }



        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            btnEdit.Enabled = true;
            btnView.Enabled = false;
            btnNew.Enabled = true;
            btnDiscon.Enabled = false;
            ClearData();
            Enable_Status(false, "View");
            DataLoad();
            Ac = "View";
        }


        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
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

        private void btnAdd_Row_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        private void Add_Item(int Row, string CodeNo, string ItemNo, string ItemDescription, string GroupCode, decimal OrderQty, decimal PCSUnit
           , string UnitBuy, decimal StandardCost, string PRNo, DateTime? DeliveryDate, string Status, int Refid, int id)
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
                ee.Cells["dgvAmount"].Value = OrderQty * StandardCost;
                ee.Cells["dgvPRNo"].Value = PRNo;
                ee.Cells["dgvPRItem"].Value = Refid;
                ee.Cells["dgvStatus"].Value = Status;
                ee.Cells["dgvid"].Value = 0;
                ee.Cells["dgvBackOrder"].Value = OrderQty;

                if (dbClss.TSt(DeliveryDate) != "")
                    ee.Cells["dgvDeliveryDate"].Value = DeliveryDate;

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
                    //ee.Cells["dgvUnit"].ReadOnly = true;
                    //ee.Cells["dgvCost"].ReadOnly = true;
                }
                else
                {
                    ee.Cells["dgvCodeNo"].ReadOnly = false;
                    ee.Cells["dgvItemName"].ReadOnly = false;
                    ee.Cells["dgvItemDesc"].ReadOnly = false;

                    ee.Cells["dgvPCSUnit"].ReadOnly = false;
                    //ee.Cells["dgvUnit"].ReadOnly = false;
                    //ee.Cells["dgvCost"].ReadOnly = false;
                }

                if (Refid > 0)
                {
                    ee.Cells["dgvCodeNo"].ReadOnly = true;
                    ee.Cells["dgvItemName"].ReadOnly = true;
                    ee.Cells["dgvItemDesc"].ReadOnly = true;
                    ee.Cells["dgvPCSUnit"].ReadOnly = true;
                    //ee.Cells["dgvUnit"].ReadOnly = true;
                    //ee.Cells["dgvCost"].ReadOnly = true;
                    ee.Cells["dgvOrderQty"].ReadOnly = true;
                }

                //dbclass.SetRowNo1(dgvData);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dbClss.AddError("CreatePO", ex.Message + " : Add_Item", this.Name); }

        }

        private void getTotal()
        {
            try
            {
                dgvData.EndEdit();
                if (dgvData.Rows.Count > 0)
                {

                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnDiscon_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (lblStatus.Text != "Completed")
            //    {
            //        lblStatus.Text = "Discon";
            //        Ac = "Discon";
            //        int cc = 0;
            //        if (MessageBox.Show("ต้องการยกเลิกรายการ ( " + txtPONo.Text + " ) หรือไม่ ?", "ยกเลิกรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //        {
            //            this.Cursor = Cursors.WaitCursor;
            //            using (DataClasses1DataContext db = new DataClasses1DataContext())
            //            {

            //                foreach (var g in dgvData.Rows)
            //                {
            //                    if (g.IsVisible.Equals(true) && StockControl.dbClss.TBo(g.Cells["dgvDiscon_B"].Value))
            //                    {

            //                        if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) > 0
            //                            && StockControl.dbClss.TSt(g.Cells["dgvStatus"].Value) != "Full"
            //                            && StockControl.dbClss.TSt(g.Cells["dgvStatus"].Value) == "Partial"
            //                            )
            //                        {
            //                            foreach (DataRow row in dt_PODT.Rows)
            //                            {
            //                                if (StockControl.dbClss.TInt(g.Cells["dgvid"].Value) == StockControl.dbClss.TInt(row["id"]))
            //                                {
            //                                    var u = (from ix in db.tb_PurchaseOrderDetails
            //                                             where ix.TempPNo == txtTempNo.Text.Trim()
            //                                             // && ix.TempNo == txtTempNo.Text                                             
            //                                             && ix.id == StockControl.dbClss.TInt(g.Cells["dgvid"].Value)
            //                                             && ix.BackOrder > 0
            //                                             select ix).First();

            //                                    u.Discon = u.BackOrder;
            //                                    u.BackOrder = 0;
            //                                    cc += 1;

            //                                    db.SubmitChanges();
            //                                    dbClss.AddHistory(this.Name, "แก้ไข Item PO", "แก้ไขยกเลิกการรับส่วนที่เหลือ [" + u.CodeNo.ToString() + "]", txtPONo.Text);

            //                                    db.sp_010_Update_StockItem(StockControl.dbClss.TSt(g.Cells["dgvCodeNo"].Value), "BackOrder");
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //                //Calculate Status
            //                db.sp_022_POHD_Cal_Status(txtTempNo.Text, txtPONo.Text);

            //            }

            //            if (cc > 0)
            //                MessageBox.Show("บันทึกรายการ สำเร็จ!");
            //            btnRefresh_Click(null, null);

            //        }
            //    }

            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
            //finally { this.Cursor = Cursors.Default; }
        }


        private void CallDiscontLast(bool am)
        {
            //try
            //{
            //    decimal TaxBase = 0;
            //    decimal Amount = 0;
            //    decimal DisP = 0;
            //    decimal DisA = 0;

            //    decimal.TryParse(lbOrderSubtotal.Text, out TaxBase);
            //    decimal.TryParse(txtLessPoDiscountAmount.Text, out DisA);
            //    decimal.TryParse(txtLessPoDiscountAmountPersen.Text, out DisP);
            //    //decimal SumDis = 0;
            //    dgvData.EndEdit();
            //    foreach (var r2 in dgvData.Rows)
            //    {
            //        Amount = 0;
            //        decimal.TryParse(Convert.ToString(r2.Cells["dgvAmount"].Value), out Amount);
            //        if (!am) // Persent
            //        {
            //            //r2.Cells["dgvdiscount"].Value = (Amount*DisP) / 100;
            //            r2.Cells["dgvDF"].Value = 4;
            //            r2.Cells["dgvDiscountAmount"].Value = ((Amount * DisP) / 100);
            //            r2.Cells["dgvDiscountExt"].Value = ((Amount * DisP) / 100);
            //            r2.Cells["dgvDiscount"].Value = (((Amount * DisP) / 100) / Amount) * 100;
            //            // SumDis += ((Amount * DisP) / 100);
            //        }
            //        else // Amount
            //        {
            //            // MessageBox.Show("xx" + TaxBase+","+Amount);

            //            r2.Cells["dgvDF"].Value = 5;
            //            r2.Cells["dgvDiscountAmount"].Value = ((Amount * DisA) / TaxBase);
            //            r2.Cells["dgvDiscountExt"].Value = ((Amount * DisA) / TaxBase);
            //            r2.Cells["dgvDiscount"].Value = (((Amount * DisA) / TaxBase) / Amount) * 100;
            //        }
            //    }

            //}
            //catch { }
        }

        private void CallSumDiscountLast(bool am)
        {
            //try
            //{
            //    decimal UnitCost = 0;
            //    decimal ExtendedCost = 0;
            //    decimal Qty = 0;
            //    decimal PA = 0;
            //    decimal PR = 0;
            //    decimal SumP = 0;
            //    decimal SumA = 0;
            //    dgvData.EndEdit();
            //    foreach (var r2 in dgvData.Rows)
            //    {
            //        UnitCost = 0;
            //        Qty = 0;
            //        PA = 0;
            //        PR = 0;

            //        decimal.TryParse(Convert.ToString(r2.Cells["dgvOrderQty"].Value), out Qty);
            //        decimal.TryParse(Convert.ToString(r2.Cells["dgvCost"].Value), out UnitCost);
            //        decimal.TryParse(Convert.ToString(r2.Cells["dgvDiscountAmount"].Value), out PA);
            //        decimal.TryParse(Convert.ToString(r2.Cells["dgvDiscount"].Value), out PR);
            //        decimal.TryParse(Convert.ToString(r2.Cells["dgvAmount"].Value), out ExtendedCost);

            //        SumP += ExtendedCost;
            //        SumA += PA;
            //    }
            //    if (am)
            //    {
            //        txtLessPoDiscountAmountPersen.Text = ((SumA / SumP) * 100).ToString("##0.00");
            //    }
            //    else
            //    {

            //        txtLessPoDiscountAmount.Text = (SumA).ToString("###,###,##0.00");
            //    }
            //}
            //catch { }
        }
        private void CallTotal()
        {
            try
            {

            }
            catch (Exception ex) { MessageBox.Show("err2: " + ex.Message); }
        }

        private void cbbCSTM_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtCSTMNo.Text = cbbCSTM.SelectedValue.ToSt();
            }
            catch
            {

                txtCSTMNo.Text = "";
            }
        }
    }
}
