<template>
    <f7-page>
        <f7-button v-for="projectInfos in ProjectInfos" 
          :key="projectInfos.projectId" 
          @click="GetStorage(projectInfos.projectId)">
          {{ projectInfos.projectName }}
        </f7-button>

        <f7-button
          @click="GetLogin()">
          Go loginScreen
        </f7-button>
    </f7-page>
</template>

<script>
import Api from '../helpers/Api.js'
import OrderScreen from './OrderScreen'
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
      StorageInfos: ''
    };
  },

  async created() {
    const config = {
      identityEndPoint: {
        hostname: "192.168.1.31",
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

  methods: {

    async isAccessible() {
      await this.authService.refresh(true, true, true);
      if (this.authService != null) {
        if (this.authService.authenticationInfo.level == 0) {
          this.$f7router.navigate({ name: 'login' });
        }
      }
    },

    async getProjectInfos(endpoint){
      let response = await Api.get(endpoint);
      this.ProjectInfos = response.data;
    },

    async getStorageInfos(endpoint){
      let response = await Api.get(endpoint);
      this.StorageInfos = response.data;
      this.$f7router.navigate({ name: 'order' }, {
        props: { projectinfos: this.StorageInfos }
      });
    },

    GetStorage(projectId) {
      this.StorageId = projectId
      this.getStorageInfos("storage/project/"+ this.StorageId)
    },

    GetLogin(){
      this.$f7router.navigate({ name: 'login' });
    },
  },
};

</script>

<style>
.text-color-primary {
  color: blue;
  font-size: 30;
}
</style>