<template>
    <div class="app" id="app">
        <div v-if="errorMsg" class="error">
            {{ errorMsg }}
        </div>
    
        <el-row>
            <a href="#" @click="remoteLogin()">Se connecter via GitHub</a>
        </el-row>

    </div>
</template>

<script>
export default {

    methods: {
        remoteLogin() {
        ApplicationAuthService.instance.startPopupLogin(this.providers.GitHub);
        },
    }
}
</script>

<style>

</style>
