using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dictionary Storage", menuName = "Data Objects/Dictionary Storage Object")]
public class DictionaryScriptableObject : ScriptableObject
{
    [SerializeField]
    List<string> keys = new List<string>();
    [SerializeField]
    List<ElementCard> values = new List<ElementCard>();

    public List<string> Keys { get => keys; set => keys = value; }
    public List<ElementCard> Values { get => values; set => values = value; }
}
