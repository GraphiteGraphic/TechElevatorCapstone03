<template>
  <div>
    <h2>{{ recipe.recipeName }}</h2>
    <p v-for="ingredient of ingredients" :key="ingredient.ingredientId">{{ingredient.quantity}} {{ingredient.unit}} {{ ingredient.ingredientName }}</p>
    <p v-for="num of instructions.length" :key="num"><span>{{num}}.</span> {{ instructions[num - 1] }}</p>
  </div>
</template>

<script>
import authServices from '../services/AuthService.js'

export default {
  data() {
    return {
      recipe: {},
      // contains name, quantity, and unit of each ingredient
      ingredients: [{}],
    };
  },
  computed: {
    instructions() {
      return this.recipe.instructions.split('|||');
    }
  },
  created() {
    this.recipe = this.$store.state.recipes.find( (recipe) => {
        return this.$route.params.id == recipe.recipeId;});
    if (!this.recipe){
      this.recipe = this.$store.state.myRecipes.find((recipe)=>{
        return this.$route.params.id == recipe.recipeId;
      });
    }

    //TODO: Replace following code with api call to get ingredients
    authServices.getIngredients(this.recipe).then( (resp) => {
      this.ingredients = resp.data;
    });
  },
};
</script>

<style scoped>
  span {
    font-size: 500%;
  }
</style>