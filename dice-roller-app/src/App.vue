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
          <input id="d4-input" name="d4-input" type="number" v-model="state.d4Amount" min="0" />
        </div>
      </div>
      <div class="col col-1 col-mx-auto">
        <div class="dice-panel">
          <div class="dice-icon">
            <img src="./assets/d6-icon.png" alt="1d6" class="dice" />
          </div>
          <label for="d6-input"><b>1d6</b></label>
          <input id="d6-input" name="d6-input" type="number" v-model="state.d6Amount" min="0" />
        </div>
      </div>
      <div class="col col-1 col-mx-auto">
        <div class="dice-panel">
          <div class="dice-icon">
            <img src="./assets/d8-icon.png" alt="1d8" class="dice" />
          </div>
          <label for="d8-input"><b>1d8</b></label>
          <input id="d8-input" name="d8-input" type="number" v-model="state.d8Amount" min="0" />
        </div>
      </div>
      <div class="col col-1 col-mx-auto">
        <div class="dice-panel">
          <div class="dice-icon">
            <img src="./assets/d10-icon.png" alt="1d10" class="dice" />
          </div>
          <label for="d10-input"><b>1d10</b></label>
          <input id="d10-input" name="d10-input" type="number" v-model="state.d10Amount" min="0" />
        </div>
      </div>
      <div class="col col-1 col-mx-auto">
        <div class="dice-panel">
          <div class="dice-icon">
            <img src="./assets/d12-icon.png" alt="1d12" class="dice" />
          </div>
          <label for="d12-input"><b>1d12</b></label>
          <input id="d12-input" name="d12-input" type="number" v-model="state.d12Amount" min="0" />
        </div>
      </div>
      <div class="col col-1 col-mx-auto">
        <div class="dice-panel">
          <div class="dice-icon">
            <img src="./assets/d20-icon.png" alt="1d20" class="dice" />
          </div>
          <label for="d20-input"><b>1d20</b></label>
          <input id="d20-input" name="d20-input" type="number" v-model="state.d20Amount" min="0" />
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
            v-model="state.d100Amount"
            min="0"
          />
        </div>
      </div>
    </div>
    <div class="columns" v-if="Object.keys(state.latestDiceResult).length">
      <div class="col col-10 col-mx-auto">
        <table class="table table-striped">
          <thead>
            <th>Sides</th>
            <th>Results</th>
            <th>Total</th>
          </thead>
          <tbody>
            <tr v-for="(diceGroup, index) in state.latestDiceResult" :key="index">
              <td>{{ index }}</td>
              <td>{{ diceGroup.join(', ') }}</td>
              <td>{{ calculateSum(diceGroup) }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <div class="columns">
      <div class="col col-10 col-mx-auto">
        <button class="btn btn-primary" v-on:click="onSubmitDicePool()">
          Roll dice!
        </button>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, reactive } from 'vue';
import { EnvironmentHelper } from '@/helpers/environment-helper';
import { RollResult } from '@/models/rollResult';
import { DicePool } from '@/models/dicePool';
import { DiceRoll } from '@/models/diceRoll';
import { GroupedResult } from '@/models/groupedResult';
import axios, { AxiosResponse } from 'axios';

interface State {
  d4Amount: number;
  d6Amount: number;
  d8Amount: number;
  d10Amount: number;
  d12Amount: number;
  d20Amount: number;
  d100Amount: number;
  latestDiceResult: GroupedResult;
}

export default defineComponent({
  name: 'App',
  props: {},
  components: {},
  setup() {
    const state = reactive<State>({
      d4Amount: 0,
      d6Amount: 0,
      d8Amount: 0,
      d10Amount: 0,
      d12Amount: 0,
      d20Amount: 0,
      d100Amount: 0,
      latestDiceResult: new GroupedResult()
    });

    const collectDicePool = (): DicePool =>
      new DicePool({
        rolls: [
          new DiceRoll({
            sides: 4,
            amount: state.d4Amount
          }),
          new DiceRoll({
            sides: 6,
            amount: state.d6Amount
          }),
          new DiceRoll({
            sides: 8,
            amount: state.d8Amount
          }),
          new DiceRoll({
            sides: 10,
            amount: state.d10Amount
          }),
          new DiceRoll({
            sides: 12,
            amount: state.d12Amount
          }),
          new DiceRoll({
            sides: 20,
            amount: state.d20Amount
          }),
          new DiceRoll({
            sides: 100,
            amount: state.d100Amount
          })
        ]
      });

    const calculateSum = (numbers: number[]) =>
      numbers.reduce((acc: number, current: number) => acc + current, 0);

    const groupDiceResults = (result: RollResult[]): GroupedResult => {
      return result.reduce((acc: GroupedResult, current: any) => {
        if (!acc[current.sides]) {
          acc[current.sides] = [];
        }

        acc[current.sides] = [...acc[current.sides], current.result];
        return acc;
      }, new GroupedResult());
    };

    const onSubmitDicePool = (): void => {
      axios
        .post(`${EnvironmentHelper.baseUrl}/api/calculate`, collectDicePool())
        .then(
          (result: AxiosResponse<RollResult[]>) =>
            (state.latestDiceResult = groupDiceResults(result.data))
        );
    };

    return {
      state,
      calculateSum,
      onSubmitDicePool
    };
  }
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
