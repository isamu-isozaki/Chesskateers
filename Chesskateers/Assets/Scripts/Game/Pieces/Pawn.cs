using System.Collections;
using UnityEngine;
public class Pawn : Chessman
{
    public SpecialCard pawnSpecial; // Refers to Card Game, creates empty special card object

    public Pawn(int hp=30, int damage=3) : base(hp, damage)
    {// Inherits from Chessman.cs constructor, creates HP and DMG values for this piece
    }

    public override bool[,] PossibleMoves() //Movement stuff for Chess Game
    {
        bool[,] r = new bool[8, 8];

        Chessman c, c2;
        int[] e = BoardManager.Instance.EnPassantMove;

        if (isWhite)
        {
            ////// White team move //////

            // Diagonal left
            if (CurrentX != 0 && CurrentY != 7)
            {
                if(e[0] == CurrentX -1 && e[1] == CurrentY + 1)
                    r[CurrentX - 1, CurrentY + 1] = true;

                c = ChesskateersGame.Chessmans[CurrentX - 1, CurrentY + 1];
                if (c != null && !c.isWhite)
                    r[CurrentX - 1, CurrentY + 1] = true;
            }

            // Diagonal right
            if (CurrentX != 7 && CurrentY != 7)
            {
                if (e[0] == CurrentX + 1 && e[1] == CurrentY + 1)
                    r[CurrentX + 1, CurrentY + 1] = true;

                c = ChesskateersGame.Chessmans[CurrentX + 1, CurrentY + 1];
                if (c != null && !c.isWhite)
                    r[CurrentX + 1, CurrentY + 1] = true;
            }

            // Middle
            if (CurrentY != 7)
            {
                c = ChesskateersGame.Chessmans[CurrentX, CurrentY + 1];
                if (c == null)
                    r[CurrentX, CurrentY + 1] = true;
            }

            // Middle on first move
            if (CurrentY == 1)
            {
                c = ChesskateersGame.Chessmans[CurrentX, CurrentY + 1];
                c2 = ChesskateersGame.Chessmans[CurrentX, CurrentY + 2];
                if (c == null && c2 == null)
                    r[CurrentX, CurrentY + 2] = true;
            }
        }
        else
        {
            ////// Black team move //////

            // Diagonal left
            if (CurrentX != 0 && CurrentY != 0)
            {
                if (e[0] == CurrentX - 1 && e[1] == CurrentY - 1)
                    r[CurrentX - 1, CurrentY - 1] = true;

                c = ChesskateersGame.Chessmans[CurrentX - 1, CurrentY - 1];
                if (c != null && c.isWhite)
                    r[CurrentX - 1, CurrentY - 1] = true;
            }

            // Diagonal right
            if (CurrentX != 7 && CurrentY != 0)
            {
                if (e[0] == CurrentX + 1 && e[1] == CurrentY - 1)
                    r[CurrentX + 1, CurrentY - 1] = true;

                c = ChesskateersGame.Chessmans[CurrentX + 1, CurrentY - 1];
                if (c != null && c.isWhite)
                    r[CurrentX + 1, CurrentY - 1] = true;
            }

            // Middle
            if (CurrentY != 0)
            {
                c = ChesskateersGame.Chessmans[CurrentX, CurrentY - 1];
                if (c == null)
                    r[CurrentX, CurrentY - 1] = true;
            }

            // Middle on first move
            if (CurrentY == 6)
            {
                c = ChesskateersGame.Chessmans[CurrentX, CurrentY - 1];
                c2 = ChesskateersGame.Chessmans[CurrentX, CurrentY - 2];
                if (c == null && c2 == null)
                    r[CurrentX, CurrentY - 2] = true;
            }
        }

        return r;
    }

    public override SpecialCard makeSpecial() //Creates new Special Card based on piece type
    {
        pawnSpecial = new SpecialCard(10, 0, AttacksJohnson.attackList[1], isWhite);
        // Gets card prefab from AttacksJohnson.cs, checks side with isWhite value from Chessman.cs
        return pawnSpecial;
    }

    public override void initialize() //Creates piece with proper HP and DMG, I don't even remember if we use this
    {
        Pawn pawn = new Pawn();
        HP = pawn.HP;
        damage = pawn.damage;
    }
}