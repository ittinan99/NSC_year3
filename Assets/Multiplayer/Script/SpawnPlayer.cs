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
    DataCollect dataCollect;
    IReadOnlyList<ulong> Clientlist;

    void Start()
    {
        GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
        if (IsOwnedByServer)
        {
            Clientlist = NetworkManager.Singleton.ConnectedClientsIds;
            PhotonRealtimeTransport photo = NetworkManager.Singleton.GetComponent<PhotonRealtimeTransport>();
            foreach (ulong x in Clientlist)
            {
                ChangeServerRpc(new DataCollect { PlayerId = x, PlayerName = photo.NickName });
            }
            Destroy(this.gameObject);
        }
    }
    [ServerRpc]
    void ChangeServerRpc(DataCollect data)
    {
        ChangeClientRpc(data);
    }
    [ClientRpc]
    void ChangeClientRpc(DataCollect data)
    {
        prefap = Instantiate(StagePrefap, GetRandomSpawn(), Quaternion.identity);
        prefap.name = data.PlayerName;
        prefap.GetComponent<NetworkObject>().SpawnAsPlayerObject(data.PlayerId, true);
    }
    Vector3 GetRandomSpawn()
    {
        float x = Random.Range(-10f, 10f);
        float y = 4f;
        float z = Random.Range(-10f, 10f);
        return new Vector3(x, y, z);
    }
}
