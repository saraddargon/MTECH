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
    public partial class CustomerPO_List_Print : Telerik.WinControls.UI.RadRibbonForm
    {
        public CustomerPO_List_Print()
        {
            InitializeComponent();
        }

        private void CustomerPO_List_Print_Load(object sender, EventArgs e)
        {
            dtFrom.Value = DateTime.Now;
            dtTo.Value = DateTime.Now;
        }



        private void radButtonElement1_Click(object sender, EventArgs e)
        {
            PrintE();
        }
        void PrintE()
        {
            Report.Reportx1.Value = new string[2];
            Report.Reportx1.Value[0] = dtFrom.Value.Date.ToString("dd/MMM/yyyy");
            Report.Reportx1.Value[1] = dtTo.Value.Date.ToString("dd/MMM/yyyy");
            Report.Reportx1.WReport = "ReceiveAndDelivery";
            Report.Reportx1 op = new Report.Reportx1("ReceiveAndDelivery.rpt");
            op.Show();
        }


    }
}
