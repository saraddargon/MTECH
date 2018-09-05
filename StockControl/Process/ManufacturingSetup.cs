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
    public partial class ManufacturingSetup : Telerik.WinControls.UI.RadRibbonForm
    {
        public ManufacturingSetup()
        {
            InitializeComponent();
        }

        //private int RowView = 50;
        //private int ColView = 10;
        DataTable dt = new DataTable();
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
        private void GETDTRow()
        {
            dt.Columns.Add("No", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Address", typeof(string));
            dt.Columns.Add("VATRegistration", typeof(bool));
            dt.Columns.Add("PhoneNo", typeof(string));
            dt.Columns.Add("FaxNo", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("ContactName", typeof(string));
            dt.Columns.Add("VendorGroup", typeof(int));
            dt.Columns.Add("VatGroup", typeof(int));
            dt.Columns.Add("DefaultCurrency", typeof(int));
            dt.Columns.Add("ShippingTime", typeof(int));
            dt.Columns.Add("PriceIncludeingVat", typeof(bool));
            dt.Columns.Add("ReceiveAddress", typeof(string));
            dt.Columns.Add("AttachFile", typeof(string));

        }
        System.Drawing.Font MyFont;
        private void Unit_Load(object sender, EventArgs e)
        {
            GETDTRow();
            //for (int i = 0; i <= RowView; i++)
            //{
            //    DataRow rd = dt.NewRow();
            //    rd["UnitCode"] = "";
            //    rd["UnitDetail"] = "";
            //    rd["UnitActive"] = false;
            //    dt.Rows.Add(rd);
            //}


            DataLoad();
            LoadDefualt();
            MyFont = new System.Drawing.Font(
    "Tahoma", 9,
    FontStyle.Italic,    // + obviously doesn't work, but what am I meant to do?
    GraphicsUnit.Pixel);
        }

        private void RMenu6_Click(object sender, EventArgs e)
        {
            //ลบ
            //throw new NotImplementedException();
            btnDelete_Click(sender, e);
        }

        private void RMenu5_Click(object sender, EventArgs e)
        {
            btnEdit_Click(sender, e);
        }

        private void RMenu4_Click(object sender, EventArgs e)
        {
            ////เพิ่มผู้ขาย
            //throw new NotImplementedException();
            btnNew_Click(sender, e);
        }

        private void RMenu3_Click(object sender, EventArgs e)
        {
        }

        private void RadMenuItem3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("menu");
            // throw new NotImplementedException();
        }
        
        private void LoadDefualt()
        {
            try
            {

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var unit = db.mh_UnitTimes.ToList();
                    cbbShowCapa.DisplayMember = "Name";
                    cbbShowCapa.ValueMember = "id";
                    cbbShowCapa.DataSource = unit;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("Default", ex.Message, this.Name);
            }
        }
        private void DataLoad()
        {
            //dt.Rows.Clear();
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                var t = db.mh_ManufacturingSetups.FirstOrDefault();
                if(t != null)
                {
                    DateTime dd = new DateTime(1999, 1, 1, t.StandardStartingTime.Hours, t.StandardStartingTime.Minutes, 0);
                    DateTime d2 = new DateTime(1999, 1, 1, t.StandardEndingTime.Hours, t.StandardEndingTime.Minutes, 0);
                    txtStartingTime.Value = dd;
                    txtEndingTime.Value = d2;
                    cbbShowCapa.SelectedValue = t.ShowCapacityInUOM;
                }
            }


            //    radGridView1.DataSource = dt;
        }

        private bool AddUnit()
        {
            bool ck = false;
            int C = 0;
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    var t = db.mh_ManufacturingSetups.FirstOrDefault();
                    if(t != null)
                    {
                        t.StandardStartingTime = new TimeSpan(txtStartingTime.Value.Value.Hour, txtStartingTime.Value.Value.Minute, 0);
                        t.StandardEndingTime = new TimeSpan(txtEndingTime.Value.Value.Hour, txtEndingTime.Value.Value.Minute, 0);
                        t.ShowCapacityInUOM = cbbShowCapa.SelectedValue.ToInt();
                    }
                    else
                    {
                        t = new mh_ManufacturingSetup();
                        t.ShowCapacityInUOM = cbbShowCapa.SelectedValue.ToInt();
                        t.StandardStartingTime = new TimeSpan(txtStartingTime.Value.Value.Hour, txtStartingTime.Value.Value.Minute, 0);
                        t.StandardEndingTime = new TimeSpan(txtEndingTime.Value.Value.Hour, txtEndingTime.Value.Value.Minute, 0);

                        db.mh_ManufacturingSetups.InsertOnSubmit(t);
                        
                    }
                    db.SubmitChanges();
                }

                baseClass.Info("Save complete.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("เพิ่มกะทำงาน", ex.Message, this.Name);
            }

            if (C > 0)
                MessageBox.Show("บันทึกสำเร็จ!");

            return ck;
        }
        private bool DeleteUnit()
        {
            bool ck = false;

            int C = 0;
            int m = 0;
            try
            {
                
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("ลบกะทำงาน", ex.Message, this.Name);
            }

            if (C > 0)
            {
                MessageBox.Show("ลบรายการ สำเร็จ!");
            }




            return ck;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DataLoad();
        }
        private void NewClick()
        {
        }
        private void EditClick()
        {
        }
        private void ViewClick()
        {
            DataLoad();
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            NewClick();

        }
        
        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditClick();
        }
        private bool Check_Save()
        {
            bool re = true;
            string err = "";
            try
            {

                

                if (!err.Equals(""))
                    MessageBox.Show(err);
                else
                    re = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError(this.Name, ex.Message, this.Name);
            }

            return re;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Check_Save())
                return;

            if (MessageBox.Show("ต้องการบันทึก ?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                AddUnit();
                DataLoad();
            }
        }
        
        private void Unit_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // MessageBox.Show(e.KeyCode.ToString());
        }
        
        private void btnDelete_Click(object sender, EventArgs e)
        {

            DeleteUnit();
            DataLoad();

        }
        

        private void ImportData()
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {

                    foreach (DataRow rd in dt.Rows)
                    {
                        if (!rd["VendorName"].ToString().Equals(""))
                        {

                            var x = (from ix in db.tb_Vendors where ix.VendorNo.ToLower().Trim() == rd["VendorNo"].ToString().ToLower().Trim() select ix).FirstOrDefault();
                            //var x = db.mh_Vendors.Where(x => x.No.ToLower() == rd["VendorNo"].ToSt().ToLower().Trim()).FirstOrDefault();

                            if (x == null)
                            {

                                tb_Vendor ts = new tb_Vendor();
                                ts.VendorNo = dbClss.GetNo(1, 2);
                                ts.VendorName = Convert.ToString(rd["VendorName"].ToString());
                                ts.Address = Convert.ToString(rd["Address"].ToString());
                                ts.CRRNCY = Convert.ToString(rd["CRRNCY"].ToString());
                                ts.Remark = Convert.ToString(rd["Remark"].ToString());
                                ts.Active = Convert.ToBoolean(rd["Active"].ToString());
                                db.tb_Vendors.InsertOnSubmit(ts);
                                db.SubmitChanges();

                            }
                            else
                            {
                                x.VendorName = Convert.ToString(rd["VendorName"].ToString());
                                x.Address = Convert.ToString(rd["Address"].ToString());
                                x.CRRNCY = Convert.ToString(rd["CRRNCY"].ToString());
                                x.Remark = Convert.ToString(rd["Remark"].ToString());
                                x.Active = Convert.ToBoolean(rd["Active"].ToString());
                                db.SubmitChanges();
                            }


                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("InportData", ex.Message, this.Name);
            }
        }

        private void btnFilter1_Click(object sender, EventArgs e)
        {

        }

        private void btnUnfilter1_Click(object sender, EventArgs e)
        {

        }

        private void radMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        

    }
}
