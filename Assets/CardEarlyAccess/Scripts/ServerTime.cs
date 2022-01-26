using System.Collections.Generic;
using Unity.Netcode;

public struct ServerTime : INetworkSerializable
{
    public string Servertime;
    public ServerTime(string NewTime)
    {
        Servertime = NewTime;
    }
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Servertime);
    }
}

