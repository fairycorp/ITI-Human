<template>
  <f7-page>
    <f7-list>
      <f7-list-item title="Ta Salle" smart-select :smart-select-params="{openIn: 'popover'}">
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
      <f7-button
        v-if="selectValue != ''"
        @click="Continue()"> 
        Continue
      </f7-button>
    </f7-list>
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
  props: { projectinfos: Object },

  data() {
    return {
      authService: null,      
      Projects: '',
      StorageProducts: '',
      ProductName: '',
      Classrooms: [],
      selectValue: ''
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

  mounted() {
    this.Projects = this.projectinfos;
    this.GetStorageProducts("storage/products/from/"+this.Projects.storageId);
    this.GetClassrooms("classroom");
  },

  watch: {
    authService: function(newValue, oldValue) {
      this.isAccessible();
    }
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
        userId: this.authService.authenticationInfo.user.userId,
        classroomId: ClassroomId,
        products: []
      }

      this.$f7router.navigate({ name: 'chooseproducts' }, {
        props: { projectinfos: order }
      });
    }
  },
};
</script>

<style lang="scss">

</style>