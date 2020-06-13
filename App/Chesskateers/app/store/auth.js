const LOAD_USER = 'LOAD_USER';
const LOAD_USER_SUCCESS = 'LOAD_USER_SUCCESS';
const LOAD_USER_FAIL = 'LOAD_USER_FAIL';
const SET_FIREBASE_USER = 'SET_FIREBASE_USER';

// firebaseUser corresponds to the user object provided by firebase, whereas user is
// the user object stored in mongo
const initialState = {
  user: null,
  firebaseUser: null,
};

export default function auth(state = initialState, {type, payload}) {
  switch (type) {
    case LOAD_USER_SUCCESS:
      return {...state, user: payload.data.payload.user};

    case LOAD_USER_FAIL:
      return {...state, user: null};

    case SET_FIREBASE_USER:
      return {...state, firebaseUser: payload.firebaseUser};

    default:
      return state;
  }
}

export function loadUser() {
  return {
    type: LOAD_USER,
    payload: {
      request: {
        url: '/get-user',
        method: 'GET',
      },
    },
  };
}

export function setFirebaseUser(firebaseUser) {
  return {
    type: SET_FIREBASE_USER,
    payload: {
      request: {
        url: '/create-user',
        method: 'POST',
        data: {user_id: firebaseUser.uid, email: firebaseUser.email, name: firebaseUser.displayName, photoURL: firebaseUser.photoURL},
      },
    },
  };
}
