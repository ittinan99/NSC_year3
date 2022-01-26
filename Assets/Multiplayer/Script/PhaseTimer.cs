using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.UI;
using Unity.Netcode;
public class PhaseTimer : NetworkBehaviour
{
    float currentTime = 0;
    public float TaskStartingTime;
    public float CombineStartingTime;
    public float AttackStartingTime;
    [SerializeField]
    private float SpeedDivide;
    public TextMeshProUGUI TimerText;
    private GameSystem GS;
    public CardPanel CP;
    // Start is called before the first frame update
    private void Awake()
    {
        GS = GameObject.Find("GameSystem").GetComponent<GameSystem>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [ServerRpc]
    public void TimeServerRpc(ServerTime data)
    {
        TimeClientRpc(data);
    }
    [ClientRpc]
    public void TimeClientRpc(ServerTime data)
    {
        TimerText.text = currentTime.ToString("F2");
    }
    public void TaskCountDownMethod()
    {
        TaskCountDown();
    }
    public async void TaskCountDown()
    {
        currentTime = TaskStartingTime;
        while(currentTime > 0)
        {
            if (IsOwnedByServer)
            {
                TimeServerRpc(new ServerTime { Servertime = currentTime.ToString("F2") });
            }
            currentTime -= Time.deltaTime;
            await Task.Yield();
        }
        GS.CombinePhaseServerRpc();
    }
    public async void CombineCountDownMethod()
    {
        currentTime = CombineStartingTime;
        while (currentTime > 0)
        {
            if (IsOwnedByServer)
            {
                TimeServerRpc(new ServerTime { Servertime = currentTime.ToString("F2") });
            }
            currentTime -= Time.deltaTime;
            await Task.Yield();
        }
        GS.AttackPhaseServerRpc();
    }
    public async void AttackCountDownMethod()
    {
        currentTime = AttackStartingTime;
        while (currentTime > 0)
        {
            if (IsOwnedByServer)
            {
                TimeServerRpc(new ServerTime { Servertime = currentTime.ToString("F2") });
            }
            currentTime -= Time.deltaTime;
            await Task.Yield();
        }
        CheckBattleResult();

    }
    public void CheckBattleResult()
    {
        int ProteinAlive = 0;
        int CarboAlive = 0;
        foreach (GameObject Player in GS.ProteinList)
        {
            if (Player.GetComponent<TurnBaseSystem>().die == false)
            {
                ProteinAlive++;
            }
        }
        foreach (GameObject Player in GS.CarboList)
        {
            if (Player.GetComponent<TurnBaseSystem>().die == false)
            {
                CarboAlive++;
            }
        }
        if (CarboAlive == 0)
        {
            foreach (GameObject Player in GS.CarboList)
            {
                Player.GetComponent<TurnBaseSystem>().PlayerState = TurnBaseSystem.GameState.Lose;
                Debug.Log("Carbo Lose");
            }
            foreach (GameObject Player in GS.ProteinList)
            {
                Player.GetComponent<TurnBaseSystem>().PlayerState = TurnBaseSystem.GameState.Win;
                Debug.Log("Protein Win");
            }
        }
        else if (ProteinAlive == 0)
        {
            foreach (GameObject Player in GS.ProteinList)
            {
                Player.GetComponent<TurnBaseSystem>().PlayerState = TurnBaseSystem.GameState.Lose;
                Debug.Log("Protein Lose");
            }
            foreach (GameObject Player in GS.CarboList)
            {
                Player.GetComponent<TurnBaseSystem>().PlayerState = TurnBaseSystem.GameState.Win;
                Debug.Log("Carbo Win");
            }
        }
        else
        {
            GS.TaskPhaseServerRpc();
        }
    }
}
