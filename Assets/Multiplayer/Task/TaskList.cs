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
    public string ETinfo;
    public string MTinfo;
    public string PTinfo;
    public Minimap Mm;
    // Start is called before the first frame update
    void Start()
    {
        WireFixCount = 0;
        MedicalTaskCount = 0;
        PickupCount = 0;
        allET = GameObject.FindObjectsOfType<ElectricTask>();
        allMT = GameObject.FindObjectsOfType<MedicalTask>();
        allPT = GameObject.FindObjectsOfType<PickupTask>();
        Mm = GameObject.FindObjectOfType<Minimap>();
        resetTask();
        Mm.SpawnMiniIcon();
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
        WireText.text = $"{ETinfo} : 0/{allET.Length}";
        MedicalText.text = $"{MTinfo} : 0/{allMT.Length}";
        PickupText.text = $"{PTinfo} : 0/{allPT.Length}";
    }
    public void HideShowTask()
    {
        foreach (ElectricTask ET in allET)
        {
            ET.Task.SetActive(false);
        }
        foreach (MedicalTask MT in allMT)
        {
            MT.Task.SetActive(false);
        }
        foreach (PickupTask PT in allPT)
        {
            PT.DestroyAllSpawn();
        }
    }
    public void WireTaskComp()
    {
        WireFixCount++;
        WireText.text = $"{ETinfo} : {WireFixCount}/{allET.Length}";
    }
    public void MedicalTaskComp()
    {
        MedicalTaskCount++;
        MedicalText.text = $"{MTinfo} : {MedicalTaskCount}/{allMT.Length}";
    }
    public void PickupTaskComp()
    {
        PickupCount++;
        PickupText.text = $"{PTinfo} : {PickupCount}/{allPT.Length}";
    }
}
