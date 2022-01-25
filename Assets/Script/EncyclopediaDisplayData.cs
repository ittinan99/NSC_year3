using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EncyclopediaDisplayData : MonoBehaviour
{
    public string CardS_Name;
    public string CardF_Name;
    public string Description;
    public Sprite Artwork = null;
    public TMP_Text CardF_NameText;
    public TMP_Text CardDescription;
    public Image PopupSprite;
    public void Start()
    {
        
    }
    public void Update()
    {
        CardF_NameText.text = CardF_Name;
        CardDescription.text = Description;
        PopupSprite.sprite = Artwork;
    }
}
