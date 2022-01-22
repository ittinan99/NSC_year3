using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanBottle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(other.GetComponent<TurnBaseSystem>() != null)
            {
                other.GetComponent<TurnBaseSystem>().GetScanServerRpc();
            }
        }
    }
}
