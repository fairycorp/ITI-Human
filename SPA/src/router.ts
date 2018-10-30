import Vue from 'vue';
import Router from 'vue-router';

// Components.
import Home from './components/Home.vue';
import OrderPanel from './components/order-related/OLD-OrderStaffPanel.vue';

Vue.use(Router);

export default new Router({
  routes: [
    {
      path: '/',
      name: 'home',
      component: Home,
    },

    {
      path: '/order',
      name: 'orderPanel',
      component: OrderPanel,
    },
  ],
});
