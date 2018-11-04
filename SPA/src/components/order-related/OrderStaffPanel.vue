<template>
    <div class="main">

    </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { IBasicDataOrder, IDetailedDataOrder } from '@/model/order/OrderModel';
import API from '@/services/API';
import Endpoint from '../../helpers/Endpoint';

@Component
export default class OrderStaffPanel extends Vue {
    private orders: IDetailedDataOrder[];

    /** Gets Order list. */
    public get orderList() { return this.orders; }

    constructor() {
        super();
        // Initializations.
        this.orders = [];
        this.fetchOrderList();
    }

    private async fetchOrderList() {
        API.get(Endpoint.Order).then( (response) => {
            this.orders = response.data;

        }).catch( (error) => {
            console.log(error);
        });
    }
}
</script>

<style>

</style>
