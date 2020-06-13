// Name: AttacksJohnson.cs
// Purpose: Used to reference prop GameObjects
// Version: 1. 
// Date: 2020/6/8
// Author: Ryan Dixon
// Dependencies: None

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A "Johnson" is basically a cool little list of things which are all loaded at the beginning of the scene to be
// referenced when necessary
// This Johnson holds the different possible Props, which are called during certain special attack animations
public class PropsJohnson : MonoBehaviour
{
    public GameObject whiteRookWall;
    public GameObject blackRookWall;
    public static List<GameObject> propsList;
    //Just both types of wall for each of the Rooks
    // All of these GameObjects start empty but are filled in through Unity-side

    private void Start()
    {
        propsList = new List<GameObject> { whiteRookWall, blackRookWall };
        //Puts all the GameObjects into a handy little list
    }
}      /* WRookWall   [0]
          BRookWall   [1]
          */
