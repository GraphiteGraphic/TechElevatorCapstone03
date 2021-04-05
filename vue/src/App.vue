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
        v-bind:to="{ name: 'logout' }"
        v-if="$store.state.token != ''"
      >{{currentUser.username}} Logout</router-link
      >
    </div>
    <router-view />
  </div>
</template>

<script>
import authServices from '@/services/AuthService.js';

export default {
  computed: {
    currentUser() {
      return this.$store.state.user;
    },
  },
  created() {
    authServices.getRecipes(this.currentUser);
  }
};
</script>

<style>
body {
  text-align: center;
  color: white;
  background-color: rgb(0, 175, 240);  
}

#nav{
  text-align: right;
}

a {
    text-decoration: none;
}

a:visited {
  color: white;
}
</style>