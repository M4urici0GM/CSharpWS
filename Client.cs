using System;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace CSharpWS2 {
    public class Client {
        public delegate void OnDataReceived(Client client, byte[] data);
        public delegate void OnClientDisconnected(Client client);

        public event OnDataReceived OnDataReceivedEventHandler;
        public event OnClientDisconnected OnClientDisconnectedEventhandler;

        public IPEndPoint IPEndPoint { get; private set; }
        public Guid Id { get; set; }

        private Socket ConnectedSocket { get; set; }
        private byte[] buffer;

        public Client(Socket connectedSocket) {
            Id = Guid.NewGuid();
            IPEndPoint = (IPEndPoint) connectedSocket.RemoteEndPoint;
            ConnectedSocket = connectedSocket;
            ConnectedSocket.BeginReceive(new byte[] { 0 }, 0, 0, 0, StartReceiveCallback, connectedSocket);
        }

        private void StartReceiveCallback(IAsyncResult asyncResult){
            Socket socket = asyncResult.AsyncState as Socket;
            try {
                socket.EndReceive(asyncResult);
                buffer = new byte[8192];
                int received = socket.Receive(buffer, buffer.Length, 0);

                if (received <= 0)
                    throw new SocketException();

                if (buffer.Length > received)
                    Array.Resize(ref buffer, received);

                OnDataReceivedEventHandler?.Invoke(this, buffer);

                socket.BeginReceive(new byte[] { 0 }, 0, 0, 0, StartReceiveCallback, null);
            } catch {
                OnClientDisconnectedEventhandler(this);
                CloseConnection();
            }
        }

        public void CloseConnection() {
            ConnectedSocket.Close();
            ConnectedSocket.Dispose();
        }
    }
}
