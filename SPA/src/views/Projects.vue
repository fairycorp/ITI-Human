<template>
    <div class="left-page">
        <h1>Liste de vos projets</h1>
        <div class="cover-photo"></div>

        <div class="content">
            <div v-if="displayedProject !== null && displayedProject !== undefined">
                <h3 class="pretitle">À L'AFFICHE</h3>
                <div class="project-title">{{ displayedProject.projectName }}</div>

                <div class="memberlist">
                    <div v-for="(member, index) in displayedProject.members" :key="member.projectMemberId" class="member">
                        <UserTooltip class="member" :userId="member.userId" />
                        <span v-if="index < displayedProject.members.length - 1">, </span><span v-else>.</span>
                    </div>
                </div>

                <div class="medium-top-margin headline">
                    <div class="subtitle"><span class="openSans-bold">SLOGAN</span> DU PROJET</div>
                    {{ displayedProject.projectHeadline }}
                </div>

                <div class="medium-top-margin pitch">
                    <div class="subtitle"><span class="openSans-bold">PITCH</span> DU PROJET</div>
                    {{ displayedProject.projectPitch }}
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
import UserTooltip from "@/components/UserTooltip.vue";

import { IBasicDataProject } from "@/models/model.Project";

@Component({
    components: {
        UserTooltip
    }
})
export default class Projects extends Vue {
    @Prop() private authService!: AuthService;
    @Prop() private displayedProject!: IBasicDataProject;
    private userProjects: IBasicDataProject[] = [];

    constructor() {
        super();
        this.isAccessible();
        this.fetchUserProjects();
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

    // GETTERS METHODS.
    private async fetchUserProjects() {
        const response = await API
            .get(`${Endpoint.Project}/u/3`);
        this.userProjects = response.data;

        this.displayedProject = this.userProjects[0];
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

.content {
    position: absolute;
    top: 420px;
}

.pretitle {
    
    color: #808080;
}

.project-title {
    font-family: "gotham-bold";
    font-size: 200%;
    margin-top: -20px;
}

.member {
    display: inline;
}

.subtitle {
    font-size: 120%;
    color: #707070;
}
</style>