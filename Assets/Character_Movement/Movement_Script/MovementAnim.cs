using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Animations;
public class MovementAnim : NetworkBehaviour
{
    public Animator playerAnim;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    [ServerRpc]
    public void AnimationStateServerRpc(string paramName)
    {
        AnimationStateClientRpc(paramName);
    }
    [ClientRpc]
    public void AnimationStateClientRpc(string paramName)
    {
        foreach (AnimatorControllerParameter parameter in playerAnim.parameters)
        {
            playerAnim.SetBool(parameter.name, false);
        }
        playerAnim.SetBool(paramName, true);
    }
    [ServerRpc]
    public void DodgeServerRpc()
    {
        DodgeClientRpc();
    }
    [ClientRpc]
    public void DodgeClientRpc()
    {
        if (playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Dodge")) { return; }

        playerAnim.SetTrigger("roll");
    }

 }
