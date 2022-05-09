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
    public void DespawnBin()
    {
        Task.SetActive(false);
        CP.gameObject.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {
        if (GameSystem.gamePhase == GameSystem.GamePhase.TaskState && other.CompareTag("Player") && other.gameObject.GetComponent<NetworkObject>().IsLocalPlayer)
        {
            Debug.Log("Enter : Bin");
            spawnBin();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (GameSystem.gamePhase == GameSystem.GamePhase.TaskState && other.CompareTag("Player") && other.gameObject.GetComponent<NetworkObject>().IsLocalPlayer)
        {
            Debug.Log("Left : Bin");
            DespawnBin();
        }
        else if(GameSystem.gamePhase == GameSystem.GamePhase.CombineState && other.gameObject.GetComponent<NetworkObject>().IsLocalPlayer)
        {
            Task.SetActive(false);
        }
    }
}
