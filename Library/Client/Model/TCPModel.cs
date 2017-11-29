
using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Threading;
using Client.Common;

namespace Client
{
    /// <summary>
    /// Description of TCPModel.
    /// </summary>
    public class TCPModel
    {
        public static Boolean isDownloading = false;
        private TcpClient tcpclnt;
        private Stream stm;
        private byte[] byteSend;
        private byte[] byteReceive;
        private string IPofServer;
        private int port;

        public TCPModel(string ip, int p)
        {
            IPofServer = ip;
            port = p;
            tcpclnt = new TcpClient();
            byteReceive = new byte[100];
        }
        //show information of client to update UI
        public string UpdateInformation()
        {
            string str = "";
            try
            {
                Socket s = tcpclnt.Client;
                str = str + s.LocalEndPoint;
                Console.WriteLine(str);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
            return str;
        }
        //Set up a connection to server and get stream for communication
        public int ConnectToServer()
        {
            try
            {
                tcpclnt.Connect(IPofServer, port);
                stm = tcpclnt.GetStream();
                stm.ReadTimeout = 10000;
                Console.WriteLine("OK!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
                return -1;
            }
            return 1;
        }
        //Send data to server after connection is set up
        public void SendData(string str)
        {
            try
            {
                ASCIIEncoding asen = new ASCIIEncoding();
                byteSend = asen.GetBytes(str);
                stm.Write(byteSend, 0, byteSend.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }
        //Read data from server after connection is set up
        public string ReadData()
        {
            string str = "";
            try
            {
                //count the length of data received
                int k = stm.Read(byteReceive, 0, 100);
                Console.WriteLine("Length of data received: " + k.ToString());
                Console.WriteLine("From server: ");
                //conver the byte recevied into string
                char[] c = new char[k];
                for (int i = 0; i < k; i++)
                {
                    Console.Write(Convert.ToChar(byteReceive[i]));
                    c[i] = Convert.ToChar(byteReceive[i]);
                }
                str = new string(c);
            }
            catch (Exception e)
            {
                str = "Error..... " + e.StackTrace;
                Console.WriteLine(str);
            }
            return str;
        }

        internal List<String> ReadListData()
        {
            var bin = new BinaryFormatter();
            var list = (List<string>)bin.Deserialize(stm);
            return list;
        }

        //close connection
        public void CloseConnection()
        {
            tcpclnt.Close();
        }

        internal void DownloadData()
        {
            Thread t = new Thread(DownloadDataByThread);
            t.Start();

        }

        private void DownloadDataByThread()
        {
            isDownloading = true;
            BinaryWriter bWrite = null;
            int fileLenght = 0;
            int originalLength = 0;
            decimal byteReadAll = 0;
            try
            {
                String receivedPath = ClientManager.FOLDER_DOWLOAD;
                if (!Directory.Exists(receivedPath))
                    Directory.CreateDirectory(receivedPath);


                byte[] clientData = new byte[100];

                int byteReceive = stm.Read(clientData, 0, 4);
                int fileNameLen = BitConverter.ToInt32(clientData, 0);
                byteReceive = stm.Read(clientData, 0, 4);
                fileLenght = BitConverter.ToInt32(clientData, 0);
                originalLength = fileLenght;
                Console.WriteLine("fileNameLen = {0}", fileNameLen);

                stm.Read(clientData, 0, fileNameLen);

                String fileName = Encoding.ASCII.GetString(clientData, 0, fileNameLen);
                Console.WriteLine("fileName = {0}", fileName);
                int k = 1;
                while (File.Exists(receivedPath + "/" + fileName))
                {
                    if (fileName.StartsWith("("))
                        fileName = fileName.Substring(3);
                    fileName = "(" + k++ + ")" + fileName;
                }

                bWrite = new BinaryWriter(File.Open(receivedPath + "/" + fileName, FileMode.Append)); ;

                int bufferLength = 1024;
                byte[] buffer = new byte[bufferLength];

                int byteRead;

                while ((byteRead = stm.Read(buffer, 0, bufferLength)) > 0)
                {
                    if (fileLenght <= bufferLength)
                    {
                        bWrite.Write(buffer, 0, fileLenght);
                        byteReadAll += fileLenght;
                    }
                    else
                    {
                        bWrite.Write(buffer, 0, byteRead);
                        byteReadAll += byteRead;
                    }
                    if (byteReadAll == originalLength)
                    {
                        isDownloading = false;
                        Console.Write("Recieved all :" + byteReadAll);
                        bWrite.Close();
                        return;
                    }
                   
                    fileLenght -= bufferLength;

                    //DEBUG
                    Console.WriteLine("Recieveding :" + byteReadAll);

                };
            }
            catch (Exception ex)
            {
                Console.Write("Recieved all :" + byteReadAll);
                Console.Write("Download Fail");
                isDownloading = false;
                if (bWrite != null)
                    bWrite.Close();
            }
        }
    }
}
