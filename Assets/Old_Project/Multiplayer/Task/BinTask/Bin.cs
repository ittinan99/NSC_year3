using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bin : MonoBehaviour
{
    // Start is called before the first frame update
    public CardPanel CP;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ElementCardDisplay>())
        {
            CP.RemoveCard(collision.GetComponent<ElementCardDisplay>());
        }
    }
}
