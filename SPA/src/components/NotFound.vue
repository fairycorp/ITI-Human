<template>
    <div class="main">
        <div class="status-code">404</div>
        <div class="headline">... oups.</div>
        <div class="text">
            Il semblerait que cette page soit introuvable.
        </div>
        <router-link to="/">
            <button class="basic-button return-button">RETOURNER À LA PAGE D'ACCUEIL</button>
        </router-link>
    </div>
</template>

<script>
import { Component, Prop, Vue } from "vue-property-decorator";

@Component()
export default class NotFound extends Vue {
    // ...
}
</script>

<style>
    .status-code {
        position: fixed;
        top: 50%; left: 50%;
        transform: translate(-50%, -170%);
        font-size: 900%;
        color: #01b04b;
        font-weight: lighter;
    }

    .headline {
        position: fixed;
        top: 50%; left: 50%;
        transform: translate(-50%, -220%);
        font-size: 300%;
        color: #5a5a5a;
        font-weight: lighter;
    }

    .text {
        position: fixed;
        top: 50%; left: 50%;
        transform: translate(-50%, -220%);
        font-size: 120%;
    }

    .return-button {
        position: fixed;
        top: 50%; left: 50%;
        transform: translate(-50%, 20%);
    }
</style>
