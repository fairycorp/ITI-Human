import Home from './components/HomeScreen.vue';
import ChooseProducts from './components/ChooseProductsScreen.vue';
import Order from './components/OrderScreen.vue';

export default [
  {
    path: '/',
    component: Home,
    name: 'home'
  },
  {
    path: '/Order/',
    component: Order,
    name: 'order'
  },
  {
    path: '/ChooseProducts/',
    component: ChooseProducts,
    name: 'chooseproducts'
  },

]
