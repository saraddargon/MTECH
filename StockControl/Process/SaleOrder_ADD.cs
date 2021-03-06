﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using Telerik.WinControls.UI;

namespace StockControl
{
    public partial class SaleOrder_ADD : Telerik.WinControls.UI.RadRibbonForm
    {
        public string PONo { get; private set; } = "";
        public string CstmNo { get; private set; } = "";

        //sType = 1 : btnNew to Create Customer P/O,,, 2: btnNew to Select Customer P/O
        int sType = 1;

        public SaleOrder_ADD(int sType = 1)
        {
            InitializeComponent();
            this.sType = sType;
        }
        List<GridViewRowInfo> RetDT;
        string CSTMNo = "";
        string CSTMName = "";
        public SaleOrder_ADD(List<GridViewRowInfo> RetDT)
        {
            InitializeComponent();
            this.RetDT = RetDT;
        }
        public SaleOrder_ADD(List<GridViewRowInfo> RetDT,string CSTMNo,string CSTMName)
        {
            InitializeComponent();
            this.RetDT = RetDT;
            this.CSTMNo = CSTMNo;
            this.CSTMName = CSTMName;
            sType = 3;
        }
        public SaleOrder_ADD()
        {
            InitializeComponent();
        }


        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }

        private void Unit_Load(object sender, EventArgs e)
        {
            //radGridView1.ReadOnly = true;
            LoadDef();
            if (sType == 3)
            {
                cbbCSTM.Text = CSTMName;
                txtCSTMNo.Text = CSTMNo;
                cbbCSTM.Enabled = false;
            }
            dgvData.AutoGenerateColumns = false;
            DataLoad();
            
            dgvData.Columns.ToList().ForEach(x =>
            {
                if (x.Name != "S")
                    x.ReadOnly = true;
            });
        }

        private void LoadDef()
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
            this.Cursor = Cursors.WaitCursor;
            //dt.Rows.Clear();
            try
            {
                dgvData.DataSource = null;
                dgvData.Rows.Clear();

                using (var db = new DataClasses1DataContext())
                {
                    DateTime? dFrom = (cbChkDate.Checked) ? (DateTime?)dtFrom.Value.Date : null;
                    DateTime? dTo = (cbChkDate.Checked) ? (DateTime?)dtTo.Value.Date.AddDays(1).AddMinutes(-1) : null;
                    var m = getGrid.GetGrid_CustomerPO(txtPONo.Text.Trim(), txtCSTMNo.Text.Trim()
                        , cbbItem.SelectedValue.ToSt(), dFrom, dTo).Where(a=>a.OutSO>0);

                    dgvData.DataSource = m;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally
            {
                this.Cursor = Cursors.Default;
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
            if (dgvData.Rows.Count <= 0) return;

                try
                {
                    dgvData.EndEdit();

                    foreach (GridViewRowInfo rowinfo in dgvData.Rows.Where(o => Convert.ToBoolean(o.Cells["S"].Value)))
                    {
                        RetDT.Add(rowinfo);
                    }

                    this.Close();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            
          
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void radGridView1_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            ////select Item from Double click
            //selRow();

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
                    //var so = new SaleOrder(0);
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

        private void btnEditItem_Click(object sender, EventArgs e)
        {
            EditItem();
        }
        void EditItem()
        {
            if (dgvData.CurrentCell == null) return;
            int idCustomerPO = dgvData.CurrentCell.RowInfo.Cells["idCustomerPO"].Value.ToInt();

            CustomerPO c = new CustomerPO(idCustomerPO);
            c.ShowDialog();
            DataLoad();
        }

        private void btnViewItem_Click(object sender, EventArgs e)
        {
            EditItem();
        }

        private void btnDeleteItem_Click(object sender, EventArgs e)
        {
            Del();
        }
        void Del()
        {
            if (dgvData.CurrentCell == null) return;
            if (dgvData.CurrentCell.RowInfo.Cells["Status"].Value.ToSt() != "Waiting")
            {
                baseClass.Warning("Status Cannot Delete.\n");
                return;
            }

            int id = dgvData.CurrentCell.RowInfo.Cells["id"].Value.ToInt();
            using (var db = new DataClasses1DataContext())
            {
                var m = db.mh_CustomerPODTs.Where(x => x.id == id).FirstOrDefault();
                if (m != null)
                {
                    m.Active = false;
                    db.SubmitChanges();

                    int idCustomerPO = m.idCustomerPO;
                    var hd = db.mh_CustomerPOs.Where(x => x.id == idCustomerPO).FirstOrDefault();
                    if (hd != null)
                    {
                        hd.UpdateBy = ClassLib.Classlib.User;
                        hd.UpdateDate = DateTime.Now;
                        var j = db.mh_CustomerPODTs.Where(x => x.idCustomerPO == idCustomerPO && x.Active).ToList();
                        if (j.Count == 0)
                        {
                            hd.Active = false;
                        }
                        db.SubmitChanges();
                    }

                }

                dgvData.Rows.Remove(dgvData.CurrentCell.RowInfo);
                baseClass.Info("Delete complete.\n");
            }
        }
    }


}
