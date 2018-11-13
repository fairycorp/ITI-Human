<template>
    <view>
         <scroll-view>
             <text>{{ Message }}</text>
        </scroll-view>
    </view> 
</template>

<script>
import Api from '../helpers/Api.js';

export default {

    mounted() {
        this.myGetterFunction("order");
    },

    methods: {
        async myGetterFunction(endpoint) {
            await Api.get(endpoint)
            .then(response => {
                this.Message = (response.data);
            });
        },
    },

    data() {
        return {
            Message: [],
            Test: '',
        }
    }
}
</script>

<style>

</style>
