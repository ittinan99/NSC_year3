using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMeleeWeapon 
{
    float attackSpeed { get; }
    float staggerDamage { get; }
    statusEffect weaponStatus { get; }
}
public enum statusEffect
{
    burn,
    paralysis,
    bleed,
    poison,
    none
}
