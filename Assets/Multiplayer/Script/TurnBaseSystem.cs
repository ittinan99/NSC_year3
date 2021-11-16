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
    public GameObject CombinePanel;
    [SerializeField]
    public CardPanel cardPanel;
    [SerializeField]
    public Button EndTurnButton;
    [SerializeField]
    private Button StartBut;
    [SerializeField]
    NetworkVariable<float> currentHealth = new NetworkVariable<float>(readPerm: NetworkVariableReadPermission.Everyone);
    public float maxHealth;
    public GameObject FlaskBarrel;
    void Start()
    {
        CombinePanel = GameObject.Find("CombineSystem");
        CombinePanel.SetActive(false);
        cardPanel = GameObject.Find("CardPanel").GetComponent<CardPanel>();
        if(IsLocalPlayer)
        {
            getcomServerRpc();
        }
        currentHealth = new NetworkVariable<float>(maxHealth);
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
            if(PlayerState == GameState.Y_CombineTurn)
            {
                CombinePanel.SetActive(true);
            }
            else
            {
                CombinePanel.SetActive(false);
            }
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
    public void getcomServerRpc()
    {
        getcomClientRpc();
    }
    [ClientRpc]
    public void getcomClientRpc()
    {
        GS = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        PlayerCanvas = GameObject.FindGameObjectWithTag("PlayerCanvas");
        EndTurnButton = GameObject.Find("EndTurnButton").GetComponent<Button>();
        StartBut = GameObject.Find("StartGameBut").GetComponent<Button>();
    }
    [ServerRpc]
    public void AttackCurrentTargetServerRpc()
    {
        AttackCurrentTargetClientRpc();
    }
    [ClientRpc]
    public void AttackCurrentTargetClientRpc()
    {
        if (Physics.Raycast(FlaskBarrel.transform.position, FlaskBarrel.transform.forward, out RaycastHit hit, 200))
        {
            GameObject enemy = hit.transform.gameObject;
            if (enemy.CompareTag("Player"))
            {
                enemy.GetComponent<TurnBaseSystem>().TakeDamage(10);
                Debug.Log("Attack");
            }

        }
        Debug.Log("AttackClient");
    }
    public void TakeDamage(float DamageAmount)
    {
        Debug.Log("TakeDamage");
        currentHealth.Value -= DamageAmount;
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
