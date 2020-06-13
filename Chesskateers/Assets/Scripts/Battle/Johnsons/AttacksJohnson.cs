// Name: AttacksJohnson.cs
// Purpose: Used to reference attack GameObjects
// Version: 1. 
// Date: 2020/6/8
// Author: Ryan Dixon
// Dependencies: None

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A "Johnson" is basically a cool little list of things which are all loaded at the beginning of the scene to be
// referenced when necessary
// This Johnson holds the different possible Card types, to be referenced when player decks are created
public class AttacksJohnson : MonoBehaviour
{
    public GameObject basic;
    public GameObject specialPawn;
    public GameObject specialRook;
    public GameObject specialKnight;
    public GameObject specialBishop;
    public GameObject specialQueen;
    public GameObject heavy;
    public GameObject queenHeal;
    public static List<GameObject> attackList;
    //Basic, Heavy, all Specials, and Queen's Healing
    // All of these GameObjects start empty but are filled in through Unity-side

    private void Start()
    {
        attackList = new List<GameObject> { basic, specialPawn, specialRook, specialKnight, specialBishop, specialQueen, heavy, queenHeal };
        //Puts all the GameObjects into a handy little list
    }
}
        /* Basic   [0]
         * Pawn    [1]
         * Rook    [2]
         * Knight  [3]
         * Bishop  [4]
         * Queen   [5]
         * Heavy   [6]
         * Queen Heal [7]
         */
