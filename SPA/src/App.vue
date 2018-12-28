<template>
    <div id="global-view">
      <router-view />
    </div>
</template>

<style lang="scss">
$font-dir: "../src/assets/fonts";
$image-dir: "../src/assets/images";

/* Imports several fonts. */
@font-face {
  font-family: "gotham-bold";
  src: url("#{$font-dir}/gotham-bold.otf");
  font-weight: normal;
}

@font-face {
  font-family: "OpenSans-light";
  src: url("#{$font-dir}/openSans-light.ttf");
  font-weight: normal;
}

@font-face {
  font-family: "OpenSans-litalic";
  src: url("#{$font-dir}/openSans-litalic.ttf");
  font-weight: normal;
}

@font-face {
  font-family: "OpenSans-regular";
  src: url("#{$font-dir}/openSans-regular.ttf");
  font-weight: normal;
}

@font-face {
  font-family: "OpenSans-italic";
  src: url("#{$font-dir}/openSans-italic.ttf");
  font-weight: normal;
}

@font-face {
  font-family: "OpenSans-semiBold";
  src: url("#{$font-dir}/openSans-semiBold.ttf");
  font-weight: normal;
}

/* Defines different animation styles. */
@keyframes fadein {
  from { opacity: 0; }
  to { opacity: 1; }
}

/* Sets default background with default page properties. */
body {
  width: 100%;
  height: 100%;
  margin: 0;
  background-image: url("#{$image-dir}/page-background.jpg");
  background-attachment: fixed;

  -webkit-animation: fadein 1s;
  -moz-animation: fadein 1s;
  -ms-animation: fadein 1s;
  -o-animation: fadein 1s;
  animation: fadein 1s;
}


/* Sets default font as Open Sans (regular shape). */
* {
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  font-family: "OpenSans-regular"
}

h1 {
  margin: 0;
  padding: 0;
}

h2 {
  font-family: "gotham-bold";
  font-size: 130%;
  letter-spacing: 5px;
  color: #3e4b5f;
}

h3 {
  font-family: "gotham-bold";
  font-size: 110%;
  letter-spacing: 3px;
  color: #3e4b5f;
}

p {
  font-size: 102%;
  text-align: justify;
  color: #6c6c6c;
}

input.textual {
  width: 97%;
  padding: 10px;
  border: none;
  border-bottom: 1px solid #c8c7c8;
  font-family: "OpenSans-light";
  font-size: 105%;

  transition-property: border-bottom;
  transition-duration: 0.2s;
}

input.textual::placeholder { 
  color: #c8c7c8;
  transition-property: opacity;
  transition-duration: 0.2s;
}

input.textual:hover { 
  border-bottom: 1px solid #a7a7a7; 
}

input.textual:focus {
  outline-width: 0;
  border-bottom: 1px solid #a7a7a7;
}

input.textual:focus::placeholder { 
  opacity: 0; 
}

input.textual.error {
  border-bottom: 1px solid #b86565;
}

button.standard {
  outline-width: 0;
  width: 166px;
  height: 45px;
  border-radius: 25px;
  background-color: white;
  border: 1px solid black;
  font-family: "gotham-bold";
  color: black;
  
  transition-duration: 0.2s;
  transition-property: background-color, color;
}

button.selection {
  outline-width: 0;
  width: 80px;
  height: 50px;
  border-radius: 25px;
  background-color: white;
  border: 1px solid black;
  font-family: "gotham-bold";
  color: black;
  
  transition-duration: 0.2s;
  transition-property: background-color, color;
}

button.selection.error {
  border: 1px solid #b86565;
}

button.standard:hover, button.selection:hover, button.selection.error:hover {
  background-color: black;
  color: white;
  cursor: pointer;
}

button.selectedButton {
  background-color: black;
  color: white;
}

#global-view {
  width: 100%;
  height: 100%;
}

.left-page {
  position: fixed;
  top: 3%;
  left: 4%;
  width: 36.8%;
  height: 81.3%;
  padding: 70px;
  background-color: white;
}

.light-left-page {
  position: fixed;
  top: 3%;
  left: 4%;
  width: 37%;
  height: 41%;
  padding: 70px;
  background-color: white;
}

.right-page {
  position: fixed;
  top: 3%;
  right: 4%;
  width: 36.85%;
  height: 81.5%;
  padding: 70px;
  background-color: white;
}

.submit-button {
  width: 97.7%;
  padding: 8px;
  background-color: black;
  color: white;
  cursor: pointer;

  transition-property: background-color, color;
  transition-duration: 0.1s;
}

.submit-button:active {
  background-color: white;
  color: black;
}

.submit-button .submit-button-text {
  font-family: "OpenSans-semiBold";
  text-align: right;
}

.lightly-reduced-bottom-margin {
  margin-bottom: -10px;
}

.light-top-margin {
  margin-top: 15px;
}

.medium-top-margin {
  margin-top: 30px;
}

.high-top-margin {
  margin-top: 60px;
}

.light-right-margin {
  margin-right: 15px;
}

.medium-right-margin {
  margin-right: 30px;
}

.high-right-margin {
  margin-right: 60px;
}

.openSans-bold {
  font-family: "OpenSans-semiBold";
}

.openSans-litalic {
  font-family: "OpenSans-litalic";
}

.unselectable-text {
  user-select: none;
}
</style>