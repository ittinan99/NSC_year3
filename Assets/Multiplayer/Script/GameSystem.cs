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

    public static TurnBaseSystem localTurnbased = null;
    [SerializeField]
    private int CurrentPlayerIndex;
 
    public GameObject[] PlayerList;
    public enum GamePhase {Start,CombineState,AttackState,End}
    [SerializeField]
    public static GamePhase gamePhase;
    [SerializeField]
    private int EndTurnCount;
   
    private void Awake()
    {
        PlayerList = new GameObject[0];
        
    }
    void Start()
    {
        EndTurnCount = 0;
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
        foreach(GameObject player in PlayerList)
        {
            if (player.GetComponent<NetworkObject>().IsLocalPlayer)
            {
                localTurnbased = player.GetComponent<TurnBaseSystem>();
            }
        }
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
        EndTurnCount++;
        if(EndTurnCount == PlayerList.Length && gamePhase == GamePhase.CombineState)
        {
            gamePhase = GamePhase.AttackState;
            Debug.Log(gamePhase);
        }
        else if(EndTurnCount == PlayerList.Length && gamePhase == GamePhase.AttackState)
        {
            gamePhase = GamePhase.CombineState;
            Debug.Log(gamePhase);
        }
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
