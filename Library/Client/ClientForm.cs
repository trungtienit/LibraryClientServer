/*
/**
 * TRAN TRUNG TIEN 22/11/2017
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

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
            client.SendData("tung");
            tbSearch.Text = client.ReadData();
        }
    }
}
