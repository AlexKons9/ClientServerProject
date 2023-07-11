<template>
  <div>
    <h1>Math Calculation</h1>
    <form @submit.prevent="performCalculation">
      <div>
        <label for="calculationType">Calculation Type:</label>
        <input
          type="text"
          id="calculationType"
          v-model="calculationType"
        />
      </div>
      <div>
        <label for="numbers">Numbers (comma-separated):</label>
        <input
          type="text"
          id="numbers"
          v-model="numbers"
        />
      </div>
      <div>
        <button type="submit">Calculate</button>
      </div>
    </form>
    <div v-if="errorMessage">{{ errorMessage }}</div>
    <div v-if="result">The result is: {{ result }}</div>
  </div>
</template>

<script setup>
import { ref, inject } from "vue";
import axios from "axios";

const calculationType = ref("");
const numbers = ref("");
const errorMessage = ref("");
const result = ref("");

const globalState = inject('globalState');

async function performCalculation() {
  if (!globalState.username) {
    errorMessage.value = "You need to sign in first.";
    return;
  }

  const apiUrl = "https://localhost:7026/api/Requests/Math";

  try {
    const response = await axios.post(apiUrl, {
      mathRequest: {
        typeOfRequest: calculationType.value,
        numbers: numbers.value.split(",").map((num) => +num.trim()),
      },
      logInfo: {
        userName: globalState.username,
      },
    });

    

    const data = response.data;
    console.log(data.mathRequest.result);
    result.value = data.mathRequest.result;
    errorMessage.value = '';
  } catch (error) {
    errorMessage.value = "An error occurred while performing the calculation.";
    result.value = '';
    console.error(error);
  }

  return {
    calculationType,
    numbers,
    errorMessage,
    result
  };
}
</script>

<style scoped></style>
