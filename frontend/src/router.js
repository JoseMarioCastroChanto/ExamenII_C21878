import { createRouter, createWebHistory } from 'vue-router';
import CoffeeVendingMachine from './components/CoffeeVendingMachine.vue';  

const routes = [
  {
    path: '/',
    name: 'CoffeeVendingMachine',
    component: CoffeeVendingMachine,  
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;