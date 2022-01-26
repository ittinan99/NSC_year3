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
    public List<GameObject> ProteinList;
    public List<GameObject> CarboList;

    public enum GamePhase {Start,TaskState,CombineState,AttackState,End}
    [SerializeField]
    public static GamePhase gamePhase;
    public PhaseTimer PT;
    public AmmoPanel AP;
    public CardPanel CP;
    public CardDic CD;
    public Minimap MP;
    public TaskList TL;
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
                MP.LocalPlayer = player;
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
        PlayerList = GameObject.FindGameObjectsWithTag("Player");
        StartGameClientRpc();
        RandomRole();
    }
    
    [ClientRpc]
    public void StartGameClientRpc()
    {
        Debug.Log("GameStart");
        gamePhase = GamePhase.Start;
        gamePhase = GamePhase.TaskState;
        CD.DeserializeDictionary();
        PT.TaskCountDownMethod();
        PlayerList = GameObject.FindGameObjectsWithTag("Player");
        Gather_startPlayerClient();
    }
    public void RandomRole()
    {
        List<int> index = new List<int>();
        int count = 0;
        int half = PlayerList.Length / 2;
        if (half == 0) { half = 1; }
        while (count != half)
        {
            int Rand = Random.Range(0,PlayerList.Length-1);
            if (!index.Contains(Rand))
            {
                index.Add(Rand);
                count++;
            }
        }
        for (int i = 0; i < index.Count; i++)
        {
            RandomRoleClientRpc(index[i]);
        }
    }
    [ClientRpc]
    public void RandomRoleClientRpc(int i)
    {
        ProteinList = new List<GameObject>();
        CarboList = new List<GameObject>();
        Debug.Log($"{PlayerList[i].gameObject.name} is Protein");
        PlayerList[i].GetComponent<TurnBaseSystem>().PlayerRole = TurnBaseSystem.Role.Protein;
        foreach (GameObject player in PlayerList)
        {
            if (player.GetComponent<TurnBaseSystem>().PlayerRole == TurnBaseSystem.Role.Protein)
            {
                ProteinList.Add(player);
            }
            else
            {
                CarboList.Add(player);
            }
        }
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
        PT.CombineCountDownMethod();
    }

    [ServerRpc]
    public void TaskPhaseServerRpc()
    {
        TaskPhaseClientRpc();
    }
    [ClientRpc]
    public void TaskPhaseClientRpc()
    {
        Debug.Log("TaskPhase");
        gamePhase = GamePhase.TaskState;
        TL.resetTask();
        PT.TaskCountDownMethod();
        CP.CheckEmptyAmmoCard();
        AP.AddAmmoCard();
    }

    [ServerRpc]
    public void AttackPhaseServerRpc()
    {
        AttackPhaseClientRpc();
    }
    [ClientRpc]
    public void AttackPhaseClientRpc()
    {
        Debug.Log("AttackPhase");
        gamePhase = GamePhase.AttackState;
        AP.AddAmmoCard();
        localTurnbased.HideShowPanel();
        PT.AttackCountDownMethod();
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
