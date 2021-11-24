using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Netcode.Transports.PhotonRealtime;

public class SpawnPlayer : NetworkBehaviour
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
        Clientlist = NetworkManager.Singleton.ConnectedClientsIds;
        if (IsOwnedByServer)
        {
            foreach (ulong ClientId in Clientlist)
            {
                getphotonServerRpc();
                ChangeServerRpc(ClientId);
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
        PlayerName = photo.NickName;
    }
    [ServerRpc]
    void ChangeServerRpc(ulong ClientId)
    {
        prefap = Instantiate(StagePrefap, GetRandomSpawn(), Quaternion.identity);
        prefap.GetComponent<NetworkObject>().SpawnAsPlayerObject(ClientId, true);
        prefap.name = PlayerName;
    }
    Vector3 GetRandomSpawn()
    {
        float x = Random.Range(-10f, 10f);
        float y = 4f;
        float z = Random.Range(-10f, 10f);
        return new Vector3(x, y, z);
    }
}