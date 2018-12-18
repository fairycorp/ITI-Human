<template>
  <f7-page>
    <f7-list>
      <f7-list-item title="Ta Salle" smart-select>
        <select name="salle" v-model="selectValue">
          <option v-for="classrooms in Classrooms" 
          :key="classrooms.classroomId" :value="classrooms.name">
            <span v-if="classrooms.classroomId > 0">{{classrooms.name}}</span>
            <span v-else>À emporter</span>
          </option>
        </select>
      </f7-list-item>
      <br/>
      <br/>
      <button
        v-if="selectValue != ''"
        @click="Continue()"> 
        Continue
      </button>
    </f7-list>
  </f7-page>
</template>

<script>
import Api from '../helpers/Api.js'

export default {
  props: { projectinfos: Object },
    
  data() {
    return {
      Projects: '',
      StorageProducts: '',
      ProductName: '',
      Classrooms: [],
      selectValue: ''
    };
  },
  
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

    GetClassRoomId(classromName){
      for (let index = 0; index < this.Classrooms.length; index++) {
        if (this.Classrooms[index].name == classromName) {
          return this.Classrooms[index].classroomId

        }
      }
    },

    Continue() {

      let ClassroomId = this.GetClassRoomId(this.selectValue);

      let order = {
        storageId: this.Projects.storageId,
        userId: 1,
        classroomId: ClassroomId,
        products: []
      }

      this.$f7router.navigate({ name: 'chooseproducts' }, {
        props: { projectinfos: order }
      });
    }
  },
        //new OrderViewModel(0, 1739, 0, {StorageLinkedProductId: 2147, Quantity: 2369})
};
</script>

<style>
.text-color-primary {
  color: blue;
  font-size: 30;
}
</style>