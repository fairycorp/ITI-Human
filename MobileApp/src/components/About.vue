<template>
    <f7-page>
        <div
        class="light-left-page">
            <h1>À propos</h1>

            <h3 class="light-top-margin lightly-reduced-bottom-margin">
                QUI ÊTES-VOUS VRAIMENT ?
            </h3>
            <p class="openSans-litalic">
                Rien de plus que de simples étudiants, à vrai dire ! <span class="openSans-bold">Sylvain Goffart</span>
                et <span class="openSans-bold">Pierre Viara</span> pour être exact.
            </p>

            <h3 class="medium-top-margin lightly-reduced-bottom-margin">
                QU'EST-CE QUE FORK ?
            </h3>
            <p class="openSans-litalic">
                En fait, au cours du quatrième semestre à <span class="openSans-bold">IN’TECH</span>, les étudiants sont 
                chargés d’organiser un événement, qu’ils doivent eux-mêmes financer. Pour se faire, ils organisent en
                équipe ce qu’on appelle des “buvettes.”<br />
                Le but de <span class="openSans-bold">Fork</span> c’est tout simplement de <span class="openSans-bold">digitaliser les échanges</span>
                entre ces <span class="openSans-bold">équipes</span> et leurs <span class="openSans-bold">clients</span>.
            </p>

            <h3 class="medium-top-margin lightly-reduced-bottom-margin">
                J'AI ENCORE PLEIN DE QUESTIONS.
            </h3>
            <p class="openSans-litalic">
                À la bonne heure, les amis ! Nous serions absolument ravis d’y répondre, si toutefois vous nous contactez.
                Et cela tombe très bien, puisque nous avons une adresse électronique <span class="openSans-bold">à votre
                entière disposition</span>. Oui.
            </p>
            <a href="mailto:fork@intechinfo.fr">
                <div class="contact-button medium-top-margin"></div>
            </a>
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
      About: false,
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

  watch: {
    authService: function(newValue, oldValue) {
      this.isAccessible();
    }
  },  

  methods: {
    async isAccessible() {
      await this.authService.refresh(true, true, true);
      //console.log(this.authService.authenticationInfo.level);
      if (this.authService.authenticationInfo.level == 0) {
        this.$f7router.navigate({ name: 'login' });
      }
    },
  }, 
}
</script>

<style lang="scss">

.contact-button {
    width: 193px;
    height: 46px;
    background-image: url("../../../SPA/src/assets/images/contact-button.png");
    cursor: pointer;
}

.moved-logo {
    position: absolute;
    top: 65%;
    left: 21%;
    width: 252px;
    height: 270px;
    background-image: url("../../../SPA/src/assets/images/logo.png");
}

</style>