using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class BinTask : NetworkBehaviour
{
    // Start is called before the first frame update
    public CardPanel CP;
    public GameObject Task;
    void Start()
    {
        Task.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void spawnBin()
    {
        Task.SetActive(true);
        CP.gameObject.SetActive(true);
    }
    private void OnTriggerStay(Collider other)
    {
        if (GameSystem.gamePhase == GameSystem.GamePhase.TaskState && other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && other.gameObject.GetComponent<NetworkObject>().IsLocalPlayer)
        {
            Debug.Log("Enter : Bin");
            spawnBin();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject.GetComponent<NetworkObject>().IsLocalPlayer)
        {
            Debug.Log("Left : Bin");
            Task.SetActive(false);
            CP.gameObject.SetActive(false);
        }
    }
}