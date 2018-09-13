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
    public partial class PlanningCal : Telerik.WinControls.UI.RadRibbonForm
    {
        bool openFilter = true;
        public PlanningCal()
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

            DataLoad();

            dgvData.Columns.ToList().ForEach(x =>
            {
                x.ReadOnly = x.Name != "S";
            });
        }

        private void DataLoad()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    var m = db.mh_Planning_TEMPs.ToList();
                    dgvData.DataSource = null;
                    dgvData.AutoGenerateColumns = false;
                    dgvData.DataSource = m;
                }
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

        }


        int row = -1;
        private void radGridView1_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            row = e.RowIndex;

        }


        private void MasterTemplate_CellValueChanged(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.Column.Name.Equals("S") && e.Row.Cells["S"].Value.ToBool())
                {
                    if (e.Row.Cells["Status"].Value.ToSt() == "Over Due")
                        e.Row.Cells["S"].Value = false;
                }
            }
        }

        private void MasterTemplate_RowFormatting(object sender, RowFormattingEventArgs e)
        {

        }

        private void MasterTemplate_CellFormatting(object sender, CellFormattingEventArgs e)
        {

        }

        private void btnRecal_Click(object sender, EventArgs e)
        {
            var ft = new PlanningCal_Filter();
            ft.ShowDialog();
            if (ft.okFilter)
            {
                ClearPlan();

                radLabelElement1.Text = $"Filter date : {ft.dateFrom.ToDtString()}-{ft.dateTo.ToDtString()}";
                //LoadData(true, ft.dateFrom, ft.dateTo, ft.MRP, ft.MPS, ft.ItemNo, ft.locationItem);
                var ps = new PlanningCal_Status();
                ps.dFrom = ft.dateFrom;
                ps.dTo = ft.dateTo;
                ps.ItemNo = ft.ItemNo;
                ps.LocationItem = ft.locationItem;
                ps.ShowDialog();
                if (ps.calComplete)
                {
                    dgvData.DataSource = null;
                    dgvData.AutoGenerateColumns = false;
                    dgvData.DataSource = ps.gridPlans.Where(x => ((ft.MRP && x.PlanningType == "Purchase")
                                    || (ft.MPS && x.PlanningType == "Production"))
                                    && (ft.locationItem == "" || x.LocationItem == ft.locationItem)
                                    ).OrderBy(x => x.ReqDate).ToList();

                    SavePlan(ps.gridPlans, ft.dateFrom, ft.dateTo);
                }
            }
        }
        private void btnFil_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count == 0)
            {
                baseClass.Warning("Not have data for Filter.\n");
                return;
            }
            //
            this.Cursor = Cursors.WaitCursor;
            try
            {
                var ft = new PlanningCal_Filter();
                ft.ShowDialog();
                if (ft.okFilter)
                {
                    DateTime dFrom = ft.dateFrom.Date;
                    DateTime dTo = ft.dateTo.Date.AddDays(1).AddMinutes(-1);
                    string ItemNo = ft.ItemNo;
                    string LocationItem = ft.locationItem;
                    List<int> idA = new List<int>();
                    foreach (var item in dgvData.Rows)
                    {
                        //Date
                        var ReqDate = item.Cells["ReqDate"].Value.ToDateTime().Value;
                        if (ReqDate < dFrom || ReqDate > dTo)
                        {
                            item.IsVisible = false;
                            continue;
                        }
                        else
                            item.IsVisible = true;
                        //Location
                        var Lo = item.Cells["LocationItem"].Value.ToSt();
                        if (LocationItem == "") { }
                        else
                        {
                            if (LocationItem != Lo)
                            {
                                item.IsVisible = false;
                                continue;
                            }
                            else
                                item.IsVisible = true;
                        }
                        //MPS MRP
                        if (ft.MPS && ft.MRP) { }
                        else
                        {
                            string pType = item.Cells["PlanningType"].Value.ToSt();
                            if (ft.MPS && pType == "Production")
                                item.IsVisible = true;
                            else if (ft.MPS && pType == "Purchase")
                            {
                                item.IsVisible = false;
                                continue;
                            }
                            else if (!ft.MPS && pType == "Production")
                            {
                                item.IsVisible = false;
                                continue;
                            }
                            else if (ft.MRP && pType == "Purchase")
                                item.IsVisible = true;
                            else if(ft.MRP && pType == "Production")
                            {
                                item.IsVisible = false;
                                continue;
                            }
                            else if(!ft.MRP && pType == "Purchase")
                            {
                                item.IsVisible = false;
                                continue;
                            }

                        }
                        //Item
                        var FGNo = item.Cells["ItemNo"].Value.ToSt();
                        int idRef = item.Cells["idRef"].Value.ToInt();
                        if (ItemNo == "") { }
                        else
                        {
                            if (FGNo != ItemNo)
                            {
                                item.IsVisible = false;
                                continue;
                            }
                            else if (!idA.Any(x => x == idRef))
                            {
                                idA.Add(idRef);
                            }
                        }
                    }

                    //find idRef
                    foreach (var item in dgvData.Rows.Where(x => !x.IsVisible))
                    {
                        int idRef = item.Cells["idRef"].Value.ToInt();
                        item.IsVisible = idA.Any(x => x == idRef);
                    }
                }
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



        private void btnGenerate_Click(object sender, EventArgs e)
        {
            dgvData.EndEdit();
            GenE();
        }
        void GenE()
        {
            if (dgvData.Rows.Where(x => x.Cells["S"].Value.ToBool()).ToList().Count > 0)
            {
                if (dgvData.Rows.Where(x => x.Cells["S"].Value.ToBool() && x.Cells["SS"].Value.ToSt() == "Over Due").Count() > 0)
                    return;
                this.Cursor = Cursors.WaitCursor;
                Thread.Sleep(3000);
                baseClass.Info("Generate to Production/Purchase complete.!!");
                this.Cursor = Cursors.Default;
            }
        }

        private void MasterTemplate_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.Column.Name.Equals("S"))
                {
                    e.Cancel = e.Row.Cells["Status"].Value.Equals("Over Due");
                }
            }
        }

        private void btnClearPlan_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count > 0 && baseClass.Question("Do you want to 'Clear Plan data' ?"))
                ClearPlan(true);
        }
        void ClearPlan(bool warningMssg = false)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    db.mh_Planning_TEMPs.DeleteAllOnSubmit(db.mh_Planning_TEMPs);
                    db.SubmitChanges();

                    dgvData.DataSource = null;
                    dgvData.Rows.Clear();

                    if (warningMssg)
                        baseClass.Warning("Clear Plan complete.\n");

                }
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
        void SavePlan(List<grid_Planning> gPlans, DateTime dFrom, DateTime dTo)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    foreach (var g in gPlans)
                    {
                        var p = new mh_Planning_TEMP
                        {
                            DueDate = g.DueDate,
                            EndingDate = g.EndingDate.Value,
                            GroupType = g.GroupType,
                            idRef = g.idRef,
                            InvGroup = g.InvGroup,
                            ItemName = g.ItemName,
                            ItemNo = g.ItemNo,
                            PCSUnit = g.PCSUnit,
                            PlanDateFrom = dFrom,
                            PlanDateTo = dTo,
                            PlanningType = g.PlanningType,
                            Qty = g.Qty,
                            RefDocNo = g.RefDocNo,
                            ReqDate = g.ReqDate,
                            StartingDate = g.StartingDate.Value,
                            Status = g.Status,
                            TotalCost = g.TotalCost,
                            Type = g.Type,
                            UOM = g.UOM,
                            VendorName = g.VendorName,
                            VendorNo = g.VendorNo,
                            LocationItem = g.LocationItem,
                        };
                        db.mh_Planning_TEMPs.InsertOnSubmit(p);
                    }
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                baseClass.Warning(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void radButtonElement1_Click(object sender, EventArgs e)
        {
            dgvData.EnableFiltering = true;
        }

        private void radButtonElement2_Click(object sender, EventArgs e)
        {
            dgvData.EnableFiltering = false;
        }
    }

}
