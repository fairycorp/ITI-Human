<template>
    <div class="global-content">
        <div v-if="userInfo !== null && userInfo !== undefined" class="id-card">
            <div @click="logout()" class="logout"></div>
            <div v-if="userInfo.avatarUrl != null">
                <img :src="userInfo.avatarUrl" width="100" class="picture" />
            </div>
            <div v-else>
                <img src="" />
            </div>
            <div class="fullname">
                {{ userInfo.firstName }} <span class="openSans-bold">{{ userInfo.lastName }}</span>
            </div>
            <div class="username">
                {{ userInfo.userName }}
            </div>
        </div>

        <div v-if="accessibility">
            <div @click="changeRoute('projects')" class="menu projects">
                <div class="illustration-projects"></div>
                <div class="main-menu-title">
                    PROJETS
                </div>
                <div class="actiontypes">
                    CREER/CONSULTER
                </div>
            </div>

            <div class="menu profile">
                <div class="illustration-profile"></div>
                <div class="main-menu-title">
                    PROFIL
                </div>
                <div class="actiontypes">
                    MODIFIER
                </div>
            </div>

            <div class="menu order">
                <div class="illustration-order"></div>
                <div class="main-menu-title">
                    J'AI FAIM
                </div>
                <div class="actiontypes">
                    COMMANDER
                </div>
            </div>
        </div>
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
import { IDetailedDataUser } from "@/models/model.User";

@Component({})
export default class Landing extends Vue {
    @Prop() private authService!: AuthService;
    @Prop() private userInfo!: IDetailedDataUser;
    private accessibility: boolean = false;

    constructor() {
        super();
        this.isAccessible();
        this.getUserInfo();
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
        if (!response.data as boolean) return this.$router.push("/profile/setup");
        this.accessibility = true;
    }

    // GETTERS METHODS.
    private async getUserInfo() {
        const response = await API
            .get(`${Endpoint.User}/tooltip/${this.authService.authenticationInfo.user.userId}`);

        this.userInfo = response.data;
    }

    private async logout() {
        await this.authService.logout(true);
    }

    // ROUTING METHODS.
    private changeRoute(route: string) {
        this.$router.push(route);
    }
}
</script>

<style lang="scss">
.id-card {
    position: absolute;
    top: 0;
    left: 0;
    margin: 40px;
    width: 21%;
    height: 13%;
    background-color: white;
}

.picture {
    margin-top: 15px;
    margin-left: 25px;
    border-radius: 50%;
}

.fullname {
    position: absolute;
    font-size: 220%;
    top: 20%;
    left: 37%;
}

.username {
    position: absolute;
    font-size: 140%;
    top: 50%;
    left: 37%;
    color: grey;
}

.logout {
    position: absolute;
    top: 0;
    right: 0;
    width: 28px;
    height: 21px;
    margin: 10px;
    background-image: url("../assets/images/logout.png");
    cursor: pointer;
}

.menu {
    position: absolute;
    bottom: 36%;
    width: 595px;
    height: 460px;
    background-color: white;
    color: black;
    cursor: pointer;

    transition-property: background-color, color;
    transition-duration: 0.2s;
}

.menu:hover {
    background-color: black;
    color: white;
}

.projects {
    position: absolute;
    left: 2%;
}

.illustration-projects {
    width: 595px;
    height: 355px;
    background-image: url("../assets/images/projects-menu.png")
}

.profile {
    position: absolute;
    left: 34.5%;
}

.illustration-profile {
    width: 595px;
    height: 355px;
    background-image: url("../assets/images/profile-menu.png")
}

.order {
    position: absolute;
    left: 67%;
}

.illustration-order {
    width: 595px;
    height: 355px;
    background-image: url("../assets/images/order-menu.png")
}

.main-menu-title {
    margin-top: 15px;
    margin-left: 15px;
    font-family: "gotham-bold";
    font-size: 210%;
    letter-spacing: 6px;
}

.actiontypes {
    margin-left: 15px;
    font-family: "gotham-bold";
    font-size: 140%;
    letter-spacing: 2px;
    color: grey;
}
</style>
