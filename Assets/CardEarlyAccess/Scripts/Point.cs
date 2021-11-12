using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public GameObject canvas;
    public bool isArrow;
    public Camera cam;
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        //canvas = GameObject.Find("PlayerCanvas");
        //transform.SetParent(canvas.transform);
        //transform.localScale = Vector3.one;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Direction = Input.mousePosition;

        this.gameObject.GetComponent<RectTransform>().eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, (Mathf.Atan2(Direction.y,Direction.x)*180/Mathf.PI)-90);
    }
}
