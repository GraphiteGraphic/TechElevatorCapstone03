<template>
  <div id="app">
    <div id="nav">
      <router-link v-bind:to="{ name: 'home' }">Home</router-link>&nbsp;|&nbsp;
      <router-link
        v-bind:to="{ name: 'login' }"
        v-if="$store.state.token === ''"
        >Login</router-link
      >
      <router-link
        v-bind:to="{ name: 'profile' }"
        v-if="$store.state.token !== ''"
        >{{ currentUser.username }} |</router-link
      >
      <router-link
        v-bind:to="{ name: 'logout' }"
        v-if="$store.state.token != ''"
      >
        Logout</router-link
      >
    </div>
    <router-view v-if="isLoaded" />
  </div>
</template>

<script>
import authServices from '@/services/AuthService.js';

export default {
  data() {
    return{
      isLoaded: false
    }
  },
  computed: {
    currentUser() {
      return this.$store.state.user;
    },
  },
  created() {
    authServices.getPublicRecipes().then((response)=>{
      this.$store.commit("SET_PUBLIC_RECIPES",response.data);
      if (this.$store.state.token===''){
        this.isLoaded=true;
      }
    });
    if (this.$store.state.token!==''){
      authServices.getMyRecipes(this.$store.state.user).then((response)=>{
        this.$store.commit("SET_MY_RECIPES",response.data);
        this.isLoaded = true;
      });
    }
  }
};
</script>

<style>
body {
  text-align: center;
  color: white;
  background-color: rgb(0, 175, 240);
}

#nav {
  text-align: right;
}

a {
  color: white;
  text-decoration: none;
}

a:visited {
  color: white;
}
</style>