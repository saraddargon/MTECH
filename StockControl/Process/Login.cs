using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockControl
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void cbConfig_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if (cbConfig.Checked)
            {
                try
                {
                    lblDatabase.Visible = true;
                    lblServer.Visible = true;
                    ddlDatabase.Visible = true;
                    ddlServer.Visible = true;

                }
                catch { }
            }
            else
            {
                try
                {
                    lblDatabase.Visible = false;
                    lblServer.Visible = false;
                    ddlDatabase.Visible = false;
                    ddlServer.Visible = false;
                }
                catch { }
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            //getConfig();

            cbConfig.Checked = true;
            txtUser.Text = "";//ConnectDB.ConnectDB.user;
            ddlDatabase.Text = ConnectDB.dbname;
            ddlServer.Text = ConnectDB.server;
            //txtUser.Text = "admin";
            //txtPassword.Text = "1234";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
                Application.ExitThread();
                Environment.Exit(1);
            }
            catch { }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            //Check Data Version
            try
            {
                SetConfig();
                ConnectDB.dbconnection = "Data Source=" + ddlServer.Text + ";Initial Catalog="
                        + ddlDatabase.Text + ";User ID=" + ConnectDB.Userdb + ";Password=" + ConnectDB.PassDb + ";";
                //+ ";Connection Timeout=" + ConnectDB.ConnectDB.Timeout + ";"; //Integrated Security=SSPI; Connection Timeout=1800
                ConnectDB.user = txtUser.Text;
                ConnectDB.server = ddlServer.Text;
                ConnectDB.dbname = ddlDatabase.Text;
                ClassLib.Classlib.User = ConnectDB.user;

                ////OpenSqlConnection(); //Test Connection time out
                if (checkVr())
                {
                    //ConnectDB.ConnectDB.server = cboServer.Text;
                    //ConnectDB.ConnectDB.dbname = cboDatabase.Text;

                    this.Hide();
                    this.ShowInTaskbar = false;
                    Mainfrom rad = new Mainfrom();
                    rad.Show();
                }
            }
            //catch { }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //    dbclass.getDb.A00_ErrorLog(this.Name, ex.ToString(), ConnectDB.ConnectDB.user);
            }
        }
        private void SetConfig()
        {
            ConnectDB.SetSqlconn(ddlServer.Text, ddlDatabase.Text, ConnectDB.Userdb, ConnectDB.PassDb, txtUser.Text);
        }
        private bool checkVr()
        {
            bool ck = false;
            string err = "";
            if (txtUser.Text.Equals(""))
                err += "User Empty! \n";
            else if (txtPassword.Text.Equals(""))
                err += "Password Empty! \n";
            else if (ddlDatabase.Text.Equals(""))
                err += "Database Empty! \n";
            else if (ddlServer.Text.Equals(""))
                err += "server Empty! \n";

            if (ddlDatabase.Text.Equals("") && ddlServer.Text.Equals(""))
            {
                ConnectDB.ChangedbConnection(ddlServer.Text, ddlDatabase.Text);
            }
            //try
            //{
            //    var g = (from ix in dbclass.getDb.sp_SelectAdmin() select ix).ToList();
            //    if (g.Count > 0)
            //    {
            //        if (!g.FirstOrDefault().Version.Equals(ConnectDB.Version))
            //        {
            //            //err += "Version Not Match!! \n";

            //            if (MessageBox.Show("Do you want Update!! \n Version Not Match!! ", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //            {

            //            }
            //        }
            //    }
            //    else
            //    {
            //        err += "Database Not Connect!! \n";
            //    }
            //}
            //catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            try
            {
                if (err.Equals(""))
                {
                    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    {
                        var gx = (from ix in db.sp_SelectUser(txtUser.Text, txtPassword.Text) select ix).ToList();
                        if (gx.Count > 0)
                        {
                            ConnectDB.user = txtUser.Text;
                            ConnectDB.server = ddlServer.Text;
                            ConnectDB.dbname = ddlDatabase.Text;
                        }
                        else
                        {
                            err += "User or Password Invalid! \n";
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            if (err.Equals(""))
            {
                ck = true;
            }
            else
            {
                ck = false;
                MessageBox.Show(err);
            }

            return ck;
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnConnect_Click(sender, e);
            }
        }

        private void cbShow_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            //if (cbShow.Checked)
            //    txtPassword.PasswordChar = '*';
            //else
            //    txtPassword.PasswordChar = "-".ToCharArray();
        }
    }
}
