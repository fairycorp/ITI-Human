<template>
   <div id="app">
    <div>
      <div class="logo"></div>
      <div class="headline">
        Bonjour et bienvenue sur <span class="bold">ITI'Human</span>
      </div>
      <div class="text">
        Bénéficiez de l'ensemble de nos services en vous connectant.
      </div>

      <div class="sign-in">
        <UserInfoBox :authService=authService>
          <div slot="None">
              <el-button v-on:click="switchLoginBox()"> <i class="el-icon-arrow-right"></i> Se connecter...</el-button>
          </div>
        </UserInfoBox>
      </div>
      <div class="schemes" v-if="displayLoginBox">
          <SchemeDisplay :parameter=schemeDisplayParemeter> </SchemeDisplay>
      </div>
    </div>
    <!-- <div>
      <el-button v-on:click="logOut()"> Logout </el-button>
    </div> -->
  </div> 
</template>

<script lang="ts">
// import { Component, Vue } from "vue-property-decorator";
// import API from "@/services/API";
// import UserInfoBox from "@/components/auth-related/UserInfoBox.vue";
// import SchemeDisplay from "@/components/auth-related/SchemeDisplay.vue";
// import {
//   SchemeDisplayParameter,
//   SchemeDisplayReducedModeParameter
// } from "@/components/auth-related/SchemeDisplay.vue";

// import { appSettings } from "@/config/appSettings";
// import {
//   AuthService,
//   IAuthServiceConfiguration
// } from "@signature/webfrontauth";
// import ElementUI from "element-ui";
// import Axios from "axios";

// @Component({
//   components: {
//     SchemeDisplay,
//     UserInfoBox
//   }
// })
// export default class Authenticate extends Vue {
//   public authService: AuthService;
//   private schemeDisplayParemeter: SchemeDisplayParameter;
//   private schemeDisplayReducedModeParameter: SchemeDisplayReducedModeParameter;
//   private displayLoginBox: boolean = false;

//   constructor() {
//     super();
//     if (this.$slots.None) throw new Error("No slot provided");
//     const configuration: IAuthServiceConfiguration = appSettings;
//     this.authService = new AuthService(configuration, Axios);
//     this.authService.refresh(true, true, true);
//     this.schemeDisplayParemeter = new SchemeDisplayParameter(
//       this.authService,
//       "localhost:8081/assets/"
//     );
//     this.schemeDisplayReducedModeParameter = new SchemeDisplayReducedModeParameter(
//       this.authService,
//       "localhost:8081/assets/",
//       2,
//       () => {
//         console.log("ah");
//       }
//     );
//   }

//   private switchLoginBox(state: boolean) {
//     this.displayLoginBox = !this.displayLoginBox;
//   }

//   private async logOut() {
//     await this.authService.logout(true);
//     this.displayLoginBox = false;
//   }
// }
</script>

<style>
.logo {
  background-image: url("https://image.noelshack.com/fichiers/2018/47/5/1542987497-itih-logo.png");
  width: 152px;
  height: 152px;
  position: fixed;
  top: 50%; left: 50%;
  transform: translate(-50%, -170%);
  animation: fadein 0.5s;
}

.headline {
  position: fixed;
  top: 50%; left: 50%;
  transform: translate(-50%, -260%);
  font-size: 160%;
  color: #01b04b;
  font-weight: lighter;
  animation: fadein 0.5s;
}

.text {
  position: fixed;
  top: 50%; left: 50%;
  transform: translate(-50%, -200%);
  font-size: 120%;
  color: #5a5a5a;
  font-weight: lighter;
  animation: fadein 0.5s;
}

.sign-in {
  position: fixed;
  top: 50%; left: 50%;
  transform: translate(-50%, -20%);
  animation: fadein 0.5s;
}

.schemes {
  position: fixed;
  top: 50%; left: 50%;
  transform: translate(-50%, 40%);
}

@keyframes fadein {
  from { opacity: 0; }
  to   { opacity: 1; }
}
</style>
