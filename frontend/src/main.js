// src/main.js
import { createApp } from 'vue';
import App from './App.vue';
import router from './router';
import { createPinia } from 'pinia';

import 'vue3-toastify/dist/index.css';
import Vue3Toastify from 'vue3-toastify';

const app = createApp(App);

app.use(createPinia());
app.use(router);
app.use(Vue3Toastify, {
  autoClose: 3000,
  position: 'top-right',
});

app.mount('#app');
