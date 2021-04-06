import axios from 'axios';

export default {

  login(user) {
    return axios.post('/login', user);
  },

  register(user) {
    return axios.post('/register', user);
  },

  getPublicRecipes() {
    return axios.get('/recipes');
  },

  getMyRecipes(user) {
    return axios.get('/recipes/myrecipes', user);
  },

  addRecipe(recipe) {
    return axios.post('/recipes',recipe);
  }
}
