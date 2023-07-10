<template>
  <div>
    <h1>Login</h1>
    <form @submit.prevent="login">
      <div>
        <label for="username">Username:</label>
        <input type="text" id="username" v-model="username" />
      </div>
      <div>
        <label for="password">Password:</label>
        <input type="password" id="password" v-model="password" />
      </div>
      <div>
        <button class="button-3" type="submit">Login</button>
      </div>
    </form>
    <div v-if="errorMessage">{{ errorMessage }}</div>
    <div v-if="globalState.username">
      <button @click="logout">Sign out</button>
    </div>
  </div>
</template>

<script setup>
import { ref, inject } from 'vue';
import { useRouter } from 'vue-router';
import axios from 'axios';

const username = ref('');
const password = ref('');
const errorMessage = ref('');
const globalState = inject('globalState');

const router = useRouter();

async function login() {
  const apiUrl = 'https://localhost:7026/api/Requests/Login';

  try {
    const response = await axios.post(apiUrl, {
      id: '',
      userName: username.value,
      password: password.value,
    });

    const data = response.data;

    if (
      response.status === 200 &&
      data.username === username.value &&
      data.password === password.value
    ) {
      // Store the user information in global state
      globalState.username = data.username;

      router.push('/math');
    } else {
      errorMessage.value = 'Invalid username or password.';
    }
  } catch (error) {
    errorMessage.value = 'An error occurred while logging in.';
    console.error(error);
  }
}

async function logout() {
  globalState.username = "";
  location.reload();
}
</script>

<style scoped>
/* CSS styles */
</style>


<style scoped>

/* CSS */
.button-3 {
  appearance: none;
  background-color: #2ea44f;
  border: 1px solid rgba(27, 31, 35, .15);
  border-radius: 6px;
  box-shadow: rgba(27, 31, 35, .1) 0 1px 0;
  box-sizing: border-box;
  color: #fff;
  cursor: pointer;
  display: inline-block;
  font-family: -apple-system,system-ui,"Segoe UI",Helvetica,Arial,sans-serif,"Apple Color Emoji","Segoe UI Emoji";
  font-size: 14px;
  font-weight: 600;
  line-height: 20px;
  margin-top: 1rem;
  padding: 6px 16px;
  position: relative;
  text-align: center;
  text-decoration: none;
  user-select: none;
  -webkit-user-select: none;
  touch-action: manipulation;
  vertical-align: middle;
  white-space: nowrap;
}

.button-3:focus:not(:focus-visible):not(.focus-visible) {
  box-shadow: none;
  outline: none;
}

.button-3:hover {
  background-color: #2c974b;
}

.button-3:focus {
  box-shadow: rgba(46, 164, 79, .4) 0 0 0 3px;
  outline: none;
}

.button-3:disabled {
  background-color: #94d3a2;
  border-color: rgba(27, 31, 35, .1);
  color: rgba(255, 255, 255, .8);
  cursor: default;
}

.button-3:active {
  background-color: #298e46;
  box-shadow: rgba(20, 70, 32, .2) 0 1px 0 inset;
}
</style>
