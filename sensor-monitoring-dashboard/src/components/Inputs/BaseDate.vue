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
        <datepicker input-class="form-control" v-model="model" @selected="selectedHandler" @input="inputHandler"></datepicker>
    </slot>
  </div>
</template>

<script>
import Datepicker from 'vuejs-datepicker';
export default {
    components: {
    Datepicker
  },
  name: "base-select",
  props: {
    label: {
      type: String,
      description: "Input label",
    },
    value: {
        type: Date,
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
            console.log("date is set:", value)
          this.$emit('input', value)
        }
      }
    },
    methods: {
        selectedHandler(value) {
            console.log("Selected: ", value)
        },
        inputHandler(value){
            console.log("input handler", value)
        }
    }
};
</script>

<style>
input[readonly]{
  background-color:transparent !important;
}
input[readonly]:hover{
    cursor: auto;
}
</style>