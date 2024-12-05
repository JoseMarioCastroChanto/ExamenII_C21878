<template>
    <div class="info-panel">
    <div v-if="!cashChangeView">    
        <ul v-if="order.length > 0" class="list-group">
            <li v-for="(coffee, index) in order" :key="index" class="list-group-item">
            {{ coffee.name }} x {{ coffee.quantity }} - ₡{{ coffee.quantity * coffee.price }}
            </li>
        </ul>
        <div v-if="order.length > 0" class="list-group-item">
            <h4>Total: ₡{{ totalCost }}</h4>
        </div>
        <div class="list-group-item">
            <h4>Fondos: ₡{{ totalPaid }}</h4>
        </div>
      </div>
      <div v-if="cashChangeView"> 
        <h4>Su vuelto es de {{ CalculateCashChange }} colones </h4>
        <h4>Desglose: </h4>
        <ul v-if="filteredCashChange.length > 0" class="list-group">
        <li v-for="(item, index) in filteredCashChange" :key="index" class="list-group-item">
            {{ item.quantity }} 
            <span v-if="item.moneyValue === 1000">billete</span>
            <span v-else>moneda</span> 
            de ₡{{ item.moneyValue }}
        </li>
        </ul>
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
    props: ["totalCost","order", "status", "outOfService", "totalPaid","cashChange","cashChangeView"],
    computed: {
    filteredCashChange() {
        return this.cashChange.moneyValue
            .map((money, index) => ({
            moneyValue: money,
            quantity: this.cashChange.quantity[index]
            }))
            .filter(item => item.moneyValue > 0);
        },
        CalculateCashChange() {
            let totalCashChange = 0;
            for (let i = 0; i < this.cashChange.moneyValue.length; i++) {
                totalCashChange += this.cashChange.quantity[i] * this.cashChange.moneyValue[i];
            }
            return totalCashChange;
            }
    } 
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