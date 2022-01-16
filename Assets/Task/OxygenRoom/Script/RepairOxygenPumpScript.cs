using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepairOxygenPumpScript : MonoBehaviour
{
    public GameObject Gear;
    public Slider GaugeSlider;
    public Canvas canvas;
    public float Speed;
    public float GaugeSpeed;
    float Angle;
    float FirstPosition;
    float SecondPosition;
    float CanvasHeightSize;
    [SerializeField]
    private CardPanel CP;
    private void Awake()
    {
        CP = GameObject.Find("CardPanel").GetComponent<CardPanel>();
    }
    void Start()
    {
        GaugeSlider.value = GaugeSlider.minValue;
        CanvasHeightSize = canvas.pixelRect.height;
    }
    void Update()
    {
        if (GaugeSlider.value == GaugeSlider.maxValue)
        {
            GaugeSlider.value = GaugeSlider.minValue;
        }
        if (Input.GetMouseButtonDown(0))
        {
            FirstPosition = Input.mousePosition.y;
        }
        if (Input.GetMouseButton(0))
        {
            //Debug.Log(Input.mousePosition.y);
            if (Input.mousePosition.y < CanvasHeightSize && Input.mousePosition.y > 0)
            {
                SecondPosition = Input.mousePosition.y;
                Angle = (FirstPosition - SecondPosition) * Speed;
                if (FirstPosition != SecondPosition)
                {
                    Gear.transform.Rotate(0, 0, -Angle);
                    if (Gear.transform.eulerAngles.z >= 300 && Gear.transform.eulerAngles.z <= 360)
                    {
                        GaugeSlider.value += GaugeSpeed;
                    }
                    else if (Gear.transform.eulerAngles.z >= 0 && Gear.transform.eulerAngles.z <= 60)
                    {
                        GaugeSlider.value += GaugeSpeed;
                    }
                    else
                    {
                        GaugeSlider.value -= GaugeSpeed / 10;
                    }
                    if (GaugeSlider.value == GaugeSlider.maxValue)
                    {
                        CP.SpawnCard(1);
                        canvas.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
