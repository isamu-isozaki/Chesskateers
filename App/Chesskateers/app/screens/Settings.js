import React, {useContext, useLayoutEffect} from 'react';
import {StyleSheet} from 'react-native';
import {Container, Button, Text, Content, Icon} from 'native-base';
import auth from '@react-native-firebase/auth';
import * as Keychain from 'react-native-keychain';

import {AuthContext} from '../navigation/AuthNavigator';

export default function Settings({navigation}) {
  const user = useContext(AuthContext);

  async function logOut() {
    try {
      await auth().signOut();
      await Keychain.resetGenericPassword();
    } catch (e) {
      console.error(e);
    }
  }

  useLayoutEffect(() => {
    navigation.setOptions({
      headerRight: () => (
        <Button transparent onPress={logOut}>
          <Icon name="log-out" style={{color: '#000'}} />
        </Button>
      ),
      headerLeft: () =>
        navigation.canGoBack() ? (
          <Button transparent onPress={navigation.goBack}>
            <Icon name="arrow-back" style={{color: '#000'}} />
          </Button>
        ) : (
          undefined
        ),
    });
  }, [navigation]);

  return (
    <Container>
      <Content contentContainerStyle={styles.container}>
        <Text style={styles.title}>Welcome {user.uid}</Text>
      </Content>
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
  button: {
    flexDirection: 'row',
    borderRadius: 30,
    marginTop: 10,
    marginBottom: 10,
    width: 160,
    height: 60,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#333b3e',
  },
  buttonText: {
    color: '#dcdcdc',
    fontSize: 24,
    marginRight: 5,
  },
});
