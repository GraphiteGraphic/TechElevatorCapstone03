<template>
  <div>
    <router-link
      :to="{ name: 'recipe', params: { id: recipe.recipeId } }"
      v-for="recipe in recipeList"
      :key="recipe.recipeId"
      :name="recipe.recipeName"
      :instructions="recipe.instructions"
      >{{ recipe.recipeName }}</router-link
    >
  </div>
</template>

<script>
export default {
    name: 'recipe-list',
    props: {
        recipeList: Array
    }
};
</script>

<style scoped>
a{
    display: block;
}
</style>