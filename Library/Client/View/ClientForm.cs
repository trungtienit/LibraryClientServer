/*
/**
 * TRAN TRUNG TIEN 22/11/2017
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using Client.Common;
using System.IO;
using Client.View;

namespace Client
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
        public void Connect()
        {
            string ip = tbIpAddress.Text;
            int port = Int32.Parse(tbPort.Text);

            client = new TCPModel(ip, port);
            if (client.ConnectToServer() == 1)
            {
                this.Text = client.UpdateInformation();
                lbConnected.Visible = true;
                tbWallet.Text = ClientManager.myWallet.ToString();
                changeBookToolStripMenuItem.Enabled = true;
                btnSearch.Enabled = true;
            }
        }

        public void ChangeBook(string name)
        {
            
            client.SendData(ClientManager.TYPE_CHANGE + "");
            Book b = new Book.Builder().Path(name).Build();
            client.SendBook(b);
            try
            {
                int price = Int32.Parse(client.ReadData());
                ClientManager.myWallet += price;
                tbWallet.Text = ClientManager.myWallet.ToString();
                MessageBox.Show("You given " + price + "", "Admin", MessageBoxButtons.OK);
            }
            catch (Exception e) { };
            
        }

        #region Event
        void MainFormLoad(object sender, System.EventArgs e)
        {
            lbConnected.Visible = false;
            CheckForIllegalCrossThreadCalls = false;
            changeBookToolStripMenuItem.Enabled = false;
            btnSearch.Enabled = false;
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            if (lbConnected.Visible == true)
                MessageBox.Show("Connected to Server!", "Info", MessageBoxButtons.OK);
            else
                Connect();
        }

        private void Search_Click(object sender, EventArgs e)
        {
            if (cbType.Text.Equals(""))
            {
                MessageBox.Show("Type is invalid", "Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            client.SendData(ClientManager.TYPE_SEARCH.ToString() + ClientManager.SIGN + tbSearch.Text + ClientManager.SIGN + cbType.Text);
            updateView();
        }

        #endregion
        private void updateView()
        {
            dgvBooks.Rows.Clear();
            listBooksCurrent = client.ReadListData();
            int k = 0;

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
        }

        private void dgvBooks_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                client.SendData(ClientManager.TYPE_PREVIEW.ToString() + ClientManager.SIGN + dgvBooks.Rows[e.RowIndex].Cells[0].Value.ToString());
            }
            catch (Exception E)
            {
                return;
            }
            //  String fileName = dgvBooks.Rows[e.RowIndex].Cells[1].Value.ToString();
            if (!TCPModel.isDownloading)
                client.ReceiveBook();
            else
                MessageBox.Show("Please wait...");
        }

        private void changeBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangeBook = new ChangeBookForm();
            using (frmChangeBook)
            {
                if (frmChangeBook.ShowDialog() == DialogResult.OK)
                    this.ChangeBook(frmChangeBook.nameCurrent);
            }
            
        }

    }
}
