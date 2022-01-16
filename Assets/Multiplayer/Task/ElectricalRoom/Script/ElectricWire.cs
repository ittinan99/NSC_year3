using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ElectricWire : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    [SerializeField]
    private ElectricTask ET;
    public GameObject StartPoint;
    public float Calibrate;
    public bool IsDrag;
    public GameObject WireEndName;
    public bool Correct;

    void Start()
    {
        IsDrag = false;
        Correct = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDrag)
        {
            Vector3 MousePos = Input.mousePosition;
            float Y = MousePos.y - StartPoint.GetComponent<RectTransform>().position.y;
            float X = MousePos.x - StartPoint.GetComponent<RectTransform>().position.x;
            float RotateZ = Mathf.Atan2(Y, X);
            StartPoint.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * RotateZ);
            float Xscale = Vector3.Distance(StartPoint.GetComponent<RectTransform>().position, MousePos);
            StartPoint.GetComponent<RectTransform>().localScale = new Vector3(Xscale / Calibrate, 1, 1);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.Mouse0) && !Correct)
        {
            IsDrag = true;
            ET.currentEW = this;
        }
    }
    public void CheckWireEnd(GameObject WireEnd)
    {
        if (ET.currentEW == this && WireEnd == WireEndName)
        {
            Debug.Log(WireEnd.name);
            Debug.Log("Correct");
            Correct = true;
            ET.CorrectWireAdd();
        }
        else if(ET.currentEW == this && WireEnd != WireEndName)
        {
            Debug.Log(WireEnd.name);
            Debug.Log("false");
            Correct = false;
        }
    }
    public void ExitWireEnd(GameObject WireEnd)
    {
        if (ET.currentEW == this && WireEnd == WireEndName)
        {
            Correct = false;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (ET.currentEW == this)
        {
            if (!Correct)
            {
                IsDrag = false;
                StartPoint.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 0);
                StartPoint.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                ET.currentEW = null;
            }
            else if (Correct)
            {
                IsDrag = false;
                ET.currentEW = null;
            }
        }
        else
        {
            IsDrag = false;
            ET.currentEW = null;
        }
    }
}
