import Vue from "vue";
import Router from "vue-router";

// Components.
import NotFound from "./components/NotFound.vue";
import Home from "./components/Home.vue";
import OrderStaffPanel from "./components/order-related/OrderStaffPanel.vue";

Vue.use(Router);

export default new Router({
  routes: [
    {
      path: "*",
      name: "notFound",
      component: NotFound,
    },

    {
      path: "/",
      name: "home",
      component: Home,
    },

    {
      path: "/order-staff",
      name: "orderStaffPanel",
      component: OrderStaffPanel,
    },
  ],
});
