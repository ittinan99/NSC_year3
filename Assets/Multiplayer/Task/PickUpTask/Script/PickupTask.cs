using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTask : MonoBehaviour
{
    public int CollectAmount;
    public GameObject ObjectivePrefab;
    public List<Transform> spawnPos;
    public int collected;
    public List<PickupEvent> AllObj;
    [SerializeField]
    private KeyCode PickupKey;
    [SerializeField]
    private CardPanel CP = null;
    private void Awake()
    {
        CP = GameObject.Find("CardPanel").GetComponent<CardPanel>();
    }
    void Start()
    {
       
    }

    public void spawnObjective()
    {
        AllObj = new List<PickupEvent>();
        spawnPos = new List<Transform>();
        collected = 0;
        foreach (Transform child in transform)
        {
            spawnPos.Add(child);
        }
        for (int i = 0; i <= spawnPos.Count - 1; i++)
        {
            GameObject Obj = Instantiate(ObjectivePrefab, spawnPos[i].position,Quaternion.identity);
            Obj.GetComponent<PickupEvent>().onTriggerStay.AddListener(PlayerPickup);
            AllObj.Add(Obj.GetComponent<PickupEvent>());
        }
    }
    void Update()
    {
        
    }
    void PlayerPickup(Collider col,GameObject obj)
    {
        if (col.gameObject.tag == "Player" && Input.GetKeyDown(PickupKey))
        {
            collected++;
            if(collected == CollectAmount)
            {
                Debug.Log("Pickup Task Complete");
                CP.SpawnCard(1);
            }
            Destroy(obj);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Enter : Pickup Task");
            spawnObjective();
        }
    }
}
