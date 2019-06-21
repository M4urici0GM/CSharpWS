using System;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace CSharpWS2 {
    public class Client {
        public delegate void OnDataReceived(Client client, byte[] data);
        public delegate void OnClientDisconnected(Client client);

        public event OnDataReceived OnDataReceivedEvent;
        public event OnClientDisconnected OnClientDisconnectedEvent;

        public IPEndPoint IPEndPoint { get; private set; }
        public Guid Id { get; private set; }

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

                if (received <= 0) {
                    Console.WriteLine("No Bytes received, disconnecting..");
                    throw new SocketException();
                }


                if (buffer.Length > received)
                    Array.Resize(ref buffer, received);

                OnDataReceivedEvent?.Invoke(this, buffer);

                socket.BeginReceive(new byte[] { 0 }, 0, 0, 0, StartReceiveCallback, null);
            } catch {
                OnClientDisconnectedEvent?.Invoke(this);
                CloseConnection();
            }
        }

        public void SendData(byte[] data) {
            if (ConnectedSocket.Connected) {
                ConnectedSocket.Send(data);
            } else {
                throw new SocketException();
            }
        }

        public void CloseConnection() {
            ConnectedSocket.Close();
            ConnectedSocket.Dispose();
        }
    }
}
