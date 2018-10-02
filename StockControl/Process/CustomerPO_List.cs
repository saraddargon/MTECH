using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using ClassLib;

namespace StockControl
{
    public partial class CustomerPO_List : Telerik.WinControls.UI.RadRibbonForm
    {
        public int idCustomerPO { get; set; }

        //sType = 1 : btnNew to Create Customer P/O,,, 2: btnNew to Select Customer P/O
        int sType = 1;

        public CustomerPO_List(int sType = 1)
        {
            InitializeComponent();
            this.sType = sType;
        }
        public CustomerPO_List()
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
            dgvData.AutoGenerateColumns = false;
            cbbStatus.SelectedIndex = 1; //Waiting
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
                        , cbbItem.SelectedValue.ToSt(), dFrom, dTo, cbbStatus.Text);

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
            //dbClss.ExportGridXlSX(dgvData);

            this.Cursor = Cursors.WaitCursor;
            try
            {
                string fName = "CustomerPO_Template.csv";
                string mFile = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, fName);
                string tFile = Path.Combine(Path.GetTempPath(), fName);
                File.Copy(mFile, tFile, true);

                System.Diagnostics.Process.Start(tFile);
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
            DataLoad();
        }

        private void radGridView1_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            ////select Item from Double click
            if(e.RowIndex >= 0)
                selRow();

        }
        void selRow()
        {
            if (dgvData.CurrentCell != null && dgvData.CurrentCell.RowIndex >= 0)
            {
                var rowe = dgvData.CurrentCell.RowInfo;
                idCustomerPO = rowe.Cells["idCustomerPO"].Value.ToInt();

                if (dgvData.CurrentCell.ColumnInfo.Name.Equals("JobNo"))
                {
                    string jobNo = rowe.Cells["JobNo"].Value.ToSt();
                    if (!baseClass.Question("Do you want to open 'Job Order sheet' ?"))
                        return;
                    var m = new ProductionOrder(jobNo);
                    m.ShowDialog();
                }
                else
                {
                    if (sType == 1)
                    {
                        var p = new CustomerPO(idCustomerPO);
                        p.ShowDialog();
                        DataLoad();
                    }
                    else
                        this.Close();
                }
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
            Report.Reportx1.Value = new string[1];
            Report.Reportx1.Value[0] = "";
            Report.Reportx1.WReport = "CustomerList";
            Report.Reportx1 op = new Report.Reportx1("ReportCustomerList.rpt");
            op.Show();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            var op = new OpenFileDialog();
            op.Filter = "(*.csv)|*.csv";
            if (op.ShowDialog() == DialogResult.OK && op.FileName.ToSt() != "")
                OpenImport(op.FileName);
        }
        void OpenImport(string filePath)
        {
            //MessageBox.Show(filePath);
            this.Cursor = Cursors.WaitCursor;
            try
            {
                using (TextFieldParser parser = new TextFieldParser(filePath, Encoding.GetEncoding("windows-874")))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    int a = 0;
                    int c = 0;

                    int impCom = 0;
                    using (var db = new DataClasses1DataContext())
                    {
                        while (!parser.EndOfData)
                        {
                            a += 1;
                            string[] fields = parser.ReadFields();
                            c = 0;

                            string CustomerNo = "";
                            string CustomerPONo = "";
                            DateTime? OrderDate = null;
                            string Remark = "";
                            DateTime? CustomerReqDate = null;
                            string ItemNo = "";
                            string ItemName = "";
                            decimal OrderQty = 0.00m;
                            string UOM = "";
                            decimal PCSUnit = 0.00m;
                            decimal UnitPrice = 0.00m;
                            decimal Amnt = 0.00m;
                            foreach (string field in fields)
                            {
                                c += 1;
                                if (a < 8) continue;
                                string f = field.ToSt().Trim();
                                switch (c)
                                {
                                    case 2: CustomerNo = f; break;
                                    case 3: CustomerPONo = f; break;
                                    case 4: OrderDate = f.ToDateTime(); break;
                                    case 5: Remark = f; break;
                                    case 6: CustomerReqDate = f.ToDateTime(); break;
                                    case 7: ItemNo = f; break;
                                    case 8: ItemName = f; break;
                                    case 9: OrderQty = f.ToDecimal(); break;
                                    case 10: UOM = f; break;
                                    case 11: PCSUnit = f.ToDecimal(); break;
                                    case 12: UnitPrice = f.ToDecimal(); break;
                                    case 13: Amnt = f.ToDecimal(); break;
                                    default: break;
                                }
                            }
                            //check customerNo
                            var cstm = db.mh_Customers.Where(x => x.No == CustomerNo).FirstOrDefault();
                            if (cstm == null) continue;
                            //check OrderDate
                            if (OrderDate == null) continue;
                            //check CustomerReqDate
                            if (CustomerReqDate == null) continue;
                            //Check ItemNo
                            var tool = db.mh_Items.Where(x => x.InternalNo == ItemNo).FirstOrDefault();
                            if (tool == null) continue;
                            //Check Order Qty
                            if (OrderQty <= 0) continue;
                            //Check UOM
                            var unit = db.mh_ItemUOMs.Where(x => x.ItemNo == ItemNo && x.UOMCode == UOM).FirstOrDefault();
                            if (unit == null) continue;
                            //check PCSUnit
                            if (PCSUnit <= 0) PCSUnit = unit.QuantityPer;

                            Amnt = Math.Round(OrderQty * UnitPrice, 2);

                            //add to CustomerPO
                            var cstmPo = db.mh_CustomerPOs.Where(x => x.CustomerNo == CustomerNo && x.CustomerPONo == CustomerPONo).FirstOrDefault();
                            if (cstmPo == null)
                            {
                                cstmPo = new mh_CustomerPO();
                                db.mh_CustomerPOs.InsertOnSubmit(cstmPo);
                            }
                            cstmPo.Active = true;
                            cstmPo.CreateBy = Classlib.User;
                            cstmPo.CreateDate = DateTime.Now;
                            cstmPo.CustomerNo = CustomerNo;
                            cstmPo.CustomerPONo = CustomerPONo;
                            cstmPo.DemandType = 0;  //Customer P/O
                            cstmPo.OrderDate = OrderDate.Value.Date;
                            cstmPo.Remark = Remark;
                            cstmPo.UpdateBy = Classlib.User;
                            cstmPo.UpdateDate = DateTime.Now;
                            db.SubmitChanges();

                            //add to Customer PO Dt
                            decimal allQ = Math.Round(OrderQty * PCSUnit, 2);
                            var cstmpoDt = new mh_CustomerPODT
                            {
                                Active = true,
                                Amount = Amnt,
                                forSafetyStock = false,
                                genPR = false,
                                idCustomerPO = cstmPo.id,
                                ItemName = ItemName,
                                ItemNo = ItemNo,
                                OutPlan = OrderQty,
                                OutQty = allQ,
                                OutSO = OrderQty,
                                PCSUnit = PCSUnit,
                                Qty = OrderQty,
                                Remark = "",
                                ReplenishmentType = tool.ReplenishmentType,
                                ReqDate = CustomerReqDate.Value.Date,
                                ReqReceiveDate = CustomerReqDate.Value.Date,
                                Status = "Waiting",
                                UnitPrice = UnitPrice,
                                UOM = UOM,
                            };
                            db.mh_CustomerPODTs.InsertOnSubmit(cstmpoDt);
                            db.SubmitChanges();

                            impCom++;
                        }
                    }

                    if (impCom > 0)
                    {
                        baseClass.Info($"Improt Data({impCom}) completes.\n");
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

        private void cbbStatus_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {

        }
    }


}
