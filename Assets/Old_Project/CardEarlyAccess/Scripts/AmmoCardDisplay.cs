using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class AmmoCardDisplay : NetworkBehaviour
{
    public ElementCardDisplay ECD_Card;

    public TextMeshProUGUI AmmoText;
    public Slider AmmoSlider;
    public float AmmoAmount;

    void Start()
    {
        
    }
    public void SetVar()
    {
        if(ECD_Card != null)
        {
            AmmoText.text = ECD_Card.E_Card.element_Name;
            AmmoAmount = ECD_Card.AmmoAmount;
            AmmoSlider.maxValue = ECD_Card.E_Card.Amount;
            AmmoSlider.value = AmmoAmount;
        }
        else
        {
            AmmoText.text = "";
            AmmoSlider.value = 0;
        }
        
    }
    public void SetAmmo()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
