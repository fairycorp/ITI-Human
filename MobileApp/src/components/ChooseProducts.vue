<template>
  <f7-page>
    <f7-button class="backChButton color-white"
      @click="Back()"
      icon-f7="arrow_left">
    </f7-button>
    <f7-button class="logOutButton color-white"
      @click="LogOut()"
      icon-f7="close_round">
    </f7-button>
    <h1 class="bv">Bienvenue sur notre carte</h1>
    <h3 class="pr">Où voici donc nos produits</h3>
    <f7-list class="prodList">
      <f7-list-item 
        class="linked"
        v-for="linkedProducts in LinkedProducts" 
        :key="linkedProducts.storageLinkedProductId" :value="linkedProducts.productId">
        <p>
          <img width="50" class="pictureproduct" :src="linkedProducts.productAvatarUrl" />
          <span class="proName openSans-bold">{{ linkedProducts.productName }}</span>, <span class="price">{{ linkedProducts.unitPrice / 100 }}€</span>,
          <f7-stepper class="stepper" small raised slot="after" :input="false" :min="0" :max="linkedProducts.stock" @stepper:plusclick="addOrPlusCount(linkedProducts.productName)" @stepper:minusclick="remOrMinusCount(linkedProducts.productName)"></f7-stepper>
          <br />
          <span class="product-info"><span class="openSans-bold">{{ linkedProducts.stock }}</span> produits restants.</span>
        </p>
      </f7-list-item>
      <br/>
      <f7-button class="unselectable-text col button color-white"
        v-if="ArrayOfProducts.length > 0"
        @click="Continue()"> 
        Continuer
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
      CompInfoLProducts: [],
      CommandId: '',
    };
  },

  async created() {
    const config = {
      identityEndPoint: {
        hostname: process.env.HOST_NAME,
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
    Back() {
        this.$f7router.navigate({ name: 'home' });
    },

    async LogOut(){
      await this.authService.logout(true);
      await this.authService.logout(true);
      await this.authService.logout(true);
      await this.isAccessible();
    },

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
          if (this.ArrayOfProducts[i].storageLinkedProductId == nameOfProduct) {
            compared = true;
            this.ArrayOfProducts[i].quantity++;
          }
        }
        if (compared == false) {
          this.ArrayOfProducts.push({"storageLinkedProductId" : nameOfProduct, "quantity" : 1 });
        }
      }
      else {
        this.ArrayOfProducts.push({"storageLinkedProductId" : nameOfProduct, "quantity" : 1 });
      }
    },

    remOrMinusCount(nameOfProduct) {
      for (let i = 0; i < this.ArrayOfProducts.length; i++) {
        if (this.ArrayOfProducts[i].storageLinkedProductId == nameOfProduct) {
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

    FillTheCommand(){
      for (let i = 0; i < this.ArrayOfProducts.length; i++) {
        for (let j = 0; j < this.LinkedProducts.length; j++) {
          if (this.ArrayOfProducts[i].storageLinkedProductId == this.LinkedProducts[j].productName) {
            this.ArrayOfProducts[i].storageLinkedProductId = this.LinkedProducts[j].storageLinkedProductId
          }
        }
      }
    },

    GetCompInfoLProducts(){
      for (let i = 0; i < this.ArrayOfProducts.length; i++) {
        for (let j = 0; j < this.LinkedProducts.length; j++) {
            if (this.ArrayOfProducts[i].storageLinkedProductId == this.LinkedProducts[j].productName) {
              this.CompInfoLProducts.push( {"product" : this.LinkedProducts[j], "quantity" : this.ArrayOfProducts[i].quantity});
            }
        }
      }
    },

    Continue() {
      this.GetCompInfoLProducts();

      this.FillTheCommand();

      let order = {
        storageId: this.Projects.storageId,
        userId: this.authService.authenticationInfo.user.userId,
        classroomId: 0,
        products: this.ArrayOfProducts
      }


      this.$f7router.navigate({ name: 'order' }, {
        props: { projectinfos: order, ArrayOfInfoLProducts: this.CompInfoLProducts }
      });
    },
  },
}

</script>

<style lang="scss">
.backChButton{
    position: absolute;
    top: 0%;
    left: 0%;
}

.bv{
    position: absolute;
    top: 2%;
    left: 5%;
    color: gray;
    font-family: "Script";    
}

.pr{
    position: absolute;
    top: 10%;
    left: 23%;
    color: gray;
    font-family: "Script";

}

.pictureproduct {
    vertical-align: middle;
    width: 6%;
    height: 6%;
}

.description {
    color: #757575;
}

.product-info {
    font-size: 80%;
    color: #757575;
}

.price {
    color: #4b80ac;
}

.linked{
    background-color: white;
}

.prodList{
    position: absolute;
    top: 15%;
    width: 100%
}

.stepper{
    position: absolute;  
    right: 0%;
    top: 35%;
}

</style>