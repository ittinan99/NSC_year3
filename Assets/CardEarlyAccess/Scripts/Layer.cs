using System.Collections.Generic;
using Unity.Netcode;

public struct Layer : INetworkSerializable
{
    public float LayerWeight;
    public float target;
    public Layer(float Weight, float Target)
    {
        LayerWeight = Weight;
        target = Target;
    }
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref LayerWeight);
        serializer.SerializeValue(ref target);
    }
}
