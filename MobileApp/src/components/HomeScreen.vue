<template>
    <f7-page>
        <button v-for="projectInfos in ProjectInfos" 
          :key="projectInfos.projectId" 
          @click="GetStorage(projectInfos.projectId)">
          {{ projectInfos.projectName }}
        </button>
    </f7-page>
</template>

<script>
import Api from '../helpers/Api.js'
import OrderScreen from './OrderScreen'

export default {

  async mounted() {
    await this.getProjectInfos("project");
  },

  methods: {
    async getProjectInfos(endpoint){
      let response = await Api.get(endpoint);
      this.ProjectInfos = response.data;
    },

    getStorageInfos(endpoint){
      Api.get(endpoint).then(response => {
        this.StorageInfos = response.data;
        this.$f7router.navigate({ name: 'order' }, {
            props: { projectinfos: this.StorageInfos }
        });
      })
    },

    GetStorage(projectId) {
      this.StorageId = projectId
      this.getStorageInfos("storage/project/"+ this.StorageId)
    }
  },

  data() {
    return {
      ProjectInfos: [],
      StorageId: '',
      StorageInfos: ''
    };
  }
}


</script>

<style>
.text-color-primary {
  color: blue;
  font-size: 30;
}
</style>