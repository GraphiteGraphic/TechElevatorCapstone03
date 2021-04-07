<template>
  <form v-on:submit.prevent="saveRecipe()">
    <div class="RecipeName">
      <label
        for="Recipe Name"
        v-show="!inputBool.name"
        @click="inputBool.name = true"
        >{{ recipe.recipeName }} 🖉</label
      >
      <input
        v-show="inputBool.name"
        @blur="inputBool.name = false"
        type="text"
        placeholder="Recipe Name"
        v-model="recipe.recipeName"
        name="Recipe Name"
        required="true"
      />
    </div>
    <div class="RecipeType">
      <label for="type" v-show="!inputBool.type" @click="inputBool.type = true"
        >{{ recipe.type }} 🖉</label
      >
      <select
        v-show="inputBool.type"
        v-model="recipe.type"
        @change="inputBool.type = false"
        @blur="inputBool.type = false"
        name="type"
        required="true"
      >
        <option>Main Dish</option>
        <option>Side Dish</option>
        <option>Beverage</option>
        <option>Dessert</option>
        <option>Appetizer</option>
      </select>
    </div>
    <div class="RecipeServings">
      <label for="numServings">Number of Servings: </label>
      <input
        type="number"
        placeholder="Number of Servings"
        name="numServings"
        v-model="recipe.servings"
        required="true"
        min="1"
      />
    </div>
    <div>
      <label for="ingredientList">Ingredients</label>
      <input list="ingredients" name="ingredientList" id="ingredientList">
      <datalist name="ingredients" id="ingredients">
        <option
          v-for="ingredient in allIngredients"
          :key="ingredient.ingredientId"
          :value="ingredient.ingredientName"
        />
      </datalist>
    </div>
    <div class="RecipeInstructions">
      <div>Instructions:</div>
      <div v-for="i of instructionSteps.length" :key="i">
        {{ i }}. {{ instructionSteps[i - 1] }}
        <span @click="editSetup(i - 1)">🖉</span>
        <span @click="deleteStep(i - 1)" v-show="inputBool.addStep">🗑</span>
      </div>
      <input
        type="text"
        name="instructions"
        placeholder="Recipe Instructions"
        v-model="recipe.instructions"
      />
      <button
        v-show="inputBool.addStep"
        type="button"
        v-on:click.prevent="addStep()"
      >
        Add Instruction Step
      </button>
      <button
        v-show="!inputBool.addStep"
        type="button"
        @click.prevent="editStep()"
      >
        Edit Instruction Step
      </button>
    </div>
    <div class="RecipeShare">
      <label for="shared">Set as Public</label>
      <input type="checkbox" v-model="recipe.isShared" name="shared" />
    </div>
    <button type="submit" class="submitBtn">Save Recipe</button>
  </form>
</template>

<script>
import service from "../services/AuthService.js";
export default {
  data() {
    return {
      inputBool: {
        name: false,
        type: false,
        instructions: false,
        addStep: true,
        stepNum: Number,
      },
      recipe: {
        recipeName: "Untitled",
        instructions: "",
        type: "Main Dish",
        userId: Number,
        isShared: false,
        servings: 1,
        //existingIngredients: [],
        //newIngredients: [],
      },
      ingredient: "",
      instructionSteps: [],
      ingredientList: [],
      allIngredients: [],
    };
  },
  methods: {
    saveRecipe() {
      //this parses numservings into a number
      this.recipe.numServings = parseInt(this.recipe.numServings);

      //this changes recipe type from string into its respective type #
      if (this.recipe.type === "Main Dish") {
        this.recipe.type = 1;
      } else if (this.recipe.type === "Side Dish") {
        this.recipe.type = 2;
      } else if (this.recipe.type === "Beverage") {
        this.recipe.type = 3;
      } else if (this.recipe.type === "Dessert") {
        this.recipe.type = 4;
      } else if (this.recipe.type === "Appetizer") {
        this.recipe.type = 5;
      }

      //this joins all instructions to have all the breaking charater (|||) in the database
      this.recipe.instructions = this.instructionSteps.join("|||");

      service
        .addRecipe(this.recipe)
        .then((response) => {
          if (response.status === 201) {
            this.$store.commit("ADD_RECIPE", response.data);
            this.$router.push({ name: "profile" });
          }
        })
        .catch((error) => {
          this.errorMsg =
            "was not reponse.status of 201. " + error.response.statusText;
          alert("Error");
        });
    },
    addStep() {
      this.instructionSteps.push(this.recipe.instructions);
      this.recipe.instructions = "";
    },
    editSetup(index) {
      this.recipe.instructions = this.instructionSteps[index];
      this.inputBool.addStep = false;
      this.inputBool.stepNum = index;
    },
    editStep() {
      this.instructionSteps[this.inputBool.stepNum] = this.recipe.instructions;
      this.inputBool.addStep = true;
      this.recipe.instructions = "";
    },
    deleteStep(index){
      this.instructionSteps.splice(index, 1);
    },
    addIngredient() {
      this.ingredientList.push(this.ingredient);
      this.ingredient = "";
    },
    /*addIngredient(ingredientId, IngredientName) {
        //make object that holds both ingredients and push into it
      this.recipe.ingredients.push(ingredient);
    },*/
  },
  created() {
    service.getAllIngredients().then( (resp) => {
      this.allIngredients = resp.data;
    })
  }
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