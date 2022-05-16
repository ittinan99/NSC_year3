using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Animations;
public class MovementAnim : NetworkBehaviour
{

    public Animator playerAnim;

    [Header("Combat Manager")]
    [SerializeField]
    private int combatLayerIndex;
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
            if(parameter.name != "isCombat")
            {
                playerAnim.SetBool(parameter.name, false);
            }
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
    [ServerRpc]
    public void changeCombatLayerWeightServerRpc(float weight)
    {
        changeCombatLayerWeightClientRpc(weight);
    }
    [ClientRpc]
    public void changeCombatLayerWeightClientRpc(float weight)
    {
        StartCoroutine(SwitchStance(weight));
    }
    IEnumerator SwitchStance(float weight)
    {
        if(weight == 1)
        {
            playerAnim.SetLayerWeight(combatLayerIndex, weight);
            playerAnim.SetBool("isCombat", !playerAnim.GetBool("isCombat"));
        }
        else
        {
            playerAnim.SetBool("isCombat", !playerAnim.GetBool("isCombat"));
            yield return new WaitForSeconds(2.5f);
            playerAnim.SetLayerWeight(combatLayerIndex, weight);
        }
    }

}
