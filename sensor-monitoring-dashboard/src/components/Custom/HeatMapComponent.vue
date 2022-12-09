<template>
  <div>
    <div class="heatmap" ref="heatmap" @click="clicked"></div>
  </div>
</template>

<script>
import h337 from "heatmap.js";
import { getCursorPosition } from '../../utils/Utils';
export default {
  props: {
    config: {
      type: Object,
      default() {
        return {
          width: 642,
          height: 768,
          max: 150,
          radius: 200,
        };
      },
    },
    value: {
      type: Object,
    },
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
    listeners() {
      return {
        ...this.$listeners,
        input($event) {
          console.log("heatmap input changed: ", $event);
        },
      };
    },
  },
  data() {
    return {
      heatmapInstance: null,
    };
  },

  methods: {
    clicked(event) {
      // console.log("clicked at:",  "x: "+ event.clientX, "y:"+event.clientY);
    },
    createHeatMap(data) {
      this.$refs.heatmap.style.height = this.height + "px";
      this.$refs.heatmap.style.width = this.width + "px";
      // var max = this.config.max;
      
      this.heatmapInstance.setData(data);
    },
  },
  watch: {
    model(val, oldVal) {
      console.log("heatmap model changed: ", val);
      this.createHeatMap(val);
    },
  },
  mounted() {
    this.$refs.heatmap.style.height = this.config.height + "px";
    this.$refs.heatmap.style.width = this.config.width + "px";

    this.heatmapInstance = h337.create({
      container: this.$refs.heatmap,
    });
    var canvas = this.$refs.heatmap
    canvas.addEventListener('mousedown', function(e) {
        getCursorPosition(canvas, e)
    })
  },
};

</script>

<style>
.heatmap {
  background-image: url("/img/map.png");
}
</style>