using System;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace CSharpWS2 {
    public class Client {
        public delegate void OnDataReceived(Client client, byte[] data);

        public event OnDataReceived OnDataReceivedEventHandler;

        public IPEndPoint IPEndPoint { get; private set; }
        public Guid Id { get; set; }

        private Socket ConnectedSocket { get; set; }

        public Client(Socket connectedSocket) {
            Id = Guid.NewGuid();
            IPEndPoint = (IPEndPoint) connectedSocket.RemoteEndPoint;
            ConnectedSocket = connectedSocket;
            StartAccept();
        }

        private void StartAccept() {
            ConnectedSocket.BeginReceive(new byte[] { 0 }, 0, 0, 0, StartReceiveCallback, ConnectedSocket);
        }

        private void StartReceiveCallback(IAsyncResult asyncResult){
            try {
                byte[] buffer  = new byte[8192];
                ConnectedSocket.EndReceive(asyncResult);
                int received = ConnectedSocket.Receive(buffer, buffer.Length, 0);

                if (buffer.Length > received)
                    Array.Resize(ref buffer, received);

                if (OnDataReceivedEventHandler == null)
                    throw new Exception("Event handler for data received is needed");
                OnDataReceivedEventHandler(this, buffer);

                StartAccept();
            } catch (Exception ex) {
                ConnectedSocket.Close();
                ConnectedSocket.Dispose();
                throw ex;
            }
        }

        public void CloseConnection() {
            ConnectedSocket.Close();
            ConnectedSocket.Dispose();
        }
    }
}
