using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
public class Button_Script : MonoBehaviour
{   
    Scene wantScene;
    public string wantToGoSceneName;

    public UnityEvent wantToPlayThis;

    void Start()
    {
        if(wantToGoSceneName != null)
        {
            wantScene = SceneManager.GetSceneByName(wantToGoSceneName);
        }       
    }
}
