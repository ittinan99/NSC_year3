using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.UI;
using Unity.Netcode;
using System;
using UnityEngine.SceneManagement;

public class PhaseTimer : NetworkBehaviour
{
    NetworkVariable<float> currentTime = new NetworkVariable<float>(NetworkVariableReadPermission.Everyone);
    public float TaskStartingTime;
    public float CombineStartingTime;
    public float AttackStartingTime;
    [SerializeField]
    private float SpeedDivide;
    public TextMeshProUGUI TimerText;
    private GameSystem GS;
    public CardPanel CP;
    public GameObject ClockHand;
    public Image ClockAreaRemain;
    public Slider ClockSlider;
    float Angle = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        GS = GameObject.Find("GameSystem").GetComponent<GameSystem>();
    }
    void Start()
    {
        currentTime.OnValueChanged += ValueChange;
    }

    private void ValueChange(float previousValue, float newValue)
    {
        TimerText.text = newValue.ToString("F2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //[ServerRpc]
    //public void TimeServerRpc(ServerTime data)
    //{
    //    TimeClientRpc(data);
    //}
    //[ClientRpc]
    //public void TimeClientRpc(ServerTime data)
    //{
    //    TimerText.text = currentTime.ToString("F2");
    //}
    public void TaskCountDownMethod()
    {
        TaskCountDown();
    }
    private void ClockHandRotate(float maxtime)
    {
        Angle = 360 * (currentTime.Value / maxtime);
        ClockHand.transform.rotation = Quaternion.Euler(0, 0, Angle);
    }
    public async void TaskCountDown()
    {
        if (IsOwner)
        {
            currentTime.Value = TaskStartingTime;
        }
        ClockSlider.maxValue = TaskStartingTime;
        while (currentTime.Value > 0)
        {
            if (IsOwner)
            {
                //TimeServerRpc(new ServerTime { Servertime = currentTime.Value.ToString("F2") });
                currentTime.Value -= Time.deltaTime;
            }
            ClockSlider.value = currentTime.Value;
            Angle = 0;
            ClockHandRotate(TaskStartingTime);
            await Task.Yield();
        }
        GS.CombinePhaseServerRpc();
    }
    public async void CombineCountDownMethod()
    {
        if (IsOwner)
        {
            currentTime.Value = CombineStartingTime;
        }
        ClockSlider.maxValue = CombineStartingTime;
        while (currentTime.Value > 0)
        {
            if (IsOwner)
            {
                //TimeServerRpc(new ServerTime { Servertime = currentTime.ToString("F2") });
                currentTime.Value -= Time.deltaTime;
            }
            ClockSlider.value = currentTime.Value;
            Angle = 0;
            ClockHandRotate(CombineStartingTime);
            await Task.Yield();
        }
        GS.AttackPhaseServerRpc();
    }
    public async void AttackCountDownMethod()
    {
        if (IsOwner)
        {
            currentTime.Value = AttackStartingTime;
        }
        ClockSlider.maxValue = AttackStartingTime;
        while (currentTime.Value > 0)
        {
            if (IsOwner)
            {
                //TimeServerRpc(new ServerTime { Servertime = currentTime.ToString("F2") });
                currentTime.Value -= Time.deltaTime;
            }
            ClockSlider.value = currentTime.Value;
            Angle = 0;
            ClockHandRotate(AttackStartingTime);
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
        //GS.TaskPhaseServerRpc();
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
        StartCoroutine(WaitForHost());
        IEnumerator WaitForHost()
        {
            if(!IsServer)
            {
                if (GameSystem.localTurnbased.gameObject.GetComponent<TurnBaseSystem>().PlayerState == TurnBaseSystem.GameState.Lose)
                {
                    SceneManager.LoadScene("EndLoseScene");
                }
                else if (GameSystem.localTurnbased.gameObject.GetComponent<TurnBaseSystem>().PlayerState == TurnBaseSystem.GameState.Win)
                {
                    SceneManager.LoadScene("EndWinScene");
                }
            }
            yield return new WaitForSeconds(1);
            if (IsServer)
            {
                if (GameSystem.localTurnbased.gameObject.GetComponent<TurnBaseSystem>().PlayerState == TurnBaseSystem.GameState.Lose)
                {
                    SceneManager.LoadScene("EndLoseScene");
                }
                else if (GameSystem.localTurnbased.gameObject.GetComponent<TurnBaseSystem>().PlayerState == TurnBaseSystem.GameState.Win)
                {
                    SceneManager.LoadScene("EndWinScene");
                }
            }
        }
    }
}
