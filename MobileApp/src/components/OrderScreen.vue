<template>
  <f7-page>
    <f7-block>
      <f7-list-item title="Ta Salle" smart-select>
        <select name="salles">
          <option v-for="classrooms in Classrooms" 
          :key="classrooms.classroomId">
            {{classrooms.classroomName}}
          </option>
        </select>
      </f7-list-item>
      <br/>
      <br/>
      <button
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
    this.GetClassrooms("classroom");
  },

  methods: {
    async GetStorageProducts(endpoint){
      let response = await Api.get(endpoint);
      this.StorageProducts = response.data;
    },

    async GetProductsName(endpoint){      
      let response = await Api.get(endpoint);
      this.ProductName = response.data;
    },

    async GetClassrooms(endpoint){
      let response = await Api.get(endpoint);
      this.Classrooms = response.data;
    },

    Continue() {
      let order = {
        storageId: this.Projects.storageId,
        userId: 1,
        //ClassroomId: document.getElementById("ClassroomId").value,
        products: []
      } 
    }
  },

  data() {
    return {
      Projects: '',
      StorageProducts: '',
      ProductName: '',
      Classrooms: [],
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