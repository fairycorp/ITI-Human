<template>
  <f7-page>
  <p>{{Projects}}</p>
  </f7-page>
</template>

<script>
import Api from '../helpers/Api.js'
import OrderScreen from './OrderScreen'

export default {
  props: { projectinfos: Object },

  mounted() {
    this.Projects = this.projectinfos;
    console.log('i am on the chooseproductscreen');
  },

  methods: {
    getProjectInfos(endpoint){
      Api.get(endpoint).then(response => {
        this.ProjectInfos = response.data;
      })
    },
  },

  data() {
    return {
      Projects: [],
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