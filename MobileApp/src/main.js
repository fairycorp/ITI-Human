// Import Vue
import Vue from 'vue';

// Import Framework7
import Framework7 from 'framework7/framework7.esm.bundle.js';

// Import Framework7 Vue
import Framework7Vue from 'framework7-vue/framework7-vue.esm.bundle.js';

// Import F7 Style
import Framework7CSS from 'framework7/css/framework7.css';

// Import F7 iOS Icons
import Framework7Icons from 'framework7-icons/css/framework7-icons.css';

// Import Material Icons
import MaterialIcons from 'material-design-icons/iconfont/material-icons.css';

// Import Fontawesome Theme Styles
import FontAwesome from 'font-awesome/css/font-awesome.css';

// Import App Custom Styles
// import AppStyles from './assets/sass/main.scss'

// Import App Component
import app from './main.vue';

import { initialize } from "./helpers/AuthServiceHelp.js";

// Different F7-Vue plugin initialization with f7 v3.0
Framework7.use(Framework7Vue)
initialize().then( () => new Vue({
  // Root Element
  el: '#app',
  render: c => c('app'),
  components: {
    app
  }
}).$mount('#app') );