<template>
    <f7-page>
        <f7-button
          @click="startGithub()">
          Start GitHub
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

  watch: {
    authService: function(newValue, oldValue) {
      this.isAccessible();
    }
  },

  methods: {

    async startGithub() {
        return await this.authService.startPopupLogin("GitHub");
    },

    async isAccessible() {
      await this.authService.refresh(true, true, true);
      if (this.authService != null) {
        if (this.authService.authenticationInfo.level == 0) {
          this.$f7router.navigate({ name: 'home' });
        }
      }
    },
  }
}
</script>

<style>

</style>