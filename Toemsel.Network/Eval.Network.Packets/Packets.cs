using Network.Packets;

namespace Eval.Network.Packets
{
  public class MyRequestPacket : RequestPacket
  {
    public MyRequestPacket(string someText, int someNumber)
    {
      SomeText = someText;
      SomeNumber = someNumber;
    }

    public string SomeText { get; set; }

    public int SomeNumber { get; set; }
  }

  public class MyResponsePacket : ResponsePacket
  {
    public MyResponsePacket(MyRequestPacket requestPacket) : base(requestPacket)
    {
      SomeResponse = $"Pong {requestPacket.SomeText} with {requestPacket.SomeNumber}";
    }

    public string SomeResponse { get; set; }
  }
}