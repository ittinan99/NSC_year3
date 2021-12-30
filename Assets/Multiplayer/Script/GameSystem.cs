using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Photon.Realtime;
using Netcode.Transports.PhotonRealtime;
public class GameSystem : NetworkBehaviour
{
    public static TurnBaseSystem localTurnbased = null;
    [SerializeField]
    private int CurrentPlayerIndex;
    //public static GameObject CurrenTarget;
    public GameObject[] PlayerList;
    
    public enum GamePhase {Start,TaskState,CombineState,AttackState,End}
    [SerializeField]
    public static GamePhase gamePhase;
    public PhaseTimer PT;
    private void Awake()
    {
        PlayerList = new GameObject[0];
        
    }
    void Start()
    {
        //CurrenTarget = null;
    }
    private void Update()
    {
        
    }
    // Update is called once per frame
    //[ServerRpc]
    //void Rand_startPlayerServerRpc()
    //{
    //    Rand_startPlayerClientRpc();
    //}
    //[ClientRpc]
    void Gather_startPlayerClient()
    {
        foreach(GameObject player in PlayerList)
        {
            if (player.GetComponent<NetworkObject>().IsLocalPlayer)
            {
                localTurnbased = player.GetComponent<TurnBaseSystem>();
                localTurnbased.HideCardPanel();
            }
        }
    }
    public void startGame()
    {
        StartGameServerRpc();
    }
    [ServerRpc]
    public void StartGameServerRpc()
    {
        StartGameClientRpc();
    }
    
    [ClientRpc]
    public void StartGameClientRpc()
    {
        Debug.Log("GameStart");
        gamePhase = GamePhase.Start;
        gamePhase = GamePhase.TaskState;
        PT.TaskCountDownMethod();
        PlayerList = GameObject.FindGameObjectsWithTag("Player");
        Gather_startPlayerClient();

    }
    [ServerRpc]
    public void CombinePhaseServerRpc()
    {
        CombinePhaseClientRpc();
    }
    [ClientRpc]
    public void CombinePhaseClientRpc()
    {
        Debug.Log("CombinePhase");
        gamePhase = GamePhase.CombineState;
        localTurnbased.HideShowPanel();
    }
    //[ServerRpc]
    //public void NextPlayerTurnServerRpc()
    //{
    //    NextPlayerTurnClientRpc();
    //}
    //[ClientRpc]
    //public void NextPlayerTurnClientRpc()
    //{
    //    localTurnbased.CombinePanel.SetActive(false);
    //    EndTurnCount++;
    //    if(EndTurnCount == PlayerList.Length && gamePhase == GamePhase.CombineState)
    //    {
    //        gamePhase = GamePhase.AttackState;
    //        localTurnbased.CombinePanel.SetActive(false);
    //        EndTurnCount = 0;
    //        Debug.Log(gamePhase);
    //    }
    //    else if(EndTurnCount == PlayerList.Length && gamePhase == GamePhase.AttackState)
    //    {
    //        gamePhase = GamePhase.CombineState;
    //        EndTurnCount = 0;
    //        Debug.Log(gamePhase);
    //    }
    //    CurrentPlayerIndex++;
    //    if(CurrentPlayerIndex  > PlayerList.Length - 1)
    //    {
    //        CurrentPlayerIndex = 0;
    //    }
    //    Debug.Log("Turn : Player " + CurrentPlayerIndex);
    //    CurrentPlayerTurn = PlayerList[CurrentPlayerIndex];
    //    CurrentPlayerTurn.GetComponent<TurnBaseSystem>().isYourTurn = true;
    //    if (gamePhase == GamePhase.CombineState)
    //    {
    //        CurrentPlayerTurn.GetComponent<TurnBaseSystem>().PlayerState = TurnBaseSystem.GameState.Y_CombineTurn;
    //        if(localTurnbased == CurrentPlayerTurn.GetComponent<TurnBaseSystem>())
    //        {
    //            CurrentPlayerTurn.GetComponent<TurnBaseSystem>().CombinePanel.SetActive(true);
    //            CurrentPlayerTurn.GetComponent<TurnBaseSystem>().cardPanel.SpawnCard(1);
    //        }
    //    }
    //    else
    //    {
    //        CurrentPlayerTurn.GetComponent<TurnBaseSystem>().PlayerState = TurnBaseSystem.GameState.Y_AttackTurn;
    //    }
    //}

}
