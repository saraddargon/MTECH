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

namespace StockControl
{
    public partial class SaleOrder_List2 : Telerik.WinControls.UI.RadRibbonForm
    {
        public string PONo { get; private set; } = "";
        public string CstmNo { get; private set; } = "";

        //sType = 1 : btnNew to Create Customer P/O,,, 2: btnNew to Select Customer P/O
        int sType = 1;

        public SaleOrder_List2(int sType = 1)
        {
            InitializeComponent();
            this.sType = sType;
        }
      
        List<GridViewRowInfo> RetDT;
        public SaleOrder_List2(List<GridViewRowInfo> RetDT)
        {
            InitializeComponent();
            this.RetDT = RetDT;
            sType = 2;
        }
        public SaleOrder_List2(List<GridViewRowInfo> RetDT,string CSTMNo)
        {
            InitializeComponent();
            this.RetDT = RetDT;
            sType = 2;
            txtCSTMNo.Text = CSTMNo;
            cbbCSTM.Enabled = false;
        }
        public SaleOrder_List2()
        {
            InitializeComponent();
        }
        public SaleOrder_List2(string CstmPo, string CstmNo)
        {
            InitializeComponent();
            this.PONo = CstmPo;
            this.CstmNo = CstmNo;
            
            
        }


        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }

        private void Unit_Load(object sender, EventArgs e)
        {
            dtFrom.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));
            dtTo.Value = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US"));

            //radGridView1.ReadOnly = true;
            LoadDefault();
            txtCstmPO.Text = this.PONo;
            cbbCSTM.SelectedValue = this.CstmNo;

            dgvData.Columns.ToList().ForEach(x =>
            {
                if (x.Name == "S")
                    x.ReadOnly = false;
                else
                    x.ReadOnly = true;
            });

            dgvData.AutoGenerateColumns = false;


            DataLoad();
        }

        private void LoadDefault()
        {
            using (var db = new DataClasses1DataContext())
            {
                var cust = db.mh_Customers.Where(x => x.Active)
                    .Select(x => new CustomerCombo { No = x.No, Name = x.Name }).ToList();
                cust.Add(new CustomerCombo
                {
                    No = "",
                    Name = ""
                });
                cust = cust.OrderBy(x => x.No).ToList();
                cbbCSTM.AutoSizeDropDownToBestFit = true;
                cbbCSTM.MultiColumnComboBoxElement.DisplayMember = "Name";
                cbbCSTM.MultiColumnComboBoxElement.Value = "No";
                cbbCSTM.MultiColumnComboBoxElement.DataSource = cust;
                cbbCSTM.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

                var item = db.mh_Items.Where(x => x.Active)
                    .Select(x => new ItemCombo { Item = x.InternalNo, ItemName = x.InternalName }).ToList();
                item.Add(new ItemCombo
                {
                    Item = "",
                    ItemName = ""
                });
                item = item.OrderBy(x => x.Item).ToList();
                cbbItem.AutoSizeDropDownToBestFit = true;
                cbbItem.DisplayMember = "ItemName";
                cbbItem.ValueMember = "Item";
                cbbItem.DataSource = item;
                cbbItem.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            }
        }

        private void DataLoad()
        {

            //dt.Rows.Clear();
            try
            {
                dgvData.DataSource = null;
                this.Cursor = Cursors.WaitCursor;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    string dt1 = "";
                    string dt2 = "";
                    if(cbChkDate.Checked)
                    {
                        dt1 = Convert.ToDateTime(dtFrom.Value).ToString("yyyyMMdd");
                        dt2 = Convert.ToDateTime(dtTo.Value).ToString("yyyyMMdd");
                    }
                    dgvData.DataSource = db.sp_054_SaleOrder_List(txtPONo.Text, "", cbbItem.Text, dt1, dt2,ddlStatus.Text, cbbCSTM.SelectedValue.ToSt(), txtCstmPO.Text );
                }


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;

        }

        //


        private void btnCancel_Click(object sender, EventArgs e)
        {
            DataLoad();
        }


        int row = -1;
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
            if (dgvData.Rows.Count <= 0) return;

            try
            {
                if (sType == 2)
                {
                    dgvData.EndEdit();
                    foreach (GridViewRowInfo rowinfo in dgvData.Rows.Where(o => Convert.ToBoolean(o.Cells["S"].Value)))
                    {
                        RetDT.Add(rowinfo);
                    }

                    this.Close();
                }
                else
                {
                    dgvData.EndEdit();
                    string temp = "";
                    foreach (var ix in dgvData.Rows)
                    {
                        if (dbClss.TBo(ix.Cells["S"].Value))
                        {
                            temp = dbClss.TSt(ix.Cells["SONo"].Value);
                            break;
                        }
                    }
                    if(temp !="")
                    {
                        SaleOrder a = new SaleOrder(temp);
                        a.ShowDialog();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }


        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataLoad();
            //demo();
        }

        private void radGridView1_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            ////select Item from Double click
            //selRow();
            //demo();

            try
            {
                if (e.RowIndex >= 0)
                {
                    if (dgvData.Rows.Count <= 0)
                        return;


                    dgvData.EndEdit();
                    string temp = "";
                    temp = dbClss.TSt(e.Row.Cells["SONo"].Value);
                    if (temp != "")
                    {
                        SaleOrder a = new SaleOrder(temp);
                        a.ShowDialog();
                    }
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }

        }
        void selRow()
        {
            if (dgvData.CurrentCell != null && dgvData.CurrentCell.RowIndex >= 0)
            {
                var rowe = dgvData.CurrentCell.RowInfo;
                PONo = rowe.Cells["SONo"].Value.ToSt();
                CstmNo = rowe.Cells["CustomerNo"].Value.ToSt();

                if (sType == 1)
                {
                    var p = new SaleOrder(PONo, CstmNo);
                    p.ShowDialog();
                    DataLoad();
                    PONo = "";
                    CstmNo = "";
                }
                else
                    this.Close();
            }
        }

        private void btn_PrintPR_Click(object sender, EventArgs e)
        {
            //select Item for Print
            //throw new NotImplementedException();

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

                    //foreach (var rd in radGridView1.Rows)
                    //{
                    //    if (rd.Index <= Row)
                    //    {
                    //        radGridView1.Rows[rd.Index].PinPosition = PinnedRowPosition.Top;
                    //    }
                    //    else
                    //        break;
                    //}
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

                    //foreach (var rd in radGridView1.Columns)
                    //{
                    //    if (rd.Index <= Col)
                    //    {
                    //        radGridView1.Columns[rd.Index].PinPosition = PinnedColumnPosition.Left;
                    //    }
                    //    else
                    //        break;
                    //}
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
                //foreach (var rd in radGridView1.Rows)
                //{
                //    radGridView1.Rows[rd.Index].IsPinned = false;
                //}
                //foreach (var rd in radGridView1.Columns)
                //{
                //    radGridView1.Columns[rd.Index].IsPinned = false;                   
                //}

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }


        void CreateShipment()
        {
            //var sm = new Shipment(true);
            //sm.ShowDialog();
            List<int> idList = new List<int>();
            try
            {
                int t = 0;
                this.Cursor = Cursors.WaitCursor;
                foreach (GridViewRowInfo rowinfo in dgvData.Rows.Where(o => Convert.ToBoolean(o.Cells["S"].Value)))
                {
                    if (dbClss.TSt(rowinfo.Cells["SS"].Value) == "Partial" || dbClss.TSt(rowinfo.Cells["SS"].Value) == "Process")
                    {
                        idList.Add(dbClss.TInt(rowinfo.Cells["id"].Value));
                    }
                    else
                    {
                        t = 1;
                        MessageBox.Show("สถานะบางรายการไม่สามารถ สร้างรายการเบิกได้");
                        break;
                    }                   
                }
                if (t == 1)
                    return;

                if (idList.Count > 0)
                {
                    Shipment a = new Shipment(idList);
                    a.ShowDialog();
                    DataLoad();
                }


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }
        private void btnCreateShipment_Click(object sender, EventArgs e)
        {
            dgvData.EndEdit();
            CreateShipment();
        }
    }


}
