import axios from 'axios';

export default {

  login(user) {
    return axios.post('/login', user)
  },

  register(user) {
    return axios.post('/register', user)
  },

  getRecipes(user) {
    if(user.token === ''){
      return axios.get('/recipe')
    }
    else{
      return axios.get('/recipe', user)
    }
  }
}
