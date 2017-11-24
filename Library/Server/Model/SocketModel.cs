﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace Server
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

        internal void SendData(Book book)
        {
            Thread t = new Thread(SendDataByThread);
            t.Start(book);
        }

        public void SendDataByThread(Object o)
        {
            Book book = (Book)o;
            try
            {
                string filePath = "";
                string fileName = book.Path;
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
                //[FILE NAME LENGHT] [LENGTH FILE ] [NAME FILE] [DATA]
                //[   4            ] [   4        ] [   lengt file   ]

                //SEND INFO
                stream.Write(clientData, 0, clientData.Length);

                int bufferLength = 1024 * 1000;
                byte[] buffer = new byte[bufferLength];

                int len = (Int32)tempfile.Length;
                decimal allByteRead = 0;
                int byteRead;
                //  int tmp = bufferLength > len ? len : bufferLength;

                while ((byteRead = tempfile.Read(buffer, 0, bufferLength)) > 0)
                {
                    try
                    {
                        socket.Send(buffer);
                    }
                    catch (Exception E)
                    {
                        Console.Write("Send failed" + E.StackTrace);
                    }

                    ////Debug
                    //allByteRead += byteRead;
                    //if (allByteRead >= tempfile.Length)
                    //    break;
                    //    len -= tmp;
                    //   tmp = bufferLength > len ? len : bufferLength;
                }

                Console.WriteLine("Read all : " + tempfile.Length);

            }
            catch (Exception E)
            {
                Console.Write(E.StackTrace);
            }
        }
    }

}