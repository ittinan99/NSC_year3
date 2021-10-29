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
    [SerializeField]
    private GameObject[] PlayerList;
    private enum GamePhase {Start,CombineState,AttackState,End}
    [SerializeField]
    private GamePhase gamePhase;
    [SerializeField]
    private Button StartBut;
    private void Awake()
    {
        PlayerList = new GameObject[0];
        StartBut = GameObject.Find("StartGameBut").GetComponent<Button>();
    }
    void Start()
    {
        StartBut.onClick.AddListener(() => StartGameServerRpc());
    }
    private void Update()
    {
        if (IsOwnedByServer)
        {
            StartBut.interactable = true;
        }
        else
        {
            StartBut.interactable = false;
        }
    }
    // Update is called once per frame
    [ServerRpc]
    void Rand_startPlayerServerRpc()
    {
        Rand_startPlayerClientRpc();
    }
    [ClientRpc]
    void Rand_startPlayerClientRpc()
    {
        gamePhase = GamePhase.CombineState;
        PlayerList = GameObject.FindGameObjectsWithTag("Player");
        int randNum = Random.Range(0, PlayerList.Length - 1);
        GameObject StartPlayer = PlayerList[randNum];
        CurrentPlayerTurn = StartPlayer;
        CurrentPlayerIndex = randNum;
        StartPlayer.GetComponent<TurnBaseSystem>().isYourTurn = true;
        StartPlayer.GetComponent<TurnBaseSystem>().PlayerState = TurnBaseSystem.GameState.Y_CombineTurn;
    }
    [ServerRpc]
    void StartGameServerRpc()
    {
        StartGameClientRpc();
    }
    
    [ClientRpc]
    void StartGameClientRpc()
    {
        gamePhase = GamePhase.Start;
        Rand_startPlayerServerRpc();
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
