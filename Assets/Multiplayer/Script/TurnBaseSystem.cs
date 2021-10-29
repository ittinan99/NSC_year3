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
    private GameObject PlayerCanvas;
    [SerializeField]
    private Button EndTurnButton;

    public bool isYourTurn;
    private void Awake()
    {
        PlayerCanvas = GameObject.FindGameObjectWithTag("PlayerCanvas");
        EndTurnButton = GameObject.Find("EndTurnButton").GetComponent<Button>();
    }
    void Start()
    {
        StartStateServerRpc();
        if (IsLocalPlayer)
        {
            PlayerCanvas.SetActive(true);
        }
        else
        {
            PlayerCanvas.SetActive(false);
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
