using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public RectTransform playerInMap;
    public RectTransform map2dEnd;
    public Transform map3dParent;
    public Transform map3dEnd;
    public GameObject LocalPlayer;
    private Vector3 normalized, mapped;
    public GameObject ETmini;
    public GameObject MTmini;
    public GameObject PTmini;
    public TaskList TL;
    private void Start()
    {
        
    }
    private void Update()
    {
        if(LocalPlayer != null)
        {
            normalized = Divide(
                map3dParent.InverseTransformPoint(LocalPlayer.transform.position),
                map3dEnd.position - map3dParent.position
            );
            normalized.y = normalized.z;
            mapped = Multiply(normalized, map2dEnd.localPosition);
            mapped.z = 0;
            playerInMap.localPosition = mapped;
        }
    }

    private static Vector3 Divide(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
    }

    private static Vector3 Multiply(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }
    public void SpawnMiniIcon()
    {
        if(TL.allET.Length > 0)
        {
            foreach (ElectricTask et in TL.allET)
            {
                var icon = Instantiate(ETmini, this.transform);
                var norm = Divide(
                map3dParent.InverseTransformPoint(et.gameObject.transform.position),
                map3dEnd.position - map3dParent.position
                 );
                norm.y = norm.z;
                var map = Multiply(norm, map2dEnd.localPosition);
                map.z = 0;
                icon.GetComponent<RectTransform>().localPosition = map;
            }
        }
        if(TL.allMT.Length > 0)
        {
            foreach (MedicalTask mt in TL.allMT)
            {
                var icon = Instantiate(MTmini, this.transform);
                var norm = Divide(
                map3dParent.InverseTransformPoint(mt.gameObject.transform.position),
                map3dEnd.position - map3dParent.position
                 );
                norm.y = norm.z;
                var map = Multiply(norm, map2dEnd.localPosition);
                map.z = 0;
                icon.GetComponent<RectTransform>().localPosition = map;
            }
        }
        if (TL.allPT.Length > 0)
        {
            foreach (PickupTask pt in TL.allPT)
            {
                var icon = Instantiate(PTmini, this.transform);
                var norm = Divide(
                map3dParent.InverseTransformPoint(pt.gameObject.transform.position),
                map3dEnd.position - map3dParent.position
                 );
                norm.y = norm.z;
                var map = Multiply(norm, map2dEnd.localPosition);
                map.z = 0;
                icon.GetComponent<RectTransform>().localPosition = map;
            }
        }

    }
}