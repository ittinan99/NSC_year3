using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class ElectricTask : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private CardPanel CP = null;
    public ElectricWire currentEW;
    public ElectricWire[] allEW;
    public GameObject Task;
    public bool TaskComp;
    private TaskList TL;
    private Animator TaskCompImage;
    private void Awake()
    {
        TaskCompImage = GameObject.Find("Taskcomp").GetComponent<Animator>();
        TL = GameObject.Find("TaskList").GetComponent<TaskList>();
        CP = GameObject.Find("CardPanel").GetComponent<CardPanel>();
    }
    void Start()
    {
        Task.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (GameSystem.gamePhase == GameSystem.GamePhase.TaskState && other.CompareTag("Player")  && Input.GetKeyDown(KeyCode.E) && !TaskComp
            && other.gameObject.GetComponent<NetworkObject>().IsLocalPlayer)
        {
            Debug.Log("Enter : Electric Task");
            SpawnElectricTask();
        }
    }
    public void SpawnElectricTask()
    {
        Task.SetActive(true);
        foreach(ElectricWire ew in allEW)
        {
            ew.ResetWire();
        }
    }
    public void CorrectWireAdd()
    {
        currentEW = null;
        int i = 0;
        foreach(ElectricWire eW in allEW)
        {
            if (eW.Correct)
            {
                i++;
            }
        }
        if(i == allEW.Length)
        {
            Debug.Log("Task Completed");
            TaskComp = true;
            CP.SpawnCard(2);
            TL.WireTaskComp();
            Task.SetActive(false);
            TaskCompImage.SetTrigger("Comp");
        }
        else
        {
            Debug.Log($"Wire Correct : {i}");
        }
    }

}
