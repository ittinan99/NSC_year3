using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPanel : MonoBehaviour
{
    // Start is called before the first frame update
    public CardPanel CP;
    public List<ElementCardDisplay> AmmoCard;
    public ElementCardDisplay CurrentAmmo;
    [SerializeField]
    private int CurrentAmmoIndex;
    public AmmoCardDisplay DisplayAmmo;
    public ElementCard Empty;
    [SerializeField]
    private void Awake()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameSystem.gamePhase == GameSystem.GamePhase.AttackState)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f && AmmoCard.Count > 0) // forward
            {
                CurrentAmmoIndex++;
                if (CurrentAmmoIndex > AmmoCard.Count-1)
                {
                    CurrentAmmoIndex = 0;
                }
                CurrentAmmo = AmmoCard[CurrentAmmoIndex];
                DisplayAmmo.ECD_Card = AmmoCard[CurrentAmmoIndex];
                DisplayAmmo.SetVar();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f && AmmoCard.Count > 0) // backwards
            {
                CurrentAmmoIndex--;
                if (CurrentAmmoIndex < 0)
                {
                    CurrentAmmoIndex = AmmoCard.Count - 1;
                }
                CurrentAmmo = AmmoCard[CurrentAmmoIndex];
                DisplayAmmo.ECD_Card = AmmoCard[CurrentAmmoIndex];
                DisplayAmmo.SetVar();
            }
        }
    }
    public void AddAmmoCard()
    {
        CurrentAmmoIndex = 0;
        AmmoCard = new List<ElementCardDisplay>();
        foreach(ElementCardDisplay hcard in CP.hCard)
        {
            if (hcard.E_Card.CanAttack)
            {
                AmmoCard.Add(hcard);
            }
        }
        CurrentAmmo = AmmoCard[CurrentAmmoIndex];
        DisplayAmmo.ECD_Card = CurrentAmmo;
        DisplayAmmo.SetVar();
       
    }
    public void SetAmmoCardValue()
    {
        DisplayAmmo.AmmoAmount = CurrentAmmo.AmmoAmount;
    }
}
