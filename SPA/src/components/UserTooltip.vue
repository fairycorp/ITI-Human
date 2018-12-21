<template>
    <div class="main">
        <div v-if="info" class="tooltip">
            {{ info.userName }}
            <div class="tooltiptext">
                <div class="image">
                    <img :src="info.avatarUrl" width="30%" class="avatar" />
                </div>
                <div class="subinfo">
                    {{ info.firstName }} {{ info.lastName }},<br />
                    <span class="bold">{{ info.schoolStatusName }}</span>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { IDetailedDataUser } from "@/models/model.User";
import API from "@/services/API";
import Endpoint from "@/helpers/Endpoint";

@Component({})
export default class UserTooltip extends Vue {
    @Prop() private userId!: number;
    private info!: IDetailedDataUser | null;

    constructor() {
        super();
        this.info = null;
        this.fetchData();
    }

    private async fetchData() {
        const response =
            await API.get(`${Endpoint.User}/tooltip/${this.userId}`);
        this.info = response.data;

        if (this.info!.avatarUrl == null) this.info!.avatarUrl = 
            "https://image.noelshack.com/fichiers/2018/51/5/1545400783-hd-avatar2.png"
    }
}
</script>

<style>
    /* Tooltip container */
    .tooltip {
        position: relative;
        display: inline-block;
        border-bottom: 1px dotted lightgrey;
    }

    .avatar {
        border-radius: 3px;
    }

    /* Tooltip text */
    .tooltip .tooltiptext {
        visibility: hidden;
        width: 150px;
        background-color: white;
        color: grey;
        text-align: left;
        font-size: 90%;
        padding: 5px;
        border-radius: 6px;
        border: 1px solid rgb(204, 204, 204);
        box-shadow: 0px 4px 5px rgb(192, 192, 192);

        /* Position the tooltip text */
        position: absolute;
        z-index: 1;
        bottom: 125%;
        left: 50%;
        margin-left: -60px;

        /* Fade in tooltip */
        opacity: 0;
        transition: opacity 0.3s;
    }

    /* Tooltip arrow */
    .tooltip .tooltiptext::after {
        content: "";
        position: absolute;
        top: 100%;
        left: 50%;
        margin-left: -5px;
        border-width: 5px;
        border-style: solid;
        border-color: white transparent transparent transparent;
    }

    /* Show the tooltip text when you mouse over the tooltip container */
    .tooltip:hover .tooltiptext {
        visibility: visible;
        opacity: 1;
    }

    div.image {
        position: relative;
        top: 0;
        left: 0;
    }

    div.subinfo {
        position: absolute;
        top: 0;
        right: 17%;
        margin-top: 2px;
    }
</style>
