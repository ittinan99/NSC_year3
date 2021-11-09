using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Netcode.Transports.PhotonRealtime;
using Unity.Netcode;

public class ChangeNamePlayer : NetworkBehaviour
{
    PhotonRealtimeTransport photo;
    // Start is called before the first frame update
    void Start()
    {
        if (IsLocalPlayer)
        {
            ChangeServerRpc();
        }
    }
    [ServerRpc]
    void ChangeServerRpc()
    {
        ChangeClientRpc();
    }
    [ClientRpc]
    void ChangeClientRpc()
    {
        photo = GameObject.Find("NetworkManager").GetComponent<PhotonRealtimeTransport>();
        this.gameObject.name = photo.NickName;
        Destroy(GetComponent<ChangeNamePlayer>());
    }
}
