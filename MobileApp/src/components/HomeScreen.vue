<template>
  <f7-page>
    <f7-button class="backChButton color-white" @click="Back()" icon-f7="arrow_left"></f7-button>
    <f7-button class="logOutButton color-white" @click="LogOut()" icon-f7="close_round"></f7-button>
    <div @click="ChangeRoute('profile')" class="menu profile">
      <div class="illustration-profile"></div>
      <div class="profil unselectable-text">PROFIL</div>
      <div class="modify unselectable-text">MODIFIER</div>
    </div>
    <br>
    <div @click="ChangeRoute('hungry')" class="menu order">
      <div class="illustration-order"></div>
      <div class="hungry unselectable-text">J'AI FAIM</div>
      <div class="command unselectable-text">COMMANDER</div>
    </div>

    <button
      @click="About"
      class="about-link unselectable-text col button color-gray"
    >QUI SOMMES-NOUS</button>
    <div class="collab-info">EN COLLABORATION AVEC IN'TECH</div>
  </f7-page>
</template>

<script>
import Api from "../helpers/Api.js";
import {
  AuthService,
  IAuthServiceConfiguration,
  AuthLevel
} from "@signature/webfrontauth";
import { getAuthService } from "../helpers/AuthServiceHelp.js";
import Axios from "axios";

export default {
  data() {
    return {
      authService: null,
      ProjectInfos: [],
      StorageId: "",
      StorageInfos: ""
    };
  },

  async created() {
    this.authService = getAuthService();
    await this.isAccessible();
  },

  async mounted() {
    await this.getProjectInfos("project");
  },

  methods: {
    async getProjectInfos(endpoint) {
      let response = await Api.get(endpoint);
      this.ProjectInfos = response.data;
    },

    ChangeRoute(route) {
      if (route == "hungry") {
        this.$f7router.navigate({ name: "chooseProject" });
      } else if (route == "profile") {
        this.$f7router.navigate({ name: "about" });
      }
    },

    async isAccessible() {
      if (this.authService != null && this.authService.authenticationInfo.level == 0) {
        this.$f7router.navigate({ name: "login" });
      }
    },

    async LogOut() {
      await this.authService.logout(true);
      await this.isAccessible();
    },

    About() {
      this.$f7router.navigate({ name: "about" });
    }
  }
};
</script>

<style lang="scss">
.logOutButton {
  position: absolute;
  top: 0%;
  right: 0%;
}

.collab-info {
  position: absolute;
  bottom: 0%;
  right: 0%;
  width: 130px;
  font-family: "gotham-bold";
  letter-spacing: 2px;
  color: #cac8c9;
  font-size: 10px;
}

.about-link {
  position: absolute;
  left: 0%;
  bottom: 0%;
  width: 140px;
  font-family: "gotham-bold";
  letter-spacing: 2px;
  color: #666567;
  font-size: 13px;
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
  top: 15%;
}

.illustration-profile {
  width: 100%;
  height: 60%;
  background-image: url("../../../SPA/src/assets/images/middle-profile-menu.png");
}

.order {
  position: absolute;
  top: 50%;
}

.illustration-order {
  width: 100%;
  height: 60%;
  background-image: url("../../../SPA/src/assets/images/middle-order-menu.png");
}
</style>