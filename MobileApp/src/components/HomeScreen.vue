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
        <div @click="ChangeRoute('profile')" class="menu profile">
          <div class="illustration-profile"></div>
          <div class="profil unselectable-text">
              PROFIL
          </div>
          <div class="modify unselectable-text">
              MODIFIER
          </div>
        </div>
        <br/>
        <div @click="ChangeRoute('hungry')" class="menu order">
            <div class="illustration-order"></div>
            <div class="hungry unselectable-text">
                J'AI FAIM
            </div>
            <div class="command unselectable-text">
                COMMANDER
            </div>
        </div>

      <div class="footer">
        <button @click="About" class="about-link unselectable-text col button color-gray">
            QUI SOMMES-NOUS
        </button>
        <div class="collab-info">EN COLLABORATION AVEC IN'TECH</div>
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
import Axios from "axios";

export default {

  data() {
    return {
      authService: null,
      ProjectInfos: [],
      StorageId: '',
      StorageInfos: '',
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

  async mounted() {
    await this.getProjectInfos("project");
  },

  watch: {
    authService: function(newValue, oldValue) {
      this.isAccessible();
    }
  }, 

  methods: {

    async getProjectInfos(endpoint){
      let response = await Api.get(endpoint);
      this.ProjectInfos = response.data;
    },

    ChangeRoute(route){
      if (route == 'hungry') {
        this.$f7router.navigate({ name: 'chooseProject' });
      }
      else if (route == 'profile') {
        this.$f7router.navigate({ name: 'about' });
      }
    },

    async LogOut(){
      await this.authService.logout(true);
      await this.authService.logout(true);
      await this.authService.logout(true);
      await this.isAccessible();
    },

    async isAccessible() {
      await this.authService.refresh(true, true, true);
      console.log("MDR" + this.authService.authenticationInfo.level);
      if (this.authService.authenticationInfo.level == 0) {
        console.log("LOL" + this.authService.authenticationInfo.level);
        this.$f7router.navigate({ name: 'login' });
      }
    },

    About() {
        this.$f7router.navigate({ name: 'about' });
    }
  },
};
</script>

<style lang="scss">

.logOutButton{
    position: absolute;
    top: 0px;
    right: 0px;
}

.footer {
    position: absolute;
    bottom: 45px;
}

.collab-info {
    position: absolute;
    left: 175px;
    width: 200px;
    font-family: "gotham-bold";
    letter-spacing: 2px;
    color: #cac8c9;
    font-size: 10px
}

.about-link {
    position: absolute;
    left: 0px;
    width: 160px;
    font-family: "gotham-bold";
    letter-spacing: 2px;
    color: #666567;
    font-size: 13px
}

.menu {
    position: absolute;
    left: 10%;
    width: 80%;
    height: 30%;
    background-color: white;
    color: black;
    cursor: pointer;

    transition-property: background-color, color;
    transition-duration: 0.2s;
}

.menu:hover {
    background-color: black;
    color: white;
}

.hungry {
    position: absolute;
    top: 65%;
    left: 26%;
    font-family: "gotham-bold";
    font-size: 150%;
    letter-spacing: 6px;
}
 
.profil {
    position: absolute;
    top: 65%;
    left: 30%;    
    font-family: "gotham-bold";
    font-size: 150%;
    letter-spacing: 6px;
}

.modify {
    position: absolute;
    top: 80%;
    left: 30%;
    font-family: "gotham-bold";
    font-size: 140%;
    letter-spacing: 2px;
    color: grey;
}

.command {
    position: absolute;
    top: 80%;
    left: 25%;
    font-family: "gotham-bold";
    font-size: 140%;
    letter-spacing: 2px;
    color: grey;
}

.profile {
    position: absolute;
    top: 15%
}

.illustration-profile {
    width: 100%;
    height: 60%;
    background-image: url("../../../SPA/src/assets/images/middle-profile-menu.png")
}

.order {
    position: absolute;
    top: 50%
}

.illustration-order {
    width: 100%;
    height: 60%;
    background-image: url("../../../SPA/src/assets/images/middle-order-menu.png")
}
</style>