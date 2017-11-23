
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using Server.Common;
using System.IO;

/**
 * TRAN TRUNG TIEN 22/11/2017
 */

namespace Server
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
#region Event
        void MainFormLoad(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;

            server = new ServerModel().GetInstance();

        }
        void StartClick(object sender, EventArgs e)
        {
            LoadData();
            StartServer();
            Thread t = new Thread(ServeClients);
            t.Start();
        }
        #endregion
#region Funtion Communication Server-client
        public void StartServer()
        {
            string ip = tbIpAddress.Text;
            int port = int.Parse(tbPort.Text);
            tcp = new TCPModel(ip, port);
            tcp.Listen();
            btnStart.Enabled = false;
        }
        public void Communication(object obj)
        {
            string result = "";
            List<String> results = new List<string>(); ;
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
                string[] receive = str.Split(ServerManager.SIGN);
                if (receive[0].Equals(ServerManager.TYPE_SEARCH.ToString()))
                    socket.SendData(results = SearchView(receive[1], receive[2]));
          
            //    BroadcastResult(socket, results);
            }
        }

        private void BroadcastResult(SocketModel socket, List<string> results)
        {
            socket.SendData(results);
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

                Thread t = new Thread(Communication);
                t.Start(currentSocket);
            }
        }
        private List<string> SearchView(string name, string type)
        {
            List<String> l= new List<string>();

            foreach(Book b in DataBase.GetListBook())
            {
                if (b.Name.ToUpper().Contains(name.ToUpper()) && type.ToUpper().Equals(type.ToUpper()))
                    l.Add(b.ToString());
            }
            return l;
        }

        #endregion

        private void LoadData()
        {
            DataBase.GetListBook();
        }
    }
}
