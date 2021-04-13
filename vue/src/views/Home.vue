<template>
  <div class="home">
    <h1>Munch Munch, Yum!</h1>
    <h2>(^-^)</h2>
    <recipe-list :recipeList="recipeList" />
    <router-link
      v-bind:to="{ name: 'profile' }"
      v-if="$store.state.token !== ''"
      ><h2>My Meal Plans</h2>
    </router-link>
    <div v-if="$store.state.token===''">
    <router-link v-bind:to="{ name: 'login' }">
      <h2>Create Meal Plan</h2>
    </router-link>
    <table>
      <thead>
        <th>Sunday</th>
        <th>Monday</th>
        <th>Tuesday</th>
        <th>Wednesday</th>
        <th>Thursday</th>
        <th>Friday</th>
        <th>Saturday</th>
      </thead>
      <tbody>
        <td>
          <router-link v-bind:to="{ name: 'login' }">+ Add Recipes</router-link>
        </td>
        <td>
          <router-link v-bind:to="{ name: 'login' }">+ Add Recipes</router-link>
        </td>
        <td>
          <router-link v-bind:to="{ name: 'login' }">+ Add Recipes</router-link>
        </td>
        <td>
          <router-link v-bind:to="{ name: 'login' }">+ Add Recipes</router-link>
        </td>
        <td>
          <router-link v-bind:to="{ name: 'login' }">+ Add Recipes</router-link>
        </td>
        <td>
          <router-link v-bind:to="{ name: 'login' }">+ Add Recipes</router-link>
        </td>
        <td>
          <router-link v-bind:to="{ name: 'login' }">+ Add Recipes</router-link>
        </td>
      </tbody>
    </table>
    </div>
  </div>
</template>

<script>
import RecipeList from "../components/recipeList.vue";
export default {
  name: "home",
  components: { RecipeList },
  computed: {
    currentUser() {
      return this.$store.state.user;
    },
    recipeList() {
      return this.$store.state.recipes;
    },
  },
};
</script>

<style scoped>
span:hover {
  color: blue;
}
.entirePage {
  display: flex;
  flex-direction: column;
}
a {
  padding-left: 15px;
}
table {
  border-collapse: collapse;
  display: flex;
  flex-direction: column;
  justify-content: center;
}
th {
  border-left: white 2px solid;
  border-right: white 2px solid;
  border-top: white 2px solid;
  justify-content: center;
  width: 200px;
  min-width: 100px;
}
td {
  border-left: white 2px solid;
  border-right: white 2px solid;
  border-bottom: white 2px solid;
  min-width: 100px;
  width: 200px;
  justify-content: center;
  margin: 0px 5px;
}
button {
  margin-bottom: 10px;
}
</style>