<template>
  <div>
    <section v-for="recipe in recipeList" :key="recipe.recipeId">
      <router-link
        :to="{ name: 'recipe', params: { id: recipe.recipeId } }"
        :class="recipe.recipeName"
        >{{ recipe.recipeName }}
        <img :src="recipe.imgUrl">
        </router-link>
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
  background-color: rgba(0,0,0,0.5);
  margin: 10px;
}
a {
  display: inline-block;
  width: 300px;
  height: 400px;
  background-color: rgba(255,255,255,0.7);
  -webkit-backdrop-filter: blur(10px);
  backdrop-filter: blur(10px);
  position: relative;
  left: -1px;
  top: -1px;
}
a:hover {
  left: -3px;
  top: -3px;
}
</style>