import Home from './components/HomeScreen.vue';
import ChooseProducts from './components/ChooseProductsScreen.vue';
import Order from './components/OrderScreen.vue';
import Login from './components/Login.vue';
import About from './components/About.vue';

export default [
  {
    path: '/',
    component: Login,
    name: 'login'
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
    path: '/Home/',
    component: Home,
    name: 'home'
  },
  {
    path: '/About/',
    component: About,
    name: 'about'
  },

]
