import Vue from 'vue';
import Router from 'vue-router';
import OrderPanel from './components/OrderPanel.vue';

Vue.use(Router);

export default new Router({
  routes: [
    {
      path: '/',
      name: 'orderPanel',
      component: OrderPanel,
    },
  ],
});
