using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace StockControl
{
    public partial class Show_Signature : Telerik.WinControls.UI.RadForm
    {
        public Show_Signature()
        {
            InitializeComponent();
        }
        List<GridViewRowInfo> RetDT;
        string Signature = "";
        public Show_Signature(string Signaturex)
        {
            InitializeComponent();
            Signature = Signaturex;
        }
        private void About_Load(object sender, EventArgs e)
        {
            try
            {
                if (Signature.Equals(""))
                {
                    MessageBox.Show("not found !!!");
                }
                //System.Drawing.Image img = Image.FromFile(Signature);
                Image img = Image.FromFile(Signature);//เอาชื่อที่เก็บไว้บน server มาเชื่อมต่อกับ Path 
                Bitmap BM = new Bitmap(img);
                pnImage.BackgroundImage = BM;
            }
            catch { }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //AdjustStock_Taking_Card_2 a = new AdjustStock_Taking_Card_2();
            //a.ShowDialog();
        }
    }
}
