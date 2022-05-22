using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TestVFX : MonoBehaviour
{
    public VisualEffect[] visualEffect;
    public void OnPlayVFX()
    {
        visualEffect[0].Play();
        visualEffect[1].Play();
        visualEffect[2].Play();
    }
}
