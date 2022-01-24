using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TaskTest : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private CardPanel CP;
    private void Awake()
    {
        CP = GameObject.Find("CardPanel").GetComponent<CardPanel>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&other.gameObject.GetComponent<NetworkObject>().IsLocalPlayer)
        {
            CP.SpawnCard(1);
            Debug.Log("TaskComp");
        }
    }
}
