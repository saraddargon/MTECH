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
    public partial class PlanningStatus_Detail : Telerik.WinControls.UI.RadRibbonForm
    {
        bool openFilter = true;
        public PlanningStatus_Detail()
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

            //addrow("Delay", "PD1809-002", "I0002", "Item FG 2", new DateTime(2018, 9, 5), "CSTMPO1809-002", "SO1809-002", "C0001", "TT FUJIT TOOL SUPPORT CO.,LTD");
            addrow("On Plan", "PO1809-001", "I0008", "RM 1", 10, new DateTime(2018, 9, 12));
            addrow("Delay", "PO1809-002", "I0009", "RM 2", 10, new DateTime(2018, 9, 10));
            addrow("On Plan", "SEMI", "I0010", "WIP 1", 10, new DateTime(2018, 9, 15));
        }
        void addrow(string SS, string Document, string Item, string ItemName, decimal Qty, DateTime DueDate)
        {
            var rowe = dgvData.Rows.AddNew();
            rowe.Cells["SS"].Value = SS;
            rowe.Cells["Document"].Value = Document;
            rowe.Cells["Item"].Value = Item;
            rowe.Cells["ItemName"].Value = ItemName;
            rowe.Cells["Qty"].Value = Qty;
            rowe.Cells["DueDate"].Value = DueDate;
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

        }
    }

}
