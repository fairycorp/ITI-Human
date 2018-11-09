<template>
  <app-navigation></app-navigation>
</template>

<script>

import { StackNavigator } from "vue-native-router"
import homeScreen from './HomeScreen'
import anotherScreen from './AnotherScreen'

const AppNavigation = StackNavigator(
  {

    Homescreen: homeScreen,
    Anotherscreen: anotherScreen 

  },
  {
    initialRouteName: 'Homescreen',
  }
);

export default {
    components: { AppNavigation }
}
</script>