using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using System.Linq;

namespace StockControl
{
    public partial class CustomerPOSummary_Report : Telerik.WinControls.UI.RadRibbonForm
    {
        //0:วัน
        //1:เดือน
        int tReport = 0;
        public CustomerPOSummary_Report(int typeReport)
        {
            InitializeComponent();
            this.tReport = typeReport;
        }

        private void CustomerPO_List_Print_Load(object sender, EventArgs e)
        {
            LoadDef();

            if (tReport == 1)
            {
                lbTo.Visible = true;
                cbbMM2.Visible = true;
            }
        }
        void LoadDef()
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

                int mm = 1;
                while (mm <= 12)
                {
                    DateTime dTemp = new DateTime(yy, mm, 1);
                    cbbMM.Items.Add(dTemp.ToString("MMM"));
                    cbbMM2.Items.Add(dTemp.ToString("MMM"));
                    mm++;
                }
                cbbMM.SelectedIndex = DateTime.Now.Month - 1;
                cbbMM2.SelectedIndex = DateTime.Now.Month - 1;

                var Customers = db.mh_Customers.Where(x => x.Active)
                    .Select(x => new CustomerCombo { No = x.No, Name = x.Name }).ToList();
                Customers.Add(new CustomerCombo
                {
                    Name = "",
                    No = "",
                });
                Customers = Customers.OrderBy(x => x.No).ToList();
                cbbCSTM.AutoSizeDropDownToBestFit = true;
                cbbCSTM.DisplayMember = "Name";
                cbbCSTM.ValueMember = "No";
                cbbCSTM.DataSource = Customers;
                cbbCSTM.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cbbCSTM.SelectedIndex = 0;

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



        private void radButtonElement1_Click(object sender, EventArgs e)
        {
            PrintE();
        }
        void PrintE()
        {
            if (tReport == 0)
            {
                Report.Reportx1.Value = new string[4];
                Report.Reportx1.Value[0] = cbbYY.Text.ToSt();
                Report.Reportx1.Value[1] = (cbbMM.SelectedIndex + 1).ToSt();
                Report.Reportx1.Value[2] = cbbItem.SelectedValue.ToSt();
                Report.Reportx1.Value[3] = cbbCSTM.SelectedValue.ToSt();
                Report.Reportx1.WReport = "DeliveryPlanning";
                Report.Reportx1 op = new Report.Reportx1("DeliveryPlanning.rpt");
                op.Show();
            }
            else if (tReport == 1)
            {
                if (cbbMM.SelectedIndex > cbbMM2.SelectedIndex)
                {
                    baseClass.Warning("ไม่สามารถเลือกเดือนเริ่มต้นมากกว่าเดือนสิ้นสุดได้.\n");
                    return;
                }

                Report.Reportx1.Value = new string[5];
                Report.Reportx1.Value[0] = cbbYY.Text.ToSt();
                Report.Reportx1.Value[1] = (cbbMM.SelectedIndex + 1).ToSt();
                Report.Reportx1.Value[2] = (cbbMM2.SelectedIndex + 1).ToSt();
                Report.Reportx1.Value[3] = cbbItem.SelectedValue.ToSt();
                Report.Reportx1.Value[4] = cbbCSTM.SelectedValue.ToSt();
                Report.Reportx1.WReport = "DeliveryPlanningMonth";
                Report.Reportx1 op = new Report.Reportx1("DeliveryPlanningMonth.rpt");
                op.Show();
            }
        }


    }
}
