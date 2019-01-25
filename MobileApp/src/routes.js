import Home from './components/HomeScreen.vue';
import ChooseProducts from './components/ChooseProducts.vue';
import Order from './components/order.vue';
import Login from './components/Login.vue';
import About from './components/About.vue';
import Profile from './components/Profile.vue';
import ChooseProject from './components/ChooseProject.vue';

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
  {
    path: '/Profile/',
    component: Profile,
    name: 'profile'
  },
  {
    path: '/ChooseProject/',
    component: ChooseProject,
    name: 'chooseProject'
  },    

]
