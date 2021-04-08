<template>
  <div>
    <h2>
      My Meal Plan
      <span
        @click="
          visualParams.viewMode = true;
          visualParams.addMeal = false;
        "
        v-show="!visualParams.viewMode"
        >👁</span
      ><span
        @click="visualParams.viewMode = false"
        v-show="visualParams.viewMode"
        >🖉</span
      >
      <button type="button" @click.prevent="saveMealPlan()">
        Save Changes
      </button>
    </h2>
    <table>
      <thead>
        <th @click="this, (visualParams.dayIndex = 0)">Sunday</th>
        <th @click="this, (visualParams.dayIndex = 1)">Monday</th>
        <th @click="this, (visualParams.dayIndex = 2)">Tuesday</th>
        <th @click="this, (visualParams.dayIndex = 3)">Wednesday</th>
        <th @click="this, (visualParams.dayIndex = 4)">Thursday</th>
        <th @click="this, (visualParams.dayIndex = 5)">Friday</th>
        <th @click="this, (visualParams.dayIndex = 6)">Saturday</th>
      </thead>
      <tbody>
        <tr>
          <td v-for="i of 7" :key="i">
            <recipe-list
              v-show="visualParams.viewMode"
              :recipeList="mealPlan.MealList[i - 1].recipes"
            />
            <div
              v-show="!visualParams.viewMode"
              v-for="recipe in mealPlan.MealList[i - 1].recipes"
              :key="recipe.recipeId"
            >
              <span
                @click="
                  this, (visualParams.dayIndex = i - 1);
                  deleteRecipefromMeal(recipe);
                "
                >{{ recipe.recipeName }} 🗑</span
              >
            </div>
            <span
              @click="
                visualParams.addMeal = true;
                visualParams.dayIndex = i - 1;
              "
              v-show="!visualParams.viewMode"
              >+ Add Recipes</span
            >
          </td>
        </tr>
      </tbody>
    </table>
    <h3 v-show="visualParams.addMeal">
      {{ visualParams.DAYS_OF_WEEK[visualParams.dayIndex] }}
    </h3>
    <div
      v-show="visualParams.addMeal"
      v-for="recipe in availableRecipes"
      :key="recipe.recipeId"
    >
      <span @click="addRecipetoMeal(recipe)">{{ recipe.recipeName }}</span>
      <router-link
        :to="{ name: 'recipe', params: { id: recipe.recipeId } }"
        :name="recipe.recipeName"
        :instructions="recipe.instructions"
        >👁</router-link
      >
    </div>
  </div>
</template>

<script>
import RecipeList from "./recipeList.vue";
import services from "../services/AuthService.js";

export default {
  components: { RecipeList },
  data() {
    return {
      //Container for all variables for adjusting/modifying user view
      visualParams: {
        viewMode: true,
        addMeal: false,
        dayIndex: 0,
        DAYS_OF_WEEK: [
          "Sunday",
          "Monday",
          "Tuesday",
          "Wednesday",
          "Thursday",
          "Friday",
          "Saturday",
        ],
      },
      mealPlan: {
        Name: "MyMealPlan",
        MealList: [
          { recipes: [], MealId: 0, MealName: "", TimeOfDay: 0, UserId: 0 },
          { recipes: [], MealId: 0, MealName: "", TimeOfDay: 0, UserId: 0 },
          { recipes: [], MealId: 0, MealName: "", TimeOfDay: 0, UserId: 0 },
          { recipes: [], MealId: 0, MealName: "", TimeOfDay: 0, UserId: 0 },
          { recipes: [], MealId: 0, MealName: "", TimeOfDay: 0, UserId: 0 },
          { recipes: [], MealId: 0, MealName: "", TimeOfDay: 0, UserId: 0 },
          { recipes: [], MealId: 0, MealName: "", TimeOfDay: 0, UserId: 0 },
        ],
        MealPlanId: 0,
        UserId: 0,
      },
    };
  },
  name: "meal-plan",
  computed: {
    availableRecipes() {
      const publicRecipes = this.$store.state.recipes.map((el) => {
        return el.recipeId;
      });
      const privateRecipes = this.$store.state.myRecipes.map((el) => {
        return el.recipeId;
      });
      //this turns the array into a Set which doesn't like duplicates so it removes them, and then it turns the Set back into an array excluding any duplicates
      let uniqueSet = publicRecipes.concat(privateRecipes);
      uniqueSet = [...new Set(uniqueSet)];

      uniqueSet = uniqueSet.map((recipeId) => {
        //if in public recipes grab recipe from $store.recipes if in private recipes grab from $store.myRecipes
        if (publicRecipes.includes(recipeId)) {
          return this.$store.state.recipes.find((publicRecipes) => {
            return recipeId === publicRecipes.recipeId;
          });
        }
        if (privateRecipes.includes(recipeId)) {
          return this.$store.state.myRecipes.find((privateRecipes) => {
            return recipeId === privateRecipes.recipeId;
          });
        }
      });
      return uniqueSet;
    },
  },
  methods: {
    addRecipetoMeal(recipe) {
      this.mealPlan.MealList[this.visualParams.dayIndex].recipes.push(recipe);
    },
    deleteRecipefromMeal(recipe) {
      let recipeIndex = this.mealPlan.MealList[
        this.visualParams.dayIndex
      ].recipes.indexOf(recipe);
      this.mealPlan.MealList[this.visualParams.dayIndex].recipes.splice(
        recipeIndex,
        1
      );
    },
    saveMealPlan() {
      services.postMealPlan(this.mealPlan);
    },
    getMealPlan() {
      services.getMealPlan().then((response) => {
        this.mealPlan = response.data();
      });
    },
  },
};
</script>

<style>
a {
  padding-left: 15px;
}
</style>