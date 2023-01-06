using System;
using Eval.Network.Packets;
using Network;
using Network.Enums;

namespace Eval.Network.Server
{
  class ServerProgram
  {
    public const ushort Port = 2012;

    static void Main()
    {
      var server = ConnectionFactory.CreateServerConnectionContainer(Port, false);
      InitServer(server);

      Console.WriteLine("The server started successfully, press key 'q' to quit...");
      while (Console.ReadKey().KeyChar != 'q')
      {
      }

      Console.WriteLine("Stopping server...");
      server.Stop();
    }

    private static void InitServer(ServerConnectionContainer server)
    {
      server.ConnectionLost += OnConnectionLost;
      server.ConnectionEstablished += OnConnectionEstablished;
      server.Start();
    }

    private static void OnConnectionLost(Connection c, ConnectionType ct, CloseReason r)
    {
      Console.WriteLine($"CONNECTION LOST: Type: {ct}, OtherPort: {c.IPRemoteEndPoint.Port}, Reason: {r}");

      c.UnRegisterStaticPacketHandler<MyRequestPacket>();
    }

    private static void OnConnectionEstablished(Connection c, ConnectionType ct)
    {
      Console.WriteLine($"CONNECTED: Type {ct}, OtherPort: {c.IPRemoteEndPoint.Port}");

      c.RegisterStaticPacketHandler<MyRequestPacket>(OnRequestPacketReceived);
    }

    private static void OnRequestPacketReceived(MyRequestPacket requestPacket, Connection c)
    {
      Console.WriteLine($"RECEIVED REQUEST: SomeText: {requestPacket.SomeText}, SomeNumber: {requestPacket.SomeNumber}, OtherPort: {c.IPRemoteEndPoint.Port}");
      c.Send(new MyResponsePacket(requestPacket));
      Console.WriteLine("SENT RESPONSE.");
    }
  }
}
