<template>
    <view>
         <scroll-view>
             <text>{{ Message }}</text>
        </scroll-view>
    </view> 
</template>

<script>
import API from '../helpers/Api.js'
import Axios from 'axios';

export default {

    mounted() {
        this.myGetterFunction("order");
    },

    methods: {
        myGetterFunction: function(endpoint) {
            Axios.get("http://192.168.1.31:5000/order" )
            .then(response => {
                this.Message = (response.data);
            });
        },
    },

    data() {
        return {
            Message: [],
            Test: ''
        }
    }
}
</script>

<style>

</style>
