/*
/**
 * TRAN TRUNG TIEN 22/11/2017
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using Server.Common;
using System.IO;
using Server.View;
using Server.View;

namespace Server
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class ClientForm : System.Windows.Forms.Form
    {

        public ClientForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }
        private List<String> listBooksCurrent;
        private static TCPModel client;
        private ChangeBookForm frmChangeBook;
        private LoginForm frmLogin;
        private ShowInfoForm frmShowInfo;
        public static ViewFile frmViewBook;
        public void Connect()
        {
            string ip = tbIpAddress.Text;
            int port = Int32.Parse(tbPort.Text);

            client = new TCPModel(ip, port);
            if (client.ConnectToServer() == 1)
            {
                this.Text = client.UpdateInformation();
                TCPModel.progressBar= this.progressBar;
                TCPModel.wallet = this.tbWallet;
                lbConnected.Visible = true;
                loginToolStripMenuItem.Enabled = true;
                btnConnect.Enabled = false;
            }
        }

        public void ChangeBook(string name)
        {
            client.SendData(ClientManager.TYPE_CHANGE + "");
            Book b = new Book.Builder().Path(name).Build();
            client.SendBook(b);

        }

        #region Event
        void MainFormLoad(object sender, System.EventArgs e)
        {
            lbConnected.Visible = false;
            CheckForIllegalCrossThreadCalls = false;
            changeBookToolStripMenuItem.Enabled = false;
            btnSearch.Enabled = false;
            loginToolStripMenuItem.Enabled = false;
            frmViewBook = new ViewFile();
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            if (lbConnected.Visible == true)
                MessageBox.Show("Connected to Server!", "Info", MessageBoxButtons.OK);
            else
                Connect();
            loginToolStripMenuItem.PerformClick();
        }

        private void Search_Click(object sender, EventArgs e)
        {
            if (TCPModel.isDownloading)
            {
                MessageBox.Show("Please wait...");
                return;
            }
            if (tbSearch.Text.ToString().Trim().Equals(""))
            {
                MessageBox.Show("Input can't be blank", "Client", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }
            if (cbType.Text.Equals(""))
            {
                MessageBox.Show("Type is invalid", "Client", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }
            btnSearch.Enabled = false;
            client.SendData(ClientManager.TYPE_SEARCH.ToString() + ClientManager.SIGN + tbSearch.Text + ClientManager.SIGN + cbType.Text);
            updateView();
        }
        private void dgvBooks_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (TCPModel.isDownloading)
                {
                    MessageBox.Show("Please wait...");
                    return;
                }

                String id = dgvBooks.Rows[e.RowIndex].Cells[0].Value.ToString();
                String name = dgvBooks.Rows[e.RowIndex].Cells[1].Value.ToString();
                String type = dgvBooks.Rows[e.RowIndex].Cells[2].Value.ToString();
                String price = dgvBooks.Rows[e.RowIndex].Cells[3].Value.ToString();
                String size = dgvBooks.Rows[e.RowIndex].Cells[4].Value.ToString();

                //File extension is not supported
                if (type.Length<4)
                {
                    MessageBox.Show("File extension is not supported", "Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                using (frmShowInfo = new ShowInfoForm()
                {
                    book = new Book.Builder()
                            .Name(name)
                            .Price(Double.Parse(price))
                            .Size(size)
                            .Build()
                })
                {
                    switch (frmShowInfo.ShowDialog())
                    {
                        case DialogResult.Yes:
                            if (Double.Parse(price) > ClientManager.myWallet)
                            {
                                MessageBox.Show("Your balance is not enough to pay", "Error", MessageBoxButtons.OK);
                                break;
                            }
                            TCPModel.typeCurrent = ClientManager.TYPE_DOWNLOAD;
                            client.SendData(ClientManager.TYPE_DOWNLOAD.ToString() + ClientManager.SIGN + id);
                            break;
                        case DialogResult.No:
                            TCPModel.typeCurrent = ClientManager.TYPE_PREVIEW;
                            client.SendData(ClientManager.TYPE_PREVIEW.ToString() + ClientManager.SIGN + id);
                            break;
                        default:
                            return;
                    }
                }
            }
            catch (Exception E)
            {
                return;
            }
            frmViewBook = new ViewFile();
            client.setFormDetail(frmViewBook);
            //if()
            client.ReceiveBook();

        }

        private void changeBookToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (frmChangeBook = new ChangeBookForm())
            {
                if (frmChangeBook.ShowDialog() == DialogResult.OK)
                    this.ChangeBook(frmChangeBook.nameCurrent);
            }

        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmLogin = new LoginForm())
            {
                if (frmLogin.ShowDialog() == DialogResult.Yes)
                {
                    if (frmLogin.name.Equals("") || frmLogin.pass.Equals(""))
                    {
                        MessageBox.Show("Login Failed");
                        return;
                    }

                    client.SendData(ClientManager.TYPE_LOGIN.ToString() 
                                    +ClientManager.SIGN
                                      + frmLogin.name
                                      + ClientManager.SIGN
                                      + frmLogin.pass);
                    String rc = client.ReadData();
                    String[] rcs = rc.Split(ClientManager.SIGN);
                    if (rcs[0].Equals("LOGIN_SUCCESS"))
                    {
                        ClientManager.myWallet = Int32.Parse(rcs[2]);
                        MessageBox.Show("Login success");

                        tbWallet.Text = ClientManager.myWallet.ToString();
                        changeBookToolStripMenuItem.Enabled = true;
                        loginToolStripMenuItem.Enabled = false;
                        btnSearch.Enabled = true;
                        lbUserName.Text = rcs[1];
                    }
                    else
                        MessageBox.Show("Login Failed");
                }

            }

        }
        #endregion
        private void updateView()
        {
            dgvBooks.Rows.Clear();
            listBooksCurrent = client.ReadListData();
            int k = 0;
            if (listBooksCurrent == null)
                listBooksCurrent = new List<string>();
            foreach (String s in listBooksCurrent)
            {

                String[] a = s.Split(ClientManager.SIGN);
                Book b = new Book.Builder()
                    .Id(a[0])
                    .Name(a[1])
                    .Type(a[2])
                    .Price(Int32.Parse(a[3]))
                    .Size(a[4])
                    .Build();
                bookBindingSource.Add(b);
                this.dgvBooks.Rows[k].HeaderCell.Value = (++k).ToString();
                dgvBooks.RowsDefaultCellStyle.BackColor = Color.LightBlue;
                dgvBooks.AlternatingRowsDefaultCellStyle.BackColor = Color.LightSkyBlue;
            }
            btnSearch.Enabled = true;
        }

        
    }
}
