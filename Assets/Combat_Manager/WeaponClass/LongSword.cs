using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class LongSword : Weapon,IMeleeWeapon
{
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _staggerDamage;
    [SerializeField] private statusEffect _weaponStatus;
    public float attackSpeed
    {
        get { return _attackSpeed; }
        set { _attackSpeed = value; }
    }
    public float staggerDamage
    {
        get { return _staggerDamage; }
        set { _staggerDamage = value; }
    }

    public statusEffect weaponStatus
    {
        get { return _weaponStatus; }
        set { _weaponStatus = value; }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Attack(AttackTarget target, float damage)
    {
        base.Attack(target,damage);
        Debug.Log("Attack");
    }
}
