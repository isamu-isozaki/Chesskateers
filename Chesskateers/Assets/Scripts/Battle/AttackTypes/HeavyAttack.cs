// Name: HeavyCard.cs
// Purpose: Class for heavy attack cards
// Version: 1. 
// Date: 2020/6/8
// Author: Ryan Dixon
// Dependencies: Card.cs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttack : Card //See Card.cs to see what is inherited
{
    public HeavyAttack(int dmg, int healing, GameObject prefab, bool side) : base(dmg, healing, prefab, side)
    {
        //Card.cs constructor
    }
    public override void activateCard(ref Player cardOwner, ref Player enemy) //When card is clicked
    {
        enemy.battler.transform.GetChild(1).GetComponent<ParticleSystem>().Play(); //Play particle system attached to enemy to create heavy attack sparks

        cardOwner.HP += this.Healing; // Adds healing if necessary, doesn't happen
        enemy.HP -= (this.Damage + cardOwner.Damage); // Removes from enemy HP, card Damage plus player's inherent Damage
    }
}

