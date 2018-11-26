import Vue from "vue";
import Router from "vue-router";

// Components.
import NotFound from "@/components/NotFound.vue";
import Home from "@/components/Home.vue";
import Authenticate from "@/components/auth-related/Authenticate.vue";
import StaffDashboard from "@/components/Staff/StaffDashboard.vue";

Vue.use(Router);

export default new Router({
  routes: [

    {
      path: "/",
      name: "authenticate",
      component: Authenticate,
    },

    {
      path: "*",
      name: "notFound",
      component: NotFound,
    },

    {
      path: "/home",
      name: "home",
      component: Home,
    },

    {
      path: "/staff-dashboard",
      name: "staffDashboard",
      component: StaffDashboard,
    },
  ],
});
