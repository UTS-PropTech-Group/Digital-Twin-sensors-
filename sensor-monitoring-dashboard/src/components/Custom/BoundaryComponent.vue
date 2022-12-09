<template>
  <div><canvas class="heatmap" ref="boundary"></canvas></div>
</template>

<script>
import {
  getCursorPosition,
  getRandomColor,
  heatMapColorforValue,
} from "../../utils/Utils";

export default {
  props: {
    config: {
      type: Object,
      default() {
        return {
          width: 642,
          height: 768,
          min: 0,
          max: 100,
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
  },
  data() {
    return {
      rects: {
        "27_LT_056": [397, 24, 506, 166],
        "20_ComputerLab_015": [301, 23, 394, 94],
        "21_ComputerLab_016": [302, 100, 389, 173],
        "22_ComputerLab_017": [301, 174, 387, 246],
        "01_ComputerLab_012_Far": [239, 87, 298, 198],
        "02_ComputerLab_012_Near": [242, 199, 298, 326],
        "08_ComputerLab_013": [154, 180, 236, 247],
        "09_ComputerLab_014": [154, 252, 236, 324],
        "54_HarrisStreet": [42, 134, 75, 380],
        "52_Foyer_Lifts": [76, 332, 164, 382],
        "50_Foyer_DoorInside": [166, 331, 307, 395],
        "53_Foyer_DoorOutside": [311, 379, 367, 448],
        "23_StudyArea_050": [370, 372, 496, 397],
        "05_KitchenArea_034_Fridge": [395, 396, 480, 449],
        "04_Guthrie_Foyer_025": [167, 447, 358, 503],
        "12_Guthrie_028_Door": [181, 520, 359, 597],
        "29_GuthrieLT_028_Front": [177, 597, 358, 666],
        "26_Classroom_053": [539, 99, 610, 171],
        "25_Classroom_052": [536, 177, 613, 247],
        "24_Classroom_051": [536, 251, 610, 324],
        "28_LT_022_Front": [392, 208, 508, 323],
        "07_LT_022_Door": [392, 325, 498, 373],
        "03_ComputerLab_019": [311, 271, 389, 371],
      },
      circles: {
        "27_LT_056": [450, 100],
        "20_ComputerLab_015": [346, 62],
        "21_ComputerLab_016": [344, 139],
        "22_ComputerLab_017": [344, 209],
        "01_ComputerLab_012_Far": [271, 135],
        "02_ComputerLab_012_Near": [272, 266],
        "08_ComputerLab_013": [193, 218],
        "09_ComputerLab_014": [197,282],
        "54_HarrisStreet": [56, 289],
        "52_Foyer_Lifts": [239,406],
        "50_Foyer_DoorInside": [234 , 360],
        "53_Foyer_DoorOutside": [135, 353],
        "23_StudyArea_050": [484,383],
        "05_KitchenArea_034_Fridge": [443, 421],
        "04_Guthrie_Foyer_025": [215, 480],
        "12_Guthrie_028_Door": [326, 552],
        "29_GuthrieLT_028_Front": [229, 635],
        "26_Classroom_053": [573, 137],
        "25_Classroom_052": [573, 208],
        "24_Classroom_051": [573, 287],
        "28_LT_022_Front": [441, 254],
        "07_LT_022_Door": [446, 337],
        "03_ComputerLab_019": [347,324],
      },
    };
  },
  mounted() {
    this.$refs.boundary.height = this.config.height;
    this.$refs.boundary.width = this.config.width;

    var canvas = this.$refs.boundary;
    canvas.addEventListener("mousedown", function (e) {
      getCursorPosition(canvas, e);
    });
  },
  methods: {
    renderRects() {
      this.clearCanvas();
      var c = this.$refs.boundary;
      let min = this.value.min;
      let max = this.value.max;
      var ctx = c.getContext("2d");
      console.log("rednering canvas: ", this.value);
      for (let record of this.value.data) {
        let heatMapValue = (record.value - min) / (max - min);
        if (heatMapValue > 1) heatMapValue = 1;
        else if (heatMapValue < 0) heatMapValue = 0;
        ctx.fillStyle = heatMapColorforValue(heatMapValue);
        let r = this.rects[record.name];
        if (r) {
          ctx.fillRect(r[0], r[1], r[2] - r[0], r[3] - r[1]);
        }
      }
    },
    renderCircles(){
      this.clearCanvas();
      var c = this.$refs.boundary;
      let min = this.value.min;
      let max = this.value.max;
      var ctx = c.getContext("2d");
      console.log("rednering circles on canvas: ", this.value);
      for(var record of this.value.data){
            ctx.beginPath();
            let heatMapValue = (record.value - min) / (max - min);
            if (heatMapValue > 1) heatMapValue = 1;
            else if (heatMapValue < 0) heatMapValue = 0;
            ctx.fillStyle = heatMapColorforValue(heatMapValue);
            let circle = this.circles[record.name];
            if(!circle) continue;
            ctx.arc(circle[0], circle[1], 30, 0, 2 * Math.PI, false);
            ctx.fill();
            ctx.font = '20px Calibri';
            ctx.fillStyle = 'black';
            ctx.textAlign = 'center';
            ctx.fillText(`${record.value.toFixed(2)}`, circle[0], circle[1]+8);
      }

    },
    clearCanvas() {
      let canvas = this.$refs.boundary;
      const context = canvas.getContext("2d");
      context.clearRect(0, 0, canvas.width, canvas.height);
    },
  },
  watch: {
    model(val, oldVal) {
      console.log("heatmap model changed: ", val);
      this.renderCircles();
    },
  },
};
</script>

<style>
</style>