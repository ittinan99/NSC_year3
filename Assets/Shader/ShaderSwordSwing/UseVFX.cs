using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class UseVFX : MonoBehaviour
{
    public VisualEffect[] VisualEffects;

    public void OnUseVFX(int VFXNumber)
    {
        VisualEffects[VFXNumber].Play();
    }
}
