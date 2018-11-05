<template>
  <div id="app">
    <router-view />
  </div>
</template>

<style lang="scss">
  html {
    margin: 0;
    background: radial-gradient(circle at top, #ffffff, #d7d7d7);
    background-attachment: fixed;
  }

  #app {
    font-family: 'Yu Gothic UI';
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    color: #313538;
  }

  .bold {
    font-weight: bold;
  }

  .small-title {
    display: block;
    font-size: 120%;
    font-weight: bold;
  }

  .informative {
    display: block;
    font-size: 90%;
    color: #5d6063;
  }

  .subinformative {
    display: block;
    margin-top: 10px;
    font-size: 75%;
    color: #828588;
  }

  .basic-button {
      margin: 10px;
      padding-top: 0.5em;
      padding-bottom: 0.5em;
      padding-left: 0.8em;
      padding-right: 0.8em;
      border: none;
      border-radius: 3px;
      background-color: #ffffff;
      box-shadow: 0px 4px 6px rgb(204, 204, 204);
      font-family: 'Yu Gothic UI';
      font-size: 100%;
      font-weight: bold;
      color: #01b04b;

      transition-property: background-color, color, box-shadow;
      transition-duration: 0.18s;
  }

  .basic-button:hover {
      background-color: #01b04b;
      color: white;
      cursor: pointer;
  }

  .basic-button:focus {
      background-color: #096e33;
      color: white;
  }
</style>
