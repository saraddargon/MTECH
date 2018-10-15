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

namespace StockControl
{
    public partial class CustomerPOSummary : Telerik.WinControls.UI.RadRibbonForm
    {
        public int idCustomerPO { get; set; }

        public CustomerPOSummary()
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
            var sumRow = new GridViewSummaryRowItem();

            dgvData.SummaryRowsBottom.Add(sumRow);
            dgvData.MasterTemplate.ShowTotals = true;


            dgvData.AutoGenerateColumns = false;
            DataLoad();
        }

        private void LoadDef()
        {
            using (var db = new DataClasses1DataContext())
            {
                int yy = 2018;
                while (yy <= DateTime.Now.Year + 3)
                {
                    cbbYY.Items.Add(yy.ToSt());
                    yy++;
                }
                cbbYY.Text = DateTime.Now.Year.ToSt();

                //cbbMonth.Items.Add("");
                int mm = 1;
                while (mm <= 12)
                {
                    DateTime dTemp = new DateTime(yy, mm, 1);
                    cbbMonth.Items.Add(dTemp.ToString("MMM"));
                    mm++;
                }
                cbbMonth.SelectedIndex = DateTime.Now.Month - 1;

                var item = db.mh_Items.Where(x => x.Active && x.InventoryGroup == "FG")
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

                LoadCol();

                using (var db = new DataClasses1DataContext())
                {
                    int yy = cbbYY.Text.ToInt();
                    int mm = cbbMonth.SelectedIndex + 1;
                    string ItemNo = cbbItem.SelectedValue.ToSt();

                    //if (mm > 0)
                    //{
                    //    var m = db.sp_080_CustomerPOSummary_SELECT2(yy, mm, ItemNo).ToList();
                    //    dgvData.DataSource = m;
                    //}
                    //else
                    //{
                    //    var m = db.sp_080_CustomerPOSummary_SELECT(yy, ItemNo).ToList();
                    //    dgvData.DataSource = m;
                    //}
                    var m = db.RP_MH_CustomerPOSUmmary(yy, mm, ItemNo, DateTime.Now).ToList();
                    dgvData.DataSource = m;

                    dgvData.Rows.ToList().ForEach(x =>
                    {
                        x.Cells["Summary"].Value = sumRow(x);
                    });
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }
        decimal sumRow(GridViewRowInfo rowe)
        {
            decimal ret = 0.00m;
            //int mm = 1;
            //while(mm <= 12)
            //{
            //    DateTime d = new DateTime(DateTime.Now.Year, mm, 1);
            //    ret += rowe.Cells[d.ToString("MMM").ToUpper()].Value.ToDecimal();
            //    mm++;
            //}
            col_del_list.ForEach(x =>
            {
                if (x != "MM")
                    ret += rowe.Cells[x].Value.ToDecimal();
                
            });
            ret = Math.Round(ret, 2);
            return ret;

        }

        List<string> col_del_list = new List<string>();
        void LoadCol()
        {
            var sumRow = dgvData.SummaryRowsBottom.FirstOrDefault();
            sumRow.Clear();
            col_del_list.ForEach(x => dgvData.Columns.Remove(x));
            col_del_list.Clear();
            var sum2 = new GridViewSummaryItem("Summary", "{0:N2}", GridAggregateFunction.Sum);
            sumRow.Add(sum2);

            if (cbbMonth.SelectedIndex > 0)
            {
                //add Month Col
                DateTime dTemp = new DateTime(2018, cbbMonth.SelectedIndex + 1, 1);
                dgvData.Columns.Insert(dgvData.Columns.Count - 1, new GridViewDecimalColumn
                {
                    HeaderText = "MM",
                    Name = "MM",
                    FieldName = "MM",
                    FormatString = "{0:N0}",
                    Width = 70,
                });
                col_del_list.Add("MM");

                int dd = 1;
                int ddMax = DateTime.DaysInMonth(cbbYY.Text.ToInt(), cbbMonth.SelectedIndex + 1);
                while (dd <= ddMax)
                {
                    int cCol = dgvData.Columns.Count;
                    string cName = $"D{dd}";
                    var gcol = new GridViewDecimalColumn
                    {
                        HeaderText = dd.ToSt(),
                        Name = cName,
                        FieldName = cName + "_1",
                        FormatString = "{0:N2}",
                        Width = 70,
                    };
                    dgvData.Columns.Insert(cCol - 1, gcol);
                    col_del_list.Add(gcol.Name);

                    var sum1 = new GridViewSummaryItem(cName, "{0:N2}", GridAggregateFunction.Sum);
                    sumRow.Add(sum1);

                    dd++;
                }
            }
            else
            {
                int mm = 1;
                while (mm <= 12)
                {
                    DateTime d = new DateTime(2018, mm, 1);
                    int cCol = dgvData.Columns.Count;
                    var gcol = new GridViewDecimalColumn
                    {
                        HeaderText = d.ToString("MMM").ToUpper(),
                        Name = d.ToString("MMM").ToUpper(),
                        FieldName = d.ToString("MMM").ToUpper(),
                        FormatString = "{0:N2}",
                        Width = 70,
                    };
                    dgvData.Columns.Insert(cCol - 1, gcol);
                    col_del_list.Add(gcol.Name);

                    var sum1 = new GridViewSummaryItem(d.ToString("MMM").ToUpper(), "{0:N2}", GridAggregateFunction.Sum);
                    sumRow.Add(sum1);

                    mm++;
                }
            }

            //col_del_list.ForEach(x =>
            //{
            //    dgvData.Columns[x].ConditionalFormattingObjectList.Add(new ConditionalFormattingObject
            //    {
            //        CellForeColor = Color.Gray,
            //        Name = "E0",
            //        ConditionType = ConditionTypes.Equal,
            //        TValue1 = "0",
            //    });
            //});
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
                //var rowe = dgvData.CurrentCell.RowInfo;
                //idCustomerPO = rowe.Cells["idCustomerPO"].Value.ToInt();
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
                    List<int> idList = new List<int>();
                    var a = new List<int>();
                    foreach (var ix in dgvData.Rows)
                    {
                        if (dbClss.TBo(ix.Cells["S"].Value))
                        {
                            a.Add(dbClss.TInt(ix.Cells["id"].Value));
                            break;
                        }
                    }
                    var so = new SaleOrder(a);
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

        private void btn_PrintPR_Click_1(object sender, EventArgs e)
        {
            if(cbbMonth.SelectedIndex > 0)
            {
                Report.Reportx1.Value = new string[3];
                Report.Reportx1.Value[0] = cbbYY.Text.ToSt();
                Report.Reportx1.Value[1] = (cbbMonth.SelectedIndex + 1).ToSt();
                Report.Reportx1.Value[2] = cbbItem.SelectedValue.ToSt();
                Report.Reportx1.WReport = "DeliveryPlanning";
                Report.Reportx1 op = new Report.Reportx1("DeliveryPlanning.rpt");
                op.Show();
            }
        }

        private void dgvData_ViewRowFormatting(object sender, RowFormattingEventArgs e)
        {
            if (e.RowElement is GridSummaryRowElement)
            {
                e.RowElement.ForeColor = Color.Navy;
                var f = e.RowElement.Font;
                e.RowElement.Font = new Font(f, FontStyle.Bold);
            }
        }
    }


}
