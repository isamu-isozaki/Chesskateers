// Name: BoardManager.cs
// Purpose: Makes the Chess game and communicate with react
// Version: 1. 
// Date: 2020/6/8
// Author: Isamu Isozaki, Ryan Dixon
// Dependencies: Look at imports below

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; set; }
    private bool[,] allowedMoves { get; set; }

    private const float TILE_SIZE = 1.5f;
    private const float TILE_OFFSET = -5.28f;

    private const float RAYCAST_SCALE = 2f/3f;
    private const float RAYCAST_OFFSET = 6f;

    private int selectionX = -1;
    private int selectionY = -1;
    public GameObject[] chessPrefab;
    // "White King", "White Queen", "White Rook", "White Bishop", "White Knight", "White Pawn",
    //     "Black King", "Black Queen", "Black Rook", "Black Bishop", "Black Knight", "Black Pawn"
    private List<GameObject> activeChessman;

    private Quaternion whiteOrientation = Quaternion.Euler(-90, 270, 0);
    private Quaternion blackOrientation = Quaternion.Euler(-90, 90, 0);
    public Chessman[,] Chessmans { get; set; }

    public static bool isWhiteTurn = true;

    private Material previousMat;
    public Material selectedMat;

    public int[] EnPassantMove { set; get; }

    private Chessman tempChessman;//Here for performance reasons to reduce garbage collections in SetChessmans function
    private int[] tempPosition;
    [DllImport("__Internal")]
    private static extern void move(String fromTo);
    [DllImport("__Internal")]
    private static extern void gameOver(int side);

    private static GameObject instance;
    // Inputs: None
    // Output: bool. The side of the player playing
    // Returns the side of the player playing the game
    public bool GetSide()
    {
        return ChesskateersGame.side;
    }
    // Inputs: None
    // Output: Nothing.
    // Starts listening to events of battle game ending and chess game loading and vice versa. Also listens to messages being sent from React
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        UnityMessageManager.Instance.OnMessage += RecieveMessage;
    }
    // Inputs: None
    // Output: Nothing.
    // Stps listening to events of battle game ending and chess game loading and vice versa. Also stop listens to messages being sent from React
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        UnityMessageManager.Instance.OnMessage -= RecieveMessage;

    }
    // Inputs: None
    // Output: Nothing.
    // Starts the chess game
    public void Start()
    {
        //Thanks https://answers.unity.com/questions/982403/how-to-not-duplicate-game-objects-on-dontdestroyon.html
        //To prevent multiple boardmanagers spawning due to DontDestroyOnLoad,
        if(ChesskateersGame.debug) {
            ChesskateersGame.side = true;
        }
        DontDestroyOnLoad(gameObject);
        if (!ChesskateersGame.initialized)
        {
            Instance = this;
            InitializeChessGame();
            ChesskateersGame.initialized = true;
        } else {
            Destroy(gameObject);
        }
    }
    // Inputs: The scene and mode. Never used in function
    // Output: Nothing.
    // Set the chess game based on the results of the card game
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("level load called");
        EnPassantMove = new int[2] { -1, -1 };
        if (ChesskateersGame.goingToChess)
        {
            ChesskateersGame.goingToChess = false;
            // selectedChessman = ChesskateersGame.battlePieces[0];
            // ChesskateersGame.Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = selectedChessman;
            int player1X = ChesskateersGame.battlePieces[0].CurrentX;
            int player1Y = ChesskateersGame.battlePieces[0].CurrentY;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (ChesskateersGame.Chessmans[i, j] != null)
                    {
                        ChesskateersGame.Chessmans[i, j].gameObject.SetActive(true);
                        // && ChesskateersGame.selectedChessman
                        if (i == player1X && j == player1Y)
                        {
                            ChesskateersGame.selectedChessman = ChesskateersGame.Chessmans[i, j];
                            //Change logic because ChesskateersGame.selectedChessman is not set for the other side.
                            if (ChesskateersGame.battlePieces[1].HP <= 0) //ChesskateersGame.selectedChessman won
                            {
                                int x = ChesskateersGame.battlePieces[1].CurrentX;
                                int y = ChesskateersGame.battlePieces[1].CurrentY;
                                activeChessman.Remove(ChesskateersGame.Chessmans[x, y].gameObject);
                                Destroy(ChesskateersGame.Chessmans[x, y].gameObject);
                                if (ChesskateersGame.promotionPending)
                                {
                                    Debug.Log("promotion if called");
                                    if (y == 7)
                                    {
                                        activeChessman.Remove(ChesskateersGame.selectedChessman.gameObject);
                                        Destroy(ChesskateersGame.selectedChessman.gameObject);
                                        SpawnChessman(1, x, y, true);
                                        //SetChessmans();   
                                        ChesskateersGame.selectedChessman = ChesskateersGame.Chessmans[x, y];
                                    }
                                    else if (y == 0)
                                    {
                                        activeChessman.Remove(ChesskateersGame.selectedChessman.gameObject);
                                        Destroy(ChesskateersGame.selectedChessman.gameObject);
                                        SpawnChessman(7, x, y, false);
                                        ChesskateersGame.selectedChessman = ChesskateersGame.Chessmans[x, y];

                                    }
                                }
                                updateSelected(x, y);

                            }
                            else
                            {
                                ChesskateersGame.selectedChessman.GetComponent<MeshRenderer>().material = previousMat;
                                ChesskateersGame.selectedChessman = null;
                            }
                        }
                    }
                }
            }

            ChesskateersGame.promotionPending = false;

        }
    }

    // Inputs: None
    // Output: Nothing.
    // Called once per frame. Selects chessman and moves them.
    void Update()
    {
        if(ChesskateersGame.setSide || ChesskateersGame.debug) {
            UpdateSelection();

            if (Input.GetMouseButtonDown(0))
            {
                if (selectionX >= 0 && selectionY >= 0)
                {
                    if (ChesskateersGame.selectedChessman == null)
                    {
                        // Select the chessman
                        SelectChessman(selectionX, selectionY);
                    }
                    else
                    {
                        // Move the chessman
                        Debug.Log("request move");
                        if(ChesskateersGame.debug) {
                            MoveChessman(selectionX, selectionY);
                        } 
                        else {
                            RequestMove(selectionX, selectionY);
                        }
                        //MoveChessman(selectionX, selectionY);
                        
                    }
                }
            }

            if (Input.GetKey("escape"))
                Application.Quit();
        }
    }
    // Inputs: x and y position on the board
    // Output: Nothing
    // Set selectedChessman to the chess piece at position x, y if there's a piece that can be selected there
    private void SelectChessman(int x, int y)
    {
        if (ChesskateersGame.Chessmans[x, y] == null) return;
        if (!ChesskateersGame.debug && !ChesskateersGame.Chessmans[x, y].IsMine(ChesskateersGame.side)) return;
        if (ChesskateersGame.Chessmans[x, y].isWhite != isWhiteTurn) return;
        SetAllowedMoves(x, y);
        bool hasAtLeastOneMove = false;

        
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (allowedMoves[i, j])
                {
                    hasAtLeastOneMove = true;
                    i = 8;
                    break;
                }
            }
        }

        if (!hasAtLeastOneMove)
            return;
        SetSelectedChessman(x, y);
        selectedMat.mainTexture = previousMat.mainTexture;
        ChesskateersGame.selectedChessman.GetComponent<MeshRenderer>().material = selectedMat;

        BoardHighlights.Instance.HighLightAllowedMoves(allowedMoves);
    }
    // Inputs: x, y indices
    // Output: Nothing.
    // Go to the battle game and hide all the chess pieces. If the attacked piece is the king, lose the game
    private void CapturePiece(int x, int y)
    {
        Debug.Log("Capture piece");
        Chessman chessMan  = ChesskateersGame.Chessmans[x, y];
        ChesskateersGame.battlePieces = new Chessman[2];
        ChesskateersGame.battlePieces[0] = ChesskateersGame.selectedChessman;
        ChesskateersGame.battlePieces[1] = chessMan;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (ChesskateersGame.Chessmans[i,j] != null)
                {
                    ChesskateersGame.Chessmans[i,j].gameObject.SetActive(false);
                }
            }
        }
        if (chessMan.GetType() == typeof(King))
        {
            //If a king captures a piece, it also ends the game
            // End the game
            EndGame();
            return;
        }
        Debug.Log("loading from chess scene to card scene");
        SceneManager.LoadScene("Assets/Scenes/Battle.unity");
        /*activeChessman.Remove(chessMan.gameObject);
        Destroy(chessMan.gameObject);*/
    }
    // Inputs: Integer x and y
    // Output: Nothing.
    // Move the chess piece and request move to the react app
    private void RequestMove(int x, int y) {
        if(x > 7 || x < 0 || y > 7 || y < 0) {
            return;
        }
        if (allowedMoves[x, y]) {
            name = "move";
            JObject moveJSON = JObject.FromObject(new
            {
                fromX = ChesskateersGame.selectedChessman.CurrentX,
                fromY = ChesskateersGame.selectedChessman.CurrentY,
                toX = x,
                toY = y
            });
            // UnityMessage message = new UnityMessage(name, moveJSON);
            // For android
            // UnityMessageManager.Instance.SendMessageToRN(message);
            // For web
            move(moveJSON.ToString());
            MoveChessman(x, y);
        }
        BoardHighlights.Instance.HideHighlights();
        if(ChesskateersGame.selectedChessman == null || ChesskateersGame.selectedChessman.gameObject == null) {
            return;
        }
        ChesskateersGame.selectedChessman.GetComponent<MeshRenderer>().material = previousMat;
        ChesskateersGame.selectedChessman = null;
    }
    // Inputs: String message
    // Output: Nothing.
    // Recieve message from the react app and call the functions responsible for pressessing those messages
    private void RecieveMessage(String unityMessage) {
        Debug.Log("RecieveMessage");
        JObject unityJSONMessage = JObject.Parse(unityMessage);
        if ((string)unityJSONMessage["name"]=="set side") {
            RecieveSide(unityJSONMessage);
        } 
        else if((string)unityJSONMessage["name"]=="move") {
            RecieveMove(unityJSONMessage);
        }
    }
    // Inputs: String message
    // Output: Nothing.
    // Takes the message and sets the side
    private void RecieveSide(JObject unityMessage) {
        Debug.Log("RecieveSide");
        ChesskateersGame.side = (bool)unityMessage["side"];
        ChesskateersGame.setSide = true;
    }
    // Inputs: String message
    // Output: Nothing.
    // Takes the message and moves the chess piece
    private void RecieveMove(JObject unityMessage) {
        Debug.Log("RecieveMove");
        int fromX = (int)unityMessage["fromX"];
        int fromY = (int)unityMessage["fromY"];
        int toX = (int)unityMessage["toX"];
        int toY = (int)unityMessage["toY"];
        ChesskateersGame.selectedChessman = ChesskateersGame.Chessmans[fromX, fromY];
        MoveChessman(toX, toY);
    }
    // Inputs: integer x and y
    // Output: Nothing.
    // Moves chessman from selectedChessman.CurrentX and selectedChessman.CurrentY to x and y. Capture piece if there's a piece in the way
    private void MoveChessman(int x, int y)
    {
        bool capturedPiece = false;
        Chessman c = ChesskateersGame.Chessmans[x, y];
        if (c != null && c.isWhite != isWhiteTurn)
        {
            // Capture a piece
            Debug.Log("called capture piece");
            CapturePiece(x, y);
            capturedPiece = true;
        }
        

        if (x == EnPassantMove[0] && y == EnPassantMove[1])
        {
            //Account for pawn taking piece that goes past it
            if (isWhiteTurn) {
                c = ChesskateersGame.Chessmans[x, y-1];
                CapturePiece(x, y-1);
                capturedPiece = true;

            } else {
                c = ChesskateersGame.Chessmans[x, y+1];
                CapturePiece(x, y+1);
                capturedPiece = true;
            }
        }
        EnPassantMove[0] = -1;
        EnPassantMove[1] = -1;
        //Issue here. The Queen get's spawned but the tag seems to not be the same so the Queen does not get added
        //to the non master client's Chessmans when running SetChessmans. Ryan good luck!
        if (ChesskateersGame.selectedChessman.GetType() == typeof(Pawn))
        {
            if(y == 7) // White Promotion
            {
                if (!capturedPiece)
                {
                    activeChessman.Remove(ChesskateersGame.selectedChessman.gameObject);
                    Destroy(ChesskateersGame.selectedChessman.gameObject);
                    SpawnChessman(1, x, y, true);
                    //SetChessmans();   
                    ChesskateersGame.selectedChessman = ChesskateersGame.Chessmans[x, y];
                }
                else
                {
                    Debug.Log("promotion set true");
                    ChesskateersGame.promotionPending = true;

                }
            }
            else if (y == 0) // Black Promotion
            {
                //Black is never the master client so need to account for that with TransferOwnership and SetChessmans
                if (!capturedPiece)
                {
                    activeChessman.Remove(ChesskateersGame.selectedChessman.gameObject);
                    Destroy(ChesskateersGame.selectedChessman.gameObject);
                    SpawnChessman(7, x, y, false);
                    ChesskateersGame.selectedChessman = ChesskateersGame.Chessmans[x, y];
                }
                else
                {
                    Debug.Log("promotion set true");
                    ChesskateersGame.promotionPending = true;
                }
            }
            
            EnPassantMove[0] = x;
            if (ChesskateersGame.selectedChessman.CurrentY == 1 && y == 3)
                EnPassantMove[1] = y - 1;
            else if (ChesskateersGame.selectedChessman.CurrentY == 6 && y == 4)
                EnPassantMove[1] = y + 1;
        }

        if (!capturedPiece)
        {
            updateSelected(x, y);
        }

        isWhiteTurn = !isWhiteTurn;
        BoardHighlights.Instance.HideHighlights();
        
    }
    // Inputs: integer x and y
    // Output: Nothing.
    // Update position of selectedChessman and then set it to null
    private void updateSelected(int x, int y)
    {
        ChesskateersGame.Chessmans[ChesskateersGame.selectedChessman.CurrentX, ChesskateersGame.selectedChessman.CurrentY] = null;
        ChesskateersGame.selectedChessman.transform.position = GetTileCenter(x, y);
        ChesskateersGame.selectedChessman.SetPosition(x, y);
        ChesskateersGame.Chessmans[x, y] = ChesskateersGame.selectedChessman;
        if (ChesskateersGame.selectedChessman == null || ChesskateersGame.selectedChessman.gameObject == null)
        {
            return;
        }
        ChesskateersGame.selectedChessman.GetComponent<MeshRenderer>().material = previousMat;
        ChesskateersGame.selectedChessman = null;
    }
    // Inputs: None
    // Output: Nothing.
    // Set the initial parameters and spawn all the chess pieces
    private void InitializeChessGame()
    {
        ChesskateersGame.Chessmans = new Chessman[8, 8];
        EnPassantMove = new int[2] { -1, -1 };
        SpawnAllChessmans();
    }
    // Inputs: integer x and y
    // Output: Nothing.
    // Set selectedChessman to chess piece at position x and y
    private void SetSelectedChessman(int x, int y) {
        ChesskateersGame.selectedChessman = ChesskateersGame.Chessmans[x, y];
        if(ChesskateersGame.selectedChessman == null || ChesskateersGame.selectedChessman.gameObject == null) {
            return;
        }
        previousMat = ChesskateersGame.selectedChessman.GetComponent<MeshRenderer>().material;
    }
    // Inputs: integer x and y
    // Output: Nothing.
    // Set the allowed moves the chess piece can take
    private void SetAllowedMoves(int x, int y) {
        allowedMoves = ChesskateersGame.Chessmans[x, y].PossibleMoves();
    }
    // Inputs: Nothing
    // Output: Nothing.
    // Detects which piece is getting taken based on mouse click
    private void UpdateSelection()
    {
        if (!Camera.main) return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50.0f, LayerMask.GetMask("ChessPlane")))
        {
            selectionX = (int)((hit.point.x+RAYCAST_OFFSET)*RAYCAST_SCALE);
            selectionY = (int)((hit.point.z+RAYCAST_OFFSET)*RAYCAST_SCALE);
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }
    // Inputs: integer index, x, and y and whether if the piece is while
    // Output: Nothing.
    // Spawn chess piece based on prefab index, position given by x and y and the side
    private void SpawnChessman(int index, int x, int y, bool isWhite)
    {
        Vector3 position = GetTileCenter(x, y);
        GameObject go = null;
        if (isWhite)
        {
            go = Instantiate(chessPrefab[index], position, whiteOrientation);
        }
        else
        {
            go = Instantiate(chessPrefab[index], position, blackOrientation);
        }
        go.transform.SetParent(transform);
        ChesskateersGame.Chessmans[x, y] = go.GetComponent<Chessman>();
        ChesskateersGame.Chessmans[x, y].initialize();
        ChesskateersGame.Chessmans[x, y].SetPosition(x, y);
        activeChessman.Add(go);
    }
    // Inputs: integer x and y
    // Output: vector3
    // Returns the 3d coordinates given chess piece location on board
    private Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;

        return origin;
    }
    // Inputs: The game object of the chess piece
    // Output: Nothing.
    // Gets the board location from the chesspiece gameobject
    private void GetCenterFromPos(GameObject go, ref int[] pos)
    {
        Vector3 origin = go.transform.position;
        pos[0] = (int)((origin.x - TILE_OFFSET)/TILE_SIZE);
        pos[1] = (int)((origin.z - TILE_OFFSET)/TILE_SIZE);
    }
    // Inputs: Nothing
    // Output: Nothing.
    // Spawn all the chessmans on the board
    private void SpawnAllChessmans()
    {
        activeChessman = new List<GameObject>();

        /////// White ///////
        // King
        SpawnChessman(0, 3, 0, true);
        // Queen
        SpawnChessman(1, 4, 0, true);

        // Rooks
        SpawnChessman(2, 0, 0, true);
        SpawnChessman(2, 7, 0, true);

        // Bishops
        SpawnChessman(3, 2, 0, true);
        SpawnChessman(3, 5, 0, true);

        // Knights
        SpawnChessman(4, 1, 0, true);
        SpawnChessman(4, 6, 0, true);

        // Pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnChessman(5, i, 1, true);
        }
        /////// Black ///////
        // King
        SpawnChessman(6, 4, 7, false);

        // Queen
        SpawnChessman(7, 3, 7, false);

        // Rooks
        SpawnChessman(8, 0, 7, false);
        SpawnChessman(8, 7, 7, false);

        // Bishops
        SpawnChessman(9, 2, 7, false);
        SpawnChessman(9, 5, 7, false);

        // Knights
        SpawnChessman(10, 1, 7, false);
        SpawnChessman(10, 6, 7, false);

        // Pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnChessman(11, i, 6, false);
        }
    }
    // Inputs: Nothing.
    // Output: Nothing.
    // Ends the game
    private void EndGame()
    {
        if (isWhiteTurn){
            Debug.Log("White wins");
            gameOver(1);
        } else{
            Debug.Log("Black wins");
            gameOver(0);
        }

    }

    
}