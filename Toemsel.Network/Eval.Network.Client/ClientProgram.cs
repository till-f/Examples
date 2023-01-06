using System;
using Eval.Network.Packets;
using Network;
using Network.Enums;

namespace Eval.Network.Client
{
  class ClientProgram
  {
    public const ushort Port = 2012;

    static void Main()
    {
      TcpConnection clientConnection = null;

      Console.WriteLine("Press key to (c)onnect, (s)end, (d)isconnect, (q)uit...");

      char consoleKey;
      while ((consoleKey = Console.ReadKey().KeyChar) != 'q')
      {
        Console.WriteLine();
        switch (consoleKey)
        {
          case 'c':
            if (clientConnection != null)
            {
              Console.WriteLine("Already connected.");
              break;
            }
            Console.WriteLine("Connecting...");
            clientConnection = Connect();
            break;
          case 's':
            if (clientConnection == null || !clientConnection.IsAlive)
            {
              Console.WriteLine("Cannot send, not connected.");
              break;
            }
            clientConnection.Send(new MyRequestPacket("HelloWorld", 42));
            Console.WriteLine("SENT REQUEST.");
            break;
          case 'd':
            if (clientConnection == null)
            {
              Console.WriteLine("Cannot disconnect.");
              break;
            }
            Console.WriteLine("Closing...");
            clientConnection.Close(CloseReason.ClientClosed);
            clientConnection = null;
            break;
        }
      }
    }

    private static TcpConnection Connect()
    {
      var clientConnection = ConnectionFactory.CreateTcpConnection("127.0.0.1", Port, out var connectionResult);
      if (connectionResult == ConnectionResult.Connected)
      {
        clientConnection.RegisterStaticPacketHandler<MyResponsePacket>(OnPacketReceived);
        clientConnection.ConnectionClosed += OnConnectionClosed;
        return clientConnection;
      }

      Console.WriteLine("Could not connect.");
      return null;
    }

    private static void OnPacketReceived(MyResponsePacket responsePacket, Connection c)
    {
      Console.WriteLine($"RECEIVED RESPONSE: SomeResponse: {responsePacket.SomeResponse}, OtherPort: {c.IPRemoteEndPoint.Port}");
    }

    private static void OnConnectionClosed(CloseReason r, Connection c)
    {
      Console.WriteLine($"CONNECTION CLOSED: OtherPort: {c.IPRemoteEndPoint.Port}, Reason: {r}");
      c.UnRegisterStaticPacketHandler<MyResponsePacket>();
    }
  }
}
