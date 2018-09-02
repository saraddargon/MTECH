using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace StockControl
{
    public static class ConnectDB
    {
        public static string Version = "1.0";
        public static string dbname = "";
        public static string server = "";
        public static string Userdb = "sa";
        public static string PassDb = "";

        public static string user = "";
        public static string dbconnection = "";
        public static string Timeout = "1800";

        public static void Regedit()
        {
            try
            {
                bool isForce = true;
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SAP BusinessObjects\Crystal Reports for .NET Framework 4.0\Crystal Reports\Export\Pdf", true);

                if (key == null)
                {
                    key = Registry.CurrentUser.OpenSubKey(
                    @"Software\SAP BusinessObjects\Crystal Reports for .NET Framework 4.0\Crystal Reports\Export", true).CreateSubKey("Pdf");
                }
                key.SetValue("ForceLargerFonts", isForce, RegistryValueKind.DWord);
                key.Close();
            }
            catch { }
        }
        public static void getConfig()
        {
            try
            {
                //string FilePaht = Properties.Settings.Default.dbStockControlConnectionString3;
                string FilePaht = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Setting.xml");
                try
                {
                    //MessageBox.Show(FilePaht);
                    if (System.IO.File.Exists(FilePaht))
                    {
                        //MessageBox.Show("YES");
                        System.IO.File.Copy(AppDomain.CurrentDomain.BaseDirectory + "Setting.xml", FilePaht, true);
                    }
                    else
                    {
                        // MessageBox.Show("NO");
                        System.IO.File.Copy(AppDomain.CurrentDomain.BaseDirectory + "Setting.xml", FilePaht, true);
                    }
                }
                catch { }

                // XElement Setting2 = XElement.Load(AppDomain.CurrentDomain.BaseDirectory + "Setting.xml");
                XElement Setting2 = XElement.Load(FilePaht);
                XElement xeAll2 = new XElement("NewSet2", from c1 in Setting2.Elements() select c1);
                server = xeAll2.Elements("server").First().Value.ToString();
                dbname = xeAll2.Elements("dbname").First().Value.ToString();
                //dbnameJTEKT = xeAll2.Elements("dbnameJTEKT").First().Value.ToString();
                Userdb = xeAll2.Elements("userdb").First().Value.ToString();
                PassDb = xeAll2.Elements("passdb").First().Value.ToString();
                user = xeAll2.Elements("user").First().Value.ToString();
                dbconnection = "Data Source=" + server + ";Initial Catalog=" + dbname + ";User ID=" + Userdb + ";Password=" + PassDb + ";";//Connection Timeout=" + Timeout +";";
                
                // db = new dbStoreClass.DataClasses1DataContext(dbconnection);
            }
            catch { }
        }
        public static void SetSqlconn(string server, string Dbname, string LoginUser, string LoginPass, string User1)
        {
            try
            {


                //<?xml version="1.0" encoding="utf-8"?>
                //<configuration>
                //  <server>MAC-PC</server>
                //  <dbname>DBJTEKT</dbname>
                //  <dbnameJTEKT>JATHDAT</dbnameJTEKT>
                //  <userdb>sa</userdb>
                //  <passdb></passdb>
                //  <user></user>
                //</configuration>
                string FilePaht = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Setting.xml");

                StringBuilder sb = new StringBuilder();
                sb.Remove(0, sb.Length);
                sb.Append("<?xml version='1.0' encoding='utf-8' ?>");
                sb.Append("<configuration>");
                sb.Append("<server>" + server + "</server>");
                sb.Append("<dbname>" + Dbname + "</dbname>");
                //sb.Append("<dbnameJTEKT>" + DbJTekt + "</dbnameJTEKT>");
                sb.Append("<userdb>" + LoginUser + "</userdb>");
                sb.Append("<passdb>" + LoginPass + "</passdb>");
                sb.Append("<user>" + User1 + "</user>");
                sb.Append("</configuration>");
                string StrXml = sb.ToString();
                XElement xe = XElement.Parse(StrXml);
                xe.Save(FilePaht);
                //if (chk == 0)
                //    MessageBox.Show("Save Completed");
                sb = null;
                xe = null;
                StrXml = null;
                //GetSqlconn();
            }
            catch { }
        }


        public static void ChangedbConnection(string serverx, string dbnamex)
        {
            dbconnection = "Data Source=" + serverx + ";Initial Catalog=" + dbnamex + ";User ID=" + Userdb + ";Password=" + PassDb + ";";
        }

    }
}
