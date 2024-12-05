<template>
    <div class="coffee-list">
      <div v-for="coffee in coffees" :key="coffee.id" class="coffee-card position-relative mb-4">
        <div class="card-body p-3">
          <div class="d-flex align-items-center">
            <img :src="coffee.image" alt="Imagen del café" class="coffee-image me-3" />
            <div class="flex-grow-1">
              <h5 class="card-title">{{ coffee.name }}</h5>
              <p class="card-text">₡{{ coffee.price }}</p>
            </div>
            <div class="quantity-container ms-3">
              <label for="quantity" class="form-label">Cantidad</label>
              <input
                type="number"
                id="quantity"
                class="form-control"
                v-model.number="coffee.quantity"
                min="0"
                :max="coffee.available"
                @change="updateOrder()"
              />
            </div>
            <div class="availability-info ms-3">
              <div class="availability-text">Disponible</div>
              <div class="availability-circle">
                <span>{{ coffee.available }}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    props: ["coffees", "updateOrder"],
  };
  </script>
  
  <style scoped>
  .coffee-list {
    background-color: #433123;
    padding: 15px;
    border-radius: 8px;
  }
  
  .coffee-card {
    position: relative;
    background-color: #a6744c;
    border-radius: 10px 10px 0 0;
    overflow: hidden;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  }
  
  .card-body {
    padding: 12px;
    color: white;
  }
  
  .card-title {
    font-size: 1rem;
    font-weight: bold;
    margin-bottom: 5px;
  }
  
  .card-text {
    font-size: 0.9rem;
    margin-bottom: 10px;
  }
  
  .coffee-image {
    width: 50px;
    height: 50px;
    object-fit: cover;
    border-radius: 50%;
  }
  
  .quantity-container {
    width: 120px;
  }
  
  .availability-info {
    text-align: center;
  }
  
  .availability-text {
    font-size: 0.8rem;
    color: white;
    margin-bottom: 5px;
  }
  
  .availability-circle {
    width: 30px;
    height: 30px;
    margin-left: 15px;
    background-color: #e4d9a7;
    border-radius: 50%;
    display: flex;
    justify-content: center;
    align-items: center;
    color: black;
    font-size: 0.9rem;
    font-weight: bold;
  }
  
  .form-label {
    font-size: 0.8rem;
    margin-bottom: 5px;
  }
  
  .form-control {
    font-size: 0.9rem;
    padding: 6px;
    background-color: #f8f9fa;
    border: 1px solid #ced4da;
  }
  
  .text-muted {
    font-size: 0.7rem;
    margin-top: 5px;
  }
  
  .d-flex {
    justify-content: space-between;
    align-items: center;
  }
  </style>