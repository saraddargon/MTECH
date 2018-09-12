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
    public partial class ProductionOrder_List : Telerik.WinControls.UI.RadRibbonForm
    {
        public string PONo { get; private set; } = "";
        public string CstmNo { get; private set; } = "";

        //sType = 1 : btnNew to Create Customer P/O,,, 2: btnNew to Select Customer P/O
        int sType = 1;

        public ProductionOrder_List(int sType = 1)
        {
            InitializeComponent();
            this.sType = sType;
        }
        public ProductionOrder_List()
        {
            InitializeComponent();
        }


        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }

        private void Unit_Load(object sender, EventArgs e)
        {
            //radGridView1.ReadOnly = true;
            using (var db = new DataClasses1DataContext())
            {

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

            dgvData.AutoGenerateColumns = false;
            //DataLoad();

            dgvData.Columns.ToList().ForEach(x =>
            {
                if (x.Name != "S")
                    x.ReadOnly = true;
            });


            //
            DemoData();
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

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;


            //    radGridView1.DataSource = dt;
        }
        

        void DemoData()
        {
            try
            {
                dgvData.DataSource = null;
                dgvData.Rows.Clear();

                demoadd("On Plan", "JOB1809-001", new DateTime(2018, 9, 30), "I0001", "FG 1", 100, "PCS", new DateTime(2018, 09, 10), new DateTime(2018, 09, 20), "CSTMPO1809-001");
                demoadd("Delay", "JOB1809-002", new DateTime(2018, 9, 8), "I0002", "FG 2", 50, "PCS", new DateTime(2018, 09, 1), new DateTime(2018, 09, 7), "CSTMPO1809-002");
                demoadd("On Plan", "JOB1809-003", new DateTime(2018, 9, 30), "I0003", "FG 3", 10, "PCS", new DateTime(2018, 09, 9), new DateTime(2018, 09, 15), "CSTMPO1809-003");
                demoadd("On Plan", "JOB1809-004", new DateTime(2018, 9, 30), "I0001", "FG 1", 100, "PCS", new DateTime(2018, 09, 10), new DateTime(2018, 09, 20), "CSTMPO1809-004");
                demoadd("On Plan", "JOB1809-005", new DateTime(2018, 9, 30), "I0002", "FG 2", 50, "PCS", new DateTime(2018, 09, 1), new DateTime(2018, 09, 7), "CSTMPO1809-005");
                demoadd("Delay", "JOB1809-006", new DateTime(2018, 9, 8), "I0003", "FG 3", 10, "PCS", new DateTime(2018, 09, 9), new DateTime(2018, 09, 15), "CSTMPO1809-006");
                demoadd("On Plan", "JOB1809-001", new DateTime(2018, 9, 30), "I0001", "FG 1", 100, "PCS", new DateTime(2018, 09, 10), new DateTime(2018, 09, 20), "CSTMPO1809-001");
                demoadd("Delay", "JOB1809-002", new DateTime(2018, 9, 8), "I0002", "FG 2", 50, "PCS", new DateTime(2018, 09, 1), new DateTime(2018, 09, 7), "CSTMPO1809-002");
                demoadd("On Plan", "JOB1809-003", new DateTime(2018, 9, 30), "I0003", "FG 3", 10, "PCS", new DateTime(2018, 09, 9), new DateTime(2018, 09, 15), "CSTMPO1809-003");
                demoadd("On Plan", "JOB1809-004", new DateTime(2018, 9, 30), "I0001", "FG 1", 100, "PCS", new DateTime(2018, 09, 10), new DateTime(2018, 09, 20), "CSTMPO1809-004");
                demoadd("On Plan", "JOB1809-005", new DateTime(2018, 9, 30), "I0002", "FG 2", 50, "PCS", new DateTime(2018, 09, 1), new DateTime(2018, 09, 7), "CSTMPO1809-005");
                demoadd("Delay", "JOB1809-006", new DateTime(2018, 9, 8), "I0003", "FG 3", 10, "PCS", new DateTime(2018, 09, 9), new DateTime(2018, 09, 15), "CSTMPO1809-006");
                demoadd("Delay", "JOB1809-001", new DateTime(2018, 9, 30), "I0001", "FG 1", 100, "PCS", new DateTime(2018, 09, 10), new DateTime(2018, 09, 20), "CSTMPO1809-001");
                demoadd("Delay", "JOB1809-002", new DateTime(2018, 9, 8), "I0002", "FG 2", 50, "PCS", new DateTime(2018, 09, 1), new DateTime(2018, 09, 7), "CSTMPO1809-002");
                demoadd("On Plan", "JOB1809-003", new DateTime(2018, 9, 30), "I0003", "FG 3", 10, "PCS", new DateTime(2018, 09, 9), new DateTime(2018, 09, 15), "CSTMPO1809-003");
                demoadd("On Plan", "JOB1809-004", new DateTime(2018, 9, 30), "I0001", "FG 1", 100, "PCS", new DateTime(2018, 09, 10), new DateTime(2018, 09, 20), "CSTMPO1809-004");
                demoadd("On Plan", "JOB1809-005", new DateTime(2018, 9, 30), "I0002", "FG 2", 50, "PCS", new DateTime(2018, 09, 1), new DateTime(2018, 09, 7), "CSTMPO1809-005");
                demoadd("Delay", "JOB1809-006", new DateTime(2018, 9, 8), "I0003", "FG 3", 10, "PCS", new DateTime(2018, 09, 9), new DateTime(2018, 09, 15), "CSTMPO1809-006");

            }
            catch (Exception ex)
            {
                baseClass.Warning(ex.Message);
            }
        }
        void demoadd(string SS, string ProductionNo, DateTime DueDate, string ItemNo, string ItemName
            , decimal Qty, string Unit, DateTime StartingDate, DateTime EndingDate, string RefDocNo)
        {
            var row = dgvData.Rows.AddNew();
            row.Cells["SS"].Value = SS;
            row.Cells["ProductionNo"].Value = "JOB1809-" + (row.Index + 1).ToString("000");
            row.Cells["DueDate"].Value = DueDate;
            row.Cells["Item"].Value = ItemNo;
            row.Cells["ItemName"].Value = ItemName;
            row.Cells["Qty"].Value = Qty;
            row.Cells["Unit"].Value = Unit;
            row.Cells["StartingDate"].Value = StartingDate;
            row.Cells["EndingDate"].Value = EndingDate;
            row.Cells["RefDocNo"].Value = RefDocNo;
        }
        


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
            //select Item
            if (sType == 1)
            {
                var t = new CustomerPO();
                t.ShowDialog();
            }
            else
                selRow();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //DataLoad();
            DemoData();
        }

        private void radGridView1_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            ////select Item from Double click
            //selRow();
            var ee = new ProductionOrder(true);
            ee.ShowDialog();
        }
        void selRow()
        {
            if (dgvData.CurrentCell != null && dgvData.CurrentCell.RowIndex >= 0)
            {
                var rowe = dgvData.CurrentCell.RowInfo;
                PONo = rowe.Cells["PONo"].Value.ToSt();
                CstmNo = rowe.Cells["CustomerNo"].Value.ToSt();

                if (sType == 1)
                {
                    //var p = new CustomerPO(PONo, CstmNo);
                    //p.ShowDialog();
                    //DataLoad();
                    //PONo = "";
                    //CstmNo = "";
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

        private void btnCreateSaleOrder_Click(object sender, EventArgs e)
        {
            CreateSaleOrder();
        }
        void CreateSaleOrder()
        {
            dgvData.EndEdit();
            try
            {
                if (dgvData.Rows.Where(x => x.Cells["S"].Value.ToBool()).Count() > 0)
                {
                    //var rowS = dgvData.Rows.Where(x => x.Cells["S"].Value.ToBool()).ToList();
                    //if (rowS.Select(x => x.Cells["CustomerNo"].Value.ToSt()).Count() > 1)
                    //{
                    //    baseClass.Warning("Sale order have only 1 Customer.");
                    //    return;
                    //}

                    //var idList = new List<int>();
                    //foreach (var item in rowS)
                    //{
                    //    int id = item.Cells["id"].Value.ToInt();
                    //    if (item.Cells["OutSO"].Value.ToDecimal() <= 0)
                    //    {
                    //        baseClass.Warning("Status P/O Cannot create Sale Order.\n");
                    //        return;
                    //    }
                    //    idList.Add(id);
                    //}

                    //var so = new SaleOrder(idList);
                    //so.ShowDialog();
                    //DataLoad();

                    var so = new SaleOrder(true);
                    so.ShowDialog();
                }
                else
                {
                    baseClass.Warning("Please select data.");
                    return;
                }
            }
            catch (Exception ex)
            {
                baseClass.Warning(ex.Message);
            }
        }
    }



}
