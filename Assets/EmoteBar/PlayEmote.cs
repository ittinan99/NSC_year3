using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Animations;
using Unity.Netcode;

public class PlayEmote : NetworkBehaviour
{
    // Start is called before the first frame update
    public Animator PlayerAnimator;
    public GameObject EmoteBar;
    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                EmoteBar.SetActive(!EmoteBar.activeSelf);
            }
        }
    }

    public void Emote(int var)
    {
        PlayerAnimator.Play($"Dance{var}");
        EmoteServerRpc(var);
        EmoteBar.SetActive(!EmoteBar.activeSelf);
    }
    [ServerRpc]
    public void EmoteServerRpc(int var)
    {
        PlayerAnimator.Play($"Dance{var}");
    }
}
