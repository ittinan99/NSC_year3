using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PickupEvent : MonoBehaviour
{
    public UnityEvent<Collider,GameObject> onTriggerStay;
    void OnTriggerStay(Collider col)
    {
        if (onTriggerStay != null) onTriggerStay.Invoke(col,this.gameObject);
    }
}
