 # Chesskateers

## Prerequisites

- Unity version 2019.1.1f1

### How to run

- Open unity projects  at Chesskateers folder

- Find Lobby scene in Assets/Photon/PhotonUnityNetworking/Scenes

- Double click scene to open it.

- Press Ctrl+B to build the project.

- Once that is done, press the play button to have a second instance of the game running

- Choose names for each instance, log in. 

- Make one instance create a room with 2 players.

- Press Ready

- Have the other join the room by pressing Join random room or show room list and joining.

- Press Ready

- In the window where the room was made, click start game.

- Both instances should now start a chess game. The room maker should be the white side and the other should be the black side.

### Script location and general functionalities

##### Location

Assets/Scripts

##### Lobby folder

The lobby folder contains the scripts for the lobby where the player makes a game, joins a game and basically prepares everything for the chess game. This was taken mostly from the Astroid Game demo provided by Photon 2 once the asset is obtained. The scripts haven't been modified significantly.

##### Game folder

The game folder contains the scripts for the chess game functionalities. This based on [This project]([https://github.com/SacuL/3D-Chess-Unity/tree/master/Assets](https://github.com/SacuL/3D-Chess-Unity/tree/master/Assets) but was modified so that it can do multiplayer.

##### ChesskateersGame.cs

ChesskateersGame.cs contains basic configuration and utilities.

### Game scripts

- BoardHighlight.cs provides functionality for highlight on the board. This wasn't modified very much from the original script

- Chessman.cs is the base class for all the chess pieces. 
  
  Modifications: 
  
  - Added a PhotonView component so that it can be owned by a player.
  
  - Added IsMine function to check if the piece is a certain player's. 
  
  - Added ChangeOwner function to change the owner of the chess piece because originally, all pieces are owned by the master client(the player who made the game)

- Pieces folder contains all pieces that inherits from Chessman.cs such as King, Queen, etc. The main additions that are done after inheriting is returning a boolean mask which is 8 by 8 telling the game where the chess piece can go with the PossibleMoves function. Has not been modified from the original version.

- BoardManager.cs manages the game overall.
  
  Modifications
  
  - Combined functionalities with AstroidGame and thus added
    
    - The OnDisconnected function which loads the lobby scene once the player has disconnected
    
    - The OnLeftRoom function which disconnects once the master client leaves the room
    
    - The OnPlayerPropertiesUpdate function which hasn't been modified. It detects if all the players are loaded and adds a timer which can be extended in the future, possibly for a turn timer
    
    - The CheckAllPlayerLoadedLevel function which checks if all players are in the level
    
    - The End function is called when the game is over(the king piece is taken). This will disconnect the player from the game and thus call the OnDisconnected function.
  
  - Custom functions added are
    
    - The CapturePiece function is called when a piece is going to be captured. If a king is captured, the End function is called. Destroys game object and removes the gameObject of the piece getting captured from the list of current pieces on the board.
    
    - The InitializeChessGame function with a PunRPC field. This initializes the chess game variables for both players so that they can be modified in the future. Also the master client spawns all the chess man on the screen. The other player won't spawn anything.
    
    - The SetChessmans function with a PunRPC field is only called for the none-master client player. The function gets all the chess pieces in the current scene and initializes the Chessmans array with the pieces and their locations as the index of the array. This array is essential for moving pieces and selecting them. This function can't be simplified such as by serializing the chesspieces to an integer array and sending them as that kept returning an error. The same is true for sending the chess piece game objects. 
    - The SetSelectedChessmans with a PunRPC field takes two integer inputs(x and y) which correspond to the indices in the Chessmans array. Chessmans[x, y] will be the selectedChessman for both players which will be the chess piece that will be moved in MoveChessman. Also the texture is stored in previousMat.
    
    - The SetAllowedMoves function with a PunRPC field takes integer arguments x and y and assigns the possible moves for the chesspiece at Chessmans[x, y] to a boolean array called allowedMoves. This is essential as when the chess piece is moved with the MoveChessman function, which is called for both players, allowedMoves need to be set properly for it to be registers as moving properly on both screens. By this, it is meant that while on the screen, the chess piece can be seen as moving, in the Chessmans array for one player, it has moved and for the other player, it will be considered an invalid move and thus will be considered to not have moved.
    
    - The GetCenterFromPos is a function for SetChessmans where given the 3d position of the gameobjects, it returns the integer indices of the Chessmans array where the GameObject should be placed. 
  - Modified pre-existing functions
    - The Start function sets the master client's properties to loaded level and gets the Photon View which is used for doing the RPC calls which calls a function for all players. 
    - The Update functioin first checks what square the mouse is over with the UpdateSelection function. Then, once the mouse is pushed down, if no chess piece is selected, there will be an attempt to select a chesspiece with the SelectChessman function. If a chess piece is already selected, a RPC call for all players are made which will attempt to move the piece for all players. It will also check if the user pressed the escape key in which case, the game will end. Finally, the update function will check if both players have joined. If so, InitializeChessGame is called for all players, the ownership of the black pieces are transferred to the black player with the TransferOwnership function and finally the RPC is called for SetChessman for the non-masterclient player.
    - The SelectChessman function takes two integers, x and y, as arguments and checks if the selected chessman can be selected. This is done by first checking if there is a chesspiece in the given space, whether that piece is that player's piece(The white player only has the white pieces, the black player has the black pieces). And finally, it checks if it's that player's turn.

    Then, SetAllowedMoves is called by RPC setting the possible moves for that piece to allowedMoves. If there's at least one valid move, then SetSelectedChessman will be RPC called to everyone, the selectedChessman's texture will change to selectedMat which is public and can be loaded from assets, and all available moves are highlighted.
    - The MoveChessmans function with a PunRPC field takes two arguments x and y. It first checks if x and y are valid(the selectedChessman can move there). Then, if the piece can be captured, it will capture the piece with the CapturePiece function. The EnpassantMoves are considered as well. An attempt at promotion is done but that was unsuccessful. Finally, the Chessmans[x, y] of the original index is set to null and the chess piece is moved to the new location.
    isWhiteTurn is inverted so that it will be the other player's turn. The highlights are now hidden and selectedChessman is set to null.
    - The GetTileCenter function initializes the chess piece gameObject's 3d position given the index it should be in in the Chessmans array
    - The SpawnAllChessmans does nothing if it's not the master client. Otherwise, it spawns all the chessmans with SpawnChessman function.
    - The SpawnChessman instantitates the chesspieces given their name with PhotonNetwork.InstantiateSceneObject. The board is set as the parent, the Chessmans[x, y] is set as the gameObject's Chessman component and the position is set. Finally, the game object is added to activeChessman.


### How to change assets

- For the board 
  - Place it on the screen at 0, 0, 0 and scale to your satisfaction. All the other adjustments will compensate for changes in the size of the board. However, it's recommended to make the board the same size as the current one.
  - Attach the BoardHighlight and BoardManager script. For the BoardHighlight, attach the prefab which highlights the board when a piece is clicked which will show the available moves. For the BoardManager, attach a material which colors the piece if it's selected. The default is a red material.
  - Create a plane and add put it in a layer called ChessPlane or use the preexisting ChessPlane GameObject. Modify the box collider so that The plane just covers all the squares and nothing else.
  - Adjust RAYCAST_SCALE and RAYCAST_OFFSET in BoardManager.cs so that the places that are clicked correspond to the indices of the Chessmans array. (0,0) at one corner and (7, 7) at the other. This can only be achieved by logging the raycast value in UpdateSelection function.


- For the chess pieces 
  
  - Scale the pieces to the appropriate size(enough to fit in one square on the boared). 
  
  - Add the appropriate script to each of the pieces from the Pieces folder in the Game folder. For example, if it's the king, Add the King script to the prefab. Click on isWhite checkbox if it's white and don't if it's black
  - Add Photon View and Photon View Transform. Drag the Photon View Transform script into Photon View's observed components.  
  - the TILE_SIZE and TILE_OFFSET variables in BoardMangaer.cs needs to be modified when working with new assets as each asset has a different center position and different size. These variables need to be modified until all chess pieces appear in the right place on the chess board. 
  - Tag all the pieces with their names. The black king will be "Black King". The white queen will be "White Queen".
- For the highlight prefab
  - Scale the highlight prefab to the size of one of the squares in the chess board. If the chess board is the same size as the original no modifications are needed.
  - Add the prefab to the BoardHighlight script's highlightPrefab which should be a component on the board.
  - Modify TILE_SIZE and HIGHTLIGHT_OFFSET so that when you click on the chess piece in the game, the correct locations the chess piece can go is highlighted just right.
### Issues
- Can't castle
- Pawn promotion leads to exceptions being thrown


### References

The chess game is based on [This project](https://github.com/SacuL/3D-Chess-Unity/tree/master/Assets)

The multiplayer functionality is based on the Astroid Game demo of Photon 2

The chess game is modified so it can use new assets. 














