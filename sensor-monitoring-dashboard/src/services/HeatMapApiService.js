
import axios from 'axios'
import { formatDate } from '../utils/Utils'
const APIUrl = "http://" + window.location.hostname + ":9090"
// const APIUrl = "http://localhost:9090"


export async function fetchHeatMapData(config) {
    console.log(APIUrl)
    if (config.from instanceof Date) config.from = formatDate(config.from)
    if (config.to instanceof Date) config.to = formatDate(config.to)

    var res = await axios.get(APIUrl + `/sensors?type=${config.type}&from=${config.from}&to=${config.to}&interval=${config.interval}`)
    return { tableData: Object.keys(res.data), data: res.data }
}

function getMockData(config) {
    Date.prototype.addDays = function (days) {
        var date = new Date(this.valueOf());
        date.setDate(date.getDate() + days);
        return date;
    }

    function getDates(startDate, stopDate) {
        var dateArray = new Array();
        var currentDate = startDate;
        while (currentDate <= stopDate) {
            dateArray.push(new Date(currentDate));
            currentDate = currentDate.addDays(1);
        }
        return dateArray;
    }

    var days = getDates(config.from, config.to)
    var data = {}
    var tableData = []
    for (let i in days) {
        var day = days[i].toISOString().split('T')[0]
        tableData.push(day)
        data[day] = []
        for (var j = 0; j < parseInt(Math.random() * 100); j++) {
            data[day].push({
                device_id: "x" + i,
                x: Math.random() * 500,
                y: Math.random() * 500,
                value: Math.random() * 25
            })
        }
    }
    return { data, tableData }
}