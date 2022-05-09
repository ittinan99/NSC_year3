using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCombineSystem : MonoBehaviour
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
    private ElementCardDisplay C_Input1 = null;
    [SerializeField]
    private ElementCardDisplay C_Input2 = null;
    public GameObject outputCard;
    void Start()
    {
        Input1.GetComponent<CardEvent>().CardAddListener(GetInputCard1);
        Input2.GetComponent<CardEvent>().CardAddListener(GetInputCard2);
    }
    public void GetInputCard1(ElementCardDisplay C_Input)
    {
        C_Input1 = C_Input;
        if(C_Input != null)
        {
            CheckOutPut();
        }
        else
        {
            ResetOutput();
        }
       
    }
    public void GetInputCard2(ElementCardDisplay C_Input)
    {
        C_Input2 = C_Input;
        if (C_Input != null)
        {
            CheckOutPut();
        }
        else
        {
            ResetOutput();
        }
    }
    public void CheckOutPut()
    {
        if(C_Input1 != null && C_Input2 != null)
        {
            string Result = C_Input1.E_Card.element_Name + C_Input2.E_Card.element_Name;
            Debug.Log("Result Card : "+Result.Trim());
            foreach (Transform child in Output.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            if (CombineReceipe.E_CardDic.ContainsKey(Result))
            {
                outputCard = Instantiate(Cardprefab, Output.transform.position, Quaternion.identity);
                Debug.Log(outputCard);
                outputCard.GetComponent<ElementCardDisplay>().E_Card = CombineReceipe.E_CardDic[Result];
                outputCard.GetComponent<ElementCardDisplay>().IsOutPutCard = true;
                outputCard.transform.parent = Output.transform;
                outputCard.gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                outputCard.gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            }
           
        }
    }
    public void ConfirmCombine()
    {
        GameObject cardPanel = GameObject.Find("CardPanel");
        outputCard = null;
        cardPanel.GetComponent<CardPanel>().hCard.Remove(C_Input1);
        cardPanel.GetComponent<CardPanel>().hCard.Remove(C_Input2);
        Destroy(C_Input1.gameObject);
        Destroy(C_Input2.gameObject);
        C_Input1 = null;
        C_Input2 = null;
        Input1.GetComponent<CardEvent>().IsEmpty = true;
        Input2.GetComponent<CardEvent>().IsEmpty = true;
    }
    public void ResetOutput()
    {
        if(outputCard != null)
        {
            Destroy(outputCard.gameObject);
            outputCard = null;
        }
    }
    public void ResetRemainCard()
    {
        if(C_Input1 != null)
        {
            C_Input1.ResetFromCombine();
            C_Input1 = null;
            Input1.GetComponent<CardEvent>().IsEmpty = true;
        }
        if(C_Input2 != null)
        {
            C_Input2.ResetFromCombine();
            C_Input2 = null;
            Input2.GetComponent<CardEvent>().IsEmpty = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
       
    }
}
