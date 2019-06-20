using System;
using System.Net;

namespace CSharpWS2 {
    public class CSharpWebSocket {
        public IPAddress IPAddress { get; private set; }
        public int Port { get; private set; }
        public int BackLog { get; private set; }


        public CSharpWebSocket(IPAddress ipAddress, int port, int backLog) {
            IPAddress = ipAddress;
            Port = port;
            BackLog = backLog;
        }

        public void Listen() {

        }

    }
}
