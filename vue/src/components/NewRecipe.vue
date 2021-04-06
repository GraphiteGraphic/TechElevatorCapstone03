<template>
  <form v-on:submit.prevent="saveRecipe">
    <div>
      <label for="Recipe Name">Recipe Name: </label>
      <input
        type="text"
        placeholder="Recipe Name"
        v-model="recipe.recipeName"
        name="Recipe Name"
        required="true"
      />
    </div>
    <div>
      <label for="type">Recipe Type: </label>
      <select v-model="recipe.type" name="type" required="true">
        <option>Main Dish</option>
        <option>Side Dish</option>
        <option>Beverage</option>
        <option>Dessert</option>
        <option>Appetizer</option>
      </select>
    </div>
    <div>
      <label for="shared">Do you want to share this?</label>
      <input type="checkbox" v-model="recipe.isShared" name="shared" />
    </div>
    <div>
      <label for="numServings">Number of Servings: </label>
      <input
        type="number"
        placeholder="Number of Servings"
        name="numServings"
        v-model="recipe.numServings"
        required="true"
      />
    </div>
    <!--
    <div>
      <datalist id="ingredients">
        <option
          v-for="ingredient in recipe.ingredients"
          :key="ingredient.ingredientId"
          value="{{ingredient.ingredientName}}"
          @change="addIngredient(ingredient.ingredientId, ingredient.ingredientName)"
        />
      </datalist>
    </div>
    -->
    <div>
      <label for="instructions">Recipe Instructions: </label>
      <input
        type="text"
        name="instructions"
        placeholder="Recipe Instructions"
        v-model="recipe.instructions"
        required="true"
        min="1"
      />
    </div>
    <button type="submit">Save Recipe</button>
  </form>
</template>

<script>
//import service from "../services/AuthService.js"
export default {
  data() {
    return {
      recipe: {
        recipeName: "",
        instructions: "",
        type: "",
        userId: Number,
        isShared: false,
        numServings: 1,
        existingIngredients: [],
        newIngredients: [],
      },
    };
  },
  methods: {
    saveRecipe() {
      //service.addRecipe(this.recipe);
    },
    /*addIngredient(ingredientId, IngredientName) {
        //make object that holds both ingredients and push into it
      this.recipe.ingredients.push(ingredient);
    },*/
  },
};
</script>

<style scoped>
form {
  display: block;
}
</style>