using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncyclopediaPageManager : MonoBehaviour
{
    public GameObject[] CardBar;
    public int PageCount = 0;
    public GameObject PopUp;
    void Start()
    {
        PageCount = 0;
        SetFalse();
        CardBar[PageCount].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnNextPage()
    {
        SetFalse();
        PageCount++;
        CardBar[Mathf.Abs(PageCount) % 3].SetActive(true);
    }
    public void OnBackPage()
    {
        SetFalse();
        PageCount--;
        if(PageCount < 0)
        {
            PageCount = 3;
        }
        CardBar[Mathf.Abs(PageCount) % 3].SetActive(true);
    }
    public void SetFalse()
    {
        foreach(GameObject Cardbar in CardBar)
        {
            Cardbar.SetActive(false);
        }
    }
}
