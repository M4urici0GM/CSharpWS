using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace CSharpWS2 {
    public class CSharpWebSocket {
        public delegate void OnClientConnect(Client client);
        public delegate void OnServerStart();
        //public delegate void OnClientEvent(object sender, EventArgs e);

        public event OnClientConnect OnClientConnectEventHandler;
        public event OnServerStart OnServerStartEventHandler;

        public IPAddress IPAddress { get; private set; }
        public int Port { get; private set; }
        public int BackLog { get; private set; }

        private Listener serverListener { get; set; }
        private IPEndPoint IPEndPoint { get; set; }
        private Dictionary<Guid, Client> Sockets { get; set; }
        //private Dictionary<string, > Events;

        public CSharpWebSocket(IPAddress ipAddress, int port, int backLog, int bufferSize) {
            IPEndPoint = new IPEndPoint(ipAddress, port);
            IPAddress = ipAddress;
            Port = port;
            BackLog = backLog;

            serverListener = new Listener(IPEndPoint, backLog);
        }

        public void Listen() {
            serverListener.StartServer();
            RegisterEvents();
            serverListener.OnSocketConnectEventHandler += OnSocketConnectEventHandler;
            OnServerStartEventHandler();
        }


        //public Client GetSocket(Guid socketGuid) {
        //    Sockets.TryGetValue(socketGuid, out new Client)
        //}

        /*
         * Event handler to socket connection event       
         */       
        private void OnSocketConnectEventHandler(Socket socket) {
            Client clientConnected = new Client(socket);
            Sockets.Add(clientConnected.Id, clientConnected);
            OnClientConnectEventHandler(clientConnected);
        }

        /*
         * method used to register custom events
         */
        private void RegisterEvents() {

        }
    }
}
