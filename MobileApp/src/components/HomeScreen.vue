<template>
    <f7-page>
      <div
          class="home-logo">
      </div>
        <f7-button class="logOutButton color-white"
            @click="LogOut()"
            icon-f7="close_round">
        </f7-button>

      <div class="header">
      <h3 class="h3"> Voici la liste des Projets  </h3>
      <f7-button class="unselectable-text col button color-white" v-for="projectInfos in ProjectInfos" 
          :key="projectInfos.projectId" 
          @click="GetStorage(projectInfos.projectId)">
          {{ projectInfos.projectName }}
      </f7-button>
      </div>

      <div class="footer">
        <button @click="About" class="about-link unselectable-text col button color-gray">
            QUI SOMMES-NOUS
        </button>
        <div class="collab-info">EN COLLABORATION AVEC IN'TECH</div>
      </div>
    </f7-page>
</template>

<script>
import Api from '../helpers/Api.js'
import {
    AuthService,
    IAuthServiceConfiguration,
    AuthLevel
} from "@signature/webfrontauth";
import Axios from "axios";

export default {

  data() {
    return {
      authService: null,
      ProjectInfos: [],
      StorageId: '',
      StorageInfos: '',
    };
  },

  async created() {
    const config = {
      identityEndPoint: {
        hostname: process.env.HOST_NAME,
        port: 5000,
        disableSsl: true
      }
    };
    this.authService = new AuthService(config, Axios);
    await this.isAccessible();
  },

  async mounted() {
    await this.getProjectInfos("project");
  },

  watch: {
    authService: function(newValue, oldValue) {
      this.isAccessible();
    }
  }, 

  methods: {

    async getProjectInfos(endpoint){
      let response = await Api.get(endpoint);
      this.ProjectInfos = response.data;
    },

    async getStorageInfos(endpoint){
      let response = await Api.get(endpoint);
      this.StorageInfos = response.data;
      this.$f7router.navigate({ name: 'chooseproducts' }, {
        props: { projectinfos: this.StorageInfos }
      });
    },

    GetStorage(projectId) {
      this.StorageId = projectId;
      this.getStorageInfos("storage/project/"+ this.StorageId);
    },

    async LogOut(){
      await this.authService.logout(true);
      await this.isAccessible();
    },

    async isAccessible() {
      await this.authService.refresh(true, true, true);
      //console.log(this.authService.authenticationInfo.level);
      if (this.authService.authenticationInfo.level == 0) {
        this.$f7router.navigate({ name: 'login' });
      }
    },

    About() {
        this.$f7router.navigate({ name: 'about' });
    }
  },
};
</script>

<style lang="scss">

/*.p{
    color: white;
}*/

.home-logo {
    position: absolute;
    top: 40%;
    left: 35%;
    width: 126px;
    height: 135px;
    background-image: url("../../../SPA/src/assets/images/logo-small.png");
}

.logOutButton{
    position: absolute;
    top: 0px;
    right: 0px;
}

.header{
    position: absolute;
    left: 26%;
    top: 0px;
}

.header .h3{
    font-family: "gotham-bold";
    color: gray;
}


.footer {
    position: absolute;
    bottom: 45px;
}

.collab-info {
    position: absolute;
    left: 175px;
    width: 200px;
    font-family: "gotham-bold";
    letter-spacing: 2px;
    color: #cac8c9;
    font-size: 10px
}

.about-link {
    position: absolute;
    left: 0px;
    width: 160px;
    font-family: "gotham-bold";
    letter-spacing: 2px;
    color: #666567;
    font-size: 13px
}
</style>