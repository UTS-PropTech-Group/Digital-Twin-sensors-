<template>
  <l-table
    class="table-hover table-striped table-sm"
    :columns="columns"
    :data="model.data"
  >
  </l-table>
</template>
<script>
import LTable from "src/components/Table.vue";

export default {
  components: {
    LTable,
  },
  props: {
    value: {
      type: Object,
    },
  },
  data() {
    return {
      columns: ["ts", "name", "device_id", "value", "type", "x", "y"],
    };
  },
  computed: {
    model: {
      get() {
        console.log("get called");
        return this.value;
      },
      set(value) {
        console.log("data is set:", value);
        this.$emit("input", value);
      },
    },
  },
  watch: {
    model(val, oldVal) {
      console.log("heatmap model changed: ", val);
    },
  },
};
</script>
<style>
</style>
