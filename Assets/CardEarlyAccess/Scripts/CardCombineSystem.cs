using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCombineSystem : MonoBehaviour
{
    public GameObject card_Input1;
    public GameObject card_Input2;
    public GameObject outputCard;
    [SerializeField]
    private GameObject Cardprefab;
    [SerializeField]
    private CardDic CombineReceipe;
    [SerializeField]
    private CardDic eCardData;
    [SerializeField]
    private ElementCardDisplay slot_Input1 = null;
    [SerializeField]
    private ElementCardDisplay slot_Input2 = null;
    public GameObject slot_Output;
    void Start()
    {
        card_Input1.GetComponent<CardEvent>().CardAddListener(GetInputCard1);
        card_Input2.GetComponent<CardEvent>().CardAddListener(GetInputCard2);
    }
    public void GetInputCard1(ElementCardDisplay card_Input)
    {
        card_Input1 = card_Input;
        if(card_Input != null)
        {
            ClearOutputCard();
            CheckandSpawn_OutPutCard();
        }
        else
        {
            ResetOutputCard();
        }
       
    }
    public void GetInputCard2(ElementCardDisplay card_Input)
    {
        card_Input2 = card_Input;
        if (card_Input != null)
        {
            ClearOutputCard();
            CheckandSpawn_OutPutCard();
        }
        else
        {
            ResetOutputCard();
        }
    }
    
    public void CheckandSpawn_OutPutCard()
    {
        if(card_Input1 == null || card_Input2 == null) { return; }
        string Result = card_Input1.E_Card.element_Name + card_Input2.E_Card.element_Name;
        if (CombineReceipe.E_CardDic.ContainsKey(Result))
        {
            SpawnCard_FromCardDic(Result);
        }
    }
    public void SpawnCard_FromCardDic(string Result)
    {
        outputCard = Instantiate(Cardprefab, slot_Output.transform.position, Quaternion.identity);
        outputCard.GetComponent<ElementCardDisplay>().E_Card = CombineReceipe.E_CardDic[Result];
        outputCard.GetComponent<ElementCardDisplay>().IsOutPutCard = true;
        outputCard.transform.parent = slot_Output.transform;
        outputCard.gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        outputCard.gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
    }
    public void ConfirmCombine()
    {
        GameObject cardPanel = GameObject.Find("CardPanel");
        outputCard = null;
        ClearSlot_Input();
    }
    public void ResetOutputCard()
    {
        if(outputCard != null)
        {
            Destroy(outputCard.gameObject);
            outputCard = null;
        }
    }
    public void ResetRemainCard()
    {
        if(card_Input1 != null)
        {
            card_Input1.ResetFromCombine();
            card_Input1 = null;
            slot_Input1.GetComponent<CardEvent>().IsEmpty = true;
        }
        if(C_Input2 != null)
        {
            card_Input2.ResetFromCombine();
            card_Input2 = null;
            slot_Input2.GetComponent<CardEvent>().IsEmpty = true;
        }
    }
    public void ClearSlot_Output()
    {
        foreach (Transform child in slot_Output.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    public void ClearSlot_Input()
    {
        cardPanel.GetComponent<CardPanel>().hCard.Remove(card_Input1);
        cardPanel.GetComponent<CardPanel>().hCard.Remove(card_Input2);
        Destroy(card_Input1.gameObject);
        Destroy(card_Input2.gameObject);
        card_Input1 = null;
        card_Input2 = null;
        slot_Input1.GetComponent<CardEvent>().IsEmpty = true;
        slot_Input2.GetComponent<CardEvent>().IsEmpty = true;
    }
    // Update is called once per frame
    void Update()
    {
       
    }
}
