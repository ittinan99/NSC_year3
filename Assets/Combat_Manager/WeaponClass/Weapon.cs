using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public abstract class Weapon : NetworkBehaviour,WeaponData
{
    [SerializeField] protected float _weaponDamage;
    [SerializeField] protected float _weight;
    [SerializeField] protected int _sellPrice;
    public float weaponDamage
    {
        get { return _weaponDamage; }
        set { _weaponDamage = value; }
    }

    public float critRate
    {
        get { return critRate; }
        set { critRate = value; }
    }

    public float weight
    {
        get { return _weight; }
        set { _weight = value; }
    }

    public int sellPrice
    {
        get { return _sellPrice; }
        set { _sellPrice = value; }
    }

    public virtual void Attack(AttackTarget target, float damage)
    {
        target.receiveAttack(damage);
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        
    }

}
