import React, {useState, useEffect, createContext} from 'react';
import auth from '@react-native-firebase/auth';
import SignInStack from './SignInStack';
import AuthScreen from '../screens/AuthScreen';

// import SignOutStack from './SignOutStack';
import store from '../store';
import {setFirebaseUser, loadUser} from '../store/auth';
import {socket} from '../socket';

export const AuthContext = createContext(null);

export default function AuthNavigator() {
  const [initializing, setInitializing] = useState(true);
  const [user, setUser] = useState(null);

  // Handle user state changes
  // eslint-disable-next-line react-hooks/exhaustive-deps
  function onAuthStateChanged(result) {
    if (result !== null) {
      store.dispatch(setFirebaseUser(result));
      socket.emit("connect player", result.uid);
    }
    setUser(result);
    if (initializing) {
      setInitializing(false);
    }
  }

  useEffect(() => {
    const authSubscriber = auth().onAuthStateChanged(onAuthStateChanged);

    // unsubscribe on unmount
    return authSubscriber;
  }, [initializing, onAuthStateChanged]);

  if (initializing) {
    return null;
  }

  return user ? (
    <AuthContext.Provider value={user}>
      <SignInStack />
    </AuthContext.Provider>
  ) : (
    <AuthScreen />
  );
}
