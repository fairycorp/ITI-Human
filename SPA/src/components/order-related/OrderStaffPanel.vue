<template>
    <div class="main">
        <div class="table">

            <div class="row"
            v-for="element in orderList" v-bind:key="element.orderInfo.orderId">

                <div id="orderId" class="cell">
                    Commande N°<span class="bold">{{ element.orderInfo.orderId }}</span>
                </div>

                <div id="userId" class="cell">
                    pour <span class="bold">{{ element.orderInfo.userName }}</span>
                </div>

            </div>

        </div>
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
    .main {
        margin: 5em;
    }
    .row {
        margin-bottom: 40px;
        padding: 15px;
        border: 1px dashed rgb(202, 202, 202);
    }
    .cell {
        display: inline;
        margin-left: 20px;
    }
</style>
