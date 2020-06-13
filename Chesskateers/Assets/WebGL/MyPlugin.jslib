mergeInto(LibraryManager.library, {

  // Create a new function with the same name as
  // the event listeners name and make sure the
  // parameters match as well.
  move: function(fromTo) {
    ReactUnityWebGL.move(Pointer_stringify(fromTo));
  },
  gameOver: function(side) {
    ReactUnityWebGL.gameOver(side);
  },
  playCard: function(cardInfo) {
    ReactUnityWebGL.playCard(Pointer_stringify(cardInfo));
  }
});