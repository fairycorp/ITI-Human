<template>
  <f7-page>
    <f7-block>
      <p> En quelle salle êtes vous </p>
      <br/>
      <f7-input type="text" input-id="ClassroomId" placeholder="Your name"></f7-input>
      <br/>
      <br/>
      <button v-for="projectInfos in ProjectInfos" 
        :key="projectInfos.projectId" 
        @click="Continue()"> 
        Continue
      </button>
    </f7-block>
  </f7-page>
</template>

<script>
import Api from '../helpers/Api.js'

export default {
  props: { projectinfos: Object },
  
  mounted() {
    this.Projects = this.projectinfos;
    this.GetStorageProducts("storage/products/from/"+this.Projects.storageId);
  },

  methods: {
    GetStorageProducts(endpoint){
      Api.get(endpoint).then(response => {
        this.StorageProducts = response.data;
      })
    },

    GetProductsName(endpoint){      
      Api.get(endpoint).then(response => {
        this.ProductName = response.data;
      })
    },

    Continue() {
      let order = {
        storageId: this.Projects.storageId,
        userId: 1,
        ClassroomId: document.getElementById("ClassroomId").value,
        products: []
      } 
    }
  },

  data() {
    return {
      Projects: '',
      StorageProducts: '',
      ProductName: '',
    };
  }
        //new OrderViewModel(0, 1739, 0, {StorageLinkedProductId: 2147, Quantity: 2369})
};
</script>

<style>
.text-color-primary {
  color: blue;
  font-size: 30;
}
</style>