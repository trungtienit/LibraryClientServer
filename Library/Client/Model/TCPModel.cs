
using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Threading;
using Server.Common;
using System.Windows.Forms;
using System.Diagnostics;

namespace Server
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
        private bool isDataSending;
        public String bookCurrent;
        private ProgressBar progressBar;
        private Label wallet;
        private ViewFile viewFile;
        internal byte typeCurrent;

        public TCPModel(string ip, int p)
        {
            IPofServer = ip;
            port = p;
            tcpclnt = new TcpClient();
            tcpclnt.ReceiveTimeout = Timeout.Infinite;
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

        internal void setWallet(Label tbWallet)
        {
            this.wallet = tbWallet;
        }

        internal void setProgressBar(ProgressBar progressBar)
        {
            this.progressBar = progressBar;
        }

        //Set up a connection to server and get stream for communication
        public int ConnectToServer()
        {
            try
            {
                tcpclnt.Connect(IPofServer, port);
                stm = tcpclnt.GetStream();
                stm.ReadTimeout = Timeout.Infinite;
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
            List<String> list = null;
            try
            {
                list = (List<string>)bin.Deserialize(stm);
            }
            catch
            {
                return list;
            }
            return list;
        }

        //close connection
        public void CloseConnection()
        {
            tcpclnt.Close();
        }

        internal void ReceiveBook()
        {
            bookCurrent = "";
            ReceiveBookByThread();
            //  Thread t = new Thread(ReceiveBookByThread);
            //  t.Start();

        }

        private void ReceiveBookByThread()
        {
            isDownloading = true;
            BinaryWriter bWrite = null;
            int fileLenght = 0;
            int originalLength = 0;
            decimal byteReadAll = 0;
            progressBar.Value = 0;
            progressBar.Update();
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

                progressBar.Maximum = 100;

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

                int bufferLength = ClientManager.BUFFER_SIZE;
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
                    progressBar.Value = (Int32)byteReadAll * 100 / originalLength;
                    progressBar.Update();
                    if (byteReadAll == originalLength)
                    {
                        isDownloading = false;
                        Console.Write("Recieved all :" + byteReadAll);
                        bookCurrent = receivedPath + "/" + fileName;

                        bWrite.Close();

                        //
                        FileInfo f = new FileInfo(bookCurrent);
                        if (typeCurrent == ClientManager.TYPE_PREVIEW)
                        {
                            viewFile.bookCurrent = f.FullName;
                            viewFile.Show();
                        }
                        DownloadSuccess();

                        return;
                    }

                    fileLenght -= bufferLength;

                    //DEBUG
                    Console.WriteLine("Recieveding :" + byteReadAll);

                };
            }
            catch (Exception ex)
            {
                if (bWrite != null)
                    bWrite.Close();
                isDownloading = false;

                if (byteReadAll == originalLength)
                {
                    if (typeCurrent == ClientManager.TYPE_PREVIEW)
                    {
                        //Wait Convert Pdf
                        if (byteReadAll == 0)
                            ReceiveBookByThread();
                        else
                        {
                            FileInfo f = new FileInfo(bookCurrent);
                            viewFile.bookCurrent = f.Name;
                            viewFile.Show();
                        }
                    }
                    else
                        DownloadSuccess();
                }
                else
                    Console.Write("Download Fail");

                Console.Write("Recieved all :" + byteReadAll);
                return;

            }
        }

        private void DownloadSuccess()
        {
            DialogResult d = MessageBox.Show("Open folder Download", "Info", MessageBoxButtons.YesNo);
            if (d == DialogResult.Yes)
                OpenFolderDownload();
            return;
        }

        private void OpenFolderDownload()
        {
            Process.Start("explorer.exe", ClientManager.FOLDER_DOWLOAD);
        }

        internal void setFormDetail(ViewFile frmViewBook)
        {
            this.viewFile = frmViewBook;

        }

        internal void SendBook(Book book)
        {
            Thread t = new Thread(SendBookByThread);
            t.Start(book);
        }

        private void SendBookByThread(Object o)
        {
            isDataSending = true;
            Book b = (Book)o;
            progressBar.Value = 0;
            progressBar.Update();
            try
            {
                string filePath = "";
                string fileName = b.Path;

                fileName = fileName.Replace("\\", "/");
                while (fileName.IndexOf("/") > -1)
                {
                    filePath += fileName.Substring(0, fileName.IndexOf("/") + 1);
                    fileName = fileName.Substring(fileName.IndexOf("/") + 1);
                }

                byte[] fileNameByte = Encoding.ASCII.GetBytes(fileName);

                FileStream tempfile = File.OpenRead(filePath + fileName);
                byte[] clientData = new byte[4 + 4 + fileName.Length];

                byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);
                byte[] fileLength = BitConverter.GetBytes(tempfile.Length);

                progressBar.Maximum = 100;


                fileNameLen.CopyTo(clientData, 0);
                fileLength.CopyTo(clientData, 4);
                fileNameByte.CopyTo(clientData, 8);
                //[FILE NAME LENGHT] [LENGTH FILE ] [NAME FILE] 
                //[   4            ] [   4        ] [  name   ]

                //SEND INFO
                stm.Write(clientData, 0, clientData.Length);

                int bufferLength = ClientManager.BUFFER_SIZE;
                byte[] buffer = new byte[bufferLength];

                int orginalLength = (Int32)tempfile.Length;
                int len = orginalLength;
                int byteRead;
                int byteAllRead = 0;
                while ((byteRead = tempfile.Read(buffer, 0, bufferLength)) > 0)
                {
                    try
                    {
                        stm.Write(buffer, 0, bufferLength);
                        //DEBUG
                        len -= byteRead;

                        if (len < byteRead) byteAllRead += len;
                        else
                            byteAllRead += byteRead;
                        Console.WriteLine("Reading : " + byteAllRead);
                        int vl = byteAllRead * 100 / orginalLength;
                        progressBar.Value =vl;
                        progressBar.Update();
                    }
                    catch (Exception E)
                    {
                        Console.Write("Send failed" + E.StackTrace);
                        isDataSending = false;
                    }

                }
                byteAllRead += bufferLength;
                Console.WriteLine("Read all : " + byteAllRead);
                isDataSending = false;
                try
                {
                    int price = Int32.Parse(ReadData());
                    ClientManager.myWallet += price;
                    wallet.Text = ClientManager.myWallet.ToString();
                    MessageBox.Show("You given " + price + "", "Admin", MessageBoxButtons.OK);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Changed book faild " + e.StackTrace);
                };
            }
            catch (Exception E)
            {
                Console.Write(E.StackTrace);
                isDataSending = false;
            }
        }
    }
}
