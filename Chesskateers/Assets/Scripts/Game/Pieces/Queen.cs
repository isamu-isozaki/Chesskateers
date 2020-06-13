using System.Collections;
using UnityEngine;
public class Queen : Chessman
{
    public SpecialCard queenSpecial; // Refers to Card Game, creates empty special card object

    public Queen(int hp=47, int damage=10) : base(hp, damage)
    {// Inherits from Chessman.cs constructor, creates HP and DMG values for this piece
    }

    public override bool[,] PossibleMoves() //Movement stuff for Chess Game
    {
        bool[,] r = new bool[8, 8];

        int i, j;

        // Top left
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--;
            j++;
            if (i < 0 || j >= 8) break;

            if (Move(i, j, ref r)) break;
        }

        // Top right
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++;
            j++;
            if (i >= 8 || j >= 8) break;

            if (Move(i, j, ref r)) break;
        }

        // Down left
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--;
            j--;
            if (i < 0 || j < 0) break;

            if (Move(i, j, ref r)) break;
        }

        // Down right
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++;
            j--;
            if (i >= 8 || j < 0) break;

            if (Move(i, j, ref r)) break;
        }

        // Right
        i = CurrentX;
        while (true)
        {
            i++;
            if (i >= 8) break;

            if (Move(i, CurrentY, ref r)) break;
        }

        // Left
        i = CurrentX;
        while (true)
        {
            i--;
            if (i < 0) break;

            if (Move(i, CurrentY, ref r)) break;
        }

        // Up
        i = CurrentY;
        while (true)
        {
            i++;
            if (i >= 8) break;

            if (Move(CurrentX, i, ref r)) break;
        }

        // Down
        i = CurrentY;
        while (true)
        {
            i--;
            if (i < 0) break;

            if (Move(CurrentX, i, ref r)) break;

        }

        return r;
    }

    public override SpecialCard makeSpecial() //Creates new Special Card based on piece type
    {
        queenSpecial = new SpecialCard(15, 0, AttacksJohnson.attackList[5], isWhite);
        // Gets card prefab from AttacksJohnson.cs, checks side with isWhite value from Chessman.cs
        return queenSpecial;
    }

    public override void initialize() //Creates piece with proper HP and DMG, I don't even remember if we use this
    {
        Queen queen = new Queen();
        HP = queen.HP;
        damage = queen.damage;
    }

}