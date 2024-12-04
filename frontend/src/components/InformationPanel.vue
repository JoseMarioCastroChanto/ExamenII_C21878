<template>
    <div class="info-panel">
      <ul v-if="order.length > 0" class="list-group">
        <li v-for="(coffee, index) in order" :key="index" class="list-group-item">
          {{ coffee.name }} x {{ coffee.quantity }} - ₡{{ coffee.quantity * coffee.price }}
        </li>
      </ul>
      <div v-if="order.length > 0">
        <h4>Total: ₡{{ totalCost }}</h4>
      </div>
      <div>
        <h4>Fondos: ₡{{ totalPaid }}</h4>
      </div>
      <div v-if="outOfService" class="alert alert-danger mt-3">
        La máquina está fuera de servicio.
      </div>
      <div v-if="status != ''" class="alert-danger">
        <strong>{{ status }}</strong>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    props: ["totalCost","order", "status", "outOfService", "totalPaid"],
  };
  </script>
  
  <style scoped>
  .info-panel {
    padding: 20px;
    background-color: #f8f9fa;
    width: 580px; 
    height: 200px; 
    overflow-y: auto; 
    border: 1px solid #ddd;
  }
  </style>