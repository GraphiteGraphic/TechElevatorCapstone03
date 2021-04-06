<template>
  <div class="home">
    <h1>Home</h1>
    <router-link
      :to="{ name: 'recipe', params: { id: recipe.recipeId } }"
      v-for="recipe in recipeList"
      :key="recipe.recipeId"
      :name= recipe.recipeName
      :instructions = recipe.instructions
      >{{ recipe.recipeName }}</router-link
    >
  </div>
</template>

<script>
export default {
  name: "home",
  computed: {
    currentUser() {
      return this.$store.state.user;
    },
    recipeList() {
      return this.$store.state.recipes;
    },
  },
};
</script>

<style scoped>
a {
  display: block;
}
</style>