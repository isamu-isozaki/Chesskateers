using System.Collections;
using UnityEngine;
public class Bishop : Chessman
{
    public SpecialCard bishopSpecial; // Refers to Card Game, creates empty special card object

    public Bishop(int hp=47, int damage=7) : base(hp, damage)
    { // Inherits from Chessman.cs constructor, creates HP and DMG values for this piece
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


        return r;
    }
    public override SpecialCard makeSpecial() //Creates new Special Card based on piece type
    {
        bishopSpecial = new SpecialCard(15, 0, AttacksJohnson.attackList[4], isWhite);
        // Gets card prefab from AttacksJohnson.cs, checks side with isWhite value from Chessman.cs
        return bishopSpecial;
    }

    public override void initialize() //Creates piece with proper HP and DMG, I don't even remember if we use this
    {
        Bishop bishop = new Bishop();
        HP = bishop.HP;
        damage = bishop.damage;
    }

}