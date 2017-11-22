
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;

/**
 * TRAN TRUNG TIEN 22/11/2017
 */
 
namespace Library
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class ServerForm : Form
    {
      
        public ServerForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        private TCPModel tcp;
        ServerModel server;
        SocketModel currentSocket;
        void MainFormLoad(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;

            server = new ServerModel().GetInstance();

        }

        public void StartServer()
        {
            string ip = tbIpAddress.Text;
            int port = int.Parse(tbPort.Text);
            tcp = new TCPModel(ip, port);
            tcp.Listen();
            btnStart.Enabled = false;
        }

        public void Commmunication(object obj)
        {
            SocketModel socket = (SocketModel) obj;
            while (true)
            {
                string str = socket.ReceiveData();
                //Remove socket fail : disconnect
                if (str.Equals(SocketModel.CLIENT_DISSCONECT + socket.GetRemoteEndpoint()))
                {
                    tbLogConnect.AppendText(str + "\r\n");
                    Console.WriteLine(str);
                    server.Remove(socket);
                    return;
                }
                Random random = new Random();
                int num = random.Next(1, 6);
                string result = num.ToString();
                BroadcastResult(socket, result);
            }
        }
        public void BroadcastResult(SocketModel socket, string result)
        {
            socket.SendData(result);
        }
        public void ServeClients()
        {
            while (true)
            {
                int status = -1;
                Socket s = tcp.SetUpANewConnection(ref status);
                currentSocket = new SocketModel(s);
                server.Add(currentSocket);
                string str = currentSocket.GetRemoteEndpoint();
                string str1 = "New connection from: " + str + "\r\n";
                
                tbLogConnect.AppendText(str1);
                lbNumberClient.Text = server.GetSocketCounts().ToString();

                Thread t = new Thread(Commmunication);
                t.Start(currentSocket);
            }
        }
        void StartClick(object sender, EventArgs e)
        {
            LoadData();
            StartServer();
            Thread t = new Thread(ServeClients);
            t.Start();

            Book b = new Book();

        }

        private void LoadData()
        {
            DataBase.InitDB(DataTest.GetListBook());


        }
    }
}
