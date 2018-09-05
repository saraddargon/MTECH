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
    public partial class WorkCenters_Capa : Telerik.WinControls.UI.RadRibbonForm
    {
        bool openFilter = true;
        public WorkCenters_Capa(bool openFilter = true)
        {
            InitializeComponent();
            this.openFilter = openFilter;
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
            dgvData.ReadOnly = true;
            dgvData.AutoGenerateColumns = false;


            if (openFilter)
            {
                btnFil_Click(null, null);
            }
        }


        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                dgvData.Rows[e.RowIndex].Cells["dgvC"].Value = "T";
                //string check1 = Convert.ToString(radGridView1.Rows[e.RowIndex].Cells["VendorName"].Value);
                //string TM= Convert.ToString(radGridView1.Rows[e.RowIndex].Cells["dgvCodeTemp2"].Value);
                //if (!check1.Trim().Equals("") && TM.Equals(""))
                //{

                //    if (!CheckDuplicate(check1.Trim()))
                //    {
                //        MessageBox.Show("ชื้อผู้ขายซ้ำ ซ้ำ");
                //        radGridView1.Rows[e.RowIndex].Cells["GroupCode"].Value = "";
                //        radGridView1.Rows[e.RowIndex].Cells["GroupCode"].IsSelected = true;

                //    }
                //}


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
            var cal = new WorkCenters_Cal();
            cal.ShowDialog();
        }

        private void btnFil_Click(object sender, EventArgs e)
        {
            var ft = new WorkCenters_Filter();
            ft.ShowDialog();
            if (ft.okFilter)
            {
                radLabelElement1.Text = $"Filter date : {ft.dateFrom.ToDtString()}-{ft.dateTo.ToDtString()}";
                LoadData(ft.dateFrom, ft.dateTo, ft.workId);
            }
        }
        void LoadData(DateTime dtFrom, DateTime dtTo, int workId)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    dgvData.Rows.Clear();
                    var colS = dgvData.Columns.Where(x => x.Name.StartsWith("d_")).ToList();
                    colS.ForEach(x =>
                    {
                        dgvData.Columns.Remove(x);
                    });

                    var c = db.mh_CapacityAvailables
                        .Where(x => x.Date >= dtFrom && x.Date <= dtTo && (workId == 0 || x.WorkCenterID == workId)).ToList();
                    var dTemp = dtFrom;

                    do
                    {
                        string colName = $"d_{dTemp.ToString("yyyyMMdd")}";
                        var col = new GridViewDecimalColumn(colName);
                        col.HeaderText = dTemp.ToDtString();
                        col.DecimalPlaces = 2;
                        col.FormatString = "{0:N2}";
                        col.Width = 80;
                        dgvData.Columns.Add(col);
                        decimal? capa = null;
                        var wlist = new List<mh_WorkCenter>();

                        foreach (var e in c.Where(x => x.Date == dTemp))
                        {
                            //find Row
                            var rowE = dgvData.Rows.Where(x => x.Tag.ToInt() == e.WorkCenterID).FirstOrDefault();
                            //find Work
                            var w = wlist.Where(x => x.id == e.WorkCenterID).FirstOrDefault();
                            if (w == null)
                            {
                                w = db.mh_WorkCenters.Where(x => x.id == e.WorkCenterID).FirstOrDefault();
                                wlist.Add(w);
                            }

                            if (w == null)
                                continue;

                            //set row
                            if (rowE == null)
                            {
                                rowE = dgvData.Rows.NewRow();
                                rowE.Cells["id"].Value = e.WorkCenterID;
                                rowE.Tag = e.WorkCenterID.ToInt();

                                rowE.Cells["No"].Value = w.WorkCenterNo;
                                rowE.Cells["Name"].Value = w.WorkCenterName;
                                dgvData.Rows.Add(rowE);
                            }
                            
                            //set capa
                            if (e.Capacity != null)
                            {
                                ////UOM : Min(0), Hour(1), Day(2)
                                //if (w.UOM == 0)
                                //    capa = e.Capacity;
                                //else if (w.UOM == 1) //Hour
                                //    capa = Math.Round(e.Capacity.ToDouble() / 60, 2).ToDecimal();
                                //else if (w.UOM == 2) //Day
                                //    capa = Math.Round(e.Capacity.ToDouble() / (60 * 24), 2).ToDecimal();
                                capa = e.Capacity;
                            }


                            rowE.Cells[colName].Value = capa;
                        }

                        dTemp = dTemp.AddDays(1);
                    } while (dTemp <= dtTo);
                }

            }
            catch (Exception ex)
            {
                baseClass.Error(ex.Message);
            }
            finally { this.Cursor = Cursors.Default; }
        }

    }
}
