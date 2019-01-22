<template>
    <f7-page>
      <div
          class="default-logo">
      </div>
      <f7-list class="list" inline-labels no-hairlines-md>
        <h2 class="h2"> Identifiez-vous  </h2>
          <f7-list-input
            class="name"
            label="Name"           
            :value="Name"
            @input="Name = $event.target.value"
            type="text"
            placeholder="Your name"
            clear-button
          >
          </f7-list-input>

          <f7-list-input
            class="password"
            label="Password"
            :value="Password"
            @input="Password = $event.target.value"
            type="password"
            placeholder="Your password"
            clear-button
          >
          </f7-list-input>

          <f7-button class="connectingButton unselectable-text col button color-white"
            @click="Connecting()"> 
            Se connecter
          </f7-button>
      </f7-list>  
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
        Password: "",
        Name: "",
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
    await this.isAccessible();
  },

  watch: {
    authService: function(newValue, oldValue) {
      this.isAccessible();
    }
  },

  methods: {
    async Connecting() {
        await this.authService.basicLogin(this.Name, this.Password);
        await this.isAccessible();
    },

    async isAccessible() {
      await this.authService.refresh(true, true, true);
      if (this.authService != null) {
        if (this.authService.authenticationInfo.level > 0) {
          this.$f7router.navigate({ name: 'home' });
        }
      }
    },
  }
}
</script>

<style lang="scss">

.default-logo {
    position: absolute;
    top: 50px;
    left: 35%;
    width: 126px;
    height: 135px;
    background-image: url("../../../SPA/src/assets/images/logo-small.png");
}

.list .h2{
    position: absolute;
    font-family: "gotham-bold";
    left: 25%;
    bottom: 85px;
    color: gray;
}

.list {
    position: absolute;
    top: 215px;
    left: 5%;
}

.list .name {
    background-color: white;
}

.list .password {
    background-color: white;
}

.connectingButton{
    position: absolute;
    left: 30%;
}

</style>