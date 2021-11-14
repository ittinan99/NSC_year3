using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Netcode.Transports.PhotonRealtime;
using Unity.Netcode;
using TMPro;

public class ChangeNamePlayer : NetworkBehaviour
{
    PhotonRealtimeTransport photo;
    bool connectionrun = false;
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
        if (connectionrun)
        {
            ChangeServerRpc(new DataCollect { PlayerId = OwnerClientId, PlayerName = photo.NickName });
            connectionrun = false;
        }
    }

    private void ChangeNamePlayer_OnClientConnectedCallback(ulong obj)
    {
        if (IsLocalPlayer)
        {
            ChangeServerRpc(new DataCollect { PlayerId = obj, PlayerName = photo.NickName });
            connectionrun = true;
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
