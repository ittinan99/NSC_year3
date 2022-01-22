using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class ScanBottle : MonoBehaviour
{
    public GameObject Spawner;
    private void Start()
    {
        DeSpawn(4f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject != Spawner)
        {
            if(other.GetComponent<TurnBaseSystem>() != null)
            {
                other.GetComponent<TurnBaseSystem>().GetScanServerRpc();
                Destroy(this.gameObject);
            }
        }
    }
    public async void DeSpawn(float Duration)
    {
        float currentTime = 0;
        currentTime = Duration;
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            await Task.Yield();
        }
        Destroy(this.gameObject);
    }
}
