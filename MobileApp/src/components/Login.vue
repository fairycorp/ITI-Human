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
    IAuthServiceConfiguration,
    AuthLevel
} from "@signature/webfrontauth";
import Axios from "axios";

export default {
    data() {
      return {
        authService: null
      }
    },

  async created() {
    const config = {
      identityEndPoint: {
        hostname: "192.168.1.31",
        port: 5000,
        disableSsl: true
      }
    };
    this.authService = new AuthService(config, Axios);
    //await this.isAccessible();
  },

  methods: {
    async isAccessible() {
      await this.authService.refresh(true, true, true);
      if (this.authService != null) {
        if (this.authService.authenticationInfo.level == 0) {
          this.$f7router.navigate({ name: 'home' });
        }
      }
    },

    GetLogin(){
      this.$f7router.navigate({ name: 'login' });
    },
  }
}
</script>

<style>

</style>