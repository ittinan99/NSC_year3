using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MedicalTask : NetworkBehaviour
{
    [SerializeField]
    private CardPanel CP = null;
    public GameObject Task;
    public SampleCollector SC;
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
        if (GameSystem.gamePhase == GameSystem.GamePhase.TaskState && other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && !TaskComp
            && other.gameObject.GetComponent<NetworkObject>().IsLocalPlayer)
        {
            Debug.Log("Enter : Medical Task");
            SpawnMedicalTask();
        }
    }
    public void SpawnMedicalTask()
    {

        Task.SetActive(true);
        SC.RandomSwap();
    }
    public void CompleteTask()
    {
        Debug.Log("Task Completed");
        CP.SpawnCard(2);
        TaskComp = true;
        TL.MedicalTaskComp();
        TaskCompImage.SetTrigger("Comp");
        Task.SetActive(false);
    }
}
