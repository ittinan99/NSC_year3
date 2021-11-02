using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class ElementCardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerDownHandler,IPointerUpHandler
{
    public ElementCard E_Card;

    public TextMeshProUGUI CardText;
    public TextMeshProUGUI F_CardText;
    public Image Artwork = null;

    public bool IsCombine;
    [SerializeField]
    private bool IsPressed;
    [SerializeField]
    private Vector2 StartPos;
    void Start()
    {
        IsCombine = false;
        IsPressed = false;
        StartPos = this.transform.position;
        CardText.text = E_Card.element_Name;
        F_CardText.text = E_Card.element_FName;
        if(E_Card.ArtWork != null)
        {
            Artwork.sprite = E_Card.ArtWork;
        }
    }
    private void Update()
    {
        if (IsPressed && !IsCombine)
        {
            this.transform.position = Input.mousePosition;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            StartPos = this.transform.position;
            IsPressed = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (IsCombine)
        {

        }
        else
        {
            this.transform.position = StartPos;
            IsPressed = false;
        }
        
    }
}
