<template>
  <div>
    <section class="user-recipes">
      <h2 @click.prevent="collapse = !collapse">
        <span v-show="!collapse">⮞</span><span v-show="collapse">⮟</span> My
        Recipes
      </h2>
      <router-link v-show="collapse" v-bind:to="{ name: 'addRecipe' }"
        >+ Add Recipe</router-link
      >
      <recipe-list :recipeList="myRecipes" v-show="collapse" />
    </section>
    <meal-plan />
  </div>
</template>

<script>
import recipeList from "../components/recipeList.vue";
import mealPlan from "../components/MealPlan.vue";
import authServices from "../services/AuthService";

export default {
  data() {
    return {
      collapse: false,
    };
  },
  components: { recipeList, mealPlan },
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
      this.$store.commit("SET_MY_RECIPES", resp.data);
    });
  },
};
</script>

<style scoped>
.user-recipes {
  background-color: rgba(255, 255, 255, 0.7);
  -webkit-backdrop-filter: blur(10px);
  backdrop-filter: blur(10px);
  border-radius: 25px;
  margin-bottom: 1vh;
}
</style>