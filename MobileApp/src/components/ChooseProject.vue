<template>
  <f7-page>  
    <f7-button class="backChButton color-white"
      @click="Back()"
      icon-f7="arrow_left">
    </f7-button>
    <f7-button class="logOutButton color-white"
      @click="LogOut()"
      icon-f7="close_round">
    </f7-button>

    <div v-if="projectsLaunchs == false">
      <div class="fork-chefs-logo"></div>
      <div class="main-button">
          <button @click="SeeProjects()" class="big unselectable-text">LEURS CARTES</button>
      </div>
    </div>
    <div v-else>
      <div class="projects">
      <div
        v-for="projectsReady in OpenProject"
        @click="GetStorage(projectsReady.storageId)"
        :key="projectsReady.projectId"
        class="chef-button unselectable-text">
        <div class="chef-text unselectable-text">Chef</div>
        <div class="project-title unselectable-text">{{ projectsReady.projectName }}</div>
      </div>
      </div>
    </div>

    <div class="text-at-bottom unselectable-text">
        “ La grande cuisine n’est pas faite pour les timorés. Il faut avoir de l’imagination, de l’audace.
        Il faut prendre le risque de commettre des erreurs et personne n’a le droit de vous imposer des limites,
        quelles que soient vos origines, d’où que vous veniez. Votre seule limite, c’est votre âme. C’est la vérité,
        tout le monde peut cuisiner, mais le véritable génie, n’appartient qu’aux audacieux. ”
    </div>
    <div class="signature unselectable-text">Chef Gusteau</div>
  </f7-page>    
</template>

<script>
import Api from '../helpers/Api.js'
import {
  AuthService,
  IAuthServiceConfiguration,
  AuthLevel
} from "@signature/webfrontauth";
import {
    getAuthService
} from "../helpers/AuthServiceHelp.js";
import Axios from "axios";

export default {
  data() {
    return {
      authService: null,
      Projects: '',
      OpenProject: [],
      ProjectInfos: [],
      StorageId: '',
      StorageInfos: '',
      projectsLaunchs: false,
    };
  },

  async created() {
    this.authService = getAuthService();
    await this.isAccessible();
    await this.getProjectInfos("project");
    this.IsProjectOpen();
  },

  methods: {
    async getProjectInfos(endpoint){
      let response = await Api.get(endpoint);
      this.Projects = response.data;
    },

    async getStorageInfos(endpoint){
      let response = await Api.get(endpoint);
      this.StorageInfos = response.data;
      this.$f7router.navigate({ name: 'chooseproducts' }, {
        props: { projectinfos: this.StorageInfos }
      });
    },

    GetStorage(projectId) {
      this.StorageId = projectId
      this.getStorageInfos("storage/project/"+ this.StorageId)
    },

    IsProjectOpen(){
      for (let i = 0; i < this.Projects.length; i++) {
        if (this.Projects[i].openedStall === true) {
          this.OpenProject.push(this.Projects[i]);
        }
      }
    },

    async isAccessible() {
      if (this.authService.authenticationInfo.level == 0) {
        this.$f7router.navigate({ name: 'login' });
      }
    },

    SeeProjects(){
      this.projectsLaunchs = true;
    },

    Back() {
    this.$f7router.navigate({ name: 'home' });
    },    

    async LogOut(){
      await this.authService.logout(true);
      await this.isAccessible();
    },
  },
}
</script>

<style lang="scss">
.transparent {
    opacity: 0.2;
}

.signature {
    position: absolute;
    top: 85%;
    left: 0%;
    width: 83%;
    font-family: "Gloria";
    font-size: 140%;
    color: gray;
}

.main-button {
    position: absolute;
    top: 40%;
    left: 25%;
}

.fork-chefs-logo {
    position: absolute;
    top: 5%;
    left: 18%;
    width: 232px;
    height: 206px;
    background-image: url("../../../SPA/src/assets/images/fork-chefs.png");
}

.text-at-bottom {
    position: absolute;
    top: 48%;
    width: 100%;
    font-family: "Gloria";
    font-size: 120%;
    color: #ababab;
    text-align: justify;
}

button.big {
    outline-width: 0;
    width: 175px;
    height: 30px;
    border-radius: 45px;
    background-color: white;
    border: 1px solid black;
    font-family: "gotham-bold";
    color: black;
    letter-spacing: 2px;
    
    transition-duration: 0.2s;
    transition-property: background-color, color;
}

.chef-button {
  color: white;
}

.chef-text {
    position: absolute;
    top: 40px;
    left: 0%;
    font-family: "Script";
    font-size: 150%;
}

.project-title {
    position: absolute;
    top: 75px;
    left: 0%;
    font-family: "Gotham-bold";
    font-size: 150%;
}

</style>
