<template>
    <div class="global-content">
        
    </div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";
import { appSettings } from "@/config/appSettings";
import {
  AuthService,
  IAuthServiceConfiguration,
AuthLevel
} from "@signature/webfrontauth";
import Axios from "axios";
import API from "@/services/API";
import Endpoint from "@/helpers/Endpoint";
import { IDetailedDataUser } from "@/models/model.User";
import { IBasicDataProject } from "@/models/model.Project";
import { IBasicDataStorageLinkedProduct,
IBasicDataStorageLinkedProductInWayToBeOrdered } from '@/models/model.Storage';
import { IOrderCreationViewModel, IBasicDataProductToOrder } from "@/models/model.Order";
import { IBasicDataClassroom } from '@/models/model.Classroom';

@Component({})
export default class OrderDashboard extends Vue {
    @Prop() private authService!: AuthService;

    constructor() {
        super();
        this.isAccessible();
    }

    mounted() {
        this.fetchOrders(this.$route.params.id);
        console.log(this.$route.params.id);
    }

    // DATACHECKING METHODS.
    /** Checks if the current view route can be accessed. */
    private isAccessible() {
        if (Vue.prototype.$authService === undefined) {
            this.$router.push("/");
        } else {
            this.authService = Vue.prototype.$authService;
            this.authService.refresh(true, true, true);

            if (this.authService.authenticationInfo.level === 0) {
                this.$router.push("/");
            }
        }
    }

    private async fetchOrders(identifier: string) {
        const response = await API.get(`${Endpoint.Order}/project/${identifier}`);
    }
}
</script>

<style lang="scss">

</style>
