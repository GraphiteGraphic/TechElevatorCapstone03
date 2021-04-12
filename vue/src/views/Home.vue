<template>
  <div class="home">
    <h1>Munch Munch, Yum!</h1>
    <h2>(^-^)</h2>
    <recipe-list :recipeList="recipeList" />
  </div>
</template>

<script>
import RecipeList from "../components/recipeList.vue";
export default {
  name: "home",
  components: { RecipeList },
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