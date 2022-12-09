<template>
  <div class="frame-wrapper">
    <iframe :src="heatmap3DSource" frameborder="0"></iframe>
  </div>
</template>

<script>
export default {
  data() {
    return {
      heatmap3DSource:
        window.location.protocol + "//" + window.location.hostname + ":8282",
    };
  },
};
</script>

<style>
.frame-wrapper {
  height: 95vh;
}
.frame-wrapper iframe {
  width: 100%;
  height: 100%;
}
</style>