using System;
using System.Linq;
using System.Windows.Forms;
using ClassLib;
namespace StockControl
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //ConnectDB.Regedit();
            //ConnectDB.getConfig();//Get from setting

            ClassLib.Classlib.User = System.Environment.UserName;
            ClassLib.Classlib.DomainUser = System.Environment.UserDomainName;
            ClassLib.Classlib.ScreenWidth = Screen.PrimaryScreen.Bounds.Width;
            ClassLib.Classlib.ScreenHight = Screen.PrimaryScreen.Bounds.Height;
            Report.CRRReport.ServerName = "";
            Report.CRRReport.DbName = "";
            Report.CRRReport.dbUser = "";
            Report.CRRReport.dbPass = "";
            Report.CRRReport.dbPartReport = "";

            Report.CRRReport.dbPartReport = AppDomain.CurrentDomain.BaseDirectory + @"Report\";
            string apc = Properties.Settings.Default.dbStockControlConnectionString1;
            try
            {
                if (!apc.Equals(""))
                {
                    //Data Source=XTH-TOO\SQLEXPRESS;Initial Catalog=dbStockControl;User ID=sa;Password=;
                    string[] a = apc.Split(';');
                    string[] b = a[0].Split('=');
                    string[] c = a[1].Split('=');
                    string[] d = a[2].Split('=');
                    string[] f = a[3].Split('=');
                    Report.CRRReport.ServerName = b[1];
                    Report.CRRReport.DbName = c[1];
                    Report.CRRReport.dbUser = d[1];
                    Report.CRRReport.dbPass = f[1];

                    ConnectDB.server = b[1];
                    ConnectDB.dbname = c[1];
                    ConnectDB.Userdb = d[1];
                    ConnectDB.PassDb = f[1];

                    ConnectDB.dbconnection = "Data Source=" + ConnectDB.server + ";Initial Catalog=" + ConnectDB.dbname + ";User ID=" + ConnectDB.Userdb + ";Password=" + ConnectDB.PassDb + ";";//Connection Timeout=" + Timeout +";";
                    // txtServer.Text = b[1];
                    // txtDatabase.Text = c[1];
                    // tbUser.Text = d[1];
                    // tbPass.Text = f[1];
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Mainfrom());
            Application.Run(new Login());
        }
    }
}