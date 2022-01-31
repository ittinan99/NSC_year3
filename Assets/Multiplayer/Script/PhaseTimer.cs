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
    NetworkVariable<float> ClockValue = new NetworkVariable<float>(NetworkVariableReadPermission.Everyone);
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

    public AudioClip TaskPhaseMusic;
    public AudioClip CombPhaseMusic;
    public AudioClip ATKPhaseMusic;
    [SerializeField]
    private AudioSource audioS;
    float Angle = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        GS = GameObject.Find("GameSystem").GetComponent<GameSystem>();
    }
    void Start()
    {
        currentTime.OnValueChanged += ValueChange;
        ClockValue.OnValueChanged += clockchange;
        currentTime.Value = 0;
    }
    private void ValueChange(float previousValue, float newValue)
    {
        TimerText.text = newValue.ToString("F2");
    }
    private void clockchange(float previousValue, float newValue)
    {
        ClockSlider.value = newValue;
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
        audioS.clip = TaskPhaseMusic;
        audioS.Play();
        if (IsOwner)
        {
            currentTime.Value = TaskStartingTime;
        }
        ClockSlider.maxValue = TaskStartingTime;
        ClockAreaRemain.color = Color.yellow;
        while (currentTime.Value > 0)
        {
            if (IsOwner)
            {
                //TimeServerRpc(new ServerTime { Servertime = currentTime.Value.ToString("F2") });
                currentTime.Value -= Time.deltaTime;
                ClockValue.Value = currentTime.Value;
            }
            Angle = 0;
            ClockHandRotate(TaskStartingTime);
            await Task.Yield();
        }
        audioS.Stop();
        GS.CombinePhaseServerRpc();
    }
    public async void CombineCountDownMethod()
    {
        audioS.clip = CombPhaseMusic;
        audioS.Play();
        if (IsOwner)
        {
            currentTime.Value = CombineStartingTime;
        }
        ClockSlider.maxValue = CombineStartingTime;
        ClockAreaRemain.color = Color.green;
        while (currentTime.Value > 0)
        {
            if (IsOwner)
            {
                //TimeServerRpc(new ServerTime { Servertime = currentTime.ToString("F2") });
                currentTime.Value -= Time.deltaTime;
                ClockValue.Value = currentTime.Value;
            }
            Angle = 0;
            ClockHandRotate(CombineStartingTime);
            await Task.Yield();
        }
        audioS.Stop();
        GS.AttackPhaseServerRpc();
    }
    public async void AttackCountDownMethod()
    {
        audioS.clip = ATKPhaseMusic;
        audioS.Play();
        if (IsOwner)
        {
            currentTime.Value = AttackStartingTime;
        }
        ClockSlider.maxValue = AttackStartingTime;
        ClockAreaRemain.color = Color.red;
        while (currentTime.Value > 0)
        {
            if (IsOwner)
            {
                //TimeServerRpc(new ServerTime { Servertime = currentTime.ToString("F2") });
                currentTime.Value -= Time.deltaTime;
                ClockValue.Value = currentTime.Value;
            }
            Angle = 0;
            ClockHandRotate(AttackStartingTime);
            await Task.Yield();
        }
        audioS.Stop();
        Cursor.visible = true;
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
