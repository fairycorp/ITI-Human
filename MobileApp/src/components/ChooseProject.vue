<template>
    <f7-page>
            
            <div class="text-at-bottom">
                “ La grande cuisine n’est pas faite pour les timorés. Il faut avoir de l’imagination, de l’audace.
                Il faut prendre le risque de commettre des erreurs et personne n’a le droit de vous imposer des limites,
                quelles que soient vos origines, d’où que vous veniez. Votre seule limite, c’est votre âme. C’est la vérité,
                tout le monde peut cuisiner, mais le véritable génie, n’appartient qu’aux audacieux. ”
            </div>
            <div class="signature">Chef Gusteau</div>
        <div
            v-for="projectsReady in OpenProject"
            @click="GetStorage(projectsReady.storageId)"
            :key="projectsReady.projectId"
            :class="{ highmargintop : index > 0 }"
            class="little-right-page chef-button">
            <div class="chef-text">Chef</div>
            <div class="project-title">{{ stall.projectName }}</div>
            <div class="votes">
                <img
                    src="../../../SPA/src/assets/images/toque.png"
                    :class="{ transparent : stall.average < 5 || stall.average == 0 }"
                    class="heavylight-right-margin" />
                <img
                    src="../../../SPA/src/assets/images/toque.png"
                    :class="{ transparent : stall.average < 4 || stall.average == 0 }"
                    class="heavylight-right-margin" />
                <img
                    src="../../../SPA/src/assets/images/toque.png"
                    :class="{ transparent : stall.average < 3 || stall.average == 0 }"
                    class="heavylight-right-margin" />
                <img
                    src="../../../SPA/src/assets/images/toque.png"
                    :class="{ transparent : stall.average < 2 || stall.average == 0 }"
                    class="heavylight-right-margin" />
                <img
                    src="../../../SPA/src/assets/images/toque.png"
                    :class="{ transparent : stall.average < 1 || stall.average == 0 }"
                    class="heavylight-right-margin" />
            </div>
            <div class="thanks"></div>
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
      Projects: '',
      OpenProject: [],
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

  mounted() {
    this.GetProjects("project");
  },

  watch: {
    authService: function(newValue, oldValue) {
    this.isAccessible();
    }
  },

  methods: {
    async getProjectInfos(endpoint){
      let response = await Api.get(endpoint);
      this.Projects = response.data;
    },

    async getStorageInfos(endpoint){
      let response = await Api.get(endpoint);
      this.StorageInfos = response.data;
      this.$f7router.navigate({ name: 'order' }, {
        props: { projectinfos: this.StorageInfos }
      });
    },

    GetStorage(projectId) {
      this.StorageId = projectId
      this.getStorageInfos("storage/project/"+ this.StorageId)
    },

    IsProjectOpen(){
      for (let i = 0; i < this.Projects.length; i++) {
        if (this.Projects[i].openedStall) {
          this.OpenProject.push(this.Projects[i]);
        }
      }
    },

    Back() {
    this.$f7router.navigate({ name: 'home' });
    },    

    async LogOut(){
      await this.authService.logout(true);
      await this.isAccessible();
    },

    async isAccessible() {
      await this.authService.refresh(true, true, true);
      if (this.authService.authenticationInfo.level == 0) {
        this.$f7router.navigate({ name: 'login' });
      }
    },
  },
}
</script>

<style>

</style>
