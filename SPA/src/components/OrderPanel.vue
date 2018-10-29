<template>
    <div id="main">
        <div id="wip-message">
            <span class="bold">Projet en cours |</span> Visionnage des commandes effectuées (en cours et terminées).
        </div>
        <button @click="loadInfo()">load info</button>
    </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import API from '../services/API';

@Component
export default class OrderPanel extends Vue {
    private componentEndpoint: string;

    private get endpoint() { return this.componentEndpoint; }

    public constructor() {
        super();
        this.componentEndpoint = 'order';
    }

    /**
     * Loads all Orders info.
     */
    private loadInfo(): void {
        API.get(this.endpoint).then( (response) => {
            console.log(response.data);
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

.bold {
    font-weight: bold;
}
</style>

