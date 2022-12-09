<template>
  <div
    class="form-group"
  >
    <slot name="label">
      <label v-if="label" class="control-label">
        {{ label }}
      </label>
    </slot>

    <slot>
      <select class="custom-select" id="inputGroupSelect01" v-model="model"
        v-bind="$attrs">
        <option v-if="!options" selected>no data</option>
        <option v-else v-for="option in options" :value="option" :key="option">{{option}}</option>
      </select>
    </slot>
  </div>
</template>

<script>
export default {
  name: "base-select",
  props: {
    label: {
      type: String,
      description: "Input label",
    },
    options: {
        type: Array,
        description: "Input options",
    },
    value: {
        type: [String, Number],
        description: "Input value"
    },
  },

  computed: {
      model: {
        get () {
            console.log("get called")
          return this.value
        },
        set (value) {
            console.log("data is set:", value)
          this.$emit('input', value)
        }
      }
  }
};
</script>

<style>
</style>