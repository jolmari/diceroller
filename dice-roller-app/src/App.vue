<template>
  <div id="main-container">
    <div id="navbar">
      <div id="logo-container">
        <img id="logo" alt="Diceroller logo" src="./assets/logo.png" />
      </div>
    </div>

    <div class="columns">
      <div class="col col-2 col-mx-auto">
        <div class="dice-panel">
          Foobar
        </div>
      </div>
      <div class="col col-2 col-mx-auto">
        <div class="dice-panel">
          Foobar
        </div>
      </div>
      <div class="col col-2 col-mx-auto">
        <div class="dice-panel">
          Foobar
        </div>
      </div>
      <div class="col col-2 col-mx-auto">
        <div class="dice-panel">
          Foobar
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "App",
  components: {},
});
</script>

<style lang="scss">
html {
  width: 100%;
  height: 100%;
}

body {
  width: 100%;
  height: 100%;
  margin: 0;
}

#main-container {
  display: flex;
  height: 100%;
  flex-direction: column;

  & > div#navbar {
    flex: 1;
    height: 5rem;
    margin-bottom: 2rem;
    padding: 0.5rem;
    background-color: black;

    & > div#logo-container {
      display: inline-block;
      width: 5em;
      height: 5em;

      & > img#logo {
        width: 100%;
        height: 100%;
      }
    }
  }

  & div.dice-panel {
    height: 5rem;
    padding: 1rem;
    background-color: red;
  }
}
</style>
