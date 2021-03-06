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
    NetworkVariable<float> currentHealth = new NetworkVariable<float>();
    public float CurrentHealth;
    public float maxHealth;
    public GameObject FlaskBarrel;

    [SerializeField]
    private ElementCardDisplay ATKcard;

    private Ray ray;
    void Start()
    {
        //if(IsLocalPlayer)
        //{
        //    getcomServerRpc();
        //}
        StartState();
        currentHealth = new NetworkVariable<float>(maxHealth);
        if (IsLocalPlayer)
        {
            if (IsOwnedByServer)
            {
                StartBut.onClick.AddListener(() => GS.startGame());
                Debug.Log("AddStartBut");
            }
        }
    }
    private void Update()
    {
        CurrentHealth = currentHealth.Value;
        if (PlayerCanvas == null)
        {
            PlayerCanvas  = GameObject.Find("PlayerCanvas");
        }
        if (CombinePanel == null)
        {
            CombinePanel = GameObject.Find("CombineSystem");
        }
        if (EndTurnButton == null)
        {
            EndTurnButton = GameObject.Find("EndTurnButton").GetComponent<Button>();
            currentHealth = new NetworkVariable<float>(maxHealth);
            EndTurnButton.onClick.AddListener(() => EndTurn());
        }
        if (StartBut == null)
        {
            StartBut = GameObject.Find("StartGameBut").GetComponent<Button>();
        }
        if(GS == null)
        {
            GS = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        }
        if (cardPanel == null)
        {
            cardPanel = GameObject.Find("CardPanel").GetComponent<CardPanel>();
        }
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
    //[ServerRpc]
    //public void getcomServerRpc()
    //{
    //    getcomClientRpc();
    //}
    //[ClientRpc]
    //public void getcomClientRpc()
    //{
    //    GS = GameObject.Find("GameSystem").GetComponent<GameSystem>();
    //    PlayerCanvas = GameObject.FindGameObjectWithTag("PlayerCanvas");
    //    EndTurnButton = GameObject.Find("EndTurnButton").GetComponent<Button>();
    //    StartBut = GameObject.Find("StartGameBut").GetComponent<Button>();
    //}
    public void ATKcardFunc(ElementCardDisplay atkCard)
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        ATKcard = atkCard;

        AttackCurrentTargetServerRpc();
    }
    [ServerRpc]
    public void AttackCurrentTargetServerRpc()
    {
        
        Debug.Log("AttackServer");
        
        AttackCurrentTargetClientRpc();
    }
    [ClientRpc]
    public void AttackCurrentTargetClientRpc()
    {
        if (FlaskBarrel.GetComponent<FlaskEnemy>().Enemy != null)
        {
            GameObject enemy = FlaskBarrel.GetComponent<FlaskEnemy>().Enemy;
            enemy.GetComponent<TurnBaseSystem>().TakeDamage(10);
            Debug.Log(enemy.GetComponent<TurnBaseSystem>().currentHealth.Value);
            if (IsLocalPlayer)
            {
                cardPanel.GetComponent<CardPanel>().hCard.Remove(ATKcard);
                cardPanel.GetComponent<CardPanel>().SetCardPos();
                Destroy(ATKcard.gameObject);
            }
            Debug.Log("Attack");
        }

        //FlaskBarrel.transform.position, FlaskBarrel.transform.forward
        //if (Physics.Raycast(FlaskBarrel.transform.position, FlaskBarrel.transform.forward, out RaycastHit hit, 20))
        //{
        //    GameObject enemy = hit.transform.gameObject;
        //    if (enemy.CompareTag("Player"))
        //    {
        //        enemy.GetComponent<TurnBaseSystem>().TakeDamage(10);
        //        Debug.Log(enemy.GetComponent<TurnBaseSystem>().currentHealth.Value);
        //        if (IsLocalPlayer)
        //        {
        //            cardPanel.GetComponent<CardPanel>().hCard.Remove(ATKcard);
        //            cardPanel.GetComponent<CardPanel>().SetCardPos();
        //            Destroy(ATKcard.gameObject);
        //        }
        //        Debug.Log("Attack");
        //    }
        //    else
        //    {
        //        ATKcard = null;
        //    }

        //}
        Debug.Log(currentHealth.Value);
    }
    public void TakeDamage(float DamageAmount)
    {
        Debug.Log("TakeDamage");
        currentHealth.Value -= DamageAmount;
    }
    //[ServerRpc]
    //public void StartStateServerRpc()
    //{
    //    StartStateClientRpc();
    //}
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
    //[ClientRpc]
    public void StartState()
    {
        currentHealth = new NetworkVariable<float>(maxHealth);
        StartBut = GameObject.Find("StartGameBut").GetComponent<Button>();
        PlayerCanvas = GameObject.Find("PlayerCanvas");
        CombinePanel = GameObject.Find("CombineSystem");
        EndTurnButton = GameObject.Find("EndTurnButton").GetComponent<Button>();
        CombinePanel.SetActive(false);
        cardPanel = GameObject.Find("CardPanel").GetComponent<CardPanel>();
        PlayerState = GameState.Start;
        isYourTurn = false;
        EndTurnButton = GameObject.Find("EndTurnButton").GetComponent<Button>();
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
