using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
public class LobbyManager : NetworkBehaviour 
{
    private TMP_Text[] Text;
    private Transform Panel;
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
            if (IsLocalPlayer) 
            {
                PlayerChangeNameServerRpc();
                count++;
            }
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
        Text[0].text = this.gameObject.name;
    }
}
