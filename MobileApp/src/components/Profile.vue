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

    <h1>Setup de votre profil</h1>

    <h3 class="medium-top-margin">MINUSCULE ETAT CIVIL</h3>
    <f7-list class="listLogin" inline-labels no-hairlines-md>
      <f7-list-input class="textual light-top-margin" label="FirstName" :value="FirstName" @input="FirstName = $event.target.value" type="text" placeholder="Your FirstName">
      </f7-list-input>
      <f7-list-input  class="textual medium-top-margin" label="LastName" :value="LastName" @input="LastName = $event.target.value" type="text" placeholder="Your LastName">
      </f7-list-input>
      <f7-list-input  class="textual medium-top-margin" label="Bio" :value="Bio" @input="Bio = $event.target.value" type="textarea" placeholder="Votre (courte) bio">
      </f7-list-input>
    </f7-list> 
    <div
      class="submit-button">
      <div
        @click="submit()"
        class="submit-button-text unselectable-text">
        VALIDEZ-MOI.
      </div>
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
  data() {
    return {
      Password: "",
      Name: "",
      authService: null
    }
  },

  methods: {
    Back() {
    this.$f7router.navigate({ name: 'home' });
    },    

    async LogOut(){
      await this.authService.logout(true);
      await this.isAccessible();
    },
  },  
}
</script>

<style>

</style>
