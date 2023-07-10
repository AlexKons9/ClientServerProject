import './assets/main.css'

import { createApp, provide } from 'vue'
import App from './App.vue'
import router from './router'


// Define your global state object
const globalState = {
    // Define your initial state here
    username: null,
  };


const app = createApp(App)
app.provide('globalState', globalState);
app.use(router)

app.mount('#app')
