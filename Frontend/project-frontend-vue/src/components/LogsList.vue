<template>
    <div>
      <h1>Logs</h1>
      <form @submit.prevent="fetchLogs">
        <div>
          <label for="startDate">Start Date:</label>
          <input
            type="date"
            id="startDate"
            v-model="startDate"
          />
        </div>
        <div>
          <label for="endDate">End Date:</label>
          <input
            type="date"
            id="endDate"
            v-model="endDate"
          />
        </div>
        <div>
          <button type="submit">Fetch Logs</button>
        </div>
      </form>
      <div v-if="logs.length > 0">
        <h2>Log Entries:</h2>
        <ul>
          <li
            class="item"
            v-for="log in logs"
            :key="log.id"
          >
            <p><span class="title">Username:</span> {{ log.username }}</p>
            <p><span class="title">Details:</span> {{ log.details }}</p>
            <p><span class="title">Date of Log:</span> {{ log.dateoflog }}</p>
            <p><span class="title">IP:</span> {{ log.ip }}</p>
            <!-- <p>Duration: {{ log.duration }}</p> -->
          </li>
        </ul>
      </div>
      <div v-else-if="!errorMessage">No logs available.</div>
      <div v-if="errorMessage">{{ errorMessage }}</div>
    </div>
  </template>
  
  <script setup>
  import { ref, inject } from "vue";
  import axios from "axios";
  
  const startDate = ref("");
  const endDate = ref("");
  const errorMessage = ref("");
  const logs = ref([]);
  
  const globalState = inject("globalState");
  
  async function fetchLogs() {
    if (!globalState.username) {
      errorMessage.value = "You need to sign in first.";
      return;
    }
  
    const apiUrl = "https://localhost:7026/api/Requests/Logs";
    const formattedStartDate = startDate.value ? new Date(startDate.value).toISOString() : "";
    const formattedEndDate = endDate.value ? new Date(endDate.value).toISOString() : "";
  
    const requestData = {
      logInfo: {
        userName: globalState.username,
        ip: globalState.ip
      },
      startTime: formattedStartDate,
      endTime: formattedEndDate,
    };
  
    try {
      const response = await axios.post(apiUrl, requestData);
      logs.value = response.data;
    } catch (error) {
      console.error(error);
    }
  }
  </script>
  
  <style scoped>
   .item{
    margin-bottom: 0.5rem;
    padding: 5px;
    border: 1px solid #c1c1c1;
    border-radius: 5px;
   }
   .title{
    color: yellow;
    font-weight: 600;
   }
  </style>
  