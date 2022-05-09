using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDic : MonoBehaviour
{
    [SerializeField]
    private DictionaryScriptableObject dictionaryData;

    [SerializeField]
    private List<string> keys = new List<string>();
    [SerializeField]
    private List<ElementCard> values = new List<ElementCard>();

    public Dictionary<string, ElementCard> E_CardDic = new Dictionary<string, ElementCard>();

    public bool modifyValues;

    private void Awake()
    {
        for (int i = 0; i < Mathf.Min(dictionaryData.Keys.Count, dictionaryData.Values.Count); i++)
        {
            E_CardDic.Add(dictionaryData.Keys[i], dictionaryData.Values[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        if (modifyValues == false)
        {
            keys.Clear();
            values.Clear();
            for (int i = 0; i < Mathf.Min(dictionaryData.Keys.Count, dictionaryData.Values.Count); i++)
            {
                keys.Add(dictionaryData.Keys[i]);
                values.Add(dictionaryData.Values[i]);
            }
        }
    }

    public void OnAfterDeserialize()
    {

    }

    public void DeserializeDictionary()
    {
        Debug.Log("DESERIALIZATION");
        E_CardDic = new Dictionary<string, ElementCard>();
        dictionaryData.Keys.Clear();
        dictionaryData.Values.Clear();
        for (int i = 0; i < Mathf.Min(keys.Count, values.Count); i++)
        {
            dictionaryData.Keys.Add(keys[i]);
            dictionaryData.Values.Add(values[i]);
            E_CardDic.Add(keys[i], values[i]);
        }
        modifyValues = false;
    }

    public void PrintDictionary()
    {
        foreach (var pair in E_CardDic)
        {
            Debug.Log("Key: " + pair.Key + " Value: " + pair.Value);
        }
    }


}

