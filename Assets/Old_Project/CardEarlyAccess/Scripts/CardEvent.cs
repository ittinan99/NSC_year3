using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardUnityEvent : UnityEvent<ElementCardDisplay>
{
}
public class CardEvent : MonoBehaviour
{
    public bool IsEmpty;
    CardUnityEvent CUE = new CardUnityEvent();
    private void Start()
    {
        IsEmpty = true;
    }
    public void CardAddListener(UnityAction<ElementCardDisplay> cardEvent)
    {
        CUE.AddListener(cardEvent);
    }
    public void PlaceCard(ElementCardDisplay eCard)
    {
        Debug.Log("Place Card");
        IsEmpty = false;
        CUE.Invoke(eCard);
    }
    public void RemoveCard()
    {
        IsEmpty = true;
        CUE.Invoke(null);
    }
    //private void OnTriggerStay2D(Collider2D other)
    //{
    //    if (other.CompareTag("Card") && IsEmpty) 
    //    {
    //        Debug.Log("Place Card");
    //        IsEmpty = false;
    //        CUE.Invoke(other.GetComponent<ElementCardDisplay>().E_Card);
    //    }
    //}
    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.CompareTag("Card") && !IsEmpty)
    //    {
    //        IsEmpty = true;
    //        CUE.Invoke(null) ;
    //    }
    //}
}
