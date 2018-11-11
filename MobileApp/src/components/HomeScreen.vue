<template>
    <view>
        <button 
          :on-press="handleLetGoBtnPress"
          title="Go to order screen"
        />
    </view>
</template>

<script>
export default {
  props: {
    navigation: {
      type: Object
    }
  },
  methods: {
    handleLetGoBtnPress: function() {
      this.navigation.navigate("Orderscreen");
    }
  }
}
</script>

<style>
.text-color-primary {
  color: blue;
  font-size: 30;
}
</style>