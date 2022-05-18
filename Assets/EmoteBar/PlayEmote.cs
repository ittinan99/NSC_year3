using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayEmote : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator PlayerAnimator;
    public GameObject EmoteBar;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            EmoteBar.SetActive(!EmoteBar.activeSelf);
        }
    }

    public void Emote1()
    {
        PlayerAnimator.Play("Dance Moves");
        EmoteBar.SetActive(!EmoteBar.activeSelf);
    }

}
