using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.UI;
public class PhaseTimer : MonoBehaviour
{
    float currentTime = 0;
    public float TaskStartingTime;
    public float CombineStartingTime;
    public float AttackStartingTime;
    public TextMeshProUGUI TimerText;
    private GameSystem GS;
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
    public void TaskCountDownMethod()
    {
        TaskCountDown();
    }
    public async void TaskCountDown()
    {
        currentTime = TaskStartingTime;
        while(currentTime > 0)
        {
            TimerText.text = currentTime.ToString("F2");
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
            TimerText.text = currentTime.ToString("F2");
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
            TimerText.text = currentTime.ToString("F2");
            currentTime -= Time.deltaTime;
            await Task.Yield();
        }
        GS.AttackPhaseServerRpc();
    }
}
