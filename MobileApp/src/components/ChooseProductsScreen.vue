<template>
  <scroll-view>
    <view>
        <button v-for="projectInfos in ProjectInfos" :key="projectInfos.projectId"
          :on-press="() => GetStorage(projectInfos)"
          :title="projectInfos.projectName"
        />
    </view>
  </scroll-view>
</template>

<script>
import Api from '../helpers/Api.js'
import OrderScreen from './OrderScreen'

export default {
  props: {
    navigation: {
      type: Object
    }
  },

  mounted() {
    this.getProjectInfos("project");
  },

  methods: {
    getProjectInfos(endpoint){
      Api.get(endpoint).then(response => {
        this.ProjectInfos = response.data;
      })
    },

    getStorageInfos(endpoint){
      Api.get(endpoint).then(response => {
        this.StorageInfos = response.data;
        this.navigation.navigate("Orderscreen", {
          projectinfos: this.StorageInfos
        });
      })
    },

    GetStorage(project) {
      this.StorageId = project.projectId
      this.getStorageInfos("storage/project/"+ this.StorageId)
    }
  },

  data() {
    return {
      ProjectInfos: '',
      StorageId: '',
      StorageInfos: '',
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