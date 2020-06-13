using System.Collections;
using UnityEngine;

public class Knight : Chessman
{
    public SpecialCard knightSpecial; // Refers to Card Game, creates empty special card object

    public Knight(int hp=45, int damage=9) : base(hp, damage)
    { // Inherits from Chessman.cs constructor, creates HP and DMG values for this piece
    }

    public override bool[,] PossibleMoves() //Movement stuff for Chess Game
    {
        bool[,] r = new bool[8, 8];

        // Up left
        Move(CurrentX - 1, CurrentY + 2, ref r);

        // Up right
        Move(CurrentX + 1, CurrentY + 2, ref r);

        // Down left
        Move(CurrentX - 1, CurrentY - 2, ref r);

        // Down right
        Move(CurrentX + 1, CurrentY - 2, ref r);


        // Left Down
        Move(CurrentX - 2, CurrentY - 1, ref r);

        // Right Down
        Move(CurrentX + 2, CurrentY - 1, ref r);

        // Left Up
        Move(CurrentX - 2, CurrentY + 1, ref r);

        // Right Up
        Move(CurrentX + 2, CurrentY + 1, ref r);

        return r;
    }

    public override SpecialCard makeSpecial() //Creates new Special Card based on piece type
    {
        knightSpecial = new SpecialCard(15, 0, AttacksJohnson.attackList[3], isWhite);
        // Gets card prefab from AttacksJohnson.cs, checks side with isWhite value from Chessman.cs
        return knightSpecial;
    }

    public override void initialize() //Creates piece with proper HP and DMG, I don't even remember if we use this
    { 
        Knight knight = new Knight();
        HP = knight.HP;
        damage = knight.damage;
    }

}