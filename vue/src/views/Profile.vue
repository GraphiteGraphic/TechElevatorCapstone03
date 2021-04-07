<template>
  <div>
    <h2>My Recipes</h2>
    <recipe-list :recipeList="myRecipes"/>
    <router-link v-bind:to="{ name: 'addRecipe' }">+ Add Recipe</router-link>
  </div>
</template>

<script>
import recipeList from '../components/recipeList.vue';
import authServices from "../services/AuthService";

export default {
  components: { recipeList },
  data() {
    return {
    };
  },
  computed: {
    currentUser() {
      return this.$store.state.user;
    },
    myRecipes() {
      return this.$store.state.myRecipes;
    },
  },
  created() {
    authServices.getMyRecipes(this.currentUser).then((resp) => {
      this.$store.state.myRecipes = resp.data;
    });
  },
};
</script>

<style scoped>

</style>