// Name: ChesskateersGame.cs
// Purpose: Store global state of the game unaffected by level change
// Version: 1. 
// Date: 2020/6/8
// Author: Isamu Isozaki, Ryan Dixon
// Dependencies: Look at imports below
using UnityEngine;


public class ChesskateersGame
{
    public const string PLAYER_READY = "IsPlayerReady";
    public const string PLAYER_LOADED_LEVEL = "PlayerLoadedLevel";
    public static Chessman[] battlePieces;
    public static bool initialized = false;
    public static bool promotionPending = false;
    public static bool goingToChess = false;
    public static Chessman[,] Chessmans;
    public static Chessman selectedChessman;
    public static bool side;
    public static bool setSide = false;
    public static bool debug = false;

    public static Color GetColor(int colorChoice)
    {
        switch (colorChoice)
        {
            case 0: return Color.white;
            case 1: return Color.black;
        }

        return Color.gray;
    }
}