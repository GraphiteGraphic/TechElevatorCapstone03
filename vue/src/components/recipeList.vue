<template>
  <div>
    <section v-for="recipe in recipeList" :key="recipe.recipeId">
      <router-link
        :to="{ name: 'recipe', params: { id: recipe.recipeId } }"
        :class="recipe.recipeName"
        >{{ recipe.recipeName }}</router-link
      >
    </section>
  </div>
</template>

<script>
export default {
  name: "recipe-list",
  props: {
    recipeList: Array,
  },
};
</script>

<style scoped>
div {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-around;
  align-content: space-around;
}
section{
  width: 300px;
  height: 400px;
  background-color: blue;
  margin: 10px;
}
a {
  display: inline-block;
  width: 300px;
  height: 400px;
  background-color: rgb(125, 125, 255);
  position: relative;
  left: -1px;
  top: -1px;
}
a:hover {
  left: -3px;
  top: -3px;
}
</style>