using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using Netcode.Transports.PhotonRealtime;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class LobbySetPosittionPlayer : NetworkBehaviour
{
    private TMP_Text[] Text;
    private Transform Panel;
    int count = 0;
    private void Start()
    {
        
    }

    private void Singleton_OnDisconnetedCallback(ulong obj)
    {
        Panel.GetComponent<RectTransform>().SetWidth(Panel.GetComponent<RectTransform>().GetWidth() - 325);
    }

    // Start is called before the first frame update
    void Update()
    {
        if(Panel == null)
        {
            Panel = GameObject.Find("PlayerSlotPanel").GetComponent<Transform>();
            Panel.GetComponent<RectTransform>().SetWidth(Panel.GetComponent<RectTransform>().GetWidth() + 350);
            GetComponent<Transform>().SetParent(Panel);
            GetComponent<RectTransform>().sizeDelta = new Vector2(325, 500);
            NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback += Singleton_OnDisconnetedCallback;
        }
    }

    private void Singleton_OnClientConnectedCallback(ulong obj)
    {
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
        GetComponent<Transform>().SetParent(Panel);
        GetComponent<RectTransform>().sizeDelta = new Vector2(325, 500);
    }
}
