using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject[] Popup;

    public void OnClickHTP() //How to play button
    {
        Popup[0].SetActive(true);
        Popup[1].SetActive(false);
        Popup[2].SetActive(false);
        Popup[3].SetActive(false);

    }
    public void OnClickFM() //Find Match Button
    {
        Popup[0].SetActive(false);
        Popup[1].SetActive(true);
        Popup[2].SetActive(false);
        Popup[3].SetActive(false);
    }
    public void OnClickCM() //Create Match Button
    {
        Popup[0].SetActive(false);
        Popup[1].SetActive(false);
        Popup[2].SetActive(true);
        Popup[3].SetActive(false);
    }
    public void OnClickJM() //Join Match Button
    {
        Popup[0].SetActive(false);
        Popup[1].SetActive(false);
        Popup[2].SetActive(false);
        Popup[3].SetActive(true);
    }
    public void OnClickCancel() //Join Match Button
    {
        Popup[0].SetActive(false);
        Popup[1].SetActive(false);
        Popup[2].SetActive(false);
        Popup[3].SetActive(false);
    }
    public void OnClickRTMM() //Return to Mainmenu Button
    {
        SceneManager.LoadScene("Mainmenu",LoadSceneMode.Single);
    }

}
