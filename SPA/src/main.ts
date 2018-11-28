import Vue from "vue";
import Vuex, { Store } from "vuex";
import App from "./App.vue";
import router from "./router";
import "./plugins/element.js";

Vue.config.productionTip = false;

Vue.use(Vuex);

const store = new Vuex.Store({
  state: {
      authService: null
  },
  mutations: {
      change(state, newAuthService) {
          state.authService = newAuthService;
      }
  }
});

new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount("#app");
