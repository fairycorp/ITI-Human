import Vue from "vue";
import Router from "vue-router";

// Views.
import Homepage from "@/views/Homepage.vue";
import Landing from "@/views/Landing.vue";
import ProfileSetup from "@/views/ProfileSetup.vue";
import ProjectsDB from "@/views/ProjectsDB.vue";
import ForkChefs from "@/views/ForkChefs.vue";
import Order from "@/views/Order.vue";

Vue.use(Router);

export default new Router({
  routes: [
    {
      path: "/",
      name: "homepage",
      component: Homepage
    },

    {
      path: "/landing",
      name: "landing",
      component: Landing
    },

    {
      path: "/profile/setup",
      name: "profileSetup",
      component: ProfileSetup
    },

    {
      path: "/projects",
      name: "projects",
      component: ProjectsDB
    },

    {
      path: "/hungry",
      name: "forkChefs",
      component: ForkChefs
    },

    {
      path: "/order/:id",
      name: "order",
      component: Order,
      props: true
    }
  ],
});
