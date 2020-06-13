// Name: Player.cs
// Purpose: Class for player objects in the battle scene
// Version: 1. 
// Date: 2020/6/8
// Author: Ryan Dixon
// Dependencies: AttacksJohnson.cs, BattlersJohnson.cs, Card.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a class for the player object during the Card Game portion.
public class Player : MonoBehaviour
{
    public float HP; //Health value for player
    public float Damage; //Initial Damage value
    public bool playerSide; //White or Black
    public Chessman playerType; //A Chessman object that checks what type the player is (Pawn, Rook, etc)
    public List<Card> cards_in_deck = new List<Card>(); //An empty list which stores the cards in a player's deck
    public List<Card> cards_in_hand = new List<Card>(); //An empty list which stores the cards in a player's hand
    public List<Card> cards_in_use = new List<Card>(); //I don't think we use this
    public List<Card> card_prefabs = new List<Card>(); //Or this
    public Vector3 pos; //Player position
    public Quaternion rot; //Player rotation
    public GameObject battler; //The physical GameObject that gets spawned (Different from the chess pieces because it has animation properties)
    public float OriginalHP; //Starting HP, needed for reference on battle end


    public Player(float hp, float dmg, Chessman type, bool side) // Player constructor
    { //Takes values for HP, damage, type, and side
        HP = hp;
        Damage = dmg;
        playerSide = side;
        playerType = type;
        OriginalHP = hp;
        // Creates a deck with 10 basic attacks and 10 heavy attacks
        for (int i = 0; i < 10; i++)
        {
            cards_in_deck.Add(new BasicAttack(5, 0, AttacksJohnson.attackList[0], side)); //WHEN DEBUGGING NEW ATTACKS, MAKE SURE TO CHANGE CARD() TO SPECIALCARD() IF NECESSARY
            cards_in_deck.Add(new HeavyAttack(7, 0, AttacksJohnson.attackList[6], side));
            //Takes attack type from AttacksJohnson.cs
        }
        cards_in_deck.Add(playerType.makeSpecial()); // Adds 1 special card to deck, uses the makeSpecial() function in each Chessman script
        if (playerType is Queen)
        { //If Queen, give player a healing card in deck too
            cards_in_deck.Add(new HealingCard(0, 10, AttacksJohnson.attackList[7], side));
        }
    }

    public Card Pull_from_deck_random(int rndm) //Draws a random card from the deck and puts it in the player's hand
    {
        Card pulled = cards_in_deck[rndm]; //Select card at index
        cards_in_deck.Remove(pulled); //Remove card from deck
        cards_in_hand.Add(pulled); //Add card to hand
        return pulled; //Return that card for reference in Game.cs
    }

    public void spawn_battler() //For spawning the pieces
    {
        if (playerType.isWhite)
        {// If white piece, use white spawn coordinates
            pos = new Vector3(63.7f, 39.34f, -46.2f);
            rot = Quaternion.Euler(-90, 0, 225);
            //Checks what type player is and spawns corresponding battler
            //Uses BattlersJohnson.cs
            //This code sucks, theres a better way to do this that isnt repetitive and garbage
            //If you know how to do that, email me rjd323@drexel.edu
            if (playerType is Pawn)
            {
                battler = BattlersJohnson.battlersList[5];
            }
            else if (playerType is Rook)
            {
                battler = BattlersJohnson.battlersList[2];
            }
            else if (playerType is Bishop)
            {
                battler = BattlersJohnson.battlersList[3];
            }
            else if (playerType is Knight)
            {
                battler = BattlersJohnson.battlersList[4];
            }
            else if (playerType is Queen)
            {
                battler = BattlersJohnson.battlersList[1];
            }
            else if (playerType is King)
            {
                battler = BattlersJohnson.battlersList[0];
            }
        }
        else
        { //Black spawn coordinates
            pos = new Vector3(-38.5f, 41.8f, 45.1f);
            rot = Quaternion.Euler(-90, 0, -135);
            if (playerType is Pawn)
            {
                battler = BattlersJohnson.battlersList[11];
            }
            else if (playerType is Rook)
            {
                battler = BattlersJohnson.battlersList[8];
            }
            else if (playerType is Bishop)
            {
                battler = BattlersJohnson.battlersList[9];
            }
            else if (playerType is Knight)
            {
                battler = BattlersJohnson.battlersList[10];
            }
            else if (playerType is Queen)
            {
                battler = BattlersJohnson.battlersList[7];
            }
            else if (playerType is King)
            {
                battler = BattlersJohnson.battlersList[6];
            }
        }
        battler = Instantiate(battler, pos, rot); //Instantiate chosen battler at chosen coordinates
    }

    public bool isOver() //Checks if a game is over
    {
        if (playerType is King) //If a king is captured end immediately
        {
            return true;
        }
        else
        {
            return (this.HP <= 0); //If a player's HP is at or below 0, also end it
        }
    }
}
