using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskList : MonoBehaviour
{
    public int WireFixCount;
    public int MedicalTaskCount;
    public int PickupCount;
    public ElectricTask[] allET;
    public MedicalTask[] allMT;
    public PickupTask[] allPT;
    public TextMeshProUGUI WireText;
    public TextMeshProUGUI MedicalText;
    public TextMeshProUGUI PickupText;
    // Start is called before the first frame update
    void Start()
    {
        WireFixCount = 0;
        MedicalTaskCount = 0;
        PickupCount = 0;
        allET = GameObject.FindObjectsOfType<ElectricTask>();
        allMT = GameObject.FindObjectsOfType<MedicalTask>();
        allPT = GameObject.FindObjectsOfType<PickupTask>();
        resetTask();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void resetTask()
    {
        allET = GameObject.FindObjectsOfType<ElectricTask>();
        allMT = GameObject.FindObjectsOfType<MedicalTask>();
        allPT = GameObject.FindObjectsOfType<PickupTask>();
        foreach(ElectricTask ET in allET)
        {
            ET.TaskComp = false;
        }
        foreach(MedicalTask MT in allMT)
        {
            MT.TaskComp = false;
        }
        foreach(PickupTask PT in allPT)
        {
            PT.TaskComp = false;
        }
        WireText.text = $"Electric Wire Fix : 0/{allET.Length}";
        MedicalText.text = $"Medical Task : 0/{allMT.Length}";
        PickupText.text = $"Grenade Task : 0/{allPT.Length}";
    }
    public void WireTaskComp()
    {
        WireFixCount++;
        WireText.text = $"Electric Wire Fix : {WireFixCount}/{allET.Length}";
    }
    public void MedicalTaskComp()
    {
        MedicalTaskCount++;
        MedicalText.text = $"Medical Task : {MedicalTaskCount}/{allMT.Length}";
    }
    public void PickupTaskComp()
    {
        PickupCount++;
        PickupText.text = $"Grenade Task : {PickupCount}/{allPT.Length}";
    }
}
