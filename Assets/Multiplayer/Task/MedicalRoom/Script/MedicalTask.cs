using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicalTask : MonoBehaviour
{
    [SerializeField]
    private CardPanel CP = null;
    public GameObject Task;
    private void Awake()
    {
        //CP = GameObject.Find("CardPanel").GetComponent<CardPanel>();
    }
    void Start()
    {
        //Task.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Enter : Electric Task");
            SpawnMedicalTask();
        }
    }
    public void SpawnMedicalTask()
    {
        Task.SetActive(true);
    }
  

}
