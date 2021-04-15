import Vue from 'vue'
import Vuex from 'vuex'
import axios from 'axios'

Vue.use(Vuex)

/*
 * The authorization header is set for axios when you login but what happens when you come back or
 * the page is refreshed. When that happens you need to check for the token in local storage and if it
 * exists you should set the header so that it will be attached to each request
 */
const currentToken = localStorage.getItem('token')
const currentUser = JSON.parse(localStorage.getItem('user'));

if(currentToken != null) {
  axios.defaults.headers.common['Authorization'] = `Bearer ${currentToken}`;
}

export default new Vuex.Store({
  state: {
    token: currentToken || '',
    user: currentUser || {},
    recipes: [],
    myRecipes: [],
    recipeIngredients: [{name: 'Carrot', quantity: 2, unit: 'cup'}] 
  },
  mutations: {
    SET_AUTH_TOKEN(state, token) {
      state.token = token;
      localStorage.setItem('token', token);
      axios.defaults.headers.common['Authorization'] = `Bearer ${token}`
    },
    SET_USER(state, user) {
      state.user = user;
      localStorage.setItem('user',JSON.stringify(user));
    },
    LOGOUT(state) {
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      state.token = '';
      state.user = {};
      state.myRecipes = [];
      axios.defaults.headers.common = {};
    },
    SET_PUBLIC_RECIPES(state, list){
      state.recipes=list;
    },
    SET_MY_RECIPES(state, list){
      state.myRecipes=list;
    },
    ADD_RECIPE(state, recipe){
      state.myRecipes.push(recipe);
    },
    EDIT_RECIPE(state, recipe){
      state.myRecipes[state.myRecipes.indexOf(state.myRecipes.find( (r) => {
        return r.recipeId == recipe.recipeId;
      }))] = recipe;
      state.recipes[state.recipes.indexOf(state.recipes.find( (r) => {
        return r.recipeId == recipe.recipeId;
      }))] = recipe;
    }
  }
})
