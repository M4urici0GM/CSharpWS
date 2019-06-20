# CSharpWS
Welcome to my project of using WebSocket to create real-time communication between to clients
*Dependencies:*
 - Newtonsoft.Json

# **WARNING**
Actually there's no client build yet. For now, in order to use this library, you will need to create your own client to connect
to this WebSocket server.
The server also is not sending data to the clients, only receiving, exchanging data will be implemented soon.

# Basic usage:

First, download the release from releases tab, unzip it, and copy both files 
(Newtonsoft.Json.dll and CSharpWS2.dll) to your project, add as dependency to your project,
import the CSharpWS2 namespace in your class in order to start using this awesome library.

```csharp
using System.Net;
using CSharpWS2;

int port = 1302; //Port wich server will listen on
int backlog = 10; //How many connections are allowed to stay in connection queue
int maxBuffer = 8192; //Max buffer size (may consume more memory as increase)

CSharpWebSocket csWebSocket = new CSharpWebSocket(IPAddress.Any, port, backlog, maxBuffer);
csWebSocket.Listen(); //Start WebSocket server

```

# Events:
Events that server triggers:
 - OnServerStartEvent
 - OnClientConnectionEvent
 - OnClientDisconnectionEvent

# Events usage:
You can use a lambda, or a method to handle the event, i`ll be using simple event handlers as shown
below

## Props:
### CSharpWebSocket

| Prop              | Type       | Description                                        | Access level    |
| ----------------- | ---------- | -------------------------------------------------- | --------------- |
| IPAddress         | IPAddress  | Contains the current IP wich server is litening on | public readonly |
| Port              | int        | Port wich server is listening on                   | public readonly |
| Backlog           | int        | Current backlog to the connection queue            | public readonly |
| BufferSize        | int        | Current max bufffer size to the data received      | public readonly |


# Events
### CSharpWebSocket
#### Server start event
This event will be triggered when the websocket server start
```csharp
csWebSocket.OnServerStartEvent += OnServerStartEventHandler;

private void OnServerStartEventhandler() {
    Console.WriteLine("Server running on port: {0}", csWebSocket.Port);
}
```

#### Client connection event
Event triggered when a new client connects to the server
```csharp
csWebSocket.OnClientConnectionEvent += OnClientConnectionEventHandler;

private void OnClientConnectionEventHandler(Client client) {
    Console.WriteLine("A New Client connected: {0}", client.Id);
}
```
#### Client disconnection event
Event triggered when a new client disconnects from the server
```csharp
csWebSocket.OnClientDisconnectionEvent += OnClientDisconnectionEventHandler;

private void OnClientDisconnectionEventHandler(Client client) {
    Console.WriteLine("A Client has disconnected: {0}", client.Id);
}
```



