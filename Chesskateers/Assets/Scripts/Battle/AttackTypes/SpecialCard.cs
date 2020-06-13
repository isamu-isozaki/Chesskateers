// Name: SpecialCard.cs
// Purpose: Class for special attack cards
// Version: 1. 
// Date: 2020/6/8
// Author: Ryan Dixon
// Dependencies: Card.cs, PropsJohnson.cs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCard : Card //See Card.cs to see what is inherited
{

    public SpecialCard(int dmg, int healing, GameObject prefab, bool side) : base (dmg, healing, prefab, side)
    {
        //Card.cs constructor
    }

    public override void activateCard(ref Player cardOwner, ref Player enemy) //When card is clicked
    {
        Animator anim = cardOwner.battler.GetComponent<Animator>(); //Get animator component of the user
        Animstantiation animProp = null; //Create this empty object in case a prop needs to animated (ex: Rook's Wall)
        if (cardOwner.playerType is Rook) //If player is a Rook
        {
            if (cardOwner.playerSide == true) //Check side
            {
                animProp = new Animstantiation(PropsJohnson.propsList[0]);
                //Take the WhiteWall object, make it into Animstantiation object (from Noah) which will both create and animate object
            }
            else
            {
                animProp = new Animstantiation(PropsJohnson.propsList[1]);
                //Same but with BlackWall
            }
        }
        if (cardOwner.playerType is Bishop) //Bishop has particles in his special, so run those if player is a Bishop
        {
            cardOwner.battler.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
        }

        if (!(animProp is null)) { animProp.InstantiateObject(); } //If animprop is not null and has an actual object, animate it.
        anim.SetTrigger("Active"); //Trigger special animation
        //Each piece has a unique special animation but they're all triggered the same way

        cardOwner.HP += this.Healing; //Add healing if necessary, I don't think it happens (?)
        enemy.HP -= (this.Damage + cardOwner.Damage); // Removes from enemy HP, card Damage plus player's inherent Damage
    }
}
