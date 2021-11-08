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
    GameObject[] Player = new GameObject[4];
    public GameObject Prefab;
    public Transform Panel;
    int count = 0;
    int playercount = 0;
    // Start is called before the first frame update
    void Update()
    {
        if (GameObject.Find("NetworkManager").GetComponent<NetworkManager>().IsServer && count == 0)
        {
            count++;
            playercount = GameObject.FindGameObjectsWithTag("NetworkObject").Length;
            PlayerSpawn();
            if (Player[playercount].GetComponent<NetworkObject>().IsLocalPlayer)
            {
                PlayerChangeNameServerRpc();
            }
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
        NameText = GameObject.Find("PlayerInfoText (TMP)").GetComponent<TMP_Text>();
        photo = GameObject.Find("NetworkManager").GetComponent<PhotonRealtimeTransport>();
        name = photo.NickName;
        NameText.text = name;
    }
    void PlayerSpawn()
    {
        Player[playercount] = Instantiate(Prefab, Panel);
        Player[playercount].GetComponent<RectTransform>().sizeDelta = new Vector2(320, 600);
        Player[playercount].GetComponent<NetworkObject>().Spawn();
    }
}
