
using System;
using System.Net;
using System.Net.Sockets;

namespace Client
{
	/// <summary>
	/// Description of TCPModel.
	/// </summary>
	public class TCPModel
	{
		private IPAddress ipAddress;
		private int port;
		private TcpListener tcpServer;	
		
		public TCPModel(string ip, int p)
		{
			ipAddress = IPAddress.Parse(ip);
			port = p;
			tcpServer = new TcpListener(ipAddress, port);			
		}

		public void Listen(){
			try{
				tcpServer.Start();
			}
			catch (Exception e){
				Console.WriteLine("Error.... " + e.StackTrace);
			}			
		}
		//accept a new connection
		public Socket SetUpANewConnection(ref int status){
			Socket socket = tcpServer.AcceptSocket();
			status = 1;
			return socket;
		}
		//shutdown server
		public void Shutdown(){
			tcpServer.Stop();			
		}				
	}
}
