import * as React from 'react';
import {NavigationContainer, DefaultTheme} from '@react-navigation/native';
import {createStackNavigator} from '@react-navigation/stack';
import {createBottomTabNavigator} from '@react-navigation/bottom-tabs';
import AnimatedTabBar from '@gorhom/animated-tabbar';
import {Icon, Button} from 'native-base';
import Settings from '../screens/Settings';
import Game from '../screens/Game';
import {Host} from 'react-native-portalize';

const tabs = {
  Game: {
    icon: {
      component: <Icon name={'logo-game-controller-b'} />,
      activeColor: 'rgba(91,55,183,1)',
      inactiveColor: 'rgba(0,0,0,1)',
    },
    background: {
      activeColor: 'rgba(223,215,243,1)',
      inactiveColor: 'rgba(223,215,243,0)',
    },
  },
};

const Tab = createBottomTabNavigator();

function Home({navigation}) {
  React.useLayoutEffect(() => {
    navigation.setOptions({
      headerRight: () => (
        <Button transparent onPress={() => navigation.navigate('Settings')}>
          <Icon name="settings" style={{color: '#000'}} />
        </Button>
      ),
    });
  }, [navigation]);

  return (
    <Tab.Navigator
      backBehavior={'order'}
      tabBar={props => <AnimatedTabBar tabs={tabs} {...props} />}
      screenOptions={({route}) => ({
        title: route.name,
        headerStyle: {
          color: '#000',
        },
        headerTintColor: '#000',
      })}>
      <Tab.Screen name="Game" component={Game} />
    </Tab.Navigator>
  );
}

const Stack = createStackNavigator();

const theme = {
  ...DefaultTheme,
  colors: {...DefaultTheme.colors, background: 'transparent'},
};

export default function SignInStack() {
  return (
    <NavigationContainer theme={theme}>
      <Host>
        <Stack.Navigator headerMode={'none'}>
          <Stack.Screen name="Home" component={Home} />
          <Stack.Screen name="Settings" component={Settings} />
        </Stack.Navigator>
      </Host>
    </NavigationContainer>
  );
}
