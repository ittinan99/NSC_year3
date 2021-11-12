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
    public float currentHealth;
    public float maxHealth;
    private void Awake()
    {
        GS = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        PlayerCanvas = GameObject.FindGameObjectWithTag("PlayerCanvas");
        EndTurnButton = GameObject.Find("EndTurnButton").GetComponent<Button>();
        StartBut = GameObject.Find("StartGameBut").GetComponent<Button>();
    }
    void Start()
    {
        currentHealth = maxHealth;
        if (IsLocalPlayer)
        {
            StartStateServerRpc();
            if (IsOwnedByServer)
            {
                StartBut.onClick.AddListener(() => GS.startGame());
                Debug.Log("AddStartBut");
            }
        }
    }
    private void Update()
    {
        if (IsLocalPlayer)
        {
            EndTurnButton.interactable = false;
        }
        if (IsLocalPlayer&&isYourTurn)
        {
            EndTurnButton.interactable = true;
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
    public void TakeDamageServerRpc(float DamageAmount)
    {
        TakeDamageClientRpc(DamageAmount);
    }
    [ClientRpc]
    public void TakeDamageClientRpc(float DamageAmount)
    {
        currentHealth -= DamageAmount;
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
    public void EndTurn()
    {
        if (IsLocalPlayer)
        {
            EndTurnServerRpc();
        }
    }
    [ServerRpc]
    public void EndTurnServerRpc()
    {
        EndTurnClientRpc();
        GS.NextPlayerTurnServerRpc();
    }
    [ClientRpc]
    public void StartStateClientRpc()
    {
        PlayerState = GameState.Start;
        isYourTurn = false;
        EndTurnButton.onClick.AddListener(() => EndTurn());
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
