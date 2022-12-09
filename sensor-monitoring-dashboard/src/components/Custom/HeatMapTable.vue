<template>
  <table class="table">
    <thead>
      <slot name="columns">
        <tr>
          <th v-for="column in columns" :key="column">{{column}}</th>
        </tr>
      </slot>
    </thead>
    <tbody>
    <tr v-for="(item, index) in data" :key="index">
      
      <slot :row="item">
        <td v-for="column in columns" :key="column" @click="tdClicked" :class="{ active: item == value }">{{item}}</td>
      </slot>
    </tr>
    </tbody>
  </table>
</template>
<script>
  export default {
    name: 'l-table',
    props: {
      columns: Array,
      data: Array,
      value: [String, Number],
      tdClickCallback: {
          type: Function,
          default: ()=>{}
      }
    },
    data() {
        return {
            selectedTd: undefined
        }
    },
    mounted(){
       this.tdClicked({target: document.querySelector("tr td:first-child")})
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
    },
    methods: {
    
      hasValue (item, column) {
        return item[column.toLowerCase()] !== 'undefined'
      },
      itemValue (item, column) {
        return item[column.toLowerCase()]
      },
      tdClicked($event){
          // if(this.selectedTd != undefined){
          //     this.selectedTd.style.backgroundColor = ""
          // }
          //   $event.target.style.backgroundColor = "#f5aeae"
          //   this.selectedTd = $event.target
          //   console.log("selected: ", this.selectedTd)
            this.model= $event.target.innerHTML
      }
    }
  }
</script>
<style>
  td.active{
  background-color: #f5aeae;
}
</style>
