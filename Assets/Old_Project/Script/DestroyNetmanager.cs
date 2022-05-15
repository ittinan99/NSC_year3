using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DestroyNetmanager : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        GameObject[] NetworkManagers = GameObject.FindGameObjectsWithTag("NetworkManager");
        if (NetworkManagers.Length > 1)
        {
            Destroy(NetworkManagers[1]);
        }

    }
}
