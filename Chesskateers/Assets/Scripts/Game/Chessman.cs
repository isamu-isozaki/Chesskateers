using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Chessman : MonoBehaviour // The abstract class for all piece scripts
{

    public int CurrentX { set; get; }
    public int CurrentY { set; get; }

    public bool isWhite;

    public float HP;
    public float damage;

    public Chessman(int hp, int dmg) //Constructor for HP and DMG values
    {
        HP = hp;
        damage = dmg;
    }


    public bool IsMine(bool side)
    {
        return side==isWhite;
    }
    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
    }

    public virtual bool[,] PossibleMoves()
    {
        return new bool[8, 8];
    }

    public bool Move(int x, int y, ref bool[,] r)
    {
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            Chessman c = ChesskateersGame.Chessmans[x, y];
            if (c == null)
                r[x, y] = true;
            else
            {
                if (isWhite != c.isWhite)
                    r[x, y] = true;
                return true;
            }
        }
        return false;
    }

    public virtual SpecialCard makeSpecial() // Dude I don't know what a virtual function is, but this is basically an abstract for SpecialCard in other piece scripts
    {
        return new SpecialCard(0, 0, null, true);
    }

    public abstract void initialize(); //Abstract for initializing pieces
}
