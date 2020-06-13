// Name: HealingCard.cs
// Purpose: Class for healing cards
// Version: 1. 
// Date: 2020/6/8
// Author: Ryan Dixon
// Dependencies: Card.cs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingCard : Card //See Card.cs to see what is inherited
{
    public HealingCard(int dmg, int healing, GameObject prefab, bool side) : base(dmg, healing, prefab, side)
    {
        //Card.cs constructor
    }
    public override void activateCard(ref Player cardOwner, ref Player enemy) //When card is clicked
    {
        cardOwner.battler.transform.GetChild(2).GetComponent<ParticleSystem>().Play(); //Play particle system attached to player to create healing field

        cardOwner.HP += this.Healing; // Add healing amount to player HP
    }
}
