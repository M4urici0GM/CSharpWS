using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace CSharpWS2 {
    public class CSharpWebSocket {
        public IPAddress IPAddress { get; private set; }
        public int Port { get; private set; }
        public int BackLog { get; private set; }
        private Dictionary<Guid, Client> Sockets { get; set; }

        public delegate void OnClientConnect(Client client);
        public event OnClientConnect OnClientConnectEventHandler;

        private Listener serverListener;

        public CSharpWebSocket(IPAddress ipAddress, int port, int backLog) {
            IPAddress = ipAddress;
            Port = port;
            BackLog = backLog;
        }

        public void Listen() {

        }

        public Client GetSocket(Guid socketGuid) {
            Client client;
            bool socketExists = Sockets.TryGetValue(socketGuid, out client);
            return (socketExists ? client : new Client());
        }


    }
}
