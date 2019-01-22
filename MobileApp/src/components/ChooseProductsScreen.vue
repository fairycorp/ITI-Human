<template>
  <f7-page>
    <f7-list>
      <f7-list-item 
        v-for="linkedProducts in LinkedProducts" 
        :key="linkedProducts.storageLinkedProductId" :value="linkedProducts.productId" :title="linkedProducts.productName">

        <f7-stepper small raised slot="after" :input="false" :min="0" :max="linkedProducts.stock" @stepper:plusclick="addOrPlusCount(linkedProducts.productName)" @stepper:minusclick="remOrMinusCount(linkedProducts.productName)"></f7-stepper>
      </f7-list-item>
      <br/>
      <br/>
      <f7-button
        v-if="ArrayOfProducts.length > 0"
        @click="Continue()"> 
        Commander
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
      Projects: [],
      LinkedProducts: [],
      ArrayOfProducts: [],
      CommandId: '',
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
        if (this.authService.authenticationInfo.level > 0) {
          this.$f7router.navigate({ name: 'login' });
        }
      }
    },

    addOrPlusCount(nameOfProduct) {
      let compared = false;
       if (this.ArrayOfProducts.length > 0) {
        for (let i = 0; i < this.ArrayOfProducts.length; i++) {
          if (this.ArrayOfProducts[i].product == nameOfProduct) {
            compared = true;
            this.ArrayOfProducts[i].quantity++;
          }
        }
        if (compared == false) {
          this.ArrayOfProducts.push({"product" : nameOfProduct, "quantity" : 1 });
        }
      }
      else {
        this.ArrayOfProducts.push({"product" : nameOfProduct, "quantity" : 1 });
      }
    },

    remOrMinusCount(nameOfProduct) {
      for (let i = 0; i < this.ArrayOfProducts.length; i++) {
        if (this.ArrayOfProducts[i].product == nameOfProduct) {
          this.ArrayOfProducts[i].quantity--;
          if (this.ArrayOfProducts[i].quantity == 0) {
            this.ArrayOfProducts.splice(i, 1);
          }
        }
      }
    },

    async GetStorageProducts(endpoint){
      let response = await Api.get(endpoint);
      this.LinkedProducts = response.data;
    },

    async MakeACommand(endpoint, data){
      let response = await Api.post(endpoint, data);
      this.CommandId = response.data;
    },

    FillTheCommand(){
      for (let i = 0; i < this.ArrayOfProducts.length; i++) {
        for (let j = 0; j < this.LinkedProducts.length; j++) {
          if (this.ArrayOfProducts[i].product == this.LinkedProducts[j].productName) {
            this.Projects.products.push(
              {
                storageLinkedProductId: this.LinkedProducts[j].storageLinkedProductId,
                quantity: this.ArrayOfProducts[i].quantity
              }
            );
          }
        }
      }
    },

    Continue() {
      this.FillTheCommand();

      this.MakeACommand("order/create", this.Projects);      
    },
  },
}

</script>

<style lang="scss">

</style>