using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class DamageBottle : MonoBehaviour
{
    private void Start()
    {
        DeSpawn(4f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<TurnBaseSystem>() != null)
            {
                other.GetComponent<TurnBaseSystem>().TakeDamage(10);
                Debug.Log("Hit");
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
