
using Client.Api;
using Client.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace Client
{
    /// <summary>
    /// Description of SocketModel.
    /// </summary>
    public class SocketModel
    {
        public const String CLIENT_DISSCONECT = "Socket is closed with ";
        private Socket socket;
        private byte[] array_to_receive_data;
        private string remoteEndPoint;
        private Stream stream;
        private ApiManager apiManager;
        public Boolean isDataSending = false;
        public Book bookCurrent = null;
        public bool isDownloading = false;
        public bool isReceiving = false;

        public SocketModel(Socket s)
        {
            socket = s;
            array_to_receive_data = new byte[100];
            stream = new NetworkStream(socket);
        }
        public SocketModel(Socket s, int length)
        {
            socket = s;

            array_to_receive_data = new byte[length];
        }


        //close sockket
        public void CloseSocket()
        {
            socket.Close();
        }
        //get the IP and port of connected client
        public string GetRemoteEndpoint()
        {
            string str = "";
            try
            {
                str = Convert.ToString(socket.RemoteEndPoint);
                remoteEndPoint = str;
            }
            catch (Exception e)
            {
                string str1 = "Error..... " + e.StackTrace;
                Console.WriteLine(str1);
                str = CLIENT_DISSCONECT + remoteEndPoint;
            }
            return str;
        }
        //receive data from client
        public string ReceiveData()
        {
            //server just can receive data AFTER a connection is set up between server and client
            string str = "";
            try
            {
                //count the length of data received (maximum is 100 bytes)
                int k = socket.Receive(array_to_receive_data);
                Console.WriteLine("From client:");
                //convert the byte recevied into string
                char[] c = new char[k];
                for (int i = 0; i < k; i++)
                {
                    Console.Write(Convert.ToChar(array_to_receive_data[i]));
                    c[i] = Convert.ToChar(array_to_receive_data[i]);
                }
                str = new string(c);
            }
            catch (Exception e)
            {
                string str1 = "Error..... " + e.StackTrace;
                Console.WriteLine(str1);
                str = CLIENT_DISSCONECT + remoteEndPoint;
            }
            return str;
        }

        internal void SendBook(object bookCurrent)
        {
            throw new NotImplementedException();
        }

        //send data to client
        public void SendData(string str)
        {
            try
            {
                ASCIIEncoding asen = new ASCIIEncoding();
                socket.Send(asen.GetBytes(str));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }
        public void SendData(List<String> strs)
        {
            try
            {
                var bin = new BinaryFormatter();
                bin.Serialize(stream, strs);
            }
            catch (Exception e)
            {
                Console.WriteLine("Send List String Error..... " + e.StackTrace);
            }
        }

        internal void ReceiveBook()
        {
            isReceiving = true;
            Thread t = new Thread(ReceiveBookByThread);
            t.Start();

        }

        private void ReceiveBookByThread()
        {
            BinaryWriter bWrite = null;
            int fileLenght = 0;
            int originalLength = 0;
            decimal byteReadAll = 0;
            try
            {
                String receivedPath = DataBase.PATH_DB;
                if (!Directory.Exists(receivedPath))
                    Directory.CreateDirectory(receivedPath);


                byte[] clientData = new byte[100];

                int byteReceive = stream.Read(clientData, 0, 4);
                int fileNameLen = BitConverter.ToInt32(clientData, 0);
                byteReceive = stream.Read(clientData, 0, 4);
                fileLenght = BitConverter.ToInt32(clientData, 0);
                originalLength = fileLenght;
                Console.WriteLine("fileNameLen = {0}", fileNameLen);

                stream.Read(clientData, 0, fileNameLen);

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

                int bufferLength = ServerManager.BUFFER_SIZE;
                byte[] buffer = new byte[bufferLength];

                int byteRead;

                while ((byteRead = stream.Read(buffer, 0, bufferLength)) > 0)
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
                        isReceiving = false;
                        Console.Write("Recieved all :" + byteReadAll);
                        bWrite.Close();
                        Random r = new Random();
                        SendData((r.Next(1, 55) * 1000) + "");
                        return;
                    }

                    fileLenght -= bufferLength;
             
                    //DEBUG
                    Console.WriteLine("Recieveding :" + byteReadAll);

                };
            }
            catch (Exception ex)
            {
                isReceiving = false;
                Console.Write("Recieved all :" + byteReadAll);
                Console.Write("Download Fail");
                isDownloading = false;
                if (bWrite != null)
                    bWrite.Close();
            }
        }

        internal void SendBook(Book book)
        {
            Thread t = new Thread(SendBookByThread);
            t.Start(book);
        }

        public void SendBookByThread(Object o)
        {
            isDataSending = true;
            bookCurrent = (Book)o;
            try
            {
                string filePath = "";
                string fileName = bookCurrent.Path;
                if (bookCurrent.Id.StartsWith("GD"))
                {
                    apiManager = new ApiManager();
                    fileName = apiManager.DownloadGoogleFile(bookCurrent.Path);
                }

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
                fileNameLen.CopyTo(clientData, 0);
                fileLength.CopyTo(clientData, 4);
                fileNameByte.CopyTo(clientData, 8);
                //[FILE NAME LENGHT] [LENGTH FILE ] [NAME FILE] 
                //[   4            ] [   4        ] [  name   ]

                //SEND INFO
                stream.Write(clientData, 0, clientData.Length);

                int bufferLength = ServerManager.BUFFER_SIZE;
                byte[] buffer = new byte[bufferLength];

                int len = (Int32)tempfile.Length;
                int byteRead;
                int byteAllRead = 0;
                while ((byteRead = tempfile.Read(buffer, 0, bufferLength)) > 0)
                {
                    try
                    {
                        socket.Send(buffer);
                        //DEBUG
                        len -= byteRead;

                        if (len < byteRead) byteAllRead += len;
                        else
                            byteAllRead += byteRead;
                        Console.WriteLine("Reading : " + byteAllRead);
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
            }
            catch (Exception E)
            {
                Console.Write(E.StackTrace);
                isDataSending = false;
            }
        }
    }
}
