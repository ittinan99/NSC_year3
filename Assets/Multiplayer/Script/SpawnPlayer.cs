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
    private void Awake()
    {
        Clientlist = NetworkManager.Singleton.ConnectedClientsIds;
        if (IsOwnedByServer)
        {
            foreach (ulong x in Clientlist)
            {
                getphotonServerRpc();
                ChangeServerRpc(x);
            }
        }
        CloseCameraServerRpc();
    }
    void Start()
    {
        
    }
    [ServerRpc]
    void CloseCameraServerRpc()
    {
        CloseCameraClientRpc();
    }
    [ClientRpc]
    void CloseCameraClientRpc()
    {
        GameObject.Find("MainCamera").SetActive(false);
        Destroy(this.gameObject);
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
        PlayerId = NetworkManager.Singleton.LocalClientId;
        PlayerName = photo.NickName;
    }
    [ServerRpc]
    void ChangeServerRpc(ulong data)
    {
        ChangeClientRpc();
    }
    [ClientRpc]
    void ChangeClientRpc()
    {
        photo = NetworkManager.Singleton.GetComponent<PhotonRealtimeTransport>();
        PlayerId = NetworkManager.Singleton.LocalClientId;
        PlayerName = photo.NickName;
    }
    Vector3 GetRandomSpawn()
    {
        float x = Random.Range(-10f, 10f);
        float y = 4f;
        float z = Random.Range(-10f, 10f);
        return new Vector3(x, y, z);
    }
}
