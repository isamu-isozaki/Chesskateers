// Name: Card.cs
// Purpose: Abstract class for all card types
// Version: 1. 
// Date: 2020/6/8
// Author: Ryan Dixon
// Dependencies: None

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the abstract class that all Cards/Attacks inherit from
public abstract class Card
{
    public int Damage = 0; //Individual damage value of each card
    public int Healing = 0; //Individual health restoration value of each card, if needed
    public bool side; //Stores whether the card is used by the black or white side
    public GameObject Prefab; //The actual card button that appears onscreen
    public int positionInHand = 0; //I don't think we actually use this but I'm scared to delete it since Isamu said not to change anything
    public Player myPlayer { get; set; } //Stores Player object that has this card in its hand

    public Card(int dmg, int healing,  GameObject prefab, bool side) 
    { //Card constructor for damage, healing, onscreen card, and side
        Damage = dmg;
        Healing = healing;
        Prefab = prefab;
        this.side = side;
        
    }
    public GameObject DrawCard(Vector3 pos, Quaternion rotation) //Function to instantiate card onscreen
    {
        Prefab = MonoBehaviour.Instantiate(Prefab, pos, rotation); //Instantiates Prefab object, pos and rotation
        return Prefab;
        //pos and rotation aren't really important since the cards get put in a grid object which positions it automatically
    }

    public abstract void activateCard(ref Player cardOwner, ref Player enemy); //Abstract funtion to run events when a card is clicked

}
