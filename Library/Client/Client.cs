/*
 * Created by SharpDevelop.
 * User: chepchip
 * Date: 11/11/2016
 * Time: 12:28 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
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
    public partial class Client : Form
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

        private TCPModel tcpForPlayer;
        private TCPModel tcpForOpponent;
        private int timeWait ;
        private string pointOfMe = "";
        public void Connect()
        {
            string ip = "127.0.0.1";
            int port = 13000;
            int tryConnect = 0;
           
            while (tryConnect<1)
            {
            
                tcpForPlayer = new TCPModel(ip, port);
                tryConnect = tcpForPlayer.ConnectToServer();
                Thread.Sleep(500);
            }
            this.Text = tcpForPlayer.UpdateInformation();
            tcpForOpponent = new TCPModel(ip, port);
            tcpForOpponent.ConnectToServer();
            tcpForOpponent.UpdateInformation();
            lbConnected.Visible = true;
            Thread t2 = new Thread(OpponentResult);
            t2.Start();
        }

        void MainFormLoad(object sender, System.EventArgs e)
        {
            lbConnected.Visible = false;
            CheckForIllegalCrossThreadCalls = false;
            Thread t1 = new Thread(Connect);
            t1.Start();
          
        }
        public void OpponentResult()
        {
            while (true)
            {
                string str = tcpForOpponent.ReadData();
                textBox4.Text = str;
            }
        }

        void PlayClick(object sender, EventArgs e)
        {
            tcpForPlayer.SendData("tung");
            pointOfMe = tcpForPlayer.ReadData();
            WaitResults();
          
            button2.Enabled = false;
        }

        private void WaitResults()
        {
            timeWait = 300;
            timer1 = new System.Windows.Forms.Timer();
            timer1.Interval = 1;
            timer1.Tick += new EventHandler(timer1_tick);
            timer1.Enabled = true;
        }

        private void timer1_tick(object sender, EventArgs e)
        {
            timeWait--;
            Random a = new Random();
            lbScore.Text = a.Next(1,6).ToString();
            if (timeWait < 0) {
                timer1.Stop();
                UpdatePointOfMe();
                return;
            }
        }

        private void UpdatePointOfMe()
        {
            if (pointOfMe.Length > 1) return;

            textBox3.Text = pointOfMe;
            lbScore.Text = pointOfMe;
        }

        private void ReplayClick(object sender, EventArgs e)
        {
          
        }
    }
}
