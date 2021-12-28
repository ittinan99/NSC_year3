using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using Netcode.Transports.PhotonRealtime;
public class LobbySetPosittionPlayer : NetworkBehaviour
{
    private TMP_Text[] Text;
    private Transform Panel;
    private string Name;
    int count = 0;
    private void Start()
    {
        Panel = GameObject.Find("PlayerSlotPanel").GetComponent<Transform>();
        Panel.GetComponent<RectTransform>().SetWidth(Panel.GetComponent<RectTransform>().GetWidth()+325);
        GetComponent<Transform>().SetParent(Panel);
        GetComponent<RectTransform>().sizeDelta = new Vector2(325, 600);
        NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;
    }
    // Start is called before the first frame update
    void Update()
    {
    }

    private void Singleton_OnClientConnectedCallback(ulong obj)
    {
        Debug.Log("x");
        if (IsLocalPlayer)
        {
            SetParentServerRpc();
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
}
