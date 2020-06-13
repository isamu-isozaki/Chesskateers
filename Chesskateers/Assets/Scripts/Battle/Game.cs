// Name: Game.cs
// Purpose: Handles the card battle scene
// Version: 1. 
// Date: 2020/6/8
// Author: Isamu Isozaki, Ryan Dixon
// Dependencies: Player.cs, BoardManager.cs, ChesskateersGame.cs, imports below

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;

public class Game : MonoBehaviour
{
    // TODO:
    // 1. Send request for what card is played -> done
    // 2. Recieve request and play that card by making that card and activate it -> done
    // 3. Don't flip camera -> done
    // 4. Keep showing just the cards in hand -> done
    public Player player1; //Objects for players 1 and 2
    public Player player2;
    Card chosen_card1; // We don't use these
    Card chosen_card2;
    public Camera theCamera; //The game camera
    public static Game game; //The game itself
    public GameObject dropZone; //The area on the UI where cards are placed
    public static List<object> whiteCamera = new List<object> {new Vector3(92, 67, -75), Quaternion.Euler(0, -45, 0)};
    public static List<object> blackCamera = new List<object> {new Vector3(-76, 71.3f, 83), Quaternion.Euler(0, 135, 0)};
    public List<List<object>> cameraCoords = new List<List<object>> {whiteCamera, blackCamera}; //Hi I'm Ryan I don't remember making this so maybe Isamu did this otherwise I dunno
    public int cardAmount = 4; //Amount of cards to be in a hand at a time
    public ParticleSystem particles; // this animation stuff might not be used either oh jesus
    public Animator anim;
    public Animstantiation animProp;
    public System.Random random = new System.Random(); // Random for drawing cards
    private bool canPlayCard = false;
    [DllImport("__Internal")]
    private static extern void playCard(String cardInfo);
    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        UnityMessageManager.Instance.OnMessage += RecieveMessage;
    }

    void OnDisable()
    {

        UnityMessageManager.Instance.OnMessage -= RecieveMessage;

    }

    void Start()
    {
        Chessman battlePiece1 = ChesskateersGame.battlePieces[0]; //Create battlepieces based on capturing and captured pieces from ChesskateersGame
        Chessman battlePiece2 = ChesskateersGame.battlePieces[1];
        canPlayCard = battlePiece1.isWhite == ChesskateersGame.side;
        if(ChesskateersGame.side) { //Swap camera depending on side
            theCamera.transform.position = (Vector3)cameraCoords[0][0];
            theCamera.transform.rotation = (Quaternion)cameraCoords[0][1];
        } else {
            theCamera.transform.position = (Vector3)cameraCoords[1][0];
            theCamera.transform.rotation = (Quaternion)cameraCoords[1][1];
        }
        player1 = new Player(battlePiece1.HP, battlePiece1.damage, battlePiece1, battlePiece1.isWhite); //Create both players
        player2 = new Player(battlePiece2.HP, battlePiece2.damage, battlePiece2, battlePiece2.isWhite);

        player1.spawn_battler(); //Spawn both players
        player2.spawn_battler();

        game = this;
        for (int i = 0; i < cardAmount; i++) //Pull a random hand from player 1's deck
        {
            player1.Pull_from_deck_random(random.Next(0, player1.cards_in_deck.Count)) ;
        }
        for (int j = 0; j < cardAmount; j++) //Pull a random hand from player 2's deck
        {
            player2.Pull_from_deck_random(random.Next(0, player2.cards_in_deck.Count)); //Player 1 and 2 hands are pulled in seperate loops because putting them together gives them the same random values
        }
        
        foreach (Card card in player1.cards_in_hand) //For each card in player 1's hand
        {
            card.Prefab.SetActive(true); //Activate prefab, make it visible
            GameObject playerCard = card.DrawCard(new Vector3(0, 0, 0), Quaternion.identity); //Instantiate each card so it can be seen onscreen
            playerCard.transform.SetParent(dropZone.transform, false); //Drop card into grid area on UI
            card.myPlayer = player1; //Set the card's player parameter as player 1
            playerCard.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => { Turn(card); }); //When this card is clicked, activate Turn()
        }
        foreach (Card card in player2.cards_in_hand) //For each card in player 2's hand
        {
            GameObject playerCard = card.DrawCard(new Vector3(0, 0, 0), Quaternion.identity); //Instantiate each card
            playerCard.transform.SetParent(dropZone.transform, false); //Drop card into grid UI
            card.myPlayer = player2; // Set card's player param as player 2
            playerCard.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => { Turn(card); }); //When this card is clicked, run Turn()
            card.Prefab.SetActive(false); //Deactivate cards so they don't show during player 1's starting turn
        }
        if(ChesskateersGame.side != player1.playerSide) //I think this part is for if black is the attacker, make them player 1? I actually dont remember
        {
            foreach (Card card in player1.cards_in_hand)
            {
                card.Prefab.SetActive(false);
            }
            foreach (Card card in player2.cards_in_hand)
            {
                card.Prefab.SetActive(true);
            }
        }
    }
    // Update is called once per frame

    public void Turn(Card card) //Runs when a card is clicked
    {
        Debug.Log("Called Turn");
        //Make socket call on the opponent with card parameters which I think is the simplest solution
        if(canPlayCard || ChesskateersGame.debug) {
            Debug.Log("Played card");
            Card new_card = card.myPlayer.Pull_from_deck_random(random.Next(0, card.myPlayer.cards_in_deck.Count)); //Pull a new replacement card from player's deck
            GameObject draw = new_card.DrawCard(new Vector3(0, 0, 0), Quaternion.identity);
            draw.transform.SetParent(dropZone.transform, false); //Instantiate and put in grid
            new_card.myPlayer = card.myPlayer; // Set player to the current player
            draw.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => { Turn(card); }); //Add Turn() listener
            if(!ChesskateersGame.debug) {
                RequestPlayCard(card);
            }
            playCard(card); //Play this card
            card.myPlayer.cards_in_hand.Remove(card); //Remove this card from deck
            Destroy(card.Prefab); //Destroy this card's prefab
            canPlayCard = false; //Can no longer play a card
        }
    }
    private void playCard(Card card) {
        if (player1.playerSide == card.side)
        {
            card.activateCard(ref player1, ref player2);
        }
        else
        {
            card.activateCard(ref player2, ref player1);
        }

    }
    public bool checkEnd()
    {
        if (player1.isOver()) //If player 1 is a king or at 0
        {
            ChesskateersGame.battlePieces[0].HP = player1.OriginalHP / 2; //Cut p1 HP in half
            Debug.Log("player 2 wins");
            return true;
        }
        else if (player2.isOver()) //If player 2 is a king or at 0
        {
            ChesskateersGame.battlePieces[1].HP = 0; //Kill P2
            Debug.Log("player 1 wins");
            return true;
        }
        return false;
        //Returns whether or not game is at its end

    }

    void Update()
    {
        if (checkEnd()) //If the battle is over
        {
            Debug.Log("loading from card scene to chess scene");
            ChesskateersGame.goingToChess = true; //Load chess game
            SceneManager.LoadScene("Assets/Scenes/Game.unity");
        }
    }
    // Inputs: Card card
    // Output: Nothing.
    // Send card played to react app which then is sent to the server
    private void RequestPlayCard(Card card) {
        Debug.Log("request play card");
        JObject cardInfo = JObject.FromObject(new
        {
            type = card.GetType().Name,
            damage = card.Damage,
            hp = card.Healing
        });
        // For android
        // UnityMessage message = new UnityMessage(name, moveJSON);
        // UnityMessageManager.Instance.SendMessageToRN(message);
        // For web
        Debug.Log(cardInfo.ToString());
        playCard(cardInfo.ToString());
    }
    // Inputs: string
    // Output: Nothing.
    // Get the string message from the react app
    private void RecieveMessage(String unityMessage) {
        Debug.Log("RecieveMessage");
        Debug.Log(unityMessage);
        JObject unityJSONMessage = JObject.Parse(unityMessage);
        if((string)unityJSONMessage["name"]=="card") {
            RecieveCard(unityJSONMessage);
        }
    }
    // Inputs: string
    // Output: Nothing.
    // Get the card from the message, create it, and play it
    private void RecieveCard(JObject unityMessage) {
        Debug.Log("Recieve card message");
        //Only gets called when opponent makes a move.
        //Takes a message from the server, creates a card of the opposite side, and plays that card
        String cardType = (String)unityMessage["type"];
        int Damage = (int)unityMessage["damage"];
        int Healing = (int)unityMessage["hp"];
        Card card;
        bool side = !ChesskateersGame.side;
        if(cardType == typeof(BasicAttack).Name) {
            card = new BasicAttack(Damage, Healing, AttacksJohnson.attackList[0], side);
        }
        else if(cardType == typeof(HeavyAttack).Name) {
            card = new HeavyAttack(7, 0, AttacksJohnson.attackList[6], side);
        }
        else if(cardType == typeof(SpecialCard).Name) {
            Chessman type = ChesskateersGame.battlePieces[0];
            if(ChesskateersGame.side == ChesskateersGame.battlePieces[0].isWhite) {
                type = ChesskateersGame.battlePieces[1];
                //opposite side's piece
            }
            card = type.makeSpecial();
        } else {
            card = new HealingCard(0, 10, AttacksJohnson.attackList[7], side);
        }
        playCard(card);
        canPlayCard = true;
    }
}
