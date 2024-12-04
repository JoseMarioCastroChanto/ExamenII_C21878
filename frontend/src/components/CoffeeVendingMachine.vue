<template>
    <div class="row">
      <div class="col-md-6">
        <CoffeeList :coffees="coffees" :updateOrder="updateOrder" />
      </div>
      
      <div class="col-md-6">
        <InformationPanel 
          :totalCost="totalCost" 
          :order="order" 
          :status="status" 
          :outOfService="outOfService"
        />
        <div class="btn-group d-flex flex-wrap">
          <button 
            v-for="(value, index) in [500, 100, 50, 25]" 
            :key="index" 
            class="btn btn-custom-size" 
            @click="addPayment(value)"
          >
            ₡{{ value }}
          </button>
        </div>
        <div class="d-flex flex-column">
          <button 
            class="btn btn-custom-size btn-mil" 
            @click="addPayment(1000)"
          >
            ₡1000
          </button>
          <button 
            class="btn btn-buy" 
            @click="makePurchase" 
            :disabled="outOfService || totalCost === 0"
          >
            Comprar
          </button>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  import axios from "axios";
  import CoffeeList from "@/components/CoffeeList.vue";
  import InformationPanel from "@/components/InformationPanel.vue";
  const defaultValue = 0;
  const cofeeFetchErrorMessage = "Error al obtener datos de café";
  const outOfStockMessage = "Error: no hay suficientes existencias para ";
  
  export default {
    components: {
      CoffeeList,
      InformationPanel,
    },
    data() {
      return {
        coffees: [],
        order: [],
        totalPaid: 0,
        outOfService: false,
        status:"" 
      };
    },
    methods: {
        fetchCoffees() {
            axios
                .get(this.$backendAddress + "api/Coffee")
                .then((response) => {
                    this.coffees = response.data;
                })
                .catch(() => {
                    this.status=cofeeFetchErrorMessage;
                });    
        },

        updateOrder() {
            this.validateOrder();
            this.order = this.coffees.filter(coffee => coffee.quantity > 0);
            this.totalCost = this.order.reduce(
                (total, coffee) => total + coffee.quantity * coffee.price,
                0
            );
        },
        validateOrder(){
            this.status = "";
            this.coffees.forEach(coffee => {
                if (coffee.quantity > coffee.available) {
                    coffee.quantity = defaultValue; 
                    this.status = outOfStockMessage + `${coffee.name}`;
                }
            });
        }
    },
    mounted() {
      this.fetchCoffees();
    },
  };
  </script>
  
  <style scoped>
  .row {
    display: flex;
    justify-content: space-between;
  }
  
  div {
    background-color: #433123; 
    padding: 20px;
  }
  
  .btn-custom-size {
    font-size: 1.2rem; 
    padding: 10px 20px;
    background-color: #a6744c; 
    color: white; 
    border: none; 
    margin-right: 10px; 
  }
  
  .btn-custom-size:hover {
    background-color: #8c5a3f; 
  }
  
  .btn-mil {
    margin-bottom: 10px;
  }
  
  .btn-buy {
    font-size: 1.5rem; 
    padding: 15px 30px;
    background-color: #e4d9a7; 
    color: black; 
    border: none; 
    margin-top: 20px;
  }
  
  .btn-buy:hover {
    background-color: #d1b78a; 
  }
  </style>