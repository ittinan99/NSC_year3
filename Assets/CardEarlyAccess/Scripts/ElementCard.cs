using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="NewElementCard",menuName = "elementCard")]
public class ElementCard : ScriptableObject,ICombinable,IAttackable
{
    public string element_Name;
    public string element_FName;

    public bool CanCombine;
    public bool CanAttack;

    public Sprite ArtWork;

    public string description;

    public void Attack()
    {
        if (CanAttack)
        {
            throw new System.NotImplementedException();
        }
    }

    public void Combine()
    {
        if (CanCombine)
        {
            throw new System.NotImplementedException();
        }
    }

    public void Uncombine()
    {
        if (CanCombine)
        {
            throw new System.NotImplementedException();
        }
    }
   

}
public interface ICombinable
{
    void Combine();
    void Uncombine();
}
public interface IAttackable
{
    void Attack();
}
