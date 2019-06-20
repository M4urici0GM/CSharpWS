using System;
using System.Net;
using System.Net.Sockets;

namespace CSharpWS2 {
    public class Listener {

        public delegate void OnSocketConnect(Socket socket);
        public event OnSocketConnect OnSocketConnectEventHandler;
        public bool IsListening { get; private set; }

        private IPEndPoint IPEndPoint { get; set; }
        private int BackLog { get; set; }
        private Socket Socket { get; set; }


        public Listener(IPEndPoint ipEndPoint, int backLog) {
            IPEndPoint = ipEndPoint;
            BackLog = backLog;
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void StartServer() {
            Socket.Bind(IPEndPoint);
            Socket.Listen(BackLog);
            Socket.BeginAccept(new AsyncCallback(AcceptCallback), Socket);
            IsListening = true;
        }

        private void AcceptCallback(IAsyncResult asyncResult) {
            try {
                Socket connectedSocket = Socket.EndAccept(asyncResult);
                if (connectedSocket.Connected) {
                    OnSocketConnectEventHandler(connectedSocket);
                }
                Socket.BeginAccept(AcceptCallback, Socket);
            } catch(Exception ex) {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
                Socket.Dispose();
                IsListening = false;
                throw ex;
            } 
        }

    }
}
