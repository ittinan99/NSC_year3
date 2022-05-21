using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class EnemyAttackTarget : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        Debug.Log("Attack");
        context.gameObject.GetComponent<enemyAnimController>().AttackServerRpc();
        return State.Success;
    }
}
