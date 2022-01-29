using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class PickupTask : NetworkBehaviour
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
    public bool TaskComp;

    public GameObject Task;
    public TextMeshProUGUI Pickupwhat;
    public string PickupText;

    private TaskList TL;
    private void Awake()
    {
        TL = GameObject.Find("TaskList").GetComponent<TaskList>();
        CP = GameObject.Find("CardPanel").GetComponent<CardPanel>();
        TaskComp = false;
    }
    void Start()
    {
        Task.SetActive(false);
    }

    public void spawnObjective()
    {
        Task.SetActive(true);
        AllObj = new List<PickupEvent>();
        spawnPos = new List<Transform>();
        collected = 0;
        foreach (Transform child in transform)
        {
            if(child.tag != "TaskCanvas")
            {
                spawnPos.Add(child);
            }
        }
        for (int i = 0; i <= spawnPos.Count - 1; i++)
        {
            GameObject Obj = Instantiate(ObjectivePrefab, spawnPos[i].position,Quaternion.identity);
            Obj.GetComponent<PickupEvent>().onTriggerStay.AddListener(PlayerPickup);
            AllObj.Add(Obj.GetComponent<PickupEvent>());
        }
        Pickupwhat.text = $"{PickupText} : {collected}/{CollectAmount} ";
    }
    void Update()
    {
        
    }
    public void DestroyAllSpawn()
    {
        AllObj.RemoveAll(x =>x==null);
        if(AllObj.Count > 0)
        {
            foreach (PickupEvent obj in AllObj)
            {
                Destroy(obj.gameObject);
            }
        }
        AllObj.Clear();
    }
    void PlayerPickup(Collider col,GameObject obj)
    {
        if (col.gameObject.tag == "Player" && Input.GetKeyDown(PickupKey))
        {
            collected++;
            Pickupwhat.text = $"{PickupText} : {collected}/{CollectAmount} ";
            if (collected == CollectAmount)
            {
                Debug.Log("Pickup Task Complete");
                TL.PickupTaskComp();
                TaskComp = true;
                CP.SpawnCard(1);
                Task.SetActive(false);
            }
            Destroy(obj);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (GameSystem.gamePhase == GameSystem.GamePhase.TaskState && other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && !TaskComp
            && other.gameObject.GetComponent<NetworkObject>().IsLocalPlayer)
        {
            Debug.Log("Enter : Pickup Task");
            spawnObjective();
        }
    }
}
