<template>
  <app-navigation></app-navigation>
</template>

<script>

import { StackNavigator } from "vue-native-router"
import homeScreen from './HomeScreen'
import anotherScreen from './AnotherScreen'
import orderScreen from './OrderScreen'
//import Loginscreen from './Login'

const AppNavigation = StackNavigator(
  {

    Homescreen: homeScreen,
    Anotherscreen: anotherScreen, 
    Orderscreen: orderScreen,
    //Login: Loginscreen


  },
  {
    initialRouteName: 'Homescreen',
  }
);

export default {
    components: { AppNavigation }
}
</script>