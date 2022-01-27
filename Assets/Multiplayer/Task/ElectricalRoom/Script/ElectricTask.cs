using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTask : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private CardPanel CP = null;
    public ElectricWire currentEW;
    public ElectricWire[] allEW;
    public GameObject Task;
    public bool TaskComp;
    private TaskList TL;
    private void Awake()
    {
        TL = GameObject.Find("TaskList").GetComponent<TaskList>();
        CP = GameObject.Find("CardPanel").GetComponent<CardPanel>();
    }
    void Start()
    {
        allEW = GameObject.FindObjectsOfType<ElectricWire>();
        Task.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (GameSystem.gamePhase == GameSystem.GamePhase.TaskState && other.CompareTag("Player")  && Input.GetKeyDown(KeyCode.E) && !TaskComp)
        {
            Debug.Log("Enter : Electric Task");
            SpawnElectricTask();
        }
    }
    public void SpawnElectricTask()
    {
        Task.SetActive(true);
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
            CP.SpawnCard(1);
            TL.WireTaskComp();
            Task.SetActive(false);
        }
        else
        {
            Debug.Log($"Wire Correct : {i}");
        }
    }

}
