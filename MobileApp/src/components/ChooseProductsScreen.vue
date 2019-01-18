<template>
  <f7-page>
    <p>{{LinkedProducts}}</p>
    <f7-list>
      <f7-list-item 
        v-for="linkedProducts in LinkedProducts" 
        :key="linkedProducts.storageLinkedProductId" :value="linkedProducts.productId" :title="linkedProducts.productName">

        <f7-stepper small raised slot="after" :input="false" :min="0" :max="linkedProducts.stock" @stepper:plusclick="addOrPlusCount(linkedProducts.productName)" @stepper:minusclick="remOrMinusCount(linkedProducts.productName)"></f7-stepper>
      </f7-list-item>
      <br/>
      <br/>
      <button
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
      //listOfProducts: [],
      ArrayOfProducts: [],
//      quantity: 1,
      CommandId: '',
//      Count: 0,
    };
  },

  mounted() {
    this.Projects = this.projectinfos;
    this.GetStorageProducts("storage/products/from/"+this.Projects.storageId);
  },

  methods: {
    checkProduct(event) {
      const self = this;
      const value = event.target.value;
      if (event.target.checked) {
        self.listOfProducts.push(value);
      } else {
        self.listOfProducts.splice(self.listOfProducts.indexOf(value), 1);
      }
    },

    addOrPlusCount(nameOfProduct) {
      let compared = false;
       if (this.ArrayOfProducts.length > 0) {
        for (let i = 0; i < this.ArrayOfProducts.length; i++) {
          if (this.ArrayOfProducts[i].product == nameOfProduct) {
            compared = true;
            this.ArrayOfProducts[i].number++;
          }
        }
        if (compared == false) {
          this.ArrayOfProducts.push({"product" : nameOfProduct, "number" : 1 });
        }
      }
      else {
        this.ArrayOfProducts.push({"product" : nameOfProduct, "number" : 1 });
      }
    },

    remOrMinusCount(nameOfProduct) {
      for (let i = 0; i < this.ArrayOfProducts.length; i++) {
        if (this.ArrayOfProducts[i].product == nameOfProduct) {
          this.ArrayOfProducts[i].number--;
          if (this.ArrayOfProducts[i].number == 0) {
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