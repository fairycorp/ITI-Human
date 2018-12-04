<template>
  <f7-page>
    <f7-navbar title="Form components" back-link="Back"></f7-navbar>
    <f7-list>
      <f7-list-item v-for="o in options" :key="o" checkbox :value="o" :title="o" @change="test"></f7-list-item>
    </f7-list>
    <p>test - {{ text }}</p>
  </f7-page>
</template>
<script>
  import { f7Navbar, f7Page, f7BlockTitle } from 'framework7-vue';

  export default {
    data() {
      return {
        options: ['choose', 'make', 'create'],
        text: process.env.BACKEND_URL
      }
    },

    methods: {
      async test(e) {
        this.text = JSON.stringify(e);
        await new Promise(f => setTimeout(f, 2000));
        this.text = '';
      }
    },

    components: {
      f7Navbar,
      f7Page,
      f7BlockTitle,
    },
  };
</script>
