<template>
  <app-navigation></app-navigation>
</template>

<script>

import { StackNavigator } from "vue-native-router"
import homeScreen from './src/components/HomeScreen'
import anotherScreen from './src/components/AnotherScreen'
import orderScreen from './src/components/OrderScreen'
//import Loginscreen from './src/components/Login'

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