<template>
      <div class="row">
        <div class="col-md-6">
          <CoffeeList :coffees="coffees" :updateOrder="updateOrder" />
        </div>
        
      </div>

  </template>
  
  <script>

  import CoffeeList from "@/components/CoffeeList.vue";
  
  export default {
    components: {
      CoffeeList,
    },
    data() {
      return {
        coffees: [],
      };
    },
    methods: {
      fetchCoffee() {
        const simulatedData = [
          { id: 1, image:'https://images.vexels.com/content/264248/preview/cappuccino-illustration-0442c5.png', name: "Americano", price: 950, available: 10, quantity: 0 },
          { id: 2, image:'https://images.vexels.com/content/264248/preview/cappuccino-illustration-0442c5.png',name: "Capuchino", price: 1200, available: 8, quantity: 0 },
          { id: 3, image:'https://images.vexels.com/content/264248/preview/cappuccino-illustration-0442c5.png',name: "Late", price: 1350, available: 10, quantity: 0 },
          { id: 4, image:'https://images.vexels.com/content/264248/preview/cappuccino-illustration-0442c5.png',name: "Mocachino", price: 1500, available: 15, quantity: 0 },
        ];
        this.coffees = simulatedData;
      },
    },
    mounted() {
      this.fetchCoffee();
    },
  };
  </script>
  
  <style scoped>
  .row {
    display: flex;
    justify-content: space-between;
  }
  </style>