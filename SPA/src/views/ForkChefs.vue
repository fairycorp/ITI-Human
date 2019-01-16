<template>
    <div class="global-content">
        <div @click="changeRoute('landing')" class="return-arrow"><img width="20" src="../assets/images/return-arrow.png" /></div>
        <div :class="{ invisible : WINDOW_CARDS }" class="left-page">
            <div class="fork-chefs-logo"></div>
            <div class="main-button">
                <button @click="launchCardsWindow()" class="big">LEURS CARTES</button>
            </div>
            <div class="text-at-bottom">
                “ La grande cuisine n’est pas faite pour les timorés. Il faut avoir de l’imagination, de l’audace.
                Il faut prendre le risque de commettre des erreurs et personne n’a le droit de vous imposer des limites,
                quelles que soient vos origines, d’où que vous veniez. Votre seule limite, c’est votre âme. C’est la vérité,
                tout le monde peut cuisiner, mais le véritable génie, n’appartient qu’aux audacieux. ”
            </div>
            <div class="signature">Chef Gusteau</div>
        </div>
            
        <div v-if="WINDOW_CARDS">
            <div
                v-for="(stall, index) in stallList"
                @click="goToRestaurant(stall.storageId)"
                :key="stall.projectId"
                :class="{ highmargintop : index > 0 }"
                class="little-right-page chef-button">
                <div class="chef-text">Chef</div>
                <div class="project-title">{{ stall.projectName }}</div>
                <div class="votes">
                    <img
                        src="../assets/images/toque.png"
                        :class="{ transparent : stall.average < 5 || stall.average == 0 }"
                        class="heavylight-right-margin" />
                    <img
                        src="../assets/images/toque.png"
                        :class="{ transparent : stall.average < 4 || stall.average == 0 }"
                        class="heavylight-right-margin" />
                    <img
                        src="../assets/images/toque.png"
                        :class="{ transparent : stall.average < 3 || stall.average == 0 }"
                        class="heavylight-right-margin" />
                    <img
                        src="../assets/images/toque.png"
                        :class="{ transparent : stall.average < 2 || stall.average == 0 }"
                        class="heavylight-right-margin" />
                    <img
                        src="../assets/images/toque.png"
                        :class="{ transparent : stall.average < 1 || stall.average == 0 }"
                        class="heavylight-right-margin" />
                </div>
                <div class="thanks"></div>
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
import { IBasicDataProject } from "@/models/model.Project";

@Component({})
export default class ForkChefs extends Vue {
    @Prop() private authService!: AuthService;
    @Prop() private stallList: IBasicDataProject[] = [];
    private WINDOW_CARDS: boolean = false;

    constructor() {
        super();
        this.fetchProjectList();
        this.isAccessible();
    }

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
    private async fetchProjectList() {
        const response = await API.get(`${Endpoint.Project}`);
        if (response.data) {
            const projectList: IBasicDataProject[] = response.data;
            
            projectList.forEach( (project) => {
                if (project.openedStall) {
                    this.stallList.push(project);
                }
            });

            this.stallList.forEach( (stall) => {
                stall.average = 0;
                let average = 0;
                stall.votes.forEach( (vote) => {
                    average += vote;
                });
                if (average > 0) stall.average = average / stall.votes.length;
            });
        }
    }

    // ROUTING METHODS.
    private changeRoute(route: string) {
        this.$router.push(route);
    }

    // WINDOW METHODS.
    private launchCardsWindow() {
        if (this.stallList.length > 0) {
            this.WINDOW_CARDS = true;
        }
    }

    private goToRestaurant(identifier: string) {
        this.changeRoute(`order/${identifier}`);
    }
}
</script>

<style lang="scss">
.transparent {
    opacity: 0.2;
}

.extremelyhighmargintop {
    margin-top: 200px;
}

.fork-chefs-logo {
    position: absolute;
    top: 27%;
    left: 37%;
    width: 232px;
    height: 206px;
    background-image: url("../assets/images/fork-chefs.png");
}

.text-at-bottom {
    position: absolute;
    top: 65%;
    width: 83%;
    font-family: "Gloria";
    font-size: 120%;
    color: #ababab;
    text-align: justify;
}

.signature {
    position: absolute;
    top: 87%;
    left: 70%;
    width: 83%;
    font-family: "Gloria";
    font-size: 140%;
    color: #4e4e4e;
}

.main-button {
    position: absolute;
    top: 51%;
    left: 39.5%;
}

button.big {
    outline-width: 0;
    width: 186px;
    height: 55px;
    border-radius: 45px;
    background-color: white;
    border: 1px solid black;
    font-family: "gotham-bold";
    color: black;
    letter-spacing: 2px;
    
    transition-duration: 0.2s;
    transition-property: background-color, color;
}

button.big:hover {
    background-color: black;
    color: white;
    cursor: pointer;
}

.chef-text {
    position: absolute;
    top: 40px;
    left: 70px;
    font-family: "Script";
    font-size: 230%;
}

.project-title {
    position: absolute;
    top: 75px;
    left: 70px;
    font-family: "Gotham-bold";
    font-size: 230%;
}

.votes {
    position: absolute;
    top: 65px;
    left: 69%;
}

.chef-button {
    transition-property: background-color, color;
    transition-duration: 0.2s;
}

.chef-button:hover {
    background-color: black;
    color: white;
    cursor: pointer;
}

.chef-button:hover > .votes {
    display: none;
}

.thanks {
    display: none;
}

.chef-button:hover > .thanks {
    position: absolute;
    top: 55px;
    right: 55px;
    display: block;
    width: 63px;
    height: 62px;
    background-image: url("../assets/images/thanks.png");
}
</style>
