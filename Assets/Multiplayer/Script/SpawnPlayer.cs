using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Netcode.Transports.PhotonRealtime;

public class SpawnPlayer : NetworkBehaviour , INetworkSerializable
{
    // Start is called before the first frame update
    private GameObject prefap;
    public GameObject StagePrefap;
    IReadOnlyList<ulong> Clientlist;
    PhotonRealtimeTransport photo;
    public string PlayerName;
    public ulong PlayerId;

    void Start()
    {
        GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
        if (IsOwnedByServer)
        {
            Clientlist = NetworkManager.Singleton.ConnectedClientsIds;
            getphotonServerRpc();
            foreach (ulong x in Clientlist)
            {
                PlayerName = photo.NickName;
                PlayerId = x;
                ChangeServerRpc(PlayerId);
            }
        }
    }
    [ServerRpc]
    void getphotonServerRpc()
    {
        getphotonClientRpc();
    }
    [ClientRpc]
    void getphotonClientRpc()
    {
        photo = NetworkManager.Singleton.GetComponent<PhotonRealtimeTransport>();
    }
    [ServerRpc]
    void ChangeServerRpc(ulong data)
    {
        //ChangeClientRpc(data);
        prefap = Instantiate(StagePrefap, GetRandomSpawn(), Quaternion.identity);
        prefap.GetComponent<NetworkObject>().SpawnAsPlayerObject(data, true);
    }
    Vector3 GetRandomSpawn()
    {
        float x = Random.Range(-10f, 10f);
        float y = 4f;
        float z = Random.Range(-10f, 10f);
        return new Vector3(x, y, z);
    }
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref PlayerName);
        serializer.SerializeValue(ref PlayerId);
    }
}
