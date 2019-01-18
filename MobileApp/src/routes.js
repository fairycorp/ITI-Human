import Home from './components/HomeScreen.vue';
import ChooseProducts from './components/ChooseProductsScreen.vue';
import Order from './components/OrderScreen.vue';
import Login from './components/Login.vue';

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
  {
    path: '/Login/',
    component: Login,
    name: 'login'
  },

]
