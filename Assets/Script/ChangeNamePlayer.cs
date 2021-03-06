using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Netcode.Transports.PhotonRealtime;
using Unity.Netcode;
using TMPro;

public class ChangeNamePlayer : NetworkBehaviour
{
    PhotonRealtimeTransport photo;
    // Start is called before the first frame update
    void Start()
    {
        photo = GameObject.Find("NetworkManager").GetComponent<PhotonRealtimeTransport>();
        GameObject.Find("NetworkManager").GetComponent<NetworkManager>().OnClientConnectedCallback += ChangeNamePlayer_OnClientConnectedCallback;
        if (IsLocalPlayer)
        {
            ChangeServerRpc(new DataCollect { PlayerId = OwnerClientId, PlayerName = photo.NickName });
        }
    }
    private void Update()
    {
        if (IsOwner)
        {
            ChangeServerRpc(new DataCollect { PlayerId = OwnerClientId, PlayerName = photo.NickName });
        }
    }

    private void ChangeNamePlayer_OnClientConnectedCallback(ulong obj)
    {
        if (IsLocalPlayer)
        {
            ChangeServerRpc(new DataCollect { PlayerId = obj, PlayerName = photo.NickName });
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
        GetComponentsInChildren<TMP_Text>()[0].text = data.PlayerName;
    }
} 
