<template>
  <f7-page>
    <p>{{CommandId}}</p>

    <f7-list>
      <f7-list-item title="Products" smart-select :smart-select-params="{openIn: 'popup'}">
        <select name="products" multiple v-model="selectValue">
          <option v-for="linkedProducts in LinkedProducts" 
          :key="linkedProducts.storageLinkedProductId" :value="linkedProducts.productId">
            {{linkedProducts.productId}} 
          </option>
        </select>
      </f7-list-item>
      <br/>
      <br/>
      <button
        v-if="selectValue != ''"
        @click="Continue()"> 
        Commander
      </button>
    </f7-list>  
  </f7-page>
</template>

<script>
import Api from '../helpers/Api.js'
import OrderScreen from './OrderScreen'

export default {
  props: { projectinfos: Object },

  data() {
    return {
      Projects: [],
      LinkedProducts: [],
      selectValue: [],
      quantity: 1,
      CommandId: '',
    };
  },

  mounted() {
    this.Projects = this.projectinfos;
    this.GetStorageProducts("storage/products/from/"+this.Projects.storageId);
  },

  methods: {
    async GetStorageProducts(endpoint){
      let response = await Api.get(endpoint);
      this.LinkedProducts = response.data;
    },

    async MakeACommand(endpoint, data){
      console.log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
      let response = await Api.post(endpoint, data);
      this.CommandId = response.data;
    },

    FillTheCommand(){
      for (let index = 0; index < this.LinkedProducts.length; index++) {
        for (let ind = 0; ind < this.selectValue.length; ind++) {
          if (this.LinkedProducts[index].productId == this.selectValue[ind]) {
            this.Projects.products.push(
              {
                storageLinkedProductId: this.LinkedProducts[index].storageLinkedProductId,
                quantity: this.quantity
              }
            );
          }
        }
      }
    },

    Continue() {
      this.FillTheCommand(this.selectValue);

      this.MakeACommand("order/create", this.Projects);      
    },
  },
}

</script>

<style>
.text-color-primary {
  color: blue;
  font-size: 30;
}
</style>