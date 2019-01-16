<template>
    <div class="global-content">
        <div @click="changeRoute('hungry')" class="return-arrow"><img width="20" src="../assets/images/return-arrow.png" /></div>
        <div class="left-page">
            <h1 class="specialtitle">Bienvenue sur notre carte</h1>
            <h3 class="specialtitle">Où voici donc nos produits</h3>
            <div @click="addProduct(product)" v-for="product in linkedProducts" :key="product.storageLinkedProductId" class="product">
                <div class="bar"></div>
                <p>
                    <span class="openSans-bold">{{ product.productName }}</span>, <span class="price">{{ product.unitPrice / 100 }}€</span>,
                    <span class="description">{{ product.productDesc.toLowerCase() }}</span>
                    <br />
                    <span class="product-info"><span class="openSans-bold">{{ product.stock }}</span> produits restants.</span>
                </p>
            </div>
        </div>
        <div v-if="WINDOW_ORDER && displayedOrderProducts != null && displayedOrderProducts.length > 0" class="right-page">
            <div @click="closeWindow()" class="cross">x</div>
            <h1 class="specialtitle">Votre commande</h1>
            <h3>Où voici ce que vous avez pris</h3>
            <div v-for="(product) in displayedOrderProducts" :key="product.storageLinkedProductId" class="ordered">
                <img width="50" class="pictureproduct" :src="product.productAvatarUrl" />
                <span class="quantity"><span class="grey">x </span><span class="openSans-bold">{{ product.quantity }}</span>
                <span class="grey"> ({{ product.quantity * product.unitPrice / 100 }} €)</span></span>
            </div>
            <div class="totalPrice medium-top-margin">TOTAL <span class="openSans-bold">{{ totalPrice / 100 }} €</span>,<br />
                <span v-if="selectedClassroom.classroomId === 0" class="openSans-bold">À emporter</span>
                <span v-else>Livrée en <span class="openSans-bold">{{ selectedClassroom.name }}</span></span>.
            </div>

            <h3 class="medium-top-margin">Où voulez-vous être livré(e) ?</h3>
            <button
                @click="selectClassroom(classroom)"
                class="selection light-top-margin light-right-margin"
                :class="{ selectedButton : selectedClassroom.classroomId === classroom.classroomId }"
                v-for="classroom in classrooms" :key="classroom.classroomId">
                {{ classroom.name }}
            </button>

            <div
                class="submit-button special">
                <div
                    @click="order()"
                    class="submit-button-text unselectable-text">
                    C'EST PARTI, MON JACQUIE.
                </div>
            </div>
        </div>

        <div v-if="displayOrderedMessage" class="orderedMessage"></div>
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
export default class Order extends Vue {
    @Prop() private authService!: AuthService;
    @Prop() private linkedProducts!: IBasicDataStorageLinkedProduct[];
    @Prop() private currentOrder!: IOrderCreationViewModel;
    private displayedOrderProducts: IBasicDataStorageLinkedProductInWayToBeOrdered[] = [];
    private classrooms: IBasicDataClassroom[] = [];
    private selectedClassroom!: IBasicDataClassroom;
    private totalPrice: number = 0;
    private WINDOW_ORDER: boolean = false;
    private displayOrderedMessage: boolean = false;

    constructor() {
        super();
        this.isAccessible();
        this.fetchClassrooms();
        this.selectedClassroom = {
            classroomId: 0,
            name: ""
        };
    }
    
    mounted() {
        this.fetchProject(this.$route.params.id);
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

    // ROUTING METHODS.
    private changeRoute(route: string) {
        this.$router.back();
    }

    private async fetchProject(identifier: string) {
        const response = await API.get(`${Endpoint.Storage}/products/from/${identifier}`);
        this.linkedProducts = response.data;
    }

    private async fetchClassrooms() {
        const response = await API.get(`${Endpoint.Classroom}`);
        this.classrooms = response.data;

        this.classrooms.forEach( (classroom) => {
            if (classroom.classroomId === 0) {
                classroom.name = "TWAY";
            }
        });
    }

    private async addProduct(product: IBasicDataStorageLinkedProduct) {
        this.displayOrderedMessage = false;
        this.WINDOW_ORDER = true;
        let add: boolean = true;
        this.totalPrice += product.unitPrice;
        this.displayedOrderProducts.forEach( (displayed) => {
            if (displayed.storageLinkedProductId === product.storageLinkedProductId) {
                displayed.quantity += 1;
                add = false;
            }
        });
        if (add) {
            this.displayedOrderProducts.push({
                storageLinkedProductId: product.storageLinkedProductId,
                storageId: product.storageId,
                productId: product.productId,
                productName: product.productName,
                productDesc: product.productDesc,
                productAvatarUrl: product.productAvatarUrl,
                unitPrice: product.unitPrice,
                stock: product.stock,
                creditState: product.creditState,
                quantity: 1
            });
            return;
        }
    }

    private selectClassroom(classroom: IBasicDataClassroom) {
        this.selectedClassroom = classroom;
    }

    private async order() {
        const products: IBasicDataProductToOrder[] = [];
        this.displayedOrderProducts.forEach( (displayed) => {
            products.push({
                storageLinkedProductId: displayed.storageLinkedProductId,
                quantity: displayed.quantity
            });
        });

        const payload: IOrderCreationViewModel = {
            userId: this.authService.authenticationInfo.user.userId,
            storageId: parseInt(this.$route.params.id),
            classroomId: this.selectedClassroom.classroomId,
            products: products
        };
        
        const response = await API.post(`${Endpoint.Order}/create`, payload);
        if (response.data) {
            this.WINDOW_ORDER = false;
            this.displayOrderedMessage = true;
            this.displayedOrderProducts = [];
            this.selectedClassroom = {
                classroomId: 0,
                name: ""
            };
            await this.fetchProject(this.$route.params.id);
        }
    }

    private async closeWindow() {
        this.WINDOW_ORDER = false;
        this.displayedOrderProducts = [];
        this.selectedClassroom = {
                classroomId: 0,
                name: ""
            };
        await this.fetchProject(this.$route.params.id);
    }
}
</script>

<style lang="scss">
.totalPrice {
    font-size: 140%;
}

.grey {
    color: #9e9e9e;
}

h1.specialtitle {
    font-family: "Script";
    font-size: 220%;
}

.product {
    font-size: 90%;
    margin-bottom: 15px;
    cursor: pointer;
}

.product > p {
    margin-left: 12px;
    color: black;
    user-select: none;
}

.product:hover > .bar {
    opacity: 1;
}

.description {
    color: #757575;
}

.product-info {
    font-size: 80%;
    color: #757575;
}

.price {
    color: #4b80ac;
}

.bar {
    opacity: 0;
    position: absolute;
    height: 40px;
    width: 5px;
    background-color: black;

    transition-property: opacity;
    transition-duration: 0.2s;
}

.pictureproduct {
    vertical-align: middle;
}

.quantity {
    font-size: 130%;
}

.ordered {
    margin-bottom: 10px;
}

.orderedMessage {
    position: absolute;
    top: 330px;
    right: 300px;
    width: 343px;
    height: 230px;
    background-image: url("../assets/images/preparing-order.png");
}
</style>
