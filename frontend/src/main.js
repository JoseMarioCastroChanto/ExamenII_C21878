import { createApp } from 'vue'
import App from './App.vue'
import router from './router' 

const app = createApp(App);
app.use(router)
app.config.globalProperties.$backendAddress = 'https://localhost:7263/';


app.mount('#app'); 