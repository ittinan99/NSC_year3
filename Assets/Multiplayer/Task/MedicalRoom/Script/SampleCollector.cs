using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleCollector : MonoBehaviour
{
    public List<GameObject> Beakers;
    public List<GameObject> Samples;
    public MedicalTask MT;
    public int j;
    void Start()
    {
        j = 0;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SwapGameObject(List<GameObject> samples,int indexA,int indexB)
    {
        GameObject tmp = samples[indexA];
        Vector3 A = samples[indexA].GetComponent<RectTransform>().position;
        samples[indexA].GetComponent<RectTransform>().position = samples[indexB].GetComponent<RectTransform>().position;
        samples[indexB].GetComponent<RectTransform>().position = A;
        samples[indexA] = samples[indexB];
        samples[indexB] = tmp;
        CheckCorrectOrder();
    }
    public void RandomSwap()
    {
        for(int i = 0; i <= 7; i++)
        {
            int rand = Random.Range(1, 5);
            if(rand == 1)
            {
                SwapOneTwo();
            }
            else if (rand == 2)
            {
                SwapOneThree();
            }
            else if (rand == 3)
            {
                SwapTwoFour();
            }
            else if (rand == 4)
            {
                SwapThreeFive();
            }
            else if (rand == 5)
            {
                SwapFourFive();
            }
        }
    }
    public void SwapOneTwo()
    {
        SwapGameObject(Samples, 0, 1);
    }
    public void SwapOneThree()
    {
        SwapGameObject(Samples, 0, 2);
    }
    public void SwapTwoFour()
    {
        SwapGameObject(Samples, 1, 3);
    }
    public void SwapThreeFive()
    {
        SwapGameObject(Samples, 2, 4);
    }
    public void SwapFourFive()
    {
        SwapGameObject(Samples, 3, 4);
        CheckCorrectOrder();
    }
    public void CheckCorrectOrder()
    {
        j = 0;
        for(int i = 0; i <= Beakers.Count - 1; i++)
        {
            if (Samples[i].GetComponent<SampleKey>().Key == Beakers[i])
            {
                j++;
                Debug.Log($"Sample {i} : Correct !!");
            }
        }
        if(j == 5)
        {
            Debug.Log("AllCorrect");
            MT.CompleteTask();
        }
    }
}
