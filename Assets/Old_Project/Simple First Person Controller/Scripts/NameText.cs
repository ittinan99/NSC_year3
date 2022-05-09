using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NameText : MonoBehaviour
{
    public TMP_Text NameTextPrefab;
    TMP_Text nametext;
    public GameObject NamePos;
    public GameObject PlayerCanvas;
    // Start is called before the first frame update
    void Start()
    {
        PlayerCanvas = GameObject.Find("PlayerCanvas");
        nametext = Instantiate(NameTextPrefab,PlayerCanvas.transform);
        nametext.text = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerCanvas == null)
        {
            PlayerCanvas = GameObject.Find("PlayerCanvas");
        }
        Vector3 NameTextLabel = Camera.main.WorldToScreenPoint(NamePos.transform.position);
        nametext.transform.position = NameTextLabel;
    }
}
