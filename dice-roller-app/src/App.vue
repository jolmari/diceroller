<template>
  <div id="main-container">
    <div id="navbar">
      <div id="logo-container">
        <img id="logo" alt="Diceroller logo" src="./assets/logo.png" />
      </div>
    </div>

    <div class="columns">
      <div class="col col-1 col-mx-auto">
        <div class="dice-panel">
          <div class="dice-icon">
            <img src="./assets/d4-icon.png" alt="1d4" class="dice" />
          </div>
          <label for="d4-input"><b>1d4</b></label>
          <input id="d4-input" name="d4-input" type="number" placeholder="0" />
        </div>
      </div>
      <div class="col col-1 col-mx-auto">
        <div class="dice-panel">
          <div class="dice-icon">
            <img src="./assets/d6-icon.png" alt="1d6" class="dice" />
          </div>
          <label for="d6-input"><b>1d6</b></label>
          <input id="d6-input" name="d6-input" type="number" placeholder="0" />
        </div>
      </div>
      <div class="col col-1 col-mx-auto">
        <div class="dice-panel">
          <div class="dice-icon">
            <img src="./assets/d8-icon.png" alt="1d8" class="dice" />
          </div>
          <label for="d8-input"><b>1d8</b></label>
          <input id="d8-input" name="d8-input" type="number" placeholder="0" />
        </div>
      </div>
      <div class="col col-1 col-mx-auto">
        <div class="dice-panel">
          <div class="dice-icon">
            <img src="./assets/d10-icon.png" alt="1d10" class="dice" />
          </div>
          <label for="d10-input"><b>1d10</b></label>
          <input
            id="d10-input"
            name="d10-input"
            type="number"
            placeholder="0"
          />
        </div>
      </div>
      <div class="col col-1 col-mx-auto">
        <div class="dice-panel">
          <div class="dice-icon">
            <img src="./assets/d12-icon.png" alt="1d12" class="dice" />
          </div>
          <label for="d12-input"><b>1d12</b></label>
          <input
            id="d12-input"
            name="d12-input"
            type="number"
            placeholder="0"
          />
        </div>
      </div>
      <div class="col col-1 col-mx-auto">
        <div class="dice-panel">
          <div class="dice-icon">
            <img src="./assets/d20-icon.png" alt="1d20" class="dice" />
          </div>
          <label for="d20-input"><b>1d20</b></label>
          <input
            id="d20-input"
            name="d20-input"
            type="number"
            placeholder="0"
          />
        </div>
      </div>
      <div class="col col-1 col-mx-auto">
        <div class="dice-panel">
          <div class="dice-icon">
            <img src="./assets/d100-icon.png" alt="1d100" class="dice" />
          </div>
          <label for="d100-input"><b>1d100</b></label>
          <input
            id="d100-input"
            name="d100-input"
            type="number"
            placeholder="0"
          />
        </div>
      </div>
    </div>
    <div class="roll-container">
      <button class="btn btn-primary" v-on:click="onSubmitDicePool()">
        Roll dice!
      </button>
    </div>
    <div class="result-container">
      <p>{{ state.latestDiceResult }}</p>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, reactive } from "vue";
import { EnvironmentHelper } from "@/helpers/environment-helper";
import axios from "axios";

interface State {
  latestDiceResult: any;
}

export default defineComponent({
  name: "App",
  props: {},
  components: {},
  setup(props) {
    const state = reactive<State>({
      latestDiceResult: [],
    });

    const onSubmitDicePool = (): void => {
      axios
        .post(`${EnvironmentHelper.baseUrl}/calculate`, {
          "rolls": [
            {
              "sides": 4,
              "amount": 5
            },
            {
              "sides": 12,
              "amount": 2
            }
          ]
        })
        .then((result) => (state.latestDiceResult = result));
    };

    return {
      state,
      onSubmitDicePool
    };
  },
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
    display: flex;
    flex-direction: column;
    align-items: center;
    padding: 0.5rem;
    margin-bottom: 2rem;

    & > div.dice-icon {
      width: 3rem;
      height: 3rem;
      text-align: center;

      & > img.dice {
        width: 100%;
        height: 100%;
      }
    }

    & > input {
      width: 4rem;
    }
  }

  & > div.roll-container {
    display: flex;
    justify-content: center;
  }
}
</style>
