import {createStore, applyMiddleware} from 'redux';
import axiosMiddleware from 'redux-axios-middleware';
import rootReducer from './reducers';
import axios from 'axios';
import {API_URL} from '../config';

const api = axios.create({
  baseURL: API_URL,
  responseType: 'json',
});

const store = createStore(rootReducer, applyMiddleware(axiosMiddleware(api)));

api.interceptors.request.use(
  async function(config) {
    const {firebaseUser} = store.getState().auth;
    if (firebaseUser) {
      const idToken = await firebaseUser.getIdToken();
      config.headers.authorization = 'Bearer ' + idToken;
    }
    return config;
  },
  function(error) {
    return Promise.reject(error);
  },
);

export default store;
