using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void OnClickStartGame()
    {
        SceneManager.LoadScene("Mainmenu2", LoadSceneMode.Single);
    }
    public void OnClickEncyclopedia()
    {
        SceneManager.LoadScene("Encyclopedia", LoadSceneMode.Single);
    }
}
