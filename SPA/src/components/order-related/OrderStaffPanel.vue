<template>
    <div class="main">
        <div class="table">

            <div v-bind:class="{ delivered : element.orderInfo.hasBeenEntirelyDelivered }" class="row row-basic"
            v-for="element in orderList" v-bind:key="element.orderInfo.orderId">

                <div id="icon" class="cell">
                    <img src="https://nsa39.casimages.com/img/2018/11/05/181105113413696837.png" />
                </div>

                <div id="orderId" class="cell order-number">
                    N°<span class="bold">{{ element.orderInfo.orderId }}</span>
                </div>

                <div id="userId" class="cell mode-info">
                    <span v-if="element.orderInfo.currentMode == 0">
                        <span v-if="!element.orderInfo.hasBeenEntirelyDelivered">
                            À EMPORTER
                        </span>
                        <span v-else>
                            EMPORTEE
                        </span>
                    </span>
                    <span v-else>
                        <span v-if="!element.orderInfo.hasBeenEntirelyDelivered">
                            À LIVRER
                        </span>
                        <span v-else>
                            LIVREE
                        </span>
                        EN
                    </span>
                    <span class="bold">{{ element.orderInfo.classroomName }}</span>
                </div>

                <div id="products" class="cell products">
                    <div v-for="product in element.products" v-bind:key="product.productId" class="row">
                        <div class="product-cell">
                            <span v-if="product.hasBeenDelivered">
                                <img src="https://nsa39.casimages.com/img/2018/11/05/18110512283063315.png" />
                            </span>
                            <span v-else>
                                <img src="https://nsa39.casimages.com/img/2018/11/05/181105122830113753.png" />
                            </span>
                            |
                            <span class="bold">{{ product.amount }} x</span> 
                            {{ product.price }}€ 
                            | {{ product.name }}
                        </div>
                    </div>
                    <div class="total">
                        <span class="bold">TOTAL</span> {{ element.orderInfo.total }}€
                    </div>
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
        API.get(Endpoint.Order).then( (response) => { this.orders = response.data; });
    }
}
</script>

<style>
    .main {
        margin: 5em;
    }
    .order-number {
        font-size:160%;
    }
    .row-basic {
        width: 25%;
        margin-bottom: 40px;
        padding: 15px;
        background-color: white;
        box-shadow: 3px 3px 10px rgb(197, 197, 197);
    }
    .cell {
        display: inline;
    }
    .product-cell {
        width: 50%;
    }
    .mode-info {
        display: block;
        margin-top: 3px;
        font-size: 115%;
        color: rgb(117, 117, 117);
    }
    .products {
        display: block;
        margin-top: 10px;
    }
    .total {
        margin-top: 10px;
        font-size: 107%;
    }
    .delivered {
        background-color: rgb(202, 236, 202);
    }
</style>
