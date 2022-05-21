using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class enemyAnimController : NetworkBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public bool currentAnimatorStateBaseIsName(string paramName)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(paramName);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    [ServerRpc]
    public void MovingServerRpc(bool value)
    {
        anim.SetBool("isMoving", value);
    }
    [ServerRpc]
    public void AttackServerRpc()
    {
        AttackClientRpc();
    }
    [ClientRpc]
    public void AttackClientRpc()
    {
        anim.SetTrigger("Attack");
    }
}
