<template>
    <div class="main">
        <div class="tooltip-example">
            <UserTooltip :userId="5" /><br />
            <UserTooltip :userId="2" /><br />
            <UserTooltip :userId="3" /><br />
        </div>
    </div>
</template>

<script>
import { Component, Prop, Vue } from "vue-property-decorator";
import UserTooltip from "@/components/UserTooltip.vue";

@Component({
    components: {
        UserTooltip
    }
})
export default class Home extends Vue {
    // ...
}
</script>

<style>
    .tooltip-example {
        margin-top: 60px;
        margin-left: 100px;
    }
</style>
