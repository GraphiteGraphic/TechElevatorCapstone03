<template>
  <form v-on:submit.prevent="saveRecipe()">
    <div class = "RecipeName">
      <label for="Recipe Name">Recipe Name: </label>
      <input
        type="text"
        placeholder="Recipe Name"
        v-model="recipe.recipeName"
        name="Recipe Name"
        required="true"
      />
    </div>
    <div class = "RecipeType">
      <label for="type">Recipe Type: </label>
      <select v-model="recipe.type" name="type" required="true">
        <option>Main Dish</option>
        <option>Side Dish</option>
        <option>Beverage</option>
        <option>Dessert</option>
        <option>Appetizer</option>
      </select>
    </div>
    <div class = "RecipeShare">
      <label for="shared">Do you want to share this?</label>
      <input type="checkbox" v-model="recipe.isShared" name="shared" />
    </div>
    <div class = "RecipeServings">
      <label for="numServings">Number of Servings: </label>
      <input
        type="number"
        placeholder="Number of Servings"
        name="numServings"
        v-model="recipe.servings"
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
    <div class = "RecipeInstructions">
      <label for="instructions">Recipe Instructions: </label>
      <input
        type="text"
        name="instructions"
        placeholder="Recipe Instructions"
        v-model="recipe.instructions"
        required="true"
        min="1"
      />
      <button type="button" v-on:click.prevent="addStep()">Add Instruction Step</button>
    </div>
        <div class = "IngredientList">
      <label for="ingredients">Recipe Ingredients: </label>
      <input
        type="text"
        name="ingredients"
        placeholder="Recipe Ingredients"
        v-model="ingredient"
        required="true"
        min="1"
      />
      <button type="button" v-on:click.prevent="addIngredient()">Add Ingredient</button>
    </div>
    <button type="submit" class = "submitBtn">Save Recipe</button>
  </form>
</template>

<script>
import service from "../services/AuthService.js"
export default {
  data() {
    return {
      recipe: {
        recipeName: "",
        instructions: "",
        type: "",
        userId: Number,
        isShared: false,
        servings: 1,
        //existingIngredients: [],
        //newIngredients: [],
      },
      ingredient: '',
      instructionSteps: [],
      ingredientList: [],
    };
  },
  methods: {
    saveRecipe() {
      //this parses numservings into a number
      this.recipe.numServings=parseInt(this.recipe.numServings);

      //this changes recipe type from string into its respective type #
      if (this.recipe.type==='Main Dish'){
        this.recipe.type=1;
      }
      else if (this.recipe.type==='Side Dish'){
        this.recipe.type=2;
      }
      else if (this.recipe.type==='Beverage'){
        this.recipe.type=3;
      }
      else if (this.recipe.type==='Dessert'){
        this.recipe.type=4;
      }
      else if (this.recipe.type==='Appetizer'){
        this.recipe.type=5;
      }

      //this joins all instructions to have all the breaking charater (|||) in the database
      this.recipe.instructions = this.instructionSteps.join('|||')+'|||'+ this.recipe.instructions;

      service.addRecipe(this.recipe).then((response)=>{
        if(response.status===201){
          this.$store.commit("ADD_RECIPE",response.data);
          this.$router.push({name: 'profile'});
        }
      }).catch((error)=>{
        this.errorMsg = 'was not reponse.status of 201. ' + error.response.statusText;
        alert('Error');
      });
      
    },
    addStep(){
      this.instructionSteps.push(this.recipe.instructions);
      this.recipe.instructions="";
    },
    addIngredient(){
      this.ingredientList.push(this.ingredient);
      this.ingredient="";
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
  display: flex;
  flex-direction: column;
  align-content: left;
  padding: 200px;
  text-align: left;
}
.RecipeName {
  font-size: 36px;
  font-weight: bold;
  text-align: center;
}
</style>