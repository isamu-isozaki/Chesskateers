import React, {useState, useRef, useEffect} from 'react';
import {StyleSheet} from 'react-native';
import auth from '@react-native-firebase/auth';
import * as Keychain from 'react-native-keychain';
import {
  Container,
  Toast,
  Button,
  Text,
  Item,
  Input,
  Thumbnail,
} from 'native-base';

export default function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const passwordInput = useRef(null);

  useEffect(() => {
    async function getCreds() {
      const credentials = await Keychain.getGenericPassword();
      setEmail(credentials.username);
      setPassword(credentials.password);
    }
    getCreds();
  }, []);

  function handleLoginException(e) {
    let message = '';
    switch (e.code) {
      case 'auth/weak-password':
        message = 'The given password is too weak.';
        break;
      case 'auth/wrong-password':
        message = 'The password is invalid.';
        break;
      default:
        message = e.message;
        break;
    }
    Toast.show({
      text: message,
      buttonText: 'Okay',
      style: {backgroundColor: 'red'},
    });
    console.log(e);
  }

  function areFieldsValid() {
    if (email.trim() === '') {
      handleLoginException({message: 'No email was given'});
      return false;
    }
    if (password.trim() === '') {
      handleLoginException({message: 'No password was given'});
      return false;
    }
    return true;
  }

  async function signIn() {
    if (areFieldsValid()) {
      try {
        await auth().signInWithEmailAndPassword(email, password);
        await Keychain.setGenericPassword(email, password);
      } catch (e) {
        await handleLoginException(e);
      }
    }
  }

  async function signUp() {
    if (areFieldsValid()) {
      try {
        await auth().createUserWithEmailAndPassword(email, password);
      } catch (e) {
        handleLoginException(e);
      }
    }
  }

  return (
    <Container style={styles.container}>
      <Thumbnail
        square
        large
        source={require('../assets/images/wolf-logo.png')}
      />
      <Text style={styles.title}>Wolf</Text>
      <Item rounded style={styles.input}>
        {/* <Icon active name="home" /> */}
        <Input
          style={styles.inputText}
          placeholder="Email..."
          onChangeText={text => setEmail(text)}
          value={email}
          autoCompleteType={'email'}
          keyboardType={'email-address'}
          textContentType={'emailAddress'}
          onSubmitEditing={() => passwordInput.current._root.focus()}
          returnKeyType={'next'}
          autoCapitalize={'none'}
          autoCorrect={false}
        />
      </Item>
      <Item rounded style={styles.input}>
        {/* <Icon active name="home" /> */}
        <Input
          style={styles.inputText}
          placeholder="Password..."
          onChangeText={text => setPassword(text)}
          value={password}
          secureTextEntry={true}
          textContentType={'password'}
          onSubmitEditing={signIn}
          ref={passwordInput}
          returnKeyType={'go'}
          autoCapitalize={'none'}
          autoCorrect={false}
        />
      </Item>
      <Button rounded style={styles.button} onPress={signIn}>
        <Text style={styles.buttonText}>Login</Text>
      </Button>
      <Button rounded style={styles.button} onPress={signUp}>
        <Text style={styles.buttonText}>Sign Up</Text>
      </Button>
    </Container>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: 'white',
  },
  title: {
    marginTop: 20,
    marginBottom: 30,
    fontSize: 28,
    fontWeight: '500',
    color: '#5c676d',
  },
  input: {
    flexDirection: 'row',
    borderRadius: 30,
    marginTop: 10,
    marginBottom: 10,
    width: 300,
    height: 60,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#dcdcdc',
  },
  inputText: {
    color: '#333b3e',
    fontSize: 15,
    marginRight: 5,
    marginLeft: 10,
  },
  button: {
    flexDirection: 'row',
    borderRadius: 30,
    marginTop: 10,
    marginBottom: 10,
    width: 200,
    height: 50,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#333b3e',
  },
  buttonText: {
    color: '#dcdcdc',
    fontSize: 20,
    marginRight: 5,
  },
});
