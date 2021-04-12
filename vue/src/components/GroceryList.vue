<template>
  <div>
    <h2 @click.prevent="collapse = !collapse">
      <span v-show="!collapse">⮞</span><span v-show="collapse">⮟</span> Grocery
      List
    </h2>
    <p v-show="collapse" v-for="ingredient in ingredients" :key="ingredient.ingredientId">{{ingredient.ingredientName}}</p>
  </div>
</template>

<script>
export default {
  data() {
    return {
      collapse: true,
    };
  },
  props: {
    mealPlan: Object,
  },
  computed: {
    ingredients() {
      let x = [];
      this.mealPlan.mealList.forEach((meal) => {
        meal.recipes.forEach((recipe) => {
          if (recipe.ingredientList) {
            recipe.ingredientList.forEach((ingredient) => {
              x.push(ingredient);
            });
          }
        });
      });
      x = [...new Set(x)];
      return x;
    },
  },
};
</script>

<style>
</style>