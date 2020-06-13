import React, {Component, useEffect, useContext} from 'react';
import {UnityModule, UnityView} from 'react-native-unity-view';
import {View, StyleSheet} from 'react-native';
import {socket} from '../socket';
import {AuthContext} from '../navigation/AuthNavigator';

// TODO:
// 1. Add screen to add friends
// 2. Add screen to talk with friends
// 3. Add option to click on friend or search for opponents
// How to do 3? Make search bar, make it contain a flatlist of all
// players with keyword in display name. 
// run multiple react-native emulators. login to one with lets and one with isa and play a game.


function Game() {
  const onUnityMessage = (handler) => {
    //handler= {"data": {"fromX": 4, "fromY": 1, "toX": 4, "toY": 3}, "id": 1, "name": "move", "seq": ""}
    console.log(`${handler.name}`);
    console.log(handler.data);
    if (handler.name === 'move') {
      socket.emit('move request', handler.data);
    }
  };
  socket.on('move', (fromTo) => {
    fromTo.name = 'move';
    UnityModule.postMessageToUnityManager(fromTo.stringify());
  });
  socket.on('side', (side) => {
    side.name = 'set side';
    UnityModule.postMessageToUnityManager(side.stringify());
  });
  return (
    <View style={styles.root}>
      {UnityModule.isReady() ? (
        <UnityView
          onUnityMessage={onUnityMessage.bind(this)}
          style={styles.unity}
        />
      ) : (
        <View />
      )}
    </View>
  );
}

export default Game;
//export default connect(mapStateToProps)(Game);
const styles = StyleSheet.create({
  game: {
    flex: 1,
    padding: 30,
    justifyContent: 'center',
  },
  root: {
    flex: 1,
    padding: 30,
    justifyContent: 'center',
  },
  text: {
    height: 40,
    alignContent: 'center',
    textAlign: 'center',
    fontSize: 20,
    marginBottom: 15,
  },
  unity: {
    flex: 1,
  },
});
