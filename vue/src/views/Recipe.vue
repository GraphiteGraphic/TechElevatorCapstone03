<template>
  <div>
    <h2>{{ recipe.recipeName }}</h2>
    <p>{{ ingredients }}</p>
    <p v-for="num of instructions.length" :key="num"><span>{{num}}.</span> {{ instructions[num - 1] }}</p>
  </div>
</template>

<script>
export default {
  data() {
    return {
      // contains name, quantity, and unit of each ingredient
      ingredients: [{}],
    };
  },
  computed: {
    recipe() {
      return this.$store.state.recipes.find( (recipe) => {
        return this.$route.params.id == recipe.recipeId;
      })
    },
    instructions() {
      return this.recipe.instructions.split('|||');
    }
  },
  created() {
    //TODO: Replace following code with api call to get ingredients
    this.ingredients = this.$store.state.recipeIngredients;
  },
};
</script>

<style scoped>
  span {
    font-size: 500%;
  }
</style>