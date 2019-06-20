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

        public event OnClientDisconnect OnClientDisconnectionEvent;
        public event OnClientConnect OnClientConnectionEvent;
        public event OnServerStart OnServerStartEvent;

        public IPAddress IPAddress { get; set; }
        public int Port { get; set; }
        public int Backlog { get; set; }
        public int BufferSize { get; set; }


        private Listener ServerListener { get; set; }
        private IPEndPoint IPEndPoint { get; set; }
        private Dictionary<Guid, Client> ConnectedSockets { get; set; }

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
            OnServerStartEvent();
        }

        public bool IsServerListening() {
            return ServerListener.IsListening;
        }


        private void OnSocketConnect(Socket socket) {
            Client client = new Client(socket);
            ConnectedSockets.Add(client.Id, client);
            client.OnClientDisconnectedEventhandler += (Client _client) => OnClientDisconnectionEvent(_client);
            OnClientConnectionEvent(client);
        }
    }
}
