﻿/*
/**
 * TRAN TRUNG TIEN 22/11/2017
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using Client.Common;
namespace Client
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class Client : System.Windows.Forms.Form
    {

        public Client()
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
        private TCPModel client;
        public void Connect()
        {
            string ip = tbIpAddress.Text;
            int port = Int32.Parse(tbPort.Text);

            client = new TCPModel(ip, port);
            if (client.ConnectToServer() == 1)
            {
                this.Text = client.UpdateInformation();
                lbConnected.Visible = true;
                //  Thread t2 = new Thread(OpponentResult);
                //t2.Start();
            }
        }
        #region Event
        void MainFormLoad(object sender, System.EventArgs e)
        {
            lbConnected.Visible = false;
            CheckForIllegalCrossThreadCalls = false;
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
                this.dgvBooks.Rows.Add();
                for (int i = 0; i < 5; i++)
                {
                    this.dgvBooks.Rows[k].Cells[i].Value = a[i];

                }
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
            }catch(Exception E)
            {
                return;
            }
                //  String fileName = dgvBooks.Rows[e.RowIndex].Cells[1].Value.ToString();
            if (!TCPModel.isDownloading)
                client.DownloadData();
            else
                MessageBox.Show("Please wait...");
        }
    }
}
