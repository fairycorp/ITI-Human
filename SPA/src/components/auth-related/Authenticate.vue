<template>
   <div id="app">
    <div>
        <UserInfoBox :authService=authService>
            <div slot="None">
                <el-button v-on:click="switchLoginBox()"> Login </el-button>
            </div>
            <div slot="Unsafe">
                Hello {{authService.authenticationInfo.user.userName}}
                <a v-on:click="logOut()">I'm not {{authService.authenticationInfo.unsafeUser.userName}}.</a>
                <SchemeDisplay :parameter=schemeDisplayReducedModeParameter> </SchemeDisplay>
                ----------------------------------
            </div>
            <div slot="Normal"> 
              <el-button v-on:click="authService.logout()"> Logout </el-button>
              normal </div>
            <div slot="Critical"> critical</div>
        </UserInfoBox>
        <div v-if="displayLoginBox">
            <SchemeDisplay :parameter=schemeDisplayParemeter> </SchemeDisplay>
        </div>
    </div>
    
    <router-view />
  </div> 
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import API from "@/services/API";
import UserInfoBox from "@/components/auth-related/UserInfoBox.vue";
import SchemeDisplay from "@/components/auth-related/SchemeDisplay.vue";
import {
  SchemeDisplayParameter,
  SchemeDisplayReducedModeParameter
} from "@/components/auth-related/SchemeDisplay.vue";

import { appSettings } from "@/config/appSettings";
import {
  AuthService,
  IAuthServiceConfiguration
} from "@signature/webfrontauth";
import ElementUI from "element-ui";
import Axios from "axios";

@Component({
  components: {
    SchemeDisplay,
    UserInfoBox
  }
})
export default class Authenticate extends Vue {
  public authService: AuthService;
  private schemeDisplayParemeter: SchemeDisplayParameter;
  private schemeDisplayReducedModeParameter: SchemeDisplayReducedModeParameter;
  private displayLoginBox: boolean = true;

  constructor() {
    super();
    if (this.$slots.None) throw new Error("No slot provided");
    const configuration: IAuthServiceConfiguration = appSettings;
    this.authService = new AuthService(configuration, Axios);
    this.authService.refresh(true, true, true);
    this.schemeDisplayParemeter = new SchemeDisplayParameter(
      this.authService,
      "localhost:8081/assets/"
    );
    this.schemeDisplayReducedModeParameter = new SchemeDisplayReducedModeParameter(
      this.authService,
      "localhost:8081/assets/",
      2,
      () => {
        console.log("ah");
      }
    );
  }

  private switchLoginBox(state: boolean) {
    this.displayLoginBox = !this.displayLoginBox;
    console.log("yaya" + this.displayLoginBox);
  }

  private async logOut() {
    await this.authService.logout(true);
    this.displayLoginBox = false;
  }
}
</script>

<style>

</style>
