<template>
    <div class="global-content">
        <button @click="logout()">logout</button>
    </div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";
import { appSettings } from "@/config/appSettings";
import {
  AuthService,
  IAuthServiceConfiguration,
AuthLevel
} from "@signature/webfrontauth";
import Axios from "axios";
import API from "@/services/API";
import Endpoint from "@/helpers/Endpoint";

@Component({})
export default class Landing extends Vue {
    @Prop() private authService!: AuthService;

    constructor() {
        super();
        this.isAccessible();
    }
    /** Watches authService instance information. */
    @Watch("authService.authenticationInfo.level")
    private onAuthLevelChange() {
        if (this.authService.authenticationInfo.level === 0) {
            Vue.prototype.$authService = this.authService;
            this.$router.push("/");
        }
    }
    // DATACHECKING METHODS.
    /** Checks if the current view route can be accessed. */
    private isAccessible() {
        //TODO: TEMPORARY

        if (Vue.prototype.$authService === undefined) {
            this.$router.push("/");
        } else {
            this.authService = Vue.prototype.$authService;
            this.authService.refresh(true, true, true);
            this.checkUserProfileSetupLevel();

            if (this.authService.authenticationInfo.level === 0) {
                this.$router.push("/");
            }
        }
    }

    /** Checks if user needs to setup his profile. */
    private async checkUserProfileSetupLevel() {
        const response = await API.get(`${Endpoint.User}/setup/${this.authService.authenticationInfo.user.userId}`);
        if (!response.data as boolean) this.$router.push("/firstime");
    }

    private async logout() {
        await this.authService.logout(true);
    }
}
</script>

<style lang="scss">

</style>
