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

export default {

  data() {
    return {
      ProjectInfos: [],
      StorageId: '',
      StorageInfos: ''
    };
  },
  
  async mounted() {
    await this.getProjectInfos("project");
  },

  methods: {
    async getProjectInfos(endpoint){
      let response = await Api.get(endpoint);
      this.ProjectInfos = response.data;
      console.log("OUT OF getProjectInfos METHOD")
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
      console.log("ENTER IN GETLOGIN METHOD")
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