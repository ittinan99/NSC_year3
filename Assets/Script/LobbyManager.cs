using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Netcode.Transports.PhotonRealtime;
using TMPro;
using Unity.Netcode;
public class LobbyManager : NetworkBehaviour 
{
    PhotonRealtimeTransport photo;
    private string name;
    private TMP_Text NameText;
    NetworkObject[] Player = new NetworkObject[4];
    // Start is called before the first frame update
    void Start()
    {
        Player[0] = GameObject.FindObjectOfType<NetworkObject>();
        if (Player[0].IsLocalPlayer)
        {
            PlayerChangeNameServerRpc();
        }
    }
    [ServerRpc]
    void PlayerChangeNameServerRpc()
    {
        PlayerChangeNameClientRpc();
    }
    [ClientRpc]
    void PlayerChangeNameClientRpc()
    {
        NameText = GameObject.Find("PlayerInfoBase").GetComponent<TMP_Text>();
        photo = GameObject.Find("NetworkManager").GetComponent<PhotonRealtimeTransport>();
        name = photo.NickName;
        NameText.text = name;
    }
}
