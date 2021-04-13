<template>
  <div>
    <new-recipe v-if="!recipeId"/>
    <edit-recipe v-if="recipeId"/>
    <photos />
  </div>
</template>

<script>
import EditRecipe from '../components/EditRecipe.vue'
import newRecipe from "../components/NewRecipe.vue"
import Photos from '../components/Photos.vue';
export default {
  components: {
    newRecipe,
    EditRecipe,
    Photos,
  },
  computed: {
    recipeId(){
      if(this.$route.params){
        return this.$route.params.id;
      }
      return ""
    }
  },
}
</script>

<style>

</style>