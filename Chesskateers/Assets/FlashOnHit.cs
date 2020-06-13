using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FlashOnHit : MonoBehaviour
{
    public float flashTime;
    public Renderer rend;
    public Material mat;
    public Material newmat;
    void Start()
    {
        mat = rend.material;
    }
    public void FlashRed()
    {
        rend.material = newmat;
        Invoke("ResetColor", flashTime);
        ResetColor();
    }
    void ResetColor()
    {
        rend.material = mat;
    }
}