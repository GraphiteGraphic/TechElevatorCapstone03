<template>
  <div>
    <h2>{{ recipe.name }}</h2>
    <p>{{ ingredients }}</p>
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
        return this.$route.params.id == recipe.id;
      })
    }
  },
  created() {
    //TODO: Replace following code with api call to get ingredients
    this.ingredients = this.$store.state.recipeIngredients;
  },
};
</script>

<style>
</style>