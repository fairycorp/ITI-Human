<template>
    <f7-page>

        <f7-button
          @click="GetLogin()">
          HEYYYYYYYYYYYYYYY
        </f7-button>
    </f7-page>
</template>

<script>
import {
    AuthService,
    AuthLevel
} from "@signature/webfrontauth";
import Axios from "axios";

export default {
    data() {
      return {
        authService: null
      }
    },

  created: function () {
      const config = {
        hostname: "10.8.110.166",
        port: 5000,
        disableSsl: true
      };
      this.authService = new AuthService(config, Axios);
      this.authService.refresh(true, true, true);
      console.log(this.authService);
  },

  methods: {
    GetLogin(){
      this.$f7router.navigate({ name: 'login' });
    },
  }
}           
</script>

<style>

</style>