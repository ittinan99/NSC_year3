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
    private void Awake()
    {
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
        if (other.CompareTag("Player")  && Input.GetKeyDown(KeyCode.E))
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
            CP.SpawnCard(1);
            Task.SetActive(false);
        }
        else
        {
            Debug.Log($"Wire Correct : {i}");
        }
    }

}
