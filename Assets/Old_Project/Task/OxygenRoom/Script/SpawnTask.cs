using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTask : MonoBehaviour
{
    public GameObject CanvasTask;
    public void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                CanvasTask.SetActive(true);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                CanvasTask.SetActive(true);
            }
        }
    }
}
