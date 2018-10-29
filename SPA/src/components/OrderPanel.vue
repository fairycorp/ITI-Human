<template>
    <div id="main">
        <div id="wip-message">
            <span class="bold">Projet en cours |</span> Visionnage des commandes effectuées (en cours et terminées).
        </div>
        <div id="content">
            <div id="order" v-for="order in orders" v-bind:key="order.orderInfo.orderId">
                <div id="info">
                    <span class="title">Order n°{{order.orderInfo.orderId}}</span><br />
                    <span class="little">Made by User n°{{order.orderInfo.userId}}, the {{order.orderInfo.creationDate}}.</span>
                </div>
                <br />
                <div id="products">
                    Commandé<span v-if="order.products.length > 1">s</span>
                    <div id="product" v-for="product in order.products" v-bind:key="product.orderedProductId">
                        {{product.orderedProductId}} / {{product.name}}
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import API from '../services/API';
import { IDetailedDataOrder } from '../model/Order/OrderModel';
import { AxiosResponse } from 'axios';

@Component
export default class OrderPanel extends Vue {
    private componentEndpoint: string;
    private ordersInformation: IDetailedDataOrder | null;

    private get endpoint(): string { return this.componentEndpoint; }
    private get orders(): IDetailedDataOrder | null { return this.ordersInformation; }

    public constructor() {
        super();
        // Attributions.
        this.componentEndpoint = 'order';
        this.ordersInformation = null;

        // Method call.
        this.loadInfo(this.endpoint);
    }

    /** Loads all Orders info. */
    private loadInfo(endpoint: string): void {
        API.get(endpoint).then( (response) => {
            this.ordersInformation = response.data;
        });
    }
}
</script>

<style>
#wip-message {
    background-color: rgb(243, 213, 41);
    padding: 10px;
    text-align: center;
    font-size: 80%;
}

#content {
    padding: 25px;
}

#order {
    width: 400px;
    padding: 15px;
    background-color: rgb(238, 238, 238);
    margin-bottom: 25px;
    box-shadow: 0px 0px 15px rgb(238, 238, 238);
}

.title {
    font-size: 120%;
}

.bold {
    font-weight: bold;
}

.little {
    font-size: 90%;
}
</style>

