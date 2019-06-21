using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace CSharpWS2 {
    public class CSharpWebSocket {

        public delegate void OnClientDisconnect(Client client);
        public delegate void OnClientConnect(Client connect);
        public delegate void OnServerStart();
        public delegate void OnClientDataReceived(Client client, byte[] data);

        public event OnClientDisconnect OnClientDisconnectionEvent;
        public event OnClientConnect OnClientConnectionEvent;
        public event OnServerStart OnServerStartEvent;
        public event OnClientDataReceived OnClientDataReceivedEvent;

        public IPAddress IPAddress { get; private set; }
        public int Port { get; private set; }
        public int Backlog { get; private set; }
        public int BufferSize { get; private set; }


        private Listener ServerListener { get; set; }
        private IPEndPoint IPEndPoint { get; set; }
        private Dictionary<Guid, Client> ConnectedSockets;

        public CSharpWebSocket(IPAddress ipAddress, int port, int backlog, int bufferSize) {
            IPAddress = ipAddress;
            Port = port;
            Backlog = backlog;
            IPEndPoint = new IPEndPoint(ipAddress, port);
            ServerListener = new Listener(IPEndPoint, backlog);
            ConnectedSockets = new Dictionary<Guid, Client>();
        }


        public void Listen() {
            ServerListener.StartServer();
            ServerListener.OnSocketConnectEventHandler += OnSocketConnect;
            OnServerStartEvent?.Invoke();
        }

        public bool IsServerListening() {
            return ServerListener.IsListening;
        }

        public Client GetClient(Guid clientGuid) {
            ConnectedSockets.TryGetValue(clientGuid, out Client client);
            return client;
        }

        public void BroadCast(byte[] data) {
            foreach (KeyValuePair<Guid, Client> item in ConnectedSockets) {
                item.Value.SendData(data);
            }
        }

        private void OnSocketConnect(Socket socket) {
            Client client = new Client(socket);
            ConnectedSockets.Add(client.Id, client);
            client.OnClientDisconnectedEvent += OnSocketDisconnect;
            client.OnDataReceivedEvent += (Client _client, byte[] data) => OnClientDataReceivedEvent?.Invoke(_client, data);
            OnClientConnectionEvent?.Invoke(client);
        }

        private void OnSocketDisconnect(Client client) {
            ConnectedSockets.Remove(client.Id);
            OnClientDisconnectionEvent(client);
        }
    }
}
