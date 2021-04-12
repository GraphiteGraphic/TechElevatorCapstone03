<template>
  <form v-on:submit.prevent="saveRecipe()">
    <div class="RecipeName">
      <label
        for="Recipe Name"
        v-show="!visualParam.name"
        @click="visualParam.name = true"
        >{{ recipe.recipeName }} <span>🖉</span></label
      >
      <input
        v-show="visualParam.name"
        @blur="visualParam.name = false"
        type="text"
        placeholder="Recipe Name"
        v-model="recipe.recipeName"
        name="Recipe Name"
        required="true"
      />
    </div>
    <div class="RecipeType">
      <label
        for="type"
        v-show="!visualParam.type"
        @click="visualParam.type = true"
        >{{ recipe.type }} <span>🖉</span></label
      >
      <select
        v-show="visualParam.type"
        v-model="recipe.type"
        @change="visualParam.type = false"
        @blur="visualParam.type = false"
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
      <div>Ingredients:</div>
      <div
        v-for="ingredient of recipe.ingredientList"
        v-bind:key="ingredient.ingredientName"
      >
        {{ ingredient.quantity }}
        {{ ingredient.unit }}
        {{ ingredient.ingredientName }}.
        <span
          @click="setupIngredient(recipe.ingredientList.indexOf(ingredient))"
          >🖉</span
        >
        <span
          v-show="visualParam.ingredient"
          @click="deleteIngredient(recipe.ingredientList.indexOf(ingredient))"
          >🗑</span
        >
      </div>
      <label for="Ingredient Quantity">Quantity: </label>
      <input
        type="number"
        name="Ingredient Quantity"
        min="1"
        v-model="ingredient.quantity"
      />
      <label for="Ingredient Unit">Unit: </label>
      <input
        type="text"
        name="Ingredient Unit"
        class="unit"
        v-model="ingredient.unit"
      />
      <label for="ingredientList">Ingredient: </label>
      <input
        list="ingredients"
        name="ingredientList"
        id="ingredientList"
        v-model="ingredient.ingredientName"
      />
      <button
        type="button"
        @click="addIngredient()"
        v-show="visualParam.ingredient"
      >
        Add Ingredient
      </button>
      <button
        type="button"
        @click="editIngredient()"
        v-show="!visualParam.ingredient"
      >
        Save Edit
      </button>

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
        <span @click="deleteStep(i - 1)" v-show="visualParam.addStep">🗑</span>
      </div>
      <input
        type="text"
        name="instructions"
        placeholder="Recipe Instructions"
        v-model="recipe.instructions"
      />
      <button
        v-show="visualParam.addStep"
        type="button"
        v-on:click.prevent="addStep()"
      >
        Add Instruction Step
      </button>
      <button
        v-show="!visualParam.addStep"
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
      visualParam: {
        name: false,
        type: false,
        addStep: true,
        stepNum: Number,
        ingredient: true,
        ingIndex: Number,
      },
      recipe: {
        recipeName: "Untitled",
        instructions: "",
        type: "Main Dish",
        userId: Number,
        isShared: false,
        servings: 1,
        ingredientList: [],
        newIngredients: [],
      },
      ingredient: {
        quantity: 1,
        unit: "",
        ingredientName: "",
      },
      instructionSteps: [],
      allIngredients: [], // Contains all ingredients currently in db
    };
  },
  methods: {
    saveRecipe() {
      // this parses numservings into a number
      this.recipe.numServings = parseInt(this.recipe.numServings);

      // this changes recipe type from string into its respective type #
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
        .putRecipe(this.recipe)
        .then((response) => {
          if (response.status === 200) {
            this.$store.commit("EDIT_RECIPE", response.data);
            this.$router.push({ name: "profile" });
          }
        })
        .catch((error) => {
          this.errorMsg =
            "was not reponse.status of 200. " + error.response.statusText;
          alert("Error");
        });
    },

    addStep() {
      this.instructionSteps.push(this.recipe.instructions);
      this.recipe.instructions = "";
    },
    editSetup(index) {
      this.recipe.instructions = this.instructionSteps[index];
      this.visualParam.addStep = false;
      this.visualParam.stepNum = index;
    },
    editStep() {
      this.instructionSteps[
        this.visualParam.stepNum
      ] = this.recipe.instructions;
      this.visualParam.addStep = true;
      this.recipe.instructions = "";
    },
    deleteStep(index) {
      this.instructionSteps.splice(index, 1);
    },
    deleteIngredient(index) {
      this.recipe.ingredientList.splice(index, 1);
    },
    addIngredient() {
      let chkBool = false;
      this.recipe.ingredientList.forEach(
        (i) => {
          if (i.ingredientName === this.ingredient.ingredientName) {
            chkBool = true;
            alert("Ingredient Already Exists In This Recipe");
          }
        }
      );
      if (!chkBool) {
        //when we click the button add what's in the text fields to ingredient list as an object, if name is not in the options add it to the ingredient array
        this.recipe.ingredientList.push(this.ingredient);
        if (
          !this.allIngredients.find((x) => {
            return x.ingredientName === this.ingredient.ingredientName;
          })
        ) {
          this.recipe.newIngredients.push(this.ingredient.ingredientName);
        }
        this.ingredient = { quantity: 1, unit: "", ingredientName: "" };
      }
    },
    editIngredient() {
      this.recipe.ingredientList[this.visualParam.ingIndex] = this.ingredient;

      //Reset newIngredient list and repopulate with potential new ingredients
      this.recipe.newIngredients = [];
      for (let i of this.recipe.ingredientList) {
        if (
          !this.allIngredients.find((x) => {
            return x.ingredientName === i.ingredientName;
          })
        ) {
          this.recipe.newIngredients.push(i.ingredientName);
        }
      }

      this.ingredient = { quantity: 1, unit: "", ingredientName: "" };
      this.visualParam.ingredient = true;
    },
    setupIngredient(index) {
      this.visualParam.ingIndex = index;
      this.visualParam.ingredient = false;
      this.ingredient = this.recipe.ingredientList[index];
    },
  },
  created() {
    service.getAllIngredients().then((resp) => {
      this.allIngredients = resp.data;
    });
    let tempRecipe = this.$store.state.myRecipes.find((recipe) => {
      return this.$route.params.id == recipe.recipeId;
    });
    this.recipe.recipeId = tempRecipe.recipeId;
    this.instructionSteps = tempRecipe.instructions.split("|||");
    this.recipe.isShared = tempRecipe.isShared;
    this.recipe.recipeName = tempRecipe.recipeName;
    this.recipe.servings = tempRecipe.servings;
    this.recipe.type =
      tempRecipe.type === 1
        ? "Main Dish"
        : tempRecipe.type === 2
        ? "Side Dish"
        : tempRecipe.type === 3
        ? "Beverage"
        : tempRecipe.type === 4
        ? "Dessert"
        : "Appetizer";
        this.recipe.userId = tempRecipe.userId;

        service.getIngredients(this.recipe).then( (resp) => {
      this.recipe.ingredientList = resp.data;
    });

  },
};
</script>

<style scoped>
span:hover{
  color: blue;
}
form {
  display: flex;
  flex-direction: column;
  align-content: left;
  padding: 100px;
  text-align: left;
}
.RecipeName {
  font-size: 36px;
  font-weight: bold;
  text-align: center;
}
input[type="number"] {
  width: 50px;
}
.unit {
  width: 50px;
}
</style>