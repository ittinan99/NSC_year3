using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineSystem : MonoBehaviour
{
    public GameObject Input1;
    public GameObject Input2;
    public GameObject Output;
    [SerializeField]
    private GameObject Cardprefab;
    [SerializeField]
    private CardDic CombineReceipe;
    [SerializeField]
    private CardDic eCardData;
    [SerializeField]
    private ElementCard C_Input1 = null;
    [SerializeField]
    private ElementCard C_Input2 = null;
    void Start()
    {
        Input1.GetComponent<CardEvent>().CardAddListener(GetInputCard1);
        Input2.GetComponent<CardEvent>().CardAddListener(GetInputCard2);
    }
    public void GetInputCard1(ElementCard C_Input)
    {
        C_Input1 = C_Input;
        CheckOutPut();
    }
    public void GetInputCard2(ElementCard C_Input)
    {
        C_Input2 = C_Input;
        CheckOutPut();
    }
    public void CheckOutPut()
    {
        if(C_Input1 != null && C_Input2 != null)
        {
            string Result = C_Input1.element_Name + C_Input2.element_Name;
            Debug.Log("Result Card : "+Result);
            foreach (Transform child in Output.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            GameObject outputCard = Instantiate(Cardprefab, Output.transform);
            outputCard.GetComponent<ElementCardDisplay>().E_Card = CombineReceipe.E_CardDic[Result];
            outputCard.transform.parent = Output.transform;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
