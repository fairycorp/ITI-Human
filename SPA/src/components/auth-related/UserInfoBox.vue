<template>
  <div>
    <slot v-if="checkAuth(0)" name="None"></slot>
    <slot v-if="checkAuth(1)" name="Unsafe"></slot>
    <slot v-if="checkAuth(2)" name="Normal"></slot>
    <slot v-if="checkAuth(3)"></slot>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Watch, Vue } from "vue-property-decorator";
import { AuthLevel, AuthService } from "@signature/webfrontauth";

@Component
export default class UserInfoBox extends Vue {
    @Prop( {
        required: true,
    } )
    private authService!: AuthService;
    public constructor() {
        super();
        console.log( this.authService.authenticationInfo.level );
        if ( this.authService === undefined || this.authService === null ) {
            throw new Error( "AuthService is null" );
        }
    }
    public checkAuth( level: number ): boolean {
        return level === this.authService.authenticationInfo.level;
    }
}
</script>