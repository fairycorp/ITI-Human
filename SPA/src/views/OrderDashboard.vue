<template>
    <div class="global-content">
        <div @click="closeStall()" class="return-arrow"><img width="20" src="../assets/images/return-arrow.png" /></div>
        <div class="left-page">
            <h1>Réception</h1>
            <h3 class="light-top-margin">LISTE DES COMMANDES</h3>
            <div
            v-for="order in orders" :key="order.info.orderId"
            @click="openNewOrder(order)"
            class="orderlist"
            :class="{ green: order.info.currentState == 3, red: order.info.currentState == 0, yellow: order.info.currentState == 1 }">
                <div :class="{ visible: displayedOrder != null && displayedOrder.info.orderId == order.info.orderId }" class="bar"></div>
                <p>
                    Commande n°<span class="openSans-bold">{{ order.info.orderId }}</span>, par <span class="openSans-bold">{{ order.info.userName }}</span><br />
                    <span class="articles"><span class="openSans-bold">{{ order.products.length }}</span> articles</span><br />
                    <span class="little-grey">Effectuée le {{ order.info.displayedDate }}</span>
                </p>
            </div>
        </div>
        <div v-if="displayedOrder != null && displayedOrder != undefined" class="right-page">
            <div @click="closeOrder()" class="cross">x</div>
            <span class="title">COMMANDE N°{{ displayedOrder.info.orderId }}</span>
            <div class="light-top-margin orderinfo">
                par <span class="openSans-bold">{{ displayedOrder.info.userName }}</span>,<span class="grey"> {{ displayedOrder.info.displayedDate }}</span>
            </div>
            <div class="medium-top-margin">
                <div v-for="product in displayedOrder.products" :key="product.orderedProductId" class="singleproduct">
                    <span class="bigger openSans-bold">{{ product.name }}</span> x {{ product.quantity }}
                    <div @click="changePaymentState(product)"
                    :class="{ unpaid: product.payment.state == 0, paid: product.payment.state == 1 && !(product.payment.state == 1 && product.currentState == 3), credited: product.payment.state == 2 && !(product.payment.state == 1 && product.currentState == 3), nonchangable: product.payment.state == 1 && product.currentState == 3 }">
                        <span v-if="product.payment.state == 0">Impayé</span>
                        <span v-else-if="product.payment.state == 1">Payé</span>
                        <span v-else-if="product.payment.state == 2">Crédit</span>
                    </div>

                    <div @click="changeCurrentState(product)" :class="{ paid: product.currentState == 3 && !(product.payment.state == 1 && product.currentState == 3), preparing: product.currentState == 1 && !(product.payment.state == 1 && product.currentState == 3), nonchangable: product.payment.state == 1 && product.currentState == 3 }">
                        <span v-if="product.currentState == 1">En préparation</span>
                        <span v-else-if="product.currentState == 3">Livré</span>
                    </div>
                </div>
            </div>
        </div>
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
IBasicDataStorageLinkedProductInWayToBeOrdered, 
IStallUpdateViewModel} from '@/models/model.Storage';
import { IOrderCreationViewModel, IBasicDataProductToOrder, IDetailedDataOrder, IOrderCurrentStateUpdateViewModel, State, PaymentState, IBasicDataOrderedProduct, Payment, IPaymentStateUpdateViewModel, ICurrentStateUpdateViewModel } from "@/models/model.Order";
import { IBasicDataClassroom } from '@/models/model.Classroom';

@Component({})
export default class OrderDashboard extends Vue {
    @Prop() private authService!: AuthService;
    @Prop() private orders: IDetailedDataOrder[] = [];
    @Prop() private displayedOrder: IDetailedDataOrder | null = null;

    constructor() {
        super();
        this.isAccessible();
    }

    mounted() {
        this.fetchOrders(this.$route.params.id);
    }

    // ROUTING METHODS.
    private changeRoute(route: string) {
        this.$router.push(route);
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
        this.orders = response.data;

        this.orders.forEach( async (order, idx) => {
            const date: Date = new Date(order.info.creationDate);
            order.info.displayedDate =
                `le ${date.getUTCDate()}/${date.getUTCMonth() + 1}/${date.getUTCFullYear()} à ${date.getUTCHours()}:${date.getUTCMinutes()}`;

            let fullyCompleted: boolean = true;
            order.products.forEach( (product) => {
                if (!(product.payment.state == Payment.Paid && product.currentState == State.Delivered)) {
                    fullyCompleted = false;
                }
            });
            if (fullyCompleted) {
                const payload: IOrderCurrentStateUpdateViewModel = {
                    userId: this.authService.authenticationInfo.user.userId,
                    orderId: order.info.orderId,
                    currentState: State.Delivered
                };
                const response = await API.put(`${Endpoint.Order}/currentState`, payload);
            }
        });

        if (this.displayedOrder != null) {
            this.orders.forEach( (order) => {
                if (order.info.orderId === this.displayedOrder!.info.orderId) {
                    this.displayedOrder = order;
                    return;
                }
            });          
        }
    }
    
    private async closeStall() {
        const payload: IStallUpdateViewModel = {
            userId: this.authService.authenticationInfo.user.userId,
            storageId: parseInt(this.$route.params.id),
            openState: false
        };
        const response = await API.put(`${Endpoint.Storage}/stall`, payload);

        this.$router.back();
    }

    private async openNewOrder(order: IDetailedDataOrder) {
        this.displayedOrder = order;
        
        const payload: IOrderCurrentStateUpdateViewModel = {
            userId: this.authService.authenticationInfo.user.userId,
            orderId: order.info.orderId,
            currentState: State.Underway
        };
        const response = await API.put(`${Endpoint.Order}/currentState`, payload);

        this.displayedOrder.products.forEach( async (product) => {
            const payload: ICurrentStateUpdateViewModel[] = [];
            payload.push({
                userId: this.authService.authenticationInfo.user.userId,
                orderedProductId: product.orderedProductId,
                currentState: State.Underway
            });
            await API.put(`${Endpoint.Order}/ordered/currentState`, payload);
            await this.fetchOrders(this.$route.params.id);
        });
    }

    private async closeOrder() {
        this.displayedOrder = null;
        this.fetchOrders(this.$route.params.id);
    }

    private async changePaymentState(product: IBasicDataOrderedProduct) {
        if (product.currentState == 3 && product.payment.state == 1) return;

        const payload: IPaymentStateUpdateViewModel[] = [];
        let state: PaymentState = {
            state: Payment.Unpaid,
            amount: 0
        };
        if (product.payment.state == Payment.Unpaid) {
            state.state = Payment.Paid;
        } else if (product.payment.state == Payment.Paid) {
            state.state = Payment.Unpaid;
        }

        payload.push({
            userId: this.authService.authenticationInfo.user.userId,
            orderedProductId: product.orderedProductId,
            paymentState: state
        });
        await API.put(`${Endpoint.Order}/ordered/paymentState`, payload);
        await this.fetchOrders(this.$route.params.id);
    }

    private async changeCurrentState(product: IBasicDataOrderedProduct) {
        if (product.currentState == 3 && product.payment.state == 1) return;

        const payload: ICurrentStateUpdateViewModel[] = [];
        let state: State = State.NotStarted;
        if (product.currentState == State.Underway) {
            state = State.Delivered;
        } else if (product.currentState == State.Delivered) {
            state = State.Underway;
        }

        payload.push({
            userId: this.authService.authenticationInfo.user.userId,
            orderedProductId: product.orderedProductId,
            currentState: state
        });
        await API.put(`${Endpoint.Order}/ordered/currentState`, payload);
        await this.fetchOrders(this.$route.params.id);
    }
}
</script>

<style lang="scss">
.nonchangable {
    margin-top: 5px;
    padding: 5px;
    border-radius: 3px;
    padding: 10px;
    background-color: #cacaca;
    user-select: none;
}

.bigger {
    font-size: 110%;
}

.orderlist {
    font-size: 90%;
    margin-bottom: 15px;
    padding: 1px;
    cursor: pointer;
}

.orderlist > p {
    margin-left: 12px;
    color: black;
    user-select: none;
}

.orderlist:hover > .bar {
    opacity: 1;
}

.bar {
    opacity: 0;
    position: absolute;
    height: 90px;
    width: 5px;
    background-color: black;

    transition-property: opacity;
    transition-duration: 0.2s;
}

.bar.visible {
    opacity: 1;
}

.little-grey {
    font-size: 85%;
    color: grey;
}

.green {
    border-radius: 5px;
    background-color: #d4ffd7;
}

.red {
    border-radius: 5px;
    background-color: #ffd4d4;
}

.yellow {
    border-radius: 5px;
    background-color: #fffed4;
}

.articles {
    font-size: 90%;
}

.orderinfo {
    font-size: 130%;
}

.singleproduct {
    padding: 10px;
    border-radius: 5px;
    background-color: white;
    color: black;
    border: 1px dashed black;
    margin-bottom: 20px;
}

.unpaid {
    margin-top: 5px;
    padding: 10px;
    border-radius: 3px;
    background-color: #ffd4d4;
    user-select: none;
    cursor: pointer;

    transition-property: background-color;
    transition-duration: 0.2s;
}

.unpaid:hover {
    background-color: #e0b4b4;
}

.paid {
    margin-top: 5px;
    padding: 5px;
    border-radius: 3px;
    padding: 10px;
    background-color: #d5ffd4;
    user-select: none;
    cursor: pointer;

    transition-property: background-color;
    transition-duration: 0.2s;
}

.preparing {
    margin-top: 5px;
    padding: 5px;
    border-radius: 3px;
    padding: 10px;
    background-color: #fffed4;
    user-select: none;
    cursor: pointer;

    transition-property: background-color;
    transition-duration: 0.2s;
}

.preparing:hover {
    background-color: #d6d6b1;
}

.paid:hover {
    background-color: #b9dab8;
}

.credited {
    margin-top: 5px;
    padding: 10px;
    border-radius: 3px;
    background-color: #dadada;
}
</style>
