<template>
      <button 
      :on-press="handleLetGoBtnPress"
      title="Go to order screen"
      />

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
      this.navigation.navigate("Anotherscreen");
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