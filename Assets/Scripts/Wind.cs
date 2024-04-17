using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Wind : MonoBehaviour
{
    public VisualEffect wind;
    void Start()
    {
        wind = GetComponent<VisualEffect>();
        wind.Play();
    }
}
