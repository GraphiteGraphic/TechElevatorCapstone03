<template>
  <div class="entirePage">
    <h2>
      Current Meal Plan
      <span
        @click="
          visualParams.viewMode = true;
          visualParams.name = false;
          visualParams.addMeal = false;
        "
        v-show="!visualParams.viewMode"
        >👁</span
      ><span
        @click="visualParams.viewMode = false"
        v-show="visualParams.viewMode"
        >🖉</span
      >
    </h2>
    <div>
      <label v-show="!visualParams.name" for="Meal Plan Name"
        >{{ mealPlan.name }}
        <span v-show="!visualParams.viewMode" @click="visualParams.name = true"
          >🖉</span
        ></label
      >
      <input
        v-show="visualParams.name"
        @blur="visualParams.name = false"
        type="text"
        v-model="mealPlan.name"
        name="Meal Plan Name"
        required="true"
      />
      <button
        type="button"
        v-show="!visualParams.viewMode"
        @click.prevent="saveMealPlan()"
      >
        Create new meal plan
      </button>
      <button
        type="button"
        v-show="mealPlan.mealPlanId && !visualParams.viewMode"
        @click.prevent="editMealPlan()"
      >
        Save Changes
      </button>
    </div>
    <div>
      <label for="Saved Meal Plans">Saved Meal Plans: </label>
      <select
        v-model="visualParams.selectedText"
        @change="showSavedMealPlan()"
        name="Saved Meal Plans"
        id="Saved Meal Plans"
      >
        <option
          :value="mealplan.mealPlanId"
          v-for="mealplan in listOfMealPlans"
          :key="mealplan.mealPlanId"
        >
          {{ mealplan.name }}
        </option>
      </select>
    </div>
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
            <!-- Display view mode recipes -->
            <section v-show="visualParams.viewMode" v-for="recipe in mealPlan.mealList[i - 1].recipes" :key="recipe.recipeId">
              <router-link
                :to="{ name: 'recipe', params: { id: recipe.recipeId } }"
                :class="recipe.recipeName"
                ><span class="recipe-name">{{ recipe.recipeName }}</span>
              </router-link>
            </section>
            <!-- Display edit mode recipes -->
            <div
              v-show="!visualParams.viewMode"
              v-for="recipe in mealPlan.mealList[i - 1].recipes"
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
    <h3 v-show="!visualParams.viewMode">
      {{ visualParams.DAYS_OF_WEEK[visualParams.dayIndex] }}
    </h3>
    <div
      v-show="!visualParams.viewMode"
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
    <grocery-list v-if="visualParams.viewMode" :mealPlan="mealPlan" />
  </div>
</template>

<script>
import services from "../services/AuthService.js";
import GroceryList from "./GroceryList.vue";

export default {
  components: { GroceryList },
  data() {
    return {
      //Container for all variables for adjusting/modifying user view
      visualParams: {
        viewMode: true,
        addMeal: false,
        dayIndex: 0,
        selectedText: "",
        name: false,
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
        name: "My Meal Plan",
        mealList: [
          { recipes: [], mealId: 0, mealName: "", timeOfDay: 0, userId: 0 },
          { recipes: [], mealId: 0, mealName: "", timeOfDay: 0, userId: 0 },
          { recipes: [], mealId: 0, mealName: "", timeOfDay: 0, userId: 0 },
          { recipes: [], mealId: 0, mealName: "", timeOfDay: 0, userId: 0 },
          { recipes: [], mealId: 0, mealName: "", timeOfDay: 0, userId: 0 },
          { recipes: [], mealId: 0, mealName: "", timeOfDay: 0, userId: 0 },
          { recipes: [], mealId: 0, mealName: "", timeOfDay: 0, userId: 0 },
        ],
        mealPlanId: 0,
        userId: 0,
        indices: "",
      },
      listOfMealPlans: [],
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
    showSavedMealPlan() {
      this.mealPlan = this.listOfMealPlans.find((mealplan) => {
        return mealplan.mealPlanId == this.visualParams.selectedText;
      });
    },
    addRecipetoMeal(recipe) {
      let chkBool = false;
      this.mealPlan.mealList[this.visualParams.dayIndex].recipes.forEach(
        (r) => {
          if (recipe.recipeId === r.recipeId) {
            chkBool = true;
            alert("Recipe Already Exists On This Day");
          }
        }
      );
      if (!chkBool) {
        this.mealPlan.mealList[this.visualParams.dayIndex].recipes.push(recipe);
      }
      this.mealPlan.mealList[this.visualParams.dayIndex].mealId = 0;
    },
    deleteRecipefromMeal(recipe) {
      let recipeIndex = this.mealPlan.mealList[
        this.visualParams.dayIndex
      ].recipes.indexOf(recipe);
      this.mealPlan.mealList[this.visualParams.dayIndex].recipes.splice(
        recipeIndex,
        1
      );
      this.mealPlan.mealList[this.visualParams.dayIndex].mealId = 0;
    },
    editMealPlan() {
      if (this.mealPlan.name.trim().length > 0) {
        services.putMealPlan(this.mealPlan).then((response) => {
          this.mealPlan = response.data;
          services.getMealPlan().then((resp) => {
            this.listOfMealPlans = resp.data;
            this.visualParams.selectedText = response.data.mealPlanId;
          });
        });
      } else {
        alert("Invalid meal plan name");
      }
    },
    saveMealPlan() {
      this.mealPlan.indices = "";
      this.mealPlan.mealPlanId = 0;
      this.mealPlan.userId = 0;
      this.mealPlan.mealList.forEach((meal) => {
        meal.mealId = 0;
        meal.mealName = "";
        meal.timeOfDay = 0;
        meal.userId = 0;
      });
      if (this.mealPlan.name.trim().length > 0) {
        services.postMealPlan(this.mealPlan).then((response) => {
          this.mealPlan = response.data;
          services.getMealPlan().then((resp) => {
            this.listOfMealPlans = resp.data;
            this.visualParams.selectedText = response.data.mealPlanId;
          });
        });
      } else {
        alert("Invalid meal plan name");
      }
    },
  },
  created() {
    services.getMealPlan().then((response) => {
      if (response.data.length > 0) {
        this.mealPlan = response.data[response.data.length - 1];
      }
      this.listOfMealPlans = response.data;
    });
  },
};
</script>

<style scoped>
span:hover {
  color: blue;
}
a {
  padding-left: 15px;
}
.entirePage {
  display: flex;
  flex-direction: column;
  background-color: rgba(255, 255, 255, 0.7);
  -webkit-backdrop-filter: blur(10px);
  backdrop-filter: blur(10px);
  border-radius: 25px;
}
table {
  border-collapse: collapse;
}
th {
  border-top: white 2px solid;
  border-left: white 2px solid;
  border-right: white 2px solid;
}
td {
  border-left: white 2px solid;
  border-right: white 2px solid;
  border-bottom: white 2px solid;
  margin: 0px 5px;
  min-width: 100px;
}
button {
  margin-bottom: 10px;
}
</style>