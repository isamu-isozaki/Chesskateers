using System.Collections;
using UnityEngine;

public class King : Chessman
{
    public King(int hp=20, int damage=15) : base(hp, damage)
    {
    }

    public override bool[,] PossibleMoves()
    {
        bool[,] r = new bool[8, 8];

        Move(CurrentX + 1, CurrentY, ref r); // up
        Move(CurrentX - 1, CurrentY, ref r); // down
        Move(CurrentX, CurrentY - 1, ref r); // left
        Move(CurrentX, CurrentY + 1, ref r); // right
        Move(CurrentX + 1, CurrentY - 1, ref r); // up left
        Move(CurrentX - 1, CurrentY - 1, ref r); // down left
        Move(CurrentX + 1, CurrentY + 1, ref r); // up right
        Move(CurrentX - 1, CurrentY + 1, ref r); // down right

        return r;
    }

    public override void initialize()
    {
        King king = new King();
        HP = king.HP;
        damage = king.damage;
    }

}

