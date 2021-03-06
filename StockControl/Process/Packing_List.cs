﻿using System;
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
    public partial class Packing_List : Telerik.WinControls.UI.RadRibbonForm
    {
        public string t_PackingNo = "";

        //sType = 1 : btnNew to Create Customer P/O,,, 2: btnNew to Select Customer P/O
        int sType = 1;

        public Packing_List(int sType = 1)
        {
            InitializeComponent();
            this.sType = sType;
        }
        public Packing_List()
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

            cbbStatus.SelectedIndex = 1;

            DataLoad();



        }
        private void DataLoad()
        {

            //dt.Rows.Clear();
            try
            {
                dgvData.AutoGenerateColumns = false;
                dgvData.DataSource = null;
                dgvData.Rows.Clear();
                this.Cursor = Cursors.WaitCursor;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    string pk = txtPackingNo.Text.Trim();
                    string itemno = cbbItem.SelectedValue.ToSt();
                    DateTime? dFrom = (cbChkDate.Checked) ? (DateTime?)dtFrom.Value.Date : null;
                    DateTime? dTo = (cbChkDate.Checked) ? (DateTime?)dtTo.Value.Date.AddDays(1).AddMinutes(-1) : null;
                    string CustomerPONo = txtCustomerPONo.Text.Trim();
                    string JobNo = txtJobNo.Text.Trim();
                    string Status = cbbStatus.Text.Trim();

                    var m = db.sp_070_PackingList_Search(pk, itemno, CustomerPONo, JobNo, dFrom, dTo, Status).ToList();
                    dgvData.DataSource = m;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;


            //    radGridView1.DataSource = dt;
        }

        void setStatus()
        {
            foreach (var item in dgvData.Rows)
            {
                if (DateTime.Today <= item.Cells["ReqDate"].Value.ToDateTime().Value.Date)
                    item.Cells["Status"].Value = "On Plan";
                else
                    item.Cells["Status"].Value = "Delay";
            }
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
                var t = new Packing();
                t.ShowDialog();
                DataLoad();
            }
            else
                selRow();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void radGridView1_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            ////select Item from Double click
            if (e.RowIndex >= 0)
                selRow();
        }
        void selRow()
        {
            if (dgvData.CurrentCell != null && dgvData.CurrentCell.RowIndex >= 0)
            {
                var row = dgvData.CurrentCell.RowInfo;
                if (dgvData.CurrentCell.ColumnInfo.Name == "RefNo") //JobNo
                {
                    var j = new ProductionOrder(row.Cells["RefNo"].Value.ToSt());
                    j.ShowDialog();
                }
                else if (dgvData.CurrentCell.ColumnInfo.Name == "CustomerPONo_TEMP")
                {
                    int idDt = row.Cells["idCstmPODt"].Value.ToInt();
                    using (var db = new DataClasses1DataContext())
                    {
                        //var hd = db.mh_CustomerPODTs.Where(x => x.id == idDt && !x.forSafetyStock).FirstOrDefault();
                        var hd = db.mh_SaleOrderDTs.Where(x => x.id == idDt && !x.forSafetyStock).FirstOrDefault();
                        if(hd != null)
                        {
                            //var c = new CustomerPO(hd.idCustomerPO);
                            var c = new SaleOrder(hd.SONo);
                            c.ShowDialog();
                        }
                    }
                }
                else
                {
                    t_PackingNo = row.Cells["PackingNo"].Value.ToSt();
                    if (sType == 1)
                    {
                        string status = row.Cells["Status"].Value.ToSt();
                        if (status == "Active")
                        {
                            var pk = new Packing(t_PackingNo);
                            pk.ShowDialog();
                        }
                        else
                        {
                            //packing cancel

                        }
                        DataLoad();
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
        }

        private void btn_PrintPR_Click(object sender, EventArgs e)
        {
            //select Item for Print
            //throw new NotImplementedException();
            var printe = new PrintPR("", "", "PackingList");
            printe.ShowDialog();
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

                    //var so = new SaleOrder(true);
                    //so.ShowDialog();
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

        //View
        private void radButtonElement2_Click(object sender, EventArgs e)
        {
            dgvData.EndEdit();
            if (dgvData.CurrentCell == null) return;

            string jobNo = dgvData.CurrentCell.RowInfo.Cells["PackingNo"].Value.ToSt();
            var p = new Packing(jobNo);
            p.ShowDialog();
            DataLoad();
        }
        //Edit
        private void radButtonElement3_Click(object sender, EventArgs e)
        {
            dgvData.EndEdit();
            if (dgvData.CurrentCell == null) return;

            string jobNo = dgvData.CurrentCell.RowInfo.Cells["PackingNo"].Value.ToSt();
            var p = new ProductionOrder(jobNo);
            p.ShowDialog();
            DataLoad();
        }
        //Delete
        bool chkDelE()
        {
            bool ret = true;
            string mssg = "";

            var c = dgvData.CurrentCell.RowInfo;
            var s = Math.Round(c.Cells["Qty"].Value.ToDecimal() * c.Cells["PCSUnit"].Value.ToDecimal(), 2);
            var s2 = c.Cells["OutQty"].Value.ToDecimal();

            if (s != s2)
            {
                mssg += "- Cannot delete because FG is already receive.\n";
            }

            if (mssg != "")
            {
                baseClass.Warning(mssg);
                ret = false;
            }
            return ret;
        }
        private void radButtonElement4_Click(object sender, EventArgs e)
        {
            dgvData.EndEdit();
            if (dgvData.CurrentCell == null) return;

            if (chkDelE() && baseClass.IsDel())
                DeleteE();

        }
        void DeleteE()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                using (var d = new DataClasses1DataContext())
                {
                    string jobNo = dgvData.CurrentCell.RowInfo.Cells["PackingNo"].Value.ToSt();
                    using (var db = new DataClasses1DataContext())
                    {
                        var m = db.mh_ProductionOrders.Where(x => x.JobNo == jobNo && x.Active).FirstOrDefault();
                        if (m != null)
                        {
                            m.Active = false;
                            m.UpdateDate = DateTime.Now;
                            m.UpdateBy = ClassLib.Classlib.User;

                            var po = db.mh_CustomerPODTs.Where(x => x.id == m.RefDocId).FirstOrDefault();
                            if (po != null)
                            {
                                //po.OutPlan += (m.Qty * m.PCSUnit) - m.OutQty;
                                po.OutPlan = Math.Round(m.Qty * m.PCSUnit, 3); //Full Return Qty
                                po.Status = baseClass.setCustomerPOStatus(po);
                                db.SubmitChanges();

                                DataLoad();
                                baseClass.Info("Delete complete.\n");
                            }
                        }
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

    }



}
