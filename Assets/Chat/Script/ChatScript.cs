using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using Netcode.Transports.PhotonRealtime;

public class ChatScript : NetworkBehaviour
{
    public TMP_InputField TextInput;
    public RectTransform Content;
    public TMP_Text PrefabText;
    public Scrollbar scrollbar;
    MessageText messageText = new MessageText();
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            messageText.SetMassage("PlayerName", TextInput.text);
            SendTextServerRpc(messageText);
            scrollbar.value = 0;
            TextInput.text = "";
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void SendTextServerRpc(MessageText messageText)
    {
        SendTextClientRpc(messageText);
    }
    [ClientRpc]
    public void SendTextClientRpc(MessageText messageText)
    {
        Content.sizeDelta += new Vector2(0, 25);
        PrefabText.text = messageText.Message;
        Instantiate(PrefabText, Content);
        
    }

}
