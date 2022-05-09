using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class FlaskEnemy : NetworkBehaviour
{
    public GameObject Enemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Enemy = other.gameObject;
        }
        else
        {
            Enemy = null;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Enemy = null;
        }
    }
}
