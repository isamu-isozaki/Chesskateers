using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animstantiation : MonoBehaviour //This tells the game which prop to instantiate 
{
    public GameObject Prefab = null;
    Vector3 myvec;
    Quaternion myquat;
    public SpecialCard cardsidefinder;

    public Animstantiation(GameObject prefab)
    {
        Prefab = prefab;
    }

    private void Start()
    {
        myvec = new Vector3(5.380005f, 237.26f, -1.3f);
        myquat = Quaternion.Euler(-90, 0, 45);
}
     
    public void InstantiateObject() // Used :)
    {
        Instantiate(Prefab, myvec, myquat);
    }
    public void ColorChoice() // Not used 
    {
        if (cardsidefinder.side)
        {
            // We were on the verge of a breakthrough, but alas
            // This is not a future plan, this is a past mistake
        }

    }
}
