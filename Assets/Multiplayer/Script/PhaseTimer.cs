using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PhaseTimer : MonoBehaviour
{
    float currentTime = 0;
    public float StartingTime;
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
        StartCoroutine(TaskCountDown());
    }
    IEnumerator TaskCountDown()
    {
        currentTime = StartingTime;
        while(currentTime > 0)
        {
            TimerText.text = currentTime.ToString("F2");
            currentTime -= Time.deltaTime;
            yield return null;
        }
        GS.CombinePhaseServerRpc();
    }
}
