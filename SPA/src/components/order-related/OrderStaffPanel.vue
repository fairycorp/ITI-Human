<template>
    <div class="main">

    </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { IBasicDataOrder, IDetailedDataOrder } from '@/model/order/OrderModel';
import Endpoint from '../../helpers/Endpoint';
import API from '@/services/API';

@Component
export default class OrderStaffPanel extends Vue {
    private orders: IDetailedDataOrder[];

    /** Gets Order list. */
    public get orderList() { return this.orders; }

    constructor() {
        super();
        // Initializations.
        this.orders = [];
        this.loadOrderList();
    }

    private async loadOrderList() {
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
