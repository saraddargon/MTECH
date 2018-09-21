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

namespace StockControl
{
    public partial class PlanningCal_Filter : Telerik.WinControls.UI.RadRibbonForm
    {
        public bool okFilter = false;
        public DateTime dateFrom { get; private set; }
        public DateTime dateTo { get; private set; }
        public string ItemNo { get; private set; }
        public string locationItem { get; private set; }
        public bool MRP { get;private set; } = false;
        public bool MPS { get; private set; } = false;

        public PlanningCal_Filter(bool MRP, bool MPS)
        {
            InitializeComponent();
            //this.MRP = MRP;
            //this.MPS = MPS;
            cbMRP.Checked = MRP;
            cbMPS.Checked = MPS;
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
            dtFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtTo.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

            ////for test data
            //dtFrom.Value = new DateTime(2018, 9, 4);
            //dtTo.Value = new DateTime(2018, 9, 7);

            using (var db = new DataClasses1DataContext())
            {
                var t = db.mh_Items.Where(x => x.Active)
                    .Select(x => new ItemCombo { Item = x.InternalNo, ItemName = x.InternalName }).ToList();
                t.Add(new ItemCombo { Item = "", ItemName = "" });
                t = t.OrderBy(x => x.Item).ToList();
                cbbItem.DisplayMember = "ItemName";
                cbbItem.ValueMember = "Item";
                cbbItem.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cbbItem.AutoSizeDropDownToBestFit = true;
                cbbItem.DataSource = t;

                var l = db.mh_Locations.Where(x => x.Active).ToList();
                l.Add(new mh_Location
                {
                    Active = true,
                    Code = "",
                    id = 0,
                    Name = ""
                });
                l = l.OrderBy(x => x.id).ToList();
                cbbLocation.DisplayMember = "Name";
                cbbLocation.ValueMember = "Code";
                cbbLocation.DataSource = l;
            }

        }

        private void btnRecal_Click(object sender, EventArgs e)
        {
            Calculate();
        }
        void Calculate()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if(!cbMPS.Checked && !cbMRP.Checked)
                {
                    baseClass.Warning("Please select MRP or MPS.\n");
                    return;
                }

                okFilter = true;
                dateFrom = dtFrom.Value.Date;
                dateTo = dtTo.Value.Date.AddDays(1).AddMinutes(-1);
                MPS = cbMPS.Checked;
                MRP = cbMRP.Checked;
                ItemNo = cbbItem.SelectedValue.ToSt();
                locationItem = cbbLocation.SelectedValue.ToSt();

                this.Close();
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

    }
}
