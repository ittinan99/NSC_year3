using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPanel : MonoBehaviour
{
    public List<ElementCardDisplay> hCard;
    public List<ElementCard> allEcard;
    public float Gap;
    public Vector2 PlacePosMin;
    public Vector2 PlacePosMax;
    [SerializeField]
    private GameObject E_cardPrefab;
    [SerializeField]
    private AmmoPanel AP;
    private void Awake()
    {
        hCard = new List<ElementCardDisplay>(GameObject.FindObjectsOfType<ElementCardDisplay>());
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGameCard()
    {
        SetCardPos();
        SpawnCard(5);
    }
    public void CheckEmptyAmmoCard()
    {
        List<GameObject> DeleteCard = new List<GameObject>();
        for (var i = hCard.Count - 1; i > -1; i--)
        {
            if (hCard[i].AmmoAmount == 0)
            {
                DeleteCard.Add(hCard[i].gameObject);
                hCard.Remove(hCard[i]);
            }
        }
        for (var i = DeleteCard.Count - 1; i > -1; i--)
        {
            Destroy(DeleteCard[i]);
        }
        SetCardPos();
    }
    public void SetCardPos()
    {
        Vector2 PosMin = PlacePosMin;
        Vector2 PosMax = PlacePosMax;
        Vector2 Dis = PlacePosMax - PlacePosMin;
        for (var i = hCard.Count - 1; i > -1; i--)
        {
            if (hCard[i] == null)
                hCard.RemoveAt(i);
        }
        hCard.RemoveAll(x => x == null);
        foreach (ElementCardDisplay card in hCard)
        {
            card.gameObject.GetComponent<RectTransform>().anchorMin = PosMin;
            card.gameObject.GetComponent<RectTransform>().anchorMax = PosMax;
            card.StartPos = card.transform.position;
            PosMin = new Vector2(PosMax.x + Gap, 0);
            PosMax = new Vector2(PosMin.x + Dis.x,1);
        }
    }
    public void SpawnCard(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            int RandNum = Random.Range(0, allEcard.Count - 1);
            var newCard = Instantiate(E_cardPrefab, this.transform);
            newCard.GetComponent<ElementCardDisplay>().E_Card = allEcard[RandNum];
            AddCard(newCard.GetComponent<ElementCardDisplay>());
        }
    }
    public void RemoveCard(ElementCardDisplay ECD)
    {
        List<GameObject> DeleteCard = new List<GameObject>();
        for (var i = hCard.Count - 1; i > -1; i--)
        {
            if (hCard[i] == ECD)
            {
                DeleteCard.Add(hCard[i].gameObject);
                hCard.Remove(hCard[i]);
                Debug.Log(i);
            }
        }
        for (var i = DeleteCard.Count - 1; i > -1; i--)
        {
            Destroy(DeleteCard[i]);
        }
        SetCardPos();
    }
    public void AddCard(ElementCardDisplay ECD)
    {   
        if(hCard.Count < 8)
        {
            hCard.Add(ECD);
            //hCard = new List<ElementCardDisplay>(GameObject.FindObjectsOfType<ElementCardDisplay>());
            for (var i = hCard.Count - 1; i > -1; i--)
            {
                if (hCard[i] == null)
                    hCard.RemoveAt(i);
            }
            hCard.RemoveAll(x => x == null);
            ECD.gameObject.transform.parent = this.gameObject.transform;
            Vector2 offsetmax = ECD.gameObject.GetComponent<RectTransform>().offsetMax;
            Vector2 offsetmin = ECD.gameObject.GetComponent<RectTransform>().offsetMin;
            ECD.gameObject.GetComponent<RectTransform>().position = Vector3.zero;
            ECD.gameObject.GetComponent<RectTransform>().offsetMax = Vector2.zero;
            ECD.gameObject.GetComponent<RectTransform>().offsetMin = Vector2.zero;

            Debug.Log(ECD.gameObject.GetComponent<RectTransform>().position);

            SetCardPos();
        }
    }
    
}
public static class RectTransformExtensions
{
    public static void SetDefaultScale(this RectTransform trans)
    {
        trans.localScale = new Vector3(1, 1, 1);
    }
    public static void SetPivotAndAnchors(this RectTransform trans, Vector2 aVec)
    {
        trans.pivot = aVec;
        trans.anchorMin = aVec;
        trans.anchorMax = aVec;
    }

    public static Vector2 GetSize(this RectTransform trans)
    {
        return trans.rect.size;
    }
    public static float GetWidth(this RectTransform trans)
    {
        return trans.rect.width;
    }
    public static float GetHeight(this RectTransform trans)
    {
        return trans.rect.height;
    }

    public static void SetPositionOfPivot(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x, newPos.y, trans.localPosition.z);
    }

    public static void SetLeftBottomPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }
    public static void SetLeftTopPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }
    public static void SetRightBottomPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }
    public static void SetRightTopPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }

    public static void SetSize(this RectTransform trans, Vector2 newSize)
    {
        Vector2 oldSize = trans.rect.size;
        Vector2 deltaSize = newSize - oldSize;
        trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
        trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
    }
    public static void SetWidth(this RectTransform trans, float newSize)
    {
        SetSize(trans, new Vector2(newSize, trans.rect.size.y));
    }
    public static void SetHeight(this RectTransform trans, float newSize)
    {
        SetSize(trans, new Vector2(trans.rect.size.x, newSize));
    }
}
