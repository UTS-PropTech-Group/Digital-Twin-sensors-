<template>
  <div class="content">
    <div class="row">
      <div class="col-12">
        <card>
          <template slot="header">
            <h5 class="title">Data Config</h5>
            <!-- <p class="category">Backend development</p> -->
            <div class="row">
              <div class="col-5">
                <base-select
                  label="type"
                  :options="options"
                  v-model="typeVal"
                ></base-select>
              </div>
              <div class="col-4">
                <div class="row">
                  <div class="col-6">
                    <base-date label="from date" v-model="fromDate"></base-date>
                  </div>
                  <div class="col-6">
                    <base-date label="to date" v-model="toDate"></base-date>
                  </div>
                </div>
              </div>
              <div class="col-2">
                <base-select
                  label="interval"
                  :options="intervals"
                  v-model="interval"
                ></base-select>
              </div>
              <div class="col-1">
                <div class="form-group">
                  <div>
                    <label class="control-label"> submit </label>
                  </div>
                  <div>
                    <button
                      type="submit"
                      class="btn btn-info btn-fill"
                      @click.prevent="getData"
                    >
                      Get
                    </button>
                  </div>
                </div>
              </div>
            </div>
            <div style="text-align: center" v-if="loading">Loading...</div>
          </template>
        </card>
        <div class="row">
          <div class="col-3">
            <card v-if="activeTab != 'Table'">
              <template>
                <h5>Customize</h5>
                <div class="form-group" v-if="activeTab == 'Heat Map'">
                  <label for="radiusRange" class="form-label">Radius</label>
                  <div>
                    <input
                      type="range"
                      class="form-range"
                      id="radiusRange"
                      :min="radiusRange[0]"
                      :max="radiusRange[1]"
                      v-model="customize.radius"
                      @change="customizeChanged"
                    />
                    <input
                      class="form-control-sm"
                      type="number"
                      v-model="customize.radius"
                      @change="customizeChanged"
                    />
                  </div>
                </div>
                <div class="row">
                  <div class="col-6">
                    <div class="form-group">
                      <label for="radiusRange" class="form-label">min</label>
                      <input
                        class="form-control-sm"
                        style="max-width: 100%"
                        type="number"
                        v-model="customize.min"
                        @change="customizeChanged"
                      />
                    </div>
                  </div>
                  <div class="col-6">
                    <div class="form-group">
                      <label for="radiusRange" class="form-label">max</label>
                      <input
                        class="form-control-sm"
                        style="max-width: 100%"
                        type="number"
                        v-model="customize.max"
                        @change="customizeChanged"
                      />
                    </div>
                  </div>
                </div>
              </template>
            </card>
            <card class="heatmap-table">
              <template>
                <h5>Data Records</h5>
                <heat-map-table
                  class="table-hover table-striped table-sm"
                  :columns="['time']"
                  :data="tableData"
                  v-model="tableValue"
                >
                </heat-map-table>
              </template>
            </card>
          </div>
          <div class="col-9">
            <card style="min-height: 500px">
              <template>
                <ul class="nav nav-tabs">
                  <li v-for="tab in tabs" :key="tab" class="nav-item">
                    <a
                      class="nav-link"
                      :class="{ active: tab == activeTab }"
                      @click="activeTab = tab"
                      >{{ tab }}</a
                    >
                  </li>
                </ul>
                <span class="arrow-keys">
                  <button class="btn btn-sm p-0" @click="prevRecord">
                    <img
                      width="32"
                      height="32"
                      src="img/chevron-left-solid.svg"
                      alt=""
                    />
                  </button>
                  <button class="btn ml-2 p-0">
                    <img
                      v-if="!playing"
                      width="32"
                      height="32"
                      src="img/play.svg"
                      alt=""
                      @click="playClicked"
                    />
                    <img
                      v-else
                      width="32"
                      height="32"
                      src="img/pause.svg"
                      alt=""
                      @click="playClicked"
                    />
                  </button>
                  <button class="btn btn-sm ml-2 p-0" @click="nextRecord">
                    <img
                      width="32"
                      height="32"
                      src="img/chevron-right-solid.svg"
                      alt=""
                    />
                  </button>
                  <button class="btn btn-sm ml-2 p-0" @click="restart">
                    <img
                      width="32"
                      height="32"
                      src="img/rotate-right-solid.svg"
                      alt=""
                    />
                  </button>
                  <span
                    style="
                      border-left: 1px solid black;
                      padding: 10px 0px;
                      margin: 5px;
                    "
                  ></span>
                  <button
                    class="btn btn-secondary btn-sm ml-2 p-0"
                    @click="download"
                  >
                    <div
                      style="
                        width: 32px;
                        height: 32px;
                        text-align: center;
                        padding-top: 1px;
                      "
                    >
                      <i class="fa fa-2x fa-download" aria-hidden="true"></i>
                      <div style="font-size: 10px; margin-top: -6px">ONE</div>
                    </div>
                  </button>
                  <button
                    class="btn btn-secondary btn-sm ml-2 p-0"
                    @click="downloadAll"
                  >
                    <div
                      style="
                        width: 32px;
                        height: 32px;
                        text-align: center;
                        padding-top: 1px;
                      "
                    >
                      <i class="fa fa-2x fa-download" aria-hidden="true"></i>
                      <div style="font-size: 10px; margin-top: -6px">ALL</div>
                    </div>
                  </button>
                </span>
                <heat-map-component
                  v-if="activeTab == 'Heat Map'"
                  v-model="heatMapData"
                  width="840"
                  height="500"
                ></heat-map-component>
                <boundary-component
                  v-else-if="activeTab == 'Boundary'"
                  width="840"
                  height="500"
                  v-model="heatMapData"
                >
                </boundary-component>
                <data-table-component
                  v-else-if="activeTab == 'Table'"
                  v-model="heatMapData"
                ></data-table-component>
                <div v-if="activeTab != 'Table'" class="legend-box">
                  <div>
                    <span>MAX: {{ customize.max.toFixed(2) }}</span>
                  </div>
                  <img src="img/heatmap legend2.png" height="600" />
                  <div>
                    <span>MIN: {{ customize.min.toFixed(2) }}</span>
                  </div>
                </div>
              </template>
            </card>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import BaseSelect from "src/components/Inputs/BaseSelect.vue";
import BaseDate from "src/components/Inputs/BaseDate.vue";
import HeatMapComponent from "src/components/Custom/HeatMapComponent.vue";
import HeatMapTable from "src/components/Custom/HeatMapTable.vue";
import Card from "../components/Cards/Card.vue";
import { fetchHeatMapData } from "../services/HeatMapApiService.js";
import BoundaryComponent from "src/components/Custom/BoundaryComponent.vue";
import DataTableComponent from "src/components/Custom/DataTableComponent.vue";
import { downloadAsCsv } from "../utils/Utils";
export default {
  components: {
    BaseSelect,
    BaseDate,
    HeatMapComponent,
    HeatMapTable,
    Card,
    BoundaryComponent,
    DataTableComponent,
  },
  data() {
    return {
      options: [
        "temperature",
        "pressure",
        "humidity",
        "light",
        "ultrav",
        "particleavg10",
        "particleavg2.5",
      ],
      tabs: ["Heat Map", "Boundary", "Table"],
      activeTab: "Heat Map",
      intervals: ["daily", "hourly", "30min", "15min", "5min", "minutely"],
      city: "berlin",

      myData: {
        "2022/8/10": [
          {
            device_id: "x1",
            x: 100,
            y: 50,
            value: 100,
          },
          {
            device_id: "x2",
            x: 150,
            y: 150,
            value: 50,
          },
        ],
        "2022/8/11": [
          {
            device_id: "x1",
            x: 100,
            y: 50,
            value: 10,
          },
          {
            device_id: "x2",
            x: 150,
            y: 150,
            value: 150,
          },
        ],
      },
      tableData: [],
      heatMapData: null,
      fromDate: null,
      toDate: null,
      typeVal: null,
      interval: null,
      tableValue: null,
      tableValueIndex: null,
      radiusRange: [1, 200],
      customize: {
        radius: 80,
        min: 0,
        max: 25,
      },
      loading: false,
      playing: false,
      playIntervalHandler: null,
    };
  },
  methods: {
    getData() {
      let config = {
        type: this.typeVal,
        from: this.fromDate,
        to: this.toDate,
        interval: this.interval,
      };
      console.log(config);
      this.loading = true;
      fetchHeatMapData(config).then((res) => {
        this.tableData = res.tableData;
        this.myData = res.data;
        this.updateMinMax(this.myData);
        this.tableValue = res.tableData[0];
        this.loading = false;
        console.log(res);
      });
    },

    updateHeatMapData(dataPoints) {
      for (let p of dataPoints) {
        p.radius = this.customize.radius;
      }
      this.heatMapData = {
        min: this.customize.min,
        max: this.customize.max,
        data: dataPoints,
      };
    },
    customizeChanged() {
      console.log("customized changed: ", this.customize);
      this.updateHeatMapData(this.heatMapData.data);
    },
    nextRecord() {
      let i = this.tableData.indexOf(this.tableValue);
      if (i < this.tableData.length - 1 && i != -1) {
        console.log(this.tableData[i + 1]);
        this.tableValue = this.tableData[i + 1];
      }
    },
    prevRecord() {
      let i = this.tableData.indexOf(this.tableValue);
      if (i > 0) {
        this.tableValue = this.tableData[i - 1];
      }
    },
    playClicked() {
      if (!this.playing) {
        this.playIntervalHandler = setInterval(this.nextRecord, 500);
        this.playing = true;
      } else {
        clearInterval(this.playIntervalHandler);
        this.playing = false;
      }
    },
    updateMinMax(data) {
      let min = Infinity;
      let max = -Infinity;
      console.log(data);
      for (let d of Object.keys(data)) {
        for (let p of data[d]) {
          if (p.value < min) min = p.value;
          if (p.value > max) max = p.value;
        }
      }
      this.customize.min = min;
      this.customize.max = max;
    },
    restart() {
      this.tableValue = this.tableData[0];
    },
    download() {
      if (!this.tableValue) return;
      var data = this.myData[this.tableValue];
      console.log(data);
      const columns = ["ts", "name", "device_id", "value", "type", "x", "y"];
      downloadAsCsv(
        document,
        columns,
        data,
        `${this.typeVal} - ${this.tableValue}.csv`
      );
    },
    downloadAll() {
      if (!this.tableValue) return;
      let keys = Object.keys(this.myData);
      let data = [];
      for (let k of keys) {
        data = [...data, ...this.myData[k]];
      }
      console.log(data);
      const columns = ["ts", "name", "device_id", "value", "type", "x", "y"];
      downloadAsCsv(
        document,
        columns,
        data,
        `${this.typeVal} - ${this.fromDate} - ${this.toDate}.csv`
      );
    },
  },
  watch: {
    tableValue: function (val, oldVal) {
      console.log("table value changed: ", val);
      this.selectedRecord = val;
      console.log(this.myData);
      console.log(this.myData[val]);
      this.updateHeatMapData(this.myData[val]);
    },
  },
};
</script>

<style>
.arrow-keys {
  position: absolute;
  top: 10px;
  right: 10px;
}

.heatmap-table {
  max-height: 600px;
  overflow: scroll;
}

.legend-box {
  position: absolute;
  top: 30px;
  right: 0;
  border: 2px solid black;
  border-radius: 5px;
  background-color: #fff;
  padding: 5px;
  margin: 50px 10px;
  font-weight: bold;
  align-content: center;
}
.legend-box img {
  margin: 0 7px;
}
</style>