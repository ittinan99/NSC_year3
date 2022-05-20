using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TeleportParty : NetworkBehaviour
{
    int AmountPlayerInWarpPoint = 0;
    public Button StartButton;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(AmountPlayerInWarpPoint == NetworkManager.Singleton.ConnectedClientsIds.Count)
        {
            StartButton.gameObject.SetActive(true);
        }
        else
        {
            StartButton.gameObject.SetActive(false);
        }
    }
    public void TeleportTo(string TeleportPath)
    {
        OnloadSceneServerRpc(TeleportPath);
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        SceneManager.LoadSceneAsync(TeleportPath, LoadSceneMode.Additive);
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Scene OldScene = SceneManager.GetActiveScene();
        GameObject[] Player = GameObject.FindGameObjectsWithTag("Player");
        for (int PlayerAmountCount = 0; PlayerAmountCount < Player.Length; PlayerAmountCount++)
        {
            Player[PlayerAmountCount].transform.position = new Vector3(61.7f, 9.6f, 77.3f);
            SceneManager.MoveGameObjectToScene(Player[PlayerAmountCount], arg0);
        }
        SceneManager.SetActiveScene(arg0);
        SceneManager.UnloadSceneAsync(OldScene);
    }

    [ServerRpc]
    public void OnloadSceneServerRpc(string TeleportPath)
    {
        OnloadSceneClientRpc(TeleportPath);
    }
    [ClientRpc]
    public void OnloadSceneClientRpc(string TeleportPath)
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        SceneManager.LoadSceneAsync(TeleportPath, LoadSceneMode.Additive);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<NetworkObject>().IsPlayerObject)
        {
            AmountPlayerInWarpPoint++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<NetworkObject>().IsPlayerObject)
        {
            AmountPlayerInWarpPoint--;
        }
    }
}
