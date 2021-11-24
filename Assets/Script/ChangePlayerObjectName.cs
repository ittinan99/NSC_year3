using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Netcode.Transports.PhotonRealtime;
using Unity.Netcode;
using TMPro;

public class ChangePlayerObjectName : NetworkBehaviour
{
    PhotonRealtimeTransport photo;
    // Start is called before the first frame update
    void Start()
    {
        photo = GameObject.Find("NetworkManager").GetComponent<PhotonRealtimeTransport>();
        if (IsOwner)
        {
            ChangeServerRpc(new DataCollect { PlayerId = OwnerClientId, PlayerName = photo.NickName });
            Destroy(GetComponent<ChangePlayerObjectName>());
        }
    }
    private void Update()
    {
        if (this.gameObject.name != photo.NickName)
        {
            if (IsOwner)
            {
                ChangeServerRpc(new DataCollect { PlayerId = OwnerClientId, PlayerName = photo.NickName });
                Destroy(GetComponent<ChangePlayerObjectName>());
            }
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
        this.gameObject.name = data.PlayerName;
    }
}
