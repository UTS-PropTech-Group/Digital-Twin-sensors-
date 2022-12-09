<template>
  <footer class="footer">
    <div class="container-fluid">
      <nav v-if="false">
        <ul class="footer-menu">
          <li>
            <router-link :to="{path:'/admin'}">Dashboard</router-link>
          </li>
        </ul>
      </nav>
      <div class="copyright text-center">
        &copy; Coded by
        <a href="https://smart-construction-group.com" target="_blank">Smart Construction Group</a>.
      </div>
    </div>
  </footer>
</template>
<script>
  export default {}

</script>
<style>

</style>
