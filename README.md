# CSharpWS
Welcome to my project of using WebSocket to create real-time communication between to clients
*Dependencies:*
 - Newtonsoft.Json

# Basic usage:
```csharp
using System.Net;
using CSharpWS2;

int port = 1302; //Port wich server will listen on
int backlog = 10; //How many connections are allowed to stay in connection queue
int maxBuffer = 8192; //Max buffer size (may consume more memory as increase)

CSharpWebSocket csWebSocket = new CSharpWebSocket(IPAddress.Any, port, backlog, maxBuffer);
csWebSocket.Listen(); //Start WebSocket server

```

#Events
Events that server triggers:
 - OnServerStartEvent: delegate void ()
 - OnClientConnectionEvent: delegate void (Client)
 - OnClientDisconnectionEvent: delegate void (Client)
