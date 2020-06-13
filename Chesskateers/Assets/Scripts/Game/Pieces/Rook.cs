using System.Collections;
using UnityEngine;
public class Rook : Chessman
{
    public SpecialCard rookSpecial; // Refers to Card Game, creates empty special card object

    public Rook(int hp=50, int damage=5) : base(hp, damage)
    {// Inherits from Chessman.cs constructor, creates HP and DMG values for this piece
    }

    public override bool[,] PossibleMoves() //Movement stuff for Chess Game
    {
        bool[,] r = new bool[8, 8];

        int i;

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
        rookSpecial = new SpecialCard(15, 0, AttacksJohnson.attackList[2], isWhite);
        // Gets card prefab from AttacksJohnson.cs, checks side with isWhite value from Chessman.cs
        return rookSpecial;
    }

    public override void initialize() //Creates piece with proper HP and DMG, I don't even remember if we use this
    {
        Rook rook = new Rook();
        HP = rook.HP;
        damage = rook.damage;
    }

}