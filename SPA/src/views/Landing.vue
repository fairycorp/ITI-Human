<template>
    <div class="global-content">
        <button @click="logout()">Logout</button>
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

@Component({})
export default class Landing extends Vue {
    @Prop() private authService!: AuthService;

    constructor() {
        super();
        console.log(Vue.prototype.$authService);
        if (Vue.prototype.$authService !== undefined) {
            this.authService = Vue.prototype.$authService;
            this.authService.refresh(true, true, true);
        }
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
    // ROUTE CHECKING METHODS.
    /** Checks if the current view route can be accessed. */
    private isAccessible() {
        if (Vue.prototype.$authService === undefined) {
            this.$router.push("/");
        } else {
            if (this.authService.authenticationInfo.level === 0) {
                this.$router.push("/");
            }
        }
    }

    private async logout() {
        await this.authService.logout(true);
    }
}
</script>

<style lang="scss">

</style>
