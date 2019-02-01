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
            :class="{ 
                red: order.info.currentState == 0, 
                yellow: order.info.currentState == 1 || (order.info.currentState == 3 && order.info.fullyPaid == false),
                green: order.info.currentState == 3 && order.info.fullyPaid == true
            }">
                <div :class="{ visible: displayedOrder != null && displayedOrder.info.orderId == order.info.orderId }" class="bar"></div>
                <p>
                    Commande n°<span class="openSans-bold">{{ order.info.orderId }}</span>, par <span class="openSans-bold">{{ order.info.userName }}</span><br />
                    <span class="articles"><span class="openSans-bold">{{ order.products.length }}</span> article<span v-if="order.products.length > 1">s</span></span><br />
                    <span class="little-grey">Effectuée le {{ order.info.displayedDate }}</span>
                </p>
            </div>
            <div v-if="orders == null || orders == undefined || orders.length == 0">
                <span class="little-grey">Aucune commande reçue à l'heure actuelle.</span>
            </div>
        </div>
        <div v-if="displayedOrder != null && displayedOrder != undefined" class="right-page">
            <div @click="closeOrder()" class="cross">x</div>
            <span class="title">COMMANDE N°{{ displayedOrder.info.orderId }}</span>
            <button v-if="displayedOrder.info.currentState != 3" @click="cancelOrder(displayedOrder.info.orderId)" class="cancel-button">ANNULER</button>
            <div class="light-top-margin orderinfo">
                <span class="openSans-bold">TOTAL {{ displayedOrder.info.total / 100 }}€</span> | 
                <span class="little-grey">À <span v-if="displayedOrder.info.classroomId == 0">emporter</span><span v-else>livrer en {{ displayedOrder.info.classroomName }}</span></span>
                <br />
                par <span class="openSans-bold">{{ displayedOrder.info.userName }}</span>,<span class="grey"> {{ displayedOrder.info.displayedDate }}</span>
            </div>
            <div class="medium-top-margin">
                <div v-for="product in displayedOrder.products" :key="product.orderedProductId" class="singleproduct">
                    <div v-if="product.payment.state != 2" class="cancel-order">
                        <span v-if="product.currentState != 3 && (product.payment.state != 2)" class="credit-text" @click="creditProduct(product)">CREDIT</span>
                        <span v-if="product.currentState != 3 && (product.payment.state != 2)">|</span>
                        <span v-if="product.currentState != 3 && (product.payment.state != 2)" @click="cancelProduct(product)" class="red-text">ANNULER</span></div>
                    <span class="bigger openSans-bold">{{ product.name }}</span> x {{ product.quantity }}
                    <div @click="changePaymentState(product)"
                        :class="{ unpaid: product.payment.state == 0, 
                        paid: product.payment.state == 1 && !(product.payment.state == 1 && product.currentState == 3), 
                        credited: product.payment.state == 2 && !(product.payment.state == 1 && product.currentState == 3),
                        nonchangable: product.payment.state == 1 && product.currentState == 3 || (product.payment.state == 2 && product.currentState == 3) }">
                        <span v-if="product.payment.state == 0">Impayé</span>
                        <span v-else-if="product.payment.state == 1">Payé</span>
                        <span v-else-if="product.payment.state == 2">Crédité</span>
                    </div>

                    <div @click="changeCurrentState(product)"
                        :class="{ paid: product.currentState == 3 && !(product.payment.state == 1 && product.currentState == 3) && !(product.payment.state == 2 && product.currentState == 3),
                        preparing: product.currentState == 1 && !(product.payment.state == 1 && product.currentState == 3),
                        nonchangable: (product.payment.state == 1 && product.currentState == 3) || (product.payment.state == 2 && product.currentState == 3) }">
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
                if (!((product.payment.state == Payment.Paid || product.payment.state == Payment.Credited) && product.currentState == State.Delivered)) {
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

            order.info.total = 0;
            order.products.forEach( (product) => {
                order.info.total += product.unitPrice * product.quantity;
            });
        });

        if (this.displayedOrder != null) {
            this.orders.forEach( (order) => {
                if (order.info.orderId === this.displayedOrder!.info.orderId) {
                    this.displayedOrder = order;
                    let fullyCompleted = true;
                    this.displayedOrder!.products.forEach( (product) => {
                        if (product.currentState != 3) {
                            fullyCompleted = false;
                        }
                        if (product.payment.state == Payment.Unpaid) {
                            this.displayedOrder!.info.fullyPaid = false;
                        }
                        else if (product.payment.state == Payment.Paid || product.payment.state == Payment.Credited) {
                            this.displayedOrder!.info.fullyPaid = true;
                        }
                    });
                    if (fullyCompleted) {
                        this.displayedOrder!.info.currentState = 3;
                    }
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

    private async cancelOrder(orderId: number) {
        const payload: IOrderCurrentStateUpdateViewModel = {
            userId: this.authService.authenticationInfo.user.userId,
            orderId: orderId,
            currentState: State.Canceled
        };
        const response = await API.put(`${Endpoint.Order}/currentState`, payload);
        if (response.data) {
            this.closeOrder();
        }
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
            actorId: this.authService.authenticationInfo.user.userId,
            userId: this.displayedOrder!.info.userId,
            orderedProductId: product.orderedProductId,
            paymentState: state
        });
        await API.put(`${Endpoint.Order}/ordered/paymentState`, payload);
        await this.fetchOrders(this.$route.params.id);
    }

    private async changeCurrentState(product: IBasicDataOrderedProduct) {
        if (product.currentState == 3 && (product.payment.state == 1 || product.payment.state == 2)) return;

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

    private async creditProduct(product: IBasicDataOrderedProduct) {
        const payload: IPaymentStateUpdateViewModel[] = [];
        payload.push({
            actorId: this.authService.authenticationInfo.user.userId,
            userId: this.displayedOrder!.info.userId,
            orderedProductId: product.orderedProductId,
            paymentState: {
                state: Payment.Credited,
                amount: product.unitPrice * product.quantity
            }
        });
        await API.put(`${Endpoint.Order}/ordered/paymentState`, payload);
        await this.fetchOrders(this.$route.params.id);
    }

    private async cancelProduct(product: IBasicDataOrderedProduct) {
        const payload: ICurrentStateUpdateViewModel[] = [];
        payload.push({
            userId: this.authService.authenticationInfo.user.userId,
            orderedProductId: product.orderedProductId,
            currentState: State.Canceled
        });
        await API.put(`${Endpoint.Order}/ordered/currentState`, payload);

        if (this.displayedOrder!.products.length == 1) {
            this.cancelOrder(product.orderId);
        }
        else await this.fetchOrders(this.$route.params.id);
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

.cancel-button {
    outline-width: 0;
    width: 166px;
    height: 45px;
    border-radius: 25px;
    float: right;
    background-color: white;
    border: 1px solid #c71414;
    font-family: "gotham-bold";
    color: #c71414;
    cursor: pointer;

    transition-duration: 0.2s;
    transition-property: background-color, color;
}

.cancel-button:hover {
    background-color: #c71414;
    color: white;
}

.cancel-order {
    text-align: right;
    float: right;
    user-select: none;
    font-size: 80%;
    font-weight: bold;
}

.red-text {
    color: #c71414;
    cursor: pointer;
}

.credit-text {
    cursor: pointer;
}
</style>
