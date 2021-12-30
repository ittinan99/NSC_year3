using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;
using Netcode.Transports.PhotonRealtime;
public class TurnBaseSystem : NetworkBehaviour
{
    public enum GameState { Start,Win,Lose}
    public GameState PlayerState;
    [SerializeField]
    private GameSystem GS;
    [SerializeField]
    public GameObject PlayerCanvas;
    [SerializeField]
    public GameObject CombinePanel;
    [SerializeField]
    public CardPanel cardPanel;
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
        Debug.Log("K");
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
            currentHealth = new NetworkVariable<float>(maxHealth);
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
            PlayerCanvas.SetActive(true);
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
        Debug.Log(currentHealth.Value);
    }
    public void TakeDamage(float DamageAmount)
    {
        Debug.Log("TakeDamage");
        currentHealth.Value -= DamageAmount;
    }
    public void HideShowPanel()
    {
        CombinePanel.SetActive(!CombinePanel.activeInHierarchy);
        cardPanel.gameObject.SetActive(!cardPanel.gameObject.activeInHierarchy);
    }
    public void HideCardPanel()
    {
        cardPanel.gameObject.SetActive(false);
    }
    //[ServerRpc]
    //public void StartStateServerRpc()
    //{
    //    StartStateClientRpc();
    //}
    //[ServerRpc]
    //public void Y_CombineStateServerRpc()
    //{
    //    Y_CombineStateClientRpc();
    //}
    //public void EndTurn()
    //{
    //    if (IsLocalPlayer)
    //    {
    //        EndTurnServerRpc();
    //    }
    //}
    //[ServerRpc]
    //public void EndTurnServerRpc()
    //{
    //    EndTurnClientRpc();
    //    GS.NextPlayerTurnServerRpc();
    //}
    //[ClientRpc]
    public void StartState()
    {
        currentHealth = new NetworkVariable<float>(maxHealth);
        StartBut = GameObject.Find("StartGameBut").GetComponent<Button>();
        PlayerCanvas = GameObject.Find("PlayerCanvas");
        CombinePanel = GameObject.Find("CombineSystem");
        CombinePanel.SetActive(false);
        cardPanel = GameObject.Find("CardPanel").GetComponent<CardPanel>();
        PlayerState = GameState.Start;
    }
    //[ClientRpc]
    //public void Y_CombineStateClientRpc()
    //{
    //    PlayerState = GameState.Y_CombineTurn;
    //}
    //[ClientRpc]
    //public void EndTurnClientRpc()
    //{
    //    PlayerState = GameState.otherTurn;
    //    isYourTurn = false;
    //}

}
