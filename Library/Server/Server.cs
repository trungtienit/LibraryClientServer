
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;

namespace Game_Xuc_xac
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class Server : Form
    {
        public Server()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        void MainFormLoad(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }

        private TCPModel tcp;
        private SocketModel[] socketList;
        private SocketModel[] socketList1;
        private int numberOfPlayers = 2;
        private int currentClient;

        public void StartServer()
        {
            string ip = textBox1.Text;
            int port = int.Parse(textBox2.Text);
            tcp = new TCPModel(ip, port);
            tcp.Listen();
            button1.Enabled = false;
        }
        public void ServeClients()
        {
            socketList = new SocketModel[numberOfPlayers];
            socketList1 = new SocketModel[numberOfPlayers];
            for (int i = 0; i < numberOfPlayers; i++)
            {
                ServeAClient();
            }
        }
        public void AcceptConnect1()
        {
            int status = -1;
            Socket s = tcp.SetUpANewConnection(ref status);
            socketList[currentClient] = new SocketModel(s);
            string str = socketList[currentClient].GetRemoteEndpoint();
            string str1 = "New connection from: " + str + "\n";
            textBox3.AppendText(str1);
        }
        public void AcceptConnect2()
        {
            int status = -1;
            Socket s = tcp.SetUpANewConnection(ref status);
            socketList1[currentClient] = new SocketModel(s);
        }

        public void ServeAClient()
        {
            AcceptConnect1();
            AcceptConnect2();
            currentClient++;
            Thread t = new Thread(Commmunication);
            t.Start(currentClient - 1);
        }
        public void Commmunication(object obj)
        {
            int pos = (Int32)obj;
            while (true)
            {
                string str = socketList[pos].ReceiveData();
                Console.WriteLine(str);
                Random random = new Random();
                int num = random.Next(1, 6);
                string result = num.ToString();
                BroadcastResult(pos, result);
            }
        }
        public void BroadcastResult(int pos, string result)
        {

            socketList[pos].SendData(result);
            if (pos == 0)
            {
                if (socketList1[1] != null)
                    socketList1[1].SendData(result);
                return;
            }
            socketList1[0].SendData(result);

        }

        void Button1Click(object sender, EventArgs e)
        {
            StartServer();
            Thread t = new Thread(ServeClients);
            t.Start();
        }
    }
}
