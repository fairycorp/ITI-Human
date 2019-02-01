<template>
  <f7-page>
    <div v-if="Command == false">
      <f7-button class="backChButton color-white"
        @click="Back()"
        icon-f7="arrow_left">
      </f7-button>
      <f7-button class="logOutButton color-white"
        @click="LogOut()"
        icon-f7="close_round">
      </f7-button>
      <h1 class="cm">Votre commande</h1>
      <h3 class="cm2">Où voici ce que vous avez pris</h3>
      <div class="global">
        <div class="under_global">
          <div
            v-for="infolproducts in InfoLProducts" 
            :key="infolproducts.product.storageLinkedProductId" :value="infolproducts.product.storageLinkedProductId" class="ordered">
              <img width="50" class="pictureproduct" :src="infolproducts.product.productAvatarUrl" />
              <span class="quantity">
                <span class="grey">x </span>
                <span class="openSans-bold">{{ infolproducts.quantity }}</span>
                <span class="grey"> ({{ infolproducts.product.unitPrice / 100 }} €)</span>
              </span>
          </div>
          <div class="totalPrice medium-top-margin">TOTAL <span class="openSans-bold">{{ totalPrice / 100 }} €</span>,<br />
            <span v-if="selectedClassroom.classroomId === 0" class="openSans-bold">À emporter</span>
            <span v-else>Livrée en <span class="openSans-bold">{{ selectedClassroom.name }}</span></span>.
          </div>
        </div>
        <h3 class="class medium-top-margin">Où voulez-vous être livré(e) ?</h3>
        <f7-row tag="p">
        <f7-button big small outline round
            class="selectClass selection unselectable-text button color-white light-top-margin light-right-margin"
            @click="selectClassroom(classroom)"
            :class="{ selectedButton : selectedClassroom.classroomId === classroom.classroomId }"
            v-for="classroom in Classrooms" :key="classroom.classroomId">
              <span v-if="classroom.classroomId > 0">{{classroom.name}}</span>
              <span v-else>TWAY</span>
        </f7-button>
        </f7-row>
        <f7-button class="unselectable-text col button color-white"
          @click="Continue()"> 
          Commander 
        </f7-button>
      </div>
    </div>
    <div v-else >
      <div class="orderedMessage"></div>
      <br/>
      <button @click="GoHome()" class="gohome unselectable-text">D'accord !</button>
    </div>
  </f7-page>
</template>

<script>
import Api from '../helpers/Api.js'
import {
  AuthService,
  IAuthServiceConfiguration,
  AuthLevel
} from "@signature/webfrontauth";
import {
    getAuthService
} from "../helpers/AuthServiceHelp.js";
import Axios from "axios";

export default {
  props: { projectinfos: Object, ArrayOfInfoLProducts: Object },

  data() {
    return {
      authService: null,
      totalPrice: 0,      
      Projects: '',
      InfoLProducts: '',
      Classrooms: [],
      selectValue: '',
      selectedClassroom:'',
      Command: false,
    };
  },

  async created() {
    this.authService = getAuthService();
    await this.isAccessible();
  },

  mounted() {
    this.Projects = this.projectinfos;
    this.InfoLProducts = this.ArrayOfInfoLProducts;
    this.GetClassrooms("classroom");
    this.GetTotalPrice();
  },

  methods: {

    selectClassroom(classroom) {
        this.selectedClassroom = classroom;
        if (classroom.classroomId === 0) {
          this.selectedClassroom.name = "TWAY";
        }
    },

    Back() {
        this.$f7router.navigate({ name: 'home' });
    },

    async LogOut(){
      await this.authService.logout(true);
      await this.isAccessible();
    },

    GetTotalPrice(){
      for (let i = 0; i < this.InfoLProducts.length; i++) {
        this.totalPrice = (this.totalPrice + (this.InfoLProducts[i].product.unitPrice * this.InfoLProducts[i].quantity));
      }
    },

    async isAccessible() {
      if (this.authService.authenticationInfo.level == 0) {
        this.$f7router.navigate({ name: 'login' });
      }
    },

    async GetClassrooms(endpoint){
      let response = await Api.get(endpoint);
      this.Classrooms = response.data;
      this.SetClassroom();
    },

    SetClassroom(){
      for (let i = 0; i < this.Classrooms.length; i++) {
        if (this.Classrooms[i].classroomId == 0) {
          this.selectedClassroom = this.Classrooms[i];
        }
      }
    },

    async MakeACommand(endpoint, data){
      let response = await Api.post(endpoint, data);
      this.CommandId = response.data;
    },

    Continue() {
      let ClassroomId = this.selectedClassroom.classroomId;
      this.Projects.classroomId = ClassroomId;
      this.MakeACommand("order/create", this.Projects);
      this.Command = true;
    },

    GoHome(){
      this.$f7router.navigate({ name: 'home' });
    },
  },
};
</script>

<style lang="scss">

.cm{
    position: absolute;
    top: 2%;
    left: 21%;
    color: gray;
    font-family: "Script"; 
}

.cm2{
    position: absolute;
    top: 10%;
    left: 20%;
    color: gray;
    font-family: "Script";
}

.grey {
    color: #9e9e9e;
}

.quantity {
    font-size: 130%;
}

.totalPrice {
    color: black;
    background-color: white;
    font-size: 140%;
}

.pictureproduct {
    vertical-align: middle;
}

.ordered {
    margin-bottom: 10px;
}

.global{
  position: absolute;
  top: 20%;
}

.under_global{
    background-color: white;
    padding: 10px;
}

.backOrButton{
    position: absolute;
    top: 0%;
    left: 0%;
}

.orderList{
    width: 100%;
    color: black;
    background-color: white;
}

.selectClass{
    width: 60px;
    margin-bottom: 3px;
}

.class{
  color: white;
}

.orderedMessage {
    position: absolute;
    top: 25%;
    left: 2.5%;
    width: 343px;
    height: 230px;
    background-image: url("../../../SPA/src/assets/images/preparing-order-mobile.png");
}    

button.gohome {
    position: absolute;
    outline-width: 0;
    top: 65%;
    left: 25%;
    width: 175px;
    height: 30px;
    border-radius: 45px;
    background-color: white;
    border: 1px solid black;
    font-family: "gotham-bold";
    color: black;
    letter-spacing: 2px;
    
    transition-duration: 0.2s;
    transition-property: background-color, color;
}
</style>