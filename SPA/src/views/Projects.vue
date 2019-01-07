<template>
    <div class="left-page">
        <h1>Liste de vos projets</h1>
        <div class="cover-photo"></div>
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
export default class Projects extends Vue {
    @Prop() private authService!: AuthService;

    constructor() {
        super();
        this.isAccessible();
    }

    // DATACHECKING METHODS.
    /** Checks if the current view route can be accessed. */
    private isAccessible() {
        if (Vue.prototype.$authService === undefined) {
            this.$router.push("/");
        } else {
            this.authService = Vue.prototype.$authService;
            this.authService.refresh(true, true, true);

            if (this.authService.authenticationInfo.level === 0) {
                this.$router.push("/");
            }
        }
    }
}
</script>

<style lang="scss">
.cover-photo {
    position: absolute;
    top: 160px;
    left: 0px;
    width: 846px;
    height: 247px;
    background-image: url("../assets/images/projects-cover.png");
}
</style>