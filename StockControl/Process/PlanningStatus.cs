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
using Telerik.WinControls;
using System.Threading;

namespace StockControl
{
    public partial class PlanningStatus : Telerik.WinControls.UI.RadRibbonForm
    {
        bool openFilter = true;
        public PlanningStatus()
        {
            InitializeComponent();
            openFilter = false;
        }

        private void radMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            HistoryView hw = new HistoryView(this.Name);
            this.Cursor = Cursors.Default;
            hw.ShowDialog();
        }

        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }
        private void Unit_Load(object sender, EventArgs e)
        {
            //dgvData.ReadOnly = true;
            dgvData.AutoGenerateColumns = false;

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

            dgvData.Columns.ToList().ForEach(x =>
            {
                x.ReadOnly = x.Name != "S";
            });

            demo();
        }

        void demo()
        {
            dgvData.DataSource = null;
            dgvData.Rows.Clear();

            addrow("On Plan", "JOB1809-001", "I0001", "Item FG 1", new DateTime(2018, 9, 30), "CSTMPO1809-001", "SO1809-001", "C0001", "TT FUJIT TOOL SUPPORT CO.,LTD");
            addrow("Delay", "JOB1809-002", "I0002", "Item FG 2", new DateTime(2018, 9, 20), "CSTMPO1809-002", "SO1809-002", "C0001", "TT FUJIT TOOL SUPPORT CO.,LTD");
            addrow("On Plan", "JOB1809-003", "I0003", "Item FG 3", new DateTime(2018, 10, 2), "CSTMPO1809-003", "SO1809-003", "C0001", "TT FUJIT TOOL SUPPORT CO.,LTD");
            addrow("On Plan", "JOB1809-004", "I0004", "Item FG 4", new DateTime(2018, 10, 3), "CSTMPO1809-004", "SO1809-004", "C0001", "TT FUJIT TOOL SUPPORT CO.,LTD");
            addrow("On Plan", "JOB1809-005", "I0001", "Item FG 1", new DateTime(2018, 9, 30), "CSTMPO1809-001", "SO1809-001", "C0001", "TT FUJIT TOOL SUPPORT CO.,LTD");
            addrow("Delay", "JOB1809-006", "I0002", "Item FG 2", new DateTime(2018, 9, 20), "CSTMPO1809-002", "SO1809-002", "C0001", "TT FUJIT TOOL SUPPORT CO.,LTD");
            addrow("On Plan", "JOB1809-007", "I0003", "Item FG 3", new DateTime(2018, 10, 2), "CSTMPO1809-003", "SO1809-003", "C0001", "TT FUJIT TOOL SUPPORT CO.,LTD");
            addrow("On Plan", "JOB1809-008", "I0004", "Item FG 4", new DateTime(2018, 10, 3), "CSTMPO1809-004", "SO1809-004", "C0001", "TT FUJIT TOOL SUPPORT CO.,LTD");
            addrow("On Plan", "JOB1809-009", "I0001", "Item FG 1", new DateTime(2018, 9, 30), "CSTMPO1809-001", "SO1809-001", "C0001", "TT FUJIT TOOL SUPPORT CO.,LTD");
            addrow("Delay", "JOB1809-0010", "I0002", "Item FG 2", new DateTime(2018, 9, 20), "CSTMPO1809-002", "SO1809-002", "C0001", "TT FUJIT TOOL SUPPORT CO.,LTD");
            addrow("On Plan", "JOB1809-003", "I0003", "Item FG 3", new DateTime(2018, 10, 2), "CSTMPO1809-003", "SO1809-003", "C0001", "TT FUJIT TOOL SUPPORT CO.,LTD");
            addrow("On Plan", "JOB1809-004", "I0004", "Item FG 4", new DateTime(2018, 10, 3), "CSTMPO1809-004", "SO1809-004", "C0001", "TT FUJIT TOOL SUPPORT CO.,LTD");
            addrow("Delay", "JOB1809-001", "I0001", "Item FG 1", new DateTime(2018, 9, 30), "CSTMPO1809-001", "SO1809-001", "C0001", "TT FUJIT TOOL SUPPORT CO.,LTD");
            addrow("Delay", "JOB1809-002", "I0002", "Item FG 2", new DateTime(2018, 9, 20), "CSTMPO1809-002", "SO1809-002", "C0001", "TT FUJIT TOOL SUPPORT CO.,LTD");
            addrow("Delay", "JOB1809-003", "I0003", "Item FG 3", new DateTime(2018, 10, 2), "CSTMPO1809-003", "SO1809-003", "C0001", "TT FUJIT TOOL SUPPORT CO.,LTD");
            addrow("On Plan", "JOB1809-004", "I0004", "Item FG 4", new DateTime(2018, 10, 3), "CSTMPO1809-004", "SO1809-004", "C0001", "TT FUJIT TOOL SUPPORT CO.,LTD");
            addrow("On Plan", "JOB1809-001", "I0001", "Item FG 1", new DateTime(2018, 9, 30), "CSTMPO1809-001", "SO1809-001", "C0001", "TT FUJIT TOOL SUPPORT CO.,LTD");
            addrow("On Plan", "JOB1809-002", "I0002", "Item FG 2", new DateTime(2018, 9, 20), "CSTMPO1809-002", "SO1809-002", "C0001", "TT FUJIT TOOL SUPPORT CO.,LTD");
            addrow("On Plan", "JOB1809-003", "I0003", "Item FG 3", new DateTime(2018, 10, 2), "CSTMPO1809-003", "SO1809-003", "C0001", "TT FUJIT TOOL SUPPORT CO.,LTD");
            addrow("On Plan", "JOB1809-004", "I0004", "Item FG 4", new DateTime(2018, 10, 3), "CSTMPO1809-004", "SO1809-004", "C0001", "TT FUJIT TOOL SUPPORT CO.,LTD");
            addrow("On Plan", "JOB1809-001", "I0001", "Item FG 1", new DateTime(2018, 9, 30), "CSTMPO1809-001", "SO1809-001", "C0001", "TT FUJIT TOOL SUPPORT CO.,LTD");
            addrow("Delay", "JOB1809-002", "I0002", "Item FG 2", new DateTime(2018, 9, 20), "CSTMPO1809-002", "SO1809-002", "C0001", "TT FUJIT TOOL SUPPORT CO.,LTD");
            addrow("Delay", "JOB1809-003", "I0003", "Item FG 3", new DateTime(2018, 10, 2), "CSTMPO1809-003", "SO1809-003", "C0001", "TT FUJIT TOOL SUPPORT CO.,LTD");
            addrow("On Plan", "JOB1809-004", "I0004", "Item FG 4", new DateTime(2018, 10, 3), "CSTMPO1809-004", "SO1809-004", "C0001", "TT FUJIT TOOL SUPPORT CO.,LTD");
        }
        void addrow(string SS, string JobNo, string Item, string ItemName
            , DateTime DueDate, string CustomerPO, string SaleOrder, string CustomerNo, string CustomerName)
        {
            var rowe = dgvData.Rows.AddNew();
            rowe.Cells["SS"].Value = SS;
            rowe.Cells["JobNo"].Value = "JOB1809-" + (rowe.Index + 1).ToString("000");
            rowe.Cells["Item"].Value = Item;
            rowe.Cells["ItemName"].Value = ItemName;
            rowe.Cells["DueDate"].Value = DueDate;
            rowe.Cells["CustomerPO"].Value = CustomerPO;
            rowe.Cells["SaleOrder"].Value = SaleOrder;
            rowe.Cells["CustomerNo"].Value = CustomerNo;
            rowe.Cells["CustomerName"].Value = CustomerName;
        }
        //

        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {



            }
            catch (Exception ex) { }
        }

        private void radGridView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //// MessageBox.Show(e.KeyCode.ToString());

            //if (e.KeyData == (Keys.Control | Keys.S))
            //{
            //    btnSave_Click(null, null);
            //}
            //else if (e.KeyData == (Keys.Control | Keys.N))
            //{
            //    if (MessageBox.Show("ต้องการสร้างใหม่ ?", "สร้างใหม่", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //    {
            //        NewClick();
            //    }

            //}
        }


        int row = -1;
        private void radGridView1_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            row = e.RowIndex;

        }


        private void MasterTemplate_CellValueChanged(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {

        }

        private void MasterTemplate_RowFormatting(object sender, RowFormattingEventArgs e)
        {

        }

        private void MasterTemplate_CellFormatting(object sender, CellFormattingEventArgs e)
        {

        }

        private void btnRecal_Click(object sender, EventArgs e)
        {
            var p = new PlanningCal();
            p.ShowDialog();
        }

        void Recalculate()
        {
            var pc = new PlanningCal_Status();
            pc.ShowDialog();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            demo();
        }

        private void MasterTemplate_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            //Open Planning Status_Detail
            var p = new PlanningStatus_Detail();
            p.ShowDialog();

        }
    }

}
