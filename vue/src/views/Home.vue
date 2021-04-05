<template>
  <div class="home">
    <h1>Home</h1>
    <router-link
      :to="{ name: 'recipe', params: { id: recipe.id } }"
      v-for="recipe in recipeList"
      :key="recipe.id"
      :name= recipe.name
      :instructions = recipe.instructions
      >{{ recipe.name }}</router-link
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