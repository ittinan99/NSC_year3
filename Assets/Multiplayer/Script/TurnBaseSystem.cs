using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;
using Netcode.Transports.PhotonRealtime;
public class TurnBaseSystem : NetworkBehaviour
{
    public enum GameState { Start,otherTurn,Y_CombineTurn, Y_AttackTurn,Win,Lose}
    public GameState PlayerState;
    [SerializeField]
    private GameSystem GS;
    public bool isYourTurn;
    [SerializeField]
    public GameObject PlayerCanvas;
    [SerializeField]
    public Button EndTurnButton;
    [SerializeField]
    private Button StartBut;
    private void Awake()
    {
        GS = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        PlayerCanvas = GameObject.FindGameObjectWithTag("PlayerCanvas");
        EndTurnButton = GameObject.Find("EndTurnButton").GetComponent<Button>();
        StartBut = GameObject.Find("StartGameBut").GetComponent<Button>();
    }
    void Start()
    {
        if (IsLocalPlayer)
        {
            StartStateServerRpc();
            if (IsOwnedByServer)
            {
                StartBut.onClick.AddListener(() => GS.StartGameServerRpc());
                Debug.Log("AddStartBut");
            }
        }
    }
    private void Update()
    {
        if (isYourTurn)
        {
            EndTurnButton.interactable = true;
        }
        else
        {
            EndTurnButton.interactable = false;
        }
        if (IsLocalPlayer)
        {
            PlayerCanvas.SetActive(true);
        }
        if (IsLocalPlayer)
        {
            StartBut.interactable = false;
        }
        if(IsLocalPlayer&&IsOwnedByServer)
        {
            StartBut.interactable = true;
        }
    }
    [ServerRpc]
    public void StartStateServerRpc()
    {
        StartStateClientRpc();
    }
    [ServerRpc]
    public void Y_CombineStateServerRpc()
    {
        Y_CombineStateClientRpc();
    }
    [ServerRpc]
    public void EndTurnServerRpc()
    {
        EndTurnClientRpc();
    }
    [ClientRpc]
    public void StartStateClientRpc()
    {
        PlayerState = GameState.Start;
        isYourTurn = false;
        EndTurnButton.onClick.AddListener(() => EndTurnServerRpc());
    }
    [ClientRpc]
    public void Y_CombineStateClientRpc()
    {
        PlayerState = GameState.Y_CombineTurn;
    }
    [ClientRpc]
    public void EndTurnClientRpc()
    {
        PlayerState = GameState.otherTurn;
        isYourTurn = false;
    }

}
