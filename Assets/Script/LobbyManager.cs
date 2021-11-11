using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using Netcode.Transports.PhotonRealtime;
public class LobbyManager : NetworkBehaviour
{
    private TMP_Text[] Text;
    private Transform Panel;
    private string Name;
    int count = 0;
    private void Start()
    {
        Panel = GameObject.Find("PlayerSlotPanel").GetComponent<Transform>();
        GetComponent<Transform>().SetParent(Panel);
        GetComponent<RectTransform>().sizeDelta = new Vector2(325, 600);
    }
    // Start is called before the first frame update
    void Update()
    {
        if (IsLocalPlayer)
        {
            SetParentServerRpc();
            PlayerChangeNameServerRpc();
            count++;
        }
    }
    [ServerRpc]
    void SetParentServerRpc()
    {
        SetParentClientRpc();
    }
    [ClientRpc]
    void SetParentClientRpc()
    {
        Name = NetworkManager.Singleton.GetComponent<PhotonRealtimeTransport>().NickName;
        GetComponent<Transform>().SetParent(Panel);
        GetComponent<RectTransform>().sizeDelta = new Vector2(325, 600);
    }
    [ServerRpc]
    void PlayerChangeNameServerRpc()
    {
        PlayerChangeNameClientRpc();
    }
    [ClientRpc]
    void PlayerChangeNameClientRpc()
    {
        Text = GetComponentsInChildren<TMP_Text>();
    }
}
