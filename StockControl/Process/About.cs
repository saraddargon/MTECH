﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace StockControl
{
    public partial class About : Telerik.WinControls.UI.RadForm
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            AdjustStock_Taking_Card_2 a = new AdjustStock_Taking_Card_2();
            a.ShowDialog();
        }
    }
}
