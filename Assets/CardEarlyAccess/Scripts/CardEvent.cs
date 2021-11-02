using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardUnityEvent : UnityEvent<ElementCard>
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
    public void CardAddListener(UnityAction<ElementCard> cardEvent)
    {
        CUE.AddListener(cardEvent);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Card") && IsEmpty) 
        {
            Debug.Log("Place Card");
            IsEmpty = false;
            other.GetComponent<ElementCardDisplay>().IsCombine = true;
            other.transform.position = this.transform.position;
            CUE.Invoke(other.GetComponent<ElementCardDisplay>().E_Card);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Card") && !IsEmpty)
        {
            IsEmpty = true;
            other.GetComponent<ElementCardDisplay>().IsCombine = false;
            CUE.Invoke(null) ;
        }
    }
}
