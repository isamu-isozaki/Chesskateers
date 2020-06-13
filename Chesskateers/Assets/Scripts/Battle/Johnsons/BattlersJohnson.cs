// Name: AttacksJohnson.cs
// Purpose: Used to reference battler GameObjects
// Version: 1. 
// Date: 2020/6/8
// Author: Ryan Dixon
// Dependencies: None

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A "Johnson" is basically a cool little list of things which are all loaded at the beginning of the scene to be
// referenced when necessary
// This Johnson holds the different possible Battler types, to be referenced when a player is first spawned.
public class BattlersJohnson : MonoBehaviour
{
    public GameObject whiteKing;
    public GameObject whiteQueen;
    public GameObject whiteRook;
    public GameObject whiteBishop;
    public GameObject whiteKnight;
    public GameObject whitePawn;
    public GameObject blackKing;
    public GameObject blackQueen;
    public GameObject blackRook;
    public GameObject blackBishop;
    public GameObject blackKnight;
    public GameObject blackPawn;
    public static List<GameObject> battlersList;
    //Objects for every piece, one of each for white and black
    // All of these GameObjects start empty but are filled in through Unity-side

    private void Start()
    {
        battlersList = new List<GameObject> { whiteKing, whiteQueen, whiteRook, whiteBishop, whiteKnight, whitePawn, blackKing, blackQueen, blackRook, blackBishop, blackKnight, blackPawn };
        //Puts all the GameObjects into a handy little list
    }
}      /* WKing   [0]
          WQueen  [1]
          WRook   [2]
          WBishop [3]
          WKnight [4]
          WPawn   [5]
          BKing   [6]
          BQueen  [7]
          BRook   [8]
          BBishop [9]
          BKnight [10]
          BPawn   [11]
          */
