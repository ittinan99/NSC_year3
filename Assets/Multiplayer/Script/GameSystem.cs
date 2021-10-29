using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Photon.Realtime;
using Netcode.Transports.PhotonRealtime;
public class GameSystem : NetworkBehaviour
{
    [SerializeField]
    private GameObject CurrentPlayerTurn;

    [SerializeField]
    private int CurrentPlayerIndex;
 
    public GameObject[] PlayerList;
    private enum GamePhase {Start,CombineState,AttackState,End}
    [SerializeField]
    private GamePhase gamePhase;
   
    private void Awake()
    {
        PlayerList = new GameObject[0];
        
    }
    void Start()
    {
        
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
    void Rand_startPlayerClient()
    {
        gamePhase = GamePhase.CombineState;
        PlayerList = GameObject.FindGameObjectsWithTag("Player");
        int randNum = Random.Range(0, PlayerList.Length - 1);
        GameObject StartPlayer = PlayerList[randNum];
        CurrentPlayerTurn = StartPlayer;
        CurrentPlayerIndex = randNum;
        Debug.Log("StartPlayer : Player " + CurrentPlayerIndex);
        StartPlayer.GetComponent<TurnBaseSystem>().isYourTurn = true;
        StartPlayer.GetComponent<TurnBaseSystem>().PlayerState = TurnBaseSystem.GameState.Y_CombineTurn;
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

        Rand_startPlayerClient();

    }
    [ServerRpc]
    public void NextPlayerTurnServerRpc()
    {
        NextPlayerTurnClientRpc();
    }
    [ClientRpc]
    public void NextPlayerTurnClientRpc()
    {
        CurrentPlayerIndex++;
        if(CurrentPlayerIndex  > PlayerList.Length - 1)
        {
            CurrentPlayerIndex = 0;
        }
        Debug.Log("Turn : Player " + CurrentPlayerIndex);
        CurrentPlayerTurn = PlayerList[CurrentPlayerIndex];
        CurrentPlayerTurn.GetComponent<TurnBaseSystem>().isYourTurn = true;
        if(gamePhase == GamePhase.CombineState)
        {
            CurrentPlayerTurn.GetComponent<TurnBaseSystem>().PlayerState = TurnBaseSystem.GameState.Y_CombineTurn;
        }
        else
        {
            CurrentPlayerTurn.GetComponent<TurnBaseSystem>().PlayerState = TurnBaseSystem.GameState.Y_AttackTurn;
        }
    }

}
